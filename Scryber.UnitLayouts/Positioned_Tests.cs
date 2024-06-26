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
    public class Positioned_Tests
    {
        const string TestCategoryName = "Layout";

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


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_1_BlockTest()
        {
            var path = AssertGetContentFile("FixedBlock");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_1_Block.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            Assert.IsNotNull(layout, "Layout not captured");
            var content = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(content);

            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = 0;
            Unit height = 15;

            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);

            var block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);


            //check the wrapping spans

            yOffset -= height; //Up one line height

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);

            //Next span is XOffset by the width, but on the same line.
            xOffset += arrange.RenderBounds.Width;

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_2_BlockMarginsTest()
        {
            var path = AssertGetContentFile("FixedBlockMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_2_BlockMargins.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var content = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(content);

            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = 0;
            Unit height = 15;

            yOffset += 30; //body Margins;
            xOffset += 30; //body Margins;

            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);

            var block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();

            

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);


            yOffset -= height; //Up one line height

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);

            //Next span is XOffset by the width, but on the same line.
            xOffset += arrange.RenderBounds.Width;

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
        }

        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_3_BlockFullWidthTest()
        {
            var path = AssertGetContentFile("FixedBlockFullWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_3_BlockFullWidth.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var content = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(content);


            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = 0;
            Unit height = 15;
            Unit width = layout.AllPages[0].Width; //ignores the margin width

            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);
            Assert.AreEqual(width, content.TotalBounds.Width);

            var block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();

            yOffset += 0; //body Margins;
            xOffset += 0; //body Margins;
            //still full page width as fixed

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);

            yOffset -= height; //Up one line height

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);

            //Next span is XOffset by the width, but on the same line.
            xOffset += arrange.RenderBounds.Width;

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_4_BlockNestedFullWidthTest()
        {
            var path = AssertGetContentFile("FixedBlockNestedFullWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_FixedBlock_4_NestedFullWidth.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var block = layout.AllPages[0].ContentBlock;
            block = block.Columns[0].Contents[1] as PDFLayoutBlock;
            var content = block.PositionedRegions[0] as PDFLayoutPositionedRegion;

            Assert.IsNotNull(content);


            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = 0;
            Unit height = 15;
            Unit width = layout.AllPages[0].Width; //ignores the margin width

            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);
            Assert.AreEqual(width, content.TotalBounds.Width);

            block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();

            yOffset += 0; //body Margins;
            xOffset += 0; //body Margins;
            //still full page width as fixed

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);

            yOffset -= height; //Up one line height

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);

            //Next span is XOffset by the width, but on the same line.
            xOffset += arrange.RenderBounds.Width;

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_5_BlockNestedMarginsTest()
        {
            var path = AssertGetContentFile("FixedBlockNestedMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_FixedBlock_5_NestedMargins.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var block = layout.AllPages[0].ContentBlock;
            block = block.Columns[0].Contents[1] as PDFLayoutBlock;
            var content = block.PositionedRegions[0] as PDFLayoutPositionedRegion;

            Assert.IsNotNull(content);


            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = 0;
            Unit height = 15;
            //Unit width = layout.AllPages[0].Width; //ignores the margin width

            yOffset += 20 + 10; //body Margins;
            xOffset += 20 + 20; //body Margins;

            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);
            //Assert.AreEqual(width, content.TotalBounds.Width);

            block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();

           

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);

            yOffset -= height; //Up one line height

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);

            //Next span is XOffset by the width, but on the same line.
            xOffset += arrange.RenderBounds.Width;

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_6_BlockNestedFullWidthMarginsTest()
        {
            var path = AssertGetContentFile("FixedBlockNestedFullWidthMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_FixedBlock_6_NestedFullWidthMargins.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var block = layout.AllPages[0].ContentBlock;
            block = block.Columns[0].Contents[1] as PDFLayoutBlock;
            var content = block.PositionedRegions[0] as PDFLayoutPositionedRegion;

            Assert.IsNotNull(content);


            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = 0;
            Unit height = 15;
            Unit width = layout.AllPages[0].Width; //ignores the margin width

            yOffset += 20 + 10; //body Margins;
            xOffset += 20 + 20; //body Margins;

            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);
            Assert.AreEqual(width, content.TotalBounds.Width);

            block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);

            yOffset -= height; //Up one line height

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);

            //Next span is XOffset by the width, but on the same line.
            xOffset += arrange.RenderBounds.Width;

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_7_BlockMarginsPaddingTest()
        {
            var path = AssertGetContentFile("FixedBlockMarginsPadding");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_7_BlockMarginsPadding.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var content = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(content);

            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = 0;
            Unit height = 15;

            yOffset += 30; //Margins;
            xOffset += 30; //Margins;

            yOffset += 20; //Padding
            xOffset += 10; //Padding


            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);

            var block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);


            yOffset -= height; //Up one line height

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);

            //Next span is XOffset by the width, but on the same line.
            xOffset += arrange.RenderBounds.Width;

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_8_BlockNestedMarginsPaddingTest()
        {
            var path = AssertGetContentFile("FixedBlockNestedMarginsPadding");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_8_BlockNestedMarginsPadding.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var block = layout.AllPages[0].ContentBlock;
            block = block.Columns[0].Contents[1] as PDFLayoutBlock;
            var content = block.PositionedRegions[0] as PDFLayoutPositionedRegion;

            
            Assert.IsNotNull(content);

            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = 0;
            Unit height = 15;

            yOffset += 30; //Body Margins;
            xOffset += 30; //Body Margins;

            yOffset += 20; //Body Padding
            xOffset += 10; //Body Padding

            yOffset += 10; //nest Margins;
            xOffset += 20; //nest Margins

            yOffset += 10; //nest Padding;
            xOffset += 10; //nest Padding;


            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);

            block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);


            yOffset -= height; //Up one line height

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);

            //Next span is XOffset by the width, but on the same line.
            xOffset += arrange.RenderBounds.Width;

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_9_BlockNestedMarginsPaddingFullWidthTest()
        {
            var path = AssertGetContentFile("FixedBlockNestedMarginsPaddingFullWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_9_BlockNestedMarginsPaddingFullWidth.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var block = layout.AllPages[0].ContentBlock;
            block = block.Columns[0].Contents[1] as PDFLayoutBlock;
            var content = block.PositionedRegions[0] as PDFLayoutPositionedRegion;


            Assert.IsNotNull(content);

            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = 0;
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            yOffset += 30; //Body Margins;
            xOffset += 30; //Body Margins;

            yOffset += 20; //Body Padding
            xOffset += 10; //Body Padding

            yOffset += 10; //nest Margins;
            xOffset += 20; //nest Margins

            yOffset += 10; //nest Padding;
            xOffset += 10; //nest Padding;


            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);
            Assert.AreEqual(width, content.TotalBounds.Width);

            block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);
            Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset -= height; //Up one line height

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);

            //Next span is XOffset by the width, but on the same line.
            xOffset += arrange.RenderBounds.Width;

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void FixedBlockPositionWithMarginsTest()
        {
            var path = AssertGetContentFile("FixedBlockPositionWithMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_FixedBlockPositionWithMargins.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 20;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.Inconclusive("Need to test the fixed div");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void FixedBlockPositionWithHeaderTest()
        {
            var path = AssertGetContentFile("FixedBlockPositionWithHeader");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_FixedBlockPositionWithHeader.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 20;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.Inconclusive("Need to test the fixed div");
        }

        

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void FixedBlockPositionTest()
        {
            var path = AssertGetContentFile("FixedBlockPosition");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_FixedBlockPosition.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 20;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.Inconclusive("Need to test the fixed div");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void FixedBlockPositionFullWidthTest()
        {
            var path = AssertGetContentFile("FixedBlockPositionFullWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_FixedBlockPositionFullWidth.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 20;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.Inconclusive("Need to test the fixed div");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void FixedBlockBottomRightPositionTest()
        {
            var path = AssertGetContentFile("FixedBlockBottomRightPosition");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_FixedBlockBottomRightPosition.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 20;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.Inconclusive("Need to test the fixed div");
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void RelativeBlockTest()
        {
            var path = AssertGetContentFile("BasicRelative");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_BlockRelative.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 20;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.Inconclusive("Need to test the relative div");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void RelativeBlockPositionTest()
        {
            var path = AssertGetContentFile("BasicRelativeWithPosition");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_BlockRelativeWithPosition.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 20;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.Inconclusive("Need to test the relative div");
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockRelativeToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div() {
                Height = 150, Width = 200, X = 50, Y = 100,
                PositionMode = Drawing.PositionMode.Relative,
                BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            relative.Contents.Add(new TextLiteral("Sits relative on the first page"));
            section.Contents.Add(relative);

            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            inflow.Contents.Add(new TextLiteral("In normal content flow"));
            section.Contents.Add(inflow);

            using (var ms = DocStreams.GetOutputStream("Positioned_BlockRelativeToPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(1, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.AreEqual(0, first.Height);
            Assert.AreEqual(1, first.Runs.Count);
            var posRun = first.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, content.PositionedRegions[0]);
            var posReg = content.PositionedRegions[0];

            //TODO: Clean up offsetX and Y with TotalBounds.
            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(100, posReg.TotalBounds.Y);

            Assert.AreEqual(150, posReg.TotalBounds.Height);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(1, posReg.Contents.Count);

            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(150, posBlock.Height);
            Assert.AreEqual(200, posBlock.Width);
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(2, posBlock.Columns[0].Contents.Count);

            //Check the block after to make sure it is ignoring the positioned region.
            var second = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(0, second.OffsetY);
            Assert.AreEqual(25, second.Height);
            Assert.AreEqual(1, second.Columns[0].Contents.Count); //just a line

            //block region line = textbegin, chars, textend
            Assert.AreEqual("In normal content flow", ((second.Columns[0].Contents[0] as PDFLayoutLine).Runs[1] as PDFTextRunCharacter).Characters);

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockAbsoluteToPage()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = 150,
                Width = 200,
                X = 50,
                Y = 100,
                PositionMode = Drawing.PositionMode.Absolute,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            relative.Contents.Add(new TextLiteral("Sits absolute on the first page"));
            section.Contents.Add(relative);

            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            inflow.Contents.Add(new TextLiteral("In normal content flow"));
            section.Contents.Add(inflow);

            using (var ms = DocStreams.GetOutputStream("Positioned_BlockAbsoluteToPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(1, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.AreEqual(0, first.Height);
            Assert.AreEqual(1, first.Runs.Count);
            var posRun = first.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, content.PositionedRegions[0]);
            var posReg = content.PositionedRegions[0];

            //TODO: Clean up offsetX and Y with TotalBounds.

            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(100, posReg.TotalBounds.Y);

            Assert.AreEqual(150, posReg.TotalBounds.Height);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(1, posReg.Contents.Count);

            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(150, posBlock.Height);
            Assert.AreEqual(200, posBlock.Width);
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(2, posBlock.Columns[0].Contents.Count);

            //Check the block after to make sure it is ignoring the positioned region.
            var second = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(0, second.OffsetY);
            Assert.AreEqual(25, second.Height);
            Assert.AreEqual(1, second.Columns[0].Contents.Count); //just a line

            //block region line = textbegin, chars, textend
            Assert.AreEqual("In normal content flow", ((second.Columns[0].Contents[0] as PDFLayoutLine).Runs[1] as PDFTextRunCharacter).Characters);
        }

   
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockRelativeToParent()
        {
            

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = 150,
                Width = 200,
                X = 50,
                Y = 100,
                PositionMode = Drawing.PositionMode.Relative,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            relative.Contents.Add(new TextLiteral("Sits relative to the parent div"));
            

            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(relative);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);
            

            using (var ms = DocStreams.GetOutputStream("Positioned_BlockRelativeToParent.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);
            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            //Positioned region should be mapped to the same dimensions as the inner block.
            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(100, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(150, posReg.TotalBounds.Height);

            Assert.AreEqual(1, posReg.Contents.Count);
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, posBlock.TotalBounds.X); //offsets on the region
            Assert.AreEqual(0, posBlock.TotalBounds.Y); //offsets on the region
            Assert.AreEqual(200, posBlock.TotalBounds.Width);
            Assert.AreEqual(150, posBlock.TotalBounds.Height);

            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(250, first.Height); //Includes the relative block

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockAbsoluteToParent()
        {


            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div absolute = new Div()
            {
                Height = 150,
                Width = 200,
                X = 50,
                Y = 100,
                PositionMode = Drawing.PositionMode.Absolute,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            absolute.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(absolute);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockAbsoluteToParent.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(100, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(150, posReg.TotalBounds.Height);

            Assert.AreEqual(1, posReg.Contents.Count);
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, posBlock.TotalBounds.X); //offsets on the region
            Assert.AreEqual(0, posBlock.TotalBounds.Y); //offsets on the region
            Assert.AreEqual(200, posBlock.TotalBounds.Width);
            Assert.AreEqual(150, posBlock.TotalBounds.Height);

            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(25, first.Height); //DOES NOT Include the absolute block

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }


        /// <summary>
        /// The absolutely positioned block is positioned beyond the size of the page, so should not be shown
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockAbsoluteExplicitOverPageSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div absolute = new Div()
            {
                Height = 150,
                Width = 200,
                X = 50,
                Y = 800,
                PositionMode = Drawing.PositionMode.Absolute,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            absolute.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(absolute);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockAbsoluteExplicitOverPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            //Nothing in the region - has been reset
            Assert.AreEqual(0, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);
            Assert.AreEqual(0, posReg.TotalBounds.Width); 
            Assert.AreEqual(0, posReg.TotalBounds.Height); 

            Assert.AreEqual(0, posReg.Contents.Count);
            //var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(25, first.Height); //Just the line

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }

        /// <summary>
        /// The absolutely positioned block is positioned beyond the size of the page, but page is clipped, so should all be there
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockAbsoluteExplicitClippedOverPageSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            section.OverflowAction = OverflowAction.Clip;
            doc.Pages.Add(section);

            Div absolute = new Div()
            {
                Height = 150,
                Width = 200,
                X = 50,
                Y = 800,
                PositionMode = Drawing.PositionMode.Absolute,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            absolute.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(absolute);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockAbsoluteExplicitClippedOverPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            //Nothing in the region - has been reset
            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(800, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(150, posReg.TotalBounds.Height);

            Assert.AreEqual(1, posReg.Contents.Count);
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, posBlock.TotalBounds.X); //offsets on the region
            Assert.AreEqual(0, posBlock.TotalBounds.Y); //offsets on the region
            Assert.AreEqual(200, posBlock.TotalBounds.Width);
            Assert.AreEqual(150, posBlock.TotalBounds.Height); //we explicitly fit

            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(25, first.Height); //Just the line

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }


        /// <summary>
        /// No explicit height on the absolute div, so layout should stop when the second line overflows the page
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockAbsoluteFlowOverPageSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            //section.OverflowAction = OverflowAction.Clip;
            doc.Pages.Add(section);

            Div absolute = new Div()
            {
                //Height = 150, - no explicit height
                Width = 200,
                X = 50,
                Y = 800,
                PositionMode = Drawing.PositionMode.Absolute,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            absolute.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(absolute);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockAbsoluteFlowOverToParent.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(800, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(25, posReg.TotalBounds.Height); //we only have 1 line that will fit

            Assert.AreEqual(1, posReg.Contents.Count);
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, posBlock.TotalBounds.X); //offsets on the region
            Assert.AreEqual(0, posBlock.TotalBounds.Y); //offsets on the region
            Assert.AreEqual(200, posBlock.TotalBounds.Width);
            Assert.AreEqual(25, posBlock.TotalBounds.Height); //we only have 1 line that will fit

            Assert.AreEqual(2, posBlock.Columns[0].Contents.Count); //should have both lines

            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(25, first.Height); //DOES NOT Include the absolute block

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }


        /// <summary>
        /// The section has an overflow action of clipped so the content that is absolutely positioned beyond the outside of the page
        /// will continue to be rendered.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockAbsoluteFlowClippedOverPageSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            section.OverflowAction = OverflowAction.Clip; //Set to clipped so the content is there
            doc.Pages.Add(section);

            Div absolute = new Div()
            {
                
                Width = 200,
                X = 50,
                Y = 800,
                PositionMode = Drawing.PositionMode.Absolute,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            absolute.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(absolute);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockAbsoluteFlowClippedToParent.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(800, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(50, posReg.TotalBounds.Height); //both lines fit

            Assert.AreEqual(1, posReg.Contents.Count);
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, posBlock.TotalBounds.X); //offsets on the region
            Assert.AreEqual(0, posBlock.TotalBounds.Y); //offsets on the region
            Assert.AreEqual(200, posBlock.TotalBounds.Width);
            Assert.AreEqual(50, posBlock.TotalBounds.Height); //both lines fit

            Assert.AreEqual(2, posBlock.Columns[0].Contents.Count); //should have both lines

            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(25, first.Height); //DOES NOT Include the absolute block

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }

        /// <summary>
        /// The relative positioned block is positioned beyond the size of the page,
        /// but as it is relative it increases the size of the parent block.
        /// Unusual Scenario, but the page is overflown and rendered there.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockRelativeExplicitOverPageSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div absolute = new Div()
            {
                Height = 150,
                Width = 200,
                X = 50,
                Y = 800,
                PositionMode = Drawing.PositionMode.Relative,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            absolute.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(absolute);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockRelativeExplicitOverPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(2, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(1, firstLine.Runs.Count);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;

            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(800, posReg.TotalBounds.Y);
            Assert.AreEqual(0, posReg.TotalBounds.Width); //Nothing in
            Assert.AreEqual(0, posReg.TotalBounds.Height); //Nothing in

            Assert.AreEqual(0, posReg.Contents.Count);

            //second page

            content = layout.AllPages[1].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);

            var second = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, second.PositionedRegions.Count);
            Assert.AreEqual(1, second.Columns.Length);

            Assert.AreEqual(content.Width, second.Width); //full width
            Assert.AreEqual(800 + 150, second.Height); //Pushed over

            var secondReg = second.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, secondReg.Contents.Count);


            var secondLine = secondReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, secondLine.Runs.Count);

            posRun = secondLine.Runs[0] as PDFLayoutPositionedRegionRun;

            Assert.IsNotNull(posRun);
            Assert.AreEqual(second.PositionedRegions[0], posRun.Region);

            posReg = second.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(800, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width); //Nothing in
            Assert.AreEqual(150, posReg.TotalBounds.Height); //Nothing in

            Assert.AreEqual(1, posReg.Contents.Count);

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (secondLine.Runs[2] as PDFTextRunCharacter).Characters);
        }

        /// <summary>
        /// The relative positioned block is positioned beyond the size of the page,
        /// As it is relative it increases the size of the parent block. The page is set to clip, so everything should still be shown
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockRelativeExplicitClippedOverPageSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            section.OverflowAction = OverflowAction.Clip;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = 150,
                Width = 200,
                X = 50,
                Y = 800,
                PositionMode = Drawing.PositionMode.Relative,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            relative.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(relative);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockRelativeExplicitClippedOverPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            //Matches the size of the relative block
            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(800, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width); 
            Assert.AreEqual(150, posReg.TotalBounds.Height); 

            Assert.AreEqual(1, posReg.Contents.Count);
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, posBlock.TotalBounds.X); //offsets on the region
            Assert.AreEqual(0, posBlock.TotalBounds.Y); //offsets on the region
            Assert.AreEqual(200, posBlock.TotalBounds.Width);
            Assert.AreEqual(150, posBlock.TotalBounds.Height); //both lines fit


            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(950, first.Height); //The offset and the height of the relative block are used

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }

        /// <summary>
        /// The relative positioned block is positioned beyond the size of the page, without an explicit height
        /// As it is relative it increases the size of the parent block.
        /// However the parent is not overflowing so the overflowing second line of the relative block should be hidden, and height adjusted appropraitely
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockRelativeFlowOverPageSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                //Height = 150, - No height, so trucated rather than overflowing.
                Width = 200,
                X = 50,
                Y = 800,
                PositionMode = Drawing.PositionMode.Relative,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            relative.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(relative);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockRelativeFlowOverPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            
            //Matches the size of the relative block
            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(800, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(25, posReg.TotalBounds.Height); // just one line

            Assert.AreEqual(1, posReg.Contents.Count);
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, posBlock.TotalBounds.X); //offsets on the region
            Assert.AreEqual(0, posBlock.TotalBounds.Y); //offsets on the region
            Assert.AreEqual(200, posBlock.TotalBounds.Width);
            Assert.AreEqual(25, posBlock.TotalBounds.Height); //just one line fits



            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(825, first.Height); //Just the offset + line

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }


        /// <summary>
        /// The relative positioned block is positioned beyond the size of the page, without an explicit height
        /// As it is relative it increases the size of the parent block.
        /// However the parent is not clipped so the second line of the relative block should be there, and height adjusted appropriately
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockRelativeFlowClippedOverPageSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            section.OverflowAction = OverflowAction.Clip;
            doc.Pages.Add(section);

            Div absolute = new Div()
            {
                //Height = 150,
                Width = 200,
                X = 50,
                Y = 800,
                PositionMode = Drawing.PositionMode.Relative,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            absolute.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(absolute);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockRelativeFlowClippedOverPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);


            //Matches the size of the relative block
            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(800, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(50, posReg.TotalBounds.Height); // both lines

            Assert.AreEqual(1, posReg.Contents.Count);
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, posBlock.TotalBounds.X); //offsets on the region
            Assert.AreEqual(0, posBlock.TotalBounds.Y); //offsets on the region
            Assert.AreEqual(200, posBlock.TotalBounds.Width);
            Assert.AreEqual(50, posBlock.TotalBounds.Height); //both lines fit



            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(850, first.Height); //The offset and both lines

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }


        /// <summary>
        /// The relative positioned block is positioned beyond the size of the page, without an explicit height
        /// As it is relative it increases the size of the parent block.
        /// However the page is not clipped so the the relative block should be on a new page with the parent block there, and height adjusted appropriately
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockRelativeOverflowingToNewPage()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            section.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(section);

            Div top = new Div() { Height = 800, BorderColor = Drawing.StandardColors.Aqua, ID = "top" };
            section.Contents.Add(top);
            top.Contents.Add("Pushes the relative content down over to the next page");

            Div relative = new Div()
            {
                //Height = 150,
                Width = 200,
                X = 50,
                Y = 100,
                PositionMode = Drawing.PositionMode.Relative,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                ID = "relative"
            };
            relative.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue, ID = "inflow" };
            //Add the relative block to the inflow div.
            //inflow.Contents.Add(new TextLiteral("Before the relative div."));

            inflow.Contents.Add(relative);

            inflow.Contents.Add(new TextLiteral("After the relative div."));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockRelativeOverflowingNewPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(2, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            //top block and inflow index 0 - that is empty and excluded
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var topBlock = content.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(800, topBlock.Height);

            //There is a block where inflow was satarted but it has no content
            //so excluded from output
            var excluded = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(excluded);
            Assert.AreEqual(inflow, excluded.Owner);
            Assert.IsTrue(excluded.ExcludeFromOutput);

            //move to the second page
            content = layout.AllPages[1].ContentBlock;
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(1, content.Columns[0].Contents.Count);

            var inflowBlock = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(inflowBlock);
            Assert.AreEqual(1, inflowBlock.Columns.Length);
            Assert.AreEqual(1, inflowBlock.Columns[0].Contents.Count);

            var inflowLine = inflowBlock.Columns[0].Contents[0] as PDFLayoutLine;

            Assert.IsNotNull(inflowLine);
            Assert.AreEqual(4, inflowLine.Runs.Count);

            
            var relativeRun = inflowLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(relativeRun);

            Assert.AreEqual(1, inflowBlock.PositionedRegions.Count);

            var relativeRegion = inflowBlock.PositionedRegions[0];
            Assert.AreEqual(50.0, relativeRegion.TotalBounds.X);
            Assert.AreEqual(100.0, relativeRegion.TotalBounds.Y);
            Assert.AreEqual(200.0, relativeRegion.Width);

            //Make sure we are pointing to the correct region
            Assert.AreEqual(relativeRun.Region, relativeRegion);

            Assert.AreEqual(1, relativeRegion.Contents.Count);

            var relativeBlock = relativeRegion.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, relativeBlock.OffsetX);
            Assert.AreEqual(0, relativeBlock.OffsetY);

            Assert.AreEqual(200, relativeBlock.Width);
            Assert.AreEqual(50, relativeBlock.Height); //two lines of 25 leading text

            //Check the rest of the inflow line
            Assert.AreEqual("After the relative div.", (inflowLine.Runs[2] as PDFTextRunCharacter).Characters);
        }

    }
}
