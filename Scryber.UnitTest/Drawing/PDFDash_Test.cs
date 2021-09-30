using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.PDF.Native;
using System.CodeDom;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFDash_Test and is intended
    ///to contain all PDFDash_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFDash_Test
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
        ///A test for PDFDash Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFDashConstructor_Test()
        {
            PDFNumber[] pattern = new PDFNumber[] { (PDFNumber)1.0, (PDFNumber)2.0 };
            PDFNumber len = (PDFNumber)3.0;
            PDFDash target = new PDFDash(pattern, len);

            Assert.IsNotNull(target.Pattern);
            Assert.AreEqual(target.Pattern.Length, 2);
            Assert.AreEqual(target.Pattern[0], pattern[0]);
            Assert.AreEqual(target.Pattern[1], pattern[1]);
            Assert.AreEqual(target.Phase, len);

        }

        /// <summary>
        ///A test for PDFDash Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFDashConstructor_Test1()
        {
            int[] pattern = new int[] { 1, 2 };
            int len = 3;
            PDFDash target = new PDFDash(pattern, len);
            
            Assert.IsNotNull(target.Pattern);
            Assert.AreEqual(target.Pattern.Length, 2);
            Assert.AreEqual(target.Pattern[0], (PDFNumber)pattern[0]);
            Assert.AreEqual(target.Pattern[1], (PDFNumber)pattern[1]);
            Assert.AreEqual(target.Phase, (PDFNumber)len);
        }

        /// <summary>
        ///A test for PDFDash Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFDashConstructor_Test2()
        {
            PDFDash target = new PDFDash(); //empty constructor - should result in [] 0 - solid line
            Assert.IsNotNull(target.Pattern);
            Assert.AreEqual(target.Pattern.Length, 0);
            Assert.AreEqual(target.Phase, (PDFNumber)0);
        }

        

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void Equals_Test()
        {
            int[] pattern = new int[] { 1, 2 };
            int len = 3;
            PDFDash target = new PDFDash(pattern, len);

            object obj = target;
            bool expected = true;
            bool actual = target.Equals(obj);

            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void Equals_Test1()
        {
            int[] pattern = new int[] { 1, 2 };
            int len = 3;
            PDFDash target = new PDFDash(pattern, len);

            PDFNumber[] pattern2 = new PDFNumber[] { (PDFNumber)1.0, (PDFNumber)2.0 };
            PDFNumber len2 = (PDFNumber)3.0;
            PDFDash target2 = new PDFDash(pattern2, len2);

            bool actual;
            actual = target.Equals(target2);
            Assert.IsTrue(actual);

            pattern2 = new PDFNumber[] { (PDFNumber)1.0, (PDFNumber)2.0 };
            len2 = (PDFNumber)4.0; // changed phase
            target2 = new PDFDash(pattern2, len2);
            actual = target.Equals(target2);
            Assert.IsFalse(actual);

            pattern2 = new PDFNumber[] { (PDFNumber)1.0, (PDFNumber)1.0 }; //changed number
            len2 = (PDFNumber)3.0;
            target2 = new PDFDash(pattern2, len2);
            actual = target.Equals(target2);
            Assert.IsFalse(actual);

            pattern2 = new PDFNumber[] { (PDFNumber)1.0, (PDFNumber)2.0, (PDFNumber)3.0 }; // Added number
            len2 = (PDFNumber)3.0;
            target2 = new PDFDash(pattern2, len2);
            actual = target.Equals(target2);
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void GetHashCode_Test()
        {
            int[] pattern = new int[] { 1, 2 };
            int len = 3;
            PDFDash target = new PDFDash(pattern, len);

            PDFNumber[] pattern2 = new PDFNumber[] { (PDFNumber)1.0, (PDFNumber)2.0 };
            PDFNumber len2 = (PDFNumber)3.0;
            PDFDash target2 = new PDFDash(pattern2, len2);

            int expected = target.GetHashCode();
            int actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);

            actual = target2.GetHashCode();
            Assert.AreEqual(expected, actual);

            pattern2 = new PDFNumber[] { (PDFNumber)1.0, (PDFNumber)1.0 }; //changed number
            len2 = (PDFNumber)3.0;
            target2 = new PDFDash(pattern2, len2);
            actual = target2.GetHashCode();
            Assert.AreNotEqual(actual, expected);

            pattern2 = new PDFNumber[] { (PDFNumber)1.0, (PDFNumber)2.0 };
            len2 = (PDFNumber)4.0; //changed number
            target2 = new PDFDash(pattern2, len2);
            actual = target2.GetHashCode();
            Assert.AreNotEqual(actual, expected);

            pattern2 = new PDFNumber[] { (PDFNumber)1.0, (PDFNumber)2.0, (PDFNumber)3.0 };
            len2 = (PDFNumber)3.0;
            target2 = new PDFDash(pattern2, len2);
            actual = target2.GetHashCode();
            Assert.AreNotEqual(actual, expected);
        }

        

        /// <summary>
        ///A test for Parse
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void Parse_Test()
        {
            string dashpattern = "[1, 2] 3"; 
            PDFDash actual = PDFDash.Parse(dashpattern);
            
            int[] pattern = new int[] { 1, 2 };
            int len = 3;
            PDFDash expected = new PDFDash(pattern, len);

            Assert.AreEqual(expected, actual);

            dashpattern = "[1 3]3"; 
            actual = PDFDash.Parse(dashpattern);

            Assert.AreNotEqual(expected, actual);
        }

        

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void ToString_Test()
        {
            int[] pattern = new int[] { 1, 2 };
            int len = 3;
            PDFDash target = new PDFDash(pattern, len);

            string expected = "[1 2] 3"; 
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for None
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void None_Test()
        {
            PDFDash actual;
            actual = PDFDash.None;
            //Should be empty dash - [] 0;
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Pattern);
            Assert.AreEqual(actual.Pattern.Length, 0);
            Assert.AreEqual(actual.Phase, (PDFNumber)0);
        }

        /// <summary>
        ///A test for Pattern
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void Pattern_Test()
        {
            int[] pattern = new int[] { 1, 2 };
            int len = 3;
            PDFDash target = new PDFDash(pattern, len);
            
            Assert.AreEqual(target.Pattern.Length, pattern.Length);
            for (int i = 0; i < pattern.Length; i++)
            {
                Assert.AreEqual(target.Pattern[i], (PDFNumber)pattern[i]);
            }
            
        }

        /// <summary>
        ///A test for Phase
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void Phase_Test()
        {
            int[] pattern = new int[] { 1, 2 };
            int len = 3;
            PDFDash target = new PDFDash(pattern, len);

            Assert.AreEqual(target.Phase, (PDFNumber)len);
        }
    }
}
