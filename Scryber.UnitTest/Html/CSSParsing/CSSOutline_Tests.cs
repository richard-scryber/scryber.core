using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Html.CSSParsing
{
    /// <summary>
    /// Tests the outline css properties. The outlined box is 100pt x 50pt with a 20pt margin, so its edge is at 20,20 on the page.
    /// </summary>
    [TestClass]
    public class CSSOutline_Tests
    {
        private const double PageHeight = 841.8898;

        private class OutlineRect
        {
            public double Width, X, Y, RectWidth, RectHeight;
            public string Stroke;
            public string Dash;
        }

        private static string Render(string style, object model = null)
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><body style='margin:0;padding:0'><div style='margin:20pt;width:100pt;height:50pt;color:#00FF00;" + style + "'>x</div></body></html>";
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
        /// Finds the last stroked rectangle in the content (after the text), which is the outline
        /// </summary>
        private static OutlineRect Outline(string pdf)
        {
            var start = pdf.IndexOf(" ET Q", StringComparison.Ordinal);
            Assert.IsTrue(start > 0, "No content found");
            var content = pdf.Substring(start);
            var matches = Regex.Matches(content, @"q\s+(?<w>[\-0-9\.]+) w (?<rg>[0-9\. ]+) RG\s+(?<dash>\[[^\]]*\]\s+\d+ d\s+)?(?<x>[\-0-9\.]+)\s+(?<y>[\-0-9\.]+)\s+(?<rw>[\-0-9\.]+)\s+(?<rh>[\-0-9\.]+)\s+re S Q");
            if (matches.Count == 0)
                return null;

            var m = matches[matches.Count - 1];
            var inv = System.Globalization.CultureInfo.InvariantCulture;
            return new OutlineRect()
            {
                Width = double.Parse(m.Groups["w"].Value, inv),
                Stroke = m.Groups["rg"].Value.Trim(),
                Dash = m.Groups["dash"].Success ? m.Groups["dash"].Value.Trim() : null,
                X = double.Parse(m.Groups["x"].Value, inv),
                Y = double.Parse(m.Groups["y"].Value, inv),
                RectWidth = double.Parse(m.Groups["rw"].Value, inv),
                RectHeight = double.Parse(m.Groups["rh"].Value, inv)
            };
        }

        [TestMethod]
        public void NoOutline_ByDefault()
        {
            Assert.IsNull(Outline(Render("")));
        }

        [TestMethod]
        public void Solid_IsDrawnOutsideTheBorderEdge()
        {
            var o = Outline(Render("outline: 4pt solid red"));
            Assert.IsNotNull(o);
            Assert.AreEqual(4.0, o.Width, 0.01);
            Assert.AreEqual("1.0000 0.0000 0.0000", o.Stroke);
            //the stroke is centered 2pt out from the edge (so it fills the 4pt band outside the box)
            Assert.AreEqual(18.0, o.X, 0.01);
            Assert.AreEqual(104.0, o.RectWidth, 0.01);
            Assert.AreEqual(PageHeight - 20.0 + 2.0, o.Y, 0.01);
            Assert.AreEqual(-54.0, o.RectHeight, 0.01);
            Assert.IsNull(o.Dash);
        }

        [TestMethod]
        public void Offset_MovesTheOutlineOut()
        {
            var o = Outline(Render("outline: 4pt solid red; outline-offset: 6pt"));
            Assert.AreEqual(12.0, o.X, 0.01);
            Assert.AreEqual(116.0, o.RectWidth, 0.01);
        }

        [TestMethod]
        public void Offset_Negative_DrawsInsideTheBox()
        {
            var o = Outline(Render("outline: 4pt solid red; outline-offset: -10pt"));
            //centered at 10pt inside the edge (20 + 10 = 30) less half the width: 20 + 10 - 2 + ... the stroke center is at edge + 10 - 2
            Assert.AreEqual(20.0 + 10.0 - 2.0, o.X, 0.01);
            Assert.AreEqual(100.0 - 2 * (10.0 - 2.0), o.RectWidth, 0.01);
        }

        [TestMethod]
        public void Border_OutlineStartsAfterTheBorder()
        {
            var o = Outline(Render("border: 2pt solid black; outline: 4pt solid blue"));
            //the border is centered on the edge (19 to 21) so the outline band is 15 to 19, centered at 17
            Assert.AreEqual(17.0, o.X, 0.01);
            Assert.AreEqual(106.0, o.RectWidth, 0.01);
            Assert.AreEqual("0.0000 0.0000 1.0000", o.Stroke);
        }

        [TestMethod]
        public void Dashed_AndDotted_HaveADashPattern()
        {
            Assert.IsNotNull(Outline(Render("outline: 2pt dashed red")).Dash);
            Assert.IsNotNull(Outline(Render("outline: 2pt dotted red")).Dash);
            Assert.AreNotEqual(Outline(Render("outline: 2pt dashed red")).Dash, Outline(Render("outline: 2pt dotted red")).Dash);
        }

        [TestMethod]
        public void Shorthand_AnyOrder()
        {
            foreach (var value in new[] { "2pt solid red", "red solid 2pt", "solid 2pt red", "red 2pt solid" })
            {
                var o = Outline(Render("outline: " + value));
                Assert.IsNotNull(o, value);
                Assert.AreEqual(2.0, o.Width, 0.01, value);
                Assert.AreEqual("1.0000 0.0000 0.0000", o.Stroke, value);
            }
        }

        [TestMethod]
        public void Longhands_Combine()
        {
            var o = Outline(Render("outline-style: solid; outline-width: 3pt; outline-color: #0000FF; outline-offset: 2pt"));
            Assert.AreEqual(3.0, o.Width, 0.01);
            Assert.AreEqual("0.0000 0.0000 1.0000", o.Stroke);
            Assert.AreEqual(20.0 - 2.0 - 1.5, o.X, 0.01);
        }

        [TestMethod]
        public void StyleOnly_DefaultsToMediumWidth_AndTheTextColor()
        {
            var o = Outline(Render("outline-style: solid"));
            Assert.AreEqual(2.25, o.Width, 0.01, "medium is 3px");
            Assert.AreEqual("0.0000 1.0000 0.0000", o.Stroke, "The text color");
        }

        [TestMethod]
        public void CurrentColor_IsTheTextColor()
        {
            var o = Outline(Render("outline: 2pt solid red; outline-color: currentcolor"));
            Assert.AreEqual("0.0000 1.0000 0.0000", o.Stroke);
        }

        [TestMethod]
        public void WidthKeywords()
        {
            Assert.AreEqual(0.75, Outline(Render("outline: thin solid red")).Width, 0.01);
            Assert.AreEqual(2.25, Outline(Render("outline: medium solid red")).Width, 0.01);
            Assert.AreEqual(3.75, Outline(Render("outline: thick solid red")).Width, 0.01);
        }

        [TestMethod]
        public void NoneAndZeroWidth_DrawNothing()
        {
            Assert.IsNull(Outline(Render("outline: 4pt none red")));
            Assert.IsNull(Outline(Render("outline: 0 solid red")));
            Assert.IsNull(Outline(Render("outline: 4pt solid red; outline-style: none")));
        }

        [TestMethod]
        public void Auto_IsSolid()
        {
            Assert.IsNotNull(Outline(Render("outline: 2pt auto red")));
        }

        [TestMethod]
        public void Expressions()
        {
            var o = Outline(Render("outline: 2pt solid {{model.color}}; outline-offset: {{model.offset}}", new { color = "#0000FF", offset = "4pt" }));
            Assert.AreEqual("0.0000 0.0000 1.0000", o.Stroke);
            Assert.AreEqual(20.0 - 4.0 - 1.0, o.X, 0.01);
        }

        [TestMethod]
        public void InvalidValues_AreIgnored()
        {
            Assert.IsNull(Outline(Render("outline: 4pt wibble red")));
        }
    }
}
