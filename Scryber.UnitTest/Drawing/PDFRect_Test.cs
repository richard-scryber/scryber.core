using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFRect_Test and is intended
    ///to contain all PDFRect_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFRect_Test
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
        ///A test for PDFRect Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFRectConstructor_Test()
        {
            Point location = new Point(10,20);
            Size size = new Size(30, 40);
            Rect target = new Rect(location, size);

            Assert.AreEqual(target.Location, location);
            Assert.AreEqual(target.Size, size);
        }

        /// <summary>
        ///A test for PDFRect Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFRectConstructor_Test1()
        {
            Unit x = new Unit(10);
            Unit y = new Unit(20); 
            Unit width = new Unit(30);
            Unit height = new Unit(40);
            Rect target = new Rect(x, y, width, height);

            Assert.AreEqual(x, target.X);
            Assert.AreEqual(y, target.Y);
            Assert.AreEqual(width, target.Width);
            Assert.AreEqual(height, target.Height);
        }

        /// <summary>
        ///A test for PDFRect Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFRectConstructor_Test2()
        {
            double x = 10F; // TODO: Initialize to an appropriate value
            double y = 20F; // TODO: Initialize to an appropriate value
            double width = 30F; // TODO: Initialize to an appropriate value
            double height = 40F; // TODO: Initialize to an appropriate value
            Rect target = new Rect(x, y, width, height);

            Assert.AreEqual(x, target.X.PointsValue);
            Assert.AreEqual(y, target.Y.PointsValue);
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
            Rect target = new Rect(10, 20, 30, 40);
            Rect expected = new Rect(10, 20, 30, 40);
            Rect actual;
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
            Rect target = new Rect(10, 20, 30, 40);
            Rect other = new Rect(10, 20, 30, 40);
            int expected = 0;
            int actual;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

            other = new Rect(20, 30, 40, 50);
            expected = -1;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

            other = new Rect(0, 10, 20, 30);
            expected = 1;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Contains
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Contains_Test()
        {
            Rect target = new Rect(10, 20, 30, 40);

            Point point = new Point(15, 25);
            bool expected = true;
            bool actual;
            actual = target.Contains(point);
            Assert.AreEqual(expected, actual);

            point = new Point(5, 25);
            expected = false;
            actual = target.Contains(point);
            Assert.AreEqual(expected, actual);

            point = new Point(45, 25);
            expected = false;
            actual = target.Contains(point);
            Assert.AreEqual(expected, actual);

            point = new Point(15, 15);
            expected = false;
            actual = target.Contains(point);
            Assert.AreEqual(expected, actual);

            point = new Point(15, 65);
            expected = false;
            actual = target.Contains(point);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Contains
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Contains_Test1()
        {
            Rect target = new Rect(10, 20, 30, 40);

            Point point = new Point(15, 25);
            bool expected = true;
            bool actual;
            actual = target.Contains(point.X, point.Y);
            Assert.AreEqual(expected, actual);

            point = new Point(5, 25);
            expected = false;
            actual = target.Contains(point.X, point.Y);
            Assert.AreEqual(expected, actual);

            point = new Point(45, 25);
            expected = false;
            actual = target.Contains(point.X, point.Y);
            Assert.AreEqual(expected, actual);

            point = new Point(15, 15);
            expected = false;
            actual = target.Contains(point.X, point.Y);
            Assert.AreEqual(expected, actual);

            point = new Point(15, 65);
            expected = false;
            actual = target.Contains(point.X, point.Y);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Equal
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Equal_Test()
        {
            Rect target = new Rect(10, 20, 30, 40);
            Rect other = new Rect(10, 20, 30, 40);
            bool expected = true;
            bool actual;
            actual = Rect.Equal(target, other);
            Assert.AreEqual(expected, actual);

            other = new Rect(20, 30, 40, 50);
            expected = false;
            actual = Rect.Equal(target, other);
            Assert.AreEqual(expected, actual);

            other = new Rect(0, 10, 20, 30);
            expected = false;
            actual = Rect.Equal(target, other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Equals_Test()
        {
            Rect target = new Rect(10, 20, 30, 40);
            Rect other = new Rect(10, 20, 30, 40);
            bool expected = true;
            bool actual;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new Rect(20, 30, 40, 50);
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new Rect(0, 10, 20, 30);
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
            Rect target = new Rect(10, 20, 30, 40);
            Rect other = new Rect(10, 20, 30, 40);

            object obj = null;
            bool expected = false;
            bool actual;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);

            obj = other;
            expected = true;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void GetHashCode_Test()
        {
            Rect target = new Rect(10, 20, 30, 40);
            Rect other = new Rect(10, 20, 30, 40);
            int expected = target.GetHashCode();

            int actual;
            actual = other.GetHashCode();
            Assert.AreEqual(expected, actual);

            other = new Rect(20, 20, 30, 40);
            actual = other.GetHashCode();
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for Inflate
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Inflate_Test()
        {
            Rect target = new Rect(10, 20, 30, 40);
            Size size = new Size(5,10);
            Rect other = target.Inflate(size);

            Assert.AreEqual(10.0, other.X);
            Assert.AreEqual(20.0, other.Y);
            Assert.AreEqual(35.0, other.Width);
            Assert.AreEqual(50.0, other.Height);

            size = new Size(-5, -10);
            other = target.Inflate(size);

            Assert.AreEqual(10.0, other.X);
            Assert.AreEqual(20.0, other.Y);
            Assert.AreEqual(25.0, other.Width);
            Assert.AreEqual(30.0, other.Height);
        }

        /// <summary>
        ///A test for Inflate
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Inflate_Test1()
        {
            Rect target = new Rect(10, 20, 30, 40);
            Size size = new Size(5, 10);
            Rect other = Rect.Inflate(target, size.Width, size.Height);

            Assert.AreEqual(10.0, other.X);
            Assert.AreEqual(20.0, other.Y);
            Assert.AreEqual(35.0, other.Width);
            Assert.AreEqual(50.0, other.Height);

            size = new Size(-5, -10);
            other = Rect.Inflate(target, size.Width, size.Height);

            Assert.AreEqual(10.0, other.X);
            Assert.AreEqual(20.0, other.Y);
            Assert.AreEqual(25.0, other.Width);
            Assert.AreEqual(30.0, other.Height);
        }

        /// <summary>
        ///A test for Inflate
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Inflate_Test2()
        {
            Rect target = new Rect(10, 20, 30, 40);
            Size size = new Size(5, 10);
            Rect other = target.Inflate(size.Width, size.Height);

            Assert.AreEqual(10.0, other.X);
            Assert.AreEqual(20.0, other.Y);
            Assert.AreEqual(35.0, other.Width);
            Assert.AreEqual(50.0, other.Height);

            size = new Size(-5, -10);
            other = target.Inflate(size.Width, size.Height);

            Assert.AreEqual(10.0, other.X);
            Assert.AreEqual(20.0, other.Y);
            Assert.AreEqual(25.0, other.Width);
            Assert.AreEqual(30.0, other.Height);
        }

        /// <summary>
        ///A test for Inset
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Inset_Test()
        {
            Rect target = new Rect(10, 20, 30, 40);
            Thickness thickness = new Thickness(1, 2, 3, 4);

            Rect expected = new Rect(14, 21, 24, 36);
            Rect actual;
            actual = target.Inset(thickness);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Intersect
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Intersect_Test()
        {
            Rect a = new Rect(10, 20, 30, 40);
            Rect b = new Rect(10, 20, 30, 40);

            Rect expected = new Rect(10, 20, 30, 40);
            Rect actual;
            actual = Rect.Intersect(a, b);
            Assert.AreEqual(expected, actual);

            b = new Rect(15, 25, 30, 40);
            expected = new Rect(15, 25, 25, 35);
            actual = Rect.Intersect(a, b);
            Assert.AreEqual(expected, actual);

            b = new Rect(0, 0, 60, 70);
            expected = new Rect(10, 20, 30, 40);
            actual = Rect.Intersect(a, b);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Intersect
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Intersect_Test1()
        {
            Rect a = new Rect(10, 20, 30, 40);
            Rect b = new Rect(10, 20, 30, 40);

            Rect expected = new Rect(10, 20, 30, 40);
            Rect actual;
            actual = a.Intersect(b);
            Assert.AreEqual(expected, actual);

            b = new Rect(15, 25, 30, 40);
            expected = new Rect(15, 25, 25, 35);
            actual = a.Intersect(b);
            Assert.AreEqual(expected, actual);

            b = new Rect(0, 0, 60, 70);
            expected = new Rect(10, 20, 30, 40);
            actual = a.Intersect(b);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for IntersectsWith
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void IntersectsWith_Test()
        {
            Rect a = new Rect(10, 20, 30, 40);
            Rect b = new Rect(10, 20, 30, 40);

            bool expected = true;
            bool actual;
            actual = a.IntersectsWith(b);
            Assert.AreEqual(expected, actual);

            b = new Rect(15, 25, 30, 40);
            expected = true;
            actual = a.IntersectsWith(b);
            Assert.AreEqual(expected, actual);

            b = new Rect(0, 0, 10, 20);
            expected = false;
            actual = a.IntersectsWith(b);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Offset
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Offset_Test()
        {
            Rect target = new Rect(10, 20, 30, 40);
            Point pt = new Point(5, 10);
            Rect other = target.Offset(pt.X, pt.Y);

            Assert.AreEqual(15.0, other.X);
            Assert.AreEqual(30.0, other.Y);
            Assert.AreEqual(30.0, other.Width);
            Assert.AreEqual(40.0, other.Height);

            pt = new Point(-5, -10);
            other = target.Offset(pt.X, pt.Y);

            Assert.AreEqual(5.0, other.X);
            Assert.AreEqual(10.0, other.Y);
            Assert.AreEqual(30.0, other.Width);
            Assert.AreEqual(40.0, other.Height);
        }

        /// <summary>
        ///A test for Offset
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Offset_Test1()
        {
            Rect target = new Rect(10, 20, 30, 40);
            Point pt = new Point(5, 10);
            Rect other = target.Offset(pt);

            Assert.AreEqual(15.0, other.X);
            Assert.AreEqual(30.0, other.Y);
            Assert.AreEqual(30.0, other.Width);
            Assert.AreEqual(40.0, other.Height);

            pt = new Point(-5, -10);
            other = target.Offset(pt);

            Assert.AreEqual(5.0, other.X);
            Assert.AreEqual(10.0, other.Y);
            Assert.AreEqual(30.0, other.Width);
            Assert.AreEqual(40.0, other.Height);
        }

        /// <summary>
        ///A test for Outset
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Outset_Test()
        {
            Rect target = new Rect(10, 20, 30, 40);
            Thickness thickness = new Thickness(1, 2, 3, 4);
            Rect expected = new Rect(6, 19, 36, 44);
            Rect actual;
            actual = target.Outset(thickness);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for System.ICloneable.Clone
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Scryber.dll")]
        [TestCategory("Drawing Structures")]
        public void Clone_Test1()
        {
            ICloneable target = new Rect(10, 20, 30, 40);
            object expected = target;
            object actual;
            actual = target.Clone();
            Assert.AreEqual(expected, actual);
           
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void ToString_Test()
        {
            Rect target = new Rect(10, new Unit(20, PageUnits.Millimeters), 30, 40);
            string expected = "[10pt, 20mm, 30pt, 40pt]";
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Union
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Union_Test()
        {
            Rect a = new Rect(10, 20, 30, 40);
            Rect b = new Rect(15, 25, 60, 60);
            Rect expected = new Rect(10, 20, 65, 65);
            Rect actual;
            actual = Rect.Union(a, b);
            Assert.AreEqual(expected, actual);

            b = new Rect(100, 100, 40, 40);
            expected = new Rect(10, 20, 130, 120);
            actual = Rect.Union(a, b);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Equality
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void op_Equality_Test()
        {
            Rect target = new Rect(10, 20, 30, 40);
            Rect other = new Rect(10, 20, 30, 40);
            bool expected = true;
            bool actual;
            actual = target == other;
            Assert.AreEqual(expected, actual);

            other = new Rect(20, 30, 40, 50);
            expected = false;
            actual = target == other;
            Assert.AreEqual(expected, actual);

            other = new Rect(0, 10, 20, 30);
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
            Rect target = new Rect(10, 20, 30, 40);
            Rect other = new Rect(10, 20, 30, 40);
            bool expected = false;
            bool actual;
            actual = target != other;
            Assert.AreEqual(expected, actual);

            other = new Rect(20, 30, 40, 50);
            expected = true;
            actual = target != other;
            Assert.AreEqual(expected, actual);

            other = new Rect(0, 10, 20, 30);
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
            Rect actual;
            actual = Rect.Empty;

            Assert.AreEqual(Unit.Zero, actual.X);
            Assert.AreEqual(Unit.Zero, actual.Width);
            Assert.AreEqual(Unit.Zero, actual.Y);
            Assert.AreEqual(Unit.Zero, actual.Height);
        }

        /// <summary>
        ///A test for Height
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Height_Test()
        {
            Rect target = new Rect(10, 20, 30, 40);
            Unit expected = new Unit(40);
            Assert.AreEqual(expected, target.Height);

            expected = new Unit(100, PageUnits.Millimeters);
            Unit actual;
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
            Rect target = Rect.Empty;
            bool expected = true;
            bool actual;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual);

            target = new Rect(10, 20, 30, 40);
            expected = false;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual);

            target = new Rect(0, 0, 0, 40);
            expected = false;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual);

            target = new Rect(0, 0, 30, 0);
            expected = false;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual);

            target = new Rect(0, 20, 0, 0);
            expected = false;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual);

            target = new Rect(10, 0, 0, 0);
            expected = false;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual);

            target = new Rect(0, 0, 0, 0);
            expected = true;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Location
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Location_Test()
        {
            Rect target = new Rect(10, 20, 30, 40);
            Point expected = new Point(10, 20);
            Assert.AreEqual(expected, target.Location);

            expected = new Point(new Unit(100, PageUnits.Millimeters), new Unit(50, PageUnits.Millimeters));
            Point actual;
            target.Location = expected;
            actual = target.Location;
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Size
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Size_Test()
        {
            Rect target = new Rect(10, 20, 30, 40);
            Size expected = new Size(30, 40);
            Assert.AreEqual(expected, target.Size);

            expected = new Size(new Unit(100, PageUnits.Millimeters), new Unit(50, PageUnits.Millimeters));
            Size actual;
            target.Size = expected;
            actual = target.Size;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Width
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Width_Test()
        {
            Rect target = new Rect(10, 20, 30, 40);
            Unit expected = new Unit(30);
            Assert.AreEqual(expected, target.Width);

            expected = new Unit(100, PageUnits.Millimeters);
            Unit actual;
            target.Width = expected;
            actual = target.Width;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for X
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void X_Test()
        {
            Rect target = new Rect(10, 20, 30, 40);
            Unit expected = new Unit(10);
            Assert.AreEqual(expected, target.X);

            expected = new Unit(100, PageUnits.Millimeters);
            Unit actual;
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
            Rect target = new Rect(10, 20, 30, 40);
            Unit expected = new Unit(20);
            Assert.AreEqual(expected, target.Y);

            expected = new Unit(100, PageUnits.Millimeters);
            Unit actual;
            target.Y = expected;
            actual = target.Y;
            Assert.AreEqual(expected, actual);
        }
    }
}
