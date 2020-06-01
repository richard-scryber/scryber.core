using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Drawing;
using Scryber.Native;
using Scryber;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFFillStyleTest and is intended
    ///to contain all PDFFillStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFFillStyleTest
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
        ///A test for PDFFillStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Fill_ConstructorTest()
        {
            PDFFillStyle target = new PDFFillStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(PDFStyleKeys.FillItemKey, target.ItemKey);
        }

        /// <summary>
        ///A test for CreateBrush
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Fill_CreateBrushTest()
        {
            PDFFillStyle target = new PDFFillStyle();
            target.FillStyle = FillStyle.Solid;
            target.Color = PDFColors.Red;
            PDFBrush expected = new PDFSolidBrush(PDFColors.Red);
            PDFBrush actual;
            actual = target.CreateBrush();
            Assert.AreEqual(expected.GetType(), actual.GetType());
            Assert.AreEqual(expected.FillStyle, actual.FillStyle);

            PDFSolidBrush solidexpected = (PDFSolidBrush)expected;
            PDFSolidBrush solidactual = (PDFSolidBrush)actual;
            Assert.AreEqual(solidexpected.Color, solidactual.Color);
            Assert.AreEqual(solidexpected.Opacity, solidactual.Opacity);

            target = new PDFFillStyle();
            target.FillStyle = FillStyle.Image;
            target.ImageSource = "../images/animage.png";
            target.PatternRepeat = PatternRepeat.RepeatBoth;
            target.PatternXPosition = 10;
            target.PatternXSize = 100;
            target.PatternXStep = 90;
            target.PatternYPosition = 20;
            target.PatternYSize = 200;
            target.PatternYStep = 180;
            

            expected = new PDFImageBrush("../images/animage.png");
            actual = target.CreateBrush();
            Assert.AreEqual(expected.GetType(), actual.GetType());
            Assert.AreEqual(expected.FillStyle, actual.FillStyle);

            PDFImageBrush imgexpected = (PDFImageBrush)expected;
            PDFImageBrush imgactual = (PDFImageBrush)actual;
            Assert.AreEqual(imgexpected.ImageSource, imgactual.ImageSource);
            Assert.AreEqual(imgexpected.Opacity, imgactual.Opacity);
            Assert.AreEqual(target.PatternXPosition, imgactual.XPostion);
            Assert.AreEqual(target.PatternXSize, imgactual.XSize);
            Assert.AreEqual(target.PatternXStep, imgactual.XStep);
            Assert.AreEqual(target.PatternYPosition, imgactual.YPostion);
            Assert.AreEqual(target.PatternYSize, imgactual.YSize);
            Assert.AreEqual(target.PatternYStep, imgactual.YStep);

            //check that the patterns are conformed to repeating
            
            target.PatternRepeat = PatternRepeat.RepeatX;
            imgactual = (PDFImageBrush)target.CreateBrush();
            Assert.AreEqual(imgactual.YStep, PDFFillStyle.NoYRepeatStepSize);

            target.PatternRepeat = PatternRepeat.RepeatY;
            imgactual = (PDFImageBrush)target.CreateBrush();
            Assert.AreEqual(imgactual.XStep, PDFFillStyle.NoXRepeatStepSize);

            target.PatternRepeat = PatternRepeat.None;
            imgactual = (PDFImageBrush)target.CreateBrush();
            Assert.AreEqual(imgactual.YStep, PDFFillStyle.NoYRepeatStepSize);
            Assert.AreEqual(imgactual.XStep, PDFFillStyle.NoXRepeatStepSize);
        }

        /// <summary>
        ///A test for Color
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Fill_ColorTest()
        {
            PDFFillStyle target = new PDFFillStyle();
            Assert.AreEqual(target.Color, PDFStyleConst.DefaultFillColor);

            target.Color = PDFColors.Red;
            Assert.AreEqual(target.Color, PDFColors.Red);

            target.Color = PDFColors.Blue;
            Assert.AreEqual(target.Color, PDFColors.Blue);

            target.RemoveColor();
            Assert.AreEqual(target.Color, PDFStyleConst.DefaultFillColor);

        }

        /// <summary>
        ///A test for FillStyle
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Fill_FillStyleTest()
        {
            PDFFillStyle target = new PDFFillStyle();
            Assert.AreEqual(target.FillStyle, FillStyle.None);

            target.FillStyle = FillStyle.Solid;
            Assert.AreEqual(target.FillStyle, FillStyle.Solid);

            target.FillStyle = FillStyle.Image;
            Assert.AreEqual(target.FillStyle, FillStyle.Image);

            target.RemoveFillStyle();
            Assert.AreEqual(target.FillStyle, FillStyle.None);

        }

        /// <summary>
        ///A test for ImageSource
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Fill_ImageSourceTest()
        {
            PDFFillStyle target = new PDFFillStyle();
            Assert.IsTrue(string.IsNullOrEmpty(target.ImageSource));

            string path = "../images/image.png";
            target.ImageSource = path;
            Assert.AreEqual(target.ImageSource, path);

            path = "../images/image2.png";
            target.ImageSource = path;
            Assert.AreEqual(target.ImageSource, path);

            target.RemoveImageSource();
            Assert.IsTrue(string.IsNullOrEmpty(target.ImageSource));
        }

        /// <summary>
        ///A test for Opacity and RemoveOpacity
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Fill_OpacityTest()
        {
            PDFFillStyle target = new PDFFillStyle(); 
            Assert.AreEqual(target.Opacity, (PDFReal)1.0);

            double opacity = 0.4;
            target.Opacity = opacity;
            Assert.AreEqual(target.Opacity, opacity);

            opacity = 0.8;
            target.Opacity = opacity;
            Assert.AreEqual(target.Opacity, opacity);

            target.RemoveOpacity();
            Assert.AreEqual(target.Opacity, (PDFReal)1.0);
        }

        /// <summary>
        ///A test for PatternRepeat
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Fill_PatternRepeatTest()
        {
            PDFFillStyle target = new PDFFillStyle();
            Assert.AreEqual(target.PatternRepeat, PatternRepeat.RepeatBoth); //RepeatBoth is the current default.

            target.PatternRepeat = PatternRepeat.RepeatX;
            Assert.AreEqual(target.PatternRepeat, PatternRepeat.RepeatX);

            target.PatternRepeat = PatternRepeat.None;
            Assert.AreEqual(target.PatternRepeat, PatternRepeat.None);

            target.RemovePatternRepeat();
            Assert.AreEqual(target.PatternRepeat, PatternRepeat.RepeatBoth);

        }

        /// <summary>
        ///A test for PatternXPosition
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Fill_PatternXPositionTest()
        {
            PDFFillStyle target = new PDFFillStyle();
            Assert.AreEqual(target.PatternXPosition, PDFUnit.Zero);

            PDFUnit expected = (PDFUnit)10;
            target.PatternXPosition = expected;
            Assert.AreEqual(target.PatternXPosition, expected);

            expected = -20;
            target.PatternXPosition = expected;
            Assert.AreEqual(target.PatternXPosition, expected);

            target.RemovePatternXPosition();
            Assert.AreEqual(target.PatternXPosition, PDFUnit.Zero);

            
        }

        /// <summary>
        ///A test for PatternXSize
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Fill_PatternXSizeTest()
        {
            PDFFillStyle target = new PDFFillStyle(); 
            Assert.AreEqual(target.PatternXSize, PDFUnit.Zero);

            PDFUnit expected = (PDFUnit)10;
            target.PatternXSize = expected;
            Assert.AreEqual(target.PatternXSize, expected);

            expected = -20;
            target.PatternXSize = expected;
            Assert.AreEqual(target.PatternXSize, expected);

            target.RemovePatternXSize();
            Assert.AreEqual(target.PatternXSize, PDFUnit.Zero);

        }

        /// <summary>
        ///A test for PatternXStep
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Fill_PatternXStepTest()
        {
            PDFFillStyle target = new PDFFillStyle();
            Assert.AreEqual(target.PatternXStep, PDFUnit.Zero);

            PDFUnit expected = (PDFUnit)10;
            target.PatternXStep = expected;
            Assert.AreEqual(target.PatternXStep, expected);

            expected = -20;
            target.PatternXStep = expected;
            Assert.AreEqual(target.PatternXStep, expected);

            target.RemovePatternXStep();
            Assert.AreEqual(target.PatternXStep, PDFUnit.Zero);

        }




        /// <summary>
        ///A test for PatternYPosition
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Fill_PatternYPositionTest()
        {
            PDFFillStyle target = new PDFFillStyle(); 
            Assert.AreEqual(target.PatternYPosition, PDFUnit.Zero);

            PDFUnit expected = (PDFUnit)10;
            target.PatternYPosition = expected;
            Assert.AreEqual(target.PatternYPosition, expected);

            expected = -20;
            target.PatternYPosition = expected;
            Assert.AreEqual(target.PatternYPosition, expected);

            target.RemovePatternYPosition();
            Assert.AreEqual(target.PatternYPosition, PDFUnit.Zero);

        }

        /// <summary>
        ///A test for PatternYSize
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Fill_PatternYSizeTest()
        {
            PDFFillStyle target = new PDFFillStyle();
            Assert.AreEqual(target.PatternYSize, PDFUnit.Zero);

            PDFUnit expected = (PDFUnit)10;
            target.PatternYSize = expected;
            Assert.AreEqual(target.PatternYSize, expected);

            expected = -20;
            target.PatternYSize = expected;
            Assert.AreEqual(target.PatternYSize, expected);

            target.RemovePatternYSize();
            Assert.AreEqual(target.PatternYSize, PDFUnit.Zero);

        }

        /// <summary>
        ///A test for PatternYStep
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Fill_PatternYStepTest()
        {
            PDFFillStyle target = new PDFFillStyle();
            Assert.AreEqual(target.PatternYStep, PDFUnit.Zero);

            PDFUnit expected = (PDFUnit)10;
            target.PatternYStep = expected;
            Assert.AreEqual(target.PatternYStep, expected);

            expected = -20;
            target.PatternYStep = expected;
            Assert.AreEqual(target.PatternYStep, expected);

            target.RemovePatternYStep();
            Assert.AreEqual(target.PatternYStep, PDFUnit.Zero);

        }

        
    }
}
