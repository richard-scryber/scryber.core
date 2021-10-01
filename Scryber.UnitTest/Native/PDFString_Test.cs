using Scryber.PDF.Native;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.PDF;

namespace Scryber.Core.UnitTests.Native
{
    
    
    /// <summary>
    ///This is a test class for PDFString_Test and is intended
    ///to contain all PDFString_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFString_Test
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
        ///A test for PDFString Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void PDFStringConstructor_Test()
        {
            string value = "Hello World";
            PDFString target = new PDFString(value);
            Assert.AreEqual(value,target.Value);
        }

        /// <summary>
        ///A test for PDFString Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void PDFStringConstructor_Test1()
        {
            PDFString target = new PDFString();
            Assert.IsNull(target.Value);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Equals_Test()
        {
            string value = "Hello World";
            PDFString target = new PDFString(value);
            PDFString other = new PDFString(value);
            bool expected = true;
            bool actual;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            value = "No Hello";
            other = new PDFString(value);
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            value = null;
            other = new PDFString(value);
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Equals_Test1()
        {
            string value = "Hello World";
            PDFString target = new PDFString(value); 
            object obj = new PDFString(value);
            bool expected = true;
            bool actual;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);

            value = "Not Hello World";
            obj = new PDFString(value);
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
            string value = "Hello World";
            PDFString target = new PDFString(value);
            int expected = value.GetHashCode();
            int actual;
            actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);

            PDFString other = new PDFString(value);
            expected = other.GetHashCode();
            actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);


        }

        /// <summary>
        ///A test for WriteData
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void WriteData_Test()
        {
            PDFString expected = (PDFString)"This is some text";
            string result;
            PDFString actual;

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                using (PDFWriter writer = new PDFWriter14(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic)))
                {
                    expected.WriteData(writer);
                    writer.InnerStream.Flush();

                }
                stream.Position = 0;

                using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                {
                    result = sr.ReadToEnd();
                }
            }

            actual = PDFString.Parse(result.Trim());
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for op_Explicit
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Explicit_Test()
        {
            string str = "Hello World";
            PDFString expected = new PDFString(str);
            PDFString actual;
            actual = (PDFString)str;
            Assert.AreEqual(expected, actual);

            str = "Not Hello World";
            actual = (PDFString)str;
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for op_Explicit
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Explicit_Test1()
        {
            string expected = "Hello world";
            PDFString str = new PDFString(expected);
            string actual;
            actual = ((string)(str));
            Assert.AreEqual(expected, actual);

            str = new PDFString("Not Hello world");
            actual = (string)str;
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for Type
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Type_Test()
        {
            PDFString target = new PDFString();

            ObjectType expected = ObjectTypes.String;
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
            string expected = "Hello World";
            PDFString target = new PDFString(expected);
            string actual;
            actual = target.Value;
            Assert.AreEqual(expected, actual);

            expected = "New Hello World";

            target.Value = expected;
            actual = target.Value;

            Assert.AreEqual(expected, actual);

        }
    }
}
