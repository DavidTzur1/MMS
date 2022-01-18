using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMSC.Models
{

    public class SMTPResModel
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        public string OriginatorSystem {get;set;}
        public string TransactionId { get; set; }
        public string MessageID { get; set; }

        public MailboxAddress From
        {
            get
            {
                return new MailboxAddress("admin", "admin@oklik.net");
            }
        }


        public MimeMessage Data
        {
            get
            {
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(From);
                mailMessage.To.Add(new MailboxAddress("", OriginatorSystem));
                mailMessage.Subject = "MM4_forward.RES";
                mailMessage.Headers.Add("X-Mms-3GPP-MMS-Version", "6.5.0");
                mailMessage.Headers.Add("X-Mms-MMS-Version", "1.2");
                mailMessage.Headers.Add("X-Mms-Transaction-ID", TransactionId);
                mailMessage.Headers.Add("X-Mms-Message-ID", MessageID);
                mailMessage.Headers.Add("X-Mms-Message-Type", "MM4_forward.RES");
                mailMessage.Headers.Add("X-Mms-Request-Status-Code", "Ok");
                mailMessage.Headers.Add("Content-Type", "text/plain");

                return mailMessage;

            }
        }



        public SMTPResModel()
        {


        }

    }
}