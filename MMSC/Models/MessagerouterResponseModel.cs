using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMSC.Models
{



    public class MessagerouterResponseModel
    {
        public string TransactionID {get;set;}
        public string MessageType { get; set; } = "MM7Submit.Res";
        public string Version { get; set; } = "5.3.0";
        public int RequestStatus { get; set; }
        public string RequestStatusText { get; set; }
        public string MessageID { get; set; }
        public MessagerouterResponseModel()
        {

        }

        public override string ToString()
        {
            
            
            return $"<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                $"<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                $"<SOAP-ENV:Header>" +
                $"<MM7Header xmlns=\"urn:xml-MM7Header\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" +
                $"<Transaction-ID xsi:type =\"xsd:string\">{TransactionID}</Transaction-ID>" +
                $"</MM7Header>" +
                $"</SOAP-ENV:Header>" +
                $"<SOAP-ENV:Body>" +
                $"<MultiMediaSubmitResponse xmlns=\"urn:MM7Submit.Res\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" +
                $"<Message-Type xsi:type=\"xsd: string\">{MessageType}</Message-Type>" +
                $"<Version xsi:type=\"xsd: string\">{Version}</Version>" +
                $"<Request-Status xsi:type=\"xsd:string\">{RequestStatus}</Request-Status>" +
                $"<Message-ID xsi:type=\"xsd:string\">{MessageID}</Message-ID>" +
                $"<Request-Status-Text xsi:type=\"xsd: string\">{RequestStatusText}</Request-Status-Text>" +
                $"</MultiMediaSubmitResponse>" +
                $"</SOAP-ENV:Body>" +
                $"</SOAP-ENV:Envelope>";
        }
    }

    


}