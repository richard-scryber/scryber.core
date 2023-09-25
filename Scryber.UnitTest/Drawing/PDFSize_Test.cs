using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFSize_Test and is intended
    ///to contain all PDFSize_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFSize_Test
    {


        private TestContext _testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }


        /// <summary>
        ///A test for PDFSize Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFSizeConstructor_Test()
        {
            Unit width = new Unit(10,PageUnits.Millimeters);
            Unit height = new Unit(20, PageUnits.Inches);
            Scryber.Drawing.Size target = new Scryber.Drawing.Size(width, height);

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
            Scryber.Drawing.Size target = new Scryber.Drawing.Size(width, height);
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
            Unit width = new Unit(10, PageUnits.Millimeters);
            Unit height = new Unit(20, PageUnits.Inches);
            Scryber.Drawing.Size target = new Scryber.Drawing.Size(width, height);

            Scryber.Drawing.Size expected = new Scryber.Drawing.Size(width, height);
            Scryber.Drawing.Size actual;
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
            Unit width = new Unit(10, PageUnits.Millimeters);
            Unit height = new Unit(20, PageUnits.Inches);
            Scryber.Drawing.Size target = new Scryber.Drawing.Size(width, height);
            Scryber.Drawing.Size other = new Scryber.Drawing.Size(width, height);

            int expected = 0;
            int actual;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

            height = new Unit(10, PageUnits.Inches);
            target = new Scryber.Drawing.Size(width, height);
            expected = -1;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

            height = new Unit(40, PageUnits.Inches);
            target = new Scryber.Drawing.Size(width, height);
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
            Unit width = new Unit(10, PageUnits.Millimeters);
            Unit height = new Unit(20, PageUnits.Inches);
            Scryber.Drawing.Size target = new Scryber.Drawing.Size(width, height);
            object obj = null;

            bool expected = false;
            bool actual;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);

            obj = new Scryber.Drawing.Size(width, height);
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
            Unit width = new Unit(10, PageUnits.Millimeters);
            Unit height = new Unit(20, PageUnits.Inches);
            Scryber.Drawing.Size target = new Scryber.Drawing.Size(width, height);
            Scryber.Drawing.Size other = new Scryber.Drawing.Size(width, height);

            bool expected = true;
            bool actual;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            height = new Unit(40, PageUnits.Inches);
            other = new Scryber.Drawing.Size(width, height);
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
            Unit width = new Unit(10, PageUnits.Millimeters);
            Unit height = new Unit(20, PageUnits.Inches);
            Scryber.Drawing.Size target = new Scryber.Drawing.Size(width, height);
            Scryber.Drawing.Size other = new Scryber.Drawing.Size(width, height);
            int expected = other.GetHashCode();
            int actual;
            actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);

            height = new Unit(40, PageUnits.Inches);
            target = new Scryber.Drawing.Size(width, height);
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
            Unit width = new Unit(10, PageUnits.Millimeters);
            Unit height = new Unit(20, PageUnits.Millimeters);
            Scryber.Drawing.Size target = new Scryber.Drawing.Size(width, height);

            Unit m = new Unit(5, PageUnits.Millimeters);

            Thickness margins = new Thickness(m, m, m, m);
            Scryber.Drawing.Size expected = new Scryber.Drawing.Size(new Unit(0, PageUnits.Millimeters), new Unit(10, PageUnits.Millimeters));
            Scryber.Drawing.Size actual;
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
            Unit width = new Unit(10, PageUnits.Millimeters);
            Unit height = new Unit(20, PageUnits.Millimeters);
            ICloneable target = new Scryber.Drawing.Size(width, height);
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
            Unit width = new Unit(10, PageUnits.Millimeters);
            Unit height = new Unit(20, PageUnits.Millimeters);
            Scryber.Drawing.Size target = new Scryber.Drawing.Size(width, height);
            Scryber.Drawing.Size expected = new Scryber.Drawing.Size(width.PointsValue, height.PointsValue);
            Scryber.Drawing.Size actual;
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
            Unit width = new Unit(10, PageUnits.Millimeters);
            Unit height = new Unit(20, PageUnits.Millimeters);
            Scryber.Drawing.Size target = new Scryber.Drawing.Size(width, height);
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
            Unit width = new Unit(10, PageUnits.Millimeters);
            Unit height = new Unit(20, PageUnits.Inches);
            Scryber.Drawing.Size target = new Scryber.Drawing.Size(width, height);
            Scryber.Drawing.Size other = new Scryber.Drawing.Size(width, height);

            bool expected = true;
            bool actual;
            actual = (target == other);
            Assert.AreEqual(expected, actual);

            height = new Unit(40, PageUnits.Inches);
            other = new Scryber.Drawing.Size(width, height);
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
            Unit width = new Unit(10, PageUnits.Millimeters);
            Unit height = new Unit(20, PageUnits.Inches);
            Scryber.Drawing.Size target = new Scryber.Drawing.Size(width, height);
            Scryber.Drawing.Size other = new Scryber.Drawing.Size(width, height);

            bool expected = false;
            bool actual;
            actual = (target != other);
            Assert.AreEqual(expected, actual);

            height = new Unit(40, PageUnits.Inches);
            other = new Scryber.Drawing.Size(width, height);
            expected = true;
            actual = (target != other);
            Assert.AreEqual(expected, actual);

            width = new Unit(20, PageUnits.Millimeters);
            height = new Unit(20, PageUnits.Inches);
            other = new Scryber.Drawing.Size(width, height);
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
            Unit width = Unit.Zero;
            Unit height = Unit.Zero;
            Scryber.Drawing.Size empty = new Scryber.Drawing.Size(width, height);

            Scryber.Drawing.Size actual;
            actual = Scryber.Drawing.Size.Empty;
            Assert.AreEqual(empty, actual);

        }

        /// <summary>
        ///A test for Height
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Height_Test()
        {
            Unit width = new Unit(10, PageUnits.Millimeters);
            Unit height = new Unit(20, PageUnits.Inches);
            Scryber.Drawing.Size target = new Scryber.Drawing.Size(width, height);

            Unit expected = height;
            Assert.AreEqual(expected, target.Height);

            Unit actual;
            expected = new Unit(40, PageUnits.Inches);
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
            Unit width = Unit.Zero;
            Unit height = Unit.Zero;
            Scryber.Drawing.Size empty = new Scryber.Drawing.Size(width, height);
            Assert.IsTrue(empty.IsEmpty);

            Scryber.Drawing.Size actual;
            actual = Scryber.Drawing.Size.Empty;
            Assert.IsTrue(actual.IsEmpty);

            height = new Unit(10, PageUnits.Millimeters);
            actual = new Scryber.Drawing.Size(width, height);
            Assert.IsFalse(actual.IsEmpty);
        }

        /// <summary>
        ///A test for Width
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Width_Test()
        {
            Unit width = new Unit(10, PageUnits.Millimeters);
            Unit height = new Unit(20, PageUnits.Inches);
            Scryber.Drawing.Size target = new Scryber.Drawing.Size(width, height);

            Unit expected = width;
            Assert.AreEqual(expected, target.Width);

            Unit actual;
            expected = new Unit(40, PageUnits.Inches);
            target.Width = expected;
            actual = target.Width;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IsRelative
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void IsRelative_Test()
        {
            Size target = new Size(10, 20);
            bool expected = false;
            bool actual;

            actual = target.IsRelative;
            Assert.AreEqual(expected, actual);


            target = new Size(new Unit(20, PageUnits.ViewPortWidth), 30);
            expected = true;
            actual = target.IsRelative;
            Assert.AreEqual(expected, actual);

            target = new Size(40, new Unit(50, PageUnits.ViewPortWidth));
            expected = true;
            actual = target.IsRelative;
            Assert.AreEqual(expected, actual);

            target = new Size(new Unit(20, PageUnits.ViewPortWidth), new Unit(30, PageUnits.ViewPortHeight));
            expected = true;
            actual = target.IsRelative;
            Assert.AreEqual(expected, actual);


        }
    }
}
