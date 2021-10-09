using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Base clase for the individual side parsers for left, top, bottom and right,
    /// That will parse or bind values for the border-xxx value.
    /// </summary>
    public abstract class CSSBorderSideParser : CSSStyleValueParser
    {
        private StyleKey<PDFUnit> _width;

        private StyleKey<Color> _color;
        private StyleKey<LineType> _style;
        private StyleKey<Dash> _dash;

        public CSSBorderSideParser(string cssName, StyleKey<PDFUnit> width, StyleKey<Color> color, StyleKey<LineType> style, StyleKey<Dash> dash)
            : base(cssName)
        {
            this._width = width;
            this._color = color;
            this._style = style;
            this._dash = dash;
        }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            int count = 0;
            int failed = 0;

            while (reader.ReadNextValue())
            {
                count++;

                if(IsExpression(reader.CurrentTextValue))
                {
                    //Order Is Important - expecting Width, Style, Color in that order
                    if (count == 1)
                    {
                        if (!AttachExpressionBindingHandler(style, StyleKeys.BorderWidthKey, reader.CurrentTextValue, DoConvertBorderWidth))
                            failed++;
                    }
                    else if (count == 2)
                    {
                        if (!AttachExpressionBindingHandler(style, StyleKeys.BorderStyleKey, reader.CurrentTextValue, DoConvertBorderStyle))
                            failed++;
                    }
                    else if (count == 3)
                    {
                        if (!AttachExpressionBindingHandler(style, StyleKeys.BorderColorKey, reader.CurrentTextValue, DoConvertBorderColor))
                            failed++;
                    }
                }
                else if (IsNumber(reader.CurrentTextValue))
                {
                    PDFUnit unit;
                    if (ParseCSSUnit(reader.CurrentTextValue, out unit))
                        style.SetValue(_width, unit);
                    else
                        failed++;

                }
                else if (IsColor(reader.CurrentTextValue))
                {
                    Color color;
                    double? opacity;
                    if (ParseCSSColor(reader.CurrentTextValue, out color, out opacity))
                    {
                        style.SetValue(_color, color);
                        //TODO: Set the opacity of the border side.
                    }
                    else
                        failed++;
                }
                else
                {
                    LineType line;
                    Dash dash;

                    if (CSSBorderStyleParser.TryGetLineStyleFromString(reader.CurrentTextValue, out line, out dash))
                    {
                        style.SetValue(_style, line);
                        if (null != dash)
                            style.SetValue(_dash, dash);
                    }
                    else
                        failed++;
                }

            }
            return count > 0 && failed == 0;
        }


        protected bool DoConvertBorderColor(StyleBase onStyle, object value, out Color result)
        {
            double? opacity;

            if (null == value)
            {
                result = Color.Transparent;
                return false;
            }
            else if (value is Color color)
            {
                result = color;
                return true;
            }
            else if (ParseCSSColor(value.ToString(), out result, out opacity))
            {
                //TODO:Support individual opacity on borders
                return true;
            }
            else
            {
                result = Color.Transparent;
                return false;
            }
        }

        protected bool DoConvertBorderStyle(StyleBase onStyle, object value, out LineType result)
        {
            LineType style;
            Dash dash;

            if (null == value)
            {
                result = LineType.None;
                return false;
            }
            else if (value is LineType)
            {
                result = (LineType)value;
                return true;
            }
            else if (CSSBorderStyleParser.TryGetLineStyleFromString(value.ToString(), out style, out dash))
            {
                if (null != dash)
                    onStyle.SetValue(this._dash, dash);

                result = style;
                return true;
            }
            else
            {
                result = LineType.None;
                return false;
            }
        }

        protected bool DoConvertBorderWidth(StyleBase onStyle, object value, out PDFUnit result)
        {
            if (null == value)
            {
                result = PDFUnit.Zero;
                return false;
            }
            else if (value is PDFUnit)
            {
                result = (PDFUnit)value;
                return true;
            }
            else if (ParseCSSUnit(value.ToString(), out result))
            {
                return true;
            }
            else
            {
                result = PDFUnit.Zero;
                return false;
            }
        }
    }

    public class CSSBorderTopParser : CSSBorderSideParser
    {
        public CSSBorderTopParser()
            : base(CSSStyleItems.BorderTop, StyleKeys.BorderTopWidthKey,
                  StyleKeys.BorderTopColorKey, StyleKeys.BorderTopStyleKey,
                  StyleKeys.BorderTopDashKey)
        {

        }
    }

    public class CSSBorderLeftParser : CSSBorderSideParser
    {
        public CSSBorderLeftParser()
            : base(CSSStyleItems.BorderLeft, StyleKeys.BorderLeftWidthKey,
                  StyleKeys.BorderLeftColorKey, StyleKeys.BorderLeftStyleKey,
                  StyleKeys.BorderLeftDashKey)
        {

        }
    }

    public class CSSBorderRightParser : CSSBorderSideParser
    {
        public CSSBorderRightParser()
            : base(CSSStyleItems.BorderRight, StyleKeys.BorderRightWidthKey,
                  StyleKeys.BorderRightColorKey, StyleKeys.BorderRightStyleKey,
                  StyleKeys.BorderRightDashKey)
        {

        }
    }

    public class CSSBorderBottomParser : CSSBorderSideParser
    {
        public CSSBorderBottomParser()
            : base(CSSStyleItems.BorderBottom, StyleKeys.BorderBottomWidthKey,
                  StyleKeys.BorderBottomColorKey, StyleKeys.BorderBottomStyleKey,
                  StyleKeys.BorderBottomDashKey)
        {

        }
    }
}
