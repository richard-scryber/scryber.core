using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Base for grid-column and grid-row parsers.
    /// Supports: span N, N, N / span M, N / M.
    /// </summary>
    public abstract class CSSGridSpanParserBase<TKey> : CSSStyleAttributeParser<int>
        where TKey : class
    {
        protected CSSGridSpanParserBase(string cssName, StyleKey<int> key)
            : base(cssName, key) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            // Collect all tokens (the value may be "span 2" or "1 / span 3" etc.)
            var tokens = new System.Collections.Generic.List<string>();
            while (reader.ReadNextValue())
                tokens.Add(reader.CurrentTextValue.Trim());

            int span = ParseSpan(tokens);
            if (span < 1) return false;

            this.SetValue(onStyle, span);
            return true;
        }

        private static int ParseSpan(System.Collections.Generic.List<string> tokens)
        {
            if (tokens.Count == 0) return -1;

            // "span N" — single span keyword
            if (tokens.Count >= 2
                && string.Equals(tokens[0], "span", System.StringComparison.OrdinalIgnoreCase)
                && int.TryParse(tokens[1], out int s1))
                return s1;

            // Single integer — treat as column-start only (span = 1)
            if (tokens.Count == 1 && int.TryParse(tokens[0], out int start))
                return 1;

            // "N / span M" or "N / M"
            // The reader tokenises on whitespace; "/" ends up as its own token or joined.
            // Normalise: re-join and split on "/"
            var joined = string.Join(" ", tokens);
            var parts = joined.Split('/');
            if (parts.Length == 2)
            {
                var right = parts[1].Trim();
                var rightTokens = right.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

                // "span M"
                if (rightTokens.Length >= 2
                    && string.Equals(rightTokens[0], "span", System.StringComparison.OrdinalIgnoreCase)
                    && int.TryParse(rightTokens[1], out int s2))
                    return s2;

                // "M" (end line)
                if (rightTokens.Length == 1 && int.TryParse(rightTokens[0], out int endLine))
                {
                    var left = parts[0].Trim();
                    var leftTokens = left.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                    if (leftTokens.Length == 1 && int.TryParse(leftTokens[0], out int startLine))
                        return System.Math.Max(1, endLine - startLine);
                    return 1;
                }
            }

            return -1;
        }
    }

    /// <summary>Parses grid-column: sets GridColumnSpanKey.</summary>
    public class CSSGridColumnParser : CSSGridSpanParserBase<object>
    {
        public CSSGridColumnParser() : base(CSSStyleItems.GridColumn, StyleKeys.GridColumnSpanKey) { }
    }

    /// <summary>Parses grid-row: sets GridRowSpanKey.</summary>
    public class CSSGridRowParser : CSSGridSpanParserBase<object>
    {
        public CSSGridRowParser() : base(CSSStyleItems.GridRow, StyleKeys.GridRowSpanKey) { }
    }

    /// <summary>Stores the raw grid-template-columns string value for later parsing by the layout engine.</summary>
    public class CSSGridTemplateColumnsParser : CSSStyleAttributeParser<string>
    {
        public CSSGridTemplateColumnsParser() : base(CSSStyleItems.GridTemplateColumns, StyleKeys.GridTemplateColumnsKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            // Read each space-separated token and re-join — handles "1fr 2fr", "repeat(3, 1fr)", etc.
            var sb = new System.Text.StringBuilder();
            while (reader.ReadNextValue())
            {
                if (sb.Length > 0) sb.Append(' ');
                sb.Append(reader.CurrentTextValue);
            }
            var raw = sb.ToString().Trim();
            if (string.IsNullOrWhiteSpace(raw))
                return false;

            this.SetValue(onStyle, raw);
            return true;
        }
    }

    /// <summary>Parses grid-auto-flow: row | column.</summary>
    public class CSSGridAutoFlowParser : CSSStyleAttributeParser<GridAutoFlow>
    {
        public CSSGridAutoFlowParser() : base(CSSStyleItems.GridAutoFlow, StyleKeys.GridAutoFlowKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

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

    /// <summary>Stores the raw grid-template-rows string value.</summary>
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
            if (string.IsNullOrWhiteSpace(raw))
                return false;

            this.SetValue(onStyle, raw);
            return true;
        }
    }
}
