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
using Scryber.PDF.Resources;
using Scryber.Svg.Imaging;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Tests the layout of SVG Images based on image sizes and inner svg dimensions and view-box
    /// </summary>
    [TestClass()]
    public class SVGImageLayout_Tests
    {
        
        // -----------------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------------

        PDFLayoutDocument layout;

        
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

        /// ----------------------------------
        /// Test methods
        /// ----------------------------------
        
        [TestMethod()]
        public void SVGImageContainer_NoSVGSizes()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_NoSVGSizes.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImageContainer_NoSVGSizes.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);
            var svgs = GetSVGImageData(this.layout);
            
            Assert.AreEqual(4, svgs.Count, "Expected 4 SVG images");

            // 1. SVG 300×150 (viewBox), img 300×150 — canvas stays intrinsic, run matches override
            Assert.AreEqual((Unit)300, svgs[0].Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)150, svgs[0].Canvas.Height, "1. Canvas height stays intrinsic");
            var run0 = GetImageRunFromBody(0);
            Assert.AreEqual(300.0, run0.Width.PointsValue,  1.0, "1. Run width from img override");
            Assert.AreEqual(150.0, run0.Height.PointsValue, 1.0, "1. Run height from img override");
            

            // 2. img 100×50 — canvas stays intrinsic, run matches override (same image data).
            var run1 = GetImageRunFromBody(1);
            Assert.AreEqual(100.0, run1.Width.PointsValue,  1.0, "2. Run width from img override");
            Assert.AreEqual(50.0, run1.Height.PointsValue, 1.0, "2. Run height from img override");
            
            // 3. img 200×100 — canvas stays intrinsic
            var run2 = GetImageRunFromBody(2);
            Assert.AreEqual(200.0, run2.Width.PointsValue,  1.0, "2. Run width from img override");
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
            
            // 6. img 200x200 explicit — canvas is new for aspect ratio
            Assert.AreEqual((Unit)300, svgs[1].Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)150, svgs[1].Canvas.Height, "1. Canvas height stays intrinsic");
            
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
        public void SVGImageContainer_SVGWithViewbox()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_SVGWithViewbox.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImageContainer_SVGWithViewbox.pdf"))
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
        
        /// <summary>
        /// These test for the size of the svg itself, there is no view box defined so scale should be 1:1.
        /// The width and the height will simply clip or extend beyond the drawing space.
        /// Default width and height of 300x150.
        /// </summary>
        [TestMethod()]
        public void SVGImageContainer_NoImgSize_VariousSVGDimensions()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_NoImgSizes.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("SVGImageContainer_NoImgSizes.pdf"))
                {
                    //doc.AppendTraceLog = true;
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);
                }
            }
            
            Assert.IsNotNull(this.layout, "SVG Image layout not found");
            var rsrc = this.layout.DocumentComponent.SharedResources;

            var expectedCount = (5 * 2) + 1; //5 svg images and 1 font
            
            Assert.AreEqual(11, rsrc.Count, "Expected 11 resources");
            
            // default image (200 x 150, vp: 0 0 200 150)
            Unit expectedWidth = 200;
            Unit expectedHeight = 150;
            
            var imgXObject= rsrc.ElementAt(0) as PDFImageXObject;
            var layout =  rsrc.ElementAt(6) as PDFLayoutXObjectResource;

            Assert.IsNotNull(imgXObject, "1. SVG Image xObject not found");
            Assert.IsNotNull(layout, "1. SVG Image layout not found");

            var proxy = imgXObject.ImageData as ImageDataProxy;
            Assert.IsNotNull(proxy,  "1. SVG Image proxy not found");

            var svgData = proxy.ImageData as SVGPDFImageData;
            Assert.IsNotNull(svgData,  "1. SVG Image data not found");
            
            Assert.IsNotNull(svgData.Canvas);
            Assert.AreEqual(expectedWidth, svgData.Canvas.Width, "1. Expected Width doesn't match");
            Assert.AreEqual(expectedHeight, svgData.Canvas.Height, "1. Expected Height doesn't match");
            
            //Second image - square 150, 150 with width only
            expectedWidth = 150;
            expectedHeight = 150;
            
            imgXObject= rsrc.ElementAt(1) as PDFImageXObject;
            layout =  rsrc.ElementAt(7) as PDFLayoutXObjectResource;

            Assert.IsNotNull(imgXObject, "2. SVG Image xObject not found");
            Assert.IsNotNull(layout, "2. SVG Image layout not found");

            proxy = imgXObject.ImageData as ImageDataProxy;
            Assert.IsNotNull(proxy,  "2. SVG Image proxy not found");

            svgData = proxy.ImageData as SVGPDFImageData;
            Assert.IsNotNull(svgData,  "2. SVG Image data not found");
            
            Assert.IsNotNull(svgData.Canvas);
            Assert.AreEqual(expectedWidth, svgData.Canvas.Width, "2 Expected Width doesn't match");
            Assert.AreEqual(expectedHeight, svgData.Canvas.Height, "2. Expected Height doesn't match");

            //Third image explicit 100 high
            expectedWidth = 300;
            expectedHeight = 100;
            
            imgXObject= rsrc.ElementAt(2) as PDFImageXObject;
            layout =  rsrc.ElementAt(8) as PDFLayoutXObjectResource;

            Assert.IsNotNull(imgXObject, "3. SVG Image xObject not found");
            Assert.IsNotNull(layout, "3. SVG Image layout not found");

            proxy = imgXObject.ImageData as ImageDataProxy;
            Assert.IsNotNull(proxy,  "3. SVG Image proxy not found");

            svgData = proxy.ImageData as SVGPDFImageData;
            Assert.IsNotNull(svgData,  "3. SVG Image data not found");
            
            Assert.IsNotNull(svgData.Canvas);
            Assert.AreEqual(expectedWidth, svgData.Canvas.Width, "3. Expected Width doesn't match");
            Assert.AreEqual(expectedHeight, svgData.Canvas.Height, "3. Expected Height doesn't match");

            //Fourth image explicit 100 high and 150 wide
            expectedWidth = 150;
            expectedHeight = 100;
            
            imgXObject= rsrc.ElementAt(3) as PDFImageXObject;
            layout =  rsrc.ElementAt(9) as PDFLayoutXObjectResource;

            Assert.IsNotNull(imgXObject, "4. SVG Image xObject not found");
            Assert.IsNotNull(layout, "4. SVG Image layout not found");

            proxy = imgXObject.ImageData as ImageDataProxy;
            Assert.IsNotNull(proxy,  "4. SVG Image proxy not found");

            svgData = proxy.ImageData as SVGPDFImageData;
            Assert.IsNotNull(svgData,  "4. SVG Image data not found");
            
            Assert.IsNotNull(svgData.Canvas);
            Assert.AreEqual(expectedWidth, svgData.Canvas.Width, "4. Expected Width doesn't match");
            Assert.AreEqual(expectedHeight, svgData.Canvas.Height, "4. Expected Height doesn't match");

            
            //Fifth image explicit no dimensions (default 300 x 150)
            expectedWidth = 300;
            expectedHeight = 150;
            
            imgXObject= rsrc.ElementAt(4) as PDFImageXObject;
            layout =  rsrc.ElementAt(10) as PDFLayoutXObjectResource;

            Assert.IsNotNull(imgXObject, "5. SVG Image xObject not found");
            Assert.IsNotNull(layout, "5. SVG Image layout not found");

            proxy = imgXObject.ImageData as ImageDataProxy;
            Assert.IsNotNull(proxy,  "5. SVG Image proxy not found");

            svgData = proxy.ImageData as SVGPDFImageData;
            Assert.IsNotNull(svgData,  "5. SVG Image data not found");
            
            Assert.IsNotNull(svgData.Canvas);
            Assert.AreEqual(expectedWidth, svgData.Canvas.Width, "5. Expected Width doesn't match");
            Assert.AreEqual(expectedHeight, svgData.Canvas.Height, "5. Expected Height doesn't match");

        }
        

        /// <summary>
        /// Various viewbox sizes with contents appropriately sized for the viewbox. No other dimensions specified, so the viewbox width and
        /// height are used as the size of the canvas.
        /// </summary>
        [TestMethod()]
        public void SVGImageContainer_NoImgSize_ViewboxVariants()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_NoImgSize_ViewboxVariants.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImageContainer_NoImgSize_ViewboxVariants.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);
            var svgs = GetSVGImageData(this.layout);
            Assert.AreEqual(3, svgs.Count, "Expected 3 SVG images");

            // 1. viewBox 200×150 — canvas and run both 200×150
            Assert.AreEqual((Unit)200, svgs[0].Canvas.Width,  "1. Canvas width from viewBox");
            Assert.AreEqual((Unit)150, svgs[0].Canvas.Height, "1. Canvas height from viewBox");
            var run0 = GetImageRunFromBody(0);
            Assert.AreEqual(200.0, run0.Width.PointsValue,  1.0, "1. Run width");
            Assert.AreEqual(150.0, run0.Height.PointsValue, 1.0, "1. Run height");

            // 2. viewBox 400×150 — canvas and run both 400×150
            Assert.AreEqual((Unit)400, svgs[1].Canvas.Width,  "2. Canvas width from viewBox");
            Assert.AreEqual((Unit)150, svgs[1].Canvas.Height, "2. Canvas height from viewBox");
            var run1 = GetImageRunFromBody(1);
            Assert.AreEqual(400.0, run1.Width.PointsValue,  1.0, "2. Run width");
            Assert.AreEqual(150.0, run1.Height.PointsValue, 1.0, "2. Run height");

            // 3. viewBox 150×400 — canvas and run both 150×400
            Assert.AreEqual((Unit)150, svgs[2].Canvas.Width,  "3. Canvas width from viewBox");
            Assert.AreEqual((Unit)400, svgs[2].Canvas.Height, "3. Canvas height from viewBox");
            var run2 = GetImageRunFromBody(2);
            Assert.AreEqual(150.0, run2.Width.PointsValue,  1.0, "3. Run width");
            Assert.AreEqual(400.0, run2.Height.PointsValue, 1.0, "3. Run height");
        }

        // -----------------------------------------------------------------------
        // Group: img width override only — height scales proportionally
        // -----------------------------------------------------------------------

        [TestMethod()]
        public void SVGImageContainer_ImgWidthOnly()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_ImgWidthOnly.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImageContainer_ImgWidthOnly.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);
            var svgs = GetSVGImageData(this.layout);
            Assert.AreEqual(3, svgs.Count, "Expected 3 SVG images");

            // 1. SVG 150×100, img width=300 → canvas 150×100, run 300×200
            Assert.AreEqual((Unit)150, svgs[0].Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)100, svgs[0].Canvas.Height, "1. Canvas height stays intrinsic");
            var run0 = GetImageRunFromBody(0);
            Assert.AreEqual(300.0, run0.Width.PointsValue,  1.0, "1. Run width overridden");
            Assert.AreEqual(200.0, run0.Height.PointsValue, 1.0, "1. Run height proportional (300 * 100/150)");

            // 2. SVG 200×150 (viewBox), img width=400 → canvas 200×150, run 400×300
            Assert.AreEqual((Unit)200, svgs[1].Canvas.Width,  "2. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)150, svgs[1].Canvas.Height, "2. Canvas height stays intrinsic");
            var run1 = GetImageRunFromBody(1);
            Assert.AreEqual(400.0, run1.Width.PointsValue,  1.0, "2. Run width overridden");
            Assert.AreEqual(300.0, run1.Height.PointsValue, 1.0, "2. Run height proportional (400 * 150/200)");

            // 3. SVG 300×150 (default), img width=150 → canvas 300×150, run 150×75
            Assert.AreEqual((Unit)300, svgs[2].Canvas.Width,  "3. Canvas width stays at default");
            Assert.AreEqual((Unit)150, svgs[2].Canvas.Height, "3. Canvas height stays at default");
            var run2 = GetImageRunFromBody(2);
            Assert.AreEqual(150.0, run2.Width.PointsValue,  1.0, "3. Run width overridden");
            Assert.AreEqual(75.0,  run2.Height.PointsValue, 1.0, "3. Run height proportional (150 * 150/300)");
            
            //Assert.Inconclusive();
        }

        // -----------------------------------------------------------------------
        // Group: img height override only — width scales proportionally
        // -----------------------------------------------------------------------

        [TestMethod()]
        public void SVGImageContainer_ImgHeightOnly()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_ImgHeightOnly.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImageContainer_ImgHeightOnly.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);
            var svgs = GetSVGImageData(this.layout);
            Assert.AreEqual(3, svgs.Count, "Expected 3 SVG images");

            // 1. SVG 150×100, img height=200 → canvas 150×100, run 300×200
            Assert.AreEqual((Unit)150, svgs[0].Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)100, svgs[0].Canvas.Height, "1. Canvas height stays intrinsic");
            var run0 = GetImageRunFromBody(0);
            Assert.AreEqual(300.0, run0.Width.PointsValue,  1.0, "1. Run width proportional (200 * 150/100)");
            Assert.AreEqual(200.0, run0.Height.PointsValue, 1.0, "1. Run height overridden");

            // 2. SVG 200×150 (viewBox), img height=100 → canvas 200×150, run ~133.3×100
            Assert.AreEqual((Unit)200, svgs[1].Canvas.Width,  "2. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)150, svgs[1].Canvas.Height, "2. Canvas height stays intrinsic");
            var run1 = GetImageRunFromBody(1);
            var expectedW1 = 100.0 * (200.0 / 150.0); // ≈ 133.33
            Assert.AreEqual(expectedW1, run1.Width.PointsValue,  1.0, "2. Run width proportional (100 * 200/150)");
            Assert.AreEqual(100.0,     run1.Height.PointsValue,  1.0, "2. Run height overridden");

            // 3. SVG 300×150 (default), img height=75 → canvas 300×150, run 150×75
            Assert.AreEqual((Unit)300, svgs[2].Canvas.Width,  "3. Canvas width stays at default");
            Assert.AreEqual((Unit)150, svgs[2].Canvas.Height, "3. Canvas height stays at default");
            var run2 = GetImageRunFromBody(2);
            Assert.AreEqual(150.0, run2.Width.PointsValue,  1.0, "3. Run width proportional (75 * 300/150)");
            Assert.AreEqual(75.0,  run2.Height.PointsValue, 1.0, "3. Run height overridden");
            
            Assert.Inconclusive();
        }

        // -----------------------------------------------------------------------
        // Group: img width and height both overridden
        // -----------------------------------------------------------------------

        [TestMethod()]
        public void SVGImageContainer_ImgBothDims()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_ImgBothDims.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImageContainer_ImgBothDims.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);
            var svgs = GetSVGImageData(this.layout);
            Assert.AreEqual(2, svgs.Count, "Expected 2 SVG images");

            // 1. SVG 200×150 (viewBox), img 400×300 — canvas stays intrinsic, run matches override
            Assert.AreEqual((Unit)200, svgs[0].Canvas.Width,  "1. Canvas width stays intrinsic");
            Assert.AreEqual((Unit)150, svgs[0].Canvas.Height, "1. Canvas height stays intrinsic");
            var run0 = GetImageRunFromBody(0);
            Assert.AreEqual(400.0, run0.Width.PointsValue,  1.0, "1. Run width from img override");
            Assert.AreEqual(300.0, run0.Height.PointsValue, 1.0, "1. Run height from img override");

            // 2. SVG 300×150 (default), img 100×100 — canvas stays intrinsic, run matches override
            Assert.AreEqual((Unit)300, svgs[1].Canvas.Width,  "2. Canvas width stays at default");
            Assert.AreEqual((Unit)150, svgs[1].Canvas.Height, "2. Canvas height stays at default");
            var run1 = GetImageRunFromBody(1);
            Assert.AreEqual(100.0, run1.Width.PointsValue,  1.0, "2. Run width from img override");
            Assert.AreEqual(100.0, run1.Height.PointsValue, 1.0, "2. Run height from img override");
            
            Assert.Inconclusive();
        }

        // -----------------------------------------------------------------------
        // Group: preserveAspectRatio variants — dest 300×200 via img CSS, viewBox 0 0 200 150
        // -----------------------------------------------------------------------

        [TestMethod()]
        public void SVGImageContainer_PAR_Variants()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_PAR_Variants.html");

            using (var doc = Document.ParseDocument(path))
            using (var stream = DocStreams.GetOutputStream("SVGImageContainer_PAR_Variants.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(this.layout);
            var svgs = GetSVGImageData(this.layout);
            Assert.AreEqual(5, svgs.Count, "Expected 5 SVG images");

            // dest=300×200, viewBox=0 0 200 150
            // scalex=300/200=1.5, scaley=200/150=1.333
            const double Delta = 0.5;
            double minScale = Math.Min(300.0 / 200.0, 200.0 / 150.0); // 1.333
            double maxScale = Math.Max(300.0 / 200.0, 200.0 / 150.0); // 1.5

            // All canvases: intrinsic 200×150 from viewBox (no explicit dims in SVG)
            foreach (var svg in svgs)
            {
                Assert.AreEqual((Unit)200, svg.Canvas.Width,  "Canvas width from viewBox");
                Assert.AreEqual((Unit)150, svg.Canvas.Height, "Canvas height from viewBox");
            }

            // All runs: 300×200 from img CSS
            for (int i = 0; i < 5; i++)
            {
                var run = GetImageRunFromBody(i);
                Assert.AreEqual(300.0, run.Width.PointsValue,  1.0, $"{i+1}. Run width from img CSS");
                Assert.AreEqual(200.0, run.Height.PointsValue, 1.0, $"{i+1}. Run height from img CSS");
            }

            // 1. xMidYMid meet: scale=1.333, scaledW=266.67 → TX=(300-266.67)/2=16.67, TY=0
            var m0 = svgs[0].Sizer.GetCanvasToImageMatrix(null);
            double scaledW_meet = 200 * minScale;
            double spareX_meet  = 300 - scaledW_meet;
            Assert.AreEqual(minScale,        RunScaleX(m0), 0.01, "1. xMidYMid meet scaleX");
            Assert.AreEqual(minScale,        RunScaleY(m0), 0.01, "1. xMidYMid meet scaleY");
            Assert.AreEqual(spareX_meet / 2, RunTranslateX(m0), Delta, "1. xMidYMid meet TX");
            Assert.AreEqual(0.0,             RunTranslateY(m0), Delta, "1. xMidYMid meet TY");

            // 2. xMinYMin meet: scale=1.333, TX=0, TY=spareY(=0)
            var m1 = svgs[1].Sizer.GetCanvasToImageMatrix(null);
            double scaledH_meet = 150 * minScale;
            double spareY_meet  = 200 - scaledH_meet;
            Assert.AreEqual(minScale, RunScaleX(m1), 0.01, "2. xMinYMin meet scaleX");
            Assert.AreEqual(0.0,      RunTranslateX(m1), Delta, "2. xMinYMin meet TX");
            Assert.AreEqual(spareY_meet, RunTranslateY(m1), Delta, "2. xMinYMin meet TY");

            // 3. xMaxYMax meet: scale=1.333, TX=spareX, TY=0
            var m2 = svgs[2].Sizer.GetCanvasToImageMatrix(null);
            Assert.AreEqual(minScale,       RunScaleX(m2), 0.01, "3. xMaxYMax meet scaleX");
            Assert.AreEqual(spareX_meet,    RunTranslateX(m2), Delta, "3. xMaxYMax meet TX");
            Assert.AreEqual(0.0,            RunTranslateY(m2), Delta, "3. xMaxYMax meet TY");

            // 4. none: scaleX=1.5, scaleY=1.333, TX=0, TY=0
            var m3 = svgs[3].Sizer.GetCanvasToImageMatrix(null);
            Assert.AreEqual(300.0 / 200.0, RunScaleX(m3), 0.01, "4. none scaleX");
            Assert.AreEqual(200.0 / 150.0, RunScaleY(m3), 0.01, "4. none scaleY");
            Assert.AreEqual(0.0,           RunTranslateX(m3), Delta, "4. none TX");
            Assert.AreEqual(0.0,           RunTranslateY(m3), Delta, "4. none TY");

            // 5. xMidYMid slice: scale=1.5, scaledH=225, TY=(200-225)/2=-12.5, TX=0
            var m4 = svgs[4].Sizer.GetCanvasToImageMatrix(null);
            double scaledH_slice = 150 * maxScale;
            double spareY_slice  = 200 - scaledH_slice; // -25
            Assert.AreEqual(maxScale,        RunScaleX(m4), 0.01, "5. xMidYMid slice scaleX");
            Assert.AreEqual(maxScale,        RunScaleY(m4), 0.01, "5. xMidYMid slice scaleY");
            Assert.AreEqual(0.0,             RunTranslateX(m4), Delta, "5. xMidYMid slice TX");
            Assert.AreEqual(spareY_slice / 2, RunTranslateY(m4), Delta, "5. xMidYMid slice TY");
            
            Assert.Inconclusive();
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

            // All use viewBox 0 0 200 150
            const double Delta = 0.5;

            
            // 1. width=50% → intrinsic width=100 (50% of vb 200), height=75 (proportional from vb AR)
            //    No img override → run matches intrinsic.
            // var size0 = svgs[0].Sizer.GetLayoutSize();
            // Assert.AreEqual(100.0, size0.Width.PointsValue,  Delta, "1. Sizer width: 50% of viewBox W=200");
            // Assert.AreEqual(75.0,  size0.Height.PointsValue, Delta, "1. Sizer height: proportional 100*(150/200)");
            // var run0 = GetImageRunFromBody(0);
            // Assert.AreEqual(100.0, run0.Width.PointsValue,  Delta, "1. Run width matches intrinsic");
            // Assert.AreEqual(75.0,  run0.Height.PointsValue, Delta, "1. Run height matches intrinsic");
            //
            // // 2. height=50% → intrinsic height=75 (50% of vb 150), width=100 (proportional from vb AR)
            // //    No img override → run matches intrinsic.
            // var size1 = svgs[1].Sizer.GetLayoutSize();
            // Assert.AreEqual(100.0, size1.Width.PointsValue,  Delta, "2. Sizer width: proportional 75*(200/150)");
            // Assert.AreEqual(75.0,  size1.Height.PointsValue, Delta, "2. Sizer height: 50% of viewBox H=150");
            // var run1 = GetImageRunFromBody(1);
            // Assert.AreEqual(100.0, run1.Width.PointsValue,  Delta, "2. Run width matches intrinsic");
            // Assert.AreEqual(75.0,  run1.Height.PointsValue, Delta, "2. Run height matches intrinsic");
            //
            // // 3. width=75%, height=100% → intrinsic 150×150 (non-square)
            // //    No img override → run matches intrinsic.
            // var size2 = svgs[2].Sizer.GetLayoutSize();
            // Assert.AreEqual(150.0, size2.Width.PointsValue,  Delta, "3. Sizer width: 75% of viewBox W=200");
            // Assert.AreEqual(150.0, size2.Height.PointsValue, Delta, "3. Sizer height: 100% of viewBox H=150");
            // var run2 = GetImageRunFromBody(2);
            // Assert.AreEqual(150.0, run2.Width.PointsValue,  Delta, "3. Run width matches intrinsic");
            // Assert.AreEqual(150.0, run2.Height.PointsValue, Delta, "3. Run height matches intrinsic");
            //
            // // 4. width=50% → intrinsic 100×75; img override width=200pt → run 200×150 (proportional from intrinsic AR)
            // var size3 = svgs[3].Sizer.GetLayoutSize();
            // Assert.AreEqual(100.0, size3.Width.PointsValue,  Delta, "4. Sizer width: 50% of viewBox W=200 (intrinsic)");
            // Assert.AreEqual(75.0,  size3.Height.PointsValue, Delta, "4. Sizer height: proportional (intrinsic)");
            // var run3 = GetImageRunFromBody(3);
            // Assert.AreEqual(200.0, run3.Width.PointsValue,  Delta, "4. Run width from img override");
            // Assert.AreEqual(150.0, run3.Height.PointsValue, Delta, "4. Run height proportional from intrinsic: 200*(75/100)");
            
            Assert.Inconclusive("Need to update the GetLayoutSize with available and context, then check again");
        }

        
    }
}
