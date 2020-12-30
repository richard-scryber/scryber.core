using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFFontStyleTest and is intended
    ///to contain all PDFFontStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFFontStyleTest
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
        ///A test for PDFFontStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Font_ConstructorTest()
        {
            Scryber.Styles.FontStyle target = new Scryber.Styles.FontStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(StyleKeys.FontItemKey, target.ItemKey);
        }

        /// <summary>
        ///A test for CreateFont
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Font_CreateFontTest()
        {
            Scryber.Styles.FontStyle target = new Scryber.Styles.FontStyle();
            PDFFont expected = new PDFFont(PDFStyleConst.DefaultFontFamily, PDFStyleConst.DefaultFontSize);
            PDFFont actual;
            actual = target.CreateFont();
            Assert.AreEqual(expected, actual,"Default not the same");

            target.FontFamily = (PDFFontSelector)"Symbol";
            expected = new PDFFont(StandardFont.Symbol, PDFStyleConst.DefaultFontSize);
            actual = target.CreateFont();
            Assert.AreEqual(expected, actual, "Symbol not the same");

            target.FontSize = 40;
            expected = new PDFFont(StandardFont.Symbol, 40);
            actual = target.CreateFont();
            Assert.AreEqual(expected, actual, "Symbol 40pt not the same");

            target.FontFamily = (PDFFontSelector)"Bauhaus 92";
            target.FontBold = true;
            target.FontItalic = true;
            target.FontSize = new PDFUnit(10, PageUnits.Millimeters);

            expected = new PDFFont("Bauhaus 92", new PDFUnit(10, PageUnits.Millimeters), Scryber.Drawing.FontStyle.Bold | Scryber.Drawing.FontStyle.Italic);
            actual = target.CreateFont();
            Assert.AreEqual(expected, actual, "Bauhaus not the same");

        }

        /// <summary>
        ///A test for FontBold
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Font_FontBoldTest()
        {
            Scryber.Styles.FontStyle target = new Scryber.Styles.FontStyle();
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
        ///A test for FontFamily
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Font_FontFamilyTest()
        {
            Scryber.Styles.FontStyle target = new Scryber.Styles.FontStyle();
            string expected = null;
            Assert.AreEqual(expected, target.FontFamily);

            expected = "Arial MT";
            target.FontFamily = (PDFFontSelector)expected;
            Assert.AreEqual((PDFFontSelector)expected, target.FontFamily);

            expected = "Helvetica";
            target.FontFamily = (PDFFontSelector)expected;
            Assert.AreEqual((PDFFontSelector)expected, target.FontFamily);


            target.RemoveFontFamily();
            expected = null;
            Assert.AreEqual(expected, target.FontFamily);
        }

        /// <summary>
        ///A test for FontItalic
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Font_FontItalicTest()
        {
            Scryber.Styles.FontStyle target = new Scryber.Styles.FontStyle();
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
        ///A test for FontSize
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void FontSizeTest()
        {
            Scryber.Styles.FontStyle target = new Scryber.Styles.FontStyle();
            PDFUnit expected = PDFUnit.Empty;
            Assert.AreEqual(expected, target.FontSize);

            expected = new PDFUnit(20,PageUnits.Millimeters);
            target.FontSize = expected;
            Assert.AreEqual(expected, target.FontSize);

            expected = new PDFUnit(10,PageUnits.Points);
            target.FontSize = expected;
            Assert.AreEqual(expected, target.FontSize);


            target.RemoveFontSize();
            expected = PDFUnit.Empty;
            Assert.AreEqual(expected, target.FontSize);

            target.FontSize = PDFUnit.Empty;
            Assert.AreEqual(PDFUnit.Empty, target.FontSize);
        }

        
    }
}
