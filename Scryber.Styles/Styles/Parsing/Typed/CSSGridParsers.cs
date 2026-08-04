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

            return SplitJoined(string.Join(" ", all));
        }

        // Same split, but operating on an already-resolved string (e.g. the output of
        // evaluating a var()/calc() expression) rather than reading from a CSS reader.
        public static (List<string> left, List<string> right) SplitJoined(string joined)
        {
            var parts = (joined ?? string.Empty).Split(new[] {'/'}, 2);

            return (Tokenise(parts[0]),
                    parts.Length > 1 ? Tokenise(parts[1]) : new List<string>());
        }

        internal static List<string> Tokenise(string s)
            => new List<string>((s ?? string.Empty).Trim().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));

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
            var all = new List<string>();
            while (reader.ReadNextValue())
                all.Add(reader.CurrentTextValue.Trim());

            if (all.Count == 1 && IsExpression(all[0]))
            {
                // Whole value is a single var()/calc() expression (e.g. resolves to "1 / 3") -
                // mirror it onto both start and end keys, each extracting its own side at
                // data-bind time.
                bool r1 = this.AttachExpressionBindingHandler(onStyle, StyleKeys.GridColumnStartKey, all[0], this.DoConvertStart);
                bool r2 = this.AttachExpressionBindingHandler(onStyle, StyleKeys.GridColumnEndKey, all[0], this.DoConvertEnd);
                return r1 || r2;
            }

            var (left, right) = GridLineParser.SplitJoined(string.Join(" ", all));

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

        private bool DoConvertStart(StyleBase onStyle, object value, out GridLineValue result)
        {
            var (left, _) = GridLineParser.SplitJoined(value?.ToString());
            result = GridLineParser.Parse(left);
            return result.IsSet;
        }

        private bool DoConvertEnd(StyleBase onStyle, object value, out GridLineValue result)
        {
            var (_, right) = GridLineParser.SplitJoined(value?.ToString());
            result = right.Count > 0 ? GridLineParser.Parse(right) : GridLineValue.Unset;
            return result.IsSet;
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
            var all = new List<string>();
            while (reader.ReadNextValue())
                all.Add(reader.CurrentTextValue.Trim());

            if (all.Count == 1 && IsExpression(all[0]))
            {
                bool r1 = this.AttachExpressionBindingHandler(onStyle, StyleKeys.GridRowStartKey, all[0], this.DoConvertStart);
                bool r2 = this.AttachExpressionBindingHandler(onStyle, StyleKeys.GridRowEndKey, all[0], this.DoConvertEnd);
                return r1 || r2;
            }

            var (left, right) = GridLineParser.SplitJoined(string.Join(" ", all));

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

        private bool DoConvertStart(StyleBase onStyle, object value, out GridLineValue result)
        {
            var (left, _) = GridLineParser.SplitJoined(value?.ToString());
            result = GridLineParser.Parse(left);
            return result.IsSet;
        }

        private bool DoConvertEnd(StyleBase onStyle, object value, out GridLineValue result)
        {
            var (_, right) = GridLineParser.SplitJoined(value?.ToString());
            result = right.Count > 0 ? GridLineParser.Parse(right) : GridLineValue.Unset;
            return result.IsSet;
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
            var all = new List<string>();
            while (reader.ReadNextValue())
                all.Add(reader.CurrentTextValue.Trim());

            if (all.Count == 1 && IsExpression(all[0]))
                return this.AttachExpressionBindingHandler(onStyle, StyleKeys.GridColumnStartKey, all[0], this.DoConvertLine);

            var (left, _) = GridLineParser.SplitJoined(string.Join(" ", all));
            var val = GridLineParser.Parse(left);
            if (val.IsSet && !val.IsAuto)
                onStyle.SetValue(StyleKeys.GridColumnStartKey, val);
            return true;
        }

        private bool DoConvertLine(StyleBase onStyle, object value, out GridLineValue result)
        {
            result = GridLineParser.Parse(GridLineParser.Tokenise(value?.ToString()));
            return result.IsSet;
        }
    }

    public class CSSGridRowStartParser : CSSStyleValueParser
    {
        public CSSGridRowStartParser() : base(CSSStyleItems.GridRowStart) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var all = new List<string>();
            while (reader.ReadNextValue())
                all.Add(reader.CurrentTextValue.Trim());

            if (all.Count == 1 && IsExpression(all[0]))
                return this.AttachExpressionBindingHandler(onStyle, StyleKeys.GridRowStartKey, all[0], this.DoConvertLine);

            var (left, _) = GridLineParser.SplitJoined(string.Join(" ", all));
            var val = GridLineParser.Parse(left);
            if (val.IsSet && !val.IsAuto)
                onStyle.SetValue(StyleKeys.GridRowStartKey, val);
            return true;
        }

        private bool DoConvertLine(StyleBase onStyle, object value, out GridLineValue result)
        {
            result = GridLineParser.Parse(GridLineParser.Tokenise(value?.ToString()));
            return result.IsSet;
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
            var all = new List<string>();
            while (reader.ReadNextValue())
                all.Add(reader.CurrentTextValue.Trim());

            if (all.Count == 1 && IsExpression(all[0]))
                return this.AttachExpressionBindingHandler(onStyle, StyleKeys.GridColumnEndKey, all[0], this.DoConvertLine);

            var (left, _) = GridLineParser.SplitJoined(string.Join(" ", all));
            var val = GridLineParser.Parse(left);
            if (val.IsSet && !val.IsAuto)
                onStyle.SetValue(StyleKeys.GridColumnEndKey, val);
            return true;
        }

        private bool DoConvertLine(StyleBase onStyle, object value, out GridLineValue result)
        {
            result = GridLineParser.Parse(GridLineParser.Tokenise(value?.ToString()));
            return result.IsSet;
        }
    }

    public class CSSGridRowEndParser : CSSStyleValueParser
    {
        public CSSGridRowEndParser() : base(CSSStyleItems.GridRowEnd) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var all = new List<string>();
            while (reader.ReadNextValue())
                all.Add(reader.CurrentTextValue.Trim());

            if (all.Count == 1 && IsExpression(all[0]))
                return this.AttachExpressionBindingHandler(onStyle, StyleKeys.GridRowEndKey, all[0], this.DoConvertLine);

            var (left, _) = GridLineParser.SplitJoined(string.Join(" ", all));
            var val = GridLineParser.Parse(left);
            if (val.IsSet && !val.IsAuto)
                onStyle.SetValue(StyleKeys.GridRowEndKey, val);
            return true;
        }

        private bool DoConvertLine(StyleBase onStyle, object value, out GridLineValue result)
        {
            result = GridLineParser.Parse(GridLineParser.Tokenise(value?.ToString()));
            return result.IsSet;
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

            if (all.Count == 1 && IsExpression(all[0]))
            {
                // Whole grid-area value is a single var()/calc() expression. Only the simple
                // named-area form is supported here (grid-area: var(--name)) - the ambiguous
                // 4-part "row/col/row/col" shorthand-via-expression form is not.
                return this.AttachExpressionBindingHandler(onStyle, StyleKeys.GridAreaNameKey, all[0], this.DoConvertName);
            }

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

        private bool DoConvertName(StyleBase onStyle, object value, out string result)
        {
            result = value?.ToString();
            return !string.IsNullOrEmpty(result);
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
            var tokens = new List<string>();
            var sb = new System.Text.StringBuilder();
            while (reader.ReadNextValue())
            {
                tokens.Add(reader.CurrentTextValue);
                if (sb.Length > 0) sb.Append(' ');
                sb.Append(reader.CurrentTextValue);
            }
            var raw = sb.ToString().Trim();
            if (string.IsNullOrWhiteSpace(raw)) return false;

            if (tokens.Count == 1 && IsExpression(tokens[0]))
                return this.AttachExpressionBindingHandler(onStyle, StyleKeys.GridTemplateAreasKey, tokens[0], this.DoConvertAreas);

            if (GridTemplateAreasValue.TryParse(raw, out var areas))
            {
                onStyle.SetValue(StyleKeys.GridTemplateAreasKey, areas);
                return true;
            }
            return false;
        }

        private bool DoConvertAreas(StyleBase onStyle, object value, out GridTemplateAreasValue result)
        {
            var text = value?.ToString() ?? string.Empty;
            if (GridTemplateAreasValue.TryParse(text, out result))
                return true;
            result = default;
            return false;
        }
    }

    // -----------------------------------------------------------------------
    // grid-template-columns / grid-template-rows / grid-auto-columns / grid-auto-rows
    // Raw string storage (track sizes are resolved at layout time). All support a
    // whole-value var()/calc() expression - the individual track sizes inside a
    // repeat()/track-list (e.g. "repeat(3, var(--w))") are not evaluated as expressions,
    // since those are resolved at layout time, not CSS parse time.
    // -----------------------------------------------------------------------

    public abstract class CSSGridTrackStringParser : CSSStyleAttributeParser<string>
    {
        protected CSSGridTrackStringParser(string cssName, StyleKey<string> key) : base(cssName, key) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var tokens = new List<string>();
            var sb = new System.Text.StringBuilder();
            while (reader.ReadNextValue())
            {
                tokens.Add(reader.CurrentTextValue);
                if (sb.Length > 0) sb.Append(' ');
                sb.Append(reader.CurrentTextValue);
            }
            var raw = sb.ToString().Trim();
            if (string.IsNullOrWhiteSpace(raw)) return false;

            // subgrid is not supported - the value would otherwise be stored and silently
            // misinterpreted as an (invalid) track list at layout time. Rejecting it here
            // means the standard CSSStyleValueParser.SetStyleValue path logs a trace warning
            // automatically (it does so for any parser that returns false), without us having
            // to reach for a TraceLog ourselves. Revisit if subgrid support is ever requested -
            // it would need each track to inherit its size from the parent grid's own tracks,
            // which this engine's track model does not currently represent.
            if (tokens.Count > 0 && tokens[0].Equals("subgrid", StringComparison.OrdinalIgnoreCase))
                return false;

            if (tokens.Count == 1 && IsExpression(tokens[0]))
                return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, tokens[0], this.DoConvertString);

            this.SetValue(onStyle, raw);
            return true;
        }

        protected virtual bool DoConvertString(StyleBase onStyle, object value, out string result)
        {
            result = value?.ToString();
            return !string.IsNullOrEmpty(result);
        }
    }

    public class CSSGridTemplateColumnsParser : CSSGridTrackStringParser
    {
        public CSSGridTemplateColumnsParser() : base(CSSStyleItems.GridTemplateColumns, StyleKeys.GridTemplateColumnsKey) { }
    }

    public class CSSGridTemplateRowsParser : CSSGridTrackStringParser
    {
        public CSSGridTemplateRowsParser() : base(CSSStyleItems.GridTemplateRows, StyleKeys.GridTemplateRowsKey) { }
    }

    public class CSSGridAutoColumnsParser : CSSGridTrackStringParser
    {
        public CSSGridAutoColumnsParser() : base(CSSStyleItems.GridAutoColumns, StyleKeys.GridAutoColumnsKey) { }
    }

    public class CSSGridAutoRowsParser : CSSGridTrackStringParser
    {
        public CSSGridAutoRowsParser() : base(CSSStyleItems.GridAutoRows, StyleKeys.GridAutoRowsKey) { }
    }

    // -----------------------------------------------------------------------
    // grid-auto-flow
    // -----------------------------------------------------------------------

    public class CSSGridAutoFlowParser : CSSStyleAttributeParser<GridAutoFlow>
    {
        public CSSGridAutoFlowParser() : base(CSSStyleItems.GridAutoFlow, StyleKeys.GridAutoFlowKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue()) return false;

            var text = reader.CurrentTextValue;
            if (IsExpression(text))
                return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, text, this.DoConvertFlow);

            if (TryGetFlow(text, out var flow))
            {
                this.SetValue(onStyle, flow);
                return true;
            }
            return false;
        }

        protected virtual bool DoConvertFlow(StyleBase onStyle, object value, out GridAutoFlow result)
            => TryGetFlow(value?.ToString() ?? string.Empty, out result);

        private static bool TryGetFlow(string value, out GridAutoFlow flow)
        {
            switch (value.ToLowerInvariant())
            {
                case "column": flow = GridAutoFlow.Column; return true;
                case "row":    flow = GridAutoFlow.Row;    return true;
                default:       flow = GridAutoFlow.Row;    return false;
            }
        }
    }
}
