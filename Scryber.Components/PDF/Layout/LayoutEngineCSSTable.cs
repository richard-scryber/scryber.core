using System;
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
        
        protected Style ContainerStyle { get; set; }
        
        public LayoutEngineCSSTable(ContainerComponent container, IPDFLayoutEngine parent, Style style)
            : base(BuildSyntheticTable(container), parent)
        {
            ContainerStyle = style;
        }

        // -----------------------------------------------------------------------
        // Synthetic table construction
        // -----------------------------------------------------------------------

        private static TableGrid BuildSyntheticTable(ContainerComponent source)
        {
            var grid = new TableGrid();

            if (!(source is IContainerComponent ic) || !ic.HasContent)
                return grid;

            // Walk children with state — if we encounter table-cells that aren't wrapped
            // in a table-row, collect them into an anonymous row (CSS anonymous box algorithm).
            TableRow anonRow = null;

            foreach (var item in ic.Content)
            {
                if (!(item is Component comp) || !comp.Visible || comp is Whitespace)
                    continue;

                var style   = comp.GetAppliedStyle();
                var display = style.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block);

                if (display == DisplayMode.TableRow)
                {
                    FlushAnonRow(ref anonRow, grid);
                    grid.Rows.Add(BuildSyntheticRow(comp));
                }
                else if (display == DisplayMode.TableCell)
                {
                    // Anonymous row: accumulate consecutive table-cell siblings
                    if (anonRow == null)
                        anonRow = new TableRow();
                    anonRow.Cells.Add(new CSSTableCell(comp, style));
                }
                else
                {
                    // Non-table element — flush any pending anonymous row and skip
                    FlushAnonRow(ref anonRow, grid);
                }
            }

            FlushAnonRow(ref anonRow, grid);
            return grid;
        }

        private static void FlushAnonRow(ref TableRow anonRow, TableGrid grid)
        {
            if (anonRow != null && anonRow.Cells.Count > 0)
            {
                grid.Rows.Add(anonRow);
                anonRow = null;
            }
        }

        private static TableRow BuildSyntheticRow(Component source)
        {
            var row = new TableRow();

            if (!(source is IContainerComponent ic) || !ic.HasContent)
                return row;

            // Walk children; any non-table-cell visible content is wrapped in an anonymous cell.
            TableCell anonCell = null;

            foreach (var item in ic.Content)
            {
                if (!(item is Component cellComp) || !cellComp.Visible || cellComp is Whitespace)
                    continue;

                var style   = cellComp.GetAppliedStyle();
                var display = style.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block);

                if (display == DisplayMode.TableCell)
                {
                    FlushAnonCell(ref anonCell, row);
                    row.Cells.Add(new CSSTableCell(cellComp, style));
                }
                else
                {
                    // Anonymous cell: wrap non-cell content so the table engine can handle it
                    if (anonCell == null)
                        anonCell = new TableCell();
                    anonCell.Contents.Add(cellComp);
                }
            }

            FlushAnonCell(ref anonCell, row);
            return row;
        }

        private static void FlushAnonCell(ref TableCell anonCell, TableRow row)
        {
            if (anonCell != null && anonCell.Contents.Count > 0)
            {
                row.Cells.Add(anonCell);
                anonCell = null;
            }
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
            private readonly Style _style;

            public CSSTableCell(Component source, Style style)
            {
                _source = (source as ContainerComponent) ?? throw new ArgumentNullException(nameof(source));
                _style = style ?? throw new ArgumentNullException(nameof(style));
                
                var cs = _style.GetValue(StyleKeys.TableCellColumnSpanKey, 1);
                var rs = _style.GetValue(StyleKeys.TableCellRowSpanKey, 1);
                
                if (cs > 1) this.CellColumnSpan = cs;
                if (rs > 1) this.CellRowSpan = rs;
                
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

            // Delegate style resolution to the source panel so that CSS rules applied to it
            // (padding, border, background, etc.) are visible to the table layout engine.
            public override Style GetAppliedStyle()
                => _source?.GetAppliedStyle() ?? base.GetAppliedStyle();
        }
    }
}
