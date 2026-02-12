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
    ///Testing the use of css properties to apply to svg elements, separated by component
    ///</summary>
    [TestClass()]
    public class SVG_CSS_Tests
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
        public void SVGLineCSS_Test()
        {
            var path = DocStreams.AssertGetTemplatePath("SVG/SVGLineCSS.html");
            
            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("SVG_XObjectOutput.pdf"))
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
        }
        
        
        
    }
}
