using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFOutlineStyleTest and is intended
    ///to contain all PDFOutlineStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFOutlineStyleTest
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
        ///A test for PDFOutlineStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Outline_ConstructorTest()
        {
            OutlineStyle target = new OutlineStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(StyleKeys.OutlineItemKey,target.ItemKey);
        }

        /// <summary>
        ///A test for Color
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Outline_ColorTest()
        {
            OutlineStyle target = new OutlineStyle();
            PDFColor expected = PDFColors.Transparent;
            Assert.AreEqual(expected, target.Color);

            expected = PDFColors.Olive;
            target.Color = expected;
            Assert.AreEqual(expected, target.Color);

            expected = PDFColors.Gray;
            target.Color = expected;
            Assert.AreEqual(expected, target.Color);

            target.RemoveColor();
            expected = PDFColors.Transparent;
            Assert.AreEqual(expected, target.Color);

        }

        /// <summary>
        ///A test for FontBold
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Outline_FontBoldTest()
        {
            OutlineStyle target = new OutlineStyle();
            bool expected = false;
            Assert.AreEqual(expected, target.FontBold);

            expected = true;
            target.FontBold = expected;
            Assert.AreEqual(expected, target.FontBold);

            expected = false;
            target.FontBold = expected;
            Assert.AreEqual(expected, target.FontBold);

            target.RemoveFontBold();
            expected = false;
            Assert.AreEqual(expected, target.FontBold);
        }

        /// <summary>
        ///A test for FontItalic
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Outline_FontItalicTest()
        {
            OutlineStyle target = new OutlineStyle();
            bool expected = false;
            Assert.AreEqual(expected, target.FontItalic);

            expected = true;
            target.FontItalic = expected;
            Assert.AreEqual(expected, target.FontItalic);

            expected = false;
            target.FontItalic = expected;
            Assert.AreEqual(expected, target.FontItalic);

            target.RemoveFontItalic();
            expected = false;
            Assert.AreEqual(expected, target.FontItalic);
        }

        /// <summary>
        ///A test for Open
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Outline_OpenTest()
        {
            OutlineStyle target = new OutlineStyle();
            bool expected = false;
            Assert.AreEqual(expected, target.Open);

            expected = true;
            target.Open = expected;
            Assert.AreEqual(expected, target.Open);

            expected = false;
            target.Open = expected;
            Assert.AreEqual(expected, target.Open);

            target.RemoveOpen();
            expected = false;
            Assert.AreEqual(expected, target.Open);
        }

        /// <summary>
        ///A test for IsOutlined
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Outline_IsOutlinedTest()
        {
            OutlineStyle target = new OutlineStyle();
            bool expected = true;
            Assert.AreEqual(expected, target.IsOutlined);

            expected = false;
            target.IsOutlined = expected;
            Assert.AreEqual(expected, target.IsOutlined);

            expected = true;
            target.IsOutlined = expected;
            Assert.AreEqual(expected, target.IsOutlined);

            target.RemoveOutline();
            expected = true;
            Assert.AreEqual(expected, target.IsOutlined);
        }

        
    }
}
