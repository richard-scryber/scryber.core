using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Binding
{
    /// <summary>
    /// The collection functions that look up a property on each item accept a quoted name ('price') as well as .price
    /// </summary>
    [TestClass]
    public class QuotedLookup_Tests
    {
        private static string Render(string body, object model)
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><body>" + body + "</body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                doc.Params["model"] = model;
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                return Generation.PanelContent_Tests.TextOf(ms.ToArray());
            }
        }

        private static readonly object Model = new
        {
            items = new[]
            {
                new { name = "b", price = 2, active = true, owner = new { city = "Paris" } },
                new { name = "a", price = 3, active = false, owner = new { city = "Oslo" } },
                new { name = "c", price = 1, active = true, owner = new { city = "Rome" } }
            }
        };

        [TestMethod]
        public void SortBy_QuotedName_SameAsDotName()
        {
            Assert.AreEqual("abc ", Render("{{#each sortBy(model.items, .name)}}<p>{{this.name}}</p>{{/each}}", Model));
            Assert.AreEqual("abc ", Render("{{#each sortBy(model.items, 'name')}}<p>{{this.name}}</p>{{/each}}", Model));
        }

        [TestMethod]
        public void SortBy_QuotedName_Descending()
        {
            Assert.AreEqual("cba ", Render("{{#each sortBy(model.items, 'name', 'desc')}}<p>{{this.name}}</p>{{/each}}", Model));
        }

        [TestMethod]
        public void SortBy_QuotedNumber()
        {
            Assert.AreEqual("cba ", Render("{{#each sortBy(model.items, 'price')}}<p>{{this.name}}</p>{{/each}}", Model));
        }

        [TestMethod]
        public void SortBy_QuotedNestedName()
        {
            //Oslo, Paris, Rome
            Assert.AreEqual("abc ", Render("{{#each sortBy(model.items, 'owner.city')}}<p>{{this.name}}</p>{{/each}}", Model));
        }

        [TestMethod]
        public void SortBy_QuotedName_WithReverse()
        {
            Assert.AreEqual("cba ", Render("{{#each reverse(sortBy(model.items, 'name'))}}<p>{{this.name}}</p>{{/each}}", Model));
        }

        [TestMethod]
        public void SumOf_AverageOf_MinOf_MaxOf_Quoted()
        {
            Assert.AreEqual("6 ", Render("<p>{{sumOf(model.items, 'price')}}</p>", Model));
            Assert.AreEqual("2 ", Render("<p>{{averageOf(model.items, 'price')}}</p>", Model));
            Assert.AreEqual("1 ", Render("<p>{{minOf(model.items, 'price')}}</p>", Model));
            Assert.AreEqual("3 ", Render("<p>{{maxOf(model.items, 'price')}}</p>", Model));
        }

        [TestMethod]
        public void EachOf_Quoted_ExtractsProperty()
        {
            Assert.AreEqual("b, a, c ", Render("<p>{{join(eachOf(model.items, 'name'), ', ')}}</p>", Model));
        }

        [TestMethod]
        public void SelectWhere_FirstWhere_QuotedBooleanProperty()
        {
            Assert.AreEqual("2 ", Render("<p>{{count(selectWhere(model.items, 'active'))}}</p>", Model));
            Assert.AreEqual("b ", Render("<p>{{firstWhere(model.items, 'active').name}}</p>", Model));
        }

        [TestMethod]
        public void SortBy_PlainValues_NoProperty()
        {
            var model = new { names = new[] { "pear", "apple", "fig" }, nums = new[] { 3, 1, 2 }, d = new System.Collections.Generic.Dictionary<string, int> { { "c", 1 }, { "a", 2 }, { "b", 3 } } };
            Assert.AreEqual("apple, fig, pear ", Render("<p>{{join(sortBy(model.names), ', ')}}</p>", model));
            Assert.AreEqual("pear, fig, apple ", Render("<p>{{join(sortBy(model.names, 'desc'), ', ')}}</p>", model));
            Assert.AreEqual("apple, fig, pear ", Render("<p>{{join(sortBy(model.names, 'asc'), ', ')}}</p>", model));
            Assert.AreEqual("123 ", Render("{{#each sortBy(model.nums)}}<p>{{this}}</p>{{/each}}", model));
            Assert.AreEqual("a,b,c ", Render("<p>{{join(sortBy(keys(model.d)), ',')}}</p>", model));
        }

        [TestMethod]
        public void SortBy_DuplicateKeys_AreAllKept_InTheirOriginalOrder()
        {
            var model = new { items = new[] { new { name = "x", group = 2 }, new { name = "y", group = 1 }, new { name = "z", group = 2 }, new { name = "w", group = 1 } } };
            Assert.AreEqual("ywxz ", Render("{{#each sortBy(model.items, 'group')}}<p>{{this.name}}</p>{{/each}}", model));
            Assert.AreEqual("xzyw ", Render("{{#each sortBy(model.items, .group, 'desc')}}<p>{{this.name}}</p>{{/each}}", model));
            Assert.AreEqual("aab ", Render("{{#each sortBy(model.values)}}<p>{{this}}</p>{{/each}}", new { values = new[] { "b", "a", "a" } }));
        }

        [TestMethod]
        public void DotName_StillWorks()
        {
            Assert.AreEqual("6 ", Render("<p>{{sumOf(model.items, .price)}}</p>", Model));
            Assert.AreEqual("2 ", Render("<p>{{count(selectWhere(model.items, .active))}}</p>", Model));
        }
    }
}
