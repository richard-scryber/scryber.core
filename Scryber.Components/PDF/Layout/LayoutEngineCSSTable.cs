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
            // Holds a run of non-row/non-cell children (e.g. a plain <img>) inside anonRow -
            // these must also be wrapped in an anonymous cell, not discarded, otherwise any
            // loose content directly inside a display:table element (a CKEditor
            // <figure class="image"> around a bare <img>, for example) silently vanishes
            // from layout entirely.
            TableCell anonRowCell = null;

            AppendRowChildren(ic, source, grid, ref anonRow, ref anonRowCell);

            FlushAnonRow(ref anonRow, grid);
            return grid;
        }

        // Walks ic's children looking for table-rows/table-cells to add to grid, exactly as
        // BuildSyntheticTable's own loop used to do inline - split out so it can recurse into
        // an IInvisibleContainer child (e.g. a {{#each}}/{{#with}} repeat instance sitting
        // directly inside a display:table element) and treat *its* children as if they were
        // direct children of the table, per IInvisibleContainer's own documented contract
        // ("it's children are laid out directly in the engine as if the components contents
        // were part of the parent collection"). Without this, a repeated table-row nested one
        // level inside the repeat component was invisible to the direct-child scan here and
        // got swallowed whole into an anonymous cell instead - laid out in isolation, with no
        // shared column widths with the table's other rows.
        private static void AppendRowChildren(IContainerComponent ic, Component source, TableGrid grid, ref TableRow anonRow, ref TableCell anonRowCell)
        {
            foreach (var item in ic.Content)
            {
                if (!(item is Component comp) || !comp.Visible || comp is Whitespace  || comp is BindingTemplateComponent)
                    continue;

                var style   = comp.GetAppliedStyle();
                var display = style.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block);

                if (display == DisplayMode.TableRow)
                {
                    FlushAnonRow(ref anonRow, grid);
                    anonRowCell = null;
                    grid.Rows.Add(BuildSyntheticRow(comp));
                }
                else if (display == DisplayMode.TableCell)
                {
                    // Anonymous row: accumulate consecutive table-cell siblings
                    if (anonRow == null)
                        anonRow = new TableRow();
                    anonRow.Cells.Add(new CSSTableCell(comp, style));
                    anonRowCell = null; // an explicit cell ends any loose-content run
                }
                else if (comp is IInvisibleContainer invisible && invisible.HasContent)
                {
                    AppendRowChildren(invisible, source, grid, ref anonRow, ref anonRowCell);
                }
                else
                {
                    // Non-table element directly inside a display:table container -
                    // wrap it in an anonymous row + anonymous cell rather than dropping it.
                    NormalizeLooseInlineBlockDisplay(style, display);
                    if (anonRow == null)
                        anonRow = new TableRow();
                    if (anonRowCell == null)
                    {
                        anonRowCell = new AnonymousCell(source);
                        anonRow.Cells.Add(anonRowCell);
                    }
                    anonRowCell.Contents.Add(comp);
                }
            }
        }

        private static void FlushAnonRow(ref TableRow anonRow, TableGrid grid)
        {
            if (anonRow != null && anonRow.Cells.Count > 0)
            {
                if (anonRow.Cells.Count == 1)
                {
                    var cell = anonRow.Cells[0];
                    if (cell.Contents.Count == 0 || IsEmptyCellContent(cell))
                    {
                        //our anonRow only contains an invisible container, whitespace or is empty
                        anonRow = null;
                        return;
                    }
                }
                else //we have more than one cell, let's check the last one and remove if 'empty'
                {
                    var cell = anonRow.Cells[anonRow.Cells.Count - 1];
                    if (cell.Contents.Count == 0 || IsEmptyCellContent(cell))
                        anonRow.Cells.Remove(cell);
                    //leave it in as the row should be added.
                }

                grid.Rows.Add(anonRow);
                anonRow = null;
            }
        }

        // NormalizeLooseInlineBlockDisplay (used just above, twice) is defined on the shared
        // LayoutEngineTable base - see its doc comment there for the full reasoning
        // (LayoutEngineFlexGrid's GridCell needs the exact same leniency).

        private static bool IsEmptyCellContent(TableCell cell)
        {
            if (cell.Contents.Count == 0)
                return true;
            else if (cell.Contents.Count == 1)
            {
                var content = cell.Contents[0];
                if(content is Whitespace)
                    return true;
                else if(content is IInvisibleContainer)
                    return true;
                else if(content is BindingTemplateComponent)
                    return true;
            }
            return false;
        }

        private static TableRow BuildSyntheticRow(Component source)
        {
            var row = new TableRow();

            if (!(source is IContainerComponent ic) || !ic.HasContent)
                return row;

            // Walk children; any non-table-cell visible content is wrapped in an anonymous cell.
            TableCell anonCell = null;

            AppendCellChildren(ic, source, row, ref anonCell);

            FlushAnonCell(ref anonCell, row);
            return row;
        }

        // Mirrors AppendRowChildren's own reasoning, one level down: a table-cell nested one
        // level inside an IInvisibleContainer child of a table-row (e.g. a {{#with}} around a
        // single cell) is treated as if it were a direct child of the row.
        private static void AppendCellChildren(IContainerComponent ic, Component source, TableRow row, ref TableCell anonCell)
        {
            foreach (var item in ic.Content)
            {
                if (!(item is Component cellComp) || !cellComp.Visible || cellComp is Whitespace || cellComp is BindingTemplateComponent)
                    continue;

                var style   = cellComp.GetAppliedStyle();
                var display = style.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block);

                if (display == DisplayMode.TableCell)
                {
                    FlushAnonCell(ref anonCell, row);
                    row.Cells.Add(new CSSTableCell(cellComp, style));
                }
                else if (cellComp is IInvisibleContainer invisible && invisible.HasContent)
                {
                    AppendCellChildren(invisible, source, row, ref anonCell);
                }
                else
                {
                    // Anonymous cell: wrap non-cell content so the table engine can handle it
                    NormalizeLooseInlineBlockDisplay(style, display);
                    if (anonCell == null)
                        anonCell = new AnonymousCell(source);
                    anonCell.Contents.Add(cellComp);
                }
            }
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

        // -----------------------------------------------------------------------
        // Anonymous cell — holds loose, non-table-structured children (CSS anonymous
        // box algorithm) without reparenting them.
        // -----------------------------------------------------------------------

        /// <summary>
        /// A synthetic TableCell used to wrap loose content that sits directly inside a
        /// display:table/table-row element without an explicit table-cell wrapper (e.g. a
        /// CKEditor "&lt;figure class="image"&gt;&lt;img&gt;&lt;/figure&gt;"). The items added here keep
        /// their real Parent (the original source container), rather than being reparented
        /// to this synthetic, never-attached cell - reparenting them would break upward
        /// lookups such as the resource container used to register image/font artefacts,
        /// which walk the real component tree, not this layout-only synthetic structure.
        /// </summary>
        private sealed class AnonymousCell : TableCell, IContainerComponent
        {
            private readonly ComponentList _items;

            public AnonymousCell(Component owner)
            {
                _items = new ComponentList(owner, owner.Type);
            }

            // Base ContainerComponent.HasContent checks its own private backing field (never
            // set here, since InnerContent is overridden below), so it must be re-implemented
            // via the interface too - same reasoning as CSSTableCell above.
            bool IContainerComponent.HasContent => _items.Count > 0;

            ComponentList IContainerComponent.Content => _items;

            public override ComponentList Contents => _items;

            protected override ComponentList InnerContent => _items;
        }
    }
}
