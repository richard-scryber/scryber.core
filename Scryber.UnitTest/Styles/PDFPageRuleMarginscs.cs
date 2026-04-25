using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;

namespace Scryber.Core.UnitTests.Styles
{
    [TestClass]
    public class PDFPageRuleMargins
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
        /// A test for parsing HTML page with specific margins on the @page rule
        /// </summary>
        [TestMethod()]
        public void PageRule_Margins_Direct()
        {
            var path = DocStreams.AssertGetTemplatePath("Html/PageMargins_Direct.html");
            // Parse the HTML document from string using StringReader
            using (Document doc = Document.ParseDocument(path))
            {
                PDFLayoutDocument layout = null;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                //doc.LayoutComplete += Doc_LayoutDocument;
                using (var stream = DocStreams.GetOutputStream("PageMargins_Direct.pdf"))
                {
                    doc.LayoutComplete += (s, e) => { layout = e.Context.GetLayout<PDFLayoutDocument>(); };
                    doc.SaveAsPDF(stream);
                }

                Assert.IsNotNull(layout);
                Assert.AreEqual(1, layout.TotalPageCount, "Total page count should be 1");

                var pg = layout.AllPages[0];
                Assert.IsNotNull(pg, "Page should be laid-out successfully");
                
                var margins = pg.PositionOptions.Margins;
                Assert.AreEqual(0, margins.Top, "Top should be 0");
                Assert.AreEqual(10, margins.Left, "Top should be 0");
                Assert.AreEqual(10, margins.Bottom, "Top should be 0");
                Assert.AreEqual(10, margins.Right, "Top should be 0");
            }
        }
        
        /// <summary>
        /// A test for parsing HTML page with specific margins on the @page rule
        /// </summary>
        [TestMethod()]
        public void PageRule_Margins_Direct_Continuation()
        {
            var path = DocStreams.AssertGetTemplatePath("Html/PageMargins_Direct_Continuation.html");
            // Parse the HTML document from string using StringReader
            using (Document doc = Document.ParseDocument(path))
            {
                PDFLayoutDocument layout = null;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                //doc.LayoutComplete += Doc_LayoutDocument;
                using (var stream = DocStreams.GetOutputStream("PageMargins_Direct_Continuation.pdf"))
                {
                    doc.LayoutComplete += (s, e) => { layout = e.Context.GetLayout<PDFLayoutDocument>(); };
                    doc.SaveAsPDF(stream);
                }

                Assert.IsNotNull(layout);
                Assert.AreEqual(2, layout.TotalPageCount, "Total page count should be 1");

                var pg = layout.AllPages[0];
                Assert.IsNotNull(pg, "Page should be laid-out successfully");
                
                var margins = pg.PositionOptions.Margins;
                Assert.AreEqual(0, margins.Top, "Top should be 0");
                Assert.AreEqual(10, margins.Left, "Top should be 10");
                Assert.AreEqual(10, margins.Bottom, "Top should be 10");
                Assert.AreEqual(10, margins.Right, "Top should be 10");
                
                pg = layout.AllPages[1];
                Assert.IsNotNull(pg, "Page should be laid-out successfully");
                
                margins = pg.PositionOptions.Margins;
                Assert.AreEqual(0, margins.Top, "Top should be 0");
                Assert.AreEqual(10, margins.Left, "Top should be 10");
                Assert.AreEqual(10, margins.Bottom, "Top should be 10");
                Assert.AreEqual(10, margins.Right, "Top should be 10");
            }
        }
        
        /// <summary>
        /// A test for parsing HTML page with specific margins on the @page rule
        /// </summary>
        [TestMethod()]
        public void PageRule_Margins_Direct_LeftAndRight()
        {
            var path = DocStreams.AssertGetTemplatePath("Html/PageMargins_Direct_LeftAndRight.html");
            // Parse the HTML document from string using StringReader
            using (Document doc = Document.ParseDocument(path))
            {
                PDFLayoutDocument layout = null;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                //doc.LayoutComplete += Doc_LayoutDocument;
                using (var stream = DocStreams.GetOutputStream("PageMargins_Direct_LeftAndRight.pdf"))
                {
                    doc.LayoutComplete += (s, e) => { layout = e.Context.GetLayout<PDFLayoutDocument>(); };
                    doc.SaveAsPDF(stream);
                }

                Assert.IsNotNull(layout);
                Assert.AreEqual(2, layout.TotalPageCount, "Total page count should be 1");

                var pg = layout.AllPages[0];
                Assert.IsNotNull(pg, "Page should be laid-out successfully");
                
                var margins = pg.PositionOptions.Margins;
                Assert.AreEqual(0, margins.Top, "Top should be 0");
                Assert.AreEqual(20, margins.Left, "Top should be 10");
                Assert.AreEqual(10, margins.Bottom, "Top should be 10");
                Assert.AreEqual(10, margins.Right, "Top should be 10");
                
                pg = layout.AllPages[1];
                Assert.IsNotNull(pg, "Page should be laid-out successfully");
                
                margins = pg.PositionOptions.Margins;
                Assert.AreEqual(0, margins.Top, "Top should be 0");
                Assert.AreEqual(10, margins.Left, "Top should be 10");
                Assert.AreEqual(10, margins.Bottom, "Top should be 10");
                Assert.AreEqual(20, margins.Right, "Top should be 10");
            }
        }

    }
}
