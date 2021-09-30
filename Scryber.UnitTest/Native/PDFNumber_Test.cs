using Scryber.PDF.Native;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.PDF;

namespace Scryber.Core.UnitTests.Native
{
    
    
    /// <summary>
    ///This is a test class for PDFNumber_Test and is intended
    ///to contain all PDFNumber_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFNumber_Test
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
        ///A test for op_LessThanOrEqual
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_LessThanOrEqual_Test()
        {
            PDFNumber left = new PDFNumber(1);
            PDFNumber right = new PDFNumber(2);
            bool expected = true;
            bool actual;
            actual = (left <= right);
            Assert.AreEqual(expected, actual);

            right = new PDFNumber(1);
            expected = true;
            actual = (left <= right);
            Assert.AreEqual(expected, actual);

            left = new PDFNumber(2);
            expected = false;
            actual = (left <= right);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for op_LessThan
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_LessThan_Test()
        {
            PDFNumber left = new PDFNumber(1);
            PDFNumber right = new PDFNumber(2);
            bool expected = true;
            bool actual;
            actual = (left < right);
            Assert.AreEqual(expected, actual);

            right = new PDFNumber(1);
            expected = false;
            actual = (left < right);
            Assert.AreEqual(expected, actual);

            left = new PDFNumber(2);
            expected = false;
            actual = (left < right);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Inequality
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Inequality_Test()
        {
            PDFNumber left = new PDFNumber(1);
            PDFNumber right = new PDFNumber(2);
            bool expected = true;
            bool actual;
            actual = (left != right);
            Assert.AreEqual(expected, actual);

            right = new PDFNumber(1);
            expected = false;
            actual = (left != right);
            Assert.AreEqual(expected, actual);

            left = new PDFNumber(2);
            expected = true;
            actual = (left != right);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_GreaterThanOrEqual
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_GreaterThanOrEqual_Test()
        {
            PDFNumber left = new PDFNumber(1);
            PDFNumber right = new PDFNumber(2);
            bool expected = false;
            bool actual;
            actual = (left >= right);
            Assert.AreEqual(expected, actual);

            right = new PDFNumber(1);
            expected = true;
            actual = (left >= right);
            Assert.AreEqual(expected, actual);

            left = new PDFNumber(2);
            expected = true;
            actual = (left >= right);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_GreaterThan
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_GreaterThan_Test()
        {
            PDFNumber left = new PDFNumber(1);
            PDFNumber right = new PDFNumber(2);
            bool expected = false;
            bool actual;
            actual = (left > right);
            Assert.AreEqual(expected, actual);

            right = new PDFNumber(1);
            expected = false;
            actual = (left > right);
            Assert.AreEqual(expected, actual);

            left = new PDFNumber(2);
            expected = true;
            actual = (left > right);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Explicit
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Explicit_Test()
        {
            int num = 10;
            PDFNumber expected = new PDFNumber(num);
            PDFNumber actual;
            actual = ((PDFNumber)(num));
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Explicit
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Explicit_Test1()
        {
            long expected = 10; 
            PDFNumber number = new PDFNumber(expected);
            long actual;
            actual = ((long)(number));
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Explicit
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Explicit_Test2()
        {
            int expected = 10; 
            PDFNumber number = new PDFNumber(expected);
            int actual;
            actual = ((int)(number));
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Explicit
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Explicit_Test3()
        {
            long num = 10;
            PDFNumber expected = new PDFNumber(num);
            PDFNumber actual;
            actual = ((PDFNumber)(num));
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Equality
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Equality_Test()
        {
            int value = 10;
            PDFNumber one = new PDFNumber(value);
            PDFNumber two = new PDFNumber(value);
            bool expected = true;
            bool actual;
            actual = (one == two);
            Assert.AreEqual(expected, actual);

            one = new PDFNumber(value + 1);
            expected = false;
            actual = (one == two);
            Assert.AreEqual(expected, actual);

            one = new PDFNumber(10L);
            expected = true;
            actual = (one == two);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Division
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Division_Test()
        {
            PDFNumber one = new PDFNumber(10);
            PDFNumber two = new PDFNumber(2);
            PDFNumber expected = new PDFNumber(5);
            PDFNumber actual;
            actual = (one / two);
            Assert.AreEqual(expected, actual);

            two = new PDFNumber(0);

            try
            {
                actual = (one / two);
                Assert.Fail("Divide by zero did not raise exception");
            }
            catch (DivideByZeroException)
            {
                TestContext.WriteLine("Correctly caught a divide by Zero attempt");
            }

            two = new PDFNumber(3);
            expected = new PDFNumber(10/3); //no integer result
            actual = (one / two);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for op_Addition
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Addition_Test()
        {
            PDFNumber one = new PDFNumber(10);
            PDFNumber two = new PDFNumber(2);
            PDFNumber expected = new PDFNumber(12);
            PDFNumber actual;
            actual = (one + two);
            Assert.AreEqual(expected, actual);

            two = new PDFNumber(-20);
            expected = new PDFNumber(-10);
            actual = (one + two);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Multiply
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Multiply_Test()
        {
            PDFNumber one = new PDFNumber(10);
            PDFNumber two = new PDFNumber(2);
            PDFNumber expected = new PDFNumber(20);
            PDFNumber actual;
            actual = (one * two);
            Assert.AreEqual(expected, actual);

            two = new PDFNumber(-20);
            expected = new PDFNumber(-200); // -20 * 10
            actual = (one * two);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Subtraction
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Subtraction_Test()
        {
            PDFNumber one = new PDFNumber(10);
            PDFNumber two = new PDFNumber(2);
            PDFNumber expected = new PDFNumber(8);
            PDFNumber actual;
            actual = (one - two);
            Assert.AreEqual(expected, actual);


            two = new PDFNumber(-20);
            expected = new PDFNumber(30); //10 - -20
            actual = (one - two);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for WriteData
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void WriteData_Test()
        {
            PDFNumber expected = (PDFNumber)(-201);
            string result;
            PDFNumber actual;

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

            actual = PDFNumber.Parse(result.Trim());
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void ToString_Test()
        {
            PDFNumber target = new PDFNumber(10);
            string expected = 10.ToString();
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);

            target = new PDFNumber(+10);
            expected = (+10).ToString();
            actual = target.ToString();
            Assert.AreEqual(expected, actual);


            target = new PDFNumber(-10);
            expected = (-10).ToString();
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Parse_Test()
        {
            string value = "102";
            long expected = long.Parse(value);
            PDFNumber actual = PDFNumber.Parse(value);

            Assert.AreEqual(expected, actual.Value);

            value = "NotANumber";

            try
            {
                actual = PDFNumber.Parse(value);
                Assert.Fail("Did not raise format exception");
            }
            catch (FormatException)
            {
                TestContext.WriteLine("Successfully caught the format exception for invalid number");
            }

            value = "102.1";

            try
            {
                actual = PDFNumber.Parse(value);
                Assert.Fail("Did not raise format exception");
            }
            catch (FormatException)
            {
                TestContext.WriteLine("Successfully caught the format exception for invalid number");
            }

            value = null;
            try
            {
                actual = PDFNumber.Parse(value);
                Assert.Fail("Did not raise a null exception");
            }
            catch (ArgumentNullException)
            {
                TestContext.WriteLine("Successfully caught the null exception for invalid number");
            }
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void GetHashCode_Test()
        {
            PDFNumber target = new PDFNumber(10); 
            int expected = (10).GetHashCode();
            int actual;
            actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Equals_Test()
        {
            PDFNumber target = new PDFNumber(10);
            object obj = new PDFNumber(10);
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);

            obj = new PDFNumber(100);
            expected = false;
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
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Equals_Test1()
        {
            PDFNumber target = new PDFNumber(10);
            PDFNumber num = new PDFNumber(10);
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Equals(num);
            Assert.AreEqual(expected, actual);

            num = new PDFNumber(100);
            expected = false;
            actual = target.Equals(num);
            Assert.AreEqual(expected, actual);

            num = default(PDFNumber);
            expected = false;
            actual = target.Equals(num);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PDFNumber Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void PDFNumberConstructor_Test()
        {
            int value = 100;
            PDFNumber target = new PDFNumber(value);
            Assert.AreEqual(value, (int)target.Value);
        }

        /// <summary>
        ///A test for PDFNumber Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void PDFNumberConstructor_Test1()
        {
            long value = (long)int.MaxValue + (long)int.MaxValue; 
            PDFNumber target = new PDFNumber(value);

            Assert.AreEqual(value, target.Value);
        }

        

        /// <summary>
        ///A test for Type
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Type_Test()
        {
            PDFNumber target = new PDFNumber();
            ObjectType expected = PDFObjectTypes.Number;

            ObjectType actual;
            actual = target.Type;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Value
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Scryber.dll")]
        [TestCategory("PDF Native")]
        public void Value_Test()
        {
            long expected = 1000L;
            PDFNumber target = new PDFNumber(expected);
            long actual;
            actual = target.Value;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Zero
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Zero_Test()
        {
            PDFNumber actual;
            actual = PDFNumber.Zero;
            Assert.AreEqual(0L, actual.Value);
        }
    }
}
