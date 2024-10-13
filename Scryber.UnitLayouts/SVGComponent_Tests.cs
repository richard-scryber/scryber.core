using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using Scryber.Drawing;
using Scryber.PDF.Resources;
using Scryber.Styles;
using Scryber.Svg;
using Scryber.Svg.Components;

namespace Scryber.UnitLayouts
{
    [TestClass]
    public class SVGComponent_Tests
    {
        
        const string TestCategoryName = "SVG Components";

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
            path = System.IO.Path.Combine(path, "../../../Content/HTML/SVG/Components/" + name + ".html");
            path = System.IO.Path.GetFullPath(path);

            if (!System.IO.File.Exists(path))
                Assert.Inconclusive("The path the file " + name + " was not found at " + path);

            return path;
        }


        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_01_Rect()
        {
            var path = AssertGetContentFile("SVGComponents_01_Rect");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_01_Rect.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(1, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count);
            var comp = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(comp);

            var rect = comp.Owner as Svg.Components.SVGRect;
            Assert.IsNotNull(rect);

            var arrange = rect.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            Assert.AreEqual(30 + 40 + 20, arrange.RenderBounds.X);
            Assert.AreEqual(30 + 50 + 15 + 20, arrange.RenderBounds.Y);
            Assert.AreEqual(30, arrange.RenderBounds.Height);
            Assert.AreEqual(40, arrange.RenderBounds.Width);

            var gpath = (rect as IGraphicPathComponent).Path;
            Assert.IsNotNull(gpath);

            Assert.AreEqual(1, gpath.SubPaths.Count());
            var p = gpath.SubPaths.First();
            var ops = p.Operations;
            Assert.AreEqual(5, ops.Count);

            var moveOp = ops[0] as PathMoveData;
            Assert.IsNotNull(moveOp);
            Assert.AreEqual(40, moveOp.MoveTo.X);
            Assert.AreEqual(50, moveOp.MoveTo.Y);

            var lineOp = ops[1] as PathLineData;
            Assert.IsNotNull(lineOp);
            Assert.AreEqual(80, lineOp.LineTo.X);
            Assert.AreEqual(50, lineOp.LineTo.Y);
            
            lineOp = ops[2] as PathLineData;
            Assert.IsNotNull(lineOp);
            Assert.AreEqual(80, lineOp.LineTo.X);
            Assert.AreEqual(80, lineOp.LineTo.Y);
            
            lineOp = ops[3] as PathLineData;
            Assert.IsNotNull(lineOp);
            Assert.AreEqual(40, lineOp.LineTo.X);
            Assert.AreEqual(80, lineOp.LineTo.Y);

            var closeOp = ops[4] as PathCloseData;
            Assert.IsNotNull(closeOp);
            
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_02_Circle()
        {
            var path = AssertGetContentFile("SVGComponents_02_Circle");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_02_Circle.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(1, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count);
            var comp = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(comp);

            var circle = comp.Owner as Svg.Components.SVGCircle;
            Assert.IsNotNull(circle);

            var arrange = circle.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //svg margins, body margins and padding center x - radius
            Assert.AreEqual(30 + 10 + 10 + 10, arrange.RenderBounds.X);
            //svg margins, body margins and padding, 1 line, center y - radius
            Assert.AreEqual(30 + 10 + 10 + 15 + 20, arrange.RenderBounds.Y);
            Assert.AreEqual(60, arrange.RenderBounds.Height);
            Assert.AreEqual(60, arrange.RenderBounds.Width);

            var gpath = (circle as IGraphicPathComponent).Path;
            Assert.IsNotNull(gpath);

            Assert.AreEqual(1, gpath.SubPaths.Count());
            var p = gpath.SubPaths.First();
            var ops = p.Operations;
            Assert.AreEqual(6, ops.Count);

            var moveOp = ops[0] as PathMoveData;
            Assert.IsNotNull(moveOp);
            Assert.AreEqual(10, moveOp.MoveTo.X);
            Assert.AreEqual(50, moveOp.MoveTo.Y);

            var curveOp = ops[1] as PathBezierCurveData;
            Assert.IsNotNull(curveOp);
            Assert.AreEqual(40, curveOp.EndPoint.X);
            Assert.AreEqual(20, curveOp.EndPoint.Y);
            
            curveOp = ops[2] as PathBezierCurveData;
            Assert.IsNotNull(curveOp);
            Assert.AreEqual(70, curveOp.EndPoint.X);
            Assert.AreEqual(50, curveOp.EndPoint.Y);
            
            curveOp = ops[3] as PathBezierCurveData;
            Assert.IsNotNull(curveOp);
            Assert.AreEqual(40, curveOp.EndPoint.X);
            Assert.AreEqual(80, curveOp.EndPoint.Y);

            var closeOp = ops[4] as PathBezierCurveData;
            Assert.IsNotNull(closeOp);
            
        }
        
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_03_Ellipse()
        {
            var path = AssertGetContentFile("SVGComponents_03_Ellipse");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_03_Ellipse.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(1, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count);
            var comp = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(comp);

            var ellipse = comp.Owner as Svg.Components.SVGEllipse;
            Assert.IsNotNull(ellipse);

            var arrange = ellipse.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, cx - rx
            Assert.AreEqual(10 + 10 + 30 + 20, arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, cy - ry
            Assert.AreEqual(10 + 10 + 15 + 30 + 10, arrange.RenderBounds.Y);
            Assert.AreEqual(80, arrange.RenderBounds.Height);
            Assert.AreEqual(40, arrange.RenderBounds.Width);

            var gpath = (ellipse as IGraphicPathComponent).Path;
            Assert.IsNotNull(gpath);

            Assert.AreEqual(1, gpath.SubPaths.Count());
            var p = gpath.SubPaths.First();
            var ops = p.Operations;
            Assert.AreEqual(6, ops.Count);

            var moveOp = ops[0] as PathMoveData;
            Assert.IsNotNull(moveOp);
            Assert.AreEqual(20, moveOp.MoveTo.X);
            Assert.AreEqual(50, moveOp.MoveTo.Y);

            var curveOp = ops[1] as PathBezierCurveData;
            Assert.IsNotNull(curveOp);
            Assert.AreEqual(40, curveOp.EndPoint.X);
            Assert.AreEqual(10, curveOp.EndPoint.Y);
            
            curveOp = ops[2] as PathBezierCurveData;
            Assert.IsNotNull(curveOp);
            Assert.AreEqual(60, curveOp.EndPoint.X);
            Assert.AreEqual(50, curveOp.EndPoint.Y);
            
            curveOp = ops[3] as PathBezierCurveData;
            Assert.IsNotNull(curveOp);
            Assert.AreEqual(40, curveOp.EndPoint.X);
            Assert.AreEqual(90, curveOp.EndPoint.Y);

            curveOp = ops[4] as PathBezierCurveData;
            Assert.IsNotNull(curveOp);
            Assert.AreEqual(20, curveOp.EndPoint.X);
            Assert.AreEqual(50, curveOp.EndPoint.Y);
            
            var closeOp = ops[5] as PathCloseData;
            Assert.IsNotNull(closeOp);
            
        }
        
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_04_PolyLine()
        {
            var path = AssertGetContentFile("SVGComponents_04_PolyLine");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_04_PolyLine.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(1, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count);
            var comp = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(comp);

            var poly = comp.Owner as Svg.Components.SVGPolyLine;
            Assert.IsNotNull(poly);

            var arrange = poly.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x inset
            Assert.AreEqual(10 + 10 + 30 + 10, arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, y inset
            Assert.AreEqual(10 + 10 + 15 + 30 + 10, arrange.RenderBounds.Y);
            Assert.AreEqual(80, arrange.RenderBounds.Height);
            Assert.AreEqual(80, arrange.RenderBounds.Width);

            var gpath = (poly as IGraphicPathComponent).Path;
            Assert.IsNotNull(gpath);

            Assert.AreEqual(1, gpath.SubPaths.Count());
            var p = gpath.SubPaths.First();
            var ops = p.Operations;
            Assert.AreEqual(4, ops.Count);

            var moveOp = ops[0] as PathMoveData;
            Assert.IsNotNull(moveOp);
            Assert.AreEqual(10, moveOp.MoveTo.X);
            Assert.AreEqual(90, moveOp.MoveTo.Y);

            var lineOp = ops[1] as PathLineData;
            Assert.IsNotNull(lineOp);
            Assert.AreEqual(50, lineOp.LineTo.X);
            Assert.AreEqual(25, lineOp.LineTo.Y);
            
            lineOp = ops[2] as PathLineData;
            Assert.IsNotNull(lineOp);
            Assert.AreEqual(50, lineOp.LineTo.X);
            Assert.AreEqual(75, lineOp.LineTo.Y);
            
            lineOp = ops[3] as PathLineData;
            Assert.IsNotNull(lineOp);
            Assert.AreEqual(90, lineOp.LineTo.X);
            Assert.AreEqual(10, lineOp.LineTo.Y);

            //not closed
            //var closeOp = ops[4] as PathCloseData;
            //Assert.IsNotNull(closeOp);
            
        }
        
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_05_Polygon()
        {
            var path = AssertGetContentFile("SVGComponents_05_Polygon");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_05_Polygon.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(1, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count);
            var comp = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(comp);

            var poly = comp.Owner as Svg.Components.SVGPolyLine;
            Assert.IsNotNull(poly);

            var arrange = poly.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margin and padding, svg margin, poly x
            Assert.AreEqual(10 + 10 + 30 + 10, arrange.RenderBounds.X);
            //body margin and padding, 1 line, svg margin, poly y
            Assert.AreEqual(10 + 10 + 15 + 30 + 10, arrange.RenderBounds.Y);
            Assert.AreEqual(80, arrange.RenderBounds.Height);
            Assert.AreEqual(80, arrange.RenderBounds.Width);

            var gpath = (poly as IGraphicPathComponent).Path;
            Assert.IsNotNull(gpath);

            Assert.AreEqual(1, gpath.SubPaths.Count());
            var p = gpath.SubPaths.First();
            var ops = p.Operations;
            Assert.AreEqual(5, ops.Count);

            var moveOp = ops[0] as PathMoveData;
            Assert.IsNotNull(moveOp);
            Assert.AreEqual(10, moveOp.MoveTo.X);
            Assert.AreEqual(90, moveOp.MoveTo.Y);

            var lineOp = ops[1] as PathLineData;
            Assert.IsNotNull(lineOp);
            Assert.AreEqual(50, lineOp.LineTo.X);
            Assert.AreEqual(25, lineOp.LineTo.Y);
            
            lineOp = ops[2] as PathLineData;
            Assert.IsNotNull(lineOp);
            Assert.AreEqual(50, lineOp.LineTo.X);
            Assert.AreEqual(75, lineOp.LineTo.Y);
            
            lineOp = ops[3] as PathLineData;
            Assert.IsNotNull(lineOp);
            Assert.AreEqual(90, lineOp.LineTo.X);
            Assert.AreEqual(10, lineOp.LineTo.Y);
            
            var closeOp = ops[4] as PathCloseData;
            Assert.IsNotNull(closeOp);
            
        }
        
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_06_RoundRect()
        {
            var path = AssertGetContentFile("SVGComponents_06_RoundRect");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_06_RoundRect.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(1, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count);
            var comp = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(comp);

            var rect = comp.Owner as Svg.Components.SVGRect;
            Assert.IsNotNull(rect);

            var arrange = rect.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, rect.X
            Assert.AreEqual(10 + 10 + 30 + 30, arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, rect.Y
            Assert.AreEqual(10 + 10 + 15 + 30 + 20, arrange.RenderBounds.Y);
            Assert.AreEqual(40, arrange.RenderBounds.Height);
            Assert.AreEqual(60, arrange.RenderBounds.Width);

            var gpath = (rect as IGraphicPathComponent).Path;
            Assert.IsNotNull(gpath);

            Assert.AreEqual(1, gpath.SubPaths.Count());
            var p = gpath.SubPaths.First();
            var ops = p.Operations;
            Assert.AreEqual(9, ops.Count);

            var moveOp = ops[0] as PathMoveData;
            Assert.IsNotNull(moveOp);
            Assert.AreEqual(50, moveOp.MoveTo.X);
            Assert.AreEqual(20, moveOp.MoveTo.Y);

            
            
            var lineOp = ops[1] as PathLineData;
            Assert.IsNotNull(lineOp);
             Assert.AreEqual(70, lineOp.LineTo.X);
             Assert.AreEqual(20, lineOp.LineTo.Y);
            
            var curveTo = ops[2] as PathBezierCurveData;
            Assert.IsNotNull(curveTo);
            Assert.AreEqual(90, curveTo.EndPoint.X); 
            Assert.AreEqual(30, curveTo.EndPoint.Y);
            
            lineOp = ops[3] as PathLineData;
            Assert.IsNotNull(lineOp);
             Assert.AreEqual(90, lineOp.LineTo.X);
             Assert.AreEqual(50, lineOp.LineTo.Y);
            
            curveTo = ops[4] as PathBezierCurveData;
            Assert.IsNotNull(curveTo);
            Assert.AreEqual(70, curveTo.EndPoint.X); 
            Assert.AreEqual(60, curveTo.EndPoint.Y);
            
            lineOp = ops[5] as PathLineData;
            Assert.IsNotNull(lineOp);
             Assert.AreEqual(50, lineOp.LineTo.X);
             Assert.AreEqual(60, lineOp.LineTo.Y);

            curveTo = ops[6] as PathBezierCurveData;
            Assert.IsNotNull(curveTo);
            Assert.AreEqual(30, curveTo.EndPoint.X); 
            Assert.AreEqual(50, curveTo.EndPoint.Y);
            
            lineOp = ops[7] as PathLineData;
            Assert.IsNotNull(lineOp);
            Assert.AreEqual(30, lineOp.LineTo.X); 
            Assert.AreEqual(30, lineOp.LineTo.Y);
            
            curveTo = ops[8] as PathBezierCurveData;
            Assert.IsNotNull(curveTo);
            Assert.AreEqual(50, curveTo.EndPoint.X); 
            Assert.AreEqual(20, curveTo.EndPoint.Y);
            
            
            
        }
        
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_07_Line()
        {
            var path = AssertGetContentFile("SVGComponents_07_Line");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_07_Line.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(1, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count);
            var comp = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(comp);

            var gline = comp.Owner as Svg.Components.SVGLine;
            Assert.IsNotNull(gline);

            var arrange = gline.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, line x
            Assert.AreEqual(10 + 10 + 30 + 10, arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, line offset
            Assert.AreEqual(10 + 10 + 15 + 30 + 20, arrange.RenderBounds.Y);
            Assert.AreEqual(60, arrange.RenderBounds.Height);
            Assert.AreEqual(80, arrange.RenderBounds.Width);

            var gpath = (gline as IGraphicPathComponent).Path;
            Assert.IsNotNull(gpath);

            Assert.AreEqual(1, gpath.SubPaths.Count());
            var p = gpath.SubPaths.First();
            var ops = p.Operations;
            Assert.AreEqual(2, ops.Count);

            var moveOp = ops[0] as PathMoveData;
            Assert.IsNotNull(moveOp);
            Assert.AreEqual(10, moveOp.MoveTo.X);
            Assert.AreEqual(20, moveOp.MoveTo.Y);

            
            var lineOp = ops[1] as PathLineData;
            Assert.IsNotNull(lineOp);
             Assert.AreEqual(90, lineOp.LineTo.X);
             Assert.AreEqual(80, lineOp.LineTo.Y);
            
             
        }
        
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_08_Path()
        {
            var path = AssertGetContentFile("SVGComponents_08_Path");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_08_Path.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(1, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count);
            var comp = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(comp);

            var ggpath = comp.Owner as Svg.Components.SVGPath;
            Assert.IsNotNull(ggpath);

            var arrange = ggpath.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, path min x
            Assert.AreEqual(10 + 10 + 30 + 10, arrange.RenderBounds.X);
            //body margins and padding, 1 line of text, svg margins, path min y
            Assert.AreEqual(10 + 10 + 15 + 30 + 10, arrange.RenderBounds.Y);
            Assert.AreEqual(80, arrange.RenderBounds.Height);
            Assert.AreEqual(80, arrange.RenderBounds.Width);

            var gpath = (ggpath as IGraphicPathComponent).Path;
            Assert.IsNotNull(gpath);

            Assert.AreEqual(1, gpath.SubPaths.Count());
            var p = gpath.SubPaths.First();
            var ops = p.Operations;
            Assert.AreEqual(6, ops.Count);

            var moveOp = ops[0] as PathMoveData;
            Assert.IsNotNull(moveOp);
            Assert.AreEqual(10, moveOp.MoveTo.X);
            Assert.AreEqual(30, moveOp.MoveTo.Y);

            var arcOp = ops[1] as PathArcData;
            Assert.IsNotNull(arcOp);
            Assert.AreEqual(20, arcOp.RadiusX);
            Assert.AreEqual(20, arcOp.RadiusY);
            Assert.AreEqual(0, arcOp.XAxisRotation);
            Assert.AreEqual(PathArcSize.Small, arcOp.ArcSize);
            Assert.AreEqual(PathArcSweep.Positive, arcOp.ArcSweep);
            Assert.AreEqual(50, arcOp.EndPoint.X);
            Assert.AreEqual(30, arcOp.EndPoint.Y);
            
            arcOp = ops[2] as PathArcData;
            Assert.IsNotNull(arcOp);
            Assert.AreEqual(20, arcOp.RadiusX);
            Assert.AreEqual(20, arcOp.RadiusY);
            Assert.AreEqual(0, arcOp.XAxisRotation);
            Assert.AreEqual(PathArcSize.Small, arcOp.ArcSize);
            Assert.AreEqual(PathArcSweep.Positive, arcOp.ArcSweep);
            Assert.AreEqual(90, arcOp.EndPoint.X);
            Assert.AreEqual(30, arcOp.EndPoint.Y);

            var quadOp = ops[3] as PathQuadraticCurve;
            Assert.IsNotNull(quadOp);
            Assert.AreEqual(90, quadOp.ControlPoint.X);
            Assert.AreEqual(60, quadOp.ControlPoint.Y);
            Assert.AreEqual(50, quadOp.EndPoint.X);
            Assert.AreEqual(90, quadOp.EndPoint.Y);
            
            quadOp = ops[4] as PathQuadraticCurve;
            Assert.IsNotNull(quadOp);
            Assert.AreEqual(10, quadOp.ControlPoint.X);
            Assert.AreEqual(60, quadOp.ControlPoint.Y);
            Assert.AreEqual(10, quadOp.EndPoint.X);
            Assert.AreEqual(30, quadOp.EndPoint.Y);

            var closeOp = ops[5] as PathCloseData;
            Assert.IsNotNull(closeOp);
            
            
        }
        
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_09_Text()
        {
            var path = AssertGetContentFile("SVGComponents_09_Text");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_09_Text.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.

            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position
            Assert.AreEqual(10 + 10 + 30 + 30, arrange.RenderBounds.X);
            
            //body margins and padding, 1 line, svg margins, y position - line to baseline (where 50 is measured to)
            Assert.AreEqual(Unit.Round(10 + 10 + 15 + 30 + 40 - 10, 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(posBlock.Width, arrange.RenderBounds.Width);

            
            
             
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_10_Text_TextLength()
        {
            var path = AssertGetContentFile("SVGComponents_10_Text_TextLength");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_10_Text_TextLength.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.

            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            var desc = begin.TextRenderOptions.GetDescender();
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position
            Assert.AreEqual(10 + 10 + 30 + 30, arrange.RenderBounds.X);
            
            //body margins and padding, 1 line, svg margins, y position - line to baseline (where 50 is measured to)
            Assert.AreEqual(Unit.Round(10 + 10 + 15 + 30 + 40 - 10, 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(70, arrange.RenderBounds.Width); //explicit width

            //make sure we have character space
            Assert.IsTrue(begin.CharSpace > Unit.Zero);
            
             
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_11_Text_TextLengthSmall()
        {
            var path = AssertGetContentFile("SVGComponents_11_Text_TextLengthSmall");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_11_Text_TextLengthSmall.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.

            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            var desc = begin.TextRenderOptions.GetDescender();
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position
            Assert.AreEqual(10 + 10 + 30 + 30, arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, y position - line to baseline (where 50 is measured to)
            Assert.AreEqual(Unit.Round(10 + 10 + 15 + 30 + 40 - 10, 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(30, arrange.RenderBounds.Width); //explicit width

            //make sure we have character space less than zero
            Assert.IsTrue(begin.CharSpace < Unit.Zero);
            
             
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_12_Text_TextLengthSmallGlyphs()
        {
            var path = AssertGetContentFile("SVGComponents_12_Text_TextLengthSmallGlyphs");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_12_Text_TextLengthSmallGlyphs.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.

            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            var desc = begin.TextRenderOptions.GetDescender();
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position
            Assert.AreEqual(10 + 10 + 30 + 30, arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, y position - line to baseline (where 50 is measured to)
            Assert.AreEqual(Unit.Round(10 + 10 + 15 + 30 + 40 - 10, 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(30, arrange.RenderBounds.Width); //explicit width

            //make sure we have character space = zero and a reduced scale
            Assert.IsTrue(begin.CharSpace == Unit.Zero);
            Assert.IsTrue(begin.TextRenderOptions.CharacterHScale.HasValue && begin.TextRenderOptions.CharacterHScale < 1.0);
            
             
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_13_Text_DominantBaselineHanging()
        {
            var path = AssertGetContentFile("SVGComponents_13_Text_DominantBaselineHanging");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_13_Text_DominantBaselineHanging.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.

            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            var desc = begin.TextRenderOptions.GetDescender();
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position
            Assert.AreEqual(10 + 10 + 30 + 30, arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, y position - half leading space (where 50 is measured to top for hanging)
            Assert.AreEqual(Unit.Round(10 + 10 + 15 + 30 + 40 - 1 , 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(posBlock.Width, arrange.RenderBounds.Width);

            //make sure we have character space = zero and a reduced scale
            Assert.IsTrue(begin.CharSpace == Unit.Zero);
            Assert.IsFalse(begin.TextRenderOptions.CharacterHScale.HasValue);
            
             
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_14_Text_DominantBaselineCentral()
        {
            var path = AssertGetContentFile("SVGComponents_14_Text_DominantBaselineCentral");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_14_Text_DominantBaselineCentral.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.

            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            var desc = begin.TextRenderOptions.GetDescender();
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position
            Assert.AreEqual(10 + 10 + 30 + 30, arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, y position - half font size (where 50 is measured to top for central)
            Assert.AreEqual(Unit.Round(10 + 10 + 15 + 30 + 40 - 6 , 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(posBlock.Width, arrange.RenderBounds.Width);

            //make sure we have character space = zero and a reduced scale
            Assert.IsTrue(begin.CharSpace == Unit.Zero);
            Assert.IsFalse(begin.TextRenderOptions.CharacterHScale.HasValue);
            
             
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_15_Text_DominantBaselineMiddle()
        {
            var path = AssertGetContentFile("SVGComponents_15_Text_DominantBaselineMiddle");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_15_Text_DominantBaselineMiddle.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.

            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            var desc = begin.TextRenderOptions.GetDescender();
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position
            Assert.AreEqual(10 + 10 + 30 + 30, arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, y position - half ex size (where 50 is measured to top for middle)
            Assert.AreEqual(Unit.Round(10 + 10 + 15 + 30 + 40 - 7 , 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(posBlock.Width, arrange.RenderBounds.Width);

            //make sure we have character space = zero and a reduced scale
            Assert.IsTrue(begin.CharSpace == Unit.Zero);
            Assert.IsFalse(begin.TextRenderOptions.CharacterHScale.HasValue);
            
             
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_16_Text_DominantBaselineMathematical()
        {
            var path = AssertGetContentFile("SVGComponents_16_Text_DominantBaselineMathematical");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_16_Text_DominantBaselineMathematical.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.

            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            var desc = begin.TextRenderOptions.GetDescender();
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position
            Assert.AreEqual(10 + 10 + 30 + 30, arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, y position - ascender height (where 50 is measured to top for mathematical)
            Assert.AreEqual(Unit.Round(10 + 10 + 15 + 30 + 40 - 3 , 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(posBlock.Width, arrange.RenderBounds.Width);

            //make sure we have character space = zero and a reduced scale
            Assert.IsTrue(begin.CharSpace == Unit.Zero);
            Assert.IsFalse(begin.TextRenderOptions.CharacterHScale.HasValue);
            
             
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_17_Text_DominantBaselineAfterEdge()
        {
            var path = AssertGetContentFile("SVGComponents_17_Text_DominantBaselineAfterEdge");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_17_Text_DominantBaselineAfterEdge.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.

            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            var desc = begin.TextRenderOptions.GetDescender();
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position
            Assert.AreEqual(10 + 10 + 30 + 30, arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, y position - font size and bottom space (where 50 is measured to top for mathematical)
            Assert.AreEqual(Unit.Round(10 + 10 + 15 + 30 + 40 - 13 , 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(posBlock.Width, arrange.RenderBounds.Width);

            //make sure we have character space = zero and a reduced scale
            Assert.IsTrue(begin.CharSpace == Unit.Zero);
            Assert.IsFalse(begin.TextRenderOptions.CharacterHScale.HasValue);
            
             
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_18_Text_DominantBaselineBeforeEdge()
        {
            var path = AssertGetContentFile("SVGComponents_18_Text_DominantBaselineBeforeEdge");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_18_Text_DominantBaselineBeforeEdge.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.

            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            var desc = begin.TextRenderOptions.GetDescender();
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position
            Assert.AreEqual(10 + 10 + 30 + 30, arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, y position - line space (measured to top for before edge)
            Assert.AreEqual(Unit.Round(10 + 10 + 15 + 30 + 40 + 2 , 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(posBlock.Width, arrange.RenderBounds.Width);

            //make sure we have character space = zero and a reduced scale
            Assert.IsTrue(begin.CharSpace == Unit.Zero);
            Assert.IsFalse(begin.TextRenderOptions.CharacterHScale.HasValue);
            
             
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_19_Text_DominantBaselineAlphabetical()
        {
            var path = AssertGetContentFile("SVGComponents_19_Text_DominantBaselineAlphabetical");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_19_Text_DominantBaselineAlphabetical.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.

            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            var desc = begin.TextRenderOptions.GetDescender();
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position
            Assert.AreEqual(10 + 10 + 30 + 30, arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, y position - (ascent height) (measured to top for alphabetical)
            Assert.AreEqual(Unit.Round(10 + 10 + 15 + 30 + 40 - 10 , 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(posBlock.Width, arrange.RenderBounds.Width);

            //make sure we have character space = zero and a reduced scale
            Assert.IsTrue(begin.CharSpace == Unit.Zero);
            Assert.IsFalse(begin.TextRenderOptions.CharacterHScale.HasValue);
            
             
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_20_Text_DominantBaselineAuto()
        {
            var path = AssertGetContentFile("SVGComponents_20_Text_DominantBaselineAuto");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_20_Text_DominantBaselineAuto.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.

            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            var desc = begin.TextRenderOptions.GetDescender();
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position
            Assert.AreEqual(10 + 10 + 30 + 30, arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, y position - (ascent height) (measured to top for auto)
            Assert.AreEqual(Unit.Round(10 + 10 + 15 + 30 + 40 - 10 , 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(posBlock.Width, arrange.RenderBounds.Width);

            //make sure we have character space = zero and a reduced scale
            Assert.IsTrue(begin.CharSpace == Unit.Zero);
            Assert.IsFalse(begin.TextRenderOptions.CharacterHScale.HasValue);
            
             
        }
        
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_21_Text_AnchorMiddle()
        {
            var path = AssertGetContentFile("SVGComponents_21_Text_AnchorMiddle");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_21_Text_AnchorMiddle.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(3, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.

            var begin = line.Runs[0] as PDFTextRunBegin;
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(begin);
            var desc = begin.TextRenderOptions.GetDescender();
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position - half width for middle
            Assert.AreEqual(10 + 10 + 30 + 30 - (chars.Width / 2), arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, y position - (ascent height) (measured to top for auto)
            Assert.AreEqual(Unit.Round(10 + 10 + 15 + 30 + 40 - 10 , 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(posBlock.Width, arrange.RenderBounds.Width);

            //make sure we have character space = zero and a reduced scale
            Assert.IsTrue(begin.CharSpace == Unit.Zero);
            Assert.IsFalse(begin.TextRenderOptions.CharacterHScale.HasValue);
            
             
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_22_Text_AnchorRight()
        {
            var path = AssertGetContentFile("SVGComponents_22_Text_AnchorRight");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_22_Text_AnchorRight.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(3, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.

            var begin = line.Runs[0] as PDFTextRunBegin;
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(begin);
            var desc = begin.TextRenderOptions.GetDescender();
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position - width for end
            Assert.AreEqual(10 + 10 + 30 + 30 - chars.Width, arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, y position - (ascent height) (measured to top for auto)
            Assert.AreEqual(Unit.Round(10 + 10 + 15 + 30 + 40 - 10 , 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(posBlock.Width, arrange.RenderBounds.Width);

            //make sure we have character space = zero and a reduced scale
            Assert.IsTrue(begin.CharSpace == Unit.Zero);
            Assert.IsFalse(begin.TextRenderOptions.CharacterHScale.HasValue);
            
             
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_23_Text_AnchorExplicitLeft()
        {
            var path = AssertGetContentFile("SVGComponents_23_Text_AnchorExplicitLeft");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_23_Text_AnchorExplicitLeft.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(3, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.

            var begin = line.Runs[0] as PDFTextRunBegin;
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(begin);
            var desc = begin.TextRenderOptions.GetDescender();
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position. Nothing else
            Assert.AreEqual(10 + 10 + 30 + 30 , arrange.RenderBounds.X);
            //body margins and padding, 1 line, svg margins, y position - (ascent height) (measured to top for auto)
            Assert.AreEqual(Unit.Round(10 + 10 + 15 + 30 + 40 - 10 , 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(posBlock.Width, arrange.RenderBounds.Width);

            //make sure we have character space = zero and a reduced scale
            Assert.IsTrue(begin.CharSpace == Unit.Zero);
            Assert.IsFalse(begin.TextRenderOptions.CharacterHScale.HasValue);
            
             
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_24_Text_GrumpyCat()
        {
            var path = AssertGetContentFile("SVGComponents_24_Text_GrumpyCat");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_24_Text_GrumpyCat.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(1, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(4, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            
             
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_25_Text_Delta()
        {
            var path = AssertGetContentFile("SVGComponents_25_Text_Delta");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_25_Text_Delta.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.

            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position
            var xOffset = 10 + 10 + 30 + 30;
            xOffset += 10; //delta x

            //body margins and padding, 1 line, svg margins, y position - line to baseline (where 50 is measured to)
            var yOffset = 10 + 10 + 15 + 30 + 40 - 10;
            yOffset += 20; // delta y
            
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(Unit.Round(yOffset, 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(posBlock.Width, arrange.RenderBounds.Width);

            
            
             
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_26_Text_Spans()
        {
            var path = AssertGetContentFile("SVGComponents_26_Text_Spans");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_26_Text_Spans.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.
            Assert.AreEqual(9, line.Runs.Count);
            
            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position
            var xOffset = 10 + 10 + 30 + 20;
            
            //body margins and padding, 1 line, svg margins, y position - line to baseline (where 50 is measured to)
            var yOffset = 10 + 10 + 15 + 30 + 40 - 11;
            
            
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(Unit.Round(yOffset, 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(posBlock.Width, arrange.RenderBounds.Width);
            
        }
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_27_Text_SpansDelta()
        {
            var path = AssertGetContentFile("SVGComponents_27_Text_SpansDelta");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_27_Text_SpansDelta.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            
            Assert.AreEqual(2, block.Columns[0].Contents.Count);

            var line = block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            
            Assert.AreEqual(1, line.Runs.Count); //text
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var text = posRun.Owner as Svg.Components.SVGText;
            Assert.IsNotNull(text);

            var pos = posRun.Region;
            var posBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(posBlock);
            Assert.AreEqual(1, posBlock.Columns[0].Contents.Count);
            line = posBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line); //this is the actual text line.
            Assert.AreEqual(9, line.Runs.Count);
            
            var begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(begin);
            
            
            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            //body margins and padding, svg margins, x position
            Unit xOffset = 10 + 10 + 30 + 20;
            
            //body margins and padding, 1 line, svg margins, y position - line to baseline (where 50 is measured to)
            Unit yOffset = 10 + 10 + 15 + 30 + 40 - 11;
            
            
            Assert.AreEqual(xOffset, arrange.RenderBounds.X);
            Assert.AreEqual(Unit.Round(yOffset, 0), Unit.Round(arrange.RenderBounds.Y, 0)); //baseline alignment
            
            Assert.AreEqual(posBlock.Height, arrange.RenderBounds.Height);
            Assert.AreEqual(posBlock.Width, arrange.RenderBounds.Width);

            var span0 = text.Content[0] as TextLiteral; // A line
            var span1 = text.Content[1] as SVGTextSpan; // with
            var span2 = text.Content[2] as SVGTextSpan; // spans
            
            Assert.IsNotNull(span0);
            Assert.IsNotNull(span1);
            Assert.IsNotNull(span2);
            arrange = span0.GetFirstArrangement();

            xOffset = 107 + 5;
            yOffset = 94 - 10;
            
            
            //with
            arrange = span1.GetFirstArrangement();
            //delta Y = -10
            Assert.AreEqual(Unit.Round(yOffset , 0), Unit.Round(arrange.RenderBounds.Y, 0));
            //delta X = 5
            Assert.AreEqual(Unit.Round( xOffset, 0), Unit.Round(arrange.RenderBounds.X, 0));

            yOffset = 95;
            xOffset = 148; 
            
            
            //spans
            arrange = span2.GetFirstArrangement();
            

            Assert.AreEqual(Unit.Round(yOffset  , 0), Unit.Round(arrange.RenderBounds.Y, 0));

            Assert.AreEqual(Unit.Round( xOffset, 0), Unit.Round(arrange.RenderBounds.X, 0));

        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_30_Use_Rect()
        {
            var path = AssertGetContentFile("SVGComponents_30_Use_Rect");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_30_Use_Rect.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var svgBlock = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsInstanceOfType(svgBlock.Owner, typeof(SVGCanvas));
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            var svgBounds = ((SVGCanvas)svgBlock.Owner).GetFirstArrangement().RenderBounds;

            var contents = svgBlock.Columns[0].Contents;
            Assert.AreEqual(2, contents.Count);
            var line = contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(0, line.Height);
            Assert.AreEqual(1, line.Runs.Count);
            
            var compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);
            Assert.IsInstanceOfType(compRun.Owner, typeof(SVGRect));
            
            var rect = (SVGRect)compRun.Owner;
            var arrange = rect.GetFirstArrangement();
            var renderBounds = arrange.RenderBounds;

            var expectedBounds = new Rect();
            expectedBounds.X = svgBounds.X + 20;
            expectedBounds.Y += svgBounds.Y + 20;
            expectedBounds.Width = 40;
            expectedBounds.Height = 30;
            
            Assert.AreEqual(expectedBounds, renderBounds);

            var style = arrange.FullStyle;
            var fill = style.GetValue(StyleKeys.SVGFillKey, null);
            Assert.IsNull(fill); //no fill set on the base rect
            

            //Now check the use reference - a placeholder component with a componentrun for the layout of the rect.
            line = contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);
            
            compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);
            Assert.IsInstanceOfType(compRun.Owner, typeof(SVGRect));
            
            rect = (SVGRect)compRun.Owner;
            Assert.AreNotEqual("Rect", rect.ID); //should change the id
            arrange = rect.GetFirstArrangement();
            renderBounds = arrange.RenderBounds;

            expectedBounds = new Rect();
            expectedBounds.X = svgBounds.X + 20 + 10; //the x and y are added to the existing position
            expectedBounds.Y += svgBounds.Y + 20 + 15;
            expectedBounds.Width = 40; //the explicit width should be ignored, as a width is set on the base rect.
            expectedBounds.Height = 30;
            
            Assert.AreEqual(expectedBounds, renderBounds);
            
            //check that the fill has been applied.
            style = arrange.FullStyle;
            fill = style.GetValue(StyleKeys.SVGFillKey, null);
            Assert.IsNotNull(fill); //no fill set on the base rect
            Assert.IsInstanceOfType(fill, typeof(SVGFillColorValue));
            Assert.AreEqual(StandardColors.Blue, ((SVGFillColorValue)fill).FillColor); //should be applied
            Assert.AreEqual(0.5, style.GetValue(StyleKeys.FillOpacityKey, 0.0)); //should be applied
            Assert.AreEqual(1.0, style.GetValue(StyleKeys.StrokeWidthKey, 0.0)); //should NOT be applied as set on the referenced rect
            Assert.AreEqual(StandardColors.Black, style.GetValue(StyleKeys.StrokeColorKey, StandardColors.Transparent)); // set on the base reference, and should be carried through.
        }
        
        
        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SVGComponents_31_Use_Circle()
        {
            var path = AssertGetContentFile("SVGComponents_31_Use_Circle");

            var doc = Document.ParseDocument(path);

            using (var ms = DocStreams.GetOutputStream("SVGComponents_31_Use_Circle.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var svgBlock = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsInstanceOfType(svgBlock.Owner, typeof(SVGCanvas));
            Assert.IsTrue(svgBlock.IsExplicitLayout);

            var svgBounds = ((SVGCanvas)svgBlock.Owner).GetFirstArrangement().RenderBounds;

            var contents = svgBlock.Columns[0].Contents;
            Assert.AreEqual(2, contents.Count);
            var line = contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(0, line.Height);
            Assert.AreEqual(1, line.Runs.Count);
            
            var compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);
            Assert.IsInstanceOfType(compRun.Owner, typeof(SVGCircle));
            
            var circle = (SVGCircle)compRun.Owner;
            var arrange = circle.GetFirstArrangement();
            var renderBounds = arrange.RenderBounds;

            var expectedBounds = new Rect();
            //cx = 20, cy = 30, r = 20;
            expectedBounds.X = svgBounds.X + 20 - 20;
            expectedBounds.Y += svgBounds.Y + 30 - 20;
            expectedBounds.Width = 40;
            expectedBounds.Height = 40;
            
            Assert.AreEqual(expectedBounds, renderBounds);

            var style = arrange.FullStyle;
            var fill = style.GetValue(StyleKeys.SVGFillKey, null);
            Assert.IsNull(fill); //no fill set on the base rect
            

            //Now check the use reference - a placeholder component with a componentrun for the layout of the circle.
            line = contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);
            
            compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);
            Assert.IsInstanceOfType(compRun.Owner, typeof(SVGCircle));
            
            circle = (SVGCircle)compRun.Owner;
            arrange = circle.GetFirstArrangement();
            renderBounds = arrange.RenderBounds;

            expectedBounds = new Rect();
            expectedBounds.X = svgBounds.X + 10; //the x and y are added to the existing position
            expectedBounds.Y += svgBounds.Y + 10 + 15;
            expectedBounds.Width = 40; //the explicit radius should be ignored, as a is set on the base circle.
            expectedBounds.Height = 40;
            
            Assert.AreEqual(expectedBounds, renderBounds);
            
            //check that the fill has been applied.
            style = arrange.FullStyle;
            fill = style.GetValue(StyleKeys.SVGFillKey, null);
            Assert.IsNotNull(fill); //no fill set on the base rect
            Assert.IsInstanceOfType(fill, typeof(SVGFillColorValue));
            Assert.AreEqual(StandardColors.Blue, ((SVGFillColorValue)fill).FillColor); //should be applied
            Assert.AreEqual(0.5, style.GetValue(StyleKeys.FillOpacityKey, 0.0)); //should be applied
            Assert.AreEqual(1.0, style.GetValue(StyleKeys.StrokeWidthKey, 0.0)); //should NOT be applied as set on the referenced rect
            Assert.AreEqual(StandardColors.Black, style.GetValue(StyleKeys.StrokeColorKey, StandardColors.Transparent)); // set on the base reference, and should be carried through.
        }
    }
}