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

        private PDFStyleItemCollection GetStdCollection()
        {
            PDFStyle style = new PDFStyle();
            PDFStyleItemCollection col = new PDFStyleItemCollection(style);
            PDFBackgroundStyle bg = new PDFBackgroundStyle();
            bg.Color = PDFColors.Red;
            col.Add(bg);

            PDFBorderStyle bor = new PDFBorderStyle();
            bor.Color = PDFColors.Green;
            col.Add(bor);

            PDFFontStyle fnt = new PDFFontStyle();
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
            PDFStyle style = new PDFStyle();
            PDFStyleItemCollection target = new PDFStyleItemCollection(style);
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
            PDFStyle style = new PDFStyle();
            PDFStyleItemCollection target = new PDFStyleItemCollection(style);

            PDFStyleItemBase item = new PDFBorderStyle();
            target.Add(item);

            Assert.AreEqual(1, target.Count);

            item = new PDFFillStyle();
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
            PDFStyle style = new PDFStyle();
            PDFStyleItemCollection target = new PDFStyleItemCollection(style);
            IEnumerable<PDFStyleItemBase> all = GetStdCollection();
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
            PDFStyleItemCollection target = GetStdCollection();
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
            PDFStyleItemCollection target = GetStdCollection();
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
            PDFStyle style = new PDFStyle();
            PDFStyleItemCollection target = new PDFStyleItemCollection(style);
            IEnumerable<PDFStyleItemBase> all = GetStdCollection();
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
            PDFStyle style = new PDFStyle();
            PDFStyleItemCollection target = new PDFStyleItemCollection(style);
            IEnumerable<PDFStyleItemBase> all = GetStdCollection();
            target.AddRange(all);
            Assert.AreEqual(StdCollectionCount, target.Count);


            PDFStyleItemBase item = new PDFFillStyle();
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
            PDFStyleItemCollection target = GetStdCollection();
            PDFStyleItemBase[] array = new PDFStyleItemBase[StdCollectionCount + offset];

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
            PDFStyleItemCollection target = GetStdCollection();
            PDFStyleItemBase item = target[PDFStyleKeys.BgItemKey]; //Assume there is more than one item
            bool expected = true; 
            bool actual = target.Remove(item);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(StdCollectionCount - 1, target.Count);
            Assert.IsFalse(target.Contains(item));
        }


       
    }
}
