using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFSolidPen_Test and is intended
    ///to contain all PDFSolidPen_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFSolidPen_Test
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
        ///A test for PDFSolidPen Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFSolidPenConstructor_Test()
        {
            PDFColor color = PDFColors.Aqua; // TODO: Initialize to an appropriate value
            PDFUnit width = 1;
            PDFSolidPen target = new PDFSolidPen(color, width);

            Assert.IsNotNull(target);
            Assert.AreEqual(color, target.Color);
            Assert.AreEqual(width, target.Width);
            
        }

        /// <summary>
        ///A test for PDFSolidPen Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFSolidPenConstructor_Test1()
        {
            PDFSolidPen target = new PDFSolidPen();

            Assert.IsNotNull(target);
            Assert.IsNull(target.Color);
            Assert.AreEqual(target.Width, PDFUnit.Zero);
        }

        

        /// <summary>
        ///A test for Color
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void Color_Test()
        {
            PDFSolidPen target = new PDFSolidPen(); // TODO: Initialize to an appropriate value
            PDFColor expected = PDFColors.Aqua;
            PDFColor actual;
            target.Color = expected;
            actual = target.Color;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for LineStyle
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void LineStyle_Test()
        {
            PDFSolidPen target = new PDFSolidPen(); // TODO: Initialize to an appropriate value
            LineType expected = LineType.Solid;
            LineType actual;
            actual = target.LineStyle;

            Assert.AreEqual(expected, actual);
        }
    }
}
