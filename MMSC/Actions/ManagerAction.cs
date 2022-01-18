using MailKit.Net.Smtp;
using MMSC.API;
using MMSC.Decoders;
using MMSC.Encoders;
using MMSC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
//using System.Text;
using System.Threading.Tasks.Dataflow;
using System.Web;
using static MMSC.API.AppSettings.OPERATORS;

namespace MMSC.Actions
{
    public class ManagerAction
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly log4net.ILog cdr = log4net.LogManager.GetLogger("cdr");

        /// <summary>
        ///  aaa
        /// </summary>

        public static ActionBlock<MMSMessageModel> ActionBlock;


        static ManagerAction()
        {

            ActionBlock = new ActionBlock<MMSMessageModel>(fn, new ExecutionDataflowBlockOptions { BoundedCapacity = AppSettings.MANAGER.BoundedCapacity, MaxDegreeOfParallelism = AppSettings.MANAGER.MaxDegreeOfParallelism });
        }

        static Action<MMSMessageModel> fn = async req =>
        {
          
            try
            {
                foreach (var item in req.To)
                {
                    
                    

                    if (item.Contains("@"))
                    {
                        //SMTP message
                        MMSMessageModel message = new MMSMessageModel() { MessageType = req.MessageType,MediaType=req.MediaType, TransactionId = req.TransactionId, MessageID = req.MessageID, From = req.From, To = new List<string> { item }, Parts = req.Parts };
                        SMTPMessageModel smtpMessage = new SMTPMessageModel(message);
                        if (await SMTPClient.Send(smtpMessage))
                        {
                            message.Status = 0;

                        }
                        else
                        {
                            message.Status = -1;
                        }


                        MMSMessageEventModel notification = new MMSMessageEventModel() { MessageType = req.MessageType, MediaType=req.MediaType, TransactionID = req.TransactionId, MessageID = req.MessageID, From = req.From, To = item, Status = message.Status, DomainRcpt = "mail", DomainSender = req.Sender };
                        int rowsAffected = await DBApi.InsertMessageEvent.Execute(notification);

                        log.Info(notification.ToString());
                        cdr.Info(notification.ToString());

                    }
                    else
                    {
                        
                       
                        IRResponseModel resp = await API.GetSubscriptionOperator.ExecuteAsync(item);
                        if(resp.Status == 0)
                        {
                            string operatorName; 
                            if(resp.Info.TryGetValue("OPERATOR_NAME",out operatorName))
                            {
                                Operator op = AppSettings.OPERATORS.GetOperatorInfo(operatorName);
                                if(op==null)
                                {
                                   // log.Warn($"The Operator {operatorName} Not suport mms (Not exist in AppSetting.xml).");
                                    MMSMessageEventModel messageEvent = new MMSMessageEventModel() { MessageType = "m-error-info", TransactionID = req.TransactionId, MessageID = req.MessageID, From = req.From, To = item, Status = 1, DomainRcpt = "", DomainSender = req.Sender, MediaType = req.MediaType,Info= operatorName + " - Not suport mms" };
                                    int rowsAffected = await DBApi.InsertMessageEvent.Execute(messageEvent);
                                    log.Warn(messageEvent);
                                    return;
                                }
                                if (op.IsInternal)
                                {
                                    
                                    //Sent mms notification

                                    PPGRequestModel ppgReq = PPGRequestModel.Create(req.MessageID,req.From, item, req.Expiry, req.MessageSize);
                                    MMSMessageEventModel notification = new MMSMessageEventModel() {TransactionID=ppgReq.TransactionID, MessageType = "m-notification-ind", From = req.From, To = item, PushID = ppgReq.PushID, MessageID = req.MessageID,DomainSender=req.Sender,DomainRcpt=op.Domain,MediaType=req.MediaType };
                                    int rowsAffected = await DBApi.InsertMessageEvent.Execute(notification);
                                    if (rowsAffected == 1)
                                    {
                                        if (await DBApi.IsBlocked.Execute(Decoder.DeviceAddress(item).Replace("/TYPE=PLMN", ""), 2))
                                        {
                                            notification.Status = 2001;
                                            await DBApi.UpdateNotification.Execute(notification);
                                            //log.Debug($"The Sub {item} is blocked");
                                           
                                        }
                                        else
                                        {
                                            notification.Status = await PPG.PostNotificationAsync(ppgReq);
                                            await DBApi.UpdateNotification.Execute(notification);
                                        }

                                        if(req.AckRequest == 1)
                                        {
                                            SMTPResModel res = new SMTPResModel() { MessageID = req.MessageID,OriginatorSystem=req.OriginatorSystem,TransactionId=req.TransactionId};
                                            if(await SMTPClient.Send(res))
                                            {

                                            }
                                        }



                                    }
                                    else
                                    {
                                        log.Error("Fail InsertNotification ");
                                    }
                                    log.Info(notification.ToString());
                                    cdr.Info(notification.ToString());

                                }
                                else
                                {
                                    string trid = Tools.UniqueID;
                                    MMSMessageModel message = new MMSMessageModel() { MessageType = "MM4_forward.REQ", TransactionId = trid, MessageID = req.MessageID, From = req.From, To = new List<string> { Decoder.DeviceAddress(item) }, Parts = req.Parts };
                                    SMTPMessageModel smtpMessage = new SMTPMessageModel(message, op.Domain);
                                    if(await SMTPClient.Send(smtpMessage))
                                    {
                                        message.Status = 0;
                                       
                                    }
                                    else
                                    {
                                        message.Status = -1;
                                    }


                                    MMSMessageEventModel notification = new MMSMessageEventModel() { MessageType = "MM4_forward.REQ", TransactionID = trid, MessageID = req.MessageID, From = req.From, To = item, Status = message.Status, DomainRcpt = op.Domain,DomainSender=req.Sender ,MediaType=req.MediaType};
                                    int rowsAffected = await DBApi.InsertMessageEvent.Execute(notification);

                                    log.Info(notification.ToString());
                                    cdr.Info(notification.ToString());
                                }
                            }
                            else
                            {
                                //log.Warn($"The operator {}")
                            }

                        }
                        else
                        {

                            MMSMessageEventModel messageEvent = new MMSMessageEventModel() { MessageType = "m-error-info", TransactionID = req.TransactionId, MessageID = req.MessageID, From = req.From, To = item, Status = 2, DomainRcpt = "", DomainSender = req.Sender, MediaType = req.MediaType ,Info= resp.Description};
                            int rowsAffected = await DBApi.InsertMessageEvent.Execute(messageEvent);
                            log.Warn(messageEvent);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        };
    }
}