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

            //Includes offset of other contents
            var bounds = new Rect(30, 150, 160, 160);
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
            var bounds = new Rect(30, 150, 160, 160);
            Assert.AreEqual(bounds, arrange.RenderBounds);
            
            img = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(img);
            
            arrange = img.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            bounds = new Rect(70, 380, 210, 210);
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

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImageAllDefinedTwice1.pdf"))
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
            
            using (var stream = DocStreams.GetOutputStream("SVGReferencedImageAllDefinedTwice2.pdf"))
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
            var bounds = new Rect(30, 150, 160, 160);
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
            var expectedBounds = new Rect(20, 60, 410, 90);
            
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
            expectedBounds.Width = 110;
            expectedBounds.Height = 260;
            
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
            var expectedBounds = new Rect(20, 20 + 20 + 20, 410, 90);
            
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
            expectedBounds.Width = 110;
            expectedBounds.Height = 260;
            
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
        /// ratio if all the sizes in svg file are specified, and a size on the actual images - proportional to the aspect ratio of the file
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedSizing_01_AllSizesDefined_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage_01_AllDefined.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage_01_AllDefined.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }

            var reference = doc.FindAComponentById("referenced");
            Assert.IsNotNull(reference);

            var arrange = reference.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            var expected = new Rect(20, 140, 150, 150);
            Assert.AreEqual(expected, arrange.RenderBounds);
            
            var reference2 = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(reference2);

            arrange = reference2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            expected = new Rect(60, 340, 40, 40);
            Assert.AreEqual(expected, arrange.RenderBounds);
            
            

        }
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if all the sizes in svg file are specified, and a size on the actual images - NOT proportional to the aspect ratio of the file
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedSizing_02_AllSizesNonProportional_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage_02_AllDefinedNonProportional.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage_02_AllDefinedNonProportional.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }

            var reference = doc.FindAComponentById("referenced");
            Assert.IsNotNull(reference);

            var arrange = reference.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            var expected = new Rect(20, 140, 200, 150);
            Assert.AreEqual(expected, arrange.RenderBounds);
            
            var reference2 = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(reference2);

            arrange = reference2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            expected = new Rect(60, 340, 100, 40);
            Assert.AreEqual(expected, arrange.RenderBounds);

        }
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if only the width, height and viewbox on the actual svg file are specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedSizing_03_SVGBoth_NoImageSizes_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage_03_SVGBoth_NoImageSize.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage_03_SVGBoth_NoImageSize.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }

            var reference = doc.FindAComponentById("referenced");
            Assert.IsNotNull(reference);

            var arrange = reference.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            var expected = new Rect(20, 140, 200, 200);
            Assert.AreEqual(expected, arrange.RenderBounds);
            
            var reference2 = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(reference2);

            arrange = reference2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            //We fit the available space
            expected = new Rect(60, 390, 50, 50);
            Assert.AreEqual(expected, arrange.RenderBounds);

        }
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if only the width and height on the actual svg file are specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedSizing_04_SVGBoth_ImageWidthOnly_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage_04_SVGBoth_ImageWidthOnly.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage_04_SVGBoth_ImageWidthOnly.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }

            var reference = doc.FindAComponentById("referenced");
            Assert.IsNotNull(reference);

            var arrange = reference.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            var contentWidth = Papers.GetSizeInMM(doc.Pages[0].PaperSize).Width - 40;
            var half = (contentWidth / 2.0).ToPoints();
            var expected = new Rect(20, 140, half, half);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(expected.Y, arrange.RenderBounds.Y);
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));
            
            var reference2 = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(reference2);

            arrange = reference2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            //20% of available width, taking into account the div padding
            var twentyPcent = ((contentWidth - 40) / 5.0).ToPoints();
            
            //20% is over the available height, but as it's explicit and overflow is set to clip - we use it.
            expected = new Rect(60, 140 + half + 50, twentyPcent, twentyPcent);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(Math.Round(expected.Y.PointsValue, 5), Math.Round(arrange.RenderBounds.Y.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));

        }
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if only the width and height on the actual svg file are specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedSizing_05_SVGBoth_ImageHeightOnly_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage_05_SVGBoth_ImageHeightOnly.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage_05_SVGBoth_ImageHeightOnly.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }

            var reference = doc.FindAComponentById("referenced");
            Assert.IsNotNull(reference);

            var arrange = reference.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            var expected = new Rect(20, 140, 300, 300);
            Assert.AreEqual(expected, arrange.RenderBounds);
            
            var reference2 = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(reference2);

            arrange = reference2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            //100% is all the space - 50pt (even with overflow visible) 
            expected = new Rect(60, 490, 50, 50);
            Assert.AreEqual(expected, arrange.RenderBounds);

        }
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if only the width and height on the actual svg file are specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedSizing_06_SVGViewBox_NoImageSize_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage_06_SVGViewBox_NoImageSize.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage_06_SVGViewBox_NoImageSize.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            var reference = doc.FindAComponentById("referenced");
            Assert.IsNotNull(reference);

            var arrange = reference.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            var contentWidth = Papers.GetSizeInMM(doc.Pages[0].PaperSize).Width - 40;
            //var half = (contentWidth / 2.0).ToPoints();
            var expected = new Rect(20, 140, contentWidth, contentWidth);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(expected.Y, arrange.RenderBounds.Y);
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));
            
            var reference2 = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(reference2);

            arrange = reference2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            
            
            //fit to the bounds proportionally as we have no explicit size -
            //should overflow to the next page and fit the content.
            expected = new Rect(60, 20, contentWidth - 40, contentWidth - 40);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(Math.Round(expected.Y.PointsValue, 5), Math.Round(arrange.RenderBounds.Y.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));

        }
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if only the width and height on the actual svg file are specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedSizing_07_SVGViewBox_ImageBoth_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage_07_SVGViewBox_ImageBoth.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage_07_SVGViewBox_ImageBoth.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            var reference = doc.FindAComponentById("referenced");
            Assert.IsNotNull(reference);

            var arrange = reference.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            //var contentWidth = Papers.GetSizeInMM(doc.Pages[0].PaperSize).Width - 40;
            //var half = (contentWidth / 2.0).ToPoints();
            var expected = new Rect(20, 140, 400, 300);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(expected.Y, arrange.RenderBounds.Y);
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));
            
            var reference2 = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(reference2);

            arrange = reference2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            
            
            //fit to the bounds proportionally as we have no explicit size.
            expected = new Rect(60, 140 + 300 + 50, 100, 40);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(Math.Round(expected.Y.PointsValue, 5), Math.Round(arrange.RenderBounds.Y.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));

        }
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if only the width and height on the actual svg file are specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedSizing_08_SVGViewBox_ImageHeight_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage_08_SVGViewBox_ImageHeight.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage_08_SVGViewBox_ImageHeight.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            var reference = doc.FindAComponentById("referenced");
            Assert.IsNotNull(reference);

            var arrange = reference.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            //var contentWidth = Papers.GetSizeInMM(doc.Pages[0].PaperSize).Width - 40;
            //var half = (contentWidth / 2.0).ToPoints();
            var expected = new Rect(20, 140, 150, 150);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(expected.Y, arrange.RenderBounds.Y);
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));
            
            var reference2 = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(reference2);

            arrange = reference2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            
            
            //explicit height over the parent size bt still scaled proportionally
            expected = new Rect(60, 140 + 150 + 50, 60, 60);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(Math.Round(expected.Y.PointsValue, 5), Math.Round(arrange.RenderBounds.Y.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));

        }
        
         /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if only the width and height on the actual svg file are specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedSizing_09_SVGViewBox_ImageWidth_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage_09_SVGViewBox_ImageWidth.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage_09_SVGViewBox_ImageWidth.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            var reference = doc.FindAComponentById("referenced");
            Assert.IsNotNull(reference);

            var arrange = reference.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            //var contentWidth = Papers.GetSizeInMM(doc.Pages[0].PaperSize).Width - 40;
            //var half = (contentWidth / 2.0).ToPoints();
            var expected = new Rect(20, 140, 150, 150);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(expected.Y, arrange.RenderBounds.Y);
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));
            
            var reference2 = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(reference2);

            arrange = reference2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            
            
            //explicit height over the parent size bt still scaled proportionally
            expected = new Rect(60, 140 + 150 + 50, 60, 60);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(Math.Round(expected.Y.PointsValue, 5), Math.Round(arrange.RenderBounds.Y.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));

        }
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if only the width and height on the actual svg file are specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedSizing_10_SVGSize_NoImageSize_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage_10_SVGSize_NoImageSize.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage_10_SVGSize_NoImageSize.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            var reference = doc.FindAComponentById("referenced");
            Assert.IsNotNull(reference);

            var arrange = reference.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            //var contentWidth = Papers.GetSizeInMM(doc.Pages[0].PaperSize).Width - 40;
            //var half = (contentWidth / 2.0).ToPoints();
            var expected = new Rect(20, 140, 200, 200);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(expected.Y, arrange.RenderBounds.Y);
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));
            
            var reference2 = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(reference2);

            arrange = reference2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            
            
            //fit to the bounds proportionally as we have no explicit size.
            expected = new Rect(60, 140 + 200 + 50, 50, 50);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(Math.Round(expected.Y.PointsValue, 5), Math.Round(arrange.RenderBounds.Y.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));

        }
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if only the width and height on the actual svg file are specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedSizing_11_SVGSize_ImageWidth_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage_11_SVGSize_ImageWidth.html", TestContext);
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage_11_SVGSize_ImageWidth.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            var reference = doc.FindAComponentById("referenced");
            Assert.IsNotNull(reference);

            var arrange = reference.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            var contentWidth = Papers.GetSizeInMM(doc.Pages[0].PaperSize).Width - 40;
            var forty = (contentWidth * 0.4).ToPoints();
            var expected = new Rect(20, 140, forty, forty);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(expected.Y, arrange.RenderBounds.Y);
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));
            
            var reference2 = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(reference2);

            arrange = reference2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            
            
            //fit to the bounds proportionally as we have no explicit size.
            expected = new Rect(60, 140 + forty + 50, 40, 40);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(Math.Round(expected.Y.PointsValue, 5), Math.Round(arrange.RenderBounds.Y.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));

        }
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if only the width and height on the actual svg file are specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedSizing_12_SVGSize_ImageHeight_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage_12_SVGSize_ImageHeight.html", TestContext);
            
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage_12_SVGSize_ImageHeight.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            var reference = doc.FindAComponentById("referenced");
            Assert.IsNotNull(reference);

            var arrange = reference.GetFirstArrangement();
            Assert.IsNotNull(arrange);

           // var contentWidth = Papers.GetSizeInMM(doc.Pages[0].PaperSize).Width - 40;
           // var forty = (contentWidth * 0.4).ToPoints();
            var expected = new Rect(20, 140, 300, 300);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(expected.Y, arrange.RenderBounds.Y);
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));
            
            var reference2 = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(reference2);

            arrange = reference2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            
            
            //fit to the bounds proportionally as we have no explicit size.
            expected = new Rect(60, 140 + 300 + 50, 50 * 1.5, 50 * 1.5);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(Math.Round(expected.Y.PointsValue, 5), Math.Round(arrange.RenderBounds.Y.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));

        }
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if only the width and height on the actual svg file are specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedSizing_13_SVGSize_ImageBoth_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage_13_SVGSize_ImageBoth.html", TestContext);
            
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage_13_SVGSize_ImageBoth.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            var reference = doc.FindAComponentById("referenced");
            Assert.IsNotNull(reference);

            var arrange = reference.GetFirstArrangement();
            Assert.IsNotNull(arrange);

           // var contentWidth = Papers.GetSizeInMM(doc.Pages[0].PaperSize).Width - 40;
           // var forty = (contentWidth * 0.4).ToPoints();
            var expected = new Rect(20, 140, 200, 300);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(expected.Y, arrange.RenderBounds.Y);
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));
            
            var reference2 = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(reference2);

            arrange = reference2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            
            
            //fit to the bounds proportionally as we have no explicit size.
            expected = new Rect(60, 140 + 300 + 50, 200, 50 * 1.5);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(Math.Round(expected.Y.PointsValue, 5), Math.Round(arrange.RenderBounds.Y.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));

        }
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if only the width and height on the actual svg file are specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedSizing_14_SVGNone_ImageBoth_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage_14_SVGNone_ImageBoth.html", TestContext);
            
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage_14_SVGNone_ImageBoth.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            var reference = doc.FindAComponentById("referenced");
            Assert.IsNotNull(reference);

            var arrange = reference.GetFirstArrangement();
            Assert.IsNotNull(arrange);

           // var contentWidth = Papers.GetSizeInMM(doc.Pages[0].PaperSize).Width - 40;
           // var forty = (contentWidth * 0.4).ToPoints();
            var expected = new Rect(20, 140, 200, 200);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(expected.Y, arrange.RenderBounds.Y);
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));
            
            var reference2 = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(reference2);

            arrange = reference2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            
            
            //fit to the bounds proportionally as we have no explicit size.
            expected = new Rect(60, 140 + 200 + 50, 100, 100);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(Math.Round(expected.Y.PointsValue, 5), Math.Round(arrange.RenderBounds.Y.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));

        }
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if only the width and height on the actual svg file are specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedSizing_15_SVGViewBoxAndWidth_ImageNone_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage_15_SVGViewBoxAndWidth_NoImageSize.html", TestContext);
            
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage_15_SVGViewBoxAndWidth_ImageNone.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            var reference = doc.FindAComponentById("referenced");
            Assert.IsNotNull(reference);

            var arrange = reference.GetFirstArrangement();
            Assert.IsNotNull(arrange);

           // var contentWidth = Papers.GetSizeInMM(doc.Pages[0].PaperSize).Width - 40;
           // var forty = (contentWidth * 0.4).ToPoints();
            var expected = new Rect(20, 140, 200, 200);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(expected.Y, arrange.RenderBounds.Y);
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));
            
            var reference2 = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(reference2);

            arrange = reference2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            
            
            //fit to the bounds proportionally as we have no explicit size.
            expected = new Rect(60, 140 + 200 + 50, 50, 50);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(Math.Round(expected.Y.PointsValue, 5), Math.Round(arrange.RenderBounds.Y.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));

        }
        
        
        
        /// <summary>
        ///A test to make sure the SVG is rendered at the correct size and aspect
        /// ratio if only the width and height on the actual svg file are specified.
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGReferencedSizing_16_SVGViewBoxAndHeight_ImageNone_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage_16_SVGViewBoxAndHeight_NoImageSize.html", TestContext);
            
            var doc = Document.ParseDocument(path);
            RenderContext context = null;

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage_16_SVGViewBoxAndHeight_ImageNone.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.PostRender += (o, e) =>
                {
                    context = e.Context;
                };
                doc.SaveAsPDF(stream);
            }
            
            var reference = doc.FindAComponentById("referenced");
            Assert.IsNotNull(reference);

            var arrange = reference.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            // var contentWidth = Papers.GetSizeInMM(doc.Pages[0].PaperSize).Width - 40;
            // var forty = (contentWidth * 0.4).ToPoints();
            var expected = new Rect(20, 140, 200, 200);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(expected.Y, arrange.RenderBounds.Y);
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));
            
            var reference2 = doc.FindAComponentById("referenced2");
            Assert.IsNotNull(reference2);

            arrange = reference2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            
            
            
            //fit to the bounds proportionally as we have no explicit size.
            expected = new Rect(60, 140 + 200 + 50, 50, 50);
            Assert.AreEqual(expected.X, arrange.RenderBounds.X);
            Assert.AreEqual(Math.Round(expected.Y.PointsValue, 5), Math.Round(arrange.RenderBounds.Y.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, 5), Math.Round(arrange.RenderBounds.Width.PointsValue, 5));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, 5), Math.Round(arrange.RenderBounds.Height.PointsValue, 5));

        }
        
    }
}
