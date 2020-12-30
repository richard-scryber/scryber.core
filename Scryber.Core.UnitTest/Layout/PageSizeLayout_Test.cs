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

            pg.Contents.Add(new TextLiteral("A4"));
            pg.HorizontalAlignment = HorizontalAlignment.Center;
            pg.VerticalAlignment = VerticalAlignment.Top;
            pg.FontSize = 30;
            pg.Margins = new Scryber.Drawing.PDFThickness(20);

            doc.ViewPreferences.PageLayout = PageLayoutMode.SinglePage;

            using (var ms = DocStreams.GetOutputStream("PageSizeA4.pdf"))
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

            pg.Contents.Add(new TextLiteral("A4 Landscape"));
            pg.HorizontalAlignment = HorizontalAlignment.Center;
            pg.VerticalAlignment = VerticalAlignment.Top;
            pg.FontSize = 30;
            pg.Margins = new Scryber.Drawing.PDFThickness(20);

            doc.ViewPreferences.PageLayout = PageLayoutMode.SinglePage;

            using (var ms = DocStreams.GetOutputStream("PageSizeA4Landscape.pdf"))
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

            pg.Contents.Add(new TextLiteral("A3"));
            pg.HorizontalAlignment = HorizontalAlignment.Center;
            pg.VerticalAlignment = VerticalAlignment.Top;
            pg.FontSize = 30;
            pg.Margins = new Scryber.Drawing.PDFThickness(20);

            doc.ViewPreferences.PageLayout = PageLayoutMode.SinglePage;

            using (var ms = DocStreams.GetOutputStream("PageSizeA3.pdf"))
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

            pg.Contents.Add(new TextLiteral("A3 Landscape"));
            pg.HorizontalAlignment = HorizontalAlignment.Center;
            pg.VerticalAlignment = VerticalAlignment.Top;
            pg.FontSize = 30;
            pg.Margins = new Scryber.Drawing.PDFThickness(20);

            doc.ViewPreferences.PageLayout = PageLayoutMode.SinglePage;

            using (var ms = DocStreams.GetOutputStream("PageSizeA3Landscape.pdf"))
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

            pg.Contents.Add(new TextLiteral("Letter"));
            pg.HorizontalAlignment = HorizontalAlignment.Center;
            pg.VerticalAlignment = VerticalAlignment.Top;
            pg.FontSize = 30;
            pg.Margins = new Scryber.Drawing.PDFThickness(20);

            doc.ViewPreferences.PageLayout = PageLayoutMode.SinglePage;

            using (var ms = DocStreams.GetOutputStream("PageSizeLetter.pdf"))
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

            pg.Contents.Add(new TextLiteral("Explicit 400 x 800"));
            pg.HorizontalAlignment = HorizontalAlignment.Center;
            pg.VerticalAlignment = VerticalAlignment.Top;
            pg.FontSize = 30;
            pg.Margins = new Scryber.Drawing.PDFThickness(20);

            doc.ViewPreferences.PageLayout = PageLayoutMode.SinglePage;

            using (var ms = DocStreams.GetOutputStream("PageSizeExplicit.pdf"))
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
