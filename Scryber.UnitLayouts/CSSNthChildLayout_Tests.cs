using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF;
using Scryber.PDF.Layout;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Verifies the CSS structural pseudo-class family (:nth-child, :first-child, :last-child,
    /// :nth-of-type, etc.) against actual rendered layout geometry, including that whitespace
    /// text nodes between elements (typical from indented HTML source) are not counted as
    /// siblings, and that the "of-type" family counts only same-tag siblings.
    /// </summary>
    [TestClass()]
    public class CSSNthChildLayout_Tests
    {
        private const string TestCategory = "Inject-Layouts";

        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void NthChild_OddEven_AlternatingWidths()
        {
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <style>
        p { height: 10pt; border: solid 1pt black; }
        p:nth-child(odd) { width: 100pt; border-color: blue; background-color: #DCE8FF; }
        p:nth-child(even) { width: 200pt; border-color: red; background-color: #FFE0E0; }
    </style>
</head>
<body>
    <p>One (odd)</p>
    <p>Two (even)</p>
    <p>Three (odd)</p>
    <p>Four (even)</p>
    <p>Five (odd)</p>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            using (var ms = DocStreams.GetOutputStream("CSSNthChildLayout_OddEven.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];

            Assert.AreEqual(5, pageRegion.Contents.Count, "Expected 5 paragraph blocks - whitespace text nodes must not be counted as content items here");

            double[] expected = { 100.0, 200.0, 100.0, 200.0, 100.0 };
            for (int i = 0; i < expected.Length; i++)
            {
                var block = pageRegion.Contents[i] as PDFLayoutBlock;
                Assert.IsNotNull(block, $"Paragraph {i + 1} layout block should not be null");
                Assert.AreEqual(expected[i], block.TotalBounds.Width.PointsValue, 1.0,
                    $"Paragraph {i + 1} (1-based index {i + 1}) should be {(  (i + 1) % 2 == 1 ? "odd (100pt)" : "even (200pt)")}");
            }
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FirstChild_LastChild_DistinctWidths()
        {
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <style>
        p { height: 10pt; width: 150pt; border: solid 1pt gray; }
        p:first-child { width: 50pt; border-color: green; background-color: #E0FFE0; }
        p:last-child { width: 300pt; border-color: red; background-color: #FFE0E0; }
    </style>
</head>
<body>
    <p>First</p>
    <p>Middle</p>
    <p>Last</p>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            using (var ms = DocStreams.GetOutputStream("CSSNthChildLayout_FirstLast.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];

            var first = pageRegion.Contents[0] as PDFLayoutBlock;
            var middle = pageRegion.Contents[1] as PDFLayoutBlock;
            var last = pageRegion.Contents[2] as PDFLayoutBlock;

            Assert.AreEqual(50.0, first.TotalBounds.Width.PointsValue, 1.0, "First child should be 50pt");
            Assert.AreEqual(150.0, middle.TotalBounds.Width.PointsValue, 1.0, "Middle child should use the default 150pt");
            Assert.AreEqual(300.0, last.TotalBounds.Width.PointsValue, 1.0, "Last child should be 300pt");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void NthOfType_CountsOnlySameTagSiblings()
        {
            //Interleaved <p> and <div> elements: nth-of-type on <p> must count only the <p> siblings,
            //ignoring the interspersed <div> elements entirely.
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <style>
        p { height: 10pt; width: 100pt; border: solid 1pt gray; }
        div.marker { height: 5pt; width: 20pt; border: dashed 1pt silver; }
        p:nth-of-type(odd) { width: 250pt; border-color: purple; background-color: #F0E0FF; }
    </style>
</head>
<body>
    <p>P1 (of-type index 1 - odd)</p>
    <div class='marker'>marker A</div>
    <p>P2 (of-type index 2 - even)</p>
    <div class='marker'>marker B</div>
    <p>P3 (of-type index 3 - odd)</p>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            using (var ms = DocStreams.GetOutputStream("CSSNthChildLayout_NthOfType.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];

            Assert.AreEqual(5, pageRegion.Contents.Count);

            var p1 = pageRegion.Contents[0] as PDFLayoutBlock;
            var markerA = pageRegion.Contents[1] as PDFLayoutBlock;
            var p2 = pageRegion.Contents[2] as PDFLayoutBlock;
            var markerB = pageRegion.Contents[3] as PDFLayoutBlock;
            var p3 = pageRegion.Contents[4] as PDFLayoutBlock;

            Assert.AreEqual(250.0, p1.TotalBounds.Width.PointsValue, 1.0, "P1 is of-type index 1 (odd)");
            Assert.AreEqual(100.0, p2.TotalBounds.Width.PointsValue, 1.0, "P2 is of-type index 2 (even) - default width");
            Assert.AreEqual(250.0, p3.TotalBounds.Width.PointsValue, 1.0, "P3 is of-type index 3 (odd), despite being overall sibling index 5");

            Assert.AreEqual(20.0, markerA.TotalBounds.Width.PointsValue, 1.0, "Marker divs are unaffected by the p:nth-of-type rule");
            Assert.AreEqual(20.0, markerB.TotalBounds.Width.PointsValue, 1.0, "Marker divs are unaffected by the p:nth-of-type rule");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void NthChild_Formula_SkipsFirstTwoThenEveryThird()
        {
            //:nth-child(3n) matches indexes 3, 6, 9... - verifies the general an+b formula end to end.
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <style>
        p { height: 10pt; width: 100pt; border: solid 1pt gray; }
        p:nth-child(3n) { width: 400pt; border-color: orange; background-color: #FFF0DC; }
    </style>
</head>
<body>
    <p>1</p>
    <p>2</p>
    <p>3 (3n)</p>
    <p>4</p>
    <p>5</p>
    <p>6 (3n)</p>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            using (var ms = DocStreams.GetOutputStream("CSSNthChildLayout_FormulaThirds.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];

            double[] expected = { 100.0, 100.0, 400.0, 100.0, 100.0, 400.0 };
            for (int i = 0; i < expected.Length; i++)
            {
                var block = pageRegion.Contents[i] as PDFLayoutBlock;
                Assert.AreEqual(expected[i], block.TotalBounds.Width.PointsValue, 1.0, $"Item {i + 1} width mismatch");
            }
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void ClassPseudoClass_AsDescendantAncestor_MatchesOnlyUnderQualifyingAncestor()
        {
            //A leading <span> means the first <div class='class'> is NOT :first-child, only
            //:first-of-type - proving the ancestor's structural check is genuinely evaluated
            //against the ancestor component, not accidentally passing via first-child coincidence.
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <style>
        div.box { border: solid 1pt gray; padding: 4pt; margin-bottom: 4pt; }
        p.sub { height: 10pt; width: 100pt; border: solid 1pt gray; }
        .box:first-of-type { border-color: green; background-color: #E0FFE0; }
        .box:first-of-type .sub { width: 300pt; border-color: green; background-color: #C0FFC0; }
    </style>
</head>
<body>
    <span>marker</span>
    <div class='box'>
        <p class='sub'>In first box (first-of-type, not first-child)</p>
    </div>
    <div class='box'>
        <p class='sub'>In second box</p>
    </div>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            using (var ms = DocStreams.GetOutputStream("CSSNthChildLayout_ClassPseudoAncestor.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];

            var firstBox = pageRegion.Contents[1] as PDFLayoutBlock;
            var secondBox = pageRegion.Contents[2] as PDFLayoutBlock;

            var firstSub = firstBox.Columns[0].Contents[0] as PDFLayoutBlock;
            var secondSub = secondBox.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(300.0, firstSub.TotalBounds.Width.PointsValue, 1.0,
                "The .sub inside the first (first-of-type) .box should match the ancestor rule");
            Assert.AreEqual(100.0, secondSub.TotalBounds.Width.PointsValue, 1.0,
                "The .sub inside the second .box should use the default width - its ancestor is not first-of-type");
        }
    }
}
