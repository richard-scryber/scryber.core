using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFNoPen_Test and is intended
    ///to contain all PDFNoPen_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFNoPen_Test
    {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for PDFNoPen Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFNoPenConstructor_Test()
        {
            PDFNoPen target = new PDFNoPen();
            Assert.IsNotNull(target);
        }

        

        /// <summary>
        ///A test for LineStyle
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void LineStyle_Test()
        {
            PDFNoPen target = new PDFNoPen(); // TODO: Initialize to an appropriate value
            LineStyle actual;
            actual = target.LineStyle;
            Assert.AreEqual(LineStyle.None, actual);
        }
    }
}
