using System.Collections.Generic;
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
    public class GridLayout_Tests
    {
        private const string TestCategory = "Layout-Grid";

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

        private static Panel CreateGrid(Page pg, string templateColumns, double width = PageW)
        {
            var panel = new Panel();
            panel.Style.Position.DisplayMode = DisplayMode.FlexGrid;
            panel.Style.Grid.TemplateColumns  = templateColumns;
            panel.Width = width;
            panel.Style.Border.LineStyle = LineType.Solid;
            panel.Style.Border.Width     = 1;
            panel.Style.Border.Color     = new Color(0, 0, 0);
            pg.Contents.Add(panel);
            return panel;
        }

        private static Panel AddItem(Panel grid, string label = null, double? height = null)
        {
            var item = new Panel();
            item.Style.Padding.All = 4;
            item.Style.Border.LineStyle = LineType.Solid;
            item.Style.Border.Width     = 1;
            item.Style.Border.Color     = new Color(100, 100, 100);
            if (height.HasValue)
                item.Height = height.Value;
            if (!string.IsNullOrEmpty(label))
                item.Contents.Add(new Label { Text = label });
            grid.Contents.Add(item);
            return item;
        }

        private static PDFLayoutBlock GetGridBlock(PDFLayoutRegion pageRegion)
            => pageRegion.Contents[0] as PDFLayoutBlock;

        private static PDFLayoutBlock GetRowBlock(PDFLayoutBlock gridBlock, int rowIndex)
            => gridBlock.Columns[0].Contents[rowIndex] as PDFLayoutBlock;

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
        // Grid_TwoColumns_EqualWidth — 1fr 1fr
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Grid_TwoColumns_EqualWidth()
        {
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr");
            AddItem(grid, label: "A", height: 50);
            AddItem(grid, label: "B", height: 50);

            using (var ms = DocStreams.GetOutputStream("Grid_TwoColumns_Equal.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock  = GetGridBlock(pageRegion);
            Assert.IsNotNull(gridBlock, "Grid block should exist");

            var rowBlock = GetRowBlock(gridBlock, 0);
            Assert.IsNotNull(rowBlock, "Row block should exist");
            Assert.AreEqual(2, rowBlock.Columns.Length, "1fr 1fr should produce 2 columns");

            double expected = PageW / 2.0;
            Assert.AreEqual(expected, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column 0 should be half the grid width");
            Assert.AreEqual(expected, rowBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Column 1 should be half the grid width");

            StringAssert.Contains(CollectText(rowBlock.Columns[0]), "A", "Column 0 text");
            StringAssert.Contains(CollectText(rowBlock.Columns[1]), "B", "Column 1 text");
        }

        // -----------------------------------------------------------------------
        // Grid_TwoColumns_Proportional — 1fr 2fr
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Grid_TwoColumns_Proportional()
        {
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 2fr");
            AddItem(grid, label: "Narrow", height: 50);
            AddItem(grid, label: "Wide", height: 50);

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

            double col0Expected = PageW / 3.0;       // 1fr of 3fr total
            double col1Expected = PageW * 2.0 / 3.0; // 2fr of 3fr total
            Assert.AreEqual(col0Expected, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column 0 (1fr) should be 1/3 of width");
            Assert.AreEqual(col1Expected, rowBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Column 1 (2fr) should be 2/3 of width");
        }

        // -----------------------------------------------------------------------
        // Grid_AutoFlow_FillsRowByRow — 4 items in 2-column grid = 2 rows
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Grid_AutoFlow_FillsRowByRow()
        {
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr");
            for (int i = 0; i < 4; i++)
                AddItem(grid, label: $"Item{i}", height: 40);

            using (var ms = DocStreams.GetOutputStream("Grid_AutoFlow_TwoRows.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion  = _layout.AllPages[0].ContentBlock.Columns[0];
            var gridBlock   = GetGridBlock(pageRegion);
            var tableRegion = gridBlock.Columns[0];

            Assert.AreEqual(2, tableRegion.Contents.Count, "4 items in 2-col grid should create 2 row blocks");

            var row0 = tableRegion.Contents[0] as PDFLayoutBlock;
            var row1 = tableRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(row0, "Row 0 block");
            Assert.IsNotNull(row1, "Row 1 block");

            Assert.AreEqual(2, row0.Columns.Length, "Row 0 should have 2 columns");
            Assert.AreEqual(2, row1.Columns.Length, "Row 1 should have 2 columns");

            StringAssert.Contains(CollectText(row0.Columns[0]), "Item0");
            StringAssert.Contains(CollectText(row0.Columns[1]), "Item1");
            StringAssert.Contains(CollectText(row1.Columns[0]), "Item2");
            StringAssert.Contains(CollectText(row1.Columns[1]), "Item3");
        }

        // -----------------------------------------------------------------------
        // Grid_ThreeColumns_EqualWidth — repeat(3, 1fr)
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Grid_Repeat_ExpandsColumns()
        {
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "repeat(3, 1fr)");
            AddItem(grid, label: "X", height: 50);
            AddItem(grid, label: "Y", height: 50);
            AddItem(grid, label: "Z", height: 50);

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
        }

        // -----------------------------------------------------------------------
        // Grid_Gap_CorrectSpacing — column-gap reduces available space
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Grid_Gap_CorrectSpacing()
        {
            const double gap = 20;
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr");
            grid.Style.Flex.ColumnGap = gap;
            AddItem(grid, label: "L", height: 50);
            AddItem(grid, label: "R", height: 50);

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

            // Each column gets (600 - 20) / 2 = 290 pt
            double expected = (PageW - gap) / 2.0;
            Assert.AreEqual(expected, rowBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column 0 width with gap");
            Assert.AreEqual(expected, rowBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Column 1 width with gap");
        }

        // -----------------------------------------------------------------------
        // Grid_CSSParsed_DisplayGrid — grid-template-columns from CSS string
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Grid_CSSParsed_DisplayGrid()
        {
            // Build via HTML so the CSS parser is exercised.
            const string html = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:grid; grid-template-columns: 1fr 1fr 1fr; width:600pt;"">
    <div style=""height:40pt;"">P</div>
    <div style=""height:40pt;"">Q</div>
    <div style=""height:40pt;"">R</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(html), ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Grid_CSSParsed.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var contentBlock = layout.AllPages[0].ContentBlock;
            // Find the first row block with exactly 3 columns.
            var rowBlock = FindRowWithCols(contentBlock.Columns[0], 3);
            Assert.IsNotNull(rowBlock, "CSS parsed display:grid should produce a row block with 3 columns");
            Assert.AreEqual(3, rowBlock.Columns.Length, "CSS parsed 3-column grid should have 3 columns");
        }

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
        // Grid_Empty_DoesNotThrow
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Grid_Empty_DoesNotThrow()
        {
            var doc  = CreateDoc(out var pg);
            var grid = CreateGrid(pg, "1fr 1fr");
            // No children added.

            using (var ms = DocStreams.GetOutputStream("Grid_Empty.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should complete without throwing");
        }
    }
}
