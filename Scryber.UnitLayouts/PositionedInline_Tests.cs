using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using Scryber.Drawing;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class PositionedInline_Tests
    {
        const string TestCategoryName = "Layout Inline";

        PDFLayoutDocument layout;

        /// <summary>
        /// Event handler that sets the layout instance variable, after the layout has completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            layout = args.Context.GetLayout<PDFLayoutDocument>();
        }


        protected string AssertGetContentFile(string name)
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Positioning/" + name + ".html");
            path = System.IO.Path.GetFullPath(path);

            if (!System.IO.File.Exists(path))
                Assert.Inconclusive("The path the file " + name + " was not found at " + path);

            return path;
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineTest()
        {
            var path = AssertGetContentFile("Inline");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var content = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;
            Assert.IsNotNull(content);

            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 15;

            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            

            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(yOffset, line.OffsetX);
            Assert.AreEqual(xOffset, line.OffsetY);
            Assert.AreEqual(height, line.Height);

            //Arrangement is for links and inner content references
            var span = layout.DocumentComponent.FindAComponentById("inline") as Span;
            var arrange = span.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineWithMarginsTest()
        {
            var path = AssertGetContentFile("InlineWithMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_InlineWithMargins.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var content = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;
            Assert.IsNotNull(content);

            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 15;

            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);


            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            //line is at offset 0,0 relative to the content region
            Assert.AreEqual(yOffset, line.OffsetX);
            Assert.AreEqual(xOffset, line.OffsetY);
            Assert.AreEqual(height, line.Height);

            //Arrangement is for links and inner content references
            var span = layout.DocumentComponent.FindAComponentById("inline") as Span;
            var arrange = span.GetFirstArrangement();

            //render bounds take into account the margins
            yOffset += 30;
            xOffset += 30;

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineNestedWithMarginsTest()
        {
            var path = AssertGetContentFile("InlineNestedWithMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_InlineNestedWithMargins.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var content = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;
            Assert.IsNotNull(content);

            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 15;

            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);


            var block = content.Contents[0] as PDFLayoutBlock;

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;

            Assert.IsNotNull(line);

           
            //line is at offset 0,0 relative to the content region
            Assert.AreEqual(yOffset, line.OffsetX);
            Assert.AreEqual(xOffset, line.OffsetY);
            Assert.AreEqual(height, line.Height);

            //Arrangement is for links and inner content references
            var span = layout.DocumentComponent.FindAComponentById("inline") as Span;
            var arrange = span.GetFirstArrangement();

            //render bounds take into account the margins on the page and the parent div
            yOffset += 30 + 50;
            xOffset += 30 + 30;

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);

        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineNestedSecondWithMarginsTest()
        {
            var path = AssertGetContentFile("InlineNestedSecondWithMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_InlineNestedSecondWithMargins.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var content = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;
            Assert.IsNotNull(content);

            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 15;

            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);


            var block = content.Contents[0] as PDFLayoutBlock;

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;

            Assert.IsNotNull(line);


            //line is at offset 0,0 relative to the content region
            Assert.AreEqual(yOffset, line.OffsetX);
            Assert.AreEqual(xOffset, line.OffsetY);
            Assert.AreEqual(height, line.Height);



            //Arrangement is for links and inner content references
            var span = layout.DocumentComponent.FindAComponentById("inline") as Span;
            var arrange = span.GetFirstArrangement();

            //render bounds take into account the margins on the page and the parent div
            yOffset += 30 + 50;
            xOffset += 30 + 20 + 10 + (line.Width - arrange.RenderBounds.Width);

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineNestedSecondRightWithMarginsTest()
        {
            var path = AssertGetContentFile("InlineNestedSecondRightWithMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_InlineNestedSecondRightWithMargins.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var content = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;
            Assert.IsNotNull(content);

            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 15;

            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);


            var block = content.Contents[0] as PDFLayoutBlock;

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;

            Assert.IsNotNull(line);


            //line is at offset 0,0 relative to the content region
            Assert.AreEqual(yOffset, line.OffsetX);
            Assert.AreEqual(xOffset, line.OffsetY);
            Assert.AreEqual(height, line.Height);



            //Arrangement is for links and inner content references
            var span = layout.DocumentComponent.FindAComponentById("inline") as Span;
            var arrange = span.GetFirstArrangement();

            //render bounds take into account the margins on the page and the parent div
            yOffset += 30 + 50;
            var pgWidth = layout.AllPages[0].Width;

            //right align with padding and margins and the actual text width
            xOffset = pgWidth - (20 + 10 + 30 + arrange.RenderBounds.Width);

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);

        }

    }
}
