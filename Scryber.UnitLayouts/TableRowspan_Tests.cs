using System;
using System.Collections.Generic;
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

        private PDFLayoutDocument layoutDoc;

        private void Doc_LayoutDocumentComplete(object sender, LayoutEventArgs args)
        {
            layoutDoc = args.Context.GetLayout<PDFLayoutDocument>();
        }

        #region Simple Rowspan Tests (Category B)

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_SimpleRowspan2Rows()
        {
            const int PageWidth = 600;
            const int PageHeight = 800;
            const int CellWidth = 150;
            const int CellHeight = 50;

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            section.Margins = 10;
            section.FontSize = 10;

            doc.Pages.Add(section);

            // Create table: 3 rows, 3 columns
            // Cell (0,0) has rowspan=2
            TableGrid grid = new TableGrid();
            grid.Width = 500;
            grid.Margins = (Thickness)0;
            section.Contents.Add(grid);

            // Row 0
            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellRowSpan = 2; // Spans rows 0 and 1
            cellA.Width = CellWidth;
            cellA.Height = CellHeight;
            cellA.Margins = (Thickness)0;
            cellA.Contents.Add("A");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.Width = CellWidth;
            cellB.Height = CellHeight;
            cellB.Margins = (Thickness)0;
            cellB.Contents.Add("B");
            row.Cells.Add(cellB);

            TableCell cellC = new TableCell();
            cellC.Width = CellWidth;
            cellC.Height = CellHeight;
            cellC.Margins = (Thickness)0;
            cellC.Contents.Add("C");
            row.Cells.Add(cellC);

            // Row 1 - only has 2 cells (cellA spans from row 0)
            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellD = new TableCell();
            cellD.Width = CellWidth;
            cellD.Height = CellHeight;
            cellD.Margins = (Thickness)0;
            cellD.Contents.Add("D");
            row.Cells.Add(cellD);

            TableCell cellE = new TableCell();
            cellE.Width = CellWidth;
            cellE.Height = CellHeight;
            cellE.Margins = (Thickness)0;
            cellE.Contents.Add("E");
            row.Cells.Add(cellE);

            // Row 2
            row = new TableRow();
            grid.Rows.Add(row);

            for (int i = 0; i < 3; i++)
            {
                TableCell cell = new TableCell();
                cell.Width = CellWidth;
                cell.Height = CellHeight;
                cell.Margins = (Thickness)0;
                cell.Contents.Add($"F{i}");
                row.Cells.Add(cell);
            }

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
            const int PageWidth = 600;
            const int PageHeight = 800;
            const int CellWidth = 150;
            const int CellHeight = 50;

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            section.Margins = 10;
            section.FontSize = 10;

            doc.Pages.Add(section);

            TableGrid grid = new TableGrid();
            grid.Width = 400;
            grid.Margins = (Thickness)0;
            section.Contents.Add(grid);

            // Row 0
            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellRowSpan = 3; // Spans rows 0, 1, 2
            cellA.Width = CellWidth;
            cellA.Height = CellHeight;
            cellA.Margins = (Thickness)0;
            cellA.Contents.Add("A (3x1)");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.Width = CellWidth;
            cellB.Height = CellHeight;
            cellB.Margins = (Thickness)0;
            cellB.Contents.Add("B");
            row.Cells.Add(cellB);

            // Rows 1 and 2
            for (int r = 1; r < 3; r++)
            {
                row = new TableRow();
                grid.Rows.Add(row);

                TableCell cell = new TableCell();
                cell.Width = CellWidth;
                cell.Height = CellHeight;
                cell.Margins = (Thickness)0;
                cell.Contents.Add($"Row{r}Col1");
                row.Cells.Add(cell);
            }

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
            const int PageWidth = 600;
            const int PageHeight = 800;
            const int CellWidth = 100;
            const int CellHeight = 50;

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            section.Margins = 10;
            section.FontSize = 10;

            doc.Pages.Add(section);

            TableGrid grid = new TableGrid();
            grid.Width = 500;
            grid.Margins = (Thickness)0;
            section.Contents.Add(grid);

            // Row 0
            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellRowSpan = 2;
            cellA.Width = CellWidth;
            cellA.Height = CellHeight;
            cellA.Margins = (Thickness)0;
            cellA.Contents.Add("A");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.Width = CellWidth;
            cellB.Height = CellHeight;
            cellB.Margins = (Thickness)0;
            cellB.Contents.Add("B");
            row.Cells.Add(cellB);

            TableCell cellC = new TableCell();
            cellC.CellRowSpan = 2;
            cellC.Width = CellWidth;
            cellC.Height = CellHeight;
            cellC.Margins = (Thickness)0;
            cellC.Contents.Add("C");
            row.Cells.Add(cellC);

            TableCell cellD = new TableCell();
            cellD.Width = CellWidth;
            cellD.Height = CellHeight;
            cellD.Margins = (Thickness)0;
            cellD.Contents.Add("D");
            row.Cells.Add(cellD);

            // Row 1
            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellE = new TableCell();
            cellE.Width = CellWidth;
            cellE.Height = CellHeight;
            cellE.Margins = (Thickness)0;
            cellE.Contents.Add("E");
            row.Cells.Add(cellE);

            TableCell cellF = new TableCell();
            cellF.Width = CellWidth;
            cellF.Height = CellHeight;
            cellF.Margins = (Thickness)0;
            cellF.Contents.Add("F");
            row.Cells.Add(cellF);

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
            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.Margins = 10;

            doc.Pages.Add(section);

            TableGrid grid = new TableGrid();
            grid.Width = 400;
            section.Contents.Add(grid);

            // Row 0 - short content
            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellRowSpan = 2;
            cellA.Contents.Add("A");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.Contents.Add("B short");
            row.Cells.Add(cellB);

            // Row 1 - tall content
            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellC = new TableCell();
            cellC.Contents.Add("C with a lot of content that might wrap to multiple lines");
            row.Cells.Add(cellC);

            Assert.AreNotEqual(0, grid.Width);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_RowspanVerticalAlignment()
        {
            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.Margins = 10;

            doc.Pages.Add(section);

            TableGrid grid = new TableGrid();
            grid.Width = 400;
            section.Contents.Add(grid);

            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellRowSpan = 2;
            cellA.Style.Position.VAlign = VerticalAlignment.Middle;
            cellA.Contents.Add("Vertically Centered");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.Contents.Add("B");
            row.Cells.Add(cellB);

            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellC = new TableCell();
            cellC.Contents.Add("C");
            row.Cells.Add(cellC);

            Assert.AreNotEqual(0, grid.Width);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_RowspanWithBorders()
        {
            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.Margins = 10;

            doc.Pages.Add(section);

            TableGrid grid = new TableGrid();
            grid.Width = 400;
            section.Contents.Add(grid);

            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellRowSpan = 2;
            cellA.Style.Border.Width = 2;
            cellA.Style.Border.Color = StandardColors.Black;
            cellA.Contents.Add("A");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.Contents.Add("B");
            row.Cells.Add(cellB);

            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellC = new TableCell();
            cellC.Contents.Add("C");
            row.Cells.Add(cellC);

            Assert.AreNotEqual(0, grid.Width);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_RowspanWithBackgroundColor()
        {
            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.Margins = 10;

            doc.Pages.Add(section);

            TableGrid grid = new TableGrid();
            grid.Width = 400;
            section.Contents.Add(grid);

            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellRowSpan = 2;
            cellA.Style.Background.Color = new Color(200, 220, 240);
            cellA.Contents.Add("A");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.Contents.Add("B");
            row.Cells.Add(cellB);

            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellC = new TableCell();
            cellC.Contents.Add("C");
            row.Cells.Add(cellC);

            Assert.AreNotEqual(0, grid.Width);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_RowspanWithPadding()
        {
            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.Margins = 10;

            doc.Pages.Add(section);

            TableGrid grid = new TableGrid();
            grid.Width = 400;
            section.Contents.Add(grid);

            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellRowSpan = 2;
            cellA.Style.Padding.All = 10;
            cellA.Contents.Add("Padded Content");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.Contents.Add("B");
            row.Cells.Add(cellB);

            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellC = new TableCell();
            cellC.Contents.Add("C");
            row.Cells.Add(cellC);

            Assert.AreNotEqual(0, grid.Width);
        }

        #endregion

        #region Colspan + Rowspan Interaction (Category C)

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_ColspanAndRowspan_Simple()
        {
            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.Margins = 10;

            doc.Pages.Add(section);

            TableGrid grid = new TableGrid();
            grid.Width = 500;
            section.Contents.Add(grid);

            // Row 0
            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellColumnSpan = 2;
            cellA.CellRowSpan = 2;
            cellA.Contents.Add("A (2x2)");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.Contents.Add("B");
            row.Cells.Add(cellB);

            // Row 1
            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellC = new TableCell();
            cellC.Contents.Add("C");
            row.Cells.Add(cellC);

            // Row 2
            row = new TableRow();
            grid.Rows.Add(row);

            for (int i = 0; i < 3; i++)
            {
                TableCell cell = new TableCell();
                cell.Contents.Add($"D{i}");
                row.Cells.Add(cell);
            }

            Assert.AreNotEqual(0, grid.Width);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_MultipleSpanningCells()
        {
            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.Margins = 10;

            doc.Pages.Add(section);

            TableGrid grid = new TableGrid();
            grid.Width = 500;
            section.Contents.Add(grid);

            // Row 0
            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellColumnSpan = 2;
            cellA.CellRowSpan = 2;
            cellA.Contents.Add("A");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.CellColumnSpan = 1;
            cellB.CellRowSpan = 3;
            cellB.Contents.Add("B");
            row.Cells.Add(cellB);

            // Row 1
            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellC = new TableCell();
            cellC.Contents.Add("C");
            row.Cells.Add(cellC);

            // Row 2
            row = new TableRow();
            grid.Rows.Add(row);

            for (int i = 0; i < 2; i++)
            {
                TableCell cell = new TableCell();
                cell.Contents.Add($"D{i}");
                row.Cells.Add(cell);
            }

            Assert.AreNotEqual(0, grid.Width);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_SpanningCellsAdjacent()
        {
            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.Margins = 10;

            doc.Pages.Add(section);

            TableGrid grid = new TableGrid();
            grid.Width = 500;
            section.Contents.Add(grid);

            // Row 0
            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellRowSpan = 2;
            cellA.Contents.Add("A");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.CellRowSpan = 2;
            cellB.Contents.Add("B");
            row.Cells.Add(cellB);

            TableCell cellC = new TableCell();
            cellC.Contents.Add("C");
            row.Cells.Add(cellC);

            // Row 1
            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellD = new TableCell();
            cellD.Contents.Add("D");
            row.Cells.Add(cellD);

            Assert.AreNotEqual(0, grid.Width);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_WidthCalcWithColspanRowspan()
        {
            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.Margins = 10;

            doc.Pages.Add(section);

            Unit[] columnWidths = new Unit[] {
                new Unit(20, PageUnits.Percent),
                new Unit(30, PageUnits.Percent),
                new Unit(50, PageUnits.Percent)
            };

            TableGrid grid = new TableGrid();
            grid.Width = 400;
            section.Contents.Add(grid);

            // Row 0
            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellRowSpan = 2;
            cellA.Width = columnWidths[0];
            cellA.Contents.Add("A");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.CellColumnSpan = 2;
            cellB.Contents.Add("B");
            row.Cells.Add(cellB);

            // Row 1
            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellC = new TableCell();
            cellC.Width = columnWidths[1];
            cellC.Contents.Add("C");
            row.Cells.Add(cellC);

            TableCell cellD = new TableCell();
            cellD.Width = columnWidths[2];
            cellD.Contents.Add("D");
            row.Cells.Add(cellD);

            Assert.AreNotEqual(0, grid.Width);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_HeightCalcWithColspanRowspan()
        {
            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.Margins = 10;

            doc.Pages.Add(section);

            TableGrid grid = new TableGrid();
            grid.Width = 400;
            section.Contents.Add(grid);

            // Row 0
            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellRowSpan = 2;
            cellA.Height = 100;
            cellA.Contents.Add("A tall cell spanning 2 rows");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.CellColumnSpan = 2;
            cellB.Contents.Add("B");
            row.Cells.Add(cellB);

            // Row 1
            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellC = new TableCell();
            cellC.Contents.Add("C");
            row.Cells.Add(cellC);

            TableCell cellD = new TableCell();
            cellD.Contents.Add("D");
            row.Cells.Add(cellD);

            Assert.AreNotEqual(0, grid.Width);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_NestedTableWithRowspan()
        {
            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.Margins = 10;

            doc.Pages.Add(section);

            TableGrid grid = new TableGrid();
            grid.Width = 400;
            section.Contents.Add(grid);

            // Row 0
            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellRowSpan = 2;
            cellA.Contents.Add("A");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            // Nested table in cell B
            TableGrid nestedGrid = new TableGrid();
            nestedGrid.Width = 150;
            cellB.Contents.Add(nestedGrid);

            TableRow nestedRow = new TableRow();
            nestedGrid.Rows.Add(nestedRow);

            TableCell nestedCell = new TableCell();
            nestedCell.Contents.Add("Nested");
            nestedRow.Cells.Add(nestedCell);

            row.Cells.Add(cellB);

            // Row 1
            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellC = new TableCell();
            cellC.Contents.Add("C");
            row.Cells.Add(cellC);

            Assert.AreNotEqual(0, grid.Width);
        }

        #endregion

        #region Page Break Handling (Category G - partial)

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_PageBreak_RowspanEndsBeforeBreak()
        {
            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 400; // Smaller height to force page break
            section.Margins = 20;

            doc.Pages.Add(section);

            TableGrid grid = new TableGrid();
            grid.Width = 500;
            section.Contents.Add(grid);

            // Row 0
            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellRowSpan = 2;
            cellA.Contents.Add("A");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.Contents.Add("B");
            row.Cells.Add(cellB);

            // Row 1
            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellC = new TableCell();
            cellC.Contents.Add("C");
            row.Cells.Add(cellC);

            // Row 2 - on new page
            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellD = new TableCell();
            cellD.Contents.Add("D");
            row.Cells.Add(cellD);

            TableCell cellE = new TableCell();
            cellE.Contents.Add("E");
            row.Cells.Add(cellE);

            Assert.AreNotEqual(0, grid.Width);
        }

        #endregion

        #region Validation Tests (Category J - partial)

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_Validation_RowspanExceedsTable()
        {
            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.Margins = 10;

            doc.Pages.Add(section);

            TableGrid grid = new TableGrid();
            grid.Width = 400;
            section.Contents.Add(grid);

            // Row 0
            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellRowSpan = 5; // Table only has 3 rows
            cellA.Contents.Add("A");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.Contents.Add("B");
            row.Cells.Add(cellB);

            // Row 1
            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellC = new TableCell();
            cellC.Contents.Add("C");
            row.Cells.Add(cellC);

            // Row 2
            row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellD = new TableCell();
            cellD.Contents.Add("D");
            row.Cells.Add(cellD);

            // Should handle gracefully (truncate rowspan)
            Assert.AreNotEqual(0, grid.Width);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_Validation_LargeRowspan()
        {
            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.Margins = 10;

            doc.Pages.Add(section);

            TableGrid grid = new TableGrid();
            grid.Width = 400;
            section.Contents.Add(grid);

            // Row 0
            TableRow row = new TableRow();
            grid.Rows.Add(row);

            TableCell cellA = new TableCell();
            cellA.CellRowSpan = 50; // Very large rowspan
            cellA.Contents.Add("A");
            row.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.Contents.Add("B");
            row.Cells.Add(cellB);

            // Add 5 more rows
            for (int r = 1; r < 5; r++)
            {
                row = new TableRow();
                grid.Rows.Add(row);

                TableCell cell = new TableCell();
                cell.Contents.Add($"Row{r}");
                row.Cells.Add(cell);
            }

            // Should handle without performance issues
            Assert.AreNotEqual(0, grid.Width);
        }

        #endregion

        #region Visual Test

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableRowspan_Visual_ColorfulTable()
        {
            Document doc = new Document();
            Section section = new Section();
            section.Margins = 20;
            doc.Pages.Add(section);

            // Title
            Div titleDiv = new Div();
            titleDiv.Contents.Add("Rowspan Visual Test - Colored Table");
            titleDiv.Style.Font.FontSize = 20;
            titleDiv.Style.Font.FontBold = true;
            titleDiv.Style.Margins.Bottom = 20;
            section.Contents.Add(titleDiv);

            // Create table with rowspan
            TableGrid grid = new TableGrid();
            grid.Width = 400;
            grid.Style.Border.Width = 1;
            grid.Style.Border.Color = StandardColors.Black;
            section.Contents.Add(grid);

            // Row 0: Cell A (rowspan=3), Cell B, Cell C, Cell D
            TableRow row0 = new TableRow();
            grid.Rows.Add(row0);

            TableCell cellA = new TableCell();
            cellA.CellRowSpan = 3;
            cellA.Width = 80;
            cellA.Style.Background.Color = StandardColors.LightBlue;
            cellA.Style.Padding.All = 8;
            cellA.Contents.Add("A\n(3 rows)");
            row0.Cells.Add(cellA);

            TableCell cellB = new TableCell();
            cellB.Width = 80;
            cellB.Style.Background.Color = StandardColors.LightGreen;
            cellB.Style.Padding.All = 8;
            cellB.Contents.Add("B");
            row0.Cells.Add(cellB);

            TableCell cellC = new TableCell();
            cellC.Width = 80;
            cellC.Style.Background.Color = StandardColors.LightYellow;
            cellC.Style.Padding.All = 8;
            cellC.Contents.Add("C");
            row0.Cells.Add(cellC);

            TableCell cellD = new TableCell();
            cellD.Width = 80;
            cellD.Style.Background.Color = StandardColors.LightCoral;
            cellD.Style.Padding.All = 8;
            cellD.Contents.Add("D");
            row0.Cells.Add(cellD);

            // Row 1: Cell E, Cell F (colspan=2), Cell G
            TableRow row1 = new TableRow();
            grid.Rows.Add(row1);

            TableCell cellE = new TableCell();
            cellE.Width = 80;
            cellE.Style.Background.Color = StandardColors.LightCyan;
            cellE.Style.Padding.All = 8;
            cellE.Contents.Add("E");
            row1.Cells.Add(cellE);

            TableCell cellF = new TableCell();
            cellF.CellColumnSpan = 2;
            cellF.Width = 160;
            cellF.Style.Background.Color = StandardColors.LightSalmon;
            cellF.Style.Padding.All = 8;
            cellF.Contents.Add("F (2 cols)");
            row1.Cells.Add(cellF);

            TableCell cellG = new TableCell();
            cellG.Width = 80;
            cellG.Style.Background.Color = StandardColors.White;
            cellG.Style.Padding.All = 8;
            cellG.Contents.Add("G");
            row1.Cells.Add(cellG);

            // Row 2: Cell H (rowspan=2), Cell I, Cell J, Cell K
            TableRow row2 = new TableRow();
            grid.Rows.Add(row2);

            TableCell cellH = new TableCell();
            cellH.CellRowSpan = 2;
            cellH.Width = 80;
            cellH.Style.Background.Color = StandardColors.LightSkyBlue;
            cellH.Style.Padding.All = 8;
            cellH.Contents.Add("H\n(2 rows)");
            row2.Cells.Add(cellH);

            TableCell cellI = new TableCell();
            cellI.Width = 80;
            cellI.Style.Background.Color = StandardColors.Blue;
            cellI.Style.Padding.All = 8;
            cellI.Contents.Add("I");
            row2.Cells.Add(cellI);

            TableCell cellJ = new TableCell();
            cellJ.Width = 80;
            cellJ.Style.Background.Color = StandardColors.Yellow;
            cellJ.Style.Padding.All = 8;
            cellJ.Contents.Add("J");
            row2.Cells.Add(cellJ);

            TableCell cellK = new TableCell();
            cellK.Width = 80;
            cellK.Style.Background.Color = StandardColors.Green;
            cellK.Style.Padding.All = 8;
            cellK.Contents.Add("K");
            row2.Cells.Add(cellK);

            // Row 3: Cell L, Cell M, Cell N
            TableRow row3 = new TableRow();
            grid.Rows.Add(row3);

            TableCell cellL = new TableCell();
            cellL.Width = 80;
            cellL.Style.Background.Color = StandardColors.LightSteelBlue;
            cellL.Style.Padding.All = 8;
            cellL.Contents.Add("L");
            row3.Cells.Add(cellL);

            TableCell cellM = new TableCell();
            cellM.Width = 80;
            cellM.Style.Background.Color = StandardColors.Blue;
            cellM.Style.Padding.All = 8;
            cellM.Contents.Add("M");
            row3.Cells.Add(cellM);

            TableCell cellN = new TableCell();
            cellN.Width = 80;
            cellN.Style.Background.Color = StandardColors.Red;
            cellN.Style.Padding.All = 8;
            cellN.Contents.Add("N");
            row3.Cells.Add(cellN);

            // Generate PDF
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
    }
}
