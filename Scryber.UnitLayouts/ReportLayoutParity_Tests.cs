using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Layouts that render the same as a browser, found in a designer built report (SASP Contour):
    /// flex items with block content are their content width, justify-content with a padded parent, margins and a gap,
    /// and absolute blocks with a left and a right but no width are stretched between them.
    /// </summary>
    [TestClass]
    public class ReportLayoutParity_Tests
    {
        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        private void Layout(string body, string css = "")
        {
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>@page{size:500pt 400pt;margin:0} body{margin:0;font-size:10pt} " + css + "</style></head><body>" + body + "</body></html>";
            using (var doc = Document.ParseDocument(new System.IO.StringReader(html), ParseSourceType.DynamicContent))
            using (var ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
        }

        private PDFLayoutBlock Block(string id)
        {
            var found = Find(id, _layout.AllPages[0].ContentBlock);
            Assert.IsNotNull(found, "No block for '" + id + "'");
            return found;
        }

        private static PDFLayoutBlock Find(string id, PDFLayoutBlock parent)
        {
            foreach (var column in parent.Columns)
            {
                var inner = Find(id, column);
                if (null != inner)
                    return inner;
            }

            if (parent.HasPositionedRegions)
            {
                foreach (var region in parent.PositionedRegions)
                {
                    var inner = Find(id, region);
                    if (null != inner)
                        return inner;
                }
            }
            return null;
        }

        private static PDFLayoutBlock Find(string id, PDFLayoutRegion region)
        {
            foreach (var item in region.Contents)
            {
                if (item is PDFLayoutBlock block)
                {
                    if (block.Owner != null && block.Owner.ID == id)
                        return block;
                    var inner = Find(id, block);
                    if (null != inner)
                        return inner;
                }
            }
            return null;
        }

        private double Width(string id)
        {
            return Block(id).TotalBounds.Width.PointsValue;
        }

        private double Left(string id)
        {
            return Block(id).PagePosition.X.PointsValue;
        }

        private double Right(string id)
        {
            var block = Block(id);
            return block.PagePosition.X.PointsValue + block.TotalBounds.Width.PointsValue;
        }

        [TestMethod]
        public void ReportLayout_01_FlexItem_WithABlockChild_IsItsContentWidth()
        {
            Layout("<div style='display:flex'><div id='a'><div>Overall</div></div><div id='b'>end</div></div>");

            Assert.IsTrue(Width("a") < 60, "The item is the width of its block child's text, not the row: " + Width("a"));
        }

        [TestMethod]
        public void ReportLayout_02_FlexItem_ThatIsAFlexColumn_IsItsContentWidth()
        {
            Layout("<div style='display:flex'><div id='a'>text</div><div id='b' style='display:flex;flex-direction:column'><span>2</span><span>photos</span></div></div>");

            Assert.IsTrue(Width("b") < 60, "The flex column is the width of its content, not the row: " + Width("b"));
        }

        [TestMethod]
        public void ReportLayout_03_FlexItem_WithAnExplicitWidthChild_IsThatWidth()
        {
            Layout("<div style='display:flex'><div id='a'><div>label</div><div style='width:315pt;height:10pt'></div></div><div id='b'>end</div></div>");

            Assert.AreEqual(315.0, Width("a"), 1.0, "The widest child");
        }

        [TestMethod]
        public void ReportLayout_04_SpaceBetween_InAPaddedParent_IsOnTheRightEdge()
        {
            Layout("<div style='padding:0 50pt'><div style='display:flex;justify-content:space-between;gap:24pt'><div id='a'><div>Overall condition</div></div><div id='b'>2 photos</div></div></div>");

            Assert.AreEqual(50.0, Left("a"), 0.5, "The first item is at the start");
            Assert.AreEqual(450.0, Right("b"), 1.0, "The last item is on the right edge of the parent's content");
        }

        [TestMethod]
        public void ReportLayout_05_SpaceBetween_WithMargins_IsOnTheRightEdge()
        {
            Layout("<div style='display:flex;justify-content:space-between;margin:0 50pt'><div id='a'>left</div><div id='b'>right</div></div>");

            Assert.AreEqual(50.0, Left("a"), 0.5, "The first item is after the margin");
            Assert.AreEqual(450.0, Right("b"), 1.0, "The last item is before the margin");
        }

        [TestMethod]
        public void ReportLayout_06_SpaceBetween_WithAGap_DoesNotOverflow()
        {
            Layout("<div style='display:flex;justify-content:space-between;gap:40pt'><div id='a'>left</div><div id='b'>middle</div><div id='c'>right</div></div>");

            Assert.AreEqual(500.0, Right("c"), 1.0, "The gaps are part of the space used, so the last item is on the edge");
        }

        [TestMethod]
        public void ReportLayout_07_Flex_ItemsThatFit_StayOnOneLine()
        {
            Layout("<div style='display:flex;justify-content:space-between;margin:0 50pt;font-size:8pt'>" +
                   "<div id='a'>Copyright 2026 Seagull Aerial Survey Ltd</div>" +
                   "<div id='b'>tony@seagullaerialphoto.co.uk - 07305 321110</div></div>");

            Assert.AreEqual(Block("a").TotalBounds.Height.PointsValue, Block("b").TotalBounds.Height.PointsValue, 0.5, "The same height");
            Assert.IsTrue(Block("a").TotalBounds.Height.PointsValue < 12, "Each is one line: " + Block("a").TotalBounds.Height);
            Assert.IsTrue(Right("b") <= 450.5, "Within the margin: " + Right("b"));
        }

        [TestMethod]
        public void ReportLayout_08_Absolute_LeftAndRight_NoWidth_IsStretchedBetweenThem()
        {
            Layout("<div style='position:relative;height:100pt'><div id='abs' style='position:absolute;left:54pt;right:54pt;top:10pt;height:50pt;background-color:red'></div></div>");

            Assert.AreEqual(54.0, Left("abs"), 0.5);
            Assert.AreEqual(500.0 - 54 - 54, Width("abs"), 0.5, "Stretched between the left and right");
        }

        [TestMethod]
        public void ReportLayout_09_Absolute_LeftAndRight_WithAWidth_UsesTheWidth()
        {
            Layout("<div style='position:relative;height:100pt'><div id='abs' style='position:absolute;left:54pt;right:54pt;width:100pt;height:50pt;background-color:red'></div></div>");

            Assert.AreEqual(54.0, Left("abs"), 0.5);
            Assert.AreEqual(100.0, Width("abs"), 0.5, "The explicit width");
        }

        [TestMethod]
        public void ReportLayout_10_Absolute_LeftAndRight_WithMaxWidth_IsLimited()
        {
            Layout("<div style='position:relative;height:100pt'><div id='abs' style='position:absolute;left:54pt;right:54pt;max-width:200pt;height:50pt;background-color:red'></div></div>");

            Assert.AreEqual(200.0, Width("abs"), 0.5, "Stretched, then limited by the max-width");
        }

        [TestMethod]
        public void ReportLayout_11_FlexItem_ThatIsEmpty_HasNoWidth()
        {
            Layout("<div style='display:flex;gap:18pt'><div id='a'></div><div id='b'>Roof findings</div></div>");

            Assert.AreEqual(18.0, Left("b"), 1.0, "The next item is after the gap, not after the space that is left");
        }

        [TestMethod]
        public void ReportLayout_12_FlexItem_ThatIsAFlexRow_IsItsContentWidth()
        {
            Layout("<div style='display:flex;gap:8pt'><span id='a'>FIG 1.1</span>" +
                   "<span id='b' style='display:flex;gap:4pt'><span style='width:15pt;height:15pt'>H</span><span>Site plan</span></span></div>");

            Assert.IsTrue(Width("b") < 100, "The inner row is the width of its items, not the space: " + Width("b"));
            //stretched to the 15pt of the inner row, but 2 lines of 10pt text would be 24pt
            Assert.IsTrue(Block("a").TotalBounds.Height.PointsValue < 20, "The first item is not squeezed onto 2 lines: " + Block("a").TotalBounds.Height);
        }
    }
}
