using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the text-decoration-thickness css property - auto, from-font or a length. Auto (and from-font) is based on the font size.
    /// </summary>
    public class CSSTextDecorationThicknessParser : CSSUnitStyleParser
    {
        public CSSTextDecorationThicknessParser()
            : base(CSSStyleItems.TextDecorationThickness, StyleKeys.TextDecorationThicknessKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var val = reader.CurrentTextValue;

            if (IsExpression(val))
                return AttachExpressionBindingHandler(onStyle, StyleKeys.TextDecorationThicknessKey, val, this.DoConvertUnit);

            return TrySetThickness(onStyle, val);
        }

        /// <summary>
        /// Sets the thickness, or removes it for auto and from-font. Returns false if it is not a valid length.
        /// </summary>
        public static bool TrySetThickness(Style onStyle, string value)
        {
            if (string.Equals(value, "auto", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "from-font", StringComparison.OrdinalIgnoreCase))
            {
                onStyle.RemoveValue(StyleKeys.TextDecorationThicknessKey);
                return true;
            }

            if (ParseCSSUnit(value, out var unit))
            {
                onStyle.SetValue(StyleKeys.TextDecorationThicknessKey, unit);
                return true;
            }

            return false;
        }
    }
}
