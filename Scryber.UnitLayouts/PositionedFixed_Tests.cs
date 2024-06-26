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
    public class PositionedFixed_Tests
    {
        const string TestCategoryName = "Layout Fixed";

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



    }
}
