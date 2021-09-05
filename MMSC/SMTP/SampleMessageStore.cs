using MMSC.Actions;
using MMSC.Decoders;
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

                var message = await MimeKit.MimeMessage.LoadAsync(stream, cancellationToken);
                var mms = MM4Decoder.Parse(message);
                string messageID = DBApi.InsertMessage.Execute(mms);
                mms.MessageID = messageID;
                ManagerAction.ActionBlock.Post(mms);
               // log.Debug(message.From);


                return SmtpResponse.Ok;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                return SmtpResponse.SyntaxError;
            }
        }
    }
}