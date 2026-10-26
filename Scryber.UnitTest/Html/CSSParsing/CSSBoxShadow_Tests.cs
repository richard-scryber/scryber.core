using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Html.CSSParsing
{
    /// <summary>
    /// Tests the box-shadow css property. The box is 100pt x 50pt with a 20pt margin, so its top left is at 20,20 on the page.
    /// </summary>
    [TestClass]
    public class CSSBoxShadow_Tests
    {
        private const double PageHeight = 841.8898;

        private static string Render(string style, object model = null, string extra = "")
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><body style='margin:0;padding:0'><div style='margin:20pt;width:100pt;height:50pt;" + style + "'>x</div>" + extra + "</body></html>";
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

        private class Fill
        {
            public string Color;
            public double X, Y, Width, Height;
            public bool Rounded;
        }

        /// <summary>
        /// The shadow fills - in the clipped (W* n) groups before the text
        /// </summary>
        private static Fill[] Fills(string pdf)
        {
            var textStart = pdf.IndexOf("BT", StringComparison.Ordinal);
            var content = textStart > 0 ? pdf.Substring(0, textStart) : pdf;
            var list = new System.Collections.Generic.List<Fill>();
            var inv = System.Globalization.CultureInfo.InvariantCulture;

            foreach (Match m in Regex.Matches(content, @"W\* n\s+(?<body>.*?)\bQ\s+Q", RegexOptions.Singleline))
            {
                var body = m.Groups["body"].Value;
                foreach (Match g in Regex.Matches(body, @"q\s+(?:/\w+ gs\s+)?(?<rg>[0-9\. ]+) rg\s+(?<path>.*?)\bf\b", RegexOptions.Singleline))
                {
                    var rect = Regex.Match(g.Groups["path"].Value, @"^\s*(?<x>[\-0-9\.]+)\s+(?<y>[\-0-9\.]+)\s+(?<w>[\-0-9\.]+)\s+(?<h>[\-0-9\.]+)\s+re");
                    var fill = new Fill() { Color = g.Groups["rg"].Value.Trim() };
                    if (rect.Success)
                    {
                        fill.X = double.Parse(rect.Groups["x"].Value, inv);
                        fill.Y = double.Parse(rect.Groups["y"].Value, inv);
                        fill.Width = double.Parse(rect.Groups["w"].Value, inv);
                        fill.Height = double.Parse(rect.Groups["h"].Value, inv);
                    }
                    else
                    {
                        fill.Rounded = true;
                        var mv = Regex.Match(g.Groups["path"].Value, @"^\s*(?<x>[\-0-9\.]+)\s+(?<y>[\-0-9\.]+)\s+m");
                        if (mv.Success)
                            fill.X = double.Parse(mv.Groups["x"].Value, inv);
                    }
                    list.Add(fill);
                }
            }
            return list.ToArray();
        }

        [TestMethod]
        public void NoShadow_ByDefault()
        {
            Assert.AreEqual(0, Fills(Render("")).Length);
        }

        [TestMethod]
        public void SharpOffset_IsTheBoxMoved()
        {
            var fills = Fills(Render("box-shadow: 4pt 6pt 0 red"));
            Assert.AreEqual(1, fills.Length);
            Assert.AreEqual("1.0000 0.0000 0.0000", fills[0].Color);
            Assert.AreEqual(24.0, fills[0].X, 0.01);
            Assert.AreEqual(100.0, fills[0].Width, 0.01);
            Assert.AreEqual(PageHeight - 20.0 - 6.0, fills[0].Y, 0.01);
            Assert.AreEqual(-50.0, fills[0].Height, 0.01);
        }

        [TestMethod]
        public void Shadow_IsClippedToOutsideTheBox()
        {
            var pdf = Render("box-shadow: 4pt 4pt 0 red");
            Assert.IsTrue(pdf.Contains("W* n"), "The shadow is clipped around the box");
        }

        [TestMethod]
        public void Spread_ExpandsAndContracts()
        {
            var grown = Fills(Render("box-shadow: 0 0 0 5pt red"))[0];
            Assert.AreEqual(15.0, grown.X, 0.01);
            Assert.AreEqual(110.0, grown.Width, 0.01);

            var shrunk = Fills(Render("box-shadow: 0 0 0 -5pt red"))[0];
            Assert.AreEqual(25.0, shrunk.X, 0.01);
            Assert.AreEqual(90.0, shrunk.Width, 0.01);
        }

        [TestMethod]
        public void Blur_UsesSeveralLayers_FadingOut()
        {
            var fills = Fills(Render("box-shadow: 0 0 10pt 0 black"));
            Assert.IsTrue(fills.Length >= 3, "Several layers: " + fills.Length);
            //the outermost is the largest, and each is smaller than the last
            for (int i = 1; i < fills.Length; i++)
                Assert.IsTrue(fills[i].Width < fills[i - 1].Width, "Each layer is smaller");
            Assert.AreEqual(100.0 + 10.0, fills[0].Width, 1.5, "The outer edge is half the blur beyond the box");
        }

        [TestMethod]
        public void Blur_Larger_HasMoreLayers_UpToAMaximum()
        {
            var small = Fills(Render("box-shadow: 0 0 4pt black")).Length;
            var large = Fills(Render("box-shadow: 0 0 40pt black")).Length;
            Assert.IsTrue(large > small);
            Assert.AreEqual(Scryber.PDF.Graphics.PDFGraphics.MaxShadowBlurLayers, large);
        }

        [TestMethod]
        public void Color_RgbaGivesAnOpacity()
        {
            var pdf = Render("box-shadow: 4pt 4pt 0 rgba(0,0,255,0.25)");
            Assert.IsTrue(pdf.Contains("0.0000 0.0000 1.0000 rg"));
            Assert.IsTrue(pdf.Contains(" gs"), "An opacity graphics state is used");
        }

        [TestMethod]
        public void Color_Default_IsTheTextColor()
        {
            var fills = Fills(Render("color: #00FF00; box-shadow: 3pt 3pt 0"));
            Assert.AreEqual("0.0000 1.0000 0.0000", fills[0].Color);
        }

        [TestMethod]
        public void Color_CanBeFirst()
        {
            var fills = Fills(Render("box-shadow: red 3pt 3pt"));
            Assert.AreEqual("1.0000 0.0000 0.0000", fills[0].Color);
            Assert.AreEqual(23.0, fills[0].X, 0.01);
        }

        [TestMethod]
        public void MultipleShadows_AreAllDrawn_FirstOnTop()
        {
            var fills = Fills(Render("box-shadow: 2pt 2pt blue, 5pt 5pt red"));
            Assert.AreEqual(2, fills.Length);
            //the last is drawn first, so the first is on top
            Assert.AreEqual("1.0000 0.0000 0.0000", fills[0].Color);
            Assert.AreEqual("0.0000 0.0000 1.0000", fills[1].Color);
        }

        [TestMethod]
        public void RoundedCorners_FollowTheBorderRadius()
        {
            var fills = Fills(Render("box-shadow: 4pt 4pt 0 red; border-radius: 10pt"));
            Assert.AreEqual(1, fills.Length);
            Assert.IsTrue(fills[0].Rounded, "A rounded path");
        }

        [TestMethod]
        public void Inset_IsNotDrawn()
        {
            Assert.AreEqual(0, Fills(Render("box-shadow: inset 4pt 4pt 0 red")).Length);
        }

        [TestMethod]
        public void None_AndInvalid_DrawNothing()
        {
            Assert.AreEqual(0, Fills(Render("box-shadow: none")).Length);
            Assert.AreEqual(0, Fills(Render("box-shadow: wobble")).Length);
            Assert.AreEqual(0, Fills(Render("box-shadow: 4pt red")).Length, "At least 2 lengths are needed");
            Assert.AreEqual(0, Fills(Render("box-shadow: 1pt 2pt -3pt red")).Length, "A blur cannot be negative");
        }

        [TestMethod]
        public void Expression()
        {
            var fills = Fills(Render("box-shadow: {{model.shadow}}", new { shadow = "5pt 5pt 0 #0000FF" }));
            Assert.AreEqual(1, fills.Length);
            Assert.AreEqual("0.0000 0.0000 1.0000", fills[0].Color);
        }

        [TestMethod]
        public void Shadow_IsDrawnBeforeTheBackground()
        {
            var pdf = Render("box-shadow: 4pt 4pt 0 red; background-color: #00FF00");
            var shadow = pdf.IndexOf("1.0000 0.0000 0.0000 rg", StringComparison.Ordinal);
            var background = pdf.IndexOf("0.0000 1.0000 0.0000 rg", StringComparison.Ordinal);
            Assert.IsTrue(shadow > 0 && background > shadow, "The shadow is behind the background");
        }
    }
}
