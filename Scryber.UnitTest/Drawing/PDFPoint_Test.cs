using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFPoint_Test and is intended
    ///to contain all PDFPoint_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFPoint_Test
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
        ///A test for PDFPoint Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFPointConstructor_Test()
        {
            Unit x = new Unit(10,PageUnits.Millimeters); 
            Unit y = new Unit(20,PageUnits.Millimeters);
            Scryber.Drawing.Point target = new Scryber.Drawing.Point(x, y);
            Assert.AreEqual(x, target.X);
            Assert.AreEqual(y, target.Y);
            
        }

        /// <summary>
        ///A test for PDFPoint Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFPointConstructor_Test1()
        {
            double x = 10.0; // TODO: Initialize to an appropriate value
            double y = 20.0; // TODO: Initialize to an appropriate value
            Scryber.Drawing.Point target = new Scryber.Drawing.Point(x, y);

            Assert.AreEqual(x, target.X.PointsValue);
            Assert.AreEqual(y, target.Y.PointsValue);
        }

        /// <summary>
        ///A test for Clone
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Clone_Test()
        {
            Scryber.Drawing.Point target = new Scryber.Drawing.Point(10, 20);
            Scryber.Drawing.Point expected = new Scryber.Drawing.Point(10, 20);
            Scryber.Drawing.Point actual;
            actual = target.Clone();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CompareTo
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void CompareTo_Test()
        {
            Scryber.Drawing.Point target = new Scryber.Drawing.Point(10, 20);
            Scryber.Drawing.Point other = new Scryber.Drawing.Point(10, 20);
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

            other = new Scryber.Drawing.Point(20, 20);
            expected = -1;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

            other = new Scryber.Drawing.Point(10, 10);
            expected = 1;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Equals_Test()
        {
            Scryber.Drawing.Point target = new Scryber.Drawing.Point(10, 20);
            Scryber.Drawing.Point other = new Scryber.Drawing.Point(10, 20);
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new Scryber.Drawing.Point(20, 20);
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new Scryber.Drawing.Point(10, 10);
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Equals_Test1()
        {
            Scryber.Drawing.Point target = new Scryber.Drawing.Point(10, 20);
            Scryber.Drawing.Point other = new Scryber.Drawing.Point(10, 20);
            bool expected = true; 
            bool actual;
            actual = Scryber.Drawing.Point.Equals(target, other);
            Assert.AreEqual(expected, actual);

            other = new Scryber.Drawing.Point(20, 20);
            expected = false;
            actual = Scryber.Drawing.Point.Equals(target, other);
            Assert.AreEqual(expected, actual);

            other = new Scryber.Drawing.Point(10, 10);
            expected = false;
            actual = Scryber.Drawing.Point.Equals(target, other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void GetHashCode_Test()
        {
            Scryber.Drawing.Point target = new Scryber.Drawing.Point(10, 20);
            Scryber.Drawing.Point other = new Scryber.Drawing.Point(10, 20);
            int expected = target.GetHashCode();
            int actual = other.GetHashCode();
            Assert.AreEqual(expected, actual);

            other = new Scryber.Drawing.Point(20, 20);
            actual = other.GetHashCode();
            Assert.AreNotEqual(expected, actual);

            other = new Scryber.Drawing.Point(10, 10);
            actual = other.GetHashCode();
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for System.ICloneable.Clone
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Scryber.dll")]
        [TestCategory("Drawing Structures")]
        public void Clone_Test1()
        {
            ICloneable target = new Scryber.Drawing.Point(10, 20);
            object expected = new Scryber.Drawing.Point(10, 20);
            object actual;
            actual = target.Clone();
            Assert.AreEqual(expected, actual);
            
        }

        

        /// <summary>
        ///A test for ToPoints
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void ToPoints_Test()
        {
            Unit x = new Unit(10,PageUnits.Millimeters);
            Unit y = new Unit(20,PageUnits.Inches);

            Scryber.Drawing.Point target = new Scryber.Drawing.Point(x, y);

            Scryber.Drawing.Point actual;
            actual = target.ToPoints();
            Assert.AreEqual(x.PointsValue, actual.X.PointsValue);
            Assert.AreEqual(y.PointsValue, actual.Y.PointsValue);
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void ToString_Test()
        {
            Unit x = new Unit(10, PageUnits.Millimeters);
            Unit y = new Unit(20, PageUnits.Inches);

            Scryber.Drawing.Point target = new Scryber.Drawing.Point(x, y);
            string expected = "[10mm, 20in]";
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for op_Equality
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void op_Equality_Test()
        {
            Scryber.Drawing.Point target = new Scryber.Drawing.Point(10, 20);
            Scryber.Drawing.Point other = new Scryber.Drawing.Point(10, 20);
            bool expected = true;
            bool actual;
            actual = target == other;
            Assert.AreEqual(expected, actual);

            other = new Scryber.Drawing.Point(20, 20);
            expected = false;
            actual = target == other;
            Assert.AreEqual(expected, actual);

            other = new Scryber.Drawing.Point(10, 10);
            expected = false;
            actual = target == other;
            Assert.AreEqual(expected, actual);

            
        }

        /// <summary>
        ///A test for op_Inequality
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void op_Inequality_Test()
        {
            Scryber.Drawing.Point target = new Scryber.Drawing.Point(10, 20);
            Scryber.Drawing.Point other = new Scryber.Drawing.Point(10, 20);
            bool expected = false;
            bool actual;
            actual = target != other;
            Assert.AreEqual(expected, actual);

            other = new Scryber.Drawing.Point(20, 20);
            expected = true;
            actual = target != other;
            Assert.AreEqual(expected, actual);

            other = new Scryber.Drawing.Point(10, 10);
            expected = true;
            actual = target != other;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Empty
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Empty_Test()
        {
            Scryber.Drawing.Point target = new Scryber.Drawing.Point(0, 0);

            Scryber.Drawing.Point actual;
            actual = Scryber.Drawing.Point.Empty;
            Assert.AreEqual(target, actual);
            
        }

        /// <summary>
        ///A test for IsEmpty
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void IsEmpty_Test()
        {
            Scryber.Drawing.Point target = new Scryber.Drawing.Point(0, 0);
            bool expected = true;
            bool actual;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual);

            target = new Scryber.Drawing.Point(10, 0);
            actual = target.IsEmpty;
            expected = false;
            Assert.AreEqual(expected, actual);

            target = new Scryber.Drawing.Point(0, 10);
            actual = target.IsEmpty;
            expected = false;
            Assert.AreEqual(expected, actual);

            target = new Scryber.Drawing.Point(-10, 10);
            actual = target.IsEmpty;
            expected = false;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for X
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void X_Test()
        {
            Scryber.Drawing.Point target = new Scryber.Drawing.Point(10, 20);
            Unit expected = new Unit(10);
            Unit actual;
            actual = target.X;

            Assert.AreEqual(expected, actual);

            expected = new Unit(20);
            target.X = expected;
            actual = target.X;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Y
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Y_Test()
        {
            Scryber.Drawing.Point target = new Scryber.Drawing.Point(10, 20);
            Unit expected = new Unit(20);
            Unit actual;
            actual = target.Y;

            Assert.AreEqual(expected, actual);

            expected = new Unit(10);
            target.Y = expected;
            actual = target.Y;

            Assert.AreEqual(expected, actual);
        }
    }
}
