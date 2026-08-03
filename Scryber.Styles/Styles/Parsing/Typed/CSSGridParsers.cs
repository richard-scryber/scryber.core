using System;
using System.Collections.Generic;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    // -----------------------------------------------------------------------
    // Shared helpers
    // -----------------------------------------------------------------------

    internal static class GridLineParser
    {
        // Splits reader tokens around the first '/' and returns left/right token lists.
        // "1 / span 2" → left=["1"], right=["span","2"]
        public static (List<string> left, List<string> right) ReadAndSplit(CSSStyleItemReader reader)
        {
            var all = new List<string>();
            while (reader.ReadNextValue())
                all.Add(reader.CurrentTextValue.Trim());

            var joined = string.Join(" ", all);
            var parts  = joined.Split(new[] {'/'}, 2);

            return (Tokenise(parts[0]),
                    parts.Length > 1 ? Tokenise(parts[1]) : new List<string>());
        }

        static List<string> Tokenise(string s)
            => new List<string>(s.Trim().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));

        // Returns true if the string looks like a CSS identifier (not a number / keyword).
        static bool IsIdentifier(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            char first = s[0];
            return char.IsLetter(first) || first == '-' || first == '_';
        }

        // Parse a token list into a single GridLineValue.
        // Handles: auto, N, span N, <name>, span <name>.
        public static GridLineValue Parse(List<string> tokens)
        {
            if (tokens.Count == 0)
                return GridLineValue.Auto;

            string t0 = tokens[0];

            if (t0.Equals("auto", StringComparison.OrdinalIgnoreCase))
                return GridLineValue.Auto;

            bool isSpanKeyword = t0.Equals("span", StringComparison.OrdinalIgnoreCase);

            if (isSpanKeyword && tokens.Count >= 2)
            {
                string t1 = tokens[1];
                // "span N"
                if (int.TryParse(t1, out int sn))
                    return GridLineValue.Span(Math.Max(1, sn));
                // "span <name>"
                if (IsIdentifier(t1))
                    return GridLineValue.Named(t1, span: true);
            }

            // Integer line number (positive or negative)
            if (tokens.Count == 1 && int.TryParse(t0, out int n) && n != 0)
                return GridLineValue.Line(n);

            // Named line reference
            if (tokens.Count == 1 && IsIdentifier(t0))
                return GridLineValue.Named(t0);

            return GridLineValue.Auto; // unrecognised — treat as auto
        }
    }

    // -----------------------------------------------------------------------
    // grid-column shorthand: grid-column: <start> [ / <end-or-span> ]
    // -----------------------------------------------------------------------

    public class CSSGridColumnParser : CSSStyleValueParser
    {
        public CSSGridColumnParser() : base(CSSStyleItems.GridColumn) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var (left, right) = GridLineParser.ReadAndSplit(reader);

            var startVal = GridLineParser.Parse(left);
            if (startVal.IsSet && !startVal.IsAuto)
                onStyle.SetValue(StyleKeys.GridColumnStartKey, startVal);

            if (right.Count > 0)
            {
                var endVal = GridLineParser.Parse(right);
                if (endVal.IsSet && !endVal.IsAuto)
                    onStyle.SetValue(StyleKeys.GridColumnEndKey, endVal);
            }

            return true;
        }
    }

    // -----------------------------------------------------------------------
    // grid-row shorthand: grid-row: <start> [ / <end-or-span> ]
    // -----------------------------------------------------------------------

    public class CSSGridRowParser : CSSStyleValueParser
    {
        public CSSGridRowParser() : base(CSSStyleItems.GridRow) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var (left, right) = GridLineParser.ReadAndSplit(reader);

            var startVal = GridLineParser.Parse(left);
            if (startVal.IsSet && !startVal.IsAuto)
                onStyle.SetValue(StyleKeys.GridRowStartKey, startVal);

            if (right.Count > 0)
            {
                var endVal = GridLineParser.Parse(right);
                if (endVal.IsSet && !endVal.IsAuto)
                    onStyle.SetValue(StyleKeys.GridRowEndKey, endVal);
            }

            return true;
        }
    }

    // -----------------------------------------------------------------------
    // grid-column-start / grid-row-start
    // -----------------------------------------------------------------------

    public class CSSGridColumnStartParser : CSSStyleValueParser
    {
        public CSSGridColumnStartParser() : base(CSSStyleItems.GridColumnStart) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var (left, _) = GridLineParser.ReadAndSplit(reader);
            var val = GridLineParser.Parse(left);
            if (val.IsSet && !val.IsAuto)
                onStyle.SetValue(StyleKeys.GridColumnStartKey, val);
            return true;
        }
    }

    public class CSSGridRowStartParser : CSSStyleValueParser
    {
        public CSSGridRowStartParser() : base(CSSStyleItems.GridRowStart) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var (left, _) = GridLineParser.ReadAndSplit(reader);
            var val = GridLineParser.Parse(left);
            if (val.IsSet && !val.IsAuto)
                onStyle.SetValue(StyleKeys.GridRowStartKey, val);
            return true;
        }
    }

    // -----------------------------------------------------------------------
    // grid-column-end / grid-row-end
    // -----------------------------------------------------------------------

    public class CSSGridColumnEndParser : CSSStyleValueParser
    {
        public CSSGridColumnEndParser() : base(CSSStyleItems.GridColumnEnd) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var (left, _) = GridLineParser.ReadAndSplit(reader);
            var val = GridLineParser.Parse(left);
            if (val.IsSet && !val.IsAuto)
                onStyle.SetValue(StyleKeys.GridColumnEndKey, val);
            return true;
        }
    }

    public class CSSGridRowEndParser : CSSStyleValueParser
    {
        public CSSGridRowEndParser() : base(CSSStyleItems.GridRowEnd) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var (left, _) = GridLineParser.ReadAndSplit(reader);
            var val = GridLineParser.Parse(left);
            if (val.IsSet && !val.IsAuto)
                onStyle.SetValue(StyleKeys.GridRowEndKey, val);
            return true;
        }
    }

    // -----------------------------------------------------------------------
    // grid-area shorthand
    // Single identifier  → grid-area: header  (named template area)
    // 4-part slash form  → grid-area: row-start / col-start / row-end / col-end
    // -----------------------------------------------------------------------

    public class CSSGridAreaParser : CSSStyleValueParser
    {
        public CSSGridAreaParser() : base(CSSStyleItems.GridArea) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var all = new List<string>();
            while (reader.ReadNextValue())
                all.Add(reader.CurrentTextValue.Trim());

            var joined = string.Join(" ", all);

            // Single token with no '/' → named area reference
            if (!joined.Contains('/'))
            {
                var tokens = new List<string>(joined.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));
                if (tokens.Count == 1)
                {
                    var val = GridLineParser.Parse(tokens);
                    if (val.IsNamed)
                    {
                        onStyle.SetValue(StyleKeys.GridAreaNameKey, val.Name);
                        return true;
                    }
                }
            }

            // 4-part form: row-start / col-start / row-end / col-end
            var parts = joined.Split(new[] {'/'}, 4);
            Apply(onStyle, parts, 0, StyleKeys.GridRowStartKey);
            Apply(onStyle, parts, 1, StyleKeys.GridColumnStartKey);
            Apply(onStyle, parts, 2, StyleKeys.GridRowEndKey);
            Apply(onStyle, parts, 3, StyleKeys.GridColumnEndKey);
            return true;
        }

        private static void Apply(Style onStyle, string[] parts, int index,
                                   StyleKey<GridLineValue> key)
        {
            if (index >= parts.Length) return;
            var tokens = new List<string>(
                parts[index].Trim().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));
            var val = GridLineParser.Parse(tokens);
            if (val.IsSet && !val.IsAuto)
                onStyle.SetValue(key, val);
        }
    }

    // -----------------------------------------------------------------------
    // grid-template-areas  — stores the parsed GridTemplateAreasValue
    // -----------------------------------------------------------------------

    public class CSSGridTemplateAreasParser : CSSStyleValueParser
    {
        public CSSGridTemplateAreasParser() : base(CSSStyleItems.GridTemplateAreas) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            // Collect all tokens (the CSS reader preserves quoted strings with their quotes)
            var sb = new System.Text.StringBuilder();
            while (reader.ReadNextValue())
            {
                if (sb.Length > 0) sb.Append(' ');
                sb.Append(reader.CurrentTextValue);
            }
            var raw = sb.ToString().Trim();
            if (string.IsNullOrWhiteSpace(raw)) return false;

            if (GridTemplateAreasValue.TryParse(raw, out var areas))
            {
                onStyle.SetValue(StyleKeys.GridTemplateAreasKey, areas);
                return true;
            }
            return false;
        }
    }

    // -----------------------------------------------------------------------
    // grid-template-columns / grid-template-rows  (unchanged — raw string storage)
    // -----------------------------------------------------------------------

    public class CSSGridTemplateColumnsParser : CSSStyleAttributeParser<string>
    {
        public CSSGridTemplateColumnsParser() : base(CSSStyleItems.GridTemplateColumns, StyleKeys.GridTemplateColumnsKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var sb = new System.Text.StringBuilder();
            while (reader.ReadNextValue())
            {
                if (sb.Length > 0) sb.Append(' ');
                sb.Append(reader.CurrentTextValue);
            }
            var raw = sb.ToString().Trim();
            if (string.IsNullOrWhiteSpace(raw)) return false;
            this.SetValue(onStyle, raw);
            return true;
        }
    }

    public class CSSGridTemplateRowsParser : CSSStyleAttributeParser<string>
    {
        public CSSGridTemplateRowsParser() : base(CSSStyleItems.GridTemplateRows, StyleKeys.GridTemplateRowsKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var sb = new System.Text.StringBuilder();
            while (reader.ReadNextValue())
            {
                if (sb.Length > 0) sb.Append(' ');
                sb.Append(reader.CurrentTextValue);
            }
            var raw = sb.ToString().Trim();
            if (string.IsNullOrWhiteSpace(raw)) return false;
            this.SetValue(onStyle, raw);
            return true;
        }
    }

    // -----------------------------------------------------------------------
    // grid-auto-columns / grid-auto-rows  — single track-size value
    // -----------------------------------------------------------------------

    public class CSSGridAutoColumnsParser : CSSStyleAttributeParser<string>
    {
        public CSSGridAutoColumnsParser() : base(CSSStyleItems.GridAutoColumns, StyleKeys.GridAutoColumnsKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var sb = new System.Text.StringBuilder();
            while (reader.ReadNextValue())
            {
                if (sb.Length > 0) sb.Append(' ');
                sb.Append(reader.CurrentTextValue);
            }
            var raw = sb.ToString().Trim();
            if (string.IsNullOrWhiteSpace(raw)) return false;
            this.SetValue(onStyle, raw);
            return true;
        }
    }

    public class CSSGridAutoRowsParser : CSSStyleAttributeParser<string>
    {
        public CSSGridAutoRowsParser() : base(CSSStyleItems.GridAutoRows, StyleKeys.GridAutoRowsKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var sb = new System.Text.StringBuilder();
            while (reader.ReadNextValue())
            {
                if (sb.Length > 0) sb.Append(' ');
                sb.Append(reader.CurrentTextValue);
            }
            var raw = sb.ToString().Trim();
            if (string.IsNullOrWhiteSpace(raw)) return false;
            this.SetValue(onStyle, raw);
            return true;
        }
    }

    // -----------------------------------------------------------------------
    // grid-auto-flow  (unchanged)
    // -----------------------------------------------------------------------

    public class CSSGridAutoFlowParser : CSSStyleAttributeParser<GridAutoFlow>
    {
        public CSSGridAutoFlowParser() : base(CSSStyleItems.GridAutoFlow, StyleKeys.GridAutoFlowKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue()) return false;
            GridAutoFlow flow;
            switch (reader.CurrentTextValue.ToLowerInvariant())
            {
                case "column": flow = GridAutoFlow.Column; break;
                case "row":    flow = GridAutoFlow.Row;    break;
                default:       return false;
            }
            this.SetValue(onStyle, flow);
            return true;
        }
    }
}
