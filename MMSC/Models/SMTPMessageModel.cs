using MimeKit;
using MMSC.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MMSC.Models
{
    public class SMTPMessageModel
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MailboxAddress From
        {
            get
            {
               // log.Debug(message.From);
                return new MailboxAddress("", $"{message.From}@oklik.net");
            }
        }
        public MailboxAddress To
        {
            get
            {
                //log.Debug(message.To);
                if (message.To.First().Contains("@"))
                {
                    return new MailboxAddress("", message.To.First());
                }
                else
                {
                    return new MailboxAddress("", $"{message.To.First()}@{toDomain}");
                }
            }
        }

       
       

        public MimeMessage Data
        {
            get
            {
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress("", message.From));
                mailMessage.To.Add(new MailboxAddress("", message.To.First()));
                mailMessage.Subject = message.Subject;

                mailMessage.Headers.Add("X-Mms-3GPP-MMS-Version", "6.5.0");
                mailMessage.Headers.Add("X-Mms-MMS-Version", "1.2");
                mailMessage.Headers.Add("X-Mms-Transaction-ID", message.TransactionId);
                mailMessage.Headers.Add("X-Mms-Message-ID", message.MessageID);
                mailMessage.Headers.Add("X-Mms-Message-Type", message.MessageType);
                mailMessage.Headers.Add("X-Mms-Originator-System", "admin@oklik.net");
                mailMessage.Headers.Add("X-Mms-Ack-Request", "Yes");

                var builder = new BodyBuilder();
               
                foreach (var part in message.Parts)
                {
                    ContentType contentType = new ContentType(part.ContentType.Split('/').First(), part.ContentType.Split('/').Last());
                    var att = (MimePart)builder.Attachments.Add(part.Name ?? part.ContentId, part.Content, contentType);
                    att.ContentId = part.ContentId;
                    att.ContentLocation = new Uri(part.ContentLocation??part.ContentId,UriKind.RelativeOrAbsolute);
                }
                mailMessage.Body = builder.ToMessageBody();

                return mailMessage;

            }
        }
              

        private MMSMessageModel message;
        private string toDomain;
        public SMTPMessageModel(MMSMessageModel message, string toDomain = "")
        {
            this.message = message;
            this.toDomain = toDomain;
        }

    }
}