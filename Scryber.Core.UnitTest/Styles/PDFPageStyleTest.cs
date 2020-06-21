using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFPageStyleTest and is intended
    ///to contain all PDFPageStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFPageStyleTest
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
        ///A test for PDFPageStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_ConstructorTest()
        {
            PDFPageStyle target = new PDFPageStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(PDFStyleKeys.PageItemKey,target.ItemKey);
        }

        /// <summary>
        ///A test for CreatePageSize
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_CreatePageSizeTest()
        {
            //default

            PaperSize size = PDFStyleConst.DefaultPaperSize;
            PaperOrientation orientation = PDFStyleConst.DefaultPaperOrientation;

            PDFPageStyle target = new PDFPageStyle(); //Empty
            PDFPageSize expected = new PDFPageSize(size, orientation);
            
            PDFPageSize actual = target.CreatePageSize();
            AssertPageSizeAreEqual(expected, actual);

            //papers

            size = PaperSize.Tabloid;
            orientation = PaperOrientation.Landscape;
            expected = new PDFPageSize(size, orientation);
            target.PaperSize = size;
            target.PaperOrientation = orientation;
            
            actual = target.CreatePageSize();
            AssertPageSizeAreEqual(expected, actual);

            size = PaperSize.A4;
            orientation = PaperOrientation.Portrait;
            target.PaperSize = size;
            target.PaperOrientation = orientation;
            expected = new PDFPageSize(size, orientation);
            actual = target.CreatePageSize();

            AssertPageSizeAreEqual(expected, actual);


            // explicit sizes

            target = new PDFPageStyle();
            PDFUnit w = 200;
            PDFUnit h = 500;
            target.Width = w;
            target.Height = h;

            expected = new PDFPageSize(new PDFSize(w, h));
            actual = target.CreatePageSize();
            AssertPageSizeAreEqual(expected, actual);


            // size overides paper

            target = new PDFPageStyle();
            w = 300;
            h = 600;
            size = PaperSize.A8;
            orientation = PaperOrientation.Landscape;

            target.Width = w;
            target.Height = h;
            target.PaperSize = size;
            target.PaperOrientation = orientation;


            expected = new PDFPageSize(new PDFSize(w, h));
            actual = target.CreatePageSize();

            AssertPageSizeAreEqual(expected, actual);

        }

        private static void AssertPageSizeAreEqual(PDFPageSize expected, PDFPageSize actual)
        {
            Assert.AreEqual(expected.Width, actual.Width);
            Assert.AreEqual(expected.Height, actual.Height);
            Assert.AreEqual(expected.Size, actual.Size);
            Assert.AreEqual(expected.PaperSize, actual.PaperSize);
            Assert.AreEqual(expected.Orientation, actual.Orientation);
        }

        

        /// <summary>
        ///A test for Height
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_HeightTest()
        {
            PDFPageStyle target = new PDFPageStyle();
            PDFUnit expected = PDFUnit.Zero;

            Assert.AreEqual(expected, target.Height);

            expected = 20;
            target.Height = expected;
            Assert.AreEqual(expected, target.Height);

            expected = 40;
            target.Height = expected;
            Assert.AreEqual(expected, target.Height);

            expected = PDFUnit.Zero;
            target.RemoveHeight();
            Assert.AreEqual(expected, target.Height);

            target.Height = expected;
            Assert.AreEqual(expected, target.Height);
            
        }

        /// <summary>
        ///A test for NumberPrefix
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_NumberPrefixTest()
        {
            PDFPageStyle target = new PDFPageStyle();
            string expected = String.Empty;

            Assert.AreEqual(expected, target.NumberPrefix);

            expected = "Page ";
            target.NumberPrefix = expected;
            Assert.AreEqual(expected, target.NumberPrefix);

            expected = "Another ";
            target.NumberPrefix = expected;
            Assert.AreEqual(expected, target.NumberPrefix);

            expected = String.Empty;
            target.RemoveNumberPrefix();
            Assert.AreEqual(expected, target.NumberPrefix);

            target.NumberPrefix = expected;
            Assert.AreEqual(expected, target.NumberPrefix);
        }

        /// <summary>
        ///A test for NumberFormat
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_NumberFormatTest()
        {
            PDFPageStyle target = new PDFPageStyle();
            string expected = String.Empty;

            Assert.AreEqual(expected, target.PageNumberFormat);

            expected = "Page {0}";
            target.PageNumberFormat = expected;
            Assert.AreEqual(expected, target.PageNumberFormat);

            expected = "Another Page {0}";
            target.PageNumberFormat = expected;
            Assert.AreEqual(expected, target.PageNumberFormat);

            expected = String.Empty;
            target.RemovePageNumberFormat();
            Assert.AreEqual(expected, target.PageNumberFormat);

            target.PageNumberFormat = expected;
            Assert.AreEqual(expected, target.PageNumberFormat);
        }


        /// <summary>
        ///A test for NumberStartIndex
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_NumberStartIndexTest()
        {
            PDFPageStyle target = new PDFPageStyle();
            int expected = 1;

            Assert.AreEqual(expected, target.NumberStartIndex);

            expected = 15;
            target.NumberStartIndex = expected;
            Assert.AreEqual(expected, target.NumberStartIndex);

            expected = 20;
            target.NumberStartIndex = expected;
            Assert.AreEqual(expected, target.NumberStartIndex);

            expected = 1;
            target.RemoveNumberStartIndex();
            Assert.AreEqual(expected, target.NumberStartIndex);

            target.NumberStartIndex = expected;
            Assert.AreEqual(expected, target.NumberStartIndex);
        }

        /// <summary>
        ///A test for NumberStyle
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_NumberStyleTest()
        {
            PDFPageStyle target = new PDFPageStyle();
            PageNumberStyle expected = PageNumberStyle.Decimals;

            Assert.AreEqual(expected, target.NumberStyle);

            expected = PageNumberStyle.LowercaseRoman;
            target.NumberStyle = expected;
            Assert.AreEqual(expected, target.NumberStyle);

            expected = PageNumberStyle.UppercaseLetters;
            target.NumberStyle = expected;
            Assert.AreEqual(expected, target.NumberStyle);

            expected = PageNumberStyle.Decimals;
            target.RemoveNumberStyle();
            Assert.AreEqual(expected, target.NumberStyle);

            target.NumberStyle = expected;
            Assert.AreEqual(expected, target.NumberStyle);
        }

        /// <summary>
        ///A test for PaperOrientation
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_PaperOrientationTest()
        {
            PDFPageStyle target = new PDFPageStyle();
            PaperOrientation expected = PaperOrientation.Portrait;

            Assert.AreEqual(expected, target.PaperOrientation);

            expected = PaperOrientation.Landscape;
            target.PaperOrientation = expected;
            Assert.AreEqual(expected, target.PaperOrientation);

            expected = PaperOrientation.Portrait;
            target.PaperOrientation = expected;
            Assert.AreEqual(expected, target.PaperOrientation);

            expected = PaperOrientation.Portrait;
            target.RemovePaperOrientation();
            Assert.AreEqual(expected, target.PaperOrientation);

           
        }

        /// <summary>
        ///A test for PaperSize
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_PaperSizeTest()
        {
            PDFPageStyle target = new PDFPageStyle();
            PaperSize expected = PaperSize.A4;

            Assert.AreEqual(expected, target.PaperSize);

            expected = PaperSize.C0;
            target.PaperSize = expected;
            Assert.AreEqual(expected, target.PaperSize);

            expected = PaperSize.Tabloid;
            target.PaperSize = expected;
            Assert.AreEqual(expected, target.PaperSize);

            expected = PaperSize.A4;
            target.RemovePaperSize();
            Assert.AreEqual(expected, target.PaperSize);
        }

        /// <summary>
        ///A test for Width
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Page_WidthTest()
        {
            PDFPageStyle target = new PDFPageStyle();
            PDFUnit expected = PDFUnit.Zero;

            Assert.AreEqual(expected, target.Width);

            expected = 20;
            target.Width = expected;
            Assert.AreEqual(expected, target.Width);

            expected = 40;
            target.Width = expected;
            Assert.AreEqual(expected, target.Width);

            expected = PDFUnit.Zero;
            target.RemoveWidth();
            Assert.AreEqual(expected, target.Width);

            target.Width = expected;
            Assert.AreEqual(expected, target.Width);
        }

        
    }
}
