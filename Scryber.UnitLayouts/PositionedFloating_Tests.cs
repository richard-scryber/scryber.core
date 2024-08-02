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
            Assert.IsNull(floatAddition.Next);

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
            Assert.IsNull(floatAddition.Next);

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
            Assert.IsNull(floatAddition.Next);

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
            Assert.IsNull(floatAddition.Next);

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
            Assert.IsNull(floatAddition.Next);

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
            Assert.IsNotNull(floatAddition.Next);
            
            
            
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
            
            //Check the second float in the additions
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count); //nw down to one
            Assert.IsNull(floatAddition.Next);

            var firstFloatWidth = floatAddition.FloatWidth;
            
            pos = nest.PositionedRegions[1];
            innerBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);
            
            Assert.AreEqual(3, innerLine.Runs.Count);
            chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            w = chars.Width;
            h = 15;
            
            Assert.AreEqual(w, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            w += 30;
            h += 30;
            Assert.AreEqual(w, innerBlock.Width);
            Assert.AreEqual(h, innerBlock.Height);
            
            Assert.AreEqual(w, floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(firstFloatWidth, floatAddition.FloatInset);

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //spacer, chars, end, posrun, start, end, newline
            
            var firstStartRun = line.Runs[0] as PDFTextRunSpacer;
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
            
            Unit yOffset = 20 + 10 + 10 + 30; //page margins and heading and first line

            yOffset += 30; //second line
            
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            Assert.AreEqual(yOffset, pos.TotalBounds.Y);
            Assert.AreEqual(15 + 30, pos.TotalBounds.Height);
            Assert.AreEqual(w, pos.TotalBounds.Width);

            Assert.AreEqual(firstFloatWidth + 20, pos.TotalBounds.X); //offset to the left + the float margins

            var after = nest.Columns[0].Contents[5] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(1, after.Columns.Length);
            Assert.AreEqual(2, after.Columns[0].Contents.Count);
            
            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(0, line.OffsetX);
            Assert.AreEqual(3, line.Runs.Count);

            var afterStartRun = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(afterStartRun);
            Assert.AreEqual(20, afterStartRun.StartTextCursor.Width); //margins of the page
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
            Assert.AreEqual(2, nest.PositionedRegions.Count);
            
            var pos = nest.PositionedRegions[0];
            
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //start, chars, end, pos-run, start, chars, new-line
            
            var floatAddition = nest.Columns[0].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(2, floatAddition.Count);
            Assert.IsNotNull(floatAddition.Next);
            
            
            
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
            Unit w = 120; //explicit
            Unit h = 15; // 2 linew

            Assert.AreEqual(chars.Width, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            w += 20; // add margins (not padding as explicit width)
            h += 15 + 30; // add second line, padding and margins
            
            Assert.AreEqual(w, innerBlock.Width); 
            Assert.AreEqual(h, innerBlock.Height);

            Assert.AreEqual(w , floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);
            
            //Check the second float in the additions
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count); //nw down to one
            Assert.IsNull(floatAddition.Next);

            var firstFloatWidth = floatAddition.FloatWidth;
            
            pos = nest.PositionedRegions[1];
            innerBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(2, innerBlock.Columns[0].Contents.Count);
            innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);
            
            Assert.AreEqual(3, innerLine.Runs.Count);
            chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            w = 120;
            h = 15;
            
            Assert.AreEqual(chars.Width, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            w += 20; //margins
            h += 15 + 30;// second line and margins + padding
            Assert.AreEqual(w, innerBlock.Width);
            Assert.AreEqual(h, innerBlock.Height);
            
            Assert.AreEqual(w, floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(firstFloatWidth, floatAddition.FloatInset);

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //spacer, chars, end, posrun, start, end, newline
            
            var firstStartRun = line.Runs[0] as PDFTextRunSpacer;
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
            
            Unit yOffset = 20 + 10 + 10 + 10 + 30; //page margins and heading and nest margins and first line

            yOffset += 30; //second line
            
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            Assert.AreEqual(yOffset, pos.TotalBounds.Y);
            Assert.AreEqual(30 + 30, pos.TotalBounds.Height);
            Assert.AreEqual(w, pos.TotalBounds.Width);

            Assert.AreEqual(10 + firstFloatWidth + 20 , pos.TotalBounds.X); //nest margins + offset to the left + the float margins

            var after = nest.Columns[0].Contents[4] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(1, after.Columns.Length);
            Assert.AreEqual(3, after.Columns[0].Contents.Count);
            
            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(0, line.OffsetX);
            Assert.AreEqual(3, line.Runs.Count);

            var afterStartRun = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(afterStartRun);
            var afterInset = 20 + 10 + (120 + 20) + (120 + 20);
            Assert.AreEqual(afterInset, afterStartRun.StartTextCursor.Width); //page margins, nest margins, float 1 width and margins, float 2 width and margins.
            
            line = after.Columns[0].Contents[1] as PDFLayoutLine; //last line should go back to the left of the nesting block
            Assert.IsNotNull(line);
            var afterNewLine = line.Runs[2] as PDFTextRunNewLine;
            Assert.IsNotNull(afterNewLine);
            Assert.AreEqual((120 + 20) + (120 + 20), afterNewLine.NewLineOffset.Width); //back 2 floating blocks and their margins
        }
        
        /// <summary>
        /// Checks a simple unsized float left div with text after.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_08_DualRightToPagePaddingMarginsWidth()
        {

            var path = AssertGetContentFile("Float_08_DualRightPaddingMarginsWidth");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_08_DualRightToPagePaddingMarginsWidth.pdf"))
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
            Assert.IsNotNull(floatAddition.Next);
            
            
            
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
            Unit w = 120; //explicit
            Unit h = 15; // 2 linew

            Assert.AreEqual(chars.Width, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            w += 20; // add margins (not padding as explicit width)
            h += 15 + 30; // add second line, padding and margins
            
            Assert.AreEqual(w, innerBlock.Width); 
            Assert.AreEqual(h, innerBlock.Height);

            Assert.AreEqual(w , floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);
            
            //Check the second float in the additions
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count); //nw down to one
            Assert.IsNull(floatAddition.Next);

            var firstFloatWidth = floatAddition.FloatWidth;
            
            pos = nest.PositionedRegions[1];
            innerBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(2, innerBlock.Columns[0].Contents.Count);
            innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);
            
            Assert.AreEqual(3, innerLine.Runs.Count);
            chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            w = 120;
            h = 15;
            
            Assert.AreEqual(chars.Width, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            w += 20; //margins
            h += 15 + 30;// second line and margins + padding
            Assert.AreEqual(w, innerBlock.Width);
            Assert.AreEqual(h, innerBlock.Height);
            
            Assert.AreEqual(w, floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(firstFloatWidth, floatAddition.FloatInset);

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //spacer, chars, end, posrun, start, end, newline
            
            var firstStartRun = line.Runs[0] as PDFTextRunSpacer;
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
            
            Unit yOffset = 20 + 10 + 10 + 10 + 30; //page margins and heading and nest margins and first line

            yOffset += 30; //second line
            
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            Assert.AreEqual(yOffset, pos.TotalBounds.Y);
            Assert.AreEqual(30 + 30, pos.TotalBounds.Height);
            Assert.AreEqual(w, pos.TotalBounds.Width);

            Assert.AreEqual(pg.Width - (10 + firstFloatWidth + 20 + pos.TotalBounds.Width) , pos.TotalBounds.X); //nest margins + offset to the right + the float margins

            var after = nest.Columns[0].Contents[4] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(1, after.Columns.Length);
            Assert.AreEqual(3, after.Columns[0].Contents.Count);
            
            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(0, line.OffsetX);
            Assert.AreEqual(3, line.Runs.Count);


        }
        
        /// <summary>
        /// Checks a simple unsized float left div with text after.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_09_MizedToPageCenter()
        {

            var path = AssertGetContentFile("Float_09_MixedCenter");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_09_MixedCenter.pdf"))
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
            Assert.IsNotNull(floatAddition.Next);
            
            
            
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
            Unit w = 120; //explicit
            Unit h = 15; // first line

            Assert.AreEqual(chars.Width, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            w += 10; // add margins (not padding as explicit width)
            h += 15 + 10; // add second line, padding and margins
            
            Assert.AreEqual(w, innerBlock.Width); 
            Assert.AreEqual(h, innerBlock.Height);

            Assert.AreEqual(w , floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);
            
            //Check the second float in the additions
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count); //nw down to one
            Assert.IsNull(floatAddition.Next);

            var firstFloatWidth = floatAddition.FloatWidth;
            
            pos = nest.PositionedRegions[1];
            innerBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(2, innerBlock.Columns[0].Contents.Count);
            innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);
            
            Assert.AreEqual(3, innerLine.Runs.Count);
            chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            w = 120;
            h = 15;
            
            Assert.AreEqual(chars.Width, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            w += 10; // 5pt margins
            h += 15 + 10;// second line and margins 
            Assert.AreEqual(w, innerBlock.Width);
            Assert.AreEqual(h, innerBlock.Height);
            
            Assert.AreEqual(w, floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //spacer, chars, end, posrun, start, end, newline
            
            var firstStartRun = line.Runs[0] as PDFTextRunSpacer;
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
            
            Unit yOffset = 20 + 10 + 10 + 10 + 30; //page margins and heading and nest margins and first line

            yOffset += 30; //second line
            
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            Assert.AreEqual(yOffset, pos.TotalBounds.Y);
            Assert.AreEqual(30 + 10, pos.TotalBounds.Height);
            Assert.AreEqual(w, pos.TotalBounds.Width);

            Assert.AreEqual(pg.Width - (10  + 20 + pos.TotalBounds.Width) , pos.TotalBounds.X); //nest margins + the float margins

            var after = nest.Columns[0].Contents[4] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(1, after.Columns.Length);
            Assert.AreEqual(2, after.Columns[0].Contents.Count);
            
            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(0, line.OffsetX);
            Assert.AreEqual(3, line.Runs.Count);


        }
        
        /// <summary>
        /// Checks a simple unsized float left div with text after.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_10_MulticolumnLeft()
        {

            var path = AssertGetContentFile("Float_10_MulticolumnLeft");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_10_MulticolumnLeft.pdf"))
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
            Assert.AreEqual(2, nest.Columns.Length);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            
            Assert.AreEqual(1, nest.Columns[0].Contents.Count);
            Assert.AreEqual(9, nest.Columns[1].Contents.Count); //8 lines, one nested block
            
            var pos = nest.PositionedRegions[0];
            var line = nest.Columns[1].Contents[0] as PDFLayoutLine;
            
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            line = nest.Columns[1].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //start, chars, end, pos-run, start, chars, new-line
            
            var floatAddition = nest.Columns[1].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count);
            Assert.IsNull(floatAddition.Next);
            
            
            
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
            Unit w = 100; //explicit
            Unit h = 15; // first line

            Assert.AreEqual(chars.Width, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            w += 10; // add margins (not padding as explicit width)
            h += 15 + 10; // add second line, padding and margins
            
            Assert.AreEqual(w, innerBlock.Width); 
            Assert.AreEqual(h, innerBlock.Height);

            Assert.AreEqual(w , floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);
            
            // No second float in the additions
            
            
            Unit yOffset = 20 + 10 + 10 + 10 + 30; //page margins and heading and nest margins and first line
            Unit xOffset = ((pg.Width - 60 - 10) / 2) + 30 + 10; //(nest width - gutter) half a column, then offset margins and gutter

            yOffset += 30; //second line
            
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            Assert.AreEqual(yOffset, pos.TotalBounds.Y);
            Assert.AreEqual(xOffset, pos.TotalBounds.X);
            Assert.AreEqual(30 + 10, pos.TotalBounds.Height);
            Assert.AreEqual(w, pos.TotalBounds.Width);

            
            var after = nest.Columns[1].Contents[8] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(1, after.Columns.Length);
            Assert.AreEqual(4, after.Columns[0].Contents.Count);
            
            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(0, line.OffsetX);
            Assert.AreEqual(3, line.Runs.Count);


        }
        
        /// <summary>
        /// Checks a simple unsized float left div with text after.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_11_MulticolumnRight()
        {

            var path = AssertGetContentFile("Float_11_MulticolumnRight");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_11_MulticolumnRight.pdf"))
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
            Assert.AreEqual(2, nest.Columns.Length);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            
            Assert.AreEqual(1, nest.Columns[0].Contents.Count);
            Assert.AreEqual(9, nest.Columns[1].Contents.Count); //8 lines, one nested block
            
            var pos = nest.PositionedRegions[0];
            var line = nest.Columns[1].Contents[0] as PDFLayoutLine;
            
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            line = nest.Columns[1].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //start, chars, end, pos-run, start, chars, new-line
            
            var floatAddition = nest.Columns[1].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count);
            Assert.IsNull(floatAddition.Next);
            
            
            
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
            Unit w = 100; //explicit
            Unit h = 15; // first line

            Assert.AreEqual(chars.Width, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            w += 10; // add margins (not padding as explicit width)
            h += 15 + 10; // add second line, padding and margins
            
            Assert.AreEqual(w, innerBlock.Width); 
            Assert.AreEqual(h, innerBlock.Height);

            Assert.AreEqual(w , floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);
            
            // No second float in the additions
            
            
            Unit yOffset = 20 + 10 + 10 + 10 + 30; //page margins and heading and nest margins and first line
            Unit xOffset = (pg.Width - 30) - w; //(page right - pos width)

            yOffset += 30; //second line
            
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            Assert.AreEqual(yOffset, pos.TotalBounds.Y);
            Assert.AreEqual(xOffset, pos.TotalBounds.X);
            Assert.AreEqual(30 + 10, pos.TotalBounds.Height);
            Assert.AreEqual(w, pos.TotalBounds.Width);

            
            var after = nest.Columns[1].Contents[8] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(1, after.Columns.Length);
            Assert.AreEqual(4, after.Columns[0].Contents.Count);
            
            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(0, line.OffsetX);
            Assert.AreEqual(3, line.Runs.Count);


        }
        
        /// <summary>
        /// Checks a simple unsized float left div with text after.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_12_MulticolumnMixed()
        {

            var path = AssertGetContentFile("Float_12_MulticolumnMixed");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_12_MulticolumnMixed.pdf"))
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
            Assert.AreEqual(2, nest.Columns.Length);
            
            //first column contains 1 block with 2 floating regions
            
            Assert.AreEqual(1, nest.Columns[0].Contents.Count);
            var first = nest.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(first);
            
            Assert.AreEqual(2, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);
            Assert.AreEqual(6, first.Columns[0].Contents.Count);
            
            var floatAddition = first.Columns[0].Floats;
            var pos = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.IsNotNull(floatAddition);
            
            Assert.AreEqual(0, floatAddition.FloatInset); //relative to first block
            Assert.AreEqual(0, floatAddition.YOffset);
            Assert.AreEqual(100 + 10, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(FloatMode.Left, floatAddition.Mode);
            
            Assert.AreEqual(30, pos.TotalBounds.X); //relative to the page
            Assert.AreEqual(80, pos.TotalBounds.Y);
            Assert.AreEqual(100 + 10, pos.TotalBounds.Width);
            Assert.AreEqual(30 + 10, pos.TotalBounds.Height);

            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);

            pos = first.PositionedRegions[1] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            
            Assert.AreEqual(0, floatAddition.FloatInset); //right to the first block
            Assert.AreEqual(60, floatAddition.YOffset);
            Assert.AreEqual(100 + 10, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(FloatMode.Right, floatAddition.Mode);
            
            Assert.AreEqual(first.Width + 30 - (100 + 10), pos.TotalBounds.X); //Right of the first block relative to the page
            Assert.AreEqual(80 + (4 * 15), pos.TotalBounds.Y); //4 lines down
            Assert.AreEqual((100 + 10), pos.Width);
            Assert.AreEqual(30 + 10, pos.TotalBounds.Height);

            var line = first.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(4, line.Runs.Count);
            var begin = line.Runs[1] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(100 + 10 + 30, begin.StartTextCursor.Width); //text starts 110 in from the left of first block
            Assert.IsTrue(line.Width <= (first.Width - 110));
            
            line = first.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            var spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(spacer);
            Assert.AreEqual(110, spacer.Width);
            
            line = first.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(spacer);
            Assert.AreEqual(110, spacer.Width);
            
            //Back to full width
            line = first.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.IsTrue(line.Width <= first.Width);
            Assert.IsTrue(line.Width >= (first.Width - 110));
            spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(spacer);
            Assert.AreEqual(0, spacer.Width); //no space
            
            //now account for float right
            line = first.Columns[0].Contents[4] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.IsTrue(line.Width <= (first.Width - 110));
            spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(spacer);
            Assert.AreEqual(0, spacer.Width); //no space
            
            
            //
            // check the second column
            //
            
            Assert.AreEqual(2, nest.PositionedRegions.Count);
            Assert.AreEqual(9, nest.Columns[1].Contents.Count); //8 lines, one nested block
            
            
            pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            line = nest.Columns[1].Contents[0] as PDFLayoutLine;
            
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            line = nest.Columns[1].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //start, chars, end, pos-run, start, chars, new-line
            
            floatAddition = nest.Columns[1].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(2, floatAddition.Count);
            Assert.IsNotNull(floatAddition.Next);
            
            
            
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
            Unit w = 100; //explicit
            Unit h = 15; // first line

            Assert.AreEqual(chars.Width, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            w += 10; // add margins (not padding as explicit width)
            h += 15 + 10; // add second line, padding and margins
            
            Assert.AreEqual(w, innerBlock.Width); 
            Assert.AreEqual(h, innerBlock.Height);

            Assert.AreEqual(w , floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);
            
            
            Unit yOffset = 80 + 30; //down 2 lines from the top of nest
            Unit xOffset = (30 + first.Width + 10); //(second column with margins and gutter)

            
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            Assert.AreEqual(yOffset, pos.TotalBounds.Y);
            Assert.AreEqual(xOffset, pos.TotalBounds.X);
            Assert.AreEqual(30 + 10, pos.TotalBounds.Height);
            Assert.AreEqual(w, pos.TotalBounds.Width);

            //second float right in second column
            pos = nest.PositionedRegions[1] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count);
            Assert.IsNull(floatAddition.Next);
            
            
            Assert.AreEqual(w , floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);
            Assert.AreEqual(FloatMode.Right, floatAddition.Mode);
            
            yOffset = 80 + (7 * 15); //down 7 lines from the top of nest
            xOffset = pg.Width - 30 - floatAddition.FloatWidth; //(second column with margins and gutter)

            
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            Assert.AreEqual(yOffset, pos.TotalBounds.Y);
            Assert.AreEqual(xOffset, pos.TotalBounds.X);
            Assert.AreEqual(30 + 10, pos.TotalBounds.Height);
            Assert.AreEqual(w, pos.TotalBounds.Width);
            
            var after = nest.Columns[1].Contents[8] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(1, after.Columns.Length);
            Assert.AreEqual(4, after.Columns[0].Contents.Count);
            
            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(0, line.OffsetX);
            Assert.AreEqual(3, line.Runs.Count);

            //should still obey the right float that impacts the flowing text
            Assert.IsTrue(line.Width <= nest.Columns[1].Width - 110);

        }
        
        /// <summary>
        /// Known issue when the parent is relative
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_13_ManyStackedMized()
        {

            var path = AssertGetContentFile("Float_13_ManyStackedMixed");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_13_ManyStackedMixed.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.1;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.RenderOptions.Compression = OutputCompressionType.None;
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
            Assert.AreEqual(6, nest.Columns[0].Contents.Count);

            var first = nest.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(first);
            
            Assert.AreEqual(1, first.Columns.Length);
            Assert.AreEqual(2, first.Columns[0].Contents.Count); //line for floats and para
            Assert.AreEqual(7, first.PositionedRegions.Count);

            var left = 0;
            var index = 0;
            var floatAddition = first.Columns[0].Floats;
            
            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);
                Assert.AreEqual(0 + (60 * index), floatAddition.FloatInset);
                Assert.AreEqual(60, floatAddition.FloatWidth);
                Assert.AreEqual(45 + 10, floatAddition.FloatHeight);
                Assert.AreEqual(0, floatAddition.YOffset);
                
                index++;
                floatAddition = floatAddition.Next;
            }
            
            Assert.AreEqual(7, index);

            var para = first.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(para);
            Assert.AreEqual(1, para.Columns.Length);
            Assert.AreEqual(5, para.Columns[0].Contents.Count);

            var xInset = 7 * 60;
            //check lines are reduced by the width of the floats.
            var line = para.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.IsTrue(first.Width - xInset == line.FullWidth);

            line = para.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.IsTrue(first.Width - xInset == line.FullWidth);
            
            //fifth line should be bat to full width of the container
            line = para.Columns[0].Contents[4] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.IsTrue(first.Width == line.FullWidth);
            
            //
            // second set of floating blocks
            // six right, then one left
            // on the second line
            //
            
            Assert.AreEqual(7, nest.PositionedRegions.Count);
            
            var right = 0;
            index = 0;
            floatAddition = nest.Columns[0].Floats;
            
            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);
                
                if (index < 6)
                {
                    Assert.AreEqual(0 + (60 * index), floatAddition.FloatInset);
                    Assert.AreEqual(60, floatAddition.FloatWidth);
                    Assert.AreEqual(45 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual((6 *15) + 10 + 10, floatAddition.YOffset); //6 lines plus <p> margins
                    Assert.AreEqual(FloatMode.Right, floatAddition.Mode);
                }
                else
                {
                    Assert.AreEqual(0, floatAddition.FloatInset);
                    Assert.AreEqual(60, floatAddition.FloatWidth);
                    Assert.AreEqual(45 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual((6 *15) + 10 + 10, floatAddition.YOffset); //6 lines plus <p> margins
                    Assert.AreEqual(FloatMode.Left, floatAddition.Mode);
                }
                

                index++;
                floatAddition = floatAddition.Next;
            }
            
            xInset = 60;
            var rightInset = 6 * 60;
            
            //Top line above the floats.
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.TotalBounds.Width - 20, line.FullWidth); //margins

            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.TotalBounds.Width - (xInset + rightInset + 20), line.FullWidth);
            
            line = nest.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.TotalBounds.Width - (xInset + rightInset + 20), line.FullWidth);
            
            //after block
            var after = nest.Columns[0].Contents[5] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(2, after.Columns[0].Contents.Count);
            
            //first line of after should be in the floats
            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(after.Width - (xInset + rightInset), line.FullWidth);
            
            //seconds line of after should be back to full width
            line = after.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(after.Width, line.FullWidth);
            
            


        }
    }
    
    
}
