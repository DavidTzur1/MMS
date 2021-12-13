﻿using MMSC.Decoders;
using MMSC.Encoders;
using MMSC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
//using System.Text;
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
            IEnumerable<string> headerValues;
            MMSMessageModel message = null; ;
            try
            {
                byte[] body = await Request.Content.ReadAsByteArrayAsync();

                //log.Debug(body.Length);
                ///////Get seander info from header/////////////////////////////////////
                string to = "";
                if (Request.Headers.TryGetValues("X-Wap-MSISDN", out headerValues))
                {
                    to = Decoder.DeviceAddress(headerValues.FirstOrDefault());
                }
                else
                {
                    log.Debug("Not found Header X-Wap-MSISDN");
                    return null;
                }

                message = await MMSC.DBApi.GetMessageByPushID.Execute(messageid);
                message.MessageType = "m-retrieve-conf";
                bool flag=true;
                if (message.MessageID == string.Empty) flag=false;

                RetrieveConfEncoder retrieveConf = new RetrieveConfEncoder() { TransactionID = message.TransactionId, MessageID = message.MessageID, From = message.From, To = to, ContentType = message.ContentType, Data = message.Data ,RetrieveStatus=flag?0x80:0xc1};
                
                var pdu = retrieveConf.Encode(flag);

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(new MemoryStream(pdu))
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.wap.mms-message");

                MMSMessageEventModel notif = new MMSMessageEventModel() { PushID = messageid, MessageType = message.MessageType, TransactionID = message.TransactionId, MessageID = message.MessageID,DomainSender=message.Sender, From = message.From, To = to,DomainRcpt= "oklik.net" ,MediaType=message.MediaType,Info=flag?"Null":"Message not found",Status= flag ? 0x80 : 0xc1 };
                await DBApi.InsertMessageEvent.Execute(notif);
                log.Info(notif.ToString());
                cdr.Info(notif.ToString());
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

