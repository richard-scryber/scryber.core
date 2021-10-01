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

            PDFColor target = new PDFColor(255, 0, 0);

            Assert.AreEqual(ColorSpace.RGB, target.ColorSpace);
            Assert.AreEqual(1.0, target.Red);
            Assert.AreEqual(0.0, target.Green);
            Assert.AreEqual(0.0, target.Blue);
            

            
            
            target = new PDFColor(128);
            Assert.AreEqual(ColorSpace.G, target.ColorSpace);
            Assert.AreEqual(Math.Round(0.5, 2), Math.Round(target.Gray, 2));
            Assert.AreEqual(128, target.Gray255);


            target = new PDFColor();

            Assert.AreEqual(ColorSpace.None, target.ColorSpace);
            Assert.IsTrue(target.IsEmpty);
            Assert.IsTrue(target.IsTransparent);

            Assert.AreEqual(-1.0F, target.Red);
            Assert.AreEqual(-1.0F, target.Green);
            Assert.AreEqual(-1.0F, target.Blue);
            Assert.AreEqual(-1.0F, target.Gray);

            Assert.AreEqual(-1.0F, target.Cyan);
            Assert.AreEqual(-1.0F, target.Magenta);
            Assert.AreEqual(-1.0F, target.Yellow);
            Assert.AreEqual(-1.0F, target.Black);
        }

        

        /// <summary>
        ///A test for PDFColor Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFColorConstructor_Test2()
        {
            int red = 255;
            int green = 0;
            int blue = 0;
            PDFColor target = new PDFColor(red, green, blue);

            Assert.AreEqual(ColorSpace.RGB, target.ColorSpace);

            Assert.AreEqual(red, target.Red255);
            Assert.AreEqual(green, target.Green255);
            Assert.AreEqual(blue, target.Blue255);

            Assert.AreEqual(1.0F, target.Red);
            Assert.AreEqual(0.0F, target.Green);
            Assert.AreEqual(0.0F, target.Blue);

            Assert.AreEqual(-1.0F, target.Gray);

            Assert.AreEqual(-1.0F, target.Cyan);
            Assert.AreEqual(-1.0F, target.Magenta);
            Assert.AreEqual(-1.0F, target.Yellow);
            Assert.AreEqual(-1.0F, target.Black);

            

            Assert.AreEqual(-1, target.Gray255);

            Assert.AreEqual(-1, target.Cyan255);
            Assert.AreEqual(-1, target.Magenta255);
            Assert.AreEqual(-1, target.Yellow255);
            Assert.AreEqual(-1, target.Black255);
        }

        /// <summary>
        ///A test for PDFColor Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFColorConstructor_Test3()
        {
            ColorSpace cs = ColorSpace.CMYK;
            int one = 51;
            int two = 255;
            int three = 128;
            int black = 255;

            PDFColor target = new PDFColor(one, two, three, black);

            Assert.AreEqual(cs, target.ColorSpace);
            Assert.AreEqual(one, target.Cyan255);
            Assert.AreEqual(two, target.Magenta255);
            Assert.AreEqual(three, target.Yellow255);
            Assert.AreEqual(black, target.Black255);

            Assert.AreEqual(Math.Round(one / 255.0F, 2), Math.Round(target.Cyan, 2));
            Assert.AreEqual(Math.Round(two / 255.0F, 2), target.Magenta);
            Assert.AreEqual(Math.Round(three / 255.0F, 2), Math.Round(target.Yellow, 2));
            Assert.AreEqual(Math.Round(black / 255.0F, 2), target.Black);
            
            Assert.AreEqual(-1.0F, target.Gray);

            Assert.AreEqual(-1.0F, target.Red);
            Assert.AreEqual(-1.0F, target.Green);
            Assert.AreEqual(-1.0F, target.Blue);
        }

        /// <summary>
        ///A test for PDFColor Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFColorConstructor_Test4()
        {

            int gray = 128;
            PDFColor target = new PDFColor(gray);

            Assert.AreEqual(ColorSpace.G, target.ColorSpace);
            int expected = gray;
            int actual = target.Gray255;
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(-1.0F, target.Red);
            Assert.AreEqual(-1.0F, target.Green);
            Assert.AreEqual(-1.0F, target.Blue);

            Assert.AreEqual(-1.0F, target.Cyan);
            Assert.AreEqual(-1.0F, target.Magenta);
            Assert.AreEqual(-1.0F, target.Yellow);
            Assert.AreEqual(-1.0F, target.Black);

        }

       

        /// <summary>
        ///A test for Parse
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Parse_Test()
        {
            string color = "#FF00FF";
            PDFColor expected = new PDFColor(255, 0, 255);
            PDFColor actual;

            actual = PDFColor.Parse(color);
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(255, actual.Red255);
            Assert.AreEqual(0, actual.Green255);
            Assert.AreEqual(255, actual.Blue255);

            Assert.AreEqual(1.0F, actual.Red);
            Assert.AreEqual(0.0F, actual.Green);
            Assert.AreEqual(1.0F, actual.Blue);

            color = "rgb(255,0,255)";
            actual = PDFColor.Parse(color);
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(1.0F, actual.Red);
            Assert.AreEqual(0.0F, actual.Green);
            Assert.AreEqual(1.0F, actual.Blue);

            color = "#F0F";
            actual = PDFColor.Parse(color);
            Assert.AreEqual(expected, actual);

            Assert.AreEqual(1.0F, actual.Red);
            Assert.AreEqual(0.0F, actual.Green);
            Assert.AreEqual(1.0F, actual.Blue);

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void ToString_Test()
        {
            PDFColor target = new PDFColor(255, 0, 128);
            string expected = "rgb(255,0,128)"; 
            string actual;
            actual = target.ToString();

            Assert.AreEqual(expected, actual);
        }

        #region RGB values

        /// <summary>
        ///A test for Red
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Red_Test()
        {
            PDFColor target = new PDFColor(255, 255, 255);
            float actual;
            actual = target.Red;
            Assert.AreEqual(actual, 1.0F);

            target = new PDFColor(128, 255, 0);
            actual = target.Red;
            Assert.AreEqual(Math.Round(actual, 2), 0.5F);
        }

        /// <summary>
        ///A test for Red
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Red255_Test()
        {
            PDFColor target = new PDFColor(255, 255, 255);
            int actual;
            actual = target.Red255;
            Assert.AreEqual(actual, 255);

            target = new PDFColor(128, 255, 255);
            actual = target.Red255;
            Assert.AreEqual(actual, 128);

            //Gray should not have a red component
            target = new PDFColor(128);
            actual = target.Red255;
            Assert.AreEqual(actual, -1);
        }

        /// <summary>
        ///A test for Green
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Green_Test()
        {
            PDFColor target = new PDFColor(255, 255, 255);
            float actual;
            actual = target.Green;
            Assert.AreEqual(actual, 1.0F);

            target = new PDFColor(255, 128, 0);
            actual = target.Green;
            Assert.AreEqual(Math.Round(actual, 2), 0.5);

            target = new PDFColor(255);
            actual = target.Green;
            Assert.AreEqual(actual, -1.0F);
        }

        /// <summary>
        ///A test for Green
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Green255_Test()
        {
            PDFColor target = new PDFColor(255, 255, 255);
            int actual;
            actual = target.Green255;
            Assert.AreEqual(actual, 255);

            target = new PDFColor(255, 128, 0);
            actual = target.Green255;
            Assert.AreEqual(actual, 128);

            target = new PDFColor(255);
            actual = target.Green255;
            Assert.AreEqual(actual, -1);
        }


        /// <summary>
        ///A test for Blue
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Blue_Test()
        {
            PDFColor target = new PDFColor(255, 255, 255); 
            float actual;
            actual = target.Blue;
            Assert.AreEqual(Math.Round(actual, 2), 1.0F);

            target = new PDFColor(255, 0, 128);
            actual = target.Blue;
            Assert.AreEqual(Math.Round(actual, 2), 0.5);

            target = new PDFColor(255);
            actual = target.Blue;

            Assert.AreEqual(actual, -1.0F);
        }

        /// <summary>
        ///A test for Blue255
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Blue255_Test()
        {
            PDFColor target = new PDFColor(255, 255, 255);
            int actual;
            actual = target.Blue255;
            Assert.AreEqual(actual, 255);

            target = new PDFColor(255, 0, 128);
            actual = target.Blue255;
            Assert.AreEqual(actual, 128);
        }

        #endregion

        /// <summary>
        ///A test for Gray
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Gray_Test()
        {
            ColorSpace cs = ColorSpace.G;
            PDFColor target = new PDFColor(255);
            float actual;
            actual = target.Gray;
            Assert.AreEqual(target.ColorSpace, cs);
            Assert.AreEqual(actual, 1.0F);

            cs = ColorSpace.G;
            target = new PDFColor(128);
            actual = target.Gray;
            Assert.AreEqual(target.ColorSpace, cs);
            Assert.AreEqual(Math.Round(actual, 2), 0.5);
        }

        /// <summary>
        ///A test for ColorSpace
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void ColorSpace_Test()
        {
            ColorSpace cs = ColorSpace.RGB;
            PDFColor target = new PDFColor(255, 255, 255);
            ColorSpace actual;
            actual = target.ColorSpace;
            Assert.AreEqual(actual, cs);

            cs = ColorSpace.CMYK;
            target = new PDFColor(255, 255, 255, 255);
            actual = target.ColorSpace;
            Assert.AreEqual(actual, cs);

            cs = ColorSpace.G;
            target = new PDFColor(255);
            actual = target.ColorSpace;
            Assert.AreEqual(actual, cs);

            cs = ColorSpace.None;
            target = new PDFColor();
            actual = target.ColorSpace;
            Assert.AreEqual(actual, cs);
        }

        

        

        /// <summary>
        ///A test for IsEmpty
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void IsEmpty_Test()
        {
            PDFColor target = new PDFColor();

            Assert.IsTrue(target.IsEmpty);
            Assert.IsTrue(target.IsTransparent);


            target = new PDFColor(255);
            Assert.IsFalse(target.IsEmpty);
            Assert.IsFalse(target.IsTransparent);
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
            Assert.IsTrue(actual.IsEmpty, "actual was not empty");
            Assert.IsTrue(actual.IsTransparent, "actual was not transparent");
            Assert.IsTrue(PDFColor.Transparent.IsTransparent, "Color.Transparent was null");

            Assert.IsTrue(actual.Equals(PDFColor.Transparent),"Colors are not equal");
        }
    }
}
