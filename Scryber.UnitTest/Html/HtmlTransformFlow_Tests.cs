using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF.Layout;

namespace Scryber.Core.UnitTests.Html
{
    /// <summary>
    /// A css transform does not change the layout of the page. The component keeps its space, and is drawn
    /// transformed around its transform-origin (the centre of its border box by default), as in a browser.
    /// Each test compares the transformed document with the same document without the transform.
    /// </summary>
    [TestClass]
    public class HtmlTransformFlow_Tests
    {
        private class Generated
        {
            public Document Document;
            public PDFLayoutDocument Layout;

            public Rect Bounds(string id)
            {
                var comp = this.Document.FindAComponentById(id) as Component;
                Assert.IsNotNull(comp, "The component " + id + " was not found");
                var arrange = comp.GetFirstArrangement();
                Assert.IsNotNull(arrange, "The component " + id + " was not arranged");
                return arrange.RenderBounds;
            }

            public PDFLayoutBlock Block(string id)
            {
                var block = FindBlock(this.Layout.AllPages[0].ContentBlock, id);
                Assert.IsNotNull(block, "No layout block was found for " + id);
                return block;
            }
        }

        private static Generated Generate(string body)
        {
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>.box{background-color:#ddf;border:1pt solid blue;}</style></head>" +
                       "<body style='padding:20pt;font-size:12pt'>" + body + "</body></html>";

            var result = new Generated();
            result.Document = Document.ParseDocument(new StringReader(html), ParseSourceType.DynamicContent);
            result.Document.LayoutComplete += (sender, args) => result.Layout = args.Context.GetLayout<PDFLayoutDocument>();

            using (var ms = new MemoryStream())
                result.Document.SaveAsPDF(ms);

            Assert.IsNotNull(result.Layout);
            return result;
        }

        private static PDFLayoutBlock FindBlock(PDFLayoutBlock block, string id)
        {
            if (block.Owner != null && block.Owner.ID == id)
                return block;

            foreach (var region in block.Columns)
            {
                var found = FindBlock(region, id);
                if (null != found)
                    return found;
            }

            if (null != block.PositionedRegions)
            {
                foreach (var region in block.PositionedRegions)
                {
                    var found = FindBlock(region, id);
                    if (null != found)
                        return found;
                }
            }
            return null;
        }

        private static PDFLayoutBlock FindBlock(PDFLayoutRegion region, string id)
        {
            foreach (var item in region.Contents)
            {
                PDFLayoutBlock found = null;
                if (item is PDFLayoutBlock inner)
                    found = FindBlock(inner, id);
                else if (item is PDFLayoutLine line)
                {
                    foreach (var run in line.Runs)
                    {
                        if (run is PDFLayoutPositionedRegionRun posRun && null != posRun.Region)
                        {
                            found = FindBlock(posRun.Region, id);
                            if (null != found)
                                break;
                        }
                    }
                }

                if (null != found)
                    return found;
            }
            return null;
        }

        private static void AssertOrigin(PDFLayoutBlock block, Unit x, Unit y, string message)
        {
#if DEBUG
            Assert.IsNotNull(block.RenderOrigin, "There is no render origin for the block - " + message);
            Assert.AreEqual(Math.Round(x.PointsValue, 2), Math.Round(block.RenderOrigin.HorizontalOrigin.PointsValue, 2), "The horizontal origin - " + message);
            Assert.AreEqual(Math.Round(y.PointsValue, 2), Math.Round(block.RenderOrigin.VerticalOrigin.PointsValue, 2), "The vertical origin - " + message);
#else
            Assert.Inconclusive("The render origin is only captured in the debug configuration");
#endif
        }

        private static void AssertCentre(PDFLayoutBlock block, Rect box, string message)
        {
            AssertOrigin(block, box.X + (box.Width / 2), box.Y + (box.Height / 2), message);
        }

        /// <summary>
        /// Checks the matrix used to render the block maps points on the page in the same way as the expected matrix
        /// </summary>
        private static void AssertSameMatrix(PDFLayoutBlock expected, PDFLayoutBlock actual, string message)
        {
#if DEBUG
            Assert.IsNotNull(expected.RenderMatrix, "There is no expected render matrix - " + message);
            Assert.IsNotNull(actual.RenderMatrix, "There is no render matrix for the block - " + message);

            foreach (var pt in new[] { new Point(0, 0), new Point(100, 0), new Point(0, 100), new Point(250, 400) })
            {
                var e = expected.RenderMatrix.TransformPoint(pt);
                var a = actual.RenderMatrix.TransformPoint(pt);
                Assert.AreEqual(Math.Round(e.X.PointsValue, 2), Math.Round(a.X.PointsValue, 2), "The x of " + pt + " - " + message);
                Assert.AreEqual(Math.Round(e.Y.PointsValue, 2), Math.Round(a.Y.PointsValue, 2), "The y of " + pt + " - " + message);
            }
#else
            Assert.Inconclusive("The render matrix is only captured in the debug configuration");
#endif
        }

        private const string BlockBody ="<p id='before'>Before</p><div id='box' class='box' style='width:200pt;height:40pt;{0}'>Rotated block</div><p id='after'>After</p>";

        [TestMethod]
        public void TransformFlow_01_Block_KeepsItsSpaceInTheFlow()
        {
            var plain = Generate(string.Format(BlockBody, ""));
            var rotated = Generate(string.Format(BlockBody, "transform:rotate(-90deg);"));

            Assert.AreEqual(plain.Bounds("after").Y, rotated.Bounds("after").Y, "The content after the block is in the same place");
            Assert.AreEqual(plain.Bounds("after").X, rotated.Bounds("after").X);

            var block = rotated.Block("box");
            Assert.AreEqual(PositionMode.Relative, block.Position.PositionMode, "A static block with a transform is relative, and stays in the flow");
        }

        [TestMethod]
        public void TransformFlow_02_Block_RotatesAroundTheCentreOfItsBox()
        {
            var plain = Generate(string.Format(BlockBody, ""));
            var box = plain.Bounds("box");

            foreach (var rotate in new[] { "-90deg", "90deg", "-45deg", "28deg" })
            {
                var rotated = Generate(string.Format(BlockBody, "transform:rotate(" + rotate + ");"));
                AssertCentre(rotated.Block("box"), box, "rotate(" + rotate + ")");
            }
        }

        [TestMethod]
        public void TransformFlow_03_InlineBlock_KeepsItsSpaceInTheLine()
        {
            const string body = "<p>Label <span id='side' class='box' style='display:inline-block;{0}'>Side</span> <span id='next' style='display:inline-block'>text after</span></p>";

            var plain = Generate(string.Format(body, ""));
            var rotated = Generate(string.Format(body, "transform:rotate(-90deg);"));

            Assert.AreEqual(plain.Bounds("next").X, rotated.Bounds("next").X, "The content after the inline-block is in the same place on the line");
            Assert.AreEqual(plain.Bounds("next").Y, rotated.Bounds("next").Y);

            AssertCentre(rotated.Block("side"), plain.Bounds("side"), "the inline-block");
        }

        [TestMethod]
        public void TransformFlow_04_Relative_IsMovedThenRotatedAroundItsMovedCentre()
        {
            const string body = "<p id='before'>Before</p><div id='box' class='box' style='position:relative;left:30pt;top:10pt;width:200pt;height:40pt;{0}'>Moved</div><p id='after'>After</p>";

            var plain = Generate(string.Format(body, ""));
            var rotated = Generate(string.Format(body, "transform:rotate(-90deg);"));

            Assert.AreEqual(plain.Bounds("after").Y, rotated.Bounds("after").Y, "The relative block keeps its space");

            var box = plain.Bounds("box");
            var unmoved = Generate(string.Format(BlockBody, "")).Bounds("box");
            Assert.AreEqual(unmoved.X + 30, box.X, "The relative left is applied");

            AssertCentre(rotated.Block("box"), box, "the left and top of the relative block are applied before the rotation");
        }

        [TestMethod]
        public void TransformFlow_05_TransformOrigin_IsWithinTheBoxOfTheComponent()
        {
            var box = Generate(string.Format(BlockBody, "")).Bounds("box");

            //percentages are of the component's own box, not its container
            var origins = new[]
            {
                ("0% 0%", box.X, box.Y),
                ("left top", box.X, box.Y),
                ("100% 100%", box.X + box.Width, box.Y + box.Height),
                ("right bottom", box.X + box.Width, box.Y + box.Height),
                ("25% 75%", box.X + (box.Width * 0.25), box.Y + (box.Height * 0.75)),
                ("10pt 20pt", box.X + 10, box.Y + 20),
            };

            foreach (var (origin, x, y) in origins)
            {
                var rotated = Generate(string.Format(BlockBody, "transform:rotate(-90deg); transform-origin:" + origin + ";"));
                AssertOrigin(rotated.Block("box"), x, y, "transform-origin: " + origin);
            }
        }

        [TestMethod]
        public void TransformFlow_06_Absolute_StaysAbsoluteAroundItsCentre()
        {
            const string body = "<div id='box' class='box' style='position:absolute;left:100pt;top:200pt;width:200pt;height:40pt;{0}'>Absolute</div><p id='after'>Flow text</p>";

            var plain = Generate(string.Format(body, ""));
            var rotated = Generate(string.Format(body, "transform:rotate(-90deg);"));

            var block = rotated.Block("box");
            Assert.AreEqual(PositionMode.Absolute, block.Position.PositionMode);
            Assert.AreEqual(plain.Bounds("after").Y, rotated.Bounds("after").Y);

            AssertCentre(block, plain.Bounds("box"), "the absolute block");
        }

        [TestMethod]
        public void TransformFlow_07_Rendering_DoesNotChangeTheTransformOrigin()
        {
            var rotated = Generate(string.Format(BlockBody, "transform:rotate(-90deg); transform-origin:10pt 20pt;"));
            var origin = rotated.Block("box").Position.TransformationOrigin;

            Assert.IsNotNull(origin);
            Assert.AreEqual(10.0, origin.HorizontalOrigin.PointsValue, "The origin of the position is not moved by the page offset when rendered");
            Assert.AreEqual(20.0, origin.VerticalOrigin.PointsValue);
        }

        [TestMethod]
        public void TransformFlow_08_InATableCell_IsDrawnAroundItsCentre()
        {
            const string body = "<table style='width:100%'><tr><th style='height:100pt;text-align:center'><div id='label' style='{0}'>Vertical header</div></th><td id='cell'>Cell</td></tr></table>";

            var plain = Generate(string.Format(body, ""));
            var rotated = Generate(string.Format(body, "transform:rotate(-90deg);"));

            var block = rotated.Block("label");
            Assert.AreEqual(PositionMode.Relative, block.Position.PositionMode, "The label stays in the cell, rather than being taken out of the table");
            Assert.AreEqual(plain.Bounds("cell").X, rotated.Bounds("cell").X);

            AssertCentre(block, plain.Bounds("label"), "the label in the cell");
        }

        [TestMethod]
        public void TransformFlow_09_TranslatePercentages_AreOfTheBoxOfTheComponent()
        {
            //the box is 200pt x 40pt, so -50% -50% is -100pt -20pt (not half of the page width)
            var transforms = new[]
            {
                ("translate(-50%, -50%)", "translate(-100pt, -20pt)"),
                ("translateX(25%)", "translateX(50pt)"),
                ("translateY(100%)", "translateY(40pt)"),
                ("translate(-50%, -50%) rotate(-45deg)", "translate(-100pt, -20pt) rotate(-45deg)"),
                ("rotate(30deg) translate(50%, 10pt)", "rotate(30deg) translate(100pt, 10pt)"),
                ("scale(2) translate(10%, 50%) rotate(10deg)", "scale(2) translate(20pt, 20pt) rotate(10deg)"),
            };

            foreach (var (percent, points) in transforms)
            {
                var expected = Generate(string.Format(BlockBody, "transform:" + points + ";")).Block("box");
                var actual = Generate(string.Format(BlockBody, "transform:" + percent + ";")).Block("box");
                AssertSameMatrix(expected, actual, percent);
            }
        }

        [TestMethod]
        public void TransformFlow_11_Translate_AfterAnotherOperation_IsAlongTheTransformedAxes()
        {
            //as css, each operation is within the ones before it, so a translate after a rotate (or skew or scale)
            //moves along the rotated axes - rotating 90deg turns the x axis down the page.
            var same = new[]
            {
                ("rotate(90deg) translate(50pt, 0)", "translate(0, 50pt) rotate(90deg)"),
                ("rotate(-90deg) translate(50pt, 10pt)", "translate(10pt, -50pt) rotate(-90deg)"),
                ("scale(2) translate(10pt, 20pt)", "translate(20pt, 40pt) scale(2)"),
                ("rotate(180deg) translate(25%, 0)", "translate(-50pt, 0) rotate(180deg)"),
            };

            foreach (var (one, two) in same)
            {
                var expected = Generate(string.Format(BlockBody, "transform:" + two + ";")).Block("box");
                var actual = Generate(string.Format(BlockBody, "transform:" + one + ";")).Block("box");
                AssertSameMatrix(expected, actual, one + " is the same as " + two);
            }
        }

        [TestMethod]
        public void TransformFlow_10_TranslatePercentages_InASharedStyle_AreOfEachComponent()
        {
            //the same class on 2 boxes of a different size - each is moved by half its own width
            const string body = "<style>.moved{ transform: rotate(10deg) translate(50%, 0); }</style>" +
                                "<div id='wide' class='box moved' style='width:200pt;height:40pt'>Wide</div>" +
                                "<div id='narrow' class='box moved' style='width:100pt;height:40pt'>Narrow</div>";
            const string expectedBody = "<div id='wide' class='box' style='width:200pt;height:40pt;transform: rotate(10deg) translate(100pt, 0)'>Wide</div>" +
                                        "<div id='narrow' class='box' style='width:100pt;height:40pt;transform: rotate(10deg) translate(50pt, 0)'>Narrow</div>";

            var actual = Generate(body);
            var expected = Generate(expectedBody);

            AssertSameMatrix(expected.Block("wide"), actual.Block("wide"), "the wide box");
            AssertSameMatrix(expected.Block("narrow"), actual.Block("narrow"), "the narrow box");
        }
    }
}
