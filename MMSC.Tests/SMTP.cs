using MailKit.Net.Smtp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit;
using MMSC.Decoders;
using MMSC.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
        public void SendSMTP()
        {
            var aaa = Convert.FromBase64String("P/VR6D0noPwP/9k=");
            var message = MimeMessage.Load(@"C:\Workspace\MMS\MMSClientTest\Trace\MM4.txt");
            //var message = MimeMessage.Load(@"C:\Workspace\MMS\MMSClientTest\Trace\MM4_Cellcom.txt");
            //var message = MimeMessage.Load(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\mm4.txt");
            MMSMessageModel res = MM4Decoder.Parse(message);

            //C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\mmsfromcelcom.txt
            //var message = MimeMessage.Load(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\mmsfromcelcom.txt");
            // MMSMessageModel  res = MM4Decoder.Parse(message);
          

            using (var smtpClient = new SmtpClient())
            {
                //smtpClient.Connect("smtpvs.partnergsm.co.il", 25, MailKit.Security.SecureSocketOptions.None);
                //smtpClient.Connect("10.11.32.43", 25, MailKit.Security.SecureSocketOptions.None);
                smtpClient.Connect("172.17.120.131", 25, MailKit.Security.SecureSocketOptions.None);
                smtpClient.Send(message, new MailboxAddress("", "email@email.here"), new[] { new MailboxAddress("", "david.tzur@partner.co.il") });
                smtpClient.Disconnect(true);
            }

           
        }
    }
}
