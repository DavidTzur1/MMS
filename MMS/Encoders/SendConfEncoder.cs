using MMS.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MMS.Encoders
{
    public class SendConfEncoder
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly byte MESSAGE_TYPE = 0x8C;
        public static readonly byte TRANSACTION_ID = 0x98;
        public static readonly byte MMS_VERSION = 0x8D;
        public static readonly byte RESPONSE_STATUS = 0x92;
        public static readonly byte RESPONSE_TEXT = 0x93;
        public static readonly byte MESSAGE_ID = 0x8B;


        public string MessageType { get; set; }
        public string TransactionId { get; set; }
        public string Version { get; set; }
        public byte ResponseStatus { get; set; }
        public string ResponseText { get; set; }
        public string MessageID { get; set; }

        /**
	 * Encode the message to a valid mms message
	 * @return a byte array of the message
	 */
        public byte[] Encode()
        {
            MemoryStream outBuffer = new MemoryStream();
            try
            {
                // Type
                outBuffer.WriteByte(MESSAGE_TYPE);
                outBuffer.WriteByte(0X81);

                // Transaction ID
                outBuffer.WriteByte(TRANSACTION_ID);
                outBuffer.Write(Encoding.UTF8.GetBytes(TransactionId), 0, TransactionId.Length);
                outBuffer.WriteByte(0x0);

                // Mms version
                outBuffer.WriteByte(MMS_VERSION);
                outBuffer.WriteByte(0x92);

                // Respons status
                outBuffer.WriteByte(RESPONSE_STATUS);
                outBuffer.WriteByte(ResponseStatus);

                // Respons Text
                outBuffer.WriteByte(RESPONSE_TEXT);
                ResponseText = ResponseStatuses.Decoder[ResponseStatus];
                outBuffer.Write(Encoding.UTF8.GetBytes(ResponseText), 0, ResponseText.Length);
                outBuffer.WriteByte(0x0);

                // Message ID
                outBuffer.WriteByte(MESSAGE_ID);
                outBuffer.Write(Encoding.UTF8.GetBytes(MessageID), 0, MessageID.Length);
                outBuffer.WriteByte(0x0);

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