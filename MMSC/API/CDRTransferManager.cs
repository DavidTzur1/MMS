using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MMSC.API
{
    public class CDRTransferManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static System.Timers.Timer timeOutTimer;
        static int transferTimer = AppSettings.CDRTransfer.TransferTimer;
        static string sourceDirectory = AppSettings.CDRTransfer.SourceDirectory;
        static string targetDirectory = AppSettings.CDRTransfer.TargetDirectory;
        static int cdrCounter = 1;

        static CDRTransferManager()
        {
            var timeOut = TimeSpan.FromSeconds(transferTimer);
            timeOutTimer = new System.Timers.Timer(timeOut.TotalMilliseconds);
            timeOutTimer.Elapsed += (s, e) => Run();

            timeOutTimer.Start();
            log.Debug("Run CDRTransferManager");

        }

        public async static void Run()
        {
            timeOutTimer.Stop();
            try
            {
               
                foreach (string filename in Directory.EnumerateFiles(sourceDirectory).Where(x=>!x.EndsWith(".cdr") ))
                {
                    //using (FileStream SourceStream = File.Open(filename, FileMode.Open))
                    //{
                    //    using (FileStream DestinationStream = File.Create(targetDirectory + filename.Substring(filename.LastIndexOf('\\'))))
                    //    {
                    //        await SourceStream.CopyToAsync(DestinationStream);
                    //    }
                    //}

                    using (FileStream SourceStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite))
                    {
                        if (cdrCounter >= 10000) cdrCounter = 1;
                        
                        using (FileStream DestinationStream = File.Create(targetDirectory + filename.Substring(filename.LastIndexOf('\\')) + "." + cdrCounter++))
                        {
                            
                            await SourceStream.CopyToAsync(DestinationStream);
                        }
                    }
                    File.Delete(filename);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            finally
            {
                timeOutTimer.Start();
            }

        }
    }
}