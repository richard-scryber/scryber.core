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
    public class PositionedAbsolute_Tests
    {
        const string TestCategoryName = "Layout Absolute";

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
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Positioning/Absolute/" + name + ".html");
            path = System.IO.Path.GetFullPath(path);

            if (!System.IO.File.Exists(path))
                Assert.Inconclusive("The path the file " + name + " was not found at " + path);

            return path;
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_01_BlockTest()
        {
            var path = AssertGetContentFile("AbsoluteBlock");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_01_Block.pdf"))
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
        public void Absolute_02_BlockMarginsTest()
        {
            var path = AssertGetContentFile("AbsoluteBlockMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_02_BlockMargins.pdf"))
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
        public void Absolute_03_BlockFullWidthTest()
        {
            var path = AssertGetContentFile("AbsoluteBlockFullWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_03_BlockFullWidth.pdf"))
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
        public void Absolute_04_BlockNestedFullWidthTest()
        {
            var path = AssertGetContentFile("AbsoluteBlockNestedFullWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_AbsoluteBlock_04_NestedFullWidth.pdf"))
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
        public void Absolute_05_BlockNestedMarginsTest()
        {
            var path = AssertGetContentFile("AbsoluteBlockNestedMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_AbsoluteBlock_05_NestedMargins.pdf"))
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
        public void Absolute_06_BlockNestedFullWidthMarginsTest()
        {
            var path = AssertGetContentFile("AbsoluteBlockNestedFullWidthMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_AbsoluteBlock_06_NestedFullWidthMargins.pdf"))
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
        public void Fixed_07_BlockMarginsPaddingTest()
        {
            var path = AssertGetContentFile("FixedBlockMarginsPadding");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_07_BlockMarginsPadding.pdf"))
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
        public void Fixed_08_BlockNestedMarginsPaddingTest()
        {
            var path = AssertGetContentFile("FixedBlockNestedMarginsPadding");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_08_BlockNestedMarginsPadding.pdf"))
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
        public void Fixed_09_BlockNestedMarginsPaddingFullWidthTest()
        {
            var path = AssertGetContentFile("FixedBlockNestedMarginsPaddingFullWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_09_BlockNestedMarginsPaddingFullWidth.pdf"))
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
        public void Fixed_10_BlockLeftPosition()
        {
            var path = AssertGetContentFile("FixedBlockLeftPosition");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_10_BlockLeftPosition.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = 50; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            


            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);
            //Assert.AreEqual(width, content.TotalBounds.Width);

            var block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);
            //Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset -= height; //Up one line height
            xOffset = 0; //back to the start

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
        public void Fixed_11_BlockLeftPositionMargins()
        {
            var path = AssertGetContentFile("FixedBlockLeftPositionMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_11_BlockLeftPositionMargins.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = 50; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            yOffset += 30; //top margins
            //explicit x should ignore the margins


            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);
            //Assert.AreEqual(width, content.TotalBounds.Width);

            var block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);
            //Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset -= height; //Up one line height
            xOffset = 30; //back to the start with margins

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
        public void Fixed_12_BlockLeftPositionNestedMargins()
        {
            var path = AssertGetContentFile("FixedBlockLeftPositionNestedMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_12_BlockLeftPositionNestedMargins.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = 50; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            yOffset += 30 + 20; //top margins and nested margins
            //explicit x should ignore the margins


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
            //Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset -= height; //Up one line height
            xOffset = 30 + 20; //back to the start with margins

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
        public void Fixed_13_BlockLeftPositionNestedMarginsFullWidth()
        {
            var path = AssertGetContentFile("FixedBlockLeftPositionNestedMarginsFullWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_13_BlockLeftPositionNestedMarginsFullWidth.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = 50; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            yOffset += 30 + 20; //top margins and nested margins
            //explicit x should ignore the margins


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
            xOffset = 30 + 20; //back to the start with margins

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
        public void Fixed_14_BlockLeftPositionNestedMarginsHalfWidth()
        {
            var path = AssertGetContentFile("FixedBlockLeftPositionNestedMarginsHalfWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_14_BlockLeftPositionNestedMarginsHalfWidth.pdf"))
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
            Unit xOffset = 50; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width / 2; //50% width

            yOffset += 30 + 20; //top margins and nested margins
            //explicit x should ignore the margins


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
            xOffset = 30 + 20; //back to the start with margins

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
        public void Fixed_15_BlockLeftPositionNestedMarginsHalfWidthTextRight()
        {
            var path = AssertGetContentFile("FixedBlockLeftPositionNestedMarginsHalfWidthTextRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_15_BlockLeftPositionNestedMarginsHalfWidthTextRight.pdf"))
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
            Unit xOffset = 50; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width / 2; //50% width

            yOffset += 30 + 20; //top margins and nested margins
            //explicit x should ignore the margins


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
            xOffset = 30 + 20; //back to the start with margins

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);

            width = layout.AllPages[0].Width;
            var right = width - (30 + 20); //page and nest margins;

            
            var arrangeBefore = before.GetFirstArrangement();
            var arrangeAfter = after.GetFirstArrangement();

            var left = right - (arrangeBefore.RenderBounds.Width + arrangeAfter.RenderBounds.Width);

            Assert.AreEqual(yOffset, arrangeBefore.RenderBounds.Y);
            Assert.AreEqual(left, arrangeBefore.RenderBounds.X);

            //Next span is XOffset by the width, but on the same line.
            left += arrangeBefore.RenderBounds.Width;

            Assert.AreEqual(yOffset, arrangeAfter.RenderBounds.Y);
            Assert.AreEqual(left, arrangeAfter.RenderBounds.X);
        }



        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_16_BlockLeftPositionPageHeader()
        {
            var path = AssertGetContentFile("FixedBlockLeftPositionPageHeader");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_16_BlockLeftPositionPageHeader.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = 50; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            yOffset += 30; //Page header;


            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);
            //Assert.AreEqual(width, content.TotalBounds.Width);

            var block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);
            //Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset -= height; //Up one line height
            xOffset = 0; //back to the start

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
        public void Fixed_17_BlockRightPosition()
        {
            var path = AssertGetContentFile("FixedBlockRightPosition");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_17_BlockRightPosition.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = layout.AllPages[0].Width - 50; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;




            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X + content.TotalBounds.Width);
            Assert.AreEqual(height, content.TotalBounds.Height);
            //Assert.AreEqual(width, content.TotalBounds.Width);

            var block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);
            //Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X + content.TotalBounds.Width);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset -= height; //Up one line height
            xOffset = 0; //back to the start

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
        public void Fixed_18_BlockRightPositionMargins()
        {
            var path = AssertGetContentFile("FixedBlockRightPositionMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_18_BlockRightPositionMargins.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = layout.AllPages[0].Width - 50; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            yOffset += 20; //body margins;


            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X + content.TotalBounds.Width);
            Assert.AreEqual(height, content.TotalBounds.Height);
            //Assert.AreEqual(width, content.TotalBounds.Width);

            var block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);
            //Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X + content.TotalBounds.Width);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset -= height; //Up one line height
            xOffset = 20; //back to the start with margins

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
        public void Fixed_19_BlockRightPositionNestedMargins()
        {
            var path = AssertGetContentFile("FixedBlockRightPositionNestedMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_19_BlockRightPositionNestedMargins.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = layout.AllPages[0].Width - 50; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            yOffset += 30 + 20; //top margins and nested margins
            
            //explicit x should ignore the margins


            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X + content.TotalBounds.Width);
            Assert.AreEqual(height, content.TotalBounds.Height);
            //Assert.AreEqual(width, content.TotalBounds.Width);

            block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);
            //Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X + content.TotalBounds.Width);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset -= height; //Up one line height
            xOffset = 30 + 20; //back to the start with margins

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
        public void Fixed_20_BlockRightPositionNestedMarginsFullWidth()
        {
            var path = AssertGetContentFile("FixedBlockRightPositionNestedMarginsFullWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_20_BlockRightPositionNestedMarginsFullWidth.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = layout.AllPages[0].Width - 50; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            yOffset += 30 + 20; //top margins and nested margins

            //explicit x should ignore the margins


            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X + content.TotalBounds.Width);
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
            Assert.AreEqual(xOffset, arrange.RenderBounds.X + content.TotalBounds.Width);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset -= height; //Up one line height
            xOffset = 30 + 20; //back to the start with margins

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
        public void Fixed_21_BlockLeftPositionNestedMarginsHalfWidth()
        {
            var path = AssertGetContentFile("FixedBlockRightPositionNestedMarginsHalfWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_21_BlockRightPositionNestedMarginsHalfWidth.pdf"))
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
            Unit xOffset = layout.AllPages[0].Width - 50; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width / 2; //50% width

            yOffset += 30 + 20; //top margins and nested margins
            //explicit x should ignore the margins


            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X + content.TotalBounds.Width);
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
            Assert.AreEqual(xOffset, arrange.RenderBounds.X + content.TotalBounds.Width);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset -= height; //Up one line height
            xOffset = 30 + 20; //back to the start with margins

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
        public void Fixed_22_BlockRightPositionNestedMarginsHalfWidthTextRight()
        {
            var path = AssertGetContentFile("FixedBlockRightPositionNestedMarginsHalfWidthTextRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_22_BlockRightPositionNestedMarginsHalfWidthTextRight.pdf"))
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
            Unit xOffset = layout.AllPages[0].Width - 50; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width / 2; //50% width

            yOffset += 30 + 20; //top margins and nested margins
            //explicit x should ignore the margins


            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X + content.TotalBounds.Width);
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
            Assert.AreEqual(xOffset, arrange.RenderBounds.X + content.TotalBounds.Width);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset -= height; //Up one line height
            xOffset = 30 + 20; //back to the start with margins

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);

            width = layout.AllPages[0].Width;
            var right = width - (30 + 20); //page and nest margins;


            var arrangeBefore = before.GetFirstArrangement();
            var arrangeAfter = after.GetFirstArrangement();

            var left = right - (arrangeBefore.RenderBounds.Width + arrangeAfter.RenderBounds.Width);

            Assert.AreEqual(yOffset, arrangeBefore.RenderBounds.Y);
            Assert.AreEqual(left, arrangeBefore.RenderBounds.X);

            //Next span is XOffset by the width, but on the same line.
            left += arrangeBefore.RenderBounds.Width;

            Assert.AreEqual(yOffset, arrangeAfter.RenderBounds.Y);
            Assert.AreEqual(left, arrangeAfter.RenderBounds.X);
        }



        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_23_BlockRightPositionPageHeader()
        {
            
            var path = AssertGetContentFile("FixedBlockRightPositionPageHeader");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_23_BlockRightPositionPageHeader.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = new Unit(10 + 10 + 30 + 15);//h5 top & bottom margin + h5 line height + span line height.
            Unit xOffset = layout.AllPages[0].Width - 50; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            yOffset += 30; //Page header;


            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X + content.TotalBounds.Width);
            Assert.AreEqual(height, content.TotalBounds.Height);
            //Assert.AreEqual(width, content.TotalBounds.Width);

            var block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);
            //Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X + content.TotalBounds.Width);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset -= height; //Up one line height
            xOffset = 0; //back to the start

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
        public void Fixed_24_BlockTopPosition()
        {
            var path = AssertGetContentFile("FixedBlockTopPosition");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_24_BlockTopPosition.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = 100;//explicit top position
            Unit xOffset = 0;
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;




            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);
            //Assert.AreEqual(width, content.TotalBounds.Width);

            var block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);
            //Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset = new Unit(10 + 10 + 30);//h5 top & bottom margin + h5 line height + span line height.
            xOffset = 0; //back to the start

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
        public void Fixed_25_BlockTopPositionMargins()
        {
            var path = AssertGetContentFile("FixedBlockTopPositionMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_25_BlockTopPositionMargins.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = 100; //explicit value
            Unit xOffset = 20; //body margins
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            yOffset += 0; //body margins ignored


            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);
            //Assert.AreEqual(width, content.TotalBounds.Width);

            var block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);
            //Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset = new Unit(10 + 10 + 30 + 20); //reset to margins
            xOffset = 20; //back to the start with margins

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
        public void Fixed_26_BlockTopPositionNestedMargins()
        {
            var path = AssertGetContentFile("FixedBlockTopPositionNestedMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_26_BlockTopPositionNestedMargins.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = 100;//explicit value
            Unit xOffset = 30 + 20; //body and nested margins
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            

            //explicit x should ignore the margins


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
            //Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset = new Unit(10 + 10 + 30 + 30 + 20); //reset to margins
            xOffset = 30 + 20; //back to the start with margins

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
        public void Fixed_27_BlockTopPositionNestedMarginsFullWidth()
        {
            var path = AssertGetContentFile("FixedBlockTopPositionNestedMarginsFullWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_27_BlockTopPositionNestedMarginsFullWidth.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = 100;//explicit value
            Unit xOffset = 30 + 20; //body and nested margins
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;



            //explicit x should ignore the margins


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


            yOffset = new Unit(10 + 10 + 30 + 30 + 20); //reset to margins
            xOffset = 30 + 20; //back to the start with margins

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
        public void Fixed_28_BlockTopPositionNestedMarginsHalfWidth()
        {
            var path = AssertGetContentFile("FixedBlockTopPositionNestedMarginsHalfWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_28_BlockTopPositionNestedMarginsHalfWidth.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = 100;//explicit value
            Unit xOffset = 30 + 20; //body and nested margins
            Unit height = 15;
            Unit width = layout.AllPages[0].Width / 2; //half width



            //explicit x should ignore the margins


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


            yOffset = new Unit(10 + 10 + 30 + 30 + 20); //reset to margins
            xOffset = 30 + 20; //back to the start with margins

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
        public void Fixed_29_BlockTopPositionPageHeader()
        {

            var path = AssertGetContentFile("FixedBlockTopPositionPageHeader");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_29_BlockTopPositionPageHeader.pdf"))
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


            Unit yOffset = 20;//explicit value
            Unit xOffset = 0; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);
            //Assert.AreEqual(width, content.TotalBounds.Width);

            var block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);
            //Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset = new Unit(10 + 10 + 30) + 30; //reset to body and header
            xOffset = 0; //back to the start with margins

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
        public void Fixed_30_BlockBottomPosition()
        {
            var path = AssertGetContentFile("FixedBlockBottomPosition");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_30_BlockBottomPosition.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = layout.AllPages[0].Height - (100 + 15);//explicit bottom position and line height
            Unit xOffset = 0;
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;




            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);
            //Assert.AreEqual(width, content.TotalBounds.Width);

            var block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);
            //Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset = new Unit(10 + 10 + 30);//h5 top & bottom margin + h5 line height + span line height.
            xOffset = 0; //back to the start

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
        public void Fixed_31_BlockBottomPositionMargins()
        {
            var path = AssertGetContentFile("FixedBlockBottomPositionMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_31_BlockBottomPositionMargins.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = layout.AllPages[0].Height - (100 + 15); //explicit value and line height
            Unit xOffset = 20; //body margins
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            yOffset += 0; //body margins ignored


            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);
            //Assert.AreEqual(width, content.TotalBounds.Width);

            var block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);
            //Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset = new Unit(10 + 10 + 30 + 20); //reset to margins
            xOffset = 20; //back to the start with margins

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
        public void Fixed_32_BlockBottomPositionNestedMargins()
        {
            var path = AssertGetContentFile("FixedBlockBottomPositionNestedMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_32_BlockBottomPositionNestedMargins.pdf"))
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


            Unit yOffset = layout.AllPages[0].Height - (100 + 15); //explicit value + line height from the bottom of the page
            Unit xOffset = 30 + 20; //body and nested margins
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;



            //explicit x should ignore the margins


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
            //Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset = new Unit(10 + 10 + 30 + 30 + 20); //reset to margins
            xOffset = 30 + 20; //back to the start with margins

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
        public void Fixed_33_BlockBottomPositionNestedMarginsFullWidth()
        {
            var path = AssertGetContentFile("FixedBlockBottomPositionNestedMarginsFullWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_33_BlockBottomPositionNestedMarginsFullWidth.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = layout.AllPages[0].Height - (100 + 15); //page height - explicit value and a line.
            Unit xOffset = 30 + 20; //body and nested margins
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;



            //explicit x should ignore the margins


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


            yOffset = new Unit(10 + 10 + 30 + 30 + 20); //reset to margins
            xOffset = 30 + 20; //back to the start with margins

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
        public void Fixed_34_BlockBottomPositionNestedMarginsHalfWidth()
        {
            var path = AssertGetContentFile("FixedBlockBottomPositionNestedMarginsHalfWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_34_BlockBottomPositionNestedMarginsHalfWidth.pdf"))
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



            Assert.IsNotNull(content);

            Unit yOffset = layout.AllPages[0].Height - (100 + 15);//explicit value
            Unit xOffset = 30 + 20; //body and nested margins
            Unit height = 15;
            Unit width = layout.AllPages[0].Width / 2; //half width



            //explicit x should ignore the margins


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


            yOffset = new Unit(10 + 10 + 30 + 30 + 20); //reset to margins
            xOffset = 30 + 20; //back to the start with margins

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
        public void Fixed_35_BlockBottomPositionPageHeader()
        {

            var path = AssertGetContentFile("FixedBlockBottomPositionPageHeader");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_35_BlockBottomPositionPageHeader.pdf"))
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


            Unit yOffset = layout.AllPages[0].Height - (100 + 15);//explicit value
            Unit xOffset = 0; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            Assert.AreEqual(yOffset, content.TotalBounds.Y);
            Assert.AreEqual(xOffset, content.TotalBounds.X);
            Assert.AreEqual(height, content.TotalBounds.Height);
            //Assert.AreEqual(width, content.TotalBounds.Width);

            var block = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            //Block is at offset 0,0 relative to the positioned region
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(height, block.TotalBounds.Height);
            //Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset = new Unit(10 + 10 + 30) + 30; //reset to body and header
            xOffset = 0; //back to the start with margins

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
        public void Fixed_36_BlockTopLeftPositionNestedMarginsHalfWidth()
        {
            var path = AssertGetContentFile("FixedBlockTopLeftPositionNestedMarginsHalfWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_36_BlockTopLeftPositionNestedMarginsHalfWidth.pdf"))
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


            Unit yOffset = 80;//explicit fixed value
            Unit xOffset = 70; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width / 2; //50% width

            


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


            yOffset = new Unit(10 + 10 + 30 + 20 + 30);//h5 top & bottom margin + h5 line height + margins
            xOffset = 30 + 20; //back to the start with margins

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
        public void Fixed_37_BlockBottomRightPositionNestedMarginsExplicitWidth()
        {
            var path = AssertGetContentFile("FixedBlockBottomRightPositionNestedMarginsExplicitWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_37_BlockBottomRightPositionNestedMarginsExplicitWidth.pdf"))
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


            Unit yOffset = layout.AllPages[0].Height - (80 + 15);//explicit bottom fixed value
            Unit xOffset = layout.AllPages[0].Width - (70 + 150); //explicit right fixed value
            Unit height = 15;
            Unit width = 150; //explicit width




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


            yOffset = new Unit(10 + 10 + 30 + 20 + 30);//h5 top & bottom margin + h5 line height + margins
            xOffset = 30 + 20; //back to the start with margins

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
        public void Fixed_38_BlockTopLeftSecondPage()
        {
            var path = AssertGetContentFile("FixedBlockTopLeftPositionSecondPage");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_38_BlockTopLeftSecondPage.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            Assert.AreEqual(2, layout.AllPages.Count);

            //Page 1 just has the heading

            var block = layout.AllPages[0].ContentBlock;
            Assert.AreEqual(1, block.Columns[0].Contents.Count);


            block = layout.AllPages[1].ContentBlock;
            Assert.AreEqual(1, block.Columns[0].Contents.Count);

            //nested block on second page
            block = block.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsTrue(block.HasPositionedRegions);

            

            var content = block.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(content);

            Unit yOffset = 10;//explicit top fixed value
            Unit xOffset = 20; //explicit left fixed value
            Unit height = 15;
            Unit width = layout.AllPages[1].Width / 2; //explicit width


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

            yOffset = 20 + 30;//margins - no heading
            xOffset = 30 + 20; //back to the start with margins

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
        public void Fixed_39_BlockTopLeftOverflow()
        {
            var path = AssertGetContentFile("FixedBlockTopLeftPositionOverflow");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_39_BlockTopLeftOverflow.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            Assert.AreEqual(2, layout.AllPages.Count);

            //Page 1 has the heading and the overflowing nested block

            var block = layout.AllPages[0].ContentBlock;
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            //Page 2 has the rest of the overflowing nested block
            block = layout.AllPages[1].ContentBlock;
            Assert.AreEqual(1, block.Columns[0].Contents.Count);


            //nested block on second page
            block = block.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsTrue(block.HasPositionedRegions);
            var content = block.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(content);

            Unit yOffset = 80;//explicit top fixed value
            Unit xOffset = 70; //explicit left fixed value
            Unit height = 15;
            Unit width = layout.AllPages[1].Width / 2; //explicit width


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

            
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_40_BlockTopLeftClipped()
        {
            var path = AssertGetContentFile("FixedBlockTopLeftPositionClipped");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Fixed_40_BlockTopLeftClipped.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");

            //Should not overflow - so just one page
            Assert.AreEqual(1, layout.AllPages.Count);

            //Page 1 has the heading and the nested block

            var block = layout.AllPages[0].ContentBlock;
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            

            //nested block on second page
            block = block.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsTrue(block.HasPositionedRegions);
            var content = block.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(content);

            Unit yOffset = 80;//explicit top fixed value
            Unit xOffset = 70; //explicit left fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width / 2; //explicit width


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


            //Check that the before and after are on the same page

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(820, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(820, arrange.RenderBounds.Y);
        }
    }



}
