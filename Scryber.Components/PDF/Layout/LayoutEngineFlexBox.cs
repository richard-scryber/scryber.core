using System;
using System.Collections.Generic;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.PDF.Layout
{
    public class LayoutEngineFlexBox : LayoutEnginePanel
    {
        public LayoutEngineFlexBox(ContainerComponent container, IPDFLayoutEngine parent)
            : base(container, parent)
        {
        }

        protected override void DoLayoutBlockComponent(PDFPositionOptions position, PDFColumnOptions columnOptions)
        {
            var flex = this.FullStyle.Flex;
            var direction = flex.Direction;
            var gap = flex.Gap;
            var rowGap = this.FullStyle.IsValueDefined(StyleKeys.FlexRowGapKey) ? flex.RowGap : gap;
            var colGap = this.FullStyle.IsValueDefined(StyleKeys.FlexColumnGapKey) ? flex.ColumnGap : gap;

            if (direction == FlexDirection.Column || direction == FlexDirection.ColumnReverse)
            {
                // Column layout: standard block stacking with optional row gap as alley
                if (rowGap.PointsValue > 0)
                    columnOptions = new PDFColumnOptions() { AlleyWidth = rowGap };
                base.DoLayoutBlockComponent(position, columnOptions);
            }
            else
            {
                // Row layout: use multi-column layout to place children side by side
                int childCount = CountVisibleChildren();
                if (childCount <= 0)
                {
                    base.DoLayoutBlockComponent(position, columnOptions);
                    return;
                }

                var widths = ComputeColumnWidths(childCount);
                var rowCols = new PDFColumnOptions()
                {
                    ColumnCount = childCount,
                    AlleyWidth = colGap,
                    ColumnWidths = widths
                };

                base.DoLayoutBlockComponent(position, rowCols);
            }
        }

        private int CountVisibleChildren()
        {
            var container = this.Component as IContainerComponent;
            if (container == null) return 0;
            int count = 0;
            foreach (var child in container.Content)
            {
                if (child is IStyledComponent sc)
                {
                    var s = sc.Style;
                    if (s != null && s.IsValueDefined(StyleKeys.PositionDisplayKey))
                    {
                        if (s.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block) == DisplayMode.Invisible)
                            continue;
                    }
                }
                if (child is Component)
                    count++;
            }
            return count;
        }

        private ColumnWidths ComputeColumnWidths(int count)
        {
            var container = this.Component as IContainerComponent;
            if (container == null) return ColumnWidths.Empty;

            double[] grows = new double[count];
            double total = 0.0;
            int i = 0;

            foreach (var child in container.Content)
            {
                if (!(child is Component)) continue;
                if (i >= count) break;

                double grow = 1.0;
                if (child is IStyledComponent sc && sc.Style != null && sc.Style.IsValueDefined(StyleKeys.FlexGrowKey))
                    grow = sc.Style.GetValue(StyleKeys.FlexGrowKey, 1.0);

                grows[i] = grow;
                total += grow;
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
