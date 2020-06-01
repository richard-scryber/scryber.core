using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Drawing;
using Scryber.Text;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFTextStyleTest and is intended
    ///to contain all PDFTextStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFTextStyleTest
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
        ///A test for PDFTextStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Text_ConstructorTest()
        {
            PDFTextStyle target = new PDFTextStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(PDFStyleKeys.TextItemKey, target.ItemKey);
        }

        
        /// <summary>
        ///A test for DateFormat
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Text_DateFormatTest()
        {
            PDFTextStyle target = new PDFTextStyle();
            string expected = string.Empty;
            string actual;
            actual = target.DateFormat;
            Assert.AreEqual(expected, actual);

            expected = "dd MM YYYY hh:mm:ss";
            target.DateFormat = expected;
            actual = target.DateFormat;
            Assert.AreEqual(expected, actual);

            expected = "D";
            target.DateFormat = expected;
            actual = target.DateFormat;
            Assert.AreEqual(expected, actual);

            expected = string.Empty;
            target.RemoveDateFormat();
            actual = target.DateFormat;
            Assert.AreEqual(expected, actual);


        }

        /// <summary>
        ///A test for Decoration
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Text_DecorationTest()
        {
            PDFTextStyle target = new PDFTextStyle();
            TextDecoration expected = TextDecoration.None;
            Assert.AreEqual(expected, target.Decoration);

            expected = TextDecoration.Overline;
            target.Decoration = expected;
            TextDecoration actual = target.Decoration;
            Assert.AreEqual(expected, actual);

            expected = TextDecoration.StrikeThrough | TextDecoration.Underline;
            target.Decoration = expected;
            actual = target.Decoration;
            Assert.AreEqual(expected, actual);

            expected = TextDecoration.None;
            target.RemoveDecoration();
            actual = target.Decoration;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for FirstLineInset
        ///</summary>
        [TestMethod()]
        public void Text_FirstLineInsetTest()
        {
            PDFTextStyle target = new PDFTextStyle();
            PDFUnit expected = PDFUnit.Zero;
            Assert.AreEqual(expected, target.FirstLineInset);
            

            expected = 10;
            PDFUnit actual;
            target.FirstLineInset = expected;
            actual = target.FirstLineInset;
            Assert.AreEqual(expected, actual);
            

            expected = new PDFUnit(20, PageUnits.Millimeters);
            target.FirstLineInset = expected;
            actual = target.FirstLineInset;
            Assert.AreEqual(expected, actual);
           

            expected = PDFUnit.Zero;
            target.RemoveFirstLineInset();
            actual = target.FirstLineInset;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Leading
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Text_LeadingTest()
        {
            PDFTextStyle target = new PDFTextStyle();
            PDFUnit expected = PDFUnit.Zero;
            Assert.AreEqual(expected, target.Leading);

            expected = 10;
            PDFUnit actual;
            target.Leading = expected;
            actual = target.Leading;
            Assert.AreEqual(expected, actual);

            expected = new PDFUnit(20, PageUnits.Millimeters);
            target.Leading = expected;
            actual = target.Leading;
            Assert.AreEqual(expected, actual);

            expected = PDFUnit.Zero;
            target.RemoveLeading();
            actual = target.Leading;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for NumberFormat
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Text_NumberFormatTest()
        {
            PDFTextStyle target = new PDFTextStyle();
            string expected = string.Empty;
            string actual;
            actual = target.NumberFormat;
            Assert.AreEqual(expected, actual);

            expected = "###0.00##";
            target.NumberFormat = expected;
            actual = target.NumberFormat;
            Assert.AreEqual(expected, actual);

            expected = "C";
            target.NumberFormat = expected;
            actual = target.NumberFormat;
            Assert.AreEqual(expected, actual);

            expected = string.Empty;
            target.RemoveNumberFormat();
            actual = target.NumberFormat;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PreserveWhitespace
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Text_PreserveWhitespaceTest()
        {
            PDFTextStyle target = new PDFTextStyle();
            bool expected = false;
            Assert.AreEqual(expected, target.PreserveWhitespace);

            expected = true;
            target.PreserveWhitespace = expected;
            bool actual = target.PreserveWhitespace;
            Assert.AreEqual(expected, actual);

            expected = false;
            target.PreserveWhitespace = expected;
            actual = target.PreserveWhitespace;
            Assert.AreEqual(expected, actual);

            expected = false;
            target.RemoveWhitespace();
            actual = target.PreserveWhitespace;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for WordSpacing
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Text_WordSpacingTest()
        {
            PDFTextStyle target = new PDFTextStyle();
            
            // Default value

            PDFUnit expected = 0;
            PDFUnit actual = target.WordSpacing;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20;
            target.WordSpacing = expected;
            actual = target.WordSpacing;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = 10;
            target.WordSpacing = expected;
            actual = target.WordSpacing;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = 0;
            target.RemoveWordSpacing();
            actual = target.WordSpacing;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for WrapText
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Text_WrapTextTest()
        {
            PDFTextStyle target = new PDFTextStyle(); 
            
            //Default 

            WordWrap expected = WordWrap.Auto;
            Assert.AreEqual(expected, target.WrapText);

            //Set value

            expected = WordWrap.Character;
            WordWrap actual;
            target.WrapText = expected;
            actual = target.WrapText;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = WordWrap.NoWrap;
            target.WrapText = expected;
            actual = target.WrapText;
            Assert.AreEqual(expected, actual);

            //Remove value

            expected = WordWrap.Auto;
            target.RemoveWrapText();
            actual = target.WrapText;
            Assert.AreEqual(expected, actual);
        }
    }
}
