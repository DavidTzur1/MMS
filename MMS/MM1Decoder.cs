using MMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMS
{
    public class MM1Decoder
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /*--------------------------*
       * Array of header contents *
       *--------------------------*/
        public static Dictionary<int, string> MMS_MESSAGE_TYPES = new Dictionary<int, string>()
            {
                {0x80, "m-send-req"},
                {0x81, "m-send-conf"},
                {0x82, "m-notification-ind"},
                {0x83, "m-notifyresp-ind"},
                {0x84, "m-retrieve-conf"},
                {0x85, "m-acknowledge-ind"},
                {0x86, "m-delivery-ind" }
            };

        private byte[] PDU;
        public MM1Decoder(Byte[] pdu)
        {
            PDU = pdu;
        }

        public MMSMessage Parse()
        {
            MMSMessage message = new MMSMessage();
            int pos = 0;
            try
            {
                return message;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                return null;
            }
            
        }
    }
}