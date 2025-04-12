using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Components;
using Scryber.Svg;
using Scryber.Svg.Components;
using System.CodeDom;
using Scryber.Imaging;
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
        ///A test to make sure the SVG is rendered as an XObject in the PDF
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGOutput_Test()
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
            
            //first image is wide left
            
            var imgX = doc.SharedResources[0] as PDFImageXObject;
            var svg = AssertImageReference(imgX);
            
            //Includes offset of other contents and padding
            var expectedOffset = new Point(0, 80);
            var expectedScale = new Size(0.4, 0.4);
            var expectedBounds = new Rect(20 + 5, 20 + 20 + 20 + 5, 400, 80);
            
            AssertWideImage("xMinYMidWide",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //second is wide middle aligned
            
            imgX = doc.SharedResources[1] as PDFImageXObject;
            svg = AssertImageReference(imgX);
            
            expectedOffset = new Point((400.0 - 80.0) / 2.0, 80.0);
            expectedScale = new Size(0.4, 0.4);
            
            expectedBounds.Y += 80 + 20 + 10; //margin + prev img height + p height
            
            AssertWideImage("xMidYMidWide",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //third is wide right aligned
            
            imgX = doc.SharedResources[2] as PDFImageXObject;
            svg = AssertImageReference(imgX);
            
            expectedOffset = new Point((400.0 - 80.0), 80.0);
            expectedScale = new Size(0.4, 0.4);
            
            expectedBounds.Y += 80 + 20 + 10; //margin + prev img height + p height
            
            AssertWideImage("xMaxYMidWide",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //fourth is wide non-proportional
            
            imgX = doc.SharedResources[3] as PDFImageXObject;
            svg = AssertImageReference(imgX);
            
            expectedOffset = new Point(0, 80.0);
            expectedScale = new Size(2, 0.4);
            
            expectedBounds.Y += 80 + 20 + 10; //margin + prev img height + p height
            
            AssertWideImage("xNoneWide",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //
            // tall images
            //

            var columnWidth = 141.31889763775;
            
            //first top aligned
            
            imgX = doc.SharedResources[4] as PDFImageXObject;
            svg = AssertImageReference(imgX);
            
            expectedOffset = new Point(0, 100.0); //just the height of the image
            expectedScale = new Size(0.5, 0.5);
            
            expectedBounds.Y += 58 + 20 + 80 + 10; //banner, title, prev image, margin
            expectedBounds.Width = 100;
            expectedBounds.Height = 250;
            
            AssertTallImage("xMidYMinTall",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //second mid aligned
            
            imgX = doc.SharedResources[1] as PDFImageXObject; //Uses the same resource as xmid ymid as on the wide row.
            svg = AssertImageReference(imgX);

            expectedOffset.Y = 175; //half space + image height
            expectedScale = new Size(0.5, 0.5);

            expectedBounds.X += columnWidth;
            
            AssertTallImage("xMidYMidTall",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            //third bottom aligned
            
            imgX = doc.SharedResources[5] as PDFImageXObject;
            svg = AssertImageReference(imgX);

            expectedOffset.Y = 250; //all space and height
            expectedScale = new Size(0.5, 0.5);

            expectedBounds.X += columnWidth;
            
            AssertTallImage("xMidYMaxTall",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
            
            
            //fourth vertically stretched
            
            imgX = doc.SharedResources[3] as PDFImageXObject; //uses NoneMeet again
            svg = AssertImageReference(imgX);

            expectedOffset.Y = 250; //all space and height
            expectedScale = new Size(0.5, 1.25);

            expectedBounds.X += columnWidth;
            
            AssertTallImage("xNoneTall",svg, imgX, expectedOffset, expectedScale, expectedBounds, context);
        }

        private SVGPDFImageData AssertImageReference(PDFImageXObject imgX)
        {
            Assert.IsNotNull(imgX);
            Assert.IsNotNull(imgX.ImageData);
            Assert.IsInstanceOfType(imgX.ImageData, typeof(ImageDataProxy));
            
            var svg = ((ImageDataProxy)imgX.ImageData).ImageData as SVGPDFImageData;
            Assert.IsNotNull(svg);
            
            //BBox of the img xObj is set to the size of the source image
            Assert.IsTrue(svg.ImgXObjectBBox.HasValue);
            //THis is the defined size of the actual referenced SVG
            Assert.AreEqual(200, svg.ImgXObjectBBox.Value.Width);
            Assert.AreEqual(200, svg.ImgXObjectBBox.Value.Width);

            return svg;
        }

        private void AssertWideImage(string imgId, SVGPDFImageData svg, PDFImageXObject imgX, Point expectedOffset, Size expectedSize, Rect expectedBounds, RenderContext context)
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
        
        private void AssertTallImage(string imgId, SVGPDFImageData svg, PDFImageXObject imgX, Point expectedOffset, Size expectedSize, Rect expectedBounds, RenderContext context)
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
    }
}
