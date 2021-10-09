using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.PDF.Native;
using System.CodeDom;
using Scryber.Styles.Parsing.Typed;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for Dash_Test and is intended
    ///to contain all Dash_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class Dash_Test
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

        


        /// <summary>
        ///A test for Dash Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void DashConstructor_Test()
        {
            int[] pattern = new int[] { 1, 2 };
            int len = 3;
            Dash target = new Dash(pattern, len);

            Assert.IsNotNull(target.Pattern);
            Assert.AreEqual(target.Pattern.Length, 2);
            Assert.AreEqual(target.Pattern[0], pattern[0]);
            Assert.AreEqual(target.Pattern[1], pattern[1]);
            Assert.AreEqual(target.Phase, len);

        }

        /// <summary>
        ///A test for Dash Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void DashConstructor_Test1()
        {
            int[] pattern = new int[] { 1, 2 };
            int len = 3;
            Dash target = new Dash(pattern, len);
            
            Assert.IsNotNull(target.Pattern);
            Assert.AreEqual(target.Pattern.Length, 2);
            Assert.AreEqual(target.Pattern[0], pattern[0]);
            Assert.AreEqual(target.Pattern[1], pattern[1]);
            Assert.AreEqual(target.Phase, len);
        }

        /// <summary>
        ///A test for Dash Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void DashConstructor_Test2()
        {
            Dash target = new Dash(); //empty constructor - should result in [] 0 - solid line
            Assert.IsNotNull(target.Pattern);
            Assert.AreEqual(target.Pattern.Length, 0);
            Assert.AreEqual(target.Phase, 0);
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
            Dash target = new Dash(pattern, len);

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
            Dash target = new Dash(pattern, len);

            int[] pattern2 = new int[] { 1, 2 };
            int len2 = 3;
            Dash target2 = new Dash(pattern2, len2);

            bool actual;
            actual = target.Equals(target2);
            Assert.IsTrue(actual);

            pattern2 = new int[] { 1, 2 };
            len2 = 4; // changed phase
            target2 = new Dash(pattern2, len2);
            actual = target.Equals(target2);
            Assert.IsFalse(actual);

            pattern2 = new int[] { 1, 1 }; //changed number
            len2 = 3;
            target2 = new Dash(pattern2, len2);
            actual = target.Equals(target2);
            Assert.IsFalse(actual);

            pattern2 = new int[] { 1, 2, 3 }; // Added number
            len2 = 3;
            target2 = new Dash(pattern2, len2);
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
            Dash target = new Dash(pattern, len);

            int[] pattern2 = new int[] { 1, 2 };
            int len2 = 3;
            Dash target2 = new Dash(pattern2, len2);

            int expected = target.GetHashCode();
            int actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);

            actual = target2.GetHashCode();
            Assert.AreEqual(expected, actual);

            pattern2 = new int[] { 1, 1 }; //changed number
            len2 = 3;
            target2 = new Dash(pattern2, len2);
            actual = target2.GetHashCode();
            Assert.AreNotEqual(actual, expected);

            pattern2 = new int[] { 1, 2 };
            len2 = 4; //changed number
            target2 = new Dash(pattern2, len2);
            actual = target2.GetHashCode();
            Assert.AreNotEqual(actual, expected);

            pattern2 = new int[] { 1, 2, 3 };
            len2 = 3;
            target2 = new Dash(pattern2, len2);
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
            Dash actual = Dash.Parse(dashpattern);
            
            int[] pattern = new int[] { 1, 2 };
            int len = 3;
            Dash expected = new Dash(pattern, len);

            Assert.AreEqual(expected, actual);

            dashpattern = "[1 3]3"; 
            actual = Dash.Parse(dashpattern);

            Assert.AreNotEqual(expected, actual);

            dashpattern = "dot";
            actual = Dash.Parse(dashpattern);

            expected = Dashes.Dot;

            Assert.AreEqual(expected, actual);

            dashpattern = "1 3"; // no phase, no brakets

            actual = Dash.Parse(dashpattern);
            expected = new Dash(new int[] { 1, 3 }, 4);

            Assert.AreEqual(expected, actual);
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
            Dash target = new Dash(pattern, len);

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
            Dash actual;
            actual = Dash.None;
            //Should be empty dash - [] 0;
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Pattern);
            Assert.AreEqual(actual.Pattern.Length, 0);
            Assert.AreEqual(actual.Phase, 0);
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
            Dash target = new Dash(pattern, len);
            
            Assert.AreEqual(target.Pattern.Length, pattern.Length);
            for (int i = 0; i < pattern.Length; i++)
            {
                Assert.AreEqual(target.Pattern[i], pattern[i]);
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
            Dash target = new Dash(pattern, len);

            Assert.AreEqual(target.Phase, len);
        }
    }
}
