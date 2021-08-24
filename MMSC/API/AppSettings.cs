using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MMSC.API
{
    public class AppSettings
    {
        static public XElement XMLSettings { get; set; }
        static AppSettings()
        {
            XMLSettings = XElement.Load(ConfigurationManager.AppSettings["XMLSettingsFile"]);
        }

        public class PPG
        {
            public static int MaxDegreeOfParallelism
            {
                get
                {
                    int value = 1;

                    if (int.TryParse(AppSettings.XMLSettings.Elements("PPG").First().Attribute("MaxDegreeOfParallelism").Value, out value))
                    {
                        return value;
                    }
                    return 1;
                }
            }
            public static int BoundedCapacity
            {
                get
                {
                    int value = 1;

                    if (int.TryParse(AppSettings.XMLSettings.Elements("PPG").First().Attribute("BoundedCapacity").Value, out value))
                    {
                        return value;
                    }
                    return 1;
                }
            }
        }
        public class IR
        {
            public static string IP
            {
                get
                {
                    return AppSettings.XMLSettings.Elements("IR").First().Attribute("IP").Value;

                }
            }

            public static string PlatformName
            {
                get
                {
                    return AppSettings.XMLSettings.Elements("IR").First().Attribute("PlatformName").Value;

                }
            }
            public static string PlatformUser
            {
                get
                {
                    return AppSettings.XMLSettings.Elements("IR").First().Attribute("PlatformUser").Value;

                }
            }
            public static string PlatformPwd
            {
                get
                {
                    return AppSettings.XMLSettings.Elements("IR").First().Attribute("PlatformPwd").Value;

                }
            }

            public static int Timeout
            {
                get
                {
                    int value = 20;

                    if (int.TryParse(AppSettings.XMLSettings.Elements("PPG").First().Attribute("Timeout").Value, out value))
                    {
                        return value;
                    }
                    return 20;
                }
            }

        }
    }
}