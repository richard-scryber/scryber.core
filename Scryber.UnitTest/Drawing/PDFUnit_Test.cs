using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using System.CodeDom;
using Scryber.Native;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFUnit_Test and is intended
    ///to contain all PDFUnit_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFUnit_Test
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

        //
        // constructors
        //

        #region .ctor tests


        /// <summary>
        ///A test for PDFUnit Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFUnitConstructor_Test()
        {
            int pointvalue = 0;
            PDFUnit target = new PDFUnit(pointvalue);
            Assert.AreEqual(0.0, target.PointsValue);
            Assert.AreEqual(0.0, target.ToInches().Value);
            Assert.AreEqual(0.0, target.ToMillimeters().Value);
            TestContext.WriteLine("Created new PDFUnit with integer value {0}", target);
        }

        /// <summary>
        ///A test for PDFUnit Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFUnitConstructor_Test1()
        {
            double pointvalue = 0; 
            PDFUnit target = new PDFUnit(pointvalue);
            Assert.AreEqual(0.0, target.PointsValue);
            Assert.AreEqual(0.0, target.ToInches().Value);
            Assert.AreEqual(0.0, target.ToMillimeters().Value);
            TestContext.WriteLine("Created new PDFUnit with double value {0}", target);
        }

        /// <summary>
        ///A test for PDFUnit Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFUnitConstructor_Test2()
        {
            double value = 0F; 
            PageUnits units = PageUnits.Millimeters;
            PDFUnit target = new PDFUnit(value, units);
            Assert.AreEqual(0.0, target.PointsValue);
            Assert.AreEqual(0.0, target.ToInches().Value);
            Assert.AreEqual(0.0, target.ToMillimeters().Value);
            TestContext.WriteLine("Created new PDFUnit with double value and mm units {0}", target);
        }

        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PDFUnitContrsuctor_Test3()
        {
            double mm = 25.4;
            PageUnits units = PageUnits.Millimeters;
            PDFUnit target = new PDFUnit(mm, units);
            Assert.AreEqual(25.4, target.Value);
            Assert.AreEqual(25.4, target.ToMillimeters().Value);
            Assert.AreEqual(1.0, target.ToInches().Value);
            Assert.AreEqual(72.0, target.ToPoints().Value);
            TestContext.WriteLine("Created new PDFUnit with double value and mm units {0}", target);
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
        [TestCategory("Drawing Structures")]
        public void Units_Test()
        {
            PDFUnit target = new PDFUnit(72);
            PageUnits expected = PageUnits.Points;
            PageUnits actual;
            actual = target.Units;
            Assert.AreEqual(expected, actual, "Default units were not Points");

            target = new PDFUnit(72, PageUnits.Points);
            actual = target.Units;
            Assert.AreEqual(expected, actual, "Units were not Points");

            target = new PDFUnit(25.4, PageUnits.Millimeters);
            actual = target.Units;
            expected = PageUnits.Millimeters;
            Assert.AreEqual(expected, actual, "Units were not Millimeters");

            target = new PDFUnit(1, PageUnits.Inches);
            actual = target.Units;
            expected = PageUnits.Inches;
            Assert.AreEqual(expected, actual, "Units were not Inches");

            TestContext.WriteLine("Units were returned correctly from the constructors");
        }

        /// <summary>
        ///A test for Value
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Value_Test()
        {
            PDFUnit target = new PDFUnit(72);
            double expected = 72;
            double actual;
            actual = target.Value;
            Assert.AreEqual(expected, actual, "1. Value was not in Points");

            target = new PDFUnit(72, PageUnits.Points);
            actual = target.Value;
            Assert.AreEqual(expected, actual, "2. Value was not in Points");

            target = new PDFUnit(25.4, PageUnits.Millimeters);
            actual = target.Value;
            expected = 25.4;
            Assert.AreEqual(expected, actual, "3. Values was not in mm");

            target = new PDFUnit(1, PageUnits.Inches);
            actual = target.Value;
            expected = 1;
            Assert.AreEqual(expected, actual, "4. Value was not in inches");

            TestContext.WriteLine("Values were returned correctly from the constructors");
        }

        #endregion

        #region IsEmpty

        /// <summary>
        ///A test for IsEmpty
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void IsEmpty_Test()
        {
            PDFUnit target = new PDFUnit();
            bool actual;
            bool expected = true;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual, "parameterless constructor did not return an Empty Unit value");

            target = new PDFUnit(1.0, PageUnits.Inches);
            expected = false;
            actual = target.IsEmpty;
            Assert.AreEqual(expected, actual, "A valid unit returned true for being empty");

        }

        #endregion

        #region PointsValue

        /// <summary>
        ///A test for PointsValue
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void PointsValue_Test()
        {
            PDFUnit target = new PDFUnit(1.0,PageUnits.Inches);
            double actual;
            actual = target.PointsValue;
            double expected = 72;
            Assert.AreEqual(expected, actual, "Points value was incorrect");

            target = new PDFUnit(25.4 * 4.0, PageUnits.Millimeters);
            expected = 72.0 * 4.0;
            actual = target.PointsValue;
            Assert.AreEqual(expected, actual, "Points value for millimeters was incorrect");

        }

        #endregion

        #region RealValue

        /// <summary>
        ///A test for RealValue
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void RealValue_Test()
        {
            PDFUnit target = new PDFUnit(1.0, PageUnits.Inches);
            PDFReal actual;
            actual = target.RealValue;
            PDFReal expected = 72;
            Assert.AreEqual(expected, actual, "Real value was incorrect");

            target = new PDFUnit(25.4 * 4.0, PageUnits.Millimeters);
            expected = 72.0 * 4.0;
            actual = target.PointsValue;
            Assert.AreEqual(expected, actual, "Real value for millimeters was incorrect");
        }

        #endregion

        //
        // calculation
        //

        #region Add (+) tests

        /// <summary>
        ///A test for Add where the units match
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Add_Test()
        {
            PDFUnit left = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit right = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit expected = new PDFUnit(20, PageUnits.Millimeters);
            PDFUnit actual;
            actual = PDFUnit.Add(left, right);
            Assert.AreEqual(expected, actual, "Addition of mm failed");
            TestContext.WriteLine("Value {0} plus {1}  calcualted as {2}", left, right, actual);
            actual = left + right;
            Assert.AreEqual(expected, actual, "Addition of mm failed");
            TestContext.WriteLine("Value {0} plus {1}  calcualted as {2}", left, right, actual);

            left = new PDFUnit(10, PageUnits.Inches);
            right = new PDFUnit(10, PageUnits.Inches);
            expected = new PDFUnit(20, PageUnits.Inches);
            actual = PDFUnit.Add(left, right);
            Assert.AreEqual(expected, actual, "Addition of inches failed");
            TestContext.WriteLine("Value {0} plus {1}  calcualted as {2}", left, right, actual);

            actual = left + right;
            Assert.AreEqual(expected, actual, "Addition of inches failed");
            TestContext.WriteLine("Value {0} plus {1}  calcualted as {2}", left, right, actual);

            left = new PDFUnit(10, PageUnits.Points);
            right = new PDFUnit(10, PageUnits.Points);
            expected = new PDFUnit(20, PageUnits.Points);
            actual = PDFUnit.Add(left, right);
            Assert.AreEqual(expected, actual, "Addition of points failed");
            TestContext.WriteLine("Value {0} plus {1}  calcualted as {2}", left, right, actual);

            actual = left + right;
            Assert.AreEqual(expected, actual, "Addition of points failed");
            TestContext.WriteLine("Value {0} plus {1}  calcualted as {2}", left, right, actual);

        }

        /// <summary>
        ///A test for Add where the units do not match
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Add_Test2()
        {
            PDFUnit left = new PDFUnit(25.4, PageUnits.Millimeters); //1 inch
            PDFUnit right = new PDFUnit(1, PageUnits.Inches); //1 inch
            PDFUnit expected = new PDFUnit(144, PageUnits.Points); //2 inches
            PDFUnit actual;
            actual = PDFUnit.Add(left, right);
            Assert.AreEqual(expected, actual, "Addition of mixed units failed");
            TestContext.WriteLine("Value {0} plus {1}  calcualted as {2}", left, right, actual);

            actual = left + right;
            Assert.AreEqual(expected, actual, "Addition of mixed units failed");
            TestContext.WriteLine("Value {0} plus {1}  calcualted as {2}", left, right, actual);



            left = new PDFUnit(144, PageUnits.Points); //2 inches
            right = new PDFUnit(2, PageUnits.Inches); //2 inches
            expected = new PDFUnit(25.4 * 4.0, PageUnits.Millimeters); //4 inches
            actual = PDFUnit.Add(left, right);
            Assert.AreEqual(expected, actual, "Addition of mixed units failed");
            TestContext.WriteLine("Value {0} plus {1}  calcualted as {2}", left, right, actual);

            actual = left + right;
            Assert.AreEqual(expected, actual, "Addition of mixed units failed");
            TestContext.WriteLine("Value {0} plus {1}  calcualted as {2}", left, right, actual);


            left = new PDFUnit(25.4, PageUnits.Millimeters);
            right = new PDFUnit(72, PageUnits.Points);
            expected = new PDFUnit(2, PageUnits.Inches);
            actual = PDFUnit.Add(left, right);
            Assert.AreEqual(expected, actual, "Addition of mixed units failed");
            TestContext.WriteLine("Value {0} plus {1}  calcualted as {2}", left, right, actual);

            actual = left + right;
            Assert.AreEqual(expected, actual, "Addition of mixed units failed");
            TestContext.WriteLine("Value {0} plus {1}  calcualted as {2}", left, right, actual);

        }

        #endregion

        #region Subtract (-) tests

        /// <summary>
        ///A test for Add where the units match
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Subtract_Test()
        {
            PDFUnit left = new PDFUnit(20, PageUnits.Millimeters);
            PDFUnit right = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit expected = new PDFUnit(10, PageUnits.Millimeters);
            PDFUnit actual;
            actual = PDFUnit.Subtract(left, right);
            Assert.AreEqual(expected, actual, "Subtraction of mm units failed");
            TestContext.WriteLine("Value {0} minus {1} calcualted as {2}", left, right, actual);

            actual = left - right;
            Assert.AreEqual(expected, actual, "Subtraction of mm units failed");
            TestContext.WriteLine("Value {0} minus {1} calcualted as {2}", left, right, actual);

            left = new PDFUnit(20, PageUnits.Inches);
            right = new PDFUnit(10, PageUnits.Inches);
            expected = new PDFUnit(10, PageUnits.Inches);
            actual = PDFUnit.Subtract(left, right);
            Assert.AreEqual(expected, actual, "Subtraction of inches units failed");
            TestContext.WriteLine("Value {0} minus {1} calcualted as {2}", left, right, actual);

            actual = left - right;
            Assert.AreEqual(expected, actual, "Subtraction of inches units failed");
            TestContext.WriteLine("Value {0} minus {1} calcualted as {2}", left, right, actual);

            left = new PDFUnit(20, PageUnits.Points);
            right = new PDFUnit(10, PageUnits.Points);
            expected = new PDFUnit(10, PageUnits.Points);
            actual = PDFUnit.Subtract(left, right);
            Assert.AreEqual(expected, actual, "Subtraction of Point units failed");
            TestContext.WriteLine("Value {0} minus {1} calcualted as {2}", left, right, actual);

            actual = left - right;
            Assert.AreEqual(expected, actual, "Subtraction of Point units failed");
            TestContext.WriteLine("Value {0} minus {1} calcualted as {2}", left, right, actual);

        }

        /// <summary>
        ///A test for Add where the units do not match
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Subtract_Test2()
        {
            PDFUnit left = new PDFUnit(50.8, PageUnits.Millimeters); //1 inch
            PDFUnit right = new PDFUnit(1, PageUnits.Inches); //1 inch
            PDFUnit expected = new PDFUnit(72, PageUnits.Points); //2 inches
            PDFUnit actual;
            actual = PDFUnit.Subtract(left, right);
            Assert.AreEqual(expected, actual, "Subtraction of mixed units failed");
            TestContext.WriteLine("Value {0} minus {1} calcualted as {2}", left, right, actual);

            actual = left - right;
            Assert.AreEqual(expected, actual, "Subtraction of mixed units failed");
            TestContext.WriteLine("Value {0} minus {1} calcualted as {2}", left, right, actual);



            left = new PDFUnit(144, PageUnits.Points); //2 inches
            right = new PDFUnit(1, PageUnits.Inches); //2 inches
            expected = new PDFUnit(25.4, PageUnits.Millimeters); //4 inches
            actual = PDFUnit.Subtract(left, right);
            Assert.AreEqual(expected, actual, "Subtraction of mixed units failed");
            TestContext.WriteLine("Value {0} minus {1} calcualted as {2}", left, right, actual);

            actual = left - right;
            Assert.AreEqual(expected, actual, "Subtraction of mixed units failed");
            TestContext.WriteLine("Value {0} minus {1} calcualted as {2}", left, right, actual);


            left = new PDFUnit(50.8, PageUnits.Millimeters);
            right = new PDFUnit(72, PageUnits.Points);
            expected = new PDFUnit(1, PageUnits.Inches);
            actual = PDFUnit.Subtract(left, right);
            Assert.AreEqual(expected, actual, "Subtraction of mixed units failed");
            TestContext.WriteLine("Value {0} minus {1} calcualted as {2}", left, right, actual);

            actual = left - right;
            Assert.AreEqual(expected, actual, "Subtraction of mixed units failed");
            TestContext.WriteLine("Value {0} minus {1} calcualted as {2}", left, right, actual);

        }

        #endregion

        #region Divide (/) test

        /// <summary>
        ///A test for Divide and the divide operator on PDFUnits and double operands
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void DivideDouble_Test()
        {
            PDFUnit left = new PDFUnit(10,PageUnits.Inches);
            double right = 2; 
            PDFUnit expected = new PDFUnit(5, PageUnits.Inches);
            PDFUnit actual;
            actual = PDFUnit.Divide(left, right);
            Assert.AreEqual(expected, actual, "Divide in inches failed");
            Assert.AreEqual(actual.Units, PageUnits.Inches, "Units changed for divide op");
            TestContext.WriteLine("Value {0} divided by {1} calculated as {2}", left, right, actual);

            actual = left / right;
            Assert.AreEqual(expected, actual, "Divide op in inches failed");
            Assert.AreEqual(actual.Units, PageUnits.Inches, "Units changed for divide op");
            TestContext.WriteLine("Value {0} divided by {1} calculated as {2}", left, right, actual);

            left = new PDFUnit(100, PageUnits.Millimeters);
            right = 2;
            expected = new PDFUnit(50, PageUnits.Millimeters);

            actual = PDFUnit.Divide(left, right);
            Assert.AreEqual(expected, actual, "Divide in millimeters failed");
            Assert.AreEqual(actual.Units, PageUnits.Millimeters, "Units changed for divide");
            TestContext.WriteLine("Value {0} divided by {1} calculated as {2}", left, right, actual);

            actual = left / right;
            Assert.AreEqual(expected, actual, "Divide op in millimeters failed");
            Assert.AreEqual(actual.Units, PageUnits.Millimeters, "Units changed for divide op");
            TestContext.WriteLine("Value {0} divided by {1} calculated as {2}", left, right, actual);

            left = new PDFUnit(144, PageUnits.Points);
            right = 2;
            expected = new PDFUnit(72, PageUnits.Points);

            actual = PDFUnit.Divide(left, right);
            Assert.AreEqual(expected, actual, "Divide in points failed");
            Assert.AreEqual(actual.Units, PageUnits.Points, "Units changed for divide");
            TestContext.WriteLine("Value {0} divided by {1} calculated as {2}", left, right, actual);

            actual = left / right;
            Assert.AreEqual(expected, actual, "Divide op in millimeters failed");
            Assert.AreEqual(actual.Units, PageUnits.Points, "Units changed for divide op");
            TestContext.WriteLine("Value {0} divided by {1} calculated as {2}", left, right, actual);
        }

        /// <summary>
        ///A test for Divide and the divide operator on PDFUnits and double operands
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void DivideInt_Test()
        {
            PDFUnit left = new PDFUnit(10, PageUnits.Inches);
            int right = 2;
            PDFUnit expected = new PDFUnit(5, PageUnits.Inches);
            PDFUnit actual;
            actual = PDFUnit.Divide(left, right);
            Assert.AreEqual(expected, actual, "Divide in inches failed");
            Assert.AreEqual(actual.Units, PageUnits.Inches, "Units changed for divide op");
            TestContext.WriteLine("Value {0} divided by {1} calculated as {2}", left, right, actual);

            actual = left / right;
            Assert.AreEqual(expected, actual, "Divide op in inches failed");
            Assert.AreEqual(actual.Units, PageUnits.Inches, "Units changed for divide op");
            TestContext.WriteLine("Value {0} divided by {1} calculated as {2}", left, right, actual);

            left = new PDFUnit(100, PageUnits.Millimeters);
            right = 2;
            expected = new PDFUnit(50, PageUnits.Millimeters);

            actual = PDFUnit.Divide(left, right);
            Assert.AreEqual(expected, actual, "Divide in millimeters failed");
            Assert.AreEqual(actual.Units, PageUnits.Millimeters, "Units changed for divide");
            TestContext.WriteLine("Value {0} divided by {1} calculated as {2}", left, right, actual);

            actual = left / right;
            Assert.AreEqual(expected, actual, "Divide op in millimeters failed");
            Assert.AreEqual(actual.Units, PageUnits.Millimeters, "Units changed for divide op");

            left = new PDFUnit(144, PageUnits.Points);
            right = 2;
            expected = new PDFUnit(72, PageUnits.Points);

            actual = PDFUnit.Divide(left, right);
            Assert.AreEqual(expected, actual, "Divide in points failed");
            Assert.AreEqual(actual.Units, PageUnits.Points, "Units changed for divide");
            TestContext.WriteLine("Value {0} divided by {1} calculated as {2}", left, right, actual);

            actual = left / right;
            Assert.AreEqual(expected, actual, "Divide op in millimeters failed");
            Assert.AreEqual(actual.Units, PageUnits.Points, "Units changed for divide op");
            TestContext.WriteLine("Value {0} divided by {1} calculated as {2}", left, right, actual);
        }

        #endregion

        #region Multiply (*) test

        /// <summary>
        ///A test for Multiply and the multiply operator for doubles
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void MultiplyDouble_Test()
        {
            PDFUnit left = new PDFUnit(10, PageUnits.Inches);
            double right = 2;
            PDFUnit expected = new PDFUnit(20, PageUnits.Inches);
            PDFUnit actual;
            actual = PDFUnit.Multiply(left, right);
            Assert.AreEqual(expected, actual, "Multiply in inches failed");
            Assert.AreEqual(actual.Units, PageUnits.Inches, "Units changed for Multiply op");
            TestContext.WriteLine("Value {0} multiplied by {1} calculated as {2}", left, right, actual);

            actual = left * right;
            Assert.AreEqual(expected, actual, "Multiply op in inches failed");
            Assert.AreEqual(actual.Units, PageUnits.Inches, "Units changed for Multiply op");
            TestContext.WriteLine("Value {0} multiplied by {1} calculated as {2}", left, right, actual);

            left = new PDFUnit(100, PageUnits.Millimeters);
            right = 2;
            expected = new PDFUnit(200, PageUnits.Millimeters);

            actual = PDFUnit.Multiply(left, right);
            Assert.AreEqual(expected, actual, "Multiply in millimeters failed");
            Assert.AreEqual(actual.Units, PageUnits.Millimeters, "Units changed for Multiply");
            TestContext.WriteLine("Value {0} multiplied by {1} calculated as {2}", left, right, actual);

            actual = left * right;
            Assert.AreEqual(expected, actual, "Multiply op in millimeters failed");
            Assert.AreEqual(actual.Units, PageUnits.Millimeters, "Units changed for Multiply op");
            TestContext.WriteLine("Value {0} multiplied by {1} calculated as {2}", left, right, actual);

            left = new PDFUnit(144, PageUnits.Points);
            right = 2;
            expected = new PDFUnit(288, PageUnits.Points);

            actual = PDFUnit.Multiply(left, right);
            Assert.AreEqual(expected, actual, "Multiply in points failed");
            Assert.AreEqual(actual.Units, PageUnits.Points, "Units changed for Multiply");
            TestContext.WriteLine("Value {0} multiplied by {1} calculated as {2}", left, right, actual);

            actual = left * right;
            Assert.AreEqual(expected, actual, "Multiply op in points failed");
            Assert.AreEqual(actual.Units, PageUnits.Points, "Units changed for Multiply op");
            TestContext.WriteLine("Value {0} multiplied by {1} calculated as {2}", left, right, actual);

        }

        /// <summary>
        ///A test for Multiply and the multiply operator
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void MultiplyInt_Test()
        {
            PDFUnit left = new PDFUnit(10, PageUnits.Inches);
            int right = 2;
            PDFUnit expected = new PDFUnit(20, PageUnits.Inches);
            PDFUnit actual;
            actual = PDFUnit.Multiply(left, right);
            Assert.AreEqual(expected, actual, "Multiply in inches failed");
            Assert.AreEqual(actual.Units, PageUnits.Inches, "Units changed for Multiply op");
            TestContext.WriteLine("Value {0} multiplied by {1} calculated as {2}", left, right, actual);

            actual = left * right;
            Assert.AreEqual(expected, actual, "Multiply op in inches failed");
            Assert.AreEqual(actual.Units, PageUnits.Inches, "Units changed for Multiply op");
            TestContext.WriteLine("Value {0} multiplied by {1} calculated as {2}", left, right, actual);

            left = new PDFUnit(100, PageUnits.Millimeters);
            right = 2;
            expected = new PDFUnit(200, PageUnits.Millimeters);

            actual = PDFUnit.Multiply(left, right);
            Assert.AreEqual(expected, actual, "Multiply in millimeters failed");
            Assert.AreEqual(actual.Units, PageUnits.Millimeters, "Units changed for Multiply");
            TestContext.WriteLine("Value {0} multiplied by {1} calculated as {2}", left, right, actual);

            actual = left * right;
            Assert.AreEqual(expected, actual, "Multiply op in millimeters failed");
            Assert.AreEqual(actual.Units, PageUnits.Millimeters, "Units changed for Multiply op");
            TestContext.WriteLine("Value {0} multiplied by {1} calculated as {2}", left, right, actual);

            left = new PDFUnit(144, PageUnits.Points);
            right = 2;
            expected = new PDFUnit(288, PageUnits.Points);

            actual = PDFUnit.Multiply(left, right);
            Assert.AreEqual(expected, actual, "Multiply in points failed");
            Assert.AreEqual(actual.Units, PageUnits.Points, "Units changed for Multiply");
            TestContext.WriteLine("Value {0} multiplied by {1} calculated as {2}", left, right, actual);

            actual = left * right;
            Assert.AreEqual(expected, actual, "Multiply op in points failed");
            Assert.AreEqual(actual.Units, PageUnits.Points, "Units changed for Multiply op");
            TestContext.WriteLine("Value {0} multiplied by {1} calculated as {2}", left, right, actual);
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
        [TestCategory("Drawing Structures")]
        public void Compare_Test()
        {
            PDFUnit one = new PDFUnit(72, PageUnits.Points);
            PDFUnit two = new PDFUnit(72, PageUnits.Points);

            int expected = 0;//equal

            int actual = PDFUnit.Compare(one, two);
            Assert.AreEqual(expected, actual, "Compare of equal Points to Points failed");
            TestContext.WriteLine("Point value {0} is equal to {1}", one, two);

            one = new PDFUnit(1, PageUnits.Inches);
            actual = PDFUnit.Compare(one, two);
            Assert.AreEqual(expected, actual, "Compare of equal Inches to Points failed");
            TestContext.WriteLine("Inch value {0} is equal to Point value {1}", one, two);

            one = new PDFUnit(25.4, PageUnits.Millimeters);
            actual = PDFUnit.Compare(one, two);
            Assert.AreEqual(expected, actual, "Compare of equal Millimeters to Points failed");
            TestContext.WriteLine("mm value {0} is equal to Point value {1}", one, two);

            expected = 1; //greater than

            one = new PDFUnit(144, PageUnits.Points);
            actual = PDFUnit.Compare(one, two);
            Assert.AreEqual(expected, actual, "Compare of greater than Points to Points failed");
            TestContext.WriteLine("Point value {0} is greater than Point value {1}", one, two);

            one = new PDFUnit(50.8, PageUnits.Millimeters);
            actual = PDFUnit.Compare(one, two);
            Assert.AreEqual(expected, actual, "Compare of greater than Millimeters to Points failed");
            TestContext.WriteLine("mm value {0} is greater than Point value {1}", one, two);

            one = new PDFUnit(2, PageUnits.Inches);
            actual = PDFUnit.Compare(one, two);
            Assert.AreEqual(expected, actual, "Compare of greater than Inches to Points failed");
            TestContext.WriteLine("Inches value {0} is greater than Point value {1}", one, two);

            expected = -1; //less than

            one = new PDFUnit(36, PageUnits.Points);
            actual = PDFUnit.Compare(one, two);
            Assert.AreEqual(expected, actual, "Compare of less than Points to Points failed");
            TestContext.WriteLine("Point value {0} is less than Point value {1}", one, two);

            one = new PDFUnit(12.8, PageUnits.Millimeters);
            actual = PDFUnit.Compare(one, two);
            Assert.AreEqual(expected, actual, "Compare of less than Millimeters to Points failed");
            TestContext.WriteLine("Millimeter value {0} is less than Point value {1}", one, two);

            one = new PDFUnit(0.5, PageUnits.Inches);
            actual = PDFUnit.Compare(one, two);
            Assert.AreEqual(expected, actual, "Compare of less than Inches to Points failed");
            TestContext.WriteLine("Inches value {0} is less than Point value {1}", one, two);


        }

        /// <summary>
        ///A test for CompareTo(object)
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void CompareTo_Test()
        {
            PDFUnit target = new PDFUnit(72, PageUnits.Points);
            object obj = new PDFUnit(72, PageUnits.Points);
            int expected = 0;
            int actual;
            actual = target.CompareTo(obj);
            Assert.AreEqual(expected, actual, "Object and Unit were not compared equal");
            TestContext.WriteLine("object value {0} is equal to Point value {1}", obj, target);

            obj = new PDFUnit(1, PageUnits.Inches);
            actual = target.CompareTo(obj);
            Assert.AreEqual(expected, actual, "Object and Unit were not compared equal");
            TestContext.WriteLine("object value {0} is equal to Point value {1}", obj, target);

            obj = new PDFUnit(25.4, PageUnits.Millimeters);
            actual = target.CompareTo(obj);
            Assert.AreEqual(expected, actual, "Object and Unit were not compared equal");
            TestContext.WriteLine("object value {0} is equal to Point value {1}", obj, target);

            expected = -1; //less than

            obj = new PDFUnit(144, PageUnits.Points);
            actual = target.CompareTo(obj);
            Assert.AreEqual(expected, actual, "Object and Unit were not compared less than");
            TestContext.WriteLine("object value {0} is less than Point value {1}", obj, target);


            obj = new PDFUnit(2, PageUnits.Inches);
            actual = target.CompareTo(obj);
            Assert.AreEqual(expected, actual, "Object and Unit were not compared less than");
            TestContext.WriteLine("object value {0} is less than Point value {1}", obj, target);

            obj = new PDFUnit(50.8, PageUnits.Millimeters);
            actual = target.CompareTo(obj);
            Assert.AreEqual(expected, actual, "Object and Unit were not compared less than");
            TestContext.WriteLine("object value {0} is less than Point value {1}", obj, target);

            expected = 1; //greater than

            obj = new PDFUnit(36, PageUnits.Points);
            actual = target.CompareTo(obj);
            Assert.AreEqual(expected, actual, "Object and Unit were not compared greater than");
            TestContext.WriteLine("object value {0} is greater than Point value {1}", obj, target);


            obj = new PDFUnit(0.5, PageUnits.Inches);
            actual = target.CompareTo(obj);
            Assert.AreEqual(expected, actual, "Object and Unit were not compared greater than");
            TestContext.WriteLine("object value {0} is greater than Point value {1}", obj, target);

            obj = new PDFUnit(12.7, PageUnits.Millimeters);
            actual = target.CompareTo(obj);
            Assert.AreEqual(expected, actual, "Object and Unit were not compared greater than");
            TestContext.WriteLine("object value {0} is greater than Point value {1}", obj, target);

        }

        /// <summary>
        ///A test for CompareTo Unit to Unit
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void CompareTo_Test1()
        {
            PDFUnit target = new PDFUnit(72, PageUnits.Points);
            PDFUnit unit = new PDFUnit(72, PageUnits.Points);
            int expected = 0;
            int actual;
            actual = target.CompareTo(unit);
            Assert.AreEqual(expected, actual, "Unit and Unit were not compared equal");
            TestContext.WriteLine("Unit value {0} is equal to {1}", target, unit);

            unit = new PDFUnit(1, PageUnits.Inches);
            actual = target.CompareTo(unit);
            Assert.AreEqual(expected, actual, "Unit and Unit were not compared equal");
            TestContext.WriteLine("Unit value {0} is equal to {1}", target, unit);

            unit = new PDFUnit(25.4, PageUnits.Millimeters);
            actual = target.CompareTo(unit);
            Assert.AreEqual(expected, actual, "Unit and Unit were not compared equal");
            TestContext.WriteLine("Unit value {0} is equal to {1}", target, unit);

            expected = -1; //less than

            unit = new PDFUnit(144, PageUnits.Points);
            actual = target.CompareTo(unit);
            Assert.AreEqual(expected, actual, "Unit and Unit were not compared less than");
            TestContext.WriteLine("Unit value {0} is less than to {1}", target, unit);

            unit = new PDFUnit(2, PageUnits.Inches);
            actual = target.CompareTo(unit);
            Assert.AreEqual(expected, actual, "Unit and Unit were not compared less than");
            TestContext.WriteLine("Unit value {0} is less than to {1}", target, unit);

            unit = new PDFUnit(50.8, PageUnits.Millimeters);
            actual = target.CompareTo(unit);
            Assert.AreEqual(expected, actual, "Unit and Unit were not compared less than");
            TestContext.WriteLine("Unit value {0} is less than to {1}", target, unit);

            expected = 1; //greater than

            unit = new PDFUnit(36, PageUnits.Points);
            actual = target.CompareTo(unit);
            Assert.AreEqual(expected, actual, "Unit and Unit were not compared greater than");
            TestContext.WriteLine("Unit value {0} is greater than to {1}", target, unit);


            unit = new PDFUnit(0.5, PageUnits.Inches);
            actual = target.CompareTo(unit);
            Assert.AreEqual(expected, actual, "Unit and Unit were not compared greater than");
            TestContext.WriteLine("Unit value {0} is greater than to {1}", target, unit);

            unit = new PDFUnit(12.7, PageUnits.Millimeters);
            actual = target.CompareTo(unit);
            Assert.AreEqual(expected, actual, "Unit and Unit were not compared greater than");
            TestContext.WriteLine("Unit value {0} is greater than to {1}", target, unit);

        }

        #endregion

        #region PDFUnit.Equals(unit1,unit2), ==, unit1.Equals(object), unit1.Equals(unit2) tests

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void EqualStatic_Test()
        {
            PDFUnit left = new PDFUnit(72,PageUnits.Points); 
            PDFUnit right = new PDFUnit(1, PageUnits.Inches);
            bool expected = true;
            bool actual;
            actual = PDFUnit.Equals(left, right);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as equal");
            TestContext.WriteLine("Value {0} equals {1} calculated as {2}", left, right, actual);

            actual = left == right;
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as equal");
            TestContext.WriteLine("Value {0} equals {1} calculated as {2}", left, right, actual);

            left = new PDFUnit(144, PageUnits.Points);
            expected = false;
            actual = PDFUnit.Equals(left, right);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as un-equal");
            TestContext.WriteLine("Value {0} equals {1} calculated as {2}", left, right, actual);

            actual = left == right;
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as un-equal");
            TestContext.WriteLine("Value {0} equals {1} calculated as {2}", left, right, actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void EqualToObject_Test1()
        {
            PDFUnit target = new PDFUnit(72,PageUnits.Points);
            object obj = new PDFUnit(1, PageUnits.Inches);
            bool expected = true; 
            bool actual;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as equal");
            TestContext.WriteLine("Value {0} equals {1} calculated as {2}", target, obj, actual);

            obj = new PDFUnit(2, PageUnits.Inches);
            expected = false;
            actual = target.Equals(obj);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as un-equal");
            TestContext.WriteLine("Value {0} equals {1} calculated as {2}", target, obj, actual);
        }


        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void EqualToUnit_Test2()
        {
            PDFUnit target = new PDFUnit(72, PageUnits.Points);
            PDFUnit unit = new PDFUnit(1, PageUnits.Inches);
            bool expected = true;
            bool actual;
            actual = target.Equals(unit);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as equal");
            TestContext.WriteLine("Value {0} equals {1} calculated as {2}", target, unit, actual);

            unit = new PDFUnit(2, PageUnits.Inches);
            expected = false;
            actual = target.Equals(unit);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as un-equal");
            TestContext.WriteLine("Value {0} equals {1} calculated as {2}", target, unit, actual);
        }

        #endregion

        #region NotEquals(left,right), !=   tests

        /// <summary>
        ///A test for NotEquals
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void NotEquals_Test()
        {
            PDFUnit left = new PDFUnit(72,PageUnits.Points); 
            PDFUnit right = new PDFUnit(1, PageUnits.Inches); 
            bool expected = false; //They are equal
            bool actual;
            actual = PDFUnit.NotEquals(left, right);
            Assert.AreEqual(expected, actual, "1. Evaluated NotEquals falied");
            TestContext.WriteLine("Value {0} not equal to {1} calculated as {2}", left, right, actual);

            actual = (left != right);
            Assert.AreEqual(expected, actual, "2. Evaluated'!=' failed");
            TestContext.WriteLine("Value {0} not equal to {1} calculated as {2}", left, right, actual);

            right = new PDFUnit(2, PageUnits.Inches);
            expected = true;

            actual = PDFUnit.NotEquals(left, right);
            Assert.AreEqual(expected, actual, "3. Evaluated NotEquals falied");
            TestContext.WriteLine("Value {0} not equal to {1} calculated as {2}", left, right, actual);

            actual = (left != right);
            Assert.AreEqual(expected, actual, "4. Evaluated '!=' failed");
            TestContext.WriteLine("Value {0} not equal to {1} calculated as {2}", left, right, actual);
        }

        #endregion

        #region GreaterThan, >, GreaterThanEqual, >=  tests

        /// <summary>
        ///A test for GreaterThan
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void GreaterThan_Test()
        {
            PDFUnit left = new PDFUnit(144, PageUnits.Points);
            PDFUnit right = new PDFUnit(1, PageUnits.Inches);

            bool expected = true;
            bool actual;
            actual = PDFUnit.GreaterThan(left, right);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as greater than");
            TestContext.WriteLine("Value {0} greater than {1} calculated as {2}", left, right, actual);

            actual = (left > right);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as greater than op");
            TestContext.WriteLine("Value {0} greater than {1} calculated as {2}", left, right, actual);

            right = new PDFUnit(2, PageUnits.Inches);
            expected = false; //same value

            actual = PDFUnit.GreaterThan(left, right);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as not greater than");
            TestContext.WriteLine("Value {0} greater than {1} calculated as {2}", left, right, actual);

            actual = (left > right);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as not greater than op");
            TestContext.WriteLine("Value {0} greater than {1} calculated as {2}", left, right, actual);

            right = new PDFUnit(3, PageUnits.Inches);
            expected = false; //different larger value

            actual = PDFUnit.GreaterThan(left, right);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as not greater than");
            TestContext.WriteLine("Value {0} greater than {1} calculated as {2}", left, right, actual);

            actual = (left > right);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as not greater than op");
            TestContext.WriteLine("Value {0} greater than {1} calculated as {2}", left, right, actual);

        }

        /// <summary>
        ///A test for GreaterThanEqual
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void GreaterThanEqual_Test()
        {
            PDFUnit left = new PDFUnit(144, PageUnits.Points);
            PDFUnit right = new PDFUnit(1, PageUnits.Inches);

            bool expected = true;
            bool actual;
            actual = PDFUnit.GreaterThanEqual(left, right);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as greater than equal");
            TestContext.WriteLine("Value {0} greater than or equal to {1} calculated as {2}", left, right, actual);

            actual = (left >= right);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as greater than equal op");
            TestContext.WriteLine("Value {0} greater than or equal to {1} calculated as {2}", left, right, actual);

            right = new PDFUnit(2, PageUnits.Inches);
            expected = true; //same value

            actual = PDFUnit.GreaterThanEqual(left, right);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as not greater than equal");
            TestContext.WriteLine("Value {0} greater than or equal to {1} calculated as {2}", left, right, actual);

            actual = (left >= right);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as not greater than equal op");
            TestContext.WriteLine("Value {0} greater than or equal to {1} calculated as {2}", left, right, actual);

            right = new PDFUnit(3, PageUnits.Inches);
            expected = false; //different larger value

            actual = PDFUnit.GreaterThanEqual(left, right);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as not greater than equal");
            TestContext.WriteLine("Value {0} greater than or equal to {1} calculated as {2}", left, right, actual);

            actual = (left >= right);
            Assert.AreEqual(expected, actual, "The PDFUnits were not evaluated as not greater than equal op");
            TestContext.WriteLine("Value {0} greater than or equal to {1} calculated as {2}", left, right, actual);
        }


        #endregion

        #region LessThan, <, LessThanEqual, <= tests

        /// <summary>
        ///A test for LessThan
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void LessThan_Test()
        {
            PDFUnit left = new PDFUnit(72, PageUnits.Points);
            PDFUnit right = new PDFUnit(2, PageUnits.Inches);

            bool expected = true;
            bool actual;
            actual = PDFUnit.LessThan(left, right);
            Assert.AreEqual(expected, actual, "1. The PDFUnits were not evaluated as less than");
            TestContext.WriteLine("Value {0} less than {1} calculated as {2}", left, right, actual);

            actual = (left < right);
            Assert.AreEqual(expected, actual, "2. The PDFUnits were not evaluated as less than op");
            TestContext.WriteLine("Value {0} less than {1} calculated as {2}", left, right, actual);

            right = new PDFUnit(1, PageUnits.Inches);
            expected = false; //same value

            actual = PDFUnit.LessThan(left, right);
            Assert.AreEqual(expected, actual, "3. The PDFUnits were not evaluated as not less than");
            TestContext.WriteLine("Value {0} less than {1} calculated as {2}", left, right, actual);

            actual = (left < right);
            Assert.AreEqual(expected, actual, "4. The PDFUnits were not evaluated as not less than op");
            TestContext.WriteLine("Value {0} less than {1} calculated as {2}", left, right, actual);

            right = new PDFUnit(0.5, PageUnits.Inches);
            expected = false; //different larger value

            actual = PDFUnit.LessThan(left, right);
            Assert.AreEqual(expected, actual, "5. The PDFUnits were not evaluated as not less than");
            TestContext.WriteLine("Value {0} less than {1} calculated as {2}", left, right, actual);

            actual = (left < right);
            Assert.AreEqual(expected, actual, "6. The PDFUnits were not evaluated as not less than op");
            TestContext.WriteLine("Value {0} less than {1} calculated as {2}", left, right, actual);
        }

        /// <summary>
        ///A test for LessThanEqual
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void LessThanEqual_Test()
        {
            PDFUnit left = new PDFUnit(72, PageUnits.Points);
            PDFUnit right = new PDFUnit(2, PageUnits.Inches);

            bool expected = true;
            bool actual;
            actual = PDFUnit.LessThanEqual(left, right);
            Assert.AreEqual(expected, actual, "1. The PDFUnits were not evaluated as less than equal");
            TestContext.WriteLine("Value {0} less than or equal to {1} calculated as {2}", left, right, actual);

            actual = (left <= right);
            Assert.AreEqual(expected, actual, "2. The PDFUnits were not evaluated as less than equal op");
            TestContext.WriteLine("Value {0} less than or equal to {1} calculated as {2}", left, right, actual);

            right = new PDFUnit(1, PageUnits.Inches);
            expected = true; //same value

            actual = PDFUnit.LessThanEqual(left, right);
            Assert.AreEqual(expected, actual, "3. The PDFUnits were not evaluated as not less than equal");
            TestContext.WriteLine("Value {0} less than or equal to {1} calculated as {2}", left, right, actual);

            actual = (left <= right);
            Assert.AreEqual(expected, actual, "4. The PDFUnits were not evaluated as not less than equal op");
            TestContext.WriteLine("Value {0} less than or equal to {1} calculated as {2}", left, right, actual);

            right = new PDFUnit(0.5, PageUnits.Inches);
            expected = false; //larger value

            actual = PDFUnit.LessThanEqual(left, right);
            Assert.AreEqual(expected, actual, "5. The PDFUnits were not evaluated as not less than equal");
            TestContext.WriteLine("Value {0} less than or equal to {1} calculated as {2}", left, right, actual);

            actual = (left <= right);
            Assert.AreEqual(expected, actual, "6. The PDFUnits were not evaluated as not less than equal op");
            TestContext.WriteLine("Value {0} less than or equal to {1} calculated as {2}", left, right, actual);
        }

        #endregion

        #region Min, Max tests

        /// <summary>
        ///A test for Max
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Max_Test()
        {
            PDFUnit a = new PDFUnit(72, PageUnits.Points);
            PDFUnit b = new PDFUnit(2, PageUnits.Inches);
            PDFUnit expected = b;
            PDFUnit actual;
            actual = PDFUnit.Max(a, b);
            Assert.AreEqual(expected, actual, "Max returned incorrect value");
            TestContext.WriteLine("Max value of {0} and {1} calculated as {2}", a, b, actual);
        }

        /// <summary>
        ///A test for Min
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Min_Test()
        {
            PDFUnit a = new PDFUnit(72, PageUnits.Points);
            PDFUnit b = new PDFUnit(2, PageUnits.Inches);
            PDFUnit expected = a;
            PDFUnit actual;
            actual = PDFUnit.Min(a, b);
            Assert.AreEqual(expected, actual, "Min returned incorrect value");
            TestContext.WriteLine("Min value of {0} and {1} calculated as {2}", a, b, actual);
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
        [TestCategory("Drawing Structures")]
        public void Parse_Test()
        {
            Parse("10pt", new PDFUnit(10,PageUnits.Points));
            Parse("20.543pt", new PDFUnit(20.543, PageUnits.Points));
            Parse("000123.456", new PDFUnit(123.456));
            Parse("1.5in", new PDFUnit(1.5, PageUnits.Inches));
            Parse("234567890123.12345mm", new PDFUnit(234567890123.12345, PageUnits.Millimeters));

            try
            {
                Parse("ThisIsAFailpt", new PDFUnit());
                throw new Exception("No exception raised for invalid argument");
            }
            catch (ArgumentException)
            {
                TestContext.WriteLine("Failed Parse ArgumentException caught correctly");
            }

        }

        private PDFUnit Parse(string value, PDFUnit expected)
        {
            PDFUnit actual;
            actual = PDFUnit.Parse(value);
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
        [TestCategory("Drawing Structures")]
        public void TryParse_Test()
        {
            TryParse("10pt", true, new PDFUnit(10, PageUnits.Points));
            TryParse("20.543pt",true, new PDFUnit(20.543, PageUnits.Points));
            TryParse("000123.456", true, new PDFUnit(123.456));
            TryParse("1.5in", true, new PDFUnit(1.5, PageUnits.Inches));
            TryParse("234567890123.12345mm", true, new PDFUnit(234567890123.12345, PageUnits.Millimeters));
            TryParse("ThisIsAFailpt", false, PDFUnit.Empty);

        }

        private void TryParse(string value, bool expectedresult, PDFUnit expectedvalue)
        {
            PDFUnit parsed;
            bool actualresult = PDFUnit.TryParse(value, out parsed);
            Assert.AreEqual(expectedresult, actualresult,"The expected TryParse returned value was not the same as the expected result");
            Assert.AreEqual(expectedvalue, parsed, "The parsed value for TryParse was not the same as the expected value");
            if(expectedresult)
                this.TestContext.WriteLine("TryParse string '{0}' succeeded parsing to PDFUnit {1}", value, parsed);
            else
                this.TestContext.WriteLine("TryParse string '{0}' was expected not to parse, and returned value {1}", value, parsed);
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
        [TestCategory("Drawing Structures")]
        public void Convert_Test()
        {
            PDFUnit expected = new PDFUnit(1, PageUnits.Inches);
            PDFUnit actual;

            PDFUnit unit = new PDFUnit(25.4, PageUnits.Millimeters);
            actual = PDFUnit.Convert(unit, PageUnits.Inches);
            Assert.AreEqual(expected.Value, actual.Value, "The converted millimeters to inches failed");
            TestContext.WriteLine("Value {0} converted to {1}", unit, actual);

            unit = new PDFUnit(72, PageUnits.Points);
            actual = PDFUnit.Convert(unit, PageUnits.Inches);
            Assert.AreEqual(expected.Value, actual.Value, "The converted points to inches failed");
            TestContext.WriteLine("Value {0} converted to {1}", unit, actual);

            expected = new PDFUnit(144, PageUnits.Points);

            unit = new PDFUnit(2, PageUnits.Inches);
            actual = PDFUnit.Convert(unit, PageUnits.Points);
            Assert.AreEqual(expected.Value, actual.Value, "The converted inches to points failed");
            TestContext.WriteLine("Value {0} converted to {1}", unit, actual);

            unit = new PDFUnit(50.8, PageUnits.Millimeters);
            actual = PDFUnit.Convert(unit, PageUnits.Points);
            Assert.AreEqual(expected.Value, actual.Value, "The converted millimeters to points failed");
            TestContext.WriteLine("Value {0} converted to {1}", unit, actual);

            expected = new PDFUnit(12.7, PageUnits.Millimeters);

            unit = new PDFUnit(0.5, PageUnits.Inches);
            actual = PDFUnit.Convert(unit, PageUnits.Millimeters);
            Assert.AreEqual(expected.Value, actual.Value, "The converted inches to millimeters failed");
            TestContext.WriteLine("Value {0} converted to {1}", unit, actual);

            unit = new PDFUnit(36, PageUnits.Points);
            actual = PDFUnit.Convert(unit, PageUnits.Millimeters);
            Assert.AreEqual(expected.Value, actual.Value, "The converted points to millimeters failed");
            TestContext.WriteLine("Value {0} converted to {1}", unit, actual);
        }

        #endregion

        #region ToInches(), ToMillimeters(), ToPoints()


        /// <summary>
        ///A test for ToInches
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void ToInches_Test()
        {
            PDFUnit target = new PDFUnit(72,PageUnits.Points); 
            PDFUnit expected = new PDFUnit(1,PageUnits.Inches);
            PDFUnit actual;
            actual = target.ToInches();
            Assert.AreEqual(expected.Value, actual.Value, "ToInches() did not return the correct Value from Points");
            Assert.AreEqual(expected.Units, actual.Units, "ToInches() did not return the correct Units from Points");
            
        }

        /// <summary>
        ///A test for ToMillimeters
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void ToMillimeters_Test()
        {
            PDFUnit target = new PDFUnit(72, PageUnits.Points);
            PDFUnit expected = new PDFUnit(25.4, PageUnits.Millimeters);
            PDFUnit actual;
            actual = target.ToMillimeters();
            Assert.AreEqual(expected.Value, actual.Value, "ToMillimeters() did not return the correct Value from Points");
            Assert.AreEqual(expected.Units, actual.Units, "ToMillimeters() did not return the correct Units from Points");
        }

        /// <summary>
        ///A test for ToPoints
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void ToPoints_Test()
        {
            PDFUnit target = new PDFUnit(1, PageUnits.Inches);
            PDFUnit expected = new PDFUnit(72, PageUnits.Points);
            PDFUnit actual;
            actual = target.ToPoints();
            Assert.AreEqual(expected.Value, actual.Value, "ToPoints() did not return the correct Value from Points");
            Assert.AreEqual(expected.Units, actual.Units, "ToPoints() did not return the correct Units from Points");
        }

        #endregion

        #region ToString() tests

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void ToString_Test()
        {
            ConfirmString("0pt", true, new PDFUnit(0, PageUnits.Points));
            ConfirmString("20pt", true, new PDFUnit(20, PageUnits.Points));
            ConfirmString("123.456pt", true, new PDFUnit(123.456));
            ConfirmString("1.5in", true, new PDFUnit(1.5, PageUnits.Inches));

            //Three decimal places seems the most accurate.
            ConfirmString("234567890123.12344mm", true, new PDFUnit(234567890123.12344, PageUnits.Millimeters));
        }


        private void ConfirmString(string expected, bool shouldmatch, PDFUnit value)
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
        [TestCategory("Drawing Structures")]
        public void GetHashCode_Test()
        {
            PDFUnit target = new PDFUnit(72, PageUnits.Points);

            int expected = new PDFUnit(1, PageUnits.Inches).GetHashCode();
            int actual;
            actual = target.GetHashCode();
            Assert.AreEqual(expected, actual, "The hash codes for equvalent values should be the same");
            TestContext.WriteLine("Hash code {0} is equal to hash code {1}", target, expected);
        }

        #endregion

        //
        // casting 
        //

        #region from double test 

        /// <summary>
        ///A test for op_Implicit
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void op_ImplicitFromDouble_Test()
        {
            double value = 100; 
            PDFUnit expected = new PDFUnit(value); 
            PDFUnit actual;
            actual = value;
            Assert.AreEqual(expected, actual, "Impicit cast from double and constructed value did not match");
        }
        #endregion

        #region from int test

        /// <summary>
        ///A test for op_Implicit
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void op_ImplicitFromInt_Test()
        {
            int value = 100;
            PDFUnit expected = new PDFUnit(100);
            PDFUnit actual;
            actual = value;
            Assert.AreEqual(expected, actual, "Impicit cast from int and constructed value did not match");
        }

        #endregion

        //
        // static properties
        //

        #region PDFUnit.Empty test

        /// <summary>
        ///A test for Empty
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Empty_Test()
        {
            PDFUnit actual;
            PDFUnit expected = new PDFUnit();

            actual = PDFUnit.Empty;
            Assert.AreEqual(expected, actual, "Returned Empty was not equal to an empty PDFUnit");
            Assert.AreEqual(expected.PointsValue, actual.PointsValue);
            Assert.AreEqual(expected.Units, actual.Units);

        }

        #endregion

        #region PDFUnit.Zero test


        /// <summary>
        ///A test for Zero
        ///</summary>
        [TestMethod()]
        [TestCategory("Drawing Structures")]
        public void Zero_Test()
        {
            PDFUnit actual;
            actual = PDFUnit.Zero;
            PDFUnit expected = new PDFUnit(0, PageUnits.Points);
            Assert.AreEqual(expected, actual, "Returned Zero was not 0pts");
            Assert.AreEqual(expected.PointsValue, actual.PointsValue);
            Assert.AreEqual(expected.Units, actual.Units);

        }

        #endregion
    }
}
