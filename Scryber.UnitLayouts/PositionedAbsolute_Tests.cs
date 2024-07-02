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

            //As parent is relative margins are included.
            Unit width = layout.AllPages[0].Width - ((20 + 20) * 2); //margin width on nest and page * 2

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
        public void Absolute_07_BlockMarginsPaddingTest()
        {
            var path = AssertGetContentFile("AbsoluteBlockMarginsPadding");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_07_BlockMarginsPadding.pdf"))
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
        public void Absolute_08_BlockNestedMarginsPaddingTest()
        {
            var path = AssertGetContentFile("AbsoluteBlockNestedMarginsPadding");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_08_BlockNestedMarginsPadding.pdf"))
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
        public void Absolute_09_BlockNestedMarginsPaddingFullWidthTest()
        {
            var path = AssertGetContentFile("AbsoluteBlockNestedMarginsPaddingFullWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_09_BlockNestedMarginsPaddingFullWidth.pdf"))
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
            Unit width = layout.AllPages[0].Width - (60 + 20 + 40); //body margins, body padding, nested margins but not the nested padding

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
        public void Absolute_10_BlockLeftPosition()
        {
            var path = AssertGetContentFile("AbsoluteBlockLeftPosition");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_10_BlockLeftPosition.pdf"))
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
        public void Absolute_11_BlockLeftPositionMargins()
        {
            var path = AssertGetContentFile("AbsoluteBlockLeftPositionMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_11_BlockLeftPositionMargins.pdf"))
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
        public void Absolute_12_BlockLeftPositionNestedMargins()
        {
            var path = AssertGetContentFile("AbsoluteBlockLeftPositionNestedMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_12_BlockLeftPositionNestedMargins.pdf"))
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
            Unit xOffset = 50; //explicit value to nest
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            yOffset += 30 + 20; //top margins and nested margins
            xOffset += 30 + 20; //top margins and nested margins


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
        public void Absolute_13_BlockLeftPositionNestedMarginsFullWidth()
        {
            var path = AssertGetContentFile("AbsoluteBlockLeftPositionNestedMarginsFullWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_13_BlockLeftPositionNestedMarginsFullWidth.pdf"))
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
            Unit width = layout.AllPages[0].Width - ((20 + 30) * 2); //page width - the margins of the page and the nesting block

            yOffset += 30 + 20; //top margins and nested margins
            xOffset += 30 + 20; //top margins and nested margins


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
        public void Absolute_14_BlockLeftPositionNestedMarginsHalfWidth()
        {
            var path = AssertGetContentFile("AbsoluteBlockLeftPositionNestedMarginsHalfWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_14_BlockLeftPositionNestedMarginsHalfWidth.pdf"))
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
            Unit width = (layout.AllPages[0].Width - (40 + 60)) / 2; //50% width of the page - magins of page and nesting block

            yOffset += 30 + 20; //top margins and nested margins
            xOffset += 30 + 20; //left margins and nested margins


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
        public void Absolute_15_BlockLeftPositionNestedMarginsHalfWidthTextRight()
        {
            var path = AssertGetContentFile("AbsoluteBlockLeftPositionNestedMarginsHalfWidthTextRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_15_BlockLeftPositionNestedMarginsHalfWidthTextRight.pdf"))
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
            Unit width = (layout.AllPages[0].Width - (40 + 60)) / 2; //50% width remaining after page and nesting margins

            yOffset += 30 + 20; //top margins and nested margins
            xOffset += 30 + 20; //left margins and nested margins;


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
        public void Absolute_16_BlockLeftPositionPageHeader()
        {
            var path = AssertGetContentFile("AbsoluteBlockLeftPositionPageHeader");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_16_BlockLeftPositionPageHeader.pdf"))
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
            Unit width = layout.AllPages[0].Width;

            yOffset += 15; //Page header;
            yOffset += 30 + 20; //Margins page and nest
            xOffset += 30 + 20; //Margins page and nest


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
            xOffset = 50; //back to the start of the nested block

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
        public void Absolute_17_BlockRightPosition()
        {
            var path = AssertGetContentFile("AbsoluteBlockRightPosition");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_17_BlockRightPosition.pdf"))
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
        public void Absolute_18_BlockRightPositionMargins()
        {
            var path = AssertGetContentFile("AbsoluteBlockRightPositionMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_18_BlockRightPositionMargins.pdf"))
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
        public void Absolute_19_BlockRightPositionNestedMargins()
        {
            var path = AssertGetContentFile("AbsoluteBlockRightPositionNestedMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_19_BlockRightPositionNestedMargins.pdf"))
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
            Unit xOffset = (layout.AllPages[0].Width - (30 + 20)) - 50; //explicit right value based on the right of the relative nesting parent
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            yOffset += 30 + 20; //top margins and nested margins
            
            


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
        public void Absolute_20_BlockRightPositionNestedMarginsFullWidth()
        {
            var path = AssertGetContentFile("AbsoluteBlockRightPositionNestedMarginsFullWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_20_BlockRightPositionNestedMarginsFullWidth.pdf"))
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
            xOffset -= 30 + 20; //right page and nested margins;
            width -= 60 + 40; //width is relative to nested so all horizontal margins


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
        public void Absolute_21_BlockRightPositionNestedMarginsHalfWidth()
        {
            var path = AssertGetContentFile("AbsoluteBlockRightPositionNestedMarginsHalfWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_21_BlockRightPositionNestedMarginsHalfWidth.pdf"))
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
            Unit width = layout.AllPages[0].Width;

            yOffset += 30 + 20; //top margins and nested margins
            xOffset -= 30 + 20; //right page and nested margins;
            width -= 60 + 40; //width is relative to nested so all horizontal margins
            width /= 2; //50% width after the margins are removed.

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
        public void Absolute_22_BlockRightPositionNestedMarginsHalfWidthToPage()
        {
            //Although nested, the absolute div should be positioned to the page, as the nest div is not relatively positioned. 
            var path = AssertGetContentFile("AbsoluteBlockRightPositionNestedMarginsHalfWidthToPage");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_22_BlockRightPositionNestedMarginsHalfWidthToPage.pdf"))
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
            Unit width = layout.AllPages[0].Width; //50% width

            yOffset += 30 + 20; //top margins and nested margins
            
            width /= 2; //50% width to the page.


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
        public void Absolute_23_BlockRightPositionPageHeader()
        {
            
            var path = AssertGetContentFile("AbsoluteBlockRightPositionPageHeader");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_23_BlockRightPositionPageHeader.pdf"))
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
            Unit width = layout.AllPages[0].Width;

            yOffset += 30 + 20; //Page header + nest margins;
            xOffset -= 20; //nest margins

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
            xOffset = 20; //back to the start - nested left margin

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
        public void Absolute_24_BlockTopPosition()
        {
            var path = AssertGetContentFile("AbsoluteBlockTopPosition");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_24_BlockTopPosition.pdf"))
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
        public void Absolute_25_BlockTopPositionMargins()
        {
            var path = AssertGetContentFile("AbsoluteBlockTopPositionMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_25_BlockTopPositionMargins.pdf"))
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
        public void Absolute_26_BlockTopPositionNestedMargins()
        {
            var path = AssertGetContentFile("AbsoluteBlockTopPositionNestedMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_26_BlockTopPositionNestedMargins.pdf"))
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

            yOffset += 30 + 50 + 20; //page margins, h5 height, nesting margins


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
        public void Absolute_27_BlockTopPositionNestedMarginsFullWidth()
        {
            var path = AssertGetContentFile("AbsoluteBlockTopPositionNestedMarginsFullWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_27_BlockTopPositionNestedMarginsFullWidth.pdf"))
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

            yOffset += 30 + 50 + 20; //page margins, h5 height, nesting margins
            width -= 40 + 60; //page margins, nesting margins


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
        public void Absolute_28_BlockTopPositionNestedMarginsHalfWidth()
        {
            var path = AssertGetContentFile("AbsoluteBlockTopPositionNestedMarginsHalfWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_28_BlockTopPositionNestedMarginsHalfWidth.pdf"))
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


            Unit yOffset = 100;//explicit value
            Unit xOffset = 30 + 20; //body and nested margins
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            yOffset += 30 + 50 + 20; //page margins, h5 height, nesting margins
            width -= 40 + 60; //page margins, nesting margins
            width /= 2;

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
        public void Absolute_29_BlockTopPositionPageHeader()
        {

            var path = AssertGetContentFile("AbsoluteBlockTopPositionPageHeader");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_29_BlockTopPositionPageHeader.pdf"))
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


            Unit yOffset = 100;//explicit value
            Unit xOffset = 30 + 20; //page margins and nest margins
            Unit height = 15;
            Unit width = layout.AllPages[0].Width;

            yOffset += 30 + 50 + 20; //page margins, h5 height, nesting margins
            yOffset += 30; //page header
            width -= 40 + 60; //page margins, nesting margins

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


            yOffset = new Unit(10 + 10 + 30) + 30 + 30 + 20; //reset to body and header with marginst for page and nest
            

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

        //TODO: Deeply nested and absolute in fixed etc.

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_41_BlockDeeplyNested()
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

            Assert.Inconclusive();

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

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_42_BlockDeeplyNestedTopLeft()
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

            Assert.Inconclusive();

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


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_43_BlockDeeplyNestedBottomRight()
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

            Assert.Inconclusive();

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

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_44_BlockDeeplyNestedBottomRightWidthHeight()
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

            Assert.Inconclusive();

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


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_44_BlockInFixedNestedBottomRightWidthHeight()
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

            Assert.Inconclusive();

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

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Fixed_44_BlockInAbsoluteNestedBottomRightWidthHeight()
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

            Assert.Inconclusive();

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
