using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Html.CSSParsing
{
    /// <summary>
    /// Tests the logical shorthands (margin-block, padding-block, inset...), and the properties that are accepted and ignored.
    /// Each is checked as producing the same content as the longhand version, and as not being the same as no style.
    /// </summary>
    [TestClass]
    public class CSSBlockInset_Tests
    {
        private static string Render(string style, string position = "", object model = null)
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>@page{size:300pt 300pt;margin:0} body{margin:0;font-size:12pt} div{background-color:#FF0000;}</style></head>" +
                      "<body><div style='height:10pt;background-color:#00FF00'>before</div><div id='box' style='" + position + "width:80pt;" + style + "'>box</div><div style='height:10pt;background-color:#0000FF'>after</div></body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                if (model != null)
                    doc.Params["model"] = model;
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                //the font resource names are random for each document
                return System.Text.RegularExpressions.Regex.Replace(PdfContent.Decompressed(ms.ToArray()), @"/[a-z]+_frsc\d+", "/F");
            }
        }

        private static void AssertSame(string shorthand, string longhand, string position = "")
        {
            var expected = Render(longhand, position);
            Assert.AreEqual(expected, Render(shorthand, position), "'" + shorthand + "' should be the same as '" + longhand + "'");
            Assert.AreNotEqual(Render("", position), expected, "'" + longhand + "' should change the layout");
        }

        // margin-block, padding-block

        [TestMethod]
        public void MarginBlock_OneValue_TopAndBottom()
        {
            AssertSame("margin-block: 20pt", "margin-top: 20pt; margin-bottom: 20pt");
        }

        [TestMethod]
        public void MarginBlock_TwoValues_StartThenEnd()
        {
            AssertSame("margin-block: 10pt 30pt", "margin-top: 10pt; margin-bottom: 30pt");
        }

        [TestMethod]
        public void MarginBlock_StartAndEnd()
        {
            AssertSame("margin-block-start: 15pt", "margin-top: 15pt");
            AssertSame("margin-block-end: 15pt", "margin-bottom: 15pt");
        }

        [TestMethod]
        public void MarginBlock_Units()
        {
            AssertSame("margin-block: 0.5in 1cm", "margin-top: 0.5in; margin-bottom: 1cm");
        }

        [TestMethod]
        public void PaddingBlock_OneValue_TopAndBottom()
        {
            AssertSame("padding-block: 12pt", "padding-top: 12pt; padding-bottom: 12pt");
        }

        [TestMethod]
        public void PaddingBlock_TwoValues_StartThenEnd()
        {
            AssertSame("padding-block: 4pt 16pt", "padding-top: 4pt; padding-bottom: 16pt");
        }

        [TestMethod]
        public void PaddingBlock_StartAndEnd()
        {
            AssertSame("padding-block-start: 9pt", "padding-top: 9pt");
            AssertSame("padding-block-end: 9pt", "padding-bottom: 9pt");
        }

        [TestMethod]
        public void MarginBlock_Expression()
        {
            var expected = Render("margin-top: 10pt; margin-bottom: 30pt");
            Assert.AreEqual(expected, Render("margin-block: {{model.start}} {{model.end}}", "", new { start = "10pt", end = "30pt" }));
            Assert.AreEqual(expected, Render("margin-block: 10pt {{model.end}}", "", new { end = "30pt" }));
        }

        [TestMethod]
        public void BlockShorthands_Invalid_AreIgnored()
        {
            var plain = Render("");
            Assert.AreEqual(plain, Render("margin-block: wibble"));
            Assert.AreEqual(plain, Render("padding-block: 10pt wibble"));
            Assert.AreEqual(plain, Render("margin-block:"));
        }

        [TestMethod]
        public void BlockShorthands_ExtraValues_AreIgnored()
        {
            Assert.AreEqual(Render("margin-top: 10pt; margin-bottom: 30pt"), Render("margin-block: 10pt 30pt 50pt"));
        }

        // inset

        private const string Absolute = "position: absolute; ";
        private const string Relative = "position: relative; ";

        [TestMethod]
        public void Inset_OneValue_AllSides()
        {
            AssertSame("inset: 20pt", "top: 20pt; right: 20pt; bottom: 20pt; left: 20pt", Absolute);
        }

        [TestMethod]
        public void Inset_TwoValues_BlockThenInline()
        {
            AssertSame("inset: 10pt 30pt", "top: 10pt; bottom: 10pt; left: 30pt; right: 30pt", Absolute);
        }

        [TestMethod]
        public void Inset_ThreeValues()
        {
            AssertSame("inset: 10pt 30pt 50pt", "top: 10pt; left: 30pt; right: 30pt; bottom: 50pt", Absolute);
        }

        [TestMethod]
        public void Inset_FourValues_TopRightBottomLeft()
        {
            AssertSame("inset: 10pt 20pt 30pt 40pt", "top: 10pt; right: 20pt; bottom: 30pt; left: 40pt", Absolute);
        }

        [TestMethod]
        public void Inset_Auto_LeavesTheSideUnset()
        {
            AssertSame("inset: 25pt auto auto 35pt", "top: 25pt; left: 35pt", Absolute);
        }

        [TestMethod]
        public void Inset_Relative_OffsetsTheBox()
        {
            AssertSame("inset: 10pt auto auto 40pt", "top: 10pt; left: 40pt", Relative);
        }

        [TestMethod]
        public void InsetBlock_AndInline()
        {
            AssertSame("inset-block: 20pt 30pt", "top: 20pt; bottom: 30pt", Absolute);
            AssertSame("inset-block: 15pt", "top: 15pt; bottom: 15pt", Absolute);
            AssertSame("inset-inline: 20pt 40pt", "left: 20pt; right: 40pt", Absolute);
            AssertSame("inset-inline: 25pt", "left: 25pt; right: 25pt", Absolute);
        }

        [TestMethod]
        public void InsetBlockAndInline_StartAndEnd()
        {
            AssertSame("inset-block-start: 12pt", "top: 12pt", Absolute);
            AssertSame("inset-block-end: 12pt", "bottom: 12pt", Absolute);
            AssertSame("inset-inline-start: 22pt", "left: 22pt", Absolute);
            AssertSame("inset-inline-end: 22pt", "right: 22pt", Absolute);
        }

        [TestMethod]
        public void Inset_Percentage()
        {
            AssertSame("inset: 10% auto auto 20%", "top: 10%; left: 20%", Absolute);
        }

        [TestMethod]
        public void Inset_Expression()
        {
            var expected = Render("top: 10pt; right: 20pt; bottom: 30pt; left: 40pt", Absolute);
            Assert.AreEqual(expected, Render("inset: {{model.t}} {{model.r}} {{model.b}} {{model.l}}", Absolute, new { t = "10pt", r = "20pt", b = "30pt", l = "40pt" }));
        }

        [TestMethod]
        public void Inset_Invalid_IsIgnored()
        {
            Assert.AreEqual(Render("", Absolute), Render("inset: sideways", Absolute));
        }

        [TestMethod]
        public void Inset_LaterLonghand_Overrides()
        {
            Assert.AreEqual(Render("top: 10pt; left: 40pt; right: 20pt; bottom: 99pt", Absolute), Render("inset: 10pt 20pt 30pt 40pt; bottom: 99pt", Absolute));
        }

        // ignored properties

        [TestMethod]
        public void IgnoredProperties_AreAccepted_AndChangeNothing()
        {
            var plain = Render("margin-top: 20pt");
            foreach (var value in new[] { "cursor: pointer", "pointer-events: none", "will-change: transform", "scroll-behavior: smooth", "user-select: none", "transition: all 0.3s ease-in-out", "animation: spin 2s linear infinite" })
                Assert.AreEqual(plain, Render("margin-top: 20pt; " + value), value);
        }

        [TestMethod]
        public void IgnoredProperties_DoNotStopOtherValues()
        {
            Assert.AreEqual(Render("margin-top: 20pt; margin-bottom: 5pt"), Render("cursor: pointer; margin-top: 20pt; transition: opacity 1s, color 2s; margin-bottom: 5pt"));
        }
    }
}
