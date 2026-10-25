using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Binding
{
    /// <summary>
    /// A name after a '.' is always a property, even when it is the same as a constant (e, pi, true, false, null)
    /// </summary>
    [TestClass]
    public class ConstantPropertyNames_Tests
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

        [TestMethod]
        public void Property_NamedE_AndPi()
        {
            var model = new { e = 5, pi = "x" };
            Assert.AreEqual("5x ", Render("<p>{{model.e}}</p><p>{{model.pi}}</p>", model));
        }

        [TestMethod]
        public void Property_NamedTrueFalseNull()
        {
            var model = new { @true = "t", @false = "f", @null = "n" };
            Assert.AreEqual("tfn ", Render("<p>{{model.true}}</p><p>{{model.false}}</p><p>{{model.null}}</p>", model));
        }

        [TestMethod]
        public void Property_NamedE_Nested_AndInMath()
        {
            var model = new { price = new { e = 7 }, e = 5 };
            Assert.AreEqual("76 ", Render("<p>{{model.price.e}}</p><p>{{model.e + 1}}</p>", model));
        }

        [TestMethod]
        public void Property_NamedE_InDictionaryAndExpando()
        {
            IDictionary<string, object> expando = new System.Dynamic.ExpandoObject();
            expando["e"] = "ex";
            var dict = new Dictionary<string, object> { { "e", "dict" }, { "pi", "dpi" } };
            Assert.AreEqual("exdictdpi ", Render("<p>{{model.x.e}}</p><p>{{model.d.e}}</p><p>{{model.d.pi}}</p>", new { x = expando, d = dict }));
        }

        [TestMethod]
        public void Property_NamedE_AsLookupInCollectionFunction()
        {
            var model = new { items = new[] { new { e = 1 }, new { e = 2 } } };
            Assert.AreEqual("3 ", Render("<p>{{sumOf(model.items, .e)}}</p>", model));
        }

        [TestMethod]
        public void Constants_StillWork_WhenNotAfterADot()
        {
            var text = Render("<p>{{round(pi, 2)}}</p><p>{{round(e, 2)}}</p><p>{{if(true, 'yes', 'no')}}</p><p>{{null ?? 'default'}}</p>", new { });
            Assert.AreEqual("3.142.72yesdefault ", text);
        }

        [TestMethod]
        public void Property_NamedE_InParentContext()
        {
            var model = new { groups = new[] { new { e = "G1", items = new[] { "i" } } } };
            Assert.AreEqual("G1 ", Render("{{#each model.groups}}{{#each this.items}}<p>{{../e}}</p>{{/each}}{{/each}}", model));
        }
    }
}
