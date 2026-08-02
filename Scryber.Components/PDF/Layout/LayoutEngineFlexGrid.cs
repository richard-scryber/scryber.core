using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.PDF.Layout
{
    /// <summary>
    /// Layout engine for elements with display:grid.
    /// Builds a synthetic TableGrid/TableRow/GridCell structure from the container's children
    /// (auto-flow, row-major), then delegates all layout work to LayoutEngineTable.
    /// Column widths from grid-template-columns (fr, pt, px, %, repeat()) are injected as explicit
    /// pt values onto the GridCell styles before the table engine processes them.
    /// </summary>
    public class LayoutEngineFlexGrid : LayoutEngineTable
    {
        // -----------------------------------------------------------------------
        // Track definition — one entry per column
        // -----------------------------------------------------------------------

        private enum TrackType { Fr, Points, Percent, Auto }

        private readonly struct TrackDef
        {
            public readonly TrackType Type;
            public readonly double Value;
            public TrackDef(TrackType t, double v) { Type = t; Value = v; }
        }

        // -----------------------------------------------------------------------
        // Instance state
        // -----------------------------------------------------------------------

        private readonly List<TrackDef> _tracks;
        private readonly List<List<GridCell>> _cellGrid; // [row][col]
        private readonly List<TrackDef> _rowTracks;
        private readonly List<TableRow> _syntheticRows;

        // Line-name maps built from [name] tokens in grid-template-columns/rows.
        // Key = name, value = sorted list of 1-based line indices.
        private readonly Dictionary<string, List<int>> _colLineNames;
        private readonly Dictionary<string, List<int>> _rowLineNames;

        // Parsed grid-template-areas (may be empty).
        private readonly GridTemplateAreasValue _templateAreas;

        protected IContainerComponent Container { get; set; }
        protected Style ContainerStyle { get; set; }

        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------

        public LayoutEngineFlexGrid(ContainerComponent container, IPDFLayoutEngine parent, Style containerStyle)
            : base(BuildSyntheticTable(container, containerStyle,
                   out var tracks, out var cellGrid, out var syntheticRows, out var rowTracks,
                   out var colLineNames, out var rowLineNames, out var templateAreas), parent)
        {
            _tracks        = tracks;
            _cellGrid      = cellGrid;
            _syntheticRows = syntheticRows;
            _rowTracks     = rowTracks;
            _colLineNames  = colLineNames;
            _rowLineNames  = rowLineNames;
            _templateAreas = templateAreas;
            ContainerStyle = containerStyle;
            Container      = container;
        }

        // -----------------------------------------------------------------------
        // Width/height injection — called before base processes cell styles
        // -----------------------------------------------------------------------

        protected override void DoLayoutComponent()
        {
            if (_tracks.Count > 0 && _cellGrid.Count > 0)
                InjectColumnWidths();

            if (_rowTracks.Count > 0 && _syntheticRows.Count > 0)
                InjectRowHeights();

            InjectRowGaps();

            var asDefined = Container.Content.ToArray();
            try
            {

                Container.Content.Clear();
                Container.Content.Insert(0, this.Table);

                base.DoLayoutComponent();

                // Strip the injected row-gap from the first row of each overflow page.
                ClearContinuationRowGaps();

                // Propagate each cell block's final height to its inner div block.
                StretchAllCellContent();
            }
            finally
            {
                Container.Content.Clear();
                Container.Content.AddRange(asDefined);
            }

        }

        // -----------------------------------------------------------------------
        // Row-span height propagation and continuation-row gap removal
        // -----------------------------------------------------------------------

        // When the grid overflows to a new page each overflow GridReference knows its
        // StartRowIndex — the first row on that page.  InjectRowGaps already injected a
        // row-gap top margin on those rows; strip it so no phantom gap appears at the top
        // of each continuation page.
        // Must run BEFORE StretchAllCellContent so the corrected heights feed into the stretch.
        private void ClearContinuationRowGaps()
        {
            int colCount = this.AllCells.TotalColumnCount;
            bool first = true;

            foreach (var grid in this.AllCells.AllGrids)
            {
                if (first) { first = false; continue; } // first grid = first page, no gap to strip

                int r = grid.StartRowIndex;
                var rowRef = this.AllCells.AllRows[r];
                if (rowRef?.Block == null) continue;

                // Find the gap amount from the first cell in this row that carries one.
                Unit gap = Unit.Zero;
                for (int c = 0; c < colCount && gap == Unit.Zero; c++)
                {
                    var cref = this.AllCells.AllCells[r, c];
                    if (cref?.Block?.Position != null && cref.Block.Position.Margins.Top > Unit.Zero)
                        gap = cref.Block.Position.Margins.Top;
                }
                if (gap == Unit.Zero) continue;

                // Remove the gap from every cell block in this row.
                for (int c = 0; c < colCount; c++)
                {
                    var cref = this.AllCells.AllCells[r, c];
                    if (cref?.Block?.Position == null) continue;
                    if (cref.Block.Position.Margins.Top == Unit.Zero) continue;

                    var margins = cref.Block.Position.Margins;
                    margins.Top = Unit.Zero;
                    cref.Block.Position.Margins = margins;

                    var bounds = cref.Block.TotalBounds;
                    bounds.Height -= gap;
                    cref.Block.TotalBounds = bounds;
                }

                // Shrink the row block to match the corrected cell heights.
                var rowBounds = rowRef.Block.TotalBounds;
                rowBounds.Height -= gap;
                rowRef.Block.TotalBounds = rowBounds;

                // Every subsequent row on this continuation page was positioned by the
                // layout engine using the original (gap-inflated) row offset, so its
                // TotalBounds.Y is `gap` too large.  Slide each one up.
                for (int rNext = r + 1; rNext <= grid.EndRowIndex; rNext++)
                {
                    var nextRow = this.AllCells.AllRows[rNext];
                    if (nextRow?.Block == null) continue;
                    var nextBounds = nextRow.Block.TotalBounds;
                    nextBounds.Y -= gap;
                    nextRow.Block.TotalBounds = nextBounds;
                }

                // Propagate the height reduction upward: the grid continuation block and its
                // containing column region still reflect the old (gap-inclusive) height.
                if (grid.TableBlock?.Columns != null && grid.TableBlock.Columns.Length > 0)
                {
                    var region = grid.TableBlock.Columns[0];

                    var colBounds = region.TotalBounds;
                    colBounds.Height -= gap;
                    region.TotalBounds = colBounds;

                    var usedSize = region.UsedSize;
                    usedSize.Height -= gap;
                    region.UsedSize = usedSize;
                }
                if (grid.TableBlock != null)
                {
                    var tableBounds = grid.TableBlock.TotalBounds;
                    tableBounds.Height -= gap;
                    grid.TableBlock.TotalBounds = tableBounds;

                    // The parent region's UsedSize was incremented by the original
                    // (gap-inflated) TableBlock height when the block was closed.
                    // Reduce it now so that any content laid out after the grid
                    // (e.g. a sibling span) is positioned correctly.
                    if (grid.TableBlock.Parent is PDFLayoutBlock parentBlock &&
                        parentBlock.Columns != null)
                    {
                        foreach (var col in parentBlock.Columns)
                        {
                            if (col?.Contents != null && col.Contents.Contains(grid.TableBlock))
                            {
                                var usedSize = col.UsedSize;
                                usedSize.Height -= gap;
                                col.UsedSize = usedSize;
                                break;
                            }
                        }
                    }
                }
            }
        }

        // After base.DoLayoutComponent(), every GridCell block has its final height:
        //   - non-spanning cells: set to the row's max height by SetCellHeightForRow
        //   - spanning cells:     set to the combined row span height by AdjustRowspanCellHeights
        // The inner grid-item block (the actual div) was sized to content height during layout
        // and is not updated by either step.  Stretch all inner blocks to fill their cell.
        // Uses AllGrids.TableBlock to search only this grid's layout blocks — one per overflow
        // page — so the search is O(grids × depth) rather than O(all-document-pages × depth).
        // This also correctly finds cells placed in headers or footers.
        private void StretchAllCellContent()
        {
            var remaining = new HashSet<GridCell>();
            foreach (var rowCells in _cellGrid)
                foreach (var cell in rowCells)
                    remaining.Add(cell);

            if (remaining.Count == 0) return;

            foreach (var grid in this.AllCells.AllGrids)
            {
                if (remaining.Count == 0) break;
                if (grid.TableBlock != null)
                    SearchAndStretch(grid.TableBlock, remaining);
            }
        }

        // Removes found cells from `remaining` as they are processed so we stop early
        // once every cell has been found (important when the grid spans multiple pages).
        private static void SearchAndStretch(PDFLayoutBlock block, HashSet<GridCell> remaining)
        {
            if (block == null || remaining.Count == 0) return;

            if (block.Owner is GridCell gc && remaining.Contains(gc))
            {
                StretchFirstChildBlock(block);
                remaining.Remove(gc);
                return; // no need to recurse inside the cell we just fixed
            }

            if (block.Columns == null) return;
            foreach (var region in block.Columns)
            {
                if (region?.Contents == null) continue;
                for (int i = 0; i < region.Contents.Count; i++)
                {
                    if (region.Contents[i] is PDFLayoutBlock child)
                        SearchAndStretch(child, remaining);
                }
            }
        }

        // The GridCell block's TotalBounds.Height includes the cell's top margin (which
        // carries the row-gap).  The inner grid-item block lives in the content area
        // *after* that margin, so we must subtract the margin before applying the height.
        private static void StretchFirstChildBlock(PDFLayoutBlock cellBlock)
        {
            var newHeight = cellBlock.TotalBounds.Height;
            if (newHeight <= Unit.Zero) return;

            // Strip the top margin (row-gap) so the inner block doesn't overflow the gap.
            if (cellBlock.Position != null)
                newHeight -= cellBlock.Position.Margins.Top;

            if (newHeight <= Unit.Zero) return;

            if (cellBlock.Columns == null || cellBlock.Columns.Length == 0) return;
            var region = cellBlock.Columns[0];
            if (region?.Contents == null || region.Contents.Count == 0) return;

            if (region.Contents[0] is PDFLayoutBlock innerBlock)
            {
                var bounds = innerBlock.TotalBounds;
                bounds.Height = newHeight;
                innerBlock.TotalBounds = bounds;
            }
        }

        private void InjectColumnWidths()
        {
            // Determine available width (same logic as CalculateTableSpace in base)
            var block = this.Context.DocumentLayout.CurrentPage.LastOpenBlock();
            var tablePos = this.FullStyle.CreatePostionOptions(this.Context.PositionDepth > 0);

            double availPts = tablePos.Width.HasValue
                ? tablePos.Width.Value.PointsValue
                : block.AvailableBounds.Width.PointsValue;

            if (!tablePos.Margins.IsEmpty && !tablePos.Width.HasValue)
                availPts -= (tablePos.Margins.Left + tablePos.Margins.Right).PointsValue;
            if (!tablePos.Padding.IsEmpty)
                availPts -= (tablePos.Padding.Left + tablePos.Padding.Right).PointsValue;

            // Column gap: gap and column-gap both write FlexColumnGapKey so CSS order wins.
            // Fall back to ColumnAlleyKey for legacy usages that only set that key.
            double gapPts = 0;
            if (this.FullStyle.IsValueDefined(StyleKeys.FlexColumnGapKey))
                gapPts = this.FullStyle.Flex.ColumnGap.PointsValue;
            else if (this.FullStyle.IsValueDefined(StyleKeys.ColumnAlleyKey))
                gapPts = this.FullStyle.GetValue(StyleKeys.ColumnAlleyKey, Drawing.Unit.Zero).PointsValue;

            int colCount = _tracks.Count;
            double totalGapPts = gapPts * (colCount - 1);
            double workingPts = Math.Max(0, availPts - totalGapPts);

            double[] colPts = CalcColumnPtWidths(workingPts);

            // Set explicit widths on GridCell styles, summing track widths for spanned cells.
            // Cells in each row are stored left-to-right, so a running column cursor is sufficient
            // for the no-row-span case; row spans that leave column gaps are handled by tracking
            // the occupied slots here too.
            //
            // Column gap is injected as margin-left on cells that are not in the first column,
            // mirroring how InjectRowGaps injects margin-top for row gaps.  The gap space was
            // already removed from workingPts so column widths are already narrower; the margin
            // puts the visual space back between adjacent columns.
            var colGapUnit = gapPts > 0 ? new Unit(gapPts, PageUnits.Points) : Unit.Zero;

            var colOccupied = new System.Collections.Generic.Dictionary<(int row, int col), bool>();
            for (int ri = 0; ri < _cellGrid.Count; ri++)
            {
                var rowCells = _cellGrid[ri];
                int colCursor = 0;
                foreach (var cell in rowCells)
                {
                    // Advance past slots already occupied by row-spans from earlier rows
                    while (colOccupied.ContainsKey((ri, colCursor)))
                        colCursor++;

                    int span    = Math.Max(1, cell.CellColumnSpan);
                    int rowSpan = Math.Max(1, cell.CellRowSpan);

                    // Sum column widths for this cell's span, clipped to available columns
                    int actualSpan = Math.Min(span, colPts.Length - colCursor);
                    double totalWidth = 0;
                    for (int tc = colCursor; tc < colCursor + actualSpan; tc++)
                        totalWidth += colPts[tc];
                    // Spanning cells must also absorb the inter-column gaps they bridge
                    if (gapPts > 0 && actualSpan > 1)
                        totalWidth += gapPts * (actualSpan - 1);

                    if (totalWidth > 0)
                        cell.Style.Size.Width = new Unit(totalWidth, PageUnits.Points);

                    // Column gap: inject left margin on every cell that is not in the first column
                    if (colCursor > 0 && gapPts > 0)
                        cell.Style.Margins.Left = colGapUnit;

                    // Mark slots occupied by this cell's row span
                    for (int dr = 1; dr < rowSpan; dr++)
                        for (int dc = 0; dc < span; dc++)
                            colOccupied[(ri + dr, colCursor + dc)] = true;

                    colCursor += span;
                }
            }
        }

        private double[] CalcColumnPtWidths(double workingPts)
        {
            // First pass: resolve fixed/percent columns, accumulate fr total.
            // Percent tracks are resolved against workingPts here so fr tracks
            // correctly share whatever space remains.
            double fixedTotal = 0;
            double frTotal = 0;

            foreach (var t in _tracks)
            {
                if (t.Type == TrackType.Points)
                    fixedTotal += t.Value;
                else if (t.Type == TrackType.Percent)
                    fixedTotal += (t.Value / 100.0) * workingPts;
                else if (t.Type == TrackType.Fr)
                    frTotal += t.Value;
                // Auto treated as fr=1 if no other fr units, else as fr=1 share
            }

            // Auto columns: if no fr columns exist, treat each Auto as 1fr
            int autoCount = 0;
            foreach (var t in _tracks)
                if (t.Type == TrackType.Auto) autoCount++;

            if (autoCount > 0 && frTotal == 0)
                frTotal = autoCount; // each Auto gets 1fr

            double frSpace = Math.Max(0, workingPts - fixedTotal);

            var widths = new double[_tracks.Count];
            for (int i = 0; i < _tracks.Count; i++)
            {
                var t = _tracks[i];
                switch (t.Type)
                {
                    case TrackType.Points:
                        widths[i] = t.Value;
                        break;
                    case TrackType.Percent:
                        widths[i] = (t.Value / 100.0) * workingPts;
                        break;
                    case TrackType.Fr:
                        widths[i] = frTotal > 0 ? (t.Value / frTotal) * frSpace : 0;
                        break;
                    case TrackType.Auto:
                        widths[i] = frTotal > 0 ? (1.0 / frTotal) * frSpace : 0;
                        break;
                }
            }
            return widths;
        }

        // -----------------------------------------------------------------------
        // Synthetic table construction
        // -----------------------------------------------------------------------

        private static TableGrid BuildSyntheticTable(
            ContainerComponent source,
            Style containerStyle,
            out List<TrackDef> tracks,
            out List<List<GridCell>> cellGrid,
            out List<TableRow> syntheticRows,
            out List<TrackDef> rowTracks,
            out Dictionary<string, List<int>> colLineNames,
            out Dictionary<string, List<int>> rowLineNames,
            out GridTemplateAreasValue templateAreas)
        {
            tracks        = ParseTemplateCols(source, containerStyle, out colLineNames);
            rowTracks     = ParseTemplateRows(source, containerStyle, out rowLineNames);
            templateAreas = ParseTemplateAreas(source, containerStyle, colLineNames, rowLineNames);
            cellGrid      = new List<List<GridCell>>();
            syntheticRows = new List<TableRow>();

            if (tracks.Count == 0)
                tracks.Add(new TrackDef(TrackType.Fr, 1));

            if (rowTracks.Count == 0)
                rowTracks.Add(new TrackDef(TrackType.Fr, 1));

            var grid     = new TableGrid();
            int colCount = tracks.Count;

            if (colCount == 0 || !(source is IContainerComponent ic) || !ic.HasContent)
                return grid;

            // Collect visible block-level children
            var items = new List<Component>();
            foreach (var item in ic.Content)
            {
                if (item is Component c && c.Visible && c is IContainerComponent)
                    items.Add(c);
            }

            if (items.Count == 0)
                return grid;

            var autoFlow = containerStyle.GetValue(StyleKeys.GridAutoFlowKey, GridAutoFlow.Row);

            if (autoFlow == GridAutoFlow.Column)
                BuildColumnMajor(items, colCount, grid, cellGrid, syntheticRows,
                                  colLineNames, rowLineNames, templateAreas);
            else
                BuildRowMajor(items, colCount, grid, cellGrid, syntheticRows,
                              colLineNames, rowLineNames, templateAreas);

            return grid;
        }

        // -----------------------------------------------------------------------
        // Placement resolution
        // -----------------------------------------------------------------------

        private struct GridItemPlacement
        {
            public Component Item;
            public int ColStart; // 0-indexed, or -1 for auto
            public int ColSpan;  // >= 1
            public int RowStart; // 0-indexed, or -1 for auto
            public int RowSpan;  // >= 1
        }

        // Resolves a component's grid placement from its applied style.
        // col/row-start -1 means "auto" (let the auto-flow algorithm choose).
        private static GridItemPlacement ResolveGridItemPlacement(
            Component item, int colCount,
            Dictionary<string, List<int>> colLineNames,
            Dictionary<string, List<int>> rowLineNames,
            GridTemplateAreasValue templateAreas)
        {
            var style = (item is IStyledComponent sc) ? sc.GetAppliedStyle() : null;

            // Named grid-area reference takes precedence over individual line values.
            if (style != null)
            {
                var areaName = style.GetValue(StyleKeys.GridAreaNameKey, null as string);
                if (!string.IsNullOrEmpty(areaName) &&
                    templateAreas.TryGetAreaBounds(areaName,
                        out int rs, out int re, out int cs, out int ce))
                {
                    return new GridItemPlacement
                    {
                        Item     = item,
                        ColStart = cs - 1,
                        ColSpan  = Math.Max(1, ce - cs),
                        RowStart = rs - 1,
                        RowSpan  = Math.Max(1, re - rs)
                    };
                }
            }

            var colStart = style?.GetValue(StyleKeys.GridColumnStartKey, GridLineValue.Unset) ?? GridLineValue.Unset;
            var colEnd   = style?.GetValue(StyleKeys.GridColumnEndKey,   GridLineValue.Unset) ?? GridLineValue.Unset;
            var rowStart = style?.GetValue(StyleKeys.GridRowStartKey,    GridLineValue.Unset) ?? GridLineValue.Unset;
            var rowEnd   = style?.GetValue(StyleKeys.GridRowEndKey,      GridLineValue.Unset) ?? GridLineValue.Unset;

            int cs0 = colStart.ResolveStart(colLineNames);  // 0-based or -1
            int rs0 = rowStart.ResolveStart(rowLineNames);

            int colSpan = colEnd.IsSet
                ? colEnd.ResolveSpan(cs0, colCount, colLineNames)
                : colStart.IsSpan ? Math.Max(1, colStart.Value) : 1;

            int rowSpan = rowEnd.IsSet
                ? rowEnd.ResolveSpan(rs0, rowLineNames.Count, rowLineNames)
                : rowStart.IsSpan ? Math.Max(1, rowStart.Value) : 1;

            // Clamp column span to available columns.
            int maxColSpan = cs0 >= 0 ? colCount - cs0 : colCount;
            colSpan = Math.Max(1, Math.Min(colSpan, maxColSpan));

            return new GridItemPlacement
            {
                Item     = item,
                ColStart = cs0,
                ColSpan  = colSpan,
                RowStart = rs0,
                RowSpan  = Math.Max(1, rowSpan)
            };
        }

        // -----------------------------------------------------------------------
        // Placement helpers
        // -----------------------------------------------------------------------

        private static void EnsureRows(
            List<TableRow> syntheticRows, TableGrid grid, List<List<GridCell>> cellGrid, int maxRow)
        {
            while (syntheticRows.Count <= maxRow)
            {
                var newRow = new TableRow();
                grid.Rows.Add(newRow);
                cellGrid.Add(new List<GridCell>());
                syntheticRows.Add(newRow);
            }
        }

        // Stores a cell in the placement map (column-keyed SortedDictionary per row) and marks
        // the occupied slots.  Cells are committed to syntheticRows/cellGrid in column order
        // after all placements are resolved, so rendering order matches column position.
        private static void StoreCell(
            in GridItemPlacement p, int r, int c,
            Dictionary<(int, int), bool> occupied,
            Dictionary<int, SortedDictionary<int, GridCell>> placedCells)
        {
            var cell = new GridCell(p.Item, p.ColSpan, p.RowSpan);
            if (!placedCells.ContainsKey(r))
                placedCells[r] = new SortedDictionary<int, GridCell>();
            placedCells[r][c] = cell;
            for (int dr = 0; dr < p.RowSpan; dr++)
                for (int dc = 0; dc < p.ColSpan; dc++)
                    occupied[(r + dr, c + dc)] = true;
        }

        // Returns true when the rectangular area [r..r+rowSpan) x [c..c+colSpan) is free.
        private static bool CanPlace(
            Dictionary<(int, int), bool> occupied, int r, int c, int colSpan, int rowSpan)
        {
            for (int dr = 0; dr < rowSpan; dr++)
                for (int dc = 0; dc < colSpan; dc++)
                    if (occupied.ContainsKey((r + dr, c + dc))) return false;
            return true;
        }

        // -----------------------------------------------------------------------
        // Row-major placement
        // -----------------------------------------------------------------------

        private static void BuildRowMajor(
            List<Component> items, int colCount,
            TableGrid grid, List<List<GridCell>> cellGrid, List<TableRow> syntheticRows,
            Dictionary<string, List<int>> colLineNames,
            Dictionary<string, List<int>> rowLineNames,
            GridTemplateAreasValue templateAreas)
        {
            var occupied    = new Dictionary<(int, int), bool>();
            // Cells are stored here (row → col-ordered map) and committed to rows in
            // column order at the end, so the table engine sees them left-to-right.
            var placedCells = new Dictionary<int, SortedDictionary<int, GridCell>>();
            var placements  = new List<GridItemPlacement>(items.Count);
            foreach (var item in items)
                placements.Add(ResolveGridItemPlacement(item, colCount,
                               colLineNames, rowLineNames, templateAreas));

            // Phase 1: items with both row AND column explicitly set.
            // Pre-place them so the auto-flow cursor can route around them.
            foreach (var p in placements)
            {
                if (p.RowStart < 0 || p.ColStart < 0) continue;
                EnsureRows(syntheticRows, grid, cellGrid, p.RowStart + p.RowSpan - 1);
                StoreCell(p, p.RowStart, p.ColStart, occupied, placedCells);
            }

            // Phase 2: auto-flow cursor for everything else.
            // Items with explicit column are placed at their column in the first
            // available row (from the cursor) but do not advance the cursor.
            // Items with explicit row only find the first free column in that row.
            // Fully auto items use the normal left-to-right, row-by-row cursor.
            int curR = 0, curC = 0;
            foreach (var p in placements)
            {
                if (p.RowStart >= 0 && p.ColStart >= 0) continue; // already placed

                int r, c;

                if (p.ColStart >= 0)
                {
                    // Explicit column, auto row — find the first row (from curR) where
                    // the column range is free.
                    c = p.ColStart;
                    r = curR;
                    while (!CanPlace(occupied, r, c, p.ColSpan, p.RowSpan))
                        r++;
                    // Explicit-column items do not advance the auto-flow cursor.
                }
                else if (p.RowStart >= 0)
                {
                    // Explicit row, auto column — scan left to right for a free slot.
                    r = p.RowStart;
                    c = 0;
                    while (c + p.ColSpan <= colCount && !CanPlace(occupied, r, c, p.ColSpan, p.RowSpan))
                        c++;
                    if (c + p.ColSpan > colCount) { r = curR; c = curC; } // fallback
                    // Explicit-row items do not advance the auto-flow cursor.
                }
                else
                {
                    // Fully auto: advance cursor until we find a slot that fits.
                    r = curR; c = curC;
                    while (c + p.ColSpan > colCount || !CanPlace(occupied, r, c, p.ColSpan, p.RowSpan))
                    {
                        c++;
                        if (c + p.ColSpan > colCount) { c = 0; r++; }
                    }
                    // Advance the cursor past this item.
                    curC = c + p.ColSpan;
                    curR = r;
                    if (curC >= colCount) { curC = 0; curR++; }
                }

                EnsureRows(syntheticRows, grid, cellGrid, r + p.RowSpan - 1);
                StoreCell(p, r, c, occupied, placedCells);
            }

            // Commit cells to rows and cellGrid in column order.
            // Columns with no placed cell AND not covered by a rowspan from above need an
            // empty placeholder so the table engine doesn't slide subsequent cells left
            // (this is the case for dot "." cells from grid-template-areas).
            for (int r = 0; r < syntheticRows.Count; r++)
            {
                if (!placedCells.TryGetValue(r, out var rowDict))
                    rowDict = new SortedDictionary<int, GridCell>();

                for (int c = 0; c < colCount; c++)
                {
                    if (!rowDict.ContainsKey(c) && !occupied.ContainsKey((r, c)))
                        rowDict[c] = new GridCell(null, 1, 1); // empty placeholder for dot/gap cell
                }

                foreach (var kvp in rowDict) // SortedDictionary iterates in ascending column order
                {
                    syntheticRows[r].Cells.Add(kvp.Value);
                    cellGrid[r].Add(kvp.Value);
                }
            }
        }

        private static void BuildColumnMajor(
            List<Component> items, int colCount,
            TableGrid grid, List<List<GridCell>> cellGrid, List<TableRow> syntheticRows,
            Dictionary<string, List<int>> colLineNames,
            Dictionary<string, List<int>> rowLineNames,
            GridTemplateAreasValue templateAreas)
        {
            // rows = ceil(itemCount / colCount)
            int rowCount = (items.Count + colCount - 1) / colCount;

            // Pre-build rows and cell lists
            var rows     = new TableRow[rowCount];
            var rowCells = new List<GridCell>[rowCount];
            for (int r = 0; r < rowCount; r++)
            {
                rows[r]     = new TableRow();
                rowCells[r] = new List<GridCell>();
            }

            // Place items column-by-column: item i → col = i/rowCount, row = i%rowCount
            for (int i = 0; i < items.Count; i++)
            {
                int row = i % rowCount;
                var item = items[i];
                var p = ResolveGridItemPlacement(item, colCount,
                        colLineNames, rowLineNames, templateAreas);
                var cell = new GridCell(item, Math.Max(1, p.ColSpan), Math.Max(1, p.RowSpan));
                rows[row].Cells.Add(cell);
                rowCells[row].Add(cell);
            }

            for (int r = 0; r < rowCount; r++)
            {
                grid.Rows.Add(rows[r]);
                cellGrid.Add(rowCells[r]);
                syntheticRows.Add(rows[r]);
            }
        }

        // -----------------------------------------------------------------------
        // Row height injection
        // -----------------------------------------------------------------------

        private void InjectRowHeights()
        {
            // TableRow strips SizeHeightKey in RemoveInapplicableStyles, so we inject
            // the explicit height onto each GridCell in the row instead.
            for (int r = 0; r < _rowTracks.Count && r < _cellGrid.Count; r++)
            {
                var track = _rowTracks[r];
                if (track.Type == TrackType.Points && track.Value > 0)
                {
                    var unit = new Unit(track.Value, PageUnits.Points);
                    foreach (var cell in _cellGrid[r])
                        cell.Style.Size.Height = unit;
                }
            }
        }

        private void InjectRowGaps()
        {
            double rowGapPts = GetRowGapPts();
            if (rowGapPts <= 0) return;

            // Add a top margin to all cells in rows after the first.
            // Cell top margin adds space between the bottom of the previous row and the
            // top of this row's cells — exactly the CSS row-gap behaviour.
            var rowGapUnit = new Unit(rowGapPts, PageUnits.Points);
            for (int r = 1; r < _cellGrid.Count; r++)
            {
                foreach (var cell in _cellGrid[r])
                    cell.Style.Margins.Top = rowGapUnit;
            }
        }

        private double GetRowGapPts()
        {
            // gap and row-gap both write FlexRowGapKey so CSS order determines the winner.
            if (this.FullStyle.IsValueDefined(StyleKeys.FlexRowGapKey))
                return this.FullStyle.Flex.RowGap.PointsValue;
            return 0;
        }

        // -----------------------------------------------------------------------
        // grid-template-columns / grid-template-rows parsers
        // -----------------------------------------------------------------------

        private static List<TrackDef> ParseTemplateRows(ContainerComponent source, Style sourceStyle,
            out Dictionary<string, List<int>> lineNames)
        {
            lineNames = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);
            if (!(source is IStyledComponent sc) || !sc.HasStyle)
                return new List<TrackDef>();

            var raw = sourceStyle.GetValue(StyleKeys.GridTemplateRowsKey, null as string);
            if (string.IsNullOrWhiteSpace(raw))
                return new List<TrackDef>();

            return ParseTrackList(raw, lineNames);
        }

        private static List<TrackDef> ParseTemplateCols(ContainerComponent source, Style sourceStyle,
            out Dictionary<string, List<int>> lineNames)
        {
            lineNames = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);
            if (null == sourceStyle)
                return new List<TrackDef>();

            var raw = sourceStyle.GetValue(StyleKeys.GridTemplateColumnsKey, null as string);
            if (string.IsNullOrWhiteSpace(raw))
                return new List<TrackDef>();

            return ParseTrackList(raw, lineNames);
        }

        // Parses a track-list string into TrackDefs and populates a name→line-index map.
        // [name1 name2] tokens are stripped; each name maps to the 1-based line index
        // immediately following the bracket group (line 1 is before track 0).
        private static List<TrackDef> ParseTrackList(string value,
            Dictionary<string, List<int>> lineNames)
        {
            var tracks = new List<TrackDef>();
            if (string.IsNullOrWhiteSpace(value))
                return tracks;

            // Expand repeat(N, ...) first — names inside repeat expand verbatim.
            var expanded = ExpandRepeat(value.Trim());

            // Split on whitespace, then walk tokens:
            // [name...] groups are recorded against the next 1-based line index;
            // track tokens are parsed and added to the track list.
            var tokens = expanded.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in tokens)
            {
                var t = token.Trim();

                // [name1 name2 ...] — line-name group; line index = tracks so far + 1
                if (t.StartsWith("[") && t.EndsWith("]"))
                {
                    int lineIndex = tracks.Count + 1;
                    foreach (var name in t.Substring(1, t.Length - 2)
                             .Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!lineNames.TryGetValue(name, out var nameList))
                            lineNames[name] = nameList = new List<int>();
                        if (!nameList.Contains(lineIndex)) nameList.Add(lineIndex);
                    }
                    continue;
                }

                t = t.ToLowerInvariant();
                if (t == "auto")
                {
                    tracks.Add(new TrackDef(TrackType.Auto, 1.0));
                }
                else if (t.EndsWith("fr"))
                {
                    if (double.TryParse(t.Substring(0, t.Length - 2),
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out var fr))
                        tracks.Add(new TrackDef(TrackType.Fr, fr));
                }
                else
                {
                    // Attempt to parse as a Scryber Unit (pt, px, mm, cm, in, %)
                    Unit u;
                    if (Unit.TryParse(t, out u))
                    {
                        if (u.IsRelative)
                            // Percent (and other relative units) — defer resolution against
                            // the available container width until CalcColumnPtWidths runs.
                            tracks.Add(new TrackDef(TrackType.Percent, u.Value));
                        else
                            tracks.Add(new TrackDef(TrackType.Points, u.PointsValue));
                    }
                }
            }

            return tracks;
        }

        private static readonly Regex RepeatRegex =
            new Regex(@"repeat\(\s*(\d+)\s*,\s*([^)]+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static string ExpandRepeat(string value)
        {
            return RepeatRegex.Replace(value, m =>
            {
                int count = int.Parse(m.Groups[1].Value);
                string track = m.Groups[2].Value.Trim();
                var parts = new List<string>();
                for (int i = 0; i < count; i++)
                    parts.Add(track);
                return string.Join(" ", parts);
            });
        }

        // Parses grid-template-areas and injects implicit area-start/area-end line names
        // into the column and row name maps.
        private static GridTemplateAreasValue ParseTemplateAreas(
            ContainerComponent source, Style containerStyle,
            Dictionary<string, List<int>> colLineNames,
            Dictionary<string, List<int>> rowLineNames)
        {
            if (!(source is IStyledComponent sc) || !sc.HasStyle)
                return default;

            var areas = containerStyle.GetValue(StyleKeys.GridTemplateAreasKey,
                            default(GridTemplateAreasValue));
            if (areas.IsEmpty) return default;

            // Inject implicit line names: each named area "foo" creates
            // foo-start and foo-end in both axes.
            foreach (var name in areas.AreaNames())
            {
                if (!areas.TryGetAreaBounds(name,
                        out int rs, out int re, out int cs, out int ce))
                    continue;

                AddLineName(colLineNames, name + "-start", cs);
                AddLineName(colLineNames, name + "-end",   ce);
                AddLineName(rowLineNames, name + "-start", rs);
                AddLineName(rowLineNames, name + "-end",   re);
            }

            return areas;
        }

        private static void AddLineName(Dictionary<string, List<int>> map, string name, int lineIndex)
        {
            if (!map.TryGetValue(name, out var list))
                map[name] = list = new List<int>();
            if (!list.Contains(lineIndex)) list.Add(lineIndex);
        }

        // -----------------------------------------------------------------------
        // Proxy cell — transparent table cell containing the grid item as a child
        // -----------------------------------------------------------------------

        internal sealed class GridCell : TableCell
        {
            /// <summary>
            /// Creates a cell that contains <paramref name="source"/> as its direct child.
            /// The source Panel is laid out with its own border, padding, and explicit height.
            /// </summary>
            public GridCell(Component source, int colSpan = 1, int rowSpan = 1) : base()
            {
                if (source != null)
                    this.Contents.Add(source);
                if (colSpan > 1) this.CellColumnSpan = colSpan;
                if (rowSpan > 1) this.CellRowSpan = rowSpan;
            }

            // GridCell has no visual styling of its own — the item Panel handles that.
            protected override Styles.Style GetBaseStyle()
            {
                var style = base.GetBaseStyle();
                style.Border.LineStyle = Drawing.LineType.None;
                style.Border.Width = Drawing.Unit.Zero;
                style.Padding.All = Drawing.Unit.Zero;
                style.Margins.All = Drawing.Unit.Zero;
                return style;
            }
        }
    }
}
