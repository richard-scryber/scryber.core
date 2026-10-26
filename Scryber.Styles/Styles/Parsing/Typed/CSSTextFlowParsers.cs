using System;
using Scryber.Html;
using Scryber.Text;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the word-break css property - normal, break-all, keep-all and break-word
    /// </summary>
    public class CSSWordBreakParser : CSSStyleValueParser
    {
        public CSSWordBreakParser()
            : base(CSSStyleItems.WordBreak)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var val = reader.CurrentTextValue;
            if (IsExpression(val))
                return AttachExpressionBindingHandler(onStyle, StyleKeys.TextWordBreakKey, val, DoConvert);

            if (TryParse(val, out var result))
            {
                onStyle.SetValue(StyleKeys.TextWordBreakKey, result);
                return true;
            }
            return false;
        }

        protected bool DoConvert(StyleBase style, object value, out WordBreak result)
        {
            result = WordBreak.Normal;
            if (null == value)
                return false;
            else if (value is WordBreak w)
            {
                result = w;
                return true;
            }
            return TryParse(value.ToString(), out result);
        }

        public static bool TryParse(string value, out WordBreak result)
        {
            switch (value.Trim().ToLowerInvariant())
            {
                case "normal": result = WordBreak.Normal; return true;
                case "break-all": result = WordBreak.BreakAll; return true;
                case "keep-all": result = WordBreak.KeepAll; return true;
                case "break-word": result = WordBreak.BreakWord; return true;
                default:
                    result = WordBreak.Normal;
                    return false;
            }
        }
    }

    /// <summary>
    /// Parses the overflow-wrap css property (and its original name word-wrap) - normal, break-word and anywhere
    /// </summary>
    public class CSSOverflowWrapParser : CSSStyleValueParser
    {
        public CSSOverflowWrapParser(string item)
            : base(item)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var val = reader.CurrentTextValue;
            if (IsExpression(val))
                return AttachExpressionBindingHandler(onStyle, StyleKeys.TextOverflowWrapKey, val, DoConvert);

            if (TryParse(val, out var result))
            {
                onStyle.SetValue(StyleKeys.TextOverflowWrapKey, result);
                return true;
            }
            return false;
        }

        protected bool DoConvert(StyleBase style, object value, out OverflowWrap result)
        {
            result = OverflowWrap.Normal;
            if (null == value)
                return false;
            else if (value is OverflowWrap w)
            {
                result = w;
                return true;
            }
            return TryParse(value.ToString(), out result);
        }

        public static bool TryParse(string value, out OverflowWrap result)
        {
            switch (value.Trim().ToLowerInvariant())
            {
                case "normal": result = OverflowWrap.Normal; return true;
                case "break-word": result = OverflowWrap.BreakWord; return true;
                case "anywhere": result = OverflowWrap.Anywhere; return true;
                default:
                    result = OverflowWrap.Normal;
                    return false;
            }
        }
    }

    /// <summary>
    /// Parses the text-overflow css property - clip (default), ellipsis, or a quoted string to show where the text is clipped.
    /// </summary>
    public class CSSTextOverflowParser : CSSStyleValueParser
    {
        public const string Ellipsis = "\u2026";

        public CSSTextOverflowParser()
            : base(CSSStyleItems.TextOverflow)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var val = reader.CurrentTextValue;

            //Only a single value is supported
            if (reader.ReadNextValue())
                return false;

            if (IsExpression(val))
                return AttachExpressionBindingHandler(onStyle, StyleKeys.TextOverflowTextKey, val, DoConvert);

            if (TryParse(val, out var text))
            {
                if (null == text)
                    onStyle.RemoveValue(StyleKeys.TextOverflowTextKey);
                else
                    onStyle.SetValue(StyleKeys.TextOverflowTextKey, text);
                return true;
            }
            return false;
        }

        protected bool DoConvert(StyleBase style, object value, out string result)
        {
            result = string.Empty;
            if (null == value)
                return false;

            if (!TryParse(value.ToString(), out var parsed))
                return false;

            //clip is an empty string so nothing is added
            result = parsed ?? string.Empty;
            return true;
        }

        /// <summary>
        /// Parses the value, setting the text to null for clip, otherwise the text to add where the line is clipped.
        /// </summary>
        public static bool TryParse(string value, out string text)
        {
            text = null;
            value = value.Trim();

            if (string.Equals(value, "clip", StringComparison.OrdinalIgnoreCase))
                return true;

            if (string.Equals(value, "ellipsis", StringComparison.OrdinalIgnoreCase))
            {
                text = Ellipsis;
                return true;
            }

            if (value.Length >= 2 && (value[0] == '\'' || value[0] == '"') && value[value.Length - 1] == value[0])
            {
                text = value.Substring(1, value.Length - 2);
                return text.Length > 0;
            }

            return false;
        }
    }
}
