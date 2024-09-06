using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using Scryber.Drawing;
using Scryber.PDF.Resources;
using Scryber.Svg.Components;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class SVG_Tests
    {
        const string TestCategoryName = "Layout SVG";

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
            path = System.IO.Path.Combine(path, "../../../Content/HTML/SVG/" + name + ".html");
            path = System.IO.Path.GetFullPath(path);

            if (!System.IO.File.Exists(path))
                Assert.Inconclusive("The path the file " + name + " was not found at " + path);

            return path;
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_01_Empty()
        {
            var path = AssertGetContentFile("SVG_01_Empty");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_01_Empty.pdf"))
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

            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //default size is 300x150
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 150;
            Unit width = 300;
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);

            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            
            var bbox = posRun.RenderBoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(0, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            //check the xObject
            Assert.AreEqual(1, doc.SharedResources.Count);
            var xObj = doc.SharedResources[0] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            Assert.IsTrue(xObj.Registered);
            Assert.AreEqual(posRun, xObj.Layout);
            Assert.IsNotNull(posRun.RenderReference);
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_02_EmptyMargins()
        {
            var path = AssertGetContentFile("SVG_02_EmptyMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_02_EmptyMargins.pdf"))
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

            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //default size is 300x150
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 150 + 20; //height + margins
            Unit width = 300 + 20; //width + margins
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);

            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            
            var bbox = posRun.RenderBoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(0, svgBlock.Columns[0].Contents.Count);

            xOffset += 40; //page and svg margins
            yOffset += 40;
            height -= 20; //render bounds does not include margins
            width -= 20;

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            //check the xObject
            Assert.AreEqual(1, doc.SharedResources.Count);
            var xObj = doc.SharedResources[0] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            Assert.IsTrue(xObj.Registered);
            Assert.AreEqual(posRun, xObj.Layout);
            Assert.IsNotNull(posRun.RenderReference);
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_03_EmptyMarginsSize()
        {
            var path = AssertGetContentFile("SVG_03_EmptyMarginsSize");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_03_EmptyMarginsSize.pdf"))
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

            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //default size is 300x150
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120 + 20; //height + margins
            Unit width = 100 + 20; //width + margins
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);

            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            
            var bbox = posRun.RenderBoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(0, svgBlock.Columns[0].Contents.Count);

            xOffset += 40; //page and svg margins
            yOffset += 40;
            height -= 20; //render bounds does not include margins
            width -= 20;

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            //check the xObject
            Assert.AreEqual(1, doc.SharedResources.Count);
            var xObj = doc.SharedResources[0] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            Assert.IsTrue(xObj.Registered);
            Assert.AreEqual(posRun, xObj.Layout);
            Assert.IsNotNull(posRun.RenderReference);
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_04_EmptyMarginsSizeInline()
        {
            var path = AssertGetContentFile("SVG_04_EmptyMarginsSizeInLine");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_04_EmptyMarginsSizeInline.pdf"))
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
            Assert.AreEqual(3, content.Contents.Count);
            
            //first line is text
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //second line has the text, svg and more text
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);

            var begin = line.Runs[0] as PDFTextRunBegin;
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //positioned svg should be on 
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //default size is 300x150
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120 + 20; //height + margins
            Unit width = 100 + 20; //width + margins
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY); //currently svg is sat the baseline
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //We should be on the baseline of the line too.
            Assert.AreEqual(height, line.BaseLineOffset);

            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            
            var bbox = posRun.RenderBoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(0, svgBlock.Columns[0].Contents.Count);

            xOffset = 30 + chars.Width + 10; //page + chars + svg margins
            yOffset = 30 + 30 + 10; //page + line + svg margins
            height = 120; //render bounds does not include margins
            width = 100;

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            //check the xObject - second after the font
            Assert.AreEqual(2, doc.SharedResources.Count);
            var xObj = doc.SharedResources[1] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            Assert.IsTrue(xObj.Registered);
            Assert.AreEqual(posRun, xObj.Layout);
            Assert.IsNotNull(posRun.RenderReference);
            
            
            
        }

        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_05_EmptyMarginsSizeInlineTop()
        {
            var path = AssertGetContentFile("SVG_05_EmptyMarginsSizeInLineTop");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_05_EmptyMarginsSizeInlineTop.pdf"))
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
            Assert.AreEqual(3, content.Contents.Count);
            
            //first line is text
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var baseline = line.BaseLineOffset;
            
            //second line has the text, svg and more text
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);

            var begin = line.Runs[0] as PDFTextRunBegin;
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //positioned svg should be on 
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //default size is 300x150
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120 + 20; //height + margins
            Unit width = 100 + 20; //width + margins
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(line.BaseLineOffset - height, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are top aligned, then the standard baseline should still be valid.
            Assert.AreEqual(baseline, line.BaseLineOffset);

            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            
            var bbox = posRun.RenderBoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(0, svgBlock.Columns[0].Contents.Count);

            xOffset = 30 + chars.Width + 10; //page + chars + svg margins
            yOffset = 30 + 30 + 10; //page + line + svg margins
            height = 120; //render bounds does not include margins
            width = 100;

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            //check the xObject - second after the font
            Assert.AreEqual(2, doc.SharedResources.Count);
            var xObj = doc.SharedResources[1] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            Assert.IsTrue(xObj.Registered);
            Assert.AreEqual(posRun, xObj.Layout);
            Assert.IsNotNull(posRun.RenderReference);
            
            
            //check the last line is down below the svg (inc. margins).
            var nl = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(nl);
            Assert.AreEqual(height + 20, nl.NewLineOffset.Height); 

        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_06_EmptyMarginsSizeInlineBottom()
        {
            var path = AssertGetContentFile("SVG_06_EmptyMarginsSizeInLineBottom");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_06_EmptyMarginsSizeInlineBottom.pdf"))
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
            Assert.AreEqual(3, content.Contents.Count);
            
            //first line is text
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var baseline = line.BaseLineOffset;
            
            //second line has the text, svg and more text
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);

            var begin = line.Runs[0] as PDFTextRunBegin;
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //positioned svg should be on 
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //default size is 300x150
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120 + 20; //height + margins
            Unit width = 100 + 20; //width + margins
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(line.BaseLineOffset - height, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are bottom aligned, then total height should be the height of the SVG
            Assert.AreEqual(height, line.Height);

            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            
            var bbox = posRun.RenderBoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(0, svgBlock.Columns[0].Contents.Count);

            xOffset = 30 + chars.Width + 10; //page + chars + svg margins
            yOffset = 30 + 30 + 10; //page + line + svg margins
            height = 120; //render bounds does not include margins
            width = 100;

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            //check the xObject - second after the font
            Assert.AreEqual(2, doc.SharedResources.Count);
            var xObj = doc.SharedResources[1] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            Assert.IsTrue(xObj.Registered);
            Assert.AreEqual(posRun, xObj.Layout);
            Assert.IsNotNull(posRun.RenderReference);
            
            
            //check the last line is standard height
            var nl = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(nl);
            Assert.AreEqual(30, nl.NewLineOffset.Height); 
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_07_EmptyMarginsSizeInlineMiddle()
        {
            var path = AssertGetContentFile("SVG_07_EmptyMarginsSizeInLineMiddle");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_07_EmptyMarginsSizeInlineMiddle.pdf"))
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
            Assert.AreEqual(3, content.Contents.Count);
            
            //first line is text
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var baseline = line.BaseLineOffset;
            
            //second line has the text, svg and more text
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);

            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //positioned svg should be on 
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //default size is 300x150
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120 + 20; //height + margins
            Unit width = 100 + 20; //width + margins
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(line.BaseLineOffset - height, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(height, line.Height);
            var mid = (height) / 2;
            Assert.AreEqual(mid, line.BaseLineOffset);
            
            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            
            var bbox = posRun.RenderBoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(0, svgBlock.Columns[0].Contents.Count);

            xOffset = 30 + chars.Width + 10; //page + chars + svg margins
            yOffset = 30 + 30 + 10; //page + line + svg margins
            height = 120; //render bounds does not include margins
            width = 100;

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            //check the xObject - second after the font
            Assert.AreEqual(2, doc.SharedResources.Count);
            var xObj = doc.SharedResources[1] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            Assert.IsTrue(xObj.Registered);
            Assert.AreEqual(posRun, xObj.Layout);
            Assert.IsNotNull(posRun.RenderReference);
            
            
            //check the last line is standard height
            var nl = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(nl);
            //half the top line height + half space around the line + font ascent = baseline offset.
            Assert.AreEqual(mid + 5 + begin.TextRenderOptions.GetAscent(), nl.NewLineOffset.Height); 
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_08_EmptyMarginsSizeInlineMiddleRight()
        {
            var path = AssertGetContentFile("SVG_08_EmptyMarginsSizeInLineMiddleRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_08_EmptyMarginsSizeInlineMiddleRight.pdf"))
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
            Assert.AreEqual(3, content.Contents.Count);
            
            //first line is text
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var baseline = line.BaseLineOffset;
            
            //second line has the text, svg and more text
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);

            var begin = line.Runs[0] as PDFTextRunBegin;
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //positioned svg should be on 
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //default size is 300x150
            
            Unit yOffset = 0;
            Unit xOffset = 0; 
            Unit height = 120 + 20; //height + margins
            Unit width = 100 + 20; //width + margins
            
            Assert.AreEqual(xOffset + line.AvailableWidth, posRun.OffsetX); //right align
            Assert.AreEqual(line.BaseLineOffset - height, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(height, line.Height);
            var mid = (height) / 2;
            Assert.AreEqual(mid, line.BaseLineOffset);
            
            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            
            var bbox = posRun.RenderBoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(0, svgBlock.Columns[0].Contents.Count);

            xOffset = 30 + chars.Width + 10; //page + chars + svg margins
            yOffset = 30 + 30 + 10; //page + line + svg margins
            height = 120; //render bounds does not include margins
            width = 100;

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(Unit.Round(xOffset + line.AvailableWidth, 5), Unit.Round(arrange.RenderBounds.X, 5)); //right align + end of text so LSB
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            //check the xObject - second after the font
            Assert.AreEqual(2, doc.SharedResources.Count);
            var xObj = doc.SharedResources[1] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            Assert.IsTrue(xObj.Registered);
            Assert.AreEqual(posRun, xObj.Layout);
            Assert.IsNotNull(posRun.RenderReference);
            
            
            //check the last line is standard height
            var nl = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(nl);
            //half the top line height + half space around the line + font ascent = baseline offset.
            Assert.AreEqual(mid + 5 + begin.TextRenderOptions.GetAscent(), nl.NewLineOffset.Height); 
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_09_EmptyMarginsSizeInlineMiddleCenter()
        {
            var path = AssertGetContentFile("SVG_09_EmptyMarginsSizeInLineMiddleCenter");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_09_EmptyMarginsSizeInlineMiddleCenter.pdf"))
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
            Assert.AreEqual(3, content.Contents.Count);
            
            //first line is text
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var baseline = line.BaseLineOffset;
            
            //second line has the text, svg and more text
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);

            var begin = line.Runs[0] as PDFTextRunBegin;
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //positioned svg should be on 
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //default size is 300x150
            
            Unit yOffset = 0;
            Unit xOffset = 0; 
            Unit height = 120 + 20; //height + margins
            Unit width = 100 + 20; //width + margins
            
            Assert.AreEqual(xOffset + (line.AvailableWidth / 2), posRun.OffsetX); //centre align
            Assert.AreEqual(line.BaseLineOffset - height, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(height, line.Height);
            var mid = (height) / 2;
            Assert.AreEqual(mid, line.BaseLineOffset);
            
            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            
            var bbox = posRun.RenderBoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(0, svgBlock.Columns[0].Contents.Count);

            xOffset = 30 + chars.Width + 10; //page + chars + svg margins
            yOffset = 30 + 30 + 10; //page + line + svg margins
            height = 120; //render bounds does not include margins
            width = 100;

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(Unit.Round(xOffset + (line.AvailableWidth / 2), 5), Unit.Round(arrange.RenderBounds.X, 5)); //centre align + end of text so LSB - account for double inaccuracy
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            //check the xObject - second after the font
            Assert.AreEqual(2, doc.SharedResources.Count);
            var xObj = doc.SharedResources[1] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            Assert.IsTrue(xObj.Registered);
            Assert.AreEqual(posRun, xObj.Layout);
            Assert.IsNotNull(posRun.RenderReference);
            
            
            //check the last line is standard height
            var nl = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(nl);
            //half the top line height + half space around the line + font ascent = baseline offset.
            Assert.AreEqual(mid + 5 + begin.TextRenderOptions.GetAscent(), nl.NewLineOffset.Height); 
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_10_EmptyMarginsSizeInlineMiddleJustify()
        {
            var path = AssertGetContentFile("SVG_10_EmptyMarginsSizeInLineMiddleJustify");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_10_EmptyMarginsSizeInlineMiddleJustify.pdf"))
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
            Assert.AreEqual(3, content.Contents.Count);
            
            //first line is text
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var baseline = line.BaseLineOffset;
            
            //second line has the text, svg and more text
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);

            var begin = line.Runs[0] as PDFTextRunBegin;
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //positioned svg should be on 
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            
            

            //default size is 300x150
            
            Unit yOffset = 0;
            Unit xOffset = 0; 
            Unit height = 120 + 20; //height + margins
            Unit width = 100 + 20; //width + margins
            
            Assert.AreEqual(xOffset, posRun.OffsetX); //justify align
            Assert.AreEqual(line.BaseLineOffset - height, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(height, line.Height);
            var mid = (height) / 2;
            Assert.AreEqual(mid, line.BaseLineOffset);
            
            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            
            var bbox = posRun.RenderBoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(0, svgBlock.Columns[0].Contents.Count);

            xOffset = 30 + chars.Width + chars.ExtraSpace + 10; //page + chars + svg margins
            yOffset = 30 + 30 + 10; //page + line + svg margins
            height = 120; //render bounds does not include margins
            width = 100;

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(Unit.Round(xOffset, 5), Unit.Round(arrange.RenderBounds.X, 5)); //centre align + end of text so LSB - account for double inaccuracy
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            //check the xObject - second after the font
            Assert.AreEqual(2, doc.SharedResources.Count);
            var xObj = doc.SharedResources[1] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            Assert.IsTrue(xObj.Registered);
            Assert.AreEqual(posRun, xObj.Layout);
            Assert.IsNotNull(posRun.RenderReference);
            
            
            //check the last line is standard height
            var nl = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(nl);
            //half the top line height + half space around the line + font ascent = baseline offset.
            Assert.AreEqual(mid + 5 + begin.TextRenderOptions.GetAscent(), nl.NewLineOffset.Height); 
        }
        
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_11_EmptyRelativeMarginsSizeInline()
        {
            var path = AssertGetContentFile("SVG_11_EmptyRelativeMarginsSizeInline");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_11_EmptyRelativeMarginsSizeInline.pdf"))
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
            Assert.AreEqual(3, content.Contents.Count);
            
            //first line is text
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var baseline = line.BaseLineOffset;
            
            //second line has the text, svg and more text
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);

            var begin = line.Runs[0] as PDFTextRunBegin;
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //positioned svg should be on 
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            
            

            //default size is 300x150
            
            Unit yOffset = 0;
            Unit xOffset = 0; 
            Unit height = 120 + 20; //height + margins
            Unit width = 100 + 20; //width + margins
            
            Assert.AreEqual(xOffset, posRun.OffsetX); //justify align
            Assert.AreEqual(line.BaseLineOffset - height, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(height, line.Height);
            var mid = (height) / 2;
            Assert.AreEqual(mid, line.BaseLineOffset);
            
            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            
            var bbox = posRun.RenderBoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(0, svgBlock.Columns[0].Contents.Count);

            xOffset = 30 + chars.Width + chars.ExtraSpace + 10; //page + chars + svg margins
            yOffset = 30 + 30 + 10; //page + line + svg margins
            height = 120; //render bounds does not include margins
            width = 100;

            yOffset += 50; //Relative position
            xOffset -= 50; //Relative position

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); //add top offset
            Assert.AreEqual(Unit.Round(xOffset, 5), Unit.Round(arrange.RenderBounds.X, 5)); //centre align + end of text so LSB - account for double inaccuracy
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            //check the xObject - second after the font
            Assert.AreEqual(2, doc.SharedResources.Count);
            var xObj = doc.SharedResources[1] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            Assert.IsTrue(xObj.Registered);
            Assert.AreEqual(posRun, xObj.Layout);
            Assert.IsNotNull(posRun.RenderReference);
            
            
            //check the last line is standard height
            var nl = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(nl);
            //half the top line height + half space around the line + font ascent = baseline offset.
            Assert.AreEqual(mid + 5 + begin.TextRenderOptions.GetAscent(), nl.NewLineOffset.Height); 
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_12_EmptyAbsoluteMarginsSizeInline()
        {
            var path = AssertGetContentFile("SVG_12_EmptyAbsoluteMarginsSizeInline");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_12_EmptyAbsoluteMarginsSizeInline.pdf"))
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
            Assert.AreEqual(3, content.Contents.Count);
            
            //first line is text
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var baseline = line.BaseLineOffset;
            
            //second line has the text, svg and more text
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);

            var begin = line.Runs[0] as PDFTextRunBegin;
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //positioned svg should be on run 3
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            
            
            
            Unit yOffset = 0; //absolute so no affect within the line - positioned independently.
            Unit xOffset = 0; //absolute so no affect within the line - positioned independently.
            Unit height = 0; //absolute so no affect within the line - positioned independently.
            Unit width = 0; //absolute so no affect within the line - positioned independently.
            
            Assert.AreEqual(xOffset, posRun.OffsetX); 
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);

            height = 30; //absolute so line should be as normal
            Assert.AreEqual(height, line.Height);

            
            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);

            width = 100 + 20; //width + margins
            height = 120 + 20; //height + margins
            
            var bbox = posRun.RenderBoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(0, svgBlock.Columns[0].Contents.Count);

            xOffset = 10; //absolute
            yOffset = 20; //absolute
            height = 120; //render bounds does not include margins
            width = 100;

            yOffset += 10; // + margins
            xOffset += 10; // + margins

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); //add top offset
            Assert.AreEqual(Unit.Round(xOffset, 5), Unit.Round(arrange.RenderBounds.X, 5)); //centre align + end of text so LSB - account for double inaccuracy
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            //check the xObject - second after the font
            Assert.AreEqual(2, doc.SharedResources.Count);
            var xObj = doc.SharedResources[1] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            Assert.IsTrue(xObj.Registered);
            Assert.AreEqual(posRun, xObj.Layout);
            Assert.IsNotNull(posRun.RenderReference);
            
            
            //check the last line is standard height
            var nl = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(nl);
            //half the top line height + half space around the line + font ascent = baseline offset.
            Assert.AreEqual(30, nl.NewLineOffset.Height); 
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_13_EmptyFixedMarginsSizeInline()
        {
            var path = AssertGetContentFile("SVG_13_EmptyFixedMarginsSizeInline");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_13_EmptyFixedMarginsSizeInline.pdf"))
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
            Assert.AreEqual(3, content.Contents.Count);
            
            //first line is text
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var baseline = line.BaseLineOffset;
            
            //second line has the text, svg and more text
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);

            var begin = line.Runs[0] as PDFTextRunBegin;
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //positioned svg should be on run 3
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            
            
            
            Unit yOffset = 0; //absolute so no affect within the line - positioned independently.
            Unit xOffset = 0; //absolute so no affect within the line - positioned independently.
            Unit height = 0; //absolute so no affect within the line - positioned independently.
            Unit width = 0; //absolute so no affect within the line - positioned independently.
            
            Assert.AreEqual(xOffset, posRun.OffsetX); 
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);

            height = 30; //absolute so line should be as normal
            Assert.AreEqual(height, line.Height);

            
            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);

            width = 100 + 20; //width + margins
            height = 120 + 20; //height + margins
            
            var bbox = posRun.RenderBoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(0, svgBlock.Columns[0].Contents.Count);

            xOffset = 10; //absolute
            yOffset = 20; //absolute
            height = 120; //render bounds does not include margins
            width = 100;

            yOffset += 10; // + margins
            xOffset += 10; // + margins

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); //add top offset
            Assert.AreEqual(Unit.Round(xOffset, 5), Unit.Round(arrange.RenderBounds.X, 5)); //centre align + end of text so LSB - account for double inaccuracy
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            //check the xObject - second after the font
            Assert.AreEqual(2, doc.SharedResources.Count);
            var xObj = doc.SharedResources[1] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            Assert.IsTrue(xObj.Registered);
            Assert.AreEqual(posRun, xObj.Layout);
            Assert.IsNotNull(posRun.RenderReference);
            
            
            //check the last line is standard height
            var nl = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(nl);
            //half the top line height + half space around the line + font ascent = baseline offset.
            Assert.AreEqual(30, nl.NewLineOffset.Height); 
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_14_EmptyRelativeMarginsSizeNested()
        {
            var path = AssertGetContentFile("SVG_14_EmptyRelativeMarginsSizeNested");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_14_EmptyRelativeMarginsSizeNested.pdf"))
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
            Assert.AreEqual(3, content.Contents.Count);
            
            //nestng content
            var nest = layout.AllPages[0].ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);

            content = nest.Columns[0];
            Assert.IsNotNull(content);
            Assert.AreEqual(3, content.Contents.Count);
            
            
            
            //first line is text
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var baseline = line.BaseLineOffset;
            
            //second line has the text, svg and more text
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);

            var begin = line.Runs[0] as PDFTextRunBegin;
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //positioned svg should be on 
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            
            

            //default size is 300x150
            
            Unit yOffset = 0;
            Unit xOffset = 0; 
            Unit height = 120 + 20; //height + margins
            Unit width = 100 + 20; //width + margins
            
            Assert.AreEqual(xOffset, posRun.OffsetX); //justify align
            Assert.AreEqual(line.BaseLineOffset - height, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(height, line.Height);
            var mid = (height) / 2;
            Assert.AreEqual(mid, line.BaseLineOffset);
            
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            
            var bbox = posRun.RenderBoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(0, svgBlock.Columns[0].Contents.Count);

            xOffset = 30 + 10 + chars.Width + chars.ExtraSpace  + 10; //page + nest padding + chars + svg margins
            yOffset = 30 + 30 + 10 + 30 + 10; //page + outerline + padding + line + svg margins
            height = 120; //render bounds does not include margins
            width = 100;

            yOffset += 50; //Relative position
            xOffset -= 50; //Relative position

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); //add top offset
            Assert.AreEqual(Unit.Round(xOffset, 5), Unit.Round(arrange.RenderBounds.X, 5)); //centre align + end of text so LSB - account for double inaccuracy
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            //check the xObject - second after the font
            Assert.AreEqual(2, doc.SharedResources.Count);
            var xObj = doc.SharedResources[1] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            Assert.IsTrue(xObj.Registered);
            Assert.AreEqual(posRun, xObj.Layout);
            Assert.IsNotNull(posRun.RenderReference);
            
            
            //check the last line is standard height
            var nl = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(nl);
            //half the top line height + half space around the line + font ascent = baseline offset.
            Assert.AreEqual(mid + 5 + begin.TextRenderOptions.GetAscent(), nl.NewLineOffset.Height); 
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_15_EmptyAbsoluteMarginsSizeNested()
        {
            var path = AssertGetContentFile("SVG_15_EmptyAbsoluteMarginsSizeNested");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_15_EmptyAbsoluteMarginsSizeNested.pdf"))
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
            Assert.AreEqual(3, content.Contents.Count);
            
            //first line is text
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var baseline = line.BaseLineOffset;
            
            //second line has the text, svg and more text
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);

            var begin = line.Runs[0] as PDFTextRunBegin;
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //positioned svg should be on run 3
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            
            
            
            Unit yOffset = 0; //absolute so no affect within the line - positioned independently.
            Unit xOffset = 0; //absolute so no affect within the line - positioned independently.
            Unit height = 0; //absolute so no affect within the line - positioned independently.
            Unit width = 0; //absolute so no affect within the line - positioned independently.
            
            Assert.AreEqual(xOffset, posRun.OffsetX); 
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);

            height = 30; //absolute so line should be as normal
            Assert.AreEqual(height, line.Height);

            
            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);

            width = 100 + 20; //width + margins
            height = 120 + 20; //height + margins
            
            var bbox = posRun.RenderBoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(0, svgBlock.Columns[0].Contents.Count);

            xOffset = 10; //absolute
            yOffset = 20; //absolute
            height = 120; //render bounds does not include margins
            width = 100;

            yOffset += 10; // + margins
            xOffset += 10; // + margins

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); //add top offset
            Assert.AreEqual(Unit.Round(xOffset, 5), Unit.Round(arrange.RenderBounds.X, 5)); //centre align + end of text so LSB - account for double inaccuracy
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            //check the xObject - second after the font
            Assert.AreEqual(2, doc.SharedResources.Count);
            var xObj = doc.SharedResources[1] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            Assert.IsTrue(xObj.Registered);
            Assert.AreEqual(posRun, xObj.Layout);
            Assert.IsNotNull(posRun.RenderReference);
            
            
            //check the last line is standard height
            var nl = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(nl);
            //half the top line height + half space around the line + font ascent = baseline offset.
            Assert.AreEqual(30, nl.NewLineOffset.Height); 
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_16_EmptyFixedMarginsSizeNested()
        {
            var path = AssertGetContentFile("SVG_16_EmptyFixedMarginsSizeNested");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_16_EmptyFixedMarginsSizeNested.pdf"))
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
            Assert.AreEqual(3, content.Contents.Count);
            
            //first line is text
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var baseline = line.BaseLineOffset;
            
            //second line has the text, svg and more text
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);

            var begin = line.Runs[0] as PDFTextRunBegin;
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //positioned svg should be on run 3
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            
            
            
            Unit yOffset = 0; //absolute so no affect within the line - positioned independently.
            Unit xOffset = 0; //absolute so no affect within the line - positioned independently.
            Unit height = 0; //absolute so no affect within the line - positioned independently.
            Unit width = 0; //absolute so no affect within the line - positioned independently.
            
            Assert.AreEqual(xOffset, posRun.OffsetX); 
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);

            height = 30; //absolute so line should be as normal
            Assert.AreEqual(height, line.Height);

            
            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);

            width = 100 + 20; //width + margins
            height = 120 + 20; //height + margins
            
            var bbox = posRun.RenderBoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(0, svgBlock.Columns[0].Contents.Count);

            xOffset = 10; //absolute
            yOffset = 20; //absolute
            height = 120; //render bounds does not include margins
            width = 100;

            yOffset += 10; // + margins
            xOffset += 10; // + margins

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); //add top offset
            Assert.AreEqual(Unit.Round(xOffset, 5), Unit.Round(arrange.RenderBounds.X, 5)); //centre align + end of text so LSB - account for double inaccuracy
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            //check the xObject - second after the font
            Assert.AreEqual(2, doc.SharedResources.Count);
            var xObj = doc.SharedResources[1] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            Assert.IsTrue(xObj.Registered);
            Assert.AreEqual(posRun, xObj.Layout);
            Assert.IsNotNull(posRun.RenderReference);
            
            
            //check the last line is standard height
            var nl = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(nl);
            //half the top line height + half space around the line + font ascent = baseline offset.
            Assert.AreEqual(30, nl.NewLineOffset.Height); 
        }
    }
}
