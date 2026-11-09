using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Html.CSSParsing
{
    /// <summary>
    /// Tests the object-fit and object-position of raster images. The image (Toroid24.png) is 511.5pt x 339pt naturally.
    /// </summary>
    [TestClass]
    public class CSSObjectFit_Tests
    {
        private const double NaturalWidth = 511.5;
        private const double NaturalHeight = 339.0;
        private const double PageHeight = 841.8898;

        private class ImageRender
        {
            public double Width, Height, X, Y;
            public bool Clipped;
        }

        private static ImageRender Render(string boxStyle, object model = null)
        {
            var png = DocStreams.AssertGetTemplatePath("HTML/Images/Toroid24.png").Replace("\\", "/");
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><body style='margin:0;padding:0'><img src='" + png + "' style='" + boxStyle + "' /></body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                if (model != null)
                    doc.Params["model"] = model;
                doc.RenderOptions.Compression = Scryber.OutputCompressionType.None;
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                var text = System.Text.Encoding.Latin1.GetString(ms.ToArray());

                var m = Regex.Match(text, @"(?<clip>[\-0-9\.]+\s+[\-0-9\.]+\s+[\-0-9\.]+\s+[\-0-9\.]+\s+re W n\s+)?(?<w>[\-0-9\.]+)\s+0\.0000\s+0\.0000\s+(?<h>[\-0-9\.]+)\s+(?<x>[\-0-9\.]+)\s+(?<y>[\-0-9\.]+)\s+cm");
                Assert.IsTrue(m.Success, "The image was not found in the output");
                var inv = System.Globalization.CultureInfo.InvariantCulture;
                return new ImageRender()
                {
                    Width = double.Parse(m.Groups["w"].Value, inv),
                    Height = double.Parse(m.Groups["h"].Value, inv),
                    X = double.Parse(m.Groups["x"].Value, inv),
                    Y = double.Parse(m.Groups["y"].Value, inv),
                    Clipped = m.Groups["clip"].Success
                };
            }
        }

        private const string Box = "width:200pt;height:100pt;";

        [TestMethod]
        public void Fill_IsTheDefault_StretchesToTheBox()
        {
            var r = Render(Box);
            Assert.AreEqual(200.0, r.Width, 0.01);
            Assert.AreEqual(100.0, r.Height, 0.01);
            Assert.AreEqual(0.0, r.X, 0.01);
            Assert.IsFalse(r.Clipped);

            var explicitFill = Render(Box + "object-fit: fill");
            Assert.AreEqual(200.0, explicitFill.Width, 0.01);
            Assert.AreEqual(100.0, explicitFill.Height, 0.01);
        }

        [TestMethod]
        public void Contain_FitsInsideKeepingTheRatio_Centered()
        {
            var r = Render(Box + "object-fit: contain");
            var expectedWidth = 100.0 * NaturalWidth / NaturalHeight;
            Assert.AreEqual(expectedWidth, r.Width, 0.05);
            Assert.AreEqual(100.0, r.Height, 0.01);
            Assert.AreEqual((200.0 - expectedWidth) / 2, r.X, 0.05);
            Assert.IsFalse(r.Clipped);
        }

        [TestMethod]
        public void Cover_FillsTheBoxKeepingTheRatio_AndClips()
        {
            var r = Render(Box + "object-fit: cover");
            var expectedHeight = 200.0 * NaturalHeight / NaturalWidth;
            Assert.AreEqual(200.0, r.Width, 0.05);
            Assert.AreEqual(expectedHeight, r.Height, 0.05);
            Assert.IsTrue(r.Clipped);
            //centered vertically, the bottom of the box is at PageHeight - 100
            Assert.AreEqual(PageHeight - 100.0 - (expectedHeight - 100.0) / 2, r.Y, 0.05);
        }

        [TestMethod]
        public void None_UsesTheNaturalSize_Centered_AndClips()
        {
            var r = Render(Box + "object-fit: none");
            Assert.AreEqual(NaturalWidth, r.Width, 0.05);
            Assert.AreEqual(NaturalHeight, r.Height, 0.05);
            Assert.AreEqual((200.0 - NaturalWidth) / 2, r.X, 0.05);
            Assert.IsTrue(r.Clipped);
        }

        [TestMethod]
        public void ScaleDown_ShrinksALargeImage_ButNeverEnlarges()
        {
            var large = Render(Box + "object-fit: scale-down");
            Assert.AreEqual(100.0 * NaturalWidth / NaturalHeight, large.Width, 0.05, "Larger than the box, so same as contain");

            var small = Render("width:700pt;height:500pt;object-fit: scale-down");
            Assert.AreEqual(NaturalWidth, small.Width, 0.05, "Smaller than the box, so the natural size");
            Assert.AreEqual(NaturalHeight, small.Height, 0.05);
            Assert.IsFalse(small.Clipped);
        }

        [TestMethod]
        public void Position_Keywords_Contain()
        {
            var expectedWidth = 100.0 * NaturalWidth / NaturalHeight;
            var left = Render(Box + "object-fit: contain; object-position: left top");
            Assert.AreEqual(0.0, left.X, 0.05);

            var right = Render(Box + "object-fit: contain; object-position: right bottom");
            Assert.AreEqual(200.0 - expectedWidth, right.X, 0.05);

            var swapped = Render(Box + "object-fit: contain; object-position: top right");
            Assert.AreEqual(200.0 - expectedWidth, swapped.X, 0.05, "keywords can be in either order");
        }

        [TestMethod]
        public void Position_Cover_TopAndBottom()
        {
            var expectedHeight = 200.0 * NaturalHeight / NaturalWidth;
            var top = Render(Box + "object-fit: cover; object-position: top");
            //single vertical keyword: top aligned, so the top of the image is the top of the box
            Assert.AreEqual(PageHeight - expectedHeight, top.Y, 0.05);

            var bottom = Render(Box + "object-fit: cover; object-position: center bottom");
            Assert.AreEqual(PageHeight - 100.0, bottom.Y, 0.05);
        }

        [TestMethod]
        public void Position_PercentagesAndLengths()
        {
            var expectedWidth = 100.0 * NaturalWidth / NaturalHeight;
            var pct = Render(Box + "object-fit: contain; object-position: 25% 50%");
            Assert.AreEqual((200.0 - expectedWidth) * 0.25, pct.X, 0.05);

            var len = Render(Box + "object-fit: contain; object-position: 10pt 5pt");
            Assert.AreEqual(10.0, len.X, 0.05);
        }

        [TestMethod]
        public void ObjectFit_FromExpression()
        {
            var r = Render(Box + "object-fit: {{model.fit}}", new { fit = "contain" });
            Assert.AreEqual(100.0 * NaturalWidth / NaturalHeight, r.Width, 0.05);
        }

        [TestMethod]
        public void ObjectFit_Invalid_IsIgnored()
        {
            var r = Render(Box + "object-fit: sideways");
            Assert.AreEqual(200.0, r.Width, 0.01);
            Assert.AreEqual(100.0, r.Height, 0.01);
        }
    }
}
