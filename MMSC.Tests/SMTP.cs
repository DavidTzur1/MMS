using MailKit.Net.Smtp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit;
using MMSC.API;
using MMSC.DBApi;
using MMSC.Decoders;
using MMSC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MMSC.Tests
{
    /// <summary>
    /// Summary description for SMTP
    /// </summary>
    [TestClass]
    public class SMTP
    {
        public SMTP()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public async Task DecodeMessageAsync()
        {
            var lenstr = MM4Decoder.IntToUIntString(36);
            var mess = await GetMessageByPushID.Execute("202201130023402350163c5030c");
            int len = mess.Data.Length / 2;
            string image = mess.Data.Substring(mess.Data.IndexOf("FFD8FFE000104A46494600010100000100010000FFE20"));
            byte[] arry = Tools.GetBytes(image);
            string str = System.Convert.ToBase64String(arry);
            File.WriteAllText(@"d:\Test11.txt", str);
            string UintStr = MM4Decoder.IntToUIntString(image.Length/2);
            string data = File.ReadAllText(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\MessageID\2022011300280392301.txt");
            
            byte[] pdu = Encoding.ASCII.GetBytes(data);
            MM1Decoder decoder = new MM1Decoder(pdu);
            decoder.Parse();
        }

        //http://172.17.120.131/servlets/retrieve?messageid=20220113002804018015abc7277

        [TestMethod]
        public void SendSMTP()
        {
            //סינון של ה WIRESHARK לבדוק את הבקשות לפתיחת חיבור
            //ip.dst == 172.17.120.9 && tcp.flags.ack == 0 && tcp.flags.syn == 1
            //..
            var aaa = Convert.FromBase64String("P/VR6D0noPwP/9k=");
            // var message = MimeMessage.Load(@"C:\Workspace\MMS\MMSClientTest\Trace\MM4.txt"); //mmscprovider.pelephone.net.il
            // var message = MimeMessage.Load(@"C:\Workspace\MMS\MMSClientTest\Trace\MM4_Cellcom.txt"); //mms.cellcom.co.il
            //var message = MimeMessage.Load(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\mm4.txt"); //mms.hotmobile.co.il
            //var message = MimeMessage.Load(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\mmsfromcelcom.txt");
            // var message = MimeMessage.Load(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\From0528449558.txt");
            //var message = MimeMessage.Load(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\From0532308826.txt");
            //var message = MimeMessage.Load(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\From0532308826Pic.txt");
            //var message = MimeMessage.Load(@"d:\Test10.txt");
            var message = MimeMessage.Load(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\MM4Req\From0528449558.txt");
            //var message = MimeMessage.Load(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\MM4Req\From0532308826.txt");
            //var message = MimeMessage.Load(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\MM4Req\Fromhotmobile.txt");

            //var message = MimeMessage.Load(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\MM4Res\PelephoneRES.txt");
            //var message = MimeMessage.Load(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\MM4Res\CellcomRES.txt");
            //var message = MimeMessage.Load(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\MM4Res\We4gRES.txt");//We4gRES
            //var message = MimeMessage.Load(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\MM4Res\GolantelecomRES.txt");//GolantelecomRES

            MMSMessageModel res = MM4Decoder.Parse(message);

            if (res.MessageType == "MM4_forward.REQ")
            {

            }
            else
            {
                string messType = res.MessageType;
            }


            //C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\mmsfromcelcom.txt
            //var message = MimeMessage.Load(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\mmsfromcelcom.txt");
            // MMSMessageModel  res = MM4Decoder.Parse(message);
            bool flage = true;
            while (flage)
            {
                for (int i = 1; i < 30; i++)
                {
                    using (var smtpClient = new SmtpClient())
                    {
                        //smtpClient.Connect("smtpvs.partnergsm.co.il", 25, MailKit.Security.SecureSocketOptions.None);
                        //smtpClient.Connect("10.11.32.43", 25, MailKit.Security.SecureSocketOptions.None);
                        smtpClient.Connect("172.17.120.131", 25, MailKit.Security.SecureSocketOptions.None);
                        //smtpClient.Send(message, new MailboxAddress("", "email@email.here"), new[] { new MailboxAddress("", "david.tzur@partner.co.il") });
                        //smtpClient.Send(message, new MailboxAddress("", "+972528449558/TYPE=PLMN@mms.cellcom.co.il"), new[] { new MailboxAddress("", "+972545246247/TYPE=PLMN@oklik.net"), new MailboxAddress("", "+972549992969/TYPE=PLMN@oklik.net"), new MailboxAddress("", "+972547789105/TYPE=PLMN@oklik.net"), new MailboxAddress("", "+972544500959/TYPE=PLMN@oklik.net") });
                        //smtpClient.Send(message, new MailboxAddress("", "+972528449558/TYPE=PLMN@mms.cellcom.co.il"), new[] { new MailboxAddress("", "+972545246247/TYPE=PLMN@oklik.net"), new MailboxAddress("", "+972549830432/TYPE=PLMN@oklik.net"), new MailboxAddress("", "+972549830440/TYPE=PLMN@oklik.net") });
                        smtpClient.Send(message, new MailboxAddress("", "+972528449558/TYPE=PLMN@mms.cellcom.co.il"), new[] { new MailboxAddress("", "+972549830432/TYPE=PLMN@oklik.net") });
                        smtpClient.Disconnect(true);
                    }
                    //Task.Delay(10000);
                }
                flage = false;
                //Task.Delay(TimeSpan.FromSeconds(30));
            }
        }

           
      
    }
}
