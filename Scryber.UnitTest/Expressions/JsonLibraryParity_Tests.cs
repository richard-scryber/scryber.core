using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Binding;
using Scryber.Components;
using Scryber.Html.Components;

namespace Scryber.Core.UnitTests.Expressions
{
    /// <summary>
    /// The same JSON data, read with System.Text.Json and with Newtonsoft, binds to the same values in expressions.
    /// </summary>
    [TestClass()]
    public class JsonLibraryParity_Tests
    {
        private const string Json = @"{
    ""name"": ""Widget"",
    ""count"": 3,
    ""price"": 2.5,
    ""active"": true,
    ""none"": null,
    ""nested"": { ""p1"": ""one"", ""p2"": ""two"" },
    ""tags"": [ ""red"", ""green"", ""blue"" ],
    ""values"": [ 10, 20, 30 ],
    ""items"": [
        { ""name"": ""Apple"", ""qty"": 1 },
        { ""name"": ""Pear"", ""qty"": 4 }
    ]
}";

        private static object SystemTextJsonModel()
        {
            return System.Text.Json.JsonSerializer.Deserialize<object>(Json);
        }

        private static object NewtonsoftModel()
        {
            return Newtonsoft.Json.Linq.JObject.Parse(Json);
        }

        private static object Evaluate(string expression, object model)
        {
            var factory = new BindingCalcExpressionFactory();
            var expr = factory.CreateExpression(expression);
            return expr.Evaluate(new Dictionary<string, object>() { { "model", model } });
        }

        /// <summary>
        /// Evaluates the expression against both models, and checks both results are the expected value (numbers are compared as doubles)
        /// </summary>
        private static void AssertBoth(object expected, string expression)
        {
            var stj = Evaluate(expression, SystemTextJsonModel());
            var nsj = Evaluate(expression, NewtonsoftModel());

            Assert.AreEqual(Normal(expected), Normal(stj), "System.Text.Json result for " + expression);
            Assert.AreEqual(Normal(expected), Normal(nsj), "Newtonsoft result for " + expression);
        }

        private static object Normal(object value)
        {
            switch (value)
            {
                case int i: return (double)i;
                case long l: return (double)l;
                case decimal d: return (double)d;
                case float f: return (double)f;
                default: return value;
            }
        }

        [TestMethod()]
        public void JsonParity_01_PropertyValuesAreNative()
        {
            AssertBoth("Widget", "model.name");
            AssertBoth(3.0, "model.count");
            AssertBoth(2.5, "model.price");
            AssertBoth(true, "model.active");
            AssertBoth(null, "model.none");
            AssertBoth(null, "model.missing");
            AssertBoth("one", "model.nested.p1");
            AssertBoth(null, "model.nested.missing");

            Assert.IsInstanceOfType(Evaluate("model.count", SystemTextJsonModel()), typeof(double));
            Assert.IsInstanceOfType(Evaluate("model.count", NewtonsoftModel()), typeof(double));
        }

        [TestMethod()]
        public void JsonParity_02_IndexesAreNative()
        {
            AssertBoth("green", "model.tags[1]");
            AssertBoth(20.0, "model.values[1]");
            AssertBoth(4.0, "model.items[1].qty");
            AssertBoth("Pear", "model.items[1]['name']");
            AssertBoth("two", "model.nested['p2']");
            AssertBoth(null, "model.nested['missing']");

            Assert.IsInstanceOfType(Evaluate("model.values[0]", SystemTextJsonModel()), typeof(double));
            Assert.IsInstanceOfType(Evaluate("model.values[0]", NewtonsoftModel()), typeof(double));
        }

        [TestMethod()]
        public void JsonParity_03_OperatorsAndComparisons()
        {
            AssertBoth(7.5, "model.price * model.count");
            AssertBoth(21.0, "model.values[1] + 1");
            AssertBoth(true, "model.count == 3");
            AssertBoth(true, "model.count > 2 && model.active");
            AssertBoth(true, "model.tags[0] == 'red'");
            AssertBoth(false, "model.items[0].qty != 1");
            AssertBoth("none", "model.none ?? 'none'");
            AssertBoth("on", "model.active ? 'on' : 'off'");
        }

        [TestMethod()]
        public void JsonParity_04_FunctionsOverArraysAndObjects()
        {
            AssertBoth(3.0, "count(model.tags)");
            AssertBoth(60.0, "sum(model.values)");
            AssertBoth(30.0, "max(model.values)");
            AssertBoth(20.0, "average(model.values)");
            AssertBoth(5.0, "sumOf(model.items, .qty)");
            AssertBoth("Pear", "firstWhere(model.items, .qty > 1).name");
            AssertBoth("red, green, blue", "join(', ', model.tags)");
            AssertBoth("p1, p2", "join(', ', keys(model.nested))");
            AssertBoth(0.0, "count(keys(model.tags))");
            AssertBoth("Widget x3", "concat(model.name, ' x', model.count)");
        }

        [TestMethod()]
        public void JsonParity_05_BoundDocumentsMatch()
        {
            var content = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<body>
    <h1 id='title'>{{model.name}} ({{count(model.items)}})</h1>
    {{#each model.items}}
    <p id=""{{concat('item', @index)}}"">{{this.name}} - {{this.qty * model.price}}</p>
    {{/each}}
    {{#each model.nested}}
    <p id=""{{concat('key', @index)}}"">{{@key}}={{this}}</p>
    {{/each}}
</body>
</html>";

            var stj = BindDocument(content, SystemTextJsonModel(), "JsonParity_SystemTextJson.pdf");
            var nsj = BindDocument(content, NewtonsoftModel(), "JsonParity_Newtonsoft.pdf");

            var expected = new[] { "Widget (2)", "Apple - 2.5", "Pear - 10", "p1=one", "p2=two" };
            CollectionAssert.AreEqual(expected, stj, "System.Text.Json: " + string.Join(" | ", stj));
            CollectionAssert.AreEqual(expected, nsj, "Newtonsoft: " + string.Join(" | ", nsj));
        }

        private static string[] BindDocument(string content, object model, string output)
        {
            using var reader = new System.IO.StringReader(content);
            using var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
            doc.Params["model"] = model;

            using (var stream = DocStreams.GetOutputStream(output))
            {
                doc.SaveAsPDF(stream);
            }

            var ids = new[] { "title", "item0", "item1", "key0", "key1" };
            var all = new List<string>();
            foreach (var id in ids)
            {
                var panel = doc.FindAComponentById(id) as Panel;
                Assert.IsNotNull(panel, "No component with id " + id);
                var sb = new System.Text.StringBuilder();
                foreach (var item in panel.Contents)
                {
                    if (item is TextLiteral lit)
                        sb.Append(lit.Text);
                }
                all.Add(sb.ToString().Trim());
            }
            return all.ToArray();
        }
    }
}
