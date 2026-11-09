using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Binding
{
    [TestClass]
    public class StrictEach_Tests
    {
        private const string Body = "<html xmlns='http://www.w3.org/1999/xhtml'><body><main>{{#each model.products}}<p>{{this.name}}</p>{{/each}}</main></body></html>";

        private static void Run(string src)
        {
            using (var sr = new StringReader(src))
            using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
            {
                doc.Params["model"] = new { products = new[] { new { name = "One" }, new { name = "Two" } } };
                using (var ms = new MemoryStream())
                    doc.SaveAsPDF(ms);
            }
        }

        [TestMethod]
        public void Each_Strict_LinqIterator_NotResettable()
        {
            var linq = new List<string> { "One", "Two" }.Select(n => new { name = n });
            using (var sr = new StringReader("<?scryber parser-mode='Strict' ?>" + Body))
            using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
            {
                doc.Params["model"] = new { products = linq };
                using (var ms = new MemoryStream())
                    doc.SaveAsPDF(ms);
            }
        }

        [TestMethod]
        public void Each_Strict_LinqIterator_Nested()
        {
            var linq = new List<string> { "One", "Two" }.Select(n => new { name = n, items = new List<int> { 1, 2 }.Select(i => i * 2) });
            var src = "<?scryber parser-mode='Strict' ?><html xmlns='http://www.w3.org/1999/xhtml'><body><main>{{#each model.products}}<p>{{this.name}}</p>{{#each this.items}}<span>{{this}}</span>{{/each}}{{/each}}</main></body></html>";
            using (var sr = new StringReader(src))
            using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
            {
                doc.Params["model"] = new { products = linq };
                using (var ms = new MemoryStream())
                    doc.SaveAsPDF(ms);
            }
        }

        [TestMethod] public void Each_Lax() { Run("<?scryber parser-mode='Lax' ?>" + Body); }
        [TestMethod] public void Each_Strict() { Run("<?scryber parser-mode='Strict' ?>" + Body); }
        [TestMethod] public void Each_Strict_Paperwork() { Run("<?paperwork parser-mode='Strict' ?>" + Body); }
    }
}
