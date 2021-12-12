using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMSC.Models
{
    public class MMSMessageModel
    {
        public string MessageType { get; set; } = string.Empty;
        public string TransactionId { get; set; }
        public string Version { get; set; } = "1.2";
        public List<string> To { get; set; }
        public string BCC { get; set; }
        public string CC { get; set; }
        public string ContentLocation { get; set; }
        public string Subject { get; set; } = "MMS Message";
        public string Sender { get; set; } = "";
        public string From { get; set; }
        
        public string MessageID { get; set; }
        public long MessageSize { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public long Expiry { get; set; } = 259200;
        public string OriginatorSystem { get; set; }
        public string MessageClass { get; set; } = "Personal";
        public string Priority { get; set; } = "Normal";
        public int SenderVisibility { get; set; }
        public int DeliveryReport { get; set; }
        public int ReadReport { get; set; }
        public int ResponseStatus { get; set; }
        public string ResponseText { get; set; } = "";
        public int Status { get; set; } = 0;
        public int ReportAllowed { get; set; }
        public string ContentType { get; set; } = "application/vnd.wap.multipart.related";
        public string ContentTypeRaw { get; set; } = string.Empty;
        public string Data { get; set; } 
        public string PushID { get; set; } = string.Empty;
        public string MediaType { get; set; } = string.Empty;
        

        public List<MMSPartModel> Parts { get; set; } = new List<MMSPartModel>();

        public MMSMessageModel()
        {
            To = new List<string>();
        }

        public override string ToString()
        {
            return $"{MessageType}|{TransactionId}|{MessageID}|PushID|{MediaType}|{From}|{string.Join(";", To)}|{ResponseStatus}";
        }
        public string ToString(string to)
        {
           // return $"{MessageType}|{TransactionId}|{MessageID}|PushID|{From}|{to}";
            return $"{MessageType}|{TransactionId}|{MessageID}|PushID|{MediaType}|{From}|{to}|{ResponseStatus}";
        }

      


    }
}