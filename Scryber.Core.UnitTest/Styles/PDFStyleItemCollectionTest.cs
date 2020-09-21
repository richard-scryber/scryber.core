using Scryber.Styles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using Scryber;
using Scryber.Drawing;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFStyleItemCollectionTest and is intended
    ///to contain all PDFStyleItemCollectionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFStyleItemCollectionTest
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

        private int StdCollectionCount = 3;

        private StyleItemCollection GetStdCollection()
        {
            Style style = new Style();
            StyleItemCollection col = new StyleItemCollection(style);
            BackgroundStyle bg = new BackgroundStyle();
            bg.Color = PDFColors.Red;
            col.Add(bg);

            BorderStyle bor = new BorderStyle();
            bor.Color = PDFColors.Green;
            col.Add(bor);

            Scryber.Styles.FontStyle fnt = new Scryber.Styles.FontStyle();
            fnt.FontFamily = "Symbol";
            col.Add(fnt);

            return col;
        }
        /// <summary>
        ///A test for PDFStyleItemCollection Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void PDFStyleItemCollectionConstructorTest()
        {
            Style style = new Style();
            StyleItemCollection target = new StyleItemCollection(style);
            Assert.IsNotNull(target);
            Assert.AreEqual(0, target.Count);
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void AddTest()
        {
            Style style = new Style();
            StyleItemCollection target = new StyleItemCollection(style);

            StyleItemBase item = new BorderStyle();
            target.Add(item);

            Assert.AreEqual(1, target.Count);

            item = new Scryber.Styles.FillStyle();
            target.Add(item);
            Assert.AreEqual(2, target.Count);
            
        }

        /// <summary>
        ///A test for AddRange
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void AddRangeTest()
        {
            Style style = new Style();
            StyleItemCollection target = new StyleItemCollection(style);
            IEnumerable<StyleItemBase> all = GetStdCollection();
            target.AddRange(all);
            Assert.AreEqual(StdCollectionCount, target.Count);
        }

        /// <summary>
        ///A test for Count
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void CountTest()
        {
            StyleItemCollection target = GetStdCollection();
            int actual;
            actual = target.Count;
            int expected = StdCollectionCount;

            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for IsReadOnly
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void IsReadOnlyTest()
        {
            StyleItemCollection target = GetStdCollection();
            bool actual;
            actual = target.IsReadOnly;
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for Clear
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void ClearTest()
        {
            Style style = new Style();
            StyleItemCollection target = new StyleItemCollection(style);
            IEnumerable<StyleItemBase> all = GetStdCollection();
            target.AddRange(all);
            Assert.AreEqual(StdCollectionCount, target.Count);

            target.Clear();
            Assert.AreEqual(0, target.Count);
        }

        /// <summary>
        ///A test for Contains
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void ContainsTest()
        {
            Style style = new Style();
            StyleItemCollection target = new StyleItemCollection(style);
            IEnumerable<StyleItemBase> all = GetStdCollection();
            target.AddRange(all);
            Assert.AreEqual(StdCollectionCount, target.Count);


            StyleItemBase item = new Scryber.Styles.FillStyle();
            bool expected = false;
            bool actual = target.Contains(item);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for CopyTo
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void CopyToTest()
        {
            int offset = 2;
            StyleItemCollection target = GetStdCollection();
            StyleItemBase[] array = new StyleItemBase[StdCollectionCount + offset];

            int arrayIndex = offset;
            target.CopyTo(array, arrayIndex);

            for (int i = 0; i < StdCollectionCount + offset; i++)
            {
                if (i < offset)
                    Assert.IsNull(array[i]);
                else
                    Assert.IsNotNull(array[i]);
            }
      

        }


        
        

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void RemoveTest()
        {
            StyleItemCollection target = GetStdCollection();
            StyleItemBase item = target[StyleKeys.BgItemKey]; //Assume there is more than one item
            bool expected = true; 
            bool actual = target.Remove(item);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(StdCollectionCount - 1, target.Count);
            Assert.IsFalse(target.Contains(item));
        }


       
    }
}
