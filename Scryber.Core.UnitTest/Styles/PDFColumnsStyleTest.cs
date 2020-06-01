using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFColumnsStyleTest and is intended
    ///to contain all PDFColumnsStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFColumnsStyleTest
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
        ///A test for PDFColumnsStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Columns_ConstructorTest()
        {
            PDFColumnsStyle target = new PDFColumnsStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(PDFStyleKeys.ColumnItemKey, target.ItemKey);
        }

        /// <summary>
        ///A test for AlleyWidth
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Columns_AlleyWidthTest()
        {
            PDFColumnsStyle target = new PDFColumnsStyle();
            Assert.AreEqual(PDFColumnsStyle.DefaultAlleyWidth, target.AlleyWidth);

            PDFUnit expected = PDFColumnsStyle.DefaultAlleyWidth + 20;
            target.AlleyWidth = expected;
            Assert.AreEqual(expected, target.AlleyWidth);

            expected += 20;
            target.AlleyWidth = expected;
            Assert.AreEqual(expected, target.AlleyWidth);

            target.RemoveAlleyWidth();
            Assert.AreEqual(PDFColumnsStyle.DefaultAlleyWidth, target.AlleyWidth);

            expected = PDFColumnsStyle.DefaultAlleyWidth;
            target.AlleyWidth = expected;
            Assert.AreEqual(expected, target.AlleyWidth);
        }

        /// <summary>
        ///A test for ColumnCount
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Columns_ColumnCountTest()
        {
            PDFColumnsStyle target = new PDFColumnsStyle();
            Assert.AreEqual(1, target.ColumnCount);

            int expected = 2;
            target.ColumnCount = expected;
            Assert.AreEqual(expected, target.ColumnCount);

            expected += 4;
            target.ColumnCount = expected;
            Assert.AreEqual(expected, target.ColumnCount);

            target.RemoveColumnCount();
            Assert.AreEqual(1, target.ColumnCount);

            expected = 1;
            target.ColumnCount = expected;
            Assert.AreEqual(expected, target.ColumnCount);
        }

        

        /// <summary>
        ///A test for AutoFlow
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Columns_AutoFlowTest()
        {
            PDFColumnsStyle target = new PDFColumnsStyle();
            Assert.AreEqual(true, target.AutoFlow);

            bool expected = true;
            target.AutoFlow = expected;
            Assert.AreEqual(expected, target.AutoFlow);

            expected = false;
            target.AutoFlow = expected;
            Assert.AreEqual(expected, target.AutoFlow);

            target.RemoveAutoFlow();
            Assert.AreEqual(true, target.AutoFlow);

            expected = true;
            target.AutoFlow = expected;
            Assert.AreEqual(expected, target.AutoFlow);
        }

    }
}
