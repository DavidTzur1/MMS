using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSC.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSC.Tests
{
    /// <summary>
    /// Summary description for AppSettingsTest
    /// </summary>
    [TestClass]
    public class AppSettingsTest
    {
        public AppSettingsTest()
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
        public void GetBoundedCapacity()
        {
            var xml = AppSettings.XMLSettings;
            var value = AppSettings.MANAGER.BoundedCapacity;
        }

        [TestMethod]
        public void GetOperatorInfo()
        {
            
            var op = AppSettings.OPERATORS.GetOperatorInfo("PARTNER");
            var domain = op.Domain;
            var isInternal = op.IsInternal;
        }
        
    }
}
