using Scryber.Native;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;

namespace Scryber.Core.UnitTests.Native
{
    
    
    /// <summary>
    ///This is a test class for PDFBoolean_Test and is intended
    ///to contain all PDFBoolean_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFBoolean_Test
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
        ///A test for PDFBoolean Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void PDFBooleanConstructor_Test()
        {
            bool val = false;
            PDFBoolean target = new PDFBoolean(val);
            Assert.AreEqual(val, target.Value);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Equals_Test()
        {

            PDFBoolean target = new PDFBoolean(true);
            object obj = new PDFBoolean(true);

            bool expected = true;
            bool actual;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual, "PDFBooleans did not compare as equal");

            obj = new PDFBoolean(false);
            expected = false;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual, "PDFBooleans compared falsely as equal");

            obj = new object();
            expected = false;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual, "PDFBoolean and Object compared falsely as equal");

        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Equals_Test1()
        {
            PDFBoolean one = new PDFBoolean(true);
            PDFBoolean two = new PDFBoolean(true);
            bool expected = true; 
            bool actual;
            actual = PDFBoolean.Equals(one, two);
            Assert.AreEqual(expected, actual, "PDFBooleans did not compare as equal");

            actual = Object.Equals(one, two);
            Assert.AreEqual(expected, actual, "PDFBooleans as objects did not compare as equal");

            two = new PDFBoolean(false);
            expected = false;
            actual = PDFBoolean.Equals(one, two);
            Assert.AreEqual(expected, actual, "PDFBooleans compared falsely as equal");

            actual = Object.Equals(one, two);
            Assert.AreEqual(expected, actual, "PDFBooleans as objects did not compare as equal");
        }


        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void GetHashCode_Test()
        {
            PDFBoolean one = new PDFBoolean(true);
            int expected = one.GetHashCode();

            PDFBoolean two = new PDFBoolean(true);
            int actual = two.GetHashCode();

            Assert.AreEqual(expected, actual,"PDFBoolean hash codes for same value were different");

            two = new PDFBoolean(false);
            actual = two.GetHashCode();

            Assert.AreNotEqual(expected, actual, "PDFBoolean hash codes for different value were the same");

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void ToString_Test()
        {
            PDFBoolean target = new PDFBoolean(true);
            string expected = PDFBoolean.TrueString;
            
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);

            target = new PDFBoolean(false);
            actual = target.ToString();
            expected = PDFBoolean.FalseString;
            Assert.AreEqual(expected, actual);

            
        }

        /// <summary>
        ///A test for WriteData
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void WriteData_Test()
        {
            // TODO: PDFBoolean.WriteData method
        }

        /// <summary>
        ///A test for op_Equality
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Equality_Test()
        {
            PDFBoolean one = new PDFBoolean(true);
            PDFBoolean two = new PDFBoolean(false);
            bool expected = false;
            bool actual;
            actual = (one == two);
            Assert.AreEqual(expected, actual);

            one = new PDFBoolean(true);
            two = new PDFBoolean(true);
            expected = true;
            actual = (one == two);
            Assert.AreEqual(expected, actual);

            one = new PDFBoolean(false);
            two = new PDFBoolean(false);
            expected = true;
            actual = (one == two);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Explicit
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Explicit_Test()
        {
            PDFBoolean value = new PDFBoolean(false);
            bool expected = false;
            bool actual;
            actual = (bool)(value);
            Assert.AreEqual(expected, actual);

            value = new PDFBoolean(true);
            expected = true;
            actual = (bool)value;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Explicit
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Explicit_Test1()
        {
            bool value = false; 
            PDFBoolean expected = new PDFBoolean(false); 
            PDFBoolean actual;
            actual = (PDFBoolean)value;
            Assert.AreEqual(expected, actual);

            expected = new PDFBoolean(true);
            value = true;
            actual = (PDFBoolean)value;
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for op_Inequality
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Inequality_Test()
        {
            PDFBoolean one = new PDFBoolean(true);
            PDFBoolean two = new PDFBoolean(false);
            bool expected = true;
            bool actual;
            actual = (one != two);
            Assert.AreEqual(expected, actual);

            one = new PDFBoolean(true);
            two = new PDFBoolean(true);
            expected = false;
            actual = (one != two);
            Assert.AreEqual(expected, actual);

            one = new PDFBoolean(false);
            two = new PDFBoolean(false);
            expected = false;
            actual = (one != two);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Type
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Type_Test()
        {
            PDFBoolean target = new PDFBoolean(false);
            PDFObjectType expected = PDFObjectTypes.Boolean;
            PDFObjectType actual;
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
            bool expected = true;
            PDFBoolean target = new PDFBoolean(expected);
            bool actual;
            actual = target.Value;
            Assert.AreEqual(expected, actual);

            expected = false;
            target = new PDFBoolean(expected);
            actual = target.Value;
            Assert.AreEqual(expected, actual);
        }
    }
}
