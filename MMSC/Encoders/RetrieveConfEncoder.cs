using MMSC.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MMSC.Encoders
{
    public class RetrieveConfEncoder
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly byte MESSAGE_TYPE = 0x8C;
        public static readonly byte TRANSACTION_ID = 0x98;
        public static readonly byte MMS_VERSION = 0x8D;
        public static readonly byte RESPONSE_STATUS = 0x92;
        public static readonly byte RESPONSE_TEXT = 0x93;
        public static readonly byte MESSAGE_ID = 0x8B;
        public static readonly byte DATE = 0x85;
        public static readonly byte FROM = 0x89;
        public static readonly byte TO = 0x97;
        public static readonly byte MESSAGE_CLASS = 0x8A;
        public static readonly byte PRIORITY = 0x8F;
        public static readonly byte DELIVERY_REPORT = 0x86;
        public static readonly byte READ_REPORT = 0x90;
        public static readonly byte CONTENT_TYPE = 0x84;
        public static readonly byte RETRIEVE_STATUS = 0x99;
        public static readonly byte RETRIEVE_TEXT = 0x9A;


        public string MessageType { get; set; }
        public string TransactionID { get; set; }
        public string Version { get; set; }
        public int RetrieveStatus { get; set; }
        public string RetrieveText { get; set; }
        public string MessageID { get; set; }
        public string ContentType { get; set; }

        public string ContentTypeRaw { get; set; }
        public string Data { get; set; }
        public string From { get; set; }
        public string To { get; set; }



        /**
	 * Encode the message to a valid mms message
	 * @return a byte array of the message
	 */

        public byte[] Encode(bool flag=true)
        {
            MemoryStream outBuffer = new MemoryStream();
            try
            {
                
                // Type
                outBuffer.WriteByte(MESSAGE_TYPE);
                outBuffer.WriteByte(0X84);

                // Transaction ID
                if (flag)
                {
                    outBuffer.WriteByte(TRANSACTION_ID);
                    outBuffer.Write(Encoding.UTF8.GetBytes(TransactionID), 0, TransactionID.Length);
                    outBuffer.WriteByte(0x0);
                }

                // Mms version
                outBuffer.WriteByte(MMS_VERSION);
                outBuffer.WriteByte(0x92);

                // Message ID
                if (flag)
                {
                    outBuffer.WriteByte(MESSAGE_ID);
                    outBuffer.Write(Encoding.UTF8.GetBytes(MessageID), 0, MessageID.Length);
                    outBuffer.WriteByte(0x0);
                }

                // Date
                outBuffer.WriteByte(DATE);
                byte[] date = Tools.GetBytes(DateTimeOffset.Now.ToUnixTimeSeconds().ToString("x"));
                outBuffer.WriteByte((byte)date.Length);
                outBuffer.Write(date, 0, date.Length);

                // From
                if (flag)
                {
                    outBuffer.WriteByte(FROM);
                    outBuffer.WriteByte((byte)(From.Length + 2));
                    outBuffer.WriteByte(0x80);
                    outBuffer.Write(Encoding.UTF8.GetBytes(From), 0, From.Length);
                    outBuffer.WriteByte(0x0);
                }

                // To
                outBuffer.WriteByte(TO);
                outBuffer.Write(Encoding.UTF8.GetBytes(To), 0, To.Length);
                outBuffer.WriteByte(0x0);

                // MESSAGE_CLASS
                outBuffer.WriteByte(MESSAGE_CLASS);
                outBuffer.WriteByte(0x80);

                // PRIORITY
                outBuffer.WriteByte(PRIORITY);
                outBuffer.WriteByte(0x81);

                // DELIVERY_REPORT
                outBuffer.WriteByte(DELIVERY_REPORT);
                outBuffer.WriteByte(0x81);

                // READ_REPORT
                outBuffer.WriteByte(READ_REPORT);
                outBuffer.WriteByte(0x81);

                //ResponseStatus
                //outBuffer.WriteByte(RETRIEVE_STATUS);
                //outBuffer.WriteByte((byte)RetrieveStatus);

                //Contant Type 
                outBuffer.WriteByte(CONTENT_TYPE);
                outBuffer.WriteByte(0xa3);//application/vnd.wap.multipart.mixed

                

                //Data
                if (flag)
                {
                    byte[] data = Tools.GetBytes(Data);
                    outBuffer.Write(data, 0, data.Length);
                }


                return outBuffer.ToArray();
            }


            catch (Exception ex)
            {
                log.Error(ex);
            }
            return null;
        }
    }
}