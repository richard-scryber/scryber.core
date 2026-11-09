using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Flex items without a width, flex-basis or flex-grow are the width of their content (measured with a layout that is discarded).
    /// </summary>
    [TestClass]
    public class FlexContentWidth_Tests
    {
        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        private void Layout(string body, string css = "", string page = "size:500pt 400pt;margin:0")
        {
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>@page{" + page + "} body{margin:0;font-size:10pt} " + css + "</style></head><body>" + body + "</body></html>";
            using (var doc = Document.ParseDocument(new System.IO.StringReader(html), ParseSourceType.DynamicContent))
            using (var ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
        }

        private PDFLayoutBlock Block(string id, int pageIndex = 0)
        {
            var found = Find(id, _layout.AllPages[pageIndex].ContentBlock);
            Assert.IsNotNull(found, "No block for '" + id + "'");
            return found;
        }

        private static PDFLayoutBlock Find(string id, PDFLayoutBlock parent)
        {
            foreach (var column in parent.Columns)
                foreach (var item in column.Contents)
                    if (item is PDFLayoutBlock block)
                    {
                        if (block.Owner != null && block.Owner.ID == id)
                            return block;
                        var inner = Find(id, block);
                        if (null != inner)
                            return inner;
                    }
            return null;
        }

        private static int Count(string id, PDFLayoutBlock parent)
        {
            int count = 0;
            foreach (var column in parent.Columns)
                foreach (var item in column.Contents)
                    if (item is PDFLayoutBlock block)
                    {
                        if (block.Owner != null && block.Owner.ID == id)
                            count++;
                        count += Count(id, block);
                    }
            return count;
        }

        private double Width(string id) { return Block(id).TotalBounds.Width.PointsValue; }

        private const string Css = ".row{display:flex} .fixed{width:100pt;flex-shrink:0} .grow{flex:1} ";

        [TestMethod]
        public void AutoItem_NextToAGrowingItem_IsItsContentWidth()
        {
            Layout("<div class='row'><div id='a' class='fixed'>A</div><div id='b' class='grow'>grows</div><div id='c'>content</div></div>", Css);
            double c = Width("c");
            Assert.IsTrue(c > 20 && c < 80, "The content width of 'content': " + c);
            Assert.AreEqual(500.0, Width("a") + Width("b") + c, 1.0, "The row is filled");
            Assert.AreEqual(100.0, Width("a"), 0.01);
        }

        [TestMethod]
        public void AutoItem_WithMargin_HasItsContentAndMargin()
        {
            Layout("<div class='row'><div id='b' class='grow'>grows</div><span id='c' style='margin-right:42pt'>x x</span></div>", Css);
            var block = Block("c");
            double content = block.TotalBounds.Width.PointsValue - (block.Position.Margins.Left + block.Position.Margins.Right).PointsValue;
            Assert.IsTrue(content > 8, "The content is not lost in the margin: " + content);
            Assert.AreEqual(500.0, Width("b") + block.TotalBounds.Width.PointsValue, 1.0);
        }

        [TestMethod]
        public void AutoItem_WithInlineBlocks_IsTheirWidth()
        {
            Layout("<div class='row'><div class='grow'>grows</div><span id='c'><i></i><i></i><i></i><i></i></span></div>",
                Css + " #c i{display:inline-block;width:10pt;height:10pt;margin-left:3pt;border:1.5pt solid black}");
            double c = Width("c");
            Assert.IsTrue(c >= 4 * 13 - 1 && c < 4 * 20, "Four 13pt+ boxes: " + c);
        }

        [TestMethod]
        public void AutoItems_Only_ArePackedToTheStart()
        {
            Layout("<div class='row'><span id='a'>one</span><span id='b'>two</span><span id='c'>three</span></div>", Css);
            double total = Width("a") + Width("b") + Width("c");
            Assert.IsTrue(total < 200, "The items are their content widths: " + total);
        }

        [TestMethod]
        public void AutoItems_JustifySpaceBetween_UseTheContentWidths()
        {
            Layout("<div class='row' style='justify-content:space-between'><span id='a'>left</span><span id='b'>middle</span><span id='c'>right</span></div>", Css);
            var c = Block("c");
            double right = c.PagePosition.X.PointsValue + c.TotalBounds.Width.PointsValue;
            Assert.AreEqual(500.0, right, 1.0, "The last item is on the right edge");
        }

        [TestMethod]
        public void AutoItem_LongContent_IsLimitedToTheRow()
        {
            Layout("<div class='row'><span id='a'>" + string.Concat(Enumerable.Repeat("A long run of words ", 20)) + "</span></div>", Css);
            Assert.IsTrue(Width("a") <= 500.5, "No wider than the row: " + Width("a"));
        }

        [TestMethod]
        public void AutoItems_AreNotLaidOutTwice()
        {
            Layout("<div class='row'><div class='grow'>grows</div><span id='c'>content</span></div>", Css);
            Assert.AreEqual(1, Count("c", _layout.AllPages[0].ContentBlock), "Only the real layout of the item is kept");
        }

        [TestMethod]
        public void Row_AtTheEndOfThePage_MeasuringDoesNotAddAPage()
        {
            Layout("<div style='height:370pt'>fill</div><div class='row'><span id='a'>left</span><span id='b'>right</span><span id='c'>more</span></div>", Css);
            Assert.AreEqual(1, _layout.AllPages.Count, "The row fits on the first page");
            Assert.AreEqual(1, Count("a", _layout.AllPages[0].ContentBlock));
        }

        [TestMethod]
        public void Row_ThatDoesNotFit_IsTheSameAsWithoutMeasuring()
        {
            //A row of growing items is not measured - the row with content width items must break in the same way
            Layout("<div style='height:395pt'>fill</div><div class='row'><span id='a' class='grow'>left</span><div class='grow'>grow</div><span class='grow'>more</span></div>", Css);
            int pages = _layout.AllPages.Count;
            int first = Count("a", _layout.AllPages[0].ContentBlock);
            int second = Count("a", _layout.AllPages[1].ContentBlock);

            Layout("<div style='height:395pt'>fill</div><div class='row'><span id='a'>left</span><div class='grow'>grow</div><span>more</span></div>", Css);
            Assert.AreEqual(pages, _layout.AllPages.Count);
            Assert.AreEqual(first, Count("a", _layout.AllPages[0].ContentBlock));
            Assert.AreEqual(second, Count("a", _layout.AllPages[1].ContentBlock));
        }

        [TestMethod]
        public void Wrap_AutoItems_BreakOntoRows_ByContentWidth()
        {
            var words = string.Concat(Enumerable.Repeat("word ", 30));
            Layout("<div class='row' style='flex-wrap:wrap'><span id='a'>" + words + "</span><span id='b'>" + words + "</span></div>", Css);
            var a = Block("a");
            var b = Block("b");
            Assert.IsTrue(b.PagePosition.Y > a.PagePosition.Y, "The second wide item is on the next row");
        }
    }
}
