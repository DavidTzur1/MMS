using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSC.Actions;
using MMSC.API;
using MMSC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MMSC.Tests
{
   
    /// <summary>
    /// Summary description for ManagerActionTest
    /// </summary>
    [TestClass]
    public class ManagerActionTest
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ManagerActionTest()
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
        public async Task MM1Test()
        {
            MMSMessageModel message = new MMSMessageModel() { From = "0545246247", To = new List<string>() { "0544604613" }, Expiry = 3600, MessageSize = 30000 ,MessageID=Tools.UniqueID};
            if(!ManagerAction.ActionBlock.Post(message)) log.Debug("ERROR");
            await Task.Delay(1000);
            
        }
    }
}
