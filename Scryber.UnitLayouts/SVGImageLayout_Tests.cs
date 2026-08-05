using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.Html.Components;
using Scryber.PDF;
using Scryber.Drawing;
using Scryber.Imaging;
using Scryber.PDF.Graphics;
using Scryber.PDF.Resources;
using Scryber.Svg.Components;
using Scryber.Svg.Imaging;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Tests the layout of SVG Images based on image sizes and inner svg dimensions and view-box
    /// </summary>
    [TestClass()]
    public class SVGImageLayout_Tests
    {
        
        #region Test Helper methods
        
        // -----------------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------------

        PDFLayoutDocument layout;
        PDFLayoutContext context;

        
        private string GetResourcePath(string category, string filename, bool assertExists = true)
        {
            var dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            dir = System.IO.Path.Combine(dir, "../../../Content");
            
            if(!string.IsNullOrEmpty(category))
                dir = System.IO.Path.Combine(dir, category);
            
            dir = System.IO.Path.Combine(dir, filename);
            
            if(assertExists)
                Assert.IsTrue(System.IO.File.Exists(dir), "Could not find the file " + dir);
            
            return dir;
        }

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            this.context = args.Context as PDFLayoutContext;
            this.layout = args.Context.GetLayout<PDFLayoutDocument>();
        }
        


        

        /// <summary>
        /// Returns all SVGPDFImageData instances from the document shared resources, in resource order.
        /// </summary>
        private List<SVGPDFImageData> GetSVGImageData(PDFLayoutDocument layout)
        {
            return layout.DocumentComponent.SharedResources
                .OfType<PDFImageXObject>()
                .Select(x => (x.ImageData as ImageDataProxy)?.ImageData as SVGPDFImageData)
                .Where(d => d != null)
                .ToList();
        }

        /// <summary>
        /// Finds the Nth PDFLayoutComponentRun that owns an image by recursively walking
        /// the full layout tree of the first page. Independent of the exact block nesting.
        /// </summary>
        private PDFLayoutComponentRun GetImageRunFromBody(int imageIndex)
        {
            var all = new List<PDFLayoutComponentRun>();
            foreach (var pg in this.layout.AllPages)
                CollectImageRuns(pg.ContentBlock, all);
            Assert.IsTrue(imageIndex < all.Count,
                $"Image run at index {imageIndex} not found — only {all.Count} image runs in layout");
            return all[imageIndex];
        }

        private static void CollectImageRuns(PDFLayoutBlock block, List<PDFLayoutComponentRun> results)
        {
            foreach (var col in block.Columns)
            {
                foreach (var item in col.Contents)
                {
                    if (item is PDFLayoutLine line)
                    {
                        foreach (var run in line.Runs.OfType<PDFLayoutComponentRun>())
                            results.Add(run);
                    }
                    else if (item is PDFLayoutBlock child)
                    {
                        CollectImageRuns(child, results);
                    }
                }
            }
        }

        private static double RunScaleX(Scryber.PDF.Graphics.PDFTransformationMatrix m) => m.Components[0];
        private static double RunScaleY(Scryber.PDF.Graphics.PDFTransformationMatrix m) => m.Components[3];
        private static double RunTranslateX(Scryber.PDF.Graphics.PDFTransformationMatrix m) => m.Components[4];
        private static double RunTranslateY(Scryber.PDF.Graphics.PDFTransformationMatrix m) => m.Components[5];

        #endregion
        
        /// ----------------------------------
        /// Test methods
        /// ----------------------------------
        
        [TestMethod()]
        public void SVGImageContainer_01_NoSVGSizes()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_01_NoSVGSizes.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImageContainer_01_NoSVGSizes.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);
            var svgs = GetSVGImageData(this.layout);
            
            Assert.AreEqual(4, svgs.Count, "Expected 4 SVG images");

            // 1. Sample_NoViewbox_NoWidth_NoHeight.svg has no sizing information of its own, so it's
            //    sized via SVGImageDataEmptySizer. The canvas's own content is now laid out at the
            //    current page size (not the 300x150 default) so content beyond 300x150 isn't
            //    permanently clipped regardless of how big any individual <img> referencing it ends
            //    up being (confirmed against real browser rendering - see
            //    SVGImageDataEmptySizer.GetContentLayoutSize). The *output box* (run0, below) still
            //    correctly defaults to 300x150 when nothing else is specified - only the internal
            //    canvas/XObject sizing changed.
            var run0 = GetImageRunFromBody(0);
            var pageSize = run0.GetLayoutPage().Size.Size;
            Assert.AreEqual(pageSize.Width,  svgs[0].Canvas.Width,  "1. Canvas width matches the page size");
            Assert.AreEqual(pageSize.Height, svgs[0].Canvas.Height, "1. Canvas height matches the page size");
            Assert.AreEqual(300.0, run0.Width.PointsValue,  1.0, "1. Run width from img override");
            Assert.AreEqual(150.0, run0.Height.PointsValue, 1.0, "1. Run height from img override");
            

            // 2. img 100×50 — canvas stays intrinsic, run matches override (same image data).
            var run1 = GetImageRunFromBody(1);
            Assert.AreEqual(100.0, run1.Width.PointsValue,  1.0, "2. Run width from img override");
            Assert.AreEqual(150.0, run1.Height.PointsValue, 1.0, "2. Run height from img override");
            
            // 3. img 200×100 — canvas stays intrinsic
            var run2 = GetImageRunFromBody(2);
            Assert.AreEqual(300.0, run2.Width.PointsValue,  1.0, "2. Run width from img override");
            Assert.AreEqual(100.0, run2.Height.PointsValue, 1.0, "2. Run height from img override");
            
            // 4. img 100×100 explicit — canvas stays intrinsic
            var run3 = GetImageRunFromBody(3);
            Assert.AreEqual(100.0, run3.Width.PointsValue,  1.0, "2. Run width from img override");
            Assert.AreEqual(100.0, run3.Height.PointsValue, 1.0, "2. Run height from img override");
            
            // 5. img 100×300 explicit — canvas stays intrinsic
            var run4 = GetImageRunFromBody(4);
            Assert.AreEqual(100.0, run4.Width.PointsValue,  1.0, "2. Run width from img override");
            Assert.AreEqual(300.0, run4.Height.PointsValue, 1.0, "2. Run height from img override");
            
            
            
            //second page
            
            // 6. img 200x200 explicit — canvas is new for aspect ratio. The SVG has no sizing
            //    information of its own (no viewBox, no width/height), so unlike a viewBox-bound
            //    canvas, there's no author-declared boundary to preserve - the canvas's own content
            //    is laid out at the page size (same as case 1 above), regardless of this
            //    particular <img>'s own 200x200 box, so content beyond 300x150 isn't clipped
            //    (confirmed against real browser rendering - see
            //    SVGImageDataEmptySizer.GetContentLayoutSize).
            Assert.AreEqual(pageSize.Width,  svgs[1].Canvas.Width,  "1. Canvas width matches the page size");
            Assert.AreEqual(pageSize.Height, svgs[1].Canvas.Height, "1. Canvas height matches the page size");
            
            var run5 = GetImageRunFromBody(5);
            Assert.AreEqual(200.0, run5.Width.PointsValue,  1.0, "2. Run width from img override");
            Assert.AreEqual(200.0, run5.Height.PointsValue, 1.0, "2. Run height from img override");
            
            var run6 = GetImageRunFromBody(6);
            Assert.AreEqual(200.0, run6.Width.PointsValue,  1.0, "2. Run width from img override");
            Assert.AreEqual(200.0, run6.Height.PointsValue, 1.0, "2. Run height from img override");
            
            var run7 = GetImageRunFromBody(7);
            Assert.AreEqual(200.0, run7.Width.PointsValue,  1.0, "2. Run width from img override");
            Assert.AreEqual(200.0, run7.Height.PointsValue, 1.0, "2. Run height from img override");
            
        }
        
         [TestMethod()]
        public void SVGImageContainer_02_SVGWithViewbox()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_02_SVGWithViewbox.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImageContainer_02_SVGWithViewbox.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);
            var svgs = GetSVGImageData(this.layout);
            
            Assert.AreEqual(4, svgs.Count, "Expected 4 SVG images");

            // 1. SVG 300×150 (viewBox), img 300×150 — canvas stays intrinsic, run matches override
            Assert.AreEqual((Unit)200, svgs[0].Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)150, svgs[0].Canvas.Height, "1. Canvas height stays intrinsic");
            var run0 = GetImageRunFromBody(0);
            var dynamicWidth = (run0.GetLayoutPage().Width.PointsValue) - 10.0;
            var dynamicHeight = (dynamicWidth / 200.0) * 150.0;
            Assert.AreEqual(dynamicWidth, run0.Width.PointsValue,  1.0, "1. Run width from img override");
            Assert.AreEqual(dynamicHeight, run0.Height.PointsValue, 1.0, "1. Run height from img override");
            
            
            // 2. img 100×50 — canvas stays intrinsic, run matches override (same image data).
            var run1 = GetImageRunFromBody(1);
            Assert.AreEqual(100.0, run1.Width.PointsValue,  1.0, "2. Run width from img override");
            Assert.AreEqual(75.0, run1.Height.PointsValue, 1.0, "2. Run height from img override");
            
            // 3. img 133.3×100 — canvas stays intrinsic
            var run2 = GetImageRunFromBody(2);
            Assert.AreEqual(133.3, run2.Width.PointsValue,  1.0, "3. Run width from img override");
            Assert.AreEqual(100.0, run2.Height.PointsValue, 1.0, "3. Run height from img override");
            
            // 4. img 100×100 explicit — canvas stays intrinsic
            var run3 = GetImageRunFromBody(3);
            Assert.AreEqual(100.0, run3.Width.PointsValue,  1.0, "4. Run width from img override");
            Assert.AreEqual(100.0, run3.Height.PointsValue, 1.0, "4. Run height from img override");
            
            //Second page
            
            // 5. img 100×300 explicit — canvas stays intrinsic
            var run4 = GetImageRunFromBody(4);
            Assert.AreEqual(100.0, run4.Width.PointsValue,  1.0, "5. Run width from img override");
            Assert.AreEqual(300.0, run4.Height.PointsValue, 1.0, "5. Run height from img override");
            
            // 6. img 400×100 explicit — canvas stays intrinsic
            var run5 = GetImageRunFromBody(5);
            Assert.AreEqual(400.0, run5.Width.PointsValue,  1.0, "6. Run width from img override");
            Assert.AreEqual(100.0, run5.Height.PointsValue, 1.0, "6. Run height from img override");
            
            //Third Page
            
            // 7. img 200 x 200 - No aspect (not checked here)
            var run6 = GetImageRunFromBody(6);
            Assert.AreEqual(200.0, run6.Width.PointsValue,  1.0, "7. Run width from img override");
            Assert.AreEqual(200.0, run6.Height.PointsValue, 1.0, "7. Run height from img override");
            
            // 8. img 200 x 200 - No aspect (not checked here)
            var run7 = GetImageRunFromBody(7);
            Assert.AreEqual(200.0, run7.Width.PointsValue,  1.0, "8. Run width from img override");
            Assert.AreEqual(200.0, run7.Height.PointsValue, 1.0, "8. Run height from img override");
            
            // 9. img 200 x 200 - No aspect (not checked here)
            var run8 = GetImageRunFromBody(8);
            Assert.AreEqual(200.0, run8.Width.PointsValue,  1.0, "9. Run width from img override");
            Assert.AreEqual(200.0, run8.Height.PointsValue, 1.0, "9. Run height from img override");
            
        }
        
         [TestMethod()]
        public void SVGImageContainer_03_SVGWithOnlySizes()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_03_SVGWithOnlySizes.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImageContainer_03_SVGWithOnlySizes.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);
            var svgs = GetSVGImageData(this.layout);
            
            Assert.AreEqual(12, svgs.Count, "Expected 12 SVG images");

            // 1. Sample_NoViewbox_HasWidth_NoHeight.svg
            // SVG 150×150 (size), img 150×150 — canvas stays 1:1, clipped
            
            var svg = svgs[0];
            var run = GetImageRunFromBody(0);
            
            Assert.AreEqual((Unit)150, svg.Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)150, svg.Canvas.Height, "1. Canvas height stays intrinsic");
            
            
            Assert.AreEqual(150, run.Width.PointsValue,  1.0, "1. Run width from img override");
            Assert.AreEqual(150, run.Height.PointsValue, 1.0, "1. Run height from img override");
            
            var matrix = svg.Sizer.GetCanvasToImageMatrix(new Size(svg.Canvas.Width, svg.Canvas.Height), Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);
            
            var scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, new Size(150, 150), context);
            Assert.AreEqual(new Size(1, 1), scale);
            
            // 2. Sample_NoViewbox_NoWidth_HasHeight.svg
            // SVG 300×100 — canvas stays intrinsic scale, run matches (same image data).
            svg = svgs[1];
            run = GetImageRunFromBody(1);
            
            Assert.AreEqual((Unit)300, svg.Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)100, svg.Canvas.Height, "1. Canvas height stays intrinsic");
            
            Assert.AreEqual(300.0, run.Width.PointsValue,  1.0, "2. Run width from img override");
            Assert.AreEqual(100.0, run.Height.PointsValue, 1.0, "2. Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(new Size(svg.Canvas.Width, svg.Canvas.Height), Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, new Size(300, 100), context);
            Assert.AreEqual(new Size(1, 1), scale);
            
            // 3. Sample_NoViewbox_HasWidth_HasHeight.svg
            // SVG 300×100 — canvas stays intrinsic scale, run matches (same image data).
            svg = svgs[2];
            run = GetImageRunFromBody(2);
            Assert.AreEqual((Unit)150, svg.Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)100, svg.Canvas.Height, "1. Canvas height stays intrinsic");
            
            run = GetImageRunFromBody(2);
            Assert.AreEqual(150.0, run.Width.PointsValue,  1.0, "2. Run width from img override");
            Assert.AreEqual(100.0, run.Height.PointsValue, 1.0, "2. Run height from img override");

            matrix = svg.Sizer.GetCanvasToImageMatrix(new Size(svg.Canvas.Width, svg.Canvas.Height), Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, new Size(150, 100), context);
            Assert.AreEqual(new Size(1, 1), scale);
            
            //Second page - widths with aspect ratio none
            
            // 4. Sample_NoViewbox_HasWidth_NoHeight_None.svg
            // SVG 150×150 (size), img 300×150 — canvas stretched horizontally 1.5:1, clipped
            svg = svgs[3];
            run = GetImageRunFromBody(3);
            Assert.AreEqual((Unit)150, svg.Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)150, svg.Canvas.Height, "1. Canvas height stays intrinsic");
            
            Assert.AreEqual(150, run.Width.PointsValue,  1.0, "1. Run width from img override");
            Assert.AreEqual(150, run.Height.PointsValue, 1.0, "1. Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(new Size(svg.Canvas.Width, svg.Canvas.Height), Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, new Size(300, 150), context);
            Assert.AreEqual(new Size(2.0, 1.0), scale);
            
            // 5. Sample_NoViewbox_HasWidth_NoHeight_Meet.svg
            // SVG 150×150 (size), img 300×150 — canvas stretched horizontally 1.5:1, clipped
            svg = svgs[4];
            run = GetImageRunFromBody(4);
            Assert.AreEqual((Unit)100, svg.Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)150, svg.Canvas.Height, "1. Canvas height stays intrinsic");
            
            Assert.AreEqual(100, run.Width.PointsValue,  1.0, "1. Run width from img override");
            Assert.AreEqual(150, run.Height.PointsValue, 1.0, "1. Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(new Size(svg.Canvas.Width, svg.Canvas.Height), Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, new Size(100, 150), context);
            Assert.AreEqual(new Size(1.0, 1.0), scale);
            
            // 6. Sample_NoViewbox_HasWidth_NoHeight_Slice.svg
            // SVG 150×150 (size), img 300×150 — canvas stretched horizontally 1.5:1, clipped
            svg = svgs[5];
            run = GetImageRunFromBody(5);
            Assert.AreEqual((Unit)300, svg.Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)150, svg.Canvas.Height, "1. Canvas height stays intrinsic");
            
            Assert.AreEqual(300, run.Width.PointsValue,  1.0, "1. Run width from img override");
            Assert.AreEqual(150, run.Height.PointsValue, 1.0, "1. Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(new Size(svg.Canvas.Width, svg.Canvas.Height), Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, new Size(300, 150), context);
            Assert.AreEqual(new Size(1.0, 1.0), scale);
            
            //page 3 - heights
            
            // 7. Sample_NoViewbox_HasWidth_NoHeight_None.svg
            // SVG 150×150 (size), img 300×150 — canvas stretched horizontally 1.5:1, clipped
            svg = svgs[6];
            run = GetImageRunFromBody(6);
            Assert.AreEqual((Unit)300, svg.Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)100, svg.Canvas.Height, "1. Canvas height stays intrinsic");
            
            Assert.AreEqual(300, run.Width.PointsValue,  1.0, "1. Run width from img override");
            Assert.AreEqual(100, run.Height.PointsValue, 1.0, "1. Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(new Size(svg.Canvas.Width, svg.Canvas.Height), Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, new Size(300, 150), context);
            Assert.AreEqual(new Size(1.0, 1.5), scale);
            
            // 8. Sample_NoViewbox_HasWidth_NoHeight_Meet.svg
            // SVG 150×150 (size), img 300×150 — canvas stretched horizontally 1.5:1, clipped
            svg = svgs[7];
            run = GetImageRunFromBody(7);
            Assert.AreEqual((Unit)300, svg.Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)100, svg.Canvas.Height, "1. Canvas height stays intrinsic");
            
            Assert.AreEqual(300, run.Width.PointsValue,  1.0, "1. Run width from img override");
            Assert.AreEqual(100, run.Height.PointsValue, 1.0, "1. Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(new Size(svg.Canvas.Width, svg.Canvas.Height), Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, new Size(300, 100), context);
            Assert.AreEqual(new Size(1.0, 1.0), scale);
            
            // 9. Sample_NoViewbox_HasWidth_NoHeight_Slice.svg
            // SVG 150×150 (size), img 300×150 — canvas stretched horizontally 1.5:1, clipped
            svg = svgs[8];
            run = GetImageRunFromBody(8);
            Assert.AreEqual((Unit)300, svg.Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)300, svg.Canvas.Height, "1. Canvas height stays intrinsic");
            
            Assert.AreEqual(300, run.Width.PointsValue,  1.0, "1. Run width from img override");
            Assert.AreEqual(300, run.Height.PointsValue, 1.0, "1. Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(new Size(300, 100), Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, new Size(300, 300), context);
            Assert.AreEqual(new Size(1.0, 1.0), scale);
            
            
            //page 4 both
            
            // 10. Sample_NoViewbox_HasWidth_HasHeight_None.svg
            // SVG 150×150 (size), img 300×150 — canvas stretched horizontally 1.5:1, clipped
            svg = svgs[9];
            run = GetImageRunFromBody(9);
            
            Assert.AreEqual((Unit)250, svg.Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)250, svg.Canvas.Height, "1. Canvas height stays intrinsic");
            
            Assert.AreEqual(250, run.Width.PointsValue,  1.0, "1. Run width from img override");
            Assert.AreEqual(250, run.Height.PointsValue, 1.0, "1. Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(new Size(svg.Canvas.Width, svg.Canvas.Height), Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, new Size(250, 250), context);
            Assert.AreEqual(new Size(1.0, 1.0), scale);
            
            // 11. Sample_NoViewbox_HasWidth_HasHeight_Meet.svg
            // SVG 150×150 (size), img 300×150 — canvas stretched horizontally 1.5:1, clipped
            svg = svgs[10];
            run = GetImageRunFromBody(10);
            Assert.AreEqual((Unit)250, svg.Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)250, svg.Canvas.Height, "1. Canvas height stays intrinsic");
            
            Assert.AreEqual(250, run.Width.PointsValue,  1.0, "1. Run width from img override");
            Assert.AreEqual(250, run.Height.PointsValue, 1.0, "1. Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(new Size(250, 250), Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, new Size(250, 250), context);
            Assert.AreEqual(new Size(1.0, 1.0), scale);
            
            // 12. Sample_NoViewbox_HasWidth_HasHeight_Slice.svg
            // SVG 150×150 (size), img 300×150 — canvas stretched horizontally 1.5:1, clipped
            svg = svgs[11];
            run = GetImageRunFromBody(11);
            Assert.AreEqual((Unit)250, svg.Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)200, svg.Canvas.Height, "1. Canvas height stays intrinsic");
            
            Assert.AreEqual(250, run.Width.PointsValue,  1.0, "1. Run width from img override");
            Assert.AreEqual(200, run.Height.PointsValue, 1.0, "1. Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(new Size(svg.Canvas.Width, svg.Canvas.Height), Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, new Size(250, 200), context);
            Assert.AreEqual(new Size(1.0, 1.0), scale);
            
            
            
        }
        
        
         [TestMethod()]
        public void SVGImageContainer_04_SVGWithSizesAndImgSizes()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_04_SVGWithSizesAndImgSizes.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImageContainer_04_SVGWithSizesAndImgSizes.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);
            var svgs = GetSVGImageData(this.layout);
            
            Assert.AreEqual(4, svgs.Count, "Expected 4 SVG images - default, none, meet, slice");

            //page 1 - Width and Height on SVG and various sizes.
            
            //1 Default
            var runIndex = 0;
            var expectedCanvas = new Size(150, 100);
            var expectedSize = new Size(150, 100);
            var expectedScale = new Size(1.0, 1.0);
            
            var svg = svgs[0];
            var run = GetImageRunFromBody(runIndex);
            
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width stays intrinsic");
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height stays intrinsic");
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, "1. Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, "1. Run height from img override");
            
            var matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);
            
            var scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale);


            //2: Half sized image
            
            runIndex = 1;
            
            expectedSize = new Size(75, 50);
            expectedScale = new Size(0.5, 0.5);
            
            svg = svg = svgs[0];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width stays intrinsic");
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height stays intrinsic");
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            
            //3: Half width, 1.5 height image
            
            runIndex = 2;
            
            expectedSize = new Size(75, 150);
            expectedScale = new Size(0.5, 1.5);
            
            svg = svg = svgs[0];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width stays intrinsic");
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height stays intrinsic");
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);

            //4: Double width image
            
            runIndex = 3;
            
            expectedSize = new Size(300, 100);
            expectedScale = new Size(2.0, 1.0);
            
            svg = svg = svgs[0];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width stays intrinsic");
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height stays intrinsic");
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);

            //5: Double both image
            
            runIndex = 4;
            
            expectedSize = new Size(300, 200);
            expectedScale = new Size(2.0, 2.0);
            
            svg = svg = svgs[0];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width stays intrinsic");
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height stays intrinsic");
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);

            
            //page 2 image sizes with aspect ratio NONE
            
            //6: Default 250x250 with ratio none
            runIndex = 5;
            
            expectedCanvas = new Size(250, 250);
            expectedSize = new Size(250, 250);
            expectedScale = new Size(1.0, 1.0);
            
            svg = svg = svgs[1];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            
            //7: Default 250x250 with 1/5th scale with ratio none
            runIndex = 6;
            
            expectedCanvas = new Size(250, 250);
            expectedSize = new Size(50, 50);
            expectedScale = new Size(0.2, 0.2);
            
            svg = svg = svgs[1];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //8: Default 250x250 with 2x width and 1/2 height scale with ratio none
            runIndex = 7;
            
            expectedCanvas = new Size(250, 250);
            expectedSize = new Size(500, 125);
            expectedScale = new Size(2, 0.5);
            
            svg = svg = svgs[1];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //9: Default 250x250 with 1/5th width and 1/2 height scale with ratio none
            runIndex = 8;
            
            expectedCanvas = new Size(250, 250);
            expectedSize = new Size(50, 125);
            expectedScale = new Size(0.2, 0.5);
            
            svg = svg = svgs[1];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //page 3 ratio Meet
            
            //10: Default 250x250 with no img sizes, 1:1 scale with ratio xMinYMax meet
            runIndex = 9;
            
            expectedCanvas = new Size(250, 250);
            expectedSize = new Size(250, 250);
            expectedScale = new Size(1, 1);
            
            svg = svg = svgs[2];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //11: Default 250x250 with 1/5th width and 1/2 height scale with ratio xMinYMax meet
            runIndex = 10;
            
            expectedCanvas = new Size(250, 250);
            expectedSize = new Size(50, 50);
            expectedScale = new Size(0.2, 0.2);
            
            svg = svg = svgs[2];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //12: Default 250x250 with 2x width and 1/2 height scale with ratio xMinYMax meet
            runIndex = 11;
            
            expectedCanvas = new Size(250, 250);
            expectedSize = new Size(500, 125);
            expectedScale = new Size(2, 0.5);
            
            svg = svg = svgs[2];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //13: Default 250x250 with 2x width and 1/2 height scale with ratio xMinYMax meet
            runIndex = 12;
            
            expectedCanvas = new Size(250, 250);
            expectedSize = new Size(50, 125);
            expectedScale = new Size(0.2, 0.5);
            
            svg = svg = svgs[2];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            
            //page 4 ratio Slice
            
            //14: Default 250x200 with no img sizes, 1:1 scale with ratio xMinYMax meet
            runIndex = 13;
            
            expectedCanvas = new Size(250, 200);
            expectedSize = new Size(250, 200);
            expectedScale = new Size(1, 1);
            
            svg = svg = svgs[3];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //15: Default 250x250 with 1/5th width and 1/2 height scale with ratio xMinYMax meet
            runIndex = 14;
            
            expectedCanvas = new Size(250, 200);
            expectedSize = new Size(50, 50);
            expectedScale = new Size(0.2, 0.25);
            
            svg = svg = svgs[3];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //16: Default 250x250 with 2x width and 1/2 height scale with ratio xMinYMax meet
            runIndex = 15;
            
            expectedCanvas = new Size(250, 200);
            expectedSize = new Size(500, 125);
            expectedScale = new Size(2, 125.0/200.0);
            
            svg = svg = svgs[3];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //17: Default 250x250 with 2x width and 1/2 height scale with ratio xMinYMax meet
            runIndex = 16;
            
            expectedCanvas = new Size(250, 200);
            expectedSize = new Size(50, 125);
            expectedScale = new Size(0.2, 125.0/200.0);
            
            svg = svg = svgs[3];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
        }

        [TestMethod()]
        public void SVGImageContainer_05_SVGWithSizesAndImgWidths()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_05_SVGWithSizesAndImgWidths.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImageContainer_05_SVGWithSizesAndImgWidths.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);
            var svgs = GetSVGImageData(this.layout);

            Assert.AreEqual(4, svgs.Count, "Expected 4 SVG images - default, none, meet, slice");

            //page 1 - Width and Height on SVG and various widths only.

            //1 Width 250
            var runIndex = 0;
            var expectedCanvas = new Size(150, 100);
            var expectedSize = new Size(150, 100);
            var expectedScale = new Size(1.0, 1.0);

            var svg = svgs[0];
            var run = GetImageRunFromBody(runIndex);


            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width, runIndex + ". Canvas width stays intrinsic");
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height stays intrinsic");

            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue, 1.0,
                "1. Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0,
                "1. Run height from img override");

            var matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);

            var scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale);


            //2: Width 75 (x0.5)
            runIndex = 1;
            
            expectedSize = new Size(75, 50);
            expectedScale = new Size(0.5, 0.5);
            
            svg = svg = svgs[0];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //3: Width 300 (x2)
            runIndex = 2;
            
            expectedSize = new Size(300, 200);
            expectedScale = new Size(2, 2);
            
            svg = svg = svgs[0];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //4: Width 75 None (x0.3)
            runIndex = 3;
            
            expectedCanvas = new Size(250, 250);
            expectedSize = new Size(75, 75);
            expectedScale = new Size(75.0 / 250.0, 75.0 / 250.0);
            
            svg = svg = svgs[1]; //new svg for None
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //5: Width 50 Meet (x0.2)
            runIndex = 4;
            
            expectedCanvas = new Size(250, 250);
            expectedSize = new Size(50, 50);
            expectedScale = new Size(50.0 / 250.0, 50.0 / 250.0);
            
            svg = svg = svgs[2]; //new svg for Meet
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //5: Width 25 Slice (x0.1)
            runIndex = 5;
            
            expectedCanvas = new Size(250, 200);
            expectedSize = new Size(25, 20);
            expectedScale = new Size(25.0 / 250.0, 20.0 / 200.0);
            
            svg = svg = svgs[3]; //new svg for Meet
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
        }
        
        
        [TestMethod()]
        public void SVGImageContainer_06_SVGWithSizesAndImgHeights()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_06_SVGWithSizesAndImgHeights.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImageContainer_06_SVGWithSizesAndImgHeights.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);
            var svgs = GetSVGImageData(this.layout);

            Assert.AreEqual(4, svgs.Count, "Expected 4 SVG images - default, none, meet, slice");

            //page 1 - Width and Height on SVG and various widths only.

            //1 Height 150
            var runIndex = 0;
            var expectedCanvas = new Size(150, 100);
            var expectedSize = new Size(150, 100);
            var expectedScale = new Size(1.0, 1.0);

            var svg = svgs[0];
            var run = GetImageRunFromBody(runIndex);


            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width, runIndex + ". Canvas width stays intrinsic");
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height stays intrinsic");

            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue, 1.0,
                "1. Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0,
                "1. Run height from img override");

            var matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);

            var scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale);


            //2: Height 75 (x0.5)
            runIndex = 1;
            
            expectedSize = new Size(75, 50);
            expectedScale = new Size(0.5, 0.5);
            
            svg = svg = svgs[0];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //3: Width 300 (x2)
            runIndex = 2;
            
            expectedSize = new Size(300, 200);
            expectedScale = new Size(2, 2);
            
            svg = svg = svgs[0];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //4: Width 75 None (x0.3)
            runIndex = 3;
            
            expectedCanvas = new Size(250, 250);
            expectedSize = new Size(75, 75);
            expectedScale = new Size(75.0 / 250.0, 75.0 / 250.0);
            
            svg = svg = svgs[1]; //new svg for None
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //5: Width 50 Meet (x0.2)
            runIndex = 4;
            
            expectedCanvas = new Size(250, 250);
            expectedSize = new Size(50, 50);
            expectedScale = new Size(50.0 / 250.0, 50.0 / 250.0);
            
            svg = svg = svgs[2]; //new svg for Meet
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //5: Width 25 Slice (x0.1)
            runIndex = 5;
            
            expectedCanvas = new Size(250, 200);
            expectedSize = new Size(31.25, 25);
            expectedScale = new Size(0.125, 0.125);
            
            svg = svg = svgs[3]; //new svg for Meet
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
        }


        [TestMethod()]
        public void SVGImageContainer_07_SVGWithPartialSizes_ImgSizes()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_07_SVGWithPartialSizes_ImgSizes.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImageContainer_07_SVGWithPartialSizes_ImgSizes.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);
            var svgs = GetSVGImageData(this.layout);

            Assert.AreEqual(4, svgs.Count, "Expected 4 SVG images - width, width_none, height, height_none");

            //page 1 - Width on SVG and various img widths.

            //1 img width 150
            var runIndex = 0;
            var expectedCanvas = new Size(150, 150);
            var expectedSize = new Size(150, 150);
            var expectedScale = new Size(1.0, 1.0);

            var svg = svgs[0];
            var run = GetImageRunFromBody(runIndex);


            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width, runIndex + ". Canvas width stays intrinsic");
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height stays intrinsic");

            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue, 1.0,
                "1. Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0,
                "1. Run height from img override");

            var matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);

            var scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale);

            //2: img width 75 (x0.2)
            runIndex = 1;
            
            expectedCanvas = new Size(150, 150);
            expectedSize = new Size(75, 150);
            expectedScale = new Size(0.5, 1.0);
            
            svg = svgs[0]; //new svg for Meet
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //3: img width 300 (x2)
            runIndex = 2;
            
            expectedCanvas = new Size(150, 150);
            expectedSize = new Size(300, 150);
            expectedScale = new Size(2.0, 1.0);
            
            svg = svgs[0];
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //4: img width 300 aspect none (x0.2)
            runIndex = 3;
            
            expectedCanvas = new Size(150, 150);
            expectedSize = new Size(300, 150);
            expectedScale = new Size(2.0, 1.0);
            
            svg = svgs[1]; //new svg for None
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            // page 2 - SVG Has Height, img has height
            
            //5: img height 100
            runIndex = 4;
            
            expectedCanvas = new Size(300, 100);
            expectedSize = new Size(300, 100);
            expectedScale = new Size(1.0, 1.0);
            
            svg = svgs[2]; //new svg for Height only
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //6: img height 50
            runIndex = 5;
            
            expectedCanvas = new Size(300, 100);
            expectedSize = new Size(300, 50);
            expectedScale = new Size(1.0, 0.5);
            
            svg = svgs[2]; //new svg for Height only
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //7: img height 200
            runIndex = 6;
            
            expectedCanvas = new Size(300, 100);
            expectedSize = new Size(300, 200);
            expectedScale = new Size(1.0, 2.0);
            
            svg = svgs[2]; //new svg for Height only
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            //8: img height 200, aspect none
            runIndex = 7;
            
            expectedCanvas = new Size(300, 100);
            expectedSize = new Size(300, 200);
            expectedScale = new Size(1.0, 2.0);
            
            svg = svgs[2]; //new svg for Height only
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);
            
            
            //8: img height 200, aspect none
            runIndex = 7;
            
            expectedCanvas = new Size(300, 100);
            expectedSize = new Size(300, 200);
            expectedScale = new Size(1.0, 2.0);
            
            svg = svgs[2]; //new svg for Height only
            run = GetImageRunFromBody(runIndex);
            
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,  runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height should be " +  expectedCanvas.Height);
            
            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue,  1.0, runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0, runIndex + ". Run height from img override");
            
            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");
            
            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale, runIndex + ". Image scale should match " + expectedScale);

        }

        [TestMethod()]
        public void SVGImageContainer_08_SVGWithViewboxAndSizes()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_08_SVGWithViewboxAndSizes.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImageContainer_08_SVGWithViewboxAndSizes.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);
            var svgs = GetSVGImageData(this.layout);

            Assert.AreEqual(8, svgs.Count, "Expected 8 SVG images");

            //page 1 - Width on SVG and various img widths.

            //1 img width 150
            var runIndex = 0;
            var expectedCanvas = new Size(200, 150);
            var expectedSize = new Size(200, 150);
            var expectedScale = new Size(1.0, 1.0);

            var svg = svgs[0];
            var run = GetImageRunFromBody(runIndex);


            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width, runIndex + ". Canvas width stays intrinsic");
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height, runIndex + ". Canvas height stays intrinsic");

            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue, 1.0,
                "1. Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0,
                "1. Run height from img override");

            var matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity);

            var scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale, scale);

            //2: svg height 75 (x0.2)
            runIndex = 1;
            
            //The scale of the view-box to the image size is set on the svg layout, not img
            expectedCanvas = new Size(100, 75);
            expectedSize = new Size(100, 75);
            expectedScale = new Size(1.0, 1.0);
            
            svg = svgs[1];
            run = GetImageRunFromBody(runIndex);

            AssertImageRun(svg, run, runIndex, expectedCanvas, expectedSize, expectedScale);
            
            //3: svg 100 x 75 (x0.5)
            runIndex = 2;

            //The scale of the view-box to the image size is set on the svg layout
            expectedCanvas = new Size(100, 75);
            expectedSize = new Size(100, 75);
            expectedScale = new Size(1.0, 1.0);

            svg = svgs[2]; 
            run = GetImageRunFromBody(runIndex);

            AssertImageRun(svg, run, runIndex, expectedCanvas, expectedSize, expectedScale);
            
            //4: svg 100 x 75 (x0.5)
            runIndex = 3;

            //The scale of the view-box to the image size is set on the svg layout
            expectedCanvas = new Size(200, 150);
            expectedSize = new Size(300, 225);
            expectedScale = new Size(1.5, 1.5);

            svg = svgs[0]; //back to width and height
            run = GetImageRunFromBody(runIndex);

            AssertImageRun(svg, run, runIndex, expectedCanvas, expectedSize, expectedScale);
            
            //5: svg 100 x 75 (x0.5)
            runIndex = 4;

            //The scale of the view-box to the image size is set on the svg layout
            expectedCanvas = new Size(200, 150);
            expectedSize = new Size(133, 100);
            expectedScale = new Size(0.665, 0.665);

            svg = svgs[0];
            run = GetImageRunFromBody(runIndex);

            AssertImageRun(svg, run, runIndex, expectedCanvas, expectedSize, expectedScale);
            
            //page 2 - all defined
            
            //6: svg 200 x 150, img: 150 x 75 (x0.5)
            runIndex = 5;

            //The scale of the view-box to the image size is set on the svg layout
            expectedCanvas = new Size(200, 150);
            expectedSize = new Size(150, 75);
            expectedScale = new Size(0.5, 0.5);
            var expectedOffset = new Point(25, 75);

            svg = svgs[0];
            run = GetImageRunFromBody(runIndex);

            AssertImageRun(svg, run, runIndex, expectedCanvas, expectedSize, expectedScale, expectedOffset);
            
            
            //7: svg 200 x 150, img: 100 x 150 centered
            runIndex = 6;

            //The scale of the view-box to the image size is set on the svg layout
            expectedCanvas = new Size(200, 150);
            expectedSize = new Size(100, 175);
            expectedScale = new Size(0.5, 0.5);
            expectedOffset = new Point(0, 125);

            svg = svgs[0]; 
            run = GetImageRunFromBody(runIndex);

            AssertImageRun(svg, run, runIndex, expectedCanvas, expectedSize, expectedScale, expectedOffset);

            //8: svg 200 x 150, img: 150 x 70 stretched
            runIndex = 7;

            //The scale of the view-box to the image size is set on the svg layout
            expectedCanvas = new Size(200, 150);
            expectedSize = new Size(150, 75);
            expectedScale = new Size(0.75, 0.5);
            expectedOffset = new Point(0, 75);

            svg = svgs[3]; //new svg for None
            run = GetImageRunFromBody(runIndex);

            AssertImageRun(svg, run, runIndex, expectedCanvas, expectedSize, expectedScale, expectedOffset);

            //9: svg 200 x 150, img: 100 x 175 stretched
            runIndex = 8;

            //The scale of the view-box to the image size is set on the svg layout
            expectedCanvas = new Size(200, 150);
            expectedSize = new Size(100, 175);
            expectedScale = new Size(0.5, 1.16667);
            expectedOffset = new Point(0, 175);

            svg = svgs[3]; //new svg for None
            run = GetImageRunFromBody(runIndex);

            AssertImageRun(svg, run, runIndex, expectedCanvas, expectedSize, expectedScale, expectedOffset);

            // page 3 meet aspect
            
            //10: svg 200 x 150, img: 150 x 75 meet TL
            runIndex = 9;

            //The scale of the view-box to the image size is set on the svg layout
            expectedCanvas = new Size(200, 150);
            expectedSize = new Size(150, 75);
            expectedScale = new Size(0.5, 0.5);
            expectedOffset = new Point(0, 75);

            svg = svgs[4]; //new svg for Meet TL
            run = GetImageRunFromBody(runIndex);

            AssertImageRun(svg, run, runIndex, expectedCanvas, expectedSize, expectedScale, expectedOffset);

            //11: svg 200 x 150, img: 100 x 175 meet TL
            runIndex = 10;

            //The scale of the view-box to the image size is set on the svg layout
            expectedCanvas = new Size(200, 150);
            expectedSize = new Size(100, 175);
            expectedScale = new Size(0.5, 0.5);
            expectedOffset = new Point(0, 75);

            svg = svgs[4]; //svg for Meet TL
            run = GetImageRunFromBody(runIndex);

            AssertImageRun(svg, run, runIndex, expectedCanvas, expectedSize, expectedScale, expectedOffset);

            //12: svg 200 x 150, img: 150 x 75 meet BR
            runIndex = 11;

            //The scale of the view-box to the image size is set on the svg layout
            expectedCanvas = new Size(200, 150);
            expectedSize = new Size(150, 75);
            expectedScale = new Size(0.5, 0.5);
            expectedOffset = new Point(50, 75);

            svg = svgs[5]; //svg for Meet BR
            run = GetImageRunFromBody(runIndex);

            AssertImageRun(svg, run, runIndex, expectedCanvas, expectedSize, expectedScale, expectedOffset);

            //13: svg 200 x 150, img: 100 x 175 meet BR
            runIndex = 12;

            //The scale of the view-box to the image size is set on the svg layout
            expectedCanvas = new Size(200, 150);
            expectedSize = new Size(100, 175);
            expectedScale = new Size(0.5, 0.5);
            expectedOffset = new Point(0, 175);

            svg = svgs[5]; //svg for Meet BR
            run = GetImageRunFromBody(runIndex);

            AssertImageRun(svg, run, runIndex, expectedCanvas, expectedSize, expectedScale, expectedOffset);

            //page 4 slice aspect

            //14: svg 200 x 150, img: 150 x 75 slice TL
            runIndex = 13;

            //The scale of the view-box to the image size is set on the svg layout
            expectedCanvas = new Size(200, 150);
            expectedSize = new Size(150, 75);
            expectedScale = new Size(0.75, 0.75);
            expectedOffset = new Point(0, 150 * 0.75);

            svg = svgs[6]; //svg for Slice TL
            run = GetImageRunFromBody(runIndex);

            AssertImageRun(svg, run, runIndex, expectedCanvas, expectedSize, expectedScale, expectedOffset);

            //15: svg 200 x 150, img: 100 x 175 slice tl
            runIndex = 14;

            //The scale of the view-box to the image size is set on the svg layout
            expectedCanvas = new Size(200, 150);
            expectedSize = new Size(100, 175);
            expectedScale = new Size(1.1667, 1.1667);
            expectedOffset = new Point(0, 175);

            svg = svgs[6]; //svg for Slice TL
            run = GetImageRunFromBody(runIndex);

            AssertImageRun(svg, run, runIndex, expectedCanvas, expectedSize, expectedScale, expectedOffset);

            //16: svg 200 x 150, img: 150 x 75 slice BR
            runIndex = 15;

            //The scale of the view-box to the image size is set on the svg layout
            expectedCanvas = new Size(200, 150);
            expectedSize = new Size(150, 75);
            expectedScale = new Size(0.75, 0.75);
            expectedOffset = new Point(0, 75);

            svg = svgs[7]; //svg for Slice BR
            run = GetImageRunFromBody(runIndex);

            AssertImageRun(svg, run, runIndex, expectedCanvas, expectedSize, expectedScale, expectedOffset);

            //17: svg 200 x 150, img: 100 x 175 slice BR
            runIndex = 14;

            //The scale of the view-box to the image size is set on the svg layout
            expectedCanvas = new Size(200, 150);
            expectedSize = new Size(100, 175);
            expectedScale = new Size(1.1667, 1.1667);
            expectedOffset = new Point(-133, 175);

            svg = svgs[7]; //svg for Slice TL
            run = GetImageRunFromBody(runIndex);

            AssertImageRun(svg, run, runIndex, expectedCanvas, expectedSize, expectedScale, expectedOffset);

        }

        private void AssertImageRun(SVGPDFImageData svg, PDFLayoutComponentRun run, int runIndex, Size expectedCanvas,
            Size expectedSize, Size expectedScale, Point? expectedOffset = null)
        {
            PDFTransformationMatrix matrix;
            Size scale;
            Assert.AreEqual(expectedCanvas.Width, svg.Canvas.Width,
                runIndex + ". Canvas width should be " + expectedCanvas.Width);
            Assert.AreEqual(expectedCanvas.Height, svg.Canvas.Height,
                runIndex + ". Canvas height should be " + expectedCanvas.Height);

            Assert.AreEqual(expectedSize.Width.PointsValue, run.Width.PointsValue, 1.0,
                runIndex + ". Run width from img override");
            Assert.AreEqual(expectedSize.Height.PointsValue, run.Height.PointsValue, 1.0,
                runIndex + ". Run height from img override");

            matrix = svg.Sizer.GetCanvasToImageMatrix(expectedSize, Point.Empty, context);
            Assert.IsTrue(matrix.IsIdentity, runIndex + ". Canvas to Image matrix should be identity");

            scale = svg.Sizer.GetRenderScaleForContent(Point.Empty, expectedSize, context);
            Assert.AreEqual(expectedScale.Width.PointsValue, scale.Width.PointsValue, 0.01, runIndex + ". Image scale should match " + expectedScale);
            Assert.AreEqual(expectedScale.Height.PointsValue, scale.Height.PointsValue, 0.01, runIndex + ". Image scale should match " + expectedScale);

            if (expectedOffset != null)
            {
                var actOffset = svg.Sizer.GetRenderOffsetForContent(Point.Empty, expectedSize, context);
                Assert.AreEqual(expectedOffset.Value.X.PointsValue, actOffset.X.PointsValue, 1, runIndex + ". Offset x should match");
                Assert.AreEqual(expectedOffset.Value.Y.PointsValue, actOffset.Y.PointsValue, 1, runIndex + ". Offset y should match");

            }
        }


        // -----------------------------------------------------------------------
        // Group: percentage width/height on the SVG element itself
        // -----------------------------------------------------------------------

        [TestMethod()]
        public void SVGImageContainer_PercentDims()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_PercentDims.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImageContainer_PercentDims.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);
            var svgs = GetSVGImageData(this.layout);
            Assert.AreEqual(4, svgs.Count, "Expected 4 SVG images");
            
            const double Delta = 0.1;
            // 1. width=50% → intrinsic width=100 (50% of vb 200), height=75 (proportional from vb AR)
            //    No img override → run matches intrinsic.

            var size0 = svgs[0].Sizer.GetLayoutSize();
            Assert.AreEqual(300.0, size0.Width.PointsValue,  Delta, "1. Sizer width: 50% of viewBox W=200");
            Assert.AreEqual(225.0,  size0.Height.PointsValue, Delta, "1. Sizer height: proportional 100*(150/200)");
            var run0 = GetImageRunFromBody(0);
            Assert.AreEqual(300.0, run0.Width.PointsValue,  Delta, "1. Run width matches intrinsic");
            Assert.AreEqual(225.0,  run0.Height.PointsValue, Delta, "1. Run height matches intrinsic");
            
            // 2. height=50% → intrinsic height=75 (50% of vb 150), width=100 (proportional from vb AR)
            //    No img override → run matches intrinsic.
            var size1 = svgs[1].Sizer.GetLayoutSize();
            Assert.AreEqual(100.0, size1.Width.PointsValue,  Delta, "2. Sizer width: proportional 75*(200/150)");
            Assert.AreEqual(75.0,  size1.Height.PointsValue, Delta, "2. Sizer height: 50% of viewBox H=150");
            var run1 = GetImageRunFromBody(1);
            Assert.AreEqual(100.0, run1.Width.PointsValue,  Delta, "2. Run width matches intrinsic");
            Assert.AreEqual(75.0,  run1.Height.PointsValue, Delta, "2. Run height matches intrinsic");
            
            // 3. width=75%, height=100% → intrinsic 150×150 (non-square)
            //    No img override → run matches intrinsic.
            var size2 = svgs[2].Sizer.GetLayoutSize();
            Assert.AreEqual(150.0, size2.Width.PointsValue,  Delta, "3. Sizer width: 75% of viewBox W=200");
            Assert.AreEqual(150.0, size2.Height.PointsValue, Delta, "3. Sizer height: 100% of viewBox H=150");
            var run2 = GetImageRunFromBody(2);
            Assert.AreEqual(150.0, run2.Width.PointsValue,  Delta, "3. Run width matches intrinsic");
            Assert.AreEqual(150.0, run2.Height.PointsValue, Delta, "3. Run height matches intrinsic");
            
            // 4. width=50% → intrinsic 100×75; img override width=200pt → run 200×150 (proportional from intrinsic AR)
            var size3 = svgs[3].Sizer.GetLayoutSize();
            Assert.AreEqual(100.0, size3.Width.PointsValue,  Delta, "4. Sizer width: 50% of viewBox W=200 (intrinsic)");
            Assert.AreEqual(75.0,  size3.Height.PointsValue, Delta, "4. Sizer height: proportional (intrinsic)");
            var run3 = GetImageRunFromBody(3);
            Assert.AreEqual(400.0, run3.Width.PointsValue,  Delta, "4. Run width from img override");
            Assert.AreEqual(200.0, run3.Height.PointsValue, Delta, "4. Run height proportional from intrinsic: 200*(75/100)");
            
        }
        
        // -----------------------------------------------------------------------
        // Group: Logo all variations
        // -----------------------------------------------------------------------

        [TestMethod()]
        public void SVGLogo_Various_Sizes()
        {
            var path = GetResourcePath("SVGImages", "Logo_VariousSizes.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGLogo_VariousSizes.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);
            var svgs = GetSVGImageData(this.layout);
            Assert.AreEqual(7, svgs.Count, "Expected 4 SVG images");

        }

        [TestMethod()]
        public void AspectRatio_01_SVGWithStyledDimension()
        {
            var path = GetResourcePath("SVGImages", "AspectRatio_01_SVGWithStyledDimension.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImages_AspectRatio_01_SVGWithStyledDimension.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);

            //0. height:60pt styled, with 300x100 (3:1) intrinsic attributes providing the ratio - overriding the
            //   SVG canvas's own default (undimensioned) 2:1 intrinsic ratio. Width should be 60 * 3 = 180pt.
            var run0 = GetImageRunFromBody(0);
            Assert.AreEqual(180.0, run0.Width.PointsValue, 1.0, "Width should be derived from the 300x100 (3:1) intrinsic attribute ratio");
            Assert.AreEqual(60.0, run0.Height.PointsValue, 1.0, "Height should be the explicit 60pt style value");

            //1. Explicit CSS aspect-ratio (1/2) takes priority over the 300x100 (3:1) intrinsic attribute ratio.
            //   Width should be 60 * 0.5 = 30pt.
            var run1 = GetImageRunFromBody(1);
            Assert.AreEqual(30.0, run1.Width.PointsValue, 1.0, "Width should be derived from the explicit CSS aspect-ratio of 1:2");
            Assert.AreEqual(60.0, run1.Height.PointsValue, 1.0, "Height should be the explicit 60pt style value");

            //2. Logo_W_Only.svg (own width, no height, no viewBox) exercises SVGImageDataOnlyWHSizer.
            //   width:60pt styled, 300x100 (3:1) intrinsic attributes provide the ratio. Height should be 60 / 3 = 20pt.
            var run2 = GetImageRunFromBody(2);
            Assert.AreEqual(60.0, run2.Width.PointsValue, 1.0, "Width should be the explicit 60pt style value");
            Assert.AreEqual(20.0, run2.Height.PointsValue, 1.0, "Height should be derived from the 300x100 (3:1) intrinsic attribute ratio");

            //3. Logo_VP_W_Only.svg (viewBox + own width, no height) exercises SVGImageDataVPAndWHSizer.
            //   width:90pt styled, 300x100 (3:1) intrinsic attributes provide the ratio, overriding the SVG's own
            //   viewBox-derived ratio. Height should be 90 / 3 = 30pt.
            var run3 = GetImageRunFromBody(3);
            Assert.AreEqual(90.0, run3.Width.PointsValue, 1.0, "Width should be the explicit 90pt style value");
            Assert.AreEqual(30.0, run3.Height.PointsValue, 1.0, "Height should be derived from the 300x100 (3:1) intrinsic attribute ratio");
        }

        [TestMethod()]
        public void MinMaxSize_01_SVG_ParityWithRasterImages()
        {
            // Confirms min-width/max-width/min-height/max-height are honoured for an unsized
            // SVG-sourced <img> (natural size 300x150, no viewBox). Verified against real browser
            // rendering: min/max-width/height on an unsized SVG resize the box independently, with
            // NO proportional coupling to the other dimension - the SVG content renders at its
            // natural scale within that box (padded when the box grows, clipped when it shrinks),
            // rather than being stretched/scaled to match.
            var path = GetResourcePath("SVGImages", "MinMaxSize_01_SVG.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImages_MinMaxSize_01_SVG.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }
            

            Assert.IsNotNull(this.layout);

            // 0. min-width:400pt, natural 300x150 -> w=400 (clamped), h=150 (untouched)
            var run0 = GetImageRunFromBody(0);
            Assert.AreEqual(400.0, run0.Width.PointsValue,  1.0, "min-width should grow the box width");
            Assert.AreEqual(150.0, run0.Height.PointsValue, 1.0, "height should stay at its natural size, not scale with width");

            // 1. max-width:150pt, natural 300x150 -> w=150 (clamped), h=150 (untouched)
            var run1 = GetImageRunFromBody(1);
            Assert.AreEqual(150.0, run1.Width.PointsValue, 1.0, "max-width should shrink the box width");
            Assert.AreEqual(150.0, run1.Height.PointsValue, 1.0, "height should stay at its natural size, not scale with width");

            // 2. min-height:300pt, natural 300x150 -> h=300 (clamped), w=300 (untouched)
            var run2 = GetImageRunFromBody(2);
            Assert.AreEqual(300.0, run2.Width.PointsValue,  1.0, "width should stay at its natural size, not scale with height");
            Assert.AreEqual(300.0, run2.Height.PointsValue, 1.0, "min-height should grow the box height");

            // 3. max-height:50pt, natural 300x150 -> h=50 (clamped), w=300 (untouched)
            var run3 = GetImageRunFromBody(3);
            Assert.AreEqual(300.0, run3.Width.PointsValue, 1.0, "width should stay at its natural size, not scale with height");
            Assert.AreEqual(50.0,  run3.Height.PointsValue, 1.0, "max-height should shrink the box height");

            // 4. min-width:350pt and min-height:200pt together, natural 300x150 -> each dimension
            //    clamps independently: w=350, h=200.
            var run4 = GetImageRunFromBody(4);
            Assert.AreEqual(350.0, run4.Width.PointsValue,  1.0, "min-width should grow the box width independently");
            Assert.AreEqual(200.0, run4.Height.PointsValue, 1.0, "min-height should grow the box height independently");

            // 5. explicit width:370pt height:350pt, both bigger than the natural 300x150. Unlike
            //    min/max, the SVG canvas itself must be sized to match this explicit box - not left
            //    at the 300x150 default - otherwise content beyond 300x150 (the rect drawn at
            //    5,5 350x320 in Logo_None.svg) would be silently clipped regardless of how big the
            //    box actually is. Confirmed against real browser rendering.
            var run5 = GetImageRunFromBody(5);
            Assert.AreEqual(370.0, run5.Width.PointsValue,  1.0, "explicit width should set the box width");
            Assert.AreEqual(350.0, run5.Height.PointsValue, 1.0, "explicit height should set the box height");

        }

        [TestMethod()]
        public void MinMaxSize_02_ExplicitSizeBiggerThanDefault_XObjectBBoxNotClippedTo300x150()
        {
            // The actual PDF-level clip boundary for a dimensionless SVG (no width/height/viewBox
            // of its own) is the /BBox entry on the shared Form XObject wrapping its content -
            // SVGImageDataEmptySizer.DoGetImageToCanvasBBox - not anything about the SVGCanvas's own
            // layout size. That must not be fixed at the 300x150 default regardless of how big any
            // individual <img> referencing it ends up being (confirmed against real browser
            // rendering, using a rect in Logo_None.svg that extends past 300x150). Verified here by
            // reading the actual /BBox written into the rendered PDF bytes, since PDFRenderContext
            // (needed to call the sizer method directly) has an internal-only constructor.
            var path = GetResourcePath("SVGImages", "MinMaxSize_02_ExplicitSizeBiggerThanDefault.html");

            using var doc = Document.ParseDocument(path);

            var stream = DocStreams.GetOutputStream("SVGImages_MinMaxSize_02_ExplicitSize.pdf");
            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            doc.SaveAsPDF(stream);

            Assert.IsNotNull(this.layout);

            var run = GetImageRunFromBody(0);
            Assert.AreEqual(370.0, run.Width.PointsValue,  1.0, "explicit width should set the box width");
            Assert.AreEqual(350.0, run.Height.PointsValue, 1.0, "explicit height should set the box height");

            stream.Position = 0;
            using var reader = new System.IO.StreamReader(stream, System.Text.Encoding.Latin1);
            var pdfText = reader.ReadToEnd();

            var matches = System.Text.RegularExpressions.Regex.Matches(pdfText,
                @"/BBox\s*\[\s*([\d.\-]+)\s+([\d.\-]+)\s+([\d.\-]+)\s+([\d.\-]+)\s*\]");
            Assert.IsTrue(matches.Count >= 2, "Expected at least 2 /BBox entries in the rendered PDF (inner content block + outer wrapper)");

            // Both the inner content block's own Form XObject and the outer wrapper must be sized
            // to the page (500x700), not the 300x150 default - otherwise content beyond 300x150
            // (the rect in Logo_None.svg) is clipped by whichever of the two is still 300x150,
            // regardless of the other being fixed.
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                var bboxWidth  = double.Parse(match.Groups[3].Value, System.Globalization.CultureInfo.InvariantCulture);
                var bboxHeight = double.Parse(match.Groups[4].Value, System.Globalization.CultureInfo.InvariantCulture);

                Assert.AreEqual(500.0, bboxWidth,  1.0, $"BBox width should match the page width (500pt): {match.Value}");
                Assert.AreEqual(700.0, bboxHeight, 1.0, $"BBox height should match the page height (700pt): {match.Value}");
            }
        }

        [TestMethod()]
        public void MinMaxSize_03_ViewBox_AspectRatioPreservedOnSingleAxis()
        {
            // Sample_ViewboxOnly_200x150.svg has viewBox="0 0 200 150" (4:3, 1.333 ratio) and no
            // width/height/aspect-ratio CSS of its own. Confirmed against real browser rendering:
            // min/max-width/height alone (no explicit width/height/aspect-ratio) derive the
            // unconstrained axis from the viewBox's own natural ratio, unless both axes end up
            // independently constrained by their own min/max, in which case the ratio can break -
            // mirroring the CSS aspect-ratio behaviour already confirmed for SVGImageDataEmptySizer.
            var path = GetResourcePath("SVGImages", "MinMaxSize_03_ViewBox.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImages_MinMaxSize_03_ViewBox.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);

            // 0. min-width:400pt in a 150pt-wide container -> fill(150,112.5), w=400, h=300
            var run0 = GetImageRunFromBody(0);
            Assert.AreEqual(400.0, run0.Width.PointsValue,  1.0, "min-width should grow the width");
            Assert.AreEqual(300.0, run0.Height.PointsValue, 1.0, "height should derive from the viewBox's 4:3 ratio");

            // 1. max-width:100pt, natural fill (wider than 100pt) -> w=100, h=75
            var run1 = GetImageRunFromBody(1);
            Assert.AreEqual(100.0, run1.Width.PointsValue, 1.0, "max-width should shrink the width");
            Assert.AreEqual(75.0,  run1.Height.PointsValue, 1.0, "height should derive from the viewBox's 4:3 ratio");

            // 2. min-height:300pt in the same 150pt-wide container -> fill height (112.5) is
            //    already shorter than 300pt, so min-height binds -> h=300, w=400
            var run2 = GetImageRunFromBody(2);
            Assert.AreEqual(400.0, run2.Width.PointsValue,  1.0, "width should derive from the viewBox's 4:3 ratio");
            Assert.AreEqual(300.0, run2.Height.PointsValue, 1.0, "min-height should grow the height");

            // 3. max-height:50pt, natural fill (taller than 50pt) -> h=50, w=66.67
            var run3 = GetImageRunFromBody(3);
            Assert.AreEqual(66.7, run3.Width.PointsValue,  1.0, "width should derive from the viewBox's 4:3 ratio");
            Assert.AreEqual(50.0, run3.Height.PointsValue, 1.0, "max-height should shrink the height");

            // 4. min-width:250pt and min-height:200pt together in the 150pt-wide container - both
            //    axes independently constrained, so the ratio is allowed to break: w=250, h=200.
            var run4 = GetImageRunFromBody(4);
            Assert.AreEqual(250.0, run4.Width.PointsValue,  1.0, "min-width should grow the width independently");
            Assert.AreEqual(200.0, run4.Height.PointsValue, 1.0, "min-height should grow the height independently");
        }

    }
}
