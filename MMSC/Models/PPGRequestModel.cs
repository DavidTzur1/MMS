using MMSC.API;
using MMSC.Encoders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MMSC.Models
{
   
    public class PPGRequestModel
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly byte MESSAGE_TYPE = 0x8C;
        public static readonly byte TRANSACTION_ID = 0x98;
        public static readonly byte MMS_VERSION = 0x8D;
        public static readonly byte RESPONSE_STATUS = 0x92;
        public static readonly byte RESPONSE_TEXT = 0x93;
        public static readonly byte MESSAGE_ID = 0x8B;
        public static readonly byte FROM = 0x89;
        public static readonly byte MESSAGE_CLASS = 0x8A;
        public static readonly byte MESSAGE_SIZE = 0x8E;
        public static readonly byte EXPIRY = 0x88;
        public static readonly byte CONTENT_LOCATION = 0x83;


        public string TransactionID { get; set; }
        public string PushID { get; set; }

        public string MessageID { get; set; }
        public string To { get; set; }
        public string From { get; set; }

        public long MessageSize { get; set; }

        public long Expiry { get; set; }
        public string ContentType { get; set; } = "application/vnd.wap.mms-message";




        public PPGRequestModel()
        {
            

        }

        public static PPGRequestModel  Create(string messageID,string from,string to,long expiry,long size)
        {
            PPGRequestModel req = new PPGRequestModel();
            try
            {
                req.TransactionID = Tools.UniqueID;
                req.PushID = Tools.UniqueID + Tools.GenerateRandom();
                req.MessageID = messageID;
                req.From = from;
                req.To = to;
                req.Expiry = expiry;
                req.MessageSize = size;
                return req;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                return null;
            }


        }


        
       
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


       

        public byte[] Content
        {
            get
            {
                using (MemoryStream outBuffer = new MemoryStream())
                {
                    // Type
                    outBuffer.WriteByte(MESSAGE_TYPE);
                    outBuffer.WriteByte(0X82);

                    // Transaction ID
                    outBuffer.WriteByte(TRANSACTION_ID);
                    outBuffer.Write(Encoding.UTF8.GetBytes(TransactionID), 0, TransactionID.Length);
                    outBuffer.WriteByte(0x0);

                    // Mms version
                    outBuffer.WriteByte(MMS_VERSION);
                    outBuffer.WriteByte(0x92);

                    //From
                    outBuffer.WriteByte(FROM);
                    outBuffer.WriteByte((Byte)(From.Length + 2));
                    outBuffer.WriteByte(0x80);
                    outBuffer.Write(Encoding.UTF8.GetBytes(From), 0, From.Length);
                    outBuffer.WriteByte(0x0);

                    // MessageClass
                    outBuffer.WriteByte(MESSAGE_CLASS);
                    outBuffer.WriteByte(0x80);

                    // MessageSize
                    outBuffer.WriteByte(MESSAGE_SIZE);
                    string hexString = MessageSize.ToString("x2");
                    hexString = (hexString.Length % 2 == 0 ? "" : "0") + hexString;
                    outBuffer.WriteByte((Byte)(hexString.Length / 2));
                    outBuffer.Write(Tools.GetBytes(hexString), 0, hexString.Length / 2);


                    // Eepirty
                    outBuffer.WriteByte(EXPIRY);
                    string hexStringSec = Expiry.ToString("x2");
                    hexStringSec = (hexStringSec.Length % 2 == 0 ? "" : "0") + hexStringSec;
                    outBuffer.WriteByte((Byte)(hexStringSec.Length / 2 + 2));
                    outBuffer.WriteByte(0x81);
                    outBuffer.WriteByte((Byte)(hexStringSec.Length / 2));
                    outBuffer.Write(Tools.GetBytes(hexStringSec), 0, hexStringSec.Length / 2);


                    //ContentLocation
                    outBuffer.WriteByte(CONTENT_LOCATION);
                    outBuffer.Write(Encoding.UTF8.GetBytes(ContentLocation), 0, ContentLocation.Length);
                    outBuffer.WriteByte(0x0);

                    return outBuffer.ToArray();
                }
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

       
    }
}