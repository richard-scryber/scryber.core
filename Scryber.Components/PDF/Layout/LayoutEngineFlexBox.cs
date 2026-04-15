using System;
using System.Collections.Generic;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.PDF.Layout
{
    public class LayoutEngineFlexBox : LayoutEnginePanel
    {
        // True while we are in row layout mode — used to gate the column-break injection.
        private bool _isRowMode;

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
                // Column layout: standard block stacking with optional row gap as alley.
                var gap    = flex.Gap;
                var rowGap = this.FullStyle.IsValueDefined(StyleKeys.FlexRowGapKey) ? flex.RowGap : gap;
                if (rowGap.PointsValue > 0)
                    columnOptions = new PDFColumnOptions() { AlleyWidth = rowGap };

                _isRowMode = false;
                base.DoLayoutBlockComponent(position, columnOptions);
            }
            else
            {
                // Row layout: N columns (one per visible child), with forced column breaks between them.
                int childCount = CountVisibleChildren();
                if (childCount <= 0)
                {
                    _isRowMode = false;
                    base.DoLayoutBlockComponent(position, columnOptions);
                    return;
                }

                var gap    = flex.Gap;
                var colGap = this.FullStyle.IsValueDefined(StyleKeys.FlexColumnGapKey) ? flex.ColumnGap : gap;
                var widths = ComputeColumnWidths(childCount);

                var rowCols = new PDFColumnOptions()
                {
                    ColumnCount   = childCount,
                    AlleyWidth    = colGap,
                    ColumnWidths  = widths
                };

                _isRowMode = true;
                base.DoLayoutBlockComponent(position, rowCols);
                _isRowMode = false;
            }
        }

        /// <summary>
        /// Override DoLayoutChildren: in row mode, force a column break after each flex item
        /// so that each child occupies exactly one column region.
        /// </summary>
        protected override void DoLayoutChildren(ComponentList children)
        {
            if (!_isRowMode)
            {
                base.DoLayoutChildren(children);
                return;
            }

            bool first = true;
            foreach (Component comp in children)
            {
                if (!comp.Visible) continue;

                bool isItem = IsFlexItem(comp);

                // Force-advance to next column before every flex item except the first.
                if (isItem && !first)
                {
                    PDFLayoutBlock block  = this.DocumentLayout.CurrentPage.LastOpenBlock();
                    PDFLayoutRegion reg   = block.CurrentRegion;
                    bool newPage;
                    this.MoveToNextRegion(Unit.Zero, ref reg, ref block, out newPage);
                }

                this.DoLayoutAChild(comp);
                if (isItem) first = false;

                if (!this.ContinueLayout
                    || this.DocumentLayout.CurrentPage.IsClosed
                    || this.DocumentLayout.CurrentPage.CurrentBlock == null)
                    break;
            }
        }

        // -----------------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------------

        // Only block-level containers are flex items; inline/text nodes are not.
        private static bool IsFlexItem(IComponent child)
            => child is IContainerComponent && child is Component c && c.Visible;

        private int CountVisibleChildren()
        {
            var container = this.Component as IContainerComponent;
            if (container == null || !container.HasContent) return 0;
            int count = 0;
            foreach (var child in container.Content)
            {
                if (IsFlexItem(child)) count++;
            }
            return count;
        }

        private ColumnWidths ComputeColumnWidths(int count)
        {
            var container = this.Component as IContainerComponent;
            if (container == null || !container.HasContent) return ColumnWidths.Empty;

            double[] grows = new double[count];
            double   total = 0.0;
            int      i     = 0;

            foreach (var child in container.Content)
            {
                if (!IsFlexItem(child)) continue;
                if (i >= count) break;

                double grow = 1.0;
                if (child is IStyledComponent sc && sc.Style != null
                    && sc.Style.IsValueDefined(StyleKeys.FlexGrowKey))
                    grow = sc.Style.GetValue(StyleKeys.FlexGrowKey, 1.0);

                grows[i] = grow;
                total   += grow;
                i++;
            }

            if (total <= 0) return ColumnWidths.Empty;

            double[] percentages = new double[count];
            for (int j = 0; j < count; j++)
                percentages[j] = grows[j] / total;

            return new ColumnWidths(percentages);
        }
    }
}
