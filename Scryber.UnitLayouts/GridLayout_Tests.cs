using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.Styles;
using Scryber.Styles.Selectors;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Layout tests for display:grid (LayoutEngineFlexGrid).
    ///
    /// Page is 600×800pt with no margin or padding.
    /// Grid containers have an explicit 600pt width and a 1pt black border unless noted.
    /// Grid items have a 1pt grey border and 4pt padding unless noted.
    ///
    /// Assertion tolerances are 1pt for widths/X and 2pt for heights/Y.
    /// </summary>
    [TestClass()]
    public class GridLayout_Tests
    {
        private const string TestCategory = "Layout-Grid";

        private const double PageW  = 600;
        private const double PageH  = 800;
        private const double GridBorder = 1;   // 1pt border on every grid container
        private const double ItemBorder = 1;   // 1pt border on every grid item
        private const double ItemPad   = 4;    // 4pt padding on every grid item

        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
            => _layout = args.Context.GetLayout<PDFLayoutDocument>();

        // ======================================================================
        // Helpers
        // ======================================================================

        private static Document CreateDoc(out Page pg)
        {
            var doc = new Document();
            pg = new Page();
            pg.Style.PageStyle.Width  = PageW;
            pg.Style.PageStyle.Height = PageH;
            pg.Style.Padding.All  = 0;
            pg.Style.Margins.All  = 0;
            doc.Pages.Add(pg);
            return doc;
        }

        /// <summary>Creates a display:grid panel with a visible outer border.</summary>
        private static Panel CreateGrid(Page pg, string templateColumns,
                                        double width = PageW,
                                        double padding = 0, double margin = 0,
                                        string templateRows = null)
        {
            var panel = new Panel();
            panel.Style.Position.DisplayMode  = DisplayMode.FlexGrid;
            panel.Style.Grid.TemplateColumns  = templateColumns;
            panel.Width = width;
            // Visible outer border
            panel.Style.Border.LineStyle = LineType.Solid;
            panel.Style.Border.Width     = GridBorder;
            panel.Style.Border.Color     = new Color(0, 0, 0);
            if (padding > 0) panel.Style.Padding.All = padding;
            if (margin  > 0) panel.Style.Margins.All = margin;
            if (templateRows != null) panel.Style.Grid.TemplateRows = templateRows;
            pg.Contents.Add(panel);
            return panel;
        }

        /// <summary>
        /// Creates a grid item panel with a visible border, padding, an optional
        /// explicit height, and a text label.
        /// </summary>
        private static Panel AddItem(Panel grid, string label,
                                     double height = 50,
                                     double padding = ItemPad,
                                     Color? borderColor = null)
        {
            var item = new Div();
            item.Style.Border.LineStyle = LineType.Solid;
            item.Style.Border.Width     = ItemBorder;
            item.Style.Border.Color     = borderColor ?? new Color(80, 80, 80);
            item.Style.Padding.All      = padding;
            item.Height = height;
            item.Contents.Add(new Label { Text = label });
            grid.Contents.Add(item);
            return item;
        }

        // Layout accessors ---------------------------------------------------

        private static PDFLayoutBlock GetGridBlock(PDFLayoutRegion pageRegion)
            => pageRegion.Contents[0] as PDFLayoutBlock;

        private static PDFLayoutBlock GetRowBlock(PDFLayoutBlock gridBlock, int rowIndex)
            => gridBlock.Columns[0].Contents[rowIndex] as PDFLayoutBlock;

        private static PDFLayoutBlock GetItemBlock(PDFLayoutBlock rowBlock, int colIndex)
        {
            var col = rowBlock.Columns[colIndex];
            foreach (var item in col.Contents)
                if (item is PDFLayoutBlock b) return b;
            return null;
        }

        // Text collector -----------------------------------------------------

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

        // Recursive search used by CSS-parsing test --------------------------

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

        // ======================================================================
        // 1. Equal fr columns — 1fr 1fr
        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_TwoColumns_EqualWidth()
        {
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr");
            AddItem(grid, "Alpha");
            AddItem(grid, "Beta");

            using (var ms = DocStreams.GetOutputStream("Grid_TwoColumns_Equal.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            Assert.AreEqual(1, _layout.AllPages.Count);

            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            Assert.IsNotNull(gridBlock, "Grid block must exist");

            // Structure: one row, two columns
            var tableRegion = gridBlock.Columns[0];
            Assert.AreEqual(1, tableRegion.Contents.Count, "1fr 1fr with 2 items should produce 1 row");

            var rowBlock = GetRowBlock(gridBlock, 0);
            Assert.IsNotNull(rowBlock, "Row block must exist");
            Assert.AreEqual(2, rowBlock.Columns.Length, "Two items should produce 2 columns");

            // Column widths: each = PageW / 2 = 300pt
            double expectedW = PageW / 2.0;
            Assert.AreEqual(expectedW, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column 0 (1fr) should be half the grid width");
            Assert.AreEqual(expectedW, rowBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Column 1 (1fr) should be half the grid width");

            // Column X positions: side-by-side
            Assert.AreEqual(0.0,      rowBlock.Columns[0].TotalBounds.X.PointsValue, 1.0,
                "Column 0 should start at X=0");
            Assert.AreEqual(expectedW, rowBlock.Columns[1].TotalBounds.X.PointsValue, 1.0,
                "Column 1 should start at X=300");

            // Row height: items are 50pt tall, row should be at least that
            Assert.IsTrue(rowBlock.TotalBounds.Height.PointsValue >= 50,
                "Row height should be at least 50pt (the item height)");

            // Content in the correct cells
            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "Alpha", "Column 0 text");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "Beta",  "Column 1 text");
        }

        // ======================================================================
        // 2. Proportional fr columns — 1fr 2fr
        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_TwoColumns_Proportional()
        {
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 2fr");
            AddItem(grid, "Narrow", height: 60, borderColor: new Color(200, 0, 0));
            AddItem(grid, "Wide",   height: 60, borderColor: new Color(0, 0, 200));

            using (var ms = DocStreams.GetOutputStream("Grid_TwoColumns_Proportional.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var rowBlock   = GetRowBlock(GetGridBlock(pageRegion), 0);
            Assert.IsNotNull(rowBlock);
            Assert.AreEqual(2, rowBlock.Columns.Length);

            // 1fr of 3fr total = 200pt; 2fr = 400pt
            double col0W = PageW / 3.0;
            double col1W = PageW * 2.0 / 3.0;
            Assert.AreEqual(col0W, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column 0 (1fr) should be 200pt");
            Assert.AreEqual(col1W, rowBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Column 1 (2fr) should be 400pt");

            // Side-by-side: col1 starts where col0 ends
            Assert.AreEqual(0.0,   rowBlock.Columns[0].TotalBounds.X.PointsValue, 1.0, "Col0 X=0");
            Assert.AreEqual(col0W, rowBlock.Columns[1].TotalBounds.X.PointsValue, 1.0, "Col1 X=200");

            // Row height ≥ explicit item height
            Assert.IsTrue(rowBlock.TotalBounds.Height.PointsValue >= 60,
                "Row height should be at least 60pt");

            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "Narrow", "Col0 text");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "Wide",   "Col1 text");
        }

        // ======================================================================
        // 3. Fixed + fr column — 200pt 1fr
        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_FixedAndFrColumn()
        {
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "200pt 1fr");
            AddItem(grid, "Fixed",     height: 50, borderColor: new Color(150, 0, 150));
            AddItem(grid, "Remaining", height: 50, borderColor: new Color(0, 150, 0));

            using (var ms = DocStreams.GetOutputStream("Grid_FixedAndFr.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var rowBlock   = GetRowBlock(GetGridBlock(pageRegion), 0);
            Assert.IsNotNull(rowBlock);
            Assert.AreEqual(2, rowBlock.Columns.Length);

            // col0 = 200pt exactly; col1 = 600 - 200 = 400pt
            Assert.AreEqual(200.0, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Fixed column should be exactly 200pt");
            Assert.AreEqual(400.0, rowBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Fr column should take remaining 400pt");

            Assert.AreEqual(0.0,   rowBlock.Columns[0].TotalBounds.X.PointsValue, 1.0, "Fixed col X=0");
            Assert.AreEqual(200.0, rowBlock.Columns[1].TotalBounds.X.PointsValue, 1.0, "Fr col X=200");

            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "Fixed",     "Col0 text");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "Remaining", "Col1 text");
        }

        // ======================================================================
        // 4. Auto-flow row-major — 6 items in 3-column grid = 2 rows
        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_AutoFlow_FillsRowByRow()
        {
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr 1fr");

            var labels = new[] { "R0C0", "R0C1", "R0C2", "R1C0", "R1C1", "R1C2" };
            foreach (var lbl in labels)
                AddItem(grid, lbl, height: 40);

            using (var ms = DocStreams.GetOutputStream("Grid_AutoFlow_TwoRows.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion  = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock   = GetGridBlock(pageRegion);
            var tableRegion = gridBlock.Columns[0];

            // Structure: 2 rows
            Assert.AreEqual(2, tableRegion.Contents.Count,
                "6 items in 3-col grid should produce 2 rows");

            var row0 = GetRowBlock(gridBlock, 0);
            var row1 = GetRowBlock(gridBlock, 1);
            Assert.IsNotNull(row0, "Row 0 must exist");
            Assert.IsNotNull(row1, "Row 1 must exist");
            Assert.AreEqual(3, row0.Columns.Length, "Row 0 should have 3 columns");
            Assert.AreEqual(3, row1.Columns.Length, "Row 1 should have 3 columns");

            // Each column is 600/3 = 200pt wide
            double expectedW = PageW / 3.0;
            for (int c = 0; c < 3; c++)
            {
                Assert.AreEqual(expectedW, row0.Columns[c].TotalBounds.Width.PointsValue, 1.0,
                    $"Row 0 col {c} width = 200pt");
                Assert.AreEqual(expectedW, row1.Columns[c].TotalBounds.Width.PointsValue, 1.0,
                    $"Row 1 col {c} width = 200pt");
            }

            // Row 1 is below row 0
            Assert.IsTrue(row1.TotalBounds.Y > row0.TotalBounds.Y,
                "Row 1 should be positioned below row 0");

            // Both rows have positive height
            Assert.IsTrue(row0.TotalBounds.Height.PointsValue >= 40, "Row 0 height ≥ 40pt");
            Assert.IsTrue(row1.TotalBounds.Height.PointsValue >= 40, "Row 1 height ≥ 40pt");

            // Correct items in each cell
            StringAssert.Contains(CollectText(row0.Columns[0]), "R0C0");
            StringAssert.Contains(CollectText(row0.Columns[1]), "R0C1");
            StringAssert.Contains(CollectText(row0.Columns[2]), "R0C2");
            StringAssert.Contains(CollectText(row1.Columns[0]), "R1C0");
            StringAssert.Contains(CollectText(row1.Columns[1]), "R1C1");
            StringAssert.Contains(CollectText(row1.Columns[2]), "R1C2");
        }

        // ======================================================================
        // 5. repeat() expansion — repeat(3, 1fr)
        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_Repeat_ExpandsColumns()
        {
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "repeat(3, 1fr)");
            AddItem(grid, "X", height: 50, borderColor: new Color(200, 100, 0));
            AddItem(grid, "Y", height: 50, borderColor: new Color(0,   200, 0));
            AddItem(grid, "Z", height: 50, borderColor: new Color(0,   0,   200));

            using (var ms = DocStreams.GetOutputStream("Grid_Repeat_ThreeColumns.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var rowBlock   = GetRowBlock(GetGridBlock(pageRegion), 0);
            Assert.IsNotNull(rowBlock);
            Assert.AreEqual(3, rowBlock.Columns.Length, "repeat(3, 1fr) should produce 3 columns");

            double expected = PageW / 3.0;
            for (int i = 0; i < 3; i++)
                Assert.AreEqual(expected, rowBlock.Columns[i].TotalBounds.Width.PointsValue, 1.0,
                    $"Column {i} should be 1/3 of grid width");

            // X positions: 0, 200, 400
            Assert.AreEqual(0.0,           rowBlock.Columns[0].TotalBounds.X.PointsValue, 1.0);
            Assert.AreEqual(expected,       rowBlock.Columns[1].TotalBounds.X.PointsValue, 1.0);
            Assert.AreEqual(expected * 2.0, rowBlock.Columns[2].TotalBounds.X.PointsValue, 1.0);

            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "X");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "Y");
            StringAssert.Contains(CollectText(rowBlock.Columns[2]), "Z");
        }

        // ======================================================================
        // 6. Column gap reduces available column width
        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_Gap_ReducesColumnWidths()
        {
            const double gap = 20;
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr");
            grid.Style.Flex.ColumnGap = gap;
            AddItem(grid, "Left",  height: 50, borderColor: new Color(200, 0, 0));
            AddItem(grid, "Right", height: 50, borderColor: new Color(0, 0, 200));

            using (var ms = DocStreams.GetOutputStream("Grid_Gap_TwoColumns.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var rowBlock   = GetRowBlock(GetGridBlock(pageRegion), 0);
            Assert.IsNotNull(rowBlock);
            Assert.AreEqual(2, rowBlock.Columns.Length);

            // Each column content = (600 - 20) / 2 = 290pt.
            // Column 0 has no left margin so TotalBounds.Width == content width.
            // Column 1 has margin-left == gap, so TotalBounds.Width == gap + content width.
            double expectedW = (PageW - gap) / 2.0;
            Assert.AreEqual(expectedW,        rowBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column 0 width with gap");
            Assert.AreEqual(gap + expectedW,  rowBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Column 1 total bounds includes margin-left gap");

            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "Left",  "Col0 text");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "Right", "Col1 text");
        }

        // ======================================================================
        // 7. Container padding reduces available column width
        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_ContainerPadding_ReducesColumnWidth()
        {
            const double pad = 20;
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr", padding: pad);
            AddItem(grid, "Padded A", height: 50);
            AddItem(grid, "Padded B", height: 50);

            using (var ms = DocStreams.GetOutputStream("Grid_ContainerPadding.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            Assert.IsNotNull(gridBlock, "Grid block must exist");

            // Grid block total width is the stated 600pt
            Assert.AreEqual(PageW, gridBlock.TotalBounds.Width.PointsValue, 1.0,
                "Grid block total width should remain 600pt");

            // Columns share the padded interior: (600 - 2×20) / 2 = 280pt each
            double expectedW = (PageW - 2 * pad) / 2.0;
            var rowBlock = GetRowBlock(gridBlock, 0);
            Assert.IsNotNull(rowBlock, "Row block must exist");
            Assert.AreEqual(2, rowBlock.Columns.Length);

            Assert.AreEqual(expectedW, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column 0 width reduced by container padding");
            Assert.AreEqual(expectedW, rowBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Column 1 width reduced by container padding");

            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "Padded A");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "Padded B");
        }

        // ======================================================================
        // 8. Container margin offsets the grid block position
        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_ContainerMargin_OffsetsPosition()
        {
            // No explicit width so the engine reads available width and subtracts margin.
            const double leftMargin = 30;
            var doc = CreateDoc(out var pg);
            var grid = new Panel();
            grid.Style.Position.DisplayMode = DisplayMode.FlexGrid;
            grid.Style.Grid.TemplateColumns  = "1fr 1fr";
            grid.Style.Border.LineStyle = LineType.Solid;
            grid.Style.Border.Width     = GridBorder;
            grid.Style.Border.Color     = new Color(0, 0, 0);
            grid.Style.Margins.Left     = leftMargin;
            pg.Contents.Add(grid);

            AddItem(grid, "M-Left",  height: 50);
            AddItem(grid, "M-Right", height: 50);

            using (var ms = DocStreams.GetOutputStream("Grid_ContainerMargin.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            Assert.IsNotNull(gridBlock, "Grid block must exist");

            // Available width = PageW - leftMargin = 570pt; each col = 285pt
            double expectedW = (PageW - leftMargin) / 2.0;
            var rowBlock = GetRowBlock(gridBlock, 0);
            Assert.IsNotNull(rowBlock);
            Assert.AreEqual(2, rowBlock.Columns.Length);
            Assert.AreEqual(expectedW, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 2.0,
                "Column 0 width should account for margin");
            Assert.AreEqual(expectedW, rowBlock.Columns[1].TotalBounds.Width.PointsValue, 2.0,
                "Column 1 width should account for margin");

            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "M-Left");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "M-Right");
        }

        // ======================================================================
        // 9. Item padding and border — inner content is inset, items still render
        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_ItemPaddingAndBorder_ContentIsInset()
        {
            const double itemPad    = 10;
            const double itemBorder = 2;

            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr");

            // Items with non-default border and padding
            var itemA = new Panel();
            itemA.Style.Border.LineStyle = LineType.Solid;
            itemA.Style.Border.Width     = itemBorder;
            itemA.Style.Border.Color     = new Color(200, 0, 0);
            itemA.Style.Padding.All      = itemPad;
            itemA.Height = 60;
            itemA.Contents.Add(new Label { Text = "Inset A" });
            grid.Contents.Add(itemA);

            var itemB = new Panel();
            itemB.Style.Border.LineStyle = LineType.Solid;
            itemB.Style.Border.Width     = itemBorder;
            itemB.Style.Border.Color     = new Color(0, 0, 200);
            itemB.Style.Padding.All      = itemPad;
            itemB.Height = 60;
            itemB.Contents.Add(new Label { Text = "Inset B" });
            grid.Contents.Add(itemB);

            using (var ms = DocStreams.GetOutputStream("Grid_ItemPadding.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var rowBlock   = GetRowBlock(GetGridBlock(pageRegion), 0);
            Assert.IsNotNull(rowBlock);
            Assert.AreEqual(2, rowBlock.Columns.Length);

            // Column widths unchanged — 300pt each
            Assert.AreEqual(PageW / 2.0, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column 0 width");
            Assert.AreEqual(PageW / 2.0, rowBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Column 1 width");

            // Item blocks exist within each column
            var blockA = GetItemBlock(rowBlock, 0);
            var blockB = GetItemBlock(rowBlock, 1);
            Assert.IsNotNull(blockA, "Item A block must exist in column 0");
            Assert.IsNotNull(blockB, "Item B block must exist in column 1");

            // Item TotalBounds.Height >= explicit height (may include border+padding overhead)
            Assert.IsTrue(blockA.TotalBounds.Height.PointsValue >= 60,
                "Item A height should be at least the explicit 60pt");
            Assert.IsTrue(blockB.TotalBounds.Height.PointsValue >= 60,
                "Item B height should be at least the explicit 60pt");

            // Row height is at least the item height
            Assert.IsTrue(rowBlock.TotalBounds.Height.PointsValue >= 60,
                "Row height should be ≥ 60pt (tallest item)");

            // Text content present despite padding
            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "Inset A");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "Inset B");
        }

        // ======================================================================
        // 10. Row height governed by tallest item — mixed heights
        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_RowHeight_TallestItemWins()
        {
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr");
            AddItem(grid, "Short", height: 40, borderColor: new Color(200, 0, 0));
            AddItem(grid, "Tall",  height: 80, borderColor: new Color(0, 0, 200));

            using (var ms = DocStreams.GetOutputStream("Grid_RowHeight_TallestWins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var rowBlock   = GetRowBlock(GetGridBlock(pageRegion), 0);
            Assert.IsNotNull(rowBlock);
            Assert.AreEqual(2, rowBlock.Columns.Length);

            // Row height should be at least as tall as the tallest item (80pt)
            Assert.IsTrue(rowBlock.TotalBounds.Height.PointsValue >= 80,
                "Row height should be at least 80pt (the taller item)");

            // Both items are in the row
            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "Short");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "Tall");
        }

        // ======================================================================
        // 11. CSS parsing — grid-template-columns via inline style
        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_CSSParsed_DisplayGrid()
        {
            const string html = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:grid; grid-template-columns: 1fr 1fr 1fr; width:600pt;
              border: 1pt solid #000;"">
    <div style=""height:50pt; padding:4pt; border:1pt solid #888;"">CSS-P</div>
    <div style=""height:50pt; padding:4pt; border:1pt solid #888;"">CSS-Q</div>
    <div style=""height:50pt; padding:4pt; border:1pt solid #888;"">CSS-R</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(html),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Grid_CSSParsed.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var contentBlock = layout.AllPages[0].ContentBlock;

            var rowBlock = FindRowWithCols(contentBlock.Columns[0], 3);
            Assert.IsNotNull(rowBlock, "CSS parsed display:grid should produce a row block with 3 columns");
            Assert.AreEqual(3, rowBlock.Columns.Length, "3 items should produce 3 columns");

            // Each column should be 200pt (600/3)
            double expectedW = 600.0 / 3.0;
            for (int i = 0; i < 3; i++)
                Assert.AreEqual(expectedW, rowBlock.Columns[i].TotalBounds.Width.PointsValue, 1.0,
                    $"CSS column {i} should be 200pt");

            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "CSS-P");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "CSS-Q");
            StringAssert.Contains(CollectText(rowBlock.Columns[2]), "CSS-R");
        }

        // ======================================================================
        // 12. Empty grid — must not throw
        // ======================================================================

        // ======================================================================
        // grid-auto-flow: column — column-major item placement
        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_AutoFlow_Column_SixItems_ThreeColumns()
        {
            // With grid-auto-flow: column, items fill top-to-bottom then next column.
            // 6 items in 3 columns → 2 rows; placement:
            //   col0    col1    col2
            //  [Item0] [Item2] [Item4]
            //  [Item1] [Item3] [Item5]
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr 1fr");
            grid.Style.Grid.AutoFlow = GridAutoFlow.Column;

            AddItem(grid, "Item0", height: 40);
            AddItem(grid, "Item1", height: 40);
            AddItem(grid, "Item2", height: 40);
            AddItem(grid, "Item3", height: 40);
            AddItem(grid, "Item4", height: 40);
            AddItem(grid, "Item5", height: 40);

            using (var ms = DocStreams.GetOutputStream("Grid_AutoFlow_Column_6x3.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var gridBlock = GetGridBlock(_layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(gridBlock);

            Assert.AreEqual(2, gridBlock.Columns[0].Contents.Count,
                "6 items in 3-col column-flow grid should produce 2 rows");

            var row0 = GetRowBlock(gridBlock, 0);
            var row1 = GetRowBlock(gridBlock, 1);
            Assert.AreEqual(3, row0.Columns.Length, "Row 0 should have 3 columns");
            Assert.AreEqual(3, row1.Columns.Length, "Row 1 should have 3 columns");

            // Column-major order: col0=Items 0,1  col1=Items 2,3  col2=Items 4,5
            StringAssert.Contains(CollectText(row0.Columns[0]), "Item0", "Row0 Col0 = Item0");
            StringAssert.Contains(CollectText(row1.Columns[0]), "Item1", "Row1 Col0 = Item1");
            StringAssert.Contains(CollectText(row0.Columns[1]), "Item2", "Row0 Col1 = Item2");
            StringAssert.Contains(CollectText(row1.Columns[1]), "Item3", "Row1 Col1 = Item3");
            StringAssert.Contains(CollectText(row0.Columns[2]), "Item4", "Row0 Col2 = Item4");
            StringAssert.Contains(CollectText(row1.Columns[2]), "Item5", "Row1 Col2 = Item5");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_AutoFlow_Column_OddItemCount()
        {
            // 5 items in 2 columns, column flow:
            //   col0    col1
            //  [Item0] [Item3]
            //  [Item1] [Item4]
            //  [Item2]  (empty)
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr");
            grid.Style.Grid.AutoFlow = GridAutoFlow.Column;

            AddItem(grid, "Item0", height: 30);
            AddItem(grid, "Item1", height: 30);
            AddItem(grid, "Item2", height: 30);
            AddItem(grid, "Item3", height: 30);
            AddItem(grid, "Item4", height: 30);

            using (var ms = DocStreams.GetOutputStream("Grid_AutoFlow_Column_5x2.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var gridBlock = GetGridBlock(_layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(gridBlock);

            // 5 items, 2 cols → ceil(5/2) = 3 rows
            Assert.AreEqual(3, gridBlock.Columns[0].Contents.Count,
                "5 items in 2-col column-flow grid should produce 3 rows");

            var row0 = GetRowBlock(gridBlock, 0);
            var row1 = GetRowBlock(gridBlock, 1);
            var row2 = GetRowBlock(gridBlock, 2);

            StringAssert.Contains(CollectText(row0.Columns[0]), "Item0");
            StringAssert.Contains(CollectText(row1.Columns[0]), "Item1");
            StringAssert.Contains(CollectText(row2.Columns[0]), "Item2");
            StringAssert.Contains(CollectText(row0.Columns[1]), "Item3");
            StringAssert.Contains(CollectText(row1.Columns[1]), "Item4");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_AutoFlow_Column_CSSParsed()
        {
            // Verify grid-auto-flow: column parsed from inline CSS string.
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0;padding:0;"">
  <div style=""display:grid;width:600pt;grid-template-columns:1fr 1fr;grid-auto-flow:column;border:1pt solid #000000;"">
    <div style=""height:40pt;padding:4pt;border:1pt solid #646464;background-color:#D0E8FF;"">A</div>
    <div style=""height:40pt;padding:4pt;border:1pt solid #646464;background-color:#FFE8D0;"">B</div>
    <div style=""height:40pt;padding:4pt;border:1pt solid #646464;background-color:#D0FFE8;"">C</div>
    <div style=""height:40pt;padding:4pt;border:1pt solid #646464;background-color:#FFD0E8;"">D</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            using (var ms = DocStreams.GetOutputStream("Grid_AutoFlow_Column_CSS.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var gridBlock = GetGridBlock(_layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(gridBlock, "Grid block must exist");

            // 4 items, 2 cols, column flow → 2 rows
            Assert.AreEqual(2, gridBlock.Columns[0].Contents.Count,
                "4 items in 2-col column-flow grid should produce 2 rows");

            var row0 = GetRowBlock(gridBlock, 0);
            var row1 = GetRowBlock(gridBlock, 1);

            // Column-major: A,B in col0; C,D in col1
            StringAssert.Contains(CollectText(row0.Columns[0]), "A", "Row0 Col0 = A");
            StringAssert.Contains(CollectText(row1.Columns[0]), "B", "Row1 Col0 = B");
            StringAssert.Contains(CollectText(row0.Columns[1]), "C", "Row0 Col1 = C");
            StringAssert.Contains(CollectText(row1.Columns[1]), "D", "Row1 Col1 = D");
        }

        // Grid_AutoFlow_Column_ExplicitRowCount — 12 items, 6 explicit cols, 3 explicit rows,
        // auto-flow: column.  The explicit row count (3) must cap each column; items wrap to the
        // next column after 3 items.  Expected: 3 rows × 4 columns, NOT 2 rows × 6 columns.
        [TestCategory(TestCategory), TestMethod()]
        public void Grid_AutoFlow_Column_ExplicitRowCountCapsCols()
        {
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <style>
    @page { size: 600pt 400pt; margin: 0; }
    body  { margin: 0; padding: 0; }
    .grid {
        display: grid;
        grid-template-columns: repeat(6, 100pt);
        grid-template-rows: repeat(3, 100pt);
        grid-auto-flow: column;
        width: 600pt;
    }
    .item { border: 1pt solid black; }
  </style>
</head>
<body>
  <div class=""grid"">
    <div class=""item"">A</div><div class=""item"">B</div><div class=""item"">C</div>
    <div class=""item"">D</div><div class=""item"">E</div><div class=""item"">F</div>
    <div class=""item"">G</div><div class=""item"">H</div><div class=""item"">I</div>
    <div class=""item"">J</div><div class=""item"">K</div><div class=""item"">L</div>
  </div>
</body>
</html>";
            // Column-major order with 3-row capacity:
            //  col0: A B C   col1: D E F   col2: G H I   col3: J K L
            // → 3 rows × 4 columns

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            using (var ms = DocStreams.GetOutputStream("Grid_AutoFlow_Column_12x3rows4cols.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var gridBlock = GetGridBlock(_layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(gridBlock, "Grid block");

            Assert.AreEqual(3, gridBlock.Columns[0].Contents.Count, "Must have 3 rows (from grid-template-rows)");

            var row0 = GetRowBlock(gridBlock, 0);
            var row1 = GetRowBlock(gridBlock, 1);
            var row2 = GetRowBlock(gridBlock, 2);
            Assert.IsNotNull(row0, "Row 0");
            Assert.IsNotNull(row1, "Row 1");
            Assert.IsNotNull(row2, "Row 2");

            Assert.AreEqual(4, row0.Columns.Length, "Row 0 should have 4 columns");
            Assert.AreEqual(4, row1.Columns.Length, "Row 1 should have 4 columns");
            Assert.AreEqual(4, row2.Columns.Length, "Row 2 should have 4 columns");

            // col0 = A,B,C  col1 = D,E,F  col2 = G,H,I  col3 = J,K,L
            StringAssert.Contains(CollectText(row0.Columns[0]), "A", "Row0 Col0 = A");
            StringAssert.Contains(CollectText(row1.Columns[0]), "B", "Row1 Col0 = B");
            StringAssert.Contains(CollectText(row2.Columns[0]), "C", "Row2 Col0 = C");
            StringAssert.Contains(CollectText(row0.Columns[1]), "D", "Row0 Col1 = D");
            StringAssert.Contains(CollectText(row1.Columns[1]), "E", "Row1 Col1 = E");
            StringAssert.Contains(CollectText(row2.Columns[1]), "F", "Row2 Col1 = F");
        }

        // Grid_AutoFlow_Column_ExplicitItemSkipsOccupied — mirrors the container-14 scenario:
        // 12 items, 6 explicit columns, grid-auto-flow: column, no explicit rows.
        // Item 2 is placed explicitly at grid-row: 2, grid-column: 3 / 6 (cols 3-5, 0-indexed 2-4).
        // The auto-flow cursor must skip the cells occupied by item 2 and route around them.
        //
        // Expected column-major layout (2 rows driven by item 2 being at row 2):
        //   col0  col1  col2  col3  col4  col5  [col6]  [col7]
        //  row0:  1    4     6     7     8     9    11     ...
        //  row1:  3    5   [item2 spans cols 2-4]  10    12
        [TestCategory(TestCategory), TestMethod()]
        public void Grid_AutoFlow_Column_ExplicitItemSkipsOccupied()
        {
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <style>
    @page { size: 700pt 300pt; margin: 0; }
    body  { margin: 0; padding: 0; }
    .grid {
        display: grid;
        grid-template-columns: repeat(6, 100pt);
        grid-auto-flow: column;
        width: 600pt;
    }
    .item  { height: 50pt; border: 1pt solid black; }
    .item2 { grid-column: 3 / 6; grid-row: 2 / 3; }
  </style>
</head>
<body>
  <div class=""grid"">
    <div class=""item"">1</div>
    <div class=""item item2"">2</div>
    <div class=""item"">3</div>
    <div class=""item"">4</div>
    <div class=""item"">5</div>
    <div class=""item"">6</div>
    <div class=""item"">7</div>
    <div class=""item"">8</div>
    <div class=""item"">9</div>
    <div class=""item"">10</div>
    <div class=""item"">11</div>
    <div class=""item"">12</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            using (var ms = DocStreams.GetOutputStream("Grid_AutoFlow_Column_ExplicitSkip.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var gridBlock = GetGridBlock(_layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(gridBlock, "Grid block");

            // Item 2 at row 2 forces at least 2 rows.
            var row0 = GetRowBlock(gridBlock, 0);
            var row1 = GetRowBlock(gridBlock, 1);
            Assert.IsNotNull(row0, "Row 0");
            Assert.IsNotNull(row1, "Row 1");

            // Item 2 is explicitly at row 1 (0-indexed), cols 2-4 — verify its text and width.
            // It spans 3 cols = 300pt.
            StringAssert.Contains(CollectText(row1.Columns[2]), "2",  "Item 2 in row1 col2");
            var item2Block = GetItemBlock(row1, 2);
            Assert.IsNotNull(item2Block, "Item 2 block");
            Assert.AreEqual(300.0, item2Block.TotalBounds.Width.PointsValue, 2.0, "Item 2 spans 3 cols = 300pt");

            // Auto-placed items: cursor goes down col 0 (items 1, 3), then col 1 (4, 5), etc.
            // Col 0 row 0 = item 1, col 0 row 1 = item 3.
            StringAssert.Contains(CollectText(row0.Columns[0]), "1",  "Row0 Col0 = item 1");
            StringAssert.Contains(CollectText(row1.Columns[0]), "3",  "Row1 Col0 = item 3");

            // Col 1: items 4, 5.
            StringAssert.Contains(CollectText(row0.Columns[1]), "4",  "Row0 Col1 = item 4");
            StringAssert.Contains(CollectText(row1.Columns[1]), "5",  "Row1 Col1 = item 5");

            // Col 2 row 0: item 6 (row 1 is occupied by item 2).
            StringAssert.Contains(CollectText(row0.Columns[2]), "6",  "Row0 Col2 = item 6");
        }

        // ======================================================================
        // grid-template-rows — explicit row sizing
        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_TemplateRows_TwoFixedRows()
        {
            // grid-template-rows: 100pt 50pt should force row heights regardless of item content.
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr", templateRows: "100pt 50pt");

            // 4 short items — content alone would produce ~20pt rows, not 100/50
            AddItem(grid, "R0C0", height: 10);
            AddItem(grid, "R0C1", height: 10);
            AddItem(grid, "R1C0", height: 10);
            AddItem(grid, "R1C1", height: 10);

            using (var ms = DocStreams.GetOutputStream("Grid_TemplateRows_TwoFixed.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var gridBlock = GetGridBlock(_layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(gridBlock, "Grid block must exist");
            Assert.AreEqual(2, gridBlock.Columns[0].Contents.Count, "Grid should have 2 rows");

            var row0 = GetRowBlock(gridBlock, 0);
            var row1 = GetRowBlock(gridBlock, 1);

            Assert.AreEqual(100.0, row0.TotalBounds.Height.PointsValue, 2.0,
                "Row 0 should be 100pt (from grid-template-rows)");
            Assert.AreEqual(50.0, row1.TotalBounds.Height.PointsValue, 2.0,
                "Row 1 should be 50pt (from grid-template-rows)");

            // Row 1 Y should be ~100pt below row 0
            Assert.IsTrue(row1.TotalBounds.Y.PointsValue >= row0.TotalBounds.Y.PointsValue + 90.0,
                "Row 1 should start after row 0");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_TemplateRows_TemplateHeightApplied_WhenItemSmaller()
        {
            // When items are shorter than the template row height the template height wins.
            // (CSS grid: fixed track sizes do not shrink to content.)
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr", templateRows: "80pt 40pt");

            // Items shorter than the row definitions
            AddItem(grid, "R0C0", height: 10);
            AddItem(grid, "R0C1", height: 10);
            AddItem(grid, "R1C0", height: 10);
            AddItem(grid, "R1C1", height: 10);

            using (var ms = DocStreams.GetOutputStream("Grid_TemplateRows_ItemSmaller.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var gridBlock = GetGridBlock(_layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(gridBlock);
            Assert.AreEqual(2, gridBlock.Columns[0].Contents.Count);

            var row0 = GetRowBlock(gridBlock, 0);
            var row1 = GetRowBlock(gridBlock, 1);

            Assert.AreEqual(80.0, row0.TotalBounds.Height.PointsValue, 2.0,
                "Row 0 should be 80pt (template wins over 10pt item)");
            Assert.AreEqual(40.0, row1.TotalBounds.Height.PointsValue, 2.0,
                "Row 1 should be 40pt (template wins over 10pt item)");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_TemplateRows_CSSParsed()
        {
            // Verify grid-template-rows parsed from an inline CSS string drives row heights.
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0;padding:0;"">
  <div style=""display:grid;width:600pt;grid-template-columns:1fr 1fr;grid-template-rows:120pt 60pt;border:1pt solid #000000;"">
    <div style=""height:10pt;padding:4pt;border:1pt solid #646464;background-color:#D0E8FF;"">A</div>
    <div style=""height:10pt;padding:4pt;border:1pt solid #646464;background-color:#FFE8D0;"">B</div>
    <div style=""height:10pt;padding:4pt;border:1pt solid #646464;background-color:#D0FFE8;"">C</div>
    <div style=""height:10pt;padding:4pt;border:1pt solid #646464;background-color:#FFD0E8;"">D</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            using (var ms = DocStreams.GetOutputStream("Grid_TemplateRows_CSS.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            Assert.IsNotNull(gridBlock, "Grid block must exist");

            var row0 = GetRowBlock(gridBlock, 0);
            var row1 = GetRowBlock(gridBlock, 1);

            Assert.AreEqual(120.0, row0.TotalBounds.Height.PointsValue, 2.0,
                "Row 0 should be 120pt from CSS grid-template-rows");
            Assert.AreEqual(60.0, row1.TotalBounds.Height.PointsValue, 2.0,
                "Row 1 should be 60pt from CSS grid-template-rows");
        }

        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_Empty_DoesNotThrow()
        {
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr 1fr");
            // No items added — synthetic table has no rows.

            using (var ms = DocStreams.GetOutputStream("Grid_Empty.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should complete without throwing");
            Assert.AreEqual(1, _layout.AllPages.Count, "Should still produce one page");
        }


        // ======================================================================
        // grid-column / grid-row span
        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_ColumnSpan_SpannedItemIsWider()
        {
            // 3-column grid: item A spans 2 columns, item B takes the remaining column.
            // Expected: A width ≈ 2/3 * 600 = 400pt; B width ≈ 1/3 * 600 = 200pt.
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr 1fr");

            var itemA = AddItem(grid, "Spanned");
            itemA.Style.Grid.ColumnEnd = Scryber.Drawing.GridLineValue.Span(2);

            var itemB = AddItem(grid, "Normal");

            using (var ms = DocStreams.GetOutputStream("Grid_ColumnSpan_SpannedWider.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            Assert.IsNotNull(gridBlock);

            var rowBlock = GetRowBlock(gridBlock, 0);
            Assert.IsNotNull(rowBlock, "Row 0 must exist");
            // 3-column grid always produces 3 physical column slots per row.
            Assert.AreEqual(3, rowBlock.Columns.Length, "3-column grid always produces 3 physical column slots");

            // Use render bounds to compare item widths: A (span:2) should be wider than B (span:1).
            var arrangeA = itemA.GetFirstArrangement();
            var arrangeB = itemB.GetFirstArrangement();
            Assert.IsNotNull(arrangeA, "Item A must have been laid out");
            Assert.IsNotNull(arrangeB, "Item B must have been laid out");

            double wA = arrangeA.RenderBounds.Width.PointsValue;
            double wB = arrangeB.RenderBounds.Width.PointsValue;

            Assert.IsTrue(wA > wB, "Spanned item (col-span:2) should be wider than the normal item");
            Assert.AreEqual(PageW * 2.0 / 3.0, wA, 4.0, "Spanned item should be ~2/3 of grid width");
            Assert.AreEqual(PageW / 3.0,         wB, 3.0, "Normal item should be ~1/3 of grid width");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_ColumnSpan_NextItemWrapsToNewRow()
        {
            // 2-column grid; item A spans 2 → fills the whole row.
            // Items B and C go into row 1, side by side.
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr");

            var itemA = AddItem(grid, "Full-row");
            itemA.Style.Grid.ColumnEnd = Scryber.Drawing.GridLineValue.Span(2);

            AddItem(grid, "B");
            AddItem(grid, "C");

            using (var ms = DocStreams.GetOutputStream("Grid_ColumnSpan_NextWraps.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            Assert.IsNotNull(gridBlock);

            var tableRegion = gridBlock.Columns[0];
            Assert.AreEqual(2, tableRegion.Contents.Count, "Should be 2 rows: A alone, then B+C");

            var row0 = GetRowBlock(gridBlock, 0);
            var row1 = GetRowBlock(gridBlock, 1);
            Assert.IsNotNull(row0, "Row 0 must exist");
            Assert.IsNotNull(row1, "Row 1 must exist");

            // The layout engine always produces one column slot per physical grid track.
            // In a 2-column grid, every row has exactly 2 column slots.
            Assert.AreEqual(2, row0.Columns.Length, "2-column grid always has 2 track slots per row");
            Assert.AreEqual(2, row1.Columns.Length, "Row 1 should have 2 cells (B and C)");

            // Row 0: A's span-2 cell fills the entire row width.
            double wFull = row0.Columns[0].TotalBounds.Width.PointsValue
                         + row0.Columns[1].TotalBounds.Width.PointsValue;
            Assert.AreEqual(PageW, wFull, 3.0, "The two tracks combined should equal the grid width");

            // Row 1: B and C are equal halves
            double w0 = row1.Columns[0].TotalBounds.Width.PointsValue;
            double w1 = row1.Columns[1].TotalBounds.Width.PointsValue;
            Assert.AreEqual(w0, w1, 2.0, "B and C should have equal widths");
            Assert.AreEqual(PageW / 2.0, w0, 2.0, "Each half-width cell should be ~300pt");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_ColumnSpan_SpanOne_NoEffect()
        {
            // Explicitly setting span:1 should be the same as no span.
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr");

            var itemA = AddItem(grid, "Alpha");
            itemA.Style.Grid.ColumnEnd = Scryber.Drawing.GridLineValue.Span(1);

            AddItem(grid, "Beta");

            using (var ms = DocStreams.GetOutputStream("Grid_ColumnSpan_SpanOne.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var rowBlock = GetRowBlock(GetGridBlock(_layout.AllPages[0].ContentBlock.Columns[0]), 0);
            Assert.IsNotNull(rowBlock);
            Assert.AreEqual(2, rowBlock.Columns.Length, "span:1 should produce 2 separate cells");

            double w0 = rowBlock.Columns[0].TotalBounds.Width.PointsValue;
            double w1 = rowBlock.Columns[1].TotalBounds.Width.PointsValue;
            Assert.AreEqual(w0, w1, 1.0, "Both cells should be equal width with span:1");
        }

        // ======================================================================
        // Percentage grid-template-columns
        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_PercentageColumns_EqualHalves()
        {
            // grid-template-columns: 50% 50% on a 600pt container should give 300pt each.
            // This previously threw InvalidOperationException because ParseTrackList called
            // Unit.ToPoints() on a relative unit.
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "50% 50%");
            AddItem(grid, "Left-Pct",  height: 50, borderColor: new Color(200, 0, 0));
            AddItem(grid, "Right-Pct", height: 50, borderColor: new Color(0, 0, 200));

            using (var ms = DocStreams.GetOutputStream("Grid_PercentColumns_50_50.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout must complete without exception");
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var rowBlock   = GetRowBlock(GetGridBlock(pageRegion), 0);
            Assert.IsNotNull(rowBlock, "Row block must exist");
            Assert.AreEqual(2, rowBlock.Columns.Length, "50% 50% should produce 2 columns");

            // Each column = 50% of 600pt = 300pt
            Assert.AreEqual(300.0, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column 0 (50%) should be 300pt");
            Assert.AreEqual(300.0, rowBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Column 1 (50%) should be 300pt");

            Assert.AreNotEqual(50.0, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 5.0,
                "Must NOT treat 50% as 50pt");

            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "Left-Pct");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "Right-Pct");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_PercentageColumns_ThreeUnequalColumns()
        {
            // 25% 50% 25% on a 600pt container → 150pt 300pt 150pt
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "25% 50% 25%");
            AddItem(grid, "Quarter-L", height: 50, borderColor: new Color(150, 0,   0));
            AddItem(grid, "Half",      height: 50, borderColor: new Color(0,   150, 0));
            AddItem(grid, "Quarter-R", height: 50, borderColor: new Color(0,   0,   150));

            using (var ms = DocStreams.GetOutputStream("Grid_PercentColumns_25_50_25.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var rowBlock = GetRowBlock(GetGridBlock(_layout.AllPages[0].ContentBlock.Columns[0]), 0);
            Assert.IsNotNull(rowBlock);
            Assert.AreEqual(3, rowBlock.Columns.Length);

            Assert.AreEqual(150.0, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column 0 (25%) = 150pt");
            Assert.AreEqual(300.0, rowBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Column 1 (50%) = 300pt");
            Assert.AreEqual(150.0, rowBlock.Columns[2].TotalBounds.Width.PointsValue, 1.0,
                "Column 2 (25%) = 150pt");

            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "Quarter-L");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "Half");
            StringAssert.Contains(CollectText(rowBlock.Columns[2]), "Quarter-R");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_PercentageColumns_MixedWithFr()
        {
            // 50% 1fr on a 600pt container: percent column = 300pt, fr column gets remaining 300pt
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "50% 1fr");
            AddItem(grid, "HalfPct", height: 50, borderColor: new Color(180, 90, 0));
            AddItem(grid, "FrRest",  height: 50, borderColor: new Color(0, 90, 180));

            using (var ms = DocStreams.GetOutputStream("Grid_PercentColumns_50pct_1fr.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var rowBlock = GetRowBlock(GetGridBlock(_layout.AllPages[0].ContentBlock.Columns[0]), 0);
            Assert.IsNotNull(rowBlock);
            Assert.AreEqual(2, rowBlock.Columns.Length);

            // 50% = 300pt; remaining 300pt goes to 1fr
            Assert.AreEqual(300.0, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column 0 (50%) = 300pt");
            Assert.AreEqual(300.0, rowBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Column 1 (1fr of remaining 300pt) = 300pt");

            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "HalfPct");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "FrRest");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_PercentageColumns_CSSParsed()
        {
            // Verify percentage columns parsed from an inline CSS string.
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0;padding:0;"">
  <div style=""display:grid;width:600pt;grid-template-columns:33% 34% 33%;border:1pt solid #000000;"">
    <div style=""height:50pt;padding:4pt;border:1pt solid #646464;"">P</div>
    <div style=""height:50pt;padding:4pt;border:1pt solid #646464;"">Q</div>
    <div style=""height:50pt;padding:4pt;border:1pt solid #646464;"">R</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Grid_PercentColumns_CSS.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout must complete without exception");
            var rowBlock = FindRowWithCols(layout.AllPages[0].ContentBlock.Columns[0], 3);
            Assert.IsNotNull(rowBlock, "Row with 3 columns must exist");

            // 33%+34%+33% of 600pt = 198+204+198 = 600pt
            Assert.AreEqual(198.0, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 2.0,
                "Column 0 (33%) ≈ 198pt");
            Assert.AreEqual(204.0, rowBlock.Columns[1].TotalBounds.Width.PointsValue, 2.0,
                "Column 1 (34%) ≈ 204pt");
            Assert.AreEqual(198.0, rowBlock.Columns[2].TotalBounds.Width.PointsValue, 2.0,
                "Column 2 (33%) ≈ 198pt");

            // Must NOT treat 33% as 33pt
            Assert.IsTrue(rowBlock.Columns[0].TotalBounds.Width.PointsValue > 100.0,
                "Column 0 must NOT be 33pt (the raw percent number)");

            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "P");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "Q");
            StringAssert.Contains(CollectText(rowBlock.Columns[2]), "R");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void MMckinstry_Issue()
        {
            var source = DocStreams.AssertGetTemplatePath("Content/HTML/Mmcinstry_issue.html");
            
            using var doc = Document.ParseDocument(source);

            for (var i = 0; i < 17; i++)
            {
                var value = "field value " + i;
                var key = "field_" + i;
                doc.Params[key] = value;
            }
            
            
            
            using (var ms = DocStreams.GetOutputStream("Grid_Mmcinstry_issue.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
            Assert.AreEqual(2, _layout.AllPages.Count, "Should be 2 Pages");
            var page = _layout.AllPages[0];

            var grids = doc.FindMatches(".padded-top-large");
            Assert.AreEqual(5, grids.Count(), "Should be 5 grids with class .padded-top-large");
            
            var rows = grids.Find("div.row");
            Assert.AreEqual(14, rows.Count(), "Should be 14 Rows with class .row");

            var offset = Unit.Zero;
            
            foreach (Component row in rows)
            {
                var arrange = row.GetFirstArrangement();
                Assert.IsNotNull(arrange, "Row arrange must exist");
                
                if(offset != Unit.Zero) //should follow straight down after the first one.
                    Assert.AreEqual(offset, arrange.RenderBounds.Y, "Render offset Y should follow after previous offset with height");
                
                offset = arrange.RenderBounds.Y + arrange.RenderBounds.Height;
                
                
            }
        }
        
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Grid_VariousLayouts_Template()
        {
            var template = DocStreams.AssertGetTemplatePath("Content/HTML/HTML5/Grid_VariousLayouts.html");

            using var doc = Document.ParseDocument(template);

            using (var ms = DocStreams.GetOutputStream("Grid_VariousLayouts.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
        }

        // ======================================================================
        // gap CSS property — single and two-value layout tests
        // ======================================================================

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_GapCSS_SingleValue_SetsColumnAndRowGap()
        {
            // gap: 20pt (single value) → column-gap = 20pt AND row-gap = 20pt.
            // Grid: 600pt wide, 2 columns (1fr 1fr), 2 rows, items 50pt high.
            // Expected column width = (600 - 20) / 2 = 290pt.
            // Row 1 should be offset by item height (50) + row gap (20) = 70pt from grid top,
            // and its total height should be item height + row gap = 70pt (top margin on cell).
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <style>
    @page { size: 600pt 800pt; margin: 0; }
    body  { margin: 0; padding: 0; }
    .grid { display: grid; grid-template-columns: 1fr 1fr; gap: 20pt; width: 600pt; }
    .item { height: 50pt; border: 1pt solid black; }
  </style>
</head>
<body>
  <div class=""grid"">
    <div class=""item"">A</div>
    <div class=""item"">B</div>
    <div class=""item"">C</div>
    <div class=""item"">D</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            using (var ms = DocStreams.GetOutputStream("Grid_GapCSS_SingleValue.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            var row0       = GetRowBlock(gridBlock, 0);
            var row1       = GetRowBlock(gridBlock, 1);
            Assert.IsNotNull(row0, "Row 0");
            Assert.IsNotNull(row1, "Row 1");

            // Column gap: each column = (600 - 20) / 2 = 290pt
            const double colGap  = 20;
            const double rowGap  = 20;
            const double itemH   = 50;
            double expectedColW  = (PageW - colGap) / 2.0;
            Assert.AreEqual(expectedColW, row0.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column width with single-value gap");

            // Row gap: row 1 starts immediately after row 0 (which is 50pt high),
            // and its height = 50pt item + 20pt top margin injected for row gap.
            Assert.AreEqual(itemH,          row1.TotalBounds.Y.PointsValue,      1.0, "Row 1 Y");
            Assert.AreEqual(itemH + rowGap, row1.TotalBounds.Height.PointsValue, 1.0, "Row 1 height includes row gap");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void Grid_GapCSS_TwoValues_SetsColumnAndRowGapIndependently()
        {
            // gap: 15pt 25pt → row-gap = 15pt, column-gap = 25pt.
            // Grid: 600pt wide, 2 columns (1fr 1fr), 2 rows, items 50pt high.
            // Expected column width = (600 - 25) / 2 = 287.5pt.
            // Row 1 height = 50pt item + 15pt row gap = 65pt.
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <style>
    @page { size: 600pt 800pt; margin: 0; }
    body  { margin: 0; padding: 0; }
    .grid { display: grid; grid-template-columns: 1fr 1fr; gap: 15pt 25pt; width: 600pt; }
    .item { height: 50pt; border: 1pt solid black; }
  </style>
</head>
<body>
  <div class=""grid"">
    <div class=""item"">A</div>
    <div class=""item"">B</div>
    <div class=""item"">C</div>
    <div class=""item"">D</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            using (var ms = DocStreams.GetOutputStream("Grid_GapCSS_TwoValues.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            var row0       = GetRowBlock(gridBlock, 0);
            var row1       = GetRowBlock(gridBlock, 1);
            Assert.IsNotNull(row0, "Row 0");
            Assert.IsNotNull(row1, "Row 1");

            const double colGap = 25;
            const double rowGap = 15;
            const double itemH  = 50;

            // Column gap = 25pt → each column = (600 - 25) / 2 = 287.5pt
            double expectedColW = (PageW - colGap) / 2.0;
            Assert.AreEqual(expectedColW, row0.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column width with two-value gap (column portion)");

            // Row gap = 15pt → row 1 height = 50pt + 15pt
            Assert.AreEqual(itemH,          row1.TotalBounds.Y.PointsValue,      1.0, "Row 1 Y");
            Assert.AreEqual(itemH + rowGap, row1.TotalBounds.Height.PointsValue, 1.0, "Row 1 height includes row gap");
        }

        // ------------------------------------------------------------------
        // Grid_30 — page-overflow: row-spanning cell inner-block stretched
        // ------------------------------------------------------------------
        // Row 2 (E, F) overflows to page 2.  B spans rows 0-1, both on page 1.
        // StretchAllCellContent must search all grid blocks across pages so B's
        // inner div is expanded to the full 2-row combined height even when the
        // current page at layout-end is page 2.
        [TestCategory(TestCategory), TestMethod()]
        public void Grid_30_PageOverflow_RowSpanStretchedOnPriorPage()
        {
            const double rowGap = 10;
            const double itemH  = 50;
            // Row 0: A (cols 0-1, colSpan=2) + B (col 2, rowSpan=2)  = 50 pt
            // Row 1: C (col 0) + D (col 1)  + B continues             = 50 + 10 gap = 60 pt
            // Row 2: E (col 0) + F (col 1)                            = 50 + 10 gap = 60 pt
            // Page 130 pt: rows 0+1 = 110 pt fit; row 2 needs 60 > 20 remaining → overflow
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <style>
    @page { size: 600pt 130pt; margin: 0; }
    body  { margin: 0; padding: 0; }
    .grid { display: grid; grid-template-columns: 100pt 100pt 100pt; column-gap: 20pt; row-gap: 10pt; }
    .a    { grid-column: 1 / span 2; grid-row: 1; }
    .b    { grid-column: 3;          grid-row: 1 / span 2; }
    .c    { grid-column: 1;          grid-row: 2; }
    .d    { grid-column: 2;          grid-row: 2; }
    .item { height: 50pt; }
  </style>
</head>
<body>
  <div class=""grid"">
    <div class=""item a"">A</div>
    <div class=""item b"">B</div>
    <div class=""item c"">C</div>
    <div class=""item d"">D</div>
    <div class=""item e"">E</div>
    <div class=""item f"">F</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            using (var ms = DocStreams.GetOutputStream("Grid_30_PageOverflow_RowSpanStretched.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            Assert.AreEqual(2, _layout.AllPages.Count, "Row 2 must overflow to a second page");

            // Locate row 0 on page 1 via FindRowWithCols (3 columns = one per grid column).
            var page1Region = _layout.AllPages[0].ContentBlock.Columns[0];
            var row0        = FindRowWithCols(page1Region, 3);
            Assert.IsNotNull(row0, "Row 0 on page 1");

            // B is at column 2 (0-indexed) of row 0.
            var bCellBlock = GetItemBlock(row0, 2);
            Assert.IsNotNull(bCellBlock, "B GridCell block on page 1");

            // AdjustRowspanCellHeights must have set the cell to row0 + row1 height.
            double bExpectedH = itemH + itemH + rowGap; // 50 + 50 + 10 = 110 pt
            Assert.AreEqual(bExpectedH, bCellBlock.TotalBounds.Height.PointsValue, 2.0,
                "B cell block height == combined row 0+1 height");

            // StretchAllCellContent must have propagated that height to the inner div.
            var bInnerBlock = bCellBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(bInnerBlock, "B inner div block");
            Assert.AreEqual(bExpectedH, bInnerBlock.TotalBounds.Height.PointsValue, 2.0,
                "B inner div is stretched to fill the 2-row span (all-grids search)");
        }

        // ------------------------------------------------------------------
        // Grid_31 — page-overflow: first continuation row carries no gap
        // ------------------------------------------------------------------
        // Same layout: row 2 (E, F) overflows to page 2.  InjectRowGaps injects
        // a 10 pt top margin on rows 1+ before layout; ClearContinuationRowGaps
        // must strip it from the first row of each overflow page so no phantom
        // space appears at the top of the second page.
        [TestCategory(TestCategory), TestMethod()]
        public void Grid_31_PageOverflow_NoContinuationRowGap()
        {
            const double itemH  = 50;
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <style>
    @page { size: 600pt 130pt; margin: 0; }
    body  { margin: 0; padding: 0; }
    .grid { display: grid; grid-template-columns: 100pt 100pt 100pt; column-gap: 20pt; row-gap: 10pt; }
    .a    { grid-column: 1 / span 2; grid-row: 1; }
    .b    { grid-column: 3;          grid-row: 1 / span 2; }
    .c    { grid-column: 1;          grid-row: 2; }
    .d    { grid-column: 2;          grid-row: 2; }
    .item { height: 50pt; }
  </style>
</head>
<body>
  <div class=""grid"">
    <div class=""item a"">A</div>
    <div class=""item b"">B</div>
    <div class=""item c"">C</div>
    <div class=""item d"">D</div>
    <div class=""item e"">E</div>
    <div class=""item f"">F</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            using (var ms = DocStreams.GetOutputStream("Grid_31_PageOverflow_NoContinuationGap.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            Assert.AreEqual(2, _layout.AllPages.Count, "Row 2 must overflow to a second page");

            // The first (only) row on page 2 is E/F's row — find it via FindRowWithCols.
            var page2Region  = _layout.AllPages[1].ContentBlock.Columns[0];
            var row2OnPage2  = FindRowWithCols(page2Region, 3);
            Assert.IsNotNull(row2OnPage2, "E/F row block on page 2");

            // Height must equal itemH only — no row-gap margin.
            Assert.AreEqual(itemH, row2OnPage2.TotalBounds.Height.PointsValue, 2.0,
                "First row on continuation page must not include row-gap margin");

            // E's cell block (col 0) must have its top margin cleared.
            var eCellBlock = GetItemBlock(row2OnPage2, 0);
            Assert.IsNotNull(eCellBlock, "E GridCell block on page 2");
            double eTopMargin = eCellBlock.Position?.Margins.Top.PointsValue ?? 0.0;
            Assert.AreEqual(0.0, eTopMargin, 0.5,
                "E's row-gap margin must be zero on the continuation page");

            // The grid continuation block itself must also reflect the reduced height so
            // that content following the grid is positioned correctly.
            var gridBlock2 = GetGridBlock(page2Region);
            Assert.IsNotNull(gridBlock2, "Grid continuation block on page 2");
            Assert.AreEqual(itemH, gridBlock2.TotalBounds.Height.PointsValue, 2.0,
                "Grid continuation block height must not include the stripped row-gap");
        }
        // ------------------------------------------------------------------
        // Grid_32 — page-overflow: subsequent continuation rows Y-shifted
        // ------------------------------------------------------------------
        // When the first row of a continuation page has its gap stripped, every
        // later row on that same page was positioned by the layout engine relative
        // to the gap-inflated first row.  Their TotalBounds.Y must be shifted up
        // by the gap so they sit immediately after the corrected first row.
        [TestCategory(TestCategory), TestMethod()]
        public void Grid_32_PageOverflow_SubsequentRowsYShifted()
        {
            const double rowGap = 10;
            const double itemH  = 50;
            // 2-column grid, 4 rows of items.  Page = 120 pt.
            // Row 0: A B → 50 pt         (cumulative 50 pt)
            // Row 1: C D → 50+10 = 60 pt (cumulative 110 pt)
            // Row 2: E F → 60 pt needed, 10 pt available → OVERFLOW to page 2
            // Row 3: G H → also on page 2
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <style>
    @page { size: 600pt 120pt; margin: 0; }
    body  { margin: 0; padding: 0; }
    .grid { display: grid; grid-template-columns: 200pt 200pt; column-gap: 20pt; row-gap: 10pt; }
    .item { height: 50pt; }
  </style>
</head>
<body>
  <div class=""grid"">
    <div class=""item"">A</div>
    <div class=""item"">B</div>
    <div class=""item"">C</div>
    <div class=""item"">D</div>
    <div class=""item"">E</div>
    <div class=""item"">F</div>
    <div class=""item"">G</div>
    <div class=""item"">H</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            using (var ms = DocStreams.GetOutputStream("Grid_32_PageOverflow_YShift.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            Assert.AreEqual(2, _layout.AllPages.Count, "Rows 2+3 must overflow to a second page");

            var page2Region = _layout.AllPages[1].ContentBlock.Columns[0];
            var gridBlock2  = GetGridBlock(page2Region);
            Assert.IsNotNull(gridBlock2, "Grid continuation block on page 2");

            // Row 2 (E/F) — first continuation row, gap stripped.
            var rowEF = GetRowBlock(gridBlock2, 0);
            Assert.IsNotNull(rowEF, "E/F row block on page 2");
            Assert.AreEqual(0.0,   rowEF.TotalBounds.Y.PointsValue,      1.0, "Row EF starts at top of page 2");
            Assert.AreEqual(itemH, rowEF.TotalBounds.Height.PointsValue,  2.0, "Row EF height has no gap");

            // Row 3 (G/H) — second row on page 2, keeps its own gap but must be
            // repositioned immediately after the (now-shorter) first row.
            var rowGH = GetRowBlock(gridBlock2, 1);
            Assert.IsNotNull(rowGH, "G/H row block on page 2");
            Assert.AreEqual(itemH + rowGap, rowGH.TotalBounds.Height.PointsValue, 2.0,
                "Row GH height keeps its own row-gap");
            Assert.AreEqual(itemH, rowGH.TotalBounds.Y.PointsValue, 1.0,
                "Row GH Y immediately follows the gap-corrected first row (Y-shift fix)");
        }
        // ------------------------------------------------------------------
        // Grid_33 — page-overflow: sibling element after grid positioned correctly
        // ------------------------------------------------------------------
        // When the first continuation row's gap is stripped, the parent region's
        // UsedSize must also be reduced so that the next sibling (a div after the
        // grid) is laid out at the corrected Y, not 10pt too low.
        [TestCategory(TestCategory), TestMethod()]
        public void Grid_33_PageOverflow_SiblingAfterGridPositionedCorrectly()
        {
            const double rowGap  = 10;
            const double itemH   = 50;
            // 2-column grid, 4 rows, page height 150 pt.
            // Page 1: rows 0+1 = 110 pt; row 2 needs 60 but only 40 available → overflows.
            // Page 2: rows 2+3 (corrected = 50+60 = 110 pt), then the after-div.
            // Without the UsedSize fix the after-div would start at Y=120 pt.
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <style>
    @page { size: 600pt 150pt; margin: 0; }
    body  { margin: 0; padding: 0; }
    .grid  { display: grid; grid-template-columns: 200pt 200pt; column-gap: 20pt; row-gap: 10pt; }
    .item  { height: 50pt; }
    .after { height: 20pt; }
  </style>
</head>
<body>
  <div class=""grid"">
    <div class=""item"">A</div>
    <div class=""item"">B</div>
    <div class=""item"">C</div>
    <div class=""item"">D</div>
    <div class=""item"">E</div>
    <div class=""item"">F</div>
    <div class=""item"">G</div>
    <div class=""item"">H</div>
  </div>
  <div class=""after"">After Grid</div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            using (var ms = DocStreams.GetOutputStream("Grid_33_PageOverflow_SiblingY.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            Assert.AreEqual(2, _layout.AllPages.Count, "Content must span 2 pages");

            // Page 2 region contains: [0] = grid continuation, [1] = after-div.
            var page2Region = _layout.AllPages[1].ContentBlock.Columns[0];

            Assert.IsTrue(page2Region.Contents.Count >= 2, "Page 2 must have grid + after-div");
            var afterBlock = page2Region.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(afterBlock, "After-div block on page 2");

            // Corrected grid height on page 2: row 2 (50) + row 3 (50+10) = 110 pt.
            double expectedY = itemH + itemH + rowGap; // 110 pt
            Assert.AreEqual(expectedY, afterBlock.TotalBounds.Y.PointsValue, 2.0,
                "After-div must start immediately after the corrected grid (UsedSize fix)");
        }

        // ======================================================================
        // Named grid lines — [name] tokens in grid-template-columns/rows
        // ======================================================================

        // Grid_34 — item placed by named column lines using fr units (fills container cleanly).
        // grid-template-columns: [c1] 1fr [c2] 2fr [c3] 1fr [c4]
        // → col widths: 150pt / 300pt / 150pt (4fr → 600pt)
        // Item B-wide: grid-column: c2 / c4 → cols 1-2, width = 300+150 = 450pt.
        [TestCategory(TestCategory), TestMethod()]
        public void Grid_34_NamedColumnLines_PlacementByName()
        {
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <style>
    @page { size: 600pt 800pt; margin: 0; }
    body  { margin: 0; padding: 0; }
    .grid { display: grid;
            grid-template-columns: [c1] 1fr [c2] 2fr [c3] 1fr [c4];
            width: 600pt; }
    .item { height: 50pt; border: 1pt solid #888; padding: 4pt; }
    .wide { grid-column: c2 / c4; }
  </style>
</head>
<body>
  <div class=""grid"">
    <div class=""item"">A</div>
    <div class=""item wide"">B-wide</div>
    <div class=""item"">C</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            using (var ms = DocStreams.GetOutputStream("Grid_34_NamedColLines.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            Assert.IsNotNull(gridBlock, "Grid block");

            // Row 0: A in col 0 (150pt), B-wide in cols 1-2 (300+150=450pt)
            var row0 = GetRowBlock(gridBlock, 0);
            Assert.IsNotNull(row0, "Row 0");
            Assert.AreEqual(3, row0.Columns.Length, "Row 0: 3 column slots");

            // 4fr total → 1fr=150, 2fr=300, 1fr=150
            Assert.AreEqual(150.0, row0.Columns[0].TotalBounds.Width.PointsValue, 2.0,
                "Col 0 (c1→c2) = 1fr = 150pt");
            Assert.AreEqual(300.0, row0.Columns[1].TotalBounds.Width.PointsValue, 2.0,
                "Col 1 (c2→c3) = 2fr = 300pt");
            Assert.AreEqual(150.0, row0.Columns[2].TotalBounds.Width.PointsValue, 2.0,
                "Col 2 (c3→c4) = 1fr = 150pt");

            // B-wide spans cols 1-2 → 300+150 = 450pt
            var bBlock = GetItemBlock(row0, 1);
            Assert.IsNotNull(bBlock, "B-wide item block");
            Assert.AreEqual(450.0, bBlock.TotalBounds.Width.PointsValue, 2.0,
                "B-wide spans c2→c4 = 2fr+1fr = 450pt");

            StringAssert.Contains(CollectText(row0.Columns[0]), "A",      "Col 0 = A");
            StringAssert.Contains(CollectText(row0.Columns[1]), "B-wide", "Col 1 = B-wide");
        }

        // Grid_35 — item placed by named row lines (programmatic API).
        // grid-template-rows: [r1] 60pt [r2] 80pt [r3]
        // A-tall: RowStart=Named("r1"), RowEnd=Named("r3") → rowspan=2, height = 60+80 = 140pt.
        [TestCategory(TestCategory), TestMethod()]
        public void Grid_35_NamedRowLines_PlacementByName()
        {
            var doc = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr", templateRows: "[r1] 60pt [r2] 80pt [r3]");

            var aTall = AddItem(grid, "A-tall");
            aTall.Style.Grid.RowStart = GridLineValue.Named("r1");
            aTall.Style.Grid.RowEnd   = GridLineValue.Named("r3");

            AddItem(grid, "B");
            AddItem(grid, "C");

            using (var ms = DocStreams.GetOutputStream("Grid_35_NamedRowLines.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            Assert.IsNotNull(gridBlock, "Grid block");

            // Must produce 2 rows
            var row0 = GetRowBlock(gridBlock, 0);
            var row1 = GetRowBlock(gridBlock, 1);
            Assert.IsNotNull(row0, "Row 0 (60pt template)");
            Assert.IsNotNull(row1, "Row 1 (80pt template)");

            // Template row heights are injected via InjectRowHeights
            Assert.AreEqual(60.0, row0.TotalBounds.Height.PointsValue, 2.0, "Row 0 = 60pt");
            Assert.AreEqual(80.0, row1.TotalBounds.Height.PointsValue, 2.0, "Row 1 = 80pt");

            // A-tall spans r1→r3 (rowspan=2) → StretchAllCellContent gives it 60+80 = 140pt
            var aTallBlock = GetItemBlock(row0, 0);
            Assert.IsNotNull(aTallBlock, "A-tall block in row 0 col 0");
            Assert.AreEqual(140.0, aTallBlock.TotalBounds.Height.PointsValue, 2.0,
                "A-tall rowspan=2 → 60+80 = 140pt");

            // Content in correct cells
            StringAssert.Contains(CollectText(row0.Columns[0]), "A-tall", "Col 0 row 0 = A-tall");
            StringAssert.Contains(CollectText(row0.Columns[1]), "B",      "Col 1 row 0 = B");
            StringAssert.Contains(CollectText(row1.Columns[1]), "C",      "Col 1 row 1 = C");
        }

        // Grid_36 — explicit integer span combined with named [col-start] lines.
        // grid-template-columns: [col-start] 1fr [col-start] 1fr [col-start] 1fr [col-end]
        // → 3 equal 200pt columns.
        // Item Wide: grid-column: 1 / span 2 → 200+200 = 400pt.
        [TestCategory(TestCategory), TestMethod()]
        public void Grid_36_NamedColumnLines_SpanByCount()
        {
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <style>
    @page { size: 600pt 800pt; margin: 0; }
    body  { margin: 0; padding: 0; }
    .grid  { display: grid;
             grid-template-columns: [col-start] 1fr [col-start] 1fr [col-start] 1fr [col-end];
             width: 600pt; }
    .item  { height: 50pt; border: 1pt solid #888; }
    .span2 { grid-column: 1 / span 2; }
  </style>
</head>
<body>
  <div class=""grid"">
    <div class=""item span2"">Wide</div>
    <div class=""item"">Right</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            using (var ms = DocStreams.GetOutputStream("Grid_36_NamedLines_SpanCount.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var row0 = GetRowBlock(GetGridBlock(_layout.AllPages[0].ContentBlock.Columns[0]), 0);
            Assert.IsNotNull(row0, "Row 0");

            // 3 equal fr columns → 200pt each
            Assert.AreEqual(200.0, row0.Columns[0].TotalBounds.Width.PointsValue, 2.0,
                "Each col-start column = 1fr = 200pt");

            // Wide spans cols 0-1 = 200+200 = 400pt
            var wideBlock = GetItemBlock(row0, 0);
            Assert.IsNotNull(wideBlock, "Wide item block");
            Assert.AreEqual(400.0, wideBlock.TotalBounds.Width.PointsValue, 2.0,
                "1 / span 2 = 200+200 = 400pt");

            StringAssert.Contains(CollectText(row0.Columns[0]), "Wide",  "Col 0 = Wide");
            StringAssert.Contains(CollectText(row0.Columns[2]), "Right", "Col 2 = Right");
        }

        // ======================================================================
        // grid-template-areas — tested via programmatic API to validate the
        // layout engine independently of the HTML CSS cascade.
        // ======================================================================

        // Grid_37 — 2×2 template-areas: header spans 2 cols, sidebar and main fill row 2.
        [TestCategory(TestCategory), TestMethod()]
        public void Grid_37_TemplateAreas_TwoByTwo()
        {
            var doc = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr");

            // Set template areas programmatically
            GridTemplateAreasValue.TryParse("\"header header\" \"sidebar main\"", out var areas);
            grid.Style.Grid.TemplateAreas = areas;

            // Items — use style API so placement is independent of CSS cascade timing
            var header  = AddItem(grid, "Header",  height: 60, borderColor: new Color(0, 0, 200));
            header.Style.Grid.AreaName = "header";

            var sidebar = AddItem(grid, "Sidebar", height: 80, borderColor: new Color(0, 160, 0));
            sidebar.Style.Grid.AreaName = "sidebar";

            var main    = AddItem(grid, "Main",    height: 80, borderColor: new Color(160, 0, 0));
            main.Style.Grid.AreaName = "main";

            using (var ms = DocStreams.GetOutputStream("Grid_37_TemplateAreas_2x2.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            Assert.IsNotNull(gridBlock, "Grid block");

            var row0 = GetRowBlock(gridBlock, 0);
            var row1 = GetRowBlock(gridBlock, 1);
            Assert.IsNotNull(row0, "Row 0 (header)");
            Assert.IsNotNull(row1, "Row 1 (sidebar/main)");

            // Header spans both columns → 600pt wide
            var headerBlock = GetItemBlock(row0, 0);
            Assert.IsNotNull(headerBlock, "Header block");
            Assert.AreEqual(600.0, headerBlock.TotalBounds.Width.PointsValue, 2.0,
                "Header spans both cols = 600pt");
            Assert.AreEqual(60.0, headerBlock.TotalBounds.Height.PointsValue, 2.0,
                "Header height = 60pt");

            // Row 1: sidebar (300pt) and main (300pt)
            Assert.AreEqual(2, row1.Columns.Length, "Row 1 has 2 column slots");
            Assert.AreEqual(300.0, row1.Columns[0].TotalBounds.Width.PointsValue, 2.0,
                "Sidebar col = 300pt (1fr)");
            Assert.AreEqual(300.0, row1.Columns[1].TotalBounds.Width.PointsValue, 2.0,
                "Main col = 300pt (1fr)");

            StringAssert.Contains(CollectText(row0.Columns[0]), "Header",  "Row 0 = Header");
            StringAssert.Contains(CollectText(row1.Columns[0]), "Sidebar", "Row 1 col 0 = Sidebar");
            StringAssert.Contains(CollectText(row1.Columns[1]), "Main",    "Row 1 col 1 = Main");
        }

        // Grid_38 — 3-area layout with dot empty cell.
        // ". sidebar main" — middle row col 0 is empty.
        [TestCategory(TestCategory), TestMethod()]
        public void Grid_38_TemplateAreas_DotEmptyCell()
        {
            var doc = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr 1fr");

            GridTemplateAreasValue.TryParse(
                "\"header header header\" \". sidebar main\" \"footer footer footer\"",
                out var areas);
            grid.Style.Grid.TemplateAreas = areas;

            var header  = AddItem(grid, "Header",  height: 50, borderColor: new Color(0, 0, 200));
            header.Style.Grid.AreaName = "header";

            var sidebar = AddItem(grid, "Sidebar", height: 70, borderColor: new Color(0, 160, 0));
            sidebar.Style.Grid.AreaName = "sidebar";

            var main    = AddItem(grid, "Main",    height: 70, borderColor: new Color(160, 0, 0));
            main.Style.Grid.AreaName = "main";

            var footer  = AddItem(grid, "Footer",  height: 40, borderColor: new Color(100, 100, 100));
            footer.Style.Grid.AreaName = "footer";

            using (var ms = DocStreams.GetOutputStream("Grid_38_TemplateAreas_DotEmpty.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            Assert.IsNotNull(gridBlock, "Grid block");

            var row0 = GetRowBlock(gridBlock, 0);
            var row1 = GetRowBlock(gridBlock, 1);
            var row2 = GetRowBlock(gridBlock, 2);
            Assert.IsNotNull(row0, "Row 0 (header)");
            Assert.IsNotNull(row1, "Row 1 (. sidebar main)");
            Assert.IsNotNull(row2, "Row 2 (footer)");

            // Header: spans 3 columns → 600pt
            var headerBlock = GetItemBlock(row0, 0);
            Assert.IsNotNull(headerBlock, "Header block");
            Assert.AreEqual(600.0, headerBlock.TotalBounds.Width.PointsValue, 2.0,
                "Header spans 3 cols = 600pt");

            // Col 0 of row 1 is the empty dot cell — a placeholder exists but has no text
            var dotCellText = CollectText(row1.Columns[0]).Trim();
            Assert.AreEqual("", dotCellText, "Row 1 col 0 is the dot (empty) cell — no text content");

            StringAssert.Contains(CollectText(row1.Columns[1]), "Sidebar", "Row 1 col 1 = Sidebar");
            StringAssert.Contains(CollectText(row1.Columns[2]), "Main",    "Row 1 col 2 = Main");

            // Footer: spans 3 columns → 600pt
            var footerBlock = GetItemBlock(row2, 0);
            Assert.IsNotNull(footerBlock, "Footer block");
            Assert.AreEqual(600.0, footerBlock.TotalBounds.Width.PointsValue, 2.0,
                "Footer spans 3 cols = 600pt");

            StringAssert.Contains(CollectText(row0.Columns[0]), "Header", "Row 0 = Header");
            StringAssert.Contains(CollectText(row2.Columns[0]), "Footer", "Row 2 = Footer");
        }

        // Grid_39 — sidebar spanning 2 explicit rows via template-areas.
        // "sidebar top" / "sidebar bottom" — sidebar spans both rows, height = 100+100 = 200pt.
        [TestCategory(TestCategory), TestMethod()]
        public void Grid_39_TemplateAreas_SidebarSpansRows()
        {
            var doc = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "150pt 1fr", templateRows: "100pt 100pt");

            // sidebar spans both rows; top and bottom are the two content slots
            GridTemplateAreasValue.TryParse(
                "\"sidebar top\" \"sidebar bottom\"", out var areas);
            grid.Style.Grid.TemplateAreas = areas;

            var sidebar  = AddItem(grid, "Sidebar spans rows", height: 50, borderColor: new Color(0, 160, 0));
            sidebar.Style.Grid.AreaName = "sidebar";

            var top = AddItem(grid, "Content row 1", height: 50, borderColor: new Color(80, 80, 80));
            top.Style.Grid.AreaName = "top";

            var bottom = AddItem(grid, "Content row 2", height: 50, borderColor: new Color(80, 80, 80));
            bottom.Style.Grid.AreaName = "bottom";

            using (var ms = DocStreams.GetOutputStream("Grid_39_TemplateAreas_SidebarSpan.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            Assert.IsNotNull(gridBlock, "Grid block");

            var row0 = GetRowBlock(gridBlock, 0);
            Assert.IsNotNull(row0, "Row 0");
            Assert.AreEqual(2, row0.Columns.Length, "2 columns");

            // Fixed-width sidebar column: 150pt; content: 600-150 = 450pt
            Assert.AreEqual(150.0, row0.Columns[0].TotalBounds.Width.PointsValue, 2.0,
                "Sidebar col = 150pt");
            Assert.AreEqual(450.0, row0.Columns[1].TotalBounds.Width.PointsValue, 2.0,
                "Content col = 450pt (1fr)");

            // Row 0 = 100pt (from template-rows)
            Assert.AreEqual(100.0, row0.TotalBounds.Height.PointsValue, 2.0, "Row 0 = 100pt");

            // Sidebar spans 2 rows: StretchAllCellContent gives it 100+100 = 200pt
            var sidebarBlock = GetItemBlock(row0, 0);
            Assert.IsNotNull(sidebarBlock, "Sidebar block");
            Assert.AreEqual(200.0, sidebarBlock.TotalBounds.Height.PointsValue, 2.0,
                "Sidebar rowspan=2 → 100+100 = 200pt");

            StringAssert.Contains(CollectText(row0.Columns[0]), "Sidebar",       "Col 0 = sidebar");
            StringAssert.Contains(CollectText(row0.Columns[1]), "Content row 1", "Col 1 row 0 = top");

            var row1 = GetRowBlock(gridBlock, 1);
            Assert.IsNotNull(row1, "Row 1");
            Assert.AreEqual(100.0, row1.TotalBounds.Height.PointsValue, 2.0, "Row 1 = 100pt");
            StringAssert.Contains(CollectText(row1.Columns[1]), "Content row 2", "Col 1 row 1 = bottom");
        }

        // Grid_43 — grid-template-areas with CSS class selectors for grid-area.
        // Items carry class names matched by descendant CSS selectors that assign grid-area.
        // Reproduces the "all cells in one row" bug reported by users of CSS-driven layouts.
        [TestCategory(TestCategory), TestMethod()]
        public void Grid_43_TemplateAreas_CSSClassGridArea()
        {
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <style>
    @page { size: 600pt 800pt; margin: 0; }
    body  { margin: 0; padding: 0; }
    .grid {
        display: grid;
        grid-template-columns: 200pt 200pt 200pt;
        grid-template-areas:
           ""....... header  header""
           ""sidebar content content"";
        width: 600pt;
    }
    .item { height: 50pt; border: 1pt solid black; }
    .header  { grid-area: header; }
    .sidebar { grid-area: sidebar; }
    .content { grid-area: content; }
  </style>
</head>
<body>
  <div class=""grid"">
    <div class=""item header"">Header</div>
    <div class=""item sidebar"">Sidebar</div>
    <div class=""item content"">Content</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            using (var ms = DocStreams.GetOutputStream("Grid_43_TemplateAreas_CSSClass.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            Assert.IsNotNull(gridBlock, "Grid block");

            // Should have 2 rows: row 0 = [dot, header], row 1 = [sidebar, content]
            var row0 = GetRowBlock(gridBlock, 0);
            var row1 = GetRowBlock(gridBlock, 1);
            Assert.IsNotNull(row0, "Row 0 (dot + header)");
            Assert.IsNotNull(row1, "Row 1 (sidebar + content)");

            // Header spans cols 1-2 (200+200=400pt)
            StringAssert.Contains(CollectText(row0.Columns[1]), "Header",  "Row 0 col 1 = Header");
            var headerBlock = GetItemBlock(row0, 1);
            Assert.IsNotNull(headerBlock, "Header block");
            Assert.AreEqual(400.0, headerBlock.TotalBounds.Width.PointsValue, 2.0,
                "Header spans 2 cols = 400pt");

            // Sidebar at col 0 of row 1
            StringAssert.Contains(CollectText(row1.Columns[0]), "Sidebar", "Row 1 col 0 = Sidebar");
            // Content spans cols 1-2 of row 1
            StringAssert.Contains(CollectText(row1.Columns[1]), "Content", "Row 1 col 1 = Content");
        }

        // Grid_41 — grid-auto-columns: item placed at column 4 of a 3-column explicit grid.
        // The 4th column gets the auto-column width (100pt), and the cell at column 4 should
        // have width = 100pt.  Columns 1–3 remain at their explicit widths (200pt each).
        [TestCategory(TestCategory), TestMethod()]
        public void Grid_41_AutoColumns_ImplicitColumnGetsAutoWidth()
        {
            var doc = CreateDoc(out var pg);
            // Explicit 3-column grid: 200pt each (600pt page with no gap).
            var grid = CreateGrid(pg, "200pt 200pt 200pt");
            grid.Style.Grid.AutoColumns = "100pt";

            AddItem(grid, "A", height: 40); // col 1 (auto)
            AddItem(grid, "B", height: 40); // col 2 (auto)
            AddItem(grid, "C", height: 40); // col 3 (auto)

            // E is placed explicitly at the implicit 4th column.
            var e = AddItem(grid, "E", height: 40);
            e.Style.Grid.ColumnStart = GridLineValue.Line(4);
            e.Style.Grid.ColumnEnd   = GridLineValue.Line(5);

            using (var ms = DocStreams.GetOutputStream("Grid_41_AutoColumns.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            Assert.IsNotNull(gridBlock, "Grid block");

            // All items land in row 0 (A at col0, B at col1, C at col2, E at col3).
            var row0 = GetRowBlock(gridBlock, 0);
            Assert.IsNotNull(row0, "Row 0");
            Assert.AreEqual(4, row0.Columns.Length, "Row should have 4 columns (3 explicit + 1 implicit)");

            var aBlock = GetItemBlock(row0, 0);
            Assert.IsNotNull(aBlock, "A block");
            Assert.AreEqual(200.0, aBlock.TotalBounds.Width.PointsValue, 1.0, "A width = 200pt");

            var eBlock = GetItemBlock(row0, 3);
            Assert.IsNotNull(eBlock, "E block in implicit col 4");
            Assert.AreEqual(100.0, eBlock.TotalBounds.Width.PointsValue, 1.0, "E width = 100pt (auto-column)");
        }

        // Grid_42 — grid-auto-rows: implicit rows get the declared height.
        // Explicit template has 1 row at 80pt.  A second row is created by auto-flow;
        // grid-auto-rows: 50pt should give that row 50pt height.
        [TestCategory(TestCategory), TestMethod()]
        public void Grid_42_AutoRows_ImplicitRowGetsAutoHeight()
        {
            var doc = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr", templateRows: "80pt");
            grid.Style.Grid.AutoRows = "50pt";

            AddItem(grid, "A", height: 40); // row 0 col 0 (explicit 80pt row)
            AddItem(grid, "B", height: 40); // row 0 col 1
            AddItem(grid, "C", height: 30); // row 1 col 0 (implicit 50pt row)
            AddItem(grid, "D", height: 30); // row 1 col 1

            using (var ms = DocStreams.GetOutputStream("Grid_42_AutoRows.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            Assert.IsNotNull(gridBlock, "Grid block");

            var row0 = GetRowBlock(gridBlock, 0);
            Assert.IsNotNull(row0, "Row 0");
            var aBlock = GetItemBlock(row0, 0);
            Assert.IsNotNull(aBlock, "A block");
            Assert.AreEqual(80.0, aBlock.TotalBounds.Height.PointsValue, 2.0, "Row 0 height = 80pt (explicit track)");

            var row1 = GetRowBlock(gridBlock, 1);
            Assert.IsNotNull(row1, "Row 1");
            var cBlock = GetItemBlock(row1, 0);
            Assert.IsNotNull(cBlock, "C block");
            Assert.AreEqual(50.0, cBlock.TotalBounds.Height.PointsValue, 2.0, "Row 1 height = 50pt (auto-row track)");
        }

        // Grid_40 — implicit line names from grid-template-areas used in grid-column.
        // "header header header" → injects header-start (col line 1) and header-end (col line 4).
        // .hdr: grid-column: header-start / header-end → spans all 3 columns = 600pt.
        [TestCategory(TestCategory), TestMethod()]
        public void Grid_40_TemplateAreas_ImplicitLineNames()
        {
            var doc = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr 1fr");

            // Template areas generate implicit col line names: header-start=1, header-end=4
            GridTemplateAreasValue.TryParse(
                "\"header header header\" \"a b c\"", out var areas);
            grid.Style.Grid.TemplateAreas = areas;

            // Header item uses implicit line names header-start / header-end
            var hdr = AddItem(grid, "Full-width Header", height: 50, borderColor: new Color(0, 0, 200));
            hdr.Style.Grid.ColumnStart = GridLineValue.Named("header-start");
            hdr.Style.Grid.ColumnEnd   = GridLineValue.Named("header-end");

            AddItem(grid, "A", height: 60);
            AddItem(grid, "B", height: 60);
            AddItem(grid, "C", height: 60);

            using (var ms = DocStreams.GetOutputStream("Grid_40_ImplicitLineNames.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            Assert.IsNotNull(gridBlock, "Grid block");

            var row0 = GetRowBlock(gridBlock, 0);
            Assert.IsNotNull(row0, "Row 0 (header row)");

            // header-start → line 1, header-end → line 4 → spans all 3 cols = 600pt
            var hdrBlock = GetItemBlock(row0, 0);
            Assert.IsNotNull(hdrBlock, "Header block");
            Assert.AreEqual(600.0, hdrBlock.TotalBounds.Width.PointsValue, 2.0,
                "header-start/header-end = 3 cols = 600pt");
            Assert.AreEqual(50.0, hdrBlock.TotalBounds.Height.PointsValue, 2.0);

            var row1 = GetRowBlock(gridBlock, 1);
            Assert.IsNotNull(row1, "Row 1 (A, B, C)");
            Assert.AreEqual(3, row1.Columns.Length);
            StringAssert.Contains(CollectText(row1.Columns[0]), "A");
            StringAssert.Contains(CollectText(row1.Columns[1]), "B");
            StringAssert.Contains(CollectText(row1.Columns[2]), "C");
        }
    }
}
