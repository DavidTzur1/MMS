﻿using MMSC.Actions;
using MMSC.Decoders;
using MMSC.Encoders;
using MMSC.Models;
using MMSC.Tables;
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
    public class MMSController : ApiController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly log4net.ILog cdr = log4net.LogManager.GetLogger("cdr");

        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            IEnumerable<string> headerValues;
            long contentLength;
            MMSMessageModel message = null; ;
            try
            {
               
                byte[] body = await Request.Content.ReadAsByteArrayAsync();

                //log.Debug(body.Length);
                ///////Get seander info from header/////////////////////////////////////
                string from = "";
                if (Request.Headers.TryGetValues("X-Wap-MSISDN", out headerValues))
                {
                    from = Decoder.DeviceAddress(headerValues.FirstOrDefault());
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
                message = decoder.Parse();

                if (message.MessageType == MM1Decoder.MMS_MESSAGE_TYPES[0x80]) //{0x80, "m-send-req"}
                {
                    message.From = from;
                    message.MessageSize = contentLength;
                    message.Sender = "oklik.net";

                    string messageID = DBApi.InsertMessage.Execute(message);

                    SendConfEncoder sendConf = new SendConfEncoder();
                    if (String.IsNullOrEmpty(messageID))
                    {
                        sendConf.TransactionId = message.TransactionId;
                        sendConf.ResponseStatus = ResponseStatuses.ErrorNetworkProblem;
                    }
                    else
                    {
                        message.MessageID = messageID;
                        ManagerAction.ActionBlock.Post(message);
                        sendConf.TransactionId = message.TransactionId;
                        sendConf.ResponseStatus = ResponseStatuses.Ok;
                        sendConf.MessageID = messageID;
                    }

                    var result = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StreamContent(new MemoryStream(sendConf.Encode()))
                    };
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.wap.mms-message");
                    log.Info(message.ToString());
                    return ResponseMessage(result);

                }
                else if (message.MessageType == MM1Decoder.MMS_MESSAGE_TYPES[0x83]) //{0x83, "m-notifyresp-ind"}
                {
                   
                    MMSMessageEventModel notifyresp = await DBApi.GetMessageEventInfo.Execute(message.TransactionId);
                    notifyresp.TransactionID = message.TransactionId;
                    notifyresp.MessageType = message.MessageType;
                    notifyresp.Status = message.Status;
                    notifyresp.To = from;
                    await DBApi.InsertMessageEvent.Execute(notifyresp);

                    var result = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StreamContent(new MemoryStream())
                    };

                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.wap.mms-message");
                    cdr.Info(notifyresp.ToString());
                    log.Info(notifyresp.ToString());
                    return ResponseMessage(result);
                }
                else if (message.MessageType == MM1Decoder.MMS_MESSAGE_TYPES[0x85]) //{0x85, "m-acknowledge-ind"}
                {
                    //message.From = $"{from}/TYPE=PLMN";
                    //await DBApi.InsertNotificationResp.Execute(message);

                    MMSMessageEventModel acknowledge = await DBApi.GetMessageEventInfo.Execute(message.TransactionId);
                    acknowledge.TransactionID = message.TransactionId;
                    acknowledge.MessageType = message.MessageType;
                    acknowledge.Status = message.Status;
                    acknowledge.To = from;
                    await DBApi.InsertMessageEvent.Execute(acknowledge);

                    var result = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StreamContent(new MemoryStream())
                    };

                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.wap.mms-message");
                    cdr.Info(acknowledge.ToString());
                    log.Info(acknowledge.ToString());
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

