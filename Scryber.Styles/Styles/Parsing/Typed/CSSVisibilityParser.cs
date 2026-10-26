using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the visibility css property - visible (default) or hidden (collapse is treated as hidden). 
    /// Hidden content still takes up its space in the layout, but is not drawn.
    /// </summary>
    public class CSSVisibilityParser : CSSStyleValueParser
    {
        public CSSVisibilityParser()
            : base(CSSStyleItems.Visibility)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var val = reader.CurrentTextValue;

            if (IsExpression(val))
                return AttachExpressionBindingHandler(onStyle, StyleKeys.TextVisibilityKey, val, DoConvertVisibility);

            if (TryParseVisibility(val, out var visibility))
            {
                onStyle.SetValue(StyleKeys.TextVisibilityKey, visibility);
                return true;
            }

            return false;
        }

        protected bool DoConvertVisibility(StyleBase style, object value, out Visibility visibility)
        {
            if (null == value)
            {
                visibility = Visibility.Visible;
                return false;
            }
            else if (value is Visibility v)
            {
                visibility = v;
                return true;
            }
            else
                return TryParseVisibility(value.ToString(), out visibility);
        }

        public static bool TryParseVisibility(string value, out Visibility visibility)
        {
            switch (value.Trim().ToLowerInvariant())
            {
                case "visible":
                    visibility = Visibility.Visible;
                    return true;
                case "hidden":
                case "collapse":
                    visibility = Visibility.Hidden;
                    return true;
                default:
                    visibility = Visibility.Visible;
                    return false;
            }
        }
    }
}
