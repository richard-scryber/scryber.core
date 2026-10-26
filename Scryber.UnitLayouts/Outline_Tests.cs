using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;

namespace Scryber.UnitLayouts
{
    [TestClass]
    public class Outline_Tests
    {
        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        private PDFLayoutBlock Find(string id, PDFLayoutBlock parent = null)
        {
            parent = parent ?? _layout.AllPages[0].ContentBlock;
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

        private void Layout(string style)
        {
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><body style='margin:0;padding:0'><div id='a' style='width:100pt;height:50pt;" + style + "'>x</div><div id='b' style='height:20pt'>y</div></body></html>";
            using (var doc = Document.ParseDocument(new System.IO.StringReader(html), ParseSourceType.DynamicContent))
            using (var ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
        }

        [TestMethod]
        public void Outline_TakesNoSpace()
        {
            Layout("");
            var plainBottom = Find("b").TotalBounds.Y.PointsValue;
            var plainWidth = Find("a").TotalBounds.Width.PointsValue;

            Layout("outline: 10pt solid red; outline-offset: 5pt");
            Assert.AreEqual(plainBottom, Find("b").TotalBounds.Y.PointsValue, 0.01, "The next block is in the same place");
            Assert.AreEqual(plainWidth, Find("a").TotalBounds.Width.PointsValue, 0.01, "The outlined block is the same size");
        }

        [TestMethod]
        public void BoxShadow_TakesNoSpace()
        {
            Layout("");
            var plainBottom = Find("b").TotalBounds.Y.PointsValue;
            var plainWidth = Find("a").TotalBounds.Width.PointsValue;

            Layout("box-shadow: 10pt 10pt 8pt 4pt rgba(0,0,0,0.5)");
            Assert.AreEqual(plainBottom, Find("b").TotalBounds.Y.PointsValue, 0.01, "The next block is in the same place");
            Assert.AreEqual(plainWidth, Find("a").TotalBounds.Width.PointsValue, 0.01, "The shadowed block is the same size");
        }
    }
}
