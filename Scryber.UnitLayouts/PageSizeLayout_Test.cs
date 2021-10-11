using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using Scryber.Drawing;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    [TestCategory("Page Sizes")]
    public class PageSizeLayout_Test
    {

        private PDFLayoutDocument layoutDoc;

        private void Doc_LayoutDocument(object sender, PDFLayoutEventArgs args)
        {
            layoutDoc = args.Context.DocumentLayout;
        }


        [TestMethod("1. Page size Single A4")]
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
            pg.Margins = new Scryber.Drawing.Thickness(20);

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

        [TestMethod("2. Page size Single A4 Landscape")]
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
            pg.Margins = new Scryber.Drawing.Thickness(20);

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

        [TestMethod("3. Page size Single A3")]
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
            pg.Margins = new Scryber.Drawing.Thickness(20);

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

        [TestMethod("4. Page size Single A3 Landscape")]
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
            pg.Margins = new Scryber.Drawing.Thickness(20);

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

        [TestMethod("5. Page size Single Letter")]
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
            pg.Margins = new Scryber.Drawing.Thickness(20);

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

        [TestMethod("6. Page size Explicit 400 x 800")]
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
            pg.Margins = new Scryber.Drawing.Thickness(20);

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


        /// <summary>
        /// All The ISO page sizes in a document
        /// </summary>
        [TestMethod("7. All the known paper sizes")]
        public void PaperSizesInADocument()
        {

            Document doc = new Document();
            var names = Enum.GetNames(typeof(PaperSize));

            foreach (var name in names)
            {

                var paper = Enum.Parse<PaperSize>(name);

                Section section = new Section();

                section.PaperSize = paper;
                if(paper == PaperSize.Custom)
                {
                    section.Width = 400;
                    section.Height = 300;
                }
                section.Padding = new Thickness(10);
                section.FontSize = 10;
                section.HorizontalAlignment = HorizontalAlignment.Center;

                var label = new TextLiteral("Page size " + name);
                section.Contents.Add(label);

                doc.Pages.Add(section);


            }

            using(var stream = DocStreams.GetOutputStream("Page_SizesAll.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(stream);
            }

            var layout = layoutDoc;

            Assert.AreEqual(names.Length, layout.AllPages.Count, "Not all pages were output");

            for(var i = 0; i < names.Length; i++)
            {
                var paper = Enum.Parse<PaperSize>(names[i]);
                Size size;
                if (paper == PaperSize.Custom)
                    size = new Size(400, 300);
                else
                    size = Papers.GetSizeInDeviceIndependentUnits(paper);

                var layoutPg = layout.AllPages[i];

                Assert.AreEqual(layoutPg.Width, size.Width, "The size of paper " + names[i] + " did not match the output size of the layout page " + i + " which was expected to be " + size.ToString());
                Assert.AreEqual(layoutPg.Height, size.Height, "The size of paper " + names[i] + " did not match the output size of the layout page " + i + " which was expected to be " + size.ToString());
            }
            
        }

        /// <summary>
        /// All The ISO page sizes as Landscape in a document
        /// </summary>
        [TestMethod("7. All the known paper sizes as landscape")]
        public void PaperLandscapeSizesInADocument()
        {

            Document doc = new Document();
            var names = Enum.GetNames(typeof(PaperSize));

            foreach (var name in names)
            {

                var paper = Enum.Parse<PaperSize>(name);

                Section section = new Section();

                section.PaperSize = paper;
                if (paper == PaperSize.Custom)
                {
                    section.Width = 400;
                    section.Height = 300;
                }
                section.PaperOrientation = PaperOrientation.Landscape;
                section.Padding = new Thickness(10);
                section.FontSize = 10;
                section.HorizontalAlignment = HorizontalAlignment.Center;

                var label = new TextLiteral("Page size " + name);
                section.Contents.Add(label);

                doc.Pages.Add(section);


            }

            using (var stream = DocStreams.GetOutputStream("Page_SizesAllLandscape.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(stream);
            }

            var layout = layoutDoc;

            Assert.AreEqual(names.Length, layout.AllPages.Count, "Not all pages were output");

            for (var i = 0; i < names.Length; i++)
            {
                var paper = Enum.Parse<PaperSize>(names[i]);
                Size size;
                if (paper == PaperSize.Custom)
                    size = new Size(400, 300);
                else
                {
                    size = Papers.GetSizeInDeviceIndependentUnits(paper);
                    //Landscape so should be Height = paper width and Width = paper height
                    size = new Size(size.Height, size.Width);
                }

                var layoutPg = layout.AllPages[i];

                Assert.AreEqual(layoutPg.Width, size.Width, "The size of paper " + names[i] + " did not match the output size of the layout page " + i + " which was expected to be " + size.ToString());
                Assert.AreEqual(layoutPg.Height, size.Height, "The size of paper " + names[i] + " did not match the output size of the layout page " + i + " which was expected to be " + size.ToString());
            }

        }

        /// <summary>
        /// Various page sizes and orientations in a document flowing onto a new layout page for sections
        /// </summary>
        [TestMethod("8. Known paper sizes overflowing")]
        public void PaperSizeContinuationInADocument()
        {

            Document doc = new Document();
            var papers = new PaperSize[] { PaperSize.A4, PaperSize.A2, PaperSize.Quarto };
            var orientations = new PaperOrientation[] { PaperOrientation.Portrait, PaperOrientation.Landscape, PaperOrientation.Portrait };
            var lengths = new int[] { 3, 6, 2 };

            for(int i = 0; i < papers.Length; i++)
            {
                var section = new Section();
                section.PaperSize = papers[i];
                section.PaperOrientation = orientations[i];
                doc.Pages.Add(section);

                for(var j = 0; j < lengths[i]; j ++)
                {
                    if (j > 0)
                        section.Contents.Add(new PageBreak());

                    var label = new TextLiteral("Page size " + papers[i].ToString() + " with index " + j);
                    section.Contents.Add(label);

                }

            }

            using (var stream = DocStreams.GetOutputStream("Page_SizesContinuations.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(stream);
            }

            var layout = layoutDoc;
            int pgIndex = 0;

            for (int i = 0; i < papers.Length; i++)
            {

                var expected = Papers.GetSizeInDeviceIndependentUnits(papers[i]);
                if (orientations[i] == PaperOrientation.Landscape)
                    expected = new Size(expected.Height, expected.Width);


                for (var j = 0; j < lengths[i]; j++)
                {
                    var layoutPg = layout.AllPages[pgIndex];

                    Assert.AreEqual(layoutPg.Width, expected.Width, "The size of page " + pgIndex + " did not match the output size of the layout page " + i + " which was expected to be " + expected.ToString());
                    Assert.AreEqual(layoutPg.Height, expected.Height, "The size of paper " + pgIndex + " did not match the output size of the layout page " + i + " which was expected to be " + expected.ToString());

                    pgIndex++;
                }

            }

            Assert.AreEqual(pgIndex, layout.AllPages.Count, "The number of pages did not match the expected output");

        }


    }
}
