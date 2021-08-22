using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMS.Models
{
    public class MMSMessage
    {
        public string MessageType { get; set; }
        public string TransactionId { get; set; }
        public string Version { get; set; } = "1.2";
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string Sender { get; set; }
        public string MessageID { get; set; }
        public long MessageSize { get; set; }
        public DateTime Expiry { get; set; }
        public string OriginatorSystem { get; set; }
    }
}