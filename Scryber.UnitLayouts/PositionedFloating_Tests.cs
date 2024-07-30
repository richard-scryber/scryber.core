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
    /// <summary>
    /// Tests the float left and right positioning
    /// </summary>
    /// <remarks>
    /// Floating blocks are treated exactly the same as relatively positioned regions , but a FloatAddition instance
    /// is added to the region containing the float. This is then used to reduce the width of the lines (on the left or the right)
    /// whilst they run with the float area.
    /// Once past the float(s) then the line is restored back to full width in the layout.
    /// </remarks>
    [TestClass()]
    public class PositionedFloating_Tests
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
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Positioning/Floating/" + name + ".html");
            path = System.IO.Path.GetFullPath(path);

            if (!System.IO.File.Exists(path))
                Assert.Inconclusive("The path the file " + name + " was not found at " + path);

            return path;
        }

        /// <summary>
        /// Checks a simple unsized float left div with text after.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_01_LeftToPage()
        {

            var path = AssertGetContentFile("Float_01_Left");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_01_LeftToPage.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.1;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;


            Assert.AreEqual(0, content.PositionedRegions.Count);
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0];
            
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //start, chars, end, pos-run, start, chars, new-line

            var floatAddition = nest.Columns[0].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count);
            Assert.IsNull(floatAddition.Prev);

            var innerBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);

            Assert.AreEqual(3, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //width and hight of the content and float should be chars width and line height
            Unit w = chars.Width;
            Unit h = 15;

            Assert.AreEqual(w, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            Assert.AreEqual(w, innerBlock.Width);
            Assert.AreEqual(h, innerBlock.Height);

            Assert.AreEqual(w, floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);

            var firstStartRun = line.Runs[0] as PDFTextRunBegin;
            chars = line.Runs[1] as PDFTextRunCharacter;
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            var secondStartRun = line.Runs[4] as PDFTextRunBegin;

            Assert.IsNotNull(firstStartRun);
            Assert.IsNotNull(chars);

            Assert.IsNotNull(posRun);
            Assert.IsNotNull(secondStartRun);
            Assert.AreEqual(0, posRun.Width); //The positioned run width is zero, as it does not affect the line height etc.
            Assert.AreEqual(0, posRun.Height); //The positioned run height is zero, as it does not affect the line height etc.

            //And the positioned region should also have the relative size
            Assert.AreEqual(pos, posRun.Region);
            
            Unit yOffset = 20 + 10 + 10 + 30; //page margins and heading.

            yOffset += 15; //first line
            
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            Assert.AreEqual(yOffset, pos.TotalBounds.Y);
            Assert.AreEqual(15, pos.TotalBounds.Height);
            Assert.AreEqual(w, pos.TotalBounds.Width);

            //The first start run should just have the page margins.
            Assert.AreEqual(20, firstStartRun.StartTextCursor.Width);
            //The second should ignore the float and continue on the first line straight after
            Assert.AreEqual(20 + firstStartRun.Width + chars.Width, secondStartRun.StartTextCursor.Width);

            //The new line should go back to less than the width of the line + the floating width.
            var newLine = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(15, newLine.NewLineOffset.Height);
            Assert.IsTrue(newLine.NewLineOffset.Width < line.Width - pos.Width);

            //After block
            var after = nest.Columns[0].Contents[3] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(0, after.TotalBounds.X);
            Assert.AreEqual(2, after.Columns[0].Contents.Count);

            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            firstStartRun = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(firstStartRun);
            Assert.AreEqual(20, firstStartRun.StartTextCursor.Width); //page margins

        }
        
        /// <summary>
        /// Checks a simple unsized float left div with text after.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_02_LeftToPagePaddingMargins()
        {

            var path = AssertGetContentFile("Float_02_LeftPaddingMargins");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_02_LeftToPagePaddingMargins.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.1;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;


            Assert.AreEqual(0, content.PositionedRegions.Count);
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0];
            
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //start, chars, end, pos-run, start, chars, new-line

            var floatAddition = nest.Columns[0].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count);
            Assert.IsNull(floatAddition.Prev);

            var innerBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);

            Assert.AreEqual(3, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //width and hight of the content and float should be chars width and line height
            Unit w = chars.Width; //padding and margins
            Unit h = 15;

            Assert.AreEqual(w, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            w += 30; // add padding and margins
            h += 30; // add padding and margins
            
            Assert.AreEqual(w, innerBlock.Width); 
            Assert.AreEqual(h, innerBlock.Height);

            Assert.AreEqual(w , floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);

            var firstStartRun = line.Runs[0] as PDFTextRunBegin;
            chars = line.Runs[1] as PDFTextRunCharacter;
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            var secondStartRun = line.Runs[4] as PDFTextRunBegin;

            Assert.IsNotNull(firstStartRun);
            Assert.IsNotNull(chars);

            Assert.IsNotNull(posRun);
            Assert.IsNotNull(secondStartRun);
            Assert.AreEqual(0, posRun.Width); //The positioned run width is zero, as it does not affect the line height etc.
            Assert.AreEqual(0, posRun.Height); //The positioned run height is zero, as it does not affect the line height etc.

            //And the positioned region should also have the relative size
            Assert.AreEqual(pos, posRun.Region);
            
            Unit yOffset = 20 + 10 + 10 + 30; //page margins and heading.

            yOffset += 15; //first line
            
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            Assert.AreEqual(yOffset, pos.TotalBounds.Y);
            Assert.AreEqual(15 + 30, pos.TotalBounds.Height);
            Assert.AreEqual(w, pos.TotalBounds.Width);

            //The first start run should just have the page margins.
            Assert.AreEqual(20, firstStartRun.StartTextCursor.Width);
            //The second should ignore the float and continue on the first line straight after
            Assert.AreEqual(20 + firstStartRun.Width + chars.Width, secondStartRun.StartTextCursor.Width);

            //The new line should go back to less than the width of the line + the floating width.
            var newLine = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(15, newLine.NewLineOffset.Height);
            Assert.IsTrue(newLine.NewLineOffset.Width < line.Width - pos.Width + 30);

            //After block
            var after = nest.Columns[0].Contents[4] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(0, after.TotalBounds.X);
            Assert.AreEqual(2, after.Columns[0].Contents.Count);

            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            firstStartRun = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(firstStartRun);
            Assert.AreEqual(20, firstStartRun.StartTextCursor.Width); //page margins

        }

        /// <summary>
        /// Checks a simple unsized float left div with text after.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_03_LeftToPagePaddingMarginsWidth()
        {

            var path = AssertGetContentFile("Float_03_LeftPaddingMarginsWidth");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_03_LeftToPagePaddingMarginsWidth.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.1;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;


            Assert.AreEqual(0, content.PositionedRegions.Count);
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0];
            
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //start, chars, end, pos-run, start, chars, new-line

            var floatAddition = nest.Columns[0].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count);
            Assert.IsNull(floatAddition.Prev);

            var innerBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(2, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);

            Assert.AreEqual(3, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //width and hight of the content and float should be chars width and line height
            Unit w = chars.Width; //padding and margins
            Unit h = 15;

            Assert.AreEqual(w, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            w = 120 + 20; // explicit width + margins
            h = 30 + 30; // 2 lines + padding and margins
            
            Assert.AreEqual(w, innerBlock.Width); 
            Assert.AreEqual(h, innerBlock.Height);

            Assert.AreEqual(w , floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);

            var firstStartRun = line.Runs[0] as PDFTextRunBegin;
            chars = line.Runs[1] as PDFTextRunCharacter;
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            var secondStartRun = line.Runs[4] as PDFTextRunBegin;

            Assert.IsNotNull(firstStartRun);
            Assert.IsNotNull(chars);

            Assert.IsNotNull(posRun);
            Assert.IsNotNull(secondStartRun);
            Assert.AreEqual(0, posRun.Width); //The positioned run width is zero, as it does not affect the line height etc.
            Assert.AreEqual(0, posRun.Height); //The positioned run height is zero, as it does not affect the line height etc.

            //And the positioned region should also have the relative size
            Assert.AreEqual(pos, posRun.Region);
            
            Unit yOffset = 20 + 10 + 10 + 30; //page margins and heading.

            yOffset += 15; //first line
            
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);
            Assert.AreEqual(20, pos.TotalBounds.X);
            Assert.AreEqual(yOffset, pos.TotalBounds.Y);
            Assert.AreEqual(30 + 30, pos.TotalBounds.Height);
            Assert.AreEqual(w, pos.TotalBounds.Width);

            //The first start run should just have the page margins.
            Assert.AreEqual(20, firstStartRun.StartTextCursor.Width);
            //The second should ignore the float and continue on the first line straight after
            Assert.AreEqual(20 + firstStartRun.Width + chars.Width, secondStartRun.StartTextCursor.Width);

            //The new line should go back to less than the width of the line + the floating width.
            var newLine = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(15, newLine.NewLineOffset.Height);
            Assert.IsTrue(newLine.NewLineOffset.Width < line.Width - pos.Width + 30);

            //After block - first line pushed out
            var after = nest.Columns[0].Contents[4] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(0, after.TotalBounds.X);
            Assert.AreEqual(2, after.Columns[0].Contents.Count);

            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            firstStartRun = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(firstStartRun);
            Assert.AreEqual(20 + 120 + 20, firstStartRun.StartTextCursor.Width); //page margins + float block + margins

            //TODO: Check the second line reset back to zero offset
        }


        /// <summary>
        /// Checks a simple unsized float left div with text after.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_04_RightToPage()
        {

            var path = AssertGetContentFile("Float_04_Right");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_04_RightToPage.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.1;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;


            Assert.AreEqual(0, content.PositionedRegions.Count);
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0];
            
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //start, chars, end, pos-run, start, chars, new-line

            var floatAddition = nest.Columns[0].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count);
            Assert.IsNull(floatAddition.Prev);

            var innerBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);

            Assert.AreEqual(3, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //width and hight of the content and float should be chars width and line height
            Unit w = chars.Width;
            Unit h = 15;

            Assert.AreEqual(w, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            Assert.AreEqual(w, innerBlock.Width);
            Assert.AreEqual(h, innerBlock.Height);

            Assert.AreEqual(w, floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);

            var firstStartRun = line.Runs[0] as PDFTextRunBegin;
            chars = line.Runs[1] as PDFTextRunCharacter;
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            var secondStartRun = line.Runs[4] as PDFTextRunBegin;

            Assert.IsNotNull(firstStartRun);
            Assert.IsNotNull(chars);

            Assert.IsNotNull(posRun);
            Assert.IsNotNull(secondStartRun);
            Assert.AreEqual(0, posRun.Width); //The positioned run width is zero, as it does not affect the line height etc.
            Assert.AreEqual(0, posRun.Height); //The positioned run height is zero, as it does not affect the line height etc.

            //And the positioned region should also have the relative size
            Assert.AreEqual(pos, posRun.Region);
            
            Unit yOffset = 20 + 10 + 10 + 30; //page margins and heading.

            yOffset += 15; //first line
            
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);
            Assert.AreEqual(pg.Width - (20 + w), pos.TotalBounds.X); //right align the float
            Assert.AreEqual(yOffset, pos.TotalBounds.Y);
            Assert.AreEqual(15, pos.TotalBounds.Height);
            Assert.AreEqual(w, pos.TotalBounds.Width);

            //The first start run should just have the page margins.
            Assert.AreEqual(20, firstStartRun.StartTextCursor.Width);
            //The second should ignore the float and continue on the first line straight after
            Assert.AreEqual(20 + firstStartRun.Width + chars.Width, secondStartRun.StartTextCursor.Width);

            //The new line should go back to less than the width of the line + the floating width.
            var newLine = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(15, newLine.NewLineOffset.Height);
            Assert.IsTrue(newLine.NewLineOffset.Width < line.Width - pos.Width);

            //After block
            var after = nest.Columns[0].Contents[3] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(0, after.TotalBounds.X);
            Assert.AreEqual(2, after.Columns[0].Contents.Count);

            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            firstStartRun = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(firstStartRun);
            Assert.AreEqual(20, firstStartRun.StartTextCursor.Width); //page margins

        }
        
        /// <summary>
        /// Checks a simple unsized float left div with text after.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_05_RightToPagePaddingMarginsWidth()
        {

            var path = AssertGetContentFile("Float_05_RightPaddingMarginsWidth");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_05_RightToPagePaddingMarginsWidth.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.1;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;


            Assert.AreEqual(0, content.PositionedRegions.Count);
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0];
            
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //start, chars, end, pos-run, start, chars, new-line

            var floatAddition = nest.Columns[0].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count);
            Assert.IsNull(floatAddition.Prev);

            var innerBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(2, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);

            Assert.AreEqual(3, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //width and hight of the content and float should be chars width and line height
            Unit w = chars.Width; //padding and margins
            Unit h = 15;

            Assert.AreEqual(w, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            w = 120 + 20; // explicit width + margins
            h = 30 + 30; // 2 lines + padding and margins
            
            Assert.AreEqual(w, innerBlock.Width); 
            Assert.AreEqual(h, innerBlock.Height);

            Assert.AreEqual(w , floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);

            var firstStartRun = line.Runs[0] as PDFTextRunBegin;
            chars = line.Runs[1] as PDFTextRunCharacter;
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            var secondStartRun = line.Runs[4] as PDFTextRunBegin;

            Assert.IsNotNull(firstStartRun);
            Assert.IsNotNull(chars);

            Assert.IsNotNull(posRun);
            Assert.IsNotNull(secondStartRun);
            Assert.AreEqual(0, posRun.Width); //The positioned run width is zero, as it does not affect the line height etc.
            Assert.AreEqual(0, posRun.Height); //The positioned run height is zero, as it does not affect the line height etc.

            //And the positioned region should also have the relative size
            Assert.AreEqual(pos, posRun.Region);
            
            Unit yOffset = 20 + 10 + 10 + 30; //page margins and heading.

            yOffset += 15; //first line
            
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);
            Assert.AreEqual(pg.Width - (20 + 10 + 10 + 120) , pos.TotalBounds.X);//region contains a 120pt block with 10pt margins and a 20pt right page margin = 160
            Assert.AreEqual(yOffset, pos.TotalBounds.Y);
            Assert.AreEqual(30 + 30, pos.TotalBounds.Height);
            Assert.AreEqual(w, pos.TotalBounds.Width);

            //The first start run should just have the page margins, but right aligned, so more
            Assert.IsTrue(20 < firstStartRun.StartTextCursor.Width);
            //The second should ignore the float and continue on the first line straight after, but right aligned
            Assert.IsTrue(20 + firstStartRun.Width + chars.Width < secondStartRun.StartTextCursor.Width);

            //The new line should go back to less than the width of the line + the floating width.
            var newLine = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(15, newLine.NewLineOffset.Height);
            Assert.IsTrue(newLine.NewLineOffset.Width < line.Width - pos.Width + 30);

            //After block - first line pushed out
            var after = nest.Columns[0].Contents[4] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(0, after.TotalBounds.X);
            Assert.AreEqual(2, after.Columns[0].Contents.Count);

            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            firstStartRun = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(firstStartRun);
            Assert.IsTrue(20 <  firstStartRun.StartTextCursor.Width); //page margins but right aligned
            
            //TODO: Check the second line reset back to zero offset
        }
        
        /// <summary>
        /// Checks with two floating divs one on a line below the other - they should be offset by their natural width and push the text lines left 
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_06_DualLeftToPagePaddingMargins()
        {

            var path = AssertGetContentFile("Float_06_DualLeftPaddingMargins");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_06_DualLeftToPagePaddingMargins.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.1;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;


            Assert.AreEqual(0, content.PositionedRegions.Count);
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(2, nest.PositionedRegions.Count);
            
            var pos = nest.PositionedRegions[0];
            
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //start, chars, end, pos-run, start, chars, new-line
            
            var floatAddition = nest.Columns[0].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(2, floatAddition.Count);
            Assert.IsNotNull(floatAddition.Prev);
            
            
            
            var innerBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);

            Assert.AreEqual(3, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //width and hight of the content and float should be chars width and line height
            Unit w = chars.Width; //padding and margins
            Unit h = 15;

            Assert.AreEqual(w, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            w += 30; // add padding and margins
            h += 30; // add padding and margins
            
            Assert.AreEqual(w, innerBlock.Width); 
            Assert.AreEqual(h, innerBlock.Height);

            Assert.AreEqual(w , floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(floatAddition.Prev.FloatWidth, floatAddition.FloatInset);
            
            //Check the second float in the addi

            var firstStartRun = line.Runs[0] as PDFTextRunBegin;
            chars = line.Runs[1] as PDFTextRunCharacter;
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            var secondStartRun = line.Runs[4] as PDFTextRunBegin;

            Assert.IsNotNull(firstStartRun);
            Assert.IsNotNull(chars);

            Assert.IsNotNull(posRun);
            Assert.IsNotNull(secondStartRun);
            Assert.AreEqual(0, posRun.Width); //The positioned run width is zero, as it does not affect the line height etc.
            Assert.AreEqual(0, posRun.Height); //The positioned run height is zero, as it does not affect the line height etc.

            //And the positioned region should also have the relative size
            Assert.AreEqual(pos, posRun.Region);
            
            Unit yOffset = 20 + 10 + 10 + 30; //page margins and heading.

            yOffset += 15; //first line
            
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            Assert.AreEqual(yOffset, pos.TotalBounds.Y);
            Assert.AreEqual(15 + 30, pos.TotalBounds.Height);
            Assert.AreEqual(w, pos.TotalBounds.Width);

            //The first start run should just have the page margins.
            Assert.AreEqual(20, firstStartRun.StartTextCursor.Width);
            //The second should ignore the float and continue on the first line straight after
            Assert.AreEqual(20 + firstStartRun.Width + chars.Width, secondStartRun.StartTextCursor.Width);

            //The new line should go back to less than the width of the line + the floating width.
            var newLine = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(15, newLine.NewLineOffset.Height);
            Assert.IsTrue(newLine.NewLineOffset.Width < line.Width - pos.Width + 30);

            //After block
            var after = nest.Columns[0].Contents[4] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(0, after.TotalBounds.X);
            Assert.AreEqual(2, after.Columns[0].Contents.Count);

            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            firstStartRun = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(firstStartRun);
            Assert.AreEqual(20, firstStartRun.StartTextCursor.Width); //page margins

        }
        
        /// <summary>
        /// Checks a simple unsized float left div with text after.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_07_DualLeftToPagePaddingMarginsWidth()
        {

            var path = AssertGetContentFile("Float_07_DualLeftPaddingMarginsWidth");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_07_DualLeftToPagePaddingMarginsWidth.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.1;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;


            Assert.AreEqual(0, content.PositionedRegions.Count);
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0];
            
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //start, chars, end, pos-run, start, chars, new-line

            var floatAddition = nest.Columns[0].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count);
            Assert.IsNull(floatAddition.Prev);

            var innerBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);

            Assert.AreEqual(3, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //width and hight of the content and float should be chars width and line height
            Unit w = chars.Width; //padding and margins
            Unit h = 15;

            Assert.AreEqual(w, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            w += 30; // add padding and margins
            h += 30; // add padding and margins
            
            Assert.AreEqual(w, innerBlock.Width); 
            Assert.AreEqual(h, innerBlock.Height);

            Assert.AreEqual(w , floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);

            var firstStartRun = line.Runs[0] as PDFTextRunBegin;
            chars = line.Runs[1] as PDFTextRunCharacter;
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            var secondStartRun = line.Runs[4] as PDFTextRunBegin;

            Assert.IsNotNull(firstStartRun);
            Assert.IsNotNull(chars);

            Assert.IsNotNull(posRun);
            Assert.IsNotNull(secondStartRun);
            Assert.AreEqual(0, posRun.Width); //The positioned run width is zero, as it does not affect the line height etc.
            Assert.AreEqual(0, posRun.Height); //The positioned run height is zero, as it does not affect the line height etc.

            //And the positioned region should also have the relative size
            Assert.AreEqual(pos, posRun.Region);
            
            Unit yOffset = 20 + 10 + 10 + 30; //page margins and heading.

            yOffset += 15; //first line
            
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            Assert.AreEqual(yOffset, pos.TotalBounds.Y);
            Assert.AreEqual(15 + 30, pos.TotalBounds.Height);
            Assert.AreEqual(w, pos.TotalBounds.Width);

            //The first start run should just have the page margins.
            Assert.AreEqual(20, firstStartRun.StartTextCursor.Width);
            //The second should ignore the float and continue on the first line straight after
            Assert.AreEqual(20 + firstStartRun.Width + chars.Width, secondStartRun.StartTextCursor.Width);

            //The new line should go back to less than the width of the line + the floating width.
            var newLine = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(15, newLine.NewLineOffset.Height);
            Assert.IsTrue(newLine.NewLineOffset.Width < line.Width - pos.Width + 30);

            //After block
            var after = nest.Columns[0].Contents[4] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(0, after.TotalBounds.X);
            Assert.AreEqual(2, after.Columns[0].Contents.Count);

            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            firstStartRun = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(firstStartRun);
            Assert.AreEqual(20, firstStartRun.StartTextCursor.Width); //page margins

        }
    }
    
    
}
