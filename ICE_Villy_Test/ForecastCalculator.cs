using CliWrap;
using CliWrap.Buffered;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ICE_Villy_Test
{
    public static class ForecastCalculator
    {
        public const float KELVIN_TO_CELSIUS = -273.15f;
        public static readonly string GRIB_CLI_EXE_PATH = $"{Config.ROOT_FOLDER_PATH}wgrib2\\wgrib2.exe";

        public static float GetCelcius(string response)
        {
            int index = response.LastIndexOf('=');
            string kelvinStr = response.Substring(index + 1, response.Length - index - 1);

            return float.Parse(kelvinStr) * KELVIN_TO_CELSIUS;
        }

        public static string RunCommand(string filePath, float longitude, float latitude)
        {
            string command = @"{cli} {filePath} -match "":(TMP:2 m above ground):"" -lon {LON} {LAT}";

            command = command.Replace("{cli}", GRIB_CLI_EXE_PATH);
            command = command.Replace("{filePath}", filePath);
            command = command.Replace("{LON}", longitude.ToString());
            command = command.Replace("{LAT}", latitude.ToString());

            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/c {command}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return output;
        }
    }
}
