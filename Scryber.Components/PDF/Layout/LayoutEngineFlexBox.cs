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

        // Absolute content widths (pts, excluding margins) per flex item for the current row.
        // Set by ComputeColumnWidths so DoLayoutAChild can override percentage widths with the
        // resolved absolute value — the flex algorithm already resolved % against the container,
        // so re-applying % against the column's available width would shrink the item further.
        private Dictionary<Component, double> _flexItemContentWidths;

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

                bool colReverse  = (direction == FlexDirection.ColumnReverse);
                var  colJustify  = NormaliseJustify(flex.JustifyContent, colReverse);
                var  colAlign    = flex.AlignItems;
                bool needsAlign  = colAlign != FlexAlignMode.FlexStart && colAlign != FlexAlignMode.Stretch;

                var parentBlock  = this.DocumentLayout.CurrentPage.LastOpenBlock();
                var parentRegion = parentBlock?.CurrentRegion;
                int priorCount   = parentRegion?.Contents.Count ?? 0;

                _isRowMode     = false;
                _reverseItems  = colReverse;
                base.DoLayoutBlockComponent(position, columnOptions);
                _isRowMode    = false;
                _reverseItems = false;

                if (colJustify != FlexJustify.FlexStart || needsAlign)
                {
                    PDFLayoutBlock flexBlock = null;
                    if (parentRegion != null && parentRegion.Contents.Count > priorCount)
                        flexBlock = parentRegion.Contents[parentRegion.Contents.Count - 1] as PDFLayoutBlock;
                    if (flexBlock == null)
                    {
                        var postParent = this.DocumentLayout.CurrentPage.LastOpenBlock();
                        var postRegion = postParent?.CurrentRegion;
                        if (postRegion != null && postRegion.Contents.Count > 0)
                            flexBlock = postRegion.Contents[postRegion.Contents.Count - 1] as PDFLayoutBlock;
                    }
                    if (flexBlock != null)
                    {
                        if (colJustify != FlexJustify.FlexStart)
                            ApplyJustifyContentColumn(flexBlock, colJustify);
                        if (needsAlign)
                            ApplyAlignItemsColumnAllPages(flexBlock, colAlign);
                    }
                }
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
                    // Use the current column region's width, not AvailableBounds.Width — the latter
                    // is the full block width (before column splitting), which is wrong when the
                    // flex container sits inside a multi-column parent (e.g. column-count: 2).
                    var openBlock = this.DocumentLayout.CurrentPage.LastOpenBlock();
                    containerW = openBlock?.CurrentRegion?.TotalBounds.Width.PointsValue
                              ?? openBlock?.AvailableBounds.Width.PointsValue
                              ?? 0;
                    if (!position.Padding.IsEmpty)
                        containerW -= (position.Padding.Left + position.Padding.Right).PointsValue;
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

                // Explicit or minimum container height → cross-axis reference for flex-end / center.
                // With no explicit height, min-height is the closest thing to a "declared" box -
                // without this, align-items/align-content would have no reference to align
                // against on a min-height-only row container. Note this only affects the
                // alignment reference, not the block's own rendered size - a min-height-only
                // flex row container doesn't yet grow its visible box either (see follow-up task).
                // Uses the same padding-box convention as containerW above.
                double? containerH = null;
                if (position.Height.HasValue || position.MinimumHeight.HasValue)
                {
                    containerH = position.Height.HasValue ? position.Height.Value.PointsValue : 0;
                    if (position.MinimumHeight.HasValue && position.MinimumHeight.Value.PointsValue > containerH)
                        containerH = position.MinimumHeight.Value.PointsValue;
                    if (!position.Padding.IsEmpty)
                        containerH -= (position.Padding.Top + position.Padding.Bottom).PointsValue;
                }

                _flexItemContentWidths = null; // clear before computing for this row
                var widths = ComputeColumnWidths(childCount, containerW, colGap.PointsValue);
                if (reverse) widths = ReverseWidths(widths);

                var rowCols = new PDFColumnOptions()
                {
                    ColumnCount  = childCount,
                    AlleyWidth   = colGap,
                    ColumnWidths = widths,
                    AutoFlow     = false
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
                _flexItemContentWidths = null;

                // Post-layout: apply align-items and justify-content.
                // If page-break-inside:avoid moved the container to a new page, parentRegion is
                // stale; fall back to re-querying the current page to find the block.
                PDFLayoutBlock flexBlock = null;
                if (parentRegion != null && parentRegion.Contents.Count > priorCount)
                    flexBlock = parentRegion.Contents[parentRegion.Contents.Count - 1] as PDFLayoutBlock;
                if (flexBlock == null)
                {
                    var postParent = this.DocumentLayout.CurrentPage.LastOpenBlock();
                    var postRegion = postParent?.CurrentRegion;
                    if (postRegion != null && postRegion.Contents.Count > 0)
                        flexBlock = postRegion.Contents[postRegion.Contents.Count - 1] as PDFLayoutBlock;
                }
                if (flexBlock != null && flexBlock.Columns.Length > 0)
                {
                    var alignItems = flex.AlignItems;
                    var justify    = NormaliseJustify(flex.JustifyContent, reverse);

                    // Build per-column align values: each item's align-self overrides align-items.
                    var items = reverse ? ListReversed(_orderedItems) : _orderedItems;
                    var perColAlign = BuildPerColAlign(items, alignItems, 0, items.Count);

                    if (alignItems != FlexAlignMode.Stretch && alignItems != FlexAlignMode.FlexStart)
                        ApplyAlignItems(flexBlock, perColAlign, containerH);
                    else if (HasAlignSelfOverride(perColAlign, alignItems))
                        ApplyAlignItems(flexBlock, perColAlign, containerH);

                    if (justify != FlexJustify.FlexStart)
                        ApplyJustifyContent(flexBlock, justify, containerW);
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
            bool   reverse  = (flex.Direction == FlexDirection.RowReverse);

            for (int rowIdx = 0; rowIdx < rows.Count; rowIdx++)
            {
                var (rowStart, rowEnd) = rows[rowIdx];
                int rowItemCount = rowEnd - rowStart;
                if (rowItemCount <= 0) continue;

                _wrapRowStart = rowStart;
                _wrapRowEnd   = rowEnd;

                _flexItemContentWidths = null; // clear per-row before recomputing
                var widths  = ComputeColumnWidths(rowItemCount, containerW, colGap.PointsValue, rowStart);
                if (reverse) widths = ReverseWidths(widths);

                var rowCols = new PDFColumnOptions()
                {
                    ColumnCount  = rowItemCount,
                    AlleyWidth   = colGap,
                    ColumnWidths = widths,
                    AutoFlow     = false
                };

                // The flex container is a single visual element: padding and margins apply
                // once at the outer edges, not between rows. Strip top padding/margin from
                // every row except the first, and bottom padding/margin from every row except
                // the last. The border is handled the same way on the layout blocks below.
                var rowPosition = position.Clone();
                bool isFirst = rowIdx == 0;
                bool isLast  = rowIdx == rows.Count - 1;
                if (!isFirst || !isLast)
                {
                    var pad = rowPosition.Padding;
                    if (!isFirst) pad.Top    = Unit.Zero;
                    if (!isLast)  pad.Bottom = Unit.Zero;
                    rowPosition.Padding = pad;

                    var mar = rowPosition.Margins;
                    if (!isFirst) mar.Top    = Unit.Zero;
                    if (!isLast)  mar.Bottom = Unit.Zero;
                    rowPosition.Margins = mar;
                }

                var parentBlock  = this.DocumentLayout.CurrentPage.LastOpenBlock();
                var parentRegion = parentBlock?.CurrentRegion;
                int priorCount   = parentRegion?.Contents.Count ?? 0;

                // Force OverflowSplit.Never so each wrap-row block is atomic: if any item's
                // content overflows the page the whole row moves to the next page rather than
                // items in the same row ending up on different pages.
                var prevSplit = this.FullStyle.GetValue(StyleKeys.OverflowSplitKey, OverflowSplit.Any);
                this.FullStyle.SetValue(StyleKeys.OverflowSplitKey, OverflowSplit.Never);

                _isRowMode    = true;
                _reverseItems = reverse;
                base.DoLayoutBlockComponent(rowPosition, rowCols);
                _isRowMode    = false;
                _reverseItems = false;
                _flexItemContentWidths = null;

                this.FullStyle.SetValue(StyleKeys.OverflowSplitKey, prevSplit);

                PDFLayoutBlock flexBlock = null;
                if (parentRegion != null && parentRegion.Contents.Count > priorCount)
                {
                    flexBlock = parentRegion.Contents[parentRegion.Contents.Count - 1] as PDFLayoutBlock;
                }
                // If OverflowSplit.Never moved the row to a new page, parentRegion no longer
                // contains the block. Re-query the current page to find where it landed.
                if (flexBlock == null)
                {
                    var postParent = this.DocumentLayout.CurrentPage.LastOpenBlock();
                    var postRegion = postParent?.CurrentRegion;
                    if (postRegion != null && postRegion.Contents.Count > 0)
                        flexBlock = postRegion.Contents[postRegion.Contents.Count - 1] as PDFLayoutBlock;
                }

                if (flexBlock != null)
                {
                    // For multi-row layouts the container border should appear as one box,
                    // not one box per row. Suppress the interior top/bottom border sides on
                    // non-edge rows so they don't double up.
                    //
                    // CreateBorderPen() builds side pens from BorderTopStyleKey etc., NOT from
                    // BorderSidesKey. To suppress a side we set its side-specific key to
                    // LineType.None with int.MaxValue priority so it wins over the base
                    // BorderStyleKey value regardless of its CSS specificity.
                    if (flexBlock != null && (!isFirst || !isLast)
                        && flexBlock.FullStyle.IsValueDefined(StyleKeys.BorderStyleKey))
                    {
                        // Clone at priority 0 so the suppress values can override.
                        var rowStyle = new Style();
                        flexBlock.FullStyle.MergeInto(rowStyle, 0);

                        var suppress = new Style();
                        if (!isFirst) suppress.SetValue(StyleKeys.BorderTopStyleKey,    LineType.None);
                        if (!isLast)  suppress.SetValue(StyleKeys.BorderBottomStyleKey, LineType.None);
                        suppress.MergeInto(rowStyle, int.MaxValue);

                        flexBlock.FullStyle = rowStyle;
                    }

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
                            ApplyJustifyContent(flexBlock, rowJustify, containerW);
                    }
                }

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
                minWidths.Add(GetItemMinWidth(child, containerW));

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
        /// Returns the fixed minimum width for a flex item (from explicit width or flex-basis),
        /// resolving percentage values against <paramref name="containerWidthPts"/>.
        /// Returns 0 for grow-only items.
        /// </summary>
        private double GetItemMinWidth(Component item, double containerWidthPts)
        {
            // Use the full applied style (includes CSS class rules) so that items that
            // receive their width via a class selector are recognised as having a fixed
            // width for wrap-row breaking — not just items with inline/direct styles.
            var applied = item.GetAppliedStyle();
            if (applied == null)
                return 0;

            // Push applied onto the stack before building the full style, exactly like the
            // base engine's own per-child layout does - this is what makes BuildFullStyle
            // resolve em/ex/rem against this item's real cascaded font (and lets any nested
            // lookups see the right inherited styles) instead of guessing.
            this.StyleStack.Push(applied);
            try
            {
                var fullStyle = this.BuildFullStyle(item);

                if (applied.IsValueDefined(StyleKeys.SizeWidthKey))
                    return ResolveWidthLikeValue(applied.Size.Width, fullStyle?.Size.Width ?? applied.Size.Width, containerWidthPts);
                if (applied.IsValueDefined(StyleKeys.FlexBasisKey) && !applied.Flex.BasisAuto)
                    return ResolveWidthLikeValue(applied.Flex.Basis, fullStyle?.Flex.Basis ?? applied.Flex.Basis, containerWidthPts);
                return 0;
            }
            finally
            {
                this.StyleStack.Pop();
            }
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
                    // Advance to the next flex column.  Close the current column first
                    // (mirroring PushBlockStackOntoNewRegion), then force-advance.
                    // AutoFlow is false on flex columns to prevent content-overflow from
                    // auto-advancing and creating ghost blocks; force=true lets the
                    // explicit inter-item advance bypass that restriction.
                    var flexBlock = this.CurrentBlock;
                    if (flexBlock == null || flexBlock.IsClosed) break;

                    var prevRegion = flexBlock.CurrentRegion;
                    if (prevRegion != null && !prevRegion.IsClosed)
                        prevRegion.Close();

                    if (!flexBlock.MoveToNextRegion(force: true, Unit.Zero, this.Context))
                        break;
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

        private static void ApplyAlignItems(PDFLayoutBlock flexBlock, FlexAlignMode[] perColAlign,
            double? containerH = null)
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

            // When the container has an explicit height, use it as the cross-axis reference
            // so flex-end / center items align to the container bottom, not just the tallest item.
            double refH = (containerH.HasValue && containerH.Value > maxH) ? containerH.Value : maxH;

            for (int i = 0; i < colCount; i++)
            {
                var align = (perColAlign != null && i < perColAlign.Length) ? perColAlign[i] : FlexAlignMode.FlexStart;
                if (align == FlexAlignMode.Stretch || align == FlexAlignMode.FlexStart)
                    continue;

                var    col    = flexBlock.Columns[i];
                double childH = FirstChildHeight(col);
                double diff   = refH - childH;
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
                var applied = item.GetAppliedStyle();
                if (applied != null && applied.IsValueDefined(StyleKeys.FlexAlignSelfKey))
                {
                    var self = applied.GetValue(StyleKeys.FlexAlignSelfKey, FlexAlignMode.Auto);
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
        // Post-layout: justify-content (main-axis / Y in column mode)
        // -----------------------------------------------------------------------

        /// <summary>
        /// Applies justify-content spacing along the main axis (Y) for flex-direction:column.
        /// The column TotalBounds.Height is the explicit container content height set by ShrinkToFit;
        /// items are repositioned within that space according to <paramref name="justify"/>.
        /// </summary>
        private static void ApplyJustifyContentColumn(PDFLayoutBlock flexBlock, FlexJustify justify)
        {
            if (flexBlock.Columns.Length < 1) return;
            var col = flexBlock.Columns[0];

            // After ShrinkToFit, column height = explicit container content height (padding already excluded).
            double containerH = col.TotalBounds.Height.PointsValue;

            var    items  = new List<PDFLayoutBlock>();
            double totalH = 0;
            foreach (var item in col.Contents)
            {
                if (item is PDFLayoutBlock b)
                {
                    items.Add(b);
                    totalH += b.TotalBounds.Height.PointsValue;
                }
            }

            if (items.Count == 0) return;

            double leftover = containerH - totalH;
            if (leftover < 1.0) return;

            double startOffset = 0;
            double gapBetween  = 0;
            int    count       = items.Count;

            switch (justify)
            {
                case FlexJustify.FlexEnd:
                    startOffset = leftover;
                    break;
                case FlexJustify.Center:
                    startOffset = leftover / 2.0;
                    break;
                case FlexJustify.SpaceBetween:
                    if (count > 1) gapBetween = leftover / (count - 1);
                    else           startOffset = leftover / 2.0;
                    break;
                case FlexJustify.SpaceAround:
                    double aroundUnit = leftover / count;
                    startOffset = aroundUnit / 2.0;
                    gapBetween  = aroundUnit;
                    break;
                case FlexJustify.SpaceEvenly:
                    double evenUnit = leftover / (count + 1);
                    startOffset = evenUnit;
                    gapBetween  = evenUnit;
                    break;
            }

            double yOffset = startOffset;
            for (int i = 0; i < items.Count; i++)
            {
                if (yOffset >= 0.5)
                {
                    var b = items[i].TotalBounds;
                    b.Y += new Unit(yOffset, PageUnits.Points);
                    items[i].TotalBounds = b;
                }
                yOffset += gapBetween; // item heights already embedded in each b.Y — only accumulate the gap
            }
        }

        /// <summary>
        /// Applies align-items cross-axis (X) alignment for flex-direction:column.
        /// Shifts items horizontally within the container based on <paramref name="containerAlign"/>.
        /// Stretch and FlexStart are no-ops (items already fill or are left-aligned by default).
        /// </summary>
        private static void ApplyAlignItemsColumn(PDFLayoutBlock flexBlock, FlexAlignMode containerAlign)
        {
            if (flexBlock.Columns.Length < 1) return;
            var col = flexBlock.Columns[0];

            double containerW = col.TotalBounds.Width.PointsValue;
            if (containerW <= 0) return;

            foreach (var item in col.Contents)
            {
                if (!(item is PDFLayoutBlock b)) continue;

                double itemW = b.TotalBounds.Width.PointsValue;
                double diff  = containerW - itemW;
                if (diff <= 0.5) continue;

                double xOffset = containerAlign switch
                {
                    FlexAlignMode.FlexEnd => diff,
                    FlexAlignMode.Center  => diff / 2.0,
                    _                     => 0
                };

                if (xOffset <= 0) continue;

                var tb = b.TotalBounds;
                tb.X += new Unit(xOffset, PageUnits.Points);
                b.TotalBounds = tb;
            }
        }

        /// <summary>
        /// Applies ApplyAlignItemsColumn to every layout block for the same flex-container
        /// component, covering both same-page multi-column overflow and multi-page overflow.
        ///
        /// Phase 1 — same-page column overflow:
        ///   Navigate directly to the parent block and scan its columns for flex blocks.
        ///   O(parent_columns × items_per_column) — no page traversal.
        ///
        /// Phase 2 — page overflow:
        ///   Search only pages [startPage+1 … currentPage] (the range the layout engine
        ///   actually used for this container), not the entire document.
        /// </summary>
        private void ApplyAlignItemsColumnAllPages(PDFLayoutBlock firstFlexBlock, FlexAlignMode containerAlign)
        {
            var flexOwner   = (IComponent)firstFlexBlock.Owner;
            var parentBlock = GetParentBlock(firstFlexBlock);

            // Phase 1 — scan all columns of the direct parent (same page, multi-column parent).
            // This also covers the primary flex block in col[0], so ApplyAlignItemsColumn is
            // called exactly once per block.
            if (parentBlock != null)
            {
                foreach (var col in parentBlock.Columns)
                {
                    if (col == null) continue;
                    foreach (var item in col.Contents)
                    {
                        if (item is PDFLayoutBlock b && b.Owner == flexOwner)
                            ApplyAlignItemsColumn(b, containerAlign);
                    }
                }
            }
            else
            {
                // No navigable parent — apply to the block we know about.
                ApplyAlignItemsColumn(firstFlexBlock, containerAlign);
            }

            // Phase 2 — page overflow: only search pages actually used by this container.
            var startPage  = firstFlexBlock.GetLayoutPage();
            if (startPage == null) return;

            var endPageIdx = this.DocumentLayout.CurrentPage.PageIndex;
            if (startPage.PageIndex >= endPageIdx) return; // no page overflow occurred

            var allPages = this.DocumentLayout.AllPages;
            for (int i = startPage.PageIndex + 1; i <= endPageIdx; i++)
            {
                var found = new List<PDFLayoutBlock>();
                CollectBlocksByOwner(allPages[i].ContentBlock, flexOwner, found);
                foreach (var b in found)
                    ApplyAlignItemsColumn(b, containerAlign);
            }
        }

        /// <summary>
        /// Returns the PDFLayoutBlock that is the direct parent of <paramref name="block"/>.
        /// A block's Parent pointer is the enclosing block (not the intermediate region that
        /// physically holds it in its Contents list).
        /// </summary>
        private static PDFLayoutBlock GetParentBlock(PDFLayoutBlock block)
        {
            return block.Parent as PDFLayoutBlock;
        }

        /// <summary>
        /// Depth-first collector: adds every PDFLayoutBlock whose Owner matches
        /// <paramref name="owner"/> to <paramref name="found"/>.
        /// Stops recursing into a matched block (its interior belongs to the container itself).
        /// </summary>
        private static void CollectBlocksByOwner(PDFLayoutBlock root, IComponent owner,
            List<PDFLayoutBlock> found)
        {
            if (root == null) return;
            if (root.Owner == owner)
            {
                found.Add(root);
                return;
            }

            foreach (var col in root.Columns)
            {
                if (col == null) continue;
                foreach (var item in col.Contents)
                {
                    if (item is PDFLayoutBlock child)
                        CollectBlocksByOwner(child, owner, found);
                }
            }
        }

        // -----------------------------------------------------------------------
        // Post-layout: justify-content (main-axis / X in row mode)
        // -----------------------------------------------------------------------

        private static void ApplyJustifyContent(PDFLayoutBlock flexBlock, FlexJustify justify, double contentW)
        {
            int colCount = flexBlock.Columns.Length;
            if (colCount < 1) return;

            // contentW is the flex container's content-area width (padding already excluded).
            // TotalBounds.Width includes padding, which would make leftover too large and
            // push the rightmost item outside the container by the padding amount.
            double containerW = contentW > 0 ? contentW : flexBlock.TotalBounds.Width.PointsValue;
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
        {
            if (!(child is Component c) || !c.Visible)
                return false;

            //Whitespace-only text nodes are a normal by-product of formatted HTML source (the
            //newline/indentation between tags becomes its own text child) and, per CSS's
            //anonymous-box rules, never generate a box of their own - counting them as real flex
            //items would eat a column/row slot for indentation whitespace alone. Checked via
            //ITextLiteral (not just TextLiteral) since the two parsers represent this
            //differently: the lenient HTML parser produces a TextLiteral even for pure
            //whitespace, while the strict XML parser produces a dedicated Whitespace component -
            //both implement ITextLiteral's Text property. Any text with actual (non-whitespace)
            //content still becomes a real, wrapped flex item (see EnsureFlexItemContainer) - e.g.
            //bare text mixed with elements.
            if (c is Scryber.ITextLiteral tl && string.IsNullOrWhiteSpace(tl.Text))
                return false;

            return true;
        }

        /// <summary>
        /// Ensures a flex item has a full block-layout-capable box to sit in. Checked
        /// specifically against Panel, not the broader IContainerComponent - a TableCell or
        /// FormInputField is a container too, but isn't necessarily a fully-fledged general
        /// flex-item box (and might bypass flex-item-specific settings if just accepted as-is).
        /// A non-Panel child - most commonly a raw text node sitting directly in a flex
        /// container, e.g. &lt;div style="display:flex"&gt;Some text&lt;span&gt;...&lt;/span&gt;&lt;/div&gt;
        /// - has no block-layout engine of its own at all (TextBase.GetEngine always returns
        /// LayoutEngineText, regardless of what display mode gets forced onto its style in
        /// DoLayoutAChild), so it could never work as a flex item directly.
        ///
        /// The wrapper is InvisibleFlexContainer (Panel + IInvisibleContainer), not a plain
        /// Panel - it still gets a real layout box (Panel already implements
        /// IPDFViewPortComponent, which LayoutEngineBase's child dispatch checks before
        /// IInvisibleContainer, so DoLayoutViewPortComponent runs and IInvisibleContainer's own
        /// "don't create a box" layout behaviour never triggers here) but is transparent to
        /// structural CSS matching (:nth-child/:nth-of-type via Component.PopulateSiblingPosition,
        /// and any future `>` awareness) since those consult IInvisibleContainer directly,
        /// independent of layout dispatch. Reparenting the child (this.Contents.Add(child) does
        /// set child.Parent = wrapper) is real and unavoidable - the transparency comes from
        /// downstream code recognising IInvisibleContainer, not from avoiding the reparent.
        /// </summary>
        private Component EnsureFlexItemContainer(Component child)
        {
            if (child is Panel)
                return child;

            var wrapper = new InvisibleFlexContainer();
            wrapper.Parent = this.Component as Component;
            wrapper.Contents.Add(child);
            return wrapper;
        }

        /// <summary>
        /// Returns the visible flex items from the container's content, sorted by their
        /// 'order' CSS property (lower values first). Items with the same order value
        /// retain their source order (stable sort via LINQ). Non-Panel children (see
        /// EnsureFlexItemContainer) come back wrapped - 'order' is read from the original
        /// child's own applied style first, since the synthetic wrapper has none of its own.
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
                var applied = comp.GetAppliedStyle();
                if (applied != null && applied.IsValueDefined(StyleKeys.FlexOrderKey))
                    order = applied.GetValue(StyleKeys.FlexOrderKey, 0);

                comp = EnsureFlexItemContainer(comp);
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

        /// <summary>
        /// Resolves a width/basis/margin value to points. % and viewport units are resolved by
        /// hand against the flex container's own width, because BuildFullStyle's own container
        /// lookup can't see the flex container yet at this point in layout (its region isn't
        /// open) and would fall back to the outer page block instead. Every other unit
        /// (em/ex/rem, absolute) trusts <paramref name="flattened"/> - BuildFullStyle's own
        /// output - which already resolved it correctly against the item's real cascaded font,
        /// provided the item's applied style was pushed onto the StyleStack before it ran.
        /// </summary>
        private static double ResolveWidthLikeValue(Unit raw, Unit flattened, double containerWidthPts)
        {
            switch (raw.Units)
            {
                case PageUnits.Percent:
                case PageUnits.ViewPortWidth:
                case PageUnits.ViewPortHeight:
                case PageUnits.ViewPortMin:
                case PageUnits.ViewPortMax:
                    return raw.ToAbsolute(new Unit(containerWidthPts, PageUnits.Points)).PointsValue;
                default:
                    return flattened.PointsValue;
            }
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

            double[] grows        = new double[count];
            double[] shrinks      = new double[count];
            double[] fixedWidths  = new double[count]; // content basis (explicit width), no margins
            double[] marginTotals = new double[count]; // left + right margin per item
            double   totalGrow    = 0.0;
            bool     anyGrow      = false;

            for (int i = 0; i < count; i++)
            {
                int src = itemOffset + i;
                if (src >= items.Count) break;
                var child = items[src];

                // CSS default: flex-grow is 0 (not 1)
                double grow   = 0.0;
                double shrink = 1.0;
                double basis  = 0.0;

                // Resolve the full computed style (CSS classes + inherited + percentages flattened).
                var applied = child.GetAppliedStyle();
                if (applied != null)
                    this.StyleStack.Push(applied);

                var fullStyle = this.BuildFullStyle(child);

                if (fullStyle != null)
                {
                    if (fullStyle.IsValueDefined(StyleKeys.FlexGrowKey))
                        grow = fullStyle.GetValue(StyleKeys.FlexGrowKey, 0.0);
                    if (fullStyle.IsValueDefined(StyleKeys.FlexShrinkKey))
                        shrink = fullStyle.GetValue(StyleKeys.FlexShrinkKey, 1.0);
                }

                // Width/basis: % (and viewport units) must resolve against the flex container's
                // own width, not the page block width BuildFullStyle's own parent-size lookup
                // would use at this point in the pipeline (the flex container's region isn't
                // open yet) - so those come from the raw, pre-flatten 'applied' value resolved
                // by hand. Everything else (em/ex/rem, absolute) is read straight from fullStyle,
                // which - because 'applied' was pushed onto the StyleStack before BuildFullStyle
                // ran above - already correctly resolved it against this item's real cascaded font.
                if (applied != null && applied.IsValueDefined(StyleKeys.SizeWidthKey))
                    basis = ResolveWidthLikeValue(applied.Size.Width, fullStyle?.Size.Width ?? applied.Size.Width, containerWidthPts);
                else if (applied != null && applied.IsValueDefined(StyleKeys.FlexBasisKey) && !applied.Flex.BasisAuto)
                    basis = ResolveWidthLikeValue(applied.Flex.Basis, fullStyle?.Flex.Basis ?? applied.Flex.Basis, containerWidthPts);
                else if (fullStyle != null)
                {
                    if (fullStyle.IsValueDefined(StyleKeys.SizeWidthKey))
                        basis = fullStyle.Size.Width.PointsValue;
                    else if (fullStyle.IsValueDefined(StyleKeys.FlexBasisKey) && !fullStyle.Flex.BasisAuto)
                        basis = fullStyle.Flex.Basis.PointsValue;
                }

                // Margins: same treatment — % against the flex container, everything else from fullStyle.
                {
                    double mLeft  = 0;
                    double mRight = 0;
                    if (applied != null && applied.IsValueDefined(StyleKeys.MarginsLeftKey))
                        mLeft = ResolveWidthLikeValue(applied.Margins.Left, fullStyle?.Margins.Left ?? applied.Margins.Left, containerWidthPts);
                    else if (fullStyle != null && fullStyle.IsValueDefined(StyleKeys.MarginsLeftKey))
                        mLeft = fullStyle.Margins.Left.PointsValue;

                    if (applied != null && applied.IsValueDefined(StyleKeys.MarginsRightKey))
                        mRight = ResolveWidthLikeValue(applied.Margins.Right, fullStyle?.Margins.Right ?? applied.Margins.Right, containerWidthPts);
                    else if (fullStyle != null && fullStyle.IsValueDefined(StyleKeys.MarginsRightKey))
                        mRight = fullStyle.Margins.Right.PointsValue;

                    marginTotals[i] = mLeft + mRight;
                }

                if (applied != null)
                    this.StyleStack.Pop();

                grows[i]      = grow;
                shrinks[i]    = shrink;
                fixedWidths[i] = basis;
                totalGrow    += grow;
                if (grow > 0) anyGrow = true;
            }

            double effectiveW = Math.Max(0, containerWidthPts - alleyPts * (count - 1));

            // --- Positive free space: grow ---
            if (anyGrow && totalGrow > 0)
            {
                // Fixed space = explicit widths + margins of grow=0 items.
                // Growing items still consume their own margins from the free space pool.
                double fixedTotal     = 0;
                double growMarginTotal = 0;
                for (int j = 0; j < count; j++)
                {
                    if (grows[j] == 0)
                        fixedTotal += fixedWidths[j] + marginTotals[j];
                    else
                        growMarginTotal += marginTotals[j];
                }

                double remaining = Math.Max(0, effectiveW - fixedTotal - growMarginTotal);
                double growSum   = 0;
                for (int j = 0; j < count; j++)
                    if (grows[j] > 0) growSum += grows[j];

                // Column width = content portion + margin so the column region already includes margin.
                double[] pct = new double[count];
                for (int j = 0; j < count; j++)
                {
                    double colW = grows[j] == 0
                        ? fixedWidths[j] + marginTotals[j]
                        : (growSum > 0 ? grows[j] / growSum * remaining : 0) + marginTotals[j];
                    pct[j] = effectiveW > 0 ? colW / effectiveW : 0;

                    // Store the resolved content width so DoLayoutAChild can override % widths.
                    StoreFlexItemContentWidth(items, itemOffset + j, colW - marginTotals[j]);
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

            // Total space each item occupies = content basis + its margins.
            double totalBasis = 0;
            for (int j = 0; j < count; j++) totalBasis += fixedWidths[j] + marginTotals[j];

            double[] finalPts = new double[count];

            if (totalBasis <= effectiveW + 0.5)
            {
                // Items fit — column = basis + margin.
                for (int j = 0; j < count; j++)
                {
                    finalPts[j] = fixedWidths[j] + marginTotals[j];
                    StoreFlexItemContentWidth(items, itemOffset + j, fixedWidths[j]);
                }
            }
            else
            {
                // --- Negative free space: flex-shrink algorithm ---
                // Shrink is applied to the content (basis) only, not to margins.
                double overflow = totalBasis - effectiveW;
                double shrinkBasisSum = 0;
                for (int j = 0; j < count; j++)
                    shrinkBasisSum += shrinks[j] * fixedWidths[j];

                for (int j = 0; j < count; j++)
                {
                    double reduction = shrinkBasisSum > 0
                        ? (shrinks[j] * fixedWidths[j] / shrinkBasisSum) * overflow
                        : 0;
                    finalPts[j] = Math.Max(marginTotals[j], fixedWidths[j] + marginTotals[j] - reduction);
                    StoreFlexItemContentWidth(items, itemOffset + j, finalPts[j] - marginTotals[j]);
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

        private void StoreFlexItemContentWidth(List<Component> items, int idx, double contentWidthPts)
        {
            if (idx < 0 || idx >= items.Count || contentWidthPts <= 0) return;
            if (_flexItemContentWidths == null)
                _flexItemContentWidths = new Dictionary<Component, double>(items.Count);
            _flexItemContentWidths[items[idx]] = contentWidthPts;
        }

        protected override void DoLayoutAChild(IComponent comp, Style full)
        {

            //Flex items are always blockified per spec - a child's own inline/inline-block
            //display (e.g. a <label> or <input>'s UA default) must not let it join the normal
            //inline flow with its siblings; each flex item is its own block-level box regardless
            //of what display value it was given. Row mode already sidesteps this via the
            //column-break-per-item trick in DoLayoutChildren, but column mode has no equivalent,
            //so inline/inline-block children just ran together on one shared line.
            if (full.Position.DisplayMode == DisplayMode.Inline || full.Position.DisplayMode == DisplayMode.InlineBlock)
            {
                full.Position.DisplayMode = DisplayMode.Block;
            }

            //Cross-axis stretch (align-items: stretch, the default, unless overridden by this
            //item's own align-self) only has something to act on when the cross axis is width -
            //column-direction containers. There's no FullHeight equivalent for row mode's cross
            //axis, so that side stays a no-op as before. Applies to any item type, not just Panel
            //descendants - a blockified <label>/<input> is just as much a flex item as a <div>.
            if (!_isRowMode && comp is Component itemComp)
            {
                var itemStyle = itemComp.GetAppliedStyle();
                var alignSelf = this.FullStyle.Flex.AlignItems;
                if (itemStyle != null && itemStyle.IsValueDefined(StyleKeys.FlexAlignSelfKey))
                {
                    var self = itemStyle.GetValue(StyleKeys.FlexAlignSelfKey, FlexAlignMode.Auto);
                    if (self != FlexAlignMode.Auto)
                        alignSelf = self;
                }

                if (alignSelf == FlexAlignMode.Stretch)
                {
                    bool hasExplicitWidth = itemStyle != null
                        && (itemStyle.IsValueDefined(StyleKeys.SizeWidthKey)
                            || (itemStyle.IsValueDefined(StyleKeys.FlexBasisKey) && !itemStyle.Flex.BasisAuto));

                    if (!hasExplicitWidth)
                        full.Size.FullWidth = true;
                }
            }

            if (_isRowMode && _flexItemContentWidths != null
                && comp is Component c
                && _flexItemContentWidths.TryGetValue(c, out double contentW)
                && contentW > 0)
            {
                // Only override when the item has an explicit width or flex-basis. Grow-only
                // items (no explicit width) should keep FillWidth=true so they fill their column;
                // overriding them with the grow-computed width misrepresents it as a content-box
                // value and causes padding/border to push TotalBounds past the column boundary.
                var appliedStyle = c.GetAppliedStyle();
                bool hasExplicitWidth = appliedStyle != null
                    && (appliedStyle.IsValueDefined(StyleKeys.SizeWidthKey)
                        || (appliedStyle.IsValueDefined(StyleKeys.FlexBasisKey) && !appliedStyle.Flex.BasisAuto));

                if (hasExplicitWidth)
                    full.Size.Width = new Unit(contentW, PageUnits.Points);
            }
            base.DoLayoutAChild(comp, full);
        }
    }
}
