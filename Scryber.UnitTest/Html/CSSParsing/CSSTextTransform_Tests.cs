using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Html.CSSParsing
{
    [TestClass]
    public class CSSTextTransform_Tests
    {
        private static string Render(string body, string css = "")
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>" + css + "</style></head><body>" + body + "</body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                return Generation.PanelContent_Tests.TextOf(ms.ToArray());
            }
        }

        [TestMethod]
        public void TextTransform_Uppercase() { Assert.AreEqual("UPPER CASE ME ", Render("<p style='text-transform: uppercase'>upper case me</p>")); }

        [TestMethod]
        public void TextTransform_Lowercase() { Assert.AreEqual("lower case me ", Render("<p style='text-transform: lowercase'>LOWER Case ME</p>")); }

        [TestMethod]
        public void TextTransform_Capitalize() { Assert.AreEqual("Capitalize Each Word ", Render("<p style='text-transform: capitalize'>capitalize each word</p>")); }

        [TestMethod]
        public void TextTransform_None() { Assert.AreEqual("Leave Me ", Render("<p style='text-transform: none'>Leave Me</p>")); }

        [TestMethod]
        public void TextTransform_InheritedFromParent_AndOverridden()
        {
            var text = Render("<div class='up'><p>inherited</p><p class='no'>not Changed</p><span>span</span></div>", ".up{text-transform:uppercase;} .no{text-transform:none;}");
            Assert.AreEqual("INHERITEDnot ChangedSPAN ", text);
        }

        [TestMethod]
        public void TextTransform_Capitalize_AcrossInlineElements()
        {
            //the word continues across the inline element, so the b is not capitalized.
            var text = Render("<p style='text-transform: capitalize'>un<b>bro</b>ken word</p>");
            Assert.AreEqual("Unbroken Word ", text);
        }

        [TestMethod]
        public void TextTransform_Expression()
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><body><p style='text-transform: {{model.t}}'>bound</p></body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                doc.Params["model"] = new { t = "uppercase" };
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                Assert.AreEqual("BOUND ", Generation.PanelContent_Tests.TextOf(ms.ToArray()));
            }
        }
    }
}
