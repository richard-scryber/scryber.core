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
        const string TestCategoryName = "SVG Layout";

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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
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
        public void SVG_02_RectMargins()
        {
            var path = AssertGetContentFile("SVG_02_RectMargins");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_02_RectMargins.pdf"))
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
            Assert.AreEqual(width + 20, posRun.Width); //inc. margins
            Assert.AreEqual(height + 20, posRun.Height); //inc. margins

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
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            yOffset = 30 + 10;
            xOffset = 30 + 10;
            
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
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset = 30 + 10 + 30; //Margins + x pos
            yOffset = 30 + 10 + 20; //Margins + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_03_RectMarginsSize()
        {
            var path = AssertGetContentFile("SVG_03_RectMarginsSize");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_03_RectMarginsSize.pdf"))
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
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width + 20, posRun.Width); //inc. margins
            Assert.AreEqual(height + 20, posRun.Height); //inc. margins

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
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            yOffset = 30 + 10;
            xOffset = 30 + 10;
            
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
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset = 30 + 10 + 30; //Margins + x pos
            yOffset = 30 + 10 + 20; //Margins + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_04_RectMarginsSizeInline()
        {
            var path = AssertGetContentFile("SVG_04_RectMarginsSizeInLine");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_04_RectMarginsSizeInline.pdf"))
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

            //Simple first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //Second line
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width + 20, posRun.Width); //inc. margins
            Assert.AreEqual(height + 20, posRun.Height); //inc. margins

            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            yOffset += 30 + 10; //add margins for render bounds
            xOffset = 30 + 10 + chars.Width; //add margins and chars for render bounds
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
            
            
            
        }

        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_05_RectMarginsSizeInlineTop()
        {
            var path = AssertGetContentFile("SVG_05_RectMarginsSizeInLineTop");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_05_RectMarginsSizeInlineTop.pdf"))
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

            //Simple first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //Second line
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width + 20, posRun.Width); //inc. margins
            Assert.AreEqual(height + 20, posRun.Height); //inc. margins

            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            yOffset += 30 + 10; //add margins for render bounds
            xOffset = 30 + 10 + chars.Width; //add margins and chars for render bounds
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);

        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_06_RectMarginsSizeInlineBottom()
        {
            var path = AssertGetContentFile("SVG_06_RectMarginsSizeInLineBottom");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_06_RectMarginsSizeInlineBottom.pdf"))
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

            //Simple first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //Second line
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width + 20, posRun.Width); //inc. margins
            Assert.AreEqual(height + 20, posRun.Height); //inc. margins

            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            yOffset += 30 + 10; //add margins for render bounds
            xOffset = 30 + 10 + chars.Width; //add margins and chars for render bounds
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);

            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_07_RectMarginsSizeInlineMiddle()
        {
            var path = AssertGetContentFile("SVG_07_RectMarginsSizeInLineMiddle");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_07_RectMarginsSizeInlineMiddle.pdf"))
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

            //Simple first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //Second line
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width + 20, posRun.Width); //inc. margins
            Assert.AreEqual(height + 20, posRun.Height); //inc. margins

            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            yOffset += 30 + 10; //add margins for render bounds
            xOffset = 30 + 10 + chars.Width; //add margins and chars for render bounds
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_08_RectMarginsSizeInlineMiddleRight()
        {
            var path = AssertGetContentFile("SVG_08_RectMarginsSizeInLineMiddleRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_08_RectMarginsSizeInlineMiddleRight.pdf"))
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

            //Simple first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //Second line
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            xOffset += line.FullWidth - line.Width; //right align inc. margins
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width + 20, posRun.Width); //inc. margins
            Assert.AreEqual(height + 20, posRun.Height); //inc. margins

            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            yOffset += 30 + 10; //add margins for render bounds
            xOffset = 30 + 10 + chars.Width + (line.FullWidth - line.Width); //add margins and chars for render bounds along with right insed
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_09_RectMarginsSizeInlineMiddleCenter()
        {
            var path = AssertGetContentFile("SVG_09_RectMarginsSizeInLineMiddleCenter");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_09_RectMarginsSizeInlineMiddleCenter.pdf"))
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

            //Simple first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //Second line
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            xOffset += (line.FullWidth - line.Width) / 2.0; //right align inc. margins
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width + 20, posRun.Width); //inc. margins
            Assert.AreEqual(height + 20, posRun.Height); //inc. margins

            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            yOffset += 30 + 10; //add margins for render bounds
            xOffset = 30 + 10 + chars.Width + ((line.FullWidth - line.Width) / 2.0); //add margins and chars for render bounds along with centre inset
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_10_RectMarginsSizeInlineMiddleJustify()
        {
            var path = AssertGetContentFile("SVG_10_RectMarginsSizeInLineMiddleJustify");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_10_RectMarginsSizeInlineMiddleJustify.pdf"))
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

            //Simple first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //Second line
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width + 20, posRun.Width); //inc. margins
            Assert.AreEqual(height + 20, posRun.Height); //inc. margins

            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            yOffset += 30 + 10; //add margins for render bounds
            xOffset = 30 + 10 + chars.Width + chars.ExtraSpace; //add margins and chars for render bounds along with justification
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_11_RectRelativeMarginsSizeInline()
        {
            var path = AssertGetContentFile("SVG_11_RectRelativeMarginsSizeInline");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_11_RectRelativeMarginsSizeInline.pdf"))
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

            //Simple first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //Second line
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width + 20, posRun.Width); //inc. margins
            Assert.AreEqual(height + 20, posRun.Height); //inc. margins

            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            yOffset += 30 + 10; //add margins for render bounds
            xOffset = 30 + 10 + chars.Width + chars.ExtraSpace; //add margins and chars for render bounds along with justification

            yOffset += 30; //relative position
            xOffset -= 30; //relative position
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_12_RectAbsoluteMarginsSizeInline()
        {
            var path = AssertGetContentFile("SVG_12_RectAbsoluteMarginsSizeInline");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_12_RectAbsoluteMarginsSizeInline.pdf"))
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

            //Simple first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //Second line
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(0, posRun.Width); //absolute so run has zero size
            Assert.AreEqual(0, posRun.Height); //absolute so run has zero size

            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(0, svgBlock.TotalBounds.Y);
            Assert.AreEqual(0, svgBlock.TotalBounds.X);
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30; //absolute position
            xOffset = 20; //absolute position
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_13_RectFixedMarginsSizeInline()
        {
            var path = AssertGetContentFile("SVG_13_RectFixedMarginsSizeInline");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_13_RectFixedMarginsSizeInline.pdf"))
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

            //Simple first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //Second line
            line = content.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(0, posRun.Width); //absolute so run has zero size
            Assert.AreEqual(0, posRun.Height); //absolute so run has zero size

            Assert.IsTrue(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.AreEqual(1, layout.AllPages[0].ContentBlock.PositionedRegions.Count);
            var pos = layout.AllPages[0].ContentBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(0, svgBlock.TotalBounds.Y);
            Assert.AreEqual(0, svgBlock.TotalBounds.X);
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 40; //absolute position + margins
            xOffset = 30; //absolute position + margins
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_14_RectRelativeMarginsSizeNested()
        {
            var path = AssertGetContentFile("SVG_14_RectRelativeMarginsSizeNested");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_14_RectRelativeMarginsSizeNested.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width + 20, posRun.Width); //inc. margins
            Assert.AreEqual(height + 20, posRun.Height); //inc. margins

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 110 + 10; //relative position
            xOffset = chars.Width + 30 + 10 + 10 - 10; //relative position
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_15_RectRelativeMarginsSizeNestedBottomRight()
        {
            var path = AssertGetContentFile("SVG_15_RectRelativeMarginsSizeNestedBottomRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_15_RectRelativeMarginsSizeNestedBottomRight.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(width + 20, posRun.Width); //inc. margins
            Assert.AreEqual(height + 20, posRun.Height); //inc. margins

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 110 - 20; //relative position
            xOffset = chars.Width + 30 + 10 + 10 - 10; //relative position
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_16_RectAbsoluteMarginsSizeNested()
        {
            var path = AssertGetContentFile("SVG_16_RectAbsoluteMarginsSizeNested");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_16_RectAbsoluteMarginsSizeNested.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(0, posRun.Width); //absolute - so no size in the line
            Assert.AreEqual(0, posRun.Height); //absolute - so no size in the line

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(0, svgBlock.TotalBounds.Y); //absolute
            Assert.AreEqual(0, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30 + 30 + 10 + 60 + 10; //absolute position - page margins, outer line, padding, 3rd line offset in nest + svg margins
            xOffset = 30 + 10 + 10; //absolute position - page margins + padding + svg margins
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_17_RectAbsoluteMarginsSizeNestedTopLeft()
        {
            var path = AssertGetContentFile("SVG_17_RectAbsoluteMarginsSizeNestedTopLeft");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_17_RectAbsoluteMarginsSizeNestedTopLeft.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(0, posRun.Width); //absolute - so no size in the line
            Assert.AreEqual(0, posRun.Height); //absolute - so no size in the line

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(0, svgBlock.TotalBounds.Y); //absolute
            Assert.AreEqual(0, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30 + 30 + 20 + 10; //absolute position - page margins, outer line, top pos, + svg margins
            xOffset = 30 + 10 + 10; //absolute position - page margins + left + svg margins
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_18_RectAbsoluteMarginsSizeNestedBottomRight()
        {
            var path = AssertGetContentFile("SVG_18_RectAbsoluteMarginsSizeNestedBottomRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_18_RectAbsoluteMarginsSizeNestedBottomRight.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(0, posRun.Width); //absolute - so no size in the line
            Assert.AreEqual(0, posRun.Height); //absolute - so no size in the line

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(0, svgBlock.TotalBounds.Y); //absolute
            Assert.AreEqual(0, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 20; //absolute position - bottom of nest - padding - 20 bottom pos - height = 20
            xOffset = layout.AllPages[0].Width - 30 - 10 - 30 - 100; //right of page - margins - padding - right pos - svg width.
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_19_RectAbsoluteMarginsSizeNestedTextRight()
        {
            var path = AssertGetContentFile("SVG_19_RectAbsoluteMarginsSizeNestedTextRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_19_RectAbsoluteMarginsSizeNestedTextRight.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(0, posRun.Width); //absolute - so no size in the line
            Assert.AreEqual(0, posRun.Height); //absolute - so no size in the line

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(0, svgBlock.TotalBounds.Y); //absolute
            Assert.AreEqual(0, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30 + 30 + 10 + 60 + 10; //absolute position - page margins, outer line, padding, 3rd line offset in nest + svg margins
            xOffset = 30 + 10 + 10; //absolute position - page margins + padding + svg margins
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_20_RectAbsoluteMarginsSizeNestedTextCentre()
        {
            var path = AssertGetContentFile("SVG_20_RectAbsoluteMarginsSizeNestedTextCentre");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_20_RectAbsoluteMarginsSizeNestedTextCentre.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(0, posRun.Width); //absolute - so no size in the line
            Assert.AreEqual(0, posRun.Height); //absolute - so no size in the line

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(0, svgBlock.TotalBounds.Y); //absolute
            Assert.AreEqual(0, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30 + 30 + 10 + 60 + 10; //absolute position - page margins, outer line, padding, 3rd line offset in nest + svg margins
            xOffset = 30 + 10 + 10; //absolute position - page margins + padding + svg margins
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_21_RectFixedMarginsSizeNested()
        {
            var path = AssertGetContentFile("SVG_21_RectFixedMarginsSizeNested");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_21_RectFixedMarginsSizeNested.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(0, posRun.Width); //absolute - so no size in the line
            Assert.AreEqual(0, posRun.Height); //absolute - so no size in the line

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(0, svgBlock.TotalBounds.Y); //absolute
            Assert.AreEqual(0, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30 + 30 + 10 + 60 + 10; //absolute position - page margins, outer line, padding, 3rd line offset in nest + svg margins
            xOffset = 30 + 10 + 10; //absolute position - page margins + padding + svg margins
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_22_RectFixedMarginsSizeNestedTopLeft()
        {
            var path = AssertGetContentFile("SVG_22_RectFixedMarginsSizeNestedTopLeft");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_22_RectFixedMarginsSizeNestedTopLeft.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(0, posRun.Width); //absolute - so no size in the line
            Assert.AreEqual(0, posRun.Height); //absolute - so no size in the line

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(0, svgBlock.TotalBounds.Y); //absolute
            Assert.AreEqual(0, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            Assert.IsNotNull(svg);
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 20; //fixed position - top + margins
            xOffset = 30; //fixed position - left + margins
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height); 
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_23_RectFixedMarginsSizeNestedBottomRight()
        {
            var path = AssertGetContentFile("SVG_23_RectFixedMarginsSizeNestedBottomRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_23_RectFixedMarginsSizeNestedBottomRight.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(0, posRun.Width); //absolute - so no size in the line
            Assert.AreEqual(0, posRun.Height); //absolute - so no size in the line

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(0, svgBlock.TotalBounds.Y); //absolute
            Assert.AreEqual(0, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            Assert.IsNotNull(svg);
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = layout.AllPages[0].Height - 20 - 120 - 10 ; //fixed position = bottom - position - height - svg margin bottom
            xOffset = layout.AllPages[0].Width - 20 - 100 - 10 ; //fixed position - right - position - width - margins right
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height); 
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_24_RectFixedMarginsSizeNestedTextRight()
        {
            var path = AssertGetContentFile("SVG_24_RectFixedMarginsSizeNestedTextRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_24_RectFixedMarginsSizeNestedTextRight.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(0, posRun.Width); //absolute - so no size in the line
            Assert.AreEqual(0, posRun.Height); //absolute - so no size in the line

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(0, svgBlock.TotalBounds.Y); //absolute
            Assert.AreEqual(0, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30 + 30 + 10 + 60 + 10; //absolute position - page margins, outer line, padding, 3rd line offset in nest + svg margins
            xOffset = 30 + 10 + 10; //absolute position - page margins + padding + svg margins
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height); 
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_25_RectFixedMarginsSizeNestedTextCentre()
        {
            var path = AssertGetContentFile("SVG_25_RectFixedMarginsSizeNestedTextCentre");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_25_RectFixedMarginsSizeNestedTextCentre.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(3, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);
            
            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.IsInstanceOfType(posRun.Owner, typeof(SVGCanvas));
            

            //explicit size is 100x120
            
            Unit yOffset = 0;
            Unit xOffset = 0;
            Unit height = 120;
            Unit width = 100;

            
            
            Assert.AreEqual(xOffset, posRun.OffsetX);
            Assert.AreEqual(yOffset, posRun.OffsetY);
            Assert.AreEqual(0, posRun.Width); //absolute - so no size in the line
            Assert.AreEqual(0, posRun.Height); //absolute - so no size in the line

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsTrue(nest.HasPositionedRegions);
            Assert.AreEqual(1, nest.PositionedRegions.Count);
            var pos = nest.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            Assert.AreEqual(posRun, pos.AssociatedRun);
            Assert.AreEqual(1, pos.Contents.Count);
            Assert.IsFalse(posRun.RenderAsXObject);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            
            
            
            var svgBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            yOffset = 30; //second line
            
            
            Assert.AreEqual(0, svgBlock.TotalBounds.Y); //absolute
            Assert.AreEqual(0, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width + 20, svgBlock.TotalBounds.Width); //inc. margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30 + 30 + 10 + 60 + 10; //absolute position - page margins, outer line, padding, 3rd line offset in nest + svg margins
            xOffset = 30 + 10 + 10; //absolute position - page margins + padding + svg margins
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 20, bbox.Width);
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height); 
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_26_RectBlockMarginsAutoSizeNestedTextCentre()
        {
            var path = AssertGetContentFile("SVG_26_RectBlockMarginsAutoSizeNestedTextCentre");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_26_RectBlockMarginsAutoSizeNestedTextCentre.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(5, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsFalse(nest.HasPositionedRegions);
            
            //explicit size is 100x120
            
            var svgBlock = nest.Columns[0].Contents[2] as PDFLayoutBlock;
            
            Unit yOffset = 60; //second line
            Unit xOffset = (nest.Width - 20 - 100) / 2.0; //centre so - half width - padding and svg width
            Unit height = 120;
            Unit width = 100;
            
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y); //After second line
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width , svgBlock.TotalBounds.Width); //auto margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30 + 30 + 10 + 60 + 10; //absolute position - page margins, outer line, padding, 3rd line offset in nest + svg margins
            xOffset += 30 + 10; //absolute = page margins + left padding
            
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
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width, bbox.Width); //no horizontal margins
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height); 
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_27_RectBlockMarginLeftAutoSizeNestedTextCentre()
        {
            var path = AssertGetContentFile("SVG_27_RectBlockMarginLeftAutoSizeNestedTextCentre");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_27_RectBlockMarginLeftAutoSizeNestedTextCentre.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(5, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsFalse(nest.HasPositionedRegions);
            
            //explicit size is 100x120
            
            var svgBlock = nest.Columns[0].Contents[2] as PDFLayoutBlock;
            
            Unit yOffset = 60; //second line
            Unit xOffset = (nest.Width - 10 - 20 - 100); //right align so - width - padding and svg width + right margin
            Unit height = 120;
            Unit width = 100;
            
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y); //After second line
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width + 10 , svgBlock.TotalBounds.Width); //auto margins left only
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30 + 30 + 10 + 60 + 10; //absolute position - page margins, outer line, padding, 3rd line offset in nest + svg margins
            xOffset += 30 + 10; //absolute = page margins + left padding
            
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
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 10, bbox.Width); //right horizontal margins
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height); 
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_28_RectBlockMarginsAutoSizeNestedTextRight()
        {
            var path = AssertGetContentFile("SVG_28_RectBlockMarginsAutoSizeNestedTextRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_28_RectBlockMarginsAutoSizeNestedTextRight.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(5, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsFalse(nest.HasPositionedRegions);
            
            //explicit size is 100x120
            
            var svgBlock = nest.Columns[0].Contents[2] as PDFLayoutBlock;
            
            Unit yOffset = 60; //second line
            Unit xOffset = (nest.Width - 20 - 100) / 2.0; //centre so - half width - padding and svg width
            Unit height = 120;
            Unit width = 100;
            
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y); //After second line
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width , svgBlock.TotalBounds.Width); //auto margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30 + 30 + 10 + 60 + 10; //absolute position - page margins, outer line, padding, 3rd line offset in nest + svg margins
            xOffset += 30 + 10; //absolute = page margins + left padding
            
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
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width, bbox.Width); //no horizontal margins
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_29_RectBlockMarginLeftAutoSizeNestedTextRight()
        {
            var path = AssertGetContentFile("SVG_29_RectBlockMarginLeftAutoSizeNestedTextRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_29_RectBlockMarginLeftAutoSizeNestedTextRight.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(5, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsFalse(nest.HasPositionedRegions);
            
            //explicit size is 100x120
            
            var svgBlock = nest.Columns[0].Contents[2] as PDFLayoutBlock;
            
            Unit yOffset = 60; //second line
            Unit xOffset = (nest.Width - 10 - 20 - 100); //right align so - width - padding and svg width + right margin
            Unit height = 120;
            Unit width = 100;
            
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y); //After second line
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width + 10 , svgBlock.TotalBounds.Width); //auto margins left only
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30 + 30 + 10 + 60 + 10; //absolute position - page margins, outer line, padding, 3rd line offset in nest + svg margins
            xOffset += 30 + 10; //absolute = page margins + left padding
            
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
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 10, bbox.Width); //right horizontal margins
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height); 
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_30_RectBlockMarginsAutoSizeNestedTextJustify()
        {
            var path = AssertGetContentFile("SVG_30_RectBlockMarginsAutoSizeNestedTextJustify");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_30_RectBlockMarginsAutoSizeNestedTextJustify.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(5, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsFalse(nest.HasPositionedRegions);
            
            //explicit size is 100x120
            
            var svgBlock = nest.Columns[0].Contents[2] as PDFLayoutBlock;
            
            Unit yOffset = 60; //second line
            Unit xOffset = (nest.Width - 20 - 100) / 2.0; //centre so - half width - padding and svg width
            Unit height = 120;
            Unit width = 100;
            
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y); //After second line
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width , svgBlock.TotalBounds.Width); //auto margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30 + 30 + 10 + 60 + 10; //absolute position - page margins, outer line, padding, 3rd line offset in nest + svg margins
            xOffset += 30 + 10; //absolute = page margins + left padding
            
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
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width, bbox.Width); //no horizontal margins
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_31_RectBlockMarginLeftAutoSizeNestedTextJustify()
        {
            var path = AssertGetContentFile("SVG_31_RectBlockMarginLeftAutoSizeNestedTextJustify");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_31_RectBlockMarginLeftAutoSizeNestedTextJustify.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(5, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsFalse(nest.HasPositionedRegions);
            
            //explicit size is 100x120
            
            var svgBlock = nest.Columns[0].Contents[2] as PDFLayoutBlock;
            
            Unit yOffset = 60; //second line
            Unit xOffset = (nest.Width - 10 - 20 - 100); //right align so - width - padding and svg width + right margin
            Unit height = 120;
            Unit width = 100;
            
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y); //After second line
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width + 10 , svgBlock.TotalBounds.Width); //auto margins left only
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30 + 30 + 10 + 60 + 10; //absolute position - page margins, outer line, padding, 3rd line offset in nest + svg margins
            xOffset += 30 + 10; //absolute = page margins + left padding
            
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
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 10, bbox.Width); //right horizontal margins
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height); 
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_32_RectBlockMarginsAutoSizeNestedTextLeft()
        {
            var path = AssertGetContentFile("SVG_32_RectBlockMarginsAutoSizeNestedTextLeft");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_32_RectBlockMarginsAutoSizeNestedTextLeft.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(5, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsFalse(nest.HasPositionedRegions);
            
            //explicit size is 100x120
            
            var svgBlock = nest.Columns[0].Contents[2] as PDFLayoutBlock;
            
            Unit yOffset = 60; //second line
            Unit xOffset = (nest.Width - 20 - 100) / 2.0; //centre so - half width - padding and svg width
            Unit height = 120;
            Unit width = 100;
            
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y); //After second line
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width , svgBlock.TotalBounds.Width); //auto margins
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30 + 30 + 10 + 60 + 10; //absolute position - page margins, outer line, padding, 3rd line offset in nest + svg margins
            xOffset += 30 + 10; //absolute = page margins + left padding
            
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
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width, bbox.Width); //no horizontal margins
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height); 
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SVG_33_RectBlockMarginLeftAutoSizeNestedTextLeft()
        {
            var path = AssertGetContentFile("SVG_33_RectBlockMarginLeftAutoSizeNestedTextLeft");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_33_RectBlockMarginLeftAutoSizeNestedTextLeft.pdf"))
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

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(5, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //inner second line
            line = nest.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsFalse(nest.HasPositionedRegions);
            
            //explicit size is 100x120
            
            var svgBlock = nest.Columns[0].Contents[2] as PDFLayoutBlock;
            
            Unit yOffset = 60; //second line
            Unit xOffset = (nest.Width - 10 - 20 - 100); //right align so - width - padding and svg width + right margin
            Unit height = 120;
            Unit width = 100;
            
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y); //After second line
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width + 10 , svgBlock.TotalBounds.Width); //auto margins left only
            Assert.AreEqual(height + 20, svgBlock.TotalBounds.Height); //inc. margins
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30 + 30 + 10 + 60 + 10; //absolute position - page margins, outer line, padding, 3rd line offset in nest + svg margins
            xOffset += 30 + 10; //absolute = page margins + left padding
            
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
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(width + 10, bbox.Width); //right horizontal margins
            Assert.AreEqual(height + 20, bbox.Height);
            
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
            //These are zero as it does not take any space on the line
            Assert.AreEqual(0, bounds.X);
            Assert.AreEqual(0, bounds.Y);
            Assert.AreEqual(0, bounds.Width); 
            Assert.AreEqual(0, bounds.Height);
            
            Assert.IsInstanceOfType<Component>(compRun.Owner);
            var owner = compRun.Owner as Component;
            Assert.IsNotNull(owner);

            xOffset +=  30; // + x pos
            yOffset +=  20; // + y pos
            
            arrange = owner.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Width);
            Assert.AreEqual(30, arrange.RenderBounds.Height); 
        }


        [TestCategory(TestCategoryName)]
        [TestMethod]
        public void SVG_40_RectAndPathWithViewport()
        {
            var path = AssertGetContentFile("SVG_40_RectAndPathWithViewport");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_40_RectAndPathWithViewport.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            var content = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;
            Assert.IsNotNull(content);
            Assert.AreEqual(3, content.Contents.Count);

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(6, nest.Columns[0].Contents.Count);
            

            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            

            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsFalse(nest.HasPositionedRegions);
            
            //explicit size is 100x120
            
            var svgBlock = nest.Columns[0].Contents[1] as PDFLayoutBlock;
            
            Unit yOffset = 30; //first line
            Unit xOffset = 0; //block own line
            Unit height = 500; // this is the viewbox height
            Unit width = 440; //this is the viewBox width
            
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y); //After second line
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width , svgBlock.TotalBounds.Width); //viewbox
            Assert.AreEqual(height , svgBlock.TotalBounds.Height); //viewbox
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            var svg = layout.DocumentComponent.FindAComponentById("Canvas1") as SVGCanvas;
            var arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;

            
            yOffset = 30 + 30 + 10 + 30 + 10; //page margins, outer line, padding, 2nd line offset in nest + svg margins
            xOffset += 30 + 10 + 10; //page margins + left padding + nest padding
            width = 150; //should be back to the output width (transformed)
            height = 100; //should be back to the output height (transformed)
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //check the xObject
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv1") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);

            
            var bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(440.0, bbox.Width); //no horizontal margins
            Assert.AreEqual(500, bbox.Height);
            
            Assert.IsTrue(rsrc.Registered);
            Assert.IsNull(rsrc.Layout);
            Assert.AreEqual(svgBlock, rsrc.Renderer.Layout);
            Assert.IsNotNull(rsrc.Renderer.RenderReference);
            
            //check the renderer for bbox and matrix
            
            var renderer = rsrc.Renderer as PDFXObjectRenderer;
            Assert.IsNotNull(renderer);
            Assert.IsNotNull(renderer.xObjectMatrix);

            bbox = renderer.RenderBoundingBox;
            
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(440.0, bbox.Width); //no horizontal margins
            Assert.AreEqual(500, bbox.Height);
            
            var components = renderer.xObjectMatrix.Components;

            var evenscale = 0.2; //The scale is proportional and based on the height
            
            Assert.AreEqual(evenscale, Math.Round(components[0], 4));
            Assert.AreEqual(0.0, components[1]);
            Assert.AreEqual(0.0, components[2]);
            Assert.AreEqual(evenscale, Math.Round(components[3], 4));
            
            //we should have a translation in the x direction of half the space
            width = 440.0 * evenscale;
            xOffset = 150 - width;
            xOffset /= 2;
            Assert.AreEqual(xOffset, components[4]);
            
            //yOffset is 0
            Assert.AreEqual(0.0, Math.Round(components[5], 4));
            
            
            //Next line after svg
            line = content.Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            
            //Second SVG
            svgBlock = nest.Columns[0].Contents[3] as PDFLayoutBlock;
            
            yOffset = 30 + 100 + 20 + 30; //first line + svg & margins + second line
            xOffset = 0; //block own line
            height = 500; // this is the viewbox height
            width = 440; //this is the viewBox width
            
            Assert.IsNotNull(svgBlock);
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            
            Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y); //After second line
            Assert.AreEqual(xOffset, svgBlock.TotalBounds.X); //absolute
            Assert.AreEqual(width , svgBlock.TotalBounds.Width); //viewbox
            Assert.AreEqual(height , svgBlock.TotalBounds.Height); //viewbox
            Assert.AreEqual(1, svgBlock.Columns.Length);
            Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);
            
            

            //Arrangement is for links and inner content references
            svg = layout.DocumentComponent.FindAComponentById("Canvas2") as SVGCanvas;
            arrange = svg.GetFirstArrangement() as ComponentMultiArrangement;
            
            
            yOffset += 30 + 30 + 10 + 10; // add page margins, outer line, padding, second svg margins
            xOffset += 30 + 10 + 10; //page margins + left padding + nest padding
            width = 150; //should be back to the output width (transformed)
            height = 100; //should be back to the output height (transformed)
            
            Assert.IsNotNull(arrange);
            Assert.AreEqual(yOffset, arrange.RenderBounds.Y);
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(height, arrange.RenderBounds.Height);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            Assert.IsNull(arrange.NextArrangement);
            
            //second xObject rsrc
            rsrc = layout.DocumentComponent.SharedResources.GetResource(PDFResource.XObjectResourceType, "canv2") as PDFLayoutXObjectResource;
            Assert.IsNotNull(rsrc);
            
            bbox = rsrc.BoundingBox;
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(440.0, bbox.Width); //no horizontal margins
            Assert.AreEqual(500, bbox.Height);
            
            Assert.IsTrue(rsrc.Registered);
            Assert.IsNull(rsrc.Layout);
            Assert.AreEqual(svgBlock, rsrc.Renderer.Layout);
            Assert.IsNotNull(rsrc.Renderer.RenderReference);
            
            //check the renderer for bbox and matrix
            
            renderer = rsrc.Renderer as PDFXObjectRenderer;
            Assert.IsNotNull(renderer);
            Assert.IsNotNull(renderer.xObjectMatrix);

            bbox = renderer.RenderBoundingBox;
            
            Assert.AreEqual(0, bbox.X);
            Assert.AreEqual(0, bbox.Y);
            Assert.AreEqual(440.0, bbox.Width); //no horizontal margins
            Assert.AreEqual(500, bbox.Height);
            
            components = renderer.xObjectMatrix.Components;
            
            
            var xscale = Math.Round(width.PointsValue / 440.0, 4);
            Assert.AreEqual(xscale, Math.Round(components[0], 4)); //0.3409
            
            Assert.AreEqual(0.0, components[1]);
            Assert.AreEqual(0.0, components[2]);
            
            var yscale = Math.Round(height.PointsValue / 500.0, 4);
            Assert.AreEqual(yscale, Math.Round(components[3], 4)); //0.2000
            
            Assert.AreEqual(0.0, components[4]);
            Assert.AreEqual(0.0, components[5]);
            
            //check that the svg rect is possitioned and SCALED correctly
            var block = renderer.Layout as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(1, block.Columns.Length);
            Assert.AreEqual(1, block.Columns[0].Contents.Count);
            
            line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            Assert.AreEqual(2, line.Runs.Count);
            var compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);

            var svgRect = compRun.Owner as SVGRect;
            Assert.IsNotNull(svgRect);

            arrange = svgRect.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);

            var bounds = arrange.RenderBounds;
            //svg rect is specified as 400 wide, then scaled in the view
            width = 400.0 * xscale;
            height = 300.0 * yscale;
            
            xOffset += 20.0 * xscale;
            yOffset += 20.0 * yscale;
            
            

            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(bounds.Width.PointsValue));
            Assert.AreEqual(Math.Round(height.PointsValue), Math.Round(bounds.Height.PointsValue));
            
            Assert.AreEqual(Math.Round(xOffset.PointsValue), Math.Round(bounds.X.PointsValue));
            Assert.AreEqual(Math.Round(yOffset.PointsValue), Math.Round(bounds.Y.PointsValue));
            
            //Check the text
            
            var posRun = line.Runs[1] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            
            var pos = posRun.Region as PDFLayoutPositionedRegion;
            Assert.IsNotNull(pos);
            
            block = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            var text = block.Owner as SVGText;
            Assert.IsNotNull(text);

            arrange = text.GetFirstArrangement() as ComponentMultiArrangement;
            Assert.IsNotNull(arrange);
            
            Assert.AreEqual(Math.Round(xOffset.PointsValue), Math.Round(arrange.RenderBounds.X.PointsValue));
            Assert.AreEqual(Math.Round(yOffset.PointsValue) - 1, Math.Round(arrange.RenderBounds.Y.PointsValue)); //there is extra padding for the hanging baseline offset.
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Width.PointsValue)); //same width as using textLength property.
        }

        [TestCategory(TestCategoryName)]
        [TestMethod]
        public void SVG_41_RectAndPathWithViewportAndRatio()
        {
            var path = AssertGetContentFile("SVG_41_RectAndPathWithViewportAndRatio");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_41_RectAndPathWithViewportAndRatio.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.AppendTraceLog = true;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var content = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;
            Assert.IsNotNull(content);
            Assert.AreEqual(3, content.Contents.Count);

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(4, nest.Columns[0].Contents.Count);


            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);


            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsFalse(nest.HasPositionedRegions);

            var table = nest.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(table);
            Assert.AreEqual(1, table.Columns.Length);
            //2 rows
            Assert.AreEqual(2, table.Columns[0].Contents.Count);

            //All svgs should have the same bounds within their container

            Unit yOffset = 14; //first line
            Unit xOffset = 0; //block own line
            Unit height = 240; // this is the viewbox height
            Unit width = 240; //this is the viewBox width

            var svgs = new List<PDFLayoutBlock>();
            PDFLayoutBlock svgBlock = null;

            for (var r = 0; r < 2; r++)
            {
                var row = table.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.IsNotNull(row);
                Assert.AreEqual(3, row.Columns.Length);

                for (var c = 0; c < 3; c++)
                {
                    Assert.AreEqual(1, row.Columns[c].Contents.Count);

                    //cell r,c
                    var cell = row.Columns[c].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(cell);
                    Assert.AreEqual(1, cell.Columns.Length);
                    Assert.AreEqual(2, cell.Columns[0].Contents.Count);

                    //div with text above
                    var topBlock = cell.Columns[0].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(topBlock);

                    svgBlock = cell.Columns[0].Contents[1] as PDFLayoutBlock;

                    Assert.IsNotNull(svgBlock);
                    Assert.IsTrue(svgBlock.IsExplicitLayout);

                    Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
                    Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
                    Assert.AreEqual(width, svgBlock.TotalBounds.Width);
                    Assert.AreEqual(height, svgBlock.TotalBounds.Height);
                    Assert.AreEqual(1, svgBlock.Columns.Length);
                    Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);

                    svgs.Add(svgBlock);
                }

            }

            Assert.AreEqual(6, svgs.Count);

            var scale = 100.0 / 240.0;
            width = 200 * scale; //actual rect width
            height = 200 * scale; //actual rect height
            
            //check the xObject
            Assert.AreEqual(7, doc.SharedResources.Count);

            //first = left positioned
            svgBlock = svgs[0];
            var svg = svgBlock.Owner as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual("Canvas11", svg.ID);
            
            var ownerBounds = svg.GetFirstArrangement().RenderBounds;
            
            Assert.AreEqual(150, ownerBounds.Width);
            Assert.AreEqual(100, ownerBounds.Height);

            //0 = whitespace
            var rect = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect);
            
            var arrange = rect.GetFirstArrangement();
            
            Assert.AreEqual(Math.Round(200 * scale), Math.Round(arrange.RenderBounds.Width.PointsValue));
            Assert.AreEqual(Math.Round(200 * scale), Math.Round(arrange.RenderBounds.Height.PointsValue));
            Assert.AreEqual(Math.Round(20 * scale) + ownerBounds.X, Math.Round(arrange.RenderBounds.X.PointsValue));
            Assert.AreEqual(Math.Round(20 * scale) + ownerBounds.Y, Math.Round(arrange.RenderBounds.Y.PointsValue));

            //second = middle top positioned
            svgBlock = svgs[1];
            svg = svgBlock.Owner as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual("Canvas12", svg.ID);
            
            ownerBounds = svg.GetFirstArrangement().RenderBounds;
            Assert.AreEqual(150, ownerBounds.Width);
            Assert.AreEqual(100, ownerBounds.Height);

            
            //centre = half (viewPort - width) * transform scale
            
            var posOffset = Math.Round((ownerBounds.Width.PointsValue - width.PointsValue) / 2);
            //0 = whitespace
            rect = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect);
            arrange = rect.GetFirstArrangement();
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Width.PointsValue));
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Height.PointsValue));
            Assert.AreEqual(Math.Round(ownerBounds.X.PointsValue + posOffset), Math.Round(arrange.RenderBounds.X.PointsValue));
            Assert.AreEqual(Math.Round(ownerBounds.Y.PointsValue + (20 * scale)), Math.Round(arrange.RenderBounds.Y.PointsValue));
            
            
            //third = right top positioned
            svgBlock = svgs[2];
            svg = svgBlock.Owner as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual("Canvas13", svg.ID);

            
            ownerBounds = svg.GetFirstArrangement().RenderBounds;
            Assert.AreEqual(150, ownerBounds.Width);
            Assert.AreEqual(100, ownerBounds.Height);

            
            //right = half (viewPort - width - scale) * transform scale
            width = 200 * scale;
            
            //20 is remainder of right sapce betwen the rect and the svg boundary
            posOffset = Math.Round((ownerBounds.Width.PointsValue - width.PointsValue - (20 * scale)));
            
            //0 = whitespace
            
            rect = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect);
            arrange = rect.GetFirstArrangement();
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Width.PointsValue));
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Height.PointsValue));
            Assert.AreEqual(Math.Round(ownerBounds.X.PointsValue + posOffset), Math.Round(arrange.RenderBounds.X.PointsValue));
            Assert.AreEqual(Math.Round(ownerBounds.Y.PointsValue + (20 * scale)), Math.Round(arrange.RenderBounds.Y.PointsValue));
            
            
            //
            // second row
            // vertical alignment
            //

            //first top aligned
            
            svgBlock = svgs[3];
            svg = svgBlock.Owner as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual("Canvas21", svg.ID);
            
            ownerBounds = svg.GetFirstArrangement().RenderBounds;
            
            Assert.AreEqual(100, ownerBounds.Width);
            Assert.AreEqual(150, ownerBounds.Height);
            Assert.AreEqual(255, ownerBounds.Y);
            
            //0 = whitespace
            rect = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect);
            
            arrange = rect.GetFirstArrangement();
            
            Assert.AreEqual(Math.Round(200 * scale), Math.Round(arrange.RenderBounds.Width.PointsValue));
            Assert.AreEqual(Math.Round(200 * scale), Math.Round(arrange.RenderBounds.Height.PointsValue));
            Assert.AreEqual(Math.Round(20 * scale) + ownerBounds.X, Math.Round(arrange.RenderBounds.X.PointsValue));
            //Doesn't correlate - off by the inverse height
//            Assert.AreEqual(Math.Round(20 * scale) + ownerBounds.Y, Math.Round(arrange.RenderBounds.Y.PointsValue));

            //second = middle top positioned
            svgBlock = svgs[4];
            svg = svgBlock.Owner as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual("Canvas22", svg.ID);
            
            ownerBounds = svg.GetFirstArrangement().RenderBounds;
            Assert.AreEqual(100, ownerBounds.Width);
            Assert.AreEqual(150, ownerBounds.Height);

            
            //centre = half (viewPort - width) * transform scale
            width = 200 * scale;
            
            posOffset = Math.Round(ownerBounds.Height.PointsValue - height.PointsValue);
            posOffset /= 2;
            //posOffset += 20 * scale; //offset after we have the bounds relative bounds
            
            //0 = whitespace
            rect = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect);
            arrange = rect.GetFirstArrangement();
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Width.PointsValue));
            Assert.AreEqual(Math.Round(height.PointsValue), Math.Round(arrange.RenderBounds.Height.PointsValue));
            Assert.AreEqual(Math.Round(ownerBounds.X.PointsValue + (20 * scale)), Math.Round(arrange.RenderBounds.X.PointsValue));
//            Assert.AreEqual(Math.Round(ownerBounds.Y.PointsValue + posOffset ), Math.Round(arrange.RenderBounds.Y.PointsValue));
            
            
            //third = right top positioned
            svgBlock = svgs[5];
            svg = svgBlock.Owner as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual("Canvas23", svg.ID);

            
            ownerBounds = svg.GetFirstArrangement().RenderBounds;
            Assert.AreEqual(100, ownerBounds.Width);
            Assert.AreEqual(150, ownerBounds.Height);

            
            //right = half (viewPort - width - scale) * transform scale
            width = 200 * scale;
            
            //20 is remainder of right sapce betwen the rect and the svg boundary
            posOffset = ownerBounds.Height.PointsValue - height.PointsValue;
            
            //0 = whitespace
            
            rect = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect);
            arrange = rect.GetFirstArrangement();
            
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Width.PointsValue));
            Assert.AreEqual(Math.Round(height.PointsValue), Math.Round(arrange.RenderBounds.Height.PointsValue));
            Assert.AreEqual(Math.Round(ownerBounds.X.PointsValue + (20 * scale)), Math.Round(arrange.RenderBounds.X.PointsValue));
 //           Assert.AreEqual(Math.Round(ownerBounds.Y.PointsValue + posOffset), Math.Round(arrange.RenderBounds.Y.PointsValue));


 Assert.Inconclusive(
     "Y offsets in the renderbounds of viewport scaled SVG's are not correct - need to set the inverse matrix so the correct Y position is calculated - applied to links in SVG's");

        }
        
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod]
        public void SVG_42_RectAndPathWithViewportAndRatioSlice()
        {
            var path = AssertGetContentFile("SVG_42_RectAndPathWithViewportAndRatioSlice");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVG_42_RectAndPathWithViewportAndRatioSlice.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.AppendTraceLog = true;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
             var content = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;
            Assert.IsNotNull(content);
            Assert.AreEqual(3, content.Contents.Count);

            //outer first line
            var line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //nested block
            var nest = content.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(nest);
            Assert.AreEqual(4, nest.Columns[0].Contents.Count);


            //inner first line
            line = content.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);


            Assert.IsFalse(layout.AllPages[0].ContentBlock.HasPositionedRegions);
            Assert.IsFalse(nest.HasPositionedRegions);

            var table = nest.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(table);
            Assert.AreEqual(1, table.Columns.Length);
            
            //2 rows
            Assert.AreEqual(3, table.Columns[0].Contents.Count);

            //All svgs should have the same bounds within their container

            Unit yOffset = 14; //first line
            Unit xOffset = 0; //block own line
            Unit height = 240; // this is the viewbox height
            Unit width = 240; //this is the viewBox width

            var svgs = new List<PDFLayoutBlock>();
            PDFLayoutBlock svgBlock = null;

            for (var r = 0; r < 3; r++)
            {
                var row = table.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.IsNotNull(row);
                Assert.AreEqual(3, row.Columns.Length);

                for (var c = 0; c < 3; c++)
                {
                    Assert.AreEqual(1, row.Columns[c].Contents.Count);

                    //cell r,c
                    var cell = row.Columns[c].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(cell);
                    Assert.AreEqual(1, cell.Columns.Length);
                    Assert.AreEqual(2, cell.Columns[0].Contents.Count);

                    //div with text above
                    var topBlock = cell.Columns[0].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(topBlock);

                    svgBlock = cell.Columns[0].Contents[1] as PDFLayoutBlock;

                    Assert.IsNotNull(svgBlock);
                    Assert.IsTrue(svgBlock.IsExplicitLayout);

                    Assert.AreEqual(yOffset, svgBlock.TotalBounds.Y);
                    Assert.AreEqual(xOffset, svgBlock.TotalBounds.X);
                    Assert.AreEqual(width, svgBlock.TotalBounds.Width);
                    Assert.AreEqual(height, svgBlock.TotalBounds.Height);
                    Assert.AreEqual(1, svgBlock.Columns.Length);
                    Assert.AreEqual(1, svgBlock.Columns[0].Contents.Count);

                    svgs.Add(svgBlock);
                }

            }

            Assert.AreEqual(9, svgs.Count);

            var scale = 150.0 / 240.0; //maxed
            width = 200 * scale; //actual rect width
            height = 200 * scale; //actual rect height
            
            //check the xObject
            Assert.AreEqual(10, doc.SharedResources.Count);

            //
            // first row - all the same, top aligned with a scale of 150/240
            
            //first = left positioned
            svgBlock = svgs[0];
            var svg = svgBlock.Owner as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual("Canvas11", svg.ID);
            
            var ownerBounds = svg.GetFirstArrangement().RenderBounds;
            
            Assert.AreEqual(150, ownerBounds.Width);
            Assert.AreEqual(100, ownerBounds.Height);

            //0 = whitespace
            var rect = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect);
            
            var arrange = rect.GetFirstArrangement();
            
            Assert.AreEqual(Math.Round(200 * scale), Math.Round(arrange.RenderBounds.Width.PointsValue));
            Assert.AreEqual(Math.Round(200 * scale), Math.Round(arrange.RenderBounds.Height.PointsValue));
            Assert.AreEqual(Math.Round(20 * scale) + ownerBounds.X + 1, Math.Round(arrange.RenderBounds.X.PointsValue)); //1+ for rounding
            //Assert.AreEqual(Math.Round(20 * scale) + ownerBounds.Y, Math.Round(arrange.RenderBounds.Y.PointsValue));

            //second = middle top positioned
            svgBlock = svgs[1];
            svg = svgBlock.Owner as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual("Canvas12", svg.ID);
            
            ownerBounds = svg.GetFirstArrangement().RenderBounds;
            Assert.AreEqual(150, ownerBounds.Width);
            Assert.AreEqual(100, ownerBounds.Height);

            
            //centre = half (viewPort - width) * transform scale

            var posOffset = 20 * scale;
            
            //0 = whitespace
            rect = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect);
            arrange = rect.GetFirstArrangement();
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Width.PointsValue));
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Height.PointsValue));
            Assert.AreEqual(Math.Round(ownerBounds.X.PointsValue + posOffset), Math.Round(arrange.RenderBounds.X.PointsValue));
            //Assert.AreEqual(Math.Round(ownerBounds.Y.PointsValue + (20 * scale)), Math.Round(arrange.RenderBounds.Y.PointsValue));
            
            
            //third = right top positioned
            svgBlock = svgs[2];
            svg = svgBlock.Owner as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual("Canvas13", svg.ID);

            
            ownerBounds = svg.GetFirstArrangement().RenderBounds;
            Assert.AreEqual(150, ownerBounds.Width);
            Assert.AreEqual(100, ownerBounds.Height);

            
            //right = half (viewPort - width - scale) * transform scale
            width = 200 * scale;
            
            
            
            //0 = whitespace
            
            rect = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect);
            arrange = rect.GetFirstArrangement();
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Width.PointsValue));
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Height.PointsValue));
            Assert.AreEqual(Math.Round(ownerBounds.X.PointsValue + posOffset), Math.Round(arrange.RenderBounds.X.PointsValue));
            //Assert.AreEqual(Math.Round(ownerBounds.Y.PointsValue + (20 * scale)), Math.Round(arrange.RenderBounds.Y.PointsValue));
            
            
            //
            // second row
            // vertical alignment
            //

            //first left aligned and overflow
            
            svgBlock = svgs[3];
            svg = svgBlock.Owner as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual("Canvas21", svg.ID);
            
            ownerBounds = svg.GetFirstArrangement().RenderBounds;
            
            Assert.AreEqual(100, ownerBounds.Width);
            Assert.AreEqual(150, ownerBounds.Height);
            Assert.AreEqual(255, ownerBounds.Y);
            
            //0 = whitespace
            rect = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect);
            
            arrange = rect.GetFirstArrangement();
            
            Assert.AreEqual(Math.Round(200 * scale), Math.Round(arrange.RenderBounds.Width.PointsValue));
            Assert.AreEqual(Math.Round(200 * scale), Math.Round(arrange.RenderBounds.Height.PointsValue));
            Assert.AreEqual(Math.Round(20 * scale) + ownerBounds.X + 1, Math.Round(arrange.RenderBounds.X.PointsValue)); //+1 for rounding
            
//            Assert.AreEqual(Math.Round(20 * scale) + ownerBounds.Y, Math.Round(arrange.RenderBounds.Y.PointsValue));

            //second = middle top positioned
            svgBlock = svgs[4];
            svg = svgBlock.Owner as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual("Canvas22", svg.ID);
            
            ownerBounds = svg.GetFirstArrangement().RenderBounds;
            Assert.AreEqual(100, ownerBounds.Width);
            Assert.AreEqual(150, ownerBounds.Height);

            
            //centre = half (viewPort - width) * transform scale
            width = 200 * scale;
            
            posOffset = Math.Round(ownerBounds.Width.PointsValue - width.PointsValue);
            posOffset /= 2;
            //posOffset += 20 * scale; //offset after we have the bounds relative bounds
            
            //0 = whitespace
            rect = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect);
            arrange = rect.GetFirstArrangement();
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Width.PointsValue));
            Assert.AreEqual(Math.Round(height.PointsValue), Math.Round(arrange.RenderBounds.Height.PointsValue));
            Assert.AreEqual(Math.Round(ownerBounds.X.PointsValue + posOffset), Math.Round(arrange.RenderBounds.X.PointsValue));
//            Assert.AreEqual(Math.Round(ownerBounds.Y.PointsValue + posOffset ), Math.Round(arrange.RenderBounds.Y.PointsValue));
            
            
            //third = right top positioned
            svgBlock = svgs[5];
            svg = svgBlock.Owner as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual("Canvas23", svg.ID);

            
            ownerBounds = svg.GetFirstArrangement().RenderBounds;
            Assert.AreEqual(100, ownerBounds.Width);
            Assert.AreEqual(150, ownerBounds.Height);

            
            //right = half (viewPort - width - scale) * transform scale
            width = 200 * scale;
            
            //20 is remainder of right sapce betwen the rect and the svg boundary
            posOffset = ownerBounds.Width.PointsValue - width.PointsValue;
            
            //0 = whitespace
            
            rect = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect);
            arrange = rect.GetFirstArrangement();
            
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Width.PointsValue));
            Assert.AreEqual(Math.Round(height.PointsValue), Math.Round(arrange.RenderBounds.Height.PointsValue));
            Assert.AreEqual(Math.Round(ownerBounds.X.PointsValue + posOffset - (20 * scale)), Math.Round(arrange.RenderBounds.X.PointsValue));
 //           Assert.AreEqual(Math.Round(ownerBounds.Y.PointsValue + posOffset), Math.Round(arrange.RenderBounds.Y.PointsValue));
            
 
 //
            // first row - all the same, top aligned with a scale of 150/240
            
            //first = left positioned
            svgBlock = svgs[6];
            svg = svgBlock.Owner as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual("Canvas31", svg.ID);
            
            ownerBounds = svg.GetFirstArrangement().RenderBounds;
            
            Assert.AreEqual(150, ownerBounds.Width);
            Assert.AreEqual(100, ownerBounds.Height);

            //0 = whitespace
            rect = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect);
            
            arrange = rect.GetFirstArrangement();
            
            Assert.AreEqual(Math.Round(200 * scale), Math.Round(arrange.RenderBounds.Width.PointsValue));
            Assert.AreEqual(Math.Round(200 * scale), Math.Round(arrange.RenderBounds.Height.PointsValue));
            Assert.AreEqual(Math.Round(20 * scale) + ownerBounds.X + 1, Math.Round(arrange.RenderBounds.X.PointsValue)); //1+ for rounding
            //Assert.AreEqual(Math.Round(20 * scale) + ownerBounds.Y, Math.Round(arrange.RenderBounds.Y.PointsValue));

            //second = middle top positioned
            svgBlock = svgs[7];
            svg = svgBlock.Owner as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual("Canvas32", svg.ID);
            
            ownerBounds = svg.GetFirstArrangement().RenderBounds;
            Assert.AreEqual(150, ownerBounds.Width);
            Assert.AreEqual(100, ownerBounds.Height);

            
            //centre = half (viewPort - width) * transform scale

            posOffset = 20 * scale;
            
            //0 = whitespace
            rect = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect);
            arrange = rect.GetFirstArrangement();
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Width.PointsValue));
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Height.PointsValue));
            Assert.AreEqual(Math.Round(ownerBounds.X.PointsValue + posOffset), Math.Round(arrange.RenderBounds.X.PointsValue));
            //Assert.AreEqual(Math.Round(ownerBounds.Y.PointsValue + (20 * scale)), Math.Round(arrange.RenderBounds.Y.PointsValue));
            
            
            //third = right top positioned
            svgBlock = svgs[8];
            svg = svgBlock.Owner as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual("Canvas33", svg.ID);

            
            ownerBounds = svg.GetFirstArrangement().RenderBounds;
            Assert.AreEqual(150, ownerBounds.Width);
            Assert.AreEqual(100, ownerBounds.Height);

            
            //right = half (viewPort - width - scale) * transform scale
            width = 200 * scale;
            
            
            
            //0 = whitespace
            
            rect = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect);
            arrange = rect.GetFirstArrangement();
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Width.PointsValue));
            Assert.AreEqual(Math.Round(width.PointsValue), Math.Round(arrange.RenderBounds.Height.PointsValue));
            Assert.AreEqual(Math.Round(ownerBounds.X.PointsValue + posOffset), Math.Round(arrange.RenderBounds.X.PointsValue));
            //Assert.AreEqual(Math.Round(ownerBounds.Y.PointsValue + (20 * scale)), Math.Round(arrange.RenderBounds.Y.PointsValue));
            
            
            
            Assert.Inconclusive("Y offsets in the renderbounds of viewport scaled SVG's are not correct - need to set the inverse matrix so the correct Y position is calculated - applied to links in SVG's");
        }
    }
}
