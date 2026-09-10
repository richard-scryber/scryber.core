using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Graphics;
using Scryber.PDF.Layout;
using Scryber.Styles;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Layout tests for &lt;hr&gt; (HTMLHorizontalRule), now a genuine box-model block (border-bottom
    /// by default) rather than a stroked Line. No prior test coverage existed for this element.
    /// </summary>
    [TestClass()]
    public class HorizontalRuleLayout_Tests
    {
        private const string TestCategory = "Layout-HR";
        private const double PageW = 600;
        private const double PageH = 400;

        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        private static PDFLayoutBlock GetHrBlock(PDFLayoutRegion pageRegion, int contentIndex = 0)
        {
            return pageRegion.Contents[contentIndex] as PDFLayoutBlock;
        }

        /// <summary>
        /// TotalBounds is the full margin box (see PDFLayoutBlock.CalculateTotalBounds - Position.Margins
        /// are added into both Width and Height), not the border/content box - so a test that wants the
        /// rendered box height (border-bottom + background, excluding the default 0.5em margins) has to
        /// subtract the margins back out rather than reading TotalBounds.Height directly.
        /// </summary>
        private static Unit GetBorderBoxHeight(PDFLayoutBlock block)
        {
            return block.TotalBounds.Height - block.Position.Margins.Top - block.Position.Margins.Bottom;
        }

        /// <summary>
        /// CreateBorderPen() only populates a side-specific pen (TopPen/LeftPen/etc.) when that side had
        /// its own explicit per-side style value - a plain general shorthand like `border: 3pt solid red;`
        /// (no per-side keys at all) resolves onto AllPen/AllSides instead, leaving the per-side pens null.
        /// This resolves the pen that will actually be used to draw a given side, mirroring what the
        /// renderer itself does.
        /// </summary>
        private static PDFPen GetEffectiveSidePen(PDFPenBorders borders, Sides side)
        {
            switch (side)
            {
                case Sides.Top:
                    if (null != borders.TopPen) return borders.TopPen;
                    break;
                case Sides.Left:
                    if (null != borders.LeftPen) return borders.LeftPen;
                    break;
                case Sides.Right:
                    if (null != borders.RightPen) return borders.RightPen;
                    break;
                case Sides.Bottom:
                    if (null != borders.BottomPen) return borders.BottomPen;
                    break;
            }

            if ((borders.AllSides & side) == side)
                return borders.AllPen;

            return null;
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void HR_Default_NoCss_FullWidthWithBottomBorderOnly()
        {
            var html = @"<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style='margin:0; padding:0;'>
  <hr />
</body>
</html>";

            using var doc = Document.ParseDocument(new System.IO.StringReader(html),
                Scryber.ParseSourceType.DynamicContent);
            doc.Pages[0].Style.PageStyle.Width = PageW;
            doc.Pages[0].Style.PageStyle.Height = PageH;

            using (var ms = DocStreams.GetOutputStream("HR_Default_NoCss.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should complete");
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var hrBlock = GetHrBlock(pageRegion);
            Assert.IsNotNull(hrBlock, "hr should produce a layout block");

            AssertAreApproxEqual(PageW, hrBlock.TotalBounds.Width.PointsValue,
                "A bare <hr> with no CSS should still span the full available width");

            // No explicit height/content - the box (excluding the default 0.5em top/bottom margins)
            // should collapse to (near) zero height, with only the 1pt border-bottom making it visible,
            // not a fixed line-drawing height.
            AssertAreApproxEqual(0.0, GetBorderBoxHeight(hrBlock).PointsValue,
                "A bare <hr> box should collapse to ~0 content height, not carry an arbitrary fixed height");

            var hr = FindFirstHr(doc);
            Assert.IsNotNull(hr, "Should find the parsed hr component");
            var applied = hr.GetAppliedStyle();
            var borders = applied.CreateBorderPen();
            var bottomPen = GetEffectiveSidePen(borders, Sides.Bottom);

            Assert.IsNotNull(bottomPen, "Default border-bottom should be present");
            Assert.AreEqual(new Unit(1, PageUnits.Points), bottomPen.Width,
                "Default border-bottom width should be 1pt");
            Assert.AreEqual(StandardColors.Black, ((Scryber.PDF.Graphics.PDFSolidPen)bottomPen).Color,
                "Default border-bottom color should be black");
            Assert.AreEqual(LineType.Solid, bottomPen.LineStyle,
                "Default border-bottom style should be solid");

            // Top/left/right must stay unset - a bare <hr> is a single bottom rule, not a full box border.
            var topPen = GetEffectiveSidePen(borders, Sides.Top);
            var leftPen = GetEffectiveSidePen(borders, Sides.Left);
            var rightPen = GetEffectiveSidePen(borders, Sides.Right);
            Assert.IsTrue(topPen == null || topPen.LineStyle == LineType.None, "No top border by default");
            Assert.IsTrue(leftPen == null || leftPen.LineStyle == LineType.None, "No left border by default");
            Assert.IsTrue(rightPen == null || rightPen.LineStyle == LineType.None, "No right border by default");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void HR_ExplicitBorder_OverridesDefaultAndAppliesToAllSides()
        {
            // Real CSS border-* now has an actual effect - previously silently ignored, since
            // Line only ever read stroke-* keys.
            var html = @"<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style='margin:0; padding:0;'>
  <hr style='border: 3pt solid #ff0000;' />
</body>
</html>";

            using var doc = Document.ParseDocument(new System.IO.StringReader(html),
                Scryber.ParseSourceType.DynamicContent);
            doc.Pages[0].Style.PageStyle.Width = PageW;
            doc.Pages[0].Style.PageStyle.Height = PageH;

            using (var ms = DocStreams.GetOutputStream("HR_ExplicitBorder.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var hr = FindFirstHr(doc);
            Assert.IsNotNull(hr, "Should find the parsed hr component");
            var applied = hr.GetAppliedStyle();
            var borders = applied.CreateBorderPen();
            var bottomPen = GetEffectiveSidePen(borders, Sides.Bottom);
            var topPen = GetEffectiveSidePen(borders, Sides.Top);

            var expectedRed = new Color(255, 0, 0);
            Assert.IsNotNull(bottomPen, "Explicit border shorthand should produce a bottom pen");
            Assert.AreEqual(new Unit(3, PageUnits.Points), bottomPen.Width,
                "Explicit border shorthand should override the default 1pt border-bottom width");
            Assert.AreEqual(expectedRed, ((Scryber.PDF.Graphics.PDFSolidPen)bottomPen).Color,
                "Explicit border shorthand should override the default black border-bottom color");
            Assert.IsNotNull(topPen, "Explicit border shorthand (all sides) should also set a top border, unlike the default");
            Assert.AreEqual(new Unit(3, PageUnits.Points), topPen.Width,
                "Explicit border shorthand (all sides) should also set a top border, unlike the default");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void HR_BackgroundColorAndHeight_RendersAFilledBar_NotADiagonalLine()
        {
            // The single most common real-world <hr> reset: no border, a background-colour bar
            // with an explicit height. Previously Line.CreatePath would draw a diagonal line from
            // (0,0) to (width, height) whenever height was set - completely broken for this pattern.
            var html = @"<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style='margin:0; padding:0;'>
  <hr style='border: none; height: 4pt; background-color: #336699; width: 250pt;' />
</body>
</html>";

            using var doc = Document.ParseDocument(new System.IO.StringReader(html),
                Scryber.ParseSourceType.DynamicContent);
            doc.Pages[0].Style.PageStyle.Width = PageW;
            doc.Pages[0].Style.PageStyle.Height = PageH;

            using (var ms = DocStreams.GetOutputStream("HR_BackgroundColorAndHeight.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should complete");
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var hrBlock = GetHrBlock(pageRegion);
            Assert.IsNotNull(hrBlock, "hr should produce a layout block");

            AssertAreApproxEqual(250.0, hrBlock.TotalBounds.Width.PointsValue,
                "Explicit width should be honoured as a normal box width");
            AssertAreApproxEqual(4.0, GetBorderBoxHeight(hrBlock).PointsValue,
                "Explicit height should be a genuine box height, not a diagonal line's rise/run");

            var hr = FindFirstHr(doc);
            var applied = hr.GetAppliedStyle();
            var expectedBlue = new Color(0x33, 0x66, 0x99);
            Assert.AreEqual(expectedBlue, applied.GetValue(StyleKeys.BgColorKey, Color.Transparent),
                "background-color should resolve as a normal fill colour");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void HR_DefaultMargins_HalfEmTopAndBottom()
        {
            var html = @"<html xmlns=""http://www.w3.org/1999/xhtml"">
<body style='margin:0; padding:0; font-size: 12pt;'>
  <p style='margin:0;'>Above</p>
  <hr />
  <p style='margin:0;'>Below</p>
</body>
</html>";

            using var doc = Document.ParseDocument(new System.IO.StringReader(html),
                Scryber.ParseSourceType.DynamicContent);
            doc.Pages[0].Style.PageStyle.Width = PageW;
            doc.Pages[0].Style.PageStyle.Height = PageH;

            using (var ms = DocStreams.GetOutputStream("HR_DefaultMargins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should complete");
            var pageRegion = _layout.AllPages[0].ContentBlock.Columns[0];
            var hrBlock = GetHrBlock(pageRegion, 1);
            Assert.IsNotNull(hrBlock, "hr should produce a layout block");

            // 0.5em at the document's 12pt base font-size = 6pt. Read from the actual laid-out
            // Position.Margins (resolved against the real font-size context during layout) rather than
            // re-deriving from the style directly - Style.CreatePostionOptions() cannot resolve a still-
            // relative (Em) margin outside of the layout pass that flattens it against font-size.
            AssertAreApproxEqual(6.0, hrBlock.Position.Margins.Top.PointsValue,
                "Default top margin should be 0.5em");
            AssertAreApproxEqual(6.0, hrBlock.Position.Margins.Bottom.PointsValue,
                "Default bottom margin should be 0.5em");
        }

        private static Scryber.Html.Components.HTMLHorizontalRule FindFirstHr(Document doc)
        {
            return FindFirstHr(doc as IContainerComponent);
        }

        private static Scryber.Html.Components.HTMLHorizontalRule FindFirstHr(IContainerComponent container)
        {
            if (container == null || !container.HasContent)
                return null;

            foreach (var item in container.Content)
            {
                if (item is Scryber.Html.Components.HTMLHorizontalRule hr)
                    return hr;

                if (item is IContainerComponent child)
                {
                    var found = FindFirstHr(child);
                    if (found != null)
                        return found;
                }
            }
            return null;
        }

        private static void AssertAreApproxEqual(double one, double two, string message = null)
        {
            int precision = 3;
            one = Math.Round(one, precision);
            two = Math.Round(two, precision);
            Assert.AreEqual(one, two, message);
        }
    }
}
