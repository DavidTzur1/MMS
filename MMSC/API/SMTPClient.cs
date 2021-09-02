using MailKit.Net.Smtp;
using MMSC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MMSC.API
{
    public class SMTPClient
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async static Task<bool> Send(SMTPMessageModel message)
        {
            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Connect("172.17.120.9", 25, MailKit.Security.SecureSocketOptions.None);
                    //log.Debug($"Message From{message.From} To {message.To }");
                    await smtpClient.SendAsync(message.Data, message.From, new[] { message.To });
                    smtpClient.Disconnect(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex);
                return true;
            }
        }
    }
}