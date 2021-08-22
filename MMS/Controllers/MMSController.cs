using MMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace MMS.Controllers
{
    public class MMSController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            IEnumerable<string> headerValues;
            long contentLength;
            try
            {


                byte[] body = await Request.Content.ReadAsByteArrayAsync();

                log.Debug(body.Length);
                ///////Get seander info from header/////////////////////////////////////
                string from = "";
                if (Request.Headers.TryGetValues("X-Wap-MSISDN", out headerValues))
                {
                    from = headerValues.FirstOrDefault();
                }
                else
                {
                    //from = "0549830440";
                    log.Debug("Not found Header X-Wap-MSISDN");
                    return null;
                }

                contentLength = (long)Request.Content.Headers.ContentLength;

                ////////////////////////////////Check validation Request////////////////////////

                MM1Decoder decoder = new MM1Decoder(body);
                MMSMessage message = decoder.Parse();

                if (message.MessageType == MM1Decoder.MMS_MESSAGE_TYPES[0x80])
                {
                    message.From = $"{from}/TYPE=PLMN";
                    message.MessageSize = contentLength;

                    string messageID = DBApi.InsertMessage.Execute(mmsDecoder);

                    SendConf sendConf = new SendConf();
                    if (String.IsNullOrEmpty(messageID))
                    {
                        sendConf.TransactionId = mmsDecoder.TransactionId;
                        sendConf.ResponseStatus = ResponseStatuses.ErrorNetworkProblem;
                    }
                    else
                    {
                        mmsDecoder.MessageID = messageID;
                        MMSAction.ActionBlock.Post(mmsDecoder);
                        sendConf.TransactionId = mmsDecoder.TransactionId;
                        sendConf.ResponseStatus = ResponseStatuses.Ok;
                        sendConf.MessageID = messageID;
                    }

                    var result = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StreamContent(new MemoryStream(sendConf.Encode()))
                    };
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.wap.mms-message");
                    return ResponseMessage(result);

                }
                else if (mmsDecoder.MessageType == MmsDecoder.MMS_MESSAGE_TYPES[0x83])
                {
                    mmsDecoder.From = $"{from}/TYPE=PLMN";
                    DBApi.InsertNotify.Execute(mmsDecoder);

                    var result = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StreamContent(new MemoryStream())
                    };

                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.wap.mms-message");
                    return ResponseMessage(result);
                }
                else if (mmsDecoder.MessageType == MmsDecoder.MMS_MESSAGE_TYPES[0x85])
                {
                    mmsDecoder.From = $"{from}/TYPE=PLMN";
                    DBApi.InsertNotify.Execute(mmsDecoder);

                    var result = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StreamContent(new MemoryStream())
                    };

                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.wap.mms-message");
                    return ResponseMessage(result);

                }
                else
                {
                    log.Error($"The value tag X-Mms-Message-Type not valid");
                    return null;
                }

            }
            catch (Exception ex)
            {
                log.Debug(ex);
                return null;
            }
        }
    }
}
