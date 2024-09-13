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
    public class PositionedInlineBlock_Tests
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
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Positioning/InlineBlock/" + name + ".html");
            path = System.IO.Path.GetFullPath(path);

            if (!System.IO.File.Exists(path))
                Assert.Inconclusive("The path the file " + name + " was not found at " + path);

            return path;
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_01_ExplicitWidth()
        {
            var path = AssertGetContentFile("InlineBlock_01_ExplicitWidth");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_01_ExplicitWidth.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);
            

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);

            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //baseline for inline block by default
            var baseline = (15 * 3) + 10 + 10;
            Assert.AreEqual(baseline, line.BaseLineOffset);
            
            // text begin, text chars, text end, img component run, whitespace begin, whitespace chars, whitespace end = 7
            // inline positioned block, whitespace begin, whitespace chars, whitespace end, inline start, text start, text chars, new line = 8
            Assert.AreEqual(15, line.Runs.Count);
            
            var chars = line.Runs[1] as PDFTextRunCharacter;
            var img = line.Runs[3] as PDFLayoutComponentRun;
            var ws = line.Runs[5] as PDFTextRunCharacter;
            var posRun = line.Runs[7] as PDFLayoutPositionedRegionRun;
            
            Assert.IsNotNull(chars);
            Assert.IsNotNull(img);
            Assert.IsNotNull(ws);
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(chars.Width + ws.Width + img.Width, posReg.TotalBounds.X);//first literal width
            Assert.AreEqual(0, posReg.TotalBounds.Y); 

            Assert.AreEqual(120, posReg.TotalBounds.Width);
            Assert.AreEqual(45 + 20, posReg.TotalBounds.Height); //3 lines + 10pt padding
            Assert.AreEqual(1, posReg.Contents.Count);


            
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(45 + 20, posBlock.Height);
            Assert.AreEqual(120, posBlock.Width);
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(3, posBlock.Columns[0].Contents.Count);

            var newLine = line.Runs[14] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(30 * 1.2, newLine.NewLineOffset.Height); //left align
            //second line

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            //spacer, chars, new line
            Assert.AreEqual(3, line.Runs.Count);
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("inline block, flowing onto a new line with", chars2.Characters);

            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds;
            var xOffset = 20 + chars.Width + ws.Width + img.Width; // page margins, text and image width
            var yOffset = 20 + 20 + 30; //page margins, h5 margins, line height
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(120, bounds.Width);
            Assert.AreEqual(45 + 20, bounds.Height);
            

        }

        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_02_ExplicitWidthExplicitBaseline()
        {
            var path = AssertGetContentFile("InlineBlock_02_ExplicitWidthExplicitBaseline");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_02_ExplicitWidthExplicitBaseline.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);
            

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);

            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //baseline for inline block by default
            var baseline = (15 * 3) + 10 + 10;
            Assert.AreEqual(baseline, line.BaseLineOffset);
            
            // text begin, text chars, text end, img component run, whitespace begin, whitespace chars, whitespace end = 7
            // inline positioned block, whitespace begin, whitespace chars, whitespace end, inline start, text start, text chars, new line = 8
            Assert.AreEqual(15, line.Runs.Count);
            
            var chars = line.Runs[1] as PDFTextRunCharacter;
            var img = line.Runs[3] as PDFLayoutComponentRun;
            var ws = line.Runs[5] as PDFTextRunCharacter;
            var posRun = line.Runs[7] as PDFLayoutPositionedRegionRun;
            
            Assert.IsNotNull(chars);
            Assert.IsNotNull(img);
            Assert.IsNotNull(ws);
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(chars.Width + ws.Width + img.Width, posReg.TotalBounds.X);//first literal width
            Assert.AreEqual(0, posReg.TotalBounds.Y); 

            Assert.AreEqual(120, posReg.TotalBounds.Width);
            Assert.AreEqual(45 + 20, posReg.TotalBounds.Height); //3 lines + 10pt padding
            Assert.AreEqual(1, posReg.Contents.Count);


            
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(45 + 20, posBlock.Height);
            Assert.AreEqual(120, posBlock.Width);
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(3, posBlock.Columns[0].Contents.Count);

            var newLine = line.Runs[14] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(30 * 1.2, newLine.NewLineOffset.Height); //left align
            //second line

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            //spacer, chars, new line
            Assert.AreEqual(3, line.Runs.Count);
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("inline block, flowing onto a new line with", chars2.Characters);

            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds;
            var xOffset = 20 + chars.Width + ws.Width + img.Width; // page margins, text and image width
            var yOffset = 20 + 20 + 30; //page margins, h5 margins, line height
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(120, bounds.Width);
            Assert.AreEqual(45 + 20, bounds.Height);
            

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_03_ExplicitWidthBottom()
        {
            var path = AssertGetContentFile("InlineBlock_03_ExplicitWidthBottom");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_03_ExplicitWidthBottom.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);
            

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);

            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //bottom for inline block by default
            var bottom = (15 * 3) + 10 + 10;
            bottom += 3; // leading with font 12 and height 15
            Assert.AreEqual(bottom, line.Height);
            
            // text begin, text chars, text end, img component run, whitespace begin, whitespace chars, whitespace end = 7
            // inline positioned block, whitespace begin, whitespace chars, whitespace end, inline start, text start, text chars, new line = 8
            Assert.AreEqual(15, line.Runs.Count);
            
            var chars = line.Runs[1] as PDFTextRunCharacter;
            var img = line.Runs[3] as PDFLayoutComponentRun;
            var ws = line.Runs[5] as PDFTextRunCharacter;
            var posRun = line.Runs[7] as PDFLayoutPositionedRegionRun;
            
            Assert.IsNotNull(chars);
            Assert.IsNotNull(img);
            Assert.IsNotNull(ws);
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(chars.Width + ws.Width + img.Width, posReg.TotalBounds.X);//first literal width
            Assert.AreEqual(0, posReg.TotalBounds.Y); 

            Assert.AreEqual(120, posReg.TotalBounds.Width);
            Assert.AreEqual(45 + 20, posReg.TotalBounds.Height); //3 lines + 10pt padding
            Assert.AreEqual(1, posReg.Contents.Count);


            
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(45 + 20, posBlock.Height);
            Assert.AreEqual(120, posBlock.Width);
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(3, posBlock.Columns[0].Contents.Count);

            var newLine = line.Runs[14] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(30 * 1.2, newLine.NewLineOffset.Height); //left align
            //second line

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            //spacer, chars, new line
            Assert.AreEqual(3, line.Runs.Count);
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("inline block, flowing onto a new line with", chars2.Characters);

            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds;
            var xOffset = 20 + chars.Width + ws.Width + img.Width; // page margins, text and image width
            var yOffset = 20 + 20 + 30; //page margins, h5 margins, line height
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(120, bounds.Width);
            Assert.AreEqual(45 + 20, bounds.Height);
            

        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_04_ExplicitWidthTop()
        {
            var path = AssertGetContentFile("InlineBlock_04_ExplicitWidthTop");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_04_ExplicitWidthTop.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);
            

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);

            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //bottom for inline block by default
            var bottom = (15 * 3) + 10 + 10;
           
            Assert.AreEqual(bottom, line.Height);
            
            // text begin, text chars, text end, img component run, whitespace begin, whitespace chars, whitespace end = 7
            // inline positioned block, whitespace begin, whitespace chars, whitespace end, inline start, text start, text chars, new line = 8
            Assert.AreEqual(15, line.Runs.Count);
            
            var chars = line.Runs[1] as PDFTextRunCharacter;
            var img = line.Runs[3] as PDFLayoutComponentRun;
            var ws = line.Runs[5] as PDFTextRunCharacter;
            var posRun = line.Runs[7] as PDFLayoutPositionedRegionRun;
            
            Assert.IsNotNull(chars);
            Assert.IsNotNull(img);
            Assert.IsNotNull(ws);
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(chars.Width + ws.Width + img.Width, posReg.TotalBounds.X);//first literal width
            Assert.AreEqual(0, posReg.TotalBounds.Y); 

            Assert.AreEqual(120, posReg.TotalBounds.Width);
            Assert.AreEqual(45 + 20, posReg.TotalBounds.Height); //3 lines + 10pt padding
            Assert.AreEqual(1, posReg.Contents.Count);


            
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(45 + 20, posBlock.Height);
            Assert.AreEqual(120, posBlock.Width);
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(3, posBlock.Columns[0].Contents.Count);

            var newLine = line.Runs[14] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(45 + 20, newLine.NewLineOffset.Height); //top align is the full height
            //second line

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            //spacer, chars, new line
            Assert.AreEqual(3, line.Runs.Count);
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("inline block, flowing onto a new line with", chars2.Characters);

            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds;
            var xOffset = 20 + chars.Width + ws.Width + img.Width; // page margins, text and image width
            var yOffset = 20 + 20 + 30; //page margins, h5 margins, line height
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(120, bounds.Width);
            Assert.AreEqual(45 + 20, bounds.Height);
            

        }
        
         [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_05_ExplicitWidthMiddle()
        {
            var path = AssertGetContentFile("InlineBlock_05_ExplicitWidthMiddle");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_05_ExplicitWidthMiddle.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);
            

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);

            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //bottom for inline block by default
            var bottom = (15 * 3) + 10 + 10;
           
            Assert.AreEqual(bottom, line.Height);
            
            // text begin, text chars, text end, img component run, whitespace begin, whitespace chars, whitespace end = 7
            // inline positioned block, whitespace begin, whitespace chars, whitespace end, inline start, text start, text chars, new line = 8
            Assert.AreEqual(15, line.Runs.Count);
            
            var chars = line.Runs[1] as PDFTextRunCharacter;
            var img = line.Runs[3] as PDFLayoutComponentRun;
            var ws = line.Runs[5] as PDFTextRunCharacter;
            var posRun = line.Runs[7] as PDFLayoutPositionedRegionRun;
            
            Assert.IsNotNull(chars);
            Assert.IsNotNull(img);
            Assert.IsNotNull(ws);
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(chars.Width + ws.Width + img.Width, posReg.TotalBounds.X);//first literal width
            Assert.AreEqual(0, posReg.TotalBounds.Y); 

            Assert.AreEqual(120, posReg.TotalBounds.Width);
            Assert.AreEqual(45 + 20, posReg.TotalBounds.Height); //3 lines + 10pt padding
            Assert.AreEqual(1, posReg.Contents.Count);


            
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(45 + 20, posBlock.Height);
            Assert.AreEqual(120, posBlock.Width);
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(3, posBlock.Columns[0].Contents.Count);

            var newLine = line.Runs[14] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            
            
            //second line

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            
            //spacer, chars, new line
            Assert.AreEqual(3, line.Runs.Count);
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("inline block, flowing onto a new line with", chars2.Characters);

            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds;
            var xOffset = 20 + chars.Width + ws.Width + img.Width; // page margins, text and image width
            var yOffset = 20 + 20 + 30; //page margins, h5 margins, line height
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(120, bounds.Width);
            Assert.AreEqual(45 + 20, bounds.Height);
            

        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_06_SmallBaseline()
        {
            var path = AssertGetContentFile("InlineBlock_06_SmallBaseline");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_06_SmallBaseline.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);
            

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);

            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //baseline for inline block by default
            var baseline = line.BaseLineOffset; //doesn't affect the baseline - checked below for offset
            
            // text begin, text chars, text end, img component run, whitespace begin, whitespace chars, whitespace end = 7
            // inline positioned block, whitespace begin, whitespace chars, whitespace end, inline start, text start, text chars, new line = 8
            Assert.AreEqual(15, line.Runs.Count);
            
            var chars = line.Runs[1] as PDFTextRunCharacter;
            var img = line.Runs[3] as PDFLayoutComponentRun;
            var ws = line.Runs[5] as PDFTextRunCharacter;
            var posRun = line.Runs[7] as PDFLayoutPositionedRegionRun;
            
            Assert.IsNotNull(chars);
            Assert.IsNotNull(img);
            Assert.IsNotNull(ws);
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(chars.Width + ws.Width + img.Width, posReg.TotalBounds.X);//first literal width
            Assert.AreEqual(0, posReg.TotalBounds.Y); 

            //inner line size
            Assert.AreEqual(1, posReg.Contents.Count);
            var innerBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);
            var innerChars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(innerChars);

            var innerWidth = innerChars.Width + 5 + 5; //chars + padding
            var innerHeight = innerLine.Height + 5 + 5;
            
            Assert.AreEqual(innerWidth, posReg.TotalBounds.Width);
            Assert.AreEqual(innerHeight, posReg.TotalBounds.Height); //3 lines + 10pt padding
            


            
            
            Assert.AreEqual(0, innerBlock.OffsetX);
            Assert.AreEqual(0, innerBlock.OffsetY);
            Assert.AreEqual(innerHeight, innerBlock.Height);
            Assert.AreEqual(innerWidth, innerBlock.Width);
            

            var newLine = line.Runs[14] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(30 * 1.2, newLine.NewLineOffset.Height); //left align
            //second line

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            //spacer, chars, new line
            Assert.AreEqual(3, line.Runs.Count);
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("block, flowing onto a new line with the", chars2.Characters);

            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds; //relative to the page
            var xOffset = 20 + chars.Width + ws.Width + img.Width; // page margins, text and image width
            var yOffset = 30 + 20 + 20 + baseline - innerHeight; //from the baseline + h5 & margins + page margins - up by the height
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(innerWidth, bounds.Width);
            Assert.AreEqual(innerHeight, bounds.Height);
            
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_07_SmallTop()
        {
            var path = AssertGetContentFile("InlineBlock_07_SmallTop");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_07_SmallTop.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);
            

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);

            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //baseline for inline block by default
            var baseline = line.BaseLineOffset; //doesn't affect the baseline - checked below for offset
            
            // text begin, text chars, text end, img component run, whitespace begin, whitespace chars, whitespace end = 7
            // inline positioned block, whitespace begin, whitespace chars, whitespace end, inline start, text start, text chars, new line = 8
            Assert.AreEqual(15, line.Runs.Count);
            
            var chars = line.Runs[1] as PDFTextRunCharacter;
            var img = line.Runs[3] as PDFLayoutComponentRun;
            var ws = line.Runs[5] as PDFTextRunCharacter;
            var posRun = line.Runs[7] as PDFLayoutPositionedRegionRun;
            
            Assert.IsNotNull(chars);
            Assert.IsNotNull(img);
            Assert.IsNotNull(ws);
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(chars.Width + ws.Width + img.Width, posReg.TotalBounds.X);//first literal width
            Assert.AreEqual(0, posReg.TotalBounds.Y); 

            //inner line size
            Assert.AreEqual(1, posReg.Contents.Count);
            var innerBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);
            var innerChars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(innerChars);

            var innerWidth = innerChars.Width + 5 + 5 + 20; //chars + padding + margins
            var innerHeight = innerLine.Height + 5 + 5 + 20;//chars + padding + margins
            
            Assert.AreEqual(innerWidth, posReg.TotalBounds.Width);
            Assert.AreEqual(innerHeight, posReg.TotalBounds.Height);
            


            
            
            Assert.AreEqual(0, innerBlock.OffsetX);
            Assert.AreEqual(0, innerBlock.OffsetY);
            Assert.AreEqual(innerHeight, innerBlock.Height);
            Assert.AreEqual(innerWidth, innerBlock.Width);
            

            var newLine = line.Runs[14] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(30 * 1.2, newLine.NewLineOffset.Height); //left align
            //second line

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            //spacer, chars, new line
            Assert.AreEqual(3, line.Runs.Count);
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("inline block, flowing onto a new line with", chars2.Characters);

            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds; //relative to the page
            var xOffset = 20 + chars.Width + ws.Width + img.Width + 10; // page margins, text, image width and left margin
            var yOffset = 30 + 20 + 20 + 10; //top so - h5 & margins + page margins and top margin
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(innerWidth - 20, bounds.Width); //render bounds without margins
            Assert.AreEqual(innerHeight - 20, bounds.Height); //render bounds without margins
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_08_SmallBottom()
        {
            var path = AssertGetContentFile("InlineBlock_08_SmallBottom");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_08_SmallBottom.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);
            

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);

            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //baseline for inline block by default
            var baselineToBottom = line.BaseLineToBottom; //doesn't affect the baseline - checked below for offset
            
            // text begin, text chars, text end, img component run, whitespace begin, whitespace chars, whitespace end = 7
            // inline positioned block, whitespace begin, whitespace chars, whitespace end, inline start, text start, text chars, new line = 8
            Assert.AreEqual(15, line.Runs.Count);
            
            var chars = line.Runs[1] as PDFTextRunCharacter;
            var img = line.Runs[3] as PDFLayoutComponentRun;
            var ws = line.Runs[5] as PDFTextRunCharacter;
            var posRun = line.Runs[7] as PDFLayoutPositionedRegionRun;
            
            Assert.IsNotNull(chars);
            Assert.IsNotNull(img);
            Assert.IsNotNull(ws);
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(chars.Width + ws.Width + img.Width, posReg.TotalBounds.X);//first literal width
            Assert.AreEqual(0, posReg.TotalBounds.Y); 

            //inner line size
            Assert.AreEqual(1, posReg.Contents.Count);
            var innerBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);
            var innerChars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(innerChars);

            var innerWidth = innerChars.Width + 5 + 5; //chars + padding
            var innerHeight = innerLine.Height + 5 + 5;
            
            Assert.AreEqual(innerWidth, posReg.TotalBounds.Width);
            Assert.AreEqual(innerHeight, posReg.TotalBounds.Height); //3 lines + 10pt padding
            


            
            
            Assert.AreEqual(0, innerBlock.OffsetX);
            Assert.AreEqual(0, innerBlock.OffsetY);
            Assert.AreEqual(innerHeight, innerBlock.Height);
            Assert.AreEqual(innerWidth, innerBlock.Width);
            

            var newLine = line.Runs[14] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(30 * 1.2, newLine.NewLineOffset.Height); //left align
            //second line

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            //spacer, chars, new line
            Assert.AreEqual(3, line.Runs.Count);
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("block, flowing onto a new line with the", chars2.Characters);

            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds; //relative to the page
            var xOffset = 20 + chars.Width + ws.Width + img.Width; // page margins, text and image width
            var yOffset = 30 + 20 + 20 + 60 + baselineToBottom - innerHeight; //from the baseline + h5 & margins + page margins - up by the height
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(innerWidth, bounds.Width);
            Assert.AreEqual(innerHeight, bounds.Height);
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_09_SmallMiddle()
        {
            var path = AssertGetContentFile("InlineBlock_09_SmallMiddle");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_09_SmallMiddle.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);
            

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);

            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //baseline for inline block by default
            var baseline = line.BaseLineOffset; //doesn't affect the baseline - checked below for offset
            var toBottom = line.BaseLineToBottom;
            
            // text begin, text chars, text end, img component run, whitespace begin, whitespace chars, whitespace end = 7
            // inline positioned block, whitespace begin, whitespace chars, whitespace end, inline start, text start, text chars, new line = 8
            Assert.AreEqual(15, line.Runs.Count);
            
            var chars = line.Runs[1] as PDFTextRunCharacter;
            var img = line.Runs[3] as PDFLayoutComponentRun;
            var ws = line.Runs[5] as PDFTextRunCharacter;
            var posRun = line.Runs[7] as PDFLayoutPositionedRegionRun;
            
            Assert.IsNotNull(chars);
            Assert.IsNotNull(img);
            Assert.IsNotNull(ws);
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(chars.Width + ws.Width + img.Width, posReg.TotalBounds.X);//first literal width
            Assert.AreEqual(0, posReg.TotalBounds.Y); 

            //inner line size
            Assert.AreEqual(1, posReg.Contents.Count);
            var innerBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);
            var innerChars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(innerChars);

            var innerWidth = innerChars.Width + 5 + 5; //chars + padding
            var innerHeight = innerLine.Height + 5 + 5;
            
            Assert.AreEqual(innerWidth, posReg.TotalBounds.Width);
            Assert.AreEqual(innerHeight, posReg.TotalBounds.Height); //3 lines + 10pt padding
            


            
            
            Assert.AreEqual(0, innerBlock.OffsetX);
            Assert.AreEqual(0, innerBlock.OffsetY);
            Assert.AreEqual(innerHeight, innerBlock.Height);
            Assert.AreEqual(innerWidth, innerBlock.Width);
            

            var newLine = line.Runs[14] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(30 * 1.2, newLine.NewLineOffset.Height); //left align
            //second line

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            //spacer, chars, new line
            Assert.AreEqual(3, line.Runs.Count);
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("block, flowing onto a new line with the", chars2.Characters);

            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds; //relative to the page
            var xOffset = 20 + chars.Width + ws.Width + img.Width; // page margins, text and image width
            var yOffset = (Unit)30 + 20 + 20 ; // h5 & margins + page margins - up by the height
            yOffset += ((baseline + toBottom) - innerHeight) / 2; //
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(innerWidth, bounds.Width);
            Assert.AreEqual(innerHeight, bounds.Height);
            
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_10_SmallTopRight()
        {
            var path = AssertGetContentFile("InlineBlock_10_SmallTopRight");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_10_SmallTopRight.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);
            

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);

            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //baseline for inline block by default
            var baseline = line.BaseLineOffset; //doesn't affect the baseline - checked below for offset
            var toBottom = line.BaseLineToBottom;
            var available = line.AvailableWidth;
            
            // text begin, text chars, text end, img component run, whitespace begin, whitespace chars, whitespace end = 7
            // inline positioned block, whitespace begin, whitespace chars, whitespace end, inline start, text start, text chars, new line = 8
            Assert.AreEqual(15, line.Runs.Count);
            
            var chars = line.Runs[1] as PDFTextRunCharacter;
            var img = line.Runs[3] as PDFLayoutComponentRun;
            var ws = line.Runs[5] as PDFTextRunCharacter;
            var posRun = line.Runs[7] as PDFLayoutPositionedRegionRun;
            
            Assert.IsNotNull(chars);
            Assert.IsNotNull(img);
            Assert.IsNotNull(ws);
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(chars.Width + ws.Width + img.Width, posReg.TotalBounds.X);//first literal width
            Assert.AreEqual(0, posReg.TotalBounds.Y); 

            //inner line size
            Assert.AreEqual(1, posReg.Contents.Count);
            var innerBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);
            var innerChars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(innerChars);

            var innerWidth = innerChars.Width + 5 + 5; //chars + padding
            var innerHeight = innerLine.Height + 5 + 5;
            
            Assert.AreEqual(innerWidth, posReg.TotalBounds.Width);
            Assert.AreEqual(innerHeight, posReg.TotalBounds.Height); //3 lines + 10pt padding
            
            
            Assert.AreEqual(0, innerBlock.OffsetX);
            Assert.AreEqual(0, innerBlock.OffsetY);
            Assert.AreEqual(innerHeight, innerBlock.Height);
            Assert.AreEqual(innerWidth, innerBlock.Width);
            

            var newLine = line.Runs[14] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(30 * 1.2, newLine.NewLineOffset.Height); //left align
            //second line

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            //spacer, chars, new line
            Assert.AreEqual(3, line.Runs.Count);
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("block, flowing onto a new line with the", chars2.Characters);

            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds; //relative to the page
            var xOffset = 20 + chars.Width + ws.Width + img.Width; // page margins, text and image width
            var yOffset = (Unit)30 + 20 + 20 ; // h5 & margins + page margins - up by the height
            
            
            Assert.AreEqual(xOffset + available, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(innerWidth, bounds.Width);
            Assert.AreEqual(innerHeight, bounds.Height);
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_11_SmallBottomCenter()
        {
            var path = AssertGetContentFile("InlineBlock_11_SmallBottomCenter");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_11_SmallBottomCenter.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);
            

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);

            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //baseline for inline block by default
            var baselineToBottom = line.BaseLineToBottom; //doesn't affect the baseline - checked below for offset
            var available = line.AvailableWidth;
            
            // text begin, text chars, text end, img component run, whitespace begin, whitespace chars, whitespace end = 7
            // inline positioned block, whitespace begin, whitespace chars, whitespace end, inline start, text start, text chars, new line = 8
            Assert.AreEqual(15, line.Runs.Count);
            
            var chars = line.Runs[1] as PDFTextRunCharacter;
            var img = line.Runs[3] as PDFLayoutComponentRun;
            var ws = line.Runs[5] as PDFTextRunCharacter;
            var posRun = line.Runs[7] as PDFLayoutPositionedRegionRun;
            
            Assert.IsNotNull(chars);
            Assert.IsNotNull(img);
            Assert.IsNotNull(ws);
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(chars.Width + ws.Width + img.Width, posReg.TotalBounds.X);//first literal width
            Assert.AreEqual(0, posReg.TotalBounds.Y); 

            //inner line size
            Assert.AreEqual(1, posReg.Contents.Count);
            var innerBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);
            var innerChars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(innerChars);

            var innerWidth = innerChars.Width + 5 + 5; //chars + padding
            var innerHeight = innerLine.Height + 5 + 5;
            
            Assert.AreEqual(innerWidth, posReg.TotalBounds.Width);
            Assert.AreEqual(innerHeight, posReg.TotalBounds.Height); //3 lines + 10pt padding
            
            
            Assert.AreEqual(0, innerBlock.OffsetX);
            Assert.AreEqual(0, innerBlock.OffsetY);
            Assert.AreEqual(innerHeight, innerBlock.Height);
            Assert.AreEqual(innerWidth, innerBlock.Width);
            

            var newLine = line.Runs[14] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            Assert.AreEqual(30 * 1.2, newLine.NewLineOffset.Height); //left align
            //second line

            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            //spacer, chars, new line
            Assert.AreEqual(3, line.Runs.Count);
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("block, flowing onto a new line with the", chars2.Characters);

            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds; //relative to the page
            var xOffset = 20 + chars.Width + ws.Width + img.Width; // page margins, text and image width
            var yOffset = 30 + 20 + 20 + 60 + baselineToBottom - innerHeight; //from the baseline + h5 & margins + page margins - up by the height

            xOffset += available / 2.0;
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(innerWidth, bounds.Width);
            Assert.AreEqual(innerHeight, bounds.Height);
            
        }
        
         [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_12_WidthToAvailableMiddle()
        {
            var path = AssertGetContentFile("InlineBlock_12_WidthToAvailableMiddle");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_12_WidthToAvailableMiddle.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);
            

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);

            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //The inline block should now
            // text begin, text chars, text end, img component run, whitespace begin, whitespace chars, whitespace end = 7
            Assert.AreEqual(7, line.Runs.Count);
            
            //top aligned image and push the content of the inline block down.
            var bottom = 60; 
           
            Assert.AreEqual(bottom, line.Height);
            
            //Now onto the second line.
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            // inline positioned block , whitespace begin, whitespace chars, whitespace end, inline start, text start, new line = 7 
            Assert.AreEqual(7, line.Runs.Count);
            Assert.AreEqual(60, line.OffsetY);
           
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            var ws = line.Runs[2] as PDFTextRunCharacter;
            
            Assert.IsNotNull(ws);
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(0, posReg.TotalBounds.X);//moved onto a new line
            Assert.AreEqual(0, posReg.TotalBounds.Y); //offset by the previous line height

            Assert.IsTrue(posReg.Width > 480);
            Assert.IsTrue(posReg.Width < line.FullWidth);
            
            Assert.AreEqual(30 + 10 + 20, posReg.TotalBounds.Height); //2 lines + 5pt padding + 10pt margins
            Assert.AreEqual(1, posReg.Contents.Count);


            
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(posReg.TotalBounds.Height, posBlock.Height);
            Assert.AreEqual(posReg.Width, posBlock.Width);
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(2, posBlock.Columns[0].Contents.Count); //on two lines

            
            var newLine = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            
            
            //third line

            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(60 + 60, line.OffsetY); //The image height + the inline block height (inc. margins)
            
            
            //spacer, chars, new line
            Assert.AreEqual(3, line.Runs.Count);
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("After the inline block, flowing onto a", chars2.Characters);

            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds;
            var xOffset = 20 + 20 + 10 + 10 ; // page margins + nest margin + nest padding + ib margin
            var yOffset = 20 + 50 + 30 + 60 + 10; //page margins, h5 + margins, nest margins and padding, line height, ib margin
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(posBlock.Width - 20, bounds.Width); //inline width - margins
            Assert.AreEqual(30 + 10, bounds.Height); //2 lines + padding (no margins)
            
        }
        
        
        
         [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_13_ExplicitWidthAndFloat()
        {
            var path = AssertGetContentFile("InlineBlock_13_ExplicitWidthAndFloat");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_13_ExplicitWidthAndFloat.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);
            

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(1, nest.PositionedRegions.Count);

            var line = nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //The inline block should now
            // text begin, text chars, text end, img component run, whitespace begin, whitespace chars, whitespace end = 7
            Assert.AreEqual(7, line.Runs.Count);
            
            //top aligned image and push the content of the inline block down.
            var bottom = 60; 
           
            Assert.AreEqual(bottom, line.Height);
            
            //Now onto the second line.
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            // inline positioned block , whitespace begin, whitespace chars, whitespace end, inline start, text start, new line = 7 
            Assert.AreEqual(7, line.Runs.Count);
            Assert.AreEqual(60, line.OffsetY);
           
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            var ws = line.Runs[2] as PDFTextRunCharacter;
            
            Assert.IsNotNull(ws);
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(0, posReg.TotalBounds.X);//moved onto a new line
            Assert.AreEqual(0, posReg.TotalBounds.Y); //offset by the previous line height

            Assert.IsTrue(posReg.Width > 480);
            Assert.IsTrue(posReg.Width < line.FullWidth);
            
            Assert.AreEqual(30 + 10 + 20, posReg.TotalBounds.Height); //2 lines + 5pt padding + 10pt margins
            Assert.AreEqual(1, posReg.Contents.Count);


            
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(posReg.TotalBounds.Height, posBlock.Height);
            Assert.AreEqual(posReg.Width, posBlock.Width);
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(2, posBlock.Columns[0].Contents.Count); //on two lines

            
            var newLine = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(newLine);
            
            
            //third line

            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(60 + 60, line.OffsetY); //The image height + the inline block height (inc. margins)
            
            
            //spacer, chars, new line
            Assert.AreEqual(3, line.Runs.Count);
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("After the inline block, flowing onto a", chars2.Characters);

            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds;
            var xOffset = 20 + 20 + 10 + 10 ; // page margins + nest margin + nest padding + ib margin
            var yOffset = 20 + 50 + 30 + 60 + 10; //page margins, h5 + margins, nest margins and padding, line height, ib margin
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(posBlock.Width - 20, bounds.Width); //inline width - margins
            Assert.AreEqual(30 + 10, bounds.Height); //2 lines + padding (no margins)
            
        }
        
        
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockBlockSmallExplicitSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            //section.TextLeading = 40;
            doc.Pages.Add(section);

            var span = new Span();
            span.Contents.Add(new TextLiteral("Before the inline "));
            section.Contents.Add(span);

            Div inline = new Div()
            {
                Height = 10,
                Width = 100,
                DisplayMode = Drawing.DisplayMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                //Margins = new Thickness(5)
            };
            //inline.Contents.Add(new TextLiteral("In the inline block"));
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral(" After the inline and flowing onto a new line with the required offset"));
            span.FontBold = true;
            span.FontSize = 30;
            section.Contents.Add(span);

            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = 10;

            using (var ms = DocStreams.GetOutputStream("Positioned_InlineBlockExplicitSizeSmall.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(1, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.AreEqual(0, first.Height);
            Assert.AreEqual(1, first.Runs.Count);
            var posRun = first.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, content.PositionedRegions[0]);
            var posReg = content.PositionedRegions[0];

            //TODO: Clean up offsetX and Y with TotalBounds.

            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(100, posReg.TotalBounds.Y);

            Assert.AreEqual(150, posReg.TotalBounds.Height);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(1, posReg.Contents.Count);

            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(150, posBlock.Height);
            Assert.AreEqual(200, posBlock.Width);
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(2, posBlock.Columns[0].Contents.Count);

            //Check the block after to make sure it is ignoring the positioned region.
            var second = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(0, second.OffsetY);
            Assert.AreEqual(25, second.Height);
            Assert.AreEqual(1, second.Columns[0].Contents.Count); //just a line

            //block region line = textbegin, chars, textend
            Assert.AreEqual("In normal content flow", ((second.Columns[0].Contents[0] as PDFLayoutLine).Runs[1] as PDFTextRunCharacter).Characters);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockMultipleBlockExplicitSize()
        {
            const int lineHeight = 25;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = lineHeight;
            section.Margins = 10;
            section.BackgroundColor = Drawing.StandardColors.Silver;
            //section.HorizontalAlignment = HorizontalAlignment.Justified;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = section.TextLeading;
            section.Style.OverlayGrid.GridXOffset = 10;
            section.Style.OverlayGrid.GridYOffset = 10;
            doc.Pages.Add(section);

            var span = new Span();
            span.Contents.Add(new TextLiteral("Before the inline "));
            section.Contents.Add(span);

            Div inline = new Div()
            {
                Height = lineHeight * 2,
                Width = 100,
                DisplayMode = Drawing.DisplayMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
            };
            //inline.Contents.Add(new TextLiteral("In the inline block"));
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral(" After the inline and flowing onto a new line with the required offset"));
            section.Contents.Add(span);

            inline = new Div()
            {
                Height = lineHeight / 2,
                Width = 50,
                DisplayMode = Drawing.DisplayMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Aqua,
            };

            section.Contents.Add(inline);
            //section.Contents.Clear();

            span = new Span();
            span.Contents.Add(new TextLiteral("After the second inline and flowing onto a new line."));
            //span.Contents.Add(new TextLiteral(" Inline and flowing onto a new line "));
            section.Contents.Add(span);



            inline = new Div()
            {
                Height = (lineHeight * 3) - 10, //take off the margins.
                Width = 100,
                DisplayMode = Drawing.DisplayMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Lime,
                Margins = 5
            };
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral("After the third inline and flowing onto a new line that should continue on in the normal height for the page."));
            section.Contents.Add(span);

            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            inflow.Contents.Add(new TextLiteral("In normal content flow"));
            section.Contents.Add(inflow);

            using (var ms = DocStreams.GetOutputStream("Positioned_InlineBlockMultipleExplicitSizeVAlignDefault.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }



            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(6, content.Columns[0].Contents.Count);
            Assert.AreEqual(3, content.PositionedRegions.Count);

            //first line - 1 inline block at 2x line height

            var line = content.Columns[0].Contents[0] as PDFLayoutLine;

            Assert.AreEqual(lineHeight * 2, line.Height);
            Assert.AreEqual(10, line.Runs.Count);

            var leftChars = line.Runs[2] as PDFTextRunCharacter;
            var inlineRun = line.Runs[5] as PDFLayoutInlineBlockRun;
            var rightChars = line.Runs[8] as PDFTextRunCharacter;
            var newline = line.Runs[9] as PDFTextRunNewLine;

            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);

            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[0]);
            var posReg = content.PositionedRegions[0];


            //TODO: Clean up offsetX and Y with TotalBounds.

            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            Assert.AreEqual(lineHeight * 2, posReg.TotalBounds.Height);
            Assert.AreEqual(100, posReg.TotalBounds.Width);

            Assert.Inconclusive();


            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(150, posBlock.Height);
            Assert.AreEqual(200, posBlock.Width);
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(2, posBlock.Columns[0].Contents.Count);

            //Check the block after to make sure it is ignoring the positioned region.
            var second = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(0, second.OffsetY);
            Assert.AreEqual(25, second.Height);
            Assert.AreEqual(1, second.Columns[0].Contents.Count); //just a line

            //block region line = textbegin, chars, textend
            Assert.AreEqual("In normal content flow", ((second.Columns[0].Contents[0] as PDFLayoutLine).Runs[1] as PDFTextRunCharacter).Characters);
        }

        [TestMethod()]
        [TestCategory(TestCategoryName)]
        public void InlineBlockMultipleBlockExplicitSizeVAlignBottom()
        {
            const int lineHeight = 30;
            const VerticalAlignment vAlign = VerticalAlignment.Bottom;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = lineHeight;
            section.Margins = 10;
            section.BackgroundColor = Drawing.StandardColors.Silver;
            //section.HorizontalAlignment = HorizontalAlignment.Justified;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = lineHeight;
            section.Style.OverlayGrid.GridXOffset = 10;
            section.Style.OverlayGrid.GridYOffset = 10;
            doc.Pages.Add(section);

            var span = new Span();
            span.Contents.Add(new TextLiteral("Before the inline "));
            section.Contents.Add(span);

            //Inline block twice line height
            Div inline = new Div()
            {
                Height = lineHeight * 2,
                Width = 100,
                DisplayMode = Drawing.DisplayMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                VerticalAlignment = vAlign
            };
            //inline.Contents.Add(new TextLiteral("In the inline block"));
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral(" After the inline and flowing onto a new line with the required offset"));
            section.Contents.Add(span);

            //Half height inline block that should be at the top.
            inline = new Div()
            {
                Height = lineHeight / 2,
                Width = 50,
                DisplayMode = Drawing.DisplayMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Aqua,
                VerticalAlignment = vAlign
            };

            section.Contents.Add(inline);


            span = new Span();
            span.Contents.Add(new TextLiteral("After the second inline and flowing onto a new line."));
            section.Contents.Add(span);

            

            //3 times line height (inc. magins inline block)
            inline = new Div()
            {
                Height = (lineHeight * 3) - 10, //take off the margins
                Width = 100 - 10, //take off the margins
                DisplayMode = Drawing.DisplayMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Lime,
                VerticalAlignment = vAlign,
                Margins = 5
            };
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral("After the third inline and flowing onto a new line that should continue on in the normal height for the page."));
            section.Contents.Add(span);

            //A new full width div - should be set nicely below the rest of the text.
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            inflow.Contents.Add(new TextLiteral("In normal content flow"));
            section.Contents.Add(inflow);

            using (var ms = DocStreams.GetOutputStream("Positioned_InlineBlockMultipleExplicitSizeVAlignBottom.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }



            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(6, content.Columns[0].Contents.Count);
            Assert.AreEqual(3, content.PositionedRegions.Count);

            //first line - 1 inline block at 2x line height

            var line = content.Columns[0].Contents[0] as PDFLayoutLine;

            Assert.AreEqual(lineHeight * 2, line.Height);
            Assert.AreEqual(10, line.Runs.Count);
            var leftBegin = line.Runs[1] as PDFTextRunBegin;
            var leftChars = line.Runs[2] as PDFTextRunCharacter;
            var inlineRun = line.Runs[5] as PDFLayoutInlineBlockRun;
            var rightBegin = line.Runs[7] as PDFTextRunBegin;
            var rightChars = line.Runs[8] as PDFTextRunCharacter;
            var newline = line.Runs[9] as PDFTextRunNewLine;

            Assert.IsNotNull(leftBegin);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);

            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[0]);
            var posReg = content.PositionedRegions[0];

            //The positioned region is relative to the origin of the first line.

            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            Assert.AreEqual(lineHeight * 2, posReg.TotalBounds.Height);
            Assert.AreEqual(100, posReg.TotalBounds.Width);

            //valign top baseline offset is ascender + half leading.
            var baseline = leftBegin.TextRenderOptions.GetAscent() + (lineHeight - section.FontSize) / 2;

            Assert.AreEqual(baseline, line.BaseLineOffset);
            //Add the margins and line height for the start text cursor
            Assert.AreEqual(baseline + section.Margins.Top + lineHeight, leftBegin.StartTextCursor.Height);
            Assert.AreEqual(section.Margins.Left, leftBegin.StartTextCursor.Width);

            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,50, 100
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight * 2, posBlock.Height);
            Assert.AreEqual(100, posBlock.Width);

            //Right begin text
            Assert.AreEqual(baseline + section.Margins.Top + lineHeight, rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line should push the cursor down and right for the inline block and first chars.
            var offset = newline.NewLineOffset;
            Assert.AreEqual(line.BaseLineOffset + line.BaseLineToBottom, offset.Height);
            Assert.AreEqual(leftChars.Width + posReg.Width, offset.Width);

            //second line - 1 small top aligned inline block with text either side

            line = content.Columns[0].Contents[1] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(9, line.Runs.Count);

            leftBegin = null;
            var leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = line.Runs[4] as PDFLayoutInlineBlockRun;
            rightBegin = line.Runs[6] as PDFTextRunBegin;
            rightChars = line.Runs[7] as PDFTextRunCharacter;
            newline = line.Runs[8] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);


            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[1]);
            posReg = content.PositionedRegions[1];

            //The positioned region is relative to the origin of the first line.
            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            Assert.AreEqual(lineHeight / 2, posReg.TotalBounds.Height);
            Assert.AreEqual(50, posReg.TotalBounds.Width);


            //Same baseline offset as line 1
            Assert.AreEqual(baseline, line.BaseLineOffset);

            posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,50, 100
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight / 2, posBlock.Height);
            Assert.AreEqual(50, posBlock.Width);

            //Right begin text - baseline and margins plue the previous line height ( = lineHeight * 2)
            Assert.AreEqual(baseline + section.Margins.Top + (lineHeight * 2), rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line - offset should be to the line height of the next line (which has a lineHeight * 3 inline block in it)
            offset = newline.NewLineOffset;
            Assert.AreEqual(lineHeight * 3, offset.Height);
            Assert.AreEqual(leftChars.Width + posReg.Width, offset.Width);

            //third line - 1 top aligned inline block 3 * line height inc margins with text either side

            line = content.Columns[0].Contents[2] as PDFLayoutLine;

            Assert.AreEqual(lineHeight * 3, line.Height);
            Assert.AreEqual(9, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = line.Runs[4] as PDFLayoutInlineBlockRun;
            rightBegin = line.Runs[6] as PDFTextRunBegin;
            rightChars = line.Runs[7] as PDFTextRunCharacter;
            newline = line.Runs[8] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);

            //third positioned region
            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[2]);
            posReg = content.PositionedRegions[2];

            //The positioned region is relative to the origin of the first line.
            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            //take off the margins
            Assert.AreEqual((lineHeight * 3) - 10, posReg.TotalBounds.Height);
            Assert.AreEqual(100 - 10, posReg.TotalBounds.Width);


            //Same baseline offset as line 1
            Assert.AreEqual(baseline, line.BaseLineOffset);

            posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,75, 110
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight * 3, posBlock.Height);
            Assert.AreEqual(100, posBlock.Width);

            //Right begin text - baseline and margins plus the previous lines height (3) + the inline height (3) - a line, so  = lineHeight * 5
            Assert.AreEqual(baseline + section.Margins.Top + (lineHeight * 5), rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line - single line height v offset
            offset = newline.NewLineOffset;
            Assert.AreEqual(line.BaseLineOffset + line.BaseLineToBottom, offset.Height);
            Assert.AreEqual(lineHeight, offset.Height);
            //include the margins as well
            Assert.AreEqual(leftChars.Width + posReg.Width + 10, offset.Width);


            //fourth line -  is just a spacer, chars and a new line 

            line = content.Columns[0].Contents[3] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(3, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = null;
            rightBegin = null;
            rightChars = null;
            newline = line.Runs[2] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            //Assert.IsNotNull(inlineRun);
            //Assert.IsNotNull(rightBegin);
            //Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);


            //Simple line from a newline offset (checked previously) so zero.
            //TODO: zero works as it's ignored for the newline flow. But could be set to make more appropriate.
            Assert.AreEqual(0, line.BaseLineOffset);


            //New line - single line height v offset and 0 width (same start point for fifth line as fourth)
            offset = newline.NewLineOffset;
            Assert.AreEqual(lineHeight, offset.Height);
            Assert.AreEqual(0, offset.Width);


            //fifth line -  is just a spacer, chars, text end and an inline end 

            line = content.Columns[0].Contents[4] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(4, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = null;
            rightBegin = null;
            rightChars = null;
            newline = null;
            var end = line.Runs[2] as PDFTextRunEnd;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(end);

            //Simple line from a newline offset (checked previously) so zero.
            Assert.AreEqual(baseline, line.BaseLineOffset);

            //check the last block is at the correct offset in the page

            var lastBlock = content.Columns[0].Contents[5] as PDFLayoutBlock;
            Assert.IsNotNull(lastBlock);

            //Should be 8 lines down based on the above content.
            Assert.AreEqual(lineHeight * 8, lastBlock.TotalBounds.Y);

            Assert.AreEqual(0, lastBlock.TotalBounds.X);

            //block region line = textbegin, chars, textend
            Assert.AreEqual("In normal content flow", ((lastBlock.Columns[0].Contents[0] as PDFLayoutLine).Runs[1] as PDFTextRunCharacter).Characters);
        }
        /// <summary>
        /// Vertical align top. All inline blocks should be from the top down,
        /// and the text placed at the top of the line where the height is greater than the set line height.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockMultipleBlockExplicitSizeVAlignTop()
        {
            const int lineHeight = 30;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = lineHeight;
            section.Margins = 10;
            section.BackgroundColor = Drawing.StandardColors.Silver;
            //section.HorizontalAlignment = HorizontalAlignment.Justified;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = lineHeight;
            section.Style.OverlayGrid.GridXOffset = 10;
            section.Style.OverlayGrid.GridYOffset = 10;
            doc.Pages.Add(section);

            var span = new Span();
            span.Contents.Add(new TextLiteral("Before the inline "));
            section.Contents.Add(span);

            //Inline block twice line height
            Div inline = new Div()
            {
                Height = lineHeight * 2,
                Width = 100,
                DisplayMode = Drawing.DisplayMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                VerticalAlignment = VerticalAlignment.Top
            };
            //inline.Contents.Add(new TextLiteral("In the inline block"));
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral(" After the inline and flowing onto a new line with the required offset"));
            section.Contents.Add(span);

            //Half height inline block that should be at the top.
            inline = new Div()
            {
                Height = lineHeight / 2,
                Width = 50,
                DisplayMode = Drawing.DisplayMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Aqua,
                VerticalAlignment = VerticalAlignment.Top
            };

            section.Contents.Add(inline);
            

            span = new Span();
            span.Contents.Add(new TextLiteral("After the second inline and flowing onto a new line."));
            section.Contents.Add(span);


            //3 times line height (inc. magins inline block)
            inline = new Div()
            {
                Height = (lineHeight * 3) - 10, //take off the margins
                Width = 100 - 10, //take off the margins
                DisplayMode = Drawing.DisplayMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Lime,
                VerticalAlignment = VerticalAlignment.Top,
                Margins = 5
            };
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral("After the third inline and flowing onto a new line that should continue on in the normal height for the page."));
            section.Contents.Add(span);

            //A new full width div - should be set nicely below the rest of the text.
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            inflow.Contents.Add(new TextLiteral("In normal content flow"));
            section.Contents.Add(inflow);

            using (var ms = DocStreams.GetOutputStream("Positioned_InlineBlockMultipleExplicitSizeVAlignTop.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(6, content.Columns[0].Contents.Count);
            Assert.AreEqual(3, content.PositionedRegions.Count);

            //first line - 1 inline block at 2x line height

            var line = content.Columns[0].Contents[0] as PDFLayoutLine;

            Assert.AreEqual(lineHeight * 2, line.Height);
            Assert.AreEqual(10, line.Runs.Count);
            var leftBegin = line.Runs[1] as PDFTextRunBegin;
            var leftChars = line.Runs[2] as PDFTextRunCharacter;
            var inlineRun = line.Runs[5] as PDFLayoutInlineBlockRun;
            var rightBegin = line.Runs[7] as PDFTextRunBegin;
            var rightChars = line.Runs[8] as PDFTextRunCharacter;
            var newline = line.Runs[9] as PDFTextRunNewLine;

            Assert.IsNotNull(leftBegin);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);

            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[0]);
            var posReg = content.PositionedRegions[0];

            //The positioned region is relative to the origin of the first line.

            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            Assert.AreEqual(lineHeight * 2, posReg.TotalBounds.Height);
            Assert.AreEqual(100, posReg.TotalBounds.Width);

            //valign top baseline offset is ascender + half leading.
            var baseline = leftBegin.TextRenderOptions.GetAscent() + (lineHeight - section.FontSize) / 2;

            Assert.AreEqual(baseline, line.BaseLineOffset);
            //Add the margins for the start text cursor
            Assert.AreEqual(baseline + section.Margins.Top, leftBegin.StartTextCursor.Height);
            Assert.AreEqual(section.Margins.Left, leftBegin.StartTextCursor.Width);

            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,50, 100
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight * 2, posBlock.Height);
            Assert.AreEqual(100, posBlock.Width);

            //Right begin text
            Assert.AreEqual(baseline + section.Margins.Top, rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line should push the cursor down and right for the inline block and first chars.
            var offset = newline.NewLineOffset;
            Assert.AreEqual(line.BaseLineOffset + line.BaseLineToBottom, offset.Height);
            Assert.AreEqual(leftChars.Width + posReg.Width, offset.Width);

            //second line - 1 small top aligned inline block with text either side

            line = content.Columns[0].Contents[1] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(9, line.Runs.Count);

            leftBegin = null;
            var leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = line.Runs[4] as PDFLayoutInlineBlockRun;
            rightBegin = line.Runs[6] as PDFTextRunBegin;
            rightChars = line.Runs[7] as PDFTextRunCharacter;
            newline = line.Runs[8] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);

            
            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[1]);
            posReg = content.PositionedRegions[1];

            //The positioned region is relative to the origin of the first line.
            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            Assert.AreEqual(lineHeight / 2, posReg.TotalBounds.Height);
            Assert.AreEqual(50, posReg.TotalBounds.Width);

            
            //Same baseline offset as line 1
            Assert.AreEqual(baseline, line.BaseLineOffset);
            
            posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,50, 100
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight / 2, posBlock.Height);
            Assert.AreEqual(50, posBlock.Width);

            //Right begin text - baseline and margins plue the previous line height ( = lineHeight * 2)
            Assert.AreEqual(baseline + section.Margins.Top + (lineHeight * 2), rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line - single line height v offset
            offset = newline.NewLineOffset;
            Assert.AreEqual(line.BaseLineOffset + line.BaseLineToBottom, offset.Height);
            Assert.AreEqual(lineHeight, offset.Height);
            Assert.AreEqual(leftChars.Width + posReg.Width, offset.Width);

            //third line - 1 top aligned inline block 3 * line height inc margins with text either side

            line = content.Columns[0].Contents[2] as PDFLayoutLine;

            Assert.AreEqual(lineHeight * 3, line.Height);
            Assert.AreEqual(9, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = line.Runs[4] as PDFLayoutInlineBlockRun;
            rightBegin = line.Runs[6] as PDFTextRunBegin;
            rightChars = line.Runs[7] as PDFTextRunCharacter;
            newline = line.Runs[8] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);

            //third positioned region
            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[2]);
            posReg = content.PositionedRegions[2];

            //The positioned region is relative to the origin of the first line.
            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            //take off the margins
            Assert.AreEqual((lineHeight * 3) - 10, posReg.TotalBounds.Height);
            Assert.AreEqual(100 - 10, posReg.TotalBounds.Width);


            //Same baseline offset as line 1
            Assert.AreEqual(baseline, line.BaseLineOffset);

            posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,75, 110
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight* 3, posBlock.Height);
            Assert.AreEqual(100, posBlock.Width);

            //Right begin text - baseline and margins plue the previous line height ( = lineHeight * 2)
            Assert.AreEqual(baseline + section.Margins.Top + (lineHeight * 3), rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line - single line height v offset
            offset = newline.NewLineOffset;
            Assert.AreEqual(line.BaseLineOffset + line.BaseLineToBottom, offset.Height);
            Assert.AreEqual(lineHeight * 3, offset.Height);
            //include the margins as well
            Assert.AreEqual(leftChars.Width + posReg.Width + 10, offset.Width);


            //fourth line -  is just a spacer, chars and a new line 

            line = content.Columns[0].Contents[3] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(3, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = null;
            rightBegin = null;
            rightChars = null;
            newline = line.Runs[2] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            //Assert.IsNotNull(inlineRun);
            //Assert.IsNotNull(rightBegin);
            //Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);


            //Simple line from a newline offset (checked previously) so zero.
            //TODO: zero works as it's ignored for the newline flow. But could be set to make more appropriate.
            Assert.AreEqual(0, line.BaseLineOffset);

            
            //New line - single line height v offset and 0 width (same start point for fifth line as fourth)
            offset = newline.NewLineOffset;
            Assert.AreEqual(lineHeight, offset.Height);
            Assert.AreEqual(0, offset.Width);


            //fifth line -  is just a spacer, chars, text end and an inline end 

            line = content.Columns[0].Contents[4] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(4, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = null;
            rightBegin = null;
            rightChars = null;
            newline = null;
            var end = line.Runs[2] as PDFTextRunEnd;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(end);

            //Simple line from a newline offset (checked previously) so zero.
            Assert.AreEqual(baseline, line.BaseLineOffset);

            //check the last block is at the correct offset in the page

            var lastBlock = content.Columns[0].Contents[5] as PDFLayoutBlock;
            Assert.IsNotNull(lastBlock);

            //Should be 8 lines down based on the above content.
            Assert.AreEqual(lineHeight * 8, lastBlock.TotalBounds.Y);

            Assert.AreEqual(0, lastBlock.TotalBounds.X);
            
            //block region line = textbegin, chars, textend
            Assert.AreEqual("In normal content flow", ((lastBlock.Columns[0].Contents[0] as PDFLayoutLine).Runs[1] as PDFTextRunCharacter).Characters);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockMultipleBlockExplicitSizeVAlignMiddle()
        {
            const double lineHeight = 30;
            const VerticalAlignment vAlign = VerticalAlignment.Middle;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = lineHeight;
            section.Margins = 10;
            section.BackgroundColor = Drawing.StandardColors.Silver;
            //section.HorizontalAlignment = HorizontalAlignment.Justified;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = lineHeight;
            section.Style.OverlayGrid.GridXOffset = 10;
            section.Style.OverlayGrid.GridYOffset = 10;
            doc.Pages.Add(section);

            var span = new Span();
            span.Contents.Add(new TextLiteral("Before the inline "));
            section.Contents.Add(span);

            //Inline block twice line height
            Div inline = new Div()
            {
                Height = lineHeight * 2,
                Width = 100,
                DisplayMode = Drawing.DisplayMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                VerticalAlignment = vAlign
            };
            //inline.Contents.Add(new TextLiteral("In the inline block"));
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral(" After the inline and flowing onto a new line with the required offset"));
            section.Contents.Add(span);

            //Half height inline block that should be at the top.
            inline = new Div()
            {
                Height = lineHeight / 2,
                Width = 50,
                DisplayMode = Drawing.DisplayMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Aqua,
                VerticalAlignment = vAlign
            };

            section.Contents.Add(inline);


            span = new Span();
            span.Contents.Add(new TextLiteral("After the second inline and flowing onto a new line."));
            section.Contents.Add(span);



            //3 times line height (inc. magins inline block)
            inline = new Div()
            {
                Height = (lineHeight * 3) - 10, //take off the margins
                Width = 100 - 10, //take off the margins
                DisplayMode = Drawing.DisplayMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Lime,
                VerticalAlignment = vAlign,
                Margins = 5
            };
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral("After the third inline and flowing onto a new line that should continue on in the normal height for the page."));
            section.Contents.Add(span);

            //A new full width div - should be set nicely below the rest of the text.
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            inflow.Contents.Add(new TextLiteral("In normal content flow"));
            section.Contents.Add(inflow);

            using (var ms = DocStreams.GetOutputStream("Positioned_InlineBlockMultipleExplicitSizeVAlignMiddle.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }



            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(6, content.Columns[0].Contents.Count);
            Assert.AreEqual(3, content.PositionedRegions.Count);

            //first line - 1 inline block at 2x line height

            var line = content.Columns[0].Contents[0] as PDFLayoutLine;

            Assert.AreEqual(lineHeight * 2, line.Height);
            Assert.AreEqual(10, line.Runs.Count);
            var leftBegin = line.Runs[1] as PDFTextRunBegin;
            var leftChars = line.Runs[2] as PDFTextRunCharacter;
            var inlineRun = line.Runs[5] as PDFLayoutInlineBlockRun;
            var rightBegin = line.Runs[7] as PDFTextRunBegin;
            var rightChars = line.Runs[8] as PDFTextRunCharacter;
            var newline = line.Runs[9] as PDFTextRunNewLine;

            Assert.IsNotNull(leftBegin);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);

            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[0]);
            var posReg = content.PositionedRegions[0];

            //The positioned region is relative to the origin of the first line.

            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            Assert.AreEqual(lineHeight * 2, posReg.TotalBounds.Height);
            Assert.AreEqual(100, posReg.TotalBounds.Width);

            //valign middle baseline offset is ascender + half leading.
            var leading = (lineHeight - leftBegin.TextRenderOptions.GetSize());
            var baseline = leftBegin.TextRenderOptions.GetAscent() + (leading / 2);

            Assert.AreEqual(baseline, line.BaseLineOffset);
            //Add the margins and half line height for the start text cursor in middle
            Assert.AreEqual(baseline + section.Margins.Top + (lineHeight / 2), leftBegin.StartTextCursor.Height);
            Assert.AreEqual(section.Margins.Left, leftBegin.StartTextCursor.Width);

            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,50, 100
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight * 2, posBlock.Height);
            Assert.AreEqual(100, posBlock.Width);

            //Right begin text
            Assert.AreEqual(baseline + section.Margins.Top + (lineHeight / 2), rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line should push the cursor down and right for the inline block and first chars.
            var offset = newline.NewLineOffset;
            Assert.AreEqual(line.BaseLineOffset + line.BaseLineToBottom, offset.Height);
            Assert.AreEqual(leftChars.Width + posReg.Width, offset.Width);

            //second line - 1 small top aligned inline block with text either side

            line = content.Columns[0].Contents[1] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(9, line.Runs.Count);

            leftBegin = null;
            var leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = line.Runs[4] as PDFLayoutInlineBlockRun;
            rightBegin = line.Runs[6] as PDFTextRunBegin;
            rightChars = line.Runs[7] as PDFTextRunCharacter;
            newline = line.Runs[8] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);


            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[1]);
            posReg = content.PositionedRegions[1];

            //The positioned region is relative to the origin of the first line.
            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            Assert.AreEqual(lineHeight / 2, posReg.TotalBounds.Height);
            Assert.AreEqual(50, posReg.TotalBounds.Width);


            //Same baseline offset as line 1
            Assert.AreEqual(baseline, line.BaseLineOffset);

            posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,50, 100
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight / 2, posBlock.Height);
            Assert.AreEqual(50, posBlock.Width);

            //Right begin text - baseline and margins plue the previous line height ( = lineHeight * 2)
            Assert.AreEqual(baseline + section.Margins.Top + (lineHeight * 2), rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line - offset should be to the line height of the next line (which has a lineHeight * 3 inline block in it) minus its line space (lineHeight)
            offset = newline.NewLineOffset;
            Assert.AreEqual((lineHeight * 2), offset.Height);
            Assert.AreEqual(leftChars.Width + posReg.Width, offset.Width);

            //third line - 1 top aligned inline block 3 * line height inc margins with text either side

            line = content.Columns[0].Contents[2] as PDFLayoutLine;

            Assert.AreEqual(lineHeight * 3, line.Height);
            Assert.AreEqual(9, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = line.Runs[4] as PDFLayoutInlineBlockRun;
            rightBegin = line.Runs[6] as PDFTextRunBegin;
            rightChars = line.Runs[7] as PDFTextRunCharacter;
            newline = line.Runs[8] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);

            //third positioned region
            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[2]);
            posReg = content.PositionedRegions[2];

            //The positioned region is relative to the origin of the first line.
            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            //take off the margins
            Assert.AreEqual((lineHeight * 3) - 10, posReg.TotalBounds.Height);
            Assert.AreEqual(100 - 10, posReg.TotalBounds.Width);


            //Same baseline offset as line 1
            Assert.AreEqual(baseline, line.BaseLineOffset);

            posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,75, 110
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight * 3, posBlock.Height);
            Assert.AreEqual(100, posBlock.Width);

            //Right begin text - baseline and margins plus the previous lines height (3 - 1 for middle) + the inline height (3) - a line, so  = lineHeight * 4
            Assert.AreEqual(baseline + section.Margins.Top + (lineHeight * 4), rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line - single line height + a line height space offset
            offset = newline.NewLineOffset;
            Assert.AreEqual(line.BaseLineOffset + line.BaseLineToBottom, offset.Height);
            Assert.AreEqual(lineHeight * 2, offset.Height);
            //include the margins as well
            Assert.AreEqual(leftChars.Width + posReg.Width + 10, offset.Width);


            //fourth line -  is just a spacer, chars and a new line 

            line = content.Columns[0].Contents[3] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(3, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = null;
            rightBegin = null;
            rightChars = null;
            newline = line.Runs[2] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            //Assert.IsNotNull(inlineRun);
            //Assert.IsNotNull(rightBegin);
            //Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);


            //Simple line from a newline offset (checked previously) so zero.
            //TODO: zero works as it's ignored for the newline flow. But could be set to make more appropriate.
            Assert.AreEqual(0, line.BaseLineOffset);


            //New line - single line height v offset and 0 width (same start point for fifth line as fourth)
            offset = newline.NewLineOffset;
            Assert.AreEqual(lineHeight, offset.Height);
            Assert.AreEqual(0, offset.Width);


            //fifth line -  is just a spacer, chars, text end and an inline end 

            line = content.Columns[0].Contents[4] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(4, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = null;
            rightBegin = null;
            rightChars = null;
            newline = null;
            var end = line.Runs[2] as PDFTextRunEnd;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(end);

            //Simple line from a newline offset (checked previously) so zero.
            Assert.AreEqual(baseline, line.BaseLineOffset);

            //check the last block is at the correct offset in the page

            var lastBlock = content.Columns[0].Contents[5] as PDFLayoutBlock;
            Assert.IsNotNull(lastBlock);

            //Should be 8 lines down based on the above content.
            Assert.AreEqual(lineHeight * 8, lastBlock.TotalBounds.Y);

            Assert.AreEqual(0, lastBlock.TotalBounds.X);

            //block region line = textbegin, chars, textend
            Assert.AreEqual("In normal content flow", ((lastBlock.Columns[0].Contents[0] as PDFLayoutLine).Runs[1] as PDFTextRunCharacter).Characters);
        }


        //
        // Tests to write
        //

        /// <summary>
        /// Various inline blocks that have different alignments
        /// with various text sizes and fonts.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockMixedAlignment()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// An inline block that cannot fit on the avaiable space on the current line
        /// based on it's explicit width, so should force a new line
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockOverflowToNewLine()
        {
            Assert.Inconclusive();
        }


        /// <summary>
        /// An inline block doesn't have an explicit width, but the content inside will
        /// force the width to be greater than available on the current line. So should
        /// move to the next line.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockOverflowToNewLineContentWidth()
        {
            Assert.Inconclusive();
        }


        /// <summary>
        /// An inline block that cannot fit on the avaiable space on the current line
        /// based on it's explicit width, so should force a new line. No line space
        /// available - so go onto an new column.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockOverflowToNewColumn()
        {
            Assert.Inconclusive();
        }

        /// <summary>
        /// An inline block that cannot fit on the avaiable space on the current line
        /// based on it's explicit width, so should force a new line. No line space
        /// available - so go onto an new PAGE.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockOverflowToNewPage()
        {
            Assert.Inconclusive();
        }



    }
}
