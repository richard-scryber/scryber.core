using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Layout;

namespace Scryber.Core.UnitTests.Layout
{
    [TestClass]
    public class PageSizeLayout_Test
    {

        private PDFLayoutDocument layoutDoc;

        private void Doc_LayoutDocument(object sender, PDFLayoutEventArgs args)
        {
            layoutDoc = args.Context.DocumentLayout;
        }


        [TestMethod]
        [TestCategory("Document Layout")]
        public void PageSizeA4()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.PaperSize = PaperSize.A4;
            doc.Pages.Add(pg);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            PDFLayoutPage layoutPg = layoutDoc.AllPages[0];

            Assert.AreEqual((int)layoutPg.Width.ToMillimeters().Value, 210);
            Assert.AreEqual((int)layoutPg.Height.ToMillimeters().Value, 297);
            
        }

        [TestMethod]
        [TestCategory("Document Layout")]
        public void PageSizeA4Landscape()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.PaperSize = PaperSize.A4;
            pg.PaperOrientation = PaperOrientation.Landscape;
            doc.Pages.Add(pg);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            PDFLayoutPage layoutPg = layoutDoc.AllPages[0];

            Assert.AreEqual((int)layoutPg.Width.ToMillimeters().Value, 297);
            Assert.AreEqual((int)layoutPg.Height.ToMillimeters().Value, 210);

        }

        [TestMethod]
        [TestCategory("Document Layout")]
        public void PageSizeA3()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.PaperSize = PaperSize.A3;
            doc.Pages.Add(pg);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            PDFLayoutPage layoutPg = layoutDoc.AllPages[0];

            Assert.AreEqual((int)layoutPg.Width.ToMillimeters().Value, 297);
            Assert.AreEqual((int)layoutPg.Height.ToMillimeters().Value, 420);

        }

        [TestMethod]
        [TestCategory("Document Layout")]
        public void PageSizeA3Landscape()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.PaperSize = PaperSize.A3;
            pg.PaperOrientation = PaperOrientation.Landscape;
            doc.Pages.Add(pg);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            PDFLayoutPage layoutPg = layoutDoc.AllPages[0];

            Assert.AreEqual((int)layoutPg.Width.ToMillimeters().Value, 420);
            Assert.AreEqual((int)layoutPg.Height.ToMillimeters().Value, 297);

        }

        [TestMethod]
        [TestCategory("Document Layout")]
        public void PageSizeLetter()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.PaperSize = PaperSize.Letter;
            doc.Pages.Add(pg);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            PDFLayoutPage layoutPg = layoutDoc.AllPages[0];

            Assert.AreEqual((int)layoutPg.Width.ToMillimeters().Value, 216);
            Assert.AreEqual((int)layoutPg.Height.ToMillimeters().Value, 279);

        }

        [TestMethod]
        [TestCategory("Document Layout")]
        public void PageSizeExplicit()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Width = 400;
            pg.Height = 800;

            doc.Pages.Add(pg);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            PDFLayoutPage layoutPg = layoutDoc.AllPages[0];

            Assert.AreEqual((int)layoutPg.Width.Value, 400);
            Assert.AreEqual((int)layoutPg.Height.Value, 800);

        }

        
    }
}
