using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Styles.Selectors;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    [TestCategory("Page Pseudo-Classes")]
    public class PagePseudoClassLayout_Tests
    {
        private PDFLayoutDocument layoutDoc;

        private void Doc_LayoutDocument(object sender, LayoutEventArgs args)
        {
            layoutDoc = args.Context.GetLayout<PDFLayoutDocument>();
        }

        /// <summary>
        /// CSS rules added via &lt;style&gt; blocks are wrapped in a StyleGroup by HTMLStyle.AddStylesToDocument.
        /// This helper recurses one level to find the first StylePageGroup.
        /// </summary>
        private static StylePageGroup FindFirstStylePageGroup(StyleCollection styles)
        {
            foreach (var s in styles)
            {
                if (s is StylePageGroup spg)
                    return spg;
                if (s is StyleGroup sg)
                {
                    var found = FindFirstStylePageGroup(sg.Styles);
                    if (found != null) return found;
                }
            }
            return null;
        }

        // -----------------------------------------------------------------------
        // Group 1 — CSS Parsing (no layout needed)
        // -----------------------------------------------------------------------

        [TestMethod("1. @page :first parses to ComponentState.First")]
        public void Parse_PageFirst()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>@page :first { margin: 20mm; } 
body { border: solid 1pt black;}</style></head>
<body><p>Content</p></body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);
            using (var ms = DocStreams.GetOutputStream("PagePseudo_Parse_First.pdf"))
                doc.SaveAsPDF(ms);

            var pg = FindFirstStylePageGroup(doc.Styles);

            Assert.IsNotNull(pg, "No StylePageGroup found");
            Assert.IsNotNull(pg.Matcher, "PageGroup has no matcher");
            Assert.AreEqual(ComponentState.First, pg.Matcher.PseudoClass, "@page :first should parse to ComponentState.First");
            Assert.IsTrue(pg.Matcher.Selectors == null || pg.Matcher.Selectors.Length == 0, "Selectors should be empty for plain :first");
        }

        [TestMethod("2. @page :left parses to ComponentState.Left")]
        public void Parse_PageLeft()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>@page :left { margin-left: 30mm; } body { border: solid 1pt black;}</style></head>
<body><p>Content</p></body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);
            using (var ms = DocStreams.GetOutputStream("PagePseudo_Parse_Left.pdf"))
                doc.SaveAsPDF(ms);

            var pg = FindFirstStylePageGroup(doc.Styles);

            Assert.IsNotNull(pg, "No StylePageGroup found");
            Assert.AreEqual(ComponentState.Left, pg.Matcher.PseudoClass, "@page :left should parse to ComponentState.Left");
        }

        [TestMethod("3. @page :right parses to ComponentState.Right")]
        public void Parse_PageRight()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>@page :right { margin-right: 25mm; } body { border: solid 1pt black;}</style></head>
<body><p>Content</p></body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);
            using (var ms = DocStreams.GetOutputStream("PagePseudo_Parse_Right.pdf"))
                doc.SaveAsPDF(ms);

            var pg = FindFirstStylePageGroup(doc.Styles);

            Assert.IsNotNull(pg, "No StylePageGroup found");
            Assert.AreEqual(ComponentState.Right, pg.Matcher.PseudoClass, "@page :right should parse to ComponentState.Right");
        }

        [TestMethod("4. @page cover:first parses to named selector + ComponentState.First")]
        public void Parse_NamedPageFirst()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>@page cover:first { margin: 5mm; } body { border: solid 1pt black;}</style></head>
<body><p>Content</p></body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);
            using (var ms = DocStreams.GetOutputStream("PagePseudo_Parse_NamedFirst.pdf"))
                doc.SaveAsPDF(ms);

            var pg = FindFirstStylePageGroup(doc.Styles);

            Assert.IsNotNull(pg, "No StylePageGroup found");
            Assert.AreEqual(ComponentState.First, pg.Matcher.PseudoClass);
            Assert.IsNotNull(pg.Matcher.Selectors, "Selectors should not be null");
            Assert.AreEqual(1, pg.Matcher.Selectors.Length);
            Assert.AreEqual("cover", pg.Matcher.Selectors[0]);
        }

        [TestMethod("5. @page cover :first (with space) parses same as cover:first")]
        public void Parse_NamedPageFirstWithSpace()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>@page cover :first { margin: 5mm; }</style></head>
<body><p>Content</p></body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);
            using (var ms = DocStreams.GetOutputStream("PagePseudo_Parse_NamedFirstSpace.pdf"))
                doc.SaveAsPDF(ms);

            var pg = FindFirstStylePageGroup(doc.Styles);

            Assert.IsNotNull(pg, "No StylePageGroup found");
            Assert.AreEqual(ComponentState.First, pg.Matcher.PseudoClass);
            Assert.IsNotNull(pg.Matcher.Selectors);
            Assert.AreEqual(1, pg.Matcher.Selectors.Length);
            Assert.AreEqual("cover", pg.Matcher.Selectors[0]);
        }

        [TestMethod("6. Plain @page has Normal PseudoClass (regression)")]
        public void Parse_PlainPageHasNormalPseudoClass()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>@page { margin: 10mm; }</style></head>
<body><p>Content</p></body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);
            using (var ms = DocStreams.GetOutputStream("PagePseudo_Parse_Plain.pdf"))
                doc.SaveAsPDF(ms);

            var pg = FindFirstStylePageGroup(doc.Styles);
            Assert.IsNotNull(pg, "No StylePageGroup found for plain @page");
            var pseudo = pg.Matcher?.PseudoClass ?? ComponentState.Normal;
            Assert.AreEqual(ComponentState.Normal, pseudo, "Plain @page should have Normal PseudoClass");
        }

        // -----------------------------------------------------------------------
        // Group 2 — :first size and margins
        // -----------------------------------------------------------------------

        [TestMethod("7. @page :first overrides size on page 0 only")]
        public void FirstPage_SizeOverride()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>
    @page { size: A4 portrait; }
    @page :first { size: A3 landscape; }
</style></head>
<body>
    <p>Page 1</p>
    <div style='page-break-before:always'><p>Page 2</p></div>
    <div style='page-break-before:always'><p>Page 3</p></div>
</body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);

            using (var ms = DocStreams.GetOutputStream("PagePseudo_FirstSizeOverride.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(3, layoutDoc.AllPages.Count);

            // Page 0 should be A3 landscape
            var pg0 = layoutDoc.AllPages[0];
            Assert.AreEqual(Unit.Mm(420), pg0.Width.ToMillimeters(), "Page 0 width should be A3 landscape (420mm)");
            Assert.AreEqual(Unit.Mm(297), pg0.Height.ToMillimeters(), "Page 0 height should be A3 landscape (297mm)");

            // Pages 1 and 2 should be A4 portrait
            var pg1 = layoutDoc.AllPages[1];
            Assert.AreEqual(Unit.Mm(210), pg1.Width.ToMillimeters(), "Page 1 width should be A4 portrait (210mm)");
            Assert.AreEqual(Unit.Mm(297), pg1.Height.ToMillimeters(), "Page 1 height should be A4 portrait (297mm)");

            var pg2 = layoutDoc.AllPages[2];
            Assert.AreEqual(Unit.Mm(210), pg2.Width.ToMillimeters(), "Page 2 width should be A4 portrait (210mm)");
        }

        [TestMethod("8. @page :first margin applies to page 0 only")]
        public void FirstPage_MarginOverride()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>
    @page { margin: 10mm; }
    @page :first { margin: 30mm; }
    body { border: solid 1pt black; }
</style></head>
<body>
    <p>Page 1</p>
    <div style='page-break-before:always'><p>Page 2</p></div>
</body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);

            using (var ms = DocStreams.GetOutputStream("PagePseudo_FirstMarginOverride.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layoutDoc.AllPages.Count);

            var pg0 = layoutDoc.AllPages[0];
            Assert.AreEqual(Unit.Mm(30), pg0.PositionOptions.Margins.Top.ToMillimeters(), "Page 0 top margin should be 30mm");
            Assert.AreEqual(Unit.Mm(30), pg0.PositionOptions.Margins.Left.ToMillimeters(), "Page 0 left margin should be 30mm");

            var pg1 = layoutDoc.AllPages[1];
            Assert.AreEqual(Unit.Mm(10), pg1.PositionOptions.Margins.Top.ToMillimeters(), "Page 1 top margin should be 10mm");
            Assert.AreEqual(Unit.Mm(10), pg1.PositionOptions.Margins.Left.ToMillimeters(), "Page 1 left margin should be 10mm");
        }

        [TestMethod("9. @page :first only - page 0 gets margin, page 1 gets default")]
        public void FirstPage_OnlyPseudo_NoBase()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>
    @page :first { margin-top: 50mm; }
    body { border: solid 1pt black;}
</style></head>
<body>
    <p>Page 1</p>
    <div style='page-break-before:always'><p>Page 2</p></div>
</body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);

            using (var ms = DocStreams.GetOutputStream("PagePseudo_FirstOnly.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layoutDoc.AllPages.Count);

            var pg0 = layoutDoc.AllPages[0];
            Assert.AreEqual(Unit.Mm(50), pg0.PositionOptions.Margins.Top.ToMillimeters(), "Page 0 top margin should be 50mm");

            var pg1 = layoutDoc.AllPages[1];
            Assert.AreNotEqual(Unit.Mm(50), pg1.PositionOptions.Margins.Top.ToMillimeters(), "Page 1 should NOT have the :first margin");
        }

        // -----------------------------------------------------------------------
        // Group 3 — :left / :right alternating margins
        // -----------------------------------------------------------------------

        [TestMethod("10. @page :right margin applies to even-indexed pages (0, 2)")]
        public void RightPage_AlternatingMargins()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>
    @page { margin: 10mm; }
    @page :right { margin-right: 40mm; }
    body { border: solid 1pt black;}
</style></head>
<body>
    <p>Page 1</p>
    <div style='page-break-before:always'><p>Page 2</p></div>
    <div style='page-break-before:always'><p>Page 3</p></div>
    <div style='page-break-before:always'><p>Page 4</p></div>
</body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);

            using (var ms = DocStreams.GetOutputStream("PagePseudo_RightAlternating.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(4, layoutDoc.AllPages.Count);

            // :right = even indices (0, 2) → margin-right = 40mm
            Assert.AreEqual(Unit.Mm(40), layoutDoc.AllPages[0].PositionOptions.Margins.Right.ToMillimeters(), "Page 0 (:right) should have 40mm right margin");
            Assert.AreEqual(Unit.Mm(10), layoutDoc.AllPages[1].PositionOptions.Margins.Right.ToMillimeters(), "Page 1 (:left)  should have 10mm right margin");
            Assert.AreEqual(Unit.Mm(40), layoutDoc.AllPages[2].PositionOptions.Margins.Right.ToMillimeters(), "Page 2 (:right) should have 40mm right margin");
            Assert.AreEqual(Unit.Mm(10), layoutDoc.AllPages[3].PositionOptions.Margins.Right.ToMillimeters(), "Page 3 (:left)  should have 10mm right margin");
        }

        [TestMethod("11. @page :left margin applies to odd-indexed pages (1, 3)")]
        public void LeftPage_AlternatingMargins()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>
    @page { margin: 10mm; }
    @page :left { margin-left: 40mm; }
    body { border: solid 1pt black;}
</style></head>
<body>
    <p>Page 1</p>
    <div style='page-break-before:always'><p>Page 2</p></div>
    <div style='page-break-before:always'><p>Page 3</p></div>
    <div style='page-break-before:always'><p>Page 4</p></div>
</body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);

            using (var ms = DocStreams.GetOutputStream("PagePseudo_LeftAlternating.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(4, layoutDoc.AllPages.Count);

            // :left = odd indices (1, 3) → margin-left = 40mm
            Assert.AreEqual(Unit.Mm(10), layoutDoc.AllPages[0].PositionOptions.Margins.Left.ToMillimeters(), "Page 0 (:right) should have 10mm left margin");
            Assert.AreEqual(Unit.Mm(40), layoutDoc.AllPages[1].PositionOptions.Margins.Left.ToMillimeters(), "Page 1 (:left)  should have 40mm left margin");
            Assert.AreEqual(Unit.Mm(10), layoutDoc.AllPages[2].PositionOptions.Margins.Left.ToMillimeters(), "Page 2 (:right) should have 10mm left margin");
            Assert.AreEqual(Unit.Mm(40), layoutDoc.AllPages[3].PositionOptions.Margins.Left.ToMillimeters(), "Page 3 (:left)  should have 40mm left margin");
        }

        [TestMethod("12. Both @page :left and :right defined — each page gets the correct margin")]
        public void LeftAndRight_BothDefined()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>
    @page { margin: 10mm; }
    @page :left  { margin-left: 40mm; margin-right: 20mm; }
    @page :right { margin-left: 20mm; margin-right: 40mm; }
    body { border: solid 1pt black;}
</style></head>
<body>
    <p>Page 1</p>
    <div style='page-break-before:always'><p>Page 2</p></div>
    <div style='page-break-before:always'><p>Page 3</p></div>
    <div style='page-break-before:always'><p>Page 4</p></div>
</body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);

            using (var ms = DocStreams.GetOutputStream("PagePseudo_LeftAndRight.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(4, layoutDoc.AllPages.Count);

            // Page 0 and 2 are :right → left=20mm, right=40mm
            Assert.AreEqual(Unit.Mm(20), layoutDoc.AllPages[0].PositionOptions.Margins.Left.ToMillimeters(),  "Page 0 (:right) left margin");
            Assert.AreEqual(Unit.Mm(40), layoutDoc.AllPages[0].PositionOptions.Margins.Right.ToMillimeters(), "Page 0 (:right) right margin");
            Assert.AreEqual(Unit.Mm(20), layoutDoc.AllPages[2].PositionOptions.Margins.Left.ToMillimeters(),  "Page 2 (:right) left margin");
            Assert.AreEqual(Unit.Mm(40), layoutDoc.AllPages[2].PositionOptions.Margins.Right.ToMillimeters(), "Page 2 (:right) right margin");

            // Page 1 and 3 are :left → left=40mm, right=20mm
            Assert.AreEqual(Unit.Mm(40), layoutDoc.AllPages[1].PositionOptions.Margins.Left.ToMillimeters(),  "Page 1 (:left) left margin");
            Assert.AreEqual(Unit.Mm(20), layoutDoc.AllPages[1].PositionOptions.Margins.Right.ToMillimeters(), "Page 1 (:left) right margin");
            Assert.AreEqual(Unit.Mm(40), layoutDoc.AllPages[3].PositionOptions.Margins.Left.ToMillimeters(),  "Page 3 (:left) left margin");
            Assert.AreEqual(Unit.Mm(20), layoutDoc.AllPages[3].PositionOptions.Margins.Right.ToMillimeters(), "Page 3 (:left) right margin");
        }

        // -----------------------------------------------------------------------
        // Group 4 — Named @page + pseudo-class
        // -----------------------------------------------------------------------

        [TestMethod("13. @page large:first applies 40mm only to first layout page of named section")]
        public void NamedPage_FirstPseudo()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>
    body { border: solid 1pt black; }
    @page large { size: A3 portrait; margin: 10mm; }
    @page large:first { margin: 40mm; }
    .chart { page: large; }
</style></head>
<body class='chart'>
    <p>Named page 1</p>
    <div style='page-break-before:always'><p>Named page 2</p></div>
    <div style='page-break-before:always'><p>Named page 3</p></div>
</body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);

            using (var ms = DocStreams.GetOutputStream("PagePseudo_NamedFirst.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            Assert.IsTrue(layoutDoc.AllPages.Count == 3, "Expected 3 pages");

            // First page (index 0) should have 40mm margins from large:first
            Assert.AreEqual(Unit.Mm(40), layoutDoc.AllPages[0].PositionOptions.Margins.Top.ToMillimeters(), "First large page should have 40mm top margin");

            // Subsequent pages should have 10mm margins from @page large
            Assert.AreEqual(Unit.Mm(10), layoutDoc.AllPages[1].PositionOptions.Margins.Top.ToMillimeters(), "Second large page should have 10mm top margin");
            Assert.AreEqual(Unit.Mm(10), layoutDoc.AllPages[2].PositionOptions.Margins.Top.ToMillimeters(), "Third large page should have 10mm top margin");
        }

        [TestMethod("14. @page wide:right applies 30mm right margin to even-indexed pages in named section")]
        public void NamedPage_RightPseudo()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>
    body { border: solid 1px black; }
   
    @page wide { size: A3 landscape; margin: 10mm; }
    @page wide:right { margin-right: 30mm; }
    .wide { page: wide; }
</style></head>
<body class='wide'>
        <p>Wide page 1</p>
        <div style='page-break-before:always'><p>Wide page 2</p></div>
        <div style='page-break-before:always'><p>Wide page 3</p></div>
        <div style='page-break-before:always'><p>Wide page 4</p></div>
</body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);

            using (var ms = DocStreams.GetOutputStream("PagePseudo_NamedRight.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            Assert.IsTrue(layoutDoc.AllPages.Count == 4, "Expected 4 pages");

            Assert.AreEqual(Unit.Mm(10), layoutDoc.AllPages[0].PositionOptions.Margins.Left.ToMillimeters(), "Page 0  should have 0mm left margin");
            Assert.AreEqual(Unit.Mm(30), layoutDoc.AllPages[0].PositionOptions.Margins.Right.ToMillimeters(), "Page 0 (:right) should have 30mm right margin");
            Assert.AreEqual(Unit.Mm(10), layoutDoc.AllPages[1].PositionOptions.Margins.Right.ToMillimeters(), "Page 1 (:left)  should have 10mm right margin");
            Assert.AreEqual(Unit.Mm(30), layoutDoc.AllPages[2].PositionOptions.Margins.Right.ToMillimeters(), "Page 2 (:right) should have 30mm right margin");
            Assert.AreEqual(Unit.Mm(10), layoutDoc.AllPages[3].PositionOptions.Margins.Right.ToMillimeters(), "Page 3 (:left)  should have 10mm right margin");
        }

        // -----------------------------------------------------------------------
        // Group 5 — Specificity / declaration order
        // -----------------------------------------------------------------------

        [TestMethod("15. :first declared before plain @page still wins (priority 20 > 0)")]
        public void Specificity_FirstBeforePlain()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>
    body { border: solid 1px black; }
    @page :first { margin: 40mm; }
    @page { margin: 10mm; }
</style></head>
<body>
    <p>Page 1</p>
    <div style='page-break-before:always'><p>Page 2</p></div>
</body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);

            using (var ms = DocStreams.GetOutputStream("PagePseudo_SpecificityFirstBeforePlain.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layoutDoc.AllPages.Count);
            Assert.AreEqual(Unit.Mm(40), layoutDoc.AllPages[0].PositionOptions.Margins.Top.ToMillimeters(), "Page 0 :first should win with 40mm even though declared first");
            Assert.AreEqual(Unit.Mm(10), layoutDoc.AllPages[1].PositionOptions.Margins.Top.ToMillimeters(), "Page 1 should use plain @page 10mm");
        }

        [TestMethod("16. :first declared after plain @page still wins (specificity over source order)")]
        public void Specificity_FirstAfterPlain()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>
    body { border: solid 1px black; }
    @page { margin: 10mm; }
    @page :first { margin: 40mm; }
</style></head>
<body>
    <p>Page 1</p>
    <div style='page-break-before:always'><p>Page 2</p></div>
</body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);

            using (var ms = DocStreams.GetOutputStream("PagePseudo_SpecificityFirstAfterPlain.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layoutDoc.AllPages.Count);
            Assert.AreEqual(Unit.Mm(40), layoutDoc.AllPages[0].PositionOptions.Margins.Top.ToMillimeters(), "Page 0 :first should win with 40mm");
            Assert.AreEqual(Unit.Mm(10), layoutDoc.AllPages[1].PositionOptions.Margins.Top.ToMillimeters(), "Page 1 should use plain @page 10mm");
        }

        [TestMethod("17. All four specificity levels: name+pseudo (30) > pseudo (20) > name (10) > plain (0)")]
        public void Specificity_AllFourLevels()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>
    body { border: solid 1px black; }
    @page { margin: 5mm; }
    @page cover { margin: 15mm; }
    @page :first { margin: 25mm; }
    @page cover:first { margin: 35mm; }
    .cover { page: cover; }
</style></head>
<body>
    <div class='cover'>
        <p>Cover page (index 0 - should get 35mm from cover:first)</p>
    </div>
    <p>Content page 1 (index 1 - plain @page, 5mm)</p>
    <div style='page-break-before:always'><p>Content page 2 (index 2 - plain @page, 5mm)</p></div>
</body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);

            using (var ms = DocStreams.GetOutputStream("PagePseudo_AllFourLevels.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            Assert.IsTrue(layoutDoc.AllPages.Count >= 2, "Expected at least 2 pages");

            // Cover page (index 0, named cover + :first) → 35mm wins
            Assert.AreEqual(Unit.Mm(35), layoutDoc.AllPages[0].PositionOptions.Margins.Top.ToMillimeters(),
                "Cover page (index 0, cover:first = priority 30) should have 35mm margin");

            // Plain content pages → plain @page 5mm
            Assert.AreEqual(Unit.Mm(5), layoutDoc.AllPages[1].PositionOptions.Margins.Top.ToMillimeters(),
                "Content page (no name, no :first) should have 5mm margin from plain @page");
        }

        // -----------------------------------------------------------------------
        // Group 6 — Continuation pages (cross-section)
        // -----------------------------------------------------------------------

        [TestMethod("18. :right/:left page index is document-absolute across section boundaries")]
        public void ContinuationPages_DocumentAbsoluteIndex()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>
    body {border: solid 1pt black}
    @page { margin: 10mm; }
    @page :right { margin-right: 30mm; }
</style></head>
<body>
    <p>Section 1 - page 1 (index 0, :right)</p>
    <div style='page-break-before:always'><p>Section 1 - page 2 (index 1, :left)</p></div>
    <section>
        <p>Section 2 - page 3 (index 2, :right)</p>
        <div style='page-break-before:always'><p>Section 2 - page 4 (index 3, :left)</p></div>
    </section>
</body></html>";

            using var reader = new StringReader(src);
            var doc = Document.ParseDocument(reader);

            using (var ms = DocStreams.GetOutputStream("PagePseudo_DocumentAbsoluteIndex.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutDocument;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(4, layoutDoc.AllPages.Count, "Expected 4 pages");

            // Indices 0 and 2 are :right → 30mm right margin
            Assert.AreEqual(Unit.Mm(30), layoutDoc.AllPages[0].PositionOptions.Margins.Right.ToMillimeters(), "Page 0 (index 0, :right) should have 30mm right margin");
            Assert.AreEqual(Unit.Mm(10), layoutDoc.AllPages[1].PositionOptions.Margins.Right.ToMillimeters(), "Page 1 (index 1, :left)  should have 10mm right margin");
            Assert.AreEqual(Unit.Mm(30), layoutDoc.AllPages[2].PositionOptions.Margins.Right.ToMillimeters(), "Page 2 (index 2, :right) should have 30mm right margin");
            Assert.AreEqual(Unit.Mm(10), layoutDoc.AllPages[3].PositionOptions.Margins.Right.ToMillimeters(), "Page 3 (index 3, :left)  should have 10mm right margin");
        }
    }
}
