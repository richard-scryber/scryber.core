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

        private enum TrackType { Fr, Points, Auto }

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

        protected IContainerComponent Container { get; set; }
        protected Style ContainerStyle { get; set; }
        
        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------

        public LayoutEngineFlexGrid(ContainerComponent container, IPDFLayoutEngine parent, Style containerStyle)
            : base(BuildSyntheticTable(container, containerStyle, out var tracks, out var cellGrid, out var syntheticRows, out var rowTracks), parent)
        {
            _tracks       = tracks;
            _cellGrid     = cellGrid;
            _syntheticRows = syntheticRows;
            _rowTracks    = rowTracks;
            ContainerStyle = containerStyle;
            Container = container;
            
            //((Panel)container).Contents.Insert(0, this.Table);
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

            var asDefined = Container.Content.ToArray();
            try
            {
                
                Container.Content.Clear();
                Container.Content.Insert(0, this.Table);
                
                base.DoLayoutComponent();
            }
            finally
            {
                Container.Content.Clear();
                Container.Content.AddRange(asDefined);
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

            // Column gap — reuse the flex gap keys (gap / column-gap)
            double gapPts = 0;
            if (this.FullStyle.IsValueDefined(StyleKeys.FlexColumnGapKey))
                gapPts = this.FullStyle.Flex.ColumnGap.PointsValue;
            else if (this.FullStyle.IsValueDefined(StyleKeys.FlexGapKey))
                gapPts = this.FullStyle.Flex.Gap.PointsValue;

            int colCount = _tracks.Count;
            double totalGapPts = gapPts * (colCount - 1);
            double workingPts = Math.Max(0, availPts - totalGapPts);

            double[] colPts = CalcColumnPtWidths(workingPts);

            // Set explicit widths on GridCell styles, summing track widths for spanned cells.
            // Cells in each row are stored left-to-right, so a running column cursor is sufficient
            // for the no-row-span case; row spans that leave column gaps are handled by tracking
            // the occupied slots here too.
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

                    int span = Math.Max(1, cell.CellColumnSpan);
                    int rowSpan = Math.Max(1, cell.CellRowSpan);
                    double totalWidth = 0;
                    for (int tc = colCursor; tc < colCursor + span && tc < colPts.Length; tc++)
                        totalWidth += colPts[tc];
                    if (totalWidth > 0)
                        cell.Style.Size.Width = new Unit(totalWidth, PageUnits.Points);

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
            // First pass: resolve fixed columns, accumulate fr total
            double fixedTotal = 0;
            double frTotal = 0;

            foreach (var t in _tracks)
            {
                if (t.Type == TrackType.Points)
                    fixedTotal += t.Value;
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
            out List<TrackDef> rowTracks)
        {
            tracks       = ParseTemplateCols(source, containerStyle);
            rowTracks    = ParseTemplateRows(source, containerStyle);
            cellGrid     = new List<List<GridCell>>();
            syntheticRows = new List<TableRow>();
            
            if(tracks.Count == 0)
                tracks.Add(new TrackDef(TrackType.Fr, 1));
            
            if(rowTracks.Count == 0)
                rowTracks.Add(new TrackDef(TrackType.Fr, 1));

            var grid = new TableGrid();
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
                BuildColumnMajor(items, colCount, grid, cellGrid, syntheticRows);
            else
                BuildRowMajor(items, colCount, grid, cellGrid, syntheticRows);

            return grid;
        }

        private static void BuildRowMajor(
            List<Component> items, int colCount,
            TableGrid grid, List<List<GridCell>> cellGrid, List<TableRow> syntheticRows)
        {
            // Track which grid positions are occupied by spans
            // slot[r][c] = true means already filled by a span from an earlier cell
            var occupied = new System.Collections.Generic.Dictionary<(int r, int c), bool>();
            int r = 0;
            int c = 0;

            foreach (var item in items)
            {
                // Find next free slot
                while (occupied.ContainsKey((r, c)))
                {
                    c++;
                    if (c >= colCount) { c = 0; r++; }
                }

                // Ensure row exists
                while (syntheticRows.Count <= r)
                {
                    var newRow = new TableRow();
                    grid.Rows.Add(newRow);
                    cellGrid.Add(new List<GridCell>());
                    syntheticRows.Add(newRow);
                }

                var itemStyle = (item is IStyledComponent sc) ? sc.GetAppliedStyle() : null;
                int colSpan = itemStyle?.GetValue(StyleKeys.GridColumnSpanKey, 1) ?? 1;
                int rowSpan = itemStyle?.GetValue(StyleKeys.GridRowSpanKey, 1) ?? 1;
                colSpan = Math.Max(1, colSpan);
                rowSpan = Math.Max(1, rowSpan);

                var cell = new GridCell(item, colSpan, rowSpan);
                syntheticRows[r].Cells.Add(cell);
                cellGrid[r].Add(cell);

                // Mark occupied slots
                for (int dr = 0; dr < rowSpan; dr++)
                    for (int dc = 0; dc < colSpan; dc++)
                        if (dr > 0 || dc > 0)
                            occupied[(r + dr, c + dc)] = true;

                c += colSpan;
                if (c >= colCount) { c = 0; r++; }
            }
        }

        private static void BuildColumnMajor(
            List<Component> items, int colCount,
            TableGrid grid, List<List<GridCell>> cellGrid, List<TableRow> syntheticRows)
        {
            // rows = ceil(itemCount / colCount)
            int rowCount = (items.Count + colCount - 1) / colCount;

            // Pre-build rows and cell lists
            var rows = new TableRow[rowCount];
            var rowCells = new List<GridCell>[rowCount];
            for (int r = 0; r < rowCount; r++)
            {
                rows[r] = new TableRow();
                rowCells[r] = new List<GridCell>();
            }

            // Place items column-by-column: item i → col = i/rowCount, row = i%rowCount
            for (int i = 0; i < items.Count; i++)
            {
                int row = i % rowCount;
                var item = items[i];
                var itemStyle = (item is IStyledComponent sc) ? sc.GetAppliedStyle() : null;
                int colSpan = itemStyle?.GetValue(StyleKeys.GridColumnSpanKey, 1) ?? 1;
                int rowSpan = itemStyle?.GetValue(StyleKeys.GridRowSpanKey, 1) ?? 1;
                var cell = new GridCell(item, Math.Max(1, colSpan), Math.Max(1, rowSpan));
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

        // -----------------------------------------------------------------------
        // grid-template-columns / grid-template-rows parsers
        // -----------------------------------------------------------------------

        private static List<TrackDef> ParseTemplateRows(ContainerComponent source, Style sourceStyle)
        {
            if (!(source is IStyledComponent sc) || !sc.HasStyle)
                return new List<TrackDef>();
            
            var raw = sourceStyle.GetValue(StyleKeys.GridTemplateRowsKey, null as string);
            if (string.IsNullOrWhiteSpace(raw))
                return new List<TrackDef>();
            
            return ParseTrackList(raw);
        }

        private static List<TrackDef> ParseTemplateCols(ContainerComponent source, Style sourceStyle)
        {
            if (null == sourceStyle)
                return new List<TrackDef>();

            var raw = sourceStyle.GetValue(StyleKeys.GridTemplateColumnsKey, null as string);
            if (string.IsNullOrWhiteSpace(raw))
                return new List<TrackDef>();

            return ParseTrackList(raw);
        }

        private static List<TrackDef> ParseTrackList(string value)
        {
            var tracks = new List<TrackDef>();
            if (string.IsNullOrWhiteSpace(value))
                return tracks;

            // Expand repeat(N, ...) first
            var expanded = ExpandRepeat(value.Trim());

            // Split on whitespace (commas inside repeat already expanded)
            var tokens = expanded.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in tokens)
            {
                var t = token.Trim().ToLowerInvariant();
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
                        // Convert to points
                        tracks.Add(new TrackDef(TrackType.Points, u.ToPoints().PointsValue));
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
