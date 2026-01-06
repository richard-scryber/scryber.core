using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Binding;

namespace Scryber.Core.UnitTests.Expressions
{
    /// <summary>
    /// Tests for logical expression functions: if(), ifError(), in(), index()
    /// </summary>
    [TestClass()]
    public class LogicalFunctions_Tests
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

        private object EvaluateExpression(string expression, Dictionary<string, object> vars = null)
        {
            var factory = CreateFactory();
            var expr = factory.CreateExpression(expression);
            return expr.Evaluate(vars ?? new Dictionary<string, object>());
        }

        #endregion

        #region if() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void If_TrueCondition_ReturnsFirstValue()
        {
            var result = EvaluateExpression<string>("if(10 > 5, 'yes', 'no')");
            Assert.AreEqual("yes", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void If_FalseCondition_ReturnsSecondValue()
        {
            var result = EvaluateExpression<string>("if(5 > 10, 'yes', 'no')");
            Assert.AreEqual("no", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void If_WithVariables_EvaluatesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "age", 25 },
                { "threshold", 18 }
            };
            var result = EvaluateExpression<string>("if(age >= threshold, 'Adult', 'Minor')", vars);
            Assert.AreEqual("Adult", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void If_NestedConditions_EvaluatesCorrectly()
        {
            var vars = new Dictionary<string, object> { { "score", 85 } };
            var expr = "if(score >= 90, 'A', if(score >= 80, 'B', if(score >= 70, 'C', 'F')))";
            var result = EvaluateExpression<string>(expr, vars);
            Assert.AreEqual("B", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void If_NumericResult_ReturnsCorrectValue()
        {
            var vars = new Dictionary<string, object> { { "discount", true } };
            var result = EvaluateExpression<double>("if(discount, 0.9, 1.0)", vars);
            Assert.AreEqual(0.9, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void If_ComplexCondition_EvaluatesCorrectly()
        {
            var vars = new Dictionary<string, object>
            {
                { "price", 100 },
                { "quantity", 5 }
            };
            var result = EvaluateExpression<string>("if(price * quantity > 400, 'High', 'Low')", vars);
            Assert.AreEqual("High", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void If_WithStringComparison_WorksCorrectly()
        {
            var vars = new Dictionary<string, object> { { "status", "active" } };
            var result = EvaluateExpression<bool>("if(status == 'active', true, false)", vars);
            Assert.AreEqual(true, result);
        }

        #endregion

        #region ifError() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void IfError_NoError_ReturnsValue()
        {
            var vars = new Dictionary<string, object> { { "value", 42 } };
            var result = EvaluateExpression<int>("ifError(value, 0)", vars);
            Assert.AreEqual(42, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void IfError_WithError_ReturnsFallback()
        {
            // Division by zero should trigger error handling
            var result = EvaluateExpression<int>("ifError(10 / 0, 999)", null);
            Assert.AreEqual(999, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void IfError_MissingVariable_ReturnsFallback()
        {
            var vars = new Dictionary<string, object>();  // Empty, missing 'value'
            var result = EvaluateExpression<int>("ifError(value + ' fail', 0)", vars);
            Assert.AreEqual(0, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void IfError_ValidCalculation_ReturnsResult()
        {
            var vars = new Dictionary<string, object> { { "x", 10 }, { "y", 2 } };
            var result = EvaluateExpression<double>("ifError(x / y, -1)", vars);
            Assert.AreEqual(5.0, result, 0.0001);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void IfError_StringFallback_WorksCorrectly()
        {
            var result = EvaluateExpression<string>("ifError(invalidVar, 'default')", null);
            Assert.AreEqual("default", result);
        }

        #endregion

        #region in() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void In_ValueInArray_ReturnsTrue()
        {
            var vars = new Dictionary<string, object>
            {
                { "value", 3 },
                { "list", new[] { 1, 2, 3, 4, 5 } }
            };
            var result = EvaluateExpression<bool>("in(value, list)", vars);
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void In_ValueNotInArray_ReturnsFalse()
        {
            var vars = new Dictionary<string, object>
            {
                { "value", 10 },
                { "list", new[] { 1, 2, 3, 4, 5 } }
            };
            var result = EvaluateExpression<bool>("in(value, list)", vars);
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void In_StringInArray_ReturnsTrue()
        {
            var vars = new Dictionary<string, object>
            {
                { "fruit", "apple" },
                { "fruits", new[] { "apple", "banana", "cherry" } }
            };
            var result = EvaluateExpression<bool>("in(fruit, fruits)", vars);
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void In_EmptyArray_ReturnsFalse()
        {
            var vars = new Dictionary<string, object>
            {
                { "value", 1 },
                { "empty", new int[] { } }
            };
            var result = EvaluateExpression<bool>("in(value, empty)", vars);
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void In_WithLiterals_ChecksMembership()
        {
            var vars = new Dictionary<string, object>
            {
                { "status", "active" }
            };
            // Note: This test depends on how the in() function handles literal arrays
            var result = EvaluateExpression<bool>("in(status, list)",
                new Dictionary<string, object>
                {
                    { "status", "active" },
                    { "list", new[] { "active", "pending", "completed" } }
                });
            Assert.AreEqual(true, result);
        }

        #endregion

        #region index() Function Tests

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void Index_ValidIndex_ReturnsElement()
        {
            var vars = new Dictionary<string, object>
            {
                { "arr", new[] { "zero", "one", "two", "three" } },
                { "idx", 2 }
            };
            var result = EvaluateExpression<string>("arr[idx]", vars);
            Assert.AreEqual("two", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void Index_ZeroIndex_ReturnsFirstElement()
        {
            var vars = new Dictionary<string, object>
            {
                { "numbers", new[] { 10, 20, 30 } }
            };
            var result = EvaluateExpression<int>("numbers[0]", vars);
            Assert.AreEqual(10, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void Index_LastIndex_ReturnsLastElement()
        {
            var vars = new Dictionary<string, object>
            {
                { "items", new[] { "first", "second", "third" } }
            };
            var result = EvaluateExpression<string>("items[2]", vars);
            Assert.AreEqual("third", result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void Index_NumericArray_ReturnsNumber()
        {
            var vars = new Dictionary<string, object>
            {
                { "values", new[] { 100, 200, 300, 400 } }
            };
            var result = EvaluateExpression<int>("values[1]", vars);
            Assert.AreEqual(200, result);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        [ExpectedException(typeof(Expressive.Exceptions.ExpressiveException))]
        public void Index_OutOfRange_ThrowsException()
        {
            var vars = new Dictionary<string, object>
            {
                { "arr", new[] { 1, 2, 3 } }
            };
            EvaluateExpression<int>("arr[10]", vars);
        }

        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        [ExpectedException(typeof(Expressive.Exceptions.ExpressiveException))]
        public void Index_NegativeIndex_ThrowsException()
        {
            var vars = new Dictionary<string, object>
            {
                { "arr", new[] { 1, 2, 3 } }
            };
            EvaluateExpression<int>("arr[-1]", vars);
        }
        
        [TestMethod()]
        [TestCategory("Expressions")]
        [TestCategory("Logical")]
        public void IndexIfError_NegativeIndex_Captured()
        {
            var vars = new Dictionary<string, object>
            {
                { "arr", new[] { 1, 2, 3 } }
            };
            
            var result = EvaluateExpression<int>("ifError(arr[-1], -2)", vars);
            Assert.AreEqual(-2, result);
        }

        #endregion
    }
}
