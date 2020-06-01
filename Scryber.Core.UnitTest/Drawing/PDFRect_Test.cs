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
            PDFPoint location = new PDFPoint(10,20);
            PDFSize size = new PDFSize(30, 40);
            PDFRect target = new PDFRect(location, size);

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
            PDFUnit x = new PDFUnit(10);
            PDFUnit y = new PDFUnit(20); 
            PDFUnit width = new PDFUnit(30);
            PDFUnit height = new PDFUnit(40);
            PDFRect target = new PDFRect(x, y, width, height);

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
            PDFRect target = new PDFRect(x, y, width, height);

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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFRect expected = new PDFRect(10, 20, 30, 40);
            PDFRect actual;
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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFRect other = new PDFRect(10, 20, 30, 40);
            int expected = 0;
            int actual;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

            other = new PDFRect(20, 30, 40, 50);
            expected = -1;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

            other = new PDFRect(0, 10, 20, 30);
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
            PDFRect target = new PDFRect(10, 20, 30, 40);

            PDFPoint point = new PDFPoint(15, 25);
            bool expected = true;
            bool actual;
            actual = target.Contains(point);
            Assert.AreEqual(expected, actual);

            point = new PDFPoint(5, 25);
            expected = false;
            actual = target.Contains(point);
            Assert.AreEqual(expected, actual);

            point = new PDFPoint(45, 25);
            expected = false;
            actual = target.Contains(point);
            Assert.AreEqual(expected, actual);

            point = new PDFPoint(15, 15);
            expected = false;
            actual = target.Contains(point);
            Assert.AreEqual(expected, actual);

            point = new PDFPoint(15, 65);
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
            PDFRect target = new PDFRect(10, 20, 30, 40);

            PDFPoint point = new PDFPoint(15, 25);
            bool expected = true;
            bool actual;
            actual = target.Contains(point.X, point.Y);
            Assert.AreEqual(expected, actual);

            point = new PDFPoint(5, 25);
            expected = false;
            actual = target.Contains(point.X, point.Y);
            Assert.AreEqual(expected, actual);

            point = new PDFPoint(45, 25);
            expected = false;
            actual = target.Contains(point.X, point.Y);
            Assert.AreEqual(expected, actual);

            point = new PDFPoint(15, 15);
            expected = false;
            actual = target.Contains(point.X, point.Y);
            Assert.AreEqual(expected, actual);

            point = new PDFPoint(15, 65);
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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFRect other = new PDFRect(10, 20, 30, 40);
            bool expected = true;
            bool actual;
            actual = PDFRect.Equal(target, other);
            Assert.AreEqual(expected, actual);

            other = new PDFRect(20, 30, 40, 50);
            expected = false;
            actual = PDFRect.Equal(target, other);
            Assert.AreEqual(expected, actual);

            other = new PDFRect(0, 10, 20, 30);
            expected = false;
            actual = PDFRect.Equal(target, other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Equals_Test()
        {
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFRect other = new PDFRect(10, 20, 30, 40);
            bool expected = true;
            bool actual;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new PDFRect(20, 30, 40, 50);
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new PDFRect(0, 10, 20, 30);
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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFRect other = new PDFRect(10, 20, 30, 40);

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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFRect other = new PDFRect(10, 20, 30, 40);
            int expected = target.GetHashCode();

            int actual;
            actual = other.GetHashCode();
            Assert.AreEqual(expected, actual);

            other = new PDFRect(20, 20, 30, 40);
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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFSize size = new PDFSize(5,10);
            PDFRect other = target.Inflate(size);

            Assert.AreEqual(10.0, other.X);
            Assert.AreEqual(20.0, other.Y);
            Assert.AreEqual(35.0, other.Width);
            Assert.AreEqual(50.0, other.Height);

            size = new PDFSize(-5, -10);
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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFSize size = new PDFSize(5, 10);
            PDFRect other = PDFRect.Inflate(target, size.Width, size.Height);

            Assert.AreEqual(10.0, other.X);
            Assert.AreEqual(20.0, other.Y);
            Assert.AreEqual(35.0, other.Width);
            Assert.AreEqual(50.0, other.Height);

            size = new PDFSize(-5, -10);
            other = PDFRect.Inflate(target, size.Width, size.Height);

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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFSize size = new PDFSize(5, 10);
            PDFRect other = target.Inflate(size.Width, size.Height);

            Assert.AreEqual(10.0, other.X);
            Assert.AreEqual(20.0, other.Y);
            Assert.AreEqual(35.0, other.Width);
            Assert.AreEqual(50.0, other.Height);

            size = new PDFSize(-5, -10);
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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFThickness thickness = new PDFThickness(1, 2, 3, 4);
            PDFRect expected = new PDFRect(12, 21, 24, 36);
            PDFRect actual;
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
            PDFRect a = new PDFRect(10, 20, 30, 40);
            PDFRect b = new PDFRect(10, 20, 30, 40);

            PDFRect expected = new PDFRect(10, 20, 30, 40);
            PDFRect actual;
            actual = PDFRect.Intersect(a, b);
            Assert.AreEqual(expected, actual);

            b = new PDFRect(15, 25, 30, 40);
            expected = new PDFRect(15, 25, 25, 35);
            actual = PDFRect.Intersect(a, b);
            Assert.AreEqual(expected, actual);

            b = new PDFRect(0, 0, 60, 70);
            expected = new PDFRect(10, 20, 30, 40);
            actual = PDFRect.Intersect(a, b);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Intersect
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Intersect_Test1()
        {
            PDFRect a = new PDFRect(10, 20, 30, 40);
            PDFRect b = new PDFRect(10, 20, 30, 40);

            PDFRect expected = new PDFRect(10, 20, 30, 40);
            PDFRect actual;
            actual = a.Intersect(b);
            Assert.AreEqual(expected, actual);

            b = new PDFRect(15, 25, 30, 40);
            expected = new PDFRect(15, 25, 25, 35);
            actual = a.Intersect(b);
            Assert.AreEqual(expected, actual);

            b = new PDFRect(0, 0, 60, 70);
            expected = new PDFRect(10, 20, 30, 40);
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
            PDFRect a = new PDFRect(10, 20, 30, 40);
            PDFRect b = new PDFRect(10, 20, 30, 40);

            bool expected = true;
            bool actual;
            actual = a.IntersectsWith(b);
            Assert.AreEqual(expected, actual);

            b = new PDFRect(15, 25, 30, 40);
            expected = true;
            actual = a.IntersectsWith(b);
            Assert.AreEqual(expected, actual);

            b = new PDFRect(0, 0, 10, 20);
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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFPoint pt = new PDFPoint(5, 10);
            PDFRect other = target.Offset(pt.X, pt.Y);

            Assert.AreEqual(15.0, other.X);
            Assert.AreEqual(30.0, other.Y);
            Assert.AreEqual(30.0, other.Width);
            Assert.AreEqual(40.0, other.Height);

            pt = new PDFPoint(-5, -10);
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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFPoint pt = new PDFPoint(5, 10);
            PDFRect other = target.Offset(pt);

            Assert.AreEqual(15.0, other.X);
            Assert.AreEqual(30.0, other.Y);
            Assert.AreEqual(30.0, other.Width);
            Assert.AreEqual(40.0, other.Height);

            pt = new PDFPoint(-5, -10);
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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFThickness thickness = new PDFThickness(1, 2, 3, 4);
            PDFRect expected = new PDFRect(8, 19, 36, 44);
            PDFRect actual;
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
            ICloneable target = new PDFRect(10, 20, 30, 40);
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
            PDFRect target = new PDFRect(10, new PDFUnit(20, PageUnits.Millimeters), 30, 40);
            string expected = "[10, 20mm, 30, 40]";
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
            PDFRect a = new PDFRect(10, 20, 30, 40);
            PDFRect b = new PDFRect(15, 25, 60, 60);
            PDFRect expected = new PDFRect(10, 20, 65, 65);
            PDFRect actual;
            actual = PDFRect.Union(a, b);
            Assert.AreEqual(expected, actual);

            b = new PDFRect(100, 100, 40, 40);
            expected = new PDFRect(10, 20, 130, 120);
            actual = PDFRect.Union(a, b);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for op_Equality
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void op_Equality_Test()
        {
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFRect other = new PDFRect(10, 20, 30, 40);
            bool expected = true;
            bool actual;
            actual = target == other;
            Assert.AreEqual(expected, actual);

            other = new PDFRect(20, 30, 40, 50);
            expected = false;
            actual = target == other;
            Assert.AreEqual(expected, actual);

            other = new PDFRect(0, 10, 20, 30);
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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFRect other = new PDFRect(10, 20, 30, 40);
            bool expected = false;
            bool actual;
            actual = target != other;
            Assert.AreEqual(expected, actual);

            other = new PDFRect(20, 30, 40, 50);
            expected = true;
            actual = target != other;
            Assert.AreEqual(expected, actual);

            other = new PDFRect(0, 10, 20, 30);
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
            PDFRect actual;
            actual = PDFRect.Empty;

            Assert.AreEqual(PDFUnit.Zero, actual.X);
            Assert.AreEqual(PDFUnit.Zero, actual.Width);
            Assert.AreEqual(PDFUnit.Zero, actual.Y);
            Assert.AreEqual(PDFUnit.Zero, actual.Height);
        }

        /// <summary>
        ///A test for Height
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Height_Test()
        {
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFUnit expected = new PDFUnit(40);
            Assert.AreEqual(expected, target.Height);

            expected = new PDFUnit(100, PageUnits.Millimeters);
            PDFUnit actual;
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
            PDFRect target = PDFRect.Empty;
            bool expected = true;
            bool actual;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual);

            target = new PDFRect(10, 20, 30, 40);
            expected = false;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual);

            target = new PDFRect(0, 0, 0, 40);
            expected = false;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual);

            target = new PDFRect(0, 0, 30, 0);
            expected = false;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual);

            target = new PDFRect(0, 20, 0, 0);
            expected = false;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual);

            target = new PDFRect(10, 0, 0, 0);
            expected = false;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual);

            target = new PDFRect(0, 0, 0, 0);
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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFPoint expected = new PDFPoint(10, 20);
            Assert.AreEqual(expected, target.Location);

            expected = new PDFPoint(new PDFUnit(100, PageUnits.Millimeters), new PDFUnit(50, PageUnits.Millimeters));
            PDFPoint actual;
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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFSize expected = new PDFSize(30, 40);
            Assert.AreEqual(expected, target.Size);

            expected = new PDFSize(new PDFUnit(100, PageUnits.Millimeters), new PDFUnit(50, PageUnits.Millimeters));
            PDFSize actual;
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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFUnit expected = new PDFUnit(30);
            Assert.AreEqual(expected, target.Width);

            expected = new PDFUnit(100, PageUnits.Millimeters);
            PDFUnit actual;
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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFUnit expected = new PDFUnit(10);
            Assert.AreEqual(expected, target.X);

            expected = new PDFUnit(100, PageUnits.Millimeters);
            PDFUnit actual;
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
            PDFRect target = new PDFRect(10, 20, 30, 40);
            PDFUnit expected = new PDFUnit(20);
            Assert.AreEqual(expected, target.Y);

            expected = new PDFUnit(100, PageUnits.Millimeters);
            PDFUnit actual;
            target.Y = expected;
            actual = target.Y;
            Assert.AreEqual(expected, actual);
        }
    }
}
