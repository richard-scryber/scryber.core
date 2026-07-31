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

        // Parses a list of tokens as a single grid-line value.
        // Returns isAuto=true when the value is unset/auto.
        // isSpan=true  → value is a span count (no explicit line number)
        // isSpan=false → value is a line number (positive or negative)
        public static bool TryParse(List<string> tokens,
                                    out int value, out bool isSpan, out bool isAuto)
        {
            value = 0; isSpan = false; isAuto = true;

            if (tokens.Count == 0) return true; // auto

            if (tokens[0].Equals("auto", StringComparison.OrdinalIgnoreCase)) return true;

            // "span N"
            if (tokens.Count >= 2
                && tokens[0].Equals("span", StringComparison.OrdinalIgnoreCase)
                && int.TryParse(tokens[1], out int s))
            {
                value = Math.Max(1, s);
                isSpan = true;
                isAuto = false;
                return true;
            }

            // Integer (positive or negative)
            if (tokens.Count == 1 && int.TryParse(tokens[0], out int n) && n != 0)
            {
                value  = n;
                isAuto = false;
                return true;
            }

            return false; // unrecognised
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

            if (!GridLineParser.TryParse(left, out int startVal, out bool startIsSpan, out bool startIsAuto))
                return false;

            // Left side
            if (!startIsAuto)
            {
                if (startIsSpan)
                    onStyle.SetValue(StyleKeys.GridColumnSpanKey, startVal);
                else
                    onStyle.SetValue(StyleKeys.GridColumnStartKey, startVal);
            }

            // Right side (optional)
            if (right.Count > 0)
            {
                if (!GridLineParser.TryParse(right, out int endVal, out bool endIsSpan, out bool endIsAuto))
                    return false;

                if (!endIsAuto)
                {
                    if (endIsSpan)
                        onStyle.SetValue(StyleKeys.GridColumnSpanKey, endVal);
                    else
                        onStyle.SetValue(StyleKeys.GridColumnEndKey, endVal);
                }
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

            if (!GridLineParser.TryParse(left, out int startVal, out bool startIsSpan, out bool startIsAuto))
                return false;

            if (!startIsAuto)
            {
                if (startIsSpan)
                    onStyle.SetValue(StyleKeys.GridRowSpanKey, startVal);
                else
                    onStyle.SetValue(StyleKeys.GridRowStartKey, startVal);
            }

            if (right.Count > 0)
            {
                if (!GridLineParser.TryParse(right, out int endVal, out bool endIsSpan, out bool endIsAuto))
                    return false;

                if (!endIsAuto)
                {
                    if (endIsSpan)
                        onStyle.SetValue(StyleKeys.GridRowSpanKey, endVal);
                    else
                        onStyle.SetValue(StyleKeys.GridRowEndKey, endVal);
                }
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
            if (!GridLineParser.TryParse(left, out int val, out bool isSpan, out bool isAuto))
                return false;
            if (!isAuto)
            {
                if (isSpan) onStyle.SetValue(StyleKeys.GridColumnSpanKey, val);
                else        onStyle.SetValue(StyleKeys.GridColumnStartKey, val);
            }
            return true;
        }
    }

    public class CSSGridRowStartParser : CSSStyleValueParser
    {
        public CSSGridRowStartParser() : base(CSSStyleItems.GridRowStart) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var (left, _) = GridLineParser.ReadAndSplit(reader);
            if (!GridLineParser.TryParse(left, out int val, out bool isSpan, out bool isAuto))
                return false;
            if (!isAuto)
            {
                if (isSpan) onStyle.SetValue(StyleKeys.GridRowSpanKey, val);
                else        onStyle.SetValue(StyleKeys.GridRowStartKey, val);
            }
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
            if (!GridLineParser.TryParse(left, out int val, out bool isSpan, out bool isAuto))
                return false;
            if (!isAuto)
            {
                if (isSpan) onStyle.SetValue(StyleKeys.GridColumnSpanKey, val);
                else        onStyle.SetValue(StyleKeys.GridColumnEndKey,   val);
            }
            return true;
        }
    }

    public class CSSGridRowEndParser : CSSStyleValueParser
    {
        public CSSGridRowEndParser() : base(CSSStyleItems.GridRowEnd) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var (left, _) = GridLineParser.ReadAndSplit(reader);
            if (!GridLineParser.TryParse(left, out int val, out bool isSpan, out bool isAuto))
                return false;
            if (!isAuto)
            {
                if (isSpan) onStyle.SetValue(StyleKeys.GridRowSpanKey, val);
                else        onStyle.SetValue(StyleKeys.GridRowEndKey,   val);
            }
            return true;
        }
    }

    // -----------------------------------------------------------------------
    // grid-area shorthand: row-start / col-start / row-end / col-end
    // Named areas are not supported; only line numbers and span values.
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
            var parts  = joined.Split(new[] { '/' }, 4);

            // Order: row-start / col-start / row-end / col-end
            Apply(onStyle, parts, 0, StyleKeys.GridRowStartKey,    StyleKeys.GridRowSpanKey);
            Apply(onStyle, parts, 1, StyleKeys.GridColumnStartKey, StyleKeys.GridColumnSpanKey);
            Apply(onStyle, parts, 2, StyleKeys.GridRowEndKey,      StyleKeys.GridRowSpanKey);
            Apply(onStyle, parts, 3, StyleKeys.GridColumnEndKey,   StyleKeys.GridColumnSpanKey);
            return true;
        }

        private static void Apply(Style onStyle, string[] parts, int index,
                                   StyleKey<int> lineKey, StyleKey<int> spanKey)
        {
            if (index >= parts.Length) return;
            var tokens = new List<string>(
                parts[index].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            if (!GridLineParser.TryParse(tokens, out int val, out bool isSpan, out bool isAuto))
                return;
            if (!isAuto)
                onStyle.SetValue(isSpan ? spanKey : lineKey, val);
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
