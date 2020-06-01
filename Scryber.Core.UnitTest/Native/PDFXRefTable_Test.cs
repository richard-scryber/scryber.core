using Scryber.Native;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using System.Collections.Generic;

namespace Scryber.Core.UnitTests.Native
{
    
    
    /// <summary>
    ///This is a test class for PDFXRefTable_Test and is intended
    ///to contain all PDFXRefTable_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFXRefTable_Test : IStreamFactory
    {
        #region IStreamFactory interface

        public PDFStream CreateStream(IStreamFilter[] filters, IIndirectObject forobject)
        {
            PDFIndirectObject indobj = (PDFIndirectObject)forobject;
            return new PDFStream(filters, indobj);
        }

        #endregion

        private class XRefTableProxy : PDFXRefTable
        {
            public XRefTableProxy(int generation)
                : base(generation)
            {
            }
        }

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
        ///A test for PDFXRefTable Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void PDFXRefTableConstructor_Test()
        {
            int generation = 1;
            PDFXRefTable target = new XRefTableProxy(generation);

            Assert.AreEqual(generation, target.Generation);
            Assert.IsNotNull(target.Sections);
            Assert.AreEqual(0, target.Offset);

            int defaultReferenceCount = 1; //there should be a single empty entry on a newly constructed XRefTable
            Assert.AreEqual(defaultReferenceCount, target.ReferenceCount);

        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Add_Test()
        {
            PDFIndirectObject obj = new PDFIndirectObject(this);

            int generation = 1;
            PDFXRefTable target = new XRefTableProxy(generation);
            int count = target.ReferenceCount;

            int expected = 1; //Should return 1 as the first item.
            int actual;
            actual = target.Append(obj);
            Assert.AreEqual(expected, actual);

            //Check that it has set the IndirectObjects' values too.
            Assert.AreEqual(target.Generation, obj.Generation);
            Assert.AreEqual(actual, obj.Number);
            Assert.AreEqual(-1L, obj.Offset);

            //And check that it is in the list of references
            Assert.AreEqual(count + 1, target.ReferenceCount);
            Assert.IsTrue(target.Contains(obj));

            //Add another and test
            PDFIndirectObject obj2 = new PDFIndirectObject(this);

            expected = 2;
            actual = target.Append(obj);
            Assert.AreEqual(expected, actual);

            //Check that it has set the IndirectObjects' values too.
            Assert.AreEqual(target.Generation, obj.Generation);
            Assert.AreEqual(actual, obj.Number);
            Assert.AreEqual(-1L, obj.Offset);

            //And check that it is in the list of references
            Assert.AreEqual(count + 2, target.ReferenceCount);
            Assert.IsTrue(target.Contains(obj));


            obj.Dispose();
            obj2.Dispose();

        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Remove_Test()
        {
            int generation = 1;
            PDFXRefTable target = new XRefTableProxy(generation);
            int count = target.ReferenceCount;

            PDFIndirectObject obj = new PDFIndirectObject(this);
            int actual = target.Append(obj);
            PDFIndirectObject obj2 = new PDFIndirectObject(this);
            actual = target.Append(obj2);

            //And check that it is in the list of references
            Assert.AreEqual(count + 2, target.ReferenceCount);
            Assert.IsTrue(target.Contains(obj2));

            //XRef tables should not actualy remove entries, but replace the cell with an empty/deleted reference.
            target.Delete(obj2);
            Assert.AreEqual(count + 2, target.ReferenceCount);
            Assert.IsTrue(target.Contains(obj2));
            
            IIndirectObject removed = target[count + 1].Reference;
            Assert.IsNotNull(removed);
            Assert.IsTrue(removed.Deleted);

            obj.Dispose();
            obj2.Dispose();
        }

        /// <summary>
        ///A test for Generation
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Generation_Test()
        {
            int generation = 1;
            PDFXRefTable target = new XRefTableProxy(generation);
            int actual;
            actual = target.Generation;
            Assert.AreEqual(generation, actual);

            PDFIndirectObject obj = new PDFIndirectObject(this);
            Assert.AreNotEqual(generation, obj.Generation);

            target.Append(obj);
            Assert.AreEqual(generation, obj.Generation);

            obj.Dispose();
        }

        /// <summary>
        ///A test for Offset
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Offset_Test()
        {
            int generation = 1;
            PDFXRefTable target = new XRefTableProxy(generation);
            long expected = 0;
            long actual;
            
            actual = target.Offset;
            Assert.AreEqual(expected, actual);

            expected = (long)int.MaxValue + 2L;
            target.Offset = expected;

            actual = target.Offset;
            Assert.AreEqual(expected, actual);
            
        }

    }
}
