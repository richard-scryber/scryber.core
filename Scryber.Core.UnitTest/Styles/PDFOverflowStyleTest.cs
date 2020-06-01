using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFOverflowStyleTest and is intended
    ///to contain all PDFOverflowStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFOverflowStyleTest
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
        ///A test for PDFOverflowStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Overflow_ConstructorTest()
        {
            PDFOverflowStyle target = new PDFOverflowStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(PDFStyleKeys.OverflowItemKey,target.ItemKey);
        }

        /// <summary>
        ///A test for Action
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Overflow_ActionTest()
        {
            PDFOverflowStyle target = new PDFOverflowStyle(); 
            OverflowAction expected = OverflowAction.None;
            Assert.AreEqual(expected, target.Action);

            expected = OverflowAction.NewPage;
            target.Action = expected;
            Assert.AreEqual(expected, target.Action);

            expected = OverflowAction.Truncate;
            target.Action = expected;
            Assert.AreEqual(expected, target.Action);
            
            target.RemoveAction();
            expected = OverflowAction.None;
            Assert.AreEqual(expected, target.Action);
        }

        /// <summary>
        ///A test for Split
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Overflow_SplitTest()
        {
            PDFOverflowStyle target = new PDFOverflowStyle(); 
            OverflowSplit expected = OverflowSplit.Any;
            Assert.AreEqual(expected, target.Split);

            expected = OverflowSplit.Never;
            target.Split = expected;
            Assert.AreEqual(expected, target.Split);

            expected = OverflowSplit.Any;
            target.Split = expected;
            Assert.AreEqual(expected, target.Split);

            target.RemoveSplit();
            expected = OverflowSplit.Any;
            Assert.AreEqual(expected, target.Split);
        }

        
    }
}
