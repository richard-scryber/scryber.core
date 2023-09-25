using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.CodeDom;
using Scryber.PDF.Native;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFUnit_Test and is intended
    ///to contain all PDFUnit_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFUnit_RelativeTest
    {

        private const string TestCategory = "Drawing Structures";

        #region public TestContext TestContext {get;set;}
        
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

        #endregion


        //
        // constructors
        //

        #region .ctor tests


        /// <summary>
        ///A test for PDFUnit Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitConstructor_Test1()
        {
            double value = 0.0; 
            PageUnits units = PageUnits.EMHeight;
            Unit target = new Unit(value, units);

            Assert.AreEqual(value, target.Value);
            Assert.AreEqual(units, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.IsTrue(target.IsZero);

            TestContext.WriteLine("Created new PDFUnit with double value and em units {0}", target);
        }

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitConstructor_Test2()
        {
            double value = 25;
            PageUnits units = PageUnits.Percent;
            Unit target = new Unit(value, units);
            Assert.AreEqual(25, target.Value);
            Assert.AreEqual(PageUnits.Percent, target.Units);
            Assert.IsTrue(target.IsRelative);

            TestContext.WriteLine("Created new PDFUnit with double value and % units {0}", target);
        }

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitConstructor_Test3()
        {
            double value = 0.25;
            PageUnits units = PageUnits.ViewPortWidth;
            Unit target = new Unit(value, units);
            Assert.AreEqual(0.25, target.Value);
            Assert.AreEqual(PageUnits.ViewPortWidth, target.Units);
            Assert.IsTrue(target.IsRelative);

            TestContext.WriteLine("Created new PDFUnit with double value and viewport width units {0}", target);
        }

        #endregion

        //
        // static factory methods
        //

        #region Percent factory

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_PercentTest()
        {
            double value = 25;

            Unit target = Unit.Percent(value);

            Assert.AreEqual(25.0, target.Value);
            Assert.AreEqual(PageUnits.Percent, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("25%", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with double value and % units {0}", target);
        }

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_PercentTest2()
        {
            int value = 26;

            Unit target = Unit.Percent(value);

            Assert.AreEqual(26.0, target.Value);
            Assert.AreEqual(PageUnits.Percent, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("26%", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with int value and % units {0}", target);
        }

        #endregion

        #region EXHeight factory

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_EXHeightTest()
        {
            double value = 27;

            Unit target = Unit.Ex(value);

            Assert.AreEqual(27.0, target.Value);
            Assert.AreEqual(PageUnits.EXHeight, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("27ex", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with double value and ex units {0}", target);
        }

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_EXHightTest2()
        {
            int value = 28;

            Unit target = Unit.Ex(value);

            Assert.AreEqual(28.0, target.Value);
            Assert.AreEqual(PageUnits.EXHeight, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("28ex", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with int value and ex units {0}", target);
        }

        #endregion

        #region EMHeight factory

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_EMHeightTest()
        {
            double value = 29;

            Unit target = Unit.Em(value);

            Assert.AreEqual(29.0, target.Value);
            Assert.AreEqual(PageUnits.EMHeight, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("29em", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with double value and em units {0}", target);
        }

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_EMHightTest2()
        {
            int value = 30;

            Unit target = Unit.Em(value);

            Assert.AreEqual(30.0, target.Value);
            Assert.AreEqual(PageUnits.EMHeight, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("30em", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with int value and em units {0}", target);
        }

        #endregion

        #region RootEMHeight factory

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_RootEMHeightTest()
        {
            double value = 31;

            Unit target = Unit.RootEm(value);

            Assert.AreEqual(31.0, target.Value);
            Assert.AreEqual(PageUnits.RootEMHeight, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("31rem", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with double value and root em units {0}", target);
        }

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_RootEMHightTest2()
        {
            int value = 32;

            Unit target = Unit.RootEm(value);

            Assert.AreEqual(32.0, target.Value);
            Assert.AreEqual(PageUnits.RootEMHeight, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("32rem", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with int value and root em units {0}", target);
        }

        #endregion

        #region ZeroCharWidth factory

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_ZeroCharWidthTest()
        {
            double value = 33;

            Unit target = Unit.Ch(value);

            Assert.AreEqual(33.0, target.Value);
            Assert.AreEqual(PageUnits.ZeroWidth, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("33ch", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with double value and 0 char units {0}", target);
        }

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_ZeroCharWidthTest2()
        {
            int value = 34;

            Unit target = Unit.Ch(value);

            Assert.AreEqual(34.0, target.Value);
            Assert.AreEqual(PageUnits.ZeroWidth, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("34ch", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with int value and 0 char units {0}", target);
        }

        #endregion

        #region Viewport Width factory

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_ViewPortWidthTest()
        {
            double value = 35;

            Unit target = Unit.Vw(value);

            Assert.AreEqual(35.0, target.Value);
            Assert.AreEqual(PageUnits.ViewPortWidth, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("35vw", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with double value and viewport width units {0}", target);
        }

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_ViewPortWidthTest2()
        {
            int value = 36;

            Unit target = Unit.Vw(value);

            Assert.AreEqual(36.0, target.Value);
            Assert.AreEqual(PageUnits.ViewPortWidth, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("36vw", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with int value and viewport width units {0}", target);
        }

        #endregion

        #region Viewport Height factory

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_ViewPortHeightTest()
        {
            double value = 37;

            Unit target = Unit.Vh(value);

            Assert.AreEqual(37.0, target.Value);
            Assert.AreEqual(PageUnits.ViewPortHeight, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("37vh", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with double value and viewport height units {0}", target);
        }

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_ViewPortHeightTest2()
        {
            int value = 38;

            Unit target = Unit.Vh(value);

            Assert.AreEqual(38.0, target.Value);
            Assert.AreEqual(PageUnits.ViewPortHeight, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("38vh", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with int value and viewport height units {0}", target);
        }

        #endregion

        #region Viewport Min factory

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_ViewPortMinTest()
        {
            double value = 39;

            Unit target = Unit.Vmin(value);

            Assert.AreEqual(39.0, target.Value);
            Assert.AreEqual(PageUnits.ViewPortMin, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("39vmin", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with double value and viewport min units {0}", target);
        }

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_ViewPortMinTest2()
        {
            int value = 40;

            Unit target = Unit.Vmin(value);

            Assert.AreEqual(40.0, target.Value);
            Assert.AreEqual(PageUnits.ViewPortMin, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("40vmin", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with int value and viewport min units {0}", target);
        }

        #endregion

        #region Viewport Min factory

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_ViewPortMaxTest()
        {
            double value = 41;

            Unit target = Unit.Vmax(value);

            Assert.AreEqual(41.0, target.Value);
            Assert.AreEqual(PageUnits.ViewPortMax, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("41vmax", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with double value and viewport max units {0}", target);
        }

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PDFUnitFactory_ViewPortMaxTest2()
        {
            int value = 42;

            Unit target = Unit.Vmax(value);

            Assert.AreEqual(42.0, target.Value);
            Assert.AreEqual(PageUnits.ViewPortMax, target.Units);
            Assert.IsTrue(target.IsRelative);
            Assert.AreEqual("42vmax", target.ToString());
            TestContext.WriteLine("Created new PDFUnit with int value and viewport max units {0}", target);
        }

        #endregion

        //
        // instance properties
        //

        #region property .Units and .Value tests

        /// <summary>
        ///A test for Units
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void Units_Test()
        {
            Unit target = new Unit(72);
            PageUnits expected = PageUnits.Points;
            PageUnits actual;
            actual = target.Units;
            Assert.AreEqual(expected, actual, "Default units were not Points");

            target = new Unit(1, PageUnits.EXHeight);
            actual = target.Units;
            expected = PageUnits.EXHeight;
            Assert.AreEqual(expected, actual, "Units were not EXHeight");
            Assert.IsTrue(target.IsRelative);

            target = new Unit(5.0, PageUnits.Percent);
            actual = target.Units;
            expected = PageUnits.Percent;
            Assert.AreEqual(expected, actual, "Units were not Percents");
            Assert.IsTrue(target.IsRelative);

            target = new Unit(1, PageUnits.ViewPortWidth);
            actual = target.Units;
            expected = PageUnits.ViewPortWidth;
            Assert.AreEqual(expected, actual, "Units were not Inches");
            Assert.IsTrue(target.IsRelative);

            TestContext.WriteLine("Relative Units were returned correctly from the constructors");
        }

        /// <summary>
        ///A test for Value
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void Value_Test()
        {
            Unit target;
            double expected = 10;
            double actual;
            

            target = new Unit(10, PageUnits.Percent);
            actual = target.Value;
            Assert.AreEqual(expected, actual, "1. Value was not in Percent");
            
            target = new Unit(15, PageUnits.ViewPortWidth);
            actual = target.Value;
            expected = 15;
            Assert.AreEqual(expected, actual, "2. Values was not in ViewPortWidth");

            target = new Unit(1, PageUnits.Inches);
            actual = target.Value;
            expected = 1;
            Assert.AreEqual(expected, actual, "3. Value was not in inches");



            TestContext.WriteLine("Values were returned correctly from the constructors");
        }

        #endregion

        #region IsEmpty

        /// <summary>
        ///A test for IsEmpty
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void IsEmpty_Test()
        {
            Unit target = new Unit();
            bool actual;
            bool expected = true;
            actual = target.IsZero;
            Assert.AreEqual(expected, actual, "parameterless constructor did not return an Empty Unit value");

            target = new Unit(0.0, PageUnits.ViewPortWidth);
            expected = true;
            actual = target.IsZero;
            Assert.AreEqual(expected, actual, "A zero unit returned false for being empty with relative units");

            target = new Unit(1.0, PageUnits.Inches);
            expected = false;
            actual = target.IsZero;
            Assert.AreEqual(expected, actual, "A valid unit returned true for being empty");

        }

        #endregion


        #region public void ToAbsolute_Test()

        [TestMethod()]
        [TestCategory(TestCategory)]
        public void ToAbsolute_Test()
        {
            //10vw of 400pt = 40pt

            Unit target = new Unit(10, PageUnits.ViewPortWidth);
            Unit reference = new Unit(400.0, PageUnits.Points);

            Unit absolute = target.ToAbsolute(reference);
            Unit expected = new Unit(40.0, PageUnits.Points); //40pt

            Assert.AreEqual(expected, absolute);
            Assert.AreEqual(expected.Value, absolute.Value);
            Assert.AreEqual(PageUnits.Points, absolute.Units);
            Assert.IsFalse(absolute.IsRelative);
            Assert.IsFalse(absolute.IsZero);

            //2em of 12pt = 24pt

            target = new Unit(2, PageUnits.EMHeight);
            reference = new Unit(12, PageUnits.Points);

            absolute = target.ToAbsolute(reference);
            expected = new Unit(24, PageUnits.Points);

            Assert.AreEqual(expected, absolute);
            Assert.AreEqual(expected.Value, absolute.Value);
            Assert.AreEqual(PageUnits.Points, absolute.Units);
            Assert.IsFalse(absolute.IsRelative);
            Assert.IsFalse(absolute.IsZero);

            //50% of 20mm = 10mm (maintain reference units)

            target = new Unit(50, PageUnits.Percent);
            reference = new Unit(20, PageUnits.Millimeters);

            absolute = target.ToAbsolute(reference);
            expected = new Unit(10, PageUnits.Millimeters);

            Assert.AreEqual(expected, absolute);
            Assert.AreEqual(expected.Value, absolute.Value);
            Assert.AreEqual(PageUnits.Millimeters, absolute.Units);
            Assert.IsFalse(absolute.IsRelative);
            Assert.IsFalse(absolute.IsZero);

            //50mm of 20mm = 50mm (reference units ignored)

            target = new Unit(50, PageUnits.Millimeters);
            reference = new Unit(20, PageUnits.Millimeters);

            absolute = target.ToAbsolute(reference);
            expected = new Unit(50, PageUnits.Millimeters);

            Assert.AreEqual(expected, absolute);
            Assert.AreEqual(expected.Value, absolute.Value);
            Assert.AreEqual(PageUnits.Millimeters, absolute.Units);
            Assert.IsFalse(absolute.IsRelative);
            Assert.IsFalse(absolute.IsZero);
        }

        #endregion

        //
        // parsing
        //

        #region Parse tests

        /// <summary>
        ///A test for Parse
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void Parse_Test()
        {
            Parse("10%", new Unit(10, PageUnits.Percent));
            Parse("20.543vw", new Unit(20.543, PageUnits.ViewPortWidth));
            Parse("000123.456em", new Unit(123.456, PageUnits.EMHeight));
            Parse("-1.5vmin", new Unit(-1.5, PageUnits.ViewPortMin));
            Parse("234567890123.12345vmax", new Unit(234567890123.12345, PageUnits.ViewPortMax));
            Parse("10vh", new Unit(10, PageUnits.ViewPortHeight));
            Parse("20.543ex", new Unit(20.543, PageUnits.EXHeight));
            Parse("000123.456rem", new Unit(123.456, PageUnits.RootEMHeight));
            Parse("-1.5ch", new Unit(-1.5, PageUnits.ZeroWidth));

        }

        private Unit Parse(string value, Unit expected)
        {
            Unit actual;
            actual = Unit.Parse(value);
            Assert.AreEqual(expected, actual, "Parse failed for '" + value + "'");
            this.TestContext.WriteLine("Parsed string '{0}' to PDFUnit {1}", value, expected);
            return expected;
        }

        #endregion

        #region TryParse tests

        /// <summary>
        ///A test for TryParse
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void TryParse_Test()
        {
            TryParse("10%", true, new Unit(10, PageUnits.Percent));
            TryParse("20.543vw", true, new Unit(20.543, PageUnits.ViewPortWidth));
            TryParse("000123.456em", true, new Unit(123.456, PageUnits.EMHeight));
            TryParse("-1.5vmin", true, new Unit(-1.5, PageUnits.ViewPortMin));
            TryParse("234567890123.12345vmax", true, new Unit(234567890123.12345, PageUnits.ViewPortMax));
            TryParse("10vh", true, new Unit(10, PageUnits.ViewPortHeight));
            TryParse("20.543ex", true, new Unit(20.543, PageUnits.EXHeight));
            TryParse("000123.456rem", true, new Unit(123.456, PageUnits.RootEMHeight));
            TryParse("-1.5ch", true, new Unit(-1.5, PageUnits.ZeroWidth));

        }

        private void TryParse(string value, bool expectedresult, Unit expectedvalue)
        {
            Unit parsed;
            bool actualresult = Unit.TryParse(value, out parsed);
            Assert.AreEqual(expectedresult, actualresult, "The expected TryParse returned value was not the same as the expected result");
            Assert.AreEqual(expectedvalue, parsed, "The parsed value for TryParse was not the same as the expected value");
            if (expectedresult)
                this.TestContext.WriteLine("TryParse string '{0}' succeeded parsing to PDFUnit {1}", value, parsed);
            else
                this.TestContext.WriteLine("TryParse string '{0}' was expected not to parse, and returned value {1}", value, parsed);
        }

        #endregion

        //
        // comparison
        //

        #region Compare and CompareTo tests

        /// <summary>
        ///A test for Compare
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void Compare_Test()
        {
            

            Unit one = new Unit(10, PageUnits.Percent);
            Unit two = new Unit(10, PageUnits.Percent);

            int expected = 0;//equal

            int actual = Unit.Compare(one, two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(2.5, PageUnits.EMHeight);
            two = new Unit(2, PageUnits.EMHeight);
            actual = Unit.Compare(one, two);
            expected = 1; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(1.5, PageUnits.EMHeight);
            two = new Unit(2, PageUnits.EMHeight);
            actual = Unit.Compare(one, two);
            expected = -1; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            two = new Unit(2, PageUnits.Points);
            bool caught = false;
            try
            {
                actual = Unit.Compare(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not throw an exception");

        }

        /// <summary>
        ///A test for CompareTo(object)
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void CompareTo_Test()
        {
            Unit one = new Unit(10, PageUnits.Percent);
            Unit two = new Unit(10, PageUnits.Percent);

            int expected = 0;//equal

            int actual = one.CompareTo(two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(2.5, PageUnits.EMHeight);
            two = new Unit(2, PageUnits.EMHeight);
            actual = one.CompareTo(two);
            expected = 1; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(1.5, PageUnits.EMHeight);
            two = new Unit(2, PageUnits.EMHeight);
            actual = one.CompareTo(two);
            expected = -1; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            two = new Unit(2, PageUnits.Points);
            bool caught = false;
            try
            {
                actual = one.CompareTo(two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not throw an exception");

        }

        /// <summary>
        ///A test for CompareTo Unit to Unit
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void CompareTo_Test1()
        {
            Unit one = new Unit(10, PageUnits.Percent);
            Unit two = new Unit(10, PageUnits.Percent);

            int expected = 0;//equal
            object obj = two;
            int actual = one.CompareTo(obj);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(2.5, PageUnits.EMHeight);
            two = new Unit(2, PageUnits.EMHeight);
            obj = two;
            actual = one.CompareTo(obj);
            expected = 1; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(1.5, PageUnits.EMHeight);
            two = new Unit(2, PageUnits.EMHeight);
            obj = two;
            actual = one.CompareTo(obj);
            expected = -1; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            two = new Unit(2, PageUnits.Points);
            bool caught = false;
            try
            {
                obj = two;
                actual = one.CompareTo(obj);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not throw an exception");


            caught = false;
            try
            {
                obj = null;
                actual = one.CompareTo(obj);
            }
            catch (NullReferenceException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing null values did not throw an exception");

        }

        #endregion

        #region Equals(unit1,unit2), ==, Equals(object), Equals(unit) tests

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void EqualStatic_Test()
        {
            Unit one = new Unit(100, PageUnits.Percent);
            Unit two = new Unit(100, PageUnits.Percent);

            bool expected = true;//equal

            bool actual = Unit.Equals(one, two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = Unit.Equals(one, two);
            expected = false; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(10.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = Unit.Equals(one, two);
            expected = false; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            two = new Unit(20, PageUnits.Points);
            actual = Unit.Equals(one, two);
            
            Assert.IsFalse(actual, "Comparing relative and absolute values are not equal");
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void EqualToUnit_Test()
        {
            Unit one = new Unit(10, PageUnits.Percent);
            Unit two = new Unit(10, PageUnits.Percent);

            bool expected = true;//equal

            bool actual = one.Equals(two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(2.5, PageUnits.EMHeight);
            two = new Unit(2, PageUnits.EMHeight);
            actual = one.Equals(two);
            expected = false; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(1.5, PageUnits.EMHeight);
            two = new Unit(2, PageUnits.EMHeight);
            actual = one.Equals(two);
            expected = false; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            two = new Unit(2, PageUnits.Points);
            actual = Unit.Equals(one, two);

            Assert.IsFalse(actual, "Comparing relative and absolute values are not equal");
        }


        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void EqualToObject_Test()
        {
            Unit one = new Unit(100, PageUnits.Percent);
            Unit two = new Unit(100, PageUnits.Percent);
            object obj = two;
            bool expected = true;//equal

            bool actual = one.Equals(obj);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            obj = two;
            actual = one.Equals(obj);
            expected = false; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(10.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            obj = two;
            actual = one.Equals(obj);
            expected = false; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            two = new Unit(20, PageUnits.Points);
            obj = two;
            expected = false;
            actual = one.Equals(obj);

            Assert.AreEqual(expected, actual, "Comparing relative and absolute values are not equal");
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void EqualOperator_Test()
        {
            Unit one = new Unit(100, PageUnits.Percent);
            Unit two = new Unit(100, PageUnits.Percent);

            bool expected = true;//equal

            bool actual = (one == two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = (one == two);
            expected = false; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(10.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = (one == two);
            expected = false; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            two = new Unit(20, PageUnits.Points);
            actual = (one == two);

            Assert.IsFalse(actual, "Comparing relative and absolute values are not equal");
        }

        #endregion

        #region NotEquals(left,right), !=   tests

        /// <summary>
        ///A test for NotEquals
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void NotEqualsStatic_Test()
        {
            Unit one = new Unit(100, PageUnits.Percent);
            Unit two = new Unit(100, PageUnits.Percent);

            bool expected = false;//equal

            bool actual = Unit.NotEquals(one, two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = Unit.NotEquals(one, two);
            expected = true; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(10.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = Unit.NotEquals(one, two);
            expected = true; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            two = new Unit(20, PageUnits.Points);
            actual = Unit.NotEquals(one, two);
            expected = true;
            Assert.AreEqual(expected, actual, "Comparing relative and absolute values are not equal");
        }

        /// <summary>
        ///A test for NotEquals
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void NotEqualsOperator_Test()
        {
            Unit one = new Unit(100, PageUnits.Percent);
            Unit two = new Unit(100, PageUnits.Percent);

            bool expected = false;//equal

            bool actual = (one != two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = (one != two);
            expected = true; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(10.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = (one != two);
            expected = true; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            two = new Unit(20, PageUnits.Points);
            actual =(one != two);
            expected = true;
            Assert.AreEqual(expected, actual, "Comparing relative and absolute values are not equal");
        }

        #endregion

        #region GreaterThan, >, GreaterThanEqual, >=  tests

        /// <summary>
        ///A test for GreaterThan
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void GreaterThan_Test()
        {
            Unit one = new Unit(100, PageUnits.Percent);
            Unit two = new Unit(100, PageUnits.Percent);

            bool expected = false;//equal

            bool actual = Unit.GreaterThan(one, two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = Unit.GreaterThan(one, two);
            expected = true; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(10.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = Unit.GreaterThan(one, two);
            expected = false; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            bool caught = false;
            two = new Unit(20, PageUnits.Points);
            try
            {
                actual = Unit.GreaterThan(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }
            
            Assert.IsTrue(caught, "Comparing relative and absolute values did not raise an exception");

            caught = false;
            two = new Unit(20, PageUnits.EXHeight);
            try
            {
                actual = Unit.GreaterThan(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative values with different units did not raise an exception");

        }

        /// <summary>
        ///A test for GreaterThan operator (>)
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void GreaterThanOperator_Test()
        {
            Unit one = new Unit(10, PageUnits.Percent);
            Unit two = new Unit(10, PageUnits.Percent);

            bool expected = false;//equal

            bool actual = (one > two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = (one > two);
            expected = true; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(10.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = (one > two);
            expected = false; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            bool caught = false;
            two = new Unit(20, PageUnits.Points);
            try
            {
                actual = (one > two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not raise an exception");

            caught = false;
            two = new Unit(20, PageUnits.EXHeight);
            try
            {
                actual = (one > two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative values with different units did not raise an exception");

        }

        /// <summary>
        ///A test for GreaterThanEqual
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void GreaterThanEqual_Test()
        {
            Unit one = new Unit(10, PageUnits.Percent);
            Unit two = new Unit(10, PageUnits.Percent);

            bool expected = true;//equal

            bool actual = Unit.GreaterThanEqual(one, two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = Unit.GreaterThanEqual(one, two);
            expected = true; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(10.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = Unit.GreaterThanEqual(one, two);
            expected = false; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            bool caught = false;
            two = new Unit(20, PageUnits.Points);
            try
            {
                actual = Unit.GreaterThanEqual(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not raise an exception");

            caught = false;
            two = new Unit(20, PageUnits.EXHeight);
            try
            {
                actual = Unit.GreaterThanEqual(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative values with different units did not raise an exception");
        }


        /// <summary>
        ///A test for GreaterThanEqual
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void GreaterThanEqualOperator_Test()
        {
            Unit one = new Unit(10, PageUnits.Percent);
            Unit two = new Unit(10, PageUnits.Percent);

            bool expected = true;//equal

            bool actual = (one >= two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = (one >= two);
            expected = true; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(10.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = (one >= two);
            expected = false; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            bool caught = false;
            two = new Unit(20, PageUnits.Points);
            try
            {
                actual = (one >= two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not raise an exception");

            caught = false;
            two = new Unit(20, PageUnits.EXHeight);
            try
            {
                actual = (one >= two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative values with different units did not raise an exception");
        }


        #endregion

        #region LessThan, <, LessThanEqual, <=  tests

        /// <summary>
        ///A test for GreaterThan
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void LessThan_Test()
        {
            Unit one = new Unit(100, PageUnits.Percent);
            Unit two = new Unit(100, PageUnits.Percent);

            bool expected = false;//equal

            bool actual = Unit.LessThan(one, two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = Unit.LessThan(one, two);
            expected = false; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(10.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = Unit.LessThan(one, two);
            expected = true; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            bool caught = false;
            two = new Unit(20, PageUnits.Points);
            try
            {
                actual = Unit.LessThan(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not raise an exception");

            caught = false;
            two = new Unit(20, PageUnits.EXHeight);
            try
            {
                actual = Unit.LessThan(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative values with different units did not raise an exception");

        }

        /// <summary>
        ///A test for GreaterThan operator (>)
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void LessThanOperator_Test()
        {
            Unit one = new Unit(10, PageUnits.Percent);
            Unit two = new Unit(10, PageUnits.Percent);

            bool expected = false;//equal

            bool actual = (one < two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = (one < two);
            expected = false; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(10.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = (one < two);
            expected = true; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            bool caught = false;
            two = new Unit(20, PageUnits.Points);
            try
            {
                actual = (one < two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not raise an exception");

            caught = false;
            two = new Unit(20, PageUnits.EXHeight);
            try
            {
                actual = (one < two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative values with different units did not raise an exception");

        }

        /// <summary>
        ///A test for GreaterThanEqual
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void LessThanEqual_Test()
        {
            Unit one = new Unit(10, PageUnits.Percent);
            Unit two = new Unit(10, PageUnits.Percent);

            bool expected = true;//equal

            bool actual = Unit.LessThanEqual(one, two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = Unit.LessThanEqual(one, two);
            expected = false; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(10.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = Unit.LessThanEqual(one, two);
            expected = true; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            bool caught = false;
            two = new Unit(20, PageUnits.Points);
            try
            {
                actual = Unit.LessThanEqual(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not raise an exception");

            caught = false;
            two = new Unit(20, PageUnits.EXHeight);
            try
            {
                actual = Unit.LessThanEqual(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative values with different units did not raise an exception");
        }


        /// <summary>
        ///A test for GreaterThanEqual
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void LessThanEqualOperator_Test()
        {
            Unit one = new Unit(10, PageUnits.Percent);
            Unit two = new Unit(10, PageUnits.Percent);

            bool expected = true;//equal

            bool actual = (one <= two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = (one <= two);
            expected = false; //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(10.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = (one <= two);
            expected = true; //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            bool caught = false;
            two = new Unit(20, PageUnits.Points);
            try
            {
                actual = (one <= two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not raise an exception");

            caught = false;
            two = new Unit(20, PageUnits.EXHeight);
            try
            {
                actual = (one <= two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative values with different units did not raise an exception");
        }


        #endregion

        #region Min, Max tests

        /// <summary>
        ///A test for Max
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void Max_Test()
        {

            Unit one = new Unit(10, PageUnits.Percent);
            Unit two = new Unit(10, PageUnits.Percent);

            Unit expected = new Unit(10, PageUnits.Percent);//equal

            Unit actual = Unit.Max(one, two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = Unit.Max(one, two);
            expected = new Unit(20.5, PageUnits.EMHeight); //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(10.5, PageUnits.EXHeight);
            two = new Unit(20, PageUnits.EXHeight);
            actual = Unit.Max(one, two);
            expected = new Unit(20, PageUnits.EXHeight); //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            bool caught = false;
            two = new Unit(20, PageUnits.Points);
            try
            {
                actual = Unit.Max(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not raise an exception");

            caught = false;
            two = new Unit(20, PageUnits.RootEMHeight);
            try
            {
                actual = Unit.Max(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative values with different units did not raise an exception");
        }

        /// <summary>
        ///A test for Min
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void Min_Test()
        {
            Unit one = new Unit(10, PageUnits.Percent);
            Unit two = new Unit(10, PageUnits.Percent);

            Unit expected = new Unit(10, PageUnits.Percent);//equal

            Unit actual = Unit.Min(one, two);
            Assert.AreEqual(expected, actual, "Compare of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} is equal to {1}", one, two);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = Unit.Min(one, two);
            expected = new Unit(20, PageUnits.EMHeight); //first is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            one = new Unit(10.5, PageUnits.EXHeight);
            two = new Unit(20, PageUnits.EXHeight);
            actual = Unit.Min(one, two);
            expected = new Unit(10.5, PageUnits.EXHeight); //second is greater

            Assert.AreEqual(expected, actual, "Compare of unequal EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} is not equal to EMHeight value {1}", one, two);

            bool caught = false;
            two = new Unit(20, PageUnits.Points);
            try
            {
                actual = Unit.Min(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not raise an exception");

            caught = false;
            two = new Unit(20, PageUnits.RootEMHeight);
            try
            {
                actual = Unit.Min(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative values with different units did not raise an exception");
        }

        #endregion

        //
        // calculation
        //


        #region Add, +, Subtract, -  tests

        /// <summary>
        ///A test for GreaterThan
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void Add_Test()
        {
            Unit one = new Unit(100, PageUnits.Percent);
            Unit two = new Unit(100, PageUnits.Percent);

            Unit expected = new Unit(200, PageUnits.Percent);

            Unit actual = Unit.Add(one, two);
            Assert.AreEqual(expected, actual, "Addition of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} plus {1} = {2}", one, two, actual);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = Unit.Add(one, two);
            expected = new Unit(40.5, PageUnits.EMHeight); 

            Assert.AreEqual(expected, actual, "Addition of EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} plus EMHeight value {1} = {2}", one, two, actual);

            one = new Unit(10.5, PageUnits.EXHeight);
            two = new Unit(20, PageUnits.EXHeight);
            actual = Unit.Add(one, two);
            expected = new Unit(30.5, PageUnits.EXHeight); 

            Assert.AreEqual(expected, actual, "Addition EXHeights to EXHeights failed");
            TestContext.WriteLine("EXHeight value {0} plus EXHeight value {1} equal {2}", one, two, actual);

            bool caught = false;
            two = new Unit(20, PageUnits.Points);
            try
            {
                actual = Unit.Add(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not raise an exception");

            caught = false;
            two = new Unit(20, PageUnits.ViewPortHeight);
            try
            {
                actual = Unit.Add(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative values with different units did not raise an exception");

        }

        /// <summary>
        ///A test for GreaterThan operator (>)
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void AddOperator_Test()
        {

            Unit one = new Unit(100, PageUnits.Percent);
            Unit two = new Unit(100, PageUnits.Percent);

            Unit expected = new Unit(200, PageUnits.Percent);

            Unit actual = (one + two);
            Assert.AreEqual(expected, actual, "Addition of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} plus {1} = {2}", one, two, actual);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = (one + two);
            expected = new Unit(40.5, PageUnits.EMHeight);

            Assert.AreEqual(expected, actual, "Addition of EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} plus EMHeight value {1} = {2}", one, two, actual);

            one = new Unit(10.5, PageUnits.EXHeight);
            two = new Unit(20, PageUnits.EXHeight);
            actual = (one + two);
            expected = new Unit(30.5, PageUnits.EXHeight);

            Assert.AreEqual(expected, actual, "Addition EXHeights to EXHeights failed");
            TestContext.WriteLine("EXHeight value {0} plus EXHeight value {1} equal {2}", one, two, actual);

            bool caught = false;
            two = new Unit(20, PageUnits.Points);
            try
            {
                actual = (one + two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not raise an exception");

            caught = false;
            two = new Unit(20, PageUnits.ViewPortHeight);
            try
            {
                actual = (one + two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative values with different units did not raise an exception");

        }

        /// <summary>
        ///A test for GreaterThanEqual
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void Subtract_Test()
        {
            Unit one = new Unit(100, PageUnits.Percent);
            Unit two = new Unit(100, PageUnits.Percent);

            Unit expected = new Unit(0, PageUnits.Percent);

            Unit actual = Unit.Subtract(one, two);
            Assert.AreEqual(expected, actual, "Addition of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} plus {1} = {2}", one, two, actual);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = Unit.Subtract(one, two);
            expected = new Unit(0.5, PageUnits.EMHeight);

            Assert.AreEqual(expected, actual, "Addition of EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} plus EMHeight value {1} = {2}", one, two, actual);

            one = new Unit(10.5, PageUnits.EXHeight);
            two = new Unit(20, PageUnits.EXHeight);
            actual = Unit.Subtract(one, two);
            expected = new Unit(-9.5, PageUnits.EXHeight);

            Assert.AreEqual(expected, actual, "Addition EXHeights to EXHeights failed");
            TestContext.WriteLine("EXHeight value {0} plus EXHeight value {1} equal {2}", one, two, actual);

            bool caught = false;
            two = new Unit(20, PageUnits.Points);
            try
            {
                actual = Unit.Subtract(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not raise an exception");

            caught = false;
            two = new Unit(20, PageUnits.ViewPortHeight);
            try
            {
                actual = Unit.Subtract(one, two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative values with different units did not raise an exception");
        }


        /// <summary>
        ///A test for GreaterThanEqual
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void SubtractOperator_Test()
        {
            Unit one = new Unit(100, PageUnits.Percent);
            Unit two = new Unit(100, PageUnits.Percent);

            Unit expected = new Unit(0, PageUnits.Percent);

            Unit actual = (one - two);
            Assert.AreEqual(expected, actual, "Addition of equal Percents to Percents failed");
            TestContext.WriteLine("Percent value {0} plus {1} = {2}", one, two, actual);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = new Unit(20, PageUnits.EMHeight);
            actual = (one - two);
            expected = new Unit(0.5, PageUnits.EMHeight);

            Assert.AreEqual(expected, actual, "Addition of EMHeights to EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} plus EMHeight value {1} = {2}", one, two, actual);

            one = new Unit(10.5, PageUnits.EXHeight);
            two = new Unit(20, PageUnits.EXHeight);
            actual = (one - two);
            expected = new Unit(-9.5, PageUnits.EXHeight);

            Assert.AreEqual(expected, actual, "Addition EXHeights to EXHeights failed");
            TestContext.WriteLine("EXHeight value {0} plus EXHeight value {1} equal {2}", one, two, actual);

            bool caught = false;
            two = new Unit(20, PageUnits.Points);
            try
            {
                actual = (one - two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not raise an exception");

            caught = false;
            two = new Unit(20, PageUnits.ViewPortHeight);
            try
            {
                actual = (one - two);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative values with different units did not raise an exception");
        }


        #endregion


        #region Multiply, *, Divide, /  tests

        /// <summary>
        ///A test for GreaterThan
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void Multiply_Test()
        {

            Unit one = new Unit(100, PageUnits.Percent);
            double two = 10.0;

            Unit expected = new Unit(1000, PageUnits.Percent);

            Unit actual = Unit.Multiply(one, two);
            Assert.AreEqual(expected, actual, "Multiply Percents failed");
            TestContext.WriteLine("Percent value {0} tines {1} = {2}", one, two, actual);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = 20;
            actual = Unit.Multiply(one, two);
            expected = new Unit(410, PageUnits.EMHeight);

            Assert.AreEqual(expected, actual, "Multiplication of EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} times EMHeight value {1} = {2}", one, two, actual);

            one = new Unit(10.5, PageUnits.EXHeight);
            two = 20;
            actual = Unit.Multiply(one, two);
            expected = new Unit(210, PageUnits.EXHeight);

            Assert.AreEqual(expected, actual, "Multiplication EXHeights failed");
            TestContext.WriteLine("EXHeight value {0} times EXHeight value {1} equal {2}", one, two, actual);

        }

        /// <summary>
        ///A test for GreaterThan operator (>)
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void MultiplyOperator_Test()
        {

            Unit one = new Unit(100, PageUnits.Percent);
            double two = 10.0;

            Unit expected = new Unit(1000, PageUnits.Percent);

            Unit actual = (one * two);
            Assert.AreEqual(expected, actual, "Multiply Percents failed");
            TestContext.WriteLine("Percent value {0} tines {1} = {2}", one, two, actual);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = 20;
            actual = (one * two);
            expected = new Unit(410, PageUnits.EMHeight);

            Assert.AreEqual(expected, actual, "Multiplication of EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} times EMHeight value {1} = {2}", one, two, actual);

            one = new Unit(10.5, PageUnits.EXHeight);
            two = 20;
            actual = (one * two);
            expected = new Unit(210, PageUnits.EXHeight);

            Assert.AreEqual(expected, actual, "Multiplication EXHeights failed");
            TestContext.WriteLine("EXHeight value {0} times EXHeight value {1} equal {2}", one, two, actual);

        }

        /// <summary>
        ///A test for GreaterThanEqual
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void Divide_Test()
        {
            Unit one = new Unit(100, PageUnits.Percent);
            double two = 10.0;

            Unit expected = new Unit(10, PageUnits.Percent);

            Unit actual = Unit.Divide(one, two);
            Assert.AreEqual(expected, actual, "Divide Percents failed");
            TestContext.WriteLine("Percent value {0} divided by {1} = {2}", one, two, actual);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = 20;
            actual = Unit.Divide(one, two);
            expected = new Unit(1.025, PageUnits.EMHeight);

            Assert.AreEqual(expected, actual, "Divide of EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} divided by EMHeight value {1} = {2}", one, two, actual);

            one = new Unit(10.5, PageUnits.EXHeight);
            two = 5;
            actual = Unit.Divide(one, two);
            expected = new Unit(2.1, PageUnits.EXHeight);

            Assert.AreEqual(expected, actual, "Divide EXHeights failed");
            TestContext.WriteLine("EXHeight value {0} divided by EXHeight value {1} equal {2}", one, two, actual);
        }


        /// <summary>
        ///A test for GreaterThanEqual
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void DivideOperator_Test()
        {
            Unit one = new Unit(100, PageUnits.Percent);
            double two = 10.0;

            Unit expected = new Unit(10, PageUnits.Percent);

            Unit actual = (one / two);
            Assert.AreEqual(expected, actual, "Divide Percents failed");
            TestContext.WriteLine("Percent value {0} divided by {1} = {2}", one, two, actual);


            one = new Unit(20.5, PageUnits.EMHeight);
            two = 20;
            actual = (one / two);
            expected = new Unit(1.025, PageUnits.EMHeight);

            Assert.AreEqual(expected, actual, "Divide of EMHeights failed");
            TestContext.WriteLine("EMHeight value {0} divided by EMHeight value {1} = {2}", one, two, actual);

            one = new Unit(10.5, PageUnits.EXHeight);
            two = 5;
            actual = (one / two);
            expected = new Unit(2.1, PageUnits.EXHeight);

            Assert.AreEqual(expected, actual, "Divide EXHeights failed");
            TestContext.WriteLine("EXHeight value {0} divided by EXHeight value {1} equal {2}", one, two, actual);
        }


        #endregion

        //
        // conversion
        //

        #region Convert test

        /// <summary>
        ///A test for Convert
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void Convert_Test()
        {
            
            //same units should be ok - no change

            Unit expected = new Unit(10, PageUnits.Percent);
            Unit actual;

            Unit unit = new Unit(10, PageUnits.Percent);
            actual = Unit.Convert(unit, PageUnits.Percent);

            Assert.AreEqual(expected.Value, actual.Value, "The converted percent to inches failed");
            TestContext.WriteLine("Value {0} converted to {1}", unit, actual);

            //relative units converted to other units should fail

            bool caught = false;
            unit = new Unit(20, PageUnits.RootEMHeight);
            try
            {
                actual = Unit.Convert(unit, PageUnits.EMHeight);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not raise an exception");

            caught = false;
            unit = new Unit(10, PageUnits.ViewPortHeight);
            try
            {
                actual = Unit.Convert(unit, PageUnits.ViewPortMin);
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "Comparing relative and absolute values did not raise an exception");



            
        }

        #endregion

        #region PointsValue

        /// <summary>
        ///A test for ToInches
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void PointsValue_Test()
        {

            Unit target = new Unit(20, PageUnits.ViewPortWidth);
            
            Unit actual;
            bool caught = false;
            try
            {
                actual = target.PointsValue;
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }
            Assert.IsTrue(caught, "ToInches() did not throw exception for relative unit");
        }

        #endregion

        #region ToInches(), ToMillimeters(), ToPoints()


        /// <summary>
        ///A test for ToInches
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void ToInches_Test()
        {
            
            Unit target = new Unit(2,PageUnits.EMHeight); 
            
            Unit actual;
            bool caught = false;
            try
            {
                actual = target.ToInches();
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }
            Assert.IsTrue(caught, "ToInches() did not throw exception for relative unit");
        }

        /// <summary>
        ///A test for ToMillimeters
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void ToMillimeters_Test()
        {
            Unit target = new Unit(2, PageUnits.EMHeight);
            
            Unit actual;
            bool caught = false;
            try
            {
                actual = target.ToMillimeters();
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }
            Assert.IsTrue(caught, "ToInches() did not throw exception for relative unit");
        }

        /// <summary>
        ///A test for ToPoints
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void ToPoints_Test()
        {
            Unit target = new Unit(2, PageUnits.Percent);
            
            Unit actual;
            bool caught = false;
            try
            {
                actual = target.ToPoints();
            }
            catch (InvalidOperationException)
            {
                caught = true;
            }
            Assert.IsTrue(caught, "ToInches() did not throw exception for relative unit");
        }

        #endregion

        #region Equals() test

        /// <summary>
        ///A test for ToPoints
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void Equals_Test()
        {
            //Assert.Inconclusive("Relative units");

            Unit target = new Unit(2, PageUnits.Percent);
            Unit equal = new Unit(2, PageUnits.Percent);
            Unit unequal = new Unit(3, PageUnits.Percent);

            Assert.IsTrue(target.Equals(equal));
            Assert.IsFalse(target.Equals(unequal));

            unequal = new Unit(2, PageUnits.Points);
            Assert.IsFalse(target.Equals(unequal));

            target = new Unit(4, PageUnits.Points);
            unequal = new Unit(4, PageUnits.ViewPortHeight);
            Assert.IsFalse(target.Equals(unequal));

            
        }

        #endregion

        #region ToString() tests

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void ToString_Test()
        {
            ConfirmString("0%", true, new Unit(0, PageUnits.Percent));
            ConfirmString("20%", true, new Unit(20, PageUnits.Percent));

            ConfirmString("123.456vw", true, new Unit(123.456, PageUnits.ViewPortWidth));
            ConfirmString("123.457vmin", true, new Unit(123.457, PageUnits.ViewPortMin));
            ConfirmString("123.458vmax", true, new Unit(123.458, PageUnits.ViewPortMax));
            ConfirmString("123.459vh", true, new Unit(123.459, PageUnits.ViewPortHeight));

            ConfirmString("12.456em", true, new Unit(12.456, PageUnits.EMHeight));
            ConfirmString("12.457ch", true, new Unit(12.457, PageUnits.ZeroWidth));
            ConfirmString("12.458ex", true, new Unit(12.458, PageUnits.EXHeight));
            ConfirmString("12.459rem", true, new Unit(12.459, PageUnits.RootEMHeight));
        }


        private void ConfirmString(string expected, bool shouldmatch, Unit value)
        {
            string actual= value.ToString();
            if (shouldmatch)
                Assert.AreEqual(expected, actual, "The expected string value '" + expected + "' should match the actual value '" + actual + "'");
            else
                Assert.AreEqual(expected, actual, "The expected string value '" + expected + "' should not match the actual value '" + actual + "'");
        }

        #endregion        
        
        #region GetHashCode test

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        [TestCategory(TestCategory)]
        public void GetHashCode_Test()
        {
            

            Unit target = new Unit(72, PageUnits.ViewPortHeight);

            int expected = new Unit(72, PageUnits.ViewPortHeight).GetHashCode();
            int actual;
            actual = target.GetHashCode();
            Assert.AreEqual(expected, actual, "The hash codes for equvalent values should be the same");

            TestContext.WriteLine("Hash code {0} is equal to hash code {1}", target, expected);
        }

        #endregion

        
    }
}
