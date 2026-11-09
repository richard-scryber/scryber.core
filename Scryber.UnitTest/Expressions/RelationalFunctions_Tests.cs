using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Binding;

namespace Scryber.Core.UnitTests.Expressions
{
    /// <summary>
    /// Tests for relational/aggregation expression functions: count(), countOf(), sum(), sumOf(), max(), maxOf(), min(), minOf()
    /// </summary>
    [TestClass()]
    public class RelationalFunctions_Tests
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

        #region count() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Count_ArrayOfIntegers_ReturnsCount()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 1, 2, 3, 4, 5 } }
            };
            var result = EvaluateExpression<int>("count(numbers)", vars);
            Assert.AreEqual(5, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Count_EmptyArray_ReturnsZero()
        {
            var vars = new Dictionary<string, object>
            {
                { "empty", new int[] { } }
            };
            var result = EvaluateExpression<int>("count(empty)", vars);
            Assert.AreEqual(0, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Count_ArrayOfStrings_ReturnsCount()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[] { "apple", "banana", "cherry" } }
            };
            var result = EvaluateExpression<int>("count(items)", vars);
            Assert.AreEqual(3, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Count_WithObjects_ReturnsCount()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { name = "Item 1" },
                        new { name = "Item 2" },
                        new { name = "Item 3" }
                    }
                }
            };
            var result = EvaluateExpression<int>("count(items)", vars);
            Assert.AreEqual(3, result);
        }

        #endregion

        #region sum() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Sum_ArrayOfIntegers_ReturnsTotal()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 10, 20, 30, 40, 50 } }
            };
            var result = EvaluateExpression<double>("sum(numbers)", vars);
            Assert.AreEqual(150.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Sum_ArrayOfDoubles_ReturnsTotal()
        {
            var vars = new Dictionary<string, object>
            {
                { "values", new[] { 1.5, 2.5, 3.0, 4.0 } }
            };
            var result = EvaluateExpression<double>("sum(values)", vars);
            Assert.AreEqual(11.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Sum_EmptyArray_ReturnsZero()
        {
            var vars = new Dictionary<string, object>
            {
                { "empty", new int[] { } }
            };
            var result = EvaluateExpression<double>("sum(empty)", vars);
            Assert.AreEqual(0.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Sum_WithNegatives_CalculatesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 10, -5, 20, -10, 15 } }
            };
            var result = EvaluateExpression<double>("sum(numbers)", vars);
            Assert.AreEqual(30.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Sum_MultipleArrays_SumsAll()
        {
            var vars = new Dictionary<string, object>
            {
                { "arr1", new[] { 1, 2, 3 } },
                { "arr2", new[] { 4, 5, 6 } }
            };
            var result = EvaluateExpression<double>("sum(arr1) + sum(arr2)", vars);
            Assert.AreEqual(21.0, result, 0.0001);
        }

        #endregion

        #region sumOf() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void SumOf_ObjectProperty_SumsValues()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { name = "Item 1", price = 10.0 },
                        new { name = "Item 2", price = 20.0 },
                        new { name = "Item 3", price = 30.0 }
                    }
                }
            };
            var result = EvaluateExpression<double>("sumOf(items, .price)", vars);
            Assert.AreEqual(60.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void SumOf_EmptyCollection_ReturnsZero()
        {
            var vars = new Dictionary<string, object>
            {
                { "empty", new object[] { } }
            };
            var result = EvaluateExpression<double>("sumOf(empty, .value)", vars);
            Assert.AreEqual(0.0, result, 0.0001);
        }

        #endregion

        #region max() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Max_ArrayOfIntegers_ReturnsMaximum()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 5, 2, 9, 1, 7 } }
            };
            var result = EvaluateExpression<double>("max(numbers)", vars);
            Assert.AreEqual(9.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Max_ArrayOfDoubles_ReturnsMaximum()
        {
            var vars = new Dictionary<string, object>
            {
                { "values", new[] { 1.5, 7.8, 3.2, 9.9, 2.1 } }
            };
            var result = EvaluateExpression<double>("max(values)", vars);
            Assert.AreEqual(9.9, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Max_WithNegatives_ReturnsMaximum()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { -5, -2, -9, -1 } }
            };
            var result = EvaluateExpression<double>("max(numbers)", vars);
            Assert.AreEqual(-1.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Max_SingleValue_ReturnsThatValue()
        {
            var vars = new Dictionary<string, object>
            {
                { "single", new[] { 42 } }
            };
            var result = EvaluateExpression<double>("max(single)", vars);
            Assert.AreEqual(42.0, result, 0.0001);
        }

        #endregion

        #region maxOf() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void MaxOf_ObjectProperty_ReturnsMaxValue()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { name = "Item 1", score = 75 },
                        new { name = "Item 2", score = 92 },
                        new { name = "Item 3", score = 88 }
                    }
                }
            };
            var result = EvaluateExpression<double>("maxOf(items, .score)", vars);
            Assert.AreEqual(92.0, result, 0.0001);
        }

        #endregion

        #region min() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Min_ArrayOfIntegers_ReturnsMinimum()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 5, 2, 9, 1, 7 } }
            };
            var result = EvaluateExpression<double>("min(numbers)", vars);
            Assert.AreEqual(1.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Min_ArrayOfDoubles_ReturnsMinimum()
        {
            var vars = new Dictionary<string, object>
            {
                { "values", new[] { 1.5, 7.8, 3.2, 0.5, 2.1 } }
            };
            var result = EvaluateExpression<double>("min(values)", vars);
            Assert.AreEqual(0.5, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Min_WithNegatives_ReturnsMinimum()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { -5, -2, -9, -1 } }
            };
            var result = EvaluateExpression<double>("min(numbers)", vars);
            Assert.AreEqual(-9.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void Min_SingleValue_ReturnsThatValue()
        {
            var vars = new Dictionary<string, object>
            {
                { "single", new[] { 42 } }
            };
            var result = EvaluateExpression<double>("min(single)", vars);
            Assert.AreEqual(42.0, result, 0.0001);
        }

        #endregion

        #region minOf() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void MinOf_ObjectProperty_ReturnsMinValue()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { name = "Item 1", price = 15.99 },
                        new { name = "Item 2", price = 9.99 },
                        new { name = "Item 3", price = 12.99 }
                    }
                }
            };
            var result = EvaluateExpression<double>("minOf(items, .price)", vars);
            Assert.AreEqual(9.99, result, 0.0001);
        }

        #endregion

        #region countOf() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void CountOf_WithCondition_CountsMatchingItems()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { name = "Item 1", active = true },
                        new { name = "Item 2", active = false },
                        new { name = "Item 3", active = true },
                        new { name = "Item 4", active = true }
                    }
                }
            };
            var result = EvaluateExpression<int>("countOf(items, .active)", vars);
            Assert.AreEqual(3, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Relational")]
        public void CountOf_NoMatches_ReturnsZero()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[]
                    {
                        new { active = false },
                        new { active = false }
                    }
                }
            };
            var result = EvaluateExpression<int>("countOf(items, .active == true)", vars);
             Assert.AreEqual(0, result);
        }

        #endregion
    }
}
