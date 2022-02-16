using MailKit.Net.Smtp;
using MimeKit;
using MMSC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
//using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace MMSC.API
{
    public class SMTPClient
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static int counter = 1;
        
        public async static Task<bool> Send(SMTPMessageModel message)
        {
            try
            {
                using (var smtpClient = new SmtpClient())
                {                   
                    //await smtpClient.ConnectAsync("172.17.120.9", 25, MailKit.Security.SecureSocketOptions.None);
                    await smtpClient.ConnectAsync(AppSettings.MTAServer.IP, AppSettings.MTAServer.Port, MailKit.Security.SecureSocketOptions.None);
                    await smtpClient.SendAsync(message.Data, message.From, new[] { message.To });
                    await smtpClient.DisconnectAsync(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return true;
            }
        }

        public async static Task<bool> Send(SMTPResModel message)
        {
           // Stopwatch sw = Stopwatch.StartNew();
           // int local = 0;
            try
            {
                
                using (var smtpClient = new SmtpClient())
                {
                    
                    await smtpClient .ConnectAsync(AppSettings.MTAServer.IP, AppSettings.MTAServer.Port, MailKit.Security.SecureSocketOptions.None);
                    await smtpClient.SendAsync(message.Data, message.From, new[] { new MailboxAddress("", message.OriginatorSystem) });
                    await smtpClient.DisconnectAsync(true);
                    //
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return true;
            }
            
            
        }
    }
}