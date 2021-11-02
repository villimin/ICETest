using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ICE_Villy_Test.Controllers
{
    public class ForecastController : ApiController
    {
        const string CACHE_KEY = "{dateTicks}:{latitude}:{longitude}";

        [Route("api/forecast")]
        public string Get([FromUri]DateTime date, float latitude, float longitude)
        {
            try
            {
                string filePath = string.Empty;
                if (!FileManager.DownloadFileIfNotExists(date, DateTime.Now, out string errorMessage, ref filePath))
                    return errorMessage;

                string key = PrepareKey(CACHE_KEY, date, latitude, longitude);
                if (Cache.Get(key, out float value))
                {
                    return value.ToString();
                }
                else
                {
                    string response = ForecastCalculator.RunCommand(filePath, longitude, latitude);
                    float result = ForecastCalculator.GetCelcius(response);
                    Cache.Set(key, result);
                    return result.ToString();
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private string PrepareKey(string key, DateTime date, float latitude, float longitude)
        {
            StringBuilder builder = new StringBuilder(key);
            builder.Replace("{dateTicks}", date.Ticks.ToString());
            builder.Replace("{latitude}", latitude.ToString());
            builder.Replace("{longitude}", longitude.ToString());

            return builder.ToString();
        }
    }
}
