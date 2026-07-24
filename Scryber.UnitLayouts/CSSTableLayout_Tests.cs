using System;
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
    public class CSSTableLayout_Tests
    {
        private const string TestCategory = "Layout-CSSTable";

        private const double PageW = 600;
        private const double PageH = 800;

        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        // -----------------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------------

        private static Document CreateDoc(out Page pg)
        {
            var doc = new Document();
            pg = new Page();
            pg.Style.PageStyle.Width  = PageW;
            pg.Style.PageStyle.Height = PageH;
            pg.Style.Padding.All = 0;
            doc.Pages.Add(pg);
            return doc;
        }

        private static Panel CreateCSSTable(Page pg, double width = PageW)
        {
            var panel = new Panel();
            panel.Style.Position.DisplayMode = DisplayMode.Table;
            panel.Width = width;
            // Outer border so the table boundary is visible in the PDF.
            panel.Style.Border.LineStyle = LineType.Solid;
            panel.Style.Border.Width     = 2;
            panel.Style.Border.Color     = new Color(0, 0, 0);
            pg.Contents.Add(panel);
            return panel;
        }

        private static Panel AddRow(Panel table)
        {
            var row = new Panel();
            row.Style.Position.DisplayMode = DisplayMode.TableRow;
            table.Contents.Add(row);
            return row;
        }

        /// <summary>Adds a display:table-cell Panel with optional explicit height and label text.</summary>
        private static Panel AddCell(Panel row, double? height = null, string label = null)
        {
            var cell = new Panel();
            cell.Style.Position.DisplayMode = DisplayMode.TableCell;
            cell.Style.Padding.All = 4;
            if (height.HasValue)
                cell.Height = height.Value;
            if (!string.IsNullOrEmpty(label))
                cell.Contents.Add(new Label { Text = label });
            row.Contents.Add(cell);
            return cell;
        }

        private static PDFLayoutBlock GetTableBlock(PDFLayoutRegion pageRegion)
        {
            // The display:table panel becomes the first item in the page region.
            return pageRegion.Contents[0] as PDFLayoutBlock;
        }

        /// <summary>
        /// Recursively collects all text characters from a layout region,
        /// searching into nested blocks and lines.
        /// </summary>
        private static string CollectText(PDFLayoutRegion region)
        {
            var sb = new StringBuilder();
            AppendText(region, sb);
            return sb.ToString();
        }

        private static void AppendText(PDFLayoutRegion region, StringBuilder sb)
        {
            foreach (var item in region.Contents)
            {
                if (item is PDFLayoutLine line)
                {
                    foreach (var run in line.Runs)
                        if (run is PDFTextRunCharacter tc)
                            sb.Append(tc.Characters);
                }
                else if (item is PDFLayoutBlock block)
                {
                    foreach (var col in block.Columns)
                        AppendText(col, sb);
                }
            }
        }

        // -----------------------------------------------------------------------
        // Basic two-column table from CSS display:table
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void CSSTable_TwoColumns_SideBySide()
        {
            var doc   = CreateDoc(out var pg);
            var table = CreateCSSTable(pg);
            var row   = AddRow(table);
            AddCell(row, height: 50, label: "Cell A");
            AddCell(row, height: 50, label: "Cell B");

            using (var ms = DocStreams.GetOutputStream("CSSTable_TwoColumns.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var tableBlock = GetTableBlock(pageRegion);
            Assert.IsNotNull(tableBlock, "Table block should exist");

            // The row block sits inside the table block and should have 2 columns.
            var tableRegion = tableBlock.Columns[0];
            Assert.IsTrue(tableRegion.Contents.Count >= 1, "Table region should have content");

            var rowBlock = tableRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(rowBlock, "Row block should exist");
            Assert.AreEqual(2, rowBlock.Columns.Length, "Two cells should produce 2 columns in the row block");

            // Each column should be half the table width (TableCell base has 2pt padding + 1pt border on each side).
            double expected = PageW / 2.0;
            Assert.AreEqual(expected, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column 0 should be half the table width");
            Assert.AreEqual(expected, rowBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Column 1 should be half the table width");

            // Text content present in each cell.
            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "Cell A", "Cell 0 text");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "Cell B", "Cell 1 text");
        }

        // -----------------------------------------------------------------------
        // Three columns
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void CSSTable_ThreeColumns_EqualWidth()
        {
            var doc   = CreateDoc(out var pg);
            var table = CreateCSSTable(pg);
            var row   = AddRow(table);
            AddCell(row, height: 50, label: "Col 1");
            AddCell(row, height: 50, label: "Col 2");
            AddCell(row, height: 50, label: "Col 3");

            using (var ms = DocStreams.GetOutputStream("CSSTable_ThreeColumns.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var tableBlock = GetTableBlock(pageRegion);
            Assert.IsNotNull(tableBlock);

            var rowBlock = tableBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(rowBlock);
            Assert.AreEqual(3, rowBlock.Columns.Length, "Three cells should produce 3 columns");

            double expected = PageW / 3.0;
            string[] labels = { "Col 1", "Col 2", "Col 3" };
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(expected, rowBlock.Columns[i].TotalBounds.Width.PointsValue, 1.0,
                    $"Column {i} should be one-third of table width");
                StringAssert.Contains(CollectText(rowBlock.Columns[i]), labels[i],
                    $"Column {i} should contain its label");
            }
        }

        // -----------------------------------------------------------------------
        // Two rows
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void CSSTable_TwoRows_Stacked()
        {
            var doc   = CreateDoc(out var pg);
            var table = CreateCSSTable(pg);

            var row0 = AddRow(table);
            AddCell(row0, height: 40, label: "R0 C0");
            AddCell(row0, height: 40, label: "R0 C1");

            var row1 = AddRow(table);
            AddCell(row1, height: 60, label: "R1 C0");
            AddCell(row1, height: 60, label: "R1 C1");

            using (var ms = DocStreams.GetOutputStream("CSSTable_TwoRows.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var tableBlock = GetTableBlock(pageRegion);
            Assert.IsNotNull(tableBlock);

            var tableRegion = tableBlock.Columns[0];
            Assert.AreEqual(2, tableRegion.Contents.Count, "Table should have 2 row blocks");

            var rowBlock0 = tableRegion.Contents[0] as PDFLayoutBlock;
            var rowBlock1 = tableRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(rowBlock0, "Row 0 block should exist");
            Assert.IsNotNull(rowBlock1, "Row 1 block should exist");

            // Row 1 should be positioned below row 0.
            Assert.IsTrue(rowBlock1.TotalBounds.Y > rowBlock0.TotalBounds.Y,
                "Row 1 Y should be greater than row 0 Y");

            // Row 0 and row 1 each have content so they should both have a positive height.
            Assert.IsTrue(rowBlock0.TotalBounds.Height.PointsValue > 0, "Row 0 should have positive height");
            Assert.IsTrue(rowBlock1.TotalBounds.Height.PointsValue > 0, "Row 1 should have positive height");

            // Text present in each row.
            var row0Text = CollectText(rowBlock0.Columns[0]) + CollectText(rowBlock0.Columns[1]);
            var row1Text = CollectText(rowBlock1.Columns[0]) + CollectText(rowBlock1.Columns[1]);
            StringAssert.Contains(row0Text, "R0 C0", "Row 0 cell 0 text");
            StringAssert.Contains(row0Text, "R0 C1", "Row 0 cell 1 text");
            StringAssert.Contains(row1Text, "R1 C0", "Row 1 cell 0 text");
            StringAssert.Contains(row1Text, "R1 C1", "Row 1 cell 1 text");
        }

        // -----------------------------------------------------------------------
        // Column span (colspan)
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void CSSTable_ColSpan_ReducesColumnCount()
        {
            var doc   = CreateDoc(out var pg);
            var table = CreateCSSTable(pg);
            var row   = AddRow(table);

            // First cell spans 2 columns, second is a normal cell.
            var cell0 = AddCell(row, height: 50, label: "Spanning");
            cell0.Style.Table.CellColumnSpan = 2;
            AddCell(row, height: 50, label: "Normal");

            using (var ms = DocStreams.GetOutputStream("CSSTable_ColSpan.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var tableBlock = GetTableBlock(pageRegion);
            Assert.IsNotNull(tableBlock);

            var rowBlock = tableBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(rowBlock);
            // A 2-column span + 1 normal = 3 columns total in the grid.
            Assert.AreEqual(3, rowBlock.Columns.Length, "ColSpan=2 + 1 cell should produce 3 column slots");

            // The spanning cell occupies 2/3 of the width.
            double expected = PageW * 2.0 / 3.0;
            Assert.AreEqual(expected,
                rowBlock.Columns[0].TotalBounds.Width.PointsValue + rowBlock.Columns[1].TotalBounds.Width.PointsValue,
                2.0, "First two columns combined should equal 2/3 of total width");

            // Text in spanning cell and normal cell.
            var spanText   = CollectText(rowBlock.Columns[0]) + CollectText(rowBlock.Columns[1]);
            var normalText = CollectText(rowBlock.Columns[2]);
            StringAssert.Contains(spanText,   "Spanning", "Spanning cell text");
            StringAssert.Contains(normalText, "Normal",   "Normal cell text");
        }

        // -----------------------------------------------------------------------
        // CSS string parsing — display:table recognised
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void CSSTable_Parsed_DisplayTable()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:table; width:600pt; border:2pt solid #000;"">
    <div style=""display:table-row;"">
      <div style=""display:table-cell; height:50pt; padding:4pt;"">Alpha</div>
      <div style=""display:table-cell; height:50pt; padding:4pt;"">Beta</div>
    </div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("CSSTable_CSS_Parsed.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            Assert.AreEqual(1, layout.AllPages.Count);

            // Find the row block: first block with exactly 2 columns.
            var contentBlock = layout.AllPages[0].ContentBlock;
            PDFLayoutBlock rowBlock = FindRowWithCols(contentBlock.Columns[0], 2);
            Assert.IsNotNull(rowBlock, "Should find a row block with 2 column cells in the layout tree");
            Assert.AreEqual(2, rowBlock.Columns.Length, "CSS display:table-row should produce 2 column cells");

            // Text present in each cell.
            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "Alpha", "Left cell text");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "Beta",  "Right cell text");
        }

        // Recursively find the first layout block that has exactly `colCount` columns.
        private static PDFLayoutBlock FindRowWithCols(PDFLayoutRegion region, int colCount)
        {
            foreach (var item in region.Contents)
            {
                if (item is PDFLayoutBlock b)
                {
                    if (b.Columns.Length == colCount)
                        return b;
                    foreach (var col in b.Columns)
                    {
                        var found = FindRowWithCols(col, colCount);
                        if (found != null) return found;
                    }
                }
            }
            return null;
        }

        // -----------------------------------------------------------------------
        // Regression: existing <table> element is unaffected by LayoutEngineCSSTable
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void CSSTable_ExistingTableElement_Unaffected()
        {
            // A real HTML <table> must still be laid out by the normal table engine,
            // not by LayoutEngineCSSTable. Place both a native table and a CSS table
            // on the same page and verify the native table produces the expected result.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <table style=""width:600pt;"">
    <tr>
      <td style=""height:50pt; padding:4pt;"">Native A</td>
      <td style=""height:50pt; padding:4pt;"">Native B</td>
    </tr>
  </table>
  <div style=""display:table; width:600pt;"">
    <div style=""display:table-row;"">
      <div style=""display:table-cell; height:50pt; padding:4pt;"">CSS A</div>
      <div style=""display:table-cell; height:50pt; padding:4pt;"">CSS B</div>
    </div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("CSSTable_NativeTableUnaffected.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            Assert.AreEqual(1, layout.AllPages.Count, "Should produce exactly one page");

            // Find all row blocks that have exactly 2 columns — both the native table and the
            // CSS table should each contribute one such block.
            var pageRegion = layout.AllPages[0].ContentBlock.Columns[0];
            int twoColRowCount = CountBlocksWithCols(pageRegion, 2);
            Assert.IsTrue(twoColRowCount >= 2,
                "Should have at least 2 row blocks with 2 columns (one native, one CSS)");

            // Collect all text on the page and confirm both tables rendered their content.
            var allText = CollectText(pageRegion);
            StringAssert.Contains(allText, "Native A", "Native table cell A should be present");
            StringAssert.Contains(allText, "Native B", "Native table cell B should be present");
            StringAssert.Contains(allText, "CSS A",    "CSS table cell A should be present");
            StringAssert.Contains(allText, "CSS B",    "CSS table cell B should be present");
        }

        private static int CountBlocksWithCols(PDFLayoutRegion region, int colCount)
        {
            int count = 0;
            foreach (var item in region.Contents)
            {
                if (item is PDFLayoutBlock b)
                {
                    if (b.Columns.Length == colCount) count++;
                    foreach (var col in b.Columns)
                        count += CountBlocksWithCols(col, colCount);
                }
            }
            return count;
        }

        // -----------------------------------------------------------------------
        // Empty CSS table does not throw
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void CSSTable_Empty_DoesNotThrow()
        {
            var doc   = CreateDoc(out var pg);
            var table = new Panel();
            table.Style.Position.DisplayMode = DisplayMode.Table;
            table.Width  = PageW;
            table.Height = 50;
            table.Style.Border.LineStyle = LineType.Solid;
            table.Style.Border.Width     = 2;
            table.Style.Border.Color     = new Color(0, 0, 0);
            pg.Contents.Add(table);

            using (var ms = DocStreams.GetOutputStream("CSSTable_Empty.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should complete without throwing for empty CSS table");
            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock, "Empty CSS table should still produce a layout block");
        }

        // -----------------------------------------------------------------------
        // Anonymous table-row wrapping (table-cells directly inside display:table)
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void CSSTable_AnonymousRow_CellsDirectInTable_SideBySide()
        {
            // Three display:table-cell divs directly inside a display:table — no explicit
            // table-row wrapper. They should be auto-wrapped in an anonymous row and
            // laid out side-by-side (like a single table row).
            var doc   = CreateDoc(out var pg);
            var table = CreateCSSTable(pg, width: 600);

            var cellA = AddCell(table, height: 40, label: "A");
            var cellB = AddCell(table, height: 40, label: "B");
            var cellC = AddCell(table, height: 40, label: "C");

            using (var ms = DocStreams.GetOutputStream("CSSTable_AnonymousRow_Three.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pg0  = _layout.AllPages[0];
            var tableBlock = GetTableBlock(pg0.ContentBlock.Columns[0]);
            Assert.IsNotNull(tableBlock, "Table block should exist");
            Assert.AreEqual(1, tableBlock.Columns.Length, "Anonymous row → single table region");
            // The single region holds one row of 3 cells; all should be on the same Y baseline.
            var rowBlock = tableBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(rowBlock, "Row block should exist inside the table");
            Assert.AreEqual(3, rowBlock.Columns.Length, "Three cells should be in the anonymous row");

            double y0 = rowBlock.Columns[0].TotalBounds.Y.PointsValue;
            double y1 = rowBlock.Columns[1].TotalBounds.Y.PointsValue;
            double y2 = rowBlock.Columns[2].TotalBounds.Y.PointsValue;
            Assert.AreEqual(y0, y1, 0.5, "All three cells should start at the same Y");
            Assert.AreEqual(y0, y2, 0.5, "All three cells should start at the same Y");

            // Cells should be side-by-side (no cell starts at X=0 except the first)
            double x0 = rowBlock.Columns[0].TotalBounds.X.PointsValue;
            double x1 = rowBlock.Columns[1].TotalBounds.X.PointsValue;
            double x2 = rowBlock.Columns[2].TotalBounds.X.PointsValue;
            Assert.IsTrue(x1 > x0, "Cell B should be to the right of Cell A");
            Assert.IsTrue(x2 > x1, "Cell C should be to the right of Cell B");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void CSSTable_AnonymousRow_MixedRowsAndCells()
        {
            // A display:table with a mix of explicit table-rows and bare table-cells.
            // The bare cells should be wrapped in an anonymous row inserted between the explicit rows.
            var doc   = CreateDoc(out var pg);
            var table = CreateCSSTable(pg, width: 600);

            // Explicit row with two cells
            var row1  = AddRow(table);
            AddCell(row1, height: 30, label: "R1C1");
            AddCell(row1, height: 30, label: "R1C2");

            // Bare cells — should become an anonymous second row
            AddCell(table, height: 30, label: "AnonC1");
            AddCell(table, height: 30, label: "AnonC2");

            // Another explicit row
            var row2 = AddRow(table);
            AddCell(row2, height: 30, label: "R3C1");

            using (var ms = DocStreams.GetOutputStream("CSSTable_AnonymousRow_Mixed.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var tableBlock = GetTableBlock(_layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(tableBlock, "Table block should exist");

            // Should have 3 rows: explicit, anonymous, explicit
            int rowCount = tableBlock.Columns[0].Contents.Count;
            Assert.AreEqual(3, rowCount, "Should be 3 rows: explicit + anonymous + explicit");

            // Each row block sits below the previous
            var row1Block = tableBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var anonBlock = tableBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            var row3Block = tableBlock.Columns[0].Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(row1Block);
            Assert.IsNotNull(anonBlock);
            Assert.IsNotNull(row3Block);

            Assert.IsTrue(anonBlock.TotalBounds.Y.PointsValue > row1Block.TotalBounds.Y.PointsValue,
                "Anonymous row should be below the first explicit row");
            Assert.IsTrue(row3Block.TotalBounds.Y.PointsValue > anonBlock.TotalBounds.Y.PointsValue,
                "Third row should be below the anonymous row");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void CSSTable_AnonymousRow_CSSParsed_DirectCells()
        {
            // CSS-parsed: two display:table-cell divs directly inside a display:table div.
            // They should be auto-wrapped in an anonymous row and rendered side-by-side.
            var html = @"<html xmlns=""http://www.w3.org/1999/xhtml"">
<body>
  <div style=""display:table; width:600pt;"">
    <div style=""display:table-cell; padding:4pt;"">Alpha</div>
    <div style=""display:table-cell; padding:4pt;"">Beta</div>
  </div>
</body>
</html>";

            using var docParsed = Document.ParseDocument(new System.IO.StringReader(html),
                Scryber.ParseSourceType.DynamicContent);

            using (var ms = DocStreams.GetOutputStream("CSSTable_AnonymousRow_CSSParsed.pdf"))
            {
                docParsed.LayoutComplete += Doc_LayoutComplete;
                docParsed.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should complete");
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];

            // The display:table div is the first block in the page.
            var tableBlock = GetTableBlock(pageRegion);
            Assert.IsNotNull(tableBlock, "Table block must exist");

            // The table region holds one anonymous row.
            var tableRegion = tableBlock.Columns[0];
            Assert.AreEqual(1, tableRegion.Contents.Count, "Should be exactly one anonymous row");

            var rowBlock = tableRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(rowBlock, "Row block must exist");
            Assert.AreEqual(2, rowBlock.Columns.Length, "Two cells should be side-by-side in one row");

            // Both cells should be on the same Y baseline.
            double y0 = rowBlock.Columns[0].TotalBounds.Y.PointsValue;
            double y1 = rowBlock.Columns[1].TotalBounds.Y.PointsValue;
            Assert.AreEqual(y0, y1, 0.5, "Alpha and Beta should share the same Y (same row)");

            // Beta should be to the right of Alpha.
            double x0 = rowBlock.Columns[0].TotalBounds.X.PointsValue;
            double x1 = rowBlock.Columns[1].TotalBounds.X.PointsValue;
            Assert.IsTrue(x1 > x0, "Beta (column 1) should be to the right of Alpha (column 0)");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void CSSTable_AnonymousTable_CSSParsed_DirectCells()
        {
            // Negative test: a display:table-row div at the body level (no display:table ancestor).
            // Without a table context, Scryber renders it as a block; the two table-cell children
            // are also treated as blocks and appear stacked vertically in normal flow — NOT side-by-side.
            var html = @"<html xmlns=""http://www.w3.org/1999/xhtml"">
<body>
  <div style=""display:table-row;"">
    <div style=""display:table-cell; padding:4pt;"">Alpha</div>
    <div style=""display:table-cell; padding:4pt;"">Beta</div>
  </div>
</body>
</html>";

            using var docParsed = Document.ParseDocument(new System.IO.StringReader(html),
                Scryber.ParseSourceType.DynamicContent);

            using (var ms = DocStreams.GetOutputStream("CSSTable_AnonymousTable_CSSParsed.pdf"))
            {
                docParsed.LayoutComplete += Doc_LayoutComplete;
                docParsed.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should complete");
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];

            // The outer table-row div renders as a plain block (single column).
            var outerBlock = pageRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(outerBlock, "Outer block must exist");
            Assert.AreEqual(1, outerBlock.Columns.Length,
                "Without a display:table ancestor, table-row renders as a single-column block");

            // Its children (the two table-cell divs) must be stacked, not side-by-side.
            // They appear as separate block items in the single column.
            var innerRegion = outerBlock.Columns[0];
            Assert.IsTrue(innerRegion.Contents.Count >= 2,
                "Both child divs should produce layout items in the block column");

            var blockAlpha = innerRegion.Contents[0] as PDFLayoutBlock;
            var blockBeta  = innerRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(blockAlpha, "Alpha block must exist");
            Assert.IsNotNull(blockBeta,  "Beta block must exist");

            double yAlpha = blockAlpha.TotalBounds.Y.PointsValue;
            double yBeta  = blockBeta.TotalBounds.Y.PointsValue;
            Assert.IsTrue(yBeta > yAlpha,
                "In normal block flow, Beta should be below Alpha — not side-by-side");

            double xAlpha = blockAlpha.TotalBounds.X.PointsValue;
            double xBeta  = blockBeta.TotalBounds.X.PointsValue;
            Assert.AreEqual(xAlpha, xBeta, 1.0,
                "Both blocks share the same left edge in normal block flow");
        }
    }
}
