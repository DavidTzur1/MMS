using SmtpServer;
using SmtpServer.Mail;
using SmtpServer.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MMSC.SMTP
{
    public class SampleMailboxFilter : IMailboxFilter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Task<MailboxFilterResult> CanAcceptFromAsync(ISessionContext context, IMailbox @from, int size, CancellationToken cancellationToken)
        {
            log.Debug(@from.Host);
            return Task.FromResult(MailboxFilterResult.Yes);
            //if (String.Equals(@from.Host, "test.com"))
            //{
            //    return Task.FromResult(MailboxFilterResult.Yes);
            //}

            //return Task.FromResult(MailboxFilterResult.NoPermanently);
        }

        public  Task<MailboxFilterResult> CanDeliverToAsync(ISessionContext context, IMailbox to, IMailbox @from, CancellationToken token)
        {
            return Task.FromResult(MailboxFilterResult.Yes);
        }

        //public IMailboxFilter CreateInstance(ISessionContext context)
        //{
        //    return new SampleMailboxFilter();
        //}
    }
}