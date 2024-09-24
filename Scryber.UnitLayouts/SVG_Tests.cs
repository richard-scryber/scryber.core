﻿using System;
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
        public void SVG_01_Rect()
        {
            var path = AssertGetContentFile("SVG_01_Rect");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_01_Rect.pdf"))
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
            Assert.IsFalse(posRun.RenderAsXObject);
            
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width, svgBlock.TotalBounds.Width);
            Assert.AreEqual(height, svgBlock.TotalBounds.Height);
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(2, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(xOffset, bbox.X);
            Assert.AreEqual(yOffset, bbox.Y);
            Assert.AreEqual(width, bbox.Width);
            Assert.AreEqual(height, bbox.Height);
            
            Assert.IsTrue(rsrc.Registered);
            Assert.IsNull(rsrc.Layout);
            Assert.AreEqual(svgBlock, rsrc.Renderer.Layout);
            Assert.IsNotNull(rsrc.Renderer.RenderReference);
            
            //check the svg rect
            line = svgBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);
            
            var compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);

            var bounds = compRun.TotalBounds;
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(30, arrange.RenderBounds.X);
            Assert.AreEqual(20, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
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
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
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
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
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
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
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
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
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
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
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
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
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
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
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
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
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
        public void SVG_15_EmptyRelativeMarginsSizeNestedBottomRight()
        {
            var path = AssertGetContentFile("SVG_15_EmptyRelativeMarginsSizeNestedBottomRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_15_EmptyRelativeMarginsSizeNestedBottomRight.pdf"))
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
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
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

            yOffset -= 20; //Relative position from bottom up
            xOffset -= 10; //Relative position from right in

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
        public void SVG_16_EmptyAbsoluteMarginsSizeNested()
        {
            var path = AssertGetContentFile("SVG_16_EmptyAbsoluteMarginsSizeNested");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_16_EmptyAbsoluteMarginsSizeNested.pdf"))
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
            Unit height = 0; //run for absolute takes no space
            Unit width = 0; //run for absolute takes no space
            
            Assert.AreEqual(xOffset, posRun.OffsetX); //justify align
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(30, line.Height);
            
            
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);

            width = 120;
            height = 140;
            
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

            xOffset = 30 + 10; //page + padding
            yOffset = 30 + 30 + 30 + 10; //page + outerline + one line of text + padding
            height = 120; //render bounds does not include margins
            width = 100;

            var beforeWidth = Unit.Zero;
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            beforeWidth = chars.Width;

            xOffset += beforeWidth; //without position, inline block is placed as per the line inset.

            xOffset += 10; //svg margins
            yOffset += 10; //svg margins

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
        public void SVG_17_EmptyAbsoluteMarginsSizeNestedTopLeft()
        {
            var path = AssertGetContentFile("SVG_17_EmptyAbsoluteMarginsSizeNestedTopLeft");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_17_EmptyAbsoluteMarginsSizeNestedTopLeft.pdf"))
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
            Unit height = 0; //run for absolute takes no space
            Unit width = 0; //run for absolute takes no space
            
            Assert.AreEqual(xOffset, posRun.OffsetX); //justify align
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(30, line.Height);
            
            
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);

            width = 120;
            height = 140;
            
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

            xOffset = 30 + 10 + 10; //page + position + svg margins
            yOffset = 30 + 30 + 20 + 10; //page + outerline + position + svg margins
            height = 120; //render bounds does not include margins
            width = 100;

            

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
        public void SVG_18_EmptyAbsoluteMarginsSizeNestedBottomRight()
        {
            var path = AssertGetContentFile("SVG_18_EmptyAbsoluteMarginsSizeNestedBottomRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_18_EmptyAbsoluteMarginsSizeNestedBottomRight.pdf"))
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
            Unit height = 0; //run for absolute takes no space
            Unit width = 0; //run for absolute takes no space
            
            Assert.AreEqual(xOffset, posRun.OffsetX); //justify align
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(30, line.Height);
            
            
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);

            width = 120;
            height = 140;
            
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

            var nestBottom = (Unit)( 30 + 30 + 20 + 90); //page margins + outer line height + both padding + 3 lines.
            var nestRight = nest.TotalBounds.Width + 30; //page margins and width
            xOffset = nestRight - (100 + 10 + 30); //right inset by width, right margin and position
            yOffset = nestBottom - (120 + 10 + 20); //bottom inset by height, bottom margin and position
            height = 120; //render bounds does not include margins
            width = 100;

            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); //add top offset
            Assert.AreEqual(Unit.Round(xOffset, 4), Unit.Round(arrange.RenderBounds.X, 4)); //centre align + end of text so LSB - account for double inaccuracy
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
        public void SVG_19_EmptyAbsoluteMarginsSizeNestedTextRight()
        {
            var path = AssertGetContentFile("SVG_19_EmptyAbsoluteMarginsSizeNestedTextRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_19_EmptyAbsoluteMarginsSizeNestedTextRight.pdf"))
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
            Unit height = 0; //run for absolute takes no space
            Unit width = 0; //run for absolute takes no space
            
            Assert.AreEqual(xOffset, posRun.OffsetX); //justify align
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(30, line.Height);
            
            
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);

            width = 120;
            height = 140;
            
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

            xOffset = 30 + 10; //page + padding
            yOffset = 30 + 30 + 30 + 10; //page + outerline + one line of text + padding
            height = 120; //render bounds does not include margins
            width = 100;

            var beforeWidth = Unit.Zero;
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            beforeWidth = chars.Width;

            xOffset += beforeWidth; //without position, inline block is placed as per the line inset.

            xOffset += line.AvailableWidth; // right aligned
            
            xOffset += 10; //svg margins
            yOffset += 10; //svg margins

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); //add top offset
            Assert.AreEqual(Unit.Round(xOffset, 4), Unit.Round(arrange.RenderBounds.X, 4)); //right align - account for double inaccuracy
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
        public void SVG_20_EmptyAbsoluteMarginsSizeNestedTextCentre()
        {
            var path = AssertGetContentFile("SVG_20_EmptyAbsoluteMarginsSizeNestedTextCentre");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_20_EmptyAbsoluteMarginsSizeNestedTextCentre.pdf"))
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
            Unit height = 0; //run for absolute takes no space
            Unit width = 0; //run for absolute takes no space
            
            Assert.AreEqual(xOffset, posRun.OffsetX); //justify align
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(30, line.Height);
            
            
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);

            width = 120;
            height = 140;
            
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

            xOffset = 30 + 10; //page + padding
            yOffset = 30 + 30 + 30 + 10; //page + outerline + one line of text + padding
            height = 120; //render bounds does not include margins
            width = 100;

            var beforeWidth = Unit.Zero;
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            beforeWidth = chars.Width;

            xOffset += beforeWidth; //without position, inline block is placed as per the line inset.

            xOffset += line.AvailableWidth / 2.0; //center aligned
            
            xOffset += 10; //svg margins
            yOffset += 10; //svg margins

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); //add top offset
            Assert.AreEqual(Unit.Round(xOffset, 4), Unit.Round(arrange.RenderBounds.X, 4)); //centre align + end of text so LSB - account for double inaccuracy
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
        public void SVG_21_EmptyFixedMarginsSizeNested()
        {
            var path = AssertGetContentFile("SVG_21_EmptyFixedMarginsSizeNested");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_21_EmptyFixedMarginsSizeNested.pdf"))
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
            Unit height = 0; //run for absolute takes no space
            Unit width = 0; //run for absolute takes no space
            
            Assert.AreEqual(xOffset, posRun.OffsetX); //justify align
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(30, line.Height);
            
            
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);

            width = 120;
            height = 140;
            
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

            xOffset = 30 + 10; //page + padding
            yOffset = 30 + 30 + 30 + 10; //page + outerline + one line of text + padding
            height = 120; //render bounds does not include margins
            width = 100;

            var beforeWidth = Unit.Zero;
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            beforeWidth = chars.Width;

            xOffset += beforeWidth; //without position, inline block is placed as per the line inset.

            xOffset += 10; //svg margins
            yOffset += 10; //svg margins

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            //TODO: Figure out why this is wrong, and the only one that is wrong.
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
        public void SVG_22_EmptyFixedMarginsSizeNestedTopLeft()
        {
            var path = AssertGetContentFile("SVG_22_EmptyFixedMarginsSizeNestedTopLeft");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_22_EmptyFixedMarginsSizeNestedTopLeft.pdf"))
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
            Unit height = 0; //run for absolute takes no space
            Unit width = 0; //run for absolute takes no space
            
            Assert.AreEqual(xOffset, posRun.OffsetX); //justify align
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(30, line.Height);
            
            
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);

            width = 120;
            height = 140;
            
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

            xOffset = 20 + 10; //position + svg margins
            yOffset = 10 + 10; //position + svg margins
            height = 120; //render bounds does not include margins
            width = 100;

            

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
        public void SVG_23_EmptyFixedMarginsSizeNestedBottomRight()
        {
            var path = AssertGetContentFile("SVG_23_EmptyFixedMarginsSizeNestedBottomRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_23_EmptyFixedMarginsSizeNestedBottomRight.pdf"))
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
            Unit height = 0; //run for absolute takes no space
            Unit width = 0; //run for absolute takes no space
            
            Assert.AreEqual(xOffset, posRun.OffsetX); //justify align
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(30, line.Height);
            
            
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);

            width = 120;
            height = 140;
            
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

            xOffset = layout.AllPages[0].Width - (20 + 10 + 100); //right - (position + svg margins + svg width)
            yOffset = layout.AllPages[0].Height - (10 + 10 + 120); //bottom - (position + svg margins + svg height)
            height = 120; //render bounds does not include margins
            width = 100;
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            Assert.AreEqual(Unit.Round(yOffset, 4), Unit.Round(arrange.RenderBounds.Y, 4)); //account for double inaccuracy
            Assert.AreEqual(Unit.Round(xOffset, 4), Unit.Round(arrange.RenderBounds.X, 4)); //account for double inaccuracy
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
        public void SVG_24_EmptyFixedMarginsSizeNestedTextRight()
        {
            var path = AssertGetContentFile("SVG_24_EmptyFixedMarginsSizeNestedTextRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_24_EmptyFixedMarginsSizeNestedTextRight.pdf"))
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
            Unit height = 0; //run for absolute takes no space
            Unit width = 0; //run for absolute takes no space
            
            Assert.AreEqual(xOffset, posRun.OffsetX); //justify align
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(30, line.Height);
            
            
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);

            width = 120;
            height = 140;
            
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

            xOffset = 30 + 10; //page + padding
            yOffset = 30 + 30 + 30 + 10; //page + outerline + one line of text + padding
            height = 120; //render bounds does not include margins
            width = 100;

            var beforeWidth = Unit.Zero;
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            beforeWidth = chars.Width;

            xOffset += beforeWidth; //without position, inline block is placed as per the line inset.
            xOffset += line.AvailableWidth; //right aligned text
            
            xOffset += 10; //svg margins
            yOffset += 10; //svg margins

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            //TODO: Figure out why this is wrong, and the only one that is wrong.
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); //add top offset
            Assert.AreEqual(Unit.Round(xOffset, 4), Unit.Round(arrange.RenderBounds.X, 4)); //right align - account for double inaccuracy
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
        public void SVG_25_EmptyFixedMarginsSizeNestedTextCentre()
        {
            var path = AssertGetContentFile("SVG_25_EmptyFixedMarginsSizeNestedTextCentre");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_25_EmptyFixedMarginsSizeNestedTextCentre.pdf"))
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
            Unit height = 0; //run for absolute takes no space
            Unit width = 0; //run for absolute takes no space
            
            Assert.AreEqual(xOffset, posRun.OffsetX); //justify align
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(30, line.Height);
            
            
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);

            width = 120;
            height = 140;
            
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

            xOffset = 30 + 10; //page + padding
            yOffset = 30 + 30 + 30 + 10; //page + outerline + one line of text + padding
            height = 120; //render bounds does not include margins
            width = 100;

            var beforeWidth = Unit.Zero;
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            beforeWidth = chars.Width;

            xOffset += beforeWidth; //without position, inline block is placed as per the line inset.
            xOffset += line.AvailableWidth / 2; //center alignment
            
            xOffset += 10; //svg margins
            yOffset += 10; //svg margins

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            //TODO: Figure out why this is wrong, and the only one that is wrong.
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); //add top offset
            Assert.AreEqual(Unit.Round(xOffset, 4), Unit.Round(arrange.RenderBounds.X, 4)); //centre align - account for double inaccuracy
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
        public void SVG_26_EmptyBlockMarginsSizeNestedTextCentre()
        {
            var path = AssertGetContentFile("SVG_26_EmptyBlockMarginsSizeNestedTextCentre");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_26_EmptyBlockMarginsSizeNestedTextCentre.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.Inconclusive("Need to sort the render as XObject and then positioning");

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
            Unit height = 0; //run for absolute takes no space
            Unit width = 0; //run for absolute takes no space
            
            Assert.AreEqual(xOffset, posRun.OffsetX); //justify align
            Assert.AreEqual(yOffset, posRun.OffsetY); //svg is sat at the top
            Assert.AreEqual(width, posRun.Width);
            Assert.AreEqual(height, posRun.Height);
            
            
            //As we are middle aligned, then baseline height should be half the height of the SVG
            Assert.AreEqual(30, line.Height);
            
            
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);

            width = 120;
            height = 140;
            
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

            xOffset = 30 + 10; //page + padding
            yOffset = 30 + 30 + 30 + 10; //page + outerline + one line of text + padding
            height = 120; //render bounds does not include margins
            width = 100;

            var beforeWidth = Unit.Zero;
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            beforeWidth = chars.Width;

            xOffset += beforeWidth; //without position, inline block is placed as per the line inset.
            xOffset += line.AvailableWidth / 2; //center alignment
            
            xOffset += 10; //svg margins
            yOffset += 10; //svg margins

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement();

            Assert.IsNotNull(arrange);
            //TODO: Figure out why this is wrong, and the only one that is wrong.
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y); //add top offset
            Assert.AreEqual(Unit.Round(xOffset, 4), Unit.Round(arrange.RenderBounds.X, 4)); //centre align - account for double inaccuracy
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
