using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Components;
using Scryber.Svg;
using Scryber.Svg.Components;
using System.CodeDom;
using Scryber.PDF.Native;

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
        public void SVGReferencedImage_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGReferencedImage.html", TestContext);
            var doc = Document.ParseDocument(path);

            using (var stream = DocStreams.GetOutputStream("SVGReferencedImage.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }
            
        }


    }
}
