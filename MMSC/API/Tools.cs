using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace MMSC.API
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

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static string GetHexString(byte[] arr)
        {
            var sb = new StringBuilder(arr.Length * 2);
            foreach (byte b in arr)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();
        }
    }
}