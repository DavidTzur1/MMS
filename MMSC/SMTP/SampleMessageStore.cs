using MMSC.Actions;
using MMSC.Decoders;
using MMSC.Models;
using MMSC.Tables;
using SmtpServer;
using SmtpServer.Protocol;
using SmtpServer.Storage;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MMSC.SMTP
{
    

    public class SampleMessageStore : MessageStore
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override async Task<SmtpResponse> SaveAsync(ISessionContext context, IMessageTransaction transaction, ReadOnlySequence<byte> buffer, CancellationToken cancellationToken)
        {
            try
            {
                var stream = new MemoryStream();

                var position = buffer.GetPosition(0);
                while (buffer.TryGet(ref position, out var memory))
                {

                    await stream.WriteAsync(memory.ToArray(), 0, memory.Length, cancellationToken);
                }

                stream.Position = 0;

                foreach (var item in transaction.Parameters)
                {
                    log.Debug($"key = {item.Key} Value= {item.Value}");
                }

                
                var message = await MimeKit.MimeMessage.LoadAsync(stream, cancellationToken);

               
                var mms = MM4Decoder.Parse(message);
                if (mms.MessageType == "MM4_forward.RES")
                {
                    MMSMessageEventModel notifyresp = await DBApi.GetMessageEventInfo.Execute(mms.TransactionId);
                    notifyresp.TransactionID = mms.TransactionId;
                    notifyresp.MessageType = mms.MessageType;
                    notifyresp.Status = mms.ResponseStatus;
                    notifyresp.Info = mms.ResponseText;

                    if (notifyresp.MessageID == string.Empty) notifyresp.Info = "TransactionId not found";
                    await DBApi.InsertMessageEvent.Execute(notifyresp);
                    log.Info(notifyresp);

                    return SmtpResponse.Ok;
                }

                else if (mms.MessageType == "MM4_forward.REQ")
                {
                    int rowsAffected = await DBApi.InsertMessage.Execute(mms);
                    if (rowsAffected == 0)
                    {
                        mms.ResponseStatus = ResponseStatuses.ErrorNetworkProblem;
                        log.Error(mms.ToString());
                        return SmtpResponse.TransactionFailed;
                    }
                    else
                    {
                        ManagerAction.ActionBlock.Post(mms);
                        mms.ResponseStatus = ResponseStatuses.Ok;
                        log.Info(mms.ToString());
                        return SmtpResponse.Ok;
                    }
                }
                else
                {
                    log.Warn(mms.ToString());
                    return SmtpResponse.SyntaxError;
                }

               
            }
            catch(Exception ex)
            {
                log.Error(ex);
                return SmtpResponse.SyntaxError;
            }
        }
    }
}