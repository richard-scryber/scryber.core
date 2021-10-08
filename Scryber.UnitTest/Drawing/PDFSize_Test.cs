using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFSize_Test and is intended
    ///to contain all PDFSize_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFSize_Test
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
        ///A test for PDFSize Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFSizeConstructor_Test()
        {
            PDFUnit width = new PDFUnit(10,PageUnits.Millimeters);
            PDFUnit height = new PDFUnit(20, PageUnits.Inches);
            PDFSize target = new PDFSize(width, height);

            Assert.AreEqual(width, target.Width);
            Assert.AreEqual(height, target.Height);
        }

        /// <summary>
        ///A test for PDFSize Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFSizeConstructor_Test1()
        {
            double width = 10F; // TODO: Initialize to an appropriate value
            double height = 20F; // TODO: Initialize to an appropriate value
            PDFSize target = new PDFSize(width, height);
            Assert.AreEqual(width, target.Width.PointsValue);
            Assert.AreEqual(height, target.Height.PointsValue);
        }

        /// <summary>
        ///A test for Clone
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Clone_Test()
        {
            PDFUnit width = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit height = new PDFUnit(20, PageUnits.Inches);
            PDFSize target = new PDFSize(width, height);

            PDFSize expected = new PDFSize(width, height);
            PDFSize actual;
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
            PDFUnit width = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit height = new PDFUnit(20, PageUnits.Inches);
            PDFSize target = new PDFSize(width, height);
            PDFSize other = new PDFSize(width, height);

            int expected = 0;
            int actual;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

            height = new PDFUnit(10, PageUnits.Inches);
            target = new PDFSize(width, height);
            expected = -1;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

            height = new PDFUnit(40, PageUnits.Inches);
            target = new PDFSize(width, height);
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
            PDFUnit width = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit height = new PDFUnit(20, PageUnits.Inches);
            PDFSize target = new PDFSize(width, height);
            object obj = null;

            bool expected = false;
            bool actual;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);

            obj = new PDFSize(width, height);
            expected = true;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Equals_Test1()
        {
            PDFUnit width = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit height = new PDFUnit(20, PageUnits.Inches);
            PDFSize target = new PDFSize(width, height);
            PDFSize other = new PDFSize(width, height);

            bool expected = true;
            bool actual;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            height = new PDFUnit(40, PageUnits.Inches);
            other = new PDFSize(width, height);
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void GetHashCode_Test()
        {
            PDFUnit width = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit height = new PDFUnit(20, PageUnits.Inches);
            PDFSize target = new PDFSize(width, height);
            PDFSize other = new PDFSize(width, height);
            int expected = other.GetHashCode();
            int actual;
            actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);

            height = new PDFUnit(40, PageUnits.Inches);
            target = new PDFSize(width, height);
            actual = target.GetHashCode();
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for Subtract
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Subtract_Test()
        {
            PDFUnit width = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit height = new PDFUnit(20, PageUnits.Millimeters);
            PDFSize target = new PDFSize(width, height);

            PDFUnit m = new PDFUnit(5, PageUnits.Millimeters);

            PDFThickness margins = new PDFThickness(m, m, m, m);
            PDFSize expected = new PDFSize(new PDFUnit(0, PageUnits.Millimeters), new PDFUnit(10, PageUnits.Millimeters));
            PDFSize actual;
            actual = target.Subtract(margins);
            Assert.AreEqual(expected.Width.PointsValue, actual.Width.PointsValue);
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 2), Math.Round(actual.Height.PointsValue, 2));
        }

        /// <summary>
        ///A test for System.ICloneable.Clone
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Scryber.dll")]
        [TestCategory("Drawing Structures")]
        public void Clone_Test1()
        {
            PDFUnit width = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit height = new PDFUnit(20, PageUnits.Millimeters);
            ICloneable target = new PDFSize(width, height);
            object expected = target;
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
            PDFUnit width = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit height = new PDFUnit(20, PageUnits.Millimeters);
            PDFSize target = new PDFSize(width, height);
            PDFSize expected = new PDFSize(width.PointsValue, height.PointsValue);
            PDFSize actual;
            actual = target.ToPoints();
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void ToString_Test()
        {
            PDFUnit width = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit height = new PDFUnit(20, PageUnits.Millimeters);
            PDFSize target = new PDFSize(width, height);
            string expected = "[10mm, 20mm]";
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
            PDFUnit width = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit height = new PDFUnit(20, PageUnits.Inches);
            PDFSize target = new PDFSize(width, height);
            PDFSize other = new PDFSize(width, height);

            bool expected = true;
            bool actual;
            actual = (target == other);
            Assert.AreEqual(expected, actual);

            height = new PDFUnit(40, PageUnits.Inches);
            other = new PDFSize(width, height);
            expected = false;
            actual = (target == other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Inequality
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void op_Inequality_Test()
        {
            PDFUnit width = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit height = new PDFUnit(20, PageUnits.Inches);
            PDFSize target = new PDFSize(width, height);
            PDFSize other = new PDFSize(width, height);

            bool expected = false;
            bool actual;
            actual = (target != other);
            Assert.AreEqual(expected, actual);

            height = new PDFUnit(40, PageUnits.Inches);
            other = new PDFSize(width, height);
            expected = true;
            actual = (target != other);
            Assert.AreEqual(expected, actual);

            width = new PDFUnit(20, PageUnits.Millimeters);
            height = new PDFUnit(20, PageUnits.Inches);
            other = new PDFSize(width, height);
            expected = true;
            actual = (target != other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Empty
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Empty_Test()
        {
            PDFUnit width = PDFUnit.Zero;
            PDFUnit height = PDFUnit.Zero;
            PDFSize empty = new PDFSize(width, height);

            PDFSize actual;
            actual = PDFSize.Empty;
            Assert.AreEqual(empty, actual);

        }

        /// <summary>
        ///A test for Height
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Height_Test()
        {
            PDFUnit width = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit height = new PDFUnit(20, PageUnits.Inches);
            PDFSize target = new PDFSize(width, height);

            PDFUnit expected = height;
            Assert.AreEqual(expected, target.Height);

            PDFUnit actual;
            expected = new PDFUnit(40, PageUnits.Inches);
            target.Height = expected;
            actual = target.Height;
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for IsEmpty
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void IsEmpty_Test()
        {
            PDFUnit width = PDFUnit.Zero;
            PDFUnit height = PDFUnit.Zero;
            PDFSize empty = new PDFSize(width, height);
            Assert.IsTrue(empty.IsEmpty);

            PDFSize actual;
            actual = PDFSize.Empty;
            Assert.IsTrue(actual.IsEmpty);

            height = new PDFUnit(10, PageUnits.Millimeters);
            actual = new PDFSize(width, height);
            Assert.IsFalse(actual.IsEmpty);
        }

        /// <summary>
        ///A test for Width
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Width_Test()
        {
            PDFUnit width = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit height = new PDFUnit(20, PageUnits.Inches);
            PDFSize target = new PDFSize(width, height);

            PDFUnit expected = width;
            Assert.AreEqual(expected, target.Width);

            PDFUnit actual;
            expected = new PDFUnit(40, PageUnits.Inches);
            target.Width = expected;
            actual = target.Width;
            Assert.AreEqual(expected, actual);
        }
    }
}
