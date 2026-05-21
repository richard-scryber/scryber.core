using System;
using System.Collections.Generic;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.PDF.Layout
{
    public class LayoutEngineFlexBox : LayoutEnginePanel
    {
        // True while we are in row layout mode — gates the column-break injection.
        private bool _isRowMode;

        // True when flex-direction is row-reverse or column-reverse.
        private bool _reverseItems;

        // Wrap mode row range: which visible flex items belong to the current row.
        // -1 = not in wrap mode.
        private int _wrapRowStart = -1;
        private int _wrapRowEnd   = -1;

        public LayoutEngineFlexBox(ContainerComponent container, IPDFLayoutEngine parent)
            : base(container, parent)
        {
        }

        protected override void DoLayoutBlockComponent(PDFPositionOptions position, PDFColumnOptions columnOptions)
        {
            var flex      = this.FullStyle.Flex;
            var direction = flex.Direction;

            if (direction == FlexDirection.Column || direction == FlexDirection.ColumnReverse)
            {
                var gap    = flex.Gap;
                var rowGap = this.FullStyle.IsValueDefined(StyleKeys.FlexRowGapKey) ? flex.RowGap : gap;
                if (rowGap.PointsValue > 0)
                    columnOptions = new PDFColumnOptions() { AlleyWidth = rowGap };

                _isRowMode     = false;
                _reverseItems  = (direction == FlexDirection.ColumnReverse);
                base.DoLayoutBlockComponent(position, columnOptions);
                _reverseItems  = false;
            }
            else
            {
                int childCount = CountVisibleChildren();
                if (childCount <= 0)
                {
                    _isRowMode = false;
                    base.DoLayoutBlockComponent(position, columnOptions);
                    return;
                }

                var gap    = flex.Gap;
                var colGap = this.FullStyle.IsValueDefined(StyleKeys.FlexColumnGapKey) ? flex.ColumnGap : gap;

                double containerW;
                if (position.Width.HasValue)
                {
                    containerW = position.Width.Value.PointsValue;
                    // position.Width is the outer/border-box width; subtract padding so
                    // fractions are computed against the same inner width that
                    // GetPercentColumnWidths receives (avail.Width).
                    if (!position.Padding.IsEmpty)
                        containerW -= (position.Padding.Left + position.Padding.Right).PointsValue;
                }
                else
                {
                    containerW = this.DocumentLayout.CurrentPage.LastOpenBlock()?.AvailableBounds.Width.PointsValue ?? 0;
                }

                // Check for wrap mode
                var wrapMode = flex.Wrap;
                if (wrapMode == FlexWrap.Wrap || wrapMode == FlexWrap.WrapReverse)
                {
                    LayoutWrapRows(position, flex, flex.AlignItems, flex.JustifyContent, containerW, colGap);
                    return;
                }

                bool reverse = (direction == FlexDirection.RowReverse);
                var widths = ComputeColumnWidths(childCount, containerW, colGap.PointsValue);
                if (reverse) widths = ReverseWidths(widths);

                var rowCols = new PDFColumnOptions()
                {
                    ColumnCount  = childCount,
                    AlleyWidth   = colGap,
                    ColumnWidths = widths
                };

                // Capture parent region so we can find the new block after layout.
                var parentBlock  = this.DocumentLayout.CurrentPage.LastOpenBlock();
                var parentRegion = parentBlock?.CurrentRegion;
                int priorCount   = parentRegion?.Contents.Count ?? 0;

                _isRowMode    = true;
                _reverseItems = reverse;
                base.DoLayoutBlockComponent(position, rowCols);
                _isRowMode    = false;
                _reverseItems = false;

                // Post-layout: apply align-items and justify-content.
                if (parentRegion != null && parentRegion.Contents.Count > priorCount)
                {
                    var flexBlock = parentRegion.Contents[parentRegion.Contents.Count - 1] as PDFLayoutBlock;
                    if (flexBlock != null && flexBlock.Columns.Length > 0)
                    {
                        var align   = flex.AlignItems;
                        var justify = NormaliseJustify(flex.JustifyContent, reverse);

                        if (align != FlexAlignMode.Stretch && align != FlexAlignMode.FlexStart)
                            ApplyAlignItems(flexBlock, align);

                        if (justify != FlexJustify.FlexStart)
                            ApplyJustifyContent(flexBlock, justify);
                    }
                }
            }
        }

        /// <summary>
        /// Lays out each wrap-row as a separate multi-column block.
        /// Before each row (after the first), checks whether the current page has enough
        /// vertical space to accommodate a row of the same height as the previous one.
        /// If not, a page-break is forced before the row is created, ensuring all columns
        /// in a row land on the same page instead of being split one-per-page.
        /// </summary>
        private void LayoutWrapRows(PDFPositionOptions position, FlexStyle flex,
            FlexAlignMode align, FlexJustify justify, double containerW, Unit colGap)
        {
            var rows = ComputeWrapRows(containerW, colGap.PointsValue);
            double prevRowH = 0;
            bool   reverse  = (flex.Direction == FlexDirection.RowReverse);

            foreach (var (rowStart, rowEnd) in rows)
            {
                int rowItemCount = rowEnd - rowStart;
                if (rowItemCount <= 0) continue;

                // From the second row onward: if available page height is less than the
                // height of the previous row, force a move to the next page so that all
                // columns of this row start together rather than overflowing individually.
                if (prevRowH > 0.5)
                {
                    var currBlock = this.DocumentLayout.CurrentPage.LastOpenBlock();
                    if (currBlock != null)
                    {
                        double availH = currBlock.AvailableBounds.Height.PointsValue;
                        if (availH < prevRowH - 0.5)
                        {
                            var reg = currBlock.CurrentRegion;
                            bool newPage;
                            this.MoveToNextRegion(new Unit(prevRowH, PageUnits.Points), ref reg, ref currBlock, out newPage);
                        }
                    }
                }

                _wrapRowStart = rowStart;
                _wrapRowEnd   = rowEnd;

                var widths  = ComputeColumnWidths(rowItemCount, containerW, colGap.PointsValue, rowStart);
                if (reverse) widths = ReverseWidths(widths);

                var rowCols = new PDFColumnOptions()
                {
                    ColumnCount  = rowItemCount,
                    AlleyWidth   = colGap,
                    ColumnWidths = widths
                };

                // Re-capture parent after any page move.
                var parentBlock  = this.DocumentLayout.CurrentPage.LastOpenBlock();
                var parentRegion = parentBlock?.CurrentRegion;
                int priorCount   = parentRegion?.Contents.Count ?? 0;

                _isRowMode    = true;
                _reverseItems = reverse;
                base.DoLayoutBlockComponent(position, rowCols);
                _isRowMode    = false;
                _reverseItems = false;

                PDFLayoutBlock flexBlock = null;
                if (parentRegion != null && parentRegion.Contents.Count > priorCount)
                {
                    flexBlock = parentRegion.Contents[parentRegion.Contents.Count - 1] as PDFLayoutBlock;
                    if (flexBlock != null && flexBlock.Columns.Length > 0)
                    {
                        if (align != FlexAlignMode.Stretch && align != FlexAlignMode.FlexStart)
                            ApplyAlignItems(flexBlock, align);
                        var rowJustify = NormaliseJustify(justify, reverse);
                        if (rowJustify != FlexJustify.FlexStart)
                            ApplyJustifyContent(flexBlock, rowJustify);
                    }
                }

                // Track the row height so the next iteration can pre-check page space.
                if (flexBlock != null)
                    prevRowH = flexBlock.TotalBounds.Height.PointsValue;
            }
            _wrapRowStart = -1;
            _wrapRowEnd   = -1;
        }

        /// <summary>
        /// Groups visible flex items into rows based on their fixed widths and the container width.
        /// Items with grow > 0 (minWidth = 0) never trigger a break on their own.
        /// </summary>
        private List<(int start, int end)> ComputeWrapRows(double containerW, double gapPts)
        {
            var rows = new List<(int start, int end)>();
            var container = this.Component as IContainerComponent;
            if (container == null || !container.HasContent) return rows;

            var minWidths = new List<double>();
            foreach (var child in container.Content)
            {
                if (!IsFlexItem(child)) continue;
                minWidths.Add(GetItemMinWidth((Component)child));
            }

            if (minWidths.Count == 0) return rows;

            int    rowStart  = 0;
            double rowFixedW = minWidths[0];

            for (int i = 1; i < minWidths.Count; i++)
            {
                double itemW = minWidths[i];
                // total = sum of item widths + gaps for (i - rowStart + 1) items
                double total = rowFixedW + itemW + gapPts * (i - rowStart);
                if (itemW > 0 && total > containerW + 0.5)
                {
                    rows.Add((rowStart, i));
                    rowStart  = i;
                    rowFixedW = itemW;
                }
                else
                {
                    rowFixedW += itemW;
                }
            }
            rows.Add((rowStart, minWidths.Count));
            return rows;
        }

        /// <summary>
        /// Returns the fixed minimum width for a flex item (from explicit width or flex-basis).
        /// Returns 0 for grow-only items.
        /// </summary>
        private static double GetItemMinWidth(Component item)
        {
            if (item is IStyledComponent sc && sc.Style != null)
            {
                if (sc.Style.IsValueDefined(StyleKeys.SizeWidthKey))
                    return sc.Style.Size.Width.PointsValue;
                if (sc.Style.IsValueDefined(StyleKeys.FlexBasisKey) && !sc.Style.Flex.BasisAuto)
                    return sc.Style.Flex.Basis.PointsValue;
            }
            return 0;
        }

        /// <summary>
        /// Override DoLayoutChildren: in row mode, force a column break after each flex item
        /// so each child occupies exactly one column region.
        /// In wrap mode, only the items in [_wrapRowStart, _wrapRowEnd) are rendered.
        /// </summary>
        protected override void DoLayoutChildren(ComponentList children)
        {
            if (!_isRowMode)
            {
                if (!_reverseItems)
                {
                    base.DoLayoutChildren(children);
                    return;
                }

                // column-reverse: render children in reverse source order.
                var all = new List<Component>();
                foreach (Component c in children)
                    if (c.Visible) all.Add(c);

                for (int k = all.Count - 1; k >= 0; k--)
                {
                    this.DoLayoutAChild(all[k]);
                    if (!this.ContinueLayout
                        || this.DocumentLayout.CurrentPage.IsClosed
                        || this.DocumentLayout.CurrentPage.CurrentBlock == null)
                        break;
                }
                return;
            }

            if (!_reverseItems)
            {
                // Original forward row logic.
                int  visIdx = -1;
                bool first  = true;

                foreach (Component comp in children)
                {
                    if (!comp.Visible) continue;

                    bool isItem = IsFlexItem(comp);

                    if (isItem)
                    {
                        visIdx++;

                        if (_wrapRowStart >= 0 && (visIdx < _wrapRowStart || visIdx >= _wrapRowEnd))
                            continue;

                        if (!first)
                        {
                            PDFLayoutBlock block = this.DocumentLayout.CurrentPage.LastOpenBlock();
                            PDFLayoutRegion reg  = block.CurrentRegion;
                            bool newPage;
                            this.MoveToNextRegion(Unit.Zero, ref reg, ref block, out newPage);
                        }
                        first = false;
                    }
                    else if (_wrapRowStart > 0)
                    {
                        continue;
                    }

                    this.DoLayoutAChild(comp);

                    if (!this.ContinueLayout
                        || this.DocumentLayout.CurrentPage.IsClosed
                        || this.DocumentLayout.CurrentPage.CurrentBlock == null)
                        break;
                }
                return;
            }

            // row-reverse: collect flex items in [_wrapRowStart, _wrapRowEnd), reverse, then layout.
            var flexItems = new List<Component>();
            {
                int vi = -1;
                foreach (Component comp in children)
                {
                    if (!comp.Visible || !IsFlexItem(comp)) continue;
                    vi++;
                    if (_wrapRowStart >= 0 && (vi < _wrapRowStart || vi >= _wrapRowEnd)) continue;
                    flexItems.Add(comp);
                }
            }
            flexItems.Reverse();

            {
                bool first = true;
                foreach (var comp in flexItems)
                {
                    if (!first)
                    {
                        PDFLayoutBlock block = this.DocumentLayout.CurrentPage.LastOpenBlock();
                        PDFLayoutRegion reg  = block.CurrentRegion;
                        bool newPage;
                        this.MoveToNextRegion(Unit.Zero, ref reg, ref block, out newPage);
                    }
                    first = false;

                    this.DoLayoutAChild(comp);

                    if (!this.ContinueLayout
                        || this.DocumentLayout.CurrentPage.IsClosed
                        || this.DocumentLayout.CurrentPage.CurrentBlock == null)
                        break;
                }
            }
        }

        // -----------------------------------------------------------------------
        // Post-layout: align-items (cross-axis / Y in row mode)
        // -----------------------------------------------------------------------

        private static void ApplyAlignItems(PDFLayoutBlock flexBlock, FlexAlignMode align)
        {
            int colCount = flexBlock.Columns.Length;
            if (colCount < 2) return;

            // Find the tallest first child block across all columns (content height, not region height).
            double maxH = 0;
            for (int i = 0; i < colCount; i++)
            {
                double h = FirstChildHeight(flexBlock.Columns[i]);
                if (h > maxH) maxH = h;
            }

            if (maxH <= 0) return;

            for (int i = 0; i < colCount; i++)
            {
                var    col      = flexBlock.Columns[i];
                double childH   = FirstChildHeight(col);
                double diff     = maxH - childH;
                if (diff <= 0.5) continue; // Already at max height.

                double yOffset = align switch
                {
                    FlexAlignMode.FlexEnd => diff,
                    FlexAlignMode.Center  => diff / 2.0,
                    _                     => 0
                };

                if (yOffset <= 0) continue;

                // Offset every child block in this column.
                foreach (var item in col.Contents)
                {
                    if (item is PDFLayoutBlock child)
                    {
                        var b = child.TotalBounds;
                        b.Y = b.Y + new Unit(yOffset, PageUnits.Points);
                        child.TotalBounds = b;
                    }
                }
            }
        }

        private static double FirstChildHeight(PDFLayoutRegion col)
        {
            foreach (var item in col.Contents)
            {
                if (item is PDFLayoutBlock b)
                    return b.TotalBounds.Height.PointsValue;
            }
            return 0;
        }

        private static double FirstChildWidth(PDFLayoutRegion col)
        {
            foreach (var item in col.Contents)
            {
                if (item is PDFLayoutBlock b)
                    return b.TotalBounds.Width.PointsValue;
            }
            return 0;
        }

        // -----------------------------------------------------------------------
        // Post-layout: justify-content (main-axis / X in row mode)
        // -----------------------------------------------------------------------

        private static void ApplyJustifyContent(PDFLayoutBlock flexBlock, FlexJustify justify)
        {
            int colCount = flexBlock.Columns.Length;
            if (colCount < 1) return;

            double containerW = flexBlock.TotalBounds.Width.PointsValue;
            double totalColW  = 0;
            for (int i = 0; i < colCount; i++)
                totalColW += flexBlock.Columns[i].TotalBounds.Width.PointsValue;

            // ShrinkToFit widens a single column to fill the block so the column width
            // equals the container width and leftover would be zero.  Use the first
            // child item's actual width to recover the true occupied space.
            if (colCount == 1)
            {
                double childW = FirstChildWidth(flexBlock.Columns[0]);
                if (childW > 0 && childW < totalColW)
                    totalColW = childW;
            }

            double leftover = containerW - totalColW;
            if (leftover < 1.0) return; // Items fill the container — nothing to distribute.

            double startOffset = 0;
            double gapBetween  = 0;

            switch (justify)
            {
                case FlexJustify.FlexEnd:
                    startOffset = leftover;
                    break;
                case FlexJustify.Center:
                    startOffset = leftover / 2.0;
                    break;
                case FlexJustify.SpaceBetween:
                    if (colCount > 1)
                        gapBetween = leftover / (colCount - 1);
                    else
                        startOffset = leftover / 2.0; // single item: centre
                    break;
                case FlexJustify.SpaceAround:
                    double aroundUnit = leftover / colCount;
                    startOffset = aroundUnit / 2.0;
                    gapBetween  = aroundUnit;
                    break;
                case FlexJustify.SpaceEvenly:
                    double evenUnit = leftover / (colCount + 1);
                    startOffset = evenUnit;
                    gapBetween  = evenUnit;
                    break;
            }

            double xOffset = startOffset;
            for (int i = 0; i < colCount; i++)
            {
                if (xOffset >= 0.5)
                {
                    var col    = flexBlock.Columns[i];
                    var bounds = col.TotalBounds;
                    bounds.X   = bounds.X + new Unit(xOffset, PageUnits.Points);
                    col.TotalBounds = bounds;
                }
                xOffset += gapBetween;
            }
        }

        // -----------------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------------

        // Reverses the column-width fractions array so that row-reverse renders
        // item N into column 1 (leftmost) and item 0 into column N (rightmost).
        private static ColumnWidths ReverseWidths(ColumnWidths widths)
        {
            double[] arr = widths.Widths;
            if (arr == null || arr.Length < 2) return widths;
            var rev = new double[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                rev[i] = arr[arr.Length - 1 - i];
            return new ColumnWidths(rev);
        }

        // For row-reverse the logical "start" is the right edge, so flex-start and
        // flex-end have opposite visual meanings compared to row.
        private static FlexJustify NormaliseJustify(FlexJustify justify, bool reverse)
        {
            if (!reverse) return justify;
            return justify switch
            {
                FlexJustify.FlexStart => FlexJustify.FlexEnd,
                FlexJustify.FlexEnd   => FlexJustify.FlexStart,
                _                     => justify
            };
        }

        private static bool IsFlexItem(IComponent child)
            => child is IContainerComponent && child is Component c && c.Visible;

        private int CountVisibleChildren()
        {
            var container = this.Component as IContainerComponent;
            if (container == null || !container.HasContent) return 0;
            int count = 0;
            foreach (var child in container.Content)
                if (IsFlexItem(child)) count++;
            return count;
        }

        /// <summary>
        /// Computes per-column width fractions for a row of <paramref name="count"/> items,
        /// optionally starting from visible-item index <paramref name="itemOffset"/>.
        /// When any item has flex-grow > 0 the fractions are proportional to grow values.
        /// When all items have flex-grow = 0 the fractions are derived from their explicit
        /// widths (width or flex-basis) if set, so that justify-content can redistribute
        /// any leftover space post-layout.
        /// </summary>
        private ColumnWidths ComputeColumnWidths(int count, double containerWidthPts, double alleyPts,
                                                  int itemOffset = 0)
        {
            var container = this.Component as IContainerComponent;
            if (container == null || !container.HasContent) return ColumnWidths.Empty;

            double[] grows       = new double[count];
            double[] fixedWidths = new double[count];
            double   totalGrow   = 0.0;
            bool     anyPositive = false;
            int      i           = 0;
            int      visIdx      = 0;

            foreach (var child in container.Content)
            {
                if (!IsFlexItem(child)) continue;
                if (visIdx < itemOffset) { visIdx++; continue; }
                if (i >= count) break;

                double grow = 1.0;
                if (child is IStyledComponent sc && sc.Style != null
                    && sc.Style.IsValueDefined(StyleKeys.FlexGrowKey))
                    grow = sc.Style.GetValue(StyleKeys.FlexGrowKey, 1.0);

                grows[i]   = grow;
                totalGrow += grow;
                if (grow > 0)
                {
                    anyPositive = true;
                }
                else if (child is IStyledComponent sc2 && sc2.Style != null)
                {
                    if (sc2.Style.IsValueDefined(StyleKeys.SizeWidthKey))
                        fixedWidths[i] = sc2.Style.Size.Width.PointsValue;
                    else if (sc2.Style.IsValueDefined(StyleKeys.FlexBasisKey) && !sc2.Style.Flex.BasisAuto)
                        fixedWidths[i] = sc2.Style.Flex.Basis.PointsValue;
                }
                visIdx++;
                i++;
            }

            double effectiveW = Math.Max(0, containerWidthPts - alleyPts * (count - 1));

            if (anyPositive && totalGrow > 0)
            {
                double fixedTotal = 0;
                for (int j = 0; j < count; j++)
                    if (grows[j] == 0) fixedTotal += fixedWidths[j];

                double remaining = Math.Max(0, effectiveW - fixedTotal);

                double growSum = 0;
                for (int j = 0; j < count; j++)
                    if (grows[j] > 0) growSum += grows[j];

                double[] pct = new double[count];
                for (int j = 0; j < count; j++)
                {
                    pct[j] = grows[j] == 0
                        ? (effectiveW > 0 ? fixedWidths[j] / effectiveW : 0)
                        : (growSum > 0 && effectiveW > 0 ? grows[j] / growSum * remaining / effectiveW : 0);
                }
                // Clamp: a fixed-width item wider than the container produces pct > 1.0,
                // which GetPercentColumnWidths rejects. Scale proportionally to fit.
                double totalPct = 0;
                for (int j = 0; j < count; j++) totalPct += pct[j];
                if (totalPct > 1.0)
                    for (int j = 0; j < count; j++) pct[j] /= totalPct;
                return new ColumnWidths(pct);
            }

            // All grow = 0: use explicit widths as fractions of effective width.
            if (effectiveW <= 0) return ColumnWidths.Empty;

            double[] fractions = new double[count];
            bool     anySet    = false;
            i      = 0;
            visIdx = 0;

            foreach (var child in container.Content)
            {
                if (!IsFlexItem(child)) continue;
                if (visIdx < itemOffset) { visIdx++; continue; }
                if (i >= count) break;

                if (child is IStyledComponent sc && sc.Style != null)
                {
                    Unit w = Unit.Zero;
                    if (sc.Style.IsValueDefined(StyleKeys.SizeWidthKey))
                        w = sc.Style.Size.Width;
                    else if (sc.Style.IsValueDefined(StyleKeys.FlexBasisKey) && !sc.Style.Flex.BasisAuto)
                        w = sc.Style.Flex.Basis;

                    if (w.PointsValue > 0)
                    {
                        fractions[i] = w.PointsValue / effectiveW;
                        anySet = true;
                    }
                }
                visIdx++;
                i++;
            }

            if (!anySet) return ColumnWidths.Empty;

            // Clamp: if items collectively exceed the container, scale fractions to 1.0 total.
            double totalFrac = 0;
            for (int j = 0; j < count; j++) totalFrac += fractions[j];
            if (totalFrac > 1.0)
                for (int j = 0; j < count; j++) fractions[j] /= totalFrac;

            return new ColumnWidths(fractions);
        }
    }
}
