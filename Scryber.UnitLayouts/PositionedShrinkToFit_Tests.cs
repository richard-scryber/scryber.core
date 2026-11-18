using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF.Layout;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Absolute, fixed, floating and inline-block content without a width is the width of its content (css shrink-to-fit),
    /// including when that content is blocks - which then fill the width of the widest. Checked against Chrome.
    /// </summary>
    [TestClass()]
    public class PositionedShrinkToFit_Tests
    {
        const string TestCategoryName = "Layout Shrink To Fit";

        private string AssertGetContentFile(string name)
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Positioning/ShrinkToFit/" + name + ".html");
            path = System.IO.Path.GetFullPath(path);

            if (!System.IO.File.Exists(path))
                Assert.Inconclusive("The path the file " + name + " was not found at " + path);

            return path;
        }

        private static Rect BoundsOf(Document doc, string id)
        {
            var c = doc.FindAComponentById(id) as Component;
            Assert.IsNotNull(c, "No component " + id);
            var arrange = c.GetFirstArrangement();
            Assert.IsNotNull(arrange, "No arrangement for " + id);
            return arrange.RenderBounds;
        }

        /// <summary>
        /// Asserts the x and width of the component (in points) against the values from Chrome, within 1.5pt for the text measurement.
        /// </summary>
        private static void AssertBounds(Document doc, string id, double x, double width)
        {
            var bounds = BoundsOf(doc, id);
            Console.WriteLine(id + " x=" + bounds.X.PointsValue.ToString("0.0") + " w=" + bounds.Width.PointsValue.ToString("0.0") + " (chrome " + x + ", " + width + ")");
            Assert.AreEqual(x, bounds.X.PointsValue, 1.5, id + " x was " + bounds.X.PointsValue);
            Assert.AreEqual(width, bounds.Width.PointsValue, 1.5, id + " width was " + bounds.Width.PointsValue);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ShrinkToFit_01_PositionedWithBlocks()
        {
            var path = AssertGetContentFile("ShrinkToFit_01_PositionedWithBlocks");
            var doc = Document.ParseDocument(path);

            PDFLayoutDocument layout = null;
            doc.LayoutComplete += (sender, args) => layout = args.Context.GetLayout<PDFLayoutDocument>();

            using (var ms = DocStreams.GetOutputStream("ShrinkToFit_01_PositionedWithBlocks.pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            Assert.AreEqual(1, layout.AllPages.Count, "All on one page");

            //text only - the width of the text, as before
            AssertBounds(doc, "absText", 10, 67.4);

            //an absolute block containing a block is the width of the block, and the block fills it
            AssertBounds(doc, "absBlock", 10, 93.4);
            AssertBounds(doc, "absBlockInner", 10, 93.4);

            //two blocks are both the width of the widest
            AssertBounds(doc, "absTwo", 10, 148.1);
            AssertBounds(doc, "absTwoA", 10, 148.1);
            AssertBounds(doc, "absTwoB", 10, 148.1);

            //fixed, with the margins of the inner block
            AssertBounds(doc, "fixBlock", 10, 119.4);
            AssertBounds(doc, "fixBlockInner", 20, 99.4);

            //the padding of the positioned block is added to its content
            AssertBounds(doc, "absPad", 10, 119.4);
            AssertBounds(doc, "absPadP", 16, 107.4);

            //max-width limits the width, and the text wraps within it
            AssertBounds(doc, "absMax", 10, 100);

            //content wider than the space to the right is limited to that space, and wraps
            AssertBounds(doc, "absLong", 200, 200);
            AssertBounds(doc, "absLongInner", 200, 200);

            //an inline-block containing a block
            AssertBounds(doc, "ib", 200, 124);
            AssertBounds(doc, "ibInner", 200, 124);

            //float (with absolute) containing a block
            AssertBounds(doc, "flt", 200, 57);

            //text only that wraps fills the space to the right
            AssertBounds(doc, "absLongText", 200, 200);

            //right aligned, from the width of the content
            AssertBounds(doc, "absRight", 299.3, 90.7);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ShrinkToFit_02_ContainerContent()
        {
            var path = AssertGetContentFile("ShrinkToFit_02_ContainerContent");
            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("ShrinkToFit_02_ContainerContent.pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            //a flex row is the width of its items and the gap
            AssertBounds(doc, "flex", 10, 66.7);
            AssertBounds(doc, "flexA", 10, 20.0);
            AssertBounds(doc, "flexB", 36, 40.7);

            //a table is the width of its columns (it keeps the space, and the table is the width of its content)
            AssertBounds(doc, "table", 10, 98.0);

            //a list is the width of its widest item, with the indent of the items.
            //(Chrome is 104.7pt, with a 30pt indent - scryber indents list items 40pt in the flow as well, so the item text is compared)
            var list = BoundsOf(doc, "list");
            var item = BoundsOf(doc, "listB");
            Assert.AreEqual(74.7, item.Width.PointsValue, 1.5, "The widest item is the width of its text");
            Assert.AreEqual((item.X + item.Width).PointsValue, (list.X + list.Width).PointsValue, 0.5, "The list ends at its widest item");
            Assert.AreEqual(item.Width.PointsValue, BoundsOf(doc, "listA").Width.PointsValue, 0.01, "The items fill the list");

            //centred text is centred in the width of the widest line
            AssertBounds(doc, "centred", 10, 108.7);
            AssertBounds(doc, "centredA", 10, 108.7);
            AssertBounds(doc, "centredB", 10, 108.7);

            //nested blocks with padding
            AssertBounds(doc, "nested", 200, 93.4);
            AssertBounds(doc, "nestedInner", 205, 83.4);
        }
    }
}
