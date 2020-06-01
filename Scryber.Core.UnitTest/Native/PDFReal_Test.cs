using Scryber.Native;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;

namespace Scryber.Core.UnitTests.Native
{
    
    
    /// <summary>
    ///This is a test class for PDFReal_Test and is intended
    ///to contain all PDFReal_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFReal_Test
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
        ///A test for PDFReal Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void PDFRealConstructor_Test()
        {
            double value = 0F;
            PDFReal target = new PDFReal(value);
            Assert.AreEqual(value, target.Value);

            value = -10.0;
            target = new PDFReal(value);
            Assert.AreEqual(value, target.Value);

            value = double.MaxValue;
            target = new PDFReal(value);
            Assert.AreEqual(value, target.Value);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Equals_Test()
        {
            PDFReal target = new PDFReal(10.0);
            PDFReal num = new PDFReal(10.0);
            bool expected = true;
            bool actual;
            actual = target.Equals(num);
            Assert.AreEqual(expected, actual);

            num = new PDFReal(-10.9);
            expected = false;
            actual = target.Equals(num);
            Assert.AreEqual(expected, actual);

            num = new PDFReal();
            expected = false;
            actual = target.Equals(num);
            Assert.AreEqual(expected, actual);

            num = new PDFReal();
            target = new PDFReal();
            expected = true;
            actual = target.Equals(num);
            Assert.AreEqual(expected, actual);



        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Equals_Test1()
        {
            double value = 10.0;
            PDFReal target = new PDFReal(value);
            object obj = null; 
            bool expected = false;
            bool actual;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);

            obj = new PDFReal(value);
            expected = true;
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
            double value = 10.0;
            PDFReal target = new PDFReal(value);
            int expected = value.GetHashCode();
            int actual;
            actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);

            PDFReal other = new PDFReal(value);
            expected = target.GetHashCode();
            actual = other.GetHashCode();
            Assert.AreEqual(expected, actual);

            other = new PDFReal(value + 1.0);
            expected = target.GetHashCode();
            actual = other.GetHashCode();
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Parse_Test()
        {

            string value = "10.2";
            PDFReal expected = new PDFReal(10.2);
            PDFReal actual;
            actual = PDFReal.Parse(value);
            Assert.AreEqual(expected, actual);

            value = "10";
            expected = new PDFReal(10);
            actual = PDFReal.Parse(value);
            Assert.AreEqual(expected, actual);

            value = "NotANumber";
            try
            {
                actual = PDFReal.Parse(value);
                Assert.Fail("Did not raise an parser exception");
            }
            catch (PDFNativeParserException)
            {
                TestContext.WriteLine("Successfully caught the format exception for invalid number");
            }

            value = null;
            try
            {
                actual = PDFReal.Parse(value);
                Assert.Fail("Did not raise a parser exception");
            }
            catch (PDFNativeParserException)
            {
                TestContext.WriteLine("Successfully caught the null exception for invalid number");
            }

        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void ToString_Test()
        {
            double value = 10.2;
            PDFReal target = new PDFReal(value);
            string expected = value.ToString();
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
            PDFReal real = 1.4;
            string result;
            PDFReal actual;

            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                using (PDFWriter writer = new PDFWriter14(stream, new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Diagnostic)))
                {
                    real.WriteData(writer);
                    writer.InnerStream.Flush();
                    
                }
                stream.Position = 0;

                using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                {
                    result = sr.ReadToEnd();
                }
            }

            actual = Double.Parse(result);
            Assert.AreEqual(real, actual);
        }

        /// <summary>
        ///A test for op_Addition
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Addition_Test()
        {
            PDFReal one = new PDFReal(10.0);
            PDFReal two = new PDFReal(22.0);
            PDFReal expected = new PDFReal(32.0);
            PDFReal actual;
            actual = (one + two);
            Assert.AreEqual(expected.Value, actual.Value);

            one = new PDFReal(-20.5);
            two = new PDFReal(20.5);
            expected = new PDFReal(0);
            actual = (one + two);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for op_Division
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Division_Test()
        {
            PDFReal one = new PDFReal(10.2);
            PDFReal two = new PDFReal(2.0); 
            PDFReal expected = new PDFReal(5.1);
            PDFReal actual;
            actual = (one / two);
            Assert.AreEqual(expected, actual);

            two = new PDFReal(0.0);

            try
            {
                actual = (one / two);
                //because we are a double then we can just about do this
                if (!double.IsInfinity(actual.Value))
                    Assert.Fail("Did not throw error for divide by zero");
            }
            catch (DivideByZeroException)
            {
                TestContext.WriteLine("Successfully caught the divide by 0 exception");
            }
        }

        /// <summary>
        ///A test for op_Equality
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Equality_Test()
        {
            PDFReal one = new PDFReal(10.5);
            PDFReal two = new PDFReal(10.5);
            bool expected = true;
            bool actual;
            actual = (one == two);
            Assert.AreEqual(expected, actual);

            two = new PDFReal(5.5);
            expected = false;
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
            double expected = 10.5;
            PDFReal number = new PDFReal(expected);
            double actual;
            actual = ((double)(number));
            Assert.AreEqual(expected, actual);

            number = new PDFReal(20.1);
            actual = (double)number;
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for op_Explicit
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Explicit_Test1()
        {
            float expected = 10.2F;
            PDFReal number = new PDFReal(expected);
            float actual;
            actual = ((float)(number));
            Assert.AreEqual(expected, actual);

            number = new PDFReal(20.4F);
            actual = (float)number;
            Assert.AreNotEqual(expected, actual);

        }

        /// <summary>
        ///A test for op_GreaterThan
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_GreaterThan_Test()
        {
            PDFReal one = new PDFReal(10.2);
            PDFReal two = new PDFReal(10.1);
            bool expected = true;
            bool actual;
            actual = (one > two);
            Assert.AreEqual(expected, actual);

            two = new PDFReal(10.3);
            expected = false;
            actual = (one > two);
            Assert.AreEqual(expected, actual);

            two = new PDFReal(10.2);
            expected = false;
            actual = (one > two);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_GreaterThanOrEqual
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_GreaterThanOrEqual_Test()
        {
            PDFReal one = new PDFReal(10.2);
            PDFReal two = new PDFReal(10.1);
            bool expected = true;
            bool actual;
            actual = (one >= two);
            Assert.AreEqual(expected, actual);

            two = new PDFReal(10.3);
            expected = false;
            actual = (one >= two);
            Assert.AreEqual(expected, actual);

            two = new PDFReal(10.2);
            expected = true;
            actual = (one >= two);
            Assert.AreEqual(expected, actual);



        }

        /// <summary>
        ///A test for op_Implicit
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Implicit_Test()
        {
            float val = 10.5F;
            PDFReal expected = new PDFReal(val);
            PDFReal actual;
            actual = val;
            Assert.AreEqual(expected, actual);

            val = 10.6F;
            actual = val;
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Implicit
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Implicit_Test1()
        {
            double val = 10.5;
            PDFReal expected = new PDFReal(val);
            PDFReal actual;
            actual = val;
            Assert.AreEqual(expected, actual);

            val = 10.6;
            actual = val;
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Inequality
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Inequality_Test()
        {
            PDFReal one = new PDFReal(10.2);
            PDFReal two = new PDFReal(10.1);
            bool expected = true;
            bool actual;
            actual = (one != two);
            Assert.AreEqual(expected, actual);

            two = new PDFReal(10.3);
            expected = true;
            actual = (one != two);
            Assert.AreEqual(expected, actual);

            two = new PDFReal(10.2);
            expected = false;
            actual = (one != two);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_LessThan
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_LessThan_Test()
        {
            PDFReal one = new PDFReal(10.2);
            PDFReal two = new PDFReal(10.1);
            bool expected = false;
            bool actual;
            actual = (one < two);
            Assert.AreEqual(expected, actual);

            two = new PDFReal(10.3);
            expected = true;
            actual = (one < two);
            Assert.AreEqual(expected, actual);

            two = new PDFReal(10.2);
            expected = false;
            actual = (one < two);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_LessThanOrEqual
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_LessThanOrEqual_Test()
        {
            PDFReal one = new PDFReal(10.2);
            PDFReal two = new PDFReal(10.1);
            bool expected = false;
            bool actual;
            actual = (one <= two);
            Assert.AreEqual(expected, actual);

            two = new PDFReal(10.3);
            expected = true;
            actual = (one <= two);
            Assert.AreEqual(expected, actual);

            two = new PDFReal(10.2);
            expected = true;
            actual = (one <= two);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Multiply
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Multiply_Test()
        {
            PDFReal one = new PDFReal(10.2);
            PDFReal two = new PDFReal(2.0);
            PDFReal expected = new PDFReal(20.4);
            PDFReal actual;
            actual = (one * two);
            Assert.AreEqual(expected, actual);

            two = new PDFReal(0.0);

            actual = (one * two);
            Assert.AreEqual(two, actual);

        }

        /// <summary>
        ///A test for op_Subtraction
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void op_Subtraction_Test()
        {
            PDFReal one = new PDFReal(10.1);
            PDFReal two = new PDFReal(20.2);
            PDFReal expected = new PDFReal(-10.1);
            PDFReal actual;
            actual = (one - two);
            Assert.AreEqual(expected, actual);

            one = new PDFReal(20.5);
            two = new PDFReal(20.5);
            expected = new PDFReal(0);
            actual = (one - two);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Type
        ///</summary>
        [TestMethod()]
        [TestCategory("PDF Native")]
        public void Type_Test()
        {
            PDFReal target = new PDFReal();
            PDFObjectType expected = PDFObjectTypes.Real;
            PDFObjectType actual;
            actual = target.Type;
            Assert.AreEqual(expected, actual);

            target = 10.5;
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
            double expected =100.5;
            PDFReal target = new PDFReal(expected);

            double actual = target.Value;
            Assert.AreEqual(actual, expected);

            expected = 200.1;
            target.Value = expected;
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
            PDFReal actual;
            actual = PDFReal.Zero;
            Assert.AreEqual(0.0, actual.Value);
        }
    }
}
