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
            // text begin, chars, text end, inline positioned block , whitespace begin, whitespace chars, whitespace end, inline start, text start, chars, new line = 11
           
            Assert.AreEqual(11, line.Runs.Count);

            var chars = line.Runs[1];
            Assert.IsNotNull(chars);
            
            //top aligned image and push the content of the inline block down.
            var bottom = (5 * 15) + 10 + 20; //5 lines + padding + margins 
           
            Assert.AreEqual(bottom, line.Height);
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            
            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(chars.Width, posReg.TotalBounds.X);//moved onto a new line
            Assert.AreEqual(0, posReg.TotalBounds.Y); //offset by the previous line height

            Assert.AreEqual(200, posReg.Width);
            
            
            Assert.AreEqual(bottom, posReg.TotalBounds.Height); //5 lines + 5pt padding + 10pt margins
            Assert.AreEqual(1, posReg.Contents.Count);


            
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(posReg.TotalBounds.Height, posBlock.Height);
            Assert.AreEqual(posReg.Width + 20, posBlock.Width); //add margins
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(5, posBlock.Columns[0].Contents.Count); //on two lines
            
            
            //Now onto the second line.
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            // text spacer, text chars, text new Line = 3
            Assert.AreEqual(3, line.Runs.Count);
            Assert.AreEqual(bottom, line.OffsetY);
            
            
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("the inline block, flowing onto a new", chars2.Characters);

            
            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds;
            var xOffset = chars.Width + 20 + 20 + 10 + 10 ; // page margins + nest margin + nest padding + ib margin
            var yOffset = 20 + 50 + 30 + 10; //page margins, h5 + margins, nest margins and padding, ib margin
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(posBlock.Width - 20, bounds.Width); //inline width - margins
            Assert.AreEqual((5 * 15) + 10, bounds.Height); //2 lines + padding (no margins)
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_14_ExplicitWidthAndHeightFloatOverflow()
        {
            var path = AssertGetContentFile("InlineBlock_14_ExplicitWidthAndHeightFloatOverflow");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_14_ExplicitWidthAndHeightFloatOverflow.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);
            

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(0, nest.PositionedRegions.Count); //we should be overflowing

            Assert.AreEqual(2, nest.Columns[0].Contents.Count);
            var push = nest.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(push);
            Assert.AreEqual(660, push.Height);
            
            //line is after
            var line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //The inline block should now
            // text begin, chars, text end
           
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //top aligned image and push the content of the inline block down.
            var bottom = 24; //Just one single 20pt line
           
            Assert.AreEqual(bottom, line.Height);
            
            
            //
            //Second page
            //

            content = layout.AllPages[1].ContentBlock;
            Assert.IsNotNull(content);
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            
            nest = content.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            
            Assert.AreEqual(1, nest.Columns.Length);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            
            line =nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            // inline positioned block , whitespace begin, whitespace chars, whitespace end, inline start, text start, chars, new line = 11
            Assert.AreEqual(8, line.Runs.Count);
            
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            
            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(0, posReg.TotalBounds.X);//moved onto a new page
            Assert.AreEqual(0, posReg.TotalBounds.Y); //moved to a new page

            Assert.AreEqual(200, posReg.Width);
            Assert.AreEqual(100, posReg.Height);
            
            
            Assert.AreEqual(100, posReg.TotalBounds.Height); //5 lines + 5pt padding + 10pt margins
            Assert.AreEqual(1, posReg.Contents.Count);


            
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(posReg.TotalBounds.Height + 20, posBlock.Height);
            Assert.AreEqual(posReg.Width + 20, posBlock.Width); //add margins
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(5, posBlock.Columns[0].Contents.Count); //on 5 lines
            
            //check the rest of the line contents
            
            //bottom alignment
            Assert.AreEqual(posRun.Height, line.Height);
            Assert.AreEqual(posRun.Height, line.BaseLineOffset + line.BaseLineToBottom);

            chars = line.Runs[6] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("After the inline", chars.Characters);
            
            //Now onto the second line.
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            // text spacer, text chars, text new Line = 3
            Assert.AreEqual(3, line.Runs.Count);
            Assert.AreEqual(120, line.OffsetY);
            
            
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("block, flowing onto a new line with", chars2.Characters);

            
            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds;
            var xOffset = 20 + 20 + 10 + 10 ; // page margins + nest margin + nest padding + ib margin
            var yOffset = 20 + 20 + 10 + 10; //page margins, nest margins and padding, ib margin
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(posBlock.Width - 20, bounds.Width); //inline width - margins
            Assert.AreEqual(100, bounds.Height); //inline height
            
        }
        

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_15_ExplicitWidthOnlyFloatOverflow()
        {
            var path = AssertGetContentFile("InlineBlock_15_ExplicitWidthOnlyFloatOverflow");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_15_ExplicitWidthOnlyFloatOverflow.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);
            

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(0, nest.PositionedRegions.Count); //we should be overflowing

            Assert.AreEqual(2, nest.Columns[0].Contents.Count);
            var push = nest.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(push);
            Assert.AreEqual(660, push.Height);
            
            //line is after
            var line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //The inline block should now
            // text begin, chars, text end
           
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //top aligned image and push the content of the inline block down.
            var bottom = 24; //Just one single 20pt line
           
            Assert.AreEqual(bottom, line.Height);
            
            
            //
            //Second page
            //

            content = layout.AllPages[1].ContentBlock;
            Assert.IsNotNull(content);
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            
            nest = content.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            
            Assert.AreEqual(1, nest.Columns.Length);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            
            line =nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            // inline positioned block , whitespace begin, whitespace chars, whitespace end, inline start, text start, chars, new line = 11
            Assert.AreEqual(8, line.Runs.Count);
            
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            
            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(0, posReg.TotalBounds.X);//moved onto a new page
            Assert.AreEqual(0, posReg.TotalBounds.Y); //moved to a new page

            Assert.AreEqual(200, posReg.Width);
            Assert.AreEqual(105, posReg.Height); //5 lines at 15pt (75) + 2 x 10 margins + 2 x 5 padding = 105pt
            
            
            Assert.AreEqual(105, posReg.TotalBounds.Height); //5 lines + 5pt padding + 10pt margins
            Assert.AreEqual(1, posReg.Contents.Count);


            
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(posReg.TotalBounds.Height , posBlock.Height);
            Assert.AreEqual(posReg.Width + 20, posBlock.Width); //add margins
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(5, posBlock.Columns[0].Contents.Count); //on 5 lines
            
            //check the rest of the line contents
            
            //bottom alignment
            Assert.AreEqual(posRun.Height, line.Height);
            Assert.AreEqual(posRun.Height, line.BaseLineOffset + line.BaseLineToBottom);

            chars = line.Runs[6] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("After the inline", chars.Characters);
            
            //Now onto the second line.
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            // text spacer, text chars, text new Line = 3
            Assert.AreEqual(3, line.Runs.Count);
            Assert.AreEqual(105, line.OffsetY);
            
            
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("block, flowing onto a new line with", chars2.Characters);

            
            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds;
            var xOffset = 20 + 20 + 10 + 10 ; // page margins + nest margin + nest padding + ib margin
            var yOffset = 20 + 20 + 10 + 10; //page margins, nest margins and padding, ib margin
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(posBlock.Width - 20, bounds.Width); //inline width - margins
            Assert.AreEqual(85, bounds.Height); //inline height - margins
            
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_16_NoSizeOverflow()
        {
            var path = AssertGetContentFile("InlineBlock_16_NoSizeOverflow");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_16_NoSizeOverflow.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);
            

            var nest = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(0, nest.PositionedRegions.Count); //we should be overflowing

            Assert.AreEqual(2, nest.Columns[0].Contents.Count);
            var push = nest.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(push);
            Assert.AreEqual(660, push.Height);
            
            //line is after
            var line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //The inline block should now
            // text begin, chars, text end
           
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            //top aligned image and push the content of the inline block down.
            var bottom = 24; //Just one single 20pt line
           
            Assert.AreEqual(bottom, line.Height);
            
            
            //
            //Second page
            //

            content = layout.AllPages[1].ContentBlock;
            Assert.IsNotNull(content);
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            
            nest = content.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            
            Assert.AreEqual(1, nest.Columns.Length);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            
            line =nest.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //pos run, whitespace begin, whitespace chars, whitespace end, inline begin, text start, new line = 7
            //no actual text content
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            
            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(0, posReg.TotalBounds.X);//moved onto a new page
            Assert.AreEqual(0, posReg.TotalBounds.Y); //moved to a new page

            Assert.IsTrue(posReg.Width > 400);
            Assert.AreEqual(60, posReg.Height); //2 lines at 15pt (30) + 2 x 10 margins + 2 x 5 padding = 60pt
            
            
            Assert.AreEqual(60, posReg.TotalBounds.Height); //5 lines + 5pt padding + 10pt margins
            Assert.AreEqual(1, posReg.Contents.Count);


            
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(posReg.TotalBounds.Height , posBlock.Height);
            Assert.AreEqual(posReg.Width, posBlock.Width); 
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(2, posBlock.Columns[0].Contents.Count); //on 5 lines
            
            //check the rest of the line contents
            
            //bottom alignment
            Assert.AreEqual(posRun.Height, line.Height);
            Assert.AreEqual(posRun.Height, line.BaseLineOffset + line.BaseLineToBottom);

            //check the line with the inline-block on, has no actual text, just a new line.
            Assert.IsInstanceOfType(line.Runs[5], typeof(PDFTextRunBegin));
            Assert.IsInstanceOfType(line.Runs[6], typeof(PDFTextRunNewLine));
            
           
            
            //Now onto the second line.
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            // text spacer, text chars, text new Line = 3
            Assert.AreEqual(3, line.Runs.Count);
            Assert.AreEqual(60, line.OffsetY);
            
            
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("After the inline block, flowing onto a", chars2.Characters);

            
            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds;
            var xOffset = 20 + 20 + 10 + 10 ; // page margins + nest margin + nest padding + ib margin
            var yOffset = 20 + 20 + 10 + 10; //page margins, nest margins and padding, ib margin
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(posBlock.Width - 20, bounds.Width); //inline width - margins
            Assert.AreEqual(40, bounds.Height); //inline height - margins
            
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_17_ExplicitWidthOnlyFloatOverflowColumn()
        {
            var path = AssertGetContentFile("InlineBlock_17_ExplicitWidthOnlyFloatOverflowColumn");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_17_ExplicitWidthOnlyFloatOverflowColumn.pdf"))
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
            Assert.AreEqual(2, nest.Columns.Length);
            Assert.AreEqual(1, nest.PositionedRegions.Count);

            Assert.AreEqual(3, nest.Columns[0].Contents.Count); //block, line with text, empty line
            var line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //The inline block should not be on this line
            // text begin, chars, text end
           
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("Before the inline block.", chars.Characters);
            
            
           
            Assert.AreEqual(24, line.Height);
            
            line = nest.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(0, line.Runs.Count);
            Assert.AreEqual(0, line.Height);
            Assert.AreEqual(0, line.Width); 
            
            // Second column

            Assert.AreEqual(5, nest.Columns[1].Contents.Count);
            line = nest.Columns[1].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //pos reg 1, whitespace 3, inline start 1, text with chars and new line 3
            Assert.AreEqual(8, line.Runs.Count);
            
            
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            
            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(0, posReg.TotalBounds.X);//moved onto a new line
            Assert.AreEqual(0, posReg.TotalBounds.Y); //offset by the previous line height

            Assert.AreEqual(110, posReg.Width);
            
            //top aligned image and push the content of the inline block down.
            var bottom = (8 * 15) + 10 + 20; //8 lines + padding + margins 
            
            Assert.AreEqual(bottom, posReg.TotalBounds.Height); //5 lines + 5pt padding + 10pt margins
            Assert.AreEqual(1, posReg.Contents.Count);


            
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(posReg.TotalBounds.Height, posBlock.Height);
            Assert.AreEqual(posReg.Width + 20, posBlock.Width); //add margins
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(8, posBlock.Columns[0].Contents.Count); //on two lines
            
            
            //Now onto the second line.
            line = nest.Columns[1].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            // text spacer, text chars, text new Line = 3
            Assert.AreEqual(3, line.Runs.Count);
            Assert.AreEqual(bottom, line.OffsetY);
            
            
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("the inline block,", chars2.Characters);

            
            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds;
            var xOffset = 20 + 20 + 10 + nest.Columns[0].Width + 10 + 10 ; // page margins + nest margin + nest padding + first column + alley  + ib margin
            var yOffset = 20 + 50 + 30 + 10; //page margins, h5 + margins, nest margins and padding, ib margin
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(posBlock.Width - 20, bounds.Width); //inline width - margins
            Assert.AreEqual((8 * 15) + 10, bounds.Height); //8 lines + padding (no margins)
            
        }
        
         [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_18_NoSizeOverflowColumn()
        {
            var path = AssertGetContentFile("InlineBlock_18_NoSizeOverflowColumn");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_18_NoSizeOverflowColumn.pdf"))
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
            Assert.AreEqual(2, nest.Columns.Length);
            Assert.AreEqual(1, nest.PositionedRegions.Count);

            Assert.AreEqual(2, nest.Columns[0].Contents.Count); //block, line with text
            var line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //The inline block should not be on this line
            // text begin, chars, text end
           
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("Before the inline block.", chars.Characters);
            
            
            Assert.AreEqual(24, line.Height);
            
            // Second column

            Assert.AreEqual(6, nest.Columns[1].Contents.Count);
            line = nest.Columns[1].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            //pos reg , inline start and new line 
            Assert.AreEqual(3, line.Runs.Count);
            
            
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            
            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(0, posReg.TotalBounds.X);//moved onto a new line
            Assert.AreEqual(0, posReg.TotalBounds.Y); //offset by the previous line height

            Assert.IsTrue(posReg.Width > 200);
            Assert.IsTrue(posReg.Width <= nest.Columns[1].Width);
            //top aligned image and push the content of the inline block down.
            var bottom = (3 * 15) + 10 + 20; //3 lines + padding + margins 
            
            Assert.AreEqual(bottom, posReg.TotalBounds.Height); //3 lines + 5pt padding + 10pt margins
            Assert.AreEqual(1, posReg.Contents.Count);


            
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(posReg.TotalBounds.Height, posBlock.Height);
            Assert.AreEqual(posReg.Width , posBlock.Width); //add margins
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(3, posBlock.Columns[0].Contents.Count); //on two lines
            
            
            //Now onto the second line.
            line = nest.Columns[1].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            // whitespace 3, inline begin, text start, text chars, text new Line = 7
            Assert.AreEqual(7, line.Runs.Count);
            Assert.AreEqual(bottom, line.OffsetY);
            
            
            var chars2 = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("After the inline", chars2.Characters);

            
            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds;
            var xOffset = 20 + 20 + 10 + nest.Columns[0].Width + 10 + 10 ; // page margins + nest margin + nest padding + first column + alley  + ib margin
            var yOffset = 20 + 50 + 30 + 10; //page margins, h5 + margins, nest margins and padding, ib margin
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(posBlock.Width - 20, bounds.Width); //inline width - margins
            Assert.AreEqual((3 * 15) + 10, bounds.Height); //8 lines + padding (no margins)
            
        }
  
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlock_19_NoSizeFloatOverflow()
        {
            var path = AssertGetContentFile("InlineBlock_19_NoSizeFloatOverflow");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("InlineBlock_19_NoSizeFloatOverflow.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.Inconclusive("Issue with floating blocks inside an inline-block not calculating the actual width of the line.");

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
            // text begin, chars, text end, inline positioned block , whitespace begin, whitespace chars, whitespace end, inline start, text start, chars, new line = 11
           
            Assert.AreEqual(11, line.Runs.Count);

            var chars = line.Runs[1];
            Assert.IsNotNull(chars);
            
            //top aligned image and push the content of the inline block down.
            var bottom = (5 * 15) + 10 + 20; //5 lines + padding + margins 
           
            Assert.AreEqual(bottom, line.Height);
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            
            Assert.ReferenceEquals(posRun.Region, nest.PositionedRegions[0]);
            var posReg = nest.PositionedRegions[0];

            

            Assert.AreEqual(chars.Width, posReg.TotalBounds.X);//moved onto a new line
            Assert.AreEqual(0, posReg.TotalBounds.Y); //offset by the previous line height

            Assert.AreEqual(200, posReg.Width);
            
            
            Assert.AreEqual(bottom, posReg.TotalBounds.Height); //5 lines + 5pt padding + 10pt margins
            Assert.AreEqual(1, posReg.Contents.Count);


            
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(posReg.TotalBounds.Height, posBlock.Height);
            Assert.AreEqual(posReg.Width + 20, posBlock.Width); //add margins
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(5, posBlock.Columns[0].Contents.Count); //on two lines
            
            
            //Now onto the second line.
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            // text spacer, text chars, text new Line = 3
            Assert.AreEqual(3, line.Runs.Count);
            Assert.AreEqual(bottom, line.OffsetY);
            
            
            var chars2 = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars2);
            Assert.AreEqual("the inline block, flowing onto a new", chars2.Characters);

            
            var comp = posReg.Owner as Component;
            Assert.IsNotNull(comp);
            var arrange = comp.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds;
            var xOffset = chars.Width + 20 + 20 + 10 + 10 ; // page margins + nest margin + nest padding + ib margin
            var yOffset = 20 + 50 + 30 + 10; //page margins, h5 + margins, nest margins and padding, ib margin
            
            Assert.AreEqual(xOffset, bounds.X);
            Assert.AreEqual(yOffset, bounds.Y);
            Assert.AreEqual(posBlock.Width - 20, bounds.Width); //inline width - margins
            Assert.AreEqual((5 * 15) + 10, bounds.Height); //2 lines + padding (no margins)
            
        }
    }
}
