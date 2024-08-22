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
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Positioning/Inline/" + name + ".html");
            path = System.IO.Path.GetFullPath(path);

            if (!System.IO.File.Exists(path))
                Assert.Inconclusive("The path the file " + name + " was not found at " + path);

            return path;
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_01_ToPage()
        {
            var path = AssertGetContentFile("Inline_01_ToPage");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_01_ToPage.pdf"))
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
        public void Inline_02_WithMarginsTest()
        {
            var path = AssertGetContentFile("Inline_02_WithMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_02_WithMargins.pdf"))
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
        public void Inline_03_NestedWithMarginsTest()
        {
            var path = AssertGetContentFile("Inline_03_NestedWithMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_03_NestedWithMargins.pdf"))
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
        public void Inline_04_NestedSecondWithMarginsTest()
        {
            var path = AssertGetContentFile("Inline_04_NestedSecondWithMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_04_NestedSecondWithMargins.pdf"))
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
            Assert.IsNotNull(block);
            
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
            
            //The x offset should be the width of the line - th content width
            xOffset += 30 + 20 + 10 + (line.Width - arrange.RenderBounds.Width);

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.IsTrue(arrange.RenderBounds.Width > 0);
            Assert.AreEqual(height, arrange.RenderBounds.Height);

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_05_NestedSecondRightWithMarginsTest()
        {
            var path = AssertGetContentFile("Inline_05_NestedSecondRightWithMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_05_NestedSecondRightWithMargins.pdf"))
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


        private void CheckRenderBounds(IComponent forComponent, int arrangeIndex, Unit left, Unit top, Unit width,
            Unit height)
        {
            var comp = forComponent as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            var index = arrangeIndex;
            while (index > 0)
            {
                arrange = arrange.NextArrangement;
                Assert.IsNotNull(arrange, "Arrange bounds not available for arrangement " + arrangeIndex + " on component " + comp.UniqueID);
                index--;

            }

            var bounds = arrange.RenderBounds;
            Assert.AreEqual(left, bounds.X, "Arrange bounds left insets do not match for arrangement " + arrangeIndex + " on component " + comp.UniqueID);
            Assert.AreEqual(top, bounds.Y, "Arrange bounds top offsets do not match for arrangement " + arrangeIndex + " on component " + comp.UniqueID);
            Assert.AreEqual(width, bounds.Width, "Arrange bounds widths do not match for arrangement " + arrangeIndex + " on component " + comp.UniqueID);
            Assert.AreEqual(height, bounds.Height, "Arrange bounds heights do not match for arrangement " + arrangeIndex + " on component " + comp.UniqueID);
            

        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_06_NestedImagesWithExplicitHeightsBaseline()
        {
            var path = AssertGetContentFile("Inline_06_NestedImagesWithExplicitHeightsBaseline");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_06_NestedImagesWithExplicitHeightssBaseline.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var content = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;
            Assert.IsNotNull(content);
            
            

            //Both images of different (scaled) heights should be on the baseline.
            
            
            var nest = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            var imgDim = new Size(682, 452);

            var baseline = Unit.Pt(100);
            
            //Approx equal for the baseline offset
            Assert.AreEqual(baseline, line.BaseLineOffset);

            var runningWidth = Unit.Zero;
            var lgHeight = baseline;
            var textStart = baseline + 30; //body margins
            var smImgHeight = Unit.Pt(30);

            Assert.AreEqual(20, line.Runs.Count);

            //0 = start span
            //1 = text begin
            var begin = line.Runs[1] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height); // + body margin
            Assert.AreEqual(30, begin.StartTextCursor.Width); // + body margin
            
            //2 = text chars
            var chars = line.Runs[2] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //3 = end text
            //4 = end span
            
            //confirm the render bounds rect for the first span - body margins and on the full baseline.
            var left = Unit.Pt(30); //body margins
            var top = 30 + baseline - begin.TextRenderOptions.GetBaselineOffset();
            var width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            var height = begin.TextRenderOptions.GetLineHeight();
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            runningWidth += chars.Width;
            
            //5 = begin whitespace
            //6 = text whitespace
            var whiteSpace = line.Runs[6] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            
            //7 = end whitespace
            
            //8 = small image
            var imgRun = line.Runs[8] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(30, imgRun.TotalBounds.Height);
            Assert.AreEqual(baseline, imgRun.TotalBounds.Y + imgRun.TotalBounds.Height);
            var imgW = (30 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the first image -
            left = 30 + runningWidth ; //body margins
            top = 30 + baseline - 30; //body margins + baseline offsets - img height
            width = imgRun.TotalBounds.Width;
            height = 30;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);

            runningWidth += imgRun.TotalBounds.Width;
            
            //9 begin whitespace
            //10 text whitespace
            whiteSpace = line.Runs[10] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            
            //11 = end whitespace
            
            //12 = large image
            imgRun = line.Runs[12] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(100, imgRun.TotalBounds.Height);
            Assert.AreEqual(baseline, imgRun.TotalBounds.Y + imgRun.TotalBounds.Height);
            imgW = (100 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the second image 
            left = 30 + runningWidth ; //body margins
            top = 30 + baseline - 100; //body margins + baseline offsets - img height
            width = imgRun.TotalBounds.Width;
            height = 100;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);
            
            runningWidth += imgRun.TotalBounds.Width;

            
            
            //13 begin whitespace
            //14 text whitespace
            whiteSpace = line.Runs[14] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            //15 = end whitespace
            
            //16 span begin
            //17 text begin
            begin = line.Runs[17] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height);
            Assert.AreEqual(runningWidth + 30, begin.StartTextCursor.Width);
            
            //18 text chars
            chars = line.Runs[18] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            //19 = newLine
            var newLine = line.Runs[19] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            var offset = newLine.NewLineOffset;
            Assert.AreEqual(runningWidth, offset.Width); //offset from the start of the start chars to the start of the next line
            Assert.AreEqual(30 * 1.2, begin.TextRenderOptions.GetLineHeight()); //default leading 1.2 font size
            Assert.AreEqual(30* 1.2, newLine.NewLineOffset.Height); //same for next line
            
            //confirm the render bounds rect for the second text run 
            left = 30 + runningWidth ; //body margins
            top = 30 + baseline - begin.TextRenderOptions.GetBaselineOffset(); //body margins + baseline offsets - text baseline offset
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = begin.TextRenderOptions.GetLineHeight();
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            runningWidth += chars.Width;
            
            //
            // second line
            //

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            Assert.AreEqual(line.Height, begin.TextRenderOptions.GetLineHeight()); //back to normal height

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //confirm the render bounds rect for the second line of the second span run 
            top += 36; //font size x 1.2
            left = 30; //margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = line.Height;
            CheckRenderBounds(begin.Owner, 1, left, top, width, height);
            
            //
            // third line
            //
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(4, line.Runs.Count); //begin, chars, end, inline end
            Assert.AreEqual(line.Height, begin.TextRenderOptions.GetLineHeight()); //back to normal height

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //confirm the render bounds rect for the second line of the second span run 
            top += 36; //font size x 1.2 - currently 1.1
            left = 30; //margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = line.Height;
            CheckRenderBounds(begin.Owner, 2, left, top, width, height);
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_07_NestedImagesWithExplicitHeightsBottom()
        {
            var path = AssertGetContentFile("Inline_07_NestedImagesWithExplicitHeightsBottom");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_07_NestedImagesWithExplicitHeightsBottom.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var content = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;
            Assert.IsNotNull(content);
            
            

            //Both images of different (scaled) heights should be on the baseline.
            
            
            var nest = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            var imgDim = new Size(682, 452);

            var lineHeight = Unit.Pt(100);
            Assert.AreEqual(lineHeight, line.Height);
            
            

            var runningWidth = Unit.Zero;
            

            Assert.AreEqual(20, line.Runs.Count);
            //pre calc the max baseline offset from the second span with the larger font
            var beginSecond = line.Runs[17] as PDFTextRunBegin;
            Assert.IsNotNull(beginSecond, "The second text start run could not be located so cannot calculate the baseline offset");

            //below baseline is the descender and half the leading
            var belowBaseline = beginSecond.TextRenderOptions.GetDescender() + (beginSecond.TextRenderOptions.GetLineHeight() - beginSecond.TextRenderOptions.GetSize())/2 ; 
            
            
            var baseline = lineHeight - belowBaseline;
            Assert.AreEqual(baseline + 30, beginSecond.StartTextCursor.Height);
            //0 = start span
            //1 = text begin
            var begin = line.Runs[1] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height); // + body margin
            Assert.AreEqual(30, begin.StartTextCursor.Width); // + body margin
            
            //2 = text chars
            var chars = line.Runs[2] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //3 = end text
            //4 = end span
            
            //confirm the render bounds rect for the first span - body margins and on the full baseline.
            var left = Unit.Pt(30); //body margins
            var top = 30 + baseline - begin.TextRenderOptions.GetBaselineOffset();
            var width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            var height = begin.TextRenderOptions.GetLineHeight();
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            runningWidth += chars.Width;
            
            //5 = begin whitespace
            //6 = text whitespace
            var whiteSpace = line.Runs[6] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            
            //7 = end whitespace
            
            //8 = small image
            var imgRun = line.Runs[8] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(30, imgRun.TotalBounds.Height);
            Assert.AreEqual(100, imgRun.TotalBounds.Y + imgRun.TotalBounds.Height);
            var imgW = (30 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the first image -
            left = 30 + runningWidth ; //body margins
            top = 30 + 100 - 30; //body margins + total line height - img height
            width = imgRun.TotalBounds.Width;
            height = 30;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);

            runningWidth += imgRun.TotalBounds.Width;
            
            //9 begin whitespace
            //10 text whitespace
            whiteSpace = line.Runs[10] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            
            //11 = end whitespace
            
            //12 = large image
            imgRun = line.Runs[12] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(100, imgRun.TotalBounds.Height);
            Assert.AreEqual(0, imgRun.TotalBounds.Y);
            imgW = (100 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the second image 
            left = 30 + runningWidth ; //body margins
            top = 30 ; //body margins
            width = imgRun.TotalBounds.Width;
            height = 100;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);
            
            runningWidth += imgRun.TotalBounds.Width;

            
            
            //13 begin whitespace
            //14 text whitespace
            whiteSpace = line.Runs[14] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            //15 = end whitespace
            
            //16 span begin
            //17 text begin
            begin = line.Runs[17] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height);
            Assert.AreEqual(runningWidth + 30, begin.StartTextCursor.Width);
            
            //18 text chars
            chars = line.Runs[18] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            //19 = newLine
            var newLine = line.Runs[19] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            var offset = newLine.NewLineOffset;
            Assert.AreEqual(runningWidth, offset.Width); //offset from the start of the start chars to the start of the next line
            Assert.AreEqual(30 * 1.2, begin.TextRenderOptions.GetLineHeight()); //default leading 1.2 font size
            Assert.AreEqual(30* 1.2, newLine.NewLineOffset.Height); //same for next line
            
            //confirm the render bounds rect for the second text run 
            left = 30 + runningWidth ; //body margins
            top = 30 + baseline - begin.TextRenderOptions.GetBaselineOffset(); //body margins + baseline offsets - text baseline offset
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = begin.TextRenderOptions.GetLineHeight();
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            runningWidth += chars.Width;
            
            //
            // second line
            //

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            Assert.AreEqual(line.Height, begin.TextRenderOptions.GetLineHeight()); //back to normal height

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //confirm the render bounds rect for the second line of the second span run 
            top += 36; //font size x 1.2
            left = 30; //margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = line.Height;
            CheckRenderBounds(begin.Owner, 1, left, top, width, height);
            
            //
            // third line
            //
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(4, line.Runs.Count); //begin, chars, end, inline end
            Assert.AreEqual(line.Height, begin.TextRenderOptions.GetLineHeight()); //back to normal height

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //confirm the render bounds rect for the second line of the second span run 
            top += 36; //font size x 1.2 - currently 1.1
            left = 30; //margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = line.Height;
            CheckRenderBounds(begin.Owner, 2, left, top, width, height);
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_08_NestedImagesWithExplicitHeightsTop()
        {
            var path = AssertGetContentFile("Inline_08_NestedImagesWithExplicitHeightsTop");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_08_NestedImagesWithExplicitHeightsTop.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var content = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;
            Assert.IsNotNull(content);
            
            

            //Both images of different (scaled) heights should be on the baseline.
            
            
            var nest = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            var imgDim = new Size(682, 452);

            var lineHeight = Unit.Pt(100);
            Assert.AreEqual(lineHeight, line.Height);
            
            

            var runningWidth = Unit.Zero;
            

            Assert.AreEqual(20, line.Runs.Count);
            //pre calc the max baseline offset from the second span with the larger font
            var beginSecond = line.Runs[17] as PDFTextRunBegin;
            Assert.IsNotNull(beginSecond, "The second text start run could not be located so cannot calculate the baseline offset");

            var belowBaseline = beginSecond.TextRenderOptions.GetDescender();
            
            //baseline is half second run line height space + font size - the descender
            var baseline = ((beginSecond.TextRenderOptions.GetLineHeight() - beginSecond.TextRenderOptions.GetSize()) / 2) + beginSecond.TextRenderOptions.GetSize() - belowBaseline;
            
            Assert.AreEqual(baseline + 30, beginSecond.StartTextCursor.Height);
            //0 = start span
            //1 = text begin
            var begin = line.Runs[1] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height); // + body margin
            Assert.AreEqual(30, begin.StartTextCursor.Width); // + body margin
            
            //2 = text chars
            var chars = line.Runs[2] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //3 = end text
            //4 = end span
            
            //confirm the render bounds rect for the first span - body margins and on the full baseline.
            var left = Unit.Pt(30); //body margins
            var top = 30 + baseline - begin.TextRenderOptions.GetBaselineOffset();
            var width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            var height = begin.TextRenderOptions.GetLineHeight();
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            runningWidth += chars.Width;
            
            //5 = begin whitespace
            //6 = text whitespace
            var whiteSpace = line.Runs[6] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            
            //7 = end whitespace
            
            //8 = small image
            var imgRun = line.Runs[8] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(0, imgRun.TotalBounds.Y);
            Assert.AreEqual(30, imgRun.TotalBounds.Height);
            Assert.AreEqual(30, imgRun.TotalBounds.Height);
            var imgW = (30 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the first image -
            left = 30 + runningWidth ; //body margins
            top = 30; //body margins + total line height - img height
            width = imgRun.TotalBounds.Width;
            height = 30;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);

            runningWidth += imgRun.TotalBounds.Width;
            
            //9 begin whitespace
            //10 text whitespace
            whiteSpace = line.Runs[10] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            
            //11 = end whitespace
            
            //12 = large image
            imgRun = line.Runs[12] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(100, imgRun.TotalBounds.Height);
            Assert.AreEqual(0, imgRun.TotalBounds.Y);
            imgW = (100 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the second image 
            left = 30 + runningWidth ; //body margins
            top = 30 ; //body margins
            width = imgRun.TotalBounds.Width;
            height = 100;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);
            
            runningWidth += imgRun.TotalBounds.Width;

            
            
            //13 begin whitespace
            //14 text whitespace
            whiteSpace = line.Runs[14] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            //15 = end whitespace
            
            //16 span begin
            //17 text begin
            begin = line.Runs[17] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height);
            Assert.AreEqual(runningWidth + 30, begin.StartTextCursor.Width);
            
            //18 text chars
            chars = line.Runs[18] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            //19 = newLine
            var newLine = line.Runs[19] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            var offset = newLine.NewLineOffset;
            Assert.AreEqual(runningWidth, offset.Width); //offset from the start of the start chars to the start of the next line
            Assert.AreEqual(30 * 1.2, begin.TextRenderOptions.GetLineHeight()); //default leading 1.2 font size
            Assert.AreEqual(100, newLine.NewLineOffset.Height); //from the top to the next line below the images
            
            //confirm the render bounds rect for the second text run 
            left = 30 + runningWidth ; //body margins
            top = 30; //body margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = begin.TextRenderOptions.GetLineHeight();
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            runningWidth += chars.Width;
            
            //
            // second line
            //

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            Assert.AreEqual(line.Height, begin.TextRenderOptions.GetLineHeight()); //back to normal height

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //confirm the render bounds rect for the second line of the second span run 
            top += 100; //add the height of the first line
            left = 30; //margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = line.Height;
            CheckRenderBounds(begin.Owner, 1, left, top, width, height);
            
            //
            // third line
            //
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(4, line.Runs.Count); //begin, chars, end, inline end
            Assert.AreEqual(line.Height, begin.TextRenderOptions.GetLineHeight()); //back to normal height

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //confirm the render bounds rect for the second line of the second span run 
            top += 36; //font size x 1.2 - currently 1.1
            left = 30; //margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = line.Height;
            CheckRenderBounds(begin.Owner, 2, left, top, width, height);
            
        }
        
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_09_NestedImagesWithExplicitHeightsMiddle()
        {
            var path = AssertGetContentFile("Inline_09_NestedImagesWithExplicitHeightsMiddle");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_09_NestedImagesWithExplicitHeightsMiddle.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var content = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;
            Assert.IsNotNull(content);
            
            

            //Both images of different (scaled) heights should be on the baseline.
            
            
            var nest = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            var imgDim = new Size(682, 452);

            var lineHeight = Unit.Pt(100);
            Assert.AreEqual(lineHeight, line.Height);
            
            

            var runningWidth = Unit.Zero;
            

            Assert.AreEqual(20, line.Runs.Count);
            
            
            //baseline is half the total height
            Unit baseline = 100 / 2;
            
            
            //0 = start span
            //1 = text begin
            var begin = line.Runs[1] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height); // + body margin
            Assert.AreEqual(30, begin.StartTextCursor.Width); // + body margin
            
            //2 = text chars
            var chars = line.Runs[2] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //3 = end text
            //4 = end span
            
            //confirm the render bounds rect for the first span - body margins and on the full baseline.
            var left = Unit.Pt(30); //body margins
            var top = 30 + baseline - begin.TextRenderOptions.GetBaselineOffset();
            var width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            var height = begin.TextRenderOptions.GetLineHeight();
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            runningWidth += chars.Width;
            
            //5 = begin whitespace
            //6 = text whitespace
            var whiteSpace = line.Runs[6] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            
            //7 = end whitespace
            
            //8 = small image
            var imgRun = line.Runs[8] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual((100 - 30) / 2, imgRun.TotalBounds.Y);
            Assert.AreEqual(30, imgRun.TotalBounds.Height);
            var imgW = (30 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the first image -
            left = 30 + runningWidth ; //body margins
            top = 30 + (100-30)/2; //body margins + total line height - img height
            width = imgRun.TotalBounds.Width;
            height = 30;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);

            runningWidth += imgRun.TotalBounds.Width;
            
            //9 begin whitespace
            //10 text whitespace
            whiteSpace = line.Runs[10] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            
            //11 = end whitespace
            
            //12 = large image
            imgRun = line.Runs[12] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(100, imgRun.TotalBounds.Height);
            Assert.AreEqual(0, imgRun.TotalBounds.Y);
            imgW = (100 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the second image 
            left = 30 + runningWidth ; //body margins
            top = 30 ; //body margins
            width = imgRun.TotalBounds.Width;
            height = 100;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);
            
            runningWidth += imgRun.TotalBounds.Width;

            
            
            //13 begin whitespace
            //14 text whitespace
            whiteSpace = line.Runs[14] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            //15 = end whitespace
            
            //16 span begin
            //17 text begin
            begin = line.Runs[17] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height);
            Assert.AreEqual(runningWidth + 30, begin.StartTextCursor.Width);
            
            //18 text chars
            chars = line.Runs[18] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            //19 = newLine
            var newLine = line.Runs[19] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            var offset = newLine.NewLineOffset;
            Assert.AreEqual(runningWidth, offset.Width); //offset from the start of the start chars to the start of the next line
            Assert.AreEqual(30 * 1.2, begin.TextRenderOptions.GetLineHeight()); //default leading 1.2 font size
            Assert.AreEqual(line.Height / 2 + begin.TextRenderOptions.GetBaselineOffset(), offset.Height); //from the middle to the baseline of the next line
            
            //confirm the render bounds rect for the second text run 
            left = 30 + runningWidth ; //body margins
            top = 30 + (100/2) - (begin.TextRenderOptions.GetLineHeight() - begin.TextRenderOptions.GetSize()) / 2 - begin.TextRenderOptions.GetAscent(); //body margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = begin.TextRenderOptions.GetLineHeight();
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            runningWidth += chars.Width;
            
            //
            // second line
            //

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            Assert.AreEqual(line.Height, begin.TextRenderOptions.GetLineHeight()); //back to normal height

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //confirm the render bounds rect for the second line of the second span run 
            top = 30 + 100; //reset  to last line height + margins
            left = 30; //margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = line.Height;
            CheckRenderBounds(begin.Owner, 1, left, top, width, height);
            
            //
            // third line
            //
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(4, line.Runs.Count); //begin, chars, end, inline end
            Assert.AreEqual(line.Height, begin.TextRenderOptions.GetLineHeight()); //back to normal height

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //confirm the render bounds rect for the second line of the second span run 
            top += 36; //font size x 1.2 - currently 1.1
            left = 30; //margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = line.Height;
            CheckRenderBounds(begin.Owner, 2, left, top, width, height);
            
        }

        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_10_NestedImagesWithExplicitHeightsPreTop()
        {
            var path = AssertGetContentFile("Inline_10_NestedImagesWithExplicitHeightsPreTop");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_10_NestedImagesWithExplicitHeightsPreTop.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var content = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;
            Assert.IsNotNull(content);
            
            

            //Both images of different (scaled) heights should be on the baseline.
            
            
            var nest = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            var imgDim = new Size(682, 452);

            var baseline = Unit.Pt(100);
            Assert.AreEqual(baseline, line.BaseLineOffset);
            
            

            var runningWidth = Unit.Zero;
            

            Assert.AreEqual(20, line.Runs.Count);
            
            
            //baseline is half the total height
            
            
            
            //0 = start span
            //1 = text begin
            var begin = line.Runs[1] as PDFTextRunBegin;
            //top position
            Assert.IsNotNull(begin);
            baseline = begin.TextRenderOptions.GetBaselineOffset();
            Assert.IsTrue(baseline < 12);//just a manual check that we are less than the font size
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height); // + body margin
            Assert.AreEqual(30, begin.StartTextCursor.Width); // + body margin
            
            //2 = text chars
            var chars = line.Runs[2] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //3 = end text
            //4 = end span
            
            //confirm the render bounds rect for the first span - body margins and on the full baseline.
            var left = Unit.Pt(30); //body margins
            var top = 30 + baseline - begin.TextRenderOptions.GetBaselineOffset();
            var width = chars.Width  + begin.TextRenderOptions.GetLeftSideBearing();
            var height = begin.TextRenderOptions.GetLineHeight();
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            runningWidth += chars.Width;
            
            //5 = begin whitespace
            //6 = text whitespace
            var whiteSpace = line.Runs[6] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            
            //7 = end whitespace
            
            //8 = small image baseline
            var imgRun = line.Runs[8] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual((100 - 30), imgRun.TotalBounds.Y);
            Assert.AreEqual(30, imgRun.TotalBounds.Height);
            var imgW = (30 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the first image -
            left = 30 + runningWidth ; //body margins
            top = 30 + (100-30); //body margins + total line height - img height
            width = imgRun.TotalBounds.Width;
            height = 30;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);

            runningWidth += imgRun.TotalBounds.Width;
            
            //9 begin whitespace
            //10 text whitespace
            whiteSpace = line.Runs[10] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            
            //11 = end whitespace
            
            //12 = large image
            imgRun = line.Runs[12] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(100, imgRun.TotalBounds.Height);
            Assert.AreEqual(0, imgRun.TotalBounds.Y);
            imgW = (100 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the second image 
            left = 30 + runningWidth ; //body margins
            top = 30 ; //body margins
            width = imgRun.TotalBounds.Width;
            height = 100;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);
            
            runningWidth += imgRun.TotalBounds.Width;

            
            
            //13 begin whitespace
            //14 text whitespace
            whiteSpace = line.Runs[14] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            //15 = end whitespace
            
            //16 span begin
            //17 text begin
            begin = line.Runs[17] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(101.44970703125, begin.StartTextCursor.Height);
            Assert.AreEqual(runningWidth + 30, begin.StartTextCursor.Width);
            
            //18 text chars
            chars = line.Runs[18] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            //19 = newLine
            var newLine = line.Runs[19] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            var offset = newLine.NewLineOffset;
            Assert.AreEqual(runningWidth, offset.Width); //offset from the start of the start chars to the start of the next line
            Assert.AreEqual(30 * 1.2, begin.TextRenderOptions.GetLineHeight()); //default leading 1.2 font size
            Assert.AreEqual(61.55029296875, offset.Height); //from the middle to the baseline of the next line
            
            //confirm the render bounds rect for the second text run 
            left = 30 + runningWidth ; //body margins
            top = 75.34912109375; // 30 + (100/2) - (begin.TextRenderOptions.GetLineHeight() - begin.TextRenderOptions.GetSize()) / 2 - begin.TextRenderOptions.GetAscent(); //body margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = begin.TextRenderOptions.GetLineHeight();
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            runningWidth += chars.Width;
            
            //
            // second line
            //

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            Assert.AreEqual(line.Height, begin.TextRenderOptions.GetLineHeight()); //back to normal height

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //confirm the render bounds rect for the second line of the second span run 
            top = 136.8994140625; //30 + 100; //reset  to last line height + margins
            left = 30; //margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = line.Height;
            CheckRenderBounds(begin.Owner, 1, left, top, width, height);
            
            //
            // third line
            //
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count); //begin, chars, end, inline end
            Assert.AreEqual(line.Height, begin.TextRenderOptions.GetLineHeight()); //back to normal height

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //confirm the render bounds rect for the second line of the second span run 
            top += 36; //font size x 1.2 - currently 1.1
            left = 30; //margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = line.Height;
            CheckRenderBounds(begin.Owner, 2, left, top, width, height);
            
        }
    }
}
