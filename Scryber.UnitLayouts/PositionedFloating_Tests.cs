using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
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
    /// Floating blocks are treated exactly the same as relative positioned regions , but a FloatAddition instance
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
        /// Checks a simple unsized float left div with text after with padding and margins.
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
        /// Checks a simple float left div with text after, with padding, margins and an explicit width.
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
        /// Checks a simple unsized float right div with text after.
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
        /// Checks a simple unsized float left div with text after, padding, margins and an explicit width.
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
        /// Checks with two floating divs one on a line below the other - they should be offset by their natural
        /// width and push the text lines left. Following contained div is full width
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
        /// Checks with two floating divs with padding, margins and explicit width - one on a line below the other - they should be offset by their 
        /// width and push the text lines left. Following contained div is also offset for the first 2 lines then full width
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
        /// Checks with two floating divs with padding, margins and explicit width - one on a line below the other - they should be offset by their 
        /// width and push the text lines RIGHT. Following contained div is also offset for the first line then full width
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

            Assert.AreEqual(yOffset + 5, pos.TotalBounds.Y); //include nest padding
            Assert.AreEqual(30 + 30, pos.TotalBounds.Height);
            Assert.AreEqual(w, pos.TotalBounds.Width);

            Assert.AreEqual(pg.Width - (10 + firstFloatWidth + 20 + 5 + pos.TotalBounds.Width) , pos.TotalBounds.X); //nest margins + offset to the right + the float margins

            var after = nest.Columns[0].Contents[5] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(1, after.Columns.Length);
            Assert.AreEqual(2, after.Columns[0].Contents.Count);
            
            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(0, line.OffsetX);
            Assert.AreEqual(3, line.Runs.Count);
            Assert.IsTrue(line.Width < after.Width - 240); //line should be inset by at least the float widths

        }
        
        /// <summary>
        /// Checks with two floating divs with padding, margins and explicit width - one on a line below the other, one left and one right - they should be offset by their 
        /// width and push the text lines BOTH left and right. Following contained div is also offset for the first line then full width. Text should be centered in the
        /// available line width.
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
        /// Checks a simple float left div with text after on the second column of a multi-column layout to make sure it is correctly floated.
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
        /// Checks a simple float RIGHT div with text after on the second column of a
        /// multi-column layout to make sure it is correctly floated and text flows around it.
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
        /// Checks a  float LEFT and RIGHT div on the left and right columns with text after on both columns of a
        /// multi-column layout to make sure it is correctly floated and text flows around the left and right, along with the alignment.
        /// Inner div should be full width of the column, with the text again flowing around the floats.
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
        /// 2 rows of floats with inner mixed content. The floats should stack next to each other for left or right, and the text flow between the stacks
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
                
                var pos = first.PositionedRegions[index] as PDFLayoutPositionedRegion;
                Assert.IsNotNull(pos);
                Assert.AreEqual(30 + (60 * index), pos.TotalBounds.X);
                Assert.AreEqual(80, pos.TotalBounds.Y);
                Assert.AreEqual(55, pos.Height);
                Assert.AreEqual(60, pos.Width);
                
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
            
            var right = pg.Width - 30;
            index = 0;
            floatAddition = nest.Columns[0].Floats;
            
            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);
                
                var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                
                if (index < 6)
                {
                    Assert.AreEqual(0 + (60 * index), floatAddition.FloatInset);
                    Assert.AreEqual(60, floatAddition.FloatWidth);
                    Assert.AreEqual(45 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual((6 *15) + 10 + 10, floatAddition.YOffset); //6 lines plus <p> margins
                    Assert.AreEqual(FloatMode.Right, floatAddition.Mode);
                    
                    Assert.IsNotNull(pos);
                    xInset = 60 * index;
                    Assert.AreEqual(right - xInset - 60, pos.TotalBounds.X, "Float positioned region at index " + index + " failed"); //from the right including the width of this 
                    Assert.AreEqual(190 , pos.TotalBounds.Y);
                    Assert.AreEqual(55, pos.Height);
                    Assert.AreEqual(60, pos.Width);
                }
                else
                {
                    Assert.AreEqual(0, floatAddition.FloatInset);
                    Assert.AreEqual(60, floatAddition.FloatWidth);
                    Assert.AreEqual(45 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual((6 *15) + 10 + 10, floatAddition.YOffset); //6 lines plus <p> margins
                    Assert.AreEqual(FloatMode.Left, floatAddition.Mode);
                    
                    Assert.IsNotNull(pos);
                    xInset = 0;
                    Assert.AreEqual(30 + xInset, pos.TotalBounds.X, "Float positioned region at index " + index + " failed"); //from the right including the width of this 
                    Assert.AreEqual(190 , pos.TotalBounds.Y);
                    Assert.AreEqual(55, pos.Height);
                    Assert.AreEqual(60, pos.Width);
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
        
        /// <summary>
        /// 2 rows of floats with inner mixed content. First row is in a positioned relative block - so should move with the content.
        /// The floats should stack next to each other for left or right, and the text flow between the stacks. The space left with
        /// relative position, should not affect the second row of floating divs.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_14_ManyStackedMizedToRelativeParent()
        {

            var path = AssertGetContentFile("Float_14_ManyStackedMixedRelative");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_14_ManyStackedMixedRelative.pdf"))
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
            var y = 80 + 20 + 220; //relative offset + padding + relative top
            
            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);
                Unit inset;
                if (index <= 4)
                    inset = 0 + (60 * index);
                else
                    inset = 0 + (60 * (index - 5)); //these are right inset
                
                Assert.AreEqual(inset, floatAddition.FloatInset, "Failed inset at float index " + index);
                Assert.AreEqual(60, floatAddition.FloatWidth, "Failed float width at index " + index);
                Assert.AreEqual(45 + 10, floatAddition.FloatHeight, "Failed float height at index " + index);
                Assert.AreEqual(0, floatAddition.YOffset, "Failed float y offset at index " + index);
                
                var pos = first.PositionedRegions[index] as PDFLayoutPositionedRegion;
                Assert.IsNotNull(pos);

                if (index <= 4)
                    Assert.AreEqual(30 + 20 + 20 + (60 * index), pos.TotalBounds.X, "Failed X at index " + index); //left:-20
                else
                    Assert.AreEqual(pg.Width - 30 - ( (50 + 10) * (index - 4)), pos.TotalBounds.X, "Failed right X offset at index " + index ); //right - pg padding - width and margins
                Assert.AreEqual(y, pos.TotalBounds.Y, "Failed Y at index " + index); //top: 200
                Assert.AreEqual(55, pos.Height);
                Assert.AreEqual(60, pos.Width);
                
                index++;
                floatAddition = floatAddition.Next;
                
            }
            
            //everything else should stay the same.
            
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
            // four right, then two left
            // on the second line
            //
            
            Assert.AreEqual(6, nest.PositionedRegions.Count);
            
            var right = pg.Width - 30;
            
            index = 0;
            floatAddition = nest.Columns[0].Floats;
            
            while (null != floatAddition)
            {
                Assert.AreEqual(6 - index, floatAddition.Count);
                
                var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                
                if (index < 4)
                {
                    Assert.AreEqual(0 + (60 * index), floatAddition.FloatInset);
                    Assert.AreEqual(60, floatAddition.FloatWidth);
                    Assert.AreEqual(45 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual((6 *15) + 10 + 10, floatAddition.YOffset); //6 lines plus <p> margins
                    Assert.AreEqual(FloatMode.Right, floatAddition.Mode);
                    
                    Assert.IsNotNull(pos);
                    xInset = 60 * index;
                    Assert.AreEqual(right - xInset - 60 - 20, pos.TotalBounds.X, "Float positioned region at index " + index + " failed"); //from the right including the width of this and the nest margins 
                    Assert.AreEqual(190 + 20 , pos.TotalBounds.Y);
                    Assert.AreEqual(55, pos.Height);
                    Assert.AreEqual(60, pos.Width);
                    
                }
                else
                {
                    Assert.AreEqual(0 + (60 * (index - 4)), floatAddition.FloatInset);
                    Assert.AreEqual(60, floatAddition.FloatWidth);
                    Assert.AreEqual(45 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual((6 *15) + 10 + 10, floatAddition.YOffset); //6 lines plus <p> margins
                    Assert.AreEqual(FloatMode.Left, floatAddition.Mode);
                    
                    Assert.IsNotNull(pos);
                    xInset = 60 * (index - 4);
                    Assert.AreEqual(30 + xInset + 20, pos.TotalBounds.X, "Float positioned region at index " + index + " failed"); //from the right including the width of this 
                    Assert.AreEqual(190 + 20 , pos.TotalBounds.Y);
                    Assert.AreEqual(55, pos.Height);
                    Assert.AreEqual(60, pos.Width);
                }
                

                index++;
                floatAddition = floatAddition.Next;
            }
            
            xInset = 120;
            var rightInset = (4 * 60) ;
            
            //Top line above the floats.
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.TotalBounds.Width - 20 - 40, line.FullWidth); //margins

            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.TotalBounds.Width - (xInset + rightInset + 20 + 40 ), line.FullWidth);
            
            line = nest.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.TotalBounds.Width - (xInset + rightInset + 20 + 40), line.FullWidth);
            
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
        
        /// <summary>
        /// 2 rows of floats with inner mixed content. First row is in a positioned absolute block - so should move with the content.
        /// The floats should stack next to each other for left or right, and the text flow between the stacks. No spare space left with
        /// absolute position, so the second row of floating divs should move up with the content.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_15_ManyStackedMizedToAbsoluteParent()
        {

            var path = AssertGetContentFile("Float_15_ManyStackedMixedAbsolute");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_15_ManyStackedMixedAbsolute.pdf"))
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
            Assert.AreEqual(6, nest.Columns[0].Contents.Count); //5 lines + after block (first is absolute and outside of the flow)
            
            Assert.AreEqual(8, nest.PositionedRegions.Count); //1 absolute and 7 floats

            var firstRegion = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(firstRegion);
            
            var first = firstRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(first);
            
            Assert.AreEqual(1, first.Columns.Length);
            Assert.AreEqual(2, first.Columns[0].Contents.Count); //line for floats and para
            Assert.AreEqual(7, first.PositionedRegions.Count);

            var left = 0;
            var index = 0;
            var floatAddition = first.Columns[0].Floats;
            
            Unit[] insets = new Unit[] {0, 60, 120,  0, 60, 180, 240};
            Unit[] offsets = new Unit[] { 30, 90, 150, first.Width - 30, first.Width - 90, 210, 270 };
            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);
                
                Assert.AreEqual(insets[index], floatAddition.FloatInset);
                

                Assert.AreEqual(60, floatAddition.FloatWidth);
                Assert.AreEqual(45 + 10, floatAddition.FloatHeight);
                Assert.AreEqual(0, floatAddition.YOffset);
                
                var pos = first.PositionedRegions[index] as PDFLayoutPositionedRegion;
                Assert.IsNotNull(pos);
                Assert.AreEqual(offsets[index], pos.TotalBounds.X); //left:-20
                Assert.AreEqual(70 + 180, pos.TotalBounds.Y); //top: 70 to nest + 180 to first
                Assert.AreEqual(55, pos.Height);
                Assert.AreEqual(60, pos.Width);
                
                index++;
                floatAddition = floatAddition.Next;
                
            }
            
            
            
            Assert.AreEqual(7, index);

            var para = first.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(para);
            Assert.AreEqual(1, para.Columns.Length);
            Assert.AreEqual(6, para.Columns[0].Contents.Count); //on 6 lines

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
            
            
            
            var right = pg.Width - 30;
            
            index = 0;
            floatAddition = nest.Columns[0].Floats;
            
            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);
                
                //+1 as we have the absolute block at index 0
                var pos = nest.PositionedRegions[index + 1] as PDFLayoutPositionedRegion;
                
                if (index < 6)
                {
                    Assert.AreEqual(0 + (60 * index), floatAddition.FloatInset);
                    if (index == 5)
                    {
                        Assert.AreEqual(90 + 10, floatAddition.FloatWidth); //90pt wide and  therefor 2 lines high
                        Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    }
                    else
                    {
                        Assert.AreEqual(60, floatAddition.FloatWidth);
                        Assert.AreEqual(45 + 10, floatAddition.FloatHeight);
                    }
                    
                    Assert.AreEqual(15, floatAddition.YOffset); //after first line
                    Assert.AreEqual(FloatMode.Right, floatAddition.Mode);
                    
                    Assert.IsNotNull(pos);
                    xInset = 60 * index;
                    if (index == 5)
                    {
                        Assert.AreEqual(right - xInset - 100, pos.TotalBounds.X, "Float positioned region at index " + index + " failed"); //from the right including the width of this 
                        Assert.AreEqual(40, pos.Height);
                        Assert.AreEqual(100, pos.Width);
                    }
                    else
                    {
                        Assert.AreEqual(right - xInset - 60, pos.TotalBounds.X, "Float positioned region at index " + index + " failed"); //from the right including the width of this 
                        Assert.AreEqual(55, pos.Height);
                        Assert.AreEqual(60, pos.Width);
                    }
                    Assert.AreEqual(80 + 15 , pos.TotalBounds.Y);
                    
                    
                    
                }
                else
                {
                    Assert.AreEqual(0, floatAddition.FloatInset);
                    Assert.AreEqual(60, floatAddition.FloatWidth);
                    Assert.AreEqual(45 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual(15, floatAddition.YOffset); //1 line
                    Assert.AreEqual(FloatMode.Left, floatAddition.Mode);
                    
                    Assert.IsNotNull(pos);
                    xInset = 0;
                    Assert.AreEqual(30 + xInset, pos.TotalBounds.X, "Float positioned region at index " + index + " failed"); //from the right including the width of this 
                    Assert.AreEqual(80 + 15 , pos.TotalBounds.Y);
                    Assert.AreEqual(55, pos.Height);
                    Assert.AreEqual(60, pos.Width);
                }
                

                index++;
                floatAddition = floatAddition.Next;
            }
            
            xInset = 60;
            var rightInset = (5 * 60) + 100;
            
            //Top line above the floats.
            line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.TotalBounds.Width - 20, line.FullWidth); //margins

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.TotalBounds.Width - (xInset + rightInset + 20), line.FullWidth);
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.TotalBounds.Width - (xInset + rightInset + 20), line.FullWidth);
            
            //last line in the content should be wider by width - margin x 2
            
            line = nest.Columns[0].Contents[4] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.TotalBounds.Width + 90 - 10 - xInset - rightInset, line.FullWidth);
            
            //after block
            var after = nest.Columns[0].Contents[5] as PDFLayoutBlock;
            Assert.IsNotNull(after);
            Assert.AreEqual(2, after.Columns[0].Contents.Count);
            
            //first line of after should full width
            line = after.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(after.Width, line.FullWidth);
            
            //seconds line of after should be back to full width
            line = after.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(after.Width, line.FullWidth);
            
            


        }
        
        /// <summary>
        /// 2 rows of floats with inner mixed content. First row is in a positioned absolute block with a %age width - so should set the correct content width.
        /// The floats should stack next to each other for left or right, and the text flow between the stacks. No spare space left with
        /// absolute position, so the second row of floating divs should move up with the content mainatining the orginal width.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_16_ManyStackedMizedToAbsoluteParentCalculatedWidth()
        {

            var path = AssertGetContentFile("Float_16_ManyStackedMixedAbsoluteCalcWidth");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_16_ManyStackedMixedAbsoluteCalcWidth.pdf"))
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
            Assert.AreEqual(5, nest.Columns[0].Contents.Count); //4 lines + after block (first is absolute and outside of the flow)
            
            Assert.AreEqual(8, nest.PositionedRegions.Count); //1 absolute and 7 floats

            var firstRegion = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(firstRegion);
            
            var first = firstRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(first);

            var w = nest.Width * 0.9; //90% width
            
            
            Assert.AreEqual(1, first.Columns.Length);
            Assert.AreEqual(2, first.Columns[0].Contents.Count); //line for floats and para
            Assert.AreEqual(7, first.PositionedRegions.Count);
            Assert.AreEqual(w, first.Width); // make sure the 90% width is applied
            
            var left = 0;
            var index = 0;
            var floatAddition = first.Columns[0].Floats;
            
            Unit[] insets = new Unit[] {0, 60, 120,  0, 60, 180, 240};
            Unit[] offsets = new Unit[] { 30, 90, 150, first.Width - 30, first.Width - 90, 210, 270 };
            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);
                
                Assert.AreEqual(insets[index], floatAddition.FloatInset);
                

                Assert.AreEqual(60, floatAddition.FloatWidth);
                Assert.AreEqual(45 + 10, floatAddition.FloatHeight);
                Assert.AreEqual(0, floatAddition.YOffset);
                
                var pos = first.PositionedRegions[index] as PDFLayoutPositionedRegion;
                Assert.IsNotNull(pos);
                Assert.AreEqual(offsets[index], pos.TotalBounds.X); //left:-20
                Assert.AreEqual(190, pos.TotalBounds.Y); //top: 70 to nest + 120 to first
                Assert.AreEqual(55, pos.Height);
                Assert.AreEqual(60, pos.Width);
                
                index++;
                floatAddition = floatAddition.Next;
                
            }
            
            
            
            Assert.AreEqual(7, index);

            var para = first.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(para);
            Assert.AreEqual(1, para.Columns.Length);
            Assert.AreEqual(6, para.Columns[0].Contents.Count); //on 6 lines

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
            
            
            
            var right = pg.Width - 30;
            
            index = 0;
            floatAddition = nest.Columns[0].Floats;
            
            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);
                
                //+1 as we have the absolute block at index 0
                var pos = nest.PositionedRegions[index + 1] as PDFLayoutPositionedRegion;
                
                if (index < 6)
                {
                    Assert.AreEqual(0 + (60 * index), floatAddition.FloatInset);
                    Assert.AreEqual(60, floatAddition.FloatWidth);
                    Assert.AreEqual(45 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual(15, floatAddition.YOffset); //after first line
                    Assert.AreEqual(FloatMode.Right, floatAddition.Mode);
                    
                    Assert.IsNotNull(pos);
                    xInset = 60 * index;
                    Assert.AreEqual(right - xInset - 60, pos.TotalBounds.X, "Float positioned region at index " + index + " failed"); //from the right including the width of this 
                    Assert.AreEqual(80 + 15 , pos.TotalBounds.Y);
                    Assert.AreEqual(55, pos.Height);
                    Assert.AreEqual(60, pos.Width);
                    
                }
                else
                {
                    Assert.AreEqual(0, floatAddition.FloatInset);
                    Assert.AreEqual(60, floatAddition.FloatWidth);
                    Assert.AreEqual(45 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual(15, floatAddition.YOffset); //1 line
                    Assert.AreEqual(FloatMode.Left, floatAddition.Mode);
                    
                    Assert.IsNotNull(pos);
                    xInset = 0;
                    Assert.AreEqual(30 + xInset, pos.TotalBounds.X, "Float positioned region at index " + index + " failed"); //from the right including the width of this 
                    Assert.AreEqual(80 + 15 , pos.TotalBounds.Y);
                    Assert.AreEqual(55, pos.Height);
                    Assert.AreEqual(60, pos.Width);
                }
                

                index++;
                floatAddition = floatAddition.Next;
            }
            
            xInset = 60;
            var rightInset = 6 * 60;
            
            //Top line above the floats.
            line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.TotalBounds.Width - 20, line.FullWidth); //margins

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.TotalBounds.Width - (xInset + rightInset + 20), line.FullWidth);
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.TotalBounds.Width - (xInset + rightInset + 20), line.FullWidth);
            
            //after block
            var after = nest.Columns[0].Contents[4] as PDFLayoutBlock;
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
        
        /// <summary>
        /// 3 different layouts for LEFT aligned floats that all together are wider than the available space,
        /// so they should drop down to the next available left position that fits the float.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_17_ManyStackedLeftOverflowLine()
        {

            var path = AssertGetContentFile("Float_17_ManyStackedLeftOverflowLine");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_17_ManyStackedLeftOverflowLine.pdf"))
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
            Assert.AreEqual(6, content.Columns[0].Contents.Count);

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(5, nest.Columns[0].Contents.Count); //5 lines of text

            
            Assert.AreEqual(7, nest.PositionedRegions.Count);

            var left = 0;
            var index = 0;
            var floatAddition = nest.Columns[0].Floats;
            
            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);
                if (index < 5)
                {
                    Assert.AreEqual(0 + (90 * index), floatAddition.FloatInset);
                    Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                    Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual(0, floatAddition.YOffset);


                    var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                    Assert.IsNotNull(pos);
                    Assert.AreEqual(30 + (90 * index), pos.TotalBounds.X); 
                    Assert.AreEqual(80, pos.TotalBounds.Y);
                    Assert.AreEqual(40, pos.Height);
                    Assert.AreEqual(80 + 10, pos.Width);
                }
                else
                {
                    Assert.AreEqual(0 + (90 * (index - 5)), floatAddition.FloatInset);
                    Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                    Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual(40, floatAddition.YOffset);


                    var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                    Assert.IsNotNull(pos);
                    Assert.AreEqual(30 + (90 * (index - 5)), pos.TotalBounds.X); 
                    Assert.AreEqual(80 + 40, pos.TotalBounds.Y);
                    Assert.AreEqual(40, pos.Height);
                    Assert.AreEqual(80 + 10, pos.Width);
                    
                }

                index++;
                floatAddition = floatAddition.Next;
                
            }

            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (2 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[4] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (2 * 90) - 20 , line.FullWidth);
            
            // Second set
            
            nest = content.Columns[0].Contents[3] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(7, nest.Columns[0].Contents.Count); //7 lines of text

            
            Assert.AreEqual(7, nest.PositionedRegions.Count);
            left = 0; 
            index = 0; 
            floatAddition = nest.Columns[0].Floats;
            
            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);
                if (index < 5)
                {
                    Assert.AreEqual(0 + (90 * index), floatAddition.FloatInset);
                    Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                    
                    if (index == 3)
                        Assert.AreEqual(60 + 10, floatAddition.FloatHeight);
                    else
                        Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    
                    Assert.AreEqual(0, floatAddition.YOffset);


                    var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                    Assert.IsNotNull(pos);
                    Assert.AreEqual(30 + (90 * index), pos.TotalBounds.X); 
                    Assert.AreEqual(200, pos.TotalBounds.Y, "Failed pos.Y for index " + index);
                    
                    if (index == 3)
                        Assert.AreEqual(60 + 10, pos.Height);
                    else
                        Assert.AreEqual(40, pos.Height);
                    
                    Assert.AreEqual(80 + 10, pos.Width);
                }
                else if (index == 5)
                {
                    //positioned after the tall float
                    Assert.AreEqual(0 + (90 * 4), floatAddition.FloatInset);
                    Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                    Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual(40, floatAddition.YOffset);


                    var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                    Assert.IsNotNull(pos);
                    Assert.AreEqual(30 + (90 * 4), pos.TotalBounds.X); 
                    Assert.AreEqual(200 + 40, pos.TotalBounds.Y);
                    Assert.AreEqual(40, pos.Height);
                    Assert.AreEqual(80 + 10, pos.Width);
                }
                else
                {
                    Assert.AreEqual(0 , floatAddition.FloatInset);
                    Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                    Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual(80, floatAddition.YOffset);


                    var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                    Assert.IsNotNull(pos);
                    Assert.AreEqual(30, pos.TotalBounds.X); 
                    Assert.AreEqual(200 + 40 + 40, pos.TotalBounds.Y);
                    Assert.AreEqual(40, pos.Height);
                    Assert.AreEqual(80 + 10, pos.Width);
                    
                }

                index++;
                floatAddition = floatAddition.Next;
                
            }

            line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[4] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[5] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            //Third set

            nest = content.Columns[0].Contents[5] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(6, nest.Columns[0].Contents.Count); //6 lines of text

            
            Assert.AreEqual(7, nest.PositionedRegions.Count);
            left = 0; 
            index = 0; 
            floatAddition = nest.Columns[0].Floats;
            
            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);
                if (index < 5)
                {
                    Assert.AreEqual(0 + (90 * index), floatAddition.FloatInset);
                    Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                    
                    if (index == 1)
                        Assert.AreEqual(60 + 10, floatAddition.FloatHeight);
                    else
                        Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    
                    Assert.AreEqual(0, floatAddition.YOffset);


                    var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                    Assert.IsNotNull(pos);
                    Assert.AreEqual(30 + (90 * index), pos.TotalBounds.X); 
                    Assert.AreEqual(350, pos.TotalBounds.Y, "Failed pos.Y for index " + index);
                    
                    if (index == 1)
                        Assert.AreEqual(60 + 10, pos.Height);
                    else
                        Assert.AreEqual(40, pos.Height);
                    
                    Assert.AreEqual(80 + 10, pos.Width);
                }
                else if (index == 5)
                {
                    //positioned after the tall float
                    Assert.AreEqual(0 + (90 * 2), floatAddition.FloatInset);
                    Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                    Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual(40, floatAddition.YOffset);


                    var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                    Assert.IsNotNull(pos);
                    Assert.AreEqual(30 + (90 * 2), pos.TotalBounds.X); 
                    Assert.AreEqual(350 + 40, pos.TotalBounds.Y);
                    Assert.AreEqual(40, pos.Height);
                    Assert.AreEqual(80 + 10, pos.Width);
                }
                else
                {
                    Assert.AreEqual(0 + (90 * 3), floatAddition.FloatInset);
                    Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                    Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual(40, floatAddition.YOffset);


                    var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                    Assert.IsNotNull(pos);
                    Assert.AreEqual(30 + (90 * 3), pos.TotalBounds.X); 
                    Assert.AreEqual(350 + 40, pos.TotalBounds.Y);
                    Assert.AreEqual(40, pos.Height);
                    Assert.AreEqual(80 + 10, pos.Width);
                    
                }

                index++;
                floatAddition = floatAddition.Next;
                
            }

            line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (4 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[4] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (4 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[5] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (4 * 90) - 20 , line.FullWidth);

        }
        
        /// <summary>
        /// 3 different layouts for RIGHT aligned floats that all together are wider than the available space,
        /// so they should drop down to the next available right position that fits the float.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_18_ManyStackedRightOverflowLine()
        {

            var path = AssertGetContentFile("Float_18_ManyStackedRightOverflowLine");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_18_ManyStackedRightOverflowLine.pdf"))
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
            Assert.AreEqual(6, content.Columns[0].Contents.Count);

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(5, nest.Columns[0].Contents.Count); //5 lines of text

            
            Assert.AreEqual(7, nest.PositionedRegions.Count);

            var left = 0;
            var index = 0;
            var floatAddition = nest.Columns[0].Floats;
            
            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);
                if (index < 5)
                {
                    Assert.AreEqual(0 + (90 * index), floatAddition.FloatInset);
                    Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                    Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual(0, floatAddition.YOffset);


                    var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                    Assert.IsNotNull(pos);
                    Assert.AreEqual(pg.Width - 30 - (90 * (index + 1)), pos.TotalBounds.X); 
                    Assert.AreEqual(80, pos.TotalBounds.Y);
                    Assert.AreEqual(40, pos.Height);
                    Assert.AreEqual(80 + 10, pos.Width);
                }
                else
                {
                    Assert.AreEqual(0 + (90 * (index - 5)), floatAddition.FloatInset);
                    Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                    Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual(40, floatAddition.YOffset);


                    var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                    Assert.IsNotNull(pos);
                    Assert.AreEqual(pg.Width - 30 - (90 * (index - 4)), pos.TotalBounds.X); 
                    Assert.AreEqual(80 + 40, pos.TotalBounds.Y);
                    Assert.AreEqual(40, pos.Height);
                    Assert.AreEqual(80 + 10, pos.Width);
                    
                }

                index++;
                floatAddition = floatAddition.Next;
                
            }

            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (2 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[4] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (2 * 90) - 20 , line.FullWidth);
            
            // Second set
            
            nest = content.Columns[0].Contents[3] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(7, nest.Columns[0].Contents.Count); //7 lines of text

            
            Assert.AreEqual(7, nest.PositionedRegions.Count);
            left = 0; 
            index = 0; 
            floatAddition = nest.Columns[0].Floats;
            
            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);
                if (index < 5)
                {
                    Assert.AreEqual(0 + (90 * index), floatAddition.FloatInset);
                    Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                    
                    if (index == 3)
                        Assert.AreEqual(60 + 10, floatAddition.FloatHeight);
                    else
                        Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    
                    Assert.AreEqual(0, floatAddition.YOffset);


                    var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                    Assert.IsNotNull(pos);
                    Assert.AreEqual(pg.Width - 30 - (90 * (index + 1)), pos.TotalBounds.X); 
                    Assert.AreEqual(200, pos.TotalBounds.Y, "Failed pos.Y for index " + index);
                    
                    if (index == 3)
                        Assert.AreEqual(60 + 10, pos.Height);
                    else
                        Assert.AreEqual(40, pos.Height);
                    
                    Assert.AreEqual(80 + 10, pos.Width);
                }
                else if (index == 5)
                {
                    //positioned left of the tall float
                    Assert.AreEqual(0 + (90 * 4), floatAddition.FloatInset);
                    Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                    Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual(40, floatAddition.YOffset);


                    var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                    Assert.IsNotNull(pos);
                    Assert.AreEqual(pg.Width - 30 - (90 * 5), pos.TotalBounds.X); 
                    Assert.AreEqual(200 + 40, pos.TotalBounds.Y);
                    Assert.AreEqual(40, pos.Height);
                    Assert.AreEqual(80 + 10, pos.Width);
                }
                else
                {
                    Assert.AreEqual(0 , floatAddition.FloatInset);
                    Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                    Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual(80, floatAddition.YOffset);


                    var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                    Assert.IsNotNull(pos);
                    Assert.AreEqual(pg.Width - 30 - 90, pos.TotalBounds.X); 
                    Assert.AreEqual(200 + 40 + 40, pos.TotalBounds.Y);
                    Assert.AreEqual(40, pos.Height);
                    Assert.AreEqual(80 + 10, pos.Width);
                    
                }

                index++;
                floatAddition = floatAddition.Next;
                
            }

            line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[4] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[5] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            //Third set

            nest = content.Columns[0].Contents[5] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(6, nest.Columns[0].Contents.Count); //6 lines of text

            
            Assert.AreEqual(7, nest.PositionedRegions.Count);
            left = 0; 
            index = 0; 
            floatAddition = nest.Columns[0].Floats;
            
            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);
                if (index < 5)
                {
                    Assert.AreEqual(0 + (90 * index), floatAddition.FloatInset);
                    Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                    
                    if (index == 1)
                        Assert.AreEqual(60 + 10, floatAddition.FloatHeight);
                    else
                        Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    
                    Assert.AreEqual(0, floatAddition.YOffset);


                    var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                    Assert.IsNotNull(pos);
                    Assert.AreEqual(pg.Width - 30 - (90 * (index + 1)), pos.TotalBounds.X); 
                    Assert.AreEqual(350, pos.TotalBounds.Y, "Failed pos.Y for index " + index);
                    
                    if (index == 1)
                        Assert.AreEqual(60 + 10, pos.Height);
                    else
                        Assert.AreEqual(40, pos.Height);
                    
                    Assert.AreEqual(80 + 10, pos.Width);
                }
                else if (index == 5)
                {
                    //positioned after the tall float
                    Assert.AreEqual(0 + (90 * 2), floatAddition.FloatInset);
                    Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                    Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual(40, floatAddition.YOffset);


                    var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                    Assert.IsNotNull(pos);
                    Assert.AreEqual(pg.Width - 30 - (90 * 3), pos.TotalBounds.X); 
                    Assert.AreEqual(350 + 40, pos.TotalBounds.Y);
                    Assert.AreEqual(40, pos.Height);
                    Assert.AreEqual(80 + 10, pos.Width);
                }
                else
                {
                    Assert.AreEqual(0 + (90 * 3), floatAddition.FloatInset);
                    Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                    Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                    Assert.AreEqual(40, floatAddition.YOffset);


                    var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                    Assert.IsNotNull(pos);
                    Assert.AreEqual(pg.Width - 30  - (90 * 4), pos.TotalBounds.X); 
                    Assert.AreEqual(350 + 40, pos.TotalBounds.Y);
                    Assert.AreEqual(40, pos.Height);
                    Assert.AreEqual(80 + 10, pos.Width);
                    
                }

                index++;
                floatAddition = floatAddition.Next;
                
            }

            line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (4 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[4] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (4 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[5] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (4 * 90) - 20 , line.FullWidth);



        }
        
        /// <summary>
        /// 3 different layouts for BOTH left and right aligned floats that all together are wider than the available space,
        /// so they should drop down to the next available position that fits the float, taking into account both the left and the right insets in the lines.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_19_ManyStackedMixedOverflowLine()
        {

            var path = AssertGetContentFile("Float_19_ManyStackedMixedOverflowLine");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_19_ManyStackedMixedOverflowLine.pdf"))
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
            Assert.AreEqual(6, content.Columns[0].Contents.Count);

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(5, nest.Columns[0].Contents.Count); //5 lines of text

            
            Assert.AreEqual(7, nest.PositionedRegions.Count);

            var left = 0;
            var index = 0;
            var floatAddition = nest.Columns[0].Floats;
            var insets = new Unit[] { 0, 90, 0, 90, 180, 0, 90 };
            var posXs = new Unit[] { 30, 120, pg.Width - 30 - 90, pg.Width - 210, 210, 30, 120  };
            
            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);
                Assert.AreEqual(insets[index], floatAddition.FloatInset);
                Assert.AreEqual(80 + 10, floatAddition.FloatWidth);
                Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                if (index >= 5)
                    Assert.AreEqual(40, floatAddition.YOffset);
                else
                    Assert.AreEqual(0, floatAddition.YOffset);


                var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                Assert.IsNotNull(pos);
                Assert.AreEqual(posXs[index], pos.TotalBounds.X);

                if (index >= 5)
                    Assert.AreEqual(120, pos.TotalBounds.Y);
                else
                    Assert.AreEqual(80, pos.TotalBounds.Y);
                Assert.AreEqual(40, pos.Height);
                Assert.AreEqual(80 + 10, pos.Width);


                index++;
                floatAddition = floatAddition.Next;

            }

            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (2 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[4] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (2 * 90) - 20 , line.FullWidth);
            
            // Second set
            
            nest = content.Columns[0].Contents[3] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(6, nest.Columns[0].Contents.Count); //6 lines of text

            
            Assert.AreEqual(7, nest.PositionedRegions.Count);
            left = 0; 
            index = 0; 
            floatAddition = nest.Columns[0].Floats;
            
            insets = new Unit[] { 0, 90, 0, 90, 180, 0, 90 };
            posXs = new Unit[] { 30, 120, pg.Width - 30 - 90, pg.Width - 210, 210, 30, 120  };

            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);
                Assert.AreEqual(insets[index], floatAddition.FloatInset, "Failed at index " + index);
                Assert.AreEqual(80 + 10, floatAddition.FloatWidth);

                if (index == 3)
                    Assert.AreEqual(60 + 10, floatAddition.FloatHeight);
                else
                    Assert.AreEqual(30 + 10, floatAddition.FloatHeight);

                if (index >= 5)
                    Assert.AreEqual(40, floatAddition.YOffset, "Failed pos.Y for index " + index);
                else
                    Assert.AreEqual(0, floatAddition.YOffset, "Failed pos.Y for index " + index);


                var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                Assert.IsNotNull(pos);
                Assert.AreEqual(posXs[index], pos.TotalBounds.X, "Failed pos.Y for index " + index);
                
                if (index >= 5)
                    Assert.AreEqual(240, pos.TotalBounds.Y, "Failed pos.Y for index " + index);
                else
                    Assert.AreEqual(200, pos.TotalBounds.Y, "Failed pos.Y for index " + index);
                
                
                if (index == 3)
                    Assert.AreEqual(60 + 10, pos.Height);
                else
                    Assert.AreEqual(40, pos.Height);

                Assert.AreEqual(80 + 10, pos.Width);



                index++;
                floatAddition = floatAddition.Next;

            }

            line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (4 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[4] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (4 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[5] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (2 * 90) - 20 , line.FullWidth);
            
            //Third set

            nest = content.Columns[0].Contents[5] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(5, nest.Columns[0].Contents.Count); //5 lines of text

            
            Assert.AreEqual(7, nest.PositionedRegions.Count);
            left = 0; 
            index = 0; 
            floatAddition = nest.Columns[0].Floats;
            
            insets = new Unit[] { 0, 90, 0, 90, 180, 180, 0 };
            posXs = new Unit[] { 30, 120, pg.Width - 30 - 90, pg.Width - 210, 210, 210, 30  };

            while (null != floatAddition)
            {
                Assert.AreEqual(7 - index, floatAddition.Count);

                Assert.AreEqual(insets[index], floatAddition.FloatInset);
                Assert.AreEqual(80 + 10, floatAddition.FloatWidth);

                if (index == 1)
                    Assert.AreEqual(60 + 10, floatAddition.FloatHeight);
                else
                    Assert.AreEqual(30 + 10, floatAddition.FloatHeight);

                if (index < 5)
                    Assert.AreEqual(0, floatAddition.YOffset);
                else if (index == 5)
                    Assert.AreEqual(40, floatAddition.YOffset);
                else
                    Assert.AreEqual(80, floatAddition.YOffset);


                var pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                Assert.IsNotNull(pos);
                Assert.AreEqual(posXs[index], pos.TotalBounds.X);
                if (index < 5)
                    Assert.AreEqual(335, pos.TotalBounds.Y, "Failed pos.Y for index " + index);
                else if(index == 5)
                    Assert.AreEqual(375, pos.TotalBounds.Y, "Failed pos.Y for index " + index);
                else
                    Assert.AreEqual(415, pos.TotalBounds.Y, "Failed pos.Y for index " + index);

                if (index == 1)
                    Assert.AreEqual(60 + 10, pos.Height);
                else
                    Assert.AreEqual(40, pos.Height);

                Assert.AreEqual(80 + 10, pos.Width);

                


                index++;
                floatAddition = floatAddition.Next;

            }

            line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (5 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (3 * 90) - 20 , line.FullWidth);
            
            line = nest.Columns[0].Contents[4] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - (3 * 90) - 20 , line.FullWidth);
            
            //line = nest.Columns[0].Contents[5] as PDFLayoutLine; - only 5 lines
            

        }
        
        /// <summary>
        /// 2 column layout with 6 floats. 2 on the first column, 4 overflow to the next column and
        /// the text is then wrapped around these
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_20_ManyStackedLeftOverflowColumn()
        {

            var path = AssertGetContentFile("Float_20_ManyStackedLeftOverflowColumn");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_20_ManyStackedLeftOverflowColumn.pdf"))
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

            var columns = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(columns);
            Assert.AreEqual(2, columns.Columns.Length);
            
            //first column - spacer and nest part 1
            Assert.AreEqual(2, columns.Columns[0].Contents.Count);
            
            var nest = columns.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.Columns.Length);
            
            //2 floats on this first block
            Assert.AreEqual(2, nest.PositionedRegions.Count);
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(2, line.Runs.Count); //make sure the overflow runs are not on this line
            Assert.IsNotNull(nest.Columns[0].Floats);
            Assert.AreEqual(2, nest.Columns[0].Floats.Count);
            
            //first float
            var floatAddition = nest.Columns[0].Floats;
            
            Assert.AreEqual(2, floatAddition.Count);
            Assert.AreEqual(0, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.YOffset);
                
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(30, pos.TotalBounds.X);
            Assert.AreEqual(80 + 650, pos.TotalBounds.Y); //heading + spacer
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            //second float
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            
            Assert.AreEqual(1, floatAddition.Count);
            Assert.AreEqual(90, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.YOffset);
                
            pos = nest.PositionedRegions[1] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(30 + 90, pos.TotalBounds.X);
            Assert.AreEqual(80 + 650, pos.TotalBounds.Y); //heading + spacer
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            //
            //Second column contains 4 floats and the text
            //
            
            nest = columns.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.Columns.Length);
            
            //4 floats on this first block
            //And 8 lines
            Assert.AreEqual(4, nest.PositionedRegions.Count);
            Assert.AreEqual(8, nest.Columns[0].Contents.Count);
            Assert.IsNotNull(nest.Columns[0].Floats);
            Assert.AreEqual(4, nest.Columns[0].Floats.Count);
            
            //first float
            floatAddition = nest.Columns[0].Floats;
            
            Assert.AreEqual(4, floatAddition.Count);
            Assert.AreEqual(0, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.YOffset);
                
            pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(30 + columns.Columns[0].Width + 10, pos.TotalBounds.X); //include the left column and gutter
            Assert.AreEqual(80, pos.TotalBounds.Y); //heading only as back up to the top
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            //second float
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            
            Assert.AreEqual(3, floatAddition.Count);
            Assert.AreEqual(90, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.YOffset);
                
            pos = nest.PositionedRegions[1] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(30 + 90 + columns.Columns[0].Width + 10, pos.TotalBounds.X);
            Assert.AreEqual(80, pos.TotalBounds.Y); //heading only
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            //third float
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            
            Assert.AreEqual(2, floatAddition.Count);
            Assert.AreEqual(0, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(40, floatAddition.YOffset);
                
            pos = nest.PositionedRegions[2] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(30 + columns.Columns[0].Width + 10, pos.TotalBounds.X);
            Assert.AreEqual(80 + 40, pos.TotalBounds.Y); //heading  + a float
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            //fourth float
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            
            Assert.AreEqual(1, floatAddition.Count);
            Assert.AreEqual(90, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(40, floatAddition.YOffset);
                
            pos = nest.PositionedRegions[3] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(30 + 90 + columns.Columns[0].Width + 10, pos.TotalBounds.X);
            Assert.AreEqual(80 + 40, pos.TotalBounds.Y); //heading + a float
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);

            for (var i = 0; i < 6; i++)
            {
                line = nest.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.IsNotNull(line);
                if (i == 0)
                {
                    Assert.AreEqual(7, line.Runs.Count); //4 pos runs + start, chars, end
                }
                else
                {
                    Assert.AreEqual(3, line.Runs.Count);
                }
                Assert.AreEqual(nest.Width - 180 - 20, line.FullWidth); //outer - margins and 2 floats
            }
            
            //back to full width
            line = nest.Columns[0].Contents[6] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - 20, line.FullWidth);
            

        }
        
        /// <summary>
        /// Single column layout where the floating blocks overflow and force a new page.
        /// This will then have 2 floats on it and the text will flow around them.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_21_ManyStackedLeftOverflowPage()
        {

            var path = AssertGetContentFile("Float_21_ManyStackedLeftOverflowPage");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_21_ManyStackedLeftOverflowPage.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.1;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;


            Assert.AreEqual(0, content.PositionedRegions.Count);
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);

            var columns = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(columns);
            Assert.AreEqual(1, columns.Columns.Length);
            
            //first column - spacer and nest part 1
            Assert.AreEqual(2, columns.Columns[0].Contents.Count);
            
            var nest = columns.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.Columns.Length);
            Assert.AreEqual(1, nest.Columns[0].Contents.Count);
            
            //5 floats on this first block and 1 line
            Assert.AreEqual(5, nest.PositionedRegions.Count);
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3 + 5, line.Runs.Count); //First line is text, with the floats after
            Assert.IsNotNull(nest.Columns[0].Floats);
            Assert.AreEqual(5, nest.Columns[0].Floats.Count);
            
            //check the floats
            int index = 0;
            var floatAddition = nest.Columns[0].Floats;
            PDFLayoutPositionedRegion pos;
            
            while (null != floatAddition)
            {
                Assert.AreEqual(5 - index, floatAddition.Count);
                Assert.AreEqual(0 + (90 * index), floatAddition.FloatInset);
                Assert.AreEqual(90, floatAddition.FloatWidth);
                Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
                Assert.AreEqual(15, floatAddition.YOffset);
                
                pos = nest.PositionedRegions[index] as PDFLayoutPositionedRegion;
                Assert.IsNotNull(pos);
                Assert.AreEqual(30 + (90 * index), pos.TotalBounds.X);
                Assert.AreEqual(80 + 650 + 15, pos.TotalBounds.Y); //heading + spacer + line height
                Assert.AreEqual(40, pos.Height);
                Assert.AreEqual(90, pos.Width);

                index++;
                floatAddition = floatAddition.Next;
            }
            
            
            //
            //Second page
            //
            pg = layout.AllPages[1];
            Assert.IsNotNull(pg);
            content = pg.ContentBlock;
            columns = content.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(columns);
            
            //overflowing nest
            nest = columns.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.Columns.Length);
            
            //2 floats on this block
            //And 4 lines
            Assert.AreEqual(2, nest.PositionedRegions.Count);
            Assert.AreEqual(4, nest.Columns[0].Contents.Count);
            Assert.IsNotNull(nest.Columns[0].Floats);
            Assert.AreEqual(2, nest.Columns[0].Floats.Count);
            
            //first float
            floatAddition = nest.Columns[0].Floats;
            
            Assert.AreEqual(2, floatAddition.Count);
            Assert.AreEqual(0, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.YOffset);
                
            pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(20  + 10, pos.TotalBounds.X);
            Assert.AreEqual(20 + 10, pos.TotalBounds.Y); 
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            //second float
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            
            Assert.AreEqual(1, floatAddition.Count);
            Assert.AreEqual(90, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.YOffset);
                
            pos = nest.PositionedRegions[1] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(20 + 90 + 10, pos.TotalBounds.X);
            Assert.AreEqual(20 + 10, pos.TotalBounds.Y); 
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            

            for (var i = 0; i < 3; i++)
            {
                line = nest.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.IsNotNull(line);
                if (i == 0)
                {
                    Assert.AreEqual(5, line.Runs.Count); //2 pos runs + start, chars, end
                }
                else
                {
                    Assert.AreEqual(3, line.Runs.Count);
                }
                Assert.AreEqual(nest.Width - 180 - 20, line.FullWidth, "Failed at index " + i); //outer - margins and 2 floats
            }
            
            //back to full width
            line = nest.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - 20, line.FullWidth);
            


        }
        
        /// <summary>
        /// Dual column layout where the floating blocks overflow and force a new page.
        /// This will then have 4 floats on it and the text will flow around them.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_22_ManyStackedLeftOverflowColumnAndPage()
        {

            var path = AssertGetContentFile("Float_22_ManyStackedLeftOverflowColumnAndPage");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_22_ManyStackedLeftOverflowColumnAndPage.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.1;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(ms);
            }

           
            // 2 pages

            Assert.AreEqual(2, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;


            Assert.AreEqual(0, content.PositionedRegions.Count);
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);

            // spacer on the first column
            
            var columns = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(columns);
            Assert.AreEqual(2, columns.Columns.Length);
            
            //first column - spacer and nest part 1
            Assert.AreEqual(1, columns.Columns[0].Contents.Count);
            
            // second column
            
            Assert.AreEqual(2, columns.Columns[1].Contents.Count);
            
            var nest = columns.Columns[1].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.Columns.Length);
            
            //2 floats on this first block
            Assert.AreEqual(2, nest.PositionedRegions.Count);
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(5, line.Runs.Count); //begin, chars, end, float, float
            Assert.IsNotNull(nest.Columns[0].Floats);
            Assert.AreEqual(2, nest.Columns[0].Floats.Count);
            
            //first float
            var floatAddition = nest.Columns[0].Floats;
            
            Assert.AreEqual(2, floatAddition.Count);
            Assert.AreEqual(0, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(15, floatAddition.YOffset); //line offset
                
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(30 + columns.Columns[0].Width + 10, pos.TotalBounds.X); // margins, a column, a gutter
            Assert.AreEqual(80 + 650 + 15, pos.TotalBounds.Y); //heading + spacer + line
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            //second float
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            
            Assert.AreEqual(1, floatAddition.Count);
            Assert.AreEqual(90, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(15, floatAddition.YOffset); // line offset
                
            pos = nest.PositionedRegions[1] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(30+ columns.Columns[0].Width + 10 + 90, pos.TotalBounds.X); //margins, a column, a gutter and a float
            Assert.AreEqual(80 + 650 + 15, pos.TotalBounds.Y); //heading + spacer + line
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            //
            //First columns on the second page contains 4 floats and the text
            //

            pg = layout.AllPages[1];
            Assert.IsNotNull(pg);
            columns = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(columns);
            Assert.AreEqual(2, columns.Columns.Length);
            
            //first column, now at the top
            nest = columns.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.Columns.Length);
            
            //4 floats on this first block
            //And 8 lines
            Assert.AreEqual(4, nest.PositionedRegions.Count);
            Assert.AreEqual(8, nest.Columns[0].Contents.Count);
            Assert.IsNotNull(nest.Columns[0].Floats);
            Assert.AreEqual(4, nest.Columns[0].Floats.Count);
            
            //first float
            floatAddition = nest.Columns[0].Floats;
            
            Assert.AreEqual(4, floatAddition.Count);
            Assert.AreEqual(0, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.YOffset);
                
            pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(30 , pos.TotalBounds.X); //back at the left inc margins
            Assert.AreEqual(30, pos.TotalBounds.Y); //page top inc margins
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            //second float
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            
            Assert.AreEqual(3, floatAddition.Count);
            Assert.AreEqual(90, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.YOffset);
                
            pos = nest.PositionedRegions[1] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(30 + 90, pos.TotalBounds.X);
            Assert.AreEqual(30, pos.TotalBounds.Y); //heading only
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            //third float
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            
            Assert.AreEqual(2, floatAddition.Count);
            Assert.AreEqual(0, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(40, floatAddition.YOffset);
                
            pos = nest.PositionedRegions[2] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(30 , pos.TotalBounds.X);
            Assert.AreEqual(40 + 30, pos.TotalBounds.Y); //margins + a float
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            //fourth float
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            
            Assert.AreEqual(1, floatAddition.Count);
            Assert.AreEqual(90, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(40, floatAddition.YOffset);
                
            pos = nest.PositionedRegions[3] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(30 + 90, pos.TotalBounds.X);
            Assert.AreEqual(40 + 30, pos.TotalBounds.Y); //margins + a float
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);

            for (var i = 0; i < 6; i++)
            {
                line = nest.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.IsNotNull(line);
                if (i == 0)
                {
                    Assert.AreEqual(7, line.Runs.Count); //4 pos runs + start, chars, end
                }
                else
                {
                    Assert.AreEqual(3, line.Runs.Count);
                }
                Assert.AreEqual(nest.Width - 180 - 20, line.FullWidth); //outer - margins and 2 floats
            }
            
            //back to full width
            line = nest.Columns[0].Contents[6] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - 20, line.FullWidth);

        }
        
        /// <summary>
        /// Known issue when the parent is relative
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_23_ManyStackedRightOverflowColumnAndPage()
        {

            var path = AssertGetContentFile("Float_23_ManyStackedRightOverflowColumnAndPage");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_23_ManyStackedRightOverflowColumnAndPage.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.1;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(ms);
            }

            // 2 pages

            Assert.AreEqual(2, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;


            Assert.AreEqual(0, content.PositionedRegions.Count);
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);

            // spacer on the first column
            
            var columns = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(columns);
            Assert.AreEqual(2, columns.Columns.Length);
            
            //first column - spacer and nest part 1
            Assert.AreEqual(1, columns.Columns[0].Contents.Count);
            
            // second column
            
            Assert.AreEqual(2, columns.Columns[1].Contents.Count);
            
            var nest = columns.Columns[1].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.Columns.Length);
            
            //2 floats on this first block
            Assert.AreEqual(2, nest.PositionedRegions.Count);
            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(5, line.Runs.Count); //begin, chars, end, float, float
            Assert.IsNotNull(nest.Columns[0].Floats);
            Assert.AreEqual(2, nest.Columns[0].Floats.Count);
            
            //first float
            var floatAddition = nest.Columns[0].Floats;
            
            Assert.AreEqual(2, floatAddition.Count);
            Assert.AreEqual(0, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(15, floatAddition.YOffset); //line offset
                
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(pg.Width - 30 - 90, pos.TotalBounds.X); // right - margins and width
            Assert.AreEqual(80 + 650 + 15, pos.TotalBounds.Y); //heading + spacer + line
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            //second float
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            
            Assert.AreEqual(1, floatAddition.Count);
            Assert.AreEqual(90, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(15, floatAddition.YOffset); // line offset
                
            pos = nest.PositionedRegions[1] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(pg.Width - 30 - 180, pos.TotalBounds.X); //right - margins, a float and width
            Assert.AreEqual(80 + 650 + 15, pos.TotalBounds.Y); //heading + spacer + line
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            //
            //First columns on the second page contains 4 floats and the text
            //

            pg = layout.AllPages[1];
            Assert.IsNotNull(pg);
            columns = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(columns);
            Assert.AreEqual(2, columns.Columns.Length);
            
            //first column, now at the top
            nest = columns.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.Columns.Length);
            
            //4 floats on this first block
            //And 8 lines
            Assert.AreEqual(4, nest.PositionedRegions.Count);
            Assert.AreEqual(8, nest.Columns[0].Contents.Count);
            Assert.IsNotNull(nest.Columns[0].Floats);
            Assert.AreEqual(4, nest.Columns[0].Floats.Count);
            
            //first float
            floatAddition = nest.Columns[0].Floats;
            
            Assert.AreEqual(4, floatAddition.Count);
            Assert.AreEqual(0, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.YOffset);

            var absColumnRight = 20 + nest.Width;
            pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(absColumnRight - 10 - 90, pos.TotalBounds.X); //back at the right of nesting inc margins and width
            Assert.AreEqual(30, pos.TotalBounds.Y); //page top inc margins
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            //second float
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            
            Assert.AreEqual(3, floatAddition.Count);
            Assert.AreEqual(90, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.YOffset);
                
            pos = nest.PositionedRegions[1] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(absColumnRight - 10 - 90 - 90, pos.TotalBounds.X);
            Assert.AreEqual(30, pos.TotalBounds.Y); //heading only
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            //third float
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            
            Assert.AreEqual(2, floatAddition.Count);
            Assert.AreEqual(0, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(40, floatAddition.YOffset);
                
            pos = nest.PositionedRegions[2] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(absColumnRight - 10 - 90, pos.TotalBounds.X);
            Assert.AreEqual(40 + 30, pos.TotalBounds.Y); //margins + a float
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);
            
            //fourth float
            floatAddition = floatAddition.Next;
            Assert.IsNotNull(floatAddition);
            
            Assert.AreEqual(1, floatAddition.Count);
            Assert.AreEqual(90, floatAddition.FloatInset);
            Assert.AreEqual(90, floatAddition.FloatWidth);
            Assert.AreEqual(30 + 10, floatAddition.FloatHeight);
            Assert.AreEqual(40, floatAddition.YOffset);
                
            pos = nest.PositionedRegions[3] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(absColumnRight - 10 - 90 - 90, pos.TotalBounds.X);
            Assert.AreEqual(40 + 30, pos.TotalBounds.Y); //margins + a float
            Assert.AreEqual(40, pos.Height);
            Assert.AreEqual(90, pos.Width);

            for (var i = 0; i < 6; i++)
            {
                line = nest.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.IsNotNull(line);
                if (i == 0)
                {
                    Assert.AreEqual(7, line.Runs.Count); //4 pos runs + start, chars, end
                }
                else
                {
                    Assert.AreEqual(3, line.Runs.Count);
                }
                Assert.AreEqual(nest.Width - 180 - 20, line.FullWidth); //outer - margins and 2 floats
            }
            
            //back to full width
            line = nest.Columns[0].Contents[6] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(nest.Width - 20, line.FullWidth);
            

        }
        
        
        
        /// <summary>
        /// Known issues when parent is flating too
        /// 1. The float left blocks are inset by the float padding amount. (.floating{ padding: 10pt})
        /// 2. The text rendering of the content after the float blocks is at the samle baseline as the floating text.
        /// Screenshot - 7th Aug 20:15
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Float_24_ManyInsideAnotherFloat()
        {

            var path = AssertGetContentFile("Float_24_ManyInsideAnotherFloat");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_24_ManyInsideAnotherFloat.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.1;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(ms);
            }

            // 1 page

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;


            Assert.AreEqual(0, content.PositionedRegions.Count);
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(3, content.Columns[0].Contents.Count);

            // spacer on the first column
            
            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.Columns.Length);
            
            //float and span over 5 lines
            Assert.AreEqual(5, nest.Columns[0].Contents.Count);
            Assert.AreEqual(1, nest.PositionedRegions.Count);

            var outerPos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(outerPos);
            Assert.AreEqual(1, outerPos.Contents.Count);

            var outer = outerPos.Contents[0] as PDFLayoutBlock;
            //3 floats on the outer block and 2 lines of content
            Assert.IsNotNull(outer);
            Assert.AreEqual(3, outer.PositionedRegions.Count);
            Assert.AreEqual(2, outer.Columns[0].Contents.Count);
            
            //check the 3 floats in the outerFloat
            //first float
            var floatAddition = outer.Columns[0].Floats;
            var pos = outer.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            var charsBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(charsBlock);
            var charsLine = charsBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(charsLine);
            var chars = charsLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            var width1 = chars.Width + 20;
            
            Assert.AreEqual(3, floatAddition.Count);
            Assert.AreEqual(0, floatAddition.FloatInset);
            Assert.AreEqual(width1, floatAddition.FloatWidth);
            Assert.AreEqual(15 + 20, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.YOffset);
            
            Assert.AreEqual(30, pos.TotalBounds.X); // right - margins and width
            Assert.AreEqual(80, pos.TotalBounds.Y); //heading + spacer + line
            Assert.AreEqual(35, pos.Height);
            Assert.AreEqual(width1, pos.Width);
            
            
            //second float
            floatAddition = floatAddition.Next;
            pos = outer.PositionedRegions[1] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            charsBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(charsBlock);
            charsLine = charsBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(charsLine);
            chars = charsLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            var width2 = chars.Width + 20;
            
            Assert.AreEqual(2, floatAddition.Count);
            Assert.AreEqual(width1, floatAddition.FloatInset);
            Assert.AreEqual(width2, floatAddition.FloatWidth);
            Assert.AreEqual(15 + 20, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.YOffset); 
            
            Assert.AreEqual(30 + width1, pos.TotalBounds.X); // right - margins and width
            Assert.AreEqual(80, pos.TotalBounds.Y); //heading + spacer + line
            Assert.AreEqual(15 + 20, pos.Height);
            Assert.AreEqual(width2, pos.Width);
            
            //third (right) float
            floatAddition = floatAddition.Next;
            pos = outer.PositionedRegions[2] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            charsBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(charsBlock);
            charsLine = charsBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(charsLine);
            chars = charsLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            var width3 = chars.Width + 20;
            
            Assert.IsNotNull(floatAddition);
            
            Assert.AreEqual(1, floatAddition.Count);
            Assert.AreEqual(0, floatAddition.FloatInset);
            Assert.AreEqual(width3, floatAddition.FloatWidth);
            Assert.AreEqual(15 + 20, floatAddition.FloatHeight);
            Assert.AreEqual(15 + 20, floatAddition.YOffset); // below the 2 left floats

            var right = outer.Width + 20 ; //left + container width
            Assert.AreEqual(right - width3 - 10, pos.TotalBounds.X); //right - width and padding
            Assert.AreEqual(80 + 35, pos.TotalBounds.Y); //heading + float
            Assert.AreEqual(35, pos.Height);
            Assert.AreEqual(width3, pos.Width);
            
            
            
            //check the content of nest that contains the outer float
            floatAddition = nest.Columns[0].Floats;
            pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.IsNotNull(floatAddition);
            
            Assert.AreEqual(1, floatAddition.Count);
            Assert.AreEqual(0, floatAddition.FloatInset);
            Assert.AreEqual(outer.Width, floatAddition.FloatWidth);
            Assert.AreEqual((15*5) + 20, floatAddition.FloatHeight); //five lines of content + padding
            Assert.AreEqual(0, floatAddition.YOffset); // below the 2 left floats

            
            Assert.AreEqual(20, pos.TotalBounds.X); //left with body margin
            Assert.AreEqual(70, pos.TotalBounds.Y); //heading and boy margin
            Assert.AreEqual((15*5) + 20, pos.Height);
            Assert.AreEqual(outer.Width, pos.Width);
            
            //check the lines of text to the right - already checked the count as 5
            for (var i = 0; i < nest.Columns[0].Contents.Count; i++)
            {
                var line = nest.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.IsNotNull(line);
                if (i == 0)
                {
                    //first line has pos run inline start then begin with inset.
                    var start = line.Runs[2] as PDFTextRunBegin;
                    Assert.IsNotNull(start);
                    Assert.AreEqual(outer.Width + 20, start.StartTextCursor.Width);
                }
                Assert.AreEqual(pg.Width - outer.Width - 40, line.FullWidth); //page - margins and outer float
                
            }
        }
    }
    
    
}
