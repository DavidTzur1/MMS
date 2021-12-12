using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMSC.Tables
{
    public class ResponseStatuses
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Dictionary<string, int> Encoder;
        public static Dictionary<int, string> Decoder;

        public static readonly byte Ok = 0x80;
        public static readonly byte ErrorNetworkProblem = 0x86;
        public static readonly byte ErrorServiceDenied = 0x82;
        public static readonly byte ErrorSendingAddressUnresolved = 132;

        static ResponseStatuses()
        {


            Decoder = new Dictionary<int, string>()
            {
                [0x80] = "Ok",
                [0x81] = "Error-unspecified",
                [0x82] = "Error- service-denied",
                [0x83] = "Error-message-format-corrupt",
                [0x84] = "Error-sending-address-unresolved",
                [0x85] = "Error-message-not-found",
                [0x86] = "Error-network-problem",
                [0x87] = "Error-content-not-accepted",
                [0x88] = "Error-unsupported-message"
            };
        }
    }
}