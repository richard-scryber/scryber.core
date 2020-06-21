using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFPositionStyleTest and is intended
    ///to contain all PDFPositionStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFPositionStyleTest
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

        // individual property tests

        #region public void Position_ConstructorTest()

        /// <summary>
        ///A test for PDFPositionStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Position_ConstructorTest()
        {
            PDFPositionStyle target = new PDFPositionStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(PDFStyleKeys.PositionItemKey, target.ItemKey);
        }

        #endregion

        #region public void Position_FullWidthTest()

        /// <summary>
        ///A test for FullWidth
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Size_FullWidthTest()
        {
            PDFSizeStyle target = new PDFSizeStyle();
            bool expected = false;
            Assert.AreEqual(expected, target.FullWidth);

            expected = true;
            target.FullWidth = expected;
            bool actual = target.FullWidth;
            Assert.AreEqual(expected, actual);

            expected = false;
            target.FullWidth = expected;
            actual = target.FullWidth;
            Assert.AreEqual(expected, actual);

            expected = false;
            target.RemoveFillWidth();
            Assert.AreEqual(expected, actual);
        }


        #endregion

        #region public void Position_HAlignTest()

        /// <summary>
        ///A test for HAlign
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Position_HAlignTest()
        {
            PDFPositionStyle target = new PDFPositionStyle();

            //Default 

            HorizontalAlignment expected = HorizontalAlignment.Left;
            Assert.AreEqual(expected, target.HAlign);

            //Set value

            expected = HorizontalAlignment.Right;
            HorizontalAlignment actual;
            target.HAlign = expected;
            actual = target.HAlign;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = HorizontalAlignment.Center;
            target.HAlign = expected;
            actual = target.HAlign;
            Assert.AreEqual(expected, actual);

            //Remove value

            expected = HorizontalAlignment.Left;
            target.RemoveHAlign();
            actual = target.HAlign;
            Assert.AreEqual(expected, actual);
        }


        #endregion

        #region public void Position_PositionModeTest()

        /// <summary>
        ///A test for PositionMode
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Position_PositionModeTest()
        {
            PDFPositionStyle target = new PDFPositionStyle();

            //Default 

            PositionMode expected = PositionMode.Block;
            Assert.AreEqual(expected, target.PositionMode);

            //Set value

            expected = PositionMode.Absolute;
            PositionMode actual;
            target.PositionMode = expected;
            actual = target.PositionMode;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = PositionMode.Relative;
            target.PositionMode = expected;
            actual = target.PositionMode;
            Assert.AreEqual(expected, actual);

            //Remove value

            expected = PositionMode.Block;
            target.RemovePositionMode();
            actual = target.PositionMode;
            Assert.AreEqual(expected, actual);

            // Check the X or Y setting to relative

            expected = PositionMode.Relative;
            target = new PDFPositionStyle();
            target.X = 20;
            actual = target.PositionMode;
            Assert.AreEqual(expected, actual);

            target.RemoveX();
            target.Y = 40;
            actual = target.PositionMode;
            Assert.AreEqual(expected, actual);

            //set both
            target.X = 50;
            actual = target.PositionMode;
            Assert.AreEqual(expected, actual);

            //explicit override even if X or Y is set
            expected = PositionMode.Absolute;
            target.PositionMode = expected;
            actual = target.PositionMode;
            Assert.AreEqual(expected, actual);
            
        }


        #endregion

        #region public void Position_VAlignTest()

        /// <summary>
        ///A test for VAlign
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Position_VAlignTest()
        {
            PDFPositionStyle target = new PDFPositionStyle();

            //Default 

            VerticalAlignment expected = VerticalAlignment.Top;
            Assert.AreEqual(expected, target.VAlign);

            //Set value

            expected = VerticalAlignment.Bottom;
            VerticalAlignment actual;
            target.VAlign = expected;
            actual = target.VAlign;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = VerticalAlignment.Middle;
            target.VAlign = expected;
            actual = target.VAlign;
            Assert.AreEqual(expected, actual);

            //Remove value

            expected = VerticalAlignment.Top;
            target.RemoveVAlign();
            actual = target.VAlign;
            Assert.AreEqual(expected, actual);
        }


        #endregion

        #region public void Position_XTest()

        /// <summary>
        ///A test for X
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Position_XTest()
        {
            PDFPositionStyle target = new PDFPositionStyle();

            // Default value

            PDFUnit expected = PDFUnit.Empty;
            PDFUnit actual = target.X;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20;
            target.X = expected;
            actual = target.X;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = new PDFUnit(120, PageUnits.Millimeters);
            target.X = expected;
            actual = target.X;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = PDFUnit.Empty;
            target.RemoveX();
            actual = target.X;
            Assert.AreEqual(expected, actual);
        }


        #endregion

        #region public void Position_YTest()

        /// <summary>
        ///A test for Y
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Position_YTest()
        {
            PDFPositionStyle target = new PDFPositionStyle();

            // Default value

            PDFUnit expected = PDFUnit.Empty;
            PDFUnit actual = target.Y;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20;
            target.Y = expected;
            actual = target.Y;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = new PDFUnit(120, PageUnits.Millimeters);
            target.Y = expected;
            actual = target.Y;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = PDFUnit.Empty;
            target.RemoveY();
            actual = target.Y;
            Assert.AreEqual(expected, actual);
        }


        #endregion

    }
}
