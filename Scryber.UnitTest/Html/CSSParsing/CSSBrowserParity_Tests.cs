using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF.Graphics;
using Scryber.Styles;
using Scryber.Styles.Parsing.Typed;

namespace Scryber.Core.UnitTests.Html.CSSParsing
{
    /// <summary>
    /// Css that renders the same as a browser: gradient stops as lengths, box-shadow colors from variables,
    /// em letter and word spacing of the element's own font size, and inline-flex.
    /// </summary>
    [TestClass]
    public class CSSBrowserParity_Tests
    {
        private static string Render(string css, string body)
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>@page{size:400pt 300pt;margin:0} body{margin:0} " + css + "</style></head>" +
                      "<body>" + body + "</body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                //font resource names are random
                return Regex.Replace(PdfContent.Decompressed(ms.ToArray()), @"/[a-z]+_frsc(\d+)", "/font$1");
            }
        }

        private static string RenderAll(string css, string body)
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>@page{size:400pt 300pt;margin:0} body{margin:0} " + css + "</style></head>" +
                      "<body>" + body + "</body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                return System.Text.Encoding.Latin1.GetString(ms.ToArray());
            }
        }

        // gradient stops

        [TestMethod]
        public void CSSBrowserParity_01_GradientStop_Length_IsOfTheGradientLine()
        {
            var linear = GradientDescriptor.Parse("linear-gradient(90deg, #E2C46F 100pt, #1B3350 100pt)");
            var fn = linear.GetGradientFunction(Point.Empty, new Size(400, 30)) as PDFGradientFunction3;

            Assert.IsNotNull(fn, "A gradient with a hard stop has several functions");
            Assert.AreEqual(2, fn.Boundaries.Length);
            Assert.AreEqual(0.25, fn.Boundaries[0].Bounds, 0.0001);
            Assert.AreEqual(0.25, fn.Boundaries[1].Bounds, 0.0001, "The same position is a hard stop");
        }

        [TestMethod]
        public void CSSBrowserParity_02_GradientStop_Length_IsResolvedForEachSize()
        {
            var linear = GradientDescriptor.Parse("linear-gradient(90deg, red 50pt, blue 50pt)");

            var wide = linear.GetGradientFunction(Point.Empty, new Size(200, 30)) as PDFGradientFunction3;
            Assert.AreEqual(0.25, wide.Boundaries[0].Bounds, 0.0001);

            var narrow = linear.GetGradientFunction(Point.Empty, new Size(100, 30)) as PDFGradientFunction3;
            Assert.AreEqual(0.5, narrow.Boundaries[0].Bounds, 0.0001);
        }

        [TestMethod]
        public void CSSBrowserParity_03_GradientStop_Percent_IsUnchanged()
        {
            var linear = GradientDescriptor.Parse("linear-gradient(90deg, red 30%, blue 30%)");
            var fn = linear.GetGradientFunction(Point.Empty, new Size(400, 30)) as PDFGradientFunction3;

            Assert.AreEqual(0.3, fn.Boundaries[0].Bounds, 0.0001);
            Assert.AreEqual(0.3, fn.Boundaries[1].Bounds, 0.0001);
        }

        [TestMethod]
        public void CSSBrowserParity_04_GradientStop_BeforeThePrevious_IsAtThePrevious()
        {
            var linear = GradientDescriptor.Parse("linear-gradient(90deg, red 40%, blue 10%)");
            var fn = linear.GetGradientFunction(Point.Empty, new Size(400, 30)) as PDFGradientFunction3;

            Assert.AreEqual(0.4, fn.Boundaries[0].Bounds, 0.0001);
            Assert.AreEqual(0.4, fn.Boundaries[1].Bounds, 0.0001);
        }

        [TestMethod]
        public void CSSBrowserParity_05_GradientStop_RgbWithAPosition_KeepsItsColor()
        {
            Assert.IsTrue(GradientColor.TryParse("rgb(255,0,0) 20%", out var color));
            Assert.AreEqual(StandardColors.Red, color.Color);
            Assert.AreEqual(20.0, color.Distance);

            Assert.IsTrue(GradientColor.TryParse("rgba(0,0,255,0.5) 12pt", out color));
            Assert.AreEqual(StandardColors.Blue, color.Color);
            Assert.AreEqual(0.5, color.Opacity);
            Assert.AreEqual(12.0, color.Length.Value.PointsValue);
            Assert.IsFalse(color.Distance.HasValue, "The distance is not known until the gradient has a size");
        }

        [TestMethod]
        public void CSSBrowserParity_06_GradientStop_LengthWithVariables_IsAHardStop()
        {
            var pdf = RenderAll(":root{--accent:#E2C46F;--navy:#1B3350} .g{height:30pt;background: linear-gradient(90deg, var(--accent) 100pt, var(--navy) 100pt);}",
                                "<div class='g'></div>");

            var bounds = Regex.Match(pdf, @"/Bounds\s*\[\s*([0-9.]+)\s+([0-9.]+)\s*\]");
            Assert.IsTrue(bounds.Success, "The gradient has 2 bounds");
            Assert.AreEqual(0.25, double.Parse(bounds.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture), 0.0001);
            Assert.AreEqual(0.25, double.Parse(bounds.Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture), 0.0001);
        }

        // box-shadow

        private const string ShadowBody = "<div class='s' style='margin:20pt;height:40pt;width:100pt;background-color:#1B3350'></div>";

        [TestMethod]
        public void CSSBrowserParity_07_BoxShadow_ColorFromAVariable()
        {
            var literal = Render(".s{box-shadow: 9pt 9pt 0 #E2C46F}", ShadowBody);
            var variable = Render(":root{--accent:#E2C46F} .s{box-shadow: 9pt 9pt 0 var(--accent)}", ShadowBody);
            var none = Render("", ShadowBody);

            Assert.AreNotEqual(none, literal);
            Assert.AreEqual(literal, variable);
        }

        [TestMethod]
        public void CSSBrowserParity_08_BoxShadow_LengthsAndColorFromVariables()
        {
            var literal = Render(".s{box-shadow: 6pt 6pt 2pt #E2C46F}", ShadowBody);
            var variable = Render(":root{--off:6pt;--accent:#E2C46F} .s{box-shadow: var(--off) var(--off) 2pt var(--accent)}", ShadowBody);

            Assert.AreEqual(literal, variable);
        }

        [TestMethod]
        public void CSSBrowserParity_09_BoxShadow_WholeValueFromAVariable()
        {
            var literal = Render(".s{box-shadow: 9pt 9pt 0 #E2C46F}", ShadowBody);
            var variable = Render(":root{--shadow:9pt 9pt 0 #E2C46F} .s{box-shadow: var(--shadow)}", ShadowBody);

            Assert.AreEqual(literal, variable);
        }

        // letter and word spacing

        [TestMethod]
        public void CSSBrowserParity_10_LetterSpacing_Em_IsOfTheOwnFontSize()
        {
            var content = Render("", "<div style='font-size:72pt;letter-spacing:-.04em'>WIDE</div>");
            StringAssert.Contains(content, "-2.88 Tc");

            content = Render("", "<div style='font-size:20pt'><span style='font-size:40pt;letter-spacing:.1em'>WIDE</span></div>");
            StringAssert.Contains(content, "4.00 Tc");
        }

        [TestMethod]
        public void CSSBrowserParity_11_LetterSpacing_EmOfARelativeFontSize()
        {
            //2em of the 20pt parent is 40pt, and .1em of that is 4pt
            var content = Render("", "<div style='font-size:20pt'><span style='font-size:2em;letter-spacing:.1em'>WIDE</span></div>");
            StringAssert.Contains(content, "4.00 Tc");
        }

        [TestMethod]
        public void CSSBrowserParity_12_LetterSpacing_Points_IsUnchanged()
        {
            var content = Render("", "<div style='font-size:72pt;letter-spacing:-3pt'>WIDE</div>");
            StringAssert.Contains(content, "-3.00 Tc");
        }

        // inline-flex

        [TestMethod]
        public void CSSBrowserParity_13_Display_InlineFlex_IsAnInlineBlockOfFlexContent()
        {
            Style style;
            var parser = new CSSDisplayParser();

            foreach (var (value, inside) in new[] { ("inline-flex", DisplayMode.FlexBox), ("inline-grid", DisplayMode.FlexGrid), ("inline-table", DisplayMode.Table) })
            {
                style = new Style();
                var reader = new Scryber.Styles.Parsing.CSSStyleItemReader(value);
                Assert.IsTrue(parser.SetStyleValue(style, reader, null), value);

                Assert.AreEqual(DisplayMode.InlineBlock, style.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block), value);
                Assert.AreEqual(inside, style.GetValue(StyleKeys.PositionDisplayInsideKey, DisplayMode.Block), value);
            }
        }

        [TestMethod]
        public void CSSBrowserParity_14_Display_Block_ReplacesInlineFlex()
        {
            var content = Render(".m{display:inline-flex;justify-content:center;width:60pt;height:20pt} .b{display:block}",
                                 "<span class='m b'>H</span>");
            var block = Render(".m{display:block;width:60pt;height:20pt}", "<span class='m'>H</span>");

            Assert.AreEqual(block, content);
        }
    }
}
