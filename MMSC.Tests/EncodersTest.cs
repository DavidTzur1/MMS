using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSC.Encoders;
using MMSC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMSC.Tests
{
    /// <summary>
    /// Summary description for EncodersTest
    /// </summary>
    [TestClass]
    public class EncodersTest
    {
        public EncodersTest()
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
        public void NotificationIndEncoder()
        {
            MMSNotificationModel message = new MMSNotificationModel();

            message.TransactionID = "mces3xzhg2wllka9l01@oklik.net";
            message.From = "+972542332013/TYPE=PLMN";
            message.MessageSize = 18514;
            message.Expiry = 259199;
           // message.ContentLocation = "http://172.16.16.10/servlets/mms?message-id=mces3xzhg2wllka9l01";

            NotificationIndEncoder encoder = new NotificationIndEncoder(message);
            var value = encoder.Encode();
        }

        [TestMethod]
        public void RetrieveConfEncoder()
        {
            MMSNotificationModel message = new MMSNotificationModel();

            message.TransactionID = "mces3xzhg2wllka9l01@oklik.net";
            message.From = "+972542332013/TYPE=PLMN";
            message.MessageSize = 18514;
            message.Expiry = 259199;
            message.ContentType = "application/vnd.wap.multipart.related; type=application/smil; start=0.smil";


            var strList = message.ContentType.Split(';').ToList();
            
            foreach(var item in strList.Skip(1))
            {
                var key = item.Split('=').FirstOrDefault();
                var value = item.Split('=').LastOrDefault();

                switch(key.ToLower().Trim())
                {
                    case "start":
                        var len = value.Length;
                        break;
                    case "type":
                        var len1 = value.Length;
                        break;
                }
            }
            // message.ContentLocation = "http://172.16.16.10/servlets/mms?message-id=mces3xzhg2wllka9l01";

            NotificationIndEncoder encoder = new NotificationIndEncoder(message);
            //var value = encoder.Encode();
        }
    }
}
