using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Binding
{
    [TestClass]
    public class KeysFunction_Tests
    {
        private static string Render(string body, object model, string pi = "")
        {
            var src = pi + "<html xmlns='http://www.w3.org/1999/xhtml'><body>" + body + "</body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                doc.Params["model"] = model;
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                return Generation.PanelContent_Tests.TextOf(ms.ToArray());
            }
        }

        private static Dictionary<string, int> Letters()
        {
            return new Dictionary<string, int> { { "c", 3 }, { "a", 1 }, { "b", 2 } };
        }

        [TestMethod]
        public void Keys_Dictionary_JoinedInOrder()
        {
            Assert.AreEqual("c,a,b ", Render("<p>{{join(',', keys(model.d))}}</p>", new { d = Letters() }));
        }

        [TestMethod]
        public void Join_CollectionThenSeparator_AndSeparatorThenItems()
        {
            var model = new { tags = new[] { "x", "y", "z" } };
            Assert.AreEqual("x, y, z ", Render("<p>{{join(model.tags, ', ')}}</p>", model), "collection first");
            Assert.AreEqual("x, y, z ", Render("<p>{{join(', ', model.tags)}}</p>", model), "separator first");
            Assert.AreEqual("a-b-c ", Render("<p>{{join('-', 'a', 'b', 'c')}}</p>", model), "separator then items");
            Assert.AreEqual("c,a,b ", Render("<p>{{join(keys(model.d), ',')}}</p>", new { d = Letters() }), "keys then separator");
        }

        [TestMethod]
        public void Keys_Dictionary_Count()
        {
            Assert.AreEqual("3 ", Render("<p>{{count(keys(model.d))}}</p>", new { d = Letters() }));
        }

        [TestMethod]
        public void Keys_Dictionary_Sorted()
        {
            Assert.AreEqual("a,b,c ", Render("<p>{{join(',', sortBy(keys(model.d), .))}}</p>", new { d = Letters() }));
        }

        [TestMethod]
        public void Keys_Dictionary_SortedDescending()
        {
            Assert.AreEqual("c,b,a ", Render("<p>{{join(',', sortBy(keys(model.d), ., 'desc'))}}</p>", new { d = Letters() }));
        }

        [TestMethod]
        public void Keys_Dictionary_EnumeratedInEach()
        {
            var text = Render("{{#each keys(model.d)}}<p>{{@index}}:{{this}}</p>{{/each}}", new { d = Letters() });
            Assert.AreEqual("0:c1:a2:b ", text);
        }

        [TestMethod]
        public void Keys_Dictionary_SortedEnumeratedInEach()
        {
            var text = Render("{{#each sortBy(keys(model.d), .)}}<p>{{this}}</p>{{/each}}", new { d = Letters() });
            Assert.AreEqual("abc ", text);
        }

        [TestMethod]
        public void Keys_Dictionary_ValidateLength()
        {
            var text = Render("{{#if count(keys(model.d)) > 2}}<p>many</p>{{else}}<p>few</p>{{/if}}", new { d = Letters() });
            Assert.AreEqual("many ", text);
            text = Render("{{#if count(keys(model.d)) > 2}}<p>many</p>{{else}}<p>few</p>{{/if}}", new { d = new Dictionary<string, int> { { "x", 1 } } });
            Assert.AreEqual("few ", text);
        }

        [TestMethod]
        public void Keys_Dictionary_Empty()
        {
            Assert.AreEqual("0 ", Render("<p>{{count(keys(model.d))}}</p>", new { d = new Dictionary<string, int>() }));
        }

        [TestMethod]
        public void Keys_Dictionary_IntKeysAsStrings()
        {
            Assert.AreEqual("10,20 ", Render("<p>{{join(',', keys(model.d))}}</p>", new { d = new Dictionary<int, string> { { 10, "a" }, { 20, "b" } } }));
        }

        [TestMethod]
        public void Keys_AnonymousObject_PropertyNames()
        {
            Assert.AreEqual("name,age ", Render("<p>{{join(',', keys(model.person))}}</p>", new { person = new { name = "Ann", age = 30 } }));
        }

        [TestMethod]
        public void Keys_NewtonsoftJObject()
        {
            var model = new { obj = Newtonsoft.Json.Linq.JObject.Parse("{\"first\":1,\"second\":{\"x\":1}}") };
            Assert.AreEqual("first,second2 ", Render("<p>{{join(',', keys(model.obj))}}</p><p>{{count(keys(model.obj))}}</p>", model));
        }

        [TestMethod]
        public void Keys_SystemTextJsonObject()
        {
            using (var jdoc = System.Text.Json.JsonDocument.Parse("{\"alpha\":\"A\",\"beta\":\"B\"}"))
            {
                var model = new { obj = jdoc.RootElement.Clone() };
                Assert.AreEqual("alpha,beta ", Render("<p>{{join(',', keys(model.obj))}}</p>", model));
            }
        }

        [TestMethod]
        public void Keys_ExpandoObject()
        {
            IDictionary<string, object> expando = new System.Dynamic.ExpandoObject();
            expando["one"] = 1;
            expando["two"] = 2;
            CollectionAssert.AreEqual(new[] { "one", "two" }, Scryber.Expressive.Functions.Coalesce.KeysFunction.GetKeys(expando));
            Assert.AreEqual("one,two ", Render("<p>{{join(keys(model.ex), ',')}}</p>", new { ex = expando }));
            Assert.AreEqual("12 ", Render("<p>{{model.ex.one}}</p><p>{{model.ex.two}}</p>", new { ex = expando }));
        }

        [TestMethod]
        public void Keys_ReadOnlyDictionary()
        {
            var ro = new System.Collections.ObjectModel.ReadOnlyDictionary<string, int>(Letters());
            Assert.AreEqual("c,a,b ", Render("<p>{{join(',', keys(model.d))}}</p>", new { d = ro }));
        }

        [TestMethod]
        public void Keys_NullArray_StringAndNumber_HaveNoKeys()
        {
            var text = Render("<p>{{count(keys(model.missing))}}</p><p>{{count(keys(model.list))}}</p><p>{{count(keys(model.text))}}</p><p>{{count(keys(model.num))}}</p>",
                new { missing = (object)null, list = new[] { 1, 2 }, text = "abc", num = 4 });
            Assert.AreEqual("0000 ", text);
        }

        [TestMethod]
        public void Keys_UsedWithKeyedLookupInEach()
        {
            var text = Render("{{#each sortBy(keys(model.d), .)}}<p>{{this}}={{model.d[this]}}</p>{{/each}}", new { d = Letters() });
            Assert.AreEqual("a=1b=2c=3 ", text);
        }

        [TestMethod]
        public void Keys_Strict()
        {
            Assert.AreEqual("c,a,b ", Render("<p>{{join(',', keys(model.d))}}</p>", new { d = Letters() }, "<?scryber parser-mode='Strict' ?>"));
        }

        [TestMethod]
        public void Keys_WrongParameterCount_Throws()
        {
            bool thrown = false;
            try { Render("<p>{{keys(model.d, model.d)}}</p>", new { d = Letters() }); }
            catch (Exception) { thrown = true; }
            Assert.IsTrue(thrown, "keys() with two parameters should throw");
        }
    }
}
