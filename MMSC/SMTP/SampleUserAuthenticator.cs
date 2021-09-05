using SmtpServer;
using SmtpServer.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MMSC.SMTP
{
    public class SampleUserAuthenticator : IUserAuthenticator, IUserAuthenticatorFactory
    {
        public SampleUserAuthenticator()
        {

        }
        public Task<bool> AuthenticateAsync(ISessionContext context, string user, string password, CancellationToken token)
        {
            Console.WriteLine("User={0} Password={1}", user, password);

            return Task.FromResult(user.Length > 4);
        }

        public IUserAuthenticator CreateInstance(ISessionContext context)
        {
            return new SampleUserAuthenticator();
        }
    }
}