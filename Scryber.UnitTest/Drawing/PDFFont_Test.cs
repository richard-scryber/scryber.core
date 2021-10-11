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
            Unit size = 12; 
            Font target = new Font(family, size);

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
            Unit size = 12;
            Font target = new Font(font, size);

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
            Unit size = 12;
            FontStyle style = FontStyle.Italic;
            int weight = FontWeights.Black;
            Font target = new Font(family, size, weight, style);

            Assert.IsNotNull(target);
            Assert.AreEqual(target.FamilyName, family);
            Assert.AreEqual(target.Size, size);
            Assert.AreEqual(target.IsStandard, false);
            Assert.AreEqual(FontWeights.Black, target.FontWeight);
            Assert.AreEqual(target.FontStyle, FontStyle.Italic);

        }

        /// <summary>
        ///A test for PDFFont Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void PDFFontConstructor_Test4()
        {
            string family = "Sans-Serif";
            Unit size = 12;
            FontStyle style = FontStyle.Regular;
            int weight = FontWeights.Light;
            Font basefont = new Font(family, size, weight, style);

            size = 24;
            Font target = new Font(basefont, size);

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
            Unit size = 20;

            Font target = new Font(font, size);

            Assert.IsNotNull(target);
            Assert.AreEqual(target.FamilyName, font.ToString());
            Assert.AreEqual(target.Size, size);
            Assert.AreEqual(target.IsStandard, true);
            Assert.AreEqual(target.FontWeight, FontWeights.Regular);
            Assert.AreEqual(target.FontStyle, FontStyle.Regular);
        }

        /// <summary>
        ///A test for PDFFont Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void PDFFontConstructor_Test6()
        {
            string family = "Sans-Serif";
            Unit size = 12;
            FontStyle style = FontStyle.Italic;
            int fontWeight = FontWeights.Black;
            Font basefont = new Font(family, size, fontWeight, style);

            style = FontStyle.Italic;
            fontWeight = FontWeights.Light;
            Font target = new Font(basefont, fontWeight,  style);

            Assert.IsNotNull(target);
            Assert.AreEqual(target.FamilyName, basefont.FamilyName);
            Assert.AreEqual(target.Size, basefont.Size);
            Assert.AreEqual(target.IsStandard, basefont.IsStandard);
            Assert.AreEqual(target.FontWeight, fontWeight);
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
            Unit size = 12;
            FontStyle style = FontStyle.Regular;
            int weight = FontWeights.Regular;
            Font basefont = new Font(family, size);

            style = FontStyle.Italic;
            size = 24;
            weight = FontWeights.Bold;
            Font target = new Font(basefont, size, weight, style);

            Assert.IsNotNull(target);
            Assert.AreEqual(target.FamilyName, basefont.FamilyName);
            Assert.AreEqual(target.Size, size);
            Assert.AreEqual(target.IsStandard, basefont.IsStandard);
            Assert.AreEqual(target.FontStyle, style);
            Assert.AreEqual(target.FontWeight, weight);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void Equals_Test()
        {
            Font one = new Font("Sans-Serif", 12, FontWeights.Black, FontStyle.Italic);
            Font two = new Font("Sans-Serif", 12, FontWeights.Black, FontStyle.Italic);

            bool actual = Font.Equals(one, two);
            Assert.IsTrue(actual);

            //Change font family
            two = new Font("Times", 12, FontWeights.Black, FontStyle.Italic);
            actual = Font.Equals(one, two);
            Assert.IsFalse(actual);

            //Change size
            two = new Font("Sans-Serif", 24, FontWeights.Black, FontStyle.Italic);
            actual = Font.Equals(one, two);
            Assert.IsFalse(actual);

            //Change style
            two = new Font("Sans-Serif", 12, FontWeights.Black, FontStyle.Oblique);
            actual = Font.Equals(one, two);
            Assert.IsFalse(actual);

            //Change weight
            two = new Font("Sans-Serif", 12, FontWeights.Light, FontStyle.Oblique);
            actual = Font.Equals(one, two);
            Assert.IsFalse(actual);

        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void Equals_Test1()
        {
            Font one = new Font("Sans-Serif", 12, FontWeights.Bold, FontStyle.Italic);
            Font two = new Font("Sans-Serif", 12, FontWeights.Bold, FontStyle.Italic);
            object oneobj = one;
            object twoobj = two;
            bool actual = Font.Equals(oneobj, twoobj);
            Assert.IsTrue(actual);


            two = new Font("Sans-Serif", 14, FontWeights.Black, FontStyle.Regular);
            oneobj = one;
            twoobj = two;
            actual = Font.Equals(oneobj, twoobj);
            Assert.IsFalse(actual);

        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void Equals_Test2()
        {
            Font one = new Font("Sans-Serif", 12, FontWeights.Bold, FontStyle.Italic);
            Font two = new Font("Sans-Serif", 12, FontWeights.Bold, FontStyle.Italic);

            bool actual = one.Equals(two);
            Assert.IsTrue(actual);
            actual = two.Equals(one);
            Assert.IsTrue(actual);

            //Change font family
            two = new Font("Times", 12, FontWeights.Bold, FontStyle.Italic);
            actual = one.Equals(two);
            Assert.IsFalse(actual);

            //Change size
            two = new Font("Sans-Serif", 24, FontWeights.Bold, FontStyle.Italic);
            actual = one.Equals(two);
            Assert.IsFalse(actual);

            //Change style
            two = new Font("Sans-Serif", 12, FontWeights.Bold, FontStyle.Regular);
            actual = one.Equals(two);
            Assert.IsFalse(actual);

            //Change weight
            two = new Font("Sans-Serif", 12, FontWeights.Regular, FontStyle.Italic);
            actual = one.Equals(two);
            Assert.IsFalse(actual);
        }

        


        /// <summary>
        ///A test for GetFullName
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void GetFullName_Test()
        {
            string family = "Sans-Serif";

            string expected = "Sans-Serif";
            string actual;
            actual = Font.GetFullName(family, false, false);
            Assert.AreEqual(expected, actual);

            expected = "Sans-Serif,Bold";
            actual = Font.GetFullName(family, true, false);
            Assert.AreEqual(expected, actual);

            expected = "Sans-Serif,Bold Italic";
            actual = Font.GetFullName(family, true, true);
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
            string actual = Font.GetFullName(family, bold, italic);
            Assert.AreEqual(expected, actual);

            bold = true;
            expected = "Sans-Serif,Bold";
            actual = Font.GetFullName(family, bold, italic);
            Assert.AreEqual(expected, actual);

            italic = true;
            expected = "Sans-Serif,Bold Italic";
            actual = Font.GetFullName(family, bold, italic);
            Assert.AreEqual(expected, actual);

            bold = false;
            expected = "Sans-Serif,Italic";
            actual = Font.GetFullName(family, bold, italic);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void GetHashCode_Test()
        {
            Font one = new Font("Sans-Serif", 12, FontWeights.Bold, FontStyle.Regular);
            Font two = new Font("Sans-Serif", 12, FontWeights.Bold, FontStyle.Regular);

            int expected = one.GetHashCode();
            int actual = two.GetHashCode();
            Assert.AreEqual(expected, actual);

            //change style
            two = new Font("Sans-Serif", 12, FontWeights.Bold, FontStyle.Italic);
            expected = one.GetHashCode();
            actual = two.GetHashCode();
            Assert.AreNotEqual(expected, actual);

            //change size
            two = new Font("Sans-Serif", 24, FontWeights.Bold, FontStyle.Regular);
            expected = one.GetHashCode();
            actual = two.GetHashCode();
            Assert.AreNotEqual(expected, actual);

            //change weight
            two = new Font("Sans-Serif", 12, FontWeights.Black, FontStyle.Regular);
            expected = one.GetHashCode();
            actual = two.GetHashCode();
            Assert.AreNotEqual(expected, actual);

            //change family
            two = new Font("Times", 12, FontWeights.Bold, FontStyle.Regular);
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
            Unit size = 12;
            Font target = new Font(family, size);
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
            Unit size = 12;
            Font target = new Font(family, size);

            FontStyle expected = FontStyle.Regular;
            FontStyle actual = target.FontStyle;

            Assert.AreEqual(expected, actual);

            expected = FontStyle.Italic;
            target.FontStyle = expected;
            actual = target.FontStyle;
            Assert.AreEqual(expected, actual);

            expected = FontStyle.Oblique;
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
            Unit size = 12;
            FontStyle style = FontStyle.Regular; //Just bold
            int weight = FontWeights.Bold;
            Font target = new Font(family, size, weight, style);

            string expected = "Sans-Serif,Bold";
            string actual = target.FullName;
            Assert.AreEqual(expected, actual);

            target = new Font(target, FontWeights.Regular, FontStyle.Regular);
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
            Unit size = 12;
            FontStyle style = FontStyle.Regular; //Just bold
            int weight = FontWeights.Bold;
            Font target = new Font(family, size, weight, style);

            bool actual;
            actual = target.IsStandard;
            Assert.IsTrue(actual);

            target = new Font(StandardFont.Helvetica, 12);
            actual = target.IsStandard;
            Assert.IsTrue(actual);

            target = new Font("Helvetica", 12);
            actual = target.IsStandard;
            Assert.IsTrue(actual);

            target = new Font("Helvetica", 12, weight, FontStyle.Italic);
            actual = target.IsStandard;
            Assert.IsTrue(actual);

            target = new Font(target, FontWeights.Bold, FontStyle.Italic);
            actual = target.IsStandard;
            Assert.IsTrue(actual);

            target = new Font("Times", 12);
            actual = target.IsStandard;
            Assert.IsTrue(actual);

            target = new Font("Zapf Dingbats", 12);
            actual = target.IsStandard;
            Assert.IsTrue(actual);

            target = new Font("Symbol", 12);
            actual = target.IsStandard;
            Assert.IsTrue(actual);

            //False for oblique, or weights other than bold or regular

            target = new Font("Helvetica", 12, FontWeights.Black, FontStyle.Italic);
            actual = target.IsStandard;
            Assert.IsFalse(actual);

            target = new Font("Helvetica", 12, FontWeights.Light, FontStyle.Regular);
            actual = target.IsStandard;
            Assert.IsFalse(actual);

            target = new Font("Helvetica", 12, FontWeights.Regular, FontStyle.Oblique);
            actual = target.IsStandard;
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for Size
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void Size_Test()
        {
            string family = "Sans-Serif";
            Unit size = 12;
            Font target = new Font(family, size);

            Unit expected = 12;
            Unit actual = target.Size;

            Assert.AreEqual(expected, actual);

            expected = 24;
            target.Size = expected;
            actual = target.Size;
            Assert.AreEqual(expected, actual);

            expected = new Unit(30, PageUnits.Millimeters);
            target.Size = expected;
            actual = target.Size;
            Assert.AreEqual(expected, actual);
        }
    }
}
