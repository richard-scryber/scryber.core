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

        // gradients that cannot be drawn

        private static string RenderWithLog(string css, string body, out Scryber.Logging.CollectorTraceLog log)
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>@page{size:400pt 300pt;margin:0} body{margin:0} " + css + "</style></head>" +
                      "<body>" + body + "</body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                log = new Scryber.Logging.CollectorTraceLog(TraceRecordLevel.Warnings, "test", true);
                ((IParsedDocument)doc).SetTraceLog(log);

                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                return Regex.Replace(PdfContent.Decompressed(ms.ToArray()), @"/[a-z]+_frsc(\d+)", "/font$1");
            }
        }

        private static bool HasWarning(Scryber.Logging.CollectorTraceLog log, string contains)
        {
            foreach (var entry in log)
            {
                if (entry.Level == TraceLevel.Warning && entry.Message.Contains(contains))
                    return true;
            }
            return false;
        }

        private const string GradientBody = "<div class='g' style='height:40pt;background-color:#1B3350'>Text</div>";

        [TestMethod]
        public void CSSBrowserParity_15_Gradient_ThatCannotBeRead_IsNotDrawn_AndLogsAWarning()
        {
            //2 positions on a stop are not supported, so this gradient has no colors
            var withGradient = RenderWithLog(".g{background-image: repeating-radial-gradient(ellipse 120% 400% at 92% 0%, transparent 0 10.5pt, rgba(255,255,255,.09) 10.5pt 11.25pt)}",
                                             GradientBody, out var log);
            var colorOnly = RenderWithLog("", GradientBody, out _);

            Assert.AreEqual(colorOnly, withGradient, "The background color is drawn, without the gradient");
            Assert.IsTrue(HasWarning(log, "could not be read"), "A warning is logged");
        }

        [TestMethod]
        public void CSSBrowserParity_16_Gradient_WithOneColor_IsNotDrawn()
        {
            var withGradient = RenderWithLog(".g{background-image: linear-gradient(red)}", GradientBody, out var log);
            var colorOnly = RenderWithLog("", GradientBody, out _);

            Assert.AreEqual(colorOnly, withGradient);
            Assert.IsTrue(HasWarning(log, "could not be read"));
        }

        [TestMethod]
        public void CSSBrowserParity_17_Gradient_TransparentColor_IsBlack_AndLogsAWarning()
        {
            var pdf = "";
            RenderWithLog(".g{background-image: radial-gradient(circle, transparent 0%, red 50%)}", GradientBody, out var log);
            Assert.IsTrue(HasWarning(log, "drawn as black"), "A warning is logged");

            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>.g{height:40pt;background-image: radial-gradient(circle, transparent 0%, red 50%)}</style></head><body><div class='g'></div></body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                pdf = System.Text.Encoding.Latin1.GetString(ms.ToArray());
            }
            StringAssert.Contains(pdf, "/C0 [ 0.0000 0.0000 0.0000] /C1 [ 1.0000 0.0000 0.0000]", "The transparent color is black");
        }

        [TestMethod]
        public void CSSBrowserParity_18_Gradient_RepeatingWithTransparentStops_IsDrawn()
        {
            var colorOnly = RenderWithLog("", GradientBody, out _);
            var withGradient = RenderWithLog(".g{background-image: repeating-radial-gradient(ellipse at 92% 0%, transparent 0%, transparent 10%, red 10%, red 12%)}",
                                             GradientBody, out var log);

            Assert.AreNotEqual(colorOnly, withGradient, "The gradient is drawn");
            Assert.IsTrue(HasWarning(log, "drawn as black"));
        }

        [TestMethod]
        public void CSSBrowserParity_19_FontShorthand_LineHeight_IsNotReadInTheCurrentCulture()
        {
            const string css = "p{font: 10pt/1.5 Helvetica}";
            const string body = "<p>First line of text that is long enough to wrap onto a second line within the page width</p>";

            var current = System.Globalization.CultureInfo.CurrentCulture;
            string english, dutch;
            try
            {
                System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-GB");
                english = Render(css, body);

                //'.' is the group separator in nl-BE, so 1.5 would be read as 15
                System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("nl-BE");
                dutch = Render(css, body);
            }
            finally
            {
                System.Globalization.CultureInfo.CurrentCulture = current;
            }

            Assert.AreEqual(english, dutch, "The line height is the same whatever the culture");
        }

        private static string RenderInCulture(string culture, string css, string body)
        {
            var current = System.Globalization.CultureInfo.CurrentCulture;
            try
            {
                System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo(culture);
                return Render(css, body);
            }
            finally
            {
                System.Globalization.CultureInfo.CurrentCulture = current;
            }
        }

        [TestMethod]
        public void CSSBrowserParity_20_Transform_IsNotReadInTheCurrentCulture()
        {
            const string css = ".t{position:absolute;left:50pt;top:50pt;width:40pt;height:20pt;background-color:red;transform: rotate(12.5deg) translate(10.5pt, 4.5pt)}";
            const string body = "<div class='t'></div>";

            Assert.AreEqual(RenderInCulture("en-GB", css, body), RenderInCulture("nl-BE", css, body));
        }

        [TestMethod]
        public void CSSBrowserParity_21_SvgTransform_IsNotReadInTheCurrentCulture()
        {
            const string body = "<svg xmlns='http://www.w3.org/2000/svg' width='200' height='100'><rect x='20' y='20' width='40' height='20' fill='red' transform='rotate(12.5deg)' /></svg>";

            //the svg canvas resource names are random
            var english = Regex.Replace(RenderInCulture("en-GB", "", body), @"/[a-z]+_canv(\d+)", "/canv$1");
            var dutch = Regex.Replace(RenderInCulture("nl-BE", "", body), @"/[a-z]+_canv(\d+)", "/canv$1");

            Assert.AreEqual(english, dutch);
        }
    }
}
