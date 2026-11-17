using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Binding;

namespace Scryber.Core.UnitTests.Expressions
{
    /// <summary>
    /// The expression values that the documentation describes, and that raised errors or gave the name of a type:
    /// mode() of text, the truth of an empty string with &amp;&amp; || ! and if(), string() of a collection, and integer() / long() of text with a fraction.
    /// </summary>
    [TestClass()]
    public class ExpressionValueFixes_Tests
    {
        private static object Evaluate(string expression, Dictionary<string, object> vars = null)
        {
            var factory = new BindingCalcExpressionFactory();
            return factory.CreateExpression(expression).Evaluate(vars ?? new Dictionary<string, object>());
        }

        private static object Json(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<object>(json);
        }

        [TestMethod()]
        public void ValueFixes_01_ModeOfText()
        {
            var vars = new Dictionary<string, object>()
            {
                { "model", Json(@"{ ""choices"": [ ""A"", ""B"", ""A"", ""C"", ""A"", ""B"" ], ""numbers"": [ 1, 2, 2, 3 ] }") }
            };

            Assert.AreEqual("A", Evaluate("mode(model.choices)", vars), "The most frequent text");
            Assert.AreEqual(2m, Evaluate("mode(model.numbers)", vars), "Numbers are still the mode as a number");
            Assert.AreEqual(2m, Evaluate("mode(1, 2, 2, 3)"), "Parameters");
            Assert.AreEqual("red", Evaluate("mode('red', 'blue', 'red')"), "Text parameters");
            Assert.AreEqual(5m, Evaluate("mode('5', 5, 6)"), "Text that is a number is compared as a number");
        }

        [TestMethod()]
        public void ValueFixes_02_EmptyStringIsFalse()
        {
            var vars = new Dictionary<string, object>() { { "bg", "" }, { "name", "Paperwork" } };

            Assert.AreEqual(false, Evaluate("'' && true"));
            Assert.AreEqual(false, Evaluate("bg && ''", vars));
            Assert.AreEqual(true, Evaluate("'' || true"));
            Assert.AreEqual(true, Evaluate("name && true", vars), "Text is true");
            Assert.AreEqual(true, Evaluate("!bg", vars), "Not an empty string");
            Assert.AreEqual(false, Evaluate("!name", vars), "Not some text");
            Assert.AreEqual("none", Evaluate("if(bg, bg, 'none')", vars), "if() with an empty string");
            Assert.AreEqual("Paperwork", Evaluate("if(name, name, 'none')", vars), "if() with text");

            //the existing values are the same
            Assert.AreEqual(false, Evaluate("'false' && true"));
            Assert.AreEqual(true, Evaluate("1 && 'true'"));
            Assert.AreEqual(false, Evaluate("0 || false"));
        }

        [TestMethod()]
        public void ValueFixes_03_StringOfACollection()
        {
            var vars = new Dictionary<string, object>()
            {
                { "array", new object[] { 1, 2, 3 } },
                { "list", new List<string>() { "a", "b" } },
                { "model", Json(@"{ ""values"": [ 1, 2, 3 ], ""nested"": [ [ 1, 2 ], [ 3 ] ] }") }
            };

            Assert.AreEqual("[1, 2, 3]", Evaluate("string(array)", vars));
            Assert.AreEqual("[a, b]", Evaluate("string(list)", vars));
            Assert.AreEqual("[1, 2, 3]", Evaluate("string(model.values)", vars));
            Assert.AreEqual("[[1, 2], [3]]", Evaluate("string(model.nested)", vars));
            Assert.AreEqual("text", Evaluate("string('text')"), "Text is not a collection");
            Assert.AreEqual("12", Evaluate("string(12)"));
        }

        [TestMethod()]
        public void ValueFixes_04_IntegerOfTextWithAFraction()
        {
            Assert.AreEqual(11, Evaluate("integer('10.6')"));
            Assert.AreEqual(11, Evaluate("integer(10.6)"), "The same as a number");
            Assert.AreEqual(11, Evaluate("int('10.6')"));
            Assert.AreEqual(-3, Evaluate("integer('-2.7')"));
            Assert.AreEqual(10, Evaluate("integer('10')"), "Whole numbers as before");
            Assert.AreEqual(255, Evaluate("integer('0xFF')"), "Hex as before");
            Assert.AreEqual(11L, Evaluate("long('10.6')"));
            Assert.AreEqual(10L, Evaluate("long('10')"));

            Assert.ThrowsExactly<Scryber.Expressive.Exceptions.ExpressiveException>(() => Evaluate("integer('ten')"), "Text that is not a number");
        }
    }
}
