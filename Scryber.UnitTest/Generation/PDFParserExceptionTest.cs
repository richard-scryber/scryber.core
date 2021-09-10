using Scryber;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Scryber.Core.UnitTests.Generation
{
    
    
    /// <summary>
    ///This is a test class for PDFParserExceptionTest and is intended
    ///to contain all PDFParserExceptionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFParserExceptionTest
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
        ///A test for PDFParserException Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Utilities")]
        public void PDFParserExceptionConstructorTest()
        {
            PDFParserException target = new PDFParserException();
            Assert.IsNull(target.InnerException);

        }

        /// <summary>
        ///A test for PDFParserException Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Utilities")]
        public void PDFParserExceptionConstructorTest1()
        {
            string message = "message"; // TODO: Initialize to an appropriate value
            PDFParserException target = new PDFParserException(message);
            Assert.AreEqual(message, target.Message);
            Assert.IsNull(target.InnerException);
        }

        /// <summary>
        ///A test for PDFParserException Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Utilities")]
        public void PDFParserExceptionConstructorTest2()
        {
            string message = "message"; // TODO: Initialize to an appropriate value
            Exception inner = new Exception(); // TODO: Initialize to an appropriate value
            PDFParserException target = new PDFParserException(message, inner);
            Assert.AreEqual(message, target.Message);
            Assert.AreEqual(inner, target.InnerException);
        }
    }
}
