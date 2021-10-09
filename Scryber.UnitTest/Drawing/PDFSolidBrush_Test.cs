using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.PDF.Native;
using Scryber;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFSolidBrush_Test and is intended
    ///to contain all PDFSolidBrush_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFSolidBrush_Test
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
        ///A test for PDFSolidBrush Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFSolidBrushConstructor_Test()
        {
            Color color = PDFColors.Aqua; 
            double opacity = 1F; 
            PDFSolidBrush target = new PDFSolidBrush(color, opacity);

            Assert.IsNotNull(target);
            Assert.AreEqual(color, target.Color);
            Assert.AreEqual(opacity, target.Opacity.Value);
        }

        /// <summary>
        ///A test for PDFSolidBrush Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFSolidBrushConstructor_Test1()
        {
            Color color = PDFColors.Aqua;
            PDFReal opacity = 1F; 
            PDFSolidBrush target = new PDFSolidBrush(color, opacity);

            Assert.IsNotNull(target);
            Assert.AreEqual(color, target.Color);
            Assert.AreEqual(opacity, target.Opacity);
            Assert.AreEqual(FillType.Solid, target.FillStyle);
        }

        /// <summary>
        ///A test for PDFSolidBrush Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFSolidBrushConstructor_Test2()
        {
            PDFSolidBrush target = new PDFSolidBrush();
           
            Assert.IsNotNull(target);
            Assert.AreEqual(PDFColors.Transparent, target.Color);
            Assert.AreEqual((PDFReal)1, target.Opacity);
            Assert.AreEqual(FillType.Solid, target.FillStyle);
        }

        /// <summary>
        ///A test for PDFSolidBrush Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFSolidBrushConstructor_Test3()
        {
            Color color = PDFColors.Aqua;
            PDFSolidBrush target = new PDFSolidBrush(color);

            Assert.IsNotNull(target);
            Assert.AreEqual(color, target.Color);
            Assert.AreEqual((PDFReal)1, target.Opacity);
            Assert.AreEqual(FillType.Solid, target.FillStyle);
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void Create_Test()
        {
            Color color = PDFColors.Aqua;
            PDFSolidBrush expected = new PDFSolidBrush(color);
            PDFSolidBrush actual;
            actual = PDFSolidBrush.Create(color);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Color, actual.Color);
            Assert.AreEqual(expected.Opacity, actual.Opacity);
            Assert.AreEqual(expected.FillStyle, actual.FillStyle);
        }

        

        /// <summary>
        ///A test for Color
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void Color_Test()
        {
            Color color = PDFColors.Aqua;
            PDFSolidBrush target = new PDFSolidBrush(color);
            Color expected = color;
            Color actual = target.Color;
            Assert.AreEqual(expected, actual);

            expected = PDFColors.White;
            target.Color = expected;
            actual = target.Color;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for FillStyle
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void FillStyle_Test()
        {
            Color color = PDFColors.Aqua;
            PDFSolidBrush target = new PDFSolidBrush(color);
            FillType expected = FillType.Solid;
            FillType actual;
            actual = target.FillStyle;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Opacity
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void Opacity_Test()
        {
            Color color = PDFColors.Aqua;
            PDFReal opacity = 1F; 
            PDFSolidBrush target = new PDFSolidBrush(color, opacity);
            Assert.AreEqual(opacity, target.Opacity);

            PDFReal expected = 0.4F;
            target.Opacity = expected;
            Assert.AreEqual(expected, target.Opacity);
        }
    }
}
