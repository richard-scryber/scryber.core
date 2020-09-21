using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFListStyleTest and is intended
    ///to contain all PDFListStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFListStyleTest
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
        ///A test for PDFListStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void List_ConstructorTest()
        {
            ListStyle target = new ListStyle();
            Assert.IsNotNull(target);
            Assert.AreEqual(StyleKeys.ListItemKey, target.ItemKey);
        }


        [TestMethod()]
        [TestCategory("Style Values")]
        public void List_NumberingStyle()
        {
            ListStyle target = new ListStyle();
            ListNumberingGroupStyle expected = ListNumberingGroupStyle.None;
            ListNumberingGroupStyle actual = target.NumberingStyle;
            Assert.AreEqual(expected, actual);

            expected = ListNumberingGroupStyle.Decimals;
            target.NumberingStyle = expected;
            actual = target.NumberingStyle;
            Assert.AreEqual(expected, actual);

            expected = ListNumberingGroupStyle.LowercaseRoman;
            target.NumberingStyle = expected;
            actual = target.NumberingStyle;
            Assert.AreEqual(expected, actual);

            target.RemoveNumberingStyle();
            expected = ListNumberingGroupStyle.None;
            actual = target.NumberingStyle;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void List_NumberingGroup()
        {
            ListStyle target = new ListStyle();
            string expected = "";
            string actual = target.NumberingGroup;
            Assert.AreEqual(expected, actual);

            expected = "list-group";
            target.NumberingGroup = expected;
            actual = target.NumberingGroup;
            Assert.AreEqual(expected, actual);

            expected = "new-group";
            target.NumberingGroup = expected;
            actual = target.NumberingGroup;
            Assert.AreEqual(expected, actual);

            target.RemoveNumberingGroup();
            expected = "";
            actual = target.NumberingGroup;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void List_NumberInset()
        {
            ListStyle target = new ListStyle();
            PDFUnit expected = PDFStyleConst.DefaultListNumberInset;
            PDFUnit actual = target.NumberInset;
            Assert.AreEqual(expected, actual);

            expected = 10;
            target.NumberInset = expected;
            actual = target.NumberInset;
            Assert.AreEqual(expected, actual);

            expected = 20;
            target.NumberInset = expected;
            actual = target.NumberInset;
            Assert.AreEqual(expected, actual);

            target.RemoveNumberInset();
            expected = PDFStyleConst.DefaultListNumberInset;
            actual = target.NumberInset;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void List_NumberPrefix()
        {
            ListStyle target = new ListStyle();
            string expected = "";
            string actual = target.NumberPrefix;
            Assert.AreEqual(expected, actual);

            expected = "list-prefix";
            target.NumberPrefix = expected;
            actual = target.NumberPrefix;
            Assert.AreEqual(expected, actual);

            expected = "new-prefix";
            target.NumberPrefix = expected;
            actual = target.NumberPrefix;
            Assert.AreEqual(expected, actual);

            target.RemoveNumberPrefix();
            expected = "";
            actual = target.NumberPrefix;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void List_NumberPostfix()
        {
            ListStyle target = new ListStyle();
            string expected = "";
            string actual = target.NumberPostfix;
            Assert.AreEqual(expected, actual);

            expected = "postfix";
            target.NumberPostfix = expected;
            actual = target.NumberPostfix;
            Assert.AreEqual(expected, actual);

            expected = "new-postfix";
            target.NumberPostfix = expected;
            actual = target.NumberPostfix;
            Assert.AreEqual(expected, actual);

            target.RemoveNumberPostfix();
            expected = "";
            actual = target.NumberPostfix;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void List_HorizontalAlignmentStyle()
        {
            ListStyle target = new ListStyle();
            HorizontalAlignment expected = HorizontalAlignment.Right;
            HorizontalAlignment actual = target.NumberAlignment;
            Assert.AreEqual(expected, actual);

            expected = HorizontalAlignment.Center;
            target.NumberAlignment = expected;
            actual = target.NumberAlignment;
            Assert.AreEqual(expected, actual);

            expected = HorizontalAlignment.Right;
            target.NumberAlignment = expected;
            actual = target.NumberAlignment;
            Assert.AreEqual(expected, actual);

            target.RemoveNumberAlignment();
            expected = HorizontalAlignment.Right;
            actual = target.NumberAlignment;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void List_Concatenate()
        {
            ListStyle target = new ListStyle();
            bool expected = false;
            bool actual = target.ConcatenateWithParent;
            Assert.AreEqual(expected, actual);

            expected = true;
            target.ConcatenateWithParent = expected;
            actual = target.ConcatenateWithParent;
            Assert.AreEqual(expected, actual);

            expected = false;
            target.ConcatenateWithParent = expected;
            actual = target.ConcatenateWithParent;
            Assert.AreEqual(expected, actual);

            target.RemoveConcatenateWithParent();
            expected = false;
            actual = target.ConcatenateWithParent;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void List_ItemLabel()
        {
            ListStyle target = new ListStyle();
            string expected = "";
            string actual = target.ItemLabel;
            Assert.AreEqual(expected, actual);

            expected = "label 1";
            target.ItemLabel = expected;
            actual = target.ItemLabel;
            Assert.AreEqual(expected, actual);

            expected = "label 2";
            target.ItemLabel = expected;
            actual = target.ItemLabel;
            Assert.AreEqual(expected, actual);

            target.RemoveItemLabel();
            expected = "";
            actual = target.ItemLabel;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void List_AllProperties()
        {
            ListStyle target = new ListStyle();
            target.NumberingStyle = ListNumberingGroupStyle.LowercaseRoman;
            target.NumberingGroup = "all-group";
            target.NumberInset = 40;
            target.NumberPrefix = "prefix";
            target.NumberPostfix = "postfix";
            target.NumberAlignment = HorizontalAlignment.Center;
            target.ConcatenateWithParent = true;
            target.ItemLabel = "label";

            Assert.AreEqual(ListNumberingGroupStyle.LowercaseRoman, target.NumberingStyle);
            Assert.AreEqual("all-group", target.NumberingGroup);
            Assert.AreEqual((PDFUnit)40, target.NumberInset);
            Assert.AreEqual("prefix", target.NumberPrefix);
            Assert.AreEqual("postfix", target.NumberPostfix);
            Assert.AreEqual(HorizontalAlignment.Center, target.NumberAlignment);
            Assert.AreEqual(true, target.ConcatenateWithParent);
            Assert.AreEqual("label", target.ItemLabel);
        }

    }
}
