using System;
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

        private static Panel AddCell(Panel row, double? height = null)
        {
            var cell = new Panel();
            cell.Style.Position.DisplayMode = DisplayMode.TableCell;
            if (height.HasValue)
                cell.Height = height.Value;
            row.Contents.Add(cell);
            return cell;
        }

        private static PDFLayoutBlock FindTableBlock(PDFLayoutRegion region)
        {
            foreach (var item in region.Contents)
            {
                if (item is PDFLayoutBlock b && b.Columns.Length == 1)
                {
                    // Look for a block whose columns contain multiple side-by-side child blocks
                    // (characteristic of a table row).
                    var col = b.Columns[0];
                    if (col.Contents.Count >= 1 && col.Contents[0] is PDFLayoutBlock row
                        && row.Columns.Length > 1)
                        return b;
                }
            }
            return null;
        }

        private static PDFLayoutBlock GetTableBlock(PDFLayoutRegion pageRegion)
        {
            // The display:table panel becomes the first item in the page region.
            return pageRegion.Contents[0] as PDFLayoutBlock;
        }

        // -----------------------------------------------------------------------
        // Basic two-column table from CSS display:table
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void CSSTable_TwoColumns_SideBySide()
        {
            var doc = CreateDoc(out var pg);
            var table = CreateCSSTable(pg);
            var row   = AddRow(table);
            AddCell(row, height: 50);
            AddCell(row, height: 50);

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

            // Each column should be half the table width.
            double expected = PageW / 2.0;
            Assert.AreEqual(expected, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column 0 should be half the table width");
            Assert.AreEqual(expected, rowBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Column 1 should be half the table width");
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
            AddCell(row, height: 50);
            AddCell(row, height: 50);
            AddCell(row, height: 50);

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
            for (int i = 0; i < 3; i++)
                Assert.AreEqual(expected, rowBlock.Columns[i].TotalBounds.Width.PointsValue, 1.0,
                    $"Column {i} should be one-third of table width");
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
            AddCell(row0, height: 40);
            AddCell(row0, height: 40);

            var row1 = AddRow(table);
            AddCell(row1, height: 60);
            AddCell(row1, height: 60);

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
            var cell0 = AddCell(row, height: 50);
            cell0.Style.Table.CellColumnSpan = 2;
            AddCell(row, height: 50);

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
            Assert.AreEqual(expected, rowBlock.Columns[0].TotalBounds.Width.PointsValue +
                                       rowBlock.Columns[1].TotalBounds.Width.PointsValue,
                2.0, "First two columns combined should equal 2/3 of total width");
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
  <div style=""display:table; width:600pt;"">
    <div style=""display:table-row;"">
      <div style=""display:table-cell; height:50pt;"" />
      <div style=""display:table-cell; height:50pt;"" />
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

            // Find the row block: first block in the page that has a child block with 2 columns.
            var contentBlock = layout.AllPages[0].ContentBlock;
            PDFLayoutBlock tableBlock = FindRowWithCols(contentBlock.Columns[0], 2);
            Assert.IsNotNull(tableBlock, "Should find a row block with 2 column cells in the layout tree");
            Assert.AreEqual(2, tableBlock.Columns.Length, "CSS display:table-row should produce 2 column cells");
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
    }
}
