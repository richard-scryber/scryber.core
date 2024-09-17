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
            Unit height, bool ensureIsLast = false)
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

            if (ensureIsLast)
            {
                Assert.IsNull(arrange.NextArrangement, "Arrange bounds was not the last in the linked list, for arrangment " + arrangeIndex + " on component " + comp.UniqueID);
            }

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
            var width = chars.Width; // + begin.TextRenderOptions.GetLeftSideBearing();
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
            top += 36; //font size x 1.2 
            left = 30; //margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing(); //last on the line gets extra lsb
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
            var width = chars.Width;// + begin.TextRenderOptions.GetLeftSideBearing();
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
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing(); //last on the line gets lsb
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
            var width = chars.Width;
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
            var width = chars.Width;
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
            var width = chars.Width;
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
        
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_11_NestedImagesWithExplicitHeightsPreTopMixed()
        {
            var path = AssertGetContentFile("Inline_11_NestedImagesWithExplicitHeightsPreTopMixed");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_11_NestedImagesWithExplicitHeightsPreTopMixed.pdf"))
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
            var width = chars.Width;
            var height = begin.TextRenderOptions.GetLineHeight();
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            runningWidth += chars.Width;
            
            //5 = begin whitespace
            //6 = text whitespace
            var whiteSpace = line.Runs[6] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            
            //7 = end whitespace
            
            //8 = small image top
            var imgRun = line.Runs[8] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(0, imgRun.TotalBounds.Y);
            Assert.AreEqual(30, imgRun.TotalBounds.Height);
            var imgW = (30 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the first image -
            left = 30 + runningWidth ; //body margins
            top = 30; //body margins
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
            Assert.AreEqual(100 + 30, begin.StartTextCursor.Height);
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
            Assert.AreEqual(36, offset.Height); //from the middle to the baseline of the next line
            
            //confirm the render bounds rect for the second text run 
            left = 30 + runningWidth ; //body margins
            top = 30 + 100 - (begin.TextRenderOptions.GetLineHeight() - begin.TextRenderOptions.GetSize()) / 2 - begin.TextRenderOptions.GetAscent(); //body margins
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
            top = 30 + 100 + 36 - (begin.TextRenderOptions.GetLineHeight() - begin.TextRenderOptions.GetSize()) / 2 - begin.TextRenderOptions.GetAscent(); //30 + 100; //reset  to last line height + margins
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
        
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_12_NestedImagesWithExplicitHeightsPreTopMixedLeading()
        {
            var path = AssertGetContentFile("Inline_12_NestedImagesWithExplicitHeightsPreTopMixedLeading");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_12_NestedImagesWithExplicitHeightsPreTopMixedLeading.pdf"))
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
            var width = chars.Width;
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
            Assert.AreEqual(0, imgRun.TotalBounds.Y);
            Assert.AreEqual(30, imgRun.TotalBounds.Height);
            var imgW = (30 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the first image -
            left = 30 + runningWidth ; //body margins
            top = 30; //body margins
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
            Assert.AreEqual(108.44970703125, begin.StartTextCursor.Height);
            Assert.AreEqual(runningWidth + 30, begin.StartTextCursor.Width);
            
            //18 text chars
            chars = line.Runs[18] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            //19 = newLine
            var newLine = line.Runs[19] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            var offset = newLine.NewLineOffset;
            Assert.AreEqual(runningWidth, offset.Width); //offset from the start of the start chars to the start of the next line
            Assert.AreEqual(50, begin.TextRenderOptions.GetLineHeight()); //explicit line height
            Assert.AreEqual(61.55029296875, offset.Height); //from the middle to the baseline of the next line
            
            //confirm the render bounds rect for the second text run 
            left = 30 + runningWidth ; //body margins
            top = 82.34912109375; // 30 + (100/2) - (begin.TextRenderOptions.GetLineHeight() - begin.TextRenderOptions.GetSize()) / 2 - begin.TextRenderOptions.GetAscent(); //body margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = 36; //font size despite the leading of 50
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
            top = 143.8994140625; //30 + 100; //reset  to last line height + margins
            left = 30; //margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = 36; //font size despite the leading of 50
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
            top += 50; //add the leading
            left = 30; //margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = 36; //font size despite the leading of 50
            CheckRenderBounds(begin.Owner, 2, left, top, width, height);
            
        }
        
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_13_NestedImagesSmallMixed()
        {
            var path = AssertGetContentFile("Inline_13_NestedImagesSmallMixed");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_13_NestedImagesSmallMixed.pdf"))
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
            
            

            
            var nest = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            var imgDim = new Size(682, 452);

            

            var runningWidth = Unit.Zero;
            

            Assert.AreEqual(8, line.Runs.Count);
            
            
            //0 = start span
            //1 = text begin
            var begin = line.Runs[1] as PDFTextRunBegin;
            //top position
            Assert.IsNotNull(begin);
            var baseline = begin.TextRenderOptions.GetBaselineOffset();
            Assert.IsTrue(baseline < 22);//just a manual check that we are less than the font size
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height); // + body margin
            Assert.AreEqual(30, begin.StartTextCursor.Width); // + body margin
            
            //2 = text chars
            var chars = line.Runs[2] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //3 = end text
            
            //confirm the render bounds rect for the first span - body margins and on the full baseline.
            var left = Unit.Pt(30); //body margins
            var top = 30 + baseline - begin.TextRenderOptions.GetBaselineOffset();
            var width = chars.Width;
            var height = begin.TextRenderOptions.GetLineHeight();
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            runningWidth += chars.Width;
            
            
            //4 = small image baseline
            var imgRun = line.Runs[4] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(0, imgRun.TotalBounds.Y); //top
            Assert.AreEqual(10, imgRun.TotalBounds.Height);
            var imgW = (10 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the first image -
            left = 30 + runningWidth ; //body margins
            top = 30; //body margins + total line height - img height
            width = imgRun.TotalBounds.Width;
            height = 10;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);

            runningWidth += imgRun.TotalBounds.Width;

            //5 = begin text
            begin = line.Runs[5] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(30 + begin.TextRenderOptions.GetBaselineOffset(), begin.StartTextCursor.Height);
            Assert.AreEqual(runningWidth + 30, begin.StartTextCursor.Width);
            
            //6 text chars
            chars = line.Runs[6] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            //7 = newLine
            var newLine = line.Runs[7] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            var offset = newLine.NewLineOffset;
            Assert.AreEqual(runningWidth, offset.Width); //offset from the start of the start chars to the start of the next line
            Assert.AreEqual(22 * 1.2, begin.TextRenderOptions.GetLineHeight()); //default leading 1.2 font size
            Assert.AreEqual(22 * 1.2, offset.Height); //standard offset of line height
            
            //confirm the render bounds rect for the second text run 
            left = 30 + runningWidth ; //body margins
            top = 30;  //body margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = begin.TextRenderOptions.GetLineHeight();
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            //
            // second line
            //
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            Assert.AreEqual(line.Height, begin.TextRenderOptions.GetLineHeight()); //back to normal height

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //reset running width
            runningWidth = chars.Width;
            
            //confirm the render bounds rect for the second line of the second span run 
            top = 30 + (22 * 1.2); //reset  to last line height + margins
            left = 30; //margins
            width = chars.Width;
            height = line.Height;
            CheckRenderBounds(begin.Owner, 1, left, top, width, height);
            
            //2 = end text
            
            //3 = larger small image
            imgRun = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(15, imgRun.TotalBounds.Height);
            Assert.AreEqual(begin.TextRenderOptions.GetBaselineOffset() - 15 + (22 * 1.2), imgRun.TotalBounds.Y);
            imgW = (15 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the second image 
            left = 30 + runningWidth ; //body margins
            top = 30 + imgRun.TotalBounds.Y; //body margins + offset we know is right above
            width = imgRun.TotalBounds.Width;
            height = 15;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);
            
            runningWidth += imgRun.TotalBounds.Width;


            //4 text begin
            begin = line.Runs[4] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(30 + (22 * 1.2) + begin.TextRenderOptions.GetBaselineOffset(), begin.StartTextCursor.Height); //margin + 1 line + baseline offset
            Assert.AreEqual(runningWidth + 30, begin.StartTextCursor.Width);
            
            //5 text chars
            chars = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            //6 = newLine
            newLine = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            offset = newLine.NewLineOffset;
            Assert.AreEqual(runningWidth, offset.Width); //offset from the start of the start chars to the start of the next line
            Assert.AreEqual(22 * 1.2, begin.TextRenderOptions.GetLineHeight()); //default leading 1.2 font size
            Assert.AreEqual(22 * 1.2, offset.Height); //from the middle to the baseline of the next line
            
            //confirm the render bounds rect for the second text run 
            left = 30 + runningWidth ; //body margins
            top = 30 + (22 * 1.2) ; //body margins + 1 line
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = begin.TextRenderOptions.GetLineHeight();
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            
            
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_14_NestedImagesSmallMixedLeading()
        {
            var path = AssertGetContentFile("Inline_14_NestedImagesSmallMixedLeading");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_14_NestedImagesSmallMixedLeading.pdf"))
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
            
            

            var nest = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            var imgDim = new Size(682, 452);

            

            var runningWidth = Unit.Zero;
            

            Assert.AreEqual(8, line.Runs.Count);
            
            
            
            
            //0 = start span
            //1 = text begin
            var begin = line.Runs[1] as PDFTextRunBegin;
            //top position
            Assert.IsNotNull(begin);
            var baseline = begin.TextRenderOptions.GetBaselineOffset();
            Assert.IsTrue(baseline < 40);//just a manual check that we are less than the line height
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height); // + body margin
            Assert.AreEqual(30, begin.StartTextCursor.Width); // + body margin
            
            //2 = text chars
            var chars = line.Runs[2] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //3 = end text
            
            //confirm the render bounds rect for the first span - body margins and on the full baseline.
            var left = Unit.Pt(30); //body margins
            var space = (Unit) (40 - (22 * 1.2)) / 2; //half total line height - font line height / 2
            var top = 30 + space; // + begin.TextRenderOptions.GetBaselineOffset();
            var width = chars.Width;
            var height = begin.TextRenderOptions.GetSize() * 1.2; //Its the font line height, rather than the full leaded line
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            runningWidth += chars.Width;
            
            
            //4 = small image baseline
            var imgRun = line.Runs[4] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(begin.TextRenderOptions.GetBaselineOffset()- 10, imgRun.TotalBounds.Y); //top
            Assert.AreEqual(10, imgRun.TotalBounds.Height);
            var imgW = (10 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the first image -
            left = 30 + runningWidth ; //body margins
            top = 30 + begin.TextRenderOptions.GetBaselineOffset() - 10; //body margins + total line height - img height
            width = imgRun.TotalBounds.Width;
            height = 10;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);

            runningWidth += imgRun.TotalBounds.Width;

            //5 = begin text
            begin = line.Runs[5] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(30 + begin.TextRenderOptions.GetBaselineOffset(), begin.StartTextCursor.Height);
            Assert.AreEqual(runningWidth + 30, begin.StartTextCursor.Width);
            
            //6 text chars
            chars = line.Runs[6] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            //7 = newLine
            var newLine = line.Runs[7] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            var offset = newLine.NewLineOffset;
            Assert.AreEqual(runningWidth, offset.Width); //offset from the start of the start chars to the start of the next line
            Assert.AreEqual(40, begin.TextRenderOptions.GetLineHeight()); //explicit leading
            Assert.AreEqual(40, offset.Height); //standard offset of line height
            
            //confirm the render bounds rect for the second text run 
            left = 30 + runningWidth ; //body margins
            top = 30 + space;  //body margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = 22 * 1.2; //actual text height
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            //
            // second line
            //
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            Assert.AreEqual(line.Height, begin.TextRenderOptions.GetLineHeight()); //back to normal height

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //reset running width
            runningWidth = chars.Width;
            
            //confirm the render bounds rect for the second line of the second span run 
            top = 30 + 40 + space; //reset  to last line height + margins + line space
            left = 30; //margins
            width = chars.Width;
            height = 22 * 1.2;
            CheckRenderBounds(begin.Owner, 1, left, top, width, height);
            
            //2 = end text
            
            //3 = larger small image
            imgRun = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(15, imgRun.TotalBounds.Height);
            var middle = (Unit) 40 / 2;
            middle -= 15.0 / 2.0;
            Assert.AreEqual(40 + middle, imgRun.TotalBounds.Y); //middle on second line
            imgW = (15 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the second image 
            left = 30 + runningWidth ; //body margins
            top = 30 + imgRun.TotalBounds.Y; //body margins + offset we know is right above
            width = imgRun.TotalBounds.Width;
            height = 15;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);
            
            runningWidth += imgRun.TotalBounds.Width;


            //4 text begin
            begin = line.Runs[4] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(30 + 40  + begin.TextRenderOptions.GetBaselineOffset(), begin.StartTextCursor.Height); //margin + 1 line + baseline offset
            Assert.AreEqual(runningWidth + 30, begin.StartTextCursor.Width);
            
            //5 text chars
            chars = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            //6 = newLine
            newLine = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            offset = newLine.NewLineOffset;
            Assert.AreEqual(runningWidth, offset.Width); //offset from the start of the start chars to the start of the next line
            Assert.AreEqual(40, begin.TextRenderOptions.GetLineHeight()); //default leading 1.2 font size
            Assert.AreEqual(40, offset.Height); //from the middle to the baseline of the next line
            
            //confirm the render bounds rect for the second text run 
            left = 30 + runningWidth ; //body margins
            top = 30 + 40 + space ; //body margins + 1 line + extra space
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = 22 * 1.2;
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
        }
        
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_15_NestedImagesFirstSmallMixed()
        {
            var path = AssertGetContentFile("Inline_15_NestedImagesFirstSmallMixed");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_15_NestedImagesFirstSmallMixed.pdf"))
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
            
            
            var nest = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            var imgDim = new Size(682, 452);

            

            var runningWidth = Unit.Zero;
            

            Assert.AreEqual(9, line.Runs.Count);
            
            
            //0 = start span
            
            //1 = small image top
            var imgRun = line.Runs[1] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(0, imgRun.TotalBounds.Y); //top
            Assert.AreEqual(10, imgRun.TotalBounds.Height);
            var imgW = (10 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places

            
            //confirm the render bounds rect for the first image -
            var left = 30 + runningWidth ; //body margins
            var top = (Unit)30; //body margins + total line height - img height
            var width = imgRun.TotalBounds.Width;
            var height = (Unit)10;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);
            
            runningWidth += imgRun.TotalBounds.Width;
            
            //2 = whitespace begin
            //3 = whitespace
            var chars = line.Runs[3] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            runningWidth += chars.Width;
            
            //4 = whitespace end
            
            //5 = second larger small
            imgRun = line.Runs[5] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(line.BaseLineOffset - 15, imgRun.TotalBounds.Y); //baseline
            Assert.AreEqual(15, imgRun.TotalBounds.Height);
            imgW = (15 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            
            //confirm the render bounds rect for the first image -
            left = 30 + runningWidth ; //body margins
            top =  30 + line.BaseLineOffset - 15; //body margins + baseline - img height
            width = imgRun.TotalBounds.Width;
            height = (Unit)15;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);

            runningWidth += width;
            
            //6 = text begin
            var begin = line.Runs[6] as PDFTextRunBegin;
            

            Assert.IsNotNull(begin);
            var baseline = begin.TextRenderOptions.GetBaselineOffset();
            Assert.IsTrue(baseline < 22);//just a manual check that we are less than the font size
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height); // + body margin
            Assert.AreEqual(30 + runningWidth, begin.StartTextCursor.Width); // + body margin
            
            //7 = text chars
            chars = line.Runs[7] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //8 = end text
            
            //confirm the render bounds rect for the first span - body margins and on the full baseline.
            left = Unit.Pt(30) + runningWidth; //body margins
            top = 30 + baseline - begin.TextRenderOptions.GetBaselineOffset();
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = begin.TextRenderOptions.GetLineHeight();
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            
            
            //
            // second line
            //
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            Assert.AreEqual(line.Height, begin.TextRenderOptions.GetLineHeight()); //back to normal height

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //reset running width
            runningWidth = chars.Width;
            
            //confirm the render bounds rect for the second line of the second span run 
            top = 30 + (22 * 1.2); //reset  to last line height + margins
            left = 30; //margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = line.Height;
            CheckRenderBounds(begin.Owner, 1, left, top, width, height);
            
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_16_NestedImagesFirstSmallMixedLeading()
        {
            var path = AssertGetContentFile("Inline_16_NestedImagesFirstSmallMixedLeading");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_16_NestedImagesFirstSmallMixedLeading.pdf"))
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

            var nest = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            var imgDim = new Size(682, 452);

            

            var runningWidth = Unit.Zero;
            

            Assert.AreEqual(9, line.Runs.Count);
            
            
            //0 = start span
            
            //1 = small image top
            var imgRun = line.Runs[1] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(0, imgRun.TotalBounds.Y); //top
            Assert.AreEqual(10, imgRun.TotalBounds.Height);
            var imgW = (10 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places

            
            //confirm the render bounds rect for the first image -
            var left = 30 + runningWidth ; //body margins
            var top = (Unit)30; //body margins + total line height - img height
            var width = imgRun.TotalBounds.Width;
            var height = (Unit)10;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);
            
            runningWidth += imgRun.TotalBounds.Width;
            
            //2 = whitespace begin
            //3 = whitespace
            var chars = line.Runs[3] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            runningWidth += chars.Width;
            
            //4 = whitespace end
            
            //5 = second larger small
            imgRun = line.Runs[5] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(line.BaseLineOffset - 15, imgRun.TotalBounds.Y); //baseline
            Assert.AreEqual(15, imgRun.TotalBounds.Height);
            imgW = (15 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            
            //confirm the render bounds rect for the first image -
            left = 30 + runningWidth ; //body margins
            top =  30 + line.BaseLineOffset - 15; //body margins + baseline - img height
            width = imgRun.TotalBounds.Width;
            height = (Unit)15;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);

            runningWidth += width;
            
            //6 = text begin
            var begin = line.Runs[6] as PDFTextRunBegin;
            

            Assert.IsNotNull(begin);
            var baseline = begin.TextRenderOptions.GetBaselineOffset();
            Assert.IsTrue(baseline < 40);//just a manual check that we are less than the font size
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height); // + body margin
            Assert.AreEqual(30 + runningWidth, begin.StartTextCursor.Width); // + body margin
            
            //7 = text chars
            chars = line.Runs[7] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //8 = end text
            var space = (Unit)(50 - (22 * 1.2)) / 2;
            //confirm the render bounds rect for the first span - body margins and on the full baseline.
            left = Unit.Pt(30) + runningWidth; //body margins
            top = 30 + space;
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = 22 * 1.2;
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            
            
            //
            // second line
            //
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            Assert.AreEqual(line.Height, begin.TextRenderOptions.GetLineHeight()); //back to normal height

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //reset running width
            runningWidth = chars.Width;
            
            //confirm the render bounds rect for the second line of the second span run 
            top = 30 + 50 + space; //reset  to last line height + margins + leading space
            left = 30; //margins
            width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing();
            height = 22 * 1.2;
            CheckRenderBounds(begin.Owner, 1, left, top, width, height);
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_17_NestedImagesLastSmallMixed()
        {
            var path = AssertGetContentFile("Inline_17_NestedImagesLastSmallMixed");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_17_NestedImagesLastSmallMixed.pdf"))
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
            
            
            var nest = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            var imgDim = new Size(682, 452);

            

            var runningWidth = Unit.Zero;
            

            //first line is simple, start inline, start text, chars, newline
            Assert.AreEqual(4, line.Runs.Count);
            Assert.AreEqual(22 * 1.2, line.Height);
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //second line simple - spacer, chars, new line
            Assert.AreEqual(3, line.Runs.Count);
            Assert.AreEqual(22 * 1.2, line.Height);
            
            //third line has the content
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(9, line.Runs.Count);
            Assert.AreEqual(22 * 1.2, line.Height);
            
            //0 = spacer
            var spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(spacer);
            
            Assert.AreEqual(spacer.Height, 0);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            runningWidth = chars.Width;
            
            //2 = end text
            var end = line.Runs[2] as PDFTextRunEnd;
            Assert.IsNotNull(end);
            Assert.AreEqual(0, end.Width);
            
            //3 = small image top
            var imgRun = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual((22 * 1.2) * 2, imgRun.TotalBounds.Y); //top
            Assert.AreEqual(10, imgRun.TotalBounds.Height);
            var imgW = (10 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places

            
            //confirm the render bounds rect for the first image -
            var left = 30 + runningWidth ; //body margins
            var top = (Unit)30 + ((22* 1.2) * 2); //body margins + 2 lines line height
            var width = imgRun.TotalBounds.Width;
            var height = (Unit)10;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);
            
            runningWidth += imgRun.TotalBounds.Width;
            
            //4 = whitespace begin
            //5 = whitespace
            chars = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            runningWidth += chars.Width;
            
            //6 = whitespace end
            
            //7 = second larger small
            imgRun = line.Runs[7] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(((22 * 1.2) * 2) + line.BaseLineOffset - 15, imgRun.TotalBounds.Y); //2 lines + baseline
            Assert.AreEqual(15, imgRun.TotalBounds.Height);
            imgW = (15 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            
            //confirm the render bounds rect for the second image 
            left = 30 + runningWidth ; //body margins
            top =  30 + ((22 * 1.2) * 2) + line.BaseLineOffset - 15; //body margins + baseline - img height
            width = imgRun.TotalBounds.Width;
            height = (Unit)15;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_18_NestedImagesLastSmallMixedLeading()
        {
            var path = AssertGetContentFile("Inline_18_NestedImagesLastSmallMixedLeading");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_18_NestedImagesLastSmallMixedLeading.pdf"))
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
            
            

            var nest = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            var imgDim = new Size(682, 452);

            

            var runningWidth = Unit.Zero;
            

            //first line is simple, start inline, start text, chars, newline
            Assert.AreEqual(4, line.Runs.Count);
            Assert.AreEqual(50, line.Height);
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //second line simple - spacer, chars, new line
            Assert.AreEqual(3, line.Runs.Count);
            Assert.AreEqual(50, line.Height);
            
            //third line has the content
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(9, line.Runs.Count);
            Assert.AreEqual(50, line.Height);
            
            //0 = spacer
            var spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(spacer);
            
            Assert.AreEqual(spacer.Height, 0);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            runningWidth = chars.Width;
            
            //2 = end text
            var end = line.Runs[2] as PDFTextRunEnd;
            Assert.IsNotNull(end);
            Assert.AreEqual(0, end.Width);
            
            //3 = small image top
            var imgRun = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(50 * 2, imgRun.TotalBounds.Y); //top
            Assert.AreEqual(10, imgRun.TotalBounds.Height);
            var imgW = (10 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places

            
            //confirm the render bounds rect for the first image -
            var left = 30 + runningWidth ; //body margins
            var top = (Unit)30 + (50 * 2); //body margins + 2 lines line height
            var width = imgRun.TotalBounds.Width;
            var height = (Unit)10;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);
            
            runningWidth += imgRun.TotalBounds.Width;
            
            //4 = whitespace begin
            //5 = whitespace
            chars = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            runningWidth += chars.Width;
            
            //6 = whitespace end
            
            //7 = second larger small
            imgRun = line.Runs[7] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual((50 * 2) + line.BaseLineOffset - 15, imgRun.TotalBounds.Y); //2 lines + baseline
            Assert.AreEqual(15, imgRun.TotalBounds.Height);
            imgW = (15 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            
            //confirm the render bounds rect for the second image 
            left = 30 + runningWidth ; //body margins
            top =  30 + (50 * 2) + line.BaseLineOffset - 15; //body margins + baseline - img height
            width = imgRun.TotalBounds.Width;
            height = (Unit)15;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);
            
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_19_NestedImagesLastSmallLargeMixed()
        {
            var path = AssertGetContentFile("Inline_19_NestedImagesLastSmallLargeMixed");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_19_NestedImagesLastSmallLargeMixed.pdf"))
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
            
            

            
            var nest = content.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            var imgDim = new Size(682, 452);

            var spanBegin = line.Runs[0] as PDFLayoutInlineBegin;
            Assert.IsNotNull(spanBegin);

            var begin = line.Runs[1] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            var lead = begin.TextRenderOptions.GetLineHeight() - begin.TextRenderOptions.GetSize();
            var halfLead = lead / 2;
            var descend = begin.TextRenderOptions.GetDescender();
            
            var baseline = 50 - halfLead - descend;
            Assert.AreEqual(baseline, line.BaseLineOffset);
            
            

            var runningWidth = Unit.Zero;
            

            Assert.AreEqual(12, line.Runs.Count);
            
            
            //0 = start span
            //1 = text begin
            
            //top position
            baseline = begin.TextRenderOptions.GetBaselineOffset();
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height); // + body margin
            Assert.AreEqual(30, begin.StartTextCursor.Width); // + body margin
            
            //2 = text chars
            var chars = line.Runs[2] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            //confirm the render bounds rect for the first span - body margins and on the full baseline.
            var left = (Unit)30; //body margins
            var width = chars.Width + chars.ExtraSpace; //justified
            var height = begin.TextRenderOptions.GetSize() * 1.2;
            var top = 30 + ((50 - height)/2); //margins + leading space
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            //3 = end text
            
            //4 = begin small span
            
            runningWidth += chars.Width + chars.ExtraSpace;
            
            //5 = small text begin
            begin = line.Runs[5] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            
            Assert.AreEqual(30 + runningWidth, begin.StartTextCursor.Width);
            Assert.AreEqual(30 + baseline, begin.StartTextCursor.Height);
            
            //6 = small chars
            chars = line.Runs[6] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            left = 30 + runningWidth;
            var smallheight = begin.TextRenderOptions.GetSize() * 1.2;
            width = chars.Width + chars.ExtraSpace;
            halfLead = (3 * 0.2) / 2;
            var smalltop = 30 + line.BaseLineOffset - begin.TextRenderOptions.GetAscent() - halfLead; //from baseline - ascent and half the leading.
            
            CheckRenderBounds(begin.Owner, 0, left, smalltop, width, smallheight);
            
            runningWidth += chars.Width + chars.ExtraSpace;
            
            //7 = end small text
            //8 = end small span
            
            //9 = remaining text begin
            begin = line.Runs[9] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(30 + runningWidth, begin.StartTextCursor.Width);
            Assert.AreEqual(30 + baseline, begin.StartTextCursor.Height);

            //10 = remaining chars
            chars = line.Runs[10] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            left = 30 + runningWidth;
            //height as per first
            //top as per first
            width = chars.Width + chars.ExtraSpace;

            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            runningWidth += chars.Width + chars.ExtraSpace;
            
            
            Assert.AreEqual(runningWidth, nest.Width); //jusified
            
            //11 = new line
            var newLine = line.Runs[11] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(50, newLine.NewLineOffset.Height);
            Assert.AreEqual(nest.Width, newLine.NewLineOffset.Width + (chars.Width + chars.ExtraSpace)); //offset back from the start of the run
            
            
            
            //
            // second line - simple
            //
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //0 = spacer
            var spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(spacer);
            Assert.AreEqual(0, spacer.Width);
            
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual(nest.Width, chars.Width + chars.ExtraSpace);
            
            newLine = line.Runs[2] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(50, newLine.NewLineOffset.Height);
            Assert.AreEqual(0, newLine.NewLineOffset.Width);

            top += 50;
            
            //second arangement on begin owner, left margin, shift down 50, container width and line height
            CheckRenderBounds(begin.Owner, 1, 30, top, nest.Width, height);
            
            
            //
            // third line - simple
            //
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //0 = spacer
            spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(spacer);
            Assert.AreEqual(0, spacer.Width);
            
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual(nest.Width, chars.Width + chars.ExtraSpace);
            
            newLine = line.Runs[2] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(100, newLine.NewLineOffset.Height); // - goes to the taller line with the images on. Currently a fail for the offset
            Assert.AreEqual(0, newLine.NewLineOffset.Width);

            top += 50;
            
            //second arangement on begin owner, left margin, shift down 50, container width and line height
            CheckRenderBounds(begin.Owner, 2, 30, top , nest.Width, height);

            //
            // fourth line
            //
            
            line = nest.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(line);

            Assert.AreEqual(12, line.Runs.Count);
            
            //0 = spacer
            spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(spacer);
            Assert.AreEqual(0, spacer.Width);

            runningWidth = 0;
            
            //1 = chars
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            

            top += 100; //line offset - needs adjustment
            //Third arrangement 
            CheckRenderBounds(begin.Owner, 3, 30, top, chars.Width, height, ensureIsLast : true);

            var end = line.Runs[2] as PDFTextRunEnd;

            runningWidth += chars.Width; //last line is not justified.
            
            
            
            //3 = small image
            var imgRun = line.Runs[3] as PDFLayoutComponentRun;
            
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            
            Assert.AreEqual(150, imgRun.TotalBounds.Y); //top align + 3 lines down
            Assert.AreEqual(10, imgRun.TotalBounds.Height);
            var imgW = (10 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the first image
            left = 30 + runningWidth ; //body margins
            top = 30 + 150; //body margins + 3 lines
            width = imgRun.TotalBounds.Width;
            height = 10;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);

            runningWidth += imgRun.TotalBounds.Width;
            
            //4 begin whitespace
            //5 text whitespace
            var whiteSpace = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            
            //6 = end whitespace
            
            //7 = large image
            imgRun = line.Runs[7] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(100, imgRun.TotalBounds.Height);
            Assert.AreEqual(150, imgRun.TotalBounds.Y);
            imgW = (100 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the second image 
            left = 30 + runningWidth ; //body margins
            top = 30 + 150 ; //body margins + 3 lines
            width = imgRun.TotalBounds.Width;
            height = 100;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);
            
            runningWidth += imgRun.TotalBounds.Width;

            
            
            //8 begin after
            begin = line.Runs[8] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(30 + runningWidth, begin.StartTextCursor.Width);
            //margins + 3 lines + large image height to baseline
            Assert.AreEqual(30 + 150 + 100 , begin.StartTextCursor.Height);
            
            //9 text after
            chars = line.Runs[9] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //10 = end text
            //11 = end span
            
            //confirm the render bounds rect for the second text run 
            left = 30 + runningWidth ; //body margins
            top = 30 + 150 + 100 - ((begin.TextRenderOptions.GetSize() * 0.2) / 2) - begin.TextRenderOptions.GetAscent(); //body margins, 3 lines, img height - half lead and ascent
            width = chars.Width;
            height = begin.TextRenderOptions.GetSize() * 1.2;
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            
            Assert.Inconclusive("Need to fix the vertical offset of the 4th line to the baseline of the large image");
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_20_NestedImagesLastLargeSmallMixed()
        {
            var path = AssertGetContentFile("Inline_20_NestedImagesLastLargeSmallMixed");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_20_NestedImagesLastLargeSmallMixed.pdf"))
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
            Assert.AreEqual(4, line.Runs.Count);
            
            var imgDim = new Size(682, 452);

            var spanBegin = line.Runs[0] as PDFLayoutInlineBegin;
            Assert.IsNotNull(spanBegin);

            
            
            

            var runningWidth = Unit.Zero;
            
            
            //0 = start span
            //1 = text begin
            var begin = line.Runs[1] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            var lead = begin.TextRenderOptions.GetLineHeight() - begin.TextRenderOptions.GetSize();
            var halfLead = lead / 2;
            var descend = begin.TextRenderOptions.GetDescender();
            
            var baseline = 50 - halfLead - descend;
            Assert.AreEqual(baseline, line.BaseLineOffset);
            
            //top position
            baseline = begin.TextRenderOptions.GetBaselineOffset();
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height); // + body margin
            Assert.AreEqual(30, begin.StartTextCursor.Width); // + body margin
            
            //2 = text chars
            var chars = line.Runs[2] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            //confirm the render bounds rect for the first span - body margins and on the full baseline.
            var left = (Unit)30; //body margins
            var width = chars.Width + begin.TextRenderOptions.GetLeftSideBearing(); //left with lsb
            var height = begin.TextRenderOptions.GetSize() * 1.2;
            var top = 30 + ((50 - height)/2); //margins + leading space
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            
            //3 = new line
            var newLine = line.Runs[3] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(50, newLine.NewLineOffset.Height);
            Assert.AreEqual(0, newLine.NewLineOffset.Width); //offset is zero for new full line
            
            
            
            //
            // second line - simple
            //
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //0 = spacer
            var spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(spacer);
            Assert.AreEqual(0, spacer.Width);
            
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual(line.Width, chars.Width);
            
            newLine = line.Runs[2] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(50, newLine.NewLineOffset.Height);
            Assert.AreEqual(0, newLine.NewLineOffset.Width);

            top += 50;
            
            //second arangement on begin owner, left margin, shift down 50, container width and line height
            CheckRenderBounds(begin.Owner, 1, 30, top, chars.Width + begin.TextRenderOptions.GetLeftSideBearing(), height);
            
            
            //
            // third line - span cont. + img + whitespace + image
            //
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(9, line.Runs.Count);
            
            //0 = spacer
            spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(spacer);
            Assert.AreEqual(0, spacer.Width);
            
            //1 = chars
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //2 = end text
            

            top += 50;
            
            //third arangement on begin owner, left margin, shift down 50, character width and line height
            CheckRenderBounds(begin.Owner, 2, 30, top , chars.Width, height);

            runningWidth = chars.Width;
            
            //3 = large image
            var imgRun = line.Runs[3] as PDFLayoutComponentRun;
            
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            
            Assert.AreEqual(100, imgRun.TotalBounds.Y); //top align + 2 lines down
            Assert.AreEqual(100, imgRun.TotalBounds.Height);
            var imgW = (100 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the first image
            left = 30 + runningWidth ; //body margins
            top = 30 + 100; //body margins + 3 lines
            width = imgRun.TotalBounds.Width;
            height = 100;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);

            runningWidth += imgRun.TotalBounds.Width;
            
            //4 begin whitespace
            //5 text whitespace
            var whiteSpace = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            
            //6 = end whitespace
            
            //7 = small image
            imgRun = line.Runs[7] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(10, imgRun.TotalBounds.Height);
            Assert.AreEqual(100 + line.BaseLineOffset - 10, imgRun.TotalBounds.Y); //sits on the baseline, so top is - 10
            imgW = (10 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the second image 
            left = 30 + runningWidth ; //body margins
            top = 30 + 100 + line.BaseLineOffset - 10 ; //body margins + 2 lines and baseline - height
            width = imgRun.TotalBounds.Width;
            height = 10;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);


            //8 - end span
            Assert.IsInstanceOfType<PDFLayoutInlineEnd>(line.Runs[8]);
            
            
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_21_NestedSVGLastLargeSmallMixed()
        {
            var path = AssertGetContentFile("Inline_21_NestedSVGLastLargeSmallMixed");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_21_NestedSVGLastLargeSmallMixed.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout not captured");
            var content = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;
            Assert.IsNotNull(content);
            
            


            //Both images of different (scaled) heights should be on the baseline.

            var topLine = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(topLine);
            Assert.AreEqual(Unit.Pt(300), topLine.Width);
            var svgRun = topLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(svgRun);
            
            var svg = svgRun.Region.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svg);
            var line = svg.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            //Assert.AreEqual(8, line.Runs.Count);
            
            var imgDim = new Size(682, 452);

            var spanBegin = line.Runs[0] as PDFLayoutInlineBegin;
            Assert.IsNotNull(spanBegin);

            Assert.Inconclusive("Needs tests applied");
            
            
            
        }
        
        
         [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_22_NestedImagesLastLargeSmallMixedRight()
        {
            var path = AssertGetContentFile("Inline_22_NestedImagesLastLargeSmallMixedRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_22_NestedImagesLastLargeSmallMixedRight.pdf"))
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
            Assert.AreEqual(4, line.Runs.Count);
            
            var imgDim = new Size(682, 452);

            var spanBegin = line.Runs[0] as PDFLayoutInlineBegin;
            Assert.IsNotNull(spanBegin);

            
            
            

            var runningWidth = Unit.Zero;
            
            
            //0 = start span
            //1 = text begin
            var begin = line.Runs[1] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            var lead = begin.TextRenderOptions.GetLineHeight() - begin.TextRenderOptions.GetSize();
            var halfLead = lead / 2;
            var descend = begin.TextRenderOptions.GetDescender();
            
            var baseline = 50 - halfLead - descend;
            Assert.AreEqual(baseline, line.BaseLineOffset);
            
            //top position
            baseline = begin.TextRenderOptions.GetBaselineOffset();
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height); // + body margin
            Assert.AreEqual(30 + line.RightInset, begin.StartTextCursor.Width); // + body margin and right align
            
            //2 = text chars
            var chars = line.Runs[2] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            //confirm the render bounds rect for the first span - body margins and on the full baseline.
            var left = (Unit)30 + line.RightInset; //body margins
            var width = chars.Width; //left without lsb - as right aligned
            var height = begin.TextRenderOptions.GetSize() * 1.2;
            var top = 30 + ((50 - height)/2); //margins + leading space
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            
            //3 = new line
            var newLine = line.Runs[3] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(50, newLine.NewLineOffset.Height);
            var diff = newLine.NextLineSpacer.Line.Width - line.Width; //newline width - curr line width gives the offset
            Assert.AreEqual(diff, newLine.NewLineOffset.Width); //offset is right aligned
            
            
            
            //
            // second line - simple
            //
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //0 = spacer
            var spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(spacer);
            Assert.AreEqual(0, spacer.Width);
            
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual(line.Width, chars.Width);
            
            newLine = line.Runs[2] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(50, newLine.NewLineOffset.Height);
            diff = newLine.NextLineSpacer.Line.Width - line.Width;
            
            Assert.AreEqual(diff, newLine.NewLineOffset.Width);

            top += 50;
            
            //second arangement on begin owner, left margin, shift down 50, container width and line height
            CheckRenderBounds(begin.Owner, 1,  85.90547336350005, top, chars.Width, height);
            
            
            //
            // third line - span cont. + img + whitespace + image
            //
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(9, line.Runs.Count);
            
            //0 = spacer
            spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(spacer);
            Assert.AreEqual(0, spacer.Width);
            
            //1 = chars
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //2 = end text
            

            top += 50;
            
            //third arangement on begin owner, left margin, shift down 50, character width and line height
            CheckRenderBounds(begin.Owner, 2, 284.360732966, top , chars.Width, height);

            runningWidth = chars.Width + line.RightInset;
            
            //3 = large image
            var imgRun = line.Runs[3] as PDFLayoutComponentRun;
            
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            
            Assert.AreEqual(100, imgRun.TotalBounds.Y); //top align + 2 lines down
            Assert.AreEqual(100, imgRun.TotalBounds.Height);
            var imgW = (100 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the first image
            left = 30 + runningWidth ; //body margins
            top = 30 + 100; //body margins + 3 lines
            width = imgRun.TotalBounds.Width;
            height = 100;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);

            runningWidth += imgRun.TotalBounds.Width;
            
            //4 begin whitespace
            //5 text whitespace
            var whiteSpace = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            
            //6 = end whitespace
            
            //7 = small image
            imgRun = line.Runs[7] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(10, imgRun.TotalBounds.Height);
            Assert.AreEqual(100 + line.BaseLineOffset - 10, imgRun.TotalBounds.Y); //sits on the baseline, so top is - 10
            imgW = (10 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the second image 
            left = 30 + runningWidth ; //body margins
            top = 30 + 100 + line.BaseLineOffset - 10 ; //body margins + 2 lines and baseline - height
            width = imgRun.TotalBounds.Width;
            height = 10;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);


            //8 - end span
            Assert.IsInstanceOfType<PDFLayoutInlineEnd>(line.Runs[8]);
            
            
            
        }
        
        
         [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Inline_23_DisplayNone()
        {
            var path = AssertGetContentFile("Inline_23_DisplayNone");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("Positioned_Inline_23_DisplayNone.pdf"))
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
            Assert.AreEqual(4, line.Runs.Count);
            
            var imgDim = new Size(682, 452);

            var spanBegin = line.Runs[0] as PDFLayoutInlineBegin;
            Assert.IsNotNull(spanBegin);

            
            
            

            var runningWidth = Unit.Zero;
            
            
            //0 = start span
            //1 = text begin
            var begin = line.Runs[1] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            var lead = begin.TextRenderOptions.GetLineHeight() - begin.TextRenderOptions.GetSize();
            var halfLead = lead / 2;
            var descend = begin.TextRenderOptions.GetDescender();
            
            var baseline = 50 - halfLead - descend;
            Assert.AreEqual(baseline, line.BaseLineOffset);
            
            //top position
            baseline = begin.TextRenderOptions.GetBaselineOffset();
            Assert.AreEqual(baseline + 30, begin.StartTextCursor.Height); // + body margin
            Assert.AreEqual(30 + line.RightInset, begin.StartTextCursor.Width); // + body margin and right align
            
            //2 = text chars
            var chars = line.Runs[2] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            //confirm the render bounds rect for the first span - body margins and on the full baseline.
            var left = (Unit)30 + line.RightInset; //body margins
            var width = chars.Width; //left without lsb - as right aligned
            var height = begin.TextRenderOptions.GetSize() * 1.2;
            var top = 30 + ((50 - height)/2); //margins + leading space
            CheckRenderBounds(begin.Owner, 0, left, top, width, height);
            
            
            //3 = new line
            var newLine = line.Runs[3] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(50, newLine.NewLineOffset.Height);
            var diff = newLine.NextLineSpacer.Line.Width - line.Width; //newline width - curr line width gives the offset
            Assert.AreEqual(diff, newLine.NewLineOffset.Width); //offset is right aligned
            
            
            
            //
            // second line - simple
            //
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //0 = spacer
            var spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(spacer);
            Assert.AreEqual(0, spacer.Width);
            
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual(line.Width, chars.Width);
            
            newLine = line.Runs[2] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(50, newLine.NewLineOffset.Height);
            diff = newLine.NextLineSpacer.Line.Width - line.Width;
            
            Assert.AreEqual(diff, newLine.NewLineOffset.Width);

            top += 50;
            
            //second arangement on begin owner, left margin, shift down 50, container width and line height
            CheckRenderBounds(begin.Owner, 1,  85.90547336350005, top, chars.Width, height);
            
            
            //
            // third line - span cont. + img + whitespace + image
            //
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(9, line.Runs.Count);
            
            //0 = spacer
            spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(spacer);
            Assert.AreEqual(0, spacer.Width);
            
            //1 = chars
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //2 = end text
            

            top += 50;
            
            //third arangement on begin owner, left margin, shift down 50, character width and line height
            CheckRenderBounds(begin.Owner, 2, 284.360732966, top , chars.Width, height);

            runningWidth = chars.Width + line.RightInset;
            
            //3 = large image
            var imgRun = line.Runs[3] as PDFLayoutComponentRun;
            
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            
            Assert.AreEqual(100, imgRun.TotalBounds.Y); //top align + 2 lines down
            Assert.AreEqual(100, imgRun.TotalBounds.Height);
            var imgW = (100 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the first image
            left = 30 + runningWidth ; //body margins
            top = 30 + 100; //body margins + 3 lines
            width = imgRun.TotalBounds.Width;
            height = 100;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);

            runningWidth += imgRun.TotalBounds.Width;
            
            //4 begin whitespace
            //5 text whitespace
            var whiteSpace = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(whiteSpace);
            runningWidth += whiteSpace.Width;
            
            //6 = end whitespace
            
            //7 = small image
            imgRun = line.Runs[7] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(runningWidth, imgRun.TotalBounds.X);
            Assert.AreEqual(10, imgRun.TotalBounds.Height);
            Assert.AreEqual(100 + line.BaseLineOffset - 10, imgRun.TotalBounds.Y); //sits on the baseline, so top is - 10
            imgW = (10 / imgDim.Height.PointsValue) * imgDim.Width.PointsValue;
            Assert.AreEqual(Math.Round(imgW, 5), Math.Round(imgRun.TotalBounds.Width.PointsValue, 5)); //approx equal to 5 decimal places
            
            
            //confirm the render bounds rect for the second image 
            left = 30 + runningWidth ; //body margins
            top = 30 + 100 + line.BaseLineOffset - 10 ; //body margins + 2 lines and baseline - height
            width = imgRun.TotalBounds.Width;
            height = 10;
            CheckRenderBounds(imgRun.Owner, 0, left, top, width, height);


            //8 - end span
            Assert.IsInstanceOfType<PDFLayoutInlineEnd>(line.Runs[8]);
            
            
            
        }
    }
}
