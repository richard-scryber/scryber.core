using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using Scryber.Drawing;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Layout tests for column-fill: balance.
    /// All HTML tests include visible borders so output PDFs can be visually inspected.
    ///
    /// Naming convention: ColumnBalance_[scenario]_[expectation]
    /// </summary>
    [TestClass()]
    public class ColumnBalance_Tests
    {
        const string TestCategoryName = "Layout ColumnBalance";

        PDFLayoutDocument layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        // =====================================================================
        // Helpers
        // =====================================================================

        /// <summary>Returns the content block for the first page.</summary>
        private PDFLayoutBlock GetPageContentBlock()
        {
            return layout.AllPages[0].ContentBlock;
        }

        /// <summary>Returns the first layout block inside body's first column.</summary>
        private PDFLayoutBlock GetContainerBlock(int bodyItemIndex = 0)
        {
            var body = layout.AllPages[0].ContentBlock;
            return body.Columns[0].Contents[bodyItemIndex] as PDFLayoutBlock;
        }

        // =====================================================================
        // 1. CSS parsing — column-fill: balance is parsed correctly
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_01_CSSParsed_Balance()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div id=""container"" style=""column-count:2; column-fill:balance; width:400pt; height:200pt;
             border:2pt solid #000000;"">
    <div style=""height:60pt; border:1pt solid #ff0000;"">Block 1</div>
    <div style=""height:60pt; border:1pt solid #0000ff;"">Block 2</div>
    <div style=""height:60pt; border:1pt solid #00aa00;"">Block 3</div>
    <div style=""height:60pt; border:1pt solid #aa6600;"">Block 4</div>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ColumnBalance_01_CSSParsed_Balance.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var container = GetContainerBlock();
            Assert.IsNotNull(container, "Container block should exist");

            Assert.AreEqual(ColumnFillMode.Balance, container.ColumnOptions.FillMode,
                "FillMode should be Balance");
            Assert.AreEqual(2, container.ColumnOptions.ColumnCount,
                "Should have 2 columns");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_02_CSSParsed_Auto()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div id=""container"" style=""column-count:2; column-fill:auto; width:400pt; height:200pt;
             border:2pt solid #000000;"">
    <div style=""height:60pt; border:1pt solid #ff0000;"">Block 1</div>
    <div style=""height:60pt; border:1pt solid #0000ff;"">Block 2</div>
    <div style=""height:60pt; border:1pt solid #00aa00;"">Block 3</div>
    <div style=""height:60pt; border:1pt solid #aa6600;"">Block 4</div>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ColumnBalance_02_CSSParsed_Auto.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var container = GetContainerBlock();
            Assert.IsNotNull(container, "Container block should exist");

            Assert.AreEqual(ColumnFillMode.Auto, container.ColumnOptions.FillMode,
                "FillMode should default to Auto");
        }

        // =====================================================================
        // 2. Two columns — 4 equal blocks split evenly (2 per column)
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_03_TwoColumns_FourEqualBlocks_Balanced()
        {
            // 4 x 100pt blocks in 2 columns. Total = 400pt, target = 200pt per column.
            // Expected: col0 = 2 blocks (200pt), col1 = 2 blocks (200pt).
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div id=""container""
       style=""column-count:2; column-fill:balance; column-gap:10;
              border:2pt solid #000000;"">
    <div style=""height:100pt; border:2pt solid #ff0000;"">Block 1</div>
    <div style=""height:100pt; border:2pt solid #0000ff;"">Block 2</div>
    <div style=""height:100pt; border:2pt solid #00aa00;"">Block 3</div>
    <div style=""height:100pt; border:2pt solid #aa6600;"">Block 4</div>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ColumnBalance_03_TwoColumns_FourEqualBlocks.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var container = GetContainerBlock();
            Assert.IsNotNull(container, "Container block should exist");
            Assert.AreEqual(2, container.Columns.Length, "Should have 2 columns");

            // Each column should have 2 items
            Assert.AreEqual(2, container.Columns[0].Contents.Count, "Col0 should have 2 items");
            Assert.AreEqual(2, container.Columns[1].Contents.Count, "Col1 should have 2 items");

            // Heights should be equal
            var col0Height = container.Columns[0].UsedSize.Height;
            var col1Height = container.Columns[1].UsedSize.Height;
            Assert.AreEqual(col0Height.PointsValue, col1Height.PointsValue, 1.0,
                "Column heights should be balanced");
        }

        // =====================================================================
        // 3. Two columns — 6 equal blocks split 3/3 (perfect balance)
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_04_TwoColumns_SixEqualBlocks_Balanced()
        {
            // 6 x 50pt blocks in 2 columns. Total = 300pt, target = 150pt per column.
            // Expected: col0 = 3 blocks (150pt), col1 = 3 blocks (150pt).
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div id=""container""
       style=""column-count:2; column-fill:balance; column-gap:0; width:400pt;
              border:2pt solid #000000;"">
    <div style=""height:50pt; border:1pt solid #ff0000;"">Block 1</div>
    <div style=""height:50pt; border:1pt solid #cc0044;"">Block 2</div>
    <div style=""height:50pt; border:1pt solid #0000ff;"">Block 3</div>
    <div style=""height:50pt; border:1pt solid #0066cc;"">Block 4</div>
    <div style=""height:50pt; border:1pt solid #00aa00;"">Block 5</div>
    <div style=""height:50pt; border:1pt solid #007700;"">Block 6</div>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ColumnBalance_04_TwoColumns_SixEqualBlocks.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var container = GetContainerBlock();
            Assert.IsNotNull(container, "Container block should exist");
            Assert.AreEqual(2, container.Columns.Length, "Should have 2 columns");

            Assert.AreEqual(3, container.Columns[0].Contents.Count, "Col0 should have 3 items");
            Assert.AreEqual(3, container.Columns[1].Contents.Count, "Col1 should have 3 items");

            var col0Height = container.Columns[0].UsedSize.Height;
            var col1Height = container.Columns[1].UsedSize.Height;
            Assert.AreEqual(col0Height.PointsValue, col1Height.PointsValue, 1.0,
                "Column heights should be perfectly balanced at 150pt each");
        }

        // =====================================================================
        // 4. Three columns — 9 equal blocks split 3/3/3
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_05_ThreeColumns_NineEqualBlocks_Balanced()
        {
            // 9 x 30pt blocks in 3 columns. Total = 270pt, target = 90pt per column.
            // Expected: each column gets 3 blocks (90pt).
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div id=""container""
       style=""column-count:3; column-fill:balance; column-gap:0; width:600pt;
              border:2pt solid #000000;"">
    <div style=""height:30pt; border:1pt solid #ff0000;"">Block 1</div>
    <div style=""height:30pt; border:1pt solid #cc0000;"">Block 2</div>
    <div style=""height:30pt; border:1pt solid #990000;"">Block 3</div>
    <div style=""height:30pt; border:1pt solid #0000ff;"">Block 4</div>
    <div style=""height:30pt; border:1pt solid #0000cc;"">Block 5</div>
    <div style=""height:30pt; border:1pt solid #000099;"">Block 6</div>
    <div style=""height:30pt; border:1pt solid #00aa00;"">Block 7</div>
    <div style=""height:30pt; border:1pt solid #008800;"">Block 8</div>
    <div style=""height:30pt; border:1pt solid #006600;"">Block 9</div>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ColumnBalance_05_ThreeColumns_NineEqual.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var container = GetContainerBlock();
            Assert.IsNotNull(container, "Container block should exist");
            Assert.AreEqual(3, container.Columns.Length, "Should have 3 columns");

            Assert.AreEqual(3, container.Columns[0].Contents.Count, "Col0 should have 3 items");
            Assert.AreEqual(3, container.Columns[1].Contents.Count, "Col1 should have 3 items");
            Assert.AreEqual(3, container.Columns[2].Contents.Count, "Col2 should have 3 items");

            // All columns should have equal height
            var h0 = container.Columns[0].UsedSize.Height.PointsValue;
            var h1 = container.Columns[1].UsedSize.Height.PointsValue;
            var h2 = container.Columns[2].UsedSize.Height.PointsValue;
            Assert.AreEqual(h0, h1, 1.0, "Col0 and Col1 should have equal height");
            Assert.AreEqual(h1, h2, 1.0, "Col1 and Col2 should have equal height");
        }

        // =====================================================================
        // 5. Compare: auto vs balance with same content
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_06_AutoVsBalance_DifferentDistribution()
        {
            // With column-fill:auto all content packs into first column then overflows.
            // With column-fill:balance content is distributed evenly.
            // Use a constrained height so overflow actually occurs with auto.
            const double blockHeight = 80;
            const double containerHeight = 200; // fits 2 blocks in one column before overflow

            var srcAuto = $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""column-count:2; column-fill:auto; column-gap:0; width:400pt; height:{containerHeight}pt;
              border:2pt solid #880000;"">
    <div style=""height:{blockHeight}pt; border:1pt solid #ff0000;"">Block 1</div>
    <div style=""height:{blockHeight}pt; border:1pt solid #cc0000;"">Block 2</div>
    <div style=""height:{blockHeight}pt; border:1pt solid #0000ff;"">Block 3</div>
    <div style=""height:{blockHeight}pt; border:1pt solid #0000cc;"">Block 4</div>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(srcAuto))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ColumnBalance_06a_Auto.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var containerAuto = GetContainerBlock();
            int col0AutoCount = containerAuto.Columns[0].Contents.Count;
            int col1AutoCount = containerAuto.Columns[1].Contents.Count;
            // With auto: col0 fills up, col1 gets overflow
            Assert.IsTrue(col0AutoCount == col1AutoCount,
                "With auto fill, col0 should have at least as many items as col1");

            // Now test with balance
            var srcBalance = $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""column-count:2; column-fill:balance; column-gap:0; width:400pt;
              border:2pt solid #000088;"">
    <div style=""height:{blockHeight}pt; border:1pt solid #ff0000;"">Block 1</div>
    <div style=""height:{blockHeight}pt; border:1pt solid #cc0000;"">Block 2</div>
    <div style=""height:{blockHeight}pt; border:1pt solid #0000ff;"">Block 3</div>
    <div style=""height:{blockHeight}pt; border:1pt solid #0000cc;"">Block 4</div>
  </div>
</body>
</html>";

            layout = null;
            using (var reader = new System.IO.StringReader(srcBalance))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ColumnBalance_06b_Balance.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var containerBalance = GetContainerBlock();
            // With balance: 4 blocks of 80pt total=320pt, target=160pt
            // col0 gets items until 160pt, col1 gets the rest
            Assert.AreEqual(2, containerBalance.Columns[0].Contents.Count, "Balance col0 should have 2 items");
            Assert.AreEqual(2, containerBalance.Columns[1].Contents.Count, "Balance col1 should have 2 items");
        }

        // =====================================================================
        // 6. Balance respects block heights — larger first block
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_07_LargeFirstBlock_SpillsEarly()
        {
            // 3 blocks: 150, 50, 50. Total = 250pt, target ≈ 125pt.
            // item1 (150): current=0, not > 0 → col0, current=150
            // item2 (50):  current=150>0, 150+50=200>125 → col1, current=0
            // item3 (50):  isLastCol → col1, current=100
            // Result: col0=150, col1=100 — not perfectly equal but greedy best-fit
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div id=""container""
       style=""column-count:2; column-fill:balance; column-gap:0; width:400pt;
              border:2pt solid #000000;"">
    <div style=""height:150pt; border:2pt solid #ff0000;"">Large block (150pt)</div>
    <div style=""height:50pt; border:2pt solid #0000ff;"">Small block (50pt)</div>
    <div style=""height:50pt; border:2pt solid #00aa00;"">Small block (50pt)</div>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ColumnBalance_07_LargeFirstBlock.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var container = GetContainerBlock();
            Assert.IsNotNull(container, "Container block should exist");
            Assert.AreEqual(2, container.Columns.Length, "Should have 2 columns");

            // Large block is taller than target so it stays in col0, rest spills to col1
            Assert.AreEqual(1, container.Columns[0].Contents.Count, "Col0 should have 1 item (the large block)");
            Assert.AreEqual(2, container.Columns[1].Contents.Count, "Col1 should have 2 items");

            // Verify heights
            Assert.AreEqual(150.0, container.Columns[0].UsedSize.Height.PointsValue, 1.0,
                "Col0 height should be 150pt");
            Assert.AreEqual(100.0, container.Columns[1].UsedSize.Height.PointsValue, 1.0,
                "Col1 height should be 100pt");
        }

        // =====================================================================
        // 7. Single column — balance is no-op
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_08_SingleColumn_NoOp()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div id=""container""
       style=""column-count:1; column-fill:balance; width:400pt;
              border:2pt solid #000000;"">
    <div style=""height:100pt; border:1pt solid #ff0000;"">Block 1</div>
    <div style=""height:100pt; border:1pt solid #0000ff;"">Block 2</div>
    <div style=""height:100pt; border:1pt solid #00aa00;"">Block 3</div>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ColumnBalance_08_SingleColumn_NoOp.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var container = GetContainerBlock();
            Assert.IsNotNull(container, "Container block should exist");

            // Single column, so all items should be in col0
            Assert.AreEqual(1, container.Columns.Length, "Should have 1 column");
            Assert.AreEqual(3, container.Columns[0].Contents.Count, "All 3 items should be in col0");
            Assert.AreEqual(300.0, container.Columns[0].UsedSize.Height.PointsValue, 1.0,
                "Col0 should have full height of 300pt");
        }

        // =====================================================================
        // 8. Block container total height is max of balanced columns (not total)
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_09_ContainerHeight_IsMaxBalancedColumnHeight()
        {
            // 4 x 50pt blocks in 2 columns. Total=200pt, target=100pt.
            // After balance: each col has 2 items at 100pt.
            // Container height should be ~100pt (not 200pt as with auto).
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div id=""container""
       style=""column-count:2; column-fill:balance; column-gap:0; width:400pt;
              border:2pt solid #000000;"">
    <div style=""height:50pt; border:1pt solid #ff0000;"">Block 1</div>
    <div style=""height:50pt; border:1pt solid #cc4400;"">Block 2</div>
    <div style=""height:50pt; border:1pt solid #0000ff;"">Block 3</div>
    <div style=""height:50pt; border:1pt solid #4400cc;"">Block 4</div>
  </div>
  <div style=""height:30pt; border:2pt solid #888888; margin-top:5pt;"">After container</div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ColumnBalance_09_ContainerHeight.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var container = GetContainerBlock();
            Assert.IsNotNull(container, "Container block should exist");

            // Container block height should reflect balanced column height (~100pt)
            // not the total of all content (~200pt)
            Assert.IsTrue(container.Height.PointsValue <= 105.0,
                $"Container height should be close to 100pt (balanced), but was {container.Height.PointsValue}pt");

            // The block after the container should be positioned close to 100pt, not 200pt
            var body = layout.AllPages[0].ContentBlock;
            var afterBlock = body.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(afterBlock, "Block after container should exist");
            Assert.IsTrue(afterBlock.TotalBounds.Y.PointsValue < 150.0,
                $"Block after container should be positioned close to 100pt (balanced height), but Y was {afterBlock.TotalBounds.Y.PointsValue}pt");
        }

        // =====================================================================
        // 9. Balance with variable height blocks — greedy produces best-effort
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_10_VariableBlocks_GreedyBestEffort()
        {
            // Blocks: 40, 60, 40, 60. Total=200pt, target=100pt per column.
            // item1 (40): col0, current=40
            // item2 (60): 40+60=100, NOT > 100 → col0, current=100
            // item3 (40): 100+40=140 > 100 → col1, current=0
            // item4 (60): isLastCol → col1, current=100
            // Result: col0=100, col1=100 — perfectly balanced!
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div id=""container""
       style=""column-count:2; column-fill:balance; column-gap:0; width:400pt;
              border:2pt solid #000000;"">
    <div style=""height:40pt; border:2pt solid #ff0000;"">40pt block</div>
    <div style=""height:60pt; border:2pt solid #cc0000;"">60pt block</div>
    <div style=""height:40pt; border:2pt solid #0000ff;"">40pt block</div>
    <div style=""height:60pt; border:2pt solid #0000cc;"">60pt block</div>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ColumnBalance_10_VariableBlocks.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var container = GetContainerBlock();
            Assert.IsNotNull(container, "Container block should exist");
            Assert.AreEqual(2, container.Columns.Length, "Should have 2 columns");

            // col0: items 1+2 = 100pt; col1: items 3+4 = 100pt
            Assert.AreEqual(2, container.Columns[0].Contents.Count, "Col0 should have 2 items");
            Assert.AreEqual(2, container.Columns[1].Contents.Count, "Col1 should have 2 items");

            var col0Height = container.Columns[0].UsedSize.Height.PointsValue;
            var col1Height = container.Columns[1].UsedSize.Height.PointsValue;
            Assert.AreEqual(100.0, col0Height, 1.0, "Col0 should be 100pt");
            Assert.AreEqual(100.0, col1Height, 1.0, "Col1 should be 100pt");
        }

        // =====================================================================
        // 10. Y-positions are correctly reset after balance redistribution
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_11_YPositions_ResetAfterRedistribution()
        {
            // After redistribution, items in col1 should have Y starting from 0
            // (not continuing from where they were in col0).
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div id=""container""
       style=""column-count:2; column-fill:balance; column-gap:0; width:400pt;
              border:2pt solid #000000;"">
    <div style=""height:80pt; border:2pt solid #ff0000;"">Block 1 (80pt)</div>
    <div style=""height:80pt; border:2pt solid #cc0000;"">Block 2 (80pt)</div>
    <div style=""height:80pt; border:2pt solid #0000ff;"">Block 3 (80pt)</div>
    <div style=""height:80pt; border:2pt solid #0000cc;"">Block 4 (80pt)</div>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ColumnBalance_11_YPositions.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var container = GetContainerBlock();
            Assert.IsNotNull(container, "Container block should exist");

            // 4 x 80 = 320pt total, target = 160pt
            // col0: items 1+2 (Y=0,80)
            // col1: items 3+4 (Y=0,80) — Y resets to 0 in new column
            var col0 = container.Columns[0];
            var col1 = container.Columns[1];

            Assert.AreEqual(2, col0.Contents.Count, "Col0 should have 2 items");
            Assert.AreEqual(2, col1.Contents.Count, "Col1 should have 2 items");

            // Items in col0
            var col0Item0 = col0.Contents[0] as PDFLayoutBlock;
            var col0Item1 = col0.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(col0Item0, "Col0 item 0 should be a block");
            Assert.IsNotNull(col0Item1, "Col0 item 1 should be a block");
            Assert.AreEqual(0.0, col0Item0.TotalBounds.Y.PointsValue, 1.0, "Col0 item0 Y should be 0");
            Assert.AreEqual(80.0, col0Item1.TotalBounds.Y.PointsValue, 1.0, "Col0 item1 Y should be 80pt");

            // Items in col1 — Y should start at 0 (reset after redistribution)
            var col1Item0 = col1.Contents[0] as PDFLayoutBlock;
            var col1Item1 = col1.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(col1Item0, "Col1 item 0 should be a block");
            Assert.IsNotNull(col1Item1, "Col1 item 1 should be a block");
            Assert.AreEqual(0.0, col1Item0.TotalBounds.Y.PointsValue, 1.0, "Col1 item0 Y should be 0 (reset)");
            Assert.AreEqual(80.0, col1Item1.TotalBounds.Y.PointsValue, 1.0, "Col1 item1 Y should be 80pt");
        }

        // =====================================================================
        // 11. Balance inside a nested container
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_12_Nested_BalancedContainer()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""border:2pt solid #888888; padding:10pt; width:500pt;"">
    <div id=""header"" style=""height:40pt; border:1pt solid #aaaaaa;"">Header</div>
    <div id=""columns""
         style=""column-count:2; column-fill:balance; column-gap:10pt;
                border:2pt solid #000000;"">
      <div style=""height:60pt; border:1pt solid #ff0000;"">Col item 1</div>
      <div style=""height:60pt; border:1pt solid #cc0000;"">Col item 2</div>
      <div style=""height:60pt; border:1pt solid #0000ff;"">Col item 3</div>
      <div style=""height:60pt; border:1pt solid #0000cc;"">Col item 4</div>
    </div>
    <div id=""footer"" style=""height:30pt; border:1pt solid #aaaaaa;"">Footer</div>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ColumnBalance_12_Nested.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var outerDiv = GetContainerBlock();
            Assert.IsNotNull(outerDiv, "Outer div should exist");

            // The balanced columns div is inside the outer div
            var colsBlock = outerDiv.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(colsBlock, "Columns block should exist");
            Assert.AreEqual(2, colsBlock.Columns.Length, "Should have 2 columns");
            Assert.AreEqual(ColumnFillMode.Balance, colsBlock.ColumnOptions.FillMode,
                "FillMode should be Balance");

            // 4 x 60pt = 240pt, target = 120pt → 2 items per column
            Assert.AreEqual(2, colsBlock.Columns[0].Contents.Count, "Nested col0 should have 2 items");
            Assert.AreEqual(2, colsBlock.Columns[1].Contents.Count, "Nested col1 should have 2 items");
        }

        // =====================================================================
        // 12. Balance via CSS class (not inline style)
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_13_ViaCSS_Class()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <style>
    .balanced-cols {
      column-count: 2;
      column-fill: balance;
      column-gap: 0;
      width: 400pt;
      border: 2pt solid #000000;
    }
    .item { border: 1pt solid #888888; }
  </style>
</head>
<body style=""margin:0; padding:0;"">
  <div class=""balanced-cols"">
    <div class=""item"" style=""height:70pt;"">Item 1</div>
    <div class=""item"" style=""height:70pt;"">Item 2</div>
    <div class=""item"" style=""height:70pt;"">Item 3</div>
    <div class=""item"" style=""height:70pt;"">Item 4</div>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ColumnBalance_13_ViaCSS.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var container = GetContainerBlock();
            Assert.IsNotNull(container, "Container block should exist");
            Assert.AreEqual(ColumnFillMode.Balance, container.ColumnOptions.FillMode,
                "FillMode should be Balance when set via CSS class");
            Assert.AreEqual(2, container.Columns.Length, "Should have 2 columns");

            // 4 x 70pt = 280pt, target = 140pt → 2 items per column
            Assert.AreEqual(2, container.Columns[0].Contents.Count, "Col0 should have 2 items");
            Assert.AreEqual(2, container.Columns[1].Contents.Count, "Col1 should have 2 items");
        }

        // =====================================================================
        // 14. Mixed content: image + variable-height text blocks, 2 columns
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_14_MixedContent_ImageAndText_TwoColumns()
        {
            // A 2-column balanced layout with a block image followed by several
            // variable-height paragraph divs. The image path is resolved absolutely
            // so the test is self-contained regardless of working directory.
            var imagePath = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(
                        System.Reflection.Assembly.GetExecutingAssembly().Location),
                    "../../../Content/Images/Toroid32.png"));

            // Image natural size: 682×452 px @ 144 ppi → 341×226 pt.
            // We constrain it to 160pt wide; height scales proportionally ≈ 106pt.
            // Paragraph divs have explicit heights so calculations are predictable.
            // Total content ≈ 106 + 40 + 50 + 35 + 55 + 45 = 331pt → target ≈ 165pt.
            // Expected greedy split: col0 ≈ image(106) + p1(40) + p2(50) = 196pt
            //   (196 ≤ 165 fails, actually: after 106+40=146 ≤ 165, +50=196 > 165 → p2 spills)
            // col0 = image(106) + p1(40) = 146pt; col1 = p2(50)+p3(35)+p4(55)+p5(45) = 185pt

            var doc = new Document();
            var section = new Section();
            section.Style.PageStyle.Width = 500;
            section.Style.PageStyle.Height = 700;
            doc.Pages.Add(section);

            var outer = new Div();
            outer.ColumnCount = 2;
            outer.Style.Columns.FillMode = ColumnFillMode.Balance;
            outer.AlleyWidth = 10;
            outer.BorderColor = StandardColors.Black;
            outer.BorderWidth = 2;
            section.Contents.Add(outer);

            // Block image
            var img = new Image();
            img.Source = imagePath;
            img.DisplayMode = DisplayMode.Block;
            img.Width = 160;
            img.BorderColor = StandardColors.Gray;
            img.BorderWidth = 1;
            outer.Contents.Add(img);

            // Variable-height paragraph divs
            int[] heights = { 40, 50, 35, 55, 45 };
            var colors = new[] {
                StandardColors.Red, StandardColors.Blue, StandardColors.Green,
                StandardColors.Teal, StandardColors.Purple };
            for (int i = 0; i < heights.Length; i++)
            {
                var para = new Div();
                para.Height = heights[i];
                para.BorderColor = colors[i];
                para.BorderWidth = 1;
                para.Contents.Add(new TextLiteral($"Paragraph {i + 1} ({heights[i]}pt)"));
                outer.Contents.Add(para);
            }

            using (var ms = DocStreams.GetOutputStream("ColumnBalance_14_MixedContent.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var body = layout.AllPages[0].ContentBlock;
            var container = body.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(container, "Container block should exist");
            Assert.AreEqual(2, container.Columns.Length, "Should have 2 columns");

            // Both columns must have content after balancing
            Assert.IsTrue(container.Columns[0].Contents.Count > 0, "Col0 should have content");
            Assert.IsTrue(container.Columns[1].Contents.Count > 0, "Col1 should have content");

            // All 6 items (image + 5 paragraphs) must be distributed across the 2 columns
            var totalItems = container.Columns[0].Contents.Count + container.Columns[1].Contents.Count;
            Assert.AreEqual(6, totalItems, "All 6 items should be distributed across both columns");

            // Heights should be reasonably balanced (within 100pt of each other)
            var h0 = container.Columns[0].UsedSize.Height.PointsValue;
            var h1 = container.Columns[1].UsedSize.Height.PointsValue;
            Assert.IsTrue(Math.Abs(h0 - h1) < 100.0,
                $"Column heights should be roughly balanced. col0={h0:F1}pt col1={h1:F1}pt");
        }

        // =====================================================================
        // 15. Real-world article: heading + image + body text, 3 balanced columns
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_15_Article_HeadingImageText_ThreeColumns()
        {
            var imagePath = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(
                        System.Reflection.Assembly.GetExecutingAssembly().Location),
                    "../../../Content/Images/Toroid32.png"));

            string loremShort  = "Quisque gravida elementum nisl, at ultrices odio suscipit interdum. " +
                                  "Sed sed diam non sem fringilla varius. Lorem ipsum dolor sit amet, " +
                                  "consectetur adipiscing elit. Curabitur viverra ligula ut tellus feugiat.";
            string loremMedium = loremShort + " Curabitur id urna sed nulla gravida ultricies. " +
                                  "Duis molestie mi id tincidunt mattis. Maecenas consectetur quis lectus " +
                                  "nec lobortis. Donec nec sapien eu mi commodo porta in quis nibh.";

            var doc = new Document();
            var section = new Section();
            section.Style.PageStyle.Width  = 595;   // A4-ish width
            section.Style.PageStyle.Height = 842;
            section.FontSize   = 10;
            section.Margins    = new Thickness(36);
            doc.Pages.Add(section);

            // Article title (outside columns)
            var title = new Head1();
            title.Contents.Add(new TextLiteral("Scryber Column Balance: A Feature Overview"));
            title.Margins  = new Thickness(0, 0, 0, 8);
            title.FontSize = 18;
            title.BorderColor = StandardColors.Black;
            title.BorderWidth = 1;
            section.Contents.Add(title);

            // Body: 3-column balanced div
            var body = new Div();
            body.ColumnCount = 3;
            body.Style.Columns.FillMode = ColumnFillMode.Balance;
            body.AlleyWidth = 8;
            body.BorderColor = StandardColors.Black;
            body.BorderWidth = 1;
            body.Padding = new Thickness(4);
            section.Contents.Add(body);

            // Lead image
            var img = new Image();
            img.Source = imagePath;
            img.DisplayMode = DisplayMode.Block;
            img.Width = 140;
            img.Margins = new Thickness(0, 0, 4, 4);
            img.BorderColor = StandardColors.Gray;
            img.BorderWidth = 1;
            body.Contents.Add(img);

            // Several paragraphs of text with explicit heights to keep layout deterministic
            int[] paraHeights = { 55, 45, 60, 40, 50, 55, 45, 40 };
            for (int i = 0; i < paraHeights.Length; i++)
            {
                var para = new Div();
                para.Height = paraHeights[i];
                para.Margins = new Thickness(0, 0, 0, 4);
                para.BorderColor = StandardColors.Gray;
                para.BorderWidth = 1;
                para.Contents.Add(new TextLiteral(loremShort));
                body.Contents.Add(para);
            }

            using (var ms = DocStreams.GetOutputStream("ColumnBalance_15_Article.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var page     = layout.AllPages[0];
            var pageBody = page.ContentBlock;

            // Title is first item in the page column
            var titleBlock = pageBody.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(titleBlock, "Title block should be first item");

            // Body (3-column div) is second item
            var bodyBlock = pageBody.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(bodyBlock, "Body block should be second item");
            Assert.AreEqual(3, bodyBlock.Columns.Length, "Body should have 3 columns");
            Assert.AreEqual(ColumnFillMode.Balance, bodyBlock.ColumnOptions.FillMode,
                "Body FillMode should be Balance");

            // All 3 columns should have content
            Assert.IsTrue(bodyBlock.Columns[0].Contents.Count > 0, "Col0 should have content");
            Assert.IsTrue(bodyBlock.Columns[1].Contents.Count > 0, "Col1 should have content");
            Assert.IsTrue(bodyBlock.Columns[2].Contents.Count > 0, "Col2 should have content");

            // All 9 items (image + 8 paragraphs) accounted for
            var total = bodyBlock.Columns[0].Contents.Count
                      + bodyBlock.Columns[1].Contents.Count
                      + bodyBlock.Columns[2].Contents.Count;
            Assert.AreEqual(9, total, "All 9 items should be spread across 3 columns");

            // Heights should be reasonably balanced
            var h0 = bodyBlock.Columns[0].UsedSize.Height.PointsValue;
            var h1 = bodyBlock.Columns[1].UsedSize.Height.PointsValue;
            var h2 = bodyBlock.Columns[2].UsedSize.Height.PointsValue;
            Assert.IsTrue(Math.Abs(h0 - h1) < 80.0, $"Col0 vs Col1 height diff should be < 80pt (was {Math.Abs(h0-h1):F1}pt)");
            Assert.IsTrue(Math.Abs(h1 - h2) < 80.0, $"Col1 vs Col2 height diff should be < 80pt (was {Math.Abs(h1-h2):F1}pt)");
        }

        // =====================================================================
        // 16. Three-column div: items overflow col0→col1→col2 during layout,
        //     then BalanceColumns redistributes for even fill
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_16_ThreeColumns_InitialOverflow_ThenBalanced()
        {
            // A height-constrained div forces items to overflow col0→col1→col2
            // during normal layout. BalanceColumns then redistributes everything
            // evenly across all three columns.
            //
            // 9 items × 40pt = 360pt total, target = 120pt per column (3 items each).
            // With height:130pt: items 1-3 fit in col0 (120pt), item 4 overflows to col1,
            // items 4-6 fill col1 (120pt), item 7 overflows to col2, items 7-9 in col2.
            // After balance: same result — 3 items per column of 120pt each.

            var doc = new Document();
            var section = new Section();
            section.Style.PageStyle.Width  = 600;
            section.Style.PageStyle.Height = 700;
            doc.Pages.Add(section);

            var outer = new Div();
            outer.ColumnCount = 3;
            outer.Style.Columns.FillMode = ColumnFillMode.Balance;
            outer.AlleyWidth = 0;
            outer.Height     = 130;   // enough for 3 × 40pt items before overflow
            outer.BorderColor = StandardColors.Black;
            outer.BorderWidth = 2;
            section.Contents.Add(outer);

            var palette = new[] {
                StandardColors.Red,   StandardColors.Maroon, StandardColors.Fuchsia,
                StandardColors.Blue,  StandardColors.Navy,   StandardColors.Teal,
                StandardColors.Green, StandardColors.Olive,  StandardColors.Purple };

            for (int i = 0; i < 9; i++)
            {
                var item = new Div();
                item.Height      = 40;
                item.BorderColor = palette[i];
                item.BorderWidth = 1;
                item.Contents.Add(new TextLiteral($"Item {i + 1}"));
                outer.Contents.Add(item);
            }

            using (var ms = DocStreams.GetOutputStream("ColumnBalance_16_ThreeColumns_Overflow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var body      = layout.AllPages[0].ContentBlock;
            var container = body.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(container, "Container block should exist");
            Assert.AreEqual(3, container.Columns.Length, "Should have 3 columns");

            // After balance: 3 items per column
            Assert.AreEqual(3, container.Columns[0].Contents.Count, "Col0 should have 3 items");
            Assert.AreEqual(3, container.Columns[1].Contents.Count, "Col1 should have 3 items");
            Assert.AreEqual(3, container.Columns[2].Contents.Count, "Col2 should have 3 items");

            // All columns should be equal height (3 × 40 = 120pt)
            Assert.AreEqual(120.0, container.Columns[0].UsedSize.Height.PointsValue, 1.0, "Col0 height should be 120pt");
            Assert.AreEqual(120.0, container.Columns[1].UsedSize.Height.PointsValue, 1.0, "Col1 height should be 120pt");
            Assert.AreEqual(120.0, container.Columns[2].UsedSize.Height.PointsValue, 1.0, "Col2 height should be 120pt");
        }

        // =====================================================================
        // 17. Page overflow: section with 3 balanced-column panels, content
        //     large enough to require a second page
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_17_PageOverflow_TwoPages_BalancedPanels()
        {
            // Two separate balanced-column divs. Panel 1 occupies most of page 1;
            // panel 2 is too tall to fit in the remaining space and overflows to page 2.
            // Both panels are independently balanced on their respective pages.
            //
            // Page available height = 400 - 20 margin = 380pt.
            // Panel 1: 8 × 50pt = 400pt → balanced = 200pt → uses 200pt on page 1.
            // Remaining on page 1: 380 - 200 = 180pt.
            // Panel 2: 8 × 50pt = 400pt total. During layout col0 fills 180pt (3 items),
            // overflows to col1 (180pt, 3 items), then section overflows to page 2 (2 items).
            // Page 2 gets the remainder of panel 2, balanced independently.

            var doc = new Document();
            var section = new Section();
            section.Style.PageStyle.Width  = 500;
            section.Style.PageStyle.Height = 400;
            section.Margins = new Thickness(10);
            doc.Pages.Add(section);

            for (int panel = 0; panel < 2; panel++)
            {
                var panelDiv = new Div();
                panelDiv.ColumnCount = 2;
                panelDiv.Style.Columns.FillMode = ColumnFillMode.Balance;
                panelDiv.AlleyWidth  = 0;
                panelDiv.BorderColor = panel == 0 ? StandardColors.Navy : StandardColors.Maroon;
                panelDiv.BorderWidth = 2;
                section.Contents.Add(panelDiv);

                var colors = panel == 0
                    ? new[] { StandardColors.Red, StandardColors.Blue, StandardColors.Green,
                               StandardColors.Fuchsia, StandardColors.Purple, StandardColors.Teal,
                               StandardColors.Olive, StandardColors.Maroon }
                    : new[] { StandardColors.Maroon, StandardColors.Navy, StandardColors.Olive,
                               StandardColors.Gray, StandardColors.Silver, StandardColors.Black,
                               StandardColors.Teal, StandardColors.Purple };

                for (int i = 0; i < 8; i++)
                {
                    var item = new Div();
                    item.Height      = 50;
                    item.BorderColor = colors[i];
                    item.BorderWidth = 1;
                    item.Contents.Add(new TextLiteral($"Panel {panel + 1} item {i + 1}"));
                    panelDiv.Contents.Add(item);
                }
            }

            using (var ms = DocStreams.GetOutputStream("ColumnBalance_17_PageOverflow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            Assert.AreEqual(2, layout.AllPages.Count, "Should have 2 pages");

            // Page 1: panel 1 fully balanced, panel 2 starts but overflows
            var page1Body   = layout.AllPages[0].ContentBlock;
            var panel1Block = page1Body.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panel1Block, "Panel 1 block should be on page 1");
            Assert.AreEqual(2, panel1Block.Columns.Length, "Panel 1 should have 2 columns");
            Assert.AreEqual(ColumnFillMode.Balance, panel1Block.ColumnOptions.FillMode,
                "Panel 1 FillMode should be Balance");

            // Panel 1: 8 × 50pt = 400pt total, target = 200pt → 4 items per column
            Assert.AreEqual(4, panel1Block.Columns[0].Contents.Count, "Panel 1 col0 should have 4 items");
            Assert.AreEqual(4, panel1Block.Columns[1].Contents.Count, "Panel 1 col1 should have 4 items");

            // Page 2: continuation of panel 2 (balanced within what fit on page 2)
            var page2Body   = layout.AllPages[1].ContentBlock;
            var panel2Page2 = page2Body.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panel2Page2, "Panel 2 continuation should be on page 2");
            Assert.AreEqual(2, panel2Page2.Columns.Length, "Panel 2 page 2 should have 2 columns");

            // Both columns on page 2 should have content (balanced)
            Assert.IsTrue(panel2Page2.Columns[0].Contents.Count > 0, "Panel 2 page 2 col0 should have content");
            Assert.IsTrue(panel2Page2.Columns[1].Contents.Count > 0, "Panel 2 page 2 col1 should have content");
        }

        // =====================================================================
        // 18. Lines of text (not paragraph blocks) balanced across 2 columns
        //     Verifies that PDFLayoutLine items are redistributed correctly,
        //     not just PDFLayoutBlock items.
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_18_TextLines_BalancedAcrossColumns()
        {
            // Content is added directly to the column div as TextLiterals (no paragraph
            // wrapper), producing PDFLayoutLine items at the top level of the column.
            // An image block is interspersed to confirm mixed line/block redistribution.
            //
            // The container is narrow enough that the text wraps into many lines,
            // giving BalanceColumns plenty of lines to redistribute.

            var imagePath = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(
                        System.Reflection.Assembly.GetExecutingAssembly().Location),
                    "../../../Content/Images/Toroid32.png"));

            string repeatText =
                "Quisque gravida elementum nisl at ultrices odio suscipit interdum. " +
                "Sed sed diam non sem fringilla varius lorem ipsum dolor sit amet. " +
                "Curabitur viverra ligula ut tellus feugiat mattis curabitur urna. " +
                "Duis molestie mi id tincidunt mattis maecenas consectetur lectus. ";

            var doc = new Document();
            var section = new Section();
            section.Style.PageStyle.Width  = 400;
            section.Style.PageStyle.Height = 700;
            section.FontSize = 10;
            doc.Pages.Add(section);

            var outer = new Div();
            outer.ColumnCount = 2;
            outer.Style.Columns.FillMode = ColumnFillMode.Balance;
            outer.AlleyWidth  = 5;
            outer.BorderColor = StandardColors.Black;
            outer.BorderWidth = 2;
            outer.Padding     = new Thickness(2);
            section.Contents.Add(outer);

            // Text added directly to the div — layout engine creates PDFLayoutLine
            // items in the region rather than nested blocks.
            outer.Contents.Add(new TextLiteral(repeatText + repeatText));

            // Block image mid-content
            var img = new Image();
            img.Source      = imagePath;
            img.DisplayMode = DisplayMode.Block;
            img.Width       = 80;
            img.BorderColor = StandardColors.Gray;
            img.BorderWidth = 1;
            outer.Contents.Add(img);

            // More text after the image
            outer.Contents.Add(new TextLiteral(repeatText + repeatText));

            using (var ms = DocStreams.GetOutputStream("ColumnBalance_18_TextLines.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var body      = layout.AllPages[0].ContentBlock;
            var container = body.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(container, "Container block should exist");
            Assert.AreEqual(2, container.Columns.Length, "Should have 2 columns");

            var col0 = container.Columns[0];
            var col1 = container.Columns[1];

            // Both columns must have content after balancing
            Assert.IsTrue(col0.Contents.Count > 0, "Col0 should have content after balance");
            Assert.IsTrue(col1.Contents.Count > 0, "Col1 should have content after balance");

            // At least one item in col0 should be a PDFLayoutLine (not just a block),
            // confirming that line-level items were redistributed correctly.
            bool col0HasLines = false;
            foreach (var item in col0.Contents)
            {
                if (item is PDFLayoutLine) { col0HasLines = true; break; }
            }
            Assert.IsTrue(col0HasLines, "Col0 should contain at least one PDFLayoutLine after redistribution");

            bool col1HasLines = false;
            foreach (var item in col1.Contents)
            {
                if (item is PDFLayoutLine) { col1HasLines = true; break; }
            }
            Assert.IsTrue(col1HasLines, "Col1 should contain at least one PDFLayoutLine after redistribution");

            // Heights should be roughly balanced (within 60pt — text lines are small
            // so greedy split should land close to the midpoint)
            var h0 = col0.UsedSize.Height.PointsValue;
            var h1 = col1.UsedSize.Height.PointsValue;
            Assert.IsTrue(h0 > 0, "Col0 height should be positive");
            Assert.IsTrue(h1 > 0, "Col1 height should be positive");
            Assert.IsTrue(Math.Abs(h0 - h1) < 60.0,
                $"Column heights should be roughly balanced. col0={h0:F1}pt col1={h1:F1}pt diff={Math.Abs(h0-h1):F1}pt");

            // Dump col0 and col1 contents to show types and positions
            static string DescribeItem(PDFLayoutItem item)
            {
                if (item is PDFLayoutBlock b)
                    return $"Block({b.Owner?.GetType().Name},Y={b.TotalBounds.Y:F1},H={b.Height:F1}) ";
                if (item is PDFLayoutLine ln)
                {
                    bool hasImage = ln.Runs.OfType<PDFLayoutComponentRun>().Any();
                    return $"Line(Y={ln.OffsetY:F1},H={ln.Height:F1}{(hasImage ? ",IMAGE" : "")}) ";
                }
                return $"Unknown(Y={item.OffsetY:F1},H={item.Height:F1}) ";
            }

            var col0Desc = new System.Text.StringBuilder();
            foreach (var item in col0.Contents)
                col0Desc.Append(DescribeItem(item));

            var col1Desc = new System.Text.StringBuilder();
            foreach (var item in col1.Contents)
                col1Desc.Append(DescribeItem(item));

            // A block image in text flow is laid out as a PDFLayoutLine containing a
            // PDFLayoutComponentRun (not a standalone PDFLayoutBlock). Find it.
            PDFLayoutLine imageLine = null;
            PDFLayoutRegion imageCol = null;
            PDFLayoutComponentRun imageRun = null;
            double imageOffsetY = -1;

            foreach (var col in new[] { col0, col1 })
            {
                foreach (var item in col.Contents)
                {
                    if (item is PDFLayoutLine ln &&
                        ln.Runs.OfType<PDFLayoutComponentRun>().Any())
                    {
                        imageLine    = ln;
                        imageCol     = col;
                        imageOffsetY = ln.OffsetY.PointsValue;
                        imageRun = ln.Runs.OfType<PDFLayoutComponentRun>().First();
                        break;
                    }
                }
                if (imageLine != null) break;
            }

            Assert.IsNotNull(imageLine,
                $"Image line should exist in one of the columns. col0=[{col0Desc}] col1=[{col1Desc}]");

            // Image must be in col1 after balancing
            Assert.AreSame(col1, imageCol,
                $"Image line should be in col1, not col0. col0=[{col0Desc}] col1=[{col1Desc}]");

            // Image offset within col1 should be near the top (preceded only by any
            // text lines that the balance algorithm placed before it in col1)
            Assert.IsTrue(imageOffsetY < h1 * 0.6,
                $"Image should be in the upper 60%% of col1 (not at the bottom). " +
                $"imageOffsetY={imageOffsetY:F1}pt col1Height={h1:F1}pt. col1=[{col1Desc}]");
            
            Assert.IsNotNull(imageRun, "ImageRun should exist in the second columns. col1=[{col1Desc}]");
            Assert.AreEqual(imageRun.TotalBounds.Y, 0.0, "Imahge run should be at offset 0");
            
        }

        // =====================================================================
        // 19. Lines of text in balanced columns that overflow onto a second page.
        //     Each page's portion is independently balanced.
        //     Confirms PDFLayoutLine items are correctly redistributed on both pages.
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_19_TextLines_BalancedAndOverflowsToPage2()
        {
            // Page available height = 250 - 20 margins = 230pt.
            // Font 10pt → line height ≈ 12pt → ~19 lines per column.
            // Two columns fill ~38 lines before the div overflows to page 2.
            // 6× repeatText ≈ 48–54 lines total, so ~10–16 lines land on page 2.
            //
            // BalanceColumns runs independently on each page's portion:
            //   page-1 block (BlockRepeatIndex=0) — lines redistributed 50/50
            //   page-2 block (BlockRepeatIndex=1) — remaining lines redistributed

            string repeatText =
                "Quisque gravida elementum nisl at ultrices odio suscipit interdum. " +
                "Sed sed diam non sem fringilla varius lorem ipsum dolor sit amet. " +
                "Curabitur viverra ligula ut tellus feugiat mattis curabitur urna. " +
                "Duis molestie mi id tincidunt mattis maecenas consectetur lectus. ";

            var doc = new Document();
            var section = new Section();
            section.Style.PageStyle.Width  = 400;
            section.Style.PageStyle.Height = 250;
            section.FontSize = 10;
            section.Margins  = new Thickness(10);
            doc.Pages.Add(section);

            var outer = new Div();
            outer.ColumnCount = 2;
            outer.Style.Columns.FillMode = ColumnFillMode.Balance;
            outer.AlleyWidth  = 5;
            outer.BorderColor = StandardColors.Black;
            outer.BorderWidth = 2;
            outer.Padding     = new Thickness(2);
            section.Contents.Add(outer);

            // Raw TextLiterals — layout engine creates PDFLayoutLine items directly
            // in the column region with no intermediate paragraph block.
            outer.Contents.Add(new TextLiteral(
                repeatText + repeatText + repeatText +
                repeatText + repeatText + repeatText));

            using (var ms = DocStreams.GetOutputStream("ColumnBalance_19_TextLines_PageOverflow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            Assert.IsTrue(layout.AllPages.Count >= 2,
                $"Should have at least 2 pages, got {layout.AllPages.Count}");

            // ── Page 1: first portion of the div (BlockRepeatIndex == 0) ──
            var page1Body  = layout.AllPages[0].ContentBlock;
            var divPage1   = page1Body.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divPage1, "Div block should be first item on page 1");
            Assert.AreEqual(0, divPage1.BlockRepeatIndex, "Page-1 block should have BlockRepeatIndex 0");
            Assert.AreEqual(2, divPage1.Columns.Length, "Page-1 div should have 2 columns");

            var p1col0 = divPage1.Columns[0];
            var p1col1 = divPage1.Columns[1];
            Assert.IsTrue(p1col0.Contents.Count > 0, "Page-1 col0 should have content");
            Assert.IsTrue(p1col1.Contents.Count > 0, "Page-1 col1 should have content");

            // Confirm lines (not blocks) are present in both columns
            Assert.IsTrue(p1col0.Contents.Any(i => i is PDFLayoutLine),
                "Page-1 col0 should contain PDFLayoutLine items");
            Assert.IsTrue(p1col1.Contents.Any(i => i is PDFLayoutLine),
                "Page-1 col1 should contain PDFLayoutLine items");

            // Heights balanced on page 1 — greedy line-level split is quite tight
            var h1c0 = p1col0.UsedSize.Height.PointsValue;
            var h1c1 = p1col1.UsedSize.Height.PointsValue;
            Assert.IsTrue(Math.Abs(h1c0 - h1c1) < 24.0,
                $"Page-1 columns should be balanced (within 2 lines). col0={h1c0:F1}pt col1={h1c1:F1}pt");

            // ── Page 2: continuation of the div (BlockRepeatIndex == 1) ──
            var page2Body  = layout.AllPages[1].ContentBlock;
            var divPage2   = page2Body.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divPage2, "Div continuation should be first item on page 2");
            Assert.AreEqual(1, divPage2.BlockRepeatIndex, "Page-2 block should have BlockRepeatIndex 1");
            Assert.AreEqual(2, divPage2.Columns.Length, "Page-2 div should have 2 columns");

            var p2col0 = divPage2.Columns[0];
            var p2col1 = divPage2.Columns[1];
            Assert.IsTrue(p2col0.Contents.Count > 0, "Page-2 col0 should have content");
            Assert.IsTrue(p2col1.Contents.Count > 0, "Page-2 col1 should have content");

            Assert.IsTrue(p2col0.Contents.Any(i => i is PDFLayoutLine),
                "Page-2 col0 should contain PDFLayoutLine items");
            Assert.IsTrue(p2col1.Contents.Any(i => i is PDFLayoutLine),
                "Page-2 col1 should contain PDFLayoutLine items");
        }

        // =====================================================================
        // 20. Long text literal balanced across 2 columns — verifies text run
        //     repair: each column's first text line must have a PDFTextRunBegin,
        //     and each column's last text line must have a PDFTextRunEnd.
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_20_LongTextLiteral_TwoColumns_RunsRepaired()
        {
            // A single long TextLiteral produces many PDFLayoutLine items in col0.
            // After balancing, half go to col1. The repair step must:
            //   (a) add a PDFTextRunBegin at the start of col1's first text line
            //   (b) add a PDFTextRunEnd to col0's last text line
            //   (c) the original PDFTextRunEnd must move to col1's last line

            string lorem =
                "Lorem ipsum dolor sit amet consectetur adipiscing elit sed do eiusmod. " +
                "Tempor incididunt ut labore et dolore magna aliqua ut enim ad minim. " +
                "Veniam quis nostrud exercitation ullamco laboris nisi ut aliquip ex. " +
                "Ea commodo consequat duis aute irure dolor in reprehenderit in voluptate. " +
                "Velit esse cillum dolore eu fugiat nulla pariatur excepteur sint occaecat. " +
                "Cupidatat non proident sunt in culpa qui officia deserunt mollit anim id est. " +
                "Laborum sed perspiciatis unde omnis iste natus error sit voluptatem accusantium. " +
                "Doloremque laudantium totam rem aperiam eaque ipsa quae ab illo inventore veritatis. ";

            var doc = new Document();
            var section = new Section();
            section.Style.PageStyle.Width  = 400;
            section.Style.PageStyle.Height = 700;
            section.FontSize = 10;
            doc.Pages.Add(section);

            var outer = new Div();
            outer.ColumnCount = 2;
            outer.Style.Columns.FillMode = ColumnFillMode.Balance;
            outer.AlleyWidth  = 6;
            outer.BorderColor = StandardColors.Black;
            outer.BorderWidth = 2;
            outer.Padding     = new Thickness(2);
            section.Contents.Add(outer);

            // One long TextLiteral — no paragraph wrapper
            outer.Contents.Add(new TextLiteral(lorem + lorem + lorem));

            using (var ms = DocStreams.GetOutputStream("ColumnBalance_20_LongText_2col.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var body      = layout.AllPages[0].ContentBlock;
            var container = body.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(container, "Container block should exist");
            Assert.AreEqual(2, container.Columns.Length, "Should have 2 columns");

            var col0 = container.Columns[0];
            var col1 = container.Columns[1];

            Assert.IsTrue(col0.Contents.Count > 0, "Col0 should have content");
            Assert.IsTrue(col1.Contents.Count > 0, "Col1 should have content");
            Assert.IsTrue(col0.Contents.Any(i => i is PDFLayoutLine), "Col0 should have PDFLayoutLine items");
            Assert.IsTrue(col1.Contents.Any(i => i is PDFLayoutLine), "Col1 should have PDFLayoutLine items");

            // Col0 must end with a PDFTextRunEnd on its last text line
            var lastCol0Line = col0.Contents.OfType<PDFLayoutLine>().Last();
            bool col0HasEnd = lastCol0Line.Runs.Any(r => r is PDFTextRunEnd);
            Assert.IsTrue(col0HasEnd, "Col0's last text line must have a PDFTextRunEnd after repair");

            // Col1 must start with a PDFTextRunBegin on its first text line
            var firstCol1Line = col1.Contents.OfType<PDFLayoutLine>().First();
            bool col1HasBegin = firstCol1Line.Runs.Any(r => r is PDFTextRunBegin);
            Assert.IsTrue(col1HasBegin, "Col1's first text line must have a PDFTextRunBegin after repair");

            // Col1 must end with a PDFTextRunEnd on its last text line
            var lastCol1Line = col1.Contents.OfType<PDFLayoutLine>().Last();
            bool col1HasEnd = lastCol1Line.Runs.Any(r => r is PDFTextRunEnd);
            Assert.IsTrue(col1HasEnd, "Col1's last text line must have a PDFTextRunEnd after repair");

            // Heights should be balanced within ~2 line-heights (24pt)
            var h0 = col0.UsedSize.Height.PointsValue;
            var h1 = col1.UsedSize.Height.PointsValue;
            Assert.IsTrue(h0 > 0, "Col0 height should be positive");
            Assert.IsTrue(h1 > 0, "Col1 height should be positive");
            Assert.IsTrue(Math.Abs(h0 - h1) <= 24.0,
                $"Column heights should be balanced within 24pt. col0={h0:F1}pt col1={h1:F1}pt diff={Math.Abs(h0-h1):F1}pt");
        }

        // =====================================================================
        // 21. Long text literal balanced across 3 columns — all three columns
        //     get a PDFTextRunBegin/End pair, and heights are roughly equal.
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_21_LongTextLiteral_ThreeColumns_RunsRepaired()
        {
            string lorem =
                "Lorem ipsum dolor sit amet consectetur adipiscing elit sed do eiusmod. " +
                "Tempor incididunt ut labore et dolore magna aliqua ut enim ad minim. " +
                "Veniam quis nostrud exercitation ullamco laboris nisi ut aliquip ex. " +
                "Ea commodo consequat duis aute irure dolor in reprehenderit in voluptate. " +
                "Velit esse cillum dolore eu fugiat nulla pariatur excepteur sint occaecat. " +
                "Cupidatat non proident sunt in culpa qui officia deserunt mollit anim id est. " +
                "Laborum sed perspiciatis unde omnis iste natus error sit voluptatem accusantium. " +
                "Doloremque laudantium totam rem aperiam eaque ipsa quae ab illo inventore veritatis. ";

            var doc = new Document();
            var section = new Section();
            section.Style.PageStyle.Width  = 500;
            section.Style.PageStyle.Height = 700;
            section.FontSize = 10;
            doc.Pages.Add(section);

            var outer = new Div();
            outer.ColumnCount = 3;
            outer.Style.Columns.FillMode = ColumnFillMode.Balance;
            outer.AlleyWidth  = 6;
            outer.BorderColor = StandardColors.Black;
            outer.BorderWidth = 2;
            outer.Padding     = new Thickness(2);
            section.Contents.Add(outer);

            outer.Contents.Add(new TextLiteral(lorem + lorem + lorem + lorem));

            using (var ms = DocStreams.GetOutputStream("ColumnBalance_21_LongText_3col.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var body      = layout.AllPages[0].ContentBlock;
            var container = body.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(container, "Container block should exist");
            Assert.AreEqual(3, container.Columns.Length, "Should have 3 columns");

            for (int ci = 0; ci < 3; ci++)
            {
                var col = container.Columns[ci];
                Assert.IsTrue(col.Contents.Count > 0, $"Col{ci} should have content");
                Assert.IsTrue(col.Contents.Any(i => i is PDFLayoutLine), $"Col{ci} should have PDFLayoutLine items");

                // First text line must have a PDFTextRunBegin
                var firstLine = col.Contents.OfType<PDFLayoutLine>().First();
                Assert.IsTrue(firstLine.Runs.Any(r => r is PDFTextRunBegin),
                    $"Col{ci} first text line must have PDFTextRunBegin");

                // Last text line must have a PDFTextRunEnd
                var lastLine = col.Contents.OfType<PDFLayoutLine>().Last();
                Assert.IsTrue(lastLine.Runs.Any(r => r is PDFTextRunEnd),
                    $"Col{ci} last text line must have PDFTextRunEnd");
            }

            // Heights should be balanced within ~3 line-heights (36pt)
            var h0 = container.Columns[0].UsedSize.Height.PointsValue;
            var h1 = container.Columns[1].UsedSize.Height.PointsValue;
            var h2 = container.Columns[2].UsedSize.Height.PointsValue;
            Assert.IsTrue(Math.Abs(h0 - h1) <= 36.0, $"Col0 vs Col1 diff should be ≤36pt (was {Math.Abs(h0-h1):F1}pt)");
            Assert.IsTrue(Math.Abs(h1 - h2) <= 36.0, $"Col1 vs Col2 diff should be ≤36pt (was {Math.Abs(h1-h2):F1}pt)");
        }

        // =====================================================================
        // 22. Float:left within balanced 2-column text layout.
        //     Text fills col0; the float and remaining text land in col1.
        //     Verifies BalanceColumns correctly redistributes the float-containing
        //     line and that the float region renders at the top of col1.
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_22_FloatLeft_BalancedAcrossColumns()
        {
            string repeatText =
                "Quisque gravida elementum nisl at ultrices odio suscipit interdum. " +
                "Sed sed diam non sem fringilla varius lorem ipsum dolor sit amet. " +
                "Curabitur viverra ligula ut tellus feugiat mattis curabitur urna. " +
                "Duis molestie mi id tincidunt mattis maecenas consectetur lectus. ";

            var doc = new Document();
            var section = new Section();
            section.Style.PageStyle.Width  = 400;
            section.Style.PageStyle.Height = 700;
            section.FontSize = 10;
            doc.Pages.Add(section);

            var outer = new Div();
            outer.ColumnCount = 2;
            outer.Style.Columns.FillMode = ColumnFillMode.Balance;
            outer.AlleyWidth  = 5;
            outer.BorderColor = StandardColors.Black;
            outer.BorderWidth = 2;
            outer.Padding     = new Thickness(2);
            outer.Height = 250;
            section.Contents.Add(outer);

            // A small heading block + text together reach just under the balance target,
            // so the float (whose line is only 12pt tall) is the item that tips col0 over
            // and gets redistributed to col1 — exposing any position-update bug.
            var heading = new Div();
            //heading.Height      = 20;
            heading.BorderColor = StandardColors.Black;
            heading.BorderWidth = 1;
            heading.Contents.Add(new TextLiteral("Section heading stretching across more than one line."));
            heading.Margins = new Thickness(10);
            outer.Contents.Add(heading);
            
            

            outer.Contents.Add(new TextLiteral(repeatText + repeatText));

            
            // Float:left div — sits mid-content so after balancing it lands in col1
            var floatDiv = new Div();
            floatDiv.Style.Position.Float = FloatMode.Left;
            floatDiv.Width       = 60;
            floatDiv.Height      = 50;
            floatDiv.BorderColor = StandardColors.Red;
            floatDiv.Margins     = new Thickness(0, 5, 0, 5);
            floatDiv.BorderWidth = 1;
            outer.Contents.Add(floatDiv);

            outer.Contents.Add(new TextLiteral(repeatText + repeatText));

            using (var ms = DocStreams.GetOutputStream("ColumnBalance_22_FloatLeft.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.RenderOptions.StringOutput = OutputStringType.Text;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var body      = layout.AllPages[0].ContentBlock;
            var container = body.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(container, "Container block should exist");
            Assert.AreEqual(2, container.Columns.Length, "Should have 2 columns");

            var col0 = container.Columns[0];
            var col1 = container.Columns[1];

            Assert.IsTrue(col0.Contents.Count > 0, "Col0 should have content after balance");
            Assert.IsTrue(col1.Contents.Count > 0, "Col1 should have content after balance");

            var h0 = col0.UsedSize.Height.PointsValue;
            var h1 = col1.UsedSize.Height.PointsValue;
            Assert.IsTrue(h0 > 0, "Col0 height should be positive");
            Assert.IsTrue(h1 > 0, "Col1 height should be positive");
            Assert.IsTrue(Math.Abs(h0 - h1) < 60.0,
                $"Column heights should be roughly balanced. col0={h0:F1}pt col1={h1:F1}pt diff={Math.Abs(h0-h1):F1}pt");

            static string DescribeFloatItem(PDFLayoutItem item)
            {
                if (item is PDFLayoutBlock b)
                    return $"Block({b.Owner?.GetType().Name},Y={b.TotalBounds.Y:F1},H={b.Height:F1}) ";
                if (item is PDFLayoutLine ln)
                {
                    bool hasFloat = ln.Runs.OfType<PDFLayoutPositionedRegionRun>().Any(r => r.IsFloating);
                    return $"Line(Y={ln.OffsetY:F1},H={ln.Height:F1}{(hasFloat ? ",FLOAT" : "")}) ";
                }
                return $"Unknown(Y={item.OffsetY:F1},H={item.Height:F1}) ";
            }

            var col0Desc = new System.Text.StringBuilder();
            foreach (var item in col0.Contents) col0Desc.Append(DescribeFloatItem(item));

            var col1Desc = new System.Text.StringBuilder();
            foreach (var item in col1.Contents) col1Desc.Append(DescribeFloatItem(item));

            // Find the line containing the float:left positioned run
            PDFLayoutLine floatLine = null;
            PDFLayoutRegion floatCol = null;
            PDFLayoutPositionedRegionRun floatRun = null;
            double floatOffsetY = -1;

            foreach (var col in new[] { col0, col1 })
            {
                foreach (var item in col.Contents)
                {
                    if (item is PDFLayoutLine ln)
                    {
                        var run = ln.Runs.OfType<PDFLayoutPositionedRegionRun>()
                                    .FirstOrDefault(r => r.IsFloating);
                        if (run != null)
                        {
                            floatLine    = ln;
                            floatCol     = col;
                            floatOffsetY = ln.OffsetY.PointsValue;
                            floatRun     = run;
                            break;
                        }
                    }
                }
                if (floatLine != null) break;
            }

            Assert.IsNotNull(floatLine,
                $"Float line should exist in one of the columns. col0=[{col0Desc}] col1=[{col1Desc}]");

            Assert.AreSame(col1, floatCol,
                $"Float:left line should be in col1 after balancing. col0=[{col0Desc}] col1=[{col1Desc}]");

            Assert.IsTrue(floatOffsetY < h1 * 0.6,
                $"Float should be in the upper 60%% of col1. " +
                $"floatOffsetY={floatOffsetY:F1}pt col1Height={h1:F1}pt. col1=[{col1Desc}]");

            // The float region's TotalBounds.Y should be near zero (top of col1)
            var floatRegionY = floatRun.Region.TotalBounds.Y.PointsValue;
            Assert.IsTrue(floatRegionY < h1 * 0.6,
                $"Float region TotalBounds.Y should be near the top of col1 after balancing. " +
                $"floatRegionY={floatRegionY:F1}pt col1Height={h1:F1}pt");
        }

        // =====================================================================
        // 23. Float:right within balanced 2-column text layout.
        //     Same as test 22 but the float appears on the right side.
        //     The float div is placed before its surrounding text in source order
        //     (required for float:right in Scryber so it appears at the correct
        //     horizontal position rather than wrapping to a new line).
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_23_FloatRight_BalancedAcrossColumns()
        {
            string repeatText =
                "Quisque gravida elementum nisl at ultrices odio suscipit interdum. " +
                "Sed sed diam non sem fringilla varius lorem ipsum dolor sit amet. " +
                "Curabitur viverra ligula ut tellus feugiat mattis curabitur urna. " +
                "Duis molestie mi id tincidunt mattis maecenas consectetur lectus. ";

            var doc = new Document();
            var section = new Section();
            section.Style.PageStyle.Width  = 400;
            section.Style.PageStyle.Height = 700;
            section.FontSize = 10;
            doc.Pages.Add(section);

            var outer = new Div();
            outer.ColumnCount = 2;
            outer.Style.Columns.FillMode = ColumnFillMode.Balance;
            outer.AlleyWidth  = 5;
            outer.BorderColor = StandardColors.Black;
            outer.BorderWidth = 2;
            outer.Padding     = new Thickness(2);
            section.Contents.Add(outer);

            // Same heading + text structure as test 22 — pushes the float to col1.
            var heading = new Div();
            heading.Height      = 20;
            heading.BorderColor = StandardColors.Black;
            heading.BorderWidth = 1;
            heading.Contents.Add(new TextLiteral("Section heading"));
            outer.Contents.Add(heading);

            outer.Contents.Add(new TextLiteral(repeatText + repeatText));

            // Float:right div — must appear in source before the inline text it floats
            // alongside (Scryber requirement for float:right), so it is added before
            // the second block of text.
            var floatDiv = new Div();
            floatDiv.Style.Position.Float = FloatMode.Right;
            floatDiv.Width       = 60;
            floatDiv.Height      = 50;
            floatDiv.BorderColor = StandardColors.Lime;
            floatDiv.BorderWidth = 1;
            outer.Contents.Add(floatDiv);

            outer.Contents.Add(new TextLiteral(repeatText + repeatText));

            using (var ms = DocStreams.GetOutputStream("ColumnBalance_23_FloatRight.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var body      = layout.AllPages[0].ContentBlock;
            var container = body.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(container, "Container block should exist");
            Assert.AreEqual(2, container.Columns.Length, "Should have 2 columns");

            var col0 = container.Columns[0];
            var col1 = container.Columns[1];

            Assert.IsTrue(col0.Contents.Count > 0, "Col0 should have content after balance");
            Assert.IsTrue(col1.Contents.Count > 0, "Col1 should have content after balance");

            var h0 = col0.UsedSize.Height.PointsValue;
            var h1 = col1.UsedSize.Height.PointsValue;
            Assert.IsTrue(h0 > 0, "Col0 height should be positive");
            Assert.IsTrue(h1 > 0, "Col1 height should be positive");
            Assert.IsTrue(Math.Abs(h0 - h1) < 60.0,
                $"Column heights should be roughly balanced. col0={h0:F1}pt col1={h1:F1}pt diff={Math.Abs(h0-h1):F1}pt");

            static string DescribeRightFloatItem(PDFLayoutItem item)
            {
                if (item is PDFLayoutBlock b)
                    return $"Block({b.Owner?.GetType().Name},Y={b.TotalBounds.Y:F1},H={b.Height:F1}) ";
                if (item is PDFLayoutLine ln)
                {
                    bool hasFloat = ln.Runs.OfType<PDFLayoutPositionedRegionRun>().Any(r => r.IsFloating);
                    return $"Line(Y={ln.OffsetY:F1},H={ln.Height:F1}{(hasFloat ? ",FLOAT" : "")}) ";
                }
                return $"Unknown(Y={item.OffsetY:F1},H={item.Height:F1}) ";
            }

            var col0Desc = new System.Text.StringBuilder();
            foreach (var item in col0.Contents) col0Desc.Append(DescribeRightFloatItem(item));

            var col1Desc = new System.Text.StringBuilder();
            foreach (var item in col1.Contents) col1Desc.Append(DescribeRightFloatItem(item));

            // Find the line containing the float:right positioned run
            PDFLayoutLine floatLine = null;
            PDFLayoutRegion floatCol = null;
            PDFLayoutPositionedRegionRun floatRun = null;
            double floatOffsetY = -1;

            foreach (var col in new[] { col0, col1 })
            {
                foreach (var item in col.Contents)
                {
                    if (item is PDFLayoutLine ln)
                    {
                        var run = ln.Runs.OfType<PDFLayoutPositionedRegionRun>()
                                    .FirstOrDefault(r => r.IsFloating);
                        if (run != null)
                        {
                            floatLine    = ln;
                            floatCol     = col;
                            floatOffsetY = ln.OffsetY.PointsValue;
                            floatRun     = run;
                            break;
                        }
                    }
                }
                if (floatLine != null) break;
            }

            Assert.IsNotNull(floatLine,
                $"Float line should exist in one of the columns. col0=[{col0Desc}] col1=[{col1Desc}]");

            Assert.AreSame(col1, floatCol,
                $"Float:right line should be in col1 after balancing. col0=[{col0Desc}] col1=[{col1Desc}]");

            Assert.IsTrue(floatOffsetY < h1 * 0.6,
                $"Float should be in the upper 60%% of col1. " +
                $"floatOffsetY={floatOffsetY:F1}pt col1Height={h1:F1}pt. col1=[{col1Desc}]");

            // The float region's TotalBounds.Y should be near zero (top of col1)
            var floatRegionY = floatRun.Region.TotalBounds.Y.PointsValue;
            Assert.IsTrue(floatRegionY < h1 * 0.6,
                $"Float region TotalBounds.Y should be near the top of col1 after balancing. " +
                $"floatRegionY={floatRegionY:F1}pt col1Height={h1:F1}pt");
        }
        
        // =====================================================================
        // 22. Float:left within balanced 2-column text layout.
        //     Text fills col0; the float and remaining text land in col1.
        //     Verifies BalanceColumns correctly redistributes the float-containing
        //     line and that the float region renders at the top of col1.
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_24_MultipleFloatLeft_BalancedAcross3Columns()
        {
            string repeatText =
                "Quisque gravida elementum nisl at ultrices odio suscipit interdum. " +
                "Sed sed diam non sem fringilla varius lorem ipsum dolor sit amet. " +
                "Curabitur viverra ligula ut tellus feugiat mattis curabitur urna. " +
                "Duis molestie mi id tincidunt mattis maecenas consectetur lectus. ";

            var doc = new Document();
            var section = new Section();
            section.Style.PageStyle.Width  = 400;
            section.Style.PageStyle.Height = 700;
            section.FontSize = 10;
            doc.Pages.Add(section);

            var outer = new Div();
            outer.ColumnCount = 3;
            outer.Style.Columns.FillMode = ColumnFillMode.Balance;
            outer.AlleyWidth  = 5;
            outer.BorderColor = StandardColors.Black;
            outer.BorderWidth = 2;
            outer.Padding     = new Thickness(2);
            //outer.Height = 250;
            section.Contents.Add(outer);

            // A small heading block + text together reach just under the balance target,
            // so the float (whose line is only 12pt tall) is the item that tips col0 over
            // and gets redistributed to col1 — exposing any position-update bug.
            var heading = new Div();
            //heading.Height      = 20;
            heading.BorderColor = StandardColors.Black;
            heading.BorderWidth = 1;
            heading.Contents.Add(new TextLiteral("Section heading stretching across more than one line."));
            heading.Padding = new Thickness(10);
            outer.Contents.Add(heading);
            
            var floatDiv = new Div();
            floatDiv.Style.Position.Float = FloatMode.Left;
            floatDiv.Width       = 60;
            floatDiv.Height      = 50;
            floatDiv.BorderColor = StandardColors.Red;
            floatDiv.Margins     = new Thickness(5, 5, 0, 5);
            floatDiv.BorderWidth = 1;
            outer.Contents.Add(floatDiv);
            

            outer.Contents.Add(new TextLiteral(repeatText));
            
            floatDiv = new Div();
            floatDiv.Style.Position.Float = FloatMode.Left;
            floatDiv.Width       = 60;
            floatDiv.Height      = 50;
            floatDiv.BorderColor = StandardColors.Red;
            floatDiv.Margins     = new Thickness(0, 5, 0, 5);
            floatDiv.BorderWidth = 1;
            outer.Contents.Add(floatDiv);
            
            outer.Contents.Add(new TextLiteral(repeatText));
            
            // Float:left div — sits mid-content so after balancing it lands in col1
            floatDiv = new Div();
            floatDiv.Style.Position.Float = FloatMode.Left;
            floatDiv.Width       = 60;
            floatDiv.Height      = 50;
            floatDiv.BorderColor = StandardColors.Red;
            floatDiv.Margins     = new Thickness(0, 5, 0, 5);
            floatDiv.BorderWidth = 1;
            outer.Contents.Add(floatDiv);

            outer.Contents.Add(new TextLiteral(repeatText));
            
            // Float:left div — sits mid-content so after balancing it lands in col1
            floatDiv = new Div();
            floatDiv.Style.Position.Float = FloatMode.Left;
            floatDiv.Width       = 60;
            floatDiv.Height      = 50;
            floatDiv.BorderColor = StandardColors.Red;
            floatDiv.Margins     = new Thickness(0, 5, 0, 5);
            floatDiv.BorderWidth = 1;
            outer.Contents.Add(floatDiv);

            outer.Contents.Add(new TextLiteral(repeatText));
            
            using (var ms = DocStreams.GetOutputStream("ColumnBalance_24_MultipleFloatLeft.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.RenderOptions.StringOutput = OutputStringType.Text;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var body      = layout.AllPages[0].ContentBlock;
            var container = body.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(container, "Container block should exist");
            Assert.AreEqual(3, container.Columns.Length, "Should have 2 columns");

            var col0 = container.Columns[0];
            var col1 = container.Columns[1];
            var col2 = container.Columns[2];

            Assert.IsTrue(col0.Contents.Count > 0, "Col0 should have content after balance");
            Assert.IsTrue(col1.Contents.Count > 0, "Col1 should have content after balance");

            var h0 = col0.UsedSize.Height.PointsValue;
            var h1 = col1.UsedSize.Height.PointsValue;
            Assert.IsTrue(h0 > 0, "Col0 height should be positive");
            Assert.IsTrue(h1 > 0, "Col1 height should be positive");
            Assert.IsTrue(Math.Abs(h0 - h1) < 60.0,
                $"Column heights should be roughly balanced. col0={h0:F1}pt col1={h1:F1}pt diff={Math.Abs(h0-h1):F1}pt");

            static string DescribeFloatItem(PDFLayoutItem item)
            {
                if (item is PDFLayoutBlock b)
                    return $"Block({b.Owner?.GetType().Name},Y={b.TotalBounds.Y:F1},H={b.Height:F1}) ";
                if (item is PDFLayoutLine ln)
                {
                    bool hasFloat = ln.Runs.OfType<PDFLayoutPositionedRegionRun>().Any(r => r.IsFloating);
                    return $"Line(Y={ln.OffsetY:F1},H={ln.Height:F1}{(hasFloat ? ",FLOAT" : "")}) ";
                }
                return $"Unknown(Y={item.OffsetY:F1},H={item.Height:F1}) ";
            }

            var col0Desc = new System.Text.StringBuilder();
            foreach (var item in col0.Contents) col0Desc.Append(DescribeFloatItem(item));

            var col1Desc = new System.Text.StringBuilder();
            foreach (var item in col1.Contents) col1Desc.Append(DescribeFloatItem(item));

            // Find the line containing the float:left positioned run
            double floatOffsetY = -1;

            List<PDFLayoutPositionedRegionRun> floatRuns = new List<PDFLayoutPositionedRegionRun>();
            
            foreach (var col in container.Columns)
            {
                foreach (var item in col.Contents)
                {
                    
                    if (item is PDFLayoutLine ln)
                    {
                        var runs = ln.Runs.OfType<PDFLayoutPositionedRegionRun>()
                                    .Select(r =>
                                    {
                                        return r.IsFloating ? r : null;
                                    });
                        if (null != runs)
                            floatRuns.AddRange(runs);
                    }
                }
            }
            
            Assert.AreEqual(4,  floatRuns.Count, "Should have 4 runs");
            Assert.IsTrue(floatRuns[0].Parent.Parent == col0, "First is on Column 1");
            Assert.IsTrue(floatRuns[1].Parent.Parent == col0,  "Second is on Column 1");
            Assert.IsTrue(floatRuns[2].Parent.Parent == col1,  "Third is on Column 2");
            Assert.IsTrue(floatRuns[3].Parent.Parent == col2, "Third is on Column 3");
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ColumnBalance_25_MultipleFloatMixed_BalancedAcross3Columns()
        {
            string repeatText =
                "Quisque gravida elementum nisl at ultrices odio suscipit interdum. " +
                "Sed sed diam non sem fringilla varius lorem ipsum dolor sit amet. " +
                "Curabitur viverra ligula ut tellus feugiat mattis curabitur urna. " +
                "Duis molestie mi id tincidunt mattis maecenas consectetur lectus. ";

            var doc = new Document();
            var section = new Section();
            section.Style.PageStyle.Width  = 400;
            section.Style.PageStyle.Height = 700;
            section.FontSize = 10;
            doc.Pages.Add(section);

            var outer = new Div();
            outer.ColumnCount = 3;
            outer.Style.Columns.FillMode = ColumnFillMode.Balance;
            outer.AlleyWidth  = 5;
            outer.BorderColor = StandardColors.Black;
            outer.BorderWidth = 2;
            outer.Padding     = new Thickness(2);
            //outer.Height = 250;
            section.Contents.Add(outer);

            // A small heading block + text together reach just under the balance target,
            // so the float (whose line is only 12pt tall) is the item that tips col0 over
            // and gets redistributed to col1 — exposing any position-update bug.
            var heading = new Div();
            //heading.Height      = 20;
            heading.BorderColor = StandardColors.Black;
            heading.BorderWidth = 1;
            heading.Contents.Add(new TextLiteral("Section heading stretching across more than one line."));
            heading.Padding = new Thickness(10);
            outer.Contents.Add(heading);
            
            var floatDiv = new Div();
            floatDiv.Style.Position.Float = FloatMode.Left;
            floatDiv.Width       = 60;
            floatDiv.Height      = 50;
            floatDiv.BorderColor = StandardColors.Red;
            floatDiv.Margins     = new Thickness(5, 5, 0, 5);
            floatDiv.BorderWidth = 1;
            outer.Contents.Add(floatDiv);
            

            outer.Contents.Add(new TextLiteral(repeatText));
            
            floatDiv = new Div();
            floatDiv.Style.Position.Float = FloatMode.Right;
            floatDiv.Width       = 60;
            floatDiv.Height      = 50;
            floatDiv.BorderColor = StandardColors.Red;
            floatDiv.Margins     = new Thickness(0, 5, 0, 5);
            floatDiv.BorderWidth = 1;
            outer.Contents.Add(floatDiv);
            
            outer.Contents.Add(new TextLiteral(repeatText));
            
            // Float:left div — sits mid-content so after balancing it lands in col1
            floatDiv = new Div();
            floatDiv.Style.Position.Float = FloatMode.Left;
            floatDiv.Width       = 60;
            floatDiv.Height      = 50;
            floatDiv.BorderColor = StandardColors.Red;
            floatDiv.Margins     = new Thickness(0, 5, 0, 5);
            floatDiv.BorderWidth = 1;
            outer.Contents.Add(floatDiv);

            outer.Contents.Add(new TextLiteral(repeatText));
            
            // Float:left div — sits mid-content so after balancing it lands in col1
            floatDiv = new Div();
            floatDiv.Style.Position.Float = FloatMode.Right;
            floatDiv.Width       = 60;
            floatDiv.Height      = 50;
            floatDiv.BorderColor = StandardColors.Red;
            floatDiv.Margins     = new Thickness(0, 5, 0, 5);
            floatDiv.BorderWidth = 1;
            outer.Contents.Add(floatDiv);

            outer.Contents.Add(new TextLiteral(repeatText));
            
            using (var ms = DocStreams.GetOutputStream("ColumnBalance_25_MultipleFloatMixed.pdf"))
            {
                //doc.RenderOptions.Compression = OutputCompressionType.None;
                //doc.RenderOptions.StringOutput = OutputStringType.Text;
                doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var body      = layout.AllPages[0].ContentBlock;
            var container = body.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(container, "Container block should exist");
            Assert.AreEqual(3, container.Columns.Length, "Should have 2 columns");

            var col0 = container.Columns[0];
            var col1 = container.Columns[1];
            var col2 = container.Columns[2];

            Assert.IsTrue(col0.Contents.Count > 0, "Col0 should have content after balance");
            Assert.IsTrue(col1.Contents.Count > 0, "Col1 should have content after balance");

            var h0 = col0.UsedSize.Height.PointsValue;
            var h1 = col1.UsedSize.Height.PointsValue;
            Assert.IsTrue(h0 > 0, "Col0 height should be positive");
            Assert.IsTrue(h1 > 0, "Col1 height should be positive");
            Assert.IsTrue(Math.Abs(h0 - h1) < 60.0,
                $"Column heights should be roughly balanced. col0={h0:F1}pt col1={h1:F1}pt diff={Math.Abs(h0-h1):F1}pt");

            static string DescribeFloatItem(PDFLayoutItem item)
            {
                if (item is PDFLayoutBlock b)
                    return $"Block({b.Owner?.GetType().Name},Y={b.TotalBounds.Y:F1},H={b.Height:F1}) ";
                if (item is PDFLayoutLine ln)
                {
                    bool hasFloat = ln.Runs.OfType<PDFLayoutPositionedRegionRun>().Any(r => r.IsFloating);
                    return $"Line(Y={ln.OffsetY:F1},H={ln.Height:F1}{(hasFloat ? ",FLOAT" : "")}) ";
                }
                return $"Unknown(Y={item.OffsetY:F1},H={item.Height:F1}) ";
            }

            var col0Desc = new System.Text.StringBuilder();
            foreach (var item in col0.Contents) col0Desc.Append(DescribeFloatItem(item));

            var col1Desc = new System.Text.StringBuilder();
            foreach (var item in col1.Contents) col1Desc.Append(DescribeFloatItem(item));

            // Find the line containing the float:left positioned run
            double floatOffsetY = -1;

            List<PDFLayoutPositionedRegionRun> floatRuns = new List<PDFLayoutPositionedRegionRun>();
            
            foreach (var col in container.Columns)
            {
                foreach (var item in col.Contents)
                {
                    
                    if (item is PDFLayoutLine ln)
                    {
                        var runs = ln.Runs.OfType<PDFLayoutPositionedRegionRun>()
                                    .Select(r =>
                                    {
                                        return r.IsFloating ? r : null;
                                    });
                        if (null != runs)
                            floatRuns.AddRange(runs);
                    }
                }
            }
            
            Assert.AreEqual(4,  floatRuns.Count, "Should have 4 runs");
            Assert.IsTrue(floatRuns[0].Parent.Parent == col0, "First is on Column 1");
            Assert.IsTrue(floatRuns[1].Parent.Parent == col0,  "Second is on Column 1");
            Assert.IsTrue(floatRuns[2].Parent.Parent == col1,  "Third is on Column 2");
            Assert.IsTrue(floatRuns[3].Parent.Parent == col2, "Third is on Column 3");
        }
    }
}
