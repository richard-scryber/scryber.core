using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using Scryber.Drawing;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Layout tests for z-index on absolutely and relatively positioned elements.
    /// All tests include visible borders so output PDFs can be visually inspected.
    ///
    /// Naming convention: ZIndex_[scenario]_[expectation]
    /// </summary>
    [TestClass()]
    public class ZIndex_Tests
    {
        const string TestCategoryName = "Layout ZIndex";

        PDFLayoutDocument layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        // =====================================================================
        // Helpers
        // =====================================================================

        /// <summary>Returns the first positioned region from the first block child of body.</summary>
        private PDFLayoutPositionedRegion GetFirstPositionedRegion()
        {
            var body = layout.AllPages[0].ContentBlock;
            var container = body.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(container, "Container block not found");
            Assert.IsTrue(container.HasPositionedRegions, "Container should have positioned regions");
            return container.PositionedRegions[0] as PDFLayoutPositionedRegion;
        }

        private PDFLayoutBlock GetContainerBlock()
        {
            var body = layout.AllPages[0].ContentBlock;
            return body.Columns[0].Contents[0] as PDFLayoutBlock;
        }

        // =====================================================================
        // 1. CSS value parsing via full document round-trip
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ZIndex_PositiveValue_ParsedAndStored()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""position:relative; width:300pt; height:150pt;
               border:2pt solid #000000; padding:5pt;"">
    <div id=""overlay""
         style=""position:absolute; top:20pt; left:20pt; width:100pt; height:60pt;
                z-index:5;
                border:2pt solid #ff0000; background-color:#ffeeee; padding:4pt;"">z-index:5</div>
    <p style=""margin:0; border:1pt dashed #888888;"">Normal flow content</p>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ZIndex_01_PositiveValue.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var reg = GetFirstPositionedRegion();
            Assert.IsNotNull(reg);
            Assert.AreEqual(5, reg.PositionOptions.ZIndex, "z-index:5 should be stored as 5");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ZIndex_NegativeValue_ParsedAndStored()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""position:relative; width:300pt; height:150pt;
               border:2pt solid #000000; padding:5pt;"">
    <div id=""background""
         style=""position:absolute; top:0; left:0; width:300pt; height:150pt;
                z-index:-1;
                border:2pt solid #0000ff; background-color:#ddeeff; padding:4pt;"">z-index:-1 (behind)</div>
    <p style=""margin:0; border:1pt dashed #888888;"">Normal content (should render above blue div)</p>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ZIndex_02_NegativeValue.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var reg = GetFirstPositionedRegion();
            Assert.IsNotNull(reg);
            Assert.AreEqual(-1, reg.PositionOptions.ZIndex, "z-index:-1 should be stored as -1");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ZIndex_AutoValue_TreatedAsZero()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""position:relative; width:300pt; height:150pt;
               border:2pt solid #000000; padding:5pt;"">
    <div id=""auto""
         style=""position:absolute; top:20pt; left:20pt; width:100pt; height:60pt;
                z-index:auto;
                border:2pt solid #008800; background-color:#eeffee; padding:4pt;"">z-index:auto</div>
    <p style=""margin:0; border:1pt dashed #888888;"">Normal flow content</p>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ZIndex_03_AutoValue.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var reg = GetFirstPositionedRegion();
            Assert.IsNotNull(reg);
            Assert.AreEqual(0, reg.PositionOptions.ZIndex, "z-index:auto should map to 0");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ZIndex_NotSet_DefaultsToZero()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""position:relative; width:300pt; height:150pt;
               border:2pt solid #000000; padding:5pt;"">
    <div id=""default""
         style=""position:absolute; top:20pt; left:20pt; width:100pt; height:60pt;
                border:2pt solid #888888; background-color:#f5f5f5; padding:4pt;"">No z-index set</div>
    <p style=""margin:0; border:1pt dashed #aaaaaa;"">Normal flow content</p>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ZIndex_04_NotSet.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var reg = GetFirstPositionedRegion();
            Assert.IsNotNull(reg);
            Assert.AreEqual(0, reg.PositionOptions.ZIndex, "Omitted z-index should default to 0");
        }

        // =====================================================================
        // 2. AssociatedRun wiring
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ZIndex_AssociatedRun_PopulatedOnRegion()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""position:relative; width:300pt; height:150pt;
               border:2pt solid #000000; padding:5pt;"">
    <div style=""position:absolute; top:15pt; left:15pt; width:120pt; height:50pt;
                z-index:3;
                border:2pt solid #cc6600; background-color:#fff3e0; padding:4pt;"">z-index:3</div>
    <p style=""margin:0; border:1pt dashed #888888;"">Body text</p>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ZIndex_05_AssociatedRun.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var reg = GetFirstPositionedRegion();
            Assert.IsNotNull(reg);
            Assert.IsNotNull(reg.AssociatedRun, "AssociatedRun must be set for z-index sorting to work");
            Assert.AreEqual(3, reg.PositionOptions.ZIndex);
        }

        // =====================================================================
        // 3. Multiple positioned children — z-index values all stored correctly
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ZIndex_ThreeChildren_ZIndexValuesAllStored()
        {
            // Back=-1 (blue), Mid=1 (green), Front=2 (red), plus normal flow.
            // Visual expectation: blue fills container behind text; green/red overlap each other
            // with red on top.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""position:relative; width:400pt; height:200pt;
               border:3pt solid #000000; padding:10pt;"">
    <div id=""back""
         style=""position:absolute; top:0; left:0; width:200pt; height:100pt;
                z-index:-1;
                border:2pt solid #0000cc; background-color:#ddeeff; padding:4pt;"">z-index:-1 BACK (blue)</div>
    <div id=""mid""
         style=""position:absolute; top:40pt; left:40pt; width:180pt; height:100pt;
                z-index:1;
                border:2pt solid #007700; background-color:#ccffcc; padding:4pt;"">z-index:1 MID (green)</div>
    <div id=""front""
         style=""position:absolute; top:70pt; left:90pt; width:180pt; height:100pt;
                z-index:2;
                border:2pt solid #cc0000; background-color:#ffcccc; padding:4pt;"">z-index:2 FRONT (red)</div>
    <p style=""margin:5pt 0; border:1pt dashed #555555;"">Normal flow — above back, below mid and front</p>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ZIndex_06_ThreeChildren.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var container = GetContainerBlock();
            Assert.IsNotNull(container);
            Assert.IsTrue(container.HasPositionedRegions);
            Assert.AreEqual(3, container.PositionedRegions.Count, "Expected 3 positioned regions");

            var zValues = new List<int>();
            foreach (PDFLayoutPositionedRegion reg in container.PositionedRegions)
            {
                zValues.Add(reg.PositionOptions.ZIndex);
                Assert.IsNotNull(reg.AssociatedRun, "Each region must have AssociatedRun set");
            }

            Assert.IsTrue(zValues.Contains(-1), "Should include z-index -1");
            Assert.IsTrue(zValues.Contains(1),  "Should include z-index 1");
            Assert.IsTrue(zValues.Contains(2),  "Should include z-index 2");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ZIndex_MixedSigns_AllRegionsHaveCorrectZIndex()
        {
            // Tests five regions: -2, -1, 0, 1, 10
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""position:relative; width:500pt; height:250pt;
               border:3pt solid #000000; padding:5pt;"">
    <div style=""position:absolute; top:0;    left:0;    width:500pt; height:250pt;
                z-index:-2; border:2pt solid #000066; background-color:#e8eaf6; padding:3pt;"">z=-2</div>
    <div style=""position:absolute; top:10pt; left:10pt; width:450pt; height:220pt;
                z-index:-1; border:2pt solid #1565c0; background-color:#e3f2fd; padding:3pt;"">z=-1</div>
    <div style=""position:absolute; top:60pt; left:60pt; width:200pt; height:80pt;
                z-index:0;  border:2pt solid #558b2f; background-color:#f1f8e9; padding:3pt;"">z=0 (default stacking)</div>
    <div style=""position:absolute; top:80pt; left:160pt; width:200pt; height:80pt;
                z-index:1;  border:2pt solid #e65100; background-color:#fff3e0; padding:3pt;"">z=1</div>
    <div style=""position:absolute; top:100pt; left:260pt; width:200pt; height:80pt;
                z-index:10; border:2pt solid #b71c1c; background-color:#ffebee; padding:3pt;"">z=10 (topmost)</div>
    <p style=""margin:5pt 0; border:1pt dashed #555555;"">Normal flow text</p>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ZIndex_07_MixedSigns.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var container = GetContainerBlock();
            Assert.AreEqual(5, container.PositionedRegions.Count, "Expected 5 positioned regions");

            var zMap = new Dictionary<int, int>(); // expected z-index → count
            foreach (PDFLayoutPositionedRegion reg in container.PositionedRegions)
            {
                int z = reg.PositionOptions.ZIndex;
                if (!zMap.ContainsKey(z)) zMap[z] = 0;
                zMap[z]++;
            }
            Assert.IsTrue(zMap.ContainsKey(-2), "Missing z-index -2");
            Assert.IsTrue(zMap.ContainsKey(-1), "Missing z-index -1");
            Assert.IsTrue(zMap.ContainsKey(0),  "Missing z-index 0");
            Assert.IsTrue(zMap.ContainsKey(1),  "Missing z-index 1");
            Assert.IsTrue(zMap.ContainsKey(10), "Missing z-index 10");
        }

        // =====================================================================
        // 4. Various position types
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ZIndex_AbsolutePositioned_ZIndexStored()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""position:relative; width:350pt; height:180pt;
               border:2pt solid #000000; padding:10pt;"">
    <div style=""position:absolute; top:30pt; left:30pt; width:140pt; height:80pt;
                z-index:4;
                border:2pt solid #9c27b0; background-color:#f3e5f5; padding:5pt;"">
      Absolute z-index:4
    </div>
    <p style=""margin:0; border:1pt dashed #999999;"">Relative container text</p>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ZIndex_08_Absolute.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var reg = GetFirstPositionedRegion();
            Assert.IsNotNull(reg);
            Assert.AreEqual(PositionMode.Absolute, reg.PositionMode, "Should be absolute");
            Assert.AreEqual(4, reg.PositionOptions.ZIndex);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ZIndex_RelativeContainer_AbsoluteChild_ZIndexStored()
        {
            // Verifies z-index on an absolute child inside a relative container —
            // the typical CSS stacking scenario.
            // Note: position:relative on the container makes it the positioned ancestor;
            // the absolute child IS added to PositionedRegions of the relative container's block.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:10pt;"">
  <div style=""position:relative; width:350pt; height:200pt;
               border:2pt solid #000000; padding:10pt;"">
    <div style=""position:absolute; top:20pt; left:20pt; width:150pt; height:60pt;
                z-index:2;
                border:2pt solid #ff6f00; background-color:#fff8e1; padding:5pt;"">
      Absolute z-index:2 inside relative container
    </div>
    <p style=""margin:5pt 0; border:1pt dashed #999999;"">Sibling text below absolute child</p>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ZIndex_09_RelativeContainer.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var container = GetContainerBlock();
            Assert.IsNotNull(container);
            Assert.IsTrue(container.HasPositionedRegions, "Relative container should have an absolute child as positioned region");
            var reg = container.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(reg);
            Assert.AreEqual(PositionMode.Absolute, reg.PositionMode, "Child should be absolute");
            Assert.AreEqual(2, reg.PositionOptions.ZIndex, "z-index should be 2");
        }

        // =====================================================================
        // 5. Nested children with z-index
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ZIndex_NestedChildren_InnerAbsoluteHasZIndex()
        {
            // Outer container is position:relative; inner div is position:absolute with z-index.
            // The inner div's children are also positioned with z-index.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""position:relative; width:400pt; height:220pt;
               border:3pt solid #000000; padding:10pt;"">
    <!-- Outer positioned child -->
    <div style=""position:absolute; top:10pt; left:10pt; width:360pt; height:180pt;
                z-index:1;
                border:2pt solid #1976d2; background-color:#e3f2fd; padding:8pt;"">
      Outer absolute z-index:1
      <!-- Inner positioned grandchild -->
      <div style=""position:absolute; top:20pt; left:20pt; width:150pt; height:60pt;
                  z-index:2;
                  border:2pt solid #c62828; background-color:#ffcdd2; padding:5pt;"">Inner absolute z-index:2</div>
      <p style=""margin:0; border:1pt dashed #90a4ae;"">Inner normal flow</p>
    </div>
    <p style=""margin:5pt 0; border:1pt dashed #555555;"">Outer normal flow text</p>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ZIndex_10_NestedChildren.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);

            // Outer container
            var outerContainer = GetContainerBlock();
            Assert.IsNotNull(outerContainer);
            Assert.IsTrue(outerContainer.HasPositionedRegions, "Outer container should have a positioned region");
            Assert.AreEqual(1, outerContainer.PositionedRegions.Count, "Outer should have one positioned child");

            var outerReg = outerContainer.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(outerReg);
            Assert.AreEqual(1, outerReg.PositionOptions.ZIndex, "Outer positioned child should be z-index:1");

            // Inner container (the outer positioned region contains a block, which has its own positioned region)
            var innerBlock = outerReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock, "Outer region should contain a layout block");
            Assert.IsTrue(innerBlock.HasPositionedRegions, "Inner block should have a nested positioned region");

            var innerReg = innerBlock.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(innerReg);
            Assert.AreEqual(2, innerReg.PositionOptions.ZIndex, "Inner positioned grandchild should be z-index:2");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ZIndex_NestedContainers_EachHaveOwnZIndex()
        {
            // Two sibling containers each with their own absolutely positioned children.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""position:relative; width:500pt; height:280pt;
               border:3pt solid #000000; padding:5pt;"">

    <!-- First sibling container -->
    <div style=""position:absolute; top:10pt; left:10pt; width:220pt; height:240pt;
                border:2pt solid #1565c0; background-color:#e8eaf6; padding:8pt;"">
      Container A
      <div style=""position:absolute; top:30pt; left:10pt; width:180pt; height:60pt;
                  z-index:2;
                  border:2pt solid #0d47a1; background-color:#bbdefb; padding:4pt;"">A child z-index:2</div>
      <div style=""position:absolute; top:110pt; left:10pt; width:180pt; height:60pt;
                  z-index:-1;
                  border:2pt solid #1a237e; background-color:#c5cae9; padding:4pt;"">A child z-index:-1</div>
      <p style=""margin:5pt 0; border:1pt dashed #7986cb;"">A normal text</p>
    </div>

    <!-- Second sibling container -->
    <div style=""position:absolute; top:10pt; left:260pt; width:220pt; height:240pt;
                border:2pt solid #558b2f; background-color:#f9fbe7; padding:8pt;"">
      Container B
      <div style=""position:absolute; top:30pt; left:10pt; width:180pt; height:60pt;
                  z-index:3;
                  border:2pt solid #33691e; background-color:#dcedc8; padding:4pt;"">B child z-index:3</div>
      <div style=""position:absolute; top:110pt; left:10pt; width:180pt; height:60pt;
                  z-index:1;
                  border:2pt solid #1b5e20; background-color:#c8e6c9; padding:4pt;"">B child z-index:1</div>
      <p style=""margin:5pt 0; border:1pt dashed #aed581;"">B normal text</p>
    </div>

  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ZIndex_11_NestedContainers.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var outerContainer = GetContainerBlock();
            Assert.IsNotNull(outerContainer);

            // Outer has 2 sibling containers (both absolute, no z-index on them)
            Assert.AreEqual(2, outerContainer.PositionedRegions.Count, "Outer should have 2 sibling containers");

            // Each sibling container's block has 2 positioned children
            foreach (PDFLayoutPositionedRegion siblingReg in outerContainer.PositionedRegions)
            {
                var siblingBlock = siblingReg.Contents[0] as PDFLayoutBlock;
                Assert.IsNotNull(siblingBlock);
                Assert.IsTrue(siblingBlock.HasPositionedRegions, "Each sibling should have inner positioned regions");
                Assert.AreEqual(2, siblingBlock.PositionedRegions.Count, "Each sibling should have 2 inner positioned children");
            }

            // Verify specific z-index values in Container A
            var containerA = (outerContainer.PositionedRegions[0] as PDFLayoutPositionedRegion).Contents[0] as PDFLayoutBlock;
            var aZValues = new List<int>();
            foreach (PDFLayoutPositionedRegion r in containerA.PositionedRegions)
                aZValues.Add(r.PositionOptions.ZIndex);
            Assert.IsTrue(aZValues.Contains(2),  "Container A should have child z-index:2");
            Assert.IsTrue(aZValues.Contains(-1), "Container A should have child z-index:-1");

            // Verify specific z-index values in Container B
            var containerB = (outerContainer.PositionedRegions[1] as PDFLayoutPositionedRegion).Contents[0] as PDFLayoutBlock;
            var bZValues = new List<int>();
            foreach (PDFLayoutPositionedRegion r in containerB.PositionedRegions)
                bZValues.Add(r.PositionOptions.ZIndex);
            Assert.IsTrue(bZValues.Contains(3), "Container B should have child z-index:3");
            Assert.IsTrue(bZValues.Contains(1), "Container B should have child z-index:1");
        }

        // =====================================================================
        // 6. Z-index via CSS class (not inline style)
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ZIndex_CSSClass_ZIndexParsedFromClass()
        {
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <style>
    .container { position:relative; width:350pt; height:180pt;
                 border:3pt solid #000000; padding:8pt; }
    .overlay   { position:absolute; top:20pt; left:20pt; width:120pt; height:70pt;
                 z-index:6;
                 border:2pt solid #6a1b9a; background-color:#f3e5f5; padding:4pt; }
    .underlay  { position:absolute; top:5pt;  left:5pt;  width:340pt; height:168pt;
                 z-index:-1;
                 border:2pt solid #4a148c; background-color:#ede7f6; padding:4pt; }
    p          { margin:0; border:1pt dashed #999999; }
  </style>
</head>
<body style=""margin:0; padding:0;"">
  <div class=""container"">
    <div class=""underlay"">CSS class underlay z-index:-1</div>
    <div class=""overlay"">CSS class overlay z-index:6</div>
    <p>Normal paragraph text</p>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ZIndex_12_CSSClass.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var container = GetContainerBlock();
            Assert.IsNotNull(container);
            Assert.AreEqual(2, container.PositionedRegions.Count, "Should have 2 positioned regions");

            var zValues = new List<int>();
            foreach (PDFLayoutPositionedRegion reg in container.PositionedRegions)
                zValues.Add(reg.PositionOptions.ZIndex);

            Assert.IsTrue(zValues.Contains(-1), "Should have underlay with z-index:-1");
            Assert.IsTrue(zValues.Contains(6),  "Should have overlay with z-index:6");
        }

        // =====================================================================
        // 7. Z-index with content inside the positioned regions
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ZIndex_PositionedRegion_ContainsContentBlocks()
        {
            // The positioned div has multiple paragraphs inside it.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""position:relative; width:400pt; height:250pt;
               border:3pt solid #000000; padding:10pt;"">
    <div style=""position:absolute; top:0; left:0; width:400pt; height:250pt;
                z-index:-1;
                border:2pt solid #1565c0; background-color:#e3f2fd; padding:10pt;"">
      <p style=""margin:0 0 5pt 0; border:1pt solid #90caf9;"">Back para 1</p>
      <p style=""margin:0;         border:1pt solid #90caf9;"">Back para 2</p>
    </div>
    <div style=""position:absolute; top:60pt; left:60pt; width:200pt; height:100pt;
                z-index:2;
                border:2pt solid #c62828; background-color:#ffcdd2; padding:8pt;"">
      <p style=""margin:0 0 5pt 0; border:1pt solid #ef9a9a;"">Front para 1</p>
      <p style=""margin:0;         border:1pt solid #ef9a9a;"">Front para 2</p>
    </div>
    <p style=""margin:5pt 0; border:1pt dashed #777777;"">Normal flow text</p>
  </div>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ZIndex_13_ContentInRegions.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var container = GetContainerBlock();
            Assert.AreEqual(2, container.PositionedRegions.Count);

            foreach (PDFLayoutPositionedRegion reg in container.PositionedRegions)
            {
                Assert.IsNotNull(reg.AssociatedRun, "AssociatedRun should be set");
                Assert.IsTrue(reg.Contents.Count > 0, "Positioned region should contain layout items");
            }

            // Find and verify each by z-index
            foreach (PDFLayoutPositionedRegion reg in container.PositionedRegions)
            {
                if (reg.PositionOptions.ZIndex == -1)
                {
                    var blk = reg.Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(blk, "Back region should contain a layout block");
                }
                else if (reg.PositionOptions.ZIndex == 2)
                {
                    var blk = reg.Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(blk, "Front region should contain a layout block");
                }
            }
        }

        // =====================================================================
        // 8. Document-level absolute (no relative parent)
        // =====================================================================

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ZIndex_PageLevelAbsolute_ZIndexStored()
        {
            // Element is absolutely positioned relative to the page, not a relative container.
            var src = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style=""margin:0; padding:0;"">
  <div style=""position:absolute; top:20pt; left:20pt; width:200pt; height:100pt;
              z-index:5;
              border:2pt solid #e53935; background-color:#ffebee; padding:6pt;"">
    Page-absolute z-index:5
  </div>
  <p style=""margin:100pt 0 0 0; border:1pt solid #aaaaaa; padding:5pt;"">Normal page content below the absolute div</p>
</body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                using (var ms = DocStreams.GetOutputStream("ZIndex_14_PageAbsolute.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }

            Assert.IsNotNull(layout);
            var page = layout.AllPages[0];
            var body = page.ContentBlock;

            // The absolute div will be in a positioned region on the page content block
            Assert.IsTrue(body.HasPositionedRegions, "Body should have page-level positioned regions");
            var reg = body.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(reg);
            Assert.AreEqual(5, reg.PositionOptions.ZIndex, "Page-level absolute should have z-index:5");
        }
    }
}
