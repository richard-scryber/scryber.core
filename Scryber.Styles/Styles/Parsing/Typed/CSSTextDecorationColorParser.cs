using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the text-decoration-color css property. The default (currentcolor) is the color of the text.
    /// </summary>
    public class CSSTextDecorationColorParser : CSSStyleValueParser
    {
        public CSSTextDecorationColorParser()
            : base(CSSStyleItems.TextDecorationColor)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var val = reader.CurrentTextValue;

            if (IsExpression(val))
                return AttachExpressionBindingHandler(onStyle, StyleKeys.TextDecorationColorKey, val, DoConvertColor);

            return TrySetColor(onStyle, val);
        }

        /// <summary>
        /// Sets the decoration color from the value, or removes any color for currentcolor. Returns false if it is not a color.
        /// </summary>
        public static bool TrySetColor(Style onStyle, string value)
        {
            if (string.Equals(value, "currentcolor", StringComparison.OrdinalIgnoreCase))
            {
                onStyle.RemoveValue(StyleKeys.TextDecorationColorKey);
                return true;
            }

            if (ParseCSSColor(value, out var color, out _))
            {
                onStyle.SetValue(StyleKeys.TextDecorationColorKey, color);
                return true;
            }

            return false;
        }

        protected bool DoConvertColor(StyleBase style, object value, out Color color)
        {
            color = Color.Transparent;
            if (null == value)
                return false;
            else if (value is Color c)
            {
                color = c;
                return true;
            }
            return ParseCSSColor(value.ToString(), out color, out _);
        }
    }
}
