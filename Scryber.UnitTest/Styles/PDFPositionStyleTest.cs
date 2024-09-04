﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            PositionStyle target = new PositionStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(StyleKeys.PositionItemKey, target.ItemKey);
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
            SizeStyle target = new SizeStyle();
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
            PositionStyle target = new PositionStyle();

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
            PositionStyle target = new PositionStyle();

            //Default 

            PositionMode expected = PositionMode.Static;
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

            expected = PositionMode.Static;
            target.RemovePositionMode();
            actual = target.PositionMode;
            Assert.AreEqual(expected, actual);

            // Check the X or Y setting to relative

            expected = PositionMode.Relative;
            target = new PositionStyle();
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
        
        #region public void Position_PositionModeTest()

        /// <summary>
        ///A test for PositionMode
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Position_DisplayModeTest()
        {
            PositionStyle target = new PositionStyle();

            //Default 

            DisplayMode expected = DisplayMode.Block;
            Assert.AreEqual(expected, target.DisplayMode);

            //Set value

            expected = DisplayMode.InlineBlock;
            DisplayMode actual;
            target.DisplayMode = expected;
            actual = target.DisplayMode;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = DisplayMode.Invisible;
            target.DisplayMode = expected;
            actual = target.DisplayMode;
            Assert.AreEqual(expected, actual);

            //Remove value

            expected = DisplayMode.Block;
            target.RemoveDisplayMode();
            actual = target.DisplayMode;
            Assert.AreEqual(expected, actual);

            // Check the X or Y setting to relative

            
            
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
            PositionStyle target = new PositionStyle();

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
            PositionStyle target = new PositionStyle();

            // Default value

            Unit expected = Unit.Empty;
            Unit actual = target.X;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20;
            target.X = expected;
            actual = target.X;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = new Unit(120, PageUnits.Millimeters);
            target.X = expected;
            actual = target.X;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = Unit.Empty;
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
            PositionStyle target = new PositionStyle();

            // Default value

            Unit expected = Unit.Empty;
            Unit actual = target.Y;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20;
            target.Y = expected;
            actual = target.Y;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = new Unit(120, PageUnits.Millimeters);
            target.Y = expected;
            actual = target.Y;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = Unit.Empty;
            target.RemoveY();
            actual = target.Y;
            Assert.AreEqual(expected, actual);
        }


        #endregion


        #region public void Position_XTest()

        /// <summary>
        ///A test for X
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Position_RightTest()
        {
            PositionStyle target = new PositionStyle();

            // Default value

            Unit expected = Unit.Empty;
            Unit actual = target.Right;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20;
            target.Right = expected;
            actual = target.Right;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = new Unit(120, PageUnits.Millimeters);
            target.Right = expected;
            actual = target.Right;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = Unit.Empty;
            target.RemoveRight();
            actual = target.Right;
            Assert.AreEqual(expected, actual);
        }


        #endregion

        #region public void Position_YTest()

        /// <summary>
        ///A test for Y
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Position_BottomTest()
        {
            PositionStyle target = new PositionStyle();

            // Default value

            Unit expected = Unit.Empty;
            Unit actual = target.Bottom;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20;
            target.Bottom = expected;
            actual = target.Bottom;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = new Unit(120, PageUnits.Millimeters);
            target.Bottom = expected;
            actual = target.Bottom;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = Unit.Empty;
            target.RemoveBottom();
            actual = target.Bottom;
            Assert.AreEqual(expected, actual);
        }


        #endregion

    }
}
