using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Components;
using Scryber.Svg;
using Scryber.Svg.Components;
using System.CodeDom;
using Scryber.Imaging;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.Svg.Imaging;

namespace Scryber.Core.UnitTests.Svg
{
    
    
    /// <summary>
    ///This is a test class for PDFColor_Test and is intended
    ///to contain all PDFColor_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class SVGXObject_Test
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /// <summary>
        ///A test to make sure the SVG is rendered as an XObject in the PDF and is registered as an inline XObject (content was part of the page)
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGOutputInlineXObject_Test()
        {
            var doc = new Document();
            var page = new Page() { Margins = 10};
            doc.Pages.Add(page);
            

            var svg = new SVGCanvas() { Width = 100, Height = 100 };
            svg.BackgroundColor = StandardColors.Silver;
            page.Contents.Add(svg);

            page.Contents.Add("After the SVG");
            var rect = new SVGRect() { X = 10, Y = 10, Width = 80, Height = 80, FillValue = new SVGFillColorValue(StandardColors.Aqua) };
            svg.Contents.Add(rect);

            using(var stream = DocStreams.GetOutputStream("SVG_XObjectOutput.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.AppendTraceLog = true;
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var xobj = doc.SharedResources[2] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xobj);
            Assert.IsNotNull(xobj.Renderer);
            Assert.IsTrue(xobj.Renderer.IsInlineXObject);
            Assert.IsNotNull(xobj.Renderer.RenderReference);

        }
        
        /// <summary>
        ///A test to make sure the SVG is rendered as an XObject in the PDF and is registered as NOT an inline XOjbect (content was loaded from an image)
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGOutputReferencedImageXObject_Test()
        {
            var doc = new Document();
            var page = new Page() { Margins = 10};
            doc.Pages.Add(page);
            
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/Chart.svg", TestContext);

            var image = new Image() { Source = path, Width = 200, Height = 200 };
            page.Contents.Add(image);
            
            page.Contents.Add("After the SVG");
            

            using(var stream = DocStreams.GetOutputStream("SVG_ReferencedImageOutput.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.AppendTraceLog = true;
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var xobj = doc.SharedResources[2] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xobj);
            Assert.IsNotNull(xobj.Renderer);
            
            //This should not be an inline xobject as the rendering is handled by the image
            Assert.IsFalse(xobj.Renderer.IsInlineXObject);
            
            Assert.IsNotNull(xobj.Renderer.RenderReference);

        }

        /// <summary>
        /// Checks to make sure the SVG Image is sized correctly when all the width, height and viewbox properties are set (on the image tag and the loaded image itself.
        /// </summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedImageAllDefined_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImageAllDefined.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImageAllDefined.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            Assert.AreEqual(3, doc.SharedResources.Count);
            var imgX = doc.SharedResources[0] as PDFImageXObject;
            Assert.IsNotNull(imgX);
            Assert.IsNotNull(imgX.ImageData);
            Assert.IsInstanceOfType(imgX.ImageData, typeof(ImageDataProxy));
            var proxy = ((ImageDataProxy)imgX.ImageData);
            
            var svg = proxy.ImageData as SVGPDFImageData;
            Assert.IsNotNull(svg);
            
            
            var font = doc.SharedResources[1] as PDFFontResource;
            Assert.IsNotNull(font);
            
            var canvas = doc.SharedResources[2] as PDFLayoutXObjectResource;
            Assert.IsNotNull(canvas);

            
            //BBox of the img xObj is set to the size of the source image
            Assert.IsTrue(svg.ImgXObjectBBox.HasValue);
            Assert.AreEqual(200, svg.ImgXObjectBBox.Value.Width);
            Assert.AreEqual(200, svg.ImgXObjectBBox.Value.Width);
            
            //Image is set to render at 150pt = 0.75% of file size
            var renderSize = imgX.GetRequiredSizeForRender(Point.Empty,  new Size(150, 150), context);
            Assert.AreEqual(0.75, renderSize.Width);
            Assert.AreEqual(0.75, renderSize.Height);
            
            
            var img = doc.FindAComponentById("referenced");
            Assert.IsNotNull(img);
            
            var arrange = img.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            //Includes offset of other contents and padding
            var bounds = new Rect(20 + 5, 20 + 120 + 5, 170, 170);
            Assert.AreEqual(bounds, arrange.RenderBounds);
        }
        
        /// <summary>
        /// Checks to make sure the SVG Image is output once and the image tags themselves reference this single image.
        /// </summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedImageAllDefinedDouble_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImageAllDefinedDouble.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImageAllDefinedDouble.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            //3 means the image was only rendede
            Assert.AreEqual(3, doc.SharedResources.Count);
            
            var imgX = doc.SharedResources[0] as PDFImageXObject;
            Assert.IsNotNull(imgX);
            Assert.IsNotNull(imgX.ImageData);
            Assert.IsInstanceOfType(imgX.ImageData, typeof(ImageDataProxy));
            var proxy = ((ImageDataProxy)imgX.ImageData);
            
            var svg = proxy.ImageData as SVGPDFImageData;
            Assert.IsNotNull(svg);
            
            //BBox of the img xObj is set to the size of the source image
            Assert.IsTrue(svg.ImgXObjectBBox.HasValue);
            Assert.AreEqual(200, svg.ImgXObjectBBox.Value.Width);
            Assert.AreEqual(200, svg.ImgXObjectBBox.Value.Width);
            
            //Image is set to render at 150pt = 0.75% of file size
            var renderSize = imgX.GetRequiredSizeForRender(Point.Empty,  new Size(150, 150), context);
            Assert.AreEqual(0.75, renderSize.Width);
            Assert.AreEqual(0.75, renderSize.Height);

            var font = doc.SharedResources[1] as PDFFontResource;
            Assert.IsNotNull(font);
            
            var canvas = doc.SharedResources[2] as PDFLayoutXObjectResource;
            Assert.IsNotNull(canvas);

            
            var img = doc.FindAComponentById("referenced");
            Assert.IsNotNull(img);
            
            var arrange = img.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            //Includes offset of other contents and padding
            var bounds = new Rect(20 + 5, 20 + 120 + 5, 170, 170);
            Assert.AreEqual(bounds, arrange.RenderBounds);
            
            img = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(img);
            
            arrange = img.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            bounds = new Rect(65, 375, 220, 220);
            Assert.AreEqual(bounds, arrange.RenderBounds);
        }
        
        /// <summary>
        /// Runs a single image twice for the same file and image - to make sure the SVGCanvas is re-loaded and laid out for each document
        /// </summary>
         [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedImageAllDefinedTwice_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImageAllDefined.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImageAllDefinedFirst.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            doc = Document.ParseDocument(path);
            
            using (var stream = DocStreams.GetOutputStream("SVGReferencedImageAllDefinedSecond.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            Assert.AreEqual(3, doc.SharedResources.Count);
            var imgX = doc.SharedResources[0] as PDFImageXObject;
            Assert.IsNotNull(imgX);
            Assert.IsNotNull(imgX.ImageData);
            Assert.IsInstanceOfType(imgX.ImageData, typeof(ImageDataProxy));
            var proxy = ((ImageDataProxy)imgX.ImageData);
            
            var svg = proxy.ImageData as SVGPDFImageData;
            Assert.IsNotNull(svg);
            
            //BBox of the img xObj is set to the size of the source image
            Assert.IsTrue(svg.ImgXObjectBBox.HasValue);
            Assert.AreEqual(200, svg.ImgXObjectBBox.Value.Width);
            Assert.AreEqual(200, svg.ImgXObjectBBox.Value.Width);
            
            //Image is set to render at 150pt = 0.75% of file size
            var renderSize = imgX.GetRequiredSizeForRender(Point.Empty,  new Size(150, 150), context);
            Assert.AreEqual(0.75, renderSize.Width);
            Assert.AreEqual(0.75, renderSize.Height);


            
            var img = doc.FindAComponentById("referenced");
            Assert.IsNotNull(img);
            
            var arrange = img.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            //Includes offset of other contents and padding
            var bounds = new Rect(20 + 5, 20 + 120 + 5, 170, 170);
            Assert.AreEqual(bounds, arrange.RenderBounds);
        }
        
        
        //
        // Support methods for following tests
        //
        
        private SVGPDFImageData DoAssertImageReference(PDFImageXObject imgX, Rect bbox)
        {
            Assert.IsNotNull(imgX);
            Assert.IsNotNull(imgX.ImageData);
            Assert.IsInstanceOfType(imgX.ImageData, typeof(ImageDataProxy));
            
            var svg = ((ImageDataProxy)imgX.ImageData).ImageData as SVGPDFImageData;
            Assert.IsNotNull(svg);
            
            //BBox of the img xObj is set to the size of the source image
            Assert.IsTrue(svg.ImgXObjectBBox.HasValue);
            //THis is the defined size of the actual referenced SVG
            Assert.AreEqual(bbox.X, svg.ImgXObjectBBox.Value.X);
            Assert.AreEqual(bbox.Y, svg.ImgXObjectBBox.Value.Y);
            Assert.AreEqual(bbox.Width, svg.ImgXObjectBBox.Value.Width);
            Assert.AreEqual(bbox.Height, svg.ImgXObjectBBox.Value.Width);

            return svg;
        }

        private void DoAssertWideImage(string imgId, SVGPDFImageData svg, PDFImageXObject imgX, Point expectedOffset, Size expectedSize, Rect expectedBounds, RenderContext context)
        {
            var renderSize = imgX.GetRequiredSizeForRender(Point.Empty, new Size(400, 80), context);
            Assert.AreEqual(expectedSize.Width, renderSize.Width, "Render width scale did not match for " + imgId);
            Assert.AreEqual(expectedSize.Height, renderSize.Height, "Render height scale did not match for " + imgId);

            var offset = imgX.GetRequiredOffsetForRender(Point.Empty, new Size(400, 80), context);
            Assert.AreEqual(expectedOffset.X, offset.X, "Offset X did not match for " + imgId);
            Assert.AreEqual(expectedOffset.Y, offset.Y,  "Offset Y did not match for " + imgId); //image height
            
            
            var img = ((Document)context.Document).FindAComponentById(imgId);
            Assert.IsNotNull(img, "Image component was not found for " + imgId);
            
            var arrange = img.GetFirstArrangement();
            Assert.IsNotNull(arrange, "Render bounds not found for " + imgId);

            
            Assert.AreEqual(expectedBounds, arrange.RenderBounds, "Render bounds did not match for " + imgId);
        }
        
        private void DoAssertTallImage(string imgId, SVGPDFImageData svg, PDFImageXObject imgX, Point expectedOffset, Size expectedSize, Rect expectedBounds, RenderContext context)
        {
            var renderSize = imgX.GetRequiredSizeForRender(Point.Empty, new Size(100, 250), context);
            Assert.AreEqual(expectedSize.Width, renderSize.Width, "Render width scale did not match for " + imgId);
            Assert.AreEqual(expectedSize.Height, renderSize.Height, "Render height scale did not match for " + imgId);

            var offset = imgX.GetRequiredOffsetForRender(Point.Empty, new Size(100, 250), context);
            Assert.AreEqual(expectedOffset.X, offset.X, "Offset X did not match for " + imgId);
            Assert.AreEqual(expectedOffset.Y, offset.Y,  "Offset Y did not match for " + imgId); //image height
            
            
            var img = ((Document)context.Document).FindAComponentById(imgId);
            Assert.IsNotNull(img, "Image component was not found for " + imgId);
            
            var arrange = img.GetFirstArrangement();
            Assert.IsNotNull(arrange, "Render bounds not found for " + imgId);

            
            Assert.AreEqual(expectedBounds, arrange.RenderBounds, "Render bounds did not match for " + imgId);
        }
        
        
        /// <summary>
        /// Validates the proportional scaling of SVG images with various preserved aspect ratios when the widths and heights do not match.
        /// Meet by default - largest size that will fit the entire graphic in the availavble space
        /// </summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedImageAllDefinedProportional_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImageAllDefinedProportional.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImageProportional.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            //2 fonts, 6 images with 6 associated canvas's
            
            Assert.AreEqual(14, doc.SharedResources.Count);
            
            var bbox = new Rect(0, 0, 200, 200);
            //first image is wide left
            
            var imgX = doc.SharedResources[0] as PDFImageXObject;
            var svg = DoAssertImageReference(imgX, bbox);
            
            //Includes offset of other contents and padding
            var expectedOffset = new Point(0, 80);
            var expectedScale = new Size(0.4, 0.4);
            var expectedBounds = new Rect(20 + 5, 20 + 20 + 20 + 5, 400, 80);
            
            DoAssertWideImage("xMinYMidWide",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //second is wide middle aligned
            
            imgX = doc.SharedResources[1] as PDFImageXObject;
            svg = DoAssertImageReference(imgX, bbox);
            
            expectedOffset = new Point((400.0 - 80.0) / 2.0, 80.0);
            expectedScale = new Size(0.4, 0.4);
            
            expectedBounds.Y += 80 + 20 + 10; //margin + prev img height + p height
            
            DoAssertWideImage("xMidYMidWide",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //third is wide right aligned
            
            imgX = doc.SharedResources[2] as PDFImageXObject;
            svg = DoAssertImageReference(imgX, bbox);
            
            expectedOffset = new Point((400.0 - 80.0), 80.0);
            expectedScale = new Size(0.4, 0.4);
            
            expectedBounds.Y += 80 + 20 + 10; //margin + prev img height + p height
            
            DoAssertWideImage("xMaxYMidWide",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //fourth is wide non-proportional
            
            imgX = doc.SharedResources[3] as PDFImageXObject;
            svg = DoAssertImageReference(imgX, bbox);
            
            expectedOffset = new Point(0, 80.0);
            expectedScale = new Size(2, 0.4);
            
            expectedBounds.Y += 80 + 20 + 10; //margin + prev img height + p height
            
            DoAssertWideImage("xNoneWide",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //
            // tall images
            //

            var columnWidth = 141.31889763775;
            
            //first top aligned
            
            imgX = doc.SharedResources[4] as PDFImageXObject;
            svg = DoAssertImageReference(imgX, bbox);
            
            expectedOffset = new Point(0, 100.0); //just the height of the image
            expectedScale = new Size(0.5, 0.5);
            
            expectedBounds.Y += 58 + 20 + 80 + 10; //banner, title, prev image, margin
            expectedBounds.Width = 100;
            expectedBounds.Height = 250;
            
            DoAssertTallImage("xMidYMinTall",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //second mid aligned
            
            imgX = doc.SharedResources[1] as PDFImageXObject; //Uses the same resource as xmid ymid as on the wide row.
            svg = DoAssertImageReference(imgX, bbox);

            expectedOffset.Y = 175; //half space + image height
            expectedScale = new Size(0.5, 0.5);

            expectedBounds.X += columnWidth;
            
            DoAssertTallImage("xMidYMidTall",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //third bottom aligned
            
            imgX = doc.SharedResources[5] as PDFImageXObject;
            svg = DoAssertImageReference(imgX, bbox);

            expectedOffset.Y = 250; //all space and height
            expectedScale = new Size(0.5, 0.5);

            expectedBounds.X += columnWidth;
            
            DoAssertTallImage("xMidYMaxTall",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            
            //fourth vertically stretched
            
            imgX = doc.SharedResources[3] as PDFImageXObject; //uses NoneMeet again
            svg = DoAssertImageReference(imgX, bbox);

            expectedOffset.Y = 250; //all space and height
            expectedScale = new Size(0.5, 1.25);

            expectedBounds.X += columnWidth;
            
            DoAssertTallImage("xNoneTall",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
        }

        /// <summary>
        /// Validates the proportional scaling of SVG images with various preserved aspect ratios when the widths and heights do not match.
        /// Explicitly Sliced (smalest scale that will fill the entire space).
        /// </summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedImageAllDefinedProportionalSlice_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImageAllDefinedProportionalSlice.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImageProportionalSlice.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            //2 fonts, 4 images with 4 associated canvas's
            
            Assert.AreEqual(10, doc.SharedResources.Count);
            
            var bbox = new Rect(0, 0, 200, 200);
            
            //first image is wide left
            
            var imgX = doc.SharedResources[0] as PDFImageXObject;
            var svg = DoAssertImageReference(imgX, bbox);
            
            //Includes offset of other contents and padding
            var expectedOffset = new Point(0, 400);
            var expectedScale = new Size(2.0, 2.0);
            var expectedBounds = new Rect(20 + 5, 20 + 20 + 20 + 5, 400, 80);
            
            DoAssertWideImage("xMinYMinWide",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //second is wide middle aligned
            
            imgX = doc.SharedResources[1] as PDFImageXObject;
            svg = DoAssertImageReference(imgX, bbox);
            
            expectedOffset = new Point(0.0, 240.0);
            expectedScale = new Size(2, 2);
            
            expectedBounds.Y += 80 + 20 + 10; //margin + prev img height + p height
            
            DoAssertWideImage("xMidYMidWide",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //third is wide right aligned
            
            imgX = doc.SharedResources[2] as PDFImageXObject;
            svg = DoAssertImageReference(imgX, bbox);
            
            expectedOffset = new Point(0.0, 80.0);
            expectedScale = new Size(2, 2);
            
            expectedBounds.Y += 80 + 20 + 10; //margin + prev img height + p height
            
            DoAssertWideImage("xMaxYMaxWide",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //fourth is wide non-proportional
            
            imgX = doc.SharedResources[3] as PDFImageXObject;
            svg = DoAssertImageReference(imgX, bbox);
            
            expectedOffset = new Point(0, 80.0);
            expectedScale = new Size(2, 0.4);
            
            expectedBounds.Y += 80 + 20 + 10; //margin + prev img height + p height
            
            DoAssertWideImage("xNoneWide",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //
            // tall images
            //

            var columnWidth = 141.31889763775;
            
            //first top aligned
            
            imgX = doc.SharedResources[0] as PDFImageXObject;
            svg = DoAssertImageReference(imgX, bbox);
            
            expectedOffset = new Point(0, 250.0); //just the height of the image
            expectedScale = new Size(1.25, 1.25);
            
            expectedBounds.Y += 58 + 20 + 80 + 10; //banner, title, prev image, margin
            expectedBounds.Width = 100;
            expectedBounds.Height = 250;
            
            DoAssertTallImage("xMinYMinTall",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //second mid aligned
            
            imgX = doc.SharedResources[1] as PDFImageXObject; //Uses the same resource as xmid ymid as on the wide row.
            svg = DoAssertImageReference(imgX, bbox);

            expectedOffset = new Point(-75, 250); //middle at scale and height of the image
            expectedScale = new Size(1.25, 1.25);

            expectedBounds.X += columnWidth;
            
            DoAssertTallImage("xMidYMidTall",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //third bottom aligned
            
            imgX = doc.SharedResources[2] as PDFImageXObject;
            svg = DoAssertImageReference(imgX, bbox);

            expectedOffset = new Point(-150, 250); //full width at scale, and height of the image
            expectedScale = new Size(1.25, 1.25);

            expectedBounds.X += columnWidth;
            
            DoAssertTallImage("xMaxYMaxTall",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            
            //fourth vertically stretched
            
            imgX = doc.SharedResources[3] as PDFImageXObject; //uses NoneMeet again
            svg = DoAssertImageReference(imgX, bbox);

            expectedOffset = new Point(0,  250); //all space and height
            expectedScale = new Size(0.5, 1.25);

            expectedBounds.X += columnWidth;
            
            DoAssertTallImage("xNoneTall",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
        }
        
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if only the width and height on the actual svg file are specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedNoImageSize_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImageNoImageSize.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImageNoImageSize.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }

            Assert.Inconclusive("Not tested - need to check the layout, render bounds, and XObject reference in the document");

        }
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if only the ViewBox on the actual svg file are specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedViewBoxOnFileOnlyOutput_Test()
        {
            var doc = new Document();
            var page = new Page();
            doc.Pages.Add(page);

            var svg = new SVGCanvas() { Width = 100, Height = 100 };
            page.Contents.Add(svg);

            var rect = new SVGRect() { X = 10, Y = 10, Width = 80, Height = 80, FillColor = StandardColors.Aqua };
            svg.Contents.Add(rect);

            using(var stream = DocStreams.GetOutputStream("SVG_XObjectOutput.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                doc.SaveAsPDF(stream);
            }

            Assert.Inconclusive("Not tested - need to check the layout, render bounds, and XObject reference in the document");

        }
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if just the ViewBox on the actual svg file is specified, and the size of the image in the referencing file is specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedViewBoxOnFileAndImgSizeOutput_Test()
        {
            var doc = new Document();
            var page = new Page();
            doc.Pages.Add(page);

            var svg = new SVGCanvas() { Width = 100, Height = 100 };
            page.Contents.Add(svg);

            var rect = new SVGRect() { X = 10, Y = 10, Width = 80, Height = 80, FillColor = StandardColors.Aqua };
            svg.Contents.Add(rect);

            using(var stream = DocStreams.GetOutputStream("SVG_XObjectOutput.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                doc.SaveAsPDF(stream);
            }

            Assert.Inconclusive("Not tested - need to check the layout, render bounds, and XObject reference in the document");

        }
    }
}
