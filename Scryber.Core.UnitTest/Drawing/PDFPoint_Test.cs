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
            PDFUnit x = new PDFUnit(10,PageUnits.Millimeters); 
            PDFUnit y = new PDFUnit(20,PageUnits.Millimeters);
            PDFPoint target = new PDFPoint(x, y);
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
            PDFPoint target = new PDFPoint(x, y);

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
            PDFPoint target = new PDFPoint(10, 20);
            PDFPoint expected = new PDFPoint(10, 20);
            PDFPoint actual;
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
            PDFPoint target = new PDFPoint(10, 20);
            PDFPoint other = new PDFPoint(10, 20);
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

            other = new PDFPoint(20, 20);
            expected = -1;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

            other = new PDFPoint(10, 10);
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
            PDFPoint target = new PDFPoint(10, 20);
            PDFPoint other = new PDFPoint(10, 20);
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new PDFPoint(20, 20);
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new PDFPoint(10, 10);
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
            PDFPoint target = new PDFPoint(10, 20);
            PDFPoint other = new PDFPoint(10, 20);
            bool expected = true; 
            bool actual;
            actual = PDFPoint.Equals(target, other);
            Assert.AreEqual(expected, actual);

            other = new PDFPoint(20, 20);
            expected = false;
            actual = PDFPoint.Equals(target, other);
            Assert.AreEqual(expected, actual);

            other = new PDFPoint(10, 10);
            expected = false;
            actual = PDFPoint.Equals(target, other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void GetHashCode_Test()
        {
            PDFPoint target = new PDFPoint(10, 20);
            PDFPoint other = new PDFPoint(10, 20);
            int expected = target.GetHashCode();
            int actual = other.GetHashCode();
            Assert.AreEqual(expected, actual);

            other = new PDFPoint(20, 20);
            actual = other.GetHashCode();
            Assert.AreNotEqual(expected, actual);

            other = new PDFPoint(10, 10);
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
            ICloneable target = new PDFPoint(10, 20);
            object expected = new PDFPoint(10, 20);
            object actual;
            actual = target.Clone();
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ToDrawing
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void ToDrawing_Test()
        {
            PDFPoint target = new PDFPoint(10, 20);
            PointF expected = new PointF(10,20);
            PointF actual;
            actual = target.ToDrawing();
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
        }

        /// <summary>
        ///A test for ToPoints
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void ToPoints_Test()
        {
            PDFUnit x = new PDFUnit(10,PageUnits.Millimeters);
            PDFUnit y = new PDFUnit(20,PageUnits.Inches);

            PDFPoint target = new PDFPoint(x, y);
            
            PDFPoint actual;
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
            PDFUnit x = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit y = new PDFUnit(20, PageUnits.Inches);

            PDFPoint target = new PDFPoint(x, y);
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
            PDFPoint target = new PDFPoint(10, 20);
            PDFPoint other = new PDFPoint(10, 20);
            bool expected = true;
            bool actual;
            actual = target == other;
            Assert.AreEqual(expected, actual);

            other = new PDFPoint(20, 20);
            expected = false;
            actual = target == other;
            Assert.AreEqual(expected, actual);

            other = new PDFPoint(10, 10);
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
            PDFPoint target = new PDFPoint(10, 20);
            PDFPoint other = new PDFPoint(10, 20);
            bool expected = false;
            bool actual;
            actual = target != other;
            Assert.AreEqual(expected, actual);

            other = new PDFPoint(20, 20);
            expected = true;
            actual = target != other;
            Assert.AreEqual(expected, actual);

            other = new PDFPoint(10, 10);
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
            PDFPoint target = new PDFPoint(0, 0);

            PDFPoint actual;
            actual = PDFPoint.Empty;
            Assert.AreEqual(target, actual);
            
        }

        /// <summary>
        ///A test for IsEmpty
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void IsEmpty_Test()
        {
            PDFPoint target = new PDFPoint(0, 0);
            bool expected = true;
            bool actual;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual);

            target = new PDFPoint(10, 0);
            actual = target.IsEmpty;
            expected = false;
            Assert.AreEqual(expected, actual);

            target = new PDFPoint(0, 10);
            actual = target.IsEmpty;
            expected = false;
            Assert.AreEqual(expected, actual);

            target = new PDFPoint(-10, 10);
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
            PDFPoint target = new PDFPoint(10, 20);
            PDFUnit expected = new PDFUnit(10);
            PDFUnit actual;
            actual = target.X;

            Assert.AreEqual(expected, actual);

            expected = new PDFUnit(20);
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
            PDFPoint target = new PDFPoint(10, 20);
            PDFUnit expected = new PDFUnit(20);
            PDFUnit actual;
            actual = target.Y;

            Assert.AreEqual(expected, actual);

            expected = new PDFUnit(10);
            target.Y = expected;
            actual = target.Y;

            Assert.AreEqual(expected, actual);
        }
    }
}
