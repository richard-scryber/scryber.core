using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the outline shorthand css property - the width, style and color in any order.
    /// </summary>
    public class CSSOutlineParser : CSSStyleValueParser
    {
        public CSSOutlineParser()
            : base(CSSStyleItems.Outline)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            int count = 0;
            int failed = 0;

            while (reader.ReadNextValue())
            {
                count++;
                var val = reader.CurrentTextValue;

                if (IsExpression(val))
                {
                    //an expression is for the width, style or color, in the order of the values
                    if (count == 1)
                    {
                        if (!AttachExpressionBindingHandler(onStyle, StyleKeys.BorderOutlineWidthKey, val, CSSOutlineWidthParser.ConvertWidth))
                            failed++;
                    }
                    else if (count == 2)
                    {
                        if (!AttachExpressionBindingHandler(onStyle, StyleKeys.BorderOutlineStyleKey, val, CSSOutlineStyleParser.ConvertStyle))
                            failed++;
                    }
                    else if (!AttachExpressionBindingHandler(onStyle, StyleKeys.BorderOutlineColorKey, val, CSSOutlineColorParser.ConvertColor))
                        failed++;
                }
                else if (CSSOutlineStyleParser.TrySetStyle(onStyle, val))
                {
                }
                else if (CSSOutlineWidthParser.TryGetWidth(val, out var width))
                {
                    onStyle.SetValue(StyleKeys.BorderOutlineWidthKey, width);
                }
                else if (CSSOutlineColorParser.TrySetColor(onStyle, val))
                {
                }
                else
                    failed++;
            }

            return count > 0 && failed == 0;
        }
    }

    /// <summary>
    /// Parses the outline-color css property
    /// </summary>
    public class CSSOutlineColorParser : CSSStyleValueParser
    {
        public CSSOutlineColorParser()
            : base(CSSStyleItems.OutlineColor)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var val = reader.CurrentTextValue;
            if (IsExpression(val))
                return AttachExpressionBindingHandler(onStyle, StyleKeys.BorderOutlineColorKey, val, ConvertColor);

            return TrySetColor(onStyle, val);
        }

        public static bool TrySetColor(Style onStyle, string value)
        {
            //invert and currentcolor (the default) use the text color
            if (string.Equals(value, "currentcolor", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "invert", StringComparison.OrdinalIgnoreCase))
            {
                onStyle.RemoveValue(StyleKeys.BorderOutlineColorKey);
                return true;
            }

            if (ParseCSSColor(value, out var color, out _))
            {
                onStyle.SetValue(StyleKeys.BorderOutlineColorKey, color);
                return true;
            }
            return false;
        }

        public static bool ConvertColor(StyleBase style, object value, out Color color)
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

    /// <summary>
    /// Parses the outline-style css property - none, solid, dashed, dotted, double (as solid) and auto (as solid)
    /// </summary>
    public class CSSOutlineStyleParser : CSSStyleValueParser
    {
        public CSSOutlineStyleParser()
            : base(CSSStyleItems.OutlineStyle)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var val = reader.CurrentTextValue;
            if (IsExpression(val))
                return AttachExpressionBindingHandler(onStyle, StyleKeys.BorderOutlineStyleKey, val, ConvertStyle);

            return TrySetStyle(onStyle, val);
        }

        public static bool TrySetStyle(Style onStyle, string value)
        {
            //numbers are parsed as enumeration values, so are not styles
            if (string.IsNullOrEmpty(value) || char.IsDigit(value[0]) || value[0] == '-' || value[0] == '.')
                return false;

            if (string.Equals(value, "auto", StringComparison.OrdinalIgnoreCase))
            {
                onStyle.SetValue(StyleKeys.BorderOutlineStyleKey, LineType.Solid);
                return true;
            }

            if (CSSBorderStyleParser.TryGetLineStyleFromString(value, out var line, out var dash))
            {
                onStyle.SetValue(StyleKeys.BorderOutlineStyleKey, line);
                if (null != dash)
                    onStyle.SetValue(StyleKeys.BorderOutlineDashKey, dash);
                else
                    onStyle.RemoveValue(StyleKeys.BorderOutlineDashKey);
                return true;
            }
            return false;
        }

        public static bool ConvertStyle(StyleBase style, object value, out LineType line)
        {
            line = LineType.None;
            if (null == value)
                return false;
            else if (value is LineType l)
            {
                line = l;
                return true;
            }

            var text = value.ToString();
            if (string.Equals(text, "auto", StringComparison.OrdinalIgnoreCase))
            {
                line = LineType.Solid;
                return true;
            }

            if (CSSBorderStyleParser.TryGetLineStyleFromString(text, out line, out var dash))
            {
                if (null != dash)
                    style.SetValue(StyleKeys.BorderOutlineDashKey, dash);
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Parses the outline-width css property - a length or thin, medium and thick
    /// </summary>
    public class CSSOutlineWidthParser : CSSUnitStyleParser
    {
        public CSSOutlineWidthParser()
            : base(CSSStyleItems.OutlineWidth, StyleKeys.BorderOutlineWidthKey)
        {
        }

        protected override bool DoConvertUnit(StyleBase onStyle, object value, out Unit result)
        {
            return ConvertWidth(onStyle, value, out result);
        }

        public static bool ConvertWidth(StyleBase onStyle, object value, out Unit result)
        {
            result = Unit.Zero;
            if (null == value)
                return false;
            else if (value is Unit u)
            {
                result = u;
                return true;
            }
            return TryGetWidth(value.ToString(), out result);
        }

        public static bool TryGetWidth(string value, out Unit width)
        {
            switch (value.Trim().ToLowerInvariant())
            {
                case "thin":
                    width = Unit.Px(1);
                    return true;
                case "medium":
                    width = Unit.Px(3);
                    return true;
                case "thick":
                    width = Unit.Px(5);
                    return true;
                default:
                    return ParseCSSUnit(value, out width) && (width.IsRelative || !Unit.IsAutoValue(width));
            }
        }
    }

    /// <summary>
    /// Parses the outline-offset css property - the distance from the border edge to the outline. Can be negative.
    /// </summary>
    public class CSSOutlineOffsetParser : CSSUnitStyleParser
    {
        public CSSOutlineOffsetParser()
            : base(CSSStyleItems.OutlineOffset, StyleKeys.BorderOutlineOffsetKey)
        {
        }
    }
}
