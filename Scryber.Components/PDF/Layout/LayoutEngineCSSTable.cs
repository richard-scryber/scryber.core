using System.Collections.Generic;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.PDF.Layout
{
    /// <summary>
    /// Layout engine for elements with display:table — builds a synthetic TableGrid/TableRow/TableCell
    /// structure from children styled with display:table-row / display:table-cell, then delegates all
    /// layout work to the existing LayoutEngineTable.
    /// </summary>
    public class LayoutEngineCSSTable : LayoutEngineTable
    {
        public LayoutEngineCSSTable(ContainerComponent container, IPDFLayoutEngine parent)
            : base(BuildSyntheticTable(container), parent)
        {
        }

        // -----------------------------------------------------------------------
        // Synthetic table construction
        // -----------------------------------------------------------------------

        private static TableGrid BuildSyntheticTable(ContainerComponent source)
        {
            var grid = new TableGrid();

            if (!(source is IContainerComponent ic) || !ic.HasContent)
                return grid;

            foreach (var item in ic.Content)
            {
                if (!(item is Component rowComp) || !rowComp.Visible)
                    continue;
                if (!(item is IStyledComponent rowStyled) || !rowStyled.HasStyle)
                    continue;

                var display = rowStyled.Style.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block);
                if (display != DisplayMode.TableRow)
                    continue;

                var row = BuildSyntheticRow(rowComp);
                grid.Rows.Add(row);
            }

            return grid;
        }

        private static TableRow BuildSyntheticRow(Component source)
        {
            var row = new TableRow();

            if (!(source is IContainerComponent ic) || !ic.HasContent)
                return row;

            foreach (var item in ic.Content)
            {
                if (!(item is Component cellComp) || !cellComp.Visible)
                    continue;
                if (!(item is IStyledComponent cellStyled) || !cellStyled.HasStyle)
                    continue;

                var display = cellStyled.Style.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block);
                if (display != DisplayMode.TableCell)
                    continue;

                row.Cells.Add(new CSSTableCell(cellComp));
            }

            return row;
        }

        // -----------------------------------------------------------------------
        // Proxy cell — wraps a display:table-cell panel without moving its children
        // -----------------------------------------------------------------------

        /// <summary>
        /// A TableCell whose content is the children of the original CSS display:table-cell panel.
        /// No children are moved; we re-implement IContainerComponent and override Contents/InnerContent
        /// to delegate transparently to the source panel's child list.
        /// </summary>
        private sealed class CSSTableCell : TableCell, IContainerComponent
        {
            private readonly ContainerComponent _source;

            public CSSTableCell(Component source)
            {
                _source = source as ContainerComponent;

                if (source is IStyledComponent sc && sc.HasStyle)
                {
                    var cs = sc.Style.GetValue(StyleKeys.TableCellColumnSpanKey, 1);
                    var rs = sc.Style.GetValue(StyleKeys.TableCellRowSpanKey, 1);
                    if (cs > 1) this.CellColumnSpan = cs;
                    if (rs > 1) this.CellRowSpan = rs;
                }
            }

            // Re-implement IContainerComponent so LayoutEngineBase.GetComponentChildren uses source's list.
            // ContainerComponent.HasContent checks its own private _children field (null for this cell).
            bool IContainerComponent.HasContent
                => _source?.HasContent ?? false;

            ComponentList IContainerComponent.Content
                => _source != null ? ((IContainerComponent)_source).Content : base.InnerContent;

            // TableCell.Contents and any code paths using InnerContent also delegate to source.
            public override ComponentList Contents
                => _source != null ? ((IContainerComponent)_source).Content : base.Contents;

            protected override ComponentList InnerContent
                => _source != null ? ((IContainerComponent)_source).Content : base.InnerContent;
        }
    }
}
