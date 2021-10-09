using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Overrides the EnumStyleParser to parse the specific css border types including dashed styles
    /// </summary>
    public class CSSBorderStyleParser : CSSEnumStyleParser<LineType>
    {
        public static readonly Dash DottedDashPattern = new Dash(new int[] { 2 }, 0);
        public static readonly Dash DashedDashPattern = new Dash(new int[] { 8 }, 0);

        private StyleKey<Dash> _dash;

        protected StyleKey<Dash> DashKey { get { return this._dash; } }

        public CSSBorderStyleParser()
            : this(CSSStyleItems.BorderStyle, StyleKeys.BorderStyleKey, StyleKeys.BorderDashKey)
        {
        }

        public CSSBorderStyleParser(string attr, StyleKey<LineType> style, StyleKey<Dash> dash)
            : base(attr, style)
        {
            this._dash = dash;
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var result = base.DoSetStyleValue(onStyle, reader);

            //Make sure we are past the end of the inner css item
            while (reader.ReadNextValue())
                ;

            return result;
        }

        protected override bool DoConvertEnum(StyleBase onStyle, object value, out LineType result)
        {
            if (null == value)
            {
                result = LineType.None;
                return false;
            }
            else
            {
                var str = value.ToString();
                Dash dash;
                LineType type;

                if (TryGetLineStyleFromString(str, out type, out dash))
                {
                    if (null != dash)
                        onStyle.SetValue(DashKey, dash);

                    result = type;
                    return true;
                }
                else
                {
                    result = LineType.None;
                    return false;
                }
            }
        }

        public static bool TryGetLineStyleFromString(string current, out LineType converted, out Dash dash)
        {
            bool result = false;
            CSSBorder parsed;
            dash = null;
            converted = LineType.None;

            if (Enum.TryParse<CSSBorder>(current, true, out parsed))
            {
                switch (parsed)
                {
                    case CSSBorder.Solid:
                    case CSSBorder.Groove:
                    case CSSBorder.Ridge:
                    case CSSBorder.Inset:
                    case CSSBorder.Outset:
                    case CSSBorder.Double:
                        converted = LineType.Solid;
                        result = true;
                        break;
                    case CSSBorder.Dotted:
                        converted = LineType.Dash;
                        dash = DottedDashPattern;
                        result = true;
                        break;
                    case CSSBorder.Dashed:
                        converted = LineType.Dash;
                        dash = DashedDashPattern;
                        result = true;
                        break;
                    case CSSBorder.None:
                    case CSSBorder.Hidden:
                        converted = LineType.None;
                        result = true;
                        break;
                    default:
                        converted = LineType.None;
                        result = true;
                        break;
                }
            }
            return result;
        }
    }

    public class CSSBorderLeftStyleParser : CSSBorderStyleParser
    {

        public CSSBorderLeftStyleParser() : base(CSSStyleItems.BorderLeftStyle, StyleKeys.BorderLeftStyleKey, StyleKeys.BorderLeftDashKey)
        { }
    }

    public class CSSBorderTopStyleParser : CSSBorderStyleParser
    {
        public CSSBorderTopStyleParser() : base(CSSStyleItems.BorderTopStyle, StyleKeys.BorderTopStyleKey, StyleKeys.BorderTopDashKey)
        { }
    }

    public class CSSBorderRightStyleParser : CSSBorderStyleParser
    {
        public CSSBorderRightStyleParser() : base(CSSStyleItems.BorderRightStyle, StyleKeys.BorderRightStyleKey, StyleKeys.BorderRightDashKey)
        { }
    }

    public class CSSBorderBottomStyleParser : CSSBorderStyleParser
    {
        public CSSBorderBottomStyleParser() : base(CSSStyleItems.BorderBottomStyle, StyleKeys.BorderBottomStyleKey, StyleKeys.BorderBottomDashKey)
        { }
    }
}
