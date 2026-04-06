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

        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------

        public LayoutEngineFlexGrid(ContainerComponent container, IPDFLayoutEngine parent)
            : base(BuildSyntheticTable(container, out var tracks, out var cellGrid), parent)
        {
            _tracks = tracks;
            _cellGrid = cellGrid;
        }

        // -----------------------------------------------------------------------
        // Width injection — called before base processes cell styles
        // -----------------------------------------------------------------------

        protected override void DoLayoutComponent()
        {
            if (_tracks.Count > 0 && _cellGrid.Count > 0)
                InjectColumnWidths();

            base.DoLayoutComponent();
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

            // Set explicit widths on GridCell styles
            foreach (var row in _cellGrid)
            {
                for (int c = 0; c < row.Count && c < colPts.Length; c++)
                {
                    row[c].Style.Size.Width = new Unit(colPts[c], PageUnits.Points);
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
            out List<TrackDef> tracks,
            out List<List<GridCell>> cellGrid)
        {
            tracks = ParseTemplateCols(source);
            cellGrid = new List<List<GridCell>>();

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

            // Group into rows of colCount (auto-flow, row-major)
            for (int i = 0; i < items.Count; i += colCount)
            {
                var row = new TableRow();
                var rowCells = new List<GridCell>();

                for (int j = 0; j < colCount && (i + j) < items.Count; j++)
                {
                    var cell = new GridCell(items[i + j]);
                    row.Cells.Add(cell);
                    rowCells.Add(cell);
                }

                grid.Rows.Add(row);
                cellGrid.Add(rowCells);
            }

            return grid;
        }

        // -----------------------------------------------------------------------
        // grid-template-columns parser
        // -----------------------------------------------------------------------

        private static List<TrackDef> ParseTemplateCols(ContainerComponent source)
        {
            if (!(source is IStyledComponent sc) || !sc.HasStyle)
                return new List<TrackDef>();

            var raw = sc.Style.GetValue(StyleKeys.GridTemplateColumnsKey, null as string);
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
        // Proxy cell — wraps a display:grid child without moving its children
        // -----------------------------------------------------------------------

        internal sealed class GridCell : TableCell, IContainerComponent
        {
            private readonly ContainerComponent _source;

            public GridCell(Component source)
            {
                _source = source as ContainerComponent;
            }

            bool IContainerComponent.HasContent
                => _source?.HasContent ?? false;

            ComponentList IContainerComponent.Content
                => _source != null ? ((IContainerComponent)_source).Content : base.InnerContent;

            public override ComponentList Contents
                => _source != null ? ((IContainerComponent)_source).Content : base.Contents;

            protected override ComponentList InnerContent
                => _source != null ? ((IContainerComponent)_source).Content : base.InnerContent;
        }
    }
}
