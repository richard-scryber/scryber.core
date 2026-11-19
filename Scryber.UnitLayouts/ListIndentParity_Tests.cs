using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF.Layout;
using System.Collections.Generic;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// The indent of html lists, as css - the items are indented by the padding of the list (40px, 30pt by default),
    /// and the marker is in that padding. Checked against Chrome.
    /// </summary>
    [TestClass()]
    public class ListIndentParity_Tests
    {
        const string TestCategoryName = "Layout Lists";

        private const string Html = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>
@page { size: 400pt 400pt; margin: 0; } html{width:400pt} body { margin: 0; padding: 10pt; font-family: Helvetica, sans-serif; font-size: 12pt; }
ul, ol, dl { margin: 0 0 10pt 0; }
</style></head>
<body>
<ul id='ul'><li id='ul1'>disc item</li><li id='ul2'>second disc item<ul id='ul2n'><li id='ul2n1'>nested item</li></ul></li></ul>
<ol id='ol'><li id='ol1'>first number</li><li>2</li><li>3</li><li>4</li><li>5</li><li>6</li><li>7</li><li>8</li><li>9</li><li id='ol10'>tenth number</li></ol>
<ul id='pad0' style='padding-left: 0'><li id='pad0li'>padding-left 0</li></ul>
<ul id='pad15' style='padding-left: 15pt'><li id='pad15li'>padding-left 15pt</li></ul>
<ul id='none' style='list-style: none'><li id='noneli'>no marker</li></ul>
<dl id='dl'><dt id='dt'>term</dt><dd id='dd'>definition</dd></dl>
</body>
</html>
";

        private static void AssertBounds(Document doc, string id, double x, double width)
        {
            var c = doc.FindAComponentById(id) as Component;
            Assert.IsNotNull(c, "No component " + id);
            var arrange = c.GetFirstArrangement();
            Assert.IsNotNull(arrange, "No arrangement for " + id);
            var bounds = arrange.RenderBounds;
            Assert.AreEqual(x, bounds.X.PointsValue, 0.5, id + " x was " + bounds.X.PointsValue);
            Assert.AreEqual(width, bounds.Width.PointsValue, 0.5, id + " width was " + bounds.Width.PointsValue);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ListIndent_01_AsChrome()
        {
            var doc = Document.ParseDocument(new StringReader(Html), ParseSourceType.DynamicContent);
            using (var ms = DocStreams.GetOutputStream("ListIndent_01_AsChrome.pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            //the list is the full width, and the items are inside its padding
            AssertBounds(doc, "ul", 10, 380);
            AssertBounds(doc, "ul1", 40, 350);
            AssertBounds(doc, "ul2", 40, 350);

            //a nested list is indented by its own padding
            AssertBounds(doc, "ul2n", 40, 350);
            AssertBounds(doc, "ul2n1", 70, 320);

            //numbers (including the wider 10.) do not change the indent
            AssertBounds(doc, "ol", 10, 380);
            AssertBounds(doc, "ol1", 40, 350);
            AssertBounds(doc, "ol10", 40, 350);

            //the padding of the list is the indent
            AssertBounds(doc, "pad0", 10, 380);
            AssertBounds(doc, "pad0li", 10, 380);
            AssertBounds(doc, "pad15", 10, 380);
            AssertBounds(doc, "pad15li", 25, 365);

            //without markers the list keeps its padding
            AssertBounds(doc, "none", 10, 380);
            AssertBounds(doc, "noneli", 40, 350);

            //a definition is indented 40px (30pt)
            AssertBounds(doc, "dl", 10, 380);
            AssertBounds(doc, "dt", 10, 380);
            AssertBounds(doc, "dd", 40, 350);
        }

        private const string MarkersHtml = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>
@page { size: 400pt 400pt; margin: 0; } html{width:400pt} body { margin: 0; padding: 10pt; font-family: Helvetica, sans-serif; font-size: 12pt; }
ul, ol { margin: 0; }
</style></head>
<body>
<ul><li id='d1'>disc<ul><li id='d2'>circle<ul><li id='d3'>square<ul><li id='d4'>still square</li></ul></li></ul></li></ul></li></ul>
<ol><li id='o1'>number<ul><li id='o2'>circle in an ol</li></ul></li></ol>
<ul style='list-style-type: square'><li id='e1'>explicit square</li></ul>
<ul><li>x<ul style='list-style-type: disc'><li id='e2'>explicit disc</li></ul></li></ul>
<ul style='list-style-type: circle'><li id='e3'>explicit circle</li></ul>
</body>
</html>
";

        /// <summary>
        /// Finds the markers in the layout, by the id of their list item
        /// </summary>
        private static void CollectMarkers(PDFLayoutBlock block, Dictionary<string, ListItemMarker> found)
        {
            if (null != block.Columns)
            {
                foreach (var region in block.Columns)
                    CollectMarkers(region, found);
            }
            if (block.HasPositionedRegions)
            {
                foreach (var region in block.PositionedRegions)
                    CollectMarkers(region, found);
            }
        }

        private static void CollectMarkers(PDFLayoutRegion region, Dictionary<string, ListItemMarker> found)
        {
            foreach (var item in region.Contents)
            {
                if (item is PDFLayoutBlock inner)
                    CollectMarkers(inner, found);
                else if (item is PDFLayoutLine line)
                {
                    foreach (var run in line.Runs)
                    {
                        if (run is PDFLayoutComponentRun comp && comp.Owner is ListItemMarker marker)
                            found[marker.ID.Replace("_Num_Marker", "")] = marker;
                    }
                }
            }
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ListIndent_02_Markers()
        {
            var doc = Document.ParseDocument(new StringReader(MarkersHtml), ParseSourceType.DynamicContent);
            PDFLayoutDocument layout = null;
            doc.LayoutComplete += (sender, args) => layout = args.Context.GetLayout<PDFLayoutDocument>();
            using (var ms = DocStreams.GetOutputStream("ListIndent_02_Markers.pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            var markers = new Dictionary<string, ListItemMarker>();
            CollectMarkers(layout.AllPages[0].ContentBlock, markers);

            //as Chrome - a disc, then a circle in a nested list (in a ul or an ol), then a square for any deeper
            Assert.AreEqual(ListNumberingGroupStyle.Disc, markers["d1"].Shape);
            Assert.AreEqual(ListNumberingGroupStyle.Circle, markers["d2"].Shape);
            Assert.AreEqual(ListNumberingGroupStyle.Square, markers["d3"].Shape);
            Assert.AreEqual(ListNumberingGroupStyle.Square, markers["d4"].Shape);
            Assert.IsFalse(markers.ContainsKey("o1"), "A number is not a marker");
            Assert.AreEqual(ListNumberingGroupStyle.Circle, markers["o2"].Shape);

            //an explicit list-style-type is used at any depth
            Assert.AreEqual(ListNumberingGroupStyle.Square, markers["e1"].Shape);
            Assert.AreEqual(ListNumberingGroupStyle.Disc, markers["e2"].Shape);
            Assert.AreEqual(ListNumberingGroupStyle.Circle, markers["e3"].Shape);

            foreach (var marker in markers.Values)
                Assert.AreEqual(12.0, marker.FontSize.PointsValue, 0.001, "The marker is sized from the font of the item");

            //and the items are where they are in Chrome
            AssertBounds(doc, "d1", 40, 350);
            AssertBounds(doc, "d2", 70, 320);
            AssertBounds(doc, "d3", 100, 290);
            AssertBounds(doc, "d4", 130, 260);
            AssertBounds(doc, "o2", 70, 320);
            AssertBounds(doc, "e2", 70, 320);
        }
    }
}
