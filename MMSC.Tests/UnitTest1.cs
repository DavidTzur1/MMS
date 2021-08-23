using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSC.Decoders;
using MMSC.Models;
using System;

namespace MMSC.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
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
    }
}
