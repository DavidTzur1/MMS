using SmtpServer;
using SmtpServer.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace MMSC.SMTP
{
    public class SMTPServerManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async static void Run()
        {
            log.Debug("Run MMSSMTPServer");

            var options = new SmtpServerOptionsBuilder()
                .ServerName("localhost")
                .Port(25)
                .Build();

            var serviceProvider = new ServiceProvider();
            serviceProvider.Add(new SampleMessageStore());
            //serviceProvider.Add(new SampleMailboxFilter());
           // serviceProvider.Add(new SampleUserAuthenticator());

            var smtpServer = new SmtpServer.SmtpServer(options, serviceProvider);
            await smtpServer.StartAsync(CancellationToken.None);

        }
    }
}