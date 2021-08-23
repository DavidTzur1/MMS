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
    }
}