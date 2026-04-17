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
    }
}
