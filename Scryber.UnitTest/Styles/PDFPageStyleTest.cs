﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFPageStyleTest and is intended
    ///to contain all PDFPageStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFPageStyleTest
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
        ///A test for PDFPageStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_ConstructorTest()
        {
            PageStyle target = new PageStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(StyleKeys.PageItemKey,target.ItemKey);
        }

        /// <summary>
        ///A test for CreatePageSize
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_CreatePageSizeTest()
        {
            //default

            PaperSize size = PDFStyleConst.DefaultPaperSize;
            PaperOrientation orientation = PDFStyleConst.DefaultPaperOrientation;

            PageStyle target = new PageStyle(); //Empty
            PageSize expected = new PageSize(size, orientation);
            
            PageSize actual = target.CreatePageSize();
            AssertPageSizeAreEqual(expected, actual);

            //papers

            size = PaperSize.Tabloid;
            orientation = PaperOrientation.Landscape;
            expected = new PageSize(size, orientation);
            target.PaperSize = size;
            target.PaperOrientation = orientation;
            
            actual = target.CreatePageSize();
            AssertPageSizeAreEqual(expected, actual);

            size = PaperSize.A4;
            orientation = PaperOrientation.Portrait;
            target.PaperSize = size;
            target.PaperOrientation = orientation;
            expected = new PageSize(size, orientation);
            actual = target.CreatePageSize();

            AssertPageSizeAreEqual(expected, actual);


            // explicit sizes

            target = new PageStyle();
            Unit w = 200;
            Unit h = 500;
            target.Width = w;
            target.Height = h;

            expected = new PageSize(new Size(w, h));
            actual = target.CreatePageSize();
            AssertPageSizeAreEqual(expected, actual);


            // size overides paper

            target = new PageStyle();
            w = 300;
            h = 600;
            size = PaperSize.A8;
            orientation = PaperOrientation.Landscape;

            target.Width = w;
            target.Height = h;
            target.PaperSize = size;
            target.PaperOrientation = orientation;


            expected = new PageSize(new Size(w, h));
            actual = target.CreatePageSize();

            AssertPageSizeAreEqual(expected, actual);

        }

        private static void AssertPageSizeAreEqual(PageSize expected, PageSize actual)
        {
            Assert.AreEqual(expected.Width, actual.Width);
            Assert.AreEqual(expected.Height, actual.Height);
            Assert.AreEqual(expected.Size, actual.Size);
            Assert.AreEqual(expected.PaperSize, actual.PaperSize);
            Assert.AreEqual(expected.Orientation, actual.Orientation);
        }

        

        /// <summary>
        ///A test for Height
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_HeightTest()
        {
            PageStyle target = new PageStyle();
            Unit expected = Unit.Zero;

            Assert.AreEqual(expected, target.Height);

            expected = 20;
            target.Height = expected;
            Assert.AreEqual(expected, target.Height);

            expected = 40;
            target.Height = expected;
            Assert.AreEqual(expected, target.Height);

            expected = Unit.Zero;
            target.RemoveHeight();
            Assert.AreEqual(expected, target.Height);

            target.Height = expected;
            Assert.AreEqual(expected, target.Height);
            
        }


        /// <summary>
        ///A test for NumberFormat
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_NumberFormatTest()
        {
            PageStyle target = new PageStyle();
            string expected = String.Empty;

            Assert.AreEqual(expected, target.PageNumberFormat);

            expected = "Page {0}";
            target.PageNumberFormat = expected;
            Assert.AreEqual(expected, target.PageNumberFormat);

            expected = "Another Page {0}";
            target.PageNumberFormat = expected;
            Assert.AreEqual(expected, target.PageNumberFormat);

            expected = String.Empty;
            target.RemovePageNumberFormat();
            Assert.AreEqual(expected, target.PageNumberFormat);

            target.PageNumberFormat = expected;
            Assert.AreEqual(expected, target.PageNumberFormat);
        }


        /// <summary>
        ///A test for NumberStartIndex
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_NumberStartIndexTest()
        {
            PageStyle target = new PageStyle();
            int expected = 1;

            Assert.AreEqual(expected, target.NumberStartIndex);

            expected = 15;
            target.NumberStartIndex = expected;
            Assert.AreEqual(expected, target.NumberStartIndex);

            expected = 20;
            target.NumberStartIndex = expected;
            Assert.AreEqual(expected, target.NumberStartIndex);

            expected = 1;
            target.RemoveNumberStartIndex();
            Assert.AreEqual(expected, target.NumberStartIndex);

            target.NumberStartIndex = expected;
            Assert.AreEqual(expected, target.NumberStartIndex);
        }

        /// <summary>
        ///A test for NumberStyle
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_NumberStyleTest()
        {
            PageStyle target = new PageStyle();
            PageNumberStyle expected = PageNumberStyle.Decimals;

            Assert.AreEqual(expected, target.NumberStyle);

            expected = PageNumberStyle.LowercaseRoman;
            target.NumberStyle = expected;
            Assert.AreEqual(expected, target.NumberStyle);

            expected = PageNumberStyle.UppercaseLetters;
            target.NumberStyle = expected;
            Assert.AreEqual(expected, target.NumberStyle);

            expected = PageNumberStyle.Decimals;
            target.RemoveNumberStyle();
            Assert.AreEqual(expected, target.NumberStyle);

            target.NumberStyle = expected;
            Assert.AreEqual(expected, target.NumberStyle);
        }

        /// <summary>
        ///A test for PaperOrientation
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_PaperOrientationTest()
        {
            PageStyle target = new PageStyle();
            PaperOrientation expected = PaperOrientation.Portrait;

            Assert.AreEqual(expected, target.PaperOrientation);

            expected = PaperOrientation.Landscape;
            target.PaperOrientation = expected;
            Assert.AreEqual(expected, target.PaperOrientation);

            expected = PaperOrientation.Portrait;
            target.PaperOrientation = expected;
            Assert.AreEqual(expected, target.PaperOrientation);

            expected = PaperOrientation.Portrait;
            target.RemovePaperOrientation();
            Assert.AreEqual(expected, target.PaperOrientation);

           
        }

        /// <summary>
        ///A test for PaperSize
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_PaperSizeTest()
        {
            PageStyle target = new PageStyle();
            PaperSize expected = PaperSize.A4;

            Assert.AreEqual(expected, target.PaperSize);

            expected = PaperSize.C0;
            target.PaperSize = expected;
            Assert.AreEqual(expected, target.PaperSize);

            expected = PaperSize.Tabloid;
            target.PaperSize = expected;
            Assert.AreEqual(expected, target.PaperSize);

            expected = PaperSize.A4;
            target.RemovePaperSize();
            Assert.AreEqual(expected, target.PaperSize);
        }

        /// <summary>
        ///A test for Width
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_WidthTest()
        {
            PageStyle target = new PageStyle();
            Unit expected = Unit.Zero;

            Assert.AreEqual(expected, target.Width);

            expected = 20;
            target.Width = expected;
            Assert.AreEqual(expected, target.Width);

            expected = 40;
            target.Width = expected;
            Assert.AreEqual(expected, target.Width);

            expected = Unit.Zero;
            target.RemoveWidth();
            Assert.AreEqual(expected, target.Width);

            target.Width = expected;
            Assert.AreEqual(expected, target.Width);
        }

        
    }
}
