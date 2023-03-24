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

       


        /// <summary>
        ///A test for PDFThickness Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFThicknessConstructor_Test()
        {
            Unit all = new Unit(10);
            Thickness target = new Thickness(all);

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
            Unit top = new Unit(10); 
            Unit left = new Unit(20); 
            Unit bottom = new Unit(30); 
            Unit right = new Unit(40);
            Thickness target = new Thickness(top, right, bottom, left);

            Assert.AreEqual(left, target.Left);
            Assert.AreEqual(top, target.Top);
            Assert.AreEqual(bottom, target.Bottom);
            Assert.AreEqual(right, target.Right);
        }

        /// <summary>
        ///A test for IsRelative
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void IsRelative_Test()
        {
            Thickness target = new Thickness(10, 20, 30, 40);
            bool expected = false;
            bool actual;

            actual = target.IsRelative;
            Assert.AreEqual(expected, actual);


            target = new Thickness(new Unit(20, PageUnits.ViewPortWidth), 30, 40, 50);
            expected = true;
            actual = target.IsRelative;
            Assert.AreEqual(expected, actual);

            target = new Thickness(20, 30, 40, new Unit(50, PageUnits.ViewPortWidth));
            expected = true;
            actual = target.IsRelative;
            Assert.AreEqual(expected, actual);

            target = new Thickness(20, 30, new Unit(40, PageUnits.ViewPortWidth), 50);
            expected = true;
            actual = target.IsRelative;
            Assert.AreEqual(expected, actual);

            target = new Thickness(20, new Unit(30, PageUnits.ViewPortWidth), 40, 50);
            expected = true;
            actual = target.IsRelative;
            Assert.AreEqual(expected, actual);


            target = new Thickness(new Unit(20, PageUnits.ViewPortWidth), new Unit(30, PageUnits.ViewPortHeight), new Unit(40, PageUnits.ViewPortWidth), new Unit(50, PageUnits.ViewPortHeight));
            expected = true;
            actual = target.IsRelative;
            Assert.AreEqual(expected, actual);


        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Add_Test()
        {
            Thickness one = new Thickness(10, 20, 30, 40);
            Thickness two = new Thickness(20, 30, 40, 50);
            Thickness expected = new Thickness(30, 50, 70, 90);
            Thickness actual;
            actual = Thickness.Add(one, two);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Clone
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Clone_Test()
        {
            Thickness target = new Thickness(10, 20, 30, 40);
            Thickness expected = new Thickness(10, 20, 30, 40);
            Thickness actual;
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
            Thickness target = new Thickness(10, 20, 30, 40);
            Thickness other = new Thickness(10, 20, 30, 40);
            int expected = 0; 
            int actual;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

            other = new Thickness(20, 30, 40, 50);
            expected = -1;
            actual = target.CompareTo(other);
            Assert.AreEqual(expected, actual);

            other = new Thickness(0, 0, 10, 50);
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
            Thickness expected = new Thickness(0, 0, 0, 0);
            Thickness actual;
            actual = Thickness.Empty();
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Equals_Test()
        {
            Thickness target = new Thickness(10, 20, 30, 40);
            Thickness other = new Thickness(10, 20, 30, 40);
            bool expected = true; 
            bool actual;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new Thickness(20, 30, 40, 50);
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new Thickness(0, 20, 30, 40);
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new Thickness(10, 0, 30, 40);
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new Thickness(10, 20, 0, 40);
            expected = false;
            actual = target.Equals(other);
            Assert.AreEqual(expected, actual);

            other = new Thickness(10, 20, 30, 0);
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
            Thickness target = new Thickness(10, 20, 30, 40);
            object obj = null;
            bool expected = false;
            bool actual;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual);

            obj = new Thickness(10, 20, 30, 40);
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
            Thickness target = new Thickness(10, 20, 30, 40);
            Thickness other = new Thickness(10, 20, 30, 40);
            int expected = other.GetHashCode();
            int actual;
            actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);

            other = new Thickness(10, 20, 30, 50);
            expected = other.GetHashCode();
            actual = target.GetHashCode();
            Assert.AreNotEqual(expected, actual);

            other = new Thickness(0, 20, 30, 40);
            expected = other.GetHashCode();
            actual = target.GetHashCode();
            Assert.AreNotEqual(expected, actual);

            other = new Thickness(10, 0, 30, 40);
            expected = other.GetHashCode();
            actual = target.GetHashCode();
            Assert.AreNotEqual(expected, actual);

            other = new Thickness(10, 20, 0, 40);
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
            Unit t = 10;
            Unit r = 20;
            Unit b = 40;
            Unit l = 30;
            Thickness target = new Thickness(t,r,b,l);
            Unit w = new Unit(5);
            Unit h = new Unit(10);
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
            Unit t = 10;
            Unit r = 20;
            Unit b = 40;
            Unit l = 30;
            Thickness target = new Thickness(t, r, b, l);
            Unit all = new Unit(10);
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
            Unit t = 10;
            Unit r = 20;
            Unit b = 40;
            Unit l = 30;
            Thickness target = new Thickness(t, r, b, l);

            
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
            Thickness expected = new Thickness(new Unit(10, PageUnits.Millimeters), new Unit(20, PageUnits.Millimeters), new Unit(30), new Unit(40, PageUnits.Inches));
            Thickness actual;
            actual = Thickness.Parse(value);
            Assert.AreEqual(expected, actual);

            value = "[20mm]";
            Unit all = new Unit(20, PageUnits.Millimeters);
            expected = new Thickness(all);
            actual = Thickness.Parse(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SetAll
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void SetAll_Test()
        {
            Unit t = 10;
            Unit r = 20;
            Unit b = 40;
            Unit l = 30;
            Thickness target = new Thickness(t, r, b, l);

            Unit all = new Unit(10);
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
            Unit t = 10;
            Unit r = 20;
            Unit b = 40;
            Unit l = 30;
            Thickness one = new Thickness(t, r, b, l);

            Thickness two = new Thickness(1, 2, 3, 4);
            Thickness expected = new Thickness(t - 1, r - 2, b - 3, l - 4);
            Thickness actual;
            actual = Thickness.Subtract(one, two);
            Assert.AreEqual(expected, actual);
            
        }

        /// <summary>
        ///A test for System.ICloneable.Clone
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Clone_Test1()
        {
            Unit t = 10;
            Unit r = 20;
            Unit b = 40;
            Unit l = 30;
            Thickness one = new Thickness(t, r, b, l);
            ICloneable target = one;
            object expected = new Thickness(t, r, b, l);
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
            Unit t = 10;
            Unit r = 20;
            Unit b = 40;
            Unit l = 30;
            Thickness target = new Thickness(t, r, b, l);
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
            Unit t = 10;
            Unit l = 20;
            Unit b = 40;
            Unit r = 30;
            Thickness one = new Thickness(t, r, b, l);

            Thickness two = new Thickness(1, 2, 3, 4);
            Thickness expected = new Thickness(t + 1, r + 2, b + 3, l + 4);
            Thickness actual;
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
            Unit t = 10;
            Unit l = 20;
            Unit b = 40;
            Unit r = 30;
            Thickness one = new Thickness(t, r, b, l);

            Thickness two = new Thickness(1, 2, 3, 4);
            Thickness expected = new Thickness(t - 1, r - 2, b - 3, l - 4);

            Thickness actual;
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
            Unit t = 10;
            Unit l = 20;
            Unit b = 40;
            Unit r = 30;
            Thickness one = new Thickness(t, r, b, l);

            Unit expected = b;
            Unit actual = one.Bottom;
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
            Thickness target = new Thickness(0, 0, 0, 0); 
            bool actual = target.IsEmpty;
            Assert.IsTrue(actual);

            target = new Thickness(1, 0, 0, 0);
            actual = target.IsEmpty;
            Assert.IsFalse(actual);

            target = new Thickness(0, 1, 0, 0);
            actual = target.IsEmpty;
            Assert.IsFalse(actual);

            target = new Thickness(0, 0, 1, 0);
            actual = target.IsEmpty;
            Assert.IsFalse(actual);

            target = new Thickness(0, 0, 0, 1);
            actual = target.IsEmpty;
            Assert.IsFalse(actual);

            target = new Thickness(1, 0, 0, -1);
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
            Unit t = 10;
            Unit l = 20;
            Unit b = 40;
            Unit r = 30;
            Thickness one = new Thickness(t, r, b, l);

            Unit expected = l;
            Unit actual = one.Left;
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
            Unit t = 10;
            Unit l = 20;
            Unit b = 40;
            Unit r = 30;
            Thickness one = new Thickness(t, r, b, l);

            Unit expected = r;
            Unit actual = one.Right;
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
            Unit t = 10;
            Unit l = 20;
            Unit b = 40;
            Unit r = 30;
            Thickness one = new Thickness(t, r, b, l) ;

            Unit expected = t;
            Unit actual = one.Top;
            Assert.AreEqual(expected, actual);

            t = 50;
            one.Top = t;
            actual = one.Top;
            Assert.AreEqual(t, actual);
        }

        
    }
}
