using MailKit.Net.Smtp;
using MMSC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks.Dataflow;
using System.Web;

namespace MMSC.Actions
{
    public class ManagerAction
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ActionBlock<MMSMessageModel> ActionBlock;
        //static ExecutionDataflowBlockOptions options;
        private static SmtpClient smtpClient;

        private static HttpClient httpClient;
    }
}