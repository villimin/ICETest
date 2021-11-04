using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace ICE_Villy_Test
{
    public static class FileManager
    {
        readonly static string ROOT_FILES_DIRECTORY = $"{Config.ROOT_FOLDER_PATH}Files";
        readonly static string URL = "https://noaa-gfs-bdp-pds.s3.amazonaws.com/gfs.{time}/00/atmos/gfs.t00z.pgrb2.0p25.f{offset}";

        private static WebClient client;

        static FileManager()
        {
            if (!Directory.Exists(ROOT_FILES_DIRECTORY))
                Directory.CreateDirectory(ROOT_FILES_DIRECTORY);

            client = new WebClient();
        }

        public static bool DownloadFileIfNotExists(DateTime forecastDate, out string errorMessage, ref string filePath)
        {
            errorMessage = string.Empty;

            try
            {
                int offsetDays = 0;
                DateTime currentTime = DateTime.Now;
                string timeStr = string.Empty;

                if(forecastDate > currentTime)
                {
                    offsetDays = (int)(forecastDate - DateTime.Now).TotalHours;
                    timeStr = currentTime.ToString("yyyyMMdd");
                }
                else
                {
                    offsetDays = (int)(DateTime.Now - forecastDate).TotalHours;
                    timeStr = forecastDate.ToString("yyyyMMdd");
                }

                string offset = offsetDays.ToString("D3");

                string url = URL;
                url = url.Replace("{time}", timeStr);
                url = url.Replace("{offset}", offset);

                string fileName = GetFileNameFromUrl(url);
                filePath = ROOT_FILES_DIRECTORY + $"\\{fileName}";

                if (!IsFileExists(fileName))
                    client.DownloadFile(url, filePath);

                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return false;
        }


        private static bool IsFileExists(string fileName)
        {
            return File.Exists($"{ROOT_FILES_DIRECTORY}/{fileName}");
        }
        private static string GetFileNameFromUrl(string url)
        {
            int index = url.LastIndexOf('/');
            return url.Substring(index + 1, url.Length - index - 1);
        }
    }
}
