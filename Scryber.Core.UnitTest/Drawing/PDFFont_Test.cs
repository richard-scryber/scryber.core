using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFFont_Test and is intended
    ///to contain all PDFFont_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFFont_Test
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
        ///A test for PDFFont Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void PDFFontConstructor_Test()
        {
            string family = "Sans-Serif";
            PDFUnit size = 12; 
            PDFFont target = new PDFFont(family, size);

            Assert.IsNotNull(target);
            Assert.AreEqual(target.FamilyName, family);
            Assert.AreEqual(target.Size, size);
            Assert.AreEqual(target.IsStandard, true);
            Assert.AreEqual(target.FontStyle, FontStyle.Regular);
        }

        /// <summary>
        ///A test for PDFFont Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void PDFFontConstructor_Test1()
        {
            StandardFont font = StandardFont.Helvetica;
            PDFUnit size = 12;
            PDFFont target = new PDFFont(font, size);

            Assert.IsNotNull(target);
            Assert.AreEqual(target.FamilyName, font.ToString());
            Assert.AreEqual(target.Size, size);
            Assert.AreEqual(target.IsStandard, true);
            Assert.AreEqual(target.FontStyle, FontStyle.Regular);
        }

        

        /// <summary>
        ///A test for PDFFont Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void PDFFontConstructor_Test3()
        {
            string family = "Sans-Serif";
            PDFUnit size = 12;
            FontStyle style = FontStyle.Bold;
            PDFFont target = new PDFFont(family, size, style);

            Assert.IsNotNull(target);
            Assert.AreEqual(target.FamilyName, family);
            Assert.AreEqual(target.Size, size);
            Assert.AreEqual(target.IsStandard, true);
            Assert.AreEqual(target.FontStyle, FontStyle.Bold);

        }

        /// <summary>
        ///A test for PDFFont Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void PDFFontConstructor_Test4()
        {
            string family = "Sans-Serif";
            PDFUnit size = 12;
            FontStyle style = FontStyle.Bold;
            PDFFont basefont = new PDFFont(family, size, style);

            size = 24;
            PDFFont target = new PDFFont(basefont, size);

            Assert.IsNotNull(target);
            Assert.AreEqual(target.FamilyName, basefont.FamilyName);
            Assert.AreEqual(target.Size, size);
            Assert.AreEqual(target.IsStandard, basefont.IsStandard);
            Assert.AreEqual(target.FontStyle, basefont.FontStyle);
        }

        /// <summary>
        ///A test for PDFFont Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void PDFFontConstructor_Test5()
        {
            StandardFont font = StandardFont.Helvetica;
            PDFUnit size = 20;
            FontStyle style = FontStyle.Bold | FontStyle.Italic;
            PDFFont target = new PDFFont(font, size, style);

            Assert.IsNotNull(target);
            Assert.AreEqual(target.FamilyName, font.ToString());
            Assert.AreEqual(target.Size, size);
            Assert.AreEqual(target.IsStandard, true);
            Assert.AreEqual(target.FontStyle, style);
        }

        /// <summary>
        ///A test for PDFFont Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void PDFFontConstructor_Test6()
        {
            string family = "Sans-Serif";
            PDFUnit size = 12;
            FontStyle style = FontStyle.Bold;
            PDFFont basefont = new PDFFont(family, size, style);

            style = FontStyle.Italic;
            PDFFont target = new PDFFont(basefont, style);

            Assert.IsNotNull(target);
            Assert.AreEqual(target.FamilyName, basefont.FamilyName);
            Assert.AreEqual(target.Size, basefont.Size);
            Assert.AreEqual(target.IsStandard, basefont.IsStandard);
            Assert.AreEqual(target.FontStyle, style);
        }

        /// <summary>
        ///A test for PDFFont Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void PDFFontConstructor_Test7()
        {
            string family = "Sans-Serif";
            PDFUnit size = 12;
            FontStyle style = FontStyle.Bold;
            PDFFont basefont = new PDFFont(family, size, style);

            style = FontStyle.Italic;
            size = 24;
            PDFFont target = new PDFFont(basefont, size, style);

            Assert.IsNotNull(target);
            Assert.AreEqual(target.FamilyName, basefont.FamilyName);
            Assert.AreEqual(target.Size, size);
            Assert.AreEqual(target.IsStandard, basefont.IsStandard);
            Assert.AreEqual(target.FontStyle, style);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void Equals_Test()
        {
            PDFFont one = new PDFFont("Sans-Serif", 12, FontStyle.Bold);
            PDFFont two = new PDFFont("Sans-Serif", 12, FontStyle.Bold);

            bool actual = PDFFont.Equals(one, two);
            Assert.IsTrue(actual);

            //Change font family
            two = new PDFFont("Times", 12, FontStyle.Bold);
            actual = PDFFont.Equals(one, two);
            Assert.IsFalse(actual);

            //Change size
            two = new PDFFont("Sans-Serif", 24, FontStyle.Bold);
            actual = PDFFont.Equals(one, two);
            Assert.IsFalse(actual);

            //Change style
            two = new PDFFont("Sans-Serif", 12, FontStyle.Bold | FontStyle.Italic);
            actual = PDFFont.Equals(one, two);
            Assert.IsFalse(actual);

        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void Equals_Test1()
        {
            PDFFont one = new PDFFont("Sans-Serif", 12, FontStyle.Bold);
            PDFFont two = new PDFFont("Sans-Serif", 12, FontStyle.Bold);
            object oneobj = one;
            object twoobj = two;
            bool actual = PDFFont.Equals(oneobj, twoobj);
            Assert.IsTrue(actual);


            two = new PDFFont("Sans-Serif", 14, FontStyle.Bold);
            oneobj = one;
            twoobj = two;
            actual = PDFFont.Equals(oneobj, twoobj);
            Assert.IsFalse(actual);

        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void Equals_Test2()
        {
            PDFFont one = new PDFFont("Sans-Serif", 12, FontStyle.Bold);
            PDFFont two = new PDFFont("Sans-Serif", 12, FontStyle.Bold);

            bool actual = one.Equals(two);
            Assert.IsTrue(actual);
            actual = two.Equals(one);
            Assert.IsTrue(actual);

            //Change font family
            two = new PDFFont("Times", 12, FontStyle.Bold);
            actual = one.Equals(two);
            Assert.IsFalse(actual);

            //Change size
            two = new PDFFont("Sans-Serif", 24, FontStyle.Bold);
            actual = one.Equals(two);
            Assert.IsFalse(actual);

            //Change style
            two = new PDFFont("Sans-Serif", 12, FontStyle.Bold | FontStyle.Italic);
            actual = one.Equals(two);
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for GetDrawingStyle
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void GetDrawingStyle_Test()
        {
            string family = "Sans-Serif";
            PDFUnit size = 12;
            PDFFont target = new PDFFont(family, size);

            
            System.Drawing.FontStyle actual;
            actual = target.GetDrawingStyle();
            Assert.AreEqual(System.Drawing.FontStyle.Regular, actual);

            target.FontStyle = FontStyle.Italic | FontStyle.Bold;
            actual = target.GetDrawingStyle();
            Assert.AreEqual(System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, actual);

        }

        /// <summary>
        ///A test for GetDrawingStyle
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void GetDrawingStyle_Test1()
        {
            FontStyle fontStyle = FontStyle.Bold;
            System.Drawing.FontStyle expected = System.Drawing.FontStyle.Bold;
            System.Drawing.FontStyle actual;
            actual = PDFFont.GetDrawingStyle(fontStyle);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetFullName
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void GetFullName_Test()
        {
            string family = "Sans-Serif";
            FontStyle style = FontStyle.Regular;
            string expected = "Sans-Serif";
            string actual;
            actual = PDFFont.GetFullName(family, false, false);
            Assert.AreEqual(expected, actual);

            style = FontStyle.Bold;
            expected = "Sans-Serif,Bold";
            actual = PDFFont.GetFullName(family, true, false);
            Assert.AreEqual(expected, actual);

            style = FontStyle.Bold | FontStyle.Italic;
            expected = "Sans-Serif,Bold Italic";
            actual = PDFFont.GetFullName(family, true, true);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetFullName
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void GetFullName_Test1()
        {
            string family = "Sans-Serif";
            bool bold = false; // TODO: Initialize to an appropriate value
            bool italic = false; // TODO: Initialize to an appropriate value
            string expected = family;
            string actual = PDFFont.GetFullName(family, bold, italic);
            Assert.AreEqual(expected, actual);

            bold = true;
            expected = "Sans-Serif,Bold";
            actual = PDFFont.GetFullName(family, bold, italic);
            Assert.AreEqual(expected, actual);

            italic = true;
            expected = "Sans-Serif,Bold Italic";
            actual = PDFFont.GetFullName(family, bold, italic);
            Assert.AreEqual(expected, actual);

            bold = false;
            expected = "Sans-Serif,Italic";
            actual = PDFFont.GetFullName(family, bold, italic);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void GetHashCode_Test()
        {
            PDFFont one = new PDFFont("Sans-Serif", 12, FontStyle.Bold);
            PDFFont two = new PDFFont("Sans-Serif", 12, FontStyle.Bold);

            int expected = one.GetHashCode();
            int actual = two.GetHashCode();
            Assert.AreEqual(expected, actual);

            two = new PDFFont("Sans-Serif", 12, FontStyle.Italic);
            expected = one.GetHashCode();
            actual = two.GetHashCode();
            Assert.AreNotEqual(expected, actual);

            two = new PDFFont("Sans-Serif", 24, FontStyle.Bold);
            expected = one.GetHashCode();
            actual = two.GetHashCode();
            Assert.AreNotEqual(expected, actual);

            two = new PDFFont("Times", 12, FontStyle.Bold);
            expected = one.GetHashCode();
            actual = two.GetHashCode();
            Assert.AreNotEqual(expected, actual);

        }

        

        /// <summary>
        ///A test for FamilyName
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void FamilyName_Test()
        {
            string family = "Sans-Serif";
            PDFUnit size = 12;
            PDFFont target = new PDFFont(family, size);
            Assert.AreEqual(family, target.FamilyName);

            family = "Times";
            target.FamilyName = family;
            Assert.AreEqual(family, target.FamilyName);
        }

        
        /// <summary>
        ///A test for FontStyle
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void FontStyle_Test()
        {
            string family = "Sans-Serif";
            PDFUnit size = 12;
            PDFFont target = new PDFFont(family, size);

            FontStyle expected = FontStyle.Regular;
            FontStyle actual = target.FontStyle;

            Assert.AreEqual(expected, actual);

            expected = FontStyle.Bold;
            target.FontStyle = expected;
            actual = target.FontStyle;
            Assert.AreEqual(expected, actual);

            expected = FontStyle.Bold | FontStyle.Regular;
            target.FontStyle = expected;
            actual = target.FontStyle;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FullName
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void FullName_Test()
        {
            string family = "Sans-Serif";
            PDFUnit size = 12;
            FontStyle style = FontStyle.Bold | FontStyle.Regular; //Just bold
            PDFFont target = new PDFFont(family, size, style);

            string expected = "Sans-Serif,Bold";
            string actual = target.FullName;
            Assert.AreEqual(expected, actual);

            target = new PDFFont(target, FontStyle.Regular);
            expected = "Sans-Serif";
            actual = target.FullName;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsStandard
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void IsStandard_Test()
        {
            string family = "Sans-Serif";
            PDFUnit size = 12;
            FontStyle style = FontStyle.Bold | FontStyle.Regular; //Just bold
            PDFFont target = new PDFFont(family, size, style);

            bool actual;
            actual = target.IsStandard;
            Assert.IsTrue(actual);

            target = new PDFFont(StandardFont.Helvetica, 12);
            actual = target.IsStandard;
            Assert.IsTrue(actual);

            target = new PDFFont("Helvetica", 12);
            actual = target.IsStandard;
            Assert.IsTrue(actual);

            target = new PDFFont("Helvetica", 12, FontStyle.Bold);
            actual = target.IsStandard;
            Assert.IsTrue(actual);

            target = new PDFFont(target, FontStyle.Bold);
            actual = target.IsStandard;
            Assert.IsTrue(actual);

            target = new PDFFont("Times", 12);
            actual = target.IsStandard;
            Assert.IsTrue(actual);

            target = new PDFFont("Zapf Dingbats", 12);
            actual = target.IsStandard;
            Assert.IsTrue(actual);

            target = new PDFFont("Symbol", 12);
            actual = target.IsStandard;
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for Size
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void Size_Test()
        {
            string family = "Sans-Serif";
            PDFUnit size = 12;
            PDFFont target = new PDFFont(family, size);

            PDFUnit expected = 12;
            PDFUnit actual = target.Size;

            Assert.AreEqual(expected, actual);

            expected = 24;
            target.Size = expected;
            actual = target.Size;
            Assert.AreEqual(expected, actual);

            expected = new PDFUnit(30, PageUnits.Millimeters);
            target.Size = expected;
            actual = target.Size;
            Assert.AreEqual(expected, actual);
        }
    }
}
