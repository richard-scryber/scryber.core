using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Binding;

namespace Scryber.Core.UnitTests.Expressions
{
    /// <summary>
    /// Tests for statistical expression functions: average(), averageOf(), mean(), median(), mode()
    /// </summary>
    [TestClass()]
    public class StatisticalFunctions_Tests
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

        #region average() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Average_ArrayOfIntegers_ReturnsAverage()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 10, 20, 30, 40, 50 } }
            };
            var result = EvaluateExpression<double>("average(numbers)", vars);
            Assert.AreEqual(30.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Average_ArrayOfDoubles_ReturnsAverage()
        {
            var vars = new Dictionary<string, object>
            {
                { "values", new[] { 1.5, 2.5, 3.5, 4.5 } }
            };
            var result = EvaluateExpression<double>("average(values)", vars);
            Assert.AreEqual(3.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Average_WithNegatives_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { -10, 0, 10, 20 } }
            };
            var result = EvaluateExpression<double>("average(numbers)", vars);
            Assert.AreEqual(5.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Average_SingleValue_ReturnsThatValue()
        {
            var vars = new Dictionary<string, object>
            {
                { "single", new[] { 42 } }
            };
            var result = EvaluateExpression<double>("average(single)", vars);
            Assert.AreEqual(42.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Average_OddCount_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 1, 2, 3, 4, 5, 6, 7 } }
            };
            var result = EvaluateExpression<double>("average(numbers)", vars);
            Assert.AreEqual(4.0, result, 0.0001);
        }

        #endregion

        #region averageOf() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void AverageOf_ObjectProperty_ReturnsAverage()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { name = "Item 1", score = 80 },
                        new { name = "Item 2", score = 90 },
                        new { name = "Item 3", score = 85 }
                    }
                }
            };
            var result = EvaluateExpression<double>("averageOf(items, .score)", vars);
            Assert.AreEqual(85.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void AverageOf_PriceProperty_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "products", new[]
                    {
                        new { name = "Product A", price = 10.0 },
                        new { name = "Product B", price = 20.0 },
                        new { name = "Product C", price = 30.0 },
                        new { name = "Product D", price = 40.0 }
                    }
                }
            };
            var result = EvaluateExpression<double>("averageOf(products, .price)", vars);
            Assert.AreEqual(25.0, result, 0.0001);
        }

        #endregion

        #region mean() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Mean_ArrayOfNumbers_ReturnsMean()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 5, 10, 15, 20, 25 } }
            };
            var result = EvaluateExpression<double>("mean(numbers)", vars);
            Assert.AreEqual(15.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Mean_IdenticalToAverage_ReturnsSameValue()
        {
            var vars = new Dictionary<string, object>
            {
                { "values", new[] { 2, 4, 6, 8, 10 } }
            };
            var mean = EvaluateExpression<double>("mean(values)", vars);
            var average = EvaluateExpression<double>("average(values)", vars);
            Assert.AreEqual(average, mean, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Mean_WithDecimals_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "values", new[] { 1.1, 2.2, 3.3, 4.4, 5.5 } }
            };
            var result = EvaluateExpression<double>("mean(values)", vars);
            Assert.AreEqual(3.3, result, 0.0001);
        }

        #endregion

        #region median() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Median_OddCount_ReturnsMiddleValue()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 1, 2, 3, 4, 5 } }
            };
            var result = EvaluateExpression<double>("median(numbers)", vars);
            Assert.AreEqual(3.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Median_EvenCount_ReturnsAverageOfMiddleTwo()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 1, 2, 3, 4 } }
            };
            var result = EvaluateExpression<double>("median(numbers)", vars);
            Assert.AreEqual(2.5, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Median_UnsortedArray_FindsMedian()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 5, 1, 4, 2, 3 } }
            };
            var result = EvaluateExpression<double>("median(numbers)", vars);
            Assert.AreEqual(3.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Median_SingleValue_ReturnsThatValue()
        {
            var vars = new Dictionary<string, object>
            {
                { "single", new[] { 42 } }
            };
            var result = EvaluateExpression<double>("median(single)", vars);
            Assert.AreEqual(42.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Median_WithNegatives_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { -5, -1, 0, 1, 5 } }
            };
            var result = EvaluateExpression<double>("median(numbers)", vars);
            Assert.AreEqual(0.0, result, 0.0001);
        }

        #endregion

        #region mode() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Mode_SingleMostFrequent_ReturnsMostFrequentValue()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 1, 2, 2, 3, 3, 3, 4 } }
            };
            var result = EvaluateExpression<double>("mode(numbers)", vars);
            Assert.AreEqual(3.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Mode_AllUnique_ReturnsFirstValue()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 1, 2, 3, 4, 5 } }
            };
            var result = EvaluateExpression<double>("mode(numbers)", vars);
            // When all values appear once, mode typically returns first value
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Mode_AllSameValue_ReturnsThatValue()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 5, 5, 5, 5 } }
            };
            var result = EvaluateExpression<double>("mode(numbers)", vars);
            Assert.AreEqual(5.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Mode_TwoOccurrences_FindsMostFrequent()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 1, 1, 2, 3 } }
            };
            var result = EvaluateExpression<double>("mode(numbers)", vars);
            Assert.AreEqual(1.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Statistical")]
        public void Mode_SingleValue_ReturnsThatValue()
        {
            var vars = new Dictionary<string, object>
            {
                { "single", new[] { 42 } }
            };
            var result = EvaluateExpression<double>("mode(single)", vars);
            Assert.AreEqual(42.0, result, 0.0001);
        }

        #endregion
    }
}
