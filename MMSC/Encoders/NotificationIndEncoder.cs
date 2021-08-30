using MMSC.API;
using MMSC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MMSC.Encoders
{
    public class NotificationIndEncoder
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

        private MMSNotificationModel Message ;
        public NotificationIndEncoder(MMSNotificationModel message)
        {
            Message = message;
        }

        public byte[] Encode()
        {
            //MemoryStream outBuffer = new MemoryStream();
            try
            {
                using (MemoryStream outBuffer = new MemoryStream())
                {
                    // Type
                    outBuffer.WriteByte(MESSAGE_TYPE);
                    outBuffer.WriteByte(0X82);

                    // Transaction ID
                    outBuffer.WriteByte(TRANSACTION_ID);
                    outBuffer.Write(Encoding.UTF8.GetBytes(Message.TransactionID), 0, Message.TransactionID.Length);
                    outBuffer.WriteByte(0x0);

                    // Mms version
                    outBuffer.WriteByte(MMS_VERSION);
                    outBuffer.WriteByte(0x92);

                    //From
                    outBuffer.WriteByte(FROM);
                    outBuffer.WriteByte((Byte)(Message.From.Length + 2));
                    outBuffer.WriteByte(0x80);
                    outBuffer.Write(Encoding.UTF8.GetBytes(Message.From), 0, Message.From.Length);
                    outBuffer.WriteByte(0x0);

                    // MessageClass
                    outBuffer.WriteByte(MESSAGE_CLASS);
                    outBuffer.WriteByte(0x80);

                    // MessageSize
                    outBuffer.WriteByte(MESSAGE_SIZE);
                    string hexString = Message.MessageSize.ToString("x2");
                    hexString = (hexString.Length % 2 == 0 ? "" : "0") + hexString;
                    outBuffer.WriteByte((Byte)(hexString.Length / 2));
                    outBuffer.Write(Tools.GetBytes(hexString), 0, hexString.Length / 2);


                    // Eepirty
                    outBuffer.WriteByte(EXPIRY);
                    string hexStringSec = Message.Expiry.ToString("x2");
                    hexStringSec = (hexStringSec.Length % 2 == 0 ? "" : "0") + hexStringSec;
                    outBuffer.WriteByte((Byte)(hexStringSec.Length / 2 + 2));
                    outBuffer.WriteByte(0x81);
                    outBuffer.WriteByte((Byte)(hexStringSec.Length / 2));
                    outBuffer.Write(Tools.GetBytes(hexStringSec), 0, hexStringSec.Length / 2);
                    

                    //ContentLocation
                    outBuffer.WriteByte(CONTENT_LOCATION);
                    outBuffer.Write(Encoding.UTF8.GetBytes(Message.ContentLocation), 0, Message.ContentLocation.Length);
                    outBuffer.WriteByte(0x0);

                    return outBuffer.ToArray();
                }
            }


            catch (Exception ex)
            {
                log.Error(ex);
            }
            return null;
        }
    }
}