using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.PDF.Graphics;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFStrokeStyleTest and is intended
    ///to contain all PDFStrokeStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFStrokeStyleTest
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
        ///A test for PDFStrokeStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Stroke_ConstructorTest()
        {
            StrokeStyle target = new StrokeStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(StyleKeys.StrokeItemKey, target.ItemKey);
        }

        /// <summary>
        ///A test for CreatePen
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Stroke_CreatePenTest()
        {
            StrokeStyle target = new StrokeStyle();

            //No values

            PDFPen expected = null;
            PDFPen actual = target.CreatePen();
            Assert.IsNull(actual);

            //Zero width

            target.Width = Unit.Empty;
            actual = target.CreatePen();
            Assert.IsNull(actual);

            //Solid pen

            expected = new PDFSolidPen(StandardColors.Purple, 10);
            target.Color = StandardColors.Purple;
            target.Width = 10;
            actual = target.CreatePen();
            AssertPensAreEqual(expected, actual);

            //Mitres

            target.Mitre = 20;
            expected.MitreLimit = 20;
            actual = target.CreatePen();
            AssertPensAreEqual(expected, actual);

            //Line Caps

            target.LineCap = LineCaps.Projecting;
            expected.LineCaps = LineCaps.Projecting;
            actual = target.CreatePen();
            AssertPensAreEqual(expected, actual);

            //Line Join

            target.LineJoin = LineJoin.Bevel;
            expected.LineJoin = LineJoin.Bevel;
            actual = target.CreatePen();
            AssertPensAreEqual(expected, actual);

            // Opacity

            target.Opacity = 0.4;
            expected.Opacity = 0.4;
            actual = target.CreatePen();
            AssertPensAreEqual(expected, actual);

            // Dash

            Dash dash = new Dash(new int[] { 4, 5, 6 }, 10);
            expected = new PDFDashPen(dash);
            ((PDFSolidPen)expected).Color = StandardColors.Lime;
            expected.Width = 8;

            target = new StrokeStyle();
            target.Dash = dash;
            target.Width = 8;
            target.Color = StandardColors.Lime;
            actual = target.CreatePen();
            AssertPensAreEqual(expected, actual);
        }

        private void AssertPensAreEqual(PDFPen expected, PDFPen actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);
            if (expected is PDFNoPen)
            {
                Assert.IsInstanceOfType(actual, typeof(PDFNoPen));
            }
            else
            {
                if (expected is PDFDashPen)
                {
                    Assert.IsInstanceOfType(actual, typeof(PDFSolidPen));
                    PDFDashPen expdash = (PDFDashPen)expected;
                    PDFDashPen actdash = (PDFDashPen)actual;
                    Assert.AreEqual(expdash.Dash, actdash.Dash);
                }
                if (expected is PDFSolidPen)
                {
                    Assert.IsInstanceOfType(actual, typeof(PDFSolidPen));
                    PDFSolidPen expSolid = (PDFSolidPen)expected;
                    PDFSolidPen actSolid = (PDFSolidPen)actual;
                    Assert.AreEqual(expSolid.Color, actSolid.Color);
                }
                Assert.AreEqual(expected.LineStyle, actual.LineStyle);
                Assert.AreEqual(expected.Width, actual.Width);
                Assert.AreEqual(expected.MitreLimit, actual.MitreLimit);
                Assert.AreEqual(expected.LineCaps, actual.LineCaps);
                Assert.AreEqual(expected.LineJoin, actual.LineJoin);
                Assert.AreEqual(expected.Opacity, actual.Opacity);
            }
            
        }


        /// <summary>
        ///A test for Color
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Style_ColorTest()
        {
            StrokeStyle target = new StrokeStyle();
            Assert.AreEqual(target.Color, StandardColors.Transparent);

            target.Color = StandardColors.Red;
            Assert.AreEqual(target.Color, StandardColors.Red);

            target.Color = StandardColors.Blue;
            Assert.AreEqual(target.Color, StandardColors.Blue);

            target.RemoveColor();
            Assert.AreEqual(target.Color, StandardColors.Transparent);
        }

        /// <summary>
        ///A test for Dash
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Stroke_DashTest()
        {
            StrokeStyle target = new StrokeStyle();
            
            //Default

            Dash expected = Dash.None;
            Dash actual = target.Dash;
            Assert.AreEqual(expected, actual);

            // Set value

            expected = new Dash(new int[] { 1, 2, 3 }, 7);
            target.Dash = expected;
            actual = target.Dash;
            Assert.AreEqual(expected, actual);

            //Change Value

            expected = new Dash(new int[] { 2, 3, 4 }, 10);
            target.Dash = expected;
            actual = target.Dash;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = Dash.None;
            target.RemoveDash();
            actual = target.Dash;
            Assert.AreEqual(expected, actual);

            
        }

        /// <summary>
        ///A test for LineCap
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Stroke_LineCapTest()
        {
            StrokeStyle target = new StrokeStyle();

            //Default 

            LineCaps expected = PDFStyleConst.DefaultLineCaps;
            Assert.AreEqual(expected, target.LineCap);

            //Set value

            expected = LineCaps.Projecting;
            LineCaps actual;
            target.LineCap = expected;
            actual = target.LineCap;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = LineCaps.Round;
            target.LineCap = expected;
            actual = target.LineCap;
            Assert.AreEqual(expected, actual);

            //Remove value

            expected = PDFStyleConst.DefaultLineCaps;
            target.RemoveLineCap();
            actual = target.LineCap;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for LineJoin
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Stroke_LineJoinTest()
        {
            StrokeStyle target = new StrokeStyle();

            //Default 

            LineJoin expected = PDFStyleConst.DefaultLineJoin;
            Assert.AreEqual(expected, target.LineJoin);

            //Set value

            expected = LineJoin.Mitre;
            LineJoin actual;
            target.LineJoin = expected;
            actual = target.LineJoin;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = LineJoin.Round;
            target.LineJoin = expected;
            actual = target.LineJoin;
            Assert.AreEqual(expected, actual);

            //Remove value

            expected = PDFStyleConst.DefaultLineJoin;
            target.RemoveLineJoin();
            actual = target.LineJoin;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for LineStyle
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Stroke_LineStyleTest()
        {
            StrokeStyle target = new StrokeStyle();
            
            //Default 

            LineType expected = LineType.None;
            Assert.AreEqual(expected, target.LineStyle);

            //With color - should be solid
            target.Color = StandardColors.Red;
            expected = LineType.Solid;
            Assert.AreEqual(expected, target.LineStyle);

            //With dash - should be dashed
            target.Dash = new Dash(new int[] { 2, 3, 4 }, 10);
            expected = LineType.Dash;
            Assert.AreEqual(expected, target.LineStyle);
            

            //Set value (should override the dash and color options)

            expected = LineType.Solid;
            LineType actual;
            target.LineStyle = expected;
            actual = target.LineStyle;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = LineType.None;
            target.LineStyle = expected;
            actual = target.LineStyle;
            Assert.AreEqual(expected, actual);

            //Remove value

            expected = LineType.None;
            target.RemoveLineStyle();
            target.RemoveDash();
            target.RemoveColor();
            actual = target.LineStyle;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Mitre
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Stroke_MitreTest()
        {
            StrokeStyle target = new StrokeStyle();

            // Default value

            float expected = 0.0F;
            float actual = target.Mitre;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20.6F;
            target.Mitre = expected;
            actual = target.Mitre;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = 30.6F;
            target.Mitre = expected;
            actual = target.Mitre;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = 0.0F;
            target.RemoveMitre();
            actual = target.Mitre;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Opacity
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Stroke_OpacityTest()
        {
            StrokeStyle target = new StrokeStyle();

            // Default value

            double expected = 1.0F;
            double actual = target.Opacity;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 0.6;
            target.Opacity = expected;
            actual = target.Opacity;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = 0.034;
            target.Opacity = expected;
            actual = target.Opacity;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = 1.0F;
            target.RemoveOpacity();
            actual = target.Opacity;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Width
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Stroke_WidthTest()
        {
            StrokeStyle target = new StrokeStyle();
            // Default value

            Unit expected = Unit.Empty;
            Unit actual = target.Width;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20;
            target.Width = expected;
            actual = target.Width;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = new Unit(120, PageUnits.Millimeters);
            target.Width = expected;
            actual = target.Width;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = Unit.Empty;
            target.RemoveWidth();
            actual = target.Width;
            Assert.AreEqual(expected, actual);
        }
    }
}
