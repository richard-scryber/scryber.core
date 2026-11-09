using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Tests that the items in a flex row stretch to the height of the row (the default align-items), and that inline
    /// elements such as a span are blockified as flex items.
    /// </summary>
    [TestClass]
    public class FlexStretch_Tests
    {
        private PDFLayoutDocument _layout;
        private string _pdf;

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
                doc.RenderOptions.Compression = Scryber.OutputCompressionType.None;
                doc.SaveAsPDF(ms);
                _pdf = System.Text.Encoding.Latin1.GetString(ms.ToArray());
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

        private double Height(string id) { return Block(id).TotalBounds.Height.PointsValue; }
        private double Width(string id) { return Block(id).TotalBounds.Width.PointsValue; }
        private double Y(string id) { return Block(id).TotalBounds.Y.PointsValue; }

        private const string Row = ".row{display:flex;margin-bottom:5pt} .fixed{width:100pt;flex-shrink:0} .grow{flex:1} ";

        [TestMethod]
        public void Row_ExplicitHeight_ItemsFillTheRow()
        {
            Layout("<div class='row' style='height:60pt'><div id='a' class='fixed'></div><div id='b' class='grow'>text</div></div>", Row);
            Assert.AreEqual(60.0, Height("a"), 0.01, "The empty fixed item");
            Assert.AreEqual(60.0, Height("b"), 0.01, "The growing item");
            Assert.AreEqual(100.0, Width("a"), 0.01);
        }

        [TestMethod]
        public void Row_AutoHeight_ItemsFillTheTallest()
        {
            Layout("<div class='row'><div id='a' class='fixed'></div><div id='b' class='grow'>one<br/>two<br/>three</div></div>", Row);
            Assert.IsTrue(Height("b") > 30.0, "The tall item is the content height: " + Height("b"));
            Assert.AreEqual(Height("b"), Height("a"), 0.6, "The short item stretches to the tallest");
        }

        [TestMethod]
        public void Row_ItemWithHeight_DoesNotStretch()
        {
            Layout("<div class='row' style='height:60pt'><div id='a' class='fixed' style='height:20pt'></div><div id='b' class='grow'>text</div></div>", Row);
            Assert.AreEqual(20.0, Height("a"), 0.01);
            Assert.AreEqual(60.0, Height("b"), 0.01);
        }

        [TestMethod]
        public void Row_AlignItemsOtherThanStretch_DoesNotStretch()
        {
            foreach (var align in new[] { "center", "flex-start", "flex-end" })
            {
                Layout("<div class='row' style='height:60pt;align-items:" + align + "'><div id='a' class='fixed'>x</div><div id='b' class='grow'>text</div></div>", Row);
                Assert.IsTrue(Height("a") < 30.0, align + " does not stretch, " + Height("a"));
            }
        }

        [TestMethod]
        public void Row_AlignSelf_OverridesAlignItems()
        {
            Layout("<div class='row' style='height:60pt;align-items:flex-start'><div id='a' class='fixed'>x</div><div id='b' class='grow' style='align-self:stretch'>text</div></div>", Row);
            Assert.IsTrue(Height("a") < 30.0);
            Assert.AreEqual(60.0, Height("b"), 0.01);

            Layout("<div class='row' style='height:60pt'><div id='a' class='fixed' style='align-self:flex-start'>x</div><div id='b' class='grow'>text</div></div>", Row);
            Assert.IsTrue(Height("a") < 30.0);
            Assert.AreEqual(60.0, Height("b"), 0.01);
        }

        [TestMethod]
        public void Row_ItemMargins_AreInsideTheRowHeight()
        {
            Layout("<div class='row' style='height:60pt'><div id='a' class='fixed' style='margin:5pt 0'></div><div id='b' class='grow'>text</div></div>", Row);
            Assert.AreEqual(60.0, Height("a"), 0.01, "The margin box of the item is the row");
        }

        [TestMethod]
        public void Row_ContentPositionedInStretchedItem_UsesTheStretchedHeight()
        {
            Layout("<div class='row' style='height:100pt'><div class='fixed' style='position:relative'><div style='position:absolute;bottom:0;height:10pt;width:10pt;background-color:#00FF00'></div></div><div class='grow'>text</div></div>", Row);
            //the green box is on the bottom of the 100pt row, so its top is at 90pt down the 400pt page (the pdf y is 310)
            var match = System.Text.RegularExpressions.Regex.Match(_pdf, @"0\.0000 1\.0000 0\.0000 rg\s+([\-0-9\.]+)\s+([\-0-9\.]+)\s+([\-0-9\.]+)\s+([\-0-9\.]+)\s+re f");
            Assert.IsTrue(match.Success, "The green box was not found");
            Assert.AreEqual(310.0, double.Parse(match.Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture), 0.6);
        }

        // inline items

        [TestMethod]
        public void Row_SpanItems_HaveTheirWidth()
        {
            Layout("<div class='row'><span id='a' style='width:120pt'>label</span><span id='b' class='grow'>text</span></div>", Row);
            Assert.AreEqual(120.0, Width("a"), 0.01);
            Assert.AreEqual(380.0, Width("b"), 0.5);
        }

        [TestMethod]
        public void Column_SpanItems_Stack()
        {
            Layout("<div style='display:flex;flex-direction:column;width:100pt'><span id='a'>one</span><span id='b'>two</span></div>");
            Assert.IsTrue(Y("b") > Y("a") + 5.0, "The second span is below the first: " + Y("a") + ", " + Y("b"));
        }

        [TestMethod]
        public void Column_SpanItems_Centred()
        {
            Layout("<div style='display:flex;flex-direction:column;align-items:center;width:200pt'><span id='a' style='width:50pt'>one</span></div>");
            Assert.AreEqual(75.0, Block("a").TotalBounds.X.PointsValue, 1.0, "centred in the 200pt");
        }
    }
}
