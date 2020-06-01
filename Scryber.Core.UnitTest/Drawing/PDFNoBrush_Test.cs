using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFNoBrush_Test and is intended
    ///to contain all PDFNoBrush_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFNoBrush_Test
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
        ///A test for PDFNoBrush Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFNoBrushConstructor_Test()
        {
            PDFNoBrush target = new PDFNoBrush();
            Assert.IsNotNull(target);
        }


        /// <summary>
        ///A test for FillStyle
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void FillStyle_Test()
        {
            PDFNoBrush target = new PDFNoBrush();
            FillStyle actual;
            actual = target.FillStyle;
            Assert.AreEqual(FillStyle.None, actual);

        }
    }
}
