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
            panel.Width  = width;
            pg.Contents.Add(panel);
            return panel;
        }

        private static Panel AddChild(Panel parent, double? height = null, double grow = 1.0)
        {
            var child = new Panel();
            child.Style.Flex.Grow = grow;
            if (height.HasValue)
                child.Height = height.Value;
            parent.Contents.Add(child);
            return child;
        }

        // -----------------------------------------------------------------------
        // FlexRow — basic two-child layout
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void FlexRow_TwoChildren_SideBySide()
        {
            var doc = CreateDoc(out var pg);
            var panel = CreateFlexContainer(pg);
            var child0 = AddChild(panel, height: 50, grow: 1);
            var child1 = AddChild(panel, height: 50, grow: 1);

            using (var ms = DocStreams.GetOutputStream("Flex_Row_TwoChildren_Equal.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);

            var lpg       = _layout.AllPages[0];
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
            Assert.AreEqual(0.0, col0.TotalBounds.X.PointsValue, 0.5, "Column 0 should start at X=0");
            Assert.AreEqual(expected, col1.TotalBounds.X.PointsValue, 0.5, "Column 1 should start at X=half-width");

            // Each column contains the child block (there may also be trailing lines — find by owner).
            var childBlock0 = FindBlockOwner(col0, child0);
            var childBlock1 = FindBlockOwner(col1, child1);
            Assert.IsNotNull(childBlock0, "Column 0 should contain child0's layout block");
            Assert.IsNotNull(childBlock1, "Column 1 should contain child1's layout block");
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
            var child0 = AddChild(panel, height: 50, grow: 1);
            var child1 = AddChild(panel, height: 50, grow: 1);

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
            var child0 = AddChild(panel, height: 50, grow: 1);
            var child1 = AddChild(panel, height: 50, grow: 2);

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
            AddChild(panel, height: 50);
            AddChild(panel, height: 50);
            AddChild(panel, height: 50);

            using (var ms = DocStreams.GetOutputStream("Flex_Row_ThreeChildren.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var panelBlock = _layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock);

            Assert.AreEqual(3, panelBlock.Columns.Length, "Three children should produce three columns");

            double expected = PageW / 3.0;
            for (int i = 0; i < 3; i++)
                Assert.AreEqual(expected, panelBlock.Columns[i].TotalBounds.Width.PointsValue, 0.5,
                    $"Column {i} should be 200pt");
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
            AddChild(panel, height: 50);
            AddChild(panel, height: 80);

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
            // Two child blocks stacked.
            Assert.AreEqual(2, region.Contents.Count, "Column region should contain two child blocks");

            var block0 = region.Contents[0] as PDFLayoutBlock;
            var block1 = region.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block0, "First child block should not be null");
            Assert.IsNotNull(block1, "Second child block should not be null");

            // Second child should start below the first.
            Assert.IsTrue(block1.TotalBounds.Y > block0.TotalBounds.Y,
                "Second child Y should be greater than first child Y");
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
            AddChild(panel, height: 50);
            AddChild(panel, height: 50);

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
            panel.Contents.Add(child0);

            var child1 = new Panel();
            child1.Height = 50;
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
  <div style=""display:flex; flex-direction:row; width:600pt;"">
    <div style=""flex-grow:1; height:50pt;"" />
    <div style=""flex-grow:1; height:50pt;"" />
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
            AddChild(panel, height: 50, grow: 1);

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
        }
    }
}
