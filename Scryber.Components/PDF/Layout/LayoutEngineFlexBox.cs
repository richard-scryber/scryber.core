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
        
        protected Style ContainerStyle { get; set; }

        public LayoutEngineFlexBox(ContainerComponent container, IPDFLayoutEngine parent, Style containerStyle)
            : base(container, parent)
        {
            ContainerStyle = containerStyle;
        }

        // Per-row ordered item list, built in DoLayoutBlockComponent and used by DoLayoutChildren.
        // Null = use source order. In wrap mode, set per-row in LayoutWrapRows.
        private List<Component> _orderedItems;

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
                // Build order-sorted item list once (used by DoLayoutChildren and ComputeWrapRows)
                _orderedItems = GetOrderedFlexItems();

                int childCount = _orderedItems.Count;
                if (childCount <= 0)
                {
                    _orderedItems = null;
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
                    _orderedItems = null;
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
                        var alignItems = flex.AlignItems;
                        var justify    = NormaliseJustify(flex.JustifyContent, reverse);

                        // Build per-column align values: each item's align-self overrides align-items.
                        var items = reverse ? ListReversed(_orderedItems) : _orderedItems;
                        var perColAlign = BuildPerColAlign(items, alignItems, 0, items.Count);

                        if (alignItems != FlexAlignMode.Stretch && alignItems != FlexAlignMode.FlexStart)
                            ApplyAlignItems(flexBlock, perColAlign);
                        else if (HasAlignSelfOverride(perColAlign, alignItems))
                            ApplyAlignItems(flexBlock, perColAlign);

                        if (justify != FlexJustify.FlexStart)
                            ApplyJustifyContent(flexBlock, justify);
                    }
                }

                _orderedItems = null;
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
            var rows   = ComputeWrapRows(containerW, colGap.PointsValue);
            double prevRowH = 0;
            bool   reverse  = (flex.Direction == FlexDirection.RowReverse);

            foreach (var (rowStart, rowEnd) in rows)
            {
                int rowItemCount = rowEnd - rowStart;
                if (rowItemCount <= 0) continue;

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
                        var rowItems     = _orderedItems ?? new List<Component>();
                        var sliceStart   = reverse ? (_orderedItems.Count - rowEnd) : rowStart;
                        var sliceEnd     = reverse ? (_orderedItems.Count - rowStart) : rowEnd;
                        var perColAlign  = BuildPerColAlign(rowItems, align, sliceStart, sliceEnd);

                        if (align != FlexAlignMode.Stretch && align != FlexAlignMode.FlexStart)
                            ApplyAlignItems(flexBlock, perColAlign);
                        else if (HasAlignSelfOverride(perColAlign, align))
                            ApplyAlignItems(flexBlock, perColAlign);

                        var rowJustify = NormaliseJustify(justify, reverse);
                        if (rowJustify != FlexJustify.FlexStart)
                            ApplyJustifyContent(flexBlock, rowJustify);
                    }
                }

                if (flexBlock != null)
                    prevRowH = flexBlock.TotalBounds.Height.PointsValue;
            }
            _wrapRowStart = -1;
            _wrapRowEnd   = -1;
        }

        /// <summary>
        /// Groups visible flex items into rows based on their fixed widths and the container width.
        /// Items with grow > 0 (minWidth = 0) never trigger a break on their own.
        /// Uses _orderedItems (already sorted by order property).
        /// </summary>
        private List<(int start, int end)> ComputeWrapRows(double containerW, double gapPts)
        {
            var rows = new List<(int start, int end)>();
            var items = _orderedItems;
            if (items == null || items.Count == 0) return rows;

            var minWidths = new List<double>(items.Count);
            foreach (var child in items)
                minWidths.Add(GetItemMinWidth(child));

            int    rowStart  = 0;
            double rowFixedW = minWidths[0];

            for (int i = 1; i < minWidths.Count; i++)
            {
                double itemW = minWidths[i];
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

                // column-reverse: render visible children in reverse source order.
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

            // Row mode: use _orderedItems (already sorted by 'order', then source order).
            // In wrap mode, only render items in [_wrapRowStart, _wrapRowEnd).
            var ordered = _orderedItems ?? new List<Component>();
            if (_reverseItems)
                ordered = ListReversed(ordered);

            bool first = true;
            for (int idx = 0; idx < ordered.Count; idx++)
            {
                if (_wrapRowStart >= 0 && (idx < _wrapRowStart || idx >= _wrapRowEnd))
                    continue;

                var comp = ordered[idx];

                if (!first)
                {
                    PDFLayoutBlock block = this.DocumentLayout.CurrentPage.LastOpenBlock();
                    PDFLayoutRegion reg  = block?.CurrentRegion;
                    if (block == null || reg == null) break;
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

        // -----------------------------------------------------------------------
        // Post-layout: align-items (cross-axis / Y in row mode)
        // -----------------------------------------------------------------------

        private static void ApplyAlignItems(PDFLayoutBlock flexBlock, FlexAlignMode[] perColAlign)
        {
            int colCount = flexBlock.Columns.Length;
            if (colCount < 1) return;

            // Find the tallest first child block across all columns.
            double maxH = 0;
            for (int i = 0; i < colCount; i++)
            {
                double h = FirstChildHeight(flexBlock.Columns[i]);
                if (h > maxH) maxH = h;
            }

            if (maxH <= 0) return;

            for (int i = 0; i < colCount; i++)
            {
                var align = (perColAlign != null && i < perColAlign.Length) ? perColAlign[i] : FlexAlignMode.FlexStart;
                if (align == FlexAlignMode.Stretch || align == FlexAlignMode.FlexStart)
                    continue;

                var    col    = flexBlock.Columns[i];
                double childH = FirstChildHeight(col);
                double diff   = maxH - childH;
                if (diff <= 0.5) continue;

                double yOffset = align switch
                {
                    FlexAlignMode.FlexEnd => diff,
                    FlexAlignMode.Center  => diff / 2.0,
                    _                     => 0
                };

                if (yOffset <= 0) continue;

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

        /// <summary>
        /// Builds per-column align values: each item's align-self overrides the container's align-items.
        /// </summary>
        private static FlexAlignMode[] BuildPerColAlign(List<Component> items, FlexAlignMode containerAlign,
            int start, int end)
        {
            int count = end - start;
            if (count <= 0) return Array.Empty<FlexAlignMode>();

            var result = new FlexAlignMode[count];
            for (int i = 0; i < count; i++)
            {
                var item = items[start + i];
                FlexAlignMode alignSelf = containerAlign;
                if (item is IStyledComponent sc && sc.Style != null
                    && sc.Style.IsValueDefined(StyleKeys.FlexAlignSelfKey))
                {
                    var self = sc.Style.GetValue(StyleKeys.FlexAlignSelfKey, FlexAlignMode.Auto);
                    if (self != FlexAlignMode.Auto)
                        alignSelf = self;
                }
                result[i] = alignSelf;
            }
            return result;
        }

        private static bool HasAlignSelfOverride(FlexAlignMode[] perColAlign, FlexAlignMode containerAlign)
        {
            if (perColAlign == null) return false;
            foreach (var a in perColAlign)
                if (a != containerAlign) return true;
            return false;
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

        /// <summary>
        /// Returns the visible flex items from the container's content, sorted by their
        /// 'order' CSS property (lower values first). Items with the same order value
        /// retain their source order (stable sort via LINQ).
        /// </summary>
        private List<Component> GetOrderedFlexItems()
        {
            var container = this.Component as IContainerComponent;
            if (container == null || !container.HasContent) return new List<Component>();

            var items = new List<(Component comp, int order, int srcIdx)>();
            int src   = 0;
            foreach (var child in container.Content)
            {
                if (!IsFlexItem(child)) { src++; continue; }
                var comp = (Component)child;
                int order = 0;
                if (comp is IStyledComponent sc && sc.Style != null
                    && sc.Style.IsValueDefined(StyleKeys.FlexOrderKey))
                    order = sc.Style.GetValue(StyleKeys.FlexOrderKey, 0);
                items.Add((comp, order, src));
                src++;
            }

            // Stable sort by order value
            items.Sort((a, b) => a.order != b.order ? a.order.CompareTo(b.order) : a.srcIdx.CompareTo(b.srcIdx));

            var result = new List<Component>(items.Count);
            foreach (var (comp, _, __) in items)
                result.Add(comp);
            return result;
        }

        private static List<Component> ListReversed(List<Component> source)
        {
            var rev = new List<Component>(source.Count);
            for (int i = source.Count - 1; i >= 0; i--)
                rev.Add(source[i]);
            return rev;
        }

        /// <summary>
        /// Computes per-column width fractions for a row of <paramref name="count"/> items.
        /// Uses <paramref name="itemOffset"/> to slice into the ordered item list for wrap rows.
        /// Handles flex-grow (positive free space) and flex-shrink (negative free space).
        /// </summary>
        private ColumnWidths ComputeColumnWidths(int count, double containerWidthPts, double alleyPts,
                                                  int itemOffset = 0)
        {
            var items = _orderedItems;
            if (items == null || items.Count == 0) return ColumnWidths.Empty;

            double[] grows       = new double[count];
            double[] shrinks     = new double[count];
            double[] fixedWidths = new double[count];
            double   totalGrow   = 0.0;
            bool     anyGrow     = false;

            for (int i = 0; i < count; i++)
            {
                int src = itemOffset + i;
                if (src >= items.Count) break;
                var child = items[src];

                double grow   = 1.0;
                double shrink = 1.0;
                double basis  = 0.0;

                if (child is IStyledComponent sc && sc.Style != null)
                {
                    if (sc.Style.IsValueDefined(StyleKeys.FlexGrowKey))
                        grow = sc.Style.GetValue(StyleKeys.FlexGrowKey, 1.0);
                    if (sc.Style.IsValueDefined(StyleKeys.FlexShrinkKey))
                        shrink = sc.Style.GetValue(StyleKeys.FlexShrinkKey, 1.0);

                    if (sc.Style.IsValueDefined(StyleKeys.SizeWidthKey))
                        basis = sc.Style.Size.Width.PointsValue;
                    else if (sc.Style.IsValueDefined(StyleKeys.FlexBasisKey) && !sc.Style.Flex.BasisAuto)
                        basis = sc.Style.Flex.Basis.PointsValue;
                }

                grows[i]       = grow;
                shrinks[i]     = shrink;
                fixedWidths[i] = basis;
                totalGrow     += grow;
                if (grow > 0) anyGrow = true;
            }

            double effectiveW = Math.Max(0, containerWidthPts - alleyPts * (count - 1));

            // --- Positive free space: grow ---
            if (anyGrow && totalGrow > 0)
            {
                double fixedTotal = 0;
                for (int j = 0; j < count; j++)
                    if (grows[j] == 0) fixedTotal += fixedWidths[j];

                double remaining = Math.Max(0, effectiveW - fixedTotal);
                double growSum   = 0;
                for (int j = 0; j < count; j++)
                    if (grows[j] > 0) growSum += grows[j];

                double[] pct = new double[count];
                for (int j = 0; j < count; j++)
                {
                    pct[j] = grows[j] == 0
                        ? (effectiveW > 0 ? fixedWidths[j] / effectiveW : 0)
                        : (growSum > 0 && effectiveW > 0 ? grows[j] / growSum * remaining / effectiveW : 0);
                }
                double totalPct = 0;
                for (int j = 0; j < count; j++) totalPct += pct[j];
                if (totalPct > 1.0)
                    for (int j = 0; j < count; j++) pct[j] /= totalPct;
                return new ColumnWidths(pct);
            }

            // --- All grow = 0: use explicit widths, apply shrink if items overflow ---
            if (effectiveW <= 0) return ColumnWidths.Empty;

            // If no explicit widths are set, we have nothing to work with.
            bool anyBasis = false;
            for (int j = 0; j < count; j++)
                if (fixedWidths[j] > 0) { anyBasis = true; break; }

            if (!anyBasis) return ColumnWidths.Empty;

            double totalBasis = 0;
            for (int j = 0; j < count; j++) totalBasis += fixedWidths[j];

            double[] finalPts = new double[count];

            if (totalBasis <= effectiveW + 0.5)
            {
                // Items fit — use their explicit widths as-is.
                for (int j = 0; j < count; j++)
                    finalPts[j] = fixedWidths[j];
            }
            else
            {
                // --- Negative free space: flex-shrink algorithm ---
                // Each item shrinks proportional to (shrink × basis) / Σ(shrink × basis).
                double overflow = totalBasis - effectiveW;
                double shrinkBasisSum = 0;
                for (int j = 0; j < count; j++)
                    shrinkBasisSum += shrinks[j] * fixedWidths[j];

                for (int j = 0; j < count; j++)
                {
                    double reduction = shrinkBasisSum > 0
                        ? (shrinks[j] * fixedWidths[j] / shrinkBasisSum) * overflow
                        : 0;
                    finalPts[j] = Math.Max(0, fixedWidths[j] - reduction);
                }
            }

            // Convert to fractions of effectiveW.
            double[] fractions = new double[count];
            double   totalF    = 0;
            for (int j = 0; j < count; j++) { fractions[j] = finalPts[j] / effectiveW; totalF += fractions[j]; }
            if (totalF > 1.0)
                for (int j = 0; j < count; j++) fractions[j] /= totalF;

            return new ColumnWidths(fractions);
        }
    }
}
