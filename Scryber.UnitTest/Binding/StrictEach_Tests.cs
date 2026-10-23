using System;
using System.IO;
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

        [TestMethod] public void Each_Lax() { Run("<?scryber parser-mode='Lax' ?>" + Body); }
        [TestMethod] public void Each_Strict() { Run("<?scryber parser-mode='Strict' ?>" + Body); }
        [TestMethod] public void Each_Strict_Paperwork() { Run("<?paperwork parser-mode='Strict' ?>" + Body); }
    }
}
