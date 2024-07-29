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
            var path = AssertGetContentFile("Absolute_01_Block");

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
            var path = AssertGetContentFile("Absolute_02_BlockMargins");

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
            var path = AssertGetContentFile("Absolute_03_BlockFullWidth");

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
            var path = AssertGetContentFile("Absolute_04_BlockNestedFullWidth");

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
            var path = AssertGetContentFile("Absolute_05_BlockNestedMargins");

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
            var path = AssertGetContentFile("Absolute_06_BlockNestedFullWidthMargins");

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
            xOffset += 20 + 20; //body Margins and nest margins;

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
            var path = AssertGetContentFile("Absolute_07_BlockMarginsPadding");

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
            var path = AssertGetContentFile("Absolute_08_BlockNestedMarginsPadding");

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
            var path = AssertGetContentFile("Absolute_09_BlockNestedMarginsPaddingFullWidth");

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
            var path = AssertGetContentFile("Absolute_10_BlockLeftPosition");

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
            var path = AssertGetContentFile("Absolute_11_BlockLeftPositionMargins");

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
            var path = AssertGetContentFile("Absolute_12_BlockLeftPositionNestedMargins");

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
            var path = AssertGetContentFile("Absolute_13_BlockLeftPositionNestedMarginsFullWidth");

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
            var path = AssertGetContentFile("Absolute_14_BlockLeftPositionNestedMarginsHalfWidth");

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
            var path = AssertGetContentFile("Absolute_15_BlockLeftPositionNestedMarginsHalfWidthTextRight");

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

            //var left = right - (arrangeBefore.RenderBounds.Width + arrangeAfter.RenderBounds.Width);

            Assert.AreEqual(yOffset, arrangeBefore.RenderBounds.Y);
            Assert.AreEqual(yOffset, arrangeAfter.RenderBounds.Y);
            //Assert.AreEqual(right, arrangeBefore.RenderBounds.X + arrangeBefore.RenderBounds.Width + arrangeAfter.RenderBounds.Width);
        }



        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_16_BlockLeftPositionPageHeader()
        {
            var path = AssertGetContentFile("Absolute_16_BlockLeftPositionPageHeader");

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
            var path = AssertGetContentFile("Absolute_17_BlockRightPosition");

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
            var path = AssertGetContentFile("Absolute_18_BlockRightPositionMargins");

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
            var path = AssertGetContentFile("Absolute_19_BlockRightPositionNestedMargins");

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
            var path = AssertGetContentFile("Absolute_20_BlockRightPositionNestedMarginsFullWidth");

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
            var path = AssertGetContentFile("Absolute_21_BlockRightPositionNestedMarginsHalfWidth");

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
            Unit width = layout.AllPages[0].Width; //explicit width

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
            var path = AssertGetContentFile("Absolute_22_BlockRightPositionNestedMarginsHalfWidthToPage");

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
            
            var path = AssertGetContentFile("Absolute_23_BlockRightPositionPageHeader");

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
            var path = AssertGetContentFile("Absolute_24_BlockTopPosition");

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
            var path = AssertGetContentFile("Absolute_25_BlockTopPositionMargins");

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
            var path = AssertGetContentFile("Absolute_26_BlockTopPositionNestedMargins");

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
            var path = AssertGetContentFile("Absolute_27_BlockTopPositionNestedMarginsFullWidth");

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
            var path = AssertGetContentFile("Absolute_28_BlockTopPositionNestedMarginsHalfWidth");

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

            var path = AssertGetContentFile("Absolute_29_BlockTopPositionPageHeader");

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
        public void Absolute_30_BlockBottomPosition()
        {
            var path = AssertGetContentFile("Absolute_30_BlockBottomPosition");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absdolute_30_BlockBottomPosition.pdf"))
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

            Unit yOffset = layout.AllPages[0].Height - (100 + 15);//explicit bottom position and line height of absolute
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
        public void Absolute_31_BlockBottomPositionMargins()
        {
            var path = AssertGetContentFile("Absolute_31_BlockBottomPositionMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_31_BlockBottomPositionMargins.pdf"))
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
        public void Absolute_32_BlockBottomPositionNestedMargins()
        {
            var path = AssertGetContentFile("Absolute_32_BlockBottomPositionNestedMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_32_BlockBottomPositionNestedMargins.pdf"))
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


            Unit yOffset = (30 + 50 + 20 + 15); //bottom of the nesting block
            Unit xOffset = 30 + 20; //body and nested margins
            Unit height = 15 * 2; // 2 lines of content
            //Unit width = layout.AllPages[0].Width;

            yOffset -= 20; //take off the explicit bottom value

            //explicit x should ignore the margins


            Assert.AreEqual(yOffset, content.TotalBounds.Y + content.TotalBounds.Height);
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
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y + arrange.RenderBounds.Height);
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
        public void Absolute_33_BlockBottomPositionNestedMarginsFullWidth()
        {
            var path = AssertGetContentFile("Absolute_33_BlockBottomPositionNestedMarginsFullWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_33_BlockBottomPositionNestedMarginsFullWidth.pdf"))
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

            Unit yOffset = (30 + 50 + 20 + 15); //bottom of the nesting block
            Unit xOffset = 30 + 20; //body and nested margins
            Unit height = 15 * 2; // 2 lines of content
            Unit width = layout.AllPages[0].Width - 60 - 40; //page width - page margins and nesting margins

            yOffset -= 20; //take off the explicit bottom value



            //explicit x should ignore the margins


            Assert.AreEqual(yOffset, content.TotalBounds.Y + content.TotalBounds.Height);
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
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y + arrange.RenderBounds.Height);
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
        public void Absolute_34_BlockBottomPositionNestedMarginsHalfWidth()
        {
            var path = AssertGetContentFile("Absolute_34_BlockBottomPositionNestedMarginsHalfWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_34_BlockBottomPositionNestedMarginsHalfWidth.pdf"))
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



            Unit yOffset = (30 + 50 + 20 + 15); //bottom of the nesting block
            Unit xOffset = 30 + 20; //body and nested margins
            Unit height = 15 * 2; // 2 lines of content
            Unit width = layout.AllPages[0].Width - 60 - 40; //page width - page margins and nesting margins

            yOffset -= 20; //take off the explicit bottom value
            width /= 2; //50% width


            //explicit x should ignore the margins


            Assert.AreEqual(yOffset, content.TotalBounds.Y + content.TotalBounds.Height);
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
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y + arrange.RenderBounds.Height);
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
        public void Absolute_35_BlockBottomPositionPageHeader()
        {

            var path = AssertGetContentFile("Absolute_35_BlockBottomPositionPageHeader");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_35_BlockBottomPositionPageHeader.pdf"))
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
        public void Absolute_36_BlockBottomPositionNestedPageHeader()
        {

            var path = AssertGetContentFile("Absolute_36_BlockBottomPositionNestedPageHeader");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_36_BlockBottomPositionNestedPageHeader.pdf"))
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


            Unit yOffset = (30 + 50 + 20 + 15); //bottom of the nesting block
            Unit xOffset = 30 + 20; //body and nested margins
            Unit height = 15; // 2 lines of content
            //Unit width = layout.AllPages[0].Width - 60 - 40; //page width - page margins and nesting margins

            yOffset += 30; //add the page header
            yOffset -= 20; //take off the explicit bottom value


            Assert.AreEqual(yOffset, content.TotalBounds.Y + content.TotalBounds.Height);
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
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y + arrange.RenderBounds.Height);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset = new Unit(10 + 10 + 30) + 30 + 30 + 20; //reset to body and header + body and nest margins
            xOffset = 30 + 20; //body and nest margins

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
        public void Absolute_37_BlockTopLeftPositionMarginsHalfWidth()
        {
            var path = AssertGetContentFile("Absolute_37_BlockTopLeftPositionMarginsHalfWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_37_BlockTopLeftPositionMarginsHalfWidth.pdf"))
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


            Unit yOffset = 80;//explicit fixed value
            Unit xOffset = 70; //explicit fixed value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width / 2; //50% width


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
            Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset = new Unit(10 + 10 + 30 + 30);//h5 top & bottom margin + h5 line height + margins
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
        public void Absolute_38_BlockTopLeftPositionNestedMarginsHalfWidth()
        {
            var path = AssertGetContentFile("Absolute_38_BlockTopLeftPositionNestedMarginsHalfWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_38_BlockTopLeftPositionNestedMarginsHalfWidth.pdf"))
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


            Unit yOffset = new Unit(10 + 10 + 30 + 30 + 20);//h5 top & bottom margin + h5 line height + page margins + nesting margin top
            Unit xOffset = new Unit(30 + 20); //nesting and page margins
            Unit height = 15;
            Unit width = layout.AllPages[0].Width - (40 + 60); //50% width

            yOffset += 80; // explicit top from relative
            xOffset += 70; // explicit left from relative
            width = width / 2; //50% width of relative


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


            yOffset = new Unit(10 + 10 + 30 + 30 + 20);//h5 top & bottom margin + h5 line height + page margins + nesting margin top
            xOffset = new Unit(30 + 20); //nesting and page margins

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
        public void Absolute_39_BlockBottomRightPositionMarginsExplicitWidth()
        {
            var path = AssertGetContentFile("Absolute_39_BlockBottomRightPositionMarginsExplicitWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_39_BlockBottomRightPositionMarginsExplicitWidth.pdf"))
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


            Unit yOffset = layout.AllPages[0].Height - (80 + 15);//explicit bottom fixed value + height of line
            Unit xOffset = layout.AllPages[0].Width - (70 + 150); //explicit right fixed value + explicit width
            Unit height = 15;
            Unit width = 150; //explicit width




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
            Assert.AreEqual(width, block.TotalBounds.Width);

            //Arrangement is for links and inner content references
            var div = block.Owner as Div;
            var arrange = div.GetFirstArrangement();



            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);


            yOffset = new Unit(10 + 10 + 30 + 30);//h5 top & bottom margin + h5 line height + margins
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
        public void Absolute_40_BlockBottomRightPositionNestedMarginsExplicitWidth()
        {
            var path = AssertGetContentFile("Absolute_40_BlockBottomRightPositionNestedMarginsExplicitWidth");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_40_BlockBottomRightPositionNestedMarginsExplicitWidth.pdf"))
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


            Unit yOffset = new Unit(10 + 10 + 30 + 30 + 20 + 15);//h5 top & bottom margin + h5 line height + page margins + nesting margin top and content height
            Unit xOffset = layout.AllPages[0].Width - new Unit(30 + 20); //nesting and page margins from the right
            Unit height = 15;
            Unit width = 150; //explicit width

            yOffset -= 80; //explicit bottom
            xOffset -= 70; //explicit right


            Assert.AreEqual(yOffset, content.TotalBounds.Y + content.TotalBounds.Height);
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
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y + content.TotalBounds.Height);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X + content.TotalBounds.Width);
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
        public void Absolute_41_BlockTopLeftSecondPage()
        {
            var path = AssertGetContentFile("Absolute_41_BlockTopLeftPositionSecondPage");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_41_BlockTopLeftSecondPage.pdf"))
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


            Unit yOffset = 30 + 20;//margin top page and nesting 
            Unit xOffset = 30 + 20; //margin left page and nesting
            Unit height = 15;
            Unit width = layout.AllPages[1].Width - (60 + 40); //width minus margins

            yOffset += 10; //explicit top value relative to nesting
            xOffset += 20; //explicit left value relative to nesting
            width /= 2; // 50% width

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
        public void Absolute_42_BlockTopLeftOverflow()
        {
            var path = AssertGetContentFile("Absolute_42_BlockTopLeftPositionOverflow");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_42_BlockTopLeftOverflow.pdf"))
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

            Unit yOffset = 30 + 20;//body and nest margins
            Unit xOffset = 30 + 20; //body and nest margins
            Unit height = 15;
            Unit width = layout.AllPages[1].Width - (60 + 40); //page - margins of body and nest

            yOffset += 80;//absolute position to relative nesting
            xOffset += 70;//absolute position to relative nesting
            width /= 2; //50% width

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
        public void Absolute_43_BlockTopLeftClipped()
        {
            var path = AssertGetContentFile("Absolute_43_BlockTopLeftPositionClipped");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_43_BlockTopLeftClipped.pdf"))
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

            Unit yOffset = 30 + 10 + 750 + 10 + 20;//body top margin, heading + margin, nesting margin top
            Unit xOffset = 30 + 20; //body lft margin + nesting left margin
            Unit height = 15;
            Unit width = layout.AllPages[0].Width - (60 + 40); //width - margins of body and nesting

            yOffset -= 80; //top position
            xOffset += 70; //left position
            width /= 2; //50% width

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
        public void Absolute_44_BlockNestedPositionedRelative()
        {
            var path = AssertGetContentFile("Absolute_44_BlockTopLeftPositionNestedPositionedRelative");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_44_BlockNestedPositionedRelative.pdf"))
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

            Unit yOffset = 30 + 10 + 10 + 60 + 20;//top margins, h5 (2 lines) and margins, and nesting margin
            Unit xOffset = 30 + 20; //left margins, nesting left margin
            Unit height = 15;
            Unit width = layout.AllPages[0].Width - (60 + 40); //page width - margins

            yOffset += 60; //nesting relative offset
            xOffset += 40; //nesting relative offset

            yOffset += 20; //absolute offset
            xOffset += 30; //absolute offset

            width /= 2; //50% width

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


            //Check that the before and after back in the relative position
            yOffset -= 20;

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_45_BlockNestedPositionedMultiRelative()
        {
            var path = AssertGetContentFile("Absolute_45_BlockTopLeftPositionNestedPositionedMultiRelative");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_45_BlockNestedPositionedMultiRelative.pdf"))
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



            //first level nested block
            block = block.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsFalse(block.HasPositionedRegions);

            Assert.AreEqual(3, block.Columns[0].Contents.Count);

            //second level nested block
            block = block.Columns[0].Contents[1] as PDFLayoutBlock;

            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0]  as PDFLayoutPositionedRegion;
            Assert.IsNotNull(content);

            Unit yOffset = 30 + 10 + 60 + 10 + 20;//top margins, h5 (2 lines) and margins, and nesting margin
            Unit xOffset = 30 + 20; //left margins, nesting left margin
            Unit height = 15;
            Unit width = layout.AllPages[0].Width - (60 + 40 + 20); //page width - margins of body, nesting


            
            yOffset += 60; //outer nesting top position
            yOffset += 15; //outer nesting line
            yOffset += 10; //inner nesting top margin
            yOffset += 30; //inner nesting top position

            xOffset += 40; //outer nesting left position
            xOffset += 10; //inner nesting left margin
            xOffset += 20; //inner nesting relative offset

            yOffset += 30; //absolute offset
            xOffset += 20; //absolute offset

            width /= 2; //50% width

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


            //Check that the before and after back in the relative position
            yOffset -= 30;

            var before = layout.DocumentComponent.FindAComponentById("before2");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after2");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_46_BlockNestedPositionedMultiRelativeRight()
        {
            var path = AssertGetContentFile("Absolute_46_BlockTopLeftPositionNestedPositionedMultiRelativeRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_46_BlockNestedPositionedMultiRelativeRight.pdf"))
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



            //first level nested block
            block = block.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsFalse(block.HasPositionedRegions);

            Assert.AreEqual(3, block.Columns[0].Contents.Count);

            //second level nested block
            block = block.Columns[0].Contents[1] as PDFLayoutBlock;

            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(content);

            Unit yOffset = 30 + 10 + 60 + 10 + 20;//top margins, h5 (2 lines) and margins, and nesting margin
            Unit xOffset = 30 + 20; //left margins, nesting left margin
            Unit height = 15;
            Unit width = layout.AllPages[0].Width - (60 + 40 + 20); //page width - margins of body, nesting



            yOffset += 20; //outer nesting top position
            yOffset += 15; //outer nesting line
            yOffset += 10; //inner nesting top margin
            yOffset += 30; //inner nesting top position

            xOffset -= 40; //outer nesting right position
            xOffset += 10; //inner nesting left margin
            xOffset += 20; //inner nesting relative offset

            yOffset += 30; //absolute offset
            xOffset += 20; //absolute offset

            width /= 2; //50% width

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


            //Check that the before and after back in the relative position
            yOffset -= 30;

            var before = layout.DocumentComponent.FindAComponentById("before2");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after2");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_47_BlockPositionedMultiColumn()
        {
            var path = AssertGetContentFile("Absolute_47_BlockLeftPositionMultiColumn");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_47_AbsoluteBlockLeftPositionMultiColumn.pdf"))
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

            //Page 1 has the heading and the line

            var block = layout.AllPages[0].ContentBlock;
            Assert.AreEqual(2, block.Columns.Length);

            Assert.AreEqual(1, block.Columns[0].Contents.Count);
            Assert.AreEqual(1, block.Columns[1].Contents.Count);


            //page content block has the positioned block
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(content);

            Unit yOffset = 10 + 15;//Runs on the second column push the absolute down, the heading on the first column does nothing to the offset.
            Unit xOffset = 50; //explicit value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width; //page width


            width /= 2; //50% width

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


            //Check that the before and after top of the second column
            yOffset = 10; //margins

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_48_BlockPositionedNestedMultiColumn()
        {
            var path = AssertGetContentFile("Absolute_48_BlockLeftPositionNestedMultiColumn");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_48_BlockPositionedNestedMultiColumn.pdf"))
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
            Assert.AreEqual(2, block.Columns.Length);
            Assert.AreEqual(1, block.Columns[0].Contents.Count);
            Assert.AreEqual(1, block.Columns[1].Contents.Count);


            //nested block
            block = block.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(1, block.Columns[0].Contents.Count);

            
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(content);

            Unit yOffset = 10 + 15;//top margins,nesting line
            Unit columnWidth = (layout.AllPages[0].Width - 30) / 2;
            Unit xOffset = 10 + columnWidth + 10; //left margins, column, column gutter
            Unit height = 15;
            Unit width = columnWidth; //100% column width

            xOffset += 50; //absolute offset to relative;
            width /= 2; //50% width

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


            //Check that the before and after back in the relative position
            yOffset = 10;

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_49_BlockPositionedMultiColumnRight()
        {
            var path = AssertGetContentFile("Absolute_49_BlockPositionMultiColumnRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_49_AbsoluteBlockLeftPositionMultiColumnRight.pdf"))
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

            //Page 1 has the heading and the line

            var block = layout.AllPages[0].ContentBlock;
            Assert.AreEqual(2, block.Columns.Length);

            Assert.AreEqual(1, block.Columns[0].Contents.Count);
            Assert.AreEqual(1, block.Columns[1].Contents.Count);


            //page content block has the positioned block
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(content);

            Unit yOffset = 10 + 15;//Runs on the second column push the absolute down, the heading on the first column does nothing to the offset.
            Unit xOffset =layout.AllPages[0].Width - 50; //explicit right value
            Unit height = 15;
            Unit width = layout.AllPages[0].Width; //page width


            width /= 2; //50% width

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


            //Check that the before and after top of the second column
            yOffset = 10; //margins

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_50_BlockPositionedNestedMultiColumnRight()
        {
            var path = AssertGetContentFile("Absolute_50_BlockRightPositionNestedMultiColumn");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_50_BlockPositionedNestedMultiColumnRight.pdf"))
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
            Assert.AreEqual(2, block.Columns.Length);
            Assert.AreEqual(1, block.Columns[0].Contents.Count);
            Assert.AreEqual(1, block.Columns[1].Contents.Count);


            //nested block
            block = block.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(1, block.Columns[0].Contents.Count);


            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(content);

            Unit yOffset = 10 + 15;//top margins,nesting line
            Unit columnWidth = (layout.AllPages[0].Width - 30) / 2;
            Unit xOffset = layout.AllPages[0].Width - (10 + 20); //left margins, column, column gutter
            Unit height = 15;
            Unit width = columnWidth - 40; //column width - nesting margins

            yOffset += 20; //nesting block margins
            xOffset -= 50; //absolute offset to relative;
            width /= 2; //50% width

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
            Assert.AreEqual(xOffset, arrange.RenderBounds.X + arrange.RenderBounds.Width);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);


            //Check that the before and after back in the relative position
            yOffset = 10 + 20;

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_51_BlockNestedPositionedNestedAbsolute()
        {
            var path = AssertGetContentFile("Absolute_51_BlockPositionNestedInAbsolute");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_51_BlockNestedPositionedInAbsolute.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            block = nest.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = 10 + 10 + 10 + 30 + 20;//top margins, h5 and margins, and nesting top margin
            Unit xOffset = 10 + 20; //left margins, nesting left margin
            Unit height = 15;
            Unit width = layout.AllPages[0].Width - 40; //page width - page margins

            yOffset += 15; //nesting line height
            

            width /= 2; //50% width

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


            //Check that the before and after back in the line position
            yOffset -= 15;

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_52_BlockNestedPositionedNestedAbsoluteTopLeft()
        {
            var path = AssertGetContentFile("Absolute_52_BlockPositionNestedInAbsoluteTopLeft");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_52_BlockNestedPositionedInAbsoluteTopLeft.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            block = nest.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = 60 + 20;//top nesting explicit position and top margins
            Unit xOffset = 80 + 20; //left nesting explicit position and left margin
            Unit height = 15;
            Unit width = 300; //explicit nesting width

            yOffset += 15; //nesting line height
            

            width /= 2; //50% width

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


            //Check that the before and after back in the line position
            yOffset -= 15;

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_53_BlockNestedPositionedNestedAbsoluteBottomRight()
        {
            var path = AssertGetContentFile("Absolute_53_BlockPositionNestedInAbsoluteBottomRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_53_BlockNestedPositionedInAbsoluteBottomRight.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            block = nest.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = layout.AllPages[0].Height - (60 + 15);//top nesting explicit position from bottom of the page and a line height for the nesting block
            Unit xOffset = layout.AllPages[0].Width - (80 + 300); //left nesting explicit position from right of the page and the explicit width
            Unit height = 15;
            Unit width = 300; //explicit nesting width

            yOffset += 15; //nesting line height
            

            width /= 2; //50% width

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


            //Check that the before and after back in the line position
            yOffset -= 15;

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_54_BlockNestedTopLeftPositionedNestedAbsoluteTopLeft()
        {
            var path = AssertGetContentFile("Absolute_54_BlockPositionTopLeftNestedInAbsoluteTopLeft");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_54_BlockNestedTopLeftPositionedInAbsoluteTopLeft.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            block = nest.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = 60 + 20;//top nesting explicit position and top margins
            Unit xOffset = 80 + 20; //left nesting explicit position and left margin
            Unit height = 15;
            Unit width = 300; //explicit nesting width

            yOffset += 20; //explicit position
            xOffset += 30; //explicit position

            width /= 2; //50% width

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


            //Check that the before and after are at the explicit position
            yOffset = 80;
            xOffset = 100;
            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            xOffset += arrange.RenderBounds.Width;
            
            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_55_BlockNestedBottomRightPositionedNestedAbsoluteTopLeft()
        {
            var path = AssertGetContentFile("Absolute_55_BlockPositionBottomRightNestedInAbsoluteTopLeft");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_55_BlockNestedBottomRightPositionedInAbsoluteTopLeft.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            block = nest.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = 60 + 20 + 15;//top nesting explicit position and top margins + nest height = bottom baseline
            Unit xOffset = 80 + 20; //left nesting explicit position and left margin
            Unit height = 15;
            Unit width = 300; //explicit nesting width

            xOffset += width; //right baseline
            yOffset -= (15 + 20); //explicit position - abs height
            xOffset -= 30; //explicit position from right

            width /= 2; //50% width

            xOffset -= width; //left side

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


            //Check that the before and after are at the explicit position
            yOffset = 80;
            xOffset = 100;
            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            xOffset += arrange.RenderBounds.Width;
            
            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_56_BlockNestedBottomPositionedNestedAbsoluteTopLeft()
        {
            var path = AssertGetContentFile("Absolute_56_BlockPositionBottomNestedInAbsoluteTopLeft");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_56_BlockNestedBottomPositionedInAbsoluteTopLeft.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            block = nest.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = 60 + 20 + 15;//top nesting explicit position and top margins + nest height = bottom baseline
            Unit xOffset = 80 + 20; //left nesting explicit position and left margin
            Unit height = 15;
            Unit width = 300; //explicit nesting width
            
            yOffset -= (15 + 20); //explicit position - abs height

            width /= 2; //50% width

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


            //Check that the before and after are at the explicit position
            yOffset = 80;
            xOffset = 100;
            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            xOffset += arrange.RenderBounds.Width;
            
            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_57_BlockNestedTopPositionedNestedAbsoluteTopLeft()
        {
            var path = AssertGetContentFile("Absolute_57_BlockPositionTopNestedInAbsoluteTopLeft");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_57_BlockNestedTopPositionedInAbsoluteTopLeft.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            block = nest.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = 60 + 20;//top nesting explicit position and top margins + nest height
            Unit xOffset = 80 + 20; //left nesting explicit position and left margin
            Unit height = 15;
            Unit width = 300; //explicit nesting width
            
            yOffset += (20); //explicit position added from the nest top

            width /= 2; //50% width

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


            //Check that the before and after are at the explicit position
            yOffset = 80;
            xOffset = 100;
            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            xOffset += arrange.RenderBounds.Width;
            
            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_58_BlockNestedLeftPositionedNestedAbsoluteTopLeft()
        {
            var path = AssertGetContentFile("Absolute_58_BlockPositionLeftNestedInAbsoluteTopLeft");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_58_BlockNestedLeftPositionedInAbsoluteTopLeft.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            block = nest.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = 60 + 20;//top nesting explicit position and top margins
            Unit xOffset = 80 + 20; //left nesting explicit position and left margin
            Unit height = 15;
            Unit width = 300; //explicit nesting width

            yOffset += 15; //nesting line height
            xOffset += 30; //explicit left position

            width /= 2; //50% width

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


            //Check that the before and after back in the line position
            yOffset -= 15;

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_59_BlockNestedRightPositionedNestedAbsoluteTopLeft()
        {
            var path = AssertGetContentFile("Absolute_59_BlockPositionRightNestedInAbsoluteTopLeft");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_59_BlockNestedRightPositionedInAbsoluteTopLeft.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            block = nest.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = 60 + 20;//top nesting explicit position and top margins
            Unit xOffset = 80 + 20; //left nesting explicit position and left margin
            Unit height = 15;
            Unit width = 300; //explicit nesting width

            yOffset += 15; //nesting line height
            

            width /= 2; //50% width
            xOffset += width - 30; //explicit right position

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


            //Check that the before and after back in the line position
            yOffset -= 15;

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_60_BlockNestedTopLeftPositionedNestedAbsoluteBottomRight()
        {
            var path = AssertGetContentFile("Absolute_60_BlockPositionTopLeftNestedInAbsoluteBottomRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_60_BlockNestedTopLeftPositionedInAbsoluteBottomRight.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            block = nest.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = layout.AllPages[0].Height - (60 + 15);//top nesting explicit position from bottom of the page and a line height for the nesting block
            Unit xOffset = layout.AllPages[0].Width - (80 + 300); //left nesting explicit position from right of the page and the explicit width
            Unit height = 15;
            Unit width = 300; //explicit nesting width

            yOffset += 20; //nesting line height
            xOffset += 60;

            width /= 2; //50% width

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


            //Check that the before and after back in position
            yOffset -= 20;

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_61_BlockNestedBottomRightPositionedNestedAbsoluteBottomRight()
        {
            var path = AssertGetContentFile("Absolute_61_BlockPositionBottomRightNestedInAbsoluteBottomRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_61_BlockNestedBottomRightPositionedInAbsoluteBottomRight.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            block = nest.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = layout.AllPages[0].Height - 60; //bottom nesting explicit position from bottom of the page
            Unit xOffset = layout.AllPages[0].Width - (80 + 300); //left nesting explicit position from right of the page and the explicit width
            Unit height = 15;
            Unit width = 300; //explicit nesting width

            yOffset += 20 - 15; //explicit bottom value - line height
            
            width /= 2; //50% width
            xOffset += width + 10;
            
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


            //Check that the before and after back in position
            yOffset -= 20;

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_62_BlockNestedBottomPositionedNestedAbsoluteBottomRight()
        {
            var path = AssertGetContentFile("Absolute_62_BlockPositionBottomNestedInAbsoluteBottomRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_62_BlockNestedBottomPositionedInAbsoluteBottomRight.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            block = nest.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = layout.AllPages[0].Height - 60; //bottom nesting explicit position from bottom of the page
            Unit xOffset = layout.AllPages[0].Width - (300 + 80); //left nesting explicit position from right of the page + nest right position
            Unit height = 15;
            Unit width = 300; //explicit nesting width

            yOffset += 20 - 15; //explicit bottom value - line height
            
            width /= 2; //50% width
            
            
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


            //Check that the before and after back in position
            yOffset -= 20;

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_63_BlockNestedTopPositionedNestedAbsoluteBottomRight()
        {
            var path = AssertGetContentFile("Absolute_63_BlockPositionTopNestedInAbsoluteBottomRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_63_BlockNestedTopPositionedInAbsoluteBottomRight.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            block = nest.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = layout.AllPages[0].Height - 60; //bottom nesting explicit position from bottom of the page
            Unit xOffset = layout.AllPages[0].Width - (300 + 80); //left nesting explicit position from right of the page + nest right position
            Unit height = 15;
            Unit width = 300; //explicit nesting width

            yOffset -= 20 + 15; //explicit top value from the bottom baseline of the nesting block
            
            width /= 2; //50% width
            
            
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


            //Check that the before and after back in position
            yOffset += 20;

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }

        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_64_BlockNestedLeftPositionedNestedAbsoluteBottomRight()
        {
            var path = AssertGetContentFile("Absolute_64_BlockPositionLeftNestedInAbsoluteBottomRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_64_BlockNestedLeftPositionedInAbsoluteBottomRight.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            block = nest.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = layout.AllPages[0].Height - 60; //bottom nesting explicit position from bottom of the page
            Unit xOffset = layout.AllPages[0].Width - (300 + 80); //left nesting explicit position from right of the page + nest right position
            Unit height = 15;
            Unit width = 300; //explicit nesting width

            //yOffset += 15; //No vertical offset so at the baseline (bottom of nesting block)
            xOffset -= 20; //Explicit left value
            width /= 2; //50% width
            
            
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


            //Check that the before and after back in position
            yOffset -= 15;

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }

        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_65_BlockNestedRightPositionedNestedAbsoluteBottomRight()
        {
            var path = AssertGetContentFile("Absolute_65_BlockPositionRightNestedInAbsoluteBottomRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_65_BlockNestedRightPositionedInAbsoluteBottomRight.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            block = nest.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.IsTrue(block.HasPositionedRegions);

            var content = block.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = layout.AllPages[0].Height - 60; //bottom nesting explicit position from bottom of the page
            Unit xOffset = layout.AllPages[0].Width -  80; //right nesting explicit position from right of the page
            Unit height = 15;
            Unit width = 300; //explicit nesting width

            //yOffset += 15; //No vertical offset so at the baseline (bottom of nesting block)
            xOffset -= (width / 2) - 20; //Width of the absolute block - Explicit right value
            width /= 2; //50% width
            
            
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


            //Check that the before and after back in position
            yOffset -= 15;

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_66_BlockNestedLeftPositionedNestedFixed()
        {
            var path = AssertGetContentFile("Absolute_66_BlockPositionLeftNestedInFixed");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_66_BlockNestedLeftPositionedInFixed.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            //check the position of the fixed nested region
            Assert.AreEqual(300, nest.TotalBounds.Y);
            
            var nestBlock = nest.Contents[0] as PDFLayoutBlock;
            
            Assert.IsNotNull(nestBlock);
            Assert.AreEqual(1, nestBlock.Columns.Length);
            Assert.AreEqual(2, nestBlock.Columns[0].Contents.Count);
            
            var padd1 = nestBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var padd2 = nestBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            
            Assert.IsNotNull(padd1);
            Assert.IsNotNull(padd2);
            
            Assert.AreEqual(0, padd1.TotalBounds.Y.PointsValue);
            Assert.AreEqual(90,padd1.TotalBounds.Height.PointsValue);
            Assert.AreEqual(90, padd2.TotalBounds.Y.PointsValue);
            
            Assert.IsTrue(padd2.HasPositionedRegions);
            
            
            var content = padd2.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = 300 + 90; //top nesting explicit position and padding height
            Unit xOffset = 10; //page margins and first padd padding
            Unit height = 15;
            Unit width = 300; //explicit nesting width

            yOffset += 10 + 15; //Inner padding and bottom of the line
            xOffset += 20; //Explicit position relative to the Fixed
            
            
            
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


            //Check that the before and after back in position
            yOffset -= 15;

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_67_BlockNestedLeftBottomPositionedNestedFixed()
        {
            var path = AssertGetContentFile("Absolute_67_BlockPositionLeftBottomNestedInFixed");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_67_BlockNestedLeftBottomPositionedInFixed.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            //check the position of the fixed nested region
            Assert.AreEqual(100, nest.TotalBounds.Y);
            
            var nestBlock = nest.Contents[0] as PDFLayoutBlock;
            
            Assert.IsNotNull(nestBlock);
            Assert.AreEqual(1, nestBlock.Columns.Length);
            Assert.AreEqual(2, nestBlock.Columns[0].Contents.Count);
            
            var padd1 = nestBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var padd2 = nestBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            
            Assert.IsNotNull(padd1);
            Assert.IsNotNull(padd2);
            
            Assert.AreEqual(0, padd1.TotalBounds.Y.PointsValue);
            Assert.AreEqual(90,padd1.TotalBounds.Height.PointsValue);
            Assert.AreEqual(90, padd2.TotalBounds.Y.PointsValue);
            
            Assert.IsTrue(padd2.HasPositionedRegions);
            
            
            var content = padd2.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = 100 + 90; //top nesting explicit position and padding height
            Unit xOffset = 0; //page padding and first padd padding
            Unit height = 15;
            //Unit width = 300; //explicit nesting width

            yOffset += 20 + 15; //Inner padding and bottom of the line to bottom of inner .padd
            yOffset -= 100; //bottom position taken away
            yOffset -= 15; //munus height takes us to the top of the absolute
            xOffset += 20; //Explicit position relative to the Fixed
            
            
            
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


            //Check that the before and after back in position
            yOffset = 100 + 90 + 10; //x fixed + padd1 height + padd2 padding

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_68_BlockNestedRightTopPositionedNestedFixed()
        {
            var path = AssertGetContentFile("Absolute_68_BlockPositionRightTopNestedInFixed");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_68_BlockNestedRightTopPositionedInFixed.pdf"))
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
            
            Assert.IsTrue(block.HasPositionedRegions);
            
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            //check the position of the fixed nested region
            Assert.AreEqual(100, nest.TotalBounds.Y);
            
            var nestBlock = nest.Contents[0] as PDFLayoutBlock;
            
            Assert.IsNotNull(nestBlock);
            Assert.AreEqual(1, nestBlock.Columns.Length);
            Assert.AreEqual(2, nestBlock.Columns[0].Contents.Count);
            
            var padd1 = nestBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var padd2 = nestBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            
            Assert.IsNotNull(padd1);
            Assert.IsNotNull(padd2);
            
            Assert.AreEqual(0, padd1.TotalBounds.Y.PointsValue);
            Assert.AreEqual(90,padd1.TotalBounds.Height.PointsValue);
            Assert.AreEqual(90, padd2.TotalBounds.Y.PointsValue);
            
            Assert.IsTrue(padd2.HasPositionedRegions);
            
            
            var content = padd2.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = 100; //top nesting explicit position
            Unit xOffset = 400 - 20; //explicit width - right
            Unit height = 10 + 30 + 10; //2 lines and padding
            //Unit width = 300; //explicit nesting width

            
            yOffset += 100; //bottom position taken away
            
            
            
            
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
            Assert.AreEqual(yOffset + 10, arrange.RenderBounds.Y); //Offset for padding
            Assert.AreEqual(xOffset, arrange.RenderBounds.X + 10 + arrange.RenderBounds.Width); //offset for the right with padding
            Assert.AreEqual(height, arrange.RenderBounds.Height + 20); //renderpounds reduced for padding
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            //Check that the before and after back in position
            yOffset = 100 + 90 + 10; //x fixed + padd1 height + padd2 padding

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_69_BlockNestedPositionedMultiNestedFixed()
        {
            var path = AssertGetContentFile("Absolute_69_BlockPositionMultiNestedInFixed");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_69_BlockMultiNestedPositionedInFixed.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Silver;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.2;
                
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");

            //Should not overflow - so just one page
            Assert.AreEqual(1, layout.AllPages.Count);

            //Page 1 has the heading and the nested block

            var block = layout.AllPages[0].ContentBlock;
            Assert.AreEqual(2, block.Columns[0].Contents.Count);
            
            Assert.IsTrue(block.HasPositionedRegions);
            
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            //check the position of the fixed nested region
            Assert.AreEqual(100, nest.TotalBounds.Y);
            
            var nestBlock = nest.Contents[0] as PDFLayoutBlock;
            
            Assert.IsNotNull(nestBlock);
            Assert.AreEqual(1, nestBlock.Columns.Length);
            Assert.AreEqual(2, nestBlock.Columns[0].Contents.Count);
            
            var padd1 = nestBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var padd2 = nestBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            
            Assert.IsNotNull(padd1);
            Assert.IsNotNull(padd2);
            
            Assert.AreEqual(0, padd1.TotalBounds.Y.PointsValue);
            Assert.AreEqual(90,padd1.TotalBounds.Height.PointsValue);
            Assert.AreEqual(90, padd2.TotalBounds.Y.PointsValue);
            
            Assert.IsFalse(padd2.HasPositionedRegions);
            Assert.AreEqual(3, padd2.Columns[0].Contents.Count); //2 lines and an inner padding

            var padd3 = padd2.Columns[0].Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(padd3);
            
            Assert.IsTrue(padd3.HasPositionedRegions);

            var content = padd3.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = 100; //top nesting explicit position
            Unit xOffset = 10 + 30 ; //page padding + padd2 margin
            Unit height = 10 + 15 + 10; //1 line and padding
            //Unit width = 300; //explicit nesting width

            
            yOffset += 90; //first padding height
            yOffset += 30; //padd2 top margin
            yOffset += 15 * 2; //padd2 text content on 2 lines
            yOffset += 10; //padd3 padding
            yOffset += 15; //padd3 line of text

            xOffset += 10; //padd3 padding
            
            
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
            Assert.AreEqual(yOffset + 10, arrange.RenderBounds.Y); //Offset for padding
            Assert.AreEqual(xOffset + 10, arrange.RenderBounds.X); //offset with padding
            Assert.AreEqual(height - 20, arrange.RenderBounds.Height); //renderpounds reduced for padding
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            //Check that the before and after back in position
            yOffset = 100 + 90 + 30 + 30 + 10; //x fixed + padd1 height + padd2 padding

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_70_BlockNestedTopLeftPositionedMultiNestedFixed()
        {
            var path = AssertGetContentFile("Absolute_70_BlockTopLeftPositionMultiNestedInFixed");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_70_BlockTopLeftMultiNestedPositionedInFixed.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Silver;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.2;
                
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");

            //Should not overflow - so just one page
            Assert.AreEqual(1, layout.AllPages.Count);

            //Page 1 has the heading and the nested block

            var block = layout.AllPages[0].ContentBlock;
            Assert.AreEqual(2, block.Columns[0].Contents.Count);
            
            Assert.IsTrue(block.HasPositionedRegions);
            
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            //check the position of the fixed nested region
            Assert.AreEqual(100, nest.TotalBounds.Y);
            
            var nestBlock = nest.Contents[0] as PDFLayoutBlock;
            
            Assert.IsNotNull(nestBlock);
            Assert.AreEqual(1, nestBlock.Columns.Length);
            Assert.AreEqual(2, nestBlock.Columns[0].Contents.Count);
            
            var padd1 = nestBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var padd2 = nestBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            
            Assert.IsNotNull(padd1);
            Assert.IsNotNull(padd2);
            
            Assert.AreEqual(0, padd1.TotalBounds.Y.PointsValue);
            Assert.AreEqual(90,padd1.TotalBounds.Height.PointsValue);
            Assert.AreEqual(90, padd2.TotalBounds.Y.PointsValue);
            
            Assert.IsFalse(padd2.HasPositionedRegions);
            Assert.AreEqual(3, padd2.Columns[0].Contents.Count); //2 lines and an inner padding

            var padd3 = padd2.Columns[0].Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(padd3);
            
            Assert.IsTrue(padd3.HasPositionedRegions);

            var content = padd3.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = 100; //top nesting explicit position
            Unit xOffset = 10 ; //page padding + padd2 margin
            Unit height = 10 + 15 + 10; //1 line and padding
            //Unit width = 300; //explicit nesting width

            
            yOffset += 40; //explicit top
            

            xOffset += 30; //explicit left
            
            
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
            Assert.AreEqual(yOffset + 10, arrange.RenderBounds.Y); //Offset for padding
            Assert.AreEqual(xOffset + 10, arrange.RenderBounds.X); //offset with padding
            Assert.AreEqual(height - 20, arrange.RenderBounds.Height); //renderpounds reduced for padding
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            //Check that the before and after back in position
            yOffset = 100 + 90 + 30 + 30 + 10; //x fixed + padd1 height + padd2 padding

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }
        
         [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_71_BlockNestedBottomRightPositionedMultiNestedFixed()
        {
            var path = AssertGetContentFile("Absolute_71_BlockBottomRightPositionMultiNestedInFixed");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_71_BlockBottomRightMultiNestedPositionedInFixed.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Silver;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.2;
                
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");

            //Should not overflow - so just one page
            Assert.AreEqual(1, layout.AllPages.Count);

            //Page 1 has the heading and the nested block

            var block = layout.AllPages[0].ContentBlock;
            Assert.AreEqual(2, block.Columns[0].Contents.Count);
            
            Assert.IsTrue(block.HasPositionedRegions);
            
            var nest = block.PositionedRegions[0];
            Assert.IsNotNull(nest);
            
            //A single line on the nested positioned region.
            Assert.AreEqual(1, nest.Contents.Count);
            
            //check the position of the fixed nested region
            Assert.AreEqual(100, nest.TotalBounds.Y);
            
            var nestBlock = nest.Contents[0] as PDFLayoutBlock;
            
            Assert.IsNotNull(nestBlock);
            Assert.AreEqual(1, nestBlock.Columns.Length);
            Assert.AreEqual(2, nestBlock.Columns[0].Contents.Count);
            
            var padd1 = nestBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var padd2 = nestBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            
            Assert.IsNotNull(padd1);
            Assert.IsNotNull(padd2);
            
            Assert.AreEqual(0, padd1.TotalBounds.Y.PointsValue);
            Assert.AreEqual(90,padd1.TotalBounds.Height.PointsValue);
            Assert.AreEqual(90, padd2.TotalBounds.Y.PointsValue);
            
            Assert.IsFalse(padd2.HasPositionedRegions);
            Assert.AreEqual(3, padd2.Columns[0].Contents.Count); //2 lines and an inner padding

            var padd3 = padd2.Columns[0].Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(padd3);
            
            Assert.IsTrue(padd3.HasPositionedRegions);

            var content = padd3.PositionedRegions[0];
            
            Assert.IsNotNull(content);

            Unit yOffset = 100; //top nesting explicit position
            Unit xOffset = 10 + 400 ; //explcit width and page margin
            Unit height = 10 + 15 + 10; //1 line and padding
            //Unit width = 300; //explicit nesting width

            yOffset += 90 + 30 + 30 + 10 + 15 + 10 + 30; //padd1, padd2 top margin and content, padd3 padding and content, padd2 bottom margin = bottom baseline
            yOffset -= 40; //explicit bottom
            

            xOffset -= 30; //explicit right
            
            
            Assert.AreEqual(yOffset, content.TotalBounds.Y + content.TotalBounds.Height);
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
            Assert.AreEqual(yOffset , arrange.RenderBounds.Y + arrange.RenderBounds.Height + 10); //Offset for padding bottom for baseline
            Assert.AreEqual(xOffset , arrange.RenderBounds.X + arrange.RenderBounds.Width + 10); //offset with padding right for right hand side
            Assert.AreEqual(height - 20, arrange.RenderBounds.Height); //renderpounds reduced for padding
            //Assert.AreEqual(width, arrange.RenderBounds.Width);


            //Check that the before and after back in position
            yOffset = 100 + 90 + 30 + 30 + 10; //x fixed + padd1 height + padd2 padding

            var before = layout.DocumentComponent.FindAComponentById("before");
            Assert.IsNotNull(before);
            arrange = before.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);

            var after = layout.DocumentComponent.FindAComponentById("after");
            Assert.IsNotNull(after);
            arrange = after.GetFirstArrangement();
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Absolute_99_BlockDeepNestedPositionedNestedAbsoluteTopLeft()
        {
            var path = AssertGetContentFile("Absolute_99_BlockPositionDeepNestedInAbsoluteTopLeft");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Absolute_99_BlockDeepNestedPositionedInAbsoluteTopLeft.pdf"))
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

            //just test the inner blocks compared to their RenderBounds
            //tests above check their Total bounds and hierarchy.

            var comp = doc.FindAComponentById("deepnest1");
            
            Assert.Inconclusive("Need to text the fixed positioning");
        }
        
    }



}
