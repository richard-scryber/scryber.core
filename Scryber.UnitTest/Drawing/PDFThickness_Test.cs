using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFThickness_Test and is intended
    ///to contain all PDFThickness_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFThickness_Test
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
        ///A test for PDFThickness Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFThicknessConstructor_Test()
        {
            PDFUnit all = new PDFUnit(10);
            PDFThickness target = new PDFThickness(all);

            Assert.AreEqual(all, target.Left);
            Assert.AreEqual(all, target.Top);
            Assert.AreEqual(all, target.Bottom);
            Assert.AreEqual(all, target.Right);
        }

        /// <summary>
        ///A test for PDFThickness Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFThicknessConstructor_Test1()
        {
            PDFUnit top = new PDFUnit(10); 
            PDFUnit left = new PDFUnit(20); 
            PDFUnit bottom = new PDFUnit(30); 
            PDFUnit right = new PDFUnit(40);
            PDFThickness target = new PDFThickness(top, right, bottom, left);

            Assert.AreEqual(left, target.Left);
            Assert.AreEqual(top, target.Top);
            Assert.AreEqual(bottom, target.Bottom);
            Assert.AreEqual(right, target.Right);
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Add_Test()
        {
            PDFThickness one = new PDFThickness(10, 20, 30, 40);
            PDFThickness two = new PDFThickness(20, 30, 40, 50);
            PDFThickness expected = new PDFThickness(30, 50, 70, 90);
            PDFThickness actual;
            actual = PDFThickness.Add(one, two);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Clone
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Clone_Test()
        {
            PDFThickness target = new PDFThickness(10, 20, 30, 40);
            PDFThickness expected = new PDFThickness(10, 20, 30, 40);
            PDFThickness actual;
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
            PDFThickness target = new PDFThickness(10, 20, 30, 40);
            PDFThickness other = new PDFThickness(10, 20, 30, 40);
            int expected = 0; 
            int actual;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

            other = new PDFThickness(20, 30, 40, 50);
            expected = -1;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

            other = new PDFThickness(0, 0, 10, 50);
            expected = 1;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for Empty
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Empty_Test()
        {
            PDFThickness expected = new PDFThickness(0, 0, 0, 0);
            PDFThickness actual;
            actual = PDFThickness.Empty();
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Equals_Test()
        {
            PDFThickness target = new PDFThickness(10, 20, 30, 40);
            PDFThickness other = new PDFThickness(10, 20, 30, 40);
            bool expected = true; 
            bool actual;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new PDFThickness(20, 30, 40, 50);
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new PDFThickness(0, 20, 30, 40);
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new PDFThickness(10, 0, 30, 40);
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new PDFThickness(10, 20, 0, 40);
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new PDFThickness(10, 20, 30, 0);
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
            PDFThickness target = new PDFThickness(10, 20, 30, 40);
            object obj = null;
            bool expected = false;
            bool actual;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);

            obj = new PDFThickness(10, 20, 30, 40);
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
            PDFThickness target = new PDFThickness(10, 20, 30, 40);
            PDFThickness other = new PDFThickness(10, 20, 30, 40);
            int expected = other.GetHashCode();
            int actual;
            actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);

            other = new PDFThickness(10, 20, 30, 50);
            expected = other.GetHashCode();
            actual = target.GetHashCode();
            Assert.AreNotEqual(expected, actual);

            other = new PDFThickness(0, 20, 30, 40);
            expected = other.GetHashCode();
            actual = target.GetHashCode();
            Assert.AreNotEqual(expected, actual);

            other = new PDFThickness(10, 0, 30, 40);
            expected = other.GetHashCode();
            actual = target.GetHashCode();
            Assert.AreNotEqual(expected, actual);

            other = new PDFThickness(10, 20, 0, 40);
            expected = other.GetHashCode();
            actual = target.GetHashCode();
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for Inflate
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Inflate_Test()
        {
            PDFUnit t = 10;
            PDFUnit r = 20;
            PDFUnit b = 40;
            PDFUnit l = 30;
            PDFThickness target = new PDFThickness(t,r,b,l);
            PDFUnit w = new PDFUnit(5);
            PDFUnit h = new PDFUnit(10);
            target.Inflate(w, h);

            Assert.AreEqual(l + w, target.Left);
            Assert.AreEqual(r + w, target.Right);
            Assert.AreEqual(t + h, target.Top);
            Assert.AreEqual(b + h, target.Bottom);

        }

        /// <summary>
        ///A test for Inflate
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Inflate_Test1()
        {
            PDFUnit t = 10;
            PDFUnit r = 20;
            PDFUnit b = 40;
            PDFUnit l = 30;
            PDFThickness target = new PDFThickness(t, r, b, l);
            PDFUnit all = new PDFUnit(10);
            target.Inflate(all);

            Assert.AreEqual(l + all, target.Left);
            Assert.AreEqual(r + all, target.Right);
            Assert.AreEqual(t + all, target.Top);
            Assert.AreEqual(b + all, target.Bottom);
        }

        /// <summary>
        ///A test for Inflate
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Inflate_Test2()
        {
            PDFUnit t = 10;
            PDFUnit r = 20;
            PDFUnit b = 40;
            PDFUnit l = 30;
            PDFThickness target = new PDFThickness(t, r, b, l);

            
            target.Inflate(t, r, b, l);

            Assert.AreEqual(l + l, target.Left);
            Assert.AreEqual(r + r, target.Right);
            Assert.AreEqual(t + t, target.Top);
            Assert.AreEqual(b + b, target.Bottom);
        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Parse_Test()
        {
            string value = "[10mm 20mm 30 40in]";
            PDFThickness expected = new PDFThickness(new PDFUnit(10, PageUnits.Millimeters), new PDFUnit(20, PageUnits.Millimeters), new PDFUnit(30), new PDFUnit(40, PageUnits.Inches));
            PDFThickness actual;
            actual = PDFThickness.Parse(value);
            Assert.AreEqual(expected, actual);

            value = "[20mm]";
            PDFUnit all = new PDFUnit(20, PageUnits.Millimeters);
            expected = new PDFThickness(all);
            actual = PDFThickness.Parse(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SetAll
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void SetAll_Test()
        {
            PDFUnit t = 10;
            PDFUnit r = 20;
            PDFUnit b = 40;
            PDFUnit l = 30;
            PDFThickness target = new PDFThickness(t, r, b, l);

            PDFUnit all = new PDFUnit(10);
            target.SetAll(all);

            Assert.AreEqual(all, target.Left);
            Assert.AreEqual(all, target.Right);
            Assert.AreEqual(all, target.Top);
            Assert.AreEqual(all, target.Bottom);
        }

        /// <summary>
        ///A test for Subtract
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Subtract_Test()
        {
            PDFUnit t = 10;
            PDFUnit r = 20;
            PDFUnit b = 40;
            PDFUnit l = 30;
            PDFThickness one = new PDFThickness(t, r, b, l);

            PDFThickness two = new PDFThickness(1, 2, 3, 4);
            PDFThickness expected = new PDFThickness(t - 1, r - 2, b - 3, l - 4);
            PDFThickness actual;
            actual = PDFThickness.Subtract(one, two);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for System.ICloneable.Clone
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Clone_Test1()
        {
            PDFUnit t = 10;
            PDFUnit r = 20;
            PDFUnit b = 40;
            PDFUnit l = 30;
            PDFThickness one = new PDFThickness(t, r, b, l);
            ICloneable target = one;
            object expected = new PDFThickness(t, r, b, l);
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
            PDFUnit t = 10;
            PDFUnit r = 20;
            PDFUnit b = 40;
            PDFUnit l = 30;
            PDFThickness target = new PDFThickness(t, r, b, l);
            string expected = "[10pt 20pt 40pt 30pt]";
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for op_Addition
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void op_Addition_Test()
        {
            PDFUnit t = 10;
            PDFUnit l = 20;
            PDFUnit b = 40;
            PDFUnit r = 30;
            PDFThickness one = new PDFThickness(t, r, b, l);

            PDFThickness two = new PDFThickness(1, 2, 3, 4);
            PDFThickness expected = new PDFThickness(t + 1, r + 2, b + 3, l + 4);
            PDFThickness actual;
            actual = (one + two);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for op_Subtraction
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void op_Subtraction_Test()
        {
            PDFUnit t = 10;
            PDFUnit l = 20;
            PDFUnit b = 40;
            PDFUnit r = 30;
            PDFThickness one = new PDFThickness(t, r, b, l);

            PDFThickness two = new PDFThickness(1, 2, 3, 4);
            PDFThickness expected = new PDFThickness(t - 1, r - 2, b - 3, l - 4);

            PDFThickness actual;
            actual = (one - two);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Bottom
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Bottom_Test()
        {
            PDFUnit t = 10;
            PDFUnit l = 20;
            PDFUnit b = 40;
            PDFUnit r = 30;
            PDFThickness one = new PDFThickness(t, r, b, l);

            PDFUnit expected = b;
            PDFUnit actual = one.Bottom;
            Assert.AreEqual(expected, actual);

            b = 50;
            one.Bottom = b;
            actual = one.Bottom;
            Assert.AreEqual(b, actual);
            
        }

        /// <summary>
        ///A test for IsEmpty
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void IsEmpty_Test()
        {
            PDFThickness target = new PDFThickness(0, 0, 0, 0); 
            bool actual = target.IsEmpty;
            Assert.IsTrue(actual);

            target = new PDFThickness(1, 0, 0, 0);
            actual = target.IsEmpty;
            Assert.IsFalse(actual);

            target = new PDFThickness(0, 1, 0, 0);
            actual = target.IsEmpty;
            Assert.IsFalse(actual);

            target = new PDFThickness(0, 0, 1, 0);
            actual = target.IsEmpty;
            Assert.IsFalse(actual);

            target = new PDFThickness(0, 0, 0, 1);
            actual = target.IsEmpty;
            Assert.IsFalse(actual);

            target = new PDFThickness(1, 0, 0, -1);
            actual = target.IsEmpty;
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for Left
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Left_Test()
        {
            PDFUnit t = 10;
            PDFUnit l = 20;
            PDFUnit b = 40;
            PDFUnit r = 30;
            PDFThickness one = new PDFThickness(t, r, b, l);

            PDFUnit expected = l;
            PDFUnit actual = one.Left;
            Assert.AreEqual(expected, actual);

            l = 50;
            one.Left = l;
            actual = one.Left;
            Assert.AreEqual(l, actual);
        }

        /// <summary>
        ///A test for Right
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Right_Test()
        {
            PDFUnit t = 10;
            PDFUnit l = 20;
            PDFUnit b = 40;
            PDFUnit r = 30;
            PDFThickness one = new PDFThickness(t, r, b, l);

            PDFUnit expected = r;
            PDFUnit actual = one.Right;
            Assert.AreEqual(expected, actual);

            r = 50;
            one.Right = r;
            actual = one.Right;
            Assert.AreEqual(r, actual);
        }

        /// <summary>
        ///A test for Top
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Top_Test()
        {
            PDFUnit t = 10;
            PDFUnit l = 20;
            PDFUnit b = 40;
            PDFUnit r = 30;
            PDFThickness one = new PDFThickness(t, r, b, l) ;

            PDFUnit expected = t;
            PDFUnit actual = one.Top;
            Assert.AreEqual(expected, actual);

            t = 50;
            one.Top = t;
            actual = one.Top;
            Assert.AreEqual(t, actual);
        }

        
    }
}
