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

            // Each column = (600 - 20) / 2 = 290pt
            double expectedW = (PageW - gap) / 2.0;
            Assert.AreEqual(expectedW, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column 0 width with gap");
            Assert.AreEqual(expectedW, rowBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Column 1 width with gap");

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
            itemA.Style.Grid.ColumnSpan = 2;

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
            itemA.Style.Grid.ColumnSpan = 2;

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
            itemA.Style.Grid.ColumnSpan = 1;

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
    }
}
