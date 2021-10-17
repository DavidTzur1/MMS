﻿using MailKit.Net.Smtp;
using MMSC.API;
using MMSC.Encoders;
using MMSC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
                        MMSMessageModel message = new MMSMessageModel() { MessageType = "MM4_forward.REQ", TransactionId = req.TransactionId, MessageID = req.MessageID, From = req.From, To = new List<string> { item }, Parts = req.Parts };
                        SMTPMessageModel smtpMessage = new SMTPMessageModel(message);
                        if (await SMTPClient.Send(smtpMessage))
                        {
                            message.Status = 0;

                        }
                        else
                        {
                            message.Status = -1;
                        }


                        MMSMessageEventModel notification = new MMSMessageEventModel() { MessageType = "MM4_forward.REQ", TransactionID = req.TransactionId, MessageID = req.MessageID, From = req.From, To = item, Status = message.Status, DomainRcpt = "mail", DomainSender = req.Sender };
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
                                    log.Warn($"The Operator {operatorName} Not suport mms (Not exist in AppSetting.xml).");
                                    return;
                                }
                                if (op.IsInternal)
                                {
                                    
                                    //Sent mms notification

                                    PPGRequestModel ppgReq = PPGRequestModel.Create(req.MessageID,req.From, item, req.Expiry, req.MessageSize);
                                    MMSMessageEventModel notification = new MMSMessageEventModel() {TransactionID=ppgReq.TransactionID, MessageType = "m-notification-ind", From = req.From, To = item, PushID = ppgReq.PushID, MessageID = req.MessageID,DomainSender=req.Sender,DomainRcpt=op.Domain };
                                    int rowsAffected = await DBApi.InsertMessageEvent.Execute(notification);
                                    if (rowsAffected == 1)
                                    {
                                        notification.Status = await PPG.PostNotificationAsync(ppgReq);
                                        await DBApi.UpdateNotification.Execute(notification);

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
                                    //SMTP message
                                    MMSMessageModel message = new MMSMessageModel() { MessageType = "MM4_forward.REQ", TransactionId = req.TransactionId, MessageID = req.MessageID,From=req.From,To= new List<string> { item },Parts=req.Parts };
                                    SMTPMessageModel smtpMessage = new SMTPMessageModel(message, op.Domain);
                                    if(await SMTPClient.Send(smtpMessage))
                                    {
                                        message.Status = 0;
                                       
                                    }
                                    else
                                    {
                                        message.Status = -1;
                                    }


                                    MMSMessageEventModel notification = new MMSMessageEventModel() { MessageType = "MM4_forward.REQ", TransactionID = req.TransactionId, MessageID = req.MessageID, From = req.From, To = item, Status = message.Status, DomainRcpt = op.Domain,DomainSender=req.Sender };
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
                            log.Warn(req.ToString(item));

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