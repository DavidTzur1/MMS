using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSC.API;
using MMSC.Decoders;
using MMSC.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MMSC.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var s = "3C736D696C3E3C686561643E3C6C61796F75743E3C726F6F742D6C61796F75742F3E3C726567696F6E2069643D22546578742220746F703D223022206C6566743D223022206865696768743D2231303025222077696474683D2231303025222F3E3C2F6C61796F75743E3C2F686561643E3C626F64793E3C706172206475723D22353030306D73223E3C74657874207372633D22746578742E3030303030302E7478742220726567696F6E3D225465787422202F3E3C2F7061723E3C2F626F64793E3C2F736D696C3E";

            var x = Tools.GetBytes(s);

            string a = Encoding.UTF7.GetString(x);

            var z = Encoding.UTF7.GetBytes(a);

            byte[] pdu = TestHelper.FileToByteArray(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\Trace\T178cad0e416.txt");

            /*------------X-Mms-Expiry----------------*
            880680 044019FE91 is decoded as: 
            0x88: X - Mms - Expiry
            0x06: # of data bytes (80044019FE91) 
            0x80: Absolute - token
            0x04: # of data bytes (4019FE91) 
            0x4019FE91: Absolute date(in delta seconds)
            -----------------------------------------*/
            //byte[] pdu = new byte[] { 0x88, 0x06, 0x80, 0x04, 0x40, 0x19, 0xFE, 0x91 };


            MM1Decoder decoder = new MM1Decoder(pdu);
            MMSMessageModel message = decoder.Parse();
        }

        private TestContext testContextInstance;
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void TestMethod2()
        {
            var s1 = File.ReadAllText(@"C:\Users\dtzur\source\repos\MMS\MMSC.Tests\StringValue.txt");
            TestContext.WriteLine(s1);
            string contentType = String.Concat(s1.Where(c => !Char.IsWhiteSpace(c)));
            TestContext.WriteLine(contentType);
            // return contentType;



        }
    }
}
