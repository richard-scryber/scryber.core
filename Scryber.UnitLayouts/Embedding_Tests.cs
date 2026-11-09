using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.Styles;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class Embedding_Tests
    {
        private const string TestCategory = "Embedding";

        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDFLayoutDocument>();
        }
        

        // -----------------------------------------------------------------------
        // FlexRow — basic two-child layout
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Embedding_SingleEmbeddedContent()
        {
            var path = DocStreams.AssertGetTemplatePath("Content/HTML/WithEmbeddedContent.html");

            using var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Embedding_SingleEmbeddedContent.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);

            var lpg        = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var panelBlock = pageRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock, "Panel layout block should not be null");
        }
        
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Embedding_NestedEmbeddedContent()
        {
            var path = DocStreams.AssertGetTemplatePath("Content/HTML/NestedEmbeddedContent.html");

            using var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Embedding_NestedEmbeddedContent.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);

            var lpg        = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var panelBlock = pageRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock, "Panel layout block should not be null");
        }
        
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Embedding_SelfReferencedEmbeddedContent()
        {
            var path = DocStreams.AssertGetTemplatePath("Content/HTML/SelfReferencedEmbeddedContent.html");

            bool caught = false;
            Document doc = null;
            Exception ex = null;
            try
            {
                doc = Document.ParseDocument(path);
            }
            catch (Exception e)
            {
                ex = e;
                caught = true;    
            }
            
            Assert.IsTrue(caught, "Should nnot be able to parse self referencing embedded content");
            Assert.IsInstanceOfType<PDFParserException>(ex, "Should be a PDFParserException");
        }

    }
}
