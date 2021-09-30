using Scryber.PDF.Native;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.PDF;

namespace Scryber.Core.UnitTests.Native
{
    
    
    /// <summary>
    ///This is a test class for PDFName_Test and is intended
    ///to contain all PDFName_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFName_Test
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
        ///A test for PDFName Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void PDFNameConstructor_Test()
        {
            string name = "ValidName";
            PDFName target = new PDFName(name);
            Assert.IsNotNull(target);
            Assert.AreEqual(name, target.Value);

            name = "Invalid/Name";

            try
            {
                target = new PDFName(name);
                Assert.Fail("Should not be able to create a name with spaces in");
            }
            catch (ArgumentException)
            {
                TestContext.WriteLine("Successfully caught the expected invalid name exception");
            }
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Equals_Test()
        {
            string name = "Name";
            PDFName target = new PDFName(name);

            object obj = new PDFName(name); 
            bool expected = true;
            bool actual;
            actual = target.Equals(obj);

            obj = new PDFName("NewName");
            expected = false;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);

            obj = new object();
            expected = false;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);

            obj = null;
            expected = false;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Equals_Test1()
        {
            string name = "Name";
            PDFName target = new PDFName(name);

            PDFName other = new PDFName(name);
            bool expected = true;
            bool actual;
            actual = target.Equals(other);

            other = new PDFName("NewName");
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = null;
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void GetHashCode_Test()
        {
            string name = "Name";
            PDFName target = new PDFName(name);

            //Hashcode for a PDFName and the hash code for the 
            //underlying string should be the same
            int expected = name.GetHashCode();
            int actual;
            actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);

            //Hashcode for 2 PDFNames with the 
            //same string should also be the same
            PDFName other = new PDFName(name);
            expected = target.GetHashCode();
            actual = other.GetHashCode();
            Assert.AreEqual(expected, actual);

            //Different name - different hash code (within reason)
            other = new PDFName("NotName");
            expected = target.GetHashCode();
            actual = other.GetHashCode();
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for ValidateName
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void ValidateName_Test()
        {
            string namevalue = "OkName";
            PDFName.ValidateName(namevalue);
            foreach (Char c in PDFName.InvalidNameChars)
            {
                string invalidvalue = namevalue + c.ToString();
                try
                {
                    PDFName.ValidateName(invalidvalue);
                    Assert.Fail("Invalid name did not raise exception");
                }
                catch (ArgumentException)
                {
                    TestContext.WriteLine("Successfully caught the invalid name");
                }
            }

            try
            {
                PDFName.ValidateName(null);
                Assert.Fail("Invalid null name did not raise exception");
            }
            catch (ArgumentException)
            {
                TestContext.WriteLine("Successfully caught the null invalid name");
            }
        }

        /// <summary>
        ///A test for WriteData
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void WriteData_Test()
        {
            PDFName expected = new PDFName("ThisIsTheName");
            string result;
            PDFName actual;

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

            actual = PDFName.Parse(result.Trim());
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Explicit
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Explicit_Test()
        {
            string name = "NewName";
            PDFName expected = new PDFName(name);
            PDFName actual;
            actual = ((PDFName)(name));
            Assert.AreEqual(expected, actual);

            actual = (PDFName)"OtherName";
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Explicit
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Explicit_Test1()
        {
            string expected = "NewName"; 
            PDFName name = new PDFName(expected);
            string actual;
            actual = ((string)(name));
            Assert.AreEqual(expected, actual);

            name = new PDFName("OtherName");
            actual = (string)name;
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for InvalidNameChars
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void InvalidNameChars_Test()
        {
            char[] actual;
            actual = PDFName.InvalidNameChars;
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.Length == 0);
        }

        /// <summary>
        ///A test for Type
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Type_Test()
        {
            string name = "Name"; 
            PDFName target = new PDFName(name); 
            ObjectType actual;
            actual = target.Type;
            ObjectType expected = PDFObjectTypes.Name;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Value
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Value_Test()
        {
            string name = "Name"; 
            PDFName target = new PDFName(name);
            string actual;
            actual = target.Value;

            Assert.AreEqual(name, actual);
        }
    }
}
