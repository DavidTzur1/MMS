using MMSC.API;
using MMSC.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMSC.Models
{
    public enum QualityOfServiceLevel
    {
        NotSpecified,
        Unconfirmed,
        PreferConfirmed,
        Confirmed
    }
    public class MMSNotificationModel
    {
        public string MessageType { get; set; } = string.Empty;
        public string TransactionID { get; set; }
        public string PushID { get; private set; }
        public string Version { get; set; } = "1.2";
        public string To { get; set; }
        public string MessageID { get; set; }
        public string From { get; set; }
        
        public long MessageSize { get; set; }

        public long Expiry { get; set; }
        public string MessageClass { get; set; } = "Personal";

        public int Status { get; set; }



        public MMSNotificationModel()
        {
            TransactionID = Tools.UniqueID;
            PushID = Tools.UniqueID + Tools.GenerateRandom();
            QualityOfService = QualityOfServiceLevel.Unconfirmed;
            

        }

        
        public QualityOfServiceLevel QualityOfService { get; set; }
        /// <summary>
        /// Address (e.g. URL) that Blackberry push service could use for notification
        /// of results related to the message
        /// </summary>
        public string PpgNotifyRequestedTo { get; set; }
        /// <summary>
        /// Date and time by which the content must be delivered,expressed as UTC
        /// Message that has aged beyond this date will not be transmitted
        /// </summary>
        public string DeliverBeforeTimestamp
        {
            get
            {
                return DateTime.UtcNow.AddSeconds(Expiry).ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
        }
        /// <summary>
        /// Date and time after which the content should be delivered,expressed as UTC
        /// Message will not be transmitted before this date
        /// </summary>
        public DateTime? DeliverAfterTimestamp { get; set; }


        public string ContentLocation
        {
            get
            {
                return $"{AppSettings.PPG.ContentLocation}?messageid={PushID}";

            }
        }
       
        
        public string ContentType { get; set; } = "application/vnd.wap.mms-message";
        public Dictionary<string,string> Headers { get; set; }

        public byte[] Content
        {
            get
            {
                NotificationIndEncoder encoder = new NotificationIndEncoder(this);
                return encoder.Encode();
            }
        }

        public string PapXml
        {
            get
            {
                string str = $"<?xml version=\"1.0\"?>\r\n" +
                    $"<!DOCTYPE pap PUBLIC \"-//WAPFORUM//DTD PAP 1.0//EN\"\r\n         \"http://www.wapforum.org/DTD/pap_1.0.dtd\">" +
                $"<pap><push-message push-id=\"{PushID}\" deliver-before-timestamp=\"{DeliverBeforeTimestamp}\" ppg-notify-requested-to=\"{AppSettings.PPG.NotifyRequestedTo}\">" +
                        $"<address address-value=\"WAPPUSH={To}\"></address>" +
                        $"<quality-of-service priority=\"medium\" delivery-method=\"unconfirmed\"></quality-of-service>" +
                        $"</push-message></pap>";

                return str;
            }
        }

        public override string ToString()
        {
            return $"{MessageType}|{TransactionID}|{MessageID}|{PushID}|{From}|{To}";
        }
    }
}