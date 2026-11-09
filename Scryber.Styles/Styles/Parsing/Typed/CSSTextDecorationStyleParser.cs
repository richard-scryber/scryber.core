using System;
using Scryber.Html;
using Scryber.Text;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the text-decoration-style css property - solid (default), double, dotted, dashed or wavy
    /// </summary>
    public class CSSTextDecorationStyleParser : CSSStyleValueParser
    {
        public CSSTextDecorationStyleParser()
            : base(CSSStyleItems.TextDecorationStyle)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var val = reader.CurrentTextValue;

            if (IsExpression(val))
                return AttachExpressionBindingHandler(onStyle, StyleKeys.TextDecorationStyleKey, val, DoConvertStyle);

            if (TryParseStyle(val, out var style))
            {
                onStyle.SetValue(StyleKeys.TextDecorationStyleKey, style);
                return true;
            }

            return false;
        }

        protected bool DoConvertStyle(StyleBase style, object value, out TextDecorationStyle result)
        {
            if (null == value)
            {
                result = TextDecorationStyle.Solid;
                return false;
            }
            else if (value is TextDecorationStyle s)
            {
                result = s;
                return true;
            }
            return TryParseStyle(value.ToString(), out result);
        }

        public static bool TryParseStyle(string value, out TextDecorationStyle style)
        {
            switch (value.Trim().ToLowerInvariant())
            {
                case "solid": style = TextDecorationStyle.Solid; return true;
                case "double": style = TextDecorationStyle.Double; return true;
                case "dotted": style = TextDecorationStyle.Dotted; return true;
                case "dashed": style = TextDecorationStyle.Dashed; return true;
                case "wavy": style = TextDecorationStyle.Wavy; return true;
                default:
                    style = TextDecorationStyle.Solid;
                    return false;
            }
        }
    }
}
