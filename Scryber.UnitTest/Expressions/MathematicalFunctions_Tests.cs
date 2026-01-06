using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Binding;
using Scryber.Expressive.Exceptions;

namespace Scryber.Core.UnitTests.Expressions
{
    /// <summary>
    /// Tests for mathematical expression functions
    /// Phase 1: abs(), round(), ceiling(), floor(), sqrt(), pow(), sign(), truncate(), pi(), e()
    /// Phase 3: sin(), cos(), tan(), asin(), acos(), atan(), log(), log10(), exp(), radians(), degrees(), random(), IEEERemainder()
    /// </summary>
    [TestClass()]
    public class MathematicalFunctions_Tests
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Helper Methods

        private BindingCalcExpressionFactory CreateFactory()
        {
            return new BindingCalcExpressionFactory();
        }

        private T EvaluateExpression<T>(string expression, Dictionary<string, object> vars = null)
        {
            var factory = CreateFactory();
            var expr = factory.CreateExpression(expression);
            return expr.Evaluate<T>(vars ?? new Dictionary<string, object>());
        }

        #endregion

        #region abs() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Abs_PositiveNumber_ReturnsPositive()
        {
            var result = EvaluateExpression<double>("abs(10)");
            Assert.AreEqual(10.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Abs_NegativeNumber_ReturnsPositive()
        {
            var result = EvaluateExpression<double>("abs(-15.5)");
            Assert.AreEqual(15.5, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Abs_Zero_ReturnsZero()
        {
            var result = EvaluateExpression<double>("abs(0)");
            Assert.AreEqual(0.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Abs_WithVariable_EvaluatesCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", -42.7 } };
            var result = EvaluateExpression<double>("abs(value)", vars);
            Assert.AreEqual(42.7, result, 0.0001);
        }

        #endregion

        #region round() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Round_PositiveDecimal_RoundsToNearest()
        {
            var result = EvaluateExpression<double>("round(3.7)");
            Assert.AreEqual(4.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Round_NegativeDecimal_RoundsToNearest()
        {
            var result = EvaluateExpression<double>("round(-2.3)");
            Assert.AreEqual(-2.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Round_Midpoint_RoundsToEven()
        {
            var result = EvaluateExpression<double>("round(2.5)");
            // .NET rounds to even (banker's rounding)
            Assert.AreEqual(2.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Round_WithVariable_RoundsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", 7.8 } };
            var result = EvaluateExpression<double>("round(value)", vars);
            Assert.AreEqual(8.0, result, 0.0001);
        }

        #endregion

        #region ceiling() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Ceiling_PositiveDecimal_RoundsUp()
        {
            var result = EvaluateExpression<double>("ceiling(3.1)");
            Assert.AreEqual(4.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Ceiling_NegativeDecimal_RoundsUp()
        {
            var result = EvaluateExpression<double>("ceiling(-2.7)");
            Assert.AreEqual(-2.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Ceiling_WholeNumber_RemainsUnchanged()
        {
            var result = EvaluateExpression<double>("ceiling(5.0)");
            Assert.AreEqual(5.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Ceiling_WithVariable_RoundsUpCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", 2.01 } };
            var result = EvaluateExpression<double>("ceiling(value)", vars);
            Assert.AreEqual(3.0, result, 0.0001);
        }

        #endregion

        #region floor() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Floor_PositiveDecimal_RoundsDown()
        {
            var result = EvaluateExpression<double>("floor(3.9)");
            Assert.AreEqual(3.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Floor_NegativeDecimal_RoundsDown()
        {
            var result = EvaluateExpression<double>("floor(-2.1)");
            Assert.AreEqual(-3.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Floor_WholeNumber_RemainsUnchanged()
        {
            var result = EvaluateExpression<double>("floor(5.0)");
            Assert.AreEqual(5.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Floor_WithVariable_RoundsDownCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", 7.99 } };
            var result = EvaluateExpression<double>("floor(value)", vars);
            Assert.AreEqual(7.0, result, 0.0001);
        }

        #endregion

        #region sqrt() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Sqrt_PerfectSquare_ReturnsSquareRoot()
        {
            var result = EvaluateExpression<double>("sqrt(16)");
            Assert.AreEqual(4.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Sqrt_NonPerfectSquare_ReturnsCorrectRoot()
        {
            var result = EvaluateExpression<double>("sqrt(2)");
            Assert.AreEqual(1.4142, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Sqrt_Zero_ReturnsZero()
        {
            var result = EvaluateExpression<double>("sqrt(0)");
            Assert.AreEqual(0.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Sqrt_WithVariable_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", 25 } };
            var result = EvaluateExpression<double>("sqrt(value)", vars);
            Assert.AreEqual(5.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Sqrt_NegativeNumber_IsNaN()
        {
            var result = EvaluateExpression<double>("sqrt(-1)");
            Assert.IsTrue(double.IsNaN(result));
        }

        #endregion

        #region pow() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Pow_PositiveExponent_CalculatesCorrectly()
        {
            var result = EvaluateExpression<double>("pow(2, 3)");
            Assert.AreEqual(8.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Pow_ZeroExponent_ReturnsOne()
        {
            var result = EvaluateExpression<double>("pow(5, 0)");
            Assert.AreEqual(1.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Pow_NegativeExponent_CalculatesReciprocal()
        {
            var result = EvaluateExpression<double>("pow(2, -2)");
            Assert.AreEqual(0.25, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Pow_FractionalExponent_CalculatesRoot()
        {
            var result = EvaluateExpression<double>("pow(16, 0.5)");
            Assert.AreEqual(4.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Pow_WithVariables_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "base", 3 },
                { "exponent", 4 }
            };
            var result = EvaluateExpression<double>("pow(base, exponent)", vars);
            Assert.AreEqual(81.0, result, 0.0001);
        }

        #endregion

        #region sign() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Sign_PositiveNumber_ReturnsOne()
        {
            var result = EvaluateExpression<int>("sign(42.5)");
            Assert.AreEqual(1, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Sign_NegativeNumber_ReturnsMinusOne()
        {
            var result = EvaluateExpression<int>("sign(-17.3)");
            Assert.AreEqual(-1, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Sign_Zero_ReturnsZero()
        {
            var result = EvaluateExpression<int>("sign(0)");
            Assert.AreEqual(0, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Sign_WithVariable_ReturnsCorrectSign()
        {
            var vars = new Dictionary<string, object> { { "value", -99 } };
            var result = EvaluateExpression<int>("sign(value)", vars);
            Assert.AreEqual(-1, result);
        }

        #endregion

        #region truncate() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Truncate_PositiveDecimal_RemovesDecimalPart()
        {
            var result = EvaluateExpression<double>("truncate(3.9)");
            Assert.AreEqual(3.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Truncate_NegativeDecimal_RemovesDecimalPart()
        {
            var result = EvaluateExpression<double>("truncate(-2.7)");
            Assert.AreEqual(-2.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Truncate_WholeNumber_RemainsUnchanged()
        {
            var result = EvaluateExpression<double>("truncate(5.0)");
            Assert.AreEqual(5.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Truncate_WithVariable_TruncatesCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", 7.99 } };
            var result = EvaluateExpression<double>("truncate(value)", vars);
            Assert.AreEqual(7.0, result, 0.0001);
        }

        #endregion

        #region pi() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Pi_NoArguments_ReturnsPiConstant()
        {
            var result = EvaluateExpression<double>("pi()");
            Assert.AreEqual(Math.PI, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Pi_InCalculation_UsesCorrectValue()
        {
            var result = EvaluateExpression<double>("pi() * 2");
            Assert.AreEqual(Math.PI * 2, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Pi_CircleArea_CalculatesCorrectly()
        {
            // Area = π * r²
            var vars = new Dictionary<string, object> { { "radius", 5 } };
            var result = EvaluateExpression<double>("pi() * pow(radius, 2)", vars);
            Assert.AreEqual(78.5398, result, 0.0001);
        }

        #endregion

        #region e() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void E_NoArguments_ReturnsEulerConstant()
        {
            var result = EvaluateExpression<double>("e()");
            Assert.AreEqual(Math.E, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void E_InCalculation_UsesCorrectValue()
        {
            var result = EvaluateExpression<double>("e() * 2");
            Assert.AreEqual(Math.E * 2, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void E_CompoundInterest_CalculatesCorrectly()
        {
            // Continuous compounding formula uses e
            var result = EvaluateExpression<double>("pow(e(), 1)");
            Assert.AreEqual(Math.E, result, 0.0001);
        }

        #endregion

        #region sin() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Sin_Zero_ReturnsZero()
        {
            var result = EvaluateExpression<double>("sin(0)");
            Assert.AreEqual(0.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Sin_PiOverTwo_ReturnsOne()
        {
            var result = EvaluateExpression<double>("sin(pi() / 2)");
            Assert.AreEqual(1.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Sin_Pi_ReturnsZero()
        {
            var result = EvaluateExpression<double>("sin(pi())");
            Assert.AreEqual(0.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Sin_WithVariable_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object> { { "angle", Math.PI / 6 } };
            var result = EvaluateExpression<double>("sin(angle)", vars);
            Assert.AreEqual(0.5, result, 0.0001);
        }

        #endregion

        #region cos() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Cos_Zero_ReturnsOne()
        {
            var result = EvaluateExpression<double>("cos(0)");
            Assert.AreEqual(1.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Cos_PiOverTwo_ReturnsZero()
        {
            var result = EvaluateExpression<double>("cos(pi() / 2)");
            Assert.AreEqual(0.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Cos_Pi_ReturnsMinusOne()
        {
            var result = EvaluateExpression<double>("cos(pi())");
            Assert.AreEqual(-1.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Cos_WithVariable_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object> { { "angle", Math.PI / 3 } };
            var result = EvaluateExpression<double>("cos(angle)", vars);
            Assert.AreEqual(0.5, result, 0.0001);
        }

        #endregion

        #region tan() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Tan_Zero_ReturnsZero()
        {
            var result = EvaluateExpression<double>("tan(0)");
            Assert.AreEqual(0.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Tan_PiOverFour_ReturnsOne()
        {
            var result = EvaluateExpression<double>("tan(pi() / 4)");
            Assert.AreEqual(1.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Tan_WithVariable_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object> { { "angle", Math.PI / 6 } };
            var result = EvaluateExpression<double>("tan(angle)", vars);
            Assert.AreEqual(0.5774, result, 0.0001);
        }

        #endregion

        #region asin() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Asin_Zero_ReturnsZero()
        {
            var result = EvaluateExpression<double>("asin(0)");
            Assert.AreEqual(0.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Asin_One_ReturnsPiOverTwo()
        {
            var result = EvaluateExpression<double>("asin(1)");
            Assert.AreEqual(Math.PI / 2, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Asin_Half_ReturnsPiOverSix()
        {
            var result = EvaluateExpression<double>("asin(0.5)");
            Assert.AreEqual(Math.PI / 6, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Asin_WithVariable_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", 0.707 } };
            var result = EvaluateExpression<double>("asin(value)", vars);
            Assert.AreEqual(0.7854, result, 0.001);
        }

        #endregion

        #region acos() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Acos_One_ReturnsZero()
        {
            var result = EvaluateExpression<double>("acos(1)");
            Assert.AreEqual(0.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Acos_Zero_ReturnsPiOverTwo()
        {
            var result = EvaluateExpression<double>("acos(0)");
            Assert.AreEqual(Math.PI / 2, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Acos_MinusOne_ReturnsPi()
        {
            var result = EvaluateExpression<double>("acos(-1)");
            Assert.AreEqual(Math.PI, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Acos_WithVariable_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", 0.5 } };
            var result = EvaluateExpression<double>("acos(value)", vars);
            Assert.AreEqual(Math.PI / 3, result, 0.0001);
        }

        #endregion

        #region atan() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Atan_Zero_ReturnsZero()
        {
            var result = EvaluateExpression<double>("atan(0)");
            Assert.AreEqual(0.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Atan_One_ReturnsPiOverFour()
        {
            var result = EvaluateExpression<double>("atan(1)");
            Assert.AreEqual(Math.PI / 4, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Atan_WithVariable_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", Math.Sqrt(3) } };
            var result = EvaluateExpression<double>("atan(value)", vars);
            Assert.AreEqual(Math.PI / 3, result, 0.0001);
        }

        #endregion

        #region log() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Log_E_ReturnsOne()
        {
            var result = EvaluateExpression<double>("log(e(), e())");
            Assert.AreEqual(1.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Log_One_ReturnsZero()
        {
            var result = EvaluateExpression<double>("log(1, 10)");
            Assert.AreEqual(0.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Log_PositiveNumber_CalculatesCorrectly()
        {
            var result = EvaluateExpression<double>("log(10, e())");
            Assert.AreEqual(2.3026, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Log_WithVariable_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", 100 } };
            var result = EvaluateExpression<double>("log(value, e())", vars);
            Assert.AreEqual(4.6052, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        [ExpectedException(typeof(ExpressiveException))]
        public void Log_NegativeNumber_ThrowsException()
        {
            EvaluateExpression<double>("log(-1)");
        }

        #endregion

        #region log10() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Log10_Ten_ReturnsOne()
        {
            var result = EvaluateExpression<double>("log10(10)");
            Assert.AreEqual(1.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Log10_Hundred_ReturnsTwo()
        {
            var result = EvaluateExpression<double>("log10(100)");
            Assert.AreEqual(2.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Log10_One_ReturnsZero()
        {
            var result = EvaluateExpression<double>("log10(1)");
            Assert.AreEqual(0.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Log10_WithVariable_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object> { { "value", 1000 } };
            var result = EvaluateExpression<double>("log10(value)", vars);
            Assert.AreEqual(3.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Log10_NegativeNumber_IsNaN()
        {
            var result = (double)EvaluateExpression<double>("log10(-1)");
            Assert.IsTrue(double.IsNaN(result));
        }

        #endregion

        #region exp() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Exp_Zero_ReturnsOne()
        {
            var result = EvaluateExpression<double>("exp(0)");
            Assert.AreEqual(1.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Exp_One_ReturnsE()
        {
            var result = EvaluateExpression<double>("exp(1)");
            Assert.AreEqual(Math.E, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Exp_Two_CalculatesCorrectly()
        {
            var result = EvaluateExpression<double>("exp(2)");
            Assert.AreEqual(7.3891, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Exp_WithVariable_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object> { { "power", 3 } };
            var result = EvaluateExpression<double>("exp(power)", vars);
            Assert.AreEqual(20.0855, result, 0.0001);
        }

        #endregion

        #region radians() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Radians_Zero_ReturnsZero()
        {
            var result = EvaluateExpression<double>("rad(0)");
            Assert.AreEqual(0.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Radians_180_ReturnsPi()
        {
            var result = EvaluateExpression<double>("rad(180)");
            Assert.AreEqual(Math.PI, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Radians_90_ReturnsPiOverTwo()
        {
            var result = EvaluateExpression<double>("rad(90)");
            Assert.AreEqual(Math.PI / 2, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Radians_WithVariable_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "degrees", 45 } };
            var result = EvaluateExpression<double>("rad(degrees)", vars);
            Assert.AreEqual(Math.PI / 4, result, 0.0001);
        }

        #endregion

        #region degrees() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Degrees_Zero_ReturnsZero()
        {
            var result = EvaluateExpression<double>("deg(0)");
            Assert.AreEqual(0.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Degrees_Pi_Returns180()
        {
            var result = EvaluateExpression<double>("deg(pi())");
            Assert.AreEqual(180.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Degrees_PiOverTwo_Returns90()
        {
            var result = EvaluateExpression<double>("deg(pi / 2)");
            Assert.AreEqual(90.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Degrees_WithVariable_ConvertsCorrectly()
        {
            var vars = new Dictionary<string, object> { { "radians", Math.PI / 4 } };
            var result = EvaluateExpression<double>("deg(radians)", vars);
            Assert.AreEqual(45.0, result, 0.0001);
        }

        #endregion

        #region random() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Random_NoArguments_ReturnsBetweenZeroAndOne()
        {
            var result = EvaluateExpression<double>("random()");
            Assert.IsTrue(result >= 0.0 && result < 1.0, $"Random returned {result}, expected 0 <= x < 1");
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Random_MultipleCalls_ReturnsDifferentValues()
        {
            // Call random() twice and ensure they're different (with very high probability)
            var result1 = EvaluateExpression<double>("random()");
            var result2 = EvaluateExpression<double>("random()");

            // While theoretically possible to be equal, probability is infinitesimally small
            Assert.AreNotEqual(result1, result2, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Random_InCalculation_ProducesValidResult()
        {
            // Generate random number scaled to range
            var result = EvaluateExpression<double>("random() * 100");
            Assert.IsTrue(result >= 0.0 && result < 100.0);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void Random_RangeArguments_ReturnsBetweenZeroAndOne()
        {
            var result = EvaluateExpression<double>("random(1, 10)");
            Assert.IsTrue(result >= 1.0 && result < 10.0, $"Random returned {result}, expected 0 <= x < 1");
        }

        #endregion

        #region IEEERemainder() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void IEEERemainder_BasicDivision_ReturnsRemainder()
        {
            var result = EvaluateExpression<double>("IEEERemainder(5, 3)");
            Assert.AreEqual(-1.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void IEEERemainder_NegativeDividend_CalculatesCorrectly()
        {
            var result = EvaluateExpression<double>("IEEERemainder(-5, 3)");
            Assert.AreEqual(1.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void IEEERemainder_WithVariables_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "dividend", 10 },
                { "divisor", 3 }
            };
            var result = EvaluateExpression<double>("IEEERemainder(dividend, divisor)", vars);
            Assert.AreEqual(1.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Mathematical")]
        public void IEEERemainder_DecimalValues_HandlesCorrectly()
        {
            var result = EvaluateExpression<double>("IEEERemainder(10.5, 3.2)");
            Assert.AreEqual(0.9, result, 0.0001);
        }

        #endregion
    }
}
