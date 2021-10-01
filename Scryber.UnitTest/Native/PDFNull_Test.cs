using Scryber.PDF.Native;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;

namespace Scryber.Core.UnitTests.Native
{
    
    
    /// <summary>
    ///This is a test class for PDFNull_Test and is intended
    ///to contain all PDFNull_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFNull_Test
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
        ///A test for PDFNull Constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Scryber.dll")]
        [TestCategory("PDF Native")]
        public void PDFNullConstructor_Test()
        {
            //We never construct a null
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Equals_Test()
        {
            PDFNull target = PDFNull.Value;

            object obj = PDFNull.Value;
            bool expected = true;
            bool actual;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);

            obj = null;
            expected = false;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);

            obj = new object();
            expected = false;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void GetHashCode_Test()
        {
            PDFNull target = PDFNull.Value;
            PDFNull another = PDFNull.Value;

            int expected = target.GetHashCode();
            int actual;
            actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void ToString_Test()
        {
            PDFNull target = PDFNull.Value;
            string expected = PDFNull.NullString;
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for WriteData
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void WriteData_Test()
        {
            //TODO: PDFNull.WriteData test
        }

        /// <summary>
        ///A test for Type
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Type_Test()
        {
            PDFNull target = PDFNull.Value;
            ObjectType expected = ObjectTypes.Null;
            ObjectType actual;
            actual = target.Type;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Value
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Value_Test()
        {
            PDFNull actual;
            actual = PDFNull.Value;
            Assert.IsNotNull(actual);
            Assert.AreEqual(PDFNull.Value, actual);
        }
    }
}
