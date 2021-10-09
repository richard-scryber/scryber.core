using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.PDF.Native;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFPen_Test and is intended
    ///to contain all PDFPen_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFPen_Test
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
        ///A test for Create
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void Create_Test()
        {
            Color color = PDFColors.Aqua;
            PDFUnit width = 1;
            PDFSolidPen expected = new PDFSolidPen();
            expected.Color = color;
            expected.Width = width;

            PDFPen actual;
            actual = PDFPen.Create(color, width);
            Assert.IsInstanceOfType(actual, typeof(PDFSolidPen));
            PDFSolidPen solid = (PDFSolidPen)actual;

            Assert.AreEqual(expected.Width, solid.Width);
            Assert.AreEqual(expected.Color, solid.Color);

            
        }

        

        internal virtual PDFPen CreatePDFPen()
        {
            // TODO: Instantiate an appropriate concrete class.
            PDFPen target = null;
            return target;
        }


        /// <summary>
        ///A test for LineCaps
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void LineCaps_Test()
        {
            PDFPen target = PDFPen.Create(PDFColors.Aqua, 1);
            LineCaps expected = LineCaps.Butt;
            LineCaps actual = target.LineCaps;
            Assert.AreEqual(expected, actual);

            target.LineCaps = LineCaps.Projecting;
            actual = target.LineCaps;
            Assert.AreEqual(actual,LineCaps.Projecting);
           
        }

        /// <summary>
        ///A test for LineJoin
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void LineJoin_Test()
        {
            PDFPen target = PDFPen.Create(PDFColors.Aqua, 1);

            LineJoin expected = LineJoin.Mitre;
            LineJoin actual = target.LineJoin;
            Assert.AreEqual(expected, actual);

            target.LineJoin = LineJoin.Round;
            Assert.AreEqual(LineJoin.Round, target.LineJoin);
            
        }

        /// <summary>
        ///A test for LineStyle
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void LineStyle_Test()
        {
            PDFPen target = PDFPen.Create(PDFColors.Aqua, 1);
            
            LineType actual;
            actual = target.LineStyle;
            Assert.AreEqual(LineType.Solid, actual);
        }

        /// <summary>
        ///A test for MitreLimit
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void MitreLimit_Test()
        {
            PDFPen target = PDFPen.Create(PDFColors.Aqua, 1);
            float expected = 0F;
            float actual = target.MitreLimit;
            Assert.AreEqual(expected, actual);

            expected = 130.5F;
            target.MitreLimit = expected;
            actual = target.MitreLimit;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Opacity
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void Opacity_Test()
        {
            PDFPen target = PDFPen.Create(PDFColors.Aqua, 1);
            PDFReal expected = -1;
            PDFReal actual = target.Opacity;
            Assert.AreEqual(expected, actual);

            expected = 0.5;
            target.Opacity = expected;
            actual = target.Opacity;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Width
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void Width_Test()
        {
            PDFUnit expected = 1;
            PDFPen target = PDFPen.Create(PDFColors.Aqua, expected);

            PDFUnit actual = target.Width;
            Assert.AreEqual(expected, actual);

            expected = new PDFUnit(4, PageUnits.Millimeters);
            target.Width = expected;
            actual = target.Width;
            Assert.AreEqual(expected, actual);
           
        }
    }
}
