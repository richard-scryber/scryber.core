using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFDashPen_Test and is intended
    ///to contain all PDFDashPen_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFDashPen_Test
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


        PDFDash dash = new PDFDash(new int[] { 1, 2, 0 }, 4);

        /// <summary>
        ///A test for PDFDashPen Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFDashPenConstructor_Test()
        {
            
            PDFDashPen target = new PDFDashPen(dash);
            Assert.IsNotNull(target);
        }

        

        /// <summary>
        ///A test for Dash
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void Dash_Test()
        {
            PDFDashPen target = new PDFDashPen(dash);
            PDFDash expected = dash;
            PDFDash actual;
            actual = target.Dash;
            Assert.AreEqual(actual, expected);

            expected = new PDFDash(new int[] { 1, 2, 5 }, 4);
            target.Dash = expected;

            Assert.AreEqual(expected, target.Dash);
            
        }

        /// <summary>
        ///A test for LineStyle
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void LineStyle_Test()
        {
            PDFDashPen target = new PDFDashPen(dash);
            LineType actual;
            actual = target.LineStyle;
            Assert.AreEqual(actual, LineType.Dash);
        }
    }
}
