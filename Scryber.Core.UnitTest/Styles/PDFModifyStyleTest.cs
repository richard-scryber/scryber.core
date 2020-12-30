using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFModifyStyle and is intended
    ///to contain all PDFModifyStyle Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFModifyStyleTest
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
        public void Modify_ConstructorTest()
        {
            ModifyPageStyle target = new ModifyPageStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(StyleKeys.ModifyPageItemKey, target.ItemKey);
        }

        
        /// <summary>
        ///A test for PageStartIndex
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Modify_PageStartIndexTest()
        {
            ModifyPageStyle target = new ModifyPageStyle(); 
            Assert.AreEqual(0, target.PageStartIndex);

            target.PageStartIndex = 4;
            Assert.AreEqual(4, target.PageStartIndex);

            target.RemovePageStartIndex();
            Assert.AreEqual(0, target.PageStartIndex);
        }

        /// <summary>
        ///A test for PageCount
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Modify_PageCountTest()
        {
            ModifyPageStyle target = new ModifyPageStyle();
            Assert.AreEqual(1, target.PageCount);

            target.PageCount = 4;
            Assert.AreEqual(4, target.PageCount);

            target.RemovePageCount();
            Assert.AreEqual(1, target.PageCount);
        }

        /// <summary>
        ///A test for RowRepeat
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Modify_ModificationTypeTest()
        {
            ModifyPageStyle target = new ModifyPageStyle();
            Assert.AreEqual(ModificationType.None, target.ModificationType);

            target.ModificationType = ModificationType.Append;
            Assert.AreEqual(ModificationType.Append, target.ModificationType);

            target.ModificationType = ModificationType.Update;
            Assert.AreEqual(ModificationType.Update, target.ModificationType);

            target.RemoveModificationType();
            Assert.AreEqual(ModificationType.None, target.ModificationType);
        }

        /// <summary>
        ///A test for RowRepeat
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void Modify_ContentRetentionTest()
        {
            ModifyPageStyle target = new ModifyPageStyle();
            Assert.AreEqual(ModifiedContentAction.OnTop, target.ContentAction);

            target.ContentAction = ModifiedContentAction.Replace;
            Assert.AreEqual(ModifiedContentAction.Replace, target.ContentAction);

            target.ContentAction = ModifiedContentAction.Underneath;
            Assert.AreEqual(ModifiedContentAction.Underneath, target.ContentAction);

            target.RemoveContentAction();
            Assert.AreEqual(ModifiedContentAction.OnTop, target.ContentAction);
        }
    }
}
