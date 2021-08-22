using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;

namespace MMS
{
    public class Tools
    {
        // private static long lastTimeStamp = DateTime.UtcNow.Ticks;
        private static long lastTimeStamp = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"));

        public static string UniqueID
        {
            get
            {
                long original, newValue;
                do
                {

                    original = lastTimeStamp;
                    //long now = DateTime.UtcNow.Ticks;
                    long now = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"));
                    newValue = Math.Max(now, original + 1);
                } while (Interlocked.CompareExchange(ref lastTimeStamp, newValue, original) != original);
                string uniqueID = newValue.ToString() + ConfigurationManager.AppSettings[Environment.MachineName];
                return uniqueID;
            }
        }
    }
}