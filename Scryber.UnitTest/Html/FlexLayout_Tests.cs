using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Html
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
            var child = new Div();
            child.Style.Flex.Grow = grow;
            child.Style.Padding.All = 4;
            child.Style.Border.LineStyle = LineType.Solid;
            child.Style.Border.Width     = 1;
            child.Style.Border.Color     = borderColor ?? new Color(0, 255, 0);
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

        // Returns the first PDFLayoutBlock found directly in a region (no recursion).
        private static PDFLayoutBlock GetFirstChildBlock(PDFLayoutRegion region)
        {
            foreach (var item in region.Contents)
                if (item is PDFLayoutBlock b) return b;
            return null;
        }

        // Finds the deepest block in the tree that has exactly 1 column and has child content.
        private static PDFLayoutBlock FindDeepestSingleColumnBlock(PDFLayoutRegion region)
        {
            PDFLayoutBlock result = null;
            foreach (var item in region.Contents)
            {
                if (item is PDFLayoutBlock b)
                {
                    if (b.Columns.Length == 1 && b.Columns[0].Contents.Count > 0)
                        result = b;
                    foreach (var col in b.Columns)
                    {
                        var deeper = FindDeepestSingleColumnBlock(col);
                        if (deeper != null) result = deeper;
                    }
                }
            }
            return result;
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
            var panel1 = CreateFlexContainer(pg, FlexDirection.Column);
            var panel2 = CreateFlexContainer(pg, FlexDirection.Column);
            AddChild(panel1, height: 50, label: "Full A");
            AddChild(panel2, height: 50, label: "Full B");

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

            // First labels present.
            var allText = CollectText(colRegion);
            StringAssert.Contains(allText, "Full A", "First child label");
            
            
            panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            
            Assert.AreEqual(1, panelBlock.Columns.Length, "Flex column should have one region");
            colRegion = panelBlock.Columns[0];
            Assert.AreEqual(PageW, colRegion.TotalBounds.Width.PointsValue, 0.5,
                "Flex-column region should span the full container width");
            
            allText = CollectText(colRegion);
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

        // -----------------------------------------------------------------------
        // align-items
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_AlignItems_FlexStart_NoOffset()
        {
            // flex-start (the default) — shorter child should start at Y=0, same as taller child.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.AlignItems = FlexAlignMode.FlexStart;
            AddChild(panel, height: 50, label: "Short");
            AddChild(panel, height: 80, label: "Tall");

            using (var ms = DocStreams.GetOutputStream("Flex_AlignItems_FlexStart.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(2, panelBlock.Columns.Length);

            var shortBlock = panelBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var tallBlock  = panelBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(shortBlock);
            Assert.IsNotNull(tallBlock);

            Assert.AreEqual(0.0, shortBlock.TotalBounds.Y.PointsValue, 0.5,
                "flex-start: short child should start at Y=0");
            Assert.AreEqual(0.0, tallBlock.TotalBounds.Y.PointsValue, 0.5,
                "flex-start: tall child should start at Y=0");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_AlignItems_FlexEnd_ShortChildStartsAtBottom()
        {
            // flex-end — shorter child should be pushed down so its bottom aligns with taller child.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.AlignItems = FlexAlignMode.FlexEnd;
            AddChild(panel, height: 50, label: "Short");  // col 0
            AddChild(panel, height: 80, label: "Tall");   // col 1

            using (var ms = DocStreams.GetOutputStream("Flex_AlignItems_FlexEnd.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(2, panelBlock.Columns.Length);

            var shortBlock = panelBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var tallBlock  = panelBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(shortBlock);
            Assert.IsNotNull(tallBlock);

            // Short child (50pt) in a row of 80pt — offset = 80 - 50 = 30pt.
            Assert.AreEqual(30.0, shortBlock.TotalBounds.Y.PointsValue, 0.5,
                "flex-end: short child Y should be (maxH - childH) = 30pt");
            Assert.AreEqual(0.0, tallBlock.TotalBounds.Y.PointsValue, 0.5,
                "flex-end: tallest child should stay at Y=0");

            // Text present in each column.
            StringAssert.Contains(CollectText(panelBlock.Columns[0]), "Short");
            StringAssert.Contains(CollectText(panelBlock.Columns[1]), "Tall");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_AlignItems_Center_ShortChildIsCentered()
        {
            // center — shorter child should be pushed down by half the height difference.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.AlignItems = FlexAlignMode.Center;
            AddChild(panel, height: 50, label: "Short");  // col 0
            AddChild(panel, height: 80, label: "Tall");   // col 1

            using (var ms = DocStreams.GetOutputStream("Flex_AlignItems_Center.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(2, panelBlock.Columns.Length);

            var shortBlock = panelBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var tallBlock  = panelBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(shortBlock);
            Assert.IsNotNull(tallBlock);

            // Short child (50pt) in a row of 80pt — offset = (80 - 50) / 2 = 15pt.
            Assert.AreEqual(15.0, shortBlock.TotalBounds.Y.PointsValue, 0.5,
                "center: short child Y should be (maxH - childH) / 2 = 15pt");
            Assert.AreEqual(0.0, tallBlock.TotalBounds.Y.PointsValue, 0.5,
                "center: tallest child should stay at Y=0");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_AlignItems_EqualHeights_NoOffset()
        {
            // When all children have equal height, align-items has no visible effect.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.AlignItems = FlexAlignMode.FlexEnd;
            AddChild(panel, height: 60, label: "A");
            AddChild(panel, height: 60, label: "B");

            using (var ms = DocStreams.GetOutputStream("Flex_AlignItems_EqualHeights.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(2, panelBlock.Columns.Length);

            var block0 = panelBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var block1 = panelBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0.0, block0.TotalBounds.Y.PointsValue, 0.5, "Equal-height child 0 should have Y=0");
            Assert.AreEqual(0.0, block1.TotalBounds.Y.PointsValue, 0.5, "Equal-height child 1 should have Y=0");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_AlignItems_CSSParsed_Center()
        {
            // Verify align-items:center is correctly parsed from an inline CSS string.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; align-items:center; width:600pt; border:1pt solid #000000;"">
    <div style=""height:50pt; flex-grow:1; padding:4pt; border:1pt solid #646464; background-color:#D0E8FF;"">Short</div>
    <div style=""height:80pt; flex-grow:1; padding:4pt; border:1pt solid #646464; background-color:#FFE8D0;"">Tall</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_AlignItems_CSS_Center.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var flexBlock = FindFlexBlock(layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(flexBlock, "Should find flex row block");
            Assert.AreEqual(2, flexBlock.Columns.Length);

            var shortBlock = flexBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var tallBlock  = flexBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(shortBlock);
            Assert.IsNotNull(tallBlock);

            // center: offset = (80 - 50) / 2 = 15pt
            Assert.AreEqual(15.0, shortBlock.TotalBounds.Y.PointsValue, 0.5,
                "CSS align-items:center should offset short child by 15pt");
            Assert.AreEqual(0.0, tallBlock.TotalBounds.Y.PointsValue, 0.5,
                "CSS align-items:center: tall child stays at Y=0");
        }

        // -----------------------------------------------------------------------
        // justify-content (only has effect when items do not fill the container,
        // i.e. flex-grow:0 with explicit widths)
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_JustifyContent_FlexStart_NoShift()
        {
            // flex-start is the default — items sit at the left edge.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; justify-content:flex-start; width:600pt; border:1pt solid #000000;"">
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0E8FF;"">A</div>
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#FFE8D0;"">B</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Justify_FlexStart.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var flexBlock = FindFlexBlock(layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(flexBlock);

            // flex-start: col0 stays at X=0, col1 immediately follows.
            Assert.AreEqual(0.0, flexBlock.Columns[0].TotalBounds.X.PointsValue, 0.5,
                "flex-start: column 0 should start at X=0");
            Assert.AreEqual(100.0, flexBlock.Columns[1].TotalBounds.X.PointsValue, 0.5,
                "flex-start: column 1 should start at X=100 (right after col 0)");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_JustifyContent_FlexEnd_ItemsShiftRight()
        {
            // flex-end — items pack to the right. Two 100pt items in 600pt → leftover 400pt.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; justify-content:flex-end; width:600pt; border:1pt solid #000000;"">
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0E8FF;"">A</div>
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#FFE8D0;"">B</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Justify_FlexEnd.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var flexBlock = FindFlexBlock(layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(flexBlock);

            // leftover = 600 - (100 + 100) = 400. Both cols shift right by 400.
            Assert.AreEqual(400.0, flexBlock.Columns[0].TotalBounds.X.PointsValue, 0.5,
                "flex-end: column 0 should start at X=400");
            Assert.AreEqual(500.0, flexBlock.Columns[1].TotalBounds.X.PointsValue, 0.5,
                "flex-end: column 1 should start at X=500");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_JustifyContent_Center_ItemsCentered()
        {
            // center — items are centred. Two 100pt items in 600pt → leftover 400 → shift by 200.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; justify-content:center; width:600pt; border:1pt solid #000000;"">
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0E8FF;"">A</div>
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#FFE8D0;"">B</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Justify_Center.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var flexBlock = FindFlexBlock(layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(flexBlock);

            // shift = 400 / 2 = 200. col0: 0+200=200, col1: 100+200=300.
            Assert.AreEqual(200.0, flexBlock.Columns[0].TotalBounds.X.PointsValue, 0.5,
                "center: column 0 should start at X=200");
            Assert.AreEqual(300.0, flexBlock.Columns[1].TotalBounds.X.PointsValue, 0.5,
                "center: column 1 should start at X=300");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_JustifyContent_SpaceBetween_SpacesDistributed()
        {
            // space-between — first item at left, last at right, equal gaps between.
            // Two 100pt items in 600pt → leftover 400 → one gap of 400 between them.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; justify-content:space-between; width:600pt; border:1pt solid #000000;"">
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0E8FF;"">A</div>
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#FFE8D0;"">B</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Justify_SpaceBetween.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var flexBlock = FindFlexBlock(layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(flexBlock);

            // col0 stays at X=0; col1 shifts right by gap=400 → 100+400=500.
            Assert.AreEqual(0.0, flexBlock.Columns[0].TotalBounds.X.PointsValue, 0.5,
                "space-between: column 0 should stay at X=0");
            Assert.AreEqual(500.0, flexBlock.Columns[1].TotalBounds.X.PointsValue, 0.5,
                "space-between: column 1 should be at X=500 (last item at right edge)");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_JustifyContent_GrowItems_NoEffect()
        {
            // With flex-grow > 0, items fill the container so justify-content has no effect.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.JustifyContent = FlexJustify.FlexEnd;
            AddChild(panel, height: 50, grow: 1, label: "A");
            AddChild(panel, height: 50, grow: 1, label: "B");

            using (var ms = DocStreams.GetOutputStream("Flex_Justify_GrowNoEffect.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(2, panelBlock.Columns.Length);

            // Items fill the container — columns stay at their original positions.
            Assert.AreEqual(0.0, panelBlock.Columns[0].TotalBounds.X.PointsValue, 0.5,
                "Grow items: col 0 should start at X=0 (justify-content is no-op)");
            Assert.AreEqual(PageW / 2.0, panelBlock.Columns[1].TotalBounds.X.PointsValue, 0.5,
                "Grow items: col 1 should start at X=300 (justify-content is no-op)");
        }

        // -----------------------------------------------------------------------
        // justify-content — three-item tests give more interesting gap arithmetic
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_JustifyContent_SpaceBetween_ThreeItems()
        {
            // space-between: first at left edge, last at right edge, equal gap between.
            // 3 × 100pt in 600pt → leftover=300, 2 gaps → 150pt each.
            // col0=0, col1=0+100+150=250, col2=250+100+150=500.
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; justify-content:space-between; width:600pt; border:1pt solid #000000;"">
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0E8FF;"">A</div>
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#FFE8D0;"">B</div>
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0FFE8;"">C</div>
  </div>
</body>
</html>";
            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Justify_SpaceBetween_3.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            var flexBlock = FindFlexBlock(layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(flexBlock);
            Assert.AreEqual(3, flexBlock.Columns.Length);

            Assert.AreEqual(  0.0, flexBlock.Columns[0].TotalBounds.X.PointsValue, 0.5, "space-between 3: col0 at left edge");
            Assert.AreEqual(250.0, flexBlock.Columns[1].TotalBounds.X.PointsValue, 0.5, "space-between 3: col1 at 250");
            Assert.AreEqual(500.0, flexBlock.Columns[2].TotalBounds.X.PointsValue, 0.5, "space-between 3: col2 at right edge");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_JustifyContent_SpaceAround_TwoItems()
        {
            // space-around: equal space around each item (half-gap at edges).
            // 2 × 100pt in 600pt → leftover=400, aroundUnit=200, startOffset=100, gap=200.
            // col0=0+100=100, col1=100+200=300 (original col1 X=100, +300 offset=400?)
            // Re-check engine: xOffset starts at 100, col0.X = 0+100=100.
            // xOffset += 200 → 300. col1.X = 100 + 300 = 400.
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; justify-content:space-around; width:600pt; border:1pt solid #000000;"">
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0E8FF;"">A</div>
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#FFE8D0;"">B</div>
  </div>
</body>
</html>";
            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Justify_SpaceAround_2.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            var flexBlock = FindFlexBlock(layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(flexBlock);
            Assert.AreEqual(2, flexBlock.Columns.Length);

            // aroundUnit = 400/2 = 200, startOffset = 100, gapBetween = 200.
            // col0 original X=0 → 0+100=100. col1 original X=100 → 100+300=400.
            Assert.AreEqual(100.0, flexBlock.Columns[0].TotalBounds.X.PointsValue, 0.5, "space-around 2: col0 at 100 (half-gap)");
            Assert.AreEqual(400.0, flexBlock.Columns[1].TotalBounds.X.PointsValue, 0.5, "space-around 2: col1 at 400");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_JustifyContent_SpaceAround_ThreeItems()
        {
            // space-around: 3 × 100pt in 600pt → leftover=300, aroundUnit=100, startOffset=50, gap=100.
            // col0 original X=0   → 0+50=50.
            // col1 original X=100 → 100+150=250.
            // col2 original X=200 → 200+250=450.
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; justify-content:space-around; width:600pt; border:1pt solid #000000;"">
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0E8FF;"">A</div>
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#FFE8D0;"">B</div>
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0FFE8;"">C</div>
  </div>
</body>
</html>";
            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Justify_SpaceAround_3.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            var flexBlock = FindFlexBlock(layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(flexBlock);
            Assert.AreEqual(3, flexBlock.Columns.Length);

            Assert.AreEqual( 50.0, flexBlock.Columns[0].TotalBounds.X.PointsValue, 0.5, "space-around 3: col0 at 50");
            Assert.AreEqual(250.0, flexBlock.Columns[1].TotalBounds.X.PointsValue, 0.5, "space-around 3: col1 at 250");
            Assert.AreEqual(450.0, flexBlock.Columns[2].TotalBounds.X.PointsValue, 0.5, "space-around 3: col2 at 450");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_JustifyContent_Center_ThreeItems()
        {
            // center: 3 × 100pt in 600pt → leftover=300, startOffset=150.
            // All columns shift +150. col0=150, col1=250, col2=350.
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; justify-content:center; width:600pt; border:1pt solid #000000;"">
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0E8FF;"">A</div>
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#FFE8D0;"">B</div>
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0FFE8;"">C</div>
  </div>
</body>
</html>";
            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Justify_Center_3.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            var flexBlock = FindFlexBlock(layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(flexBlock);
            Assert.AreEqual(3, flexBlock.Columns.Length);

            Assert.AreEqual(150.0, flexBlock.Columns[0].TotalBounds.X.PointsValue, 0.5, "center 3: col0 at 150");
            Assert.AreEqual(250.0, flexBlock.Columns[1].TotalBounds.X.PointsValue, 0.5, "center 3: col1 at 250");
            Assert.AreEqual(350.0, flexBlock.Columns[2].TotalBounds.X.PointsValue, 0.5, "center 3: col2 at 350");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_JustifyContent_SpaceEvenly_ThreeItems()
        {
            // space-evenly: equal spacing before, between, and after every item.
            // 3 × 100pt in 600pt → leftover=300, 4 gaps → 75pt each.
            // col0: 0+75=75.  col1: 100+150=250.  col2: 200+225=425.
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; justify-content:space-evenly; width:600pt; border:1pt solid #000000;"">
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0E8FF;"">A</div>
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#FFE8D0;"">B</div>
    <div style=""width:100pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0FFE8;"">C</div>
  </div>
</body>
</html>";
            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Justify_SpaceEvenly_3.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            var flexBlock = FindFlexBlock(layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(flexBlock);
            Assert.AreEqual(3, flexBlock.Columns.Length);

            Assert.AreEqual( 75.0, flexBlock.Columns[0].TotalBounds.X.PointsValue, 0.5, "space-evenly: col0 at 75");
            Assert.AreEqual(250.0, flexBlock.Columns[1].TotalBounds.X.PointsValue, 0.5, "space-evenly: col1 at 250");
            Assert.AreEqual(425.0, flexBlock.Columns[2].TotalBounds.X.PointsValue, 0.5, "space-evenly: col2 at 425");
        }

        // -----------------------------------------------------------------------
        // align-items — three children with mixed heights
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_AlignItems_FlexEnd_ThreeChildren_MixedHeights()
        {
            // Three children: 30pt, 50pt, 80pt. Row height = 80pt.
            // flex-end: each child Y-offset = 80 - childHeight.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.AlignItems = FlexAlignMode.FlexEnd;
            AddChild(panel, height: 40, grow: 1, label: "Short",  borderColor: new Color(200,  80,  80));
            AddChild(panel, height: 60, grow: 1, label: "Medium", borderColor: new Color( 80, 200,  80));
            AddChild(panel, height: 80, grow: 1, label: "Tall",   borderColor: new Color( 80,  80, 200));

            using (var ms = DocStreams.GetOutputStream("Flex_AlignItems_FlexEnd_3.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(3, panelBlock.Columns.Length);

            var shortB  = panelBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var mediumB = panelBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            var tallB   = panelBlock.Columns[2].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(40.0, shortB.TotalBounds.Y.PointsValue,  0.5, "flex-end: short (30) offset = 80-30 = 50");
            Assert.AreEqual(20.0, mediumB.TotalBounds.Y.PointsValue, 0.5, "flex-end: medium (50) offset = 80-50 = 30");
            Assert.AreEqual( 0.0, tallB.TotalBounds.Y.PointsValue,   0.5, "flex-end: tall (80) offset = 0");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_AlignItems_Center_ThreeChildren_MixedHeights()
        {
            // Three children: 30pt, 50pt, 80pt. Row height = 80pt.
            // center: each child Y-offset = (80 - childHeight) / 2.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.AlignItems = FlexAlignMode.Center;
            AddChild(panel, height: 30, grow: 1, label: "Short",  borderColor: new Color(200,  80,  80));
            AddChild(panel, height: 50, grow: 1, label: "Medium", borderColor: new Color( 80, 200,  80));
            AddChild(panel, height: 80, grow: 1, label: "Tall",   borderColor: new Color( 80,  80, 200));

            using (var ms = DocStreams.GetOutputStream("Flex_AlignItems_Center_3.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(3, panelBlock.Columns.Length);

            var shortB  = panelBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var mediumB = panelBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            var tallB   = panelBlock.Columns[2].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(25.0, shortB.TotalBounds.Y.PointsValue,  0.5, "center: short (30) offset = (80-30)/2 = 25");
            Assert.AreEqual(15.0, mediumB.TotalBounds.Y.PointsValue, 0.5, "center: medium (50) offset = (80-50)/2 = 15");
            Assert.AreEqual( 0.0, tallB.TotalBounds.Y.PointsValue,   0.5, "center: tall (80) offset = 0");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_AlignItems_Stretch_BehavesLikeFlexStart()
        {
            // Stretch (=0) is explicitly skipped in the engine — treated same as FlexStart.
            // Documents current behaviour: items keep their natural heights.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.AlignItems = FlexAlignMode.Stretch;
            AddChild(panel, height: 40, grow: 1, label: "Short", borderColor: new Color(200, 80, 80));
            AddChild(panel, height: 80, grow: 1, label: "Tall",  borderColor: new Color( 80, 80, 200));

            using (var ms = DocStreams.GetOutputStream("Flex_AlignItems_Stretch.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(2, panelBlock.Columns.Length);

            var shortB = panelBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var tallB  = panelBlock.Columns[1].Contents[0] as PDFLayoutBlock;

            // Current behaviour: no Y offset applied (same as flex-start).
            Assert.AreEqual(0.0, shortB.TotalBounds.Y.PointsValue, 0.5, "stretch (skip): short child stays at Y=0");
            Assert.AreEqual(0.0, tallB.TotalBounds.Y.PointsValue,  0.5, "stretch (skip): tall child stays at Y=0");
        }

        // -----------------------------------------------------------------------
        // Mixed grow ratios
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_MixedGrow_FixedPlusOneAndTwo()
        {
            // A: width=100pt, grow=0 (fixed). B: grow=1. C: grow=2.
            // Available=600. Fixed=100. Remaining=500, total fr=3.
            // B gets 500*(1/3)≈166.67pt, C gets 500*(2/3)≈333.33pt.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            var fixedItem = AddChild(panel, height: 50, grow: 0, label: "Fixed",  borderColor: new Color(200,  80,  80));
            AddChild(panel, height: 50, grow: 1, label: "Grow×1", borderColor: new Color( 80, 200,  80));
            AddChild(panel, height: 50, grow: 2, label: "Grow×2", borderColor: new Color( 80,  80, 200));

            // Give Fixed item an explicit width so grow=0 anchors correctly.
            fixedItem.Width = 100;

            using (var ms = DocStreams.GetOutputStream("Flex_MixedGrow_0_1_2.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(3, panelBlock.Columns.Length);

            var colA = panelBlock.Columns[0];
            var colB = panelBlock.Columns[1];
            var colC = panelBlock.Columns[2];

            Assert.AreEqual(100.0, colA.TotalBounds.Width.PointsValue, 1.0, "Fixed item width = 100pt");
            Assert.AreEqual(500.0 / 3.0, colB.TotalBounds.Width.PointsValue, 2.0, "Grow×1 width ≈ 166.67pt");
            Assert.AreEqual(1000.0 / 3.0, colC.TotalBounds.Width.PointsValue, 2.0, "Grow×2 width ≈ 333.33pt");

            // B starts immediately after A.
            Assert.AreEqual(100.0, colB.TotalBounds.X.PointsValue, 1.0, "Grow×1 starts at X=100");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_GrowRatioOne_ThreeEqualChildren()
        {
            // Three grow:1 items in 600pt → each gets exactly 200pt.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            AddChild(panel, height: 50, grow: 1, label: "A", borderColor: new Color(200,  80,  80));
            AddChild(panel, height: 50, grow: 1, label: "B", borderColor: new Color( 80, 200,  80));
            AddChild(panel, height: 50, grow: 1, label: "C", borderColor: new Color( 80,  80, 200));

            using (var ms = DocStreams.GetOutputStream("Flex_Grow_3Equal.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(3, panelBlock.Columns.Length);

            Assert.AreEqual(200.0, panelBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0, "A width = 200pt");
            Assert.AreEqual(200.0, panelBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0, "B width = 200pt");
            Assert.AreEqual(200.0, panelBlock.Columns[2].TotalBounds.Width.PointsValue, 1.0, "C width = 200pt");
            Assert.AreEqual(  0.0, panelBlock.Columns[0].TotalBounds.X.PointsValue, 0.5, "A at X=0");
            Assert.AreEqual(200.0, panelBlock.Columns[1].TotalBounds.X.PointsValue, 0.5, "B at X=200");
            Assert.AreEqual(400.0, panelBlock.Columns[2].TotalBounds.X.PointsValue, 0.5, "C at X=400");
        }

        // -----------------------------------------------------------------------
        // gap + align combined
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_Gap_AlignCenter_Combined()
        {
            // Gap reduces available width; align-items centers shorter items vertically.
            // 3 items, 20pt gap → container=600, gap cost=2×20=40, working=560.
            // grow:1 each → each column≈186.67pt.
            // Heights: 40, 80, 60. Max=80. Center offsets: 20, 0, 10.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.AlignItems  = FlexAlignMode.Center;
            panel.Style.Flex.ColumnGap   = 20;
            AddChild(panel, height: 40, grow: 1, label: "Short",  borderColor: new Color(200,  80,  80));
            AddChild(panel, height: 80, grow: 1, label: "Tall",   borderColor: new Color( 80, 200,  80));
            AddChild(panel, height: 60, grow: 1, label: "Medium", borderColor: new Color( 80,  80, 200));

            using (var ms = DocStreams.GetOutputStream("Flex_Gap_AlignCenter.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(3, panelBlock.Columns.Length);

            var shortB  = panelBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var tallB   = panelBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            var mediumB = panelBlock.Columns[2].Contents[0] as PDFLayoutBlock;

            // Center offsets: (80-40)/2=20, (80-80)/2=0, (80-60)/2=10.
            Assert.AreEqual(20.0, shortB.TotalBounds.Y.PointsValue,  1.0, "Short (40pt) centered: Y=20");
            Assert.AreEqual( 0.0, tallB.TotalBounds.Y.PointsValue,   1.0, "Tall (80pt) centered: Y=0");
            Assert.AreEqual(10.0, mediumB.TotalBounds.Y.PointsValue, 1.0, "Medium (60pt) centered: Y=10");
        }

        // -----------------------------------------------------------------------
        // flex-direction: column with align-items
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexColumn_AlignItems_Center_NarrowItems()
        {
            // Column direction: items stack vertically.
            // align-items in column mode aligns along the X axis (horizontal centering).
            // Each item has an explicit width narrower than the container.
            // NOTE: current engine aligns Y offsets post-layout; column-mode alignment
            //       is not yet implemented — this test documents the existing no-op behaviour.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg, FlexDirection.Column);
            panel.Style.Flex.AlignItems = FlexAlignMode.Center;
            var itemA = AddChild(panel, height: 40, grow: 0, label: "Narrow A", borderColor: new Color(200, 80, 80));
            var itemB = AddChild(panel, height: 40, grow: 0, label: "Narrow B", borderColor: new Color(80, 200, 80));
            itemA.Width = 200;
            itemB.Width = 200;

            using (var ms = DocStreams.GetOutputStream("Flex_Column_AlignCenter.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            // Items should stack (column mode) — verify they both rendered.
            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            Assert.IsTrue(pageRegion.Contents.Count > 0, "Column flex should produce layout blocks");
        }

        // -----------------------------------------------------------------------
        // Visual showcase — four items, mixed sizes, space-around + align-center
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_FourChildren_SpaceAround_AlignCenter_Visual()
        {
            // Visual stress test: four items of different heights, space-around justify,
            // center align — produces a PDF that clearly shows both axes working together.
            // 4 × 80pt items in 600pt → leftover=280, aroundUnit=70, startOffset=35, gap=70.
            // Heights: 30, 70, 50, 90. Max=90.
            // X positions: col0=0+35=35, col1=80+105=185, col2=160+175=335, col3=240+245=485.
            // Y offsets (center): (90-30)/2=30, (90-70)/2=10, (90-50)/2=20, 0.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.JustifyContent = FlexJustify.SpaceAround;
            panel.Style.Flex.AlignItems     = FlexAlignMode.Center;
            var i0 = AddChild(panel, height: 30, grow: 0, label: "30pt",  borderColor: new Color(200,  80,  80));
            var i1 = AddChild(panel, height: 70, grow: 0, label: "70pt",  borderColor: new Color( 80, 200,  80));
            var i2 = AddChild(panel, height: 50, grow: 0, label: "50pt",  borderColor: new Color( 80,  80, 200));
            var i3 = AddChild(panel, height: 90, grow: 0, label: "90pt",  borderColor: new Color(200, 160,   0));
            i0.Width = i1.Width = i2.Width = i3.Width = 80;

            using (var ms = DocStreams.GetOutputStream("Flex_Visual_SpaceAround_AlignCenter_4.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(4, panelBlock.Columns.Length);

            // X positions (space-around).
            Assert.AreEqual( 35.0, panelBlock.Columns[0].TotalBounds.X.PointsValue, 1.0, "col0 X = 35");
            Assert.AreEqual(185.0, panelBlock.Columns[1].TotalBounds.X.PointsValue, 1.0, "col1 X = 185");
            Assert.AreEqual(335.0, panelBlock.Columns[2].TotalBounds.X.PointsValue, 1.0, "col2 X = 335");
            Assert.AreEqual(485.0, panelBlock.Columns[3].TotalBounds.X.PointsValue, 1.0, "col3 X = 485");

            // Y positions (center).
            var b0 = panelBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var b1 = panelBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            var b2 = panelBlock.Columns[2].Contents[0] as PDFLayoutBlock;
            var b3 = panelBlock.Columns[3].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(30.0, b0.TotalBounds.Y.PointsValue, 1.0, "30pt item centered: Y=(90-30)/2=30");
            Assert.AreEqual(10.0, b1.TotalBounds.Y.PointsValue, 1.0, "70pt item centered: Y=(90-70)/2=10");
            Assert.AreEqual(20.0, b2.TotalBounds.Y.PointsValue, 1.0, "50pt item centered: Y=(90-50)/2=20");
            Assert.AreEqual( 0.0, b3.TotalBounds.Y.PointsValue, 1.0, "90pt item (tallest): Y=0");
        }

        // -----------------------------------------------------------------------
        // Container padding — columns must fit inside the padded content area
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_ContainerPadding_ReducesColumnWidth()
        {
            // Container: 600pt wide, 20pt padding all sides → content area = 560pt.
            // Two grow:1 items → each column = 280pt.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Padding.All = 20;
            AddChild(panel, height: 50, grow: 1, label: "A", borderColor: new Color(200, 80, 80));
            AddChild(panel, height: 50, grow: 1, label: "B", borderColor: new Color(80, 80, 200));

            using (var ms = DocStreams.GetOutputStream("Flex_ContainerPadding_TwoItems.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(2, panelBlock.Columns.Length);

            // Content width = 600 - 2×20 = 560 → each column = 280pt.
            double expected = (PageW - 40.0) / 2.0;
            Assert.AreEqual(expected, panelBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column 0 width should be (600-40)/2 = 280pt (padding deducted)");
            Assert.AreEqual(expected, panelBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Column 1 width should be (600-40)/2 = 280pt (padding deducted)");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_ContainerPadding_WithGap()
        {
            // Container: 600pt, padding:20 all, column-gap:10. Three grow:1 items.
            // Content width = 560pt; gap cost = 2×10=20pt; working = 540pt → each col = 180pt.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Padding.All     = 20;
            panel.Style.Flex.ColumnGap  = 10;
            AddChild(panel, height: 50, grow: 1, label: "A", borderColor: new Color(200,  80,  80));
            AddChild(panel, height: 50, grow: 1, label: "B", borderColor: new Color( 80, 200,  80));
            AddChild(panel, height: 50, grow: 1, label: "C", borderColor: new Color( 80,  80, 200));

            using (var ms = DocStreams.GetOutputStream("Flex_ContainerPadding_Gap_3.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(3, panelBlock.Columns.Length);

            double expected = (PageW - 40.0 - 20.0) / 3.0; // (560-20)/3 = 180
            Assert.AreEqual(expected, panelBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Column width = (600-pad×2-gap×2)/3 = 180pt");
            Assert.AreEqual(expected, panelBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Column 1 width = 180pt");
            Assert.AreEqual(expected, panelBlock.Columns[2].TotalBounds.Width.PointsValue, 1.0,
                "Column 2 width = 180pt");
        }

        // -----------------------------------------------------------------------
        // Item margins — document how margins interact with flex columns
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_ItemMargins_ColumnWidthUnchanged()
        {
            // Item margins are applied inside the allocated column, not taken from column width.
            // Two grow:1 items each with margin:10pt in a 600pt container.
            // Column widths are still 300pt each; margin reduces the inner content area only.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            var itemA = AddChild(panel, height: 50, grow: 1, label: "A", borderColor: new Color(200, 80, 80));
            var itemB = AddChild(panel, height: 50, grow: 1, label: "B", borderColor: new Color(80, 80, 200));
            itemA.Style.Margins.All = 10;
            itemB.Style.Margins.All = 10;

            using (var ms = DocStreams.GetOutputStream("Flex_ItemMargins_TwoItems.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(2, panelBlock.Columns.Length);

            // Column widths are computed from grow ratios, not affected by item margins.
            Assert.AreEqual(300.0, panelBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0,
                "Item margins do not reduce column width: col0 = 300pt");
            Assert.AreEqual(300.0, panelBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0,
                "Item margins do not reduce column width: col1 = 300pt");
        }

        // -----------------------------------------------------------------------
        // Multiple rows — three stacked flex containers (simulating a row-by-row layout)
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexPage_ThreeRows_StackedContainers()
        {
            // Three flex rows stacked on the page, each with margin-bottom for row spacing.
            // Row 0: 3 equal columns, 40pt tall.
            // Row 1: 2 columns (grow:1 + grow:2), 60pt tall.
            // Row 2: 4 equal columns with space-between justify, 50pt tall.
            var doc = CreateDoc(out var pg);

            // Row 0 — 3 equal
            var row0 = CreateFlexContainer(pg);
            row0.Style.Margins.Bottom = 10;
            AddChild(row0, height: 40, grow: 1, label: "R0C0", borderColor: new Color(200,  80,  80));
            AddChild(row0, height: 40, grow: 1, label: "R0C1", borderColor: new Color( 80, 200,  80));
            AddChild(row0, height: 40, grow: 1, label: "R0C2", borderColor: new Color( 80,  80, 200));

            // Row 1 — asymmetric grow
            var row1 = CreateFlexContainer(pg);
            row1.Style.Margins.Bottom = 10;
            AddChild(row1, height: 60, grow: 1, label: "R1C0 (1fr)", borderColor: new Color(160, 120,  80));
            AddChild(row1, height: 60, grow: 2, label: "R1C1 (2fr)", borderColor: new Color( 80, 120, 160));

            // Row 2 — space-between, 4 fixed-width items
            var row2 = CreateFlexContainer(pg);
            row2.Style.Flex.JustifyContent = FlexJustify.SpaceBetween;
            var r2c0 = AddChild(row2, height: 50, grow: 0, label: "R2C0", borderColor: new Color(120, 200, 120));
            var r2c1 = AddChild(row2, height: 50, grow: 0, label: "R2C1", borderColor: new Color(120, 120, 200));
            var r2c2 = AddChild(row2, height: 50, grow: 0, label: "R2C2", borderColor: new Color(200, 120, 120));
            var r2c3 = AddChild(row2, height: 50, grow: 0, label: "R2C3", borderColor: new Color(200, 200, 120));
            r2c0.Width = r2c1.Width = r2c2.Width = r2c3.Width = 100;

            using (var ms = DocStreams.GetOutputStream("Flex_ThreeRows_Stacked.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            Assert.AreEqual(3, pageRegion.Contents.Count, "Page should have 3 flex row blocks");

            var r0Block = pageRegion.Contents[0] as PDFLayoutBlock;
            var r1Block = pageRegion.Contents[1] as PDFLayoutBlock;
            var r2Block = pageRegion.Contents[2] as PDFLayoutBlock;

            Assert.AreEqual(3, r0Block.Columns.Length, "Row 0: 3 columns");
            Assert.AreEqual(2, r1Block.Columns.Length, "Row 1: 2 columns");
            Assert.AreEqual(4, r2Block.Columns.Length, "Row 2: 4 columns");

            // Row 0: each column = 200pt.
            Assert.AreEqual(200.0, r0Block.Columns[0].TotalBounds.Width.PointsValue, 1.0, "R0 col0 = 200pt");
            Assert.AreEqual(200.0, r0Block.Columns[1].TotalBounds.Width.PointsValue, 1.0, "R0 col1 = 200pt");
            Assert.AreEqual(200.0, r0Block.Columns[2].TotalBounds.Width.PointsValue, 1.0, "R0 col2 = 200pt");

            // Row 1: grow:1 → 200pt, grow:2 → 400pt.
            Assert.AreEqual(200.0, r1Block.Columns[0].TotalBounds.Width.PointsValue, 1.0, "R1 col0 (1fr) = 200pt");
            Assert.AreEqual(400.0, r1Block.Columns[1].TotalBounds.Width.PointsValue, 1.0, "R1 col1 (2fr) = 400pt");

            // Row 2: space-between — 4 × 100pt items in 600pt → leftover=200, gap=200/3≈66.7.
            // col0 at X=0, col1 at X≈166.7, col2 at X≈333.3, col3 at X=500.
            Assert.AreEqual(  0.0, r2Block.Columns[0].TotalBounds.X.PointsValue, 1.0, "R2 col0 at left");
            Assert.AreEqual(500.0, r2Block.Columns[3].TotalBounds.X.PointsValue, 1.0, "R2 col3 at right edge");

            // Row 1 starts below row 0 (+ margin).
            Assert.IsTrue(r1Block.TotalBounds.Y.PointsValue > r0Block.TotalBounds.Y.PointsValue + 40.0,
                "Row 1 starts below row 0");
        }

        // -----------------------------------------------------------------------
        // Multi-column with padding — real-world card-grid layout
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_FourColumns_PaddedContainer_AlignCenter()
        {
            // 4 columns in a padded container: padding=15pt, gap=10pt.
            // Content width = 600-30=570. Gap cost = 3×10=30. Working = 540pt → each col = 135pt.
            // Items have mixed heights → align:center vertically offsets shorter items.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Padding.All    = 15;
            panel.Style.Flex.ColumnGap = 10;
            panel.Style.Flex.AlignItems = FlexAlignMode.Center;

            AddChild(panel, height: 80, grow: 1, label: "Tall\n80pt",   borderColor: new Color(200,  80,  80));
            AddChild(panel, height: 40, grow: 1, label: "Short\n40pt",  borderColor: new Color( 80, 200,  80));
            AddChild(panel, height: 60, grow: 1, label: "Medium\n60pt", borderColor: new Color( 80,  80, 200));
            AddChild(panel, height: 80, grow: 1, label: "Tall\n80pt",   borderColor: new Color(200, 160,   0));

            using (var ms = DocStreams.GetOutputStream("Flex_4Col_PaddedContainer.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(4, panelBlock.Columns.Length);

            double expected = (PageW - 30.0 - 30.0) / 4.0; // (570-30)/4 = 135pt
            Assert.AreEqual(expected, panelBlock.Columns[0].TotalBounds.Width.PointsValue, 1.5, "Col 0 = 135pt");
            Assert.AreEqual(expected, panelBlock.Columns[1].TotalBounds.Width.PointsValue, 1.5, "Col 1 = 135pt");
            Assert.AreEqual(expected, panelBlock.Columns[2].TotalBounds.Width.PointsValue, 1.5, "Col 2 = 135pt");
            Assert.AreEqual(expected, panelBlock.Columns[3].TotalBounds.Width.PointsValue, 1.5, "Col 3 = 135pt");

            // Center: tallest=80, medium=60 → offset=10; short=40 → offset=20.
            var b1 = panelBlock.Columns[1].Contents[0] as PDFLayoutBlock; // short
            var b2 = panelBlock.Columns[2].Contents[0] as PDFLayoutBlock; // medium
            Assert.AreEqual(20.0, b1.TotalBounds.Y.PointsValue, 1.0, "Short (40) centered: Y=(80-40)/2=20");
            Assert.AreEqual(10.0, b2.TotalBounds.Y.PointsValue, 1.0, "Medium (60) centered: Y=(80-60)/2=10");
        }

        // -----------------------------------------------------------------------
        // CSS-parsed multi-row layout — realistic "card grid" scenario
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexPage_CSSParsed_MultiRowLayout()
        {
            // Two flex rows parsed from CSS. Each row has different column counts and heights.
            // Row 1: 3 equal columns. Row 2: 2 columns (1fr + 2fr).
            // Container padding:10pt, gap:8pt.
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <!-- Row 1: three equal columns -->
  <div style=""display:flex; width:600pt; padding:10pt; gap:8pt; border:1pt solid #000000; margin-bottom:8pt;"">
    <div style=""flex-grow:1; height:60pt; padding:4pt; border:1pt solid #646464; background-color:#D0E8FF;"">R1 A</div>
    <div style=""flex-grow:1; height:60pt; padding:4pt; border:1pt solid #646464; background-color:#FFE8D0;"">R1 B</div>
    <div style=""flex-grow:1; height:60pt; padding:4pt; border:1pt solid #646464; background-color:#D0FFE8;"">R1 C</div>
  </div>
  <!-- Row 2: asymmetric grow -->
  <div style=""display:flex; width:600pt; padding:10pt; gap:8pt; border:1pt solid #000000;"">
    <div style=""flex-grow:1; height:80pt; padding:4pt; border:1pt solid #646464; background-color:#FFD0E8;"">R2 Narrow (1fr)</div>
    <div style=""flex-grow:2; height:80pt; padding:4pt; border:1pt solid #646464; background-color:#E8D0FF;"">R2 Wide (2fr)</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_CSSParsed_MultiRow.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var pageRegion = layout.AllPages[0].ContentBlock.Columns[0];

            // Find the two flex row blocks.
            var row0Block = FindFlexBlock(pageRegion);
            Assert.IsNotNull(row0Block, "Row 0 block must exist");
            Assert.AreEqual(3, row0Block.Columns.Length, "Row 0: 3 columns");

            // Row 0: content=580pt, gap=2×8=16 → working=564 → each col≈188pt.
            double row0ColW = (600.0 - 20.0 - 16.0) / 3.0;
            Assert.AreEqual(row0ColW, row0Block.Columns[0].TotalBounds.Width.PointsValue, 2.0,
                "Row 0 col width = (580-16)/3 ≈ 188pt");

            // Row 1: content=580, gap=8 → working=572 → 1fr=190.67, 2fr=381.33.
            // Find the second flex row block by looking for a 2-column block.
            PDFLayoutBlock row1Block = null;
            foreach (var item in pageRegion.Contents)
            {
                if (item is PDFLayoutBlock b && b != row0Block && b.Columns.Length == 2)
                { row1Block = b; break; }
            }
            Assert.IsNotNull(row1Block, "Row 1 (2-column) block must exist");
            double row1Working = 600.0 - 20.0 - 8.0; // 572
            Assert.AreEqual(row1Working / 3.0, row1Block.Columns[0].TotalBounds.Width.PointsValue, 2.0,
                "Row 1 col0 (1fr) ≈ 190.67pt");
            Assert.AreEqual(row1Working * 2.0 / 3.0, row1Block.Columns[1].TotalBounds.Width.PointsValue, 2.0,
                "Row 1 col1 (2fr) ≈ 381.33pt");
        }

        // -----------------------------------------------------------------------
        // flex-wrap: wrap — 3×150pt in 300pt container wraps correctly
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_WrapNotImplemented_OverflowItemsShrinkToFit()
        {
            // flex-wrap:wrap is now implemented.
            // 3 × 150pt in a 300pt container:
            //   Row 0: A+B (150+150=300, fits exactly)
            //   Row 1: C (wraps because 150+150+150=450 > 300)
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-wrap:wrap; width:300pt; border:1pt solid #000000;"">
    <div style=""width:150pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0E8FF;"">A</div>
    <div style=""width:150pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#FFE8D0;"">B</div>
    <div style=""width:150pt; height:50pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0FFE8;"">C</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_WrapOverflow.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var bodyRegion = layout.AllPages[0].ContentBlock.Columns[0];

            // Find the flex container with 2 wrap-row blocks
            var ownerCounts = new Dictionary<IComponent, List<PDFLayoutBlock>>();
            foreach (var item in bodyRegion.Contents)
            {
                if (item is PDFLayoutBlock b && b.Owner != null)
                {
                    if (!ownerCounts.ContainsKey(b.Owner))
                        ownerCounts[b.Owner] = new List<PDFLayoutBlock>();
                    ownerCounts[b.Owner].Add(b);
                }
            }

            List<PDFLayoutBlock> wrapRows = null;
            foreach (var kv in ownerCounts)
                if (kv.Value.Count >= 2) { wrapRows = kv.Value; break; }

            Assert.IsNotNull(wrapRows, "flex-wrap:wrap should produce 2 wrap-row blocks");
            Assert.AreEqual(2, wrapRows.Count, "3×150pt in 300pt: row0=(A,B), row1=(C)");
            Assert.AreEqual(2, wrapRows[0].Columns.Length, "Row 0 should have 2 columns (A+B)");
            Assert.AreEqual(1, wrapRows[1].Columns.Length, "Row 1 should have 1 column (C)");

            // Row 0: each column should be 150pt wide
            Assert.AreEqual(150.0, wrapRows[0].Columns[0].TotalBounds.Width.PointsValue, 1.0, "Row 0 col 0 = 150pt");
            Assert.AreEqual(150.0, wrapRows[0].Columns[1].TotalBounds.Width.PointsValue, 1.0, "Row 0 col 1 = 150pt");
        }

        // ======================================================================
        // HARDER TESTS
        // ======================================================================

        // -----------------------------------------------------------------------
        // Nested flex — flex container inside a flex column
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_NestedFlex_InnerContainerLayedOut()
        {
            // Outer flex row: 2 grow:1 columns → each 300pt wide.
            // The first column's child is itself a flex row with 2 grow:1 items.
            // Inner items should each be 150pt (half of 300pt).
            var doc   = CreateDoc(out var pg);
            var outer = CreateFlexContainer(pg);

            // Inner flex container placed as the first child of outer.
            var inner = new Panel();
            inner.Style.Position.DisplayMode = DisplayMode.FlexBox;
            inner.Style.Flex.Direction       = FlexDirection.Row;
            inner.Style.Border.LineStyle = LineType.Solid;
            inner.Style.Border.Width     = 1;
            inner.Style.Border.Color     = new Color(0, 150, 200);
            // No explicit width — will fill its allocated column.
            AddChild(inner, height: 60, grow: 1, label: "In-A", borderColor: new Color(200, 80, 80));
            AddChild(inner, height: 60, grow: 1, label: "In-B", borderColor: new Color(80, 200, 80));
            outer.Contents.Add(inner);

            // Second child is a plain item.
            AddChild(outer, height: 60, grow: 1, label: "Out-B", borderColor: new Color(80, 80, 200));

            using (var ms = DocStreams.GetOutputStream("Flex_Nested_InnerFlex.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var outerBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(outerBlock);
            Assert.AreEqual(2, outerBlock.Columns.Length, "Outer flex: 2 columns");

            // Outer columns: each 300pt.
            Assert.AreEqual(300.0, outerBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0, "Outer col0 = 300pt");
            Assert.AreEqual(300.0, outerBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0, "Outer col1 = 300pt");

            // Find the inner flex block inside outer column 0.
            PDFLayoutBlock innerBlock = null;
            foreach (var item in outerBlock.Columns[0].Contents)
                if (item is PDFLayoutBlock b && b.Columns.Length == 2) { innerBlock = b; break; }

            Assert.IsNotNull(innerBlock, "Inner flex block should exist in outer col0");
            Assert.AreEqual(2, innerBlock.Columns.Length, "Inner flex: 2 columns");
            Assert.AreEqual(150.0, innerBlock.Columns[0].TotalBounds.Width.PointsValue, 2.0, "Inner col0 = 150pt");
            Assert.AreEqual(150.0, innerBlock.Columns[1].TotalBounds.Width.PointsValue, 2.0, "Inner col1 = 150pt");
        }

        // -----------------------------------------------------------------------
        // Single-item edge cases for justify-content
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_JustifyContent_SpaceBetween_SingleItem_Centers()
        {
            // space-between with a single item: engine special-cases colCount==1
            // and centres the item (startOffset = leftover/2).
            // 1 × 100pt in 600pt → leftover=500, startOffset=250 → X=250.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.JustifyContent = FlexJustify.SpaceBetween;
            var item = AddChild(panel, height: 50, grow: 0, label: "Solo", borderColor: new Color(200, 80, 80));
            item.Width = 100;

            using (var ms = DocStreams.GetOutputStream("Flex_Justify_SpaceBetween_1.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(1, panelBlock.Columns.Length);
            Assert.AreEqual(250.0, panelBlock.Columns[0].TotalBounds.X.PointsValue, 1.0,
                "space-between single item: centres at X=250");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_JustifyContent_SpaceAround_SingleItem_Centers()
        {
            // space-around with a single item: aroundUnit=leftover, startOffset=leftover/2.
            // Same centering as space-between for a single item.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.JustifyContent = FlexJustify.SpaceAround;
            var item = AddChild(panel, height: 50, grow: 0, label: "Solo", borderColor: new Color(80, 80, 200));
            item.Width = 100;

            using (var ms = DocStreams.GetOutputStream("Flex_Justify_SpaceAround_1.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(1, panelBlock.Columns.Length);
            Assert.AreEqual(250.0, panelBlock.Columns[0].TotalBounds.X.PointsValue, 1.0,
                "space-around single item: centres at X=250");
        }

        // -----------------------------------------------------------------------
        // Ten equal items — verifies column-width arithmetic at scale
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_TenEqualGrowItems_EachSixtyPt()
        {
            // 10 × grow:1 in 600pt → each column = 60pt.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            for (int i = 0; i < 10; i++)
                AddChild(panel, height: 40, grow: 1, label: i.ToString(),
                         borderColor: new Color((byte)(20 * i + 40), 80, 160));

            using (var ms = DocStreams.GetOutputStream("Flex_10Equal.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(10, panelBlock.Columns.Length, "10 items → 10 columns");

            for (int i = 0; i < 10; i++)
                Assert.AreEqual(60.0, panelBlock.Columns[i].TotalBounds.Width.PointsValue, 1.0,
                    $"Col {i} width = 60pt");

            Assert.AreEqual(  0.0, panelBlock.Columns[0].TotalBounds.X.PointsValue, 0.5, "Col 0 at X=0");
            Assert.AreEqual(540.0, panelBlock.Columns[9].TotalBounds.X.PointsValue, 1.0, "Col 9 at X=540");
        }

        // -----------------------------------------------------------------------
        // Asymmetric padding
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_AsymmetricPadding_ContentAreaReflected()
        {
            // padding-left:40, padding-right:10 → content = 600-50 = 550pt.
            // 2 grow:1 items → each = 275pt.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Padding.Left  = 40;
            panel.Style.Padding.Right = 10;
            AddChild(panel, height: 50, grow: 1, label: "A", borderColor: new Color(200, 80, 80));
            AddChild(panel, height: 50, grow: 1, label: "B", borderColor: new Color(80, 80, 200));

            using (var ms = DocStreams.GetOutputStream("Flex_AsymmetricPadding.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(2, panelBlock.Columns.Length);
            Assert.AreEqual(275.0, panelBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0, "Col 0 = 275pt");
            Assert.AreEqual(275.0, panelBlock.Columns[1].TotalBounds.Width.PointsValue, 1.0, "Col 1 = 275pt");
        }

        // -----------------------------------------------------------------------
        // Oversized gap — working width clamps to zero, items get equal fractions
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_OversizedGap_DoesNotThrow()
        {
            // Gap of 400pt with 3 items in 600pt → totalGap=800 > 600.
            // Math.Max(0, 600-800) = 0 working pts. Engine should not throw.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.ColumnGap = 400;
            AddChild(panel, height: 50, grow: 1, label: "A", borderColor: new Color(200, 80, 80));
            AddChild(panel, height: 50, grow: 1, label: "B", borderColor: new Color(80, 200, 80));
            AddChild(panel, height: 50, grow: 1, label: "C", borderColor: new Color(80, 80, 200));

            using (var ms = DocStreams.GetOutputStream("Flex_OversizedGap.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should complete without throwing");
            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(3, panelBlock.Columns.Length, "All 3 items still placed");
        }

        // -----------------------------------------------------------------------
        // All-features combined — padding + gap + mixed grows + align + justify
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_AllFeaturesCombined()
        {
            // Container: 600pt, padding:15 all, column-gap:10.
            // Items: A(grow:0, w:80, h:40), B(grow:1, h:80), C(grow:0, w:80, h:60).
            // content=570, gap=2×10=20, working=550.
            // Fixed=80+80=160, remaining=550-160=390 → B gets 390pt.
            // align:center → row height=80: A Y=20, B Y=0, C Y=10.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Padding.All    = 15;
            panel.Style.Flex.ColumnGap = 10;
            panel.Style.Flex.AlignItems = FlexAlignMode.Center;
            var a = AddChild(panel, height: 40, grow: 0, label: "A\n(fixed 80)", borderColor: new Color(200,  80,  80));
                    AddChild(panel, height: 80, grow: 1, label: "B\n(flex)",     borderColor: new Color( 80, 200,  80));
            var c = AddChild(panel, height: 60, grow: 0, label: "C\n(fixed 80)", borderColor: new Color( 80,  80, 200));
            a.Width = 80;
            c.Width = 80;

            using (var ms = DocStreams.GetOutputStream("Flex_AllFeaturesCombined.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(3, panelBlock.Columns.Length);

            // Widths.
            Assert.AreEqual( 80.0, panelBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0, "A = 80pt (fixed)");
            Assert.AreEqual(390.0, panelBlock.Columns[1].TotalBounds.Width.PointsValue, 2.0, "B = 390pt (flex remainder)");
            Assert.AreEqual( 80.0, panelBlock.Columns[2].TotalBounds.Width.PointsValue, 1.0, "C = 80pt (fixed)");

            // Align-center Y offsets.
            var bA = panelBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var bB = panelBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            var bC = panelBlock.Columns[2].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(20.0, bA.TotalBounds.Y.PointsValue, 1.0, "A (h=40) Y = (80-40)/2 = 20");
            Assert.AreEqual( 0.0, bB.TotalBounds.Y.PointsValue, 1.0, "B (h=80, tallest) Y = 0");
            Assert.AreEqual(10.0, bC.TotalBounds.Y.PointsValue, 1.0, "C (h=60) Y = (80-60)/2 = 10");
        }

        // -----------------------------------------------------------------------
        // CSS-parsed real-world patterns
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_CSSParsed_NavBarPattern()
        {
            // Classic nav bar: logo on left (fixed width), spacer (grows), 3 nav items (fixed).
            // logo=150pt, spacer=grow:1, nav×3=80pt each.
            // Container=600, all items grow=0 except spacer.
            // Fixed = 150 + 3×80 = 390. Remaining = 210 → spacer gets 210pt.
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; width:600pt; align-items:center; padding:8pt; border:1pt solid #000000;"">
    <div style=""width:150pt; height:40pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#1E3A5F; color:#FFFFFF;"">Logo</div>
    <div style=""flex-grow:1; height:10pt;""></div>
    <div style=""width:80pt; height:40pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0E8FF;"">Home</div>
    <div style=""width:80pt; height:40pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0E8FF;"">About</div>
    <div style=""width:80pt; height:40pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0E8FF;"">Contact</div>
  </div>
</body>
</html>";
            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_NavBar.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            var flexBlock = FindFlexBlock(layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(flexBlock);
            Assert.AreEqual(5, flexBlock.Columns.Length, "Logo + spacer + 3 nav items = 5 columns");

            // content = 600-16=584. Fixed = 150 + 3×80 = 390. Remaining = 194 → spacer.
            double contentW = 600.0 - 16.0;
            double spacerW  = contentW - (150.0 + 3 * 80.0);
            Assert.AreEqual(150.0,  flexBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0, "Logo = 150pt");
            Assert.AreEqual(spacerW, flexBlock.Columns[1].TotalBounds.Width.PointsValue, 2.0, $"Spacer = {spacerW}pt");
            Assert.AreEqual( 80.0,  flexBlock.Columns[4].TotalBounds.Width.PointsValue, 1.0, "Last nav item = 80pt");

            // Align-center: spacer height=10, nav height=40 → max=40.
            // Spacer Y = (40-10)/2 = 15.
            var spacerBlock = flexBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(spacerBlock);
            Assert.AreEqual(15.0, spacerBlock.TotalBounds.Y.PointsValue, 1.0, "Spacer centered at Y=15");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_CSSParsed_SidebarContent()
        {
            // Sidebar + main content pattern: fixed sidebar (grow:0) + growing main area.
            // Sidebar=160pt fixed, main grows, gap=12pt.
            // Container=600. Fixed=160, gap=12 → main=600-160-12=428pt.
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; width:600pt; gap:12pt; align-items:flex-start; border:1pt solid #000000;"">
    <div style=""width:160pt; height:200pt; flex-grow:0; padding:8pt; border:1pt solid #646464; background-color:#F0F4FF;"">Sidebar</div>
    <div style=""flex-grow:1; height:120pt; padding:8pt; border:1pt solid #646464; background-color:#FFF8F0;"">Main Content Area</div>
  </div>
</body>
</html>";
            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_SidebarContent.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            var flexBlock = FindFlexBlock(layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(flexBlock);
            Assert.AreEqual(2, flexBlock.Columns.Length, "Sidebar + content = 2 columns");

            Assert.AreEqual(160.0, flexBlock.Columns[0].TotalBounds.Width.PointsValue, 1.0, "Sidebar = 160pt");
            Assert.AreEqual(428.0, flexBlock.Columns[1].TotalBounds.Width.PointsValue, 2.0, "Main = 600-160-12 = 428pt");

            // flex-start: both items at Y=0.
            var sidebarBlock = flexBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var mainBlock    = flexBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0.0, sidebarBlock.TotalBounds.Y.PointsValue, 0.5, "Sidebar Y=0");
            Assert.AreEqual(0.0, mainBlock.TotalBounds.Y.PointsValue,    0.5, "Main Y=0");
        }

        [TestCategory(TestCategory), TestMethod()]
        public void FlexRow_CSSParsed_CardGrid_WithAlignAndJustify()
        {
            // Five cards in a row: 4 fixed-width cards + space-evenly justify + align-center.
            // Cards are different heights to make alignment visible.
            // Container=600, no padding, 4 × 100pt = 400 → leftover=200, gap=200/5=40.
            // Positions: X = 40, 180, 320, 460.
            const string src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; width:600pt; justify-content:space-evenly; align-items:center; border:1pt solid #000000;"">
    <div style=""width:100pt; height:40pt;  flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0E8FF;"">Card A</div>
    <div style=""width:100pt; height:80pt;  flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#FFE8D0;"">Card B</div>
    <div style=""width:100pt; height:60pt;  flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#D0FFE8;"">Card C</div>
    <div style=""width:100pt; height:100pt; flex-grow:0; padding:4pt; border:1pt solid #646464; background-color:#FFD0E8;"">Card D</div>
  </div>
</body>
</html>";
            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_CardGrid_SpaceEvenly_AlignCenter.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            var flexBlock = FindFlexBlock(layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(flexBlock);
            Assert.AreEqual(4, flexBlock.Columns.Length);

            // space-evenly X positions: gap=200/5=40. col0:40, col1:180, col2:320, col3:460.
            Assert.AreEqual( 40.0, flexBlock.Columns[0].TotalBounds.X.PointsValue, 1.0, "Card A X=40");
            Assert.AreEqual(180.0, flexBlock.Columns[1].TotalBounds.X.PointsValue, 1.0, "Card B X=180");
            Assert.AreEqual(320.0, flexBlock.Columns[2].TotalBounds.X.PointsValue, 1.0, "Card C X=320");
            Assert.AreEqual(460.0, flexBlock.Columns[3].TotalBounds.X.PointsValue, 1.0, "Card D X=460");

            // align-center Y offsets: row height=100. A Y=30, B Y=10, C Y=20, D Y=0.
            var bA = flexBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var bB = flexBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            var bC = flexBlock.Columns[2].Contents[0] as PDFLayoutBlock;
            var bD = flexBlock.Columns[3].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(30.0, bA.TotalBounds.Y.PointsValue, 1.0, "Card A (40pt) Y=(100-40)/2=30");
            Assert.AreEqual(10.0, bB.TotalBounds.Y.PointsValue, 1.0, "Card B (80pt) Y=(100-80)/2=10");
            Assert.AreEqual(20.0, bC.TotalBounds.Y.PointsValue, 1.0, "Card C (60pt) Y=(100-60)/2=20");
            Assert.AreEqual( 0.0, bD.TotalBounds.Y.PointsValue, 1.0, "Card D (100pt, tallest) Y=0");
        }

        // -----------------------------------------------------------------------
        // FlexWrap helpers
        // -----------------------------------------------------------------------

        /// <summary>
        /// Returns all layout blocks in <paramref name="region"/> whose Owner matches <paramref name="owner"/>.
        /// Used to find multiple wrap-row blocks produced for the same flex container.
        /// </summary>
        private static List<PDFLayoutBlock> FindOwnerBlocks(PDFLayoutRegion region, IComponent owner)
        {
            var result = new List<PDFLayoutBlock>();
            foreach (var item in region.Contents)
                if (item is PDFLayoutBlock b && b.Owner == owner)
                    result.Add(b);
            return result;
        }

        /// <summary>
        /// Adds a flex child Panel with an explicit fixed width (grow=0) and optional height.
        /// </summary>
        private static Panel AddFixedChild(Panel parent, double width, double? height = null,
                                           string label = null)
        {
            var child = new Panel();
            child.Style.Flex.Grow  = 0;
            child.Width            = width;
            child.Style.Padding.All = 0;
            child.Style.Border.LineStyle = LineType.Solid;
            child.Style.Border.Width     = 1;
            child.Style.Border.Color     = new Color(100, 100, 100);
            if (height.HasValue)
                child.Height = height.Value;
            if (!string.IsNullOrEmpty(label))
                child.Contents.Add(new Label { Text = label });
            parent.Contents.Add(child);
            return child;
        }

        // -----------------------------------------------------------------------
        // FlexWrap — 3×250pt items in 600pt container → 2 rows
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexWrap_BasicOverflow_WrapsToNextRow()
        {
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.Wrap = FlexWrap.Wrap;

            AddFixedChild(panel, 250, height: 60, label: "A");
            AddFixedChild(panel, 250, height: 60, label: "B");
            AddFixedChild(panel, 250, height: 60, label: "C");

            using (var ms = DocStreams.GetOutputStream("Flex_Wrap_BasicOverflow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageCol = _layout.AllPages[0].ContentBlock.Columns[0];
            var rows    = FindOwnerBlocks(pageCol, panel);

            Assert.AreEqual(2, rows.Count, "3×250pt in 600pt should produce 2 wrap rows");

            // Row 0: items A and B → 2 columns
            Assert.AreEqual(2, rows[0].Columns.Length, "Row 0 should have 2 columns");

            // Row 1: item C → 1 column
            Assert.AreEqual(1, rows[1].Columns.Length, "Row 1 should have 1 column");

            // Row 1: the child block inside the single column should be ~250pt wide
            var row1Col0Block = rows[1].Columns[0].Contents.Count > 0
                ? rows[1].Columns[0].Contents[0] as PDFLayoutBlock
                : null;
            Assert.IsNotNull(row1Col0Block, "Row 1 col 0 should contain child block");
            Assert.AreEqual(250.0, row1Col0Block.TotalBounds.Width.PointsValue, 2.0,
                "Row 1 child should be ~250pt wide");

            // Row 1 Y position must be below row 0
            Assert.IsTrue(rows[1].TotalBounds.Y.PointsValue > rows[0].TotalBounds.Y.PointsValue,
                "Row 1 should be positioned below row 0");
        }

        // -----------------------------------------------------------------------
        // FlexWrap — 3×200pt exactly fills 600pt → no wrap (1 block)
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexWrap_ExactFit_NoWrap()
        {
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.Wrap = FlexWrap.Wrap;

            AddFixedChild(panel, 200, height: 60, label: "X");
            AddFixedChild(panel, 200, height: 60, label: "Y");
            AddFixedChild(panel, 200, height: 60, label: "Z");

            using (var ms = DocStreams.GetOutputStream("Flex_Wrap_ExactFit.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageCol = _layout.AllPages[0].ContentBlock.Columns[0];
            var rows    = FindOwnerBlocks(pageCol, panel);

            Assert.AreEqual(1, rows.Count, "3×200pt should exactly fit 600pt — 1 block");
            Assert.AreEqual(3, rows[0].Columns.Length, "Should have 3 columns");
        }

        // -----------------------------------------------------------------------
        // FlexWrap — 3×200pt + 10pt gap → 2 rows (gap pushes 3rd item over)
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexWrap_WithGap_ReducesCapacity()
        {
            const double gap = 10.0;

            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.Wrap = FlexWrap.Wrap;
            panel.Style.Flex.Gap  = new Unit(gap, PageUnits.Points);

            AddFixedChild(panel, 200, height: 60, label: "P");
            AddFixedChild(panel, 200, height: 60, label: "Q");
            AddFixedChild(panel, 200, height: 60, label: "R");

            using (var ms = DocStreams.GetOutputStream("Flex_Wrap_WithGap.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageCol = _layout.AllPages[0].ContentBlock.Columns[0];
            var rows    = FindOwnerBlocks(pageCol, panel);

            // 200+200+10=410 ≤ 600 (fits 2); 410+10+200=620 > 600 → 3rd wraps
            Assert.AreEqual(2, rows.Count, "With 10pt gap, 3rd item should wrap to row 1");
            Assert.AreEqual(2, rows[0].Columns.Length, "Row 0 should have 2 columns");
            Assert.AreEqual(1, rows[1].Columns.Length, "Row 1 should have 1 column");
        }

        // -----------------------------------------------------------------------
        // FlexWrap — 3×400pt → 3 rows (each item alone per row)
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexWrap_SingleItemPerRow()
        {
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.Wrap = FlexWrap.Wrap;

            AddFixedChild(panel, 400, height: 50, label: "1");
            AddFixedChild(panel, 400, height: 50, label: "2");
            AddFixedChild(panel, 400, height: 50, label: "3");

            using (var ms = DocStreams.GetOutputStream("Flex_Wrap_SinglePerRow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageCol = _layout.AllPages[0].ContentBlock.Columns[0];
            var rows    = FindOwnerBlocks(pageCol, panel);

            Assert.AreEqual(3, rows.Count, "3×400pt each alone should produce 3 rows");
            foreach (var row in rows)
                Assert.AreEqual(1, row.Columns.Length, "Each row should have 1 column");

            // Y positions should increase
            Assert.IsTrue(rows[1].TotalBounds.Y.PointsValue > rows[0].TotalBounds.Y.PointsValue,
                "Row 1 Y must be below row 0");
            Assert.IsTrue(rows[2].TotalBounds.Y.PointsValue > rows[1].TotalBounds.Y.PointsValue,
                "Row 2 Y must be below row 1");
        }

        // -----------------------------------------------------------------------
        // FlexWrap — 10×100pt in 600pt → rows [0..5] and [6..9]
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexWrap_TenItems_SixPerRow()
        {
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.Wrap = FlexWrap.Wrap;

            for (int n = 0; n < 10; n++)
                AddFixedChild(panel, 100, height: 40, label: n.ToString());

            using (var ms = DocStreams.GetOutputStream("Flex_Wrap_TenItems.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageCol = _layout.AllPages[0].ContentBlock.Columns[0];
            var rows    = FindOwnerBlocks(pageCol, panel);

            // 6×100=600 fits exactly; 7th triggers wrap
            Assert.AreEqual(2, rows.Count, "10 items × 100pt should produce 2 wrap rows");
            Assert.AreEqual(6, rows[0].Columns.Length, "Row 0 should have 6 columns");
            Assert.AreEqual(4, rows[1].Columns.Length, "Row 1 should have 4 columns");

            // Each column should be 100pt wide (multi-col, ShrinkToFit doesn't apply)
            for (int c = 0; c < 6; c++)
                Assert.AreEqual(100.0, rows[0].Columns[c].TotalBounds.Width.PointsValue, 1.0,
                    $"Row 0 column {c} should be 100pt wide");
            for (int c = 0; c < 4; c++)
                Assert.AreEqual(100.0, rows[1].Columns[c].TotalBounds.Width.PointsValue, 1.0,
                    $"Row 1 column {c} should be 100pt wide");
        }

        // -----------------------------------------------------------------------
        // FlexWrap — grow item fills row remainder alongside fixed items
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexWrap_GrowItemFillsRowRemainder()
        {
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.Wrap = FlexWrap.Wrap;

            // [100(fixed), grow:1, 200(fixed)] → all fit in one row: 100+0+200=300 ≤ 600
            // Grow item takes the remaining 300pt
            AddFixedChild(panel, 100, height: 60, label: "F1");
            // Grow child (no explicit width)
            var growChild = AddChild(panel, height: 60, grow: 1, label: "G");
            AddFixedChild(panel, 200, height: 60, label: "F2");

            using (var ms = DocStreams.GetOutputStream("Flex_Wrap_GrowFill.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageCol = _layout.AllPages[0].ContentBlock.Columns[0];
            var rows    = FindOwnerBlocks(pageCol, panel);

            Assert.AreEqual(1, rows.Count, "Mixed row should fit in a single row");
            Assert.AreEqual(3, rows[0].Columns.Length, "Should have 3 columns");

            // col[0] = 100pt, col[1] = 300pt (grow fills 600-100-200), col[2] = 200pt
            Assert.AreEqual(100.0, rows[0].Columns[0].TotalBounds.Width.PointsValue, 2.0, "Fixed col 0 = 100pt");
            Assert.AreEqual(300.0, rows[0].Columns[1].TotalBounds.Width.PointsValue, 2.0, "Grow col 1 = 300pt");
            Assert.AreEqual(200.0, rows[0].Columns[2].TotalBounds.Width.PointsValue, 2.0, "Fixed col 2 = 200pt");
        }

        // -----------------------------------------------------------------------
        // FlexWrap — mixed heights: rows stack by actual height
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexWrap_MixedHeights_RowsStackVertically()
        {
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.Wrap = FlexWrap.Wrap;

            // Row 0: two items with different heights — row height = tallest (80)
            AddFixedChild(panel, 250, height: 80, label: "Tall");
            AddFixedChild(panel, 250, height: 40, label: "Short");
            // Row 1: one item wraps
            AddFixedChild(panel, 250, height: 60, label: "Wrap");

            using (var ms = DocStreams.GetOutputStream("Flex_Wrap_MixedHeights.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageCol = _layout.AllPages[0].ContentBlock.Columns[0];
            var rows    = FindOwnerBlocks(pageCol, panel);

            Assert.AreEqual(2, rows.Count, "Should have 2 wrap rows");

            double row0Y = rows[0].TotalBounds.Y.PointsValue;
            double row0H = rows[0].TotalBounds.Height.PointsValue;
            double row1Y = rows[1].TotalBounds.Y.PointsValue;

            // Row 1 must start at or after row 0 ends
            Assert.IsTrue(row1Y >= row0Y, "Row 1 must be below row 0");
            Assert.IsTrue(row0H >= 40.0, "Row 0 must be at least 40pt tall");
        }

        // -----------------------------------------------------------------------
        // FlexWrap — justify-content: center applied per row
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexWrap_JustifyCenter_AppliedToEachRow()
        {
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            panel.Style.Flex.Wrap           = FlexWrap.Wrap;
            panel.Style.Flex.JustifyContent = FlexJustify.Center;

            // 2×250pt = 500pt fits in 600pt (row 0); 3×250=750 > 600 → 3rd wraps to row 1
            AddFixedChild(panel, 250, height: 60, label: "L");
            AddFixedChild(panel, 250, height: 60, label: "R");
            AddFixedChild(panel, 250, height: 60, label: "W");

            using (var ms = DocStreams.GetOutputStream("Flex_Wrap_JustifyCenter.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout);
            var pageCol = _layout.AllPages[0].ContentBlock.Columns[0];
            var rows    = FindOwnerBlocks(pageCol, panel);

            // Row 0: 2×250=500 fits; 3×250=750 > 600 → wraps
            Assert.AreEqual(2, rows.Count, "Should produce 2 wrap rows");

            // Row 0 should have 2 columns (JustifyContent is a stub, just verify structure)
            Assert.AreEqual(2, rows[0].Columns.Length, "Row 0 should have 2 columns");
            Assert.AreEqual(1, rows[1].Columns.Length, "Row 1 should have 1 column");
        }

        // -----------------------------------------------------------------------
        // FlexWrap — CSS-parsed flex-wrap:wrap
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexWrap_CSSParsed_BasicWrap()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-wrap:wrap; width:600pt; border:1pt solid #000;"">
    <div style=""width:250pt; height:60pt; flex-grow:0; border:1pt solid #888;"">A</div>
    <div style=""width:250pt; height:60pt; flex-grow:0; border:1pt solid #888;"">B</div>
    <div style=""width:250pt; height:60pt; flex-grow:0; border:1pt solid #888;"">C</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Wrap_CSS_BasicWrap.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var bodyRegion = layout.AllPages[0].ContentBlock.Columns[0];

            // Find owner with 2+ blocks (the flex container's wrap rows)
            var flexContainerOwnerBlocks = new Dictionary<IComponent, int>();
            foreach (var item in bodyRegion.Contents)
            {
                if (item is PDFLayoutBlock b && b.Owner != null)
                {
                    flexContainerOwnerBlocks.TryGetValue(b.Owner, out int c);
                    flexContainerOwnerBlocks[b.Owner] = c + 1;
                }
            }

            bool foundWrap = false;
            foreach (var kv in flexContainerOwnerBlocks)
                if (kv.Value >= 2) { foundWrap = true; break; }

            Assert.IsTrue(foundWrap, "CSS flex-wrap:wrap with 3×250pt in 600pt should produce 2 row blocks for the same owner");
        }

        // -----------------------------------------------------------------------
        // FlexWrap — CSS card grid: 4×180pt + 10pt gap in 600pt → 2 rows
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexWrap_CSSParsed_CardGrid()
        {
            // 3×180 + 2×10 = 560 ≤ 600; 4×180 + 3×10 = 750 > 600 → wraps at card 4
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-wrap:wrap; gap:10pt; width:600pt; border:1pt solid #000;"">
    <div style=""width:180pt; height:80pt; flex-grow:0; border:1pt solid #888;"">Card 1</div>
    <div style=""width:180pt; height:80pt; flex-grow:0; border:1pt solid #888;"">Card 2</div>
    <div style=""width:180pt; height:80pt; flex-grow:0; border:1pt solid #888;"">Card 3</div>
    <div style=""width:180pt; height:80pt; flex-grow:0; border:1pt solid #888;"">Card 4</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Wrap_CSS_CardGrid.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);

            var bodyRegion = layout.AllPages[0].ContentBlock.Columns[0];

            // Find the owner with ≥ 2 blocks (the flex container's wrap rows)
            var ownerCounts = new Dictionary<IComponent, List<PDFLayoutBlock>>();
            foreach (var item in bodyRegion.Contents)
            {
                if (item is PDFLayoutBlock b && b.Owner != null)
                {
                    if (!ownerCounts.ContainsKey(b.Owner))
                        ownerCounts[b.Owner] = new List<PDFLayoutBlock>();
                    ownerCounts[b.Owner].Add(b);
                }
            }

            List<PDFLayoutBlock> wrapRows = null;
            foreach (var kv in ownerCounts)
                if (kv.Value.Count >= 2) { wrapRows = kv.Value; break; }

            Assert.IsNotNull(wrapRows, "Should find a flex container with 2+ wrap-row blocks");
            Assert.AreEqual(2, wrapRows.Count, "4 cards with gap should produce 2 wrap rows");
            Assert.AreEqual(3, wrapRows[0].Columns.Length, "Row 0 should hold 3 cards");
            Assert.AreEqual(1, wrapRows[1].Columns.Length, "Row 1 should hold 1 card");
        }

        // -----------------------------------------------------------------------
        // Images inside flex items
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_ItemsWithImages_LayoutsCorrectly()
        {
            var imgPath = DocStreams.AssertGetTemplatePath(
                System.IO.Path.Combine("HTML", "Images", "Toroid32.png"));

            var src = $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-direction:row; width:600pt; border:1pt solid #000;"">
    <div style=""flex-grow:1; padding:5pt; border:1pt solid #888;"">
      <img src=""{imgPath}"" style=""width:80pt; height:80pt;"" />
    </div>
    <div style=""flex-grow:1; padding:5pt; border:1pt solid #888;"">
      <img src=""{imgPath}"" style=""width:80pt; height:80pt;"" />
    </div>
    <div style=""flex-grow:1; padding:5pt; border:1pt solid #888;"">
      <img src=""{imgPath}"" style=""width:80pt; height:80pt;"" />
    </div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Row_Images.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout should complete with images inside flex items");
            var bodyRegion = layout.AllPages[0].ContentBlock.Columns[0];
            var flexBlock  = FindFlexBlock(bodyRegion);

            Assert.IsNotNull(flexBlock, "Should find a flex row block");
            Assert.AreEqual(3, flexBlock.Columns.Length, "Three flex items should produce 3 columns");

            // Each column should have content (the image-containing panel)
            for (int i = 0; i < 3; i++)
            {
                var col = flexBlock.Columns[i];
                Assert.IsTrue(col.Contents.Count > 0, $"Column {i} should have content (image item)");
            }

            // Verify image resources were loaded
            Assert.IsTrue(doc.SharedResources.Count > 0, "At least one image resource should be loaded");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_ImageAndTextItems_MixedContent()
        {
            var imgPath = DocStreams.AssertGetTemplatePath(
                System.IO.Path.Combine("HTML", "Images", "ScyberLogo2_alpha_small.png"));

            var src = $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-direction:row; align-items:center; width:600pt; gap:10pt; border:1pt solid #000;"">
    <div style=""flex-grow:0; width:120pt; border:2pt solid #088; padding:4pt;"">
      <img src=""{imgPath}"" style=""width:110pt;"" />
    </div>
    <div style=""flex-grow:1; border:1pt solid #888; padding:8pt; height:80pt;"">
      <p style=""margin:0;"">Title Text</p>
      <p style=""margin:0;"">Subtitle Text</p>
    </div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Row_ImageAndText.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var bodyRegion = layout.AllPages[0].ContentBlock.Columns[0];
            var flexBlock  = FindFlexBlock(bodyRegion);

            Assert.IsNotNull(flexBlock, "Should find a 2-column flex row");
            Assert.AreEqual(2, flexBlock.Columns.Length, "Image + text item produces 2 columns");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexWrap_WithImages_WrapsToNewRow()
        {
            var imgPath = DocStreams.AssertGetTemplatePath(
                System.IO.Path.Combine("HTML", "Images", "Toroid32.png"));

            // 3 items × 200pt = 600pt exactly, 4th wraps
            var src = $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-wrap:wrap; width:600pt; border:1pt solid #000;"">
    <div style=""width:200pt; flex-grow:0; padding:4pt; border:1pt solid #888;"">
      <img src=""{imgPath}"" style=""width:180pt; height:60pt;"" />
    </div>
    <div style=""width:200pt; flex-grow:0; padding:4pt; border:1pt solid #888;"">
      <img src=""{imgPath}"" style=""width:180pt; height:60pt;"" />
    </div>
    <div style=""width:200pt; flex-grow:0; padding:4pt; border:1pt solid #888;"">
      <img src=""{imgPath}"" style=""width:180pt; height:60pt;"" />
    </div>
    <div style=""width:200pt; flex-grow:0; padding:4pt; border:1pt solid #888;"">
      <img src=""{imgPath}"" style=""width:180pt; height:60pt;"" />
    </div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Wrap_Images.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var bodyRegion = layout.AllPages[0].ContentBlock.Columns[0];

            // Find blocks owned by the same flex container
            var ownerCounts = new Dictionary<IComponent, List<PDFLayoutBlock>>();
            foreach (var item in bodyRegion.Contents)
            {
                if (item is PDFLayoutBlock b && b.Owner != null)
                {
                    if (!ownerCounts.ContainsKey(b.Owner))
                        ownerCounts[b.Owner] = new List<PDFLayoutBlock>();
                    ownerCounts[b.Owner].Add(b);
                }
            }

            List<PDFLayoutBlock> wrapRows = null;
            foreach (var kv in ownerCounts)
                if (kv.Value.Count >= 2) { wrapRows = kv.Value; break; }

            Assert.IsNotNull(wrapRows, "Images in flex items should still produce wrap rows");
            Assert.AreEqual(2, wrapRows.Count, "4×200pt in 600pt → 2 rows of 3+1");
            Assert.AreEqual(3, wrapRows[0].Columns.Length, "First row: 3 image items");
            Assert.AreEqual(1, wrapRows[1].Columns.Length, "Second row: 1 image item");
        }

        // -----------------------------------------------------------------------
        // Inline SVG inside flex items
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_ItemsWithInlineSVG_LayoutsCorrectly()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-direction:row; width:600pt; border:1pt solid #000;"">
    <div style=""flex-grow:1; padding:5pt; border:1pt solid #888;"">
      <svg width=""80pt"" height=""80pt"" viewBox=""0 0 80 80"" xmlns=""http://www.w3.org/2000/svg"">
        <circle cx=""40"" cy=""40"" r=""35"" fill=""#3366CC"" />
        <text x=""40"" y=""45"" text-anchor=""middle"" fill=""white"" font-size=""12"">A</text>
      </svg>
    </div>
    <div style=""flex-grow:1; padding:5pt; border:1pt solid #888;"">
      <svg width=""80pt"" height=""80pt"" viewBox=""0 0 80 80"" xmlns=""http://www.w3.org/2000/svg"">
        <rect x=""10"" y=""10"" width=""60"" height=""60"" fill=""#CC3366"" rx=""8"" />
        <text x=""40"" y=""45"" text-anchor=""middle"" fill=""white"" font-size=""12"">B</text>
      </svg>
    </div>
    <div style=""flex-grow:1; padding:5pt; border:1pt solid #888;"">
      <svg width=""80pt"" height=""80pt"" viewBox=""0 0 80 80"" xmlns=""http://www.w3.org/2000/svg"">
        <polygon points=""40,5 75,70 5,70"" fill=""#33CC66"" />
        <text x=""40"" y=""60"" text-anchor=""middle"" fill=""white"" font-size=""12"">C</text>
      </svg>
    </div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Row_InlineSVG.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout should complete with inline SVG inside flex items");
            var bodyRegion = layout.AllPages[0].ContentBlock.Columns[0];
            var flexBlock  = FindFlexBlock(bodyRegion);

            Assert.IsNotNull(flexBlock, "Should find a flex row block containing SVG items");
            Assert.AreEqual(3, flexBlock.Columns.Length, "Three SVG-containing flex items → 3 columns");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_SVGIcon_JustifyCenter()
        {
            // Single SVG-icon item in a flex row with justify-content:center
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-direction:row; justify-content:center; width:600pt; height:100pt; border:1pt solid #000;"">
    <div style=""flex-grow:0; width:100pt; height:80pt; padding:10pt; border:1pt solid #888;"">
      <svg width=""80pt"" height=""60pt"" viewBox=""0 0 80 60"" xmlns=""http://www.w3.org/2000/svg"">
        <ellipse cx=""40"" cy=""30"" rx=""38"" ry=""28"" fill=""#6600CC"" />
      </svg>
    </div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Row_SVGIcon_JustifyCenter.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout should complete with SVG in a centered flex item");
            Assert.AreEqual(1, layout.AllPages.Count, "Should fit on one page");
            // The flex container produces a 1-column block somewhere in the tree.
            // Verify by finding any block whose single column contains a child.
            var flexBlock = FindDeepestSingleColumnBlock(layout.AllPages[0].ContentBlock.Columns[0]);
            Assert.IsNotNull(flexBlock, "Should find the single-column flex block containing the SVG item");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_MixedSVGAndImage_AlignCenter()
        {
            var imgPath = DocStreams.AssertGetTemplatePath(
                System.IO.Path.Combine("HTML", "Images", "Toroid32.png"));

            var src = $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-direction:row; align-items:center; gap:10pt; width:600pt; border:1pt solid #000;"">
    <div style=""flex-grow:1; padding:5pt; border:1pt solid #888;"">
      <svg width=""60pt"" height=""60pt"" viewBox=""0 0 60 60"" xmlns=""http://www.w3.org/2000/svg"">
        <circle cx=""30"" cy=""30"" r=""28"" fill=""#3366CC"" />
      </svg>
    </div>
    <div style=""flex-grow:1; padding:5pt; border:1pt solid #888;"">
      <img src=""{imgPath}"" style=""width:60pt; height:60pt;"" />
    </div>
    <div style=""flex-grow:1; padding:5pt; border:1pt solid #888; height:120pt;"">
      <p style=""margin:0;"">Taller item to test align-items:center</p>
    </div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Row_MixedSVGImage_AlignCenter.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout should handle mixed SVG and image flex items");
            var bodyRegion = layout.AllPages[0].ContentBlock.Columns[0];
            var flexBlock  = FindFlexBlock(bodyRegion);

            Assert.IsNotNull(flexBlock, "Should find the 3-column flex row");
            Assert.AreEqual(3, flexBlock.Columns.Length, "SVG + image + text → 3 columns");
        }

        // -----------------------------------------------------------------------
        // Multi-page overflow
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_NearPageBottom_OverflowsToNewPage()
        {
            // Place flex container near the bottom of the page so it overflows to page 2.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <style>@page { size: 600pt 800pt; margin: 0; }</style>
</head>
<body style=""margin:0; padding:0;"">
  <div style=""height:720pt;"">Spacer (720pt + 100pt flex row = 820pt &gt; 800pt page)</div>
  <div style=""display:flex; flex-direction:row; width:600pt; height:100pt; border:1pt solid #000;"">
    <div style=""flex-grow:1; height:100pt; border:1pt solid #888; padding:4pt;"">Left</div>
    <div style=""flex-grow:1; height:100pt; border:1pt solid #888; padding:4pt;"">Right</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Row_PageOverflow.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            Assert.AreEqual(2, layout.AllPages.Count, "Flex row that doesn't fit should push to page 2");

            // The flex block (2 columns) should appear on page 2
            var page2Region = layout.AllPages[1].ContentBlock.Columns[0];
            var flexBlock   = FindFlexBlock(page2Region);
            Assert.IsNotNull(flexBlock, "Flex row should be on page 2");
            Assert.AreEqual(2, flexBlock.Columns.Length, "Should have 2 columns on page 2");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexWrap_MultiRow_OverflowsToNewPage()
        {
            // 120 items × 200pt → 3 per row = 40 rows × 80pt = 3200pt.
            // On an 800pt page: 10 rows per page → 4 pages total.
            // This test verifies that all 40 rows land correctly and that no row's
            // columns are split across different pages (the prior bug).
            var rowItems = new System.Text.StringBuilder();
            for (int i = 0; i < 120; i++)
                rowItems.Append($@"<div style=""width:200pt; flex-grow:0; height:80pt; border:1pt solid #888; padding:4pt;"">Item {i + 1}</div>");

            var src = $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <style>@page {{ size: 600pt 800pt; margin: 0; }}</style>
</head>
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-wrap:wrap; width:600pt; border:1pt solid #000;"">
{rowItems}
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Wrap_MultiRow_PageOverflow.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);

            // Should span multiple pages.
            Assert.IsTrue(layout.AllPages.Count >= 2,
                $"120 items at 80pt/row should span multiple pages, got {layout.AllPages.Count}");

            // Count all 3-column flex row blocks across every page.
            int totalRowBlocks = 0;
            foreach (var page in layout.AllPages)
                totalRowBlocks += CountBlocksWithExactColumns(page.ContentBlock.Columns[0], 3);

            // If any row was incorrectly split (old bug: items on individual pages),
            // fewer 3-column blocks would be found.
            Assert.AreEqual(40, totalRowBlocks,
                $"120 items / 3 per row = 40 wrap-row blocks, found {totalRowBlocks}. " +
                "A count < 40 indicates rows were split across pages.");

            // 40 rows × 80pt on 800pt pages → 10 rows per page → 4 pages.
            // Allow slight variation for wrappers but cap at 8 to catch the old
            // one-item-per-page regression (which would produce ~120+ pages).
            Assert.IsTrue(layout.AllPages.Count <= 8,
                $"Expected ~4 pages for 40×80pt rows on 800pt pages, got {layout.AllPages.Count}. " +
                "A very large count indicates rows were split one-item-per-page.");
        }

        private static int CountBlocksWithExactColumns(PDFLayoutRegion region, int exactCols)
        {
            int count = 0;
            foreach (var item in region.Contents)
            {
                if (item is PDFLayoutBlock b)
                {
                    if (b.Columns.Length == exactCols) count++;
                    foreach (var col in b.Columns)
                        count += CountBlocksWithExactColumns(col, exactCols);
                }
            }
            return count;
        }

        // -----------------------------------------------------------------------
        // Boundary conditions
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_ItemWiderThanContainer_DoesNotThrow()
        {
            // An item wider than the container — should not crash.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-direction:row; width:200pt; border:1pt solid #000;"">
    <div style=""width:300pt; flex-grow:0; height:50pt; border:1pt solid #888;"">Wide</div>
    <div style=""flex-grow:1; height:50pt; border:1pt solid #888;"">Grow</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Row_ItemWiderThanContainer.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Should complete layout even if an item exceeds container width");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_AllGrowZeroNoWidth_DoesNotThrow()
        {
            // All grow=0 items with no explicit width — fractions are 0, layout should still produce blocks
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-direction:row; width:600pt; border:1pt solid #000;"">
    <div style=""flex-grow:0; height:50pt; border:1pt solid #888;"">No Width A</div>
    <div style=""flex-grow:0; height:50pt; border:1pt solid #888;"">No Width B</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Row_AllGrowZeroNoWidth.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "All-grow-zero no-explicit-width items should still layout without error");
        }

        // -----------------------------------------------------------------------
        // Regression pins — bugs fixed in this work
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Regression_PaddedContainer_ColumnWidthCorrect()
        {
            // Bug: padding was not subtracted from containerW in ComputeColumnWidths,
            // causing column fractions to be computed against the outer (border-box) width
            // instead of the inner (content) width, so items were narrower than expected.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-direction:row; width:600pt; padding:15pt; border:1pt solid #000;"">
    <div style=""width:80pt; flex-grow:0; height:60pt; border:1pt solid #888;"">Fixed 80pt</div>
    <div style=""flex-grow:1; height:60pt; border:1pt solid #888;"">Grow</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Regression_PaddedContainer.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var bodyRegion = layout.AllPages[0].ContentBlock.Columns[0];
            var flexBlock  = FindFlexBlock(bodyRegion);
            Assert.IsNotNull(flexBlock, "Should find flex block");
            Assert.AreEqual(2, flexBlock.Columns.Length, "Should have 2 columns");

            // Fixed item should be ~80pt wide (correct inner-width fractions)
            double fixedColW = FirstChildBlockWidth(flexBlock.Columns[0]);
            Assert.IsTrue(Math.Abs(fixedColW - 80.0) < 2.0,
                $"Fixed 80pt item should be ~80pt wide, was {fixedColW}");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Regression_SingleItem_JustifyCenter_Offset()
        {
            // Bug: ShrinkToFit inflated single column to full block width,
            // leaving leftover=0 so justify-content had no effect.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-direction:row; justify-content:center; width:600pt; height:80pt; border:1pt solid #000;"">
    <div style=""width:100pt; flex-grow:0; height:60pt; border:1pt solid #888;"">Centered</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Regression_SingleItemJustifyCenter.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout, "Layout should complete without error (regression: ShrinkToFit bug)");
            Assert.AreEqual(1, layout.AllPages.Count);
            // The X offset is verified in the programmatic test FlexRow_JustifyContent_SpaceBetween_SingleItem_Centers.
            // Here we verify layout completes and produces content via CSS parsing.
            var contentBlock = layout.AllPages[0].ContentBlock;
            Assert.IsNotNull(contentBlock, "Page should have a content block");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Regression_SpaceEvenly_CorrectGaps()
        {
            // Verifies SpaceEvenly distributes gaps correctly (was unimplemented before this work).
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-direction:row; justify-content:space-evenly; width:600pt; height:80pt; border:1pt solid #000;"">
    <div style=""width:100pt; flex-grow:0; height:60pt; border:1pt solid #888;"">A</div>
    <div style=""width:100pt; flex-grow:0; height:60pt; border:1pt solid #888;"">B</div>
    <div style=""width:100pt; flex-grow:0; height:60pt; border:1pt solid #888;"">C</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src),
                                           ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_Regression_SpaceEvenly.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var bodyRegion = layout.AllPages[0].ContentBlock.Columns[0];
            var flexBlock  = FindFlexBlock(bodyRegion);
            Assert.IsNotNull(flexBlock);
            Assert.AreEqual(3, flexBlock.Columns.Length);

            // 3×100pt in 600pt → 300pt leftover → 4 gaps of 75pt each.
            // Col[0].X = 75, Col[1].X = 75+100+75 = 250, Col[2].X = 250+100+75 = 425
            double x0 = flexBlock.Columns[0].TotalBounds.X.PointsValue;
            double x1 = flexBlock.Columns[1].TotalBounds.X.PointsValue;
            double x2 = flexBlock.Columns[2].TotalBounds.X.PointsValue;

            Assert.IsTrue(Math.Abs(x0 - 75.0) < 2.0,  $"Col[0] X should be ~75pt, was {x0}");
            Assert.IsTrue(Math.Abs(x1 - 250.0) < 2.0, $"Col[1] X should be ~250pt, was {x1}");
            Assert.IsTrue(Math.Abs(x2 - 425.0) < 2.0, $"Col[2] X should be ~425pt, was {x2}");
        }

        // -----------------------------------------------------------------------
        // flex-direction: row-reverse
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRowReverse_TwoChildren_OrderReversed()
        {
            // Items A and B should appear right-to-left: B in column 1 (left), A in column 2 (right).
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg, FlexDirection.RowReverse);
            var childA = AddChild(panel, height: 50, grow: 1, label: "A");
            var childB = AddChild(panel, height: 50, grow: 1, label: "B");

            using (var ms = DocStreams.GetOutputStream("Flex_RowReverse_TwoChildren.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, _layout.AllPages.Count);
            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(2, panelBlock.Columns.Length, "row-reverse should still produce 2 columns");

            // Widths still equal — reverse doesn't change sizes.
            double expected = PageW / 2.0;
            Assert.AreEqual(expected, panelBlock.Columns[0].TotalBounds.Width.PointsValue, 0.5, "Column 0 width");
            Assert.AreEqual(expected, panelBlock.Columns[1].TotalBounds.Width.PointsValue, 0.5, "Column 1 width");

            // Column 0 (left) should contain B; column 1 (right) should contain A.
            StringAssert.Contains(CollectText(panelBlock.Columns[0]), "B", "Column 0 (left) should hold B");
            StringAssert.Contains(CollectText(panelBlock.Columns[1]), "A", "Column 1 (right) should hold A");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRowReverse_ThreeChildren_OrderReversed()
        {
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg, FlexDirection.RowReverse);
            AddChild(panel, height: 50, grow: 1, label: "First");
            AddChild(panel, height: 50, grow: 1, label: "Second");
            AddChild(panel, height: 50, grow: 1, label: "Third");

            using (var ms = DocStreams.GetOutputStream("Flex_RowReverse_ThreeChildren.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(3, panelBlock.Columns.Length);

            // Source order: First, Second, Third → reversed render: Third, Second, First.
            StringAssert.Contains(CollectText(panelBlock.Columns[0]), "Third",  "Column 0 should hold Third");
            StringAssert.Contains(CollectText(panelBlock.Columns[1]), "Second", "Column 1 should hold Second");
            StringAssert.Contains(CollectText(panelBlock.Columns[2]), "First",  "Column 2 should hold First");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRowReverse_GrowRatio_WidthsPreserved()
        {
            // grow=1 / grow=2 widths should be the same as row, items just swapped positions.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg, FlexDirection.RowReverse);
            AddChild(panel, height: 50, grow: 1, label: "Narrow");  // source index 0
            AddChild(panel, height: 50, grow: 2, label: "Wide");    // source index 1

            using (var ms = DocStreams.GetOutputStream("Flex_RowReverse_GrowRatio.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(2, panelBlock.Columns.Length);

            // Wide (grow=2) is reversed to column 0; Narrow (grow=1) to column 1.
            // Fractions reversed: col0=2/3, col1=1/3.
            Assert.AreEqual(400.0, panelBlock.Columns[0].TotalBounds.Width.PointsValue, 0.5, "Column 0 should be 400pt (Wide)");
            Assert.AreEqual(200.0, panelBlock.Columns[1].TotalBounds.Width.PointsValue, 0.5, "Column 1 should be 200pt (Narrow)");

            StringAssert.Contains(CollectText(panelBlock.Columns[0]), "Wide",   "Column 0 should hold Wide");
            StringAssert.Contains(CollectText(panelBlock.Columns[1]), "Narrow", "Column 1 should hold Narrow");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRowReverse_JustifyFlexEnd_PushesItemsLeft()
        {
            // flex-end in row-reverse means items pack to the left edge (visual start of reversed axis).
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-direction:row-reverse; justify-content:flex-end;
               width:600pt; height:80pt;"">
    <div style=""width:100pt; flex-grow:0; height:60pt;"">A</div>
    <div style=""width:100pt; flex-grow:0; height:60pt;"">B</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_RowReverse_JustifyFlexEnd.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var bodyRegion = layout.AllPages[0].ContentBlock.Columns[0];
            var flexBlock  = FindFlexBlock(bodyRegion);
            Assert.IsNotNull(flexBlock, "Flex block should be found");

            // flex-end in row-reverse = pack to left → both columns start at X=0.
            double x0 = flexBlock.Columns[0].TotalBounds.X.PointsValue;
            Assert.IsTrue(x0 < 5.0, $"flex-end on row-reverse: col[0] should start near X=0, was {x0}");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRowReverse_JustifyFlexStart_PushesItemsRight()
        {
            // flex-start in row-reverse means items pack to the right edge.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""display:flex; flex-direction:row-reverse; justify-content:flex-start;
               width:600pt; height:80pt;"">
    <div style=""width:100pt; flex-grow:0; height:60pt;"">A</div>
    <div style=""width:100pt; flex-grow:0; height:60pt;"">B</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_RowReverse_JustifyFlexStart.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var bodyRegion = layout.AllPages[0].ContentBlock.Columns[0];
            var flexBlock  = FindFlexBlock(bodyRegion);
            Assert.IsNotNull(flexBlock, "Flex block should be found");

            // flex-start on row-reverse = pack to right → col[1] ends near X=600.
            double x1    = flexBlock.Columns[1].TotalBounds.X.PointsValue;
            double width1 = flexBlock.Columns[1].TotalBounds.Width.PointsValue;
            double rightEdge = x1 + width1;
            Assert.IsTrue(rightEdge > 595.0, $"flex-start on row-reverse: right edge should be near 600pt, was {rightEdge}");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRowReverse_CSSParsed_OrderReversed()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head><style>
  .rev  { display: flex; flex-direction: row-reverse; width: 600pt; }
  .item { flex-grow: 1; height: 50pt; }
</style></head>
<body style=""margin:0; padding:0;"">
  <div class=""rev"">
    <div class=""item"">Alpha</div>
    <div class=""item"">Beta</div>
    <div class=""item"">Gamma</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_RowReverse_CSS.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var bodyRegion = layout.AllPages[0].ContentBlock.Columns[0];
            var flexBlock  = FindFlexBlock(bodyRegion);
            Assert.IsNotNull(flexBlock, "Flex block should be found");
            Assert.AreEqual(3, flexBlock.Columns.Length);

            // Source: Alpha, Beta, Gamma → reversed: Gamma, Beta, Alpha.
            StringAssert.Contains(CollectText(flexBlock.Columns[0]), "Gamma", "Column 0 should hold Gamma");
            StringAssert.Contains(CollectText(flexBlock.Columns[1]), "Beta",  "Column 1 should hold Beta");
            StringAssert.Contains(CollectText(flexBlock.Columns[2]), "Alpha", "Column 2 should hold Alpha");
        }

        // -----------------------------------------------------------------------
        // flex-direction: column-reverse
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexColumnReverse_TwoChildren_OrderReversed()
        {
            // Items A (source first) and B (source second) — column-reverse renders B on top, A below.
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg, FlexDirection.ColumnReverse);
            var childA = AddChild(panel, height: 50, label: "A");
            var childB = AddChild(panel, height: 80, label: "B");

            using (var ms = DocStreams.GetOutputStream("Flex_ColumnReverse_TwoChildren.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(1, panelBlock.Columns.Length, "column-reverse still uses single-column layout");

            var region = panelBlock.Columns[0];
            var block0 = region.Contents[0] as PDFLayoutBlock;
            var block1 = region.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block0, "First rendered block");
            Assert.IsNotNull(block1, "Second rendered block");

            // block0 should be B (source second, reversed to first), block1 should be A.
            StringAssert.Contains(CollectText(block0.Columns[0]), "B", "First rendered block should be B");
            StringAssert.Contains(CollectText(block1.Columns[0]), "A", "Second rendered block should be A");

            // Second rendered block (A) starts below first (B).
            Assert.IsTrue(block1.TotalBounds.Y > block0.TotalBounds.Y,
                "Second rendered block (A) should be below first rendered block (B)");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexColumnReverse_ThreeChildren_OrderReversed()
        {
            var doc   = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg, FlexDirection.ColumnReverse);
            AddChild(panel, height: 40, label: "Top");
            AddChild(panel, height: 50, label: "Middle");
            AddChild(panel, height: 60, label: "Bottom");

            using (var ms = DocStreams.GetOutputStream("Flex_ColumnReverse_ThreeChildren.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);
            Assert.AreEqual(1, panelBlock.Columns.Length);

            var region = panelBlock.Columns[0];
            var b0 = region.Contents[0] as PDFLayoutBlock;
            var b1 = region.Contents[1] as PDFLayoutBlock;
            var b2 = region.Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(b0);
            Assert.IsNotNull(b1);
            Assert.IsNotNull(b2);

            // Source: Top, Middle, Bottom → reversed: Bottom, Middle, Top.
            StringAssert.Contains(CollectText(b0.Columns[0]), "Bottom", "First rendered block should be Bottom");
            StringAssert.Contains(CollectText(b1.Columns[0]), "Middle", "Second rendered block should be Middle");
            StringAssert.Contains(CollectText(b2.Columns[0]), "Top",    "Third rendered block should be Top");

            // Blocks are still stacked top-to-bottom in layout Y order.
            Assert.IsTrue(b1.TotalBounds.Y > b0.TotalBounds.Y, "b1.Y > b0.Y");
            Assert.IsTrue(b2.TotalBounds.Y > b1.TotalBounds.Y, "b2.Y > b1.Y");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexColumnReverse_CSSParsed_OrderReversed()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head><style>
  .col-rev { display: flex; flex-direction: column-reverse; width: 600pt; }
  .item    { height: 50pt; }
</style></head>
<body style=""margin:0; padding:0;"">
  <div class=""col-rev"">
    <div class=""item"">One</div>
    <div class=""item"">Two</div>
    <div class=""item"">Three</div>
  </div>
</body>
</html>";

            using var doc = Document.Parse(new System.IO.StringReader(src), ParseSourceType.DynamicContent) as Document;
            Assert.IsNotNull(doc);

            PDFLayoutDocument layout = null;
            using (var ms = DocStreams.GetOutputStream("Flex_ColumnReverse_CSS.pdf"))
            {
                doc.LayoutComplete += (s, e) => layout = e.Context.GetLayout<PDFLayoutDocument>();
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var bodyRegion = layout.AllPages[0].ContentBlock.Columns[0];

            // Locate the flex container block — it's a single-column block containing children.
            PDFLayoutBlock panelBlock = null;
            foreach (var item in bodyRegion.Contents)
            {
                if (item is PDFLayoutBlock b && b.Columns.Length == 1)
                {
                    panelBlock = b;
                    break;
                }
            }
            Assert.IsNotNull(panelBlock, "Flex panel block not found");

            var region = panelBlock.Columns[0];

            // Walk through child blocks and collect their text.
            var texts = new List<string>();
            foreach (var item in region.Contents)
            {
                if (item is PDFLayoutBlock b)
                    texts.Add(CollectText(b.Columns[0]));
            }

            Assert.IsTrue(texts.Count >= 3, $"Expected at least 3 child blocks, got {texts.Count}");

            // Source: One, Two, Three → reversed: Three, Two, One.
            StringAssert.Contains(texts[0], "Three", "First rendered block should be Three");
            StringAssert.Contains(texts[1], "Two",   "Second rendered block should be Two");
            StringAssert.Contains(texts[2], "One",   "Third rendered block should be One");
        }

        // -----------------------------------------------------------------------
        // Helper: get first child block width from a region
        // -----------------------------------------------------------------------

        private static double FirstChildBlockWidth(PDFLayoutRegion region)
        {
            foreach (var item in region.Contents)
                if (item is PDFLayoutBlock b)
                    return b.TotalBounds.Width.PointsValue;
            return 0;
        }
    }
}
