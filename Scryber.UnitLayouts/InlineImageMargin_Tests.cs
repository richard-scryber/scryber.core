using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// An inline image with a margin is drawn at its own width and height, inside the margin (not over it).
    /// </summary>
    [TestClass]
    public class InlineImageMargin_Tests
    {
        private static (double w, double h, double x, double y) Render(string imgStyle)
        {
            var png = DocStreams.AssertGetTemplatePath("Content/Images/Toroid32x9.png").Replace("\\", "/");
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><body style='margin:0;padding:0'><div>x <img src='" + png + "' style='width:60pt;height:30pt;" + imgStyle + "' /></div></body></html>";
            using (var doc = Document.ParseDocument(new System.IO.StringReader(html), ParseSourceType.DynamicContent))
            using (var ms = new System.IO.MemoryStream())
            {
                doc.RenderOptions.Compression = Scryber.OutputCompressionType.None;
                doc.SaveAsPDF(ms);
                var t = System.Text.Encoding.Latin1.GetString(ms.ToArray());
                var m = Regex.Match(t, @"(?<w>[\-0-9\.]+)\s+0\.0000\s+0\.0000\s+(?<h>[\-0-9\.]+)\s+(?<x>[\-0-9\.]+)\s+(?<y>[\-0-9\.]+)\s+cm");
                Assert.IsTrue(m.Success);
                var inv = System.Globalization.CultureInfo.InvariantCulture;
                return (double.Parse(m.Groups["w"].Value, inv), double.Parse(m.Groups["h"].Value, inv), double.Parse(m.Groups["x"].Value, inv), double.Parse(m.Groups["y"].Value, inv));
            }
        }

        [TestMethod]
        public void NoMargin_DrawnAtItsSize()
        {
            var r = Render("");
            Assert.AreEqual(60.0, r.w, 0.01);
            Assert.AreEqual(30.0, r.h, 0.01);
        }

        [TestMethod]
        public void Margin_DoesNotEnlargeTheImage_AndOffsetsIt()
        {
            var none = Render("");
            var margin = Render("margin: 6pt;");

            Assert.AreEqual(60.0, margin.w, 0.01, "The width is the image width, not width plus margins");
            Assert.AreEqual(30.0, margin.h, 0.01, "The height is the image height, not height plus margins");
            Assert.AreEqual(none.x + 6.0, margin.x, 0.01, "Starts after the left margin");
        }

        [TestMethod]
        public void Margin_WithBorder_ImageIsInsideTheBorder()
        {
            var margin = Render("margin: 6pt; border: solid 1pt red;");
            Assert.AreEqual(60.0, margin.w, 0.01);
            Assert.AreEqual(30.0, margin.h, 0.01);
        }

        [TestMethod]
        public void Margin_Inline_SameSizeAsBlock()
        {
            var inline = Render("margin: 6pt;");
            var block = Render("margin: 6pt; display: block;");
            Assert.AreEqual(block.w, inline.w, 0.01);
            Assert.AreEqual(block.h, inline.h, 0.01);
        }
    }
}
