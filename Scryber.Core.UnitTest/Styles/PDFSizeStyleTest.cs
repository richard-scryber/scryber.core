using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFPositionStyleTest and is intended
    ///to contain all PDFPositionStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFSizeStyleTest
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

        // individual property tests

        #region public void Size_ConstructorTest()

        /// <summary>
        ///A test for PDFSizeStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Size_ConstructorTest()
        {
            PDFSizeStyle target = new PDFSizeStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(PDFStyleKeys.SizeItemKey, target.ItemKey);
        }

        #endregion

        #region public void Size_FullWidthTest()

        /// <summary>
        ///A test for FullWidth
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Size_FullWidthTest()
        {
            PDFSizeStyle target = new PDFSizeStyle();
            bool expected = false;
            Assert.AreEqual(expected, target.FullWidth);

            expected = true;
            target.FullWidth = expected;
            bool actual = target.FullWidth;
            Assert.AreEqual(expected, actual);

            expected = false;
            target.FullWidth = expected;
            actual = target.FullWidth;
            Assert.AreEqual(expected, actual);

            expected = false;
            target.RemoveFillWidth();
            Assert.AreEqual(expected, actual);
        }


        #endregion

        #region public void Size_HeightTest()

        /// <summary>
        ///A test for Height
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Size_HeightTest()
        {
            PDFSizeStyle target = new PDFSizeStyle();

            // Default value

            PDFUnit expected = PDFUnit.Empty;
            PDFUnit actual = target.Height;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20;
            target.Height = expected;
            actual = target.Height;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = new PDFUnit(120, PageUnits.Millimeters);
            target.Height = expected;
            actual = target.Height;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = PDFUnit.Empty;
            target.RemoveHeight();
            actual = target.Height;
            Assert.AreEqual(expected, actual);
        }


        #endregion

        #region public void Size_MaximumWidthTest()

        /// <summary>
        ///A test for MaximumWidth
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Size_MaximumWidthTest()
        {
            PDFSizeStyle target = new PDFSizeStyle();

            // Default value

            PDFUnit expected = PDFUnit.Empty;
            PDFUnit actual = target.MaximumWidth;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20;
            target.MaximumWidth = expected;
            actual = target.MaximumWidth;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = new PDFUnit(120, PageUnits.Millimeters);
            target.MaximumWidth = expected;
            actual = target.MaximumWidth;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = PDFUnit.Empty;
            target.RemoveMaximumWidth();
            actual = target.MaximumWidth;
            Assert.AreEqual(expected, actual);
        }


        #endregion

        #region public void Size_MaximumHeightTest()

        /// <summary>
        ///A test for MaximumHeight
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Size_MaximumHeightTest()
        {
            PDFSizeStyle target = new PDFSizeStyle();

            // Default value

            PDFUnit expected = PDFUnit.Empty;
            PDFUnit actual = target.MaximumHeight;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20;
            target.MaximumHeight = expected;
            actual = target.MaximumHeight;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = new PDFUnit(120, PageUnits.Millimeters);
            target.MaximumHeight = expected;
            actual = target.MaximumHeight;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = PDFUnit.Empty;
            target.RemoveMaximumHeight();
            actual = target.MaximumHeight;
            Assert.AreEqual(expected, actual);
        }


        #endregion

        #region public void Size_MinimumWidthTest()

        /// <summary>
        ///A test for MinimumWidth
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Size_MinimumWidthTest()
        {
            PDFSizeStyle target = new PDFSizeStyle();

            // Default value

            PDFUnit expected = PDFUnit.Empty;
            PDFUnit actual = target.MinimumWidth;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20;
            target.MinimumWidth = expected;
            actual = target.MinimumWidth;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = new PDFUnit(120, PageUnits.Millimeters);
            target.MinimumWidth = expected;
            actual = target.MinimumWidth;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = PDFUnit.Empty;
            target.RemoveMinimumWidth();
            actual = target.MinimumWidth;
            Assert.AreEqual(expected, actual);
        }


        #endregion

        #region public void Size_MinimumHeightTest()

        /// <summary>
        ///A test for MinimumHeight
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Size_MinimumHeightTest()
        {
            PDFSizeStyle target = new PDFSizeStyle();

            // Default value

            PDFUnit expected = PDFUnit.Empty;
            PDFUnit actual = target.MinimumHeight;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20;
            target.MinimumHeight = expected;
            actual = target.MinimumHeight;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = new PDFUnit(120, PageUnits.Millimeters);
            target.MinimumHeight = expected;
            actual = target.MinimumHeight;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = PDFUnit.Empty;
            target.RemoveMinimumHeight();
            actual = target.MinimumHeight;
            Assert.AreEqual(expected, actual);
        }


        #endregion

        #region public void Size_WidthTest()

        /// <summary>
        ///A test for Width
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Size_WidthTest()
        {
            PDFSizeStyle target = new PDFSizeStyle();

            // Default value

            PDFUnit expected = PDFUnit.Empty;
            PDFUnit actual = target.Width;
            Assert.AreEqual(expected, actual);

            // Set Value

            expected = 20;
            target.Width = expected;
            actual = target.Width;
            Assert.AreEqual(expected, actual);

            // Change Value

            expected = new PDFUnit(120, PageUnits.Millimeters);
            target.Width = expected;
            actual = target.Width;
            Assert.AreEqual(expected, actual);

            // Remove Value

            expected = PDFUnit.Empty;
            target.RemoveWidth();
            actual = target.Width;
            Assert.AreEqual(expected, actual);
        }


        #endregion

    }
}
