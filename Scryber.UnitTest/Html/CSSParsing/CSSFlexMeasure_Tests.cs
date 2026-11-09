using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Html.CSSParsing
{
    /// <summary>
    /// The flex items without a width are measured with a layout that is discarded. Counters, ::before content and
    /// names must only come from the real layout.
    /// </summary>
    [TestClass]
    public class CSSFlexMeasure_Tests
    {
        private static string Render(string css, string body)
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>@page{size:400pt 300pt;margin:10pt} .row{display:flex} .grow{flex:1} " + css + "</style></head><body>" + body + "</body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                return Generation.PanelContent_Tests.TextOf(ms.ToArray());
            }
        }

        [TestMethod]
        public void Counters_AreOnlyIncrementedOnce()
        {
            var text = Render("main{counter-reset:ev} .n{counter-increment:ev} .n::before{content:'E' counter(ev)}",
                "<main><div class='row'><span class='n'></span><span class='grow'>a</span></div><div class='row'><span class='n'></span><span class='grow'>b</span></div><div class='row'><span class='n'></span><span class='grow'>c</span></div></main>");
            Assert.IsTrue(text.Contains("E1") && text.Contains("E2") && text.Contains("E3"), text);
            Assert.IsFalse(text.Contains("E4") || text.Contains("E5") || text.Contains("E6"), "Counted twice: " + text);
        }

        [TestMethod]
        public void BeforeAndAfter_AreOnlyAddedOnce()
        {
            var text = Render(".c::before{content:'['} .c::after{content:']'}",
                "<div class='row'><span class='grow'>grow</span><span class='c'>item</span></div>");
            Assert.AreEqual(1, Regex.Matches(text, @"\[").Count, text);
            Assert.AreEqual(1, Regex.Matches(text, @"\]").Count, text);
        }

        [TestMethod]
        public void PageReference_ToAMeasuredItem_IsCorrect()
        {
            var text = Render("", "<p>Ref <page for='#target' /></p><div style='page-break-before:always' class='row'><span class='grow'>grow</span><span id='target'>target</span></div>");
            Assert.IsTrue(text.Contains("Ref 2"), text);
        }
    }
}
