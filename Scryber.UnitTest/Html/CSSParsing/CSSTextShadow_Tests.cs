using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Html.CSSParsing
{
    /// <summary>
    /// Tests the text-shadow css property. Each shadow is the text drawn again, behind it, in its own text object.
    /// </summary>
    [TestClass]
    public class CSSTextShadow_Tests
    {
        private static string Render(string style, string text = "Shadowed", object model = null, string css = "")
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>" + css + "</style></head><body style='margin:0;padding:0'><p style='margin:0;" + style + "'>" + text + "</p></body></html>";
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

        private class TextObject
        {
            public string Color;
            public double X, Y;
            public int Strings;
            public bool HasGs;
        }

        private static TextObject[] TextObjects(string pdf)
        {
            var inv = System.Globalization.CultureInfo.InvariantCulture;
            var list = new System.Collections.Generic.List<TextObject>();
            foreach (Match m in Regex.Matches(pdf, @"BT(?<body>.*?)ET", RegexOptions.Singleline))
            {
                var body = m.Groups["body"].Value;
                var color = Regex.Match(body, @"(?<c>[0-9\.]+ [0-9\.]+ [0-9\.]+) rg");
                var td = Regex.Match(body, @"(?<x>[\-0-9\.]+)\s+(?<y>[\-0-9\.]+)\s+Td");
                var obj = new TextObject()
                {
                    Color = color.Success ? color.Groups["c"].Value : null,
                    X = td.Success ? double.Parse(td.Groups["x"].Value, inv) : 0,
                    Y = td.Success ? double.Parse(td.Groups["y"].Value, inv) : 0,
                    Strings = Regex.Matches(body, "Tj").Count
                };
                list.Add(obj);
            }
            return list.ToArray();
        }

        [TestMethod]
        public void NoShadow_OneTextObject()
        {
            Assert.AreEqual(1, TextObjects(Render("")).Length);
        }

        [TestMethod]
        public void Sharp_TextDrawnAgainFirst_Offset()
        {
            var o = TextObjects(Render("color: #0000FF; text-shadow: 3pt 5pt 0 red"));
            Assert.AreEqual(2, o.Length);
            //the shadow is first (behind), then the text
            Assert.AreEqual("1.0000 0.0000 0.0000", o[0].Color);
            Assert.AreEqual("0.0000 0.0000 1.0000", o[1].Color);
            Assert.AreEqual(o[1].X + 3.0, o[0].X, 0.01);
            Assert.AreEqual(o[1].Y - 5.0, o[0].Y, 0.01, "Down the page is lower in the pdf");
            Assert.AreEqual(o[1].Strings, o[0].Strings);
        }

        [TestMethod]
        public void DefaultColor_IsTheTextColor()
        {
            var o = TextObjects(Render("color: #00FF00; text-shadow: 2pt 2pt 0"));
            Assert.AreEqual("0.0000 1.0000 0.0000", o[0].Color);
        }

        [TestMethod]
        public void ColorFirst_AndRgbaOpacity()
        {
            var pdf = Render("text-shadow: rgba(255,0,0,0.5) 2pt 2pt");
            var o = TextObjects(pdf);
            Assert.AreEqual("1.0000 0.0000 0.0000", o[0].Color);
            Assert.IsTrue(pdf.Contains(" gs"), "The opacity is set");
        }

        [TestMethod]
        public void MultipleShadows_FirstIsOnTop()
        {
            var o = TextObjects(Render("text-shadow: 2pt 2pt blue, 5pt 5pt red"));
            Assert.AreEqual(3, o.Length);
            Assert.AreEqual("1.0000 0.0000 0.0000", o[0].Color, "The last listed is drawn first");
            Assert.AreEqual("0.0000 0.0000 1.0000", o[1].Color);
        }

        [TestMethod]
        public void Blur_DrawsRingOfCopies()
        {
            var o = TextObjects(Render("text-shadow: 0 0 6pt red"));
            Assert.AreEqual(10, o.Length, "9 copies for the blur and the text");
        }

        [TestMethod]
        public void MultipleLines_ShadowHasTheSameLines()
        {
            var text = string.Join(" ", Enumerable.Repeat("wrapping words that need several lines", 6));
            var o = TextObjects(Render("width: 150pt; text-shadow: 2pt 2pt 0 red", text));
            Assert.AreEqual(2, o.Length);
            Assert.IsTrue(o[1].Strings > 2, "The text wraps onto several lines");
            Assert.AreEqual(o[1].Strings, o[0].Strings, "The shadow has all the lines");
        }

        [TestMethod]
        public void Inherited_AndNoneOverrides()
        {
            var pdf = Render("", "Outer <span class='n'>inner</span>", null, "p { text-shadow: 2pt 2pt 0 red; } .n { text-shadow: none; }");
            var o = TextObjects(pdf);
            //the outer text has a shadow (2 objects), the inner span has none (1 object)
            Assert.AreEqual(3, o.Length);
        }

        [TestMethod]
        public void Hidden_Text_HasNoShadow()
        {
            //hidden text is not painted, so there is no shadow either (and a hidden block is not drawn at all)
            var o = TextObjects(Render("", "<span style='visibility: hidden; text-shadow: 2pt 2pt 0 red'>Hidden</span>"));
            Assert.AreEqual(1, o.Length);
            Assert.AreEqual(0, TextObjects(Render("visibility: hidden; text-shadow: 2pt 2pt 0 red")).Length);
        }

        [TestMethod]
        public void None_AndInvalid_NoShadow()
        {
            Assert.AreEqual(1, TextObjects(Render("text-shadow: none")).Length);
            Assert.AreEqual(1, TextObjects(Render("text-shadow: wobble")).Length);
            Assert.AreEqual(1, TextObjects(Render("text-shadow: 2pt red")).Length, "Two lengths are needed");
            Assert.AreEqual(1, TextObjects(Render("text-shadow: 1pt 2pt 3pt 4pt red")).Length, "Text shadows have no spread");
        }

        [TestMethod]
        public void Expression()
        {
            var o = TextObjects(Render("text-shadow: {{model.s}}", "Bound", new { s = "2pt 2pt 0 #0000FF" }));
            Assert.AreEqual(2, o.Length);
            Assert.AreEqual("0.0000 0.0000 1.0000", o[0].Color);
        }

        [TestMethod]
        public void BoxShadow_DoesNotAffectText()
        {
            Assert.AreEqual(1, TextObjects(Render("box-shadow: 3pt 3pt 0 red")).Length);
        }
    }
}
