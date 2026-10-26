using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// In Scryber the width and height of a box include the padding by default (as border-box).
    /// An explicit box-sizing: content-box makes the width and height the content only, adding the padding and border.
    /// </summary>
    [TestClass]
    public class BoxSizing_Tests
    {
        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        private PDFLayoutBlock LayoutBox(string css, string body, object model = null)
        {
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>" + css + "</style></head><body style='margin:0; padding:0'>" + body + "</body></html>";
            _layout = null;

            using (var doc = Document.ParseDocument(new System.IO.StringReader(html), ParseSourceType.DynamicContent))
            {
                if (model != null)
                    doc.Params["model"] = model;
                using (var ms = new System.IO.MemoryStream())
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(_layout);
            var block = FindBlock(_layout.AllPages[0].ContentBlock, "box");
            Assert.IsNotNull(block, "The box block was not found");
            return block;
        }

        private static PDFLayoutBlock FindBlock(PDFLayoutBlock parent, string id)
        {
            foreach (var column in parent.Columns)
            {
                foreach (var item in column.Contents)
                {
                    if (item is PDFLayoutBlock block)
                    {
                        if (block.Owner != null && block.Owner.ID == id)
                            return block;
                        var inner = FindBlock(block, id);
                        if (null != inner)
                            return inner;
                    }
                }
            }
            return null;
        }

        private const string Body = "<div id='box' class='b'>Content in the box</div>";
        private const string Box = ".b { width: 200pt; height: 100pt; padding: 20pt; border: 5pt solid black; ";

        [TestMethod]
        public void Default_WidthAndHeightIncludePadding()
        {
            var b = LayoutBox(Box + "}", Body);
            Assert.AreEqual(200.0, b.TotalBounds.Width.PointsValue, 0.5);
            Assert.AreEqual(100.0, b.TotalBounds.Height.PointsValue, 0.5);
        }

        [TestMethod]
        public void BorderBox_Explicit_SameAsDefault()
        {
            var b = LayoutBox(Box + "box-sizing: border-box; }", Body);
            Assert.AreEqual(200.0, b.TotalBounds.Width.PointsValue, 0.5);
            Assert.AreEqual(100.0, b.TotalBounds.Height.PointsValue, 0.5);
        }

        [TestMethod]
        public void ContentBox_AddsPaddingAndBorderToWidthAndHeight()
        {
            var b = LayoutBox(Box + "box-sizing: content-box; }", Body);
            //200 + 20+20 padding + 5+5 border
            Assert.AreEqual(250.0, b.TotalBounds.Width.PointsValue, 0.5);
            Assert.AreEqual(150.0, b.TotalBounds.Height.PointsValue, 0.5);
        }

        [TestMethod]
        public void ContentBox_PaddingOnly()
        {
            var b = LayoutBox(".b { width: 200pt; padding: 20pt; box-sizing: content-box; }", Body);
            Assert.AreEqual(240.0, b.TotalBounds.Width.PointsValue, 0.5);
            //the content width is as specified
            Assert.AreEqual(200.0, b.Columns[0].TotalBounds.Width.PointsValue, 0.5);
        }

        [TestMethod]
        public void ContentBox_SingleSides()
        {
            var b = LayoutBox(".b { width: 200pt; padding-left: 30pt; border-right: 10pt solid black; box-sizing: content-box; }", Body);
            Assert.AreEqual(240.0, b.TotalBounds.Width.PointsValue, 0.5);
        }

        [TestMethod]
        public void ContentBox_Percentage_AddsToTheContainerPercentage()
        {
            var b = LayoutBox(".c { width: 400pt; } .b { width: 50%; padding: 20pt; box-sizing: content-box; }", "<div class='c'><div id='box' class='b'>Percent</div></div>");
            Assert.AreEqual(240.0, b.TotalBounds.Width.PointsValue, 0.5);
        }

        [TestMethod]
        public void ContentBox_MinWidth_AndMaxWidth()
        {
            var min = LayoutBox(".b { min-width: 200pt; padding: 20pt; box-sizing: content-box; }", "<div id='box' class='b' style='width:10pt'>x</div>");
            Assert.AreEqual(240.0, min.TotalBounds.Width.PointsValue, 0.5);

            var max = LayoutBox(".b { max-width: 200pt; padding: 20pt; box-sizing: content-box; }", "<div id='box' class='b'>This is a long line of text that would be wider than two hundred points if it were not constrained by the maximum width</div>");
            Assert.IsTrue(max.TotalBounds.Width.PointsValue > 200.0 && max.TotalBounds.Width.PointsValue <= 240.5, "The max-width of 200pt content plus 40pt padding");
        }

        [TestMethod]
        public void ContentBox_AutoWidth_Unchanged()
        {
            var b = LayoutBox(".c { width: 300pt; } .b { padding: 20pt; box-sizing: content-box; }", "<div class='c'><div id='box' class='b'>Auto</div></div>");
            Assert.AreEqual(300.0, b.TotalBounds.Width.PointsValue, 0.5);
        }

        [TestMethod]
        public void ContentBox_NoPaddingOrBorder_Unchanged()
        {
            var b = LayoutBox(".b { width: 200pt; box-sizing: content-box; }", Body);
            Assert.AreEqual(200.0, b.TotalBounds.Width.PointsValue, 0.5);
        }

        [TestMethod]
        public void ContentBox_UniversalSelector_AppliesToAllElements()
        {
            var b = LayoutBox("* { box-sizing: content-box; } .b { width: 200pt; padding: 20pt; }", Body);
            Assert.AreEqual(240.0, b.TotalBounds.Width.PointsValue, 0.5);
        }

        [TestMethod]
        public void ContentBox_FromExpression()
        {
            var inline = "<div id='box' style='width: 200pt; padding: 20pt; box-sizing: {{model.sizing}};'>Content</div>";
            var b = LayoutBox("", inline, new { sizing = "content-box" });
            Assert.AreEqual(240.0, b.TotalBounds.Width.PointsValue, 0.5);
            b = LayoutBox("", inline, new { sizing = "border-box" });
            Assert.AreEqual(200.0, b.TotalBounds.Width.PointsValue, 0.5);
        }

        [TestMethod]
        public void ContentBox_NotInherited()
        {
            var b = LayoutBox(".o { width: 400pt; box-sizing: content-box; } .b { width: 100pt; padding: 20pt; }", "<div class='o'><div id='box' class='b'>Inner</div></div>");
            //the inner has the default, so the padding is inside the width
            Assert.AreEqual(100.0, b.TotalBounds.Width.PointsValue, 0.5);
        }
    }
}
