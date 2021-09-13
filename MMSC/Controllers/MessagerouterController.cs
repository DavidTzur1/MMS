using MMSC.Actions;
using MMSC.Decoders;
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
    public class MessagerouterController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return StatusCode(HttpStatusCode.UnsupportedMediaType);
                }

                var multipart = await Request.Content.ReadAsMultipartAsync();
                MMSMessageModel message = await MM7Decoder.Parse(multipart);
                if (message == null) return null;
                string messageID =  DBApi.InsertMessage.Execute(message);
                MessagerouterResponseModel res = new MessagerouterResponseModel();
                if (String.IsNullOrEmpty(messageID))
                {
                    res.TransactionID = message.TransactionId;
                    res.RequestStatus = -1;
                    res.RequestStatusText = "Fail";
                }
                else
                {
                    message.MessageID = messageID;
                    ManagerAction.ActionBlock.Post(message);
                    res.TransactionID = message.TransactionId;
                    res.RequestStatus = 1000;
                    res.RequestStatusText = "Successfully sent";
                    res.MessageID = messageID;
                }
                

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(res.ToString())
                };

                log.Info(message.ToString());
                return ResponseMessage(result);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }

        }
    }
}

