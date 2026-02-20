using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    [TestCategory("Page Sizes")]
    public class PageSizeLayout_Test
    {

        private PDFLayoutDocument layoutDoc;

        private void Doc_LayoutDocument(object sender, LayoutEventArgs args)
        {
            layoutDoc = args.Context.GetLayout<PDFLayoutDocument>();
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
            var defaultMargins = new StyleDefn(typeof(Section), null, null);
            defaultMargins.Margins.All = new Unit(10);
            
            doc.Styles.Add(defaultMargins);
            var names = Enum.GetNames(typeof(PaperSize));

            foreach (var name in names)
            {

                var paper = (PaperSize)Enum.Parse(typeof(PaperSize), name);

                Section section = new Section();

                section.PaperSize = paper;
                if(paper == PaperSize.Custom)
                {
                    section.Width = 400;
                    section.Height = 300;
                }
                section.Padding = new Thickness(10);
                section.Margins = new Thickness(10);
                
                section.FontSize = 20;
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
                var paper = (PaperSize)Enum.Parse(typeof(PaperSize), names[i]);
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

                var paper = (PaperSize)Enum.Parse(typeof(PaperSize), name);

                Section section = new Section();

                section.PaperSize = paper;
                if (paper == PaperSize.Custom)
                {
                    section.Width = 400;
                    section.Height = 300;
                }
                section.PaperOrientation = PaperOrientation.Landscape;
                section.Padding = new Thickness(10);
                section.FontSize = 20;
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
                var paper = (PaperSize)Enum.Parse(typeof(PaperSize),names[i]);
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


        [TestMethod("9. Defining the page sizes in an html template")]
        public void PageSizeDefaultInHTMLTemplate()
        {

            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Page Sizes</title>
    <style>
        body{ margin: 10; padding: 10; border: solid 1pt blue; }
    </style>
</head>
<body>
    <h1>Page in the default size</h1>
</body>
</html>";

            using (var reader = new StringReader(src))
            {
                var doc = Document.ParseDocument(reader);
                
                using (var stream = DocStreams.GetOutputStream("Page_SizesDefaultHTMLTemplate.pdf"))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    doc.LayoutComplete += Doc_LayoutDocument;
                    doc.SaveAsPDF(stream);
                }

            }
            
            var layout = layoutDoc;
            var pg = layout.AllPages[0];
            var w = pg.Width;
            var h = pg.Height;
            
            Assert.AreEqual(w, Unit.Mm(210));
            Assert.AreEqual(h, Unit.Mm(297));
        }
        
        
        [TestMethod]
        public void PageSizeA3LandscapeInHTMLTemplate()
        {

            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Page Sizes</title>
    <style>
        @page { 
            size: A3 landscape;
        }

        body{ margin: 10; padding: 10; border: solid 1pt blue; }
    </style>
</head>
<body>
    <h1>Page in the A3 size</h1>
</body>
</html>";

            using (var reader = new StringReader(src))
            {
                var doc = Document.ParseDocument(reader);
                
                using (var stream = DocStreams.GetOutputStream("Page_SizesA3LandscapeHTMLTemplate.pdf"))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    doc.LayoutComplete += Doc_LayoutDocument;
                    doc.SaveAsPDF(stream);
                }

            }
            
            var layout = layoutDoc;
            var pg = layout.AllPages[0];
            var w = pg.Width;
            var h = pg.Height;
            
            Assert.AreEqual(Unit.Mm(420), w.ToMillimeters());
            Assert.AreEqual(Unit.Mm(297), h.ToMillimeters());
        }
        
        [TestMethod]
        public void PageSize_A4_Page2_A2_Landscape_InHTMLTemplate()
        {

            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Page Sizes</title>
    <style>
        @page { 
            size: A4 portrait;
        }

        @page large {
            size: A2 landscape;
            margin-left: 100pt;
        }

        .chart {
            page: large;
        }

        body{ margin: 10; padding: 10; border: solid 1pt blue; }
    </style>
</head>
<body>
    <h1>Page in the A4 size</h1>
    <section class='chart'>
        <h1>Second page in the A2 landscape size</h1>
    </section>
</body>
</html>";

            using (var reader = new StringReader(src))
            {
                var doc = Document.ParseDocument(reader);
                
                using (var stream = DocStreams.GetOutputStream("Page_SizesA4_Page2_A2_Landscape_HTMLTemplate.pdf"))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    doc.LayoutComplete += Doc_LayoutDocument;
                    doc.SaveAsPDF(stream);
                }

            }
            
            var layout = layoutDoc;
            var pg = layout.AllPages[0];
            var w = pg.Width;
            var h = pg.Height;
            
            Assert.AreEqual(Unit.Mm(210), w.ToMillimeters());
            Assert.AreEqual(Unit.Mm(297), h.ToMillimeters());
            
            //check the second page
            pg = layout.AllPages[1];
            w = pg.Width;
            h = pg.Height;
            
            Assert.AreEqual(Unit.Mm(297 * 2), w.ToMillimeters());
            Assert.AreEqual(Unit.Mm(210 * 2), h.ToMillimeters());
        }

        [TestMethod]
        public void PageSize_A4_Page2_A2_Landscape_WithContinuations_InHTMLTemplate()
        {

            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Page Sizes</title>
    <style>
        @page { 
            size: A4 portrait;
        }

        @page large {
            size: A2 landscape;
            margin-left: 100pt;
        }

        .chart {
            page: large;
        }

        body{ margin: 10; padding: 10; border: solid 1pt blue; }
    </style>
</head>
<body>
    <h1>Page in the A4 size</h1>
    <section class='chart'>
        <h1>Second page in the A2 landscape size</h1>
        <div style='page-break-before: always; page-break-after: always;'>
            <h1>Third page, should stay in A2 Landscape size</h1>
        </div>
    </section>
    <h1>Fourth page, should revert back to A4</h1>
</body>
</html>";

            using (var reader = new StringReader(src))
            {
                var doc = Document.ParseDocument(reader);
                
                using (var stream = DocStreams.GetOutputStream("Page_SizesA4_Page2_A2_Landscape_WithContinuations_HTMLTemplate.pdf"))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    doc.LayoutComplete += Doc_LayoutDocument;
                    doc.SaveAsPDF(stream);
                }

            }
            
            var layout = layoutDoc;
            Assert.AreEqual(4, layout.AllPages.Count);
            
            var pg = layout.AllPages[0];
            var w = pg.Width;
            var h = pg.Height;
            
            Assert.AreEqual(Unit.Mm(210), w.ToMillimeters());
            Assert.AreEqual(Unit.Mm(297), h.ToMillimeters());
            
            //check the second page
            pg = layout.AllPages[1];
            w = pg.Width;
            h = pg.Height;
            
            Assert.AreEqual(Unit.Mm(297 * 2), w.ToMillimeters());
            Assert.AreEqual(Unit.Mm(210 * 2), h.ToMillimeters());
            
            
            //third page should be the same dimensions
            pg = layout.AllPages[1];
            w = pg.Width;
            h = pg.Height;
            
            Assert.AreEqual(Unit.Mm(297 * 2), w.ToMillimeters());
            Assert.AreEqual(Unit.Mm(210 * 2), h.ToMillimeters());
            
            //fourth page should no longer be in the large page size, so drop back to A4
            pg = layout.AllPages[1];
            w = pg.Width;
            h = pg.Height;
            
            Assert.AreEqual(Unit.Mm(210), w.ToMillimeters());
            Assert.AreEqual(Unit.Mm(297), h.ToMillimeters());
        }
    }
}
