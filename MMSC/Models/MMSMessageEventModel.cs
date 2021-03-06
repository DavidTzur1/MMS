using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMSC.Models
{
    public class MMSMessageEventModel
    {
        public string MessageType { get; set; } = string.Empty;
        public string TransactionID { get; set; }
        public string PushID { get; set; } = string.Empty;
        public string MessageID { get; set; } =  string.Empty;
        public string DomainSender { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string DomainRcpt { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;

        public int Status { get; set; }
        public string MediaType { get; set; } = string.Empty;
        public string Info { get; set; } = "Null";

        public override string ToString()
        {
            return $"{MessageType}|{MediaType}|{TransactionID}|{MessageID}|{PushID}|{DomainSender}|{From}|{DomainRcpt}|{To}|{Status}|{Info}";
        }
    }
}