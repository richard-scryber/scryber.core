using System;
using System.Threading;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the border shorthand property - for border-style, border-width, border-color
    /// </summary>
    public class CSSBorderParser : CSSStyleValueParser
    {


        public CSSBorderParser()
            : base(CSSStyleItems.Border)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {

            int count = 0;
            int failed = 0;

            while (reader.ReadNextValue())
            {
                count++;
                if(IsExpression(reader.CurrentTextValue))
                {
                    //Border order is Width, Style, Color
                    if(count == 1)
                    {
                        if (!AttachExpressionBindingHandler(onStyle, StyleKeys.BorderWidthKey, reader.CurrentTextValue, DoConvertBorderWidth))
                            failed++;
                    }
                    else if(count == 2)
                    {
                        if (!AttachExpressionBindingHandler(onStyle, StyleKeys.BorderStyleKey, reader.CurrentTextValue, DoConvertBorderStyle))
                            failed++;
                    }
                    else if(count == 3)
                    {
                        if (!AttachExpressionBindingHandler(onStyle, StyleKeys.BorderColorKey, reader.CurrentTextValue, DoConvertBorderColor))
                            failed++;
                    }
                }
                else if (IsNumber(reader.CurrentTextValue))
                {
                    PDFUnit unit;
                    if (ParseCSSUnit(reader.CurrentTextValue, out unit))
                        onStyle.Border.Width = unit;
                    else
                        failed++;

                }
                else if (IsColor(reader.CurrentTextValue))
                {
                    Color color;
                    double? opacity;
                    if (ParseCSSColor(reader.CurrentTextValue, out color, out opacity))
                    {
                        onStyle.Border.Color = color;

                        if (opacity.HasValue)
                            onStyle.Border.Opacity = opacity.Value;
                    }
                    else
                        failed++;
                }
                else
                {
                    LineType style;
                    Dash dash;
                    if (CSSBorderStyleParser.TryGetLineStyleFromString(reader.CurrentTextValue, out style, out dash))
                    {
                        onStyle.Border.LineStyle = style;
                        if (null != dash)
                            onStyle.Border.Dash = dash;
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
                if (opacity.HasValue)
                    onStyle.SetValue(StyleKeys.BorderOpacityKey, opacity.Value);
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
                    onStyle.SetValue(StyleKeys.BorderDashKey, dash);

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

}
