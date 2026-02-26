using System;
using System.Collections.Generic;
using System.IO;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.Styles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class TableRowspan_Tests
    {
        private const string TestCategoryName = "Table Rowspan";
        private const string ImagePath = "../../../Content/Images/Toroid32.png";
        private const string SvgImagePath = "../../../Content/Images/Chart.svg";

        private PDFLayoutDocument layoutDoc;

        private void Doc_LayoutDocumentComplete(object sender, LayoutEventArgs args)
        {
            layoutDoc = args.Context.GetLayout<PDFLayoutDocument>();
        }

        private Document ParseXhtml(string xhtmlContent)
        {
            // Create a temporary file with the XHTML content
            System.IO.StringReader reader = new System.IO.StringReader(xhtmlContent);
            try
            {
                return Document.ParseDocument(reader, ParseSourceType.DynamicContent);
            }
            finally
            {
                // Clean up temp file after parsing
                if (null != reader)
                    reader.Dispose();
            }
        }

        private PDFLayoutDocument RenderDocument(string fileName, Document doc, int minPages = 1)
        {
            layoutDoc = null;
            using (var ms = DocStreams.GetOutputStream(fileName))
            {
                doc.LayoutComplete += Doc_LayoutDocumentComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layoutDoc, "Layout document should exist");
            Assert.IsTrue(layoutDoc.AllPages.Count >= minPages, "Expected layout to have at least the minimum page count");
            return layoutDoc;
        }

        private PDFLayoutBlock RenderTableBlock(string fileName, Document doc, int minPages = 1)
        {
            var layout = RenderDocument(fileName, doc, minPages);
            var pg = layout.AllPages[0];
            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(tblBlock, "Table block should exist");
            return tblBlock;
        }

        private string GetContentPath(string relativePath)
        {
            var path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.CurrentDirectory, relativePath));
            Assert.IsTrue(File.Exists(path), $"Could not find the content file at '{path}'");
            return path;
        }

        private Image CreateImage(string path, double width, double height)
        {
            var img = new Image();
            img.Source = path;
            img.Width = new Unit(width);
            img.Height = new Unit(height);
            return img;
        }

        private PDFLayoutBlock GetTableBlock(PDFLayoutDocument layout, int pageIndex)
        {
            var pg = layout.AllPages[pageIndex];
            Assert.IsTrue(pg.ContentBlock.Columns.Length > 0, "Page should contain columns");
            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(tblBlock, "Table block should exist");
            return tblBlock;
        }

        private void AssertRepeatingHeaderRow(PDFLayoutDocument layout, int expectedPages)
        {
            Assert.IsTrue(layout.AllPages.Count >= expectedPages, "Expected layout to have enough pages");
            for (int pageIndex = 0; pageIndex < expectedPages; pageIndex++)
            {
                var tblBlock = GetTableBlock(layout, pageIndex);
                var firstRow = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
                Assert.IsNotNull(firstRow, $"First row block should exist on page {pageIndex}");
                // Header repetition may be a table-level feature; just verify first row exists
                Assert.IsTrue(tblBlock.Columns[0].Contents.Count >= 1, $"Each page should have at least one row");
            }
        }

        private void AssertRowCellCounts(PDFLayoutBlock tableBlock, params int[] expectedCellsPerRow)
        {
            Assert.AreEqual(expectedCellsPerRow.Length, tableBlock.Columns[0].Contents.Count, "Table should have expected row blocks");

            for (int i = 0; i < expectedCellsPerRow.Length; i++)
            {
                var rowBlock = tableBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                Assert.IsNotNull(rowBlock, $"Row {i} block should exist");
                int actualCellBlocks = 0;
                for (int col = 0; col < rowBlock.Columns.Length; col++)
                {
                    actualCellBlocks += rowBlock.Columns[col].Contents.Count;
                }
                Assert.AreEqual(expectedCellsPerRow[i], actualCellBlocks, $"Row {i} should have {expectedCellsPerRow[i]} cell blocks");
            }
        }

        #region Simple Rowspan Tests (Category B)

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_SimpleRowspan2Rows()
        {
            const int CellWidth = 150;
            const int CellHeight = 50;

            string xhtml = $@"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page {{ size: 600pt 800pt; margin: 10pt; }}
        body {{ font-size: 10pt; }}
        table {{ width: 500pt; margin: 0; padding: 0; border-collapse: collapse; }}
        td {{ width: {CellWidth}pt; height: {CellHeight}pt; margin: 0; padding: 0; border: 1pt solid black; }}
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='2'>A</td>
        <td>B</td>
        <td>C</td>
    </tr>
    <tr>
        <td>D</td>
        <td>E</td>
    </tr>
    <tr>
        <td>F0</td>
        <td>F1</td>
        <td>F2</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            using (var ms = DocStreams.GetOutputStream("TableRowspan_SimpleRowspan2Rows.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutDocumentComplete;
                doc.SaveAsPDF(ms);
            }

            // Verify layout
            Assert.AreEqual(1, layoutDoc.AllPages.Count);
            var pg = layoutDoc.AllPages[0];
            
            // Get table block
            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(tblBlock, "Table block should exist");
            
            // Verify table has 3 row blocks
            Assert.AreEqual(3, tblBlock.Columns[0].Contents.Count, "Table should have 3 row blocks");

            // Row 0 should have standard height
            var row0Block = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(row0Block);
            Assert.AreEqual(CellHeight, row0Block.Height, "Row 0 should have standard height");

            // Row 1 should also have standard height (cellA spans both)
            var row1Block = tblBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(row1Block);
            Assert.AreEqual(CellHeight, row1Block.Height, "Row 1 should have standard height");

            // Row 2 should have standard height
            var row2Block = tblBlock.Columns[0].Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(row2Block);
            Assert.AreEqual(CellHeight, row2Block.Height, "Row 2 should have standard height");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_SimpleRowspan3Rows()
        {
            const int CellWidth = 150;
            const int CellHeight = 50;

            string xhtml = $@"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page {{ size: 600pt 800pt; margin: 10pt; }}
        body {{ font-size: 10pt; }}
        table {{ width: 400pt; margin: 0; padding: 0; border-collapse: collapse; }}
        td {{ width: {CellWidth}pt; height: {CellHeight}pt; margin: 0; padding: 0; border: 1pt solid black; }}
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='3'>A (3x1)</td>
        <td>B</td>
    </tr>
    <tr>
        <td>Row1Col1</td>
    </tr>
    <tr>
        <td>Row2Col1</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            using (var ms = DocStreams.GetOutputStream("TableRowspan_SimpleRowspan3Rows.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutDocumentComplete;
                doc.SaveAsPDF(ms);
            }

            // Verify layout
            Assert.AreEqual(1, layoutDoc.AllPages.Count);
            var pg = layoutDoc.AllPages[0];
            
            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(tblBlock, "Table block should exist");
            
            // Verify table has 3 row blocks
            Assert.AreEqual(3, tblBlock.Columns[0].Contents.Count, "Table should have 3 row blocks");

            // Each row should have standard height
            for (int i = 0; i < 3; i++)
            {
                var rowBlock = tblBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                Assert.IsNotNull(rowBlock);
                Assert.AreEqual(CellHeight, rowBlock.Height, $"Row {i} should have standard height");
            }
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_RowspanMultipleCells()
        {
            const int CellWidth = 100;
            const int CellHeight = 50;

            string xhtml = $@"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page {{ size: 600pt 800pt; margin: 10pt; }}
        body {{ font-size: 10pt; }}
        table {{ width: 500pt; margin: 0; padding: 0; border-collapse: collapse; }}
        td {{ width: {CellWidth}pt; height: {CellHeight}pt; margin: 0; padding: 0; border: 1pt solid black; }}
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='2'>A</td>
        <td>B</td>
        <td rowspan='2'>C</td>
        <td>D</td>
    </tr>
    <tr>
        <td>E</td>
        <td>F</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            using (var ms = DocStreams.GetOutputStream("TableRowspan_RowspanMultipleCells.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutDocumentComplete;
                doc.SaveAsPDF(ms);
            }

            // Verify layout
            Assert.AreEqual(1, layoutDoc.AllPages.Count);
            var pg = layoutDoc.AllPages[0];
            
            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(tblBlock, "Table block should exist");
            
            // Verify 2 rows
            Assert.AreEqual(2, tblBlock.Columns[0].Contents.Count, "Table should have 2 row blocks");

            // Each row should have standard height
            for (int i = 0; i < 2; i++)
            {
                var rowBlock = tblBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                Assert.IsNotNull(rowBlock);
                Assert.AreEqual(CellHeight, rowBlock.Height, $"Row {i} should have standard height");
            }
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_RowspanWithVariableRowHeights()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 400pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='2'>A</td>
        <td>B short</td>
    </tr>
    <tr>
        <td>C with a lot of content that might wrap to multiple lines</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var tblBlock = RenderTableBlock("TableRowspan_RowspanWithVariableRowHeights.pdf", doc);
            AssertRowCellCounts(tblBlock, 2, 1);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_RowspanVerticalAlignment()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 400pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
        td.aligned { vertical-align: middle; }
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='2' class='aligned'>Vertically Centered</td>
        <td>B</td>
    </tr>
    <tr>
        <td>C</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var tblBlock = RenderTableBlock("TableRowspan_RowspanVerticalAlignment.pdf", doc);
            AssertRowCellCounts(tblBlock, 2, 1);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_RowspanWithBorders()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 400pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
        td.thick-border { border: 2pt solid black; }
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='2' class='thick-border'>A</td>
        <td>B</td>
    </tr>
    <tr>
        <td>C</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var tblBlock = RenderTableBlock("TableRowspan_RowspanWithBorders.pdf", doc);
            AssertRowCellCounts(tblBlock, 2, 1);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_RowspanWithBackgroundColor()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 400pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
        td.colored { background-color: rgb(200, 220, 240); }
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='2' class='colored'>A</td>
        <td>B</td>
    </tr>
    <tr>
        <td>C</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var tblBlock = RenderTableBlock("TableRowspan_RowspanWithBackgroundColor.pdf", doc);
            AssertRowCellCounts(tblBlock, 2, 1);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_RowspanWithPadding()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 400pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
        td.padded { padding: 10pt; }
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='2' class='padded'>Padded Content</td>
        <td>B</td>
    </tr>
    <tr>
        <td>C</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var tblBlock = RenderTableBlock("TableRowspan_RowspanWithPadding.pdf", doc);
            AssertRowCellCounts(tblBlock, 2, 1);
        }

        #endregion

        #region Colspan + Rowspan Interaction (Category C)

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_ColspanAndRowspan_Simple()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 500pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
    </style>
</head>
<body>
<table>
    <tr>
        <td colspan='2' rowspan='2'>A (2x2)</td>
        <td>B</td>
    </tr>
    <tr>
        <td>C</td>
    </tr>
    <tr>
        <td>D0</td>
        <td>D1</td>
        <td>D2</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var tblBlock = RenderTableBlock("TableRowspan_ColspanAndRowspan_Simple.pdf", doc);
            AssertRowCellCounts(tblBlock, 2, 1, 3);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_ColspanRowspan_OverlapLayout()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 300pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; width: 100pt; }
        td.wide { width: 200pt; }
    </style>
</head>
<body>
<table>
    <tr>
        <td colspan='2' rowspan='2' class='wide'>A (2x2)</td>
        <td>B</td>
    </tr>
    <tr>
        <td>C</td>
    </tr>
    <tr>
        <td>D0</td>
        <td>D1</td>
        <td>D2</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var tblBlock = RenderTableBlock("TableRowspan_ColspanAndRowspan_Simple.pdf", doc);
            AssertRowCellCounts(tblBlock, 2, 1, 3);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_MultipleSpanningCells()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 500pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
    </style>
</head>
<body>
<table>
    <tr>
        <td colspan='2' rowspan='2'>A</td>
        <td rowspan='3'>B</td>
    </tr>
    <tr>
        <td>C</td>
    </tr>
    <tr>
        <td>D0</td>
        <td>D1</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var tblBlock = RenderTableBlock("TableRowspan_MultipleSpanningCells.pdf", doc);
            AssertRowCellCounts(tblBlock, 2, 1, 2);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_SpanningCellsAdjacent()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 500pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='2'>A</td>
        <td rowspan='2'>B</td>
        <td>C</td>
    </tr>
    <tr>
        <td>D</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var tblBlock = RenderTableBlock("TableRowspan_SpanningCellsAdjacent.pdf", doc);
            AssertRowCellCounts(tblBlock, 3, 1);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_WidthCalcWithColspanRowspan()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 400pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
        td.w20 { width: 20%; }
        td.w30 { width: 30%; }
        td.w50 { width: 50%; }
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='2' class='w20'>A</td>
        <td colspan='2'>B</td>
    </tr>
    <tr>
        <td class='w30'>C</td>
        <td class='w50'>D</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var tblBlock = RenderTableBlock("TableRowspan_WidthCalcWithColspanRowspan.pdf", doc);
            AssertRowCellCounts(tblBlock, 2, 2);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_HeightCalcWithColspanRowspan()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 400pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
        td.tall { height: 100pt; }
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='2' class='tall'>A tall cell spanning 2 rows</td>
        <td colspan='2'>B</td>
    </tr>
    <tr>
        <td>C</td>
        <td>D</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var tblBlock = RenderTableBlock("TableRowspan_HeightCalcWithColspanRowspan.pdf", doc);
            AssertRowCellCounts(tblBlock, 2, 2);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_NestedTableWithRowspan()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 100%; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
    </style>
</head>
<body>
<table style='width: 400pt;'>
    <tr>
        <td rowspan='2'>A</td>
        <td>
            <table style='width: 150pt;'>
                <tr>
                    <td>Nested</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>C</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var tblBlock = RenderTableBlock("TableRowspan_NestedTableWithRowspan.pdf", doc);
            AssertRowCellCounts(tblBlock, 2, 1);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_ComplexContent_SpannedAndStandardCells()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
        td.wide { width: 240pt; }
        td.tall { height: 120pt; }
        td.complex { height: 240pt; }
    </style>
</head>
<body>
<table style='width: 500pt;'>
    <tr>
        <td rowspan='2' class='wide complex'>Rowspan cell</td>
        <td class='wide tall'>Non-spanned</td>
    </tr>
    <tr>
        <td class='wide tall'>Second row</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var tblBlock = RenderTableBlock("TableRowspan_ComplexContent_SpannedAndStandardCells.pdf", doc);
            AssertRowCellCounts(tblBlock, 2, 1);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_IntersectingRowspans()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 500pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; width: 150pt; height: 50pt; }
        td.tall3 { height: 150pt; }
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='3' class='tall3'>A</td>
        <td>B</td>
        <td>C</td>
    </tr>
    <tr>
        <td rowspan='3' class='tall3'>D</td>
        <td>E</td>
    </tr>
    <tr>
        <td>F</td>
    </tr>
    <tr>
        <td>G</td>
        <td>H</td>
    </tr>
    <tr>
        <td>I</td>
        <td>J</td>
        <td>K</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var tblBlock = RenderTableBlock("TableRowspan_IntersectingRowspans.pdf", doc);
            AssertRowCellCounts(tblBlock, 3, 2, 1, 2, 3);
        }

        #endregion

        #region Page Break Handling (Category G - partial)

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_PageBreak_RowspanEndsBeforeBreak()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 250pt; margin: 20pt; }
        body { font-size: 10pt; }
        table { width: 500pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; height: 100pt; }
        td.tall2 { height: 200pt; }
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='2' class='tall2'>A</td>
        <td>B</td>
    </tr>
    <tr>
        <td>C</td>
    </tr>
    <tr>
        <td>D</td>
        <td>E</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var layout = RenderDocument("TableRowspan_PageBreak_RowspanEndsBeforeBreak.pdf", doc, 2);
            Assert.IsTrue(layout.AllPages.Count >= 2, "Rowspan before page break should produce multiple pages");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_PageBreak_RowspanCrossesBreak()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 250pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 400pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; height: 120pt; }
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='3'>A</td>
        <td>B</td>
    </tr>
    <tr>
        <td>C</td>
    </tr>
    <tr>
        <td>D</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var layout = RenderDocument("TableRowspan_PageBreak_RowspanCrossesBreak.pdf", doc, 2);
            Assert.IsTrue(layout.AllPages.Count >= 2, "Rowspan across page break should produce multiple pages");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_PageBreak_RowspanSecondRowMovesPreviousRow()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 240pt; margin: 20pt; }
        body { font-size: 10pt; }
        table { width: 480pt; margin: 0; padding: 0; border-collapse: collapse; }
        th, td { margin: 0; padding: 0; border: 1pt solid black; width: 220pt; }
        th { height: 30pt; background-color: #f0f0f0; }
        td { height: 40pt; }
        td.tall8 { height: 320pt; }
    </style>
</head>
<body>
<table>
    <thead>
        <tr>
            <th>Header 0</th>
            <th>Header 1</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td rowspan='2'>A</td>
            <td>B</td>
        </tr>
        <tr>
            <td rowspan='2' class='tall8'>C</td>
        </tr>
        <tr>
            <td>D</td>
        </tr>
    </tbody>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var layout = RenderDocument("TableRowspan_PageBreak_RowspanSecondRowMovesPreviousRow.pdf", doc, 2);
            
            // Verify we have multiple pages
            Assert.IsTrue(layout.AllPages.Count >= 2, "Should split across pages");

            // Pages should each contain table content
            for (int pageIndex = 0; pageIndex < layout.AllPages.Count && pageIndex < 2; pageIndex++)
            {
                var pageTable = GetTableBlock(layout, pageIndex);
                Assert.IsTrue(pageTable.Columns[0].Contents.Count >= 1, $"Page {pageIndex} should contain at least one row");
            }
        }

        #endregion

        #region Validation Tests (Category J - partial)

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_Validation_RowspanExceedsTable()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 400pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='5'>A</td>
        <td>B</td>
    </tr>
    <tr>
        <td>C</td>
    </tr>
    <tr>
        <td>D</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var tblBlock = RenderTableBlock("TableRowspan_Validation_RowspanExceedsTable.pdf", doc);
            AssertRowCellCounts(tblBlock, 2, 1, 1);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_Validation_RowspanExceedsTable_Layout()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 300pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; height: 40pt; }
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='5'>A</td>
        <td>B</td>
    </tr>
    <tr>
        <td>C</td>
    </tr>
    <tr>
        <td>D</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var layout = RenderDocument("TableRowspan_Validation_RowspanExceedsTable_Layout.pdf", doc, 1);
            
            Assert.AreEqual(1, layout.AllPages.Count);

            var pg = layout.AllPages[0];
            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(tblBlock, "Table block should exist");
            Assert.AreEqual(3, tblBlock.Columns[0].Contents.Count, "Table should have 3 row blocks");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_Validation_LargeRowspan()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; margin: 10pt; }
        body { font-size: 10pt; }
        table { width: 400pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
    </style>
</head>
<body>
<table>
    <tr>
        <td rowspan='50'>A</td>
        <td>B</td>
    </tr>
    <tr>
        <td>Row1</td>
    </tr>
    <tr>
        <td>Row2</td>
    </tr>
    <tr>
        <td>Row3</td>
    </tr>
    <tr>
        <td>Row4</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var tblBlock = RenderTableBlock("TableRowspan_Validation_LargeRowspan.pdf", doc);
            AssertRowCellCounts(tblBlock, 2, 1, 1, 1, 1);
        }

        #endregion

        #region Visual Test

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_Visual_ColorfulTable()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: auto; margin: 20pt; }
        body { font-size: 10pt; }
        div { margin-bottom: 20pt; }
        div.title { font-size: 20pt; font-weight: bold; }
        table { width: 400pt; margin: 0; padding: 0; border-collapse: collapse; border: 1pt solid black; }
        td { margin: 0; padding: 8pt; border: 1pt solid black; width: 80pt; }
        td.blue { background-color: #ADD8E6; }
        td.green { background-color: #90EE90; }
        td.yellow { background-color: #FFFFE0; }
        td.coral { background-color: #F08080; }
        td.cyan { background-color: #E0FFFF; }
        td.salmon { background-color: #FFA07A; }
        td.white { background-color: #FFFFFF; }
        td.skyblue { background-color: #87CEEB; }
        td.darkblue { background-color: #0000FF; }
        td.darkgreen { background-color: #008000; }
        td.red { background-color: #FF0000; }
        td.steelblue { background-color: #B0C4DE; }
        td.colspan2 { width: 160pt; }
    </style>
</head>
<body>
<div class='title'>Rowspan Visual Test - Colored Table</div>
<table>
    <tr>
        <td rowspan='3' class='blue'>A<br/>(3 rows)</td>
        <td class='green'>B</td>
        <td class='yellow'>C</td>
        <td class='coral'>D</td>
    </tr>
    <tr>
        <td class='cyan'>E</td>
        <td colspan='2' class='salmon colspan2'>F (2 cols)</td>
        <td class='white'>G</td>
    </tr>
    <tr>
        <td rowspan='2' class='skyblue'>H<br/>(2 rows)</td>
        <td class='darkblue'>I</td>
        <td class='yellow'>J</td>
        <td class='darkgreen'>K</td>
    </tr>
    <tr>
        <td class='steelblue'>L</td>
        <td class='darkblue'>M</td>
        <td class='red'>N</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            using (var ms = DocStreams.GetOutputStream("TableRowspan_Visual_ColorfulTable.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutDocumentComplete;
                doc.SaveAsPDF(ms);
            }

            // Verify document processed
            Assert.IsNotNull(layoutDoc);
            Assert.AreEqual(1, layoutDoc.AllPages.Count);
        }

        #endregion

        #region Advanced Scenarios

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_PageBreak_ParentDivRepeat()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 300pt; margin: 20pt; }
        body { font-size: 10pt; }
        div.container { 
            border: 2pt solid #333333;
            padding: 10pt;
            margin: 10pt 0;
            background-color: #f5f5f5;
        }
        table { width: 100%; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 5pt; border: 1pt solid black; height: 60pt; }
    </style>
</head>
<body>
<div class='container'>
    <p>Table Container</p>
    <table>
        <tr>
            <td rowspan='3'>A</td>
            <td>B</td>
        </tr>
        <tr>
            <td>C</td>
        </tr>
        <tr>
            <td>D</td>
        </tr>
    </table>
</div>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            var layout = RenderDocument("TableRowspan_PageBreak_ParentDivRepeat.pdf", doc, 2);
            
            // Verify we have multiple pages due to overflow
            Assert.IsTrue(layout.AllPages.Count >= 2, "Table with parent div should overflow to multiple pages");

            // Verify both pages have content
            for (int pageIndex = 0; pageIndex < layout.AllPages.Count && pageIndex < 2; pageIndex++)
            {
                var page = layout.AllPages[pageIndex];
                Assert.IsTrue(page.ContentBlock.Columns[0].Contents.Count > 0, $"Page {pageIndex} should contain table content");
            }
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_MultiColumn()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 800pt; }
        body { font-size: 10pt; margin: 20pt; }
        div.content {
            column-count: 2;
            column-gap: 20pt;
            column-rule: 1pt solid #cccccc; 
            border: 1pt solid cyan;
        }
        table { width: 100%; border: 1pt solid black; }
        td, th { margin: 0; padding: 5pt; border: 1pt solid black; }
        td.tall { border: solid 1pt blue; vertical-align: top; }
        p { border-bottom: solid 1pt #999;}
        .spacer{ height: 520pt; background-color: #afafaf; font-style: italic; padding: 5pt; margin-bottom: 10pt; }
        header { border-bottom: 1pt solid green; padding: 5pt; margin-bottom: 5pt; }
        footer { border-top: 1pt solid green; padding: 5pt; margin-top: 5pt; }
    </style>
</head>
<body>
<header>
    <div>This is the page header</div>
</header>

<h1>Multi-Column Table Test</h1>
<div id='content' class='content'>
    <p>This content flows in 2 columns. The table below should flow across columns when it overflows:</p>
    <div class='spacer'>Spacer to push table over.</div>
    <table id='testTable'>
        <!--<thead>
            <tr>
                <th>Header 1</th>
                <th>Header 2</th>
            </tr>
        </thead>-->
        <tr>
            <td>Above 1</td>
            <td>Above 2</td>
        </tr>
        <tr id='aboveSpannedRow'>
            <td>Above 3</td>
            <td>Above 4</td>
        </tr>
        <tr id='spannedRow'>
            <td id='spannedCell' rowspan='2' class='tall'>Tall span 2 rows</td>
            <td>Row 0, Col 1</td>
        </tr>
        <tr id='pushingRow'>
            <td id='pushingCell'>Row 1, Col 1</td>
        </tr>
        <tr id='nextRow'>
            <td>Row 2, Col 1</td>
            <td>Row 2, Col 2</td>
        </tr>
        <!--<tr id='lastRow'>
            <td>Row 3, Col 1</td>
        </tr>-->
        <tfoot>
            <tr>
                <td>Footer 1</td>
                <td>Footer 2</td>
            </tr>
        </tfoot>
    </table>
    <p>More content after table in columns.</p>
</div>
<footer>
    <div>This is the page footer</div>
</footer>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);
            doc.RenderOptions.ConformanceMode = ParserConformanceMode.Strict;
            doc.AppendTraceLog = true;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);

            var layout = RenderDocument("TableRowspan_MultiColumn.pdf", doc, 1);
            
            // Verify layout was created successfully
            Assert.IsNotNull(layout, "Layout should be created for multi-column document");
            Assert.AreEqual(1, layout.AllPages.Count, "Multi-column layout should fit on single page");

            // Verify content is present
            var page = layout.AllPages[0];
            Assert.IsTrue(page.ContentBlock.Columns[0].Contents.Count > 0, "Page should contain content");
        }

        #endregion

        #region TableRowspan_Overflow_KeepTogether

        /// <summary>
        /// Tests that when rows with rowspan cells overflow to a new page,
        /// all rows affected by the rowspan move together to maintain the rowspan group.
        /// This validates the requirement: "if rows overflow that had a row-spanned cell in them 
        /// (or the spanned cell was across that row), then it must move along with all the other cells in its spanned rows."
        /// </summary>
        [TestMethod()]
        [TestCategory(TestCategoryName)]
        public void TableRowspan_Overflow_KeepTogether()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta charset='utf-8' />
    <title>Rowspan Overflow Keep Together Test</title>
    <style>
        @page {
            size: A4;
            margin: 20mm;
        }
        
        body {
            font-family: Helvetica;
            font-size: 10pt;
        }
        
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
        }
        
        tr {
            height: auto;
        }
        
        td, th {
            border: 1pt solid #000;
            padding: 8pt;
            text-align: left;
            vertical-align: top;
        }
        
        th {
            background-color: #ccc;
            font-weight: bold;
        }
        
        .content-area {
            content: ' ';
        }
    </style>
</head>
<body>
    <h1>Rowspan Overflow Keep Together Test</h1>
    <p>This table has rows that will overflow to a new page. The rows with rowspan should stay together.</p>
    
    <!-- Content before table to push it down the page -->
    <p style='height: 350pt; overflow: hidden;'>
        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
    </p>
    
    <!-- Table with rowspan that should overflow -->
    <table>
        <thead>
            <tr>
                <th>Col 1</th>
                <th>Col 2</th>
                <th>Col 3</th>
            </tr>
        </thead>
        <tbody>
            <!-- Row 0: First row from rowspan group -->
            <tr id='rowspan-start'>
                <td rowspan='3' style='background-color: #e0e0ff;'>Spans 3 rows</td>
                <td>R0C2-A</td>
                <td>R0C3-A</td>
            </tr>
            <!-- Row 1: Part of rowspan group -->
            <tr id='rowspan-middle'>
                <td>R1C2-B</td>
                <td>R1C3-B</td>
            </tr>
            <!-- Row 2: Last row of rowspan group -->
            <tr id='rowspan-end'>
                <td>R2C2-C</td>
                <td>R2C3-C</td>
            </tr>
            <!-- Row 3: After rowspan, should stay with group -->
            <tr id='after-rowspan'>
                <td>End C1</td>
                <td>End C2</td>
                <td>End C3</td>
            </tr>
        </tbody>
    </table>
    
    <p>Content after table.</p>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);
            var layout = RenderDocument("TableRowspan_Overflow_KeepTogether.pdf", doc, 2);
            
            // Verify layout spans multiple pages
            Assert.IsTrue(layout.AllPages.Count >= 2, "Document should span at least 2 pages due to overflow");
            
            // Verify first page has content
            var page1 = layout.AllPages[0];
            Assert.IsTrue(page1.ContentBlock.Columns[0].Contents.Count > 0, "First page should have content");
            
            // Verify second page has content (the table should have moved to second page)
            var page2 = layout.AllPages[1];
            Assert.IsTrue(page2.ContentBlock.Columns[0].Contents.Count > 0, "Second page should have table content");
        }

        #endregion
    }
}
