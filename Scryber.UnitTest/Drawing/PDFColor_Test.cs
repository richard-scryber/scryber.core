using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using System.Drawing;
using System.CodeDom;
using Scryber.PDF.Native;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFColor_Test and is intended
    ///to contain all PDFColor_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFColor_Test
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
        ///A test for PDFColor Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFColorConstructor_Test()
        {
            ColorSpace cs = ColorSpace.RGB;
            Color c = Color.Red;
            PDFColor target = new PDFColor(cs, c);

            Assert.AreEqual(c, target.Color);
            Assert.AreEqual(cs, target.ColorSpace);
            Assert.AreEqual(1.0, Math.Round(target.Red.Value,2));
            Assert.AreEqual(0.0, Math.Round(target.Green.Value,2));
            Assert.AreEqual(0.0, Math.Round(target.Blue.Value,2));
            

            cs = ColorSpace.G;
            c = Color.Gray;

            target = new PDFColor(cs, c);
            Assert.AreEqual(cs, target.ColorSpace);
            Assert.AreEqual(c, target.Color);

            try
            {
                target = new PDFColor(ColorSpace.LAB, c);
                Assert.Fail("LAB colour space is not supported");
            }
            catch(ArgumentException)
            {
                TestContext.WriteLine("Successfully caught the exception for the HSL Color space");
            }

            try
            {
                target = new PDFColor(ColorSpace.Custom, c);
                Assert.Fail("Custom colour space is not supported");
            }
            catch (ArgumentException)
            {
                TestContext.WriteLine("Successfully caught the exception for the HSL Color space");
            }
            
        }

        

        /// <summary>
        ///A test for PDFColor Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFColorConstructor_Test2()
        {
            double red = 1.0;
            double green = 0.0;
            double blue = 0.0;
            PDFColor target = new PDFColor(red, green, blue);

            Assert.AreEqual(ColorSpace.RGB, target.ColorSpace);
            Assert.AreEqual(red, Math.Round(target.Red.Value, 2));
            Assert.AreEqual(green, Math.Round(target.Green.Value, 2));
            Assert.AreEqual(blue, Math.Round(target.Blue.Value, 2));

        }

        /// <summary>
        ///A test for PDFColor Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFColorConstructor_Test3()
        {
            ColorSpace cs = ColorSpace.RGB;
            double one = 0.2;
            double two = 1.0;
            double three = 0.5;
            PDFColor target = new PDFColor(cs, one, two, three);

            Assert.AreEqual(cs, target.ColorSpace);
            Assert.AreEqual(one, Math.Round(target.Red.Value,1));
            Assert.AreEqual(two, Math.Round(target.Green.Value,1));
            Assert.AreEqual(three, Math.Round(target.Blue.Value, 1));

            
        }

        /// <summary>
        ///A test for PDFColor Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFColorConstructor_Test4()
        {

            double gray = 0.5;
            PDFColor target = new PDFColor(gray);

            Assert.AreEqual(ColorSpace.G, target.ColorSpace);
            double expected = Math.Round(gray, 1);
            double actual = Math.Round(target.Gray.Value, 1);
            Assert.AreEqual(expected, actual);

        }

       

        /// <summary>
        ///A test for Parse
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Parse_Test()
        {
            string color = "#FF00FF";
            PDFColor expected = new PDFColor(1.0, 0.0, 1.0);
            PDFColor actual;
            actual = PDFColor.Parse(color);
            Assert.AreEqual(expected, actual);

            color = "rgb(255,0,255)";
            actual = PDFColor.Parse(color);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void ToString_Test()
        {
            ColorSpace cs = ColorSpace.RGB;
            Color c = Color.FromArgb(255, 255, 255);
            PDFColor target = new PDFColor(cs, c);
            string expected = "rgb(255,255,255)"; 
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Implicit
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void op_Implicit_Test()
        {
            Color rgbcolor = Color.FromArgb(255, 255, 255);
            PDFColor expected = new PDFColor(1, 1, 1);
            PDFColor actual;
            actual = rgbcolor;
            Assert.AreEqual(expected, actual);
            
        }


        /// <summary>
        ///A test for Blue
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Blue_Test()
        {
            ColorSpace cs = ColorSpace.RGB;
            PDFColor target = new PDFColor(cs, 1.0, 1.0, 1.0); 
            PDFReal actual;
            actual = target.Blue;
            Assert.AreEqual(Math.Round(actual.Value,1), 1.0);

            target = new PDFColor(cs, 1.0, 0, 0.5);
            actual = target.Blue;
            Assert.AreEqual(Math.Round(actual.Value, 1), 0.5);
        }

        /// <summary>
        ///A test for Color
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Color_Test()
        {
            ColorSpace cs = ColorSpace.RGB;
            PDFColor target = new PDFColor(cs, 1.0, 1.0, 1.0);
            Color actual;
            actual = target.Color;
            Assert.AreEqual(actual.R, 255);
            Assert.AreEqual(actual.G, 255);
            Assert.AreEqual(actual.B, 255);
        }

        /// <summary>
        ///A test for ColorSpace
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void ColorSpace_Test()
        {
            ColorSpace cs = ColorSpace.RGB;
            PDFColor target = new PDFColor(cs, 1.0, 1.0, 1.0);
            ColorSpace actual;
            actual = target.ColorSpace;
            Assert.AreEqual(actual, cs);
        }

        /// <summary>
        ///A test for Gray
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Gray_Test()
        {
            ColorSpace cs = ColorSpace.RGB;
            PDFColor target = new PDFColor(cs, 1.0, 1.0, 1.0);
            PDFReal actual;
            actual = target.Gray;
            Assert.AreEqual(Math.Round(actual.Value, 1), 1.0);

            cs = ColorSpace.G;
            target = new PDFColor(0.5);
            actual = target.Gray;
            Assert.AreEqual(Math.Round(actual.Value, 1), 0.5);
        }

        /// <summary>
        ///A test for Green
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Green_Test()
        {
            ColorSpace cs = ColorSpace.RGB;
            PDFColor target = new PDFColor(cs, 1.0, 1.0, 1.0);
            PDFReal actual;
            actual = target.Green;
            Assert.AreEqual(Math.Round(actual.Value, 2), 1.0);

            target = new PDFColor(cs, 1.0, 0.5, 0);
            actual = target.Green;
            Assert.AreEqual(Math.Round(actual.Value,1), 0.5);
        }

        /// <summary>
        ///A test for IsEmpty
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void IsEmpty_Test()
        {
            ColorSpace cs = ColorSpace.RGB;
            Color c = new Color();//Empty System.Drawing.Color
            PDFColor target = new PDFColor(cs, c);

            Assert.IsTrue(target.IsEmpty);
            
            c = Color.FromArgb(255, 0, 0);
            target = new PDFColor(cs, c);
            Assert.IsFalse(target.IsEmpty);
        }

        /// <summary>
        ///A test for Red
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Red_Test()
        {
            ColorSpace cs = ColorSpace.RGB;
            PDFColor target = new PDFColor(cs, 1.0, 1.0, 1.0);
            PDFReal actual;
            actual = target.Red;
            Assert.AreEqual(Math.Round(actual.Value,2), 1.0);

            target = new PDFColor(cs, 0.5, 1.0, 0);
            actual = target.Red;
            Assert.AreEqual(Math.Round(actual.Value, 1), 0.5);
        }

        /// <summary>
        ///A test for Transparent
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Transparent_Test()
        {
            PDFColor actual;
            actual = PDFColor.Transparent;
            Assert.IsNotNull(actual, "actual was null");
            Assert.IsNotNull(actual.Color, "actual.Color was null");
            Assert.IsNotNull(Color.Transparent, "Color.Transparent was null");

            Assert.IsTrue(actual.Color == Color.Transparent,"Colors are not equal");
        }
    }
}
