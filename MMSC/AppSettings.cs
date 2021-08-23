using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace MMSC
{
    public class AppSettings
    {
        private static Dictionary<string, string> Parameters;
        static AppSettings()
        {
            Parameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(ConfigurationManager.AppSettings["AppSettingsFile"]));
        }

        public string GetValue(string key)
        {
            string str = string.Empty;
            if (Parameters.TryGetValue(key, out str))
            {
                return str;
            }
            return String.Empty;
        }

        public static int PPGBoundedCapacity
        {
            get
            {
                int value = 20000;
                if (int.TryParse(Parameters["PPGBoundedCapacity"], out value))
                {
                    return value;
                }
                return 20000;
            }
        }

        public static int PPGMaxDegreeOfParallelism
        {
            get
            {
                int value = 1;
                if (int.TryParse(Parameters["PPGMaxDegreeOfParallelism"], out value))
                {
                    return value;
                }
                return 1;
            }
        }
    }
}