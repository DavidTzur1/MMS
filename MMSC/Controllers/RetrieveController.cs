using MMSC.Encoders;
using MMSC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace MMSC.Controllers
{
    public class RetrieveController : ApiController
    {
       private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private static readonly log4net.ILog cdr = log4net.LogManager.GetLogger("cdr");

        public async Task<IHttpActionResult> Get([FromUri] string messageid)
        {
            MMSMessageModel message = null; ;
            try
            {
                cdr.Info("Test");
                message = await MMSC.DBApi.GetMessageByPushID.Execute(messageid);
                message.MessageType = "m-retrieve-conf";
                RetrieveConfEncoder retrieveConf = new RetrieveConfEncoder() { TransactionID = message.TransactionId, MessageID = message.MessageID, From = message.From, To = message.To.First(), ContentType = message.ContentType, Data = message.Data };
                var pdu = retrieveConf.Encode();

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(new MemoryStream(pdu))
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.wap.mms-message");

                MMSNotificationModel notif = new MMSNotificationModel() { PushID = messageid, MessageType = message.MessageType, TransactionID = message.TransactionId, MessageID = message.MessageID, From = message.From, To = message.To.First() };
                await DBApi.InsertNotification.Execute(notif);
                return ResponseMessage(result);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
            finally
            {
                
                log.Info(message.ToString());
            }


        }
    }
}

