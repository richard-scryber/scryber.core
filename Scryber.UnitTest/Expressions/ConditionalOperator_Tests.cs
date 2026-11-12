using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Binding;
using Scryber.Components;
using Scryber.Html.Components;

namespace Scryber.Core.UnitTests.Expressions
{
    /// <summary>
    /// Tests for the conditional (ternary) operator: condition ? whenTrue : whenFalse,
    /// along with the === and !== forms of equality.
    /// </summary>
    [TestClass()]
    public class ConditionalOperator_Tests
    {
        private static object Evaluate(string expression, Dictionary<string, object> vars = null)
        {
            var factory = new BindingCalcExpressionFactory();
            var expr = factory.CreateExpression(expression);
            return expr.Evaluate(vars ?? new Dictionary<string, object>());
        }

        private static Exception EvaluateError(string expression)
        {
            try
            {
                Evaluate(expression);
            }
            catch (Exception ex)
            {
                return ex;
            }
            Assert.Fail("No error was raised for the expression " + expression);
            return null;
        }

        [TestMethod()]
        public void Conditional_01_TrueAndFalseConditions()
        {
            Assert.AreEqual("yes", Evaluate("true ? 'yes' : 'no'"));
            Assert.AreEqual("no", Evaluate("false ? 'yes' : 'no'"));
            Assert.AreEqual(1, Evaluate("true?1:2"), "Without spaces");
        }

        [TestMethod()]
        public void Conditional_02_LowestPrecedence()
        {
            //The condition and both values take all the operators around them
            Assert.AreEqual("a", Evaluate("1 + 1 == 2 ? 'a' : 'b'"));
            Assert.AreEqual(15, Convert.ToInt32(Evaluate("2 > 1 && 3 > 2 ? 10 + 5 : 0")));
            Assert.AreEqual(6, Convert.ToInt32(Evaluate("false || false ? 0 : 2 * 3")));

            //and the null coalesce is part of the condition
            var vars = new Dictionary<string, object>() { { "missing", null } };
            Assert.AreEqual("n", Evaluate("missing ?? false ? 'y' : 'n'", vars));
        }

        [TestMethod()]
        public void Conditional_03_ChainReadsFromTheRight()
        {
            var expr = "score >= 90 ? 'A' : score >= 70 ? 'B' : score >= 50 ? 'C' : 'D'";

            Assert.AreEqual("A", Evaluate(expr, new Dictionary<string, object>() { { "score", 95 } }));
            Assert.AreEqual("B", Evaluate(expr, new Dictionary<string, object>() { { "score", 70 } }));
            Assert.AreEqual("C", Evaluate(expr, new Dictionary<string, object>() { { "score", 55.5 } }));
            Assert.AreEqual("D", Evaluate(expr, new Dictionary<string, object>() { { "score", 10 } }));
        }

        [TestMethod()]
        public void Conditional_04_NestedInTheTrueValue()
        {
            var expr = "a ? b ? 'ab' : 'a only' : 'not a'";

            Assert.AreEqual("ab", Evaluate(expr, new Dictionary<string, object>() { { "a", true }, { "b", true } }));
            Assert.AreEqual("a only", Evaluate(expr, new Dictionary<string, object>() { { "a", true }, { "b", false } }));
            Assert.AreEqual("not a", Evaluate(expr, new Dictionary<string, object>() { { "a", false }, { "b", true } }));

            //and in brackets
            Assert.AreEqual("a only", Evaluate("a ? (b ? 'ab' : 'a only') : 'not a'", new Dictionary<string, object>() { { "a", true }, { "b", false } }));
        }

        [TestMethod()]
        public void Conditional_05_InBracketsFunctionsAndIndexes()
        {
            var vars = new Dictionary<string, object>()
            {
                { "count", 3 },
                { "values", new[] { 10, 20, 30 } }
            };

            Assert.AreEqual("3 items", Evaluate("concat(count, ' ', (count == 1 ? 'item' : 'items'))", vars));
            Assert.AreEqual("3 items", Evaluate("concat(count, ' ', count == 1 ? 'item' : 'items')", vars), "As a function parameter");
            Assert.AreEqual("first", Evaluate("concat(count > 1 ? 'first' : 'second', '')", vars), "As the first function parameter");
            Assert.AreEqual(2, Convert.ToInt32(Evaluate("if(true, count > 5 ? 1 : 2, 3)", vars)), "Inside if()");
            Assert.AreEqual(30, Convert.ToInt32(Evaluate("values[count > 2 ? 2 : 0]", vars)), "As an index");
            Assert.AreEqual(12, Convert.ToInt32(Evaluate("(count > 2 ? 10 : 0) + 2", vars)), "Bracketed then added to");
            Assert.AreEqual(30, Convert.ToInt32(Evaluate("max(count > 2 ? values : count)", vars)), "Function of the result");
        }

        [TestMethod()]
        public void Conditional_06_OnlyTheChosenValueIsEvaluated()
        {
            //int('abc') would raise an error if it was evaluated
            Assert.AreEqual(5, Evaluate("false ? int('abc') : 5"));
            Assert.AreEqual(5, Evaluate("true ? 5 : int('abc')"));
        }

        [TestMethod()]
        public void Conditional_07_ConditionTruth()
        {
            var vars = new Dictionary<string, object>()
            {
                { "nothing", null },
                { "zero", 0 },
                { "zerod", 0.0 },
                { "number", 12.5 },
                { "empty", "" },
                { "text", "some text" },
                { "falsetext", "false" },
                { "truetext", "True" },
                { "date", new DateTime(2024, 3, 15) },
                { "list", new[] { 1, 2 } }
            };

            Assert.AreEqual("f", Evaluate("nothing ? 't' : 'f'", vars), "null");
            Assert.AreEqual("f", Evaluate("zero ? 't' : 'f'", vars), "0");
            Assert.AreEqual("f", Evaluate("zerod ? 't' : 'f'", vars), "0.0");
            Assert.AreEqual("t", Evaluate("number ? 't' : 'f'", vars), "number");
            Assert.AreEqual("f", Evaluate("empty ? 't' : 'f'", vars), "empty string");
            Assert.AreEqual("t", Evaluate("text ? 't' : 'f'", vars), "text");
            Assert.AreEqual("f", Evaluate("falsetext ? 't' : 'f'", vars), "'false'");
            Assert.AreEqual("t", Evaluate("truetext ? 't' : 'f'", vars), "'True'");
            Assert.AreEqual("t", Evaluate("date ? 't' : 'f'", vars), "date");
            Assert.AreEqual("t", Evaluate("list ? 't' : 'f'", vars), "array");
        }

        [TestMethod()]
        public void Conditional_08_StrictEqualityForms()
        {
            Assert.AreEqual(true, Evaluate("1 === 1"));
            Assert.AreEqual(false, Evaluate("1 === 2"));
            Assert.AreEqual(true, Evaluate("'a' !== 'b'"));
            Assert.AreEqual(false, Evaluate("'a' !== 'a'"));
            Assert.AreEqual("same", Evaluate("'x'==='x' ? 'same' : 'different'"));

            //the existing forms are unchanged
            Assert.AreEqual(true, Evaluate("1 == 1"));
            Assert.AreEqual(true, Evaluate("1 != 2"));
            Assert.AreEqual(false, Evaluate("!true"));
        }

        [TestMethod()]
        public void Conditional_09_IncompleteConditionalsRaiseErrors()
        {
            Assert.IsNotNull(EvaluateError("true ? 1"), "No ':'");
            Assert.IsNotNull(EvaluateError("? 1 : 2"), "No condition");
            Assert.IsNotNull(EvaluateError("true ? : 2"), "No true value");
            Assert.IsNotNull(EvaluateError("true ? 1 :"), "No false value");
            Assert.IsNotNull(EvaluateError("1 : 2"), "A ':' without a '?'");
            Assert.IsNotNull(EvaluateError("concat(true ? 1 :, 'x')"), "No false value in a parameter");
        }

        [TestMethod()]
        public void Conditional_10_BoundInDocumentContentAndAttributes()
        {
            var content = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<body>
    <h1 id='title' class=""{{model.active ? 'on' : 'off'}}"">{{model.active ? 'Active' : 'Inactive'}}</h1>
    {{#each model.items}}
    <p id=""{{concat('item', @index)}}"">{{@first ? 'First: ' : ''}}{{this.name}} - {{this.qty == 1 ? 'one' : concat(this.qty, ' units')}}</p>
    {{/each}}
</body>
</html>";

            var json = @"{ ""active"": true, ""items"": [ { ""name"": ""Apple"", ""qty"": 1 }, { ""name"": ""Pear"", ""qty"": 4 } ] }";

            using var reader = new System.IO.StringReader(content);
            using var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
            doc.Params["model"] = System.Text.Json.JsonSerializer.Deserialize<object>(json);

            using (var output = DocStreams.GetOutputStream("ConditionalOperator_Bound.pdf"))
            {
                doc.SaveAsPDF(output);
            }

            var h1 = doc.FindAComponentById("title") as Head1;
            Assert.IsNotNull(h1);
            Assert.AreEqual("on", h1.StyleClass);
            Assert.AreEqual("Active", TextOf(h1));

            var p0 = doc.FindAComponentById("item0") as Paragraph;
            Assert.IsNotNull(p0);
            Assert.AreEqual("First: Apple - one", TextOf(p0));

            var p1 = doc.FindAComponentById("item1") as Paragraph;
            Assert.IsNotNull(p1);
            Assert.AreEqual("Pear - 4 units", TextOf(p1));
        }

        private static string TextOf(Panel panel)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var item in panel.Contents)
            {
                if (item is TextLiteral lit)
                    sb.Append(lit.Text);
            }
            return sb.ToString().Trim();
        }
    }
}
