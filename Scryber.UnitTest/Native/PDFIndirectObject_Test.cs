using Scryber.Native;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;

namespace Scryber.Core.UnitTests.Native
{
    
    
    /// <summary>
    ///This is a test class for PDFIndirectObject_Test and is intended
    ///to contain all PDFIndirectObject_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFIndirectObject_Test : IStreamFactory
    {
        #region IStreamFactory interface

        public PDFStream CreateStream(IStreamFilter[] filters, IIndirectObject forobject)
        {
            PDFIndirectObject indobj = (PDFIndirectObject)forobject;
            return new PDFStream(filters, indobj);
        }

        #endregion


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
        ///A test for PDFIndirectObject Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void PDFIndirectObjectConstructor_Test()
        {
            PDFIndirectObject target = new PDFIndirectObject(this);
            Assert.IsNotNull(target);

            //Check the initialized values are empty
            Assert.AreEqual(-1, target.Generation);
            Assert.AreEqual(-1, target.Number);
            Assert.AreEqual(-1L, target.Offset);
            Assert.IsNotNull(target.ObjectData);
            Assert.IsFalse(target.HasStream);
            Assert.IsNull(target.Stream);
            Assert.AreEqual(PDFObjectTypes.IndirectObject, target.Type);
            Assert.IsFalse(target.Deleted);

            target.Dispose();
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Dispose_Test()
        {
            PDFIndirectObject target = new PDFIndirectObject(this);

            Assert.IsNotNull(target.ObjectData);
            target.Dispose();
            Assert.IsNull(target.ObjectData);

            target = new PDFIndirectObject(this);
            target.InitStream(null);
            Assert.IsNotNull(target.ObjectData);
            Assert.IsNotNull(target.Stream);
            target.Dispose();
            Assert.IsNull(target.ObjectData);
            Assert.IsNull(target.Stream);

            try
            {
                target.InitStream(null);
                Assert.Fail("Initialized a new stream on a disposed instance");
            }
            catch (InvalidOperationException)
            {
                TestContext.WriteLine("Successfully caught the invalid operation for a disposed object");
            }

        }

        /// <summary>
        ///A test for GetObjectData
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void GetObjectData_Test()
        {
            using (PDFIndirectObject target = new PDFIndirectObject(this))
            {
                byte[] expected = new byte[] { };
                byte[] actual;
                actual = target.GetObjectData();

                Assert.IsNotNull(actual);
                Assert.AreEqual(expected.Length, actual.Length);
            }

        }

        /// <summary>
        ///A test for GetStreamData
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void GetStreamData_Test()
        {
            using (PDFIndirectObject target = new PDFIndirectObject(this))
            {
                byte[] expected = new byte[] { };
                byte[] actual;

                target.InitStream(null);
                actual = target.GetStreamData();
                Assert.IsNotNull(actual);
                Assert.AreEqual(expected.Length, actual.Length);

            }
        }

        /// <summary>
        ///A test for InitStream
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void InitStream_Test()
        {
            using (PDFIndirectObject target = new PDFIndirectObject(this))
            {
                byte[] expected = new byte[] { };
                byte[] actual;
                Assert.IsNull(target.Stream);
                target.InitStream(null);
                actual = target.GetStreamData();
                Assert.IsNotNull(actual);
                Assert.AreEqual(expected.Length, actual.Length);

            }
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void ToString_Test()
        {
            PDFIndirectObject target = new PDFIndirectObject(this);
            target.Generation = 1;
            target.Number = 10;

            string expected = "10 1 R";
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);

            target.Number = 20;
            expected = "20 1 R";
            actual = target.ToString();
            Assert.AreEqual(expected, actual);

            target.Dispose();
        }

        

        /// <summary>
        ///A test for Deleted
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Deleted_Test()
        {
            PDFIndirectObject target = new PDFIndirectObject(this);
            bool expected = false;
            bool actual;
            
            actual = target.Deleted;
            Assert.AreEqual(expected, actual);

            target.Deleted = true;
            expected = true;
            actual = target.Deleted;
            Assert.AreEqual(expected, actual);

            target.Dispose();
        }

        /// <summary>
        ///A test for Generation
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Generation_Test()
        {
            PDFIndirectObject target = new PDFIndirectObject(this);
            int expected = -1;
            int actual;

            actual = target.Generation;
            Assert.AreEqual(expected, actual);

            target.Generation = 1;
            expected = 1;
            actual = target.Generation;
            Assert.AreEqual(expected, actual);

            target.Dispose();

        }

        /// <summary>
        ///A test for HasStream
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void HasStream_Test()
        {
            PDFIndirectObject target = new PDFIndirectObject(this);
            bool actual;
            actual = target.HasStream;
            Assert.IsFalse(actual);

            target.InitStream(null);
            actual = target.HasStream;
            Assert.IsTrue(actual);

            target.Dispose();
        }

        /// <summary>
        ///A test for Number
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Number_Test()
        {
            PDFIndirectObject target = new PDFIndirectObject(this);
            int expected = -1;
            int actual;

            actual = target.Number;
            Assert.AreEqual(expected, actual);

            target.Number = 1;
            expected = 1;
            actual = target.Number;
            Assert.AreEqual(expected, actual);

            target.Dispose();
        }

        /// <summary>
        ///A test for ObjectData
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void ObjectData_Test()
        {
            PDFIndirectObject target = new PDFIndirectObject(this);
            PDFStream actual;
            actual = target.ObjectData;
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Type == PDFObjectTypes.Stream);

            target.Dispose();
        }

        /// <summary>
        ///A test for Offset
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Offset_Test()
        {
            PDFIndirectObject target = new PDFIndirectObject(this);
            long expected = -1;
            long actual;

            actual = target.Offset;
            Assert.AreEqual(expected, actual);

            target.Offset = ((long)int.MaxValue) * 2L;
            expected = ((long)int.MaxValue) * 2L; 

            actual = target.Offset;
            Assert.AreEqual(expected, actual);

            target.Dispose();
        }

        /// <summary>
        ///A test for Stream
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Stream_Test()
        {
            PDFIndirectObject target = new PDFIndirectObject(this);
            PDFStream actual;
            actual = target.Stream;
            Assert.IsNull(actual);

            target.InitStream(null);
            actual = target.Stream;
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Type == PDFObjectTypes.Stream);

            target.Dispose();
        }

        /// <summary>
        ///A test for Type
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Type_Test()
        {
            PDFIndirectObject target = new PDFIndirectObject(this);
            PDFObjectType actual;
            actual = target.Type;
            Assert.AreEqual(actual, PDFObjectTypes.IndirectObject);
        }
    }
}
