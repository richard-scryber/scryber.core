using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Html.CSSParsing
{
    /// <summary>
    /// Tests the text-decoration-color, -style and -thickness, and the shorthand
    /// </summary>
    [TestClass]
    public class CSSTextDecoration_Tests
    {
        private static string Render(string style, object model = null)
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><body><p style='" + style + "'>Decorated</p></body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                if (model != null)
                    doc.Params["model"] = model;
                doc.RenderOptions.Compression = Scryber.OutputCompressionType.None;
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                return System.Text.Encoding.Latin1.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// Returns the part of the content for the decoration, after the text has ended (after ET)
        /// </summary>
        private static string Decoration(string pdf)
        {
            var i = pdf.IndexOf(" ET Q", StringComparison.Ordinal);
            Assert.IsTrue(i > 0, "No text found");
            var end = pdf.IndexOf("endstream", i, StringComparison.Ordinal);
            return pdf.Substring(i, end - i);
        }

        private static int Count(string text, string pattern)
        {
            return Regex.Matches(text, pattern).Count;
        }

        [TestMethod]
        public void Underline_Default_IsSolidInTheTextColor_AndFontThickness()
        {
            var d = Decoration(Render("text-decoration: underline; color: #0000FF"));
            Assert.IsTrue(d.Contains("0.0000 0.0000 1.0000 RG"), "Text color: " + d);
            Assert.IsTrue(d.Contains(" 1.33 w"), "The thickness is the font size / 12: " + d);
            Assert.AreEqual(1, Count(d, @" l S"));
        }

        [TestMethod]
        public void Color_Overrides_TheTextColor()
        {
            var d = Decoration(Render("text-decoration: underline; color: #0000FF; text-decoration-color: #FF0000"));
            Assert.IsTrue(d.Contains("1.0000 0.0000 0.0000 RG"), d);
            Assert.IsFalse(d.Contains("0.0000 0.0000 1.0000 RG"));
        }

        [TestMethod]
        public void Color_CurrentColor_IsTheTextColor()
        {
            var d = Decoration(Render("text-decoration: underline; color: #0000FF; text-decoration-color: currentcolor"));
            Assert.IsTrue(d.Contains("0.0000 0.0000 1.0000 RG"), d);
        }

        [TestMethod]
        public void Thickness_Length_SetsTheLineWidth()
        {
            var d = Decoration(Render("text-decoration: underline; text-decoration-thickness: 3pt"));
            Assert.IsTrue(d.Contains(" 3.00 w"), d);
        }

        [TestMethod]
        public void Thickness_AutoAndFromFont_UseTheFontSize()
        {
            Assert.IsTrue(Decoration(Render("text-decoration: underline; text-decoration-thickness: auto")).Contains(" 1.33 w"));
            Assert.IsTrue(Decoration(Render("text-decoration: underline; text-decoration-thickness: from-font")).Contains(" 1.33 w"));
        }

        [TestMethod]
        public void Style_Double_DrawsTwoLines()
        {
            var d = Decoration(Render("text-decoration: underline; text-decoration-style: double"));
            Assert.AreEqual(2, Count(d, @" l S"), d);
        }

        [TestMethod]
        public void Style_Dotted_AndDashed_SetADashPattern()
        {
            var dotted = Decoration(Render("text-decoration: underline; text-decoration-style: dotted"));
            Assert.IsTrue(Regex.IsMatch(dotted, @"\[[0-9\.\s]+\]\s+0\s+d"), "Dotted should set a dash: " + dotted);

            var dashed = Decoration(Render("text-decoration: underline; text-decoration-style: dashed"));
            Assert.IsTrue(Regex.IsMatch(dashed, @"\[[0-9\.\s]+\]\s+0\s+d"), "Dashed should set a dash: " + dashed);
            Assert.AreNotEqual(Regex.Match(dotted, @"\[[0-9\.\s]+\]").Value, Regex.Match(dashed, @"\[[0-9\.\s]+\]").Value, "The patterns differ");
        }

        [TestMethod]
        public void Style_Wavy_DrawsCurves()
        {
            var d = Decoration(Render("text-decoration: underline; text-decoration-style: wavy"));
            Assert.IsTrue(Count(d, @"\sc\s") > 2, "A wavy line is made of curves: " + d);
            Assert.AreEqual(0, Count(d, @" l S"), "There are no straight lines");
        }

        [TestMethod]
        public void Style_Solid_IsTheDefault()
        {
            var d = Decoration(Render("text-decoration: underline; text-decoration-style: solid"));
            Assert.AreEqual(1, Count(d, @" l S"));
            Assert.IsFalse(Regex.IsMatch(d, @"\]\s+0\s+d"));
        }

        [TestMethod]
        public void Shorthand_LineStyleColorThickness()
        {
            var d = Decoration(Render("text-decoration: underline wavy #00FF00 2pt"));
            Assert.IsTrue(d.Contains("0.0000 1.0000 0.0000 RG"), d);
            Assert.IsTrue(d.Contains(" 2.00 w"), d);
            Assert.IsTrue(Count(d, @"\sc\s") > 2);
        }

        [TestMethod]
        public void Shorthand_ColorFirst_AndLineThrough()
        {
            var d = Decoration(Render("text-decoration: red double line-through"));
            Assert.IsTrue(d.Contains("1.0000 0.0000 0.0000 RG"), d);
            Assert.AreEqual(2, Count(d, @" l S"));
        }

        [TestMethod]
        public void Overline_Double_DrawsTwoLinesAbove()
        {
            var d = Decoration(Render("text-decoration: overline; text-decoration-style: double"));
            Assert.AreEqual(2, Count(d, @" l S"), d);
        }

        [TestMethod]
        public void LineThrough_Wavy_Colored()
        {
            var d = Decoration(Render("text-decoration-line: line-through; text-decoration-style: wavy; text-decoration-color: blue"));
            Assert.IsTrue(d.Contains("0.0000 0.0000 1.0000 RG"), d);
            Assert.IsTrue(Count(d, @"\sc\s") > 2);
        }

        [TestMethod]
        public void Expressions_ColorStyleAndThickness()
        {
            var d = Decoration(Render("text-decoration: underline; text-decoration-color: {{model.c}}; text-decoration-style: {{model.s}}; text-decoration-thickness: {{model.t}}",
                new { c = "#FF0000", s = "double", t = "4pt" }));
            Assert.IsTrue(d.Contains("1.0000 0.0000 0.0000 RG"), d);
            Assert.IsTrue(d.Contains(" 4.00 w"), d);
            Assert.AreEqual(2, Count(d, @" l S"));
        }

        [TestMethod]
        public void InvalidValues_AreIgnored()
        {
            var d = Decoration(Render("text-decoration: underline; text-decoration-style: squiggly; text-decoration-color: notacolor; text-decoration-thickness: huge"));
            Assert.AreEqual(1, Count(d, @" l S"), d);
            Assert.IsTrue(d.Contains(" 1.33 w"));
        }

        [TestMethod]
        public void NoDecoration_NoLines()
        {
            var d = Decoration(Render("text-decoration-color: red"));
            Assert.AreEqual(0, Count(d, @" l S"));
        }
    }
}
