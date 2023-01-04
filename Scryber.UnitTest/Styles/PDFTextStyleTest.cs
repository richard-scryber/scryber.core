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

        


        /// <summary>
        ///A test for PDFTextStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Text_ConstructorTest()
        {
            TextStyle target = new TextStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(StyleKeys.TextItemKey, target.ItemKey);
        }

        
        /// <summary>
        ///A test for DateFormat
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Text_DateFormatTest()
        {
            TextStyle target = new TextStyle();
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
            TextStyle target = new TextStyle();
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
            TextStyle target = new TextStyle();
            Unit expected = Unit.Zero;
            Assert.AreEqual(expected, target.FirstLineInset);
            

            expected = 10;
            Unit actual;
            target.FirstLineInset = expected;
            actual = target.FirstLineInset;
            Assert.AreEqual(expected, actual);
            

            expected = new Unit(20, PageUnits.Millimeters);
            target.FirstLineInset = expected;
            actual = target.FirstLineInset;
            Assert.AreEqual(expected, actual);
           

            expected = Unit.Zero;
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
            TextStyle target = new TextStyle();
            Unit expected = Unit.Zero;
            Assert.AreEqual(expected, target.Leading);

            expected = 10;
            Unit actual;
            target.Leading = expected;
            actual = target.Leading;
            Assert.AreEqual(expected, actual);

            expected = new Unit(20, PageUnits.Millimeters);
            target.Leading = expected;
            actual = target.Leading;
            Assert.AreEqual(expected, actual);

            expected = Unit.Zero;
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
            TextStyle target = new TextStyle();
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
            TextStyle target = new TextStyle();
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
            TextStyle target = new TextStyle();
            
            // Default value

            Unit expected = 0;
            Unit actual = target.WordSpacing;
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
            TextStyle target = new TextStyle(); 
            
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

        /// <summary>
        ///A test for WrapText
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Text_ContentTest()
        {
            TextStyle target = new TextStyle();

            //Default 

            
            Assert.IsNull(target.Content);

            //Set value
            ContentDescriptor expected = new ContentTextDescriptor("This is the content");

            ContentDescriptor actual = new ContentTextDescriptor("This is the content");
            target.Content = expected;
            actual = target.Content;
            Assert.AreEqual(expected.Value, actual.Value);

            // Change Value with cast

            expected = (ContentDescriptor)"url(path.to/image.png)";
            Assert.IsNotNull(expected);
            target.Content = expected;
            actual = target.Content;
            Assert.IsNotNull(actual);
            Assert.AreEqual((expected as ContentImageDescriptor).Source, (actual as ContentImageDescriptor).Source);

            //Remove value

            expected = null;
            target.RemoveContent();
            actual = target.Content;
            Assert.AreEqual(expected, actual);
        }
    }
}
