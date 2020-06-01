using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFTableStyleTest and is intended
    ///to contain all PDFTableStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFTableStyleTest
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
        ///A test for PDFTableStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Table_ConstructorTest()
        {
            PDFTableStyle target = new PDFTableStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(PDFStyleKeys.TableItemKey,target.ItemKey);
        }

        
        /// <summary>
        ///A test for CellColumnSpan
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Table_CellColumnSpanTest()
        {
            PDFTableStyle target = new PDFTableStyle(); 
            Assert.AreEqual(1, target.CellColumnSpan);

            target.CellColumnSpan = 4;
            Assert.AreEqual(4, target.CellColumnSpan);

            target.RemoveCellColumnSpan();
            Assert.AreEqual(1, target.CellColumnSpan);
        }

        /// <summary>
        ///A test for RowRepeat
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Table_RowRepeatTest()
        {
            PDFTableStyle target = new PDFTableStyle();
            Assert.AreEqual(TableRowRepeat.None, target.RowRepeat);

            target.RowRepeat = TableRowRepeat.RepeatAtTop;
            Assert.AreEqual(TableRowRepeat.RepeatAtTop, target.RowRepeat);

            target.RemoveRepatAtTop();
            Assert.AreEqual(TableRowRepeat.None, target.RowRepeat);
        }
    }
}
