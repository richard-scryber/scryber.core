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
    public class FlexLayout_Tests
    {
        private const string TestCategory = "Layout-Flex";

        // Page and container sizes chosen to give clean integer column widths.
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

        private static Panel CreateFlexContainer(Page pg, FlexDirection dir = FlexDirection.Row, double width = PageW)
        {
            var panel = new Panel();
            panel.Style.Position.DisplayMode = DisplayMode.FlexBox;
            panel.Style.Flex.Direction       = dir;
            panel.Width = width;
            // Visible outer border so the container boundary is clear in the PDF.
            panel.Style.Border.LineStyle = LineType.Solid;
            panel.Style.Border.Width     = 1;
            panel.Style.Border.Color     = new Color(0, 0, 0);
            pg.Contents.Add(panel);
            return panel;
        }

        /// <summary>Adds a flex child Panel with an optional text label and border.</summary>
        private static Panel AddChild(Panel parent, double? height = null, double grow = 1.0,
                                      string label = null, Color? borderColor = null)
        {
            var child = new Panel();
            child.Style.Flex.Grow = grow;
            child.Style.Padding.All = 4;
            child.Style.Border.LineStyle = LineType.Solid;
            child.Style.Border.Width     = 1;
            child.Style.Border.Color     = borderColor ?? new Color(100, 100, 100);
            if (height.HasValue)
                child.Height = height.Value;
            if (!string.IsNullOrEmpty(label))
                child.Contents.Add(new Label { Text = label });
            parent.Contents.Add(child);
            return child;
        }

        private static PDFLayoutBlock FindBlockOwner(PDFLayoutRegion region, Component owner)
        {
            foreach (var item in region.Contents)
            {
                if (item is PDFLayoutBlock b && b.Owner == owner)
                    return b;
            }
            return null;
        }

        private static PDFLayoutBlock FindFlexBlock(PDFLayoutRegion region)
        {
            foreach (var item in region.Contents)
            {
                if (item is PDFLayoutBlock b && b.Columns.Length > 1)
                    return b;
                if (item is PDFLayoutBlock nested)
                {
                    foreach (var col in nested.Columns)
                    {
                        var found = FindFlexBlock(col);
                        if (found != null) return found;
                    }
                }
            }
            return null;
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
        // FlexRow — basic two-child layout
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_TwoChildren_SideBySide()
        {
            var doc    = CreateDoc(out var pg);
            var panel  = CreateFlexContainer(pg);
            var child0 = AddChild(panel, height: 50, grow: 1, label: "Col 0");
            var child1 = AddChild(panel, height: 50, grow: 1, label: "Col 1");

            using (var ms = DocStreams.GetOutputStream("Flex_Row_TwoChildren_Equal.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);

            var lpg        = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var panelBlock = pageRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock, "Panel layout block should not be null");

            // Flex row creates one column per child.
            Assert.AreEqual(2, panelBlock.Columns.Length, "Flex row with 2 children should produce 2 columns");

            var col0 = panelBlock.Columns[0];
            var col1 = panelBlock.Columns[1];

            // Each column should be half the container width.
            double expected = PageW / 2.0;
            Assert.AreEqual(expected, col0.TotalBounds.Width.PointsValue, 0.5, "Column 0 width should be half the container");
            Assert.AreEqual(expected, col1.TotalBounds.Width.PointsValue, 0.5, "Column 1 width should be half the container");

            // Column 0 starts at X=0; column 1 starts immediately after.
            Assert.AreEqual(0.0,      col0.TotalBounds.X.PointsValue, 0.5, "Column 0 should start at X=0");
            Assert.AreEqual(expected, col1.TotalBounds.X.PointsValue, 0.5, "Column 1 should start at X=half-width");

            // Each column should contain the matching child's layout block.
            Assert.IsNotNull(FindBlockOwner(col0, child0), "Column 0 should contain child0's layout block");
            Assert.IsNotNull(FindBlockOwner(col1, child1), "Column 1 should contain child1's layout block");

            // Each column should contain its label text.
            StringAssert.Contains(CollectText(col0), "Col 0", "Column 0 should contain label text");
            StringAssert.Contains(CollectText(col1), "Col 1", "Column 1 should contain label text");
        }

        // -----------------------------------------------------------------------
        // FlexRow — gap between children
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_Gap_ReducesColumnWidths()
        {
            const double gap = 20.0;

            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.Gap = new Unit(gap, PageUnits.Points);
            AddChild(panel, height: 50, grow: 1, label: "A");
            AddChild(panel, height: 50, grow: 1, label: "B");

            using (var ms = DocStreams.GetOutputStream("Flex_Row_Gap.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(2, panelBlock.Columns.Length, "Gap test should still produce 2 columns");

            // With one alley of 20pt, available = 600-20 = 580; each column = 290.
            double expected = (PageW - gap) / 2.0;
            Assert.AreEqual(expected, panelBlock.Columns[0].TotalBounds.Width.PointsValue, 0.5, "Column 0 width with gap");
            Assert.AreEqual(expected, panelBlock.Columns[1].TotalBounds.Width.PointsValue, 0.5, "Column 1 width with gap");

            // Text still present on each side of the gap.
            StringAssert.Contains(CollectText(panelBlock.Columns[0]), "A", "Column 0 text");
            StringAssert.Contains(CollectText(panelBlock.Columns[1]), "B", "Column 1 text");
        }

        // -----------------------------------------------------------------------
        // FlexRow — proportional grow
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_GrowRatio_DistributesSpace()
        {
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            AddChild(panel, height: 50, grow: 1, label: "Narrow");
            AddChild(panel, height: 50, grow: 2, label: "Wide");

            using (var ms = DocStreams.GetOutputStream("Flex_Row_GrowRatio.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(2, panelBlock.Columns.Length);

            // grow=1 → 1/3 of 600 = 200; grow=2 → 2/3 of 600 = 400.
            Assert.AreEqual(200.0, panelBlock.Columns[0].TotalBounds.Width.PointsValue, 0.5, "Column 0 (grow=1) should be 200pt");
            Assert.AreEqual(400.0, panelBlock.Columns[1].TotalBounds.Width.PointsValue, 0.5, "Column 1 (grow=2) should be 400pt");

            StringAssert.Contains(CollectText(panelBlock.Columns[0]), "Narrow", "Narrow column text");
            StringAssert.Contains(CollectText(panelBlock.Columns[1]), "Wide",   "Wide column text");
        }

        // -----------------------------------------------------------------------
        // FlexRow — three children
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_ThreeChildren_EqualWidth()
        {
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            AddChild(panel, height: 50, label: "One");
            AddChild(panel, height: 50, label: "Two");
            AddChild(panel, height: 50, label: "Three");

            using (var ms = DocStreams.GetOutputStream("Flex_Row_ThreeChildren.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);

            Assert.AreEqual(3, panelBlock.Columns.Length, "Three children should produce three columns");

            double expected = PageW / 3.0;
            string[] labels = { "One", "Two", "Three" };
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(expected, panelBlock.Columns[i].TotalBounds.Width.PointsValue, 0.5,
                    $"Column {i} should be 200pt");
                StringAssert.Contains(CollectText(panelBlock.Columns[i]), labels[i],
                    $"Column {i} should contain its label");
            }
        }

        // -----------------------------------------------------------------------
        // FlexColumn — children stacked vertically
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexColumn_TwoChildren_Stacked()
        {
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg, FlexDirection.Column);
            AddChild(panel, height: 50, label: "Row A");
            AddChild(panel, height: 80, label: "Row B");

            using (var ms = DocStreams.GetOutputStream("Flex_Column_TwoChildren.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);

            // Column direction uses a single column (standard block stacking).
            Assert.AreEqual(1, panelBlock.Columns.Length, "Flex column direction should have a single region");

            var region = panelBlock.Columns[0];
            var block0 = region.Contents[0] as PDFLayoutBlock;
            var block1 = region.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block0, "First child block should not be null");
            Assert.IsNotNull(block1, "Second child block should not be null");

            // Second child should start below the first.
            Assert.IsTrue(block1.TotalBounds.Y > block0.TotalBounds.Y,
                "Second child Y should be greater than first child Y");

            // First child is 50pt tall (explicit height).
            Assert.AreEqual(50.0, block0.TotalBounds.Height.PointsValue, 0.5,
                "First child height should be 50pt");

            // Text content present in each stacked child.
            StringAssert.Contains(CollectText(block0.Columns[0]), "Row A", "First child text");
            StringAssert.Contains(CollectText(block1.Columns[0]), "Row B", "Second child text");
        }

        // -----------------------------------------------------------------------
        // FlexColumn — children fill full width
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexColumn_Children_FullWidth()
        {
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg, FlexDirection.Column);
            AddChild(panel, height: 50, label: "Full A");
            AddChild(panel, height: 50, label: "Full B");

            using (var ms = DocStreams.GetOutputStream("Flex_Column_FullWidth.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);

            Assert.AreEqual(1, panelBlock.Columns.Length, "Flex column should have one region");

            // The column region itself should span the full container width.
            var colRegion = panelBlock.Columns[0];
            Assert.AreEqual(PageW, colRegion.TotalBounds.Width.PointsValue, 0.5,
                "Flex-column region should span the full container width");

            // Both labels present.
            var allText = CollectText(colRegion);
            StringAssert.Contains(allText, "Full A", "First child label");
            StringAssert.Contains(allText, "Full B", "Second child label");
        }

        // -----------------------------------------------------------------------
        // FlexRow — default grow=1 when not explicitly set
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_DefaultGrow_EqualColumns()
        {
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);

            // Add children without explicitly setting grow (default = 1)
            var child0 = new Panel();
            child0.Height = 50;
            child0.Style.Padding.All = 4;
            child0.Style.Border.LineStyle = LineType.Solid;
            child0.Style.Border.Width     = 1;
            child0.Style.Border.Color     = new Color(100, 100, 100);
            child0.Contents.Add(new Label { Text = "Default A" });
            panel.Contents.Add(child0);

            var child1 = new Panel();
            child1.Height = 50;
            child1.Style.Padding.All = 4;
            child1.Style.Border.LineStyle = LineType.Solid;
            child1.Style.Border.Width     = 1;
            child1.Style.Border.Color     = new Color(100, 100, 100);
            child1.Contents.Add(new Label { Text = "Default B" });
            panel.Contents.Add(child1);

            using (var ms = DocStreams.GetOutputStream("Flex_Row_DefaultGrow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(2, panelBlock.Columns.Length, "Two children without grow should still produce 2 equal columns");

            double expected = PageW / 2.0;
            Assert.AreEqual(expected, panelBlock.Columns[0].TotalBounds.Width.PointsValue, 0.5, "Column 0 equal width");
            Assert.AreEqual(expected, panelBlock.Columns[1].TotalBounds.Width.PointsValue, 0.5, "Column 1 equal width");

            StringAssert.Contains(CollectText(panelBlock.Columns[0]), "Default A", "Column 0 text");
            StringAssert.Contains(CollectText(panelBlock.Columns[1]), "Default B", "Column 1 text");
        }

        // -----------------------------------------------------------------------
        // FlexRow — display:flex recognised from CSS string
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_CSSParsed_DisplayFlex()
        {
            // Build the document from inline CSS to verify the CSS parser path.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-direction:row; width:600pt; border:1pt solid #000;"">
    <div style=""flex-grow:1; height:50pt; border:1pt solid #888; padding:4pt;"">CSS Left</div>
    <div style=""flex-grow:1; height:50pt; border:1pt solid #888; padding:4pt;"">CSS Right</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc, "Parsed document should not be null");

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_CSS_DisplayFlex.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout should not be null after CSS parse");
            Assert.AreEqual(1, layout.AllPages.Count);

            // Recursively find the flex container block (has 2 columns for the 2 flex children).
            var contentBlock = layout.AllPages[0].ContentBlock;
            var flexBlock = FindFlexBlock(contentBlock.Columns[0]);

            Assert.IsNotNull(flexBlock, "Should find a block with 2 columns (flex row) in the layout tree");
            Assert.AreEqual(2, flexBlock.Columns.Length, "CSS flex-direction:row should produce 2 columns");

            // Both text strings present in their respective columns.
            StringAssert.Contains(CollectText(flexBlock.Columns[0]), "CSS Left",  "Left column text");
            StringAssert.Contains(CollectText(flexBlock.Columns[1]), "CSS Right", "Right column text");
        }

        // -----------------------------------------------------------------------
        // FlexRow — empty container does not throw
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_EmptyContainer_DoesNotThrow()
        {
            var doc   = CreateDoc(out var pg);
            var panel = new Panel();
            panel.Style.Position.DisplayMode = DisplayMode.FlexBox;
            panel.Width  = PageW;
            panel.Height = 50;
            panel.Style.Border.LineStyle = LineType.Solid;
            panel.Style.Border.Width     = 1;
            panel.Style.Border.Color     = new Color(0, 0, 0);
            pg.Contents.Add(panel);

            using (var ms = DocStreams.GetOutputStream("Flex_Row_Empty.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should complete without throwing for empty flex container");
            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock, "Empty flex container should still produce a layout block");
        }

        // -----------------------------------------------------------------------
        // FlexRow — single child takes full width
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_SingleChild_FullWidth()
        {
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            AddChild(panel, height: 50, grow: 1, label: "Only Child");

            using (var ms = DocStreams.GetOutputStream("Flex_Row_SingleChild.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(1, panelBlock.Columns.Length, "Single child should produce one column");
            Assert.AreEqual(PageW, panelBlock.Columns[0].TotalBounds.Width.PointsValue, 0.5,
                "Single-child flex row column should span the full container width");

            StringAssert.Contains(CollectText(panelBlock.Columns[0]), "Only Child", "Single child text");
        }
    }
}
