using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSOpacityParser : CSSStyleAttributeParser<double>
    {
        public CSSOpacityParser()
            : this(CSSStyleItems.Opacity, StyleKeys.FillOpacityKey)
        {
        }

        public CSSOpacityParser(string css, PDFStyleKey<double> key)
            : base(css, key)
        {

        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            double number;

            if (reader.ReadNextValue())
            {
                if (IsExpression(reader.CurrentTextValue))
                {
                    return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, reader.CurrentTextValue, DoConvertOpacity);
                }
                else if (ParseDouble(reader.CurrentTextValue, out number))
                {
                    if (number < 0.0)
                        number = 0.0;
                    if (number > 1.0)
                        number = 1.0;

                    this.SetValue(onStyle, number);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        protected bool DoConvertOpacity(StyleBase style, object value, out double parsed)
        {
            if(null == value)
            {
                parsed = 0.0;
                return false;
            }
            else if(value is double d)
            {
                parsed = d;
                return true;
            }
            else if(ParseDouble(value.ToString(), out parsed))
            {
                if (parsed < 0.0)
                    parsed = 0.0;
                if (parsed > 1.0)
                    parsed = 1.0;
                return true;
            }
            else
            {
                parsed = 0.0;
                return false;
            }
        }
    }
}
