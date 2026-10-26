using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the box-sizing css property - content-box (default) or border-box
    /// </summary>
    public class CSSBoxSizingParser : CSSStyleValueParser
    {
        public CSSBoxSizingParser()
            : base(CSSStyleItems.BoxSizing)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var val = reader.CurrentTextValue;

            if (IsExpression(val))
                return AttachExpressionBindingHandler(onStyle, StyleKeys.SizeBoxSizingKey, val, DoConvertBoxSizing);

            if (TryParseBoxSizing(val, out var sizing))
            {
                onStyle.SetValue(StyleKeys.SizeBoxSizingKey, sizing);
                return true;
            }

            return false;
        }

        protected bool DoConvertBoxSizing(StyleBase style, object value, out BoxSizing sizing)
        {
            if (null == value)
            {
                sizing = BoxSizing.ContentBox;
                return false;
            }
            else if (value is BoxSizing b)
            {
                sizing = b;
                return true;
            }
            else
                return TryParseBoxSizing(value.ToString(), out sizing);
        }

        public static bool TryParseBoxSizing(string value, out BoxSizing sizing)
        {
            switch (value.Trim().ToLowerInvariant())
            {
                case "content-box":
                    sizing = BoxSizing.ContentBox;
                    return true;
                case "border-box":
                    sizing = BoxSizing.BorderBox;
                    return true;
                default:
                    sizing = BoxSizing.ContentBox;
                    return false;
            }
        }
    }
}
