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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static public XElement XMLSettings { get; set; }
        static AppSettings()
        {
            XMLSettings = XElement.Load(ConfigurationManager.AppSettings["XMLSettingsFile"]);
        }

        public class Service
        {
            public static string Status
            {
                get
                {
                    return AppSettings.XMLSettings.Element("Service").Attribute("Status").Value;

                }
            }
        }

        public class MANAGER
        {
            public static int MaxDegreeOfParallelism
            {
                get
                {
                    int value = 1;

                    if (int.TryParse(AppSettings.XMLSettings.Element("MANAGER").Attribute("MaxDegreeOfParallelism").Value, out value))
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

                    if (int.TryParse(AppSettings.XMLSettings.Element("MANAGER").Attribute("BoundedCapacity").Value, out value))
                    {
                        return value;
                    }
                    return 1;
                }
            }
        }

        public class PPG
        {
            public static string URL
            {
                get
                {
                    return AppSettings.XMLSettings.Element("PPG").Attribute("URL").Value;

                }
            }
            public static string AuthToken
            {
                get
                {
                    return AppSettings.XMLSettings.Element("PPG").Attribute("AuthToken").Value;
                    //return Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Parameters["UserName"]}:{Parameters["Password"]}"));

                }
            }

            public static string NotifyRequestedTo
            {
                get
                {
                    return AppSettings.XMLSettings.Element("PPG").Attribute("NotifyRequestedTo").Value;
                }
            }





            public static int Timeout
            {
                get
                {
                    int value = 20;

                    if (int.TryParse(AppSettings.XMLSettings.Element("PPG").Attribute("Timeout").Value, out value))
                    {
                        return value;
                    }
                    return 20;
                }
            }



            public static string ContentLocation
            {
                get
                {
                    return AppSettings.XMLSettings.Element("PPG").Attribute("ContentLocation").Value;
                }
            }
        }
        public class IR
        {
            public static string IP
            {
                get
                {
                    return AppSettings.XMLSettings.Element("IR").Attribute("IP").Value;

                }
            }

            public static string PlatformName
            {
                get
                {
                    return AppSettings.XMLSettings.Element("IR").Attribute("PlatformName").Value;

                }
            }
            public static string PlatformUser
            {
                get
                {
                    return AppSettings.XMLSettings.Element("IR").Attribute("PlatformUser").Value;

                }
            }
            public static string PlatformPwd
            {
                get
                {
                    return AppSettings.XMLSettings.Element("IR").Attribute("PlatformPwd").Value;

                }
            }

            public static int Timeout
            {
                get
                {
                    int value = 20;

                    if (int.TryParse(AppSettings.XMLSettings.Element("IR").Attribute("Timeout").Value, out value))
                    {
                        return value;
                    }
                    return 20;
                }
            }

        }

        public class SMTPServer
        {
            public static string IP
            {
                get
                {
                    return AppSettings.XMLSettings.Element("SMTPServer").Attribute("IP").Value;

                }
            }

            public static int Port
            {
                get
                {
                    int value = 25;

                    if (int.TryParse(AppSettings.XMLSettings.Element("SMTPServer").Attribute("Port").Value, out value))
                    {
                        return value;
                    }
                    return 25;
                }
            }

        }
        public class OPERATORS
        {
            public class Operator
            {
               public string Name { get; set; }
                public string Domain { get; set; }
                public bool IsInternal { get; set; }
                public Operator()
                {

                }

            }
           
            

            public static Operator GetOperatorInfo(string name)
            {
                Operator op = null;
                try
                {
                    var xml = AppSettings.XMLSettings.Element("OPERATORS").Elements("OPERATOR").First(item => item.Attribute("Name").Value == name); ;
                    op = new Operator();
                    op.Name = xml.Attribute("Name").Value;
                    op.Domain = xml.Attribute("Domain").Value;
                    op.IsInternal = xml.Attribute("IsInternal").Value == "1" ? true : false;
                    return op;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return null;
                }
                
            }
            
        }

       
    }
}