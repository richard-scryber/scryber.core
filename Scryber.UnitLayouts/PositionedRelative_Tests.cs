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
using Scryber.Html.Components;
using Component = System.ComponentModel.Component;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Tests the positioning with relative units (e.g 50% or 4em)
    /// </summary>
    [TestClass()]
    public class PositionedRelative_Tests
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
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Positioning/Relative/" + name + ".html");
            path = System.IO.Path.GetFullPath(path);

            if (!System.IO.File.Exists(path))
                Assert.Inconclusive("The path the file " + name + " was not found at " + path);

            return path;
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_01_BlockPercentToPage()
        {

            var path = AssertGetContentFile("Relative_01_BlockPercentToPage");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_01_BlockPercentToPage.pdf"))
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
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(4, pg.ContentBlock.Columns[0].Contents.Count); //heading, span, relative, span
            var block = pg.ContentBlock.Columns[0].Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            
            
            //check the bounds.
            Unit yOffset = 50 + 15;
            Unit xOffset = 0;
            Unit width = pg.Width / 2.0;
            Unit height = pg.Height / 2.0;
            
            Assert.AreEqual(width, block.Width);
            Assert.AreEqual(height, block.Height);
            
            Assert.AreEqual(yOffset,  block.TotalBounds.Y);
            Assert.AreEqual(xOffset, block.TotalBounds.X);
            Assert.AreEqual(width, block.TotalBounds.Width);
            Assert.AreEqual(height, block.TotalBounds.Height);
            
            //check before and after
            var before = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(before);
            Assert.AreEqual(yOffset - 15, before.OffsetY);

            var after = pg.ContentBlock.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height, after.OffsetY);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_02_BlockPercentToPageMarginsPadding()
        {

            var path = AssertGetContentFile("Relative_02_BlockPercentToPageMarginsPadding");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_02_BlockPercentToPageMarginsPadding.pdf"))
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
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(4, pg.ContentBlock.Columns[0].Contents.Count); //heading, span, relative, span
            var block = pg.ContentBlock.Columns[0].Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            
            
            //check the bounds.
            Unit yOffset = 50 + 15;
            Unit xOffset = 0;
            Unit width = (pg.Width - 40 ) / 2.0;
            Unit height = (pg.Height - 40 ) / 2.0; 
            
            Assert.AreEqual(width + 40, block.Width);
            Assert.AreEqual(height + 40, block.Height);
            
            Assert.AreEqual(yOffset,  block.TotalBounds.Y);
            Assert.AreEqual(xOffset, block.TotalBounds.X);
            Assert.AreEqual(width + 40, block.TotalBounds.Width); //The block total bounds includes the margins
            Assert.AreEqual(height + 40, block.TotalBounds.Height);
            
            Assert.AreEqual(width - 20, block.Columns[0].TotalBounds.Width); //The inner region total bounds includes the padding.
            Assert.AreEqual(height - 20, block.Columns[0].TotalBounds.Height);
            
            //check before and after
            var before = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(before);
            Assert.AreEqual(yOffset - 15, before.OffsetY);

            var after = pg.ContentBlock.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height + 40, after.OffsetY);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_03_BlockPercentToPageMarginsPaddingTopLeft()
        {

            var path = AssertGetContentFile("Relative_03_BlockPercentToPageMarginsPaddingTopLeft");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_03_BlockPercentToPageMarginsPaddingTopLeft.pdf"))
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
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(4, pg.ContentBlock.Columns[0].Contents.Count); //heading, span, relative, span
            var block = pg.ContentBlock.Columns[0].Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            
            
            //check the bounds.
            Unit yOffset = 30 + 20 + 15; //heading, head margins, a line
            Unit xOffset = 0;
            Unit width = (pg.Width - 40) / 2.0;
            Unit height = (pg.Height - 40) / 2.0; 
            
            Assert.AreEqual(width + 10, block.Width); //includes the margins
            Assert.AreEqual(height + 10 , block.Height);
            
            Assert.AreEqual(yOffset,  block.TotalBounds.Y);
            Assert.AreEqual(xOffset, block.TotalBounds.X);
            Assert.AreEqual(width + 10 , block.TotalBounds.Width); //The block total bounds includes the margins
            Assert.AreEqual(height + 10 , block.TotalBounds.Height);
            
            Assert.AreEqual(width - 20, block.Columns[0].TotalBounds.Width); //The inner region total bounds includes the padding.
            Assert.AreEqual(height - 20, block.Columns[0].TotalBounds.Height);
            
            //check before and after
            var before = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(before);
            Assert.AreEqual(yOffset - 15, before.OffsetY);

            var after = pg.ContentBlock.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height + 10, after.OffsetY); //relative block margins

            //relative position is applied to the render bounds AFTER layout.
            //because it is just part of the layout as normal, then moved
            
            var arrange = ((Div)block.Owner).GetFirstArrangement();

            yOffset = 20 + 50 + 15 + 5 + 20; //top margin, heading + margins, line, rel margin, explicit top value
            xOffset = 20 + 5 + 10; //left page margin, rel margin, explcit left value
            Assert.AreEqual(yOffset , arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset  , arrange.RenderBounds.X);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            
            //check that the text has moved with the block (inc padding)
            Assert.AreEqual(1, block.Columns.Length);
            Assert.AreEqual(2, block.Columns[0].Contents.Count);
            
            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count); //start, chars, newline
            
            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(xOffset + 10, begin.StartTextCursor.Width);
            //vertical start text cursor should be offset down the padding + the baseline offset.
            Assert.AreEqual(yOffset + 10, begin.StartTextCursor.Height - begin.TextRenderOptions.GetBaselineOffset());
            
            //next line should be level with the previous, down 15 pt for the leading
            var newLine = line.Runs[2] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(0, newLine.NewLineOffset.Width);
            Assert.AreEqual(15, newLine.NewLineOffset.Height);
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_04_BlockPercentToPageMarginsPaddingBottomRight()
        {

            var path = AssertGetContentFile("Relative_04_BlockPercentToPageMarginsPaddingBottomRight");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_04_BlockPercentToPageMarginsPaddingBottomRight.pdf"))
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
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(4, pg.ContentBlock.Columns[0].Contents.Count); //heading, span, relative, span
            var block = pg.ContentBlock.Columns[0].Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            
            
            //check the bounds.
            Unit yOffset = 30 + 20 + 15; //heading, head margins, a line
            Unit xOffset = 0;
            Unit width = (pg.Width - 40) / 2.0;
            Unit height = (pg.Height - 40) / 2.0; 
            
            Assert.AreEqual(width + 10, block.Width); //includes the margins
            Assert.AreEqual(height + 10 , block.Height);
            
            Assert.AreEqual(yOffset,  block.TotalBounds.Y);
            Assert.AreEqual(xOffset, block.TotalBounds.X);
            Assert.AreEqual(width + 10 , block.TotalBounds.Width); //The block total bounds includes the margins
            Assert.AreEqual(height + 10 , block.TotalBounds.Height);
            
            Assert.AreEqual(width - 20, block.Columns[0].TotalBounds.Width); //The inner region total bounds includes the padding.
            Assert.AreEqual(height - 20, block.Columns[0].TotalBounds.Height);
            
            //check before and after
            var before = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(before);
            Assert.AreEqual(yOffset - 15, before.OffsetY);

            var after = pg.ContentBlock.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height + 10, after.OffsetY); //relative block margins

            //relative position is applied to the render bounds AFTER layout.
            //because it is just part of the layout as normal, then moved
            
            var arrange = ((Div)block.Owner).GetFirstArrangement();

            yOffset = 20 + 50 + 15 + 5 - 20; //top margin, heading + margin, line, rel margin, explicit bottom value
            xOffset = 20 + 5 - 10; //left page margin, rel margin, explcit right value
            Assert.AreEqual(yOffset , arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset  , arrange.RenderBounds.X);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            
            //check that the text has moved with the block (inc padding)
            Assert.AreEqual(1, block.Columns.Length);
            Assert.AreEqual(2, block.Columns[0].Contents.Count);
            
            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count); //start, chars, newline
            
            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(xOffset + 10, begin.StartTextCursor.Width);
            //vertical start text cursor should be offset down the padding + the baseline offset.
            Assert.AreEqual(yOffset + 10, begin.StartTextCursor.Height - begin.TextRenderOptions.GetBaselineOffset());
            
            //next line should be level with the previous, down 15 pt for the leading
            var newLine = line.Runs[2] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(0, newLine.NewLineOffset.Width);
            Assert.AreEqual(15, newLine.NewLineOffset.Height);
        }
        
        
          [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_05_BlockPercentToNesting()
        {

            var path = AssertGetContentFile("Relative_05_BlockPercentToNesting");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_05_BlockPercentToNesting.pdf"))
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
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(2, pg.ContentBlock.Columns[0].Contents.Count); //heading, nest

            var nest = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.Columns.Length);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            var block = nest.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            
            
            //check the bounds.
            Unit yOffset = 15; //50 + 15; now relative to the nesting block so just one line height
            Unit xOffset = 0;
            Unit width = pg.Width / 2.0;
            Unit height = (pg.Height - 50) / 2.0; //50% relative to the nest availalbe height.
            
            Assert.AreEqual(width, block.Width);
            Assert.AreEqual(height, block.Height);
            
            Assert.AreEqual(yOffset,  block.TotalBounds.Y);
            Assert.AreEqual(xOffset, block.TotalBounds.X);
            Assert.AreEqual(width, block.TotalBounds.Width);
            Assert.AreEqual(height, block.TotalBounds.Height);
            
            //check before and after
            var before = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(before);
            Assert.AreEqual(yOffset - 15, before.OffsetY);

            var after = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height, after.OffsetY);

            var comp = block.Owner as Div;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            yOffset = 50 + 15; //top margin, heading + margins, line, rel margin, explicit top value
            xOffset = 0; //left page margin, rel margin, explcit left value
            Assert.AreEqual(yOffset , arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset  , arrange.RenderBounds.X);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_06_BlockPercentToNestingMarginsPadding()
        {

            var path = AssertGetContentFile("Relative_06_BlockPercentToNestingMarginsPadding");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_06_BlockPercentToNestingMarginsPadding.pdf"))
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
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(2, pg.ContentBlock.Columns[0].Contents.Count); //heading,nesting

            var nest = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.Columns.Length);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            var rel = nest.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(rel);

            
            
            //check the bounds.
            Unit yOffset = 15; //body margin, h5 height and margins, nest v margin, line, rel margin 
            Unit xOffset = 0;
            
            Unit width = (pg.Width - 40 - 40 - 30  ) / 2.0; //50% of content space = page width - body margins (2x20), nest margins (2x20), nest padding (2x15)
            Unit height = (160 - 30 ) / 2.0; //50% of content space =  explicit height - nest padding (2x15)
            
            Assert.AreEqual(yOffset,  rel.TotalBounds.Y);
            Assert.AreEqual(xOffset, rel.TotalBounds.X);
            
            Assert.AreEqual(width + 20 , rel.Width); //take account of the rel margins
            Assert.AreEqual(height + 20, rel.Height);
            
            
            Assert.AreEqual(width + 20, rel.TotalBounds.Width); //The block total bounds includes the margins
            Assert.AreEqual(height + 20, rel.TotalBounds.Height); //The block total bounds includes the margins
            
            Assert.AreEqual(width - 20, rel.Columns[0].TotalBounds.Width); //The inner region total bounds includes the padding.
            Assert.AreEqual(height - 20, rel.Columns[0].TotalBounds.Height);
            
            //check before and after
            var before = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(before);
            Assert.AreEqual(yOffset - 15, before.OffsetY);

            var after = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height + 20, after.OffsetY); //10pt margins again
            
            //relative position is applied to the render bounds AFTER layout.
            //because it is just part of the layout as normal, then moved
            
            var arrange = ((Div)rel.Owner).GetFirstArrangement();

            yOffset = 20 + 50 + (10 + 15) + 15 + 10; //top margin, heading + margins, nest margin and padding, line, rel margin
            xOffset = 20 + 20 + 15 + 10; //left page margin, nest margin, nest padding, rel margin
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset , arrange.RenderBounds.X);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            
            //check that the text has moved with the block (inc padding)
            Assert.AreEqual(1, rel.Columns.Length);
            Assert.AreEqual(2, rel.Columns[0].Contents.Count);
            
            var line = rel.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count); //start, chars, newline
            
            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(xOffset + 10, begin.StartTextCursor.Width);
            //vertical start text cursor should be offset down the padding + the baseline offset.
            Assert.AreEqual(yOffset + 10, begin.StartTextCursor.Height - begin.TextRenderOptions.GetBaselineOffset());
            
            //next line should be level with the previous, down 15 pt for the leading
            var newLine = line.Runs[2] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(0, newLine.NewLineOffset.Width);
            Assert.AreEqual(15, newLine.NewLineOffset.Height);
            
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_07_BlockPercentToNestingMarginsPaddingTopLeft()
        {

            var path = AssertGetContentFile("Relative_07_BlockPercentToNestingMarginsPaddingTopLeft");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_07_BlockPercentToNestingMarginsPaddingTopLeft.pdf"))
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
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(2, pg.ContentBlock.Columns[0].Contents.Count); //heading,nesting

            var nest = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.Columns.Length);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            var rel = nest.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(rel);

            
            
            //check the bounds.
            Unit yOffset = 15; //body margin, h5 height and margins, nest v margin, line, rel margin 
            Unit xOffset = 0;
            
            Unit width = (pg.Width - 40 - 40 - 30  ) / 2.0; //50% of content space = page width - body margins (2x20), nest margins (2x20), nest padding (2x15)
            Unit height = (160 - 30 ) / 2.0; //50% of content space =  explicit height - nest padding (2x15)
            
            Assert.AreEqual(yOffset,  rel.TotalBounds.Y);
            Assert.AreEqual(xOffset, rel.TotalBounds.X);
            
            Assert.AreEqual(width + 20 , rel.Width); //take account of the rel margins
            Assert.AreEqual(height + 20, rel.Height);
            
            
            Assert.AreEqual(width + 20, rel.TotalBounds.Width); //The block total bounds includes the margins
            Assert.AreEqual(height + 20, rel.TotalBounds.Height); //The block total bounds includes the margins
            
            Assert.AreEqual(width - 20, rel.Columns[0].TotalBounds.Width); //The inner region total bounds includes the padding.
            Assert.AreEqual(height - 20, rel.Columns[0].TotalBounds.Height);
            
            //check before and after
            var before = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(before);
            Assert.AreEqual(yOffset - 15, before.OffsetY);

            var after = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height + 20, after.OffsetY); //10pt margins again
            
            //relative position is applied to the render bounds AFTER layout.
            //because it is just part of the layout as normal, then moved
            
            var arrange = ((Div)rel.Owner).GetFirstArrangement();

            yOffset = 20 + 50 + (10 + 15) + 15 + 10; //top margin, heading + margins, nest margin and padding, line, rel margin
            xOffset = 20 + 20 + 15 + 10; //left page margin, nest margin, nest padding, rel margin
            Assert.AreEqual(yOffset + 60, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset + 30, arrange.RenderBounds.X);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            
            //check that the text has moved with the block (inc padding)
            Assert.AreEqual(1, rel.Columns.Length);
            Assert.AreEqual(2, rel.Columns[0].Contents.Count);
            
            var line = rel.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count); //start, chars, newline
            
            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(xOffset + 10 + 30, begin.StartTextCursor.Width);
            //vertical start text cursor should be offset down the padding + the baseline offset.
            Assert.AreEqual(yOffset + 10 + 60, begin.StartTextCursor.Height - begin.TextRenderOptions.GetBaselineOffset());
            
            //next line should be level with the previous, down 15 pt for the leading
            var newLine = line.Runs[2] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(0, newLine.NewLineOffset.Width);
            Assert.AreEqual(15, newLine.NewLineOffset.Height);
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_08_BlockPercentToNestingMarginsPaddingBottomRight()
        {

            var path = AssertGetContentFile("Relative_08_BlockPercentToNestingMarginsPaddingBottomRight");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_08_BlockPercentToNestingMarginsPaddingBottomRight.pdf"))
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
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(2, pg.ContentBlock.Columns[0].Contents.Count); //heading,nesting

            var nest = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.Columns.Length);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            var rel = nest.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(rel);

            
            
            //check the bounds.
            Unit yOffset = 15; //body margin, h5 height and margins, nest v margin, line, rel margin 
            Unit xOffset = 0;
            
            Unit width = (pg.Width - 40 - 40 - 30  ) / 2.0; //50% of content space = page width - body margins (2x20), nest margins (2x20), nest padding (2x15)
            Unit height = (160 - 30 ) / 2.0; //50% of content space =  explicit height - nest padding (2x15)
            
            Assert.AreEqual(yOffset,  rel.TotalBounds.Y);
            Assert.AreEqual(xOffset, rel.TotalBounds.X);
            
            Assert.AreEqual(width + 20 , rel.Width); //take account of the rel margins
            Assert.AreEqual(height + 20, rel.Height);
            
            
            Assert.AreEqual(width + 20, rel.TotalBounds.Width); //The block total bounds includes the margins
            Assert.AreEqual(height + 20, rel.TotalBounds.Height); //The block total bounds includes the margins
            
            Assert.AreEqual(width - 20, rel.Columns[0].TotalBounds.Width); //The inner region total bounds includes the padding.
            Assert.AreEqual(height - 20, rel.Columns[0].TotalBounds.Height);
            
            //check before and after
            var before = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(before);
            Assert.AreEqual(yOffset - 15, before.OffsetY);

            var after = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height + 20, after.OffsetY); //10pt margins again
            
            //relative position is applied to the render bounds AFTER layout.
            //because it is just part of the layout as normal, then moved
            
            var arrange = ((Div)rel.Owner).GetFirstArrangement();

            yOffset = 20 + 50 + (10 + 15) + 15 + 10; //top margin, heading + margins, nest margin and padding, line, rel margin
            xOffset = 20 + 20 + 15 + 10; //left page margin, nest margin, nest padding, rel margin
            Assert.AreEqual(yOffset - 60, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset - 30, arrange.RenderBounds.X);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            
            //check that the text has moved with the block (inc padding)
            Assert.AreEqual(1, rel.Columns.Length);
            Assert.AreEqual(2, rel.Columns[0].Contents.Count);
            
            var line = rel.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count); //start, chars, newline
            
            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            Assert.AreEqual(xOffset + 10 - 30, begin.StartTextCursor.Width);
            //vertical start text cursor should be offset down the padding + the baseline offset.
            Assert.AreEqual(yOffset + 10 - 60, begin.StartTextCursor.Height - begin.TextRenderOptions.GetBaselineOffset());
            
            //next line should be level with the previous, down 15 pt for the leading
            var newLine = line.Runs[2] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(0, newLine.NewLineOffset.Width);
            Assert.AreEqual(15, newLine.NewLineOffset.Height);
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_09_BlockPercentToPage2ndColumn()
        {

            var path = AssertGetContentFile("Relative_09_BlockPercentToPage2ndColumn");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_09_BlockPercentToPage2ndColumn.pdf"))
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
            Assert.AreEqual(2, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count); //heading only
            Assert.AreEqual(3, pg.ContentBlock.Columns[1].Contents.Count);
            var block = pg.ContentBlock.Columns[1].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            
            
            //check the bounds.
            Unit yOffset = 15; //back to the top with 1 line above
            Unit xOffset = 0;
            Unit width = pg.ContentBlock.Columns[1].TotalBounds.Width / 2.0;
            Unit height = pg.Height / 2.0;
            
            Assert.AreEqual(width, block.Width);
            Assert.AreEqual(height, block.Height);
            
            Assert.AreEqual(yOffset,  block.TotalBounds.Y);
            Assert.AreEqual(xOffset, block.TotalBounds.X);
            Assert.AreEqual(width, block.TotalBounds.Width);
            Assert.AreEqual(height, block.TotalBounds.Height);
            
            //check before and after
            var before = pg.ContentBlock.Columns[1].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(before);
            Assert.AreEqual(yOffset - 15, before.OffsetY);

            var after = pg.ContentBlock.Columns[1].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height, after.OffsetY);
            
            

            var arrange = (block.Owner as Scryber.Components.Component).GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(pg.ContentBlock.Columns[1].OffsetX + xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_10_BlockPercentToPage2ndColumnMarginsPadding()
        {

            var path = AssertGetContentFile("Relative_10_BlockPercentToPage2ndColumnMarginsPadding");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_10_BlockPercentToPage2ndColumnMarginsPadding.pdf"))
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
            Assert.AreEqual(2, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count); //heading only
            Assert.AreEqual(3, pg.ContentBlock.Columns[1].Contents.Count);
            var block = pg.ContentBlock.Columns[1].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            
            
            //check the bounds.
            
            
            Unit yOffset = 15; //back to the top with 1 line above
            Unit xOffset = 0;
            
            Unit width = (pg.Width - 40 - 10) / 2.0; //one column width
            width = width / 2; //50%
            Unit height = (pg.Height - 40) / 4.0; //25%
            
            Assert.AreEqual(width + 40, block.Width); //inc margins
            Assert.AreEqual(height + 40 , block.Height); // inc margins
            
            Assert.AreEqual(yOffset,  block.TotalBounds.Y);
            Assert.AreEqual(xOffset, block.TotalBounds.X);
            Assert.AreEqual(width + 40, block.TotalBounds.Width); //inc margins
            Assert.AreEqual(height + 40, block.TotalBounds.Height); //inc margins
            
            //check before and after
            var before = pg.ContentBlock.Columns[1].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(before);
            Assert.AreEqual(yOffset - 15, before.OffsetY);

            var after = pg.ContentBlock.Columns[1].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height + 40, after.OffsetY); //inc margins

            var comp = (block.Owner as Scryber.Components.Component);
            Assert.IsNotNull(comp);
            
            var arrange = comp.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            yOffset += 20 + 20; //body margins + rel margins
            xOffset = pg.ContentBlock.Columns[1].OffsetX + 20 + 20; //column xOffset + body margins + rel margins
            
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); 
            Assert.AreEqual(xOffset, arrange.RenderBounds.X); //body margins + rel margins
                        

            Assert.AreEqual(height, arrange.RenderBounds.Height); //exl margins
            Assert.AreEqual(width, arrange.RenderBounds.Width); //exl margins
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_11_BlockPercentToPage2ndColumnMarginsPaddingTopLeft()
        {

            var path = AssertGetContentFile("Relative_11_BlockPercentToPage2ndColumnMarginsPaddingTopLeft");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_11_BlockPercentToPage2ndColumnMarginsPaddingTopLeft.pdf"))
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
            Assert.AreEqual(2, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count); //heading only
            Assert.AreEqual(3, pg.ContentBlock.Columns[1].Contents.Count);
            var block = pg.ContentBlock.Columns[1].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            
            
            //check the bounds.
            
            Unit yOffset = 15; //back to the top with 1 line above
            Unit xOffset = 0;
            
            Unit width = (pg.Width - 40 - 10) / 2.0; //one column width
            width = width / 2; //50%
            Unit height = (pg.Height - 40) / 4.0; //25%
            
            Assert.AreEqual(width + 40, block.Width); //inc margins
            Assert.AreEqual(height + 40 , block.Height); // inc margins
            
            Assert.AreEqual(yOffset,  block.TotalBounds.Y);
            Assert.AreEqual(xOffset, block.TotalBounds.X);
            Assert.AreEqual(width + 40, block.TotalBounds.Width); //inc margins
            Assert.AreEqual(height + 40, block.TotalBounds.Height); //inc margins
            
            //check before and after
            var before = pg.ContentBlock.Columns[1].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(before);
            Assert.AreEqual(yOffset - 15, before.OffsetY);

            var after = pg.ContentBlock.Columns[1].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height + 40, after.OffsetY); //inc margins

            var comp = (block.Owner as Scryber.Components.Component);
            Assert.IsNotNull(comp);
            
            var arrange = comp.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            yOffset += 20 + 20; //body margins + rel margins
            xOffset = pg.ContentBlock.Columns[1].OffsetX + 20 + 20; //column xOffset + body margins + rel margins

            yOffset += 20; //explicit position applied at render time
            xOffset += 10; //explicit position applied at render time
            
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); 
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);

            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_12_BlockPercentToPage2ndColumnOverflow()
        {

            var path = AssertGetContentFile("Relative_12_BlockPercentToPage2ndColumnOverflow");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_12_BlockPercentToPage2ndColumnOveflow.pdf"))
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
            Assert.AreEqual(2, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(2, pg.ContentBlock.Columns[0].Contents.Count); //heading only
            Assert.AreEqual(2, pg.ContentBlock.Columns[1].Contents.Count);
            var block = pg.ContentBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            
            
            //check the bounds.
            
            Unit yOffset = 0; //back to the top 
            Unit xOffset = 0;
            
            Unit width = (pg.Width - 40 - 10) / 2.0; //one column width
            width = width / 2; //50%
            Unit height = (pg.Height - 40) / 4.0; //25%
            
            Assert.AreEqual(width + 40, block.Width); //inc margins
            Assert.AreEqual(height + 40 , block.Height); // inc margins
            
            Assert.AreEqual(yOffset,  block.TotalBounds.Y);
            Assert.AreEqual(xOffset, block.TotalBounds.X);
            Assert.AreEqual(width + 40, block.TotalBounds.Width); //inc margins
            Assert.AreEqual(height + 40, block.TotalBounds.Height); //inc margins
            
            //check the after (no before)
            //var before = pg.ContentBlock.Columns[1].Contents[0] as PDFLayoutLine;
            //Assert.IsNotNull(before);
            //Assert.AreEqual(yOffset - 15, before.OffsetY);

            var after = pg.ContentBlock.Columns[1].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height + 40, after.OffsetY); //inc margins

            var comp = (block.Owner as Scryber.Components.Component);
            Assert.IsNotNull(comp);
            
            var arrange = comp.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            yOffset += 20 + 20; //body margins + rel margins
            xOffset = pg.ContentBlock.Columns[1].OffsetX + 20 + 20; //column xOffset + body margins + rel margins

            
            
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); 
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);

            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
        }

        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_13_BlockPercentToPage2ndColumnOverflowNaturalSize()
        {

            var path = AssertGetContentFile("Relative_13_BlockPercentToPage2ndColumnOverflowNaturalSize");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_13_BlockPercentToPage2ndColumnOverflowNaturalSize.pdf"))
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
            Assert.AreEqual(2, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(2, pg.ContentBlock.Columns[0].Contents.Count); //heading only
            Assert.AreEqual(2, pg.ContentBlock.Columns[1].Contents.Count);
            var block = pg.ContentBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            
            
            //check the bounds.
            Unit yOffset = 0; //back to the top
            Unit xOffset = 0;
            Unit width = pg.ContentBlock.Columns[1].TotalBounds.Width / 2.0;
            Unit height = (11 * 15) + 20; //11 lines of text + padding
            
            Assert.AreEqual(width + 40, block.Width); // inc margins
            Assert.AreEqual(height + 40, block.Height); //inc margins
            
            Assert.AreEqual(yOffset,  block.TotalBounds.Y);
            Assert.AreEqual(xOffset, block.TotalBounds.X);
            Assert.AreEqual(width + 40, block.TotalBounds.Width);
            Assert.AreEqual(height + 40, block.TotalBounds.Height);
            
            //check after
            

            var after = pg.ContentBlock.Columns[1].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height + 40, after.OffsetY);
            
            var comp = (block.Owner as Scryber.Components.Component);
            Assert.IsNotNull(comp);
            
            var arrange = comp.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            yOffset += 20 + 20; //body margins + rel margins
            xOffset = pg.ContentBlock.Columns[1].OffsetX + 20 + 20; //column xOffset + body margins + rel margins
            
            
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); 
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);

            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_14_BlockPercentToPage2ndColumnOverflowTopLeft()
        {

            var path = AssertGetContentFile("Relative_14_BlockPercentToPage2ndColumnOverflowTopLeft");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_14_BlockPercentToPage2ndColumnOverflowTopLeft.pdf"))
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
            Assert.AreEqual(2, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(2, pg.ContentBlock.Columns[0].Contents.Count); //heading only
            Assert.AreEqual(2, pg.ContentBlock.Columns[1].Contents.Count);
            var block = pg.ContentBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            
            
            //check the bounds.
            Unit yOffset = 0; //back to the top
            Unit xOffset = 0;
            Unit width = pg.ContentBlock.Columns[1].TotalBounds.Width / 2.0;
            Unit height = (11 * 15) + 20; //11 lines of text + padding
            
            Assert.AreEqual(width + 40, block.Width); // inc margins
            Assert.AreEqual(height + 40, block.Height); //inc margins
            
            Assert.AreEqual(yOffset,  block.TotalBounds.Y);
            Assert.AreEqual(xOffset, block.TotalBounds.X);
            Assert.AreEqual(width + 40, block.TotalBounds.Width);
            Assert.AreEqual(height + 40, block.TotalBounds.Height);
            
            //check after
            

            var after = pg.ContentBlock.Columns[1].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height + 40, after.OffsetY);
            
            var comp = (block.Owner as Scryber.Components.Component);
            Assert.IsNotNull(comp);
            
            var arrange = comp.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            yOffset += 20 + 20; //body margins + rel margins
            xOffset = pg.ContentBlock.Columns[1].OffsetX + 20 + 20; //column xOffset + body margins + rel margins

            yOffset -= 30; //explicit
            xOffset -= 10; //explicit
            
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); 
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);

            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_15_BlockPercentToPage2ndColumnSplitTopLeft()
        {

            var path = AssertGetContentFile("Relative_15_BlockPercentToPage2ndColumnSplitTopLeft");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_15_BlockPercentToPage2ndColumnSplitTopLeft.pdf"))
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
            Assert.AreEqual(2, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count); //heading spacer and half text
            Assert.AreEqual(2, pg.ContentBlock.Columns[1].Contents.Count); //other half text and after
            
            // first half
            
            var block = pg.ContentBlock.Columns[0].Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            
            
            //check the bounds.
            Unit yOffset = 80 + 600; //heading + spacer height
            Unit xOffset = 0;
            Unit width = pg.ContentBlock.Columns[1].TotalBounds.Width / 2.0;
            Unit height = (4 * 15) + 20; //4 lines of text + padding
            
            Assert.AreEqual(width + 40, block.Width); // inc margins
            Assert.AreEqual(height + 40, block.Height); //inc margins
            
            Assert.AreEqual(yOffset,  block.TotalBounds.Y);
            Assert.AreEqual(xOffset, block.TotalBounds.X);
            Assert.AreEqual(width + 40, block.TotalBounds.Width);
            Assert.AreEqual(height + 40, block.TotalBounds.Height);
            
            //check after
            
            
            var comp = (block.Owner as Scryber.Components.Component);
            Assert.IsNotNull(comp);
            
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            yOffset += 20 + 20; //body margins + rel margins
            xOffset += 20 + 20; //body margins + rel margins

            yOffset -= 30; //explicit
            xOffset -= 10; //explicit
            
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); 
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);

            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            // second half
            
            block = pg.ContentBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            
            //check the bounds.
            yOffset = 0; //back to the top
            xOffset = 0;
            width = pg.ContentBlock.Columns[1].TotalBounds.Width / 2.0;
            height = (8 * 15) + 20; //8 lines of text + padding
            
            Assert.AreEqual(width + 40, block.Width); // inc margins
            Assert.AreEqual(height + 40, block.Height); //inc margins
            
            Assert.AreEqual(yOffset,  block.TotalBounds.Y);
            Assert.AreEqual(xOffset, block.TotalBounds.X);
            Assert.AreEqual(width + 40, block.TotalBounds.Width);
            Assert.AreEqual(height + 40, block.TotalBounds.Height);
            
            //check after
            

            var after = pg.ContentBlock.Columns[1].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height + 40, after.OffsetY);
            
            comp = (block.Owner as Scryber.Components.Component);
            Assert.IsNotNull(comp);
            
            //Move to the next block arrangement
            arrange = arrange.NextArrangement;
            Assert.IsNotNull(arrange);

            yOffset += 20 + 20; //body margins + rel margins
            xOffset = pg.ContentBlock.Columns[1].OffsetX + 20 + 20; //column xOffset + body margins + rel margins

            yOffset -= 30; //explicit
            xOffset -= 10; //explicit
            
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); 
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);

            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
        }
    }
}
