using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSFillParser : CSSColorStyleParser
    {
        public CSSFillParser()
            : this(CSSStyleItems.FillColor, StyleKeys.FillColorKey, StyleKeys.FillOpacityKey)
        {
        }

        public CSSFillParser(string css, PDFStyleKey<PDFColor> key, PDFStyleKey<double> opacity)
            : base(css, key, opacity)
        { }


        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {

            bool result = false;
            PDFColor color;

            if (reader.ReadNextValue() && !string.IsNullOrEmpty(reader.CurrentTextValue))
            {

                var val = reader.CurrentTextValue;

                if (IsExpression(val))
                {
                    result = AttachExpressionBindingHandler(onStyle, this.StyleAttribute, val, DoConvertFillValue);
                }
                else if (SetColorValue(onStyle, val, out color))
                {
                    result = true;
                }
                else
                    return false;
               
            }
            return result;
        }

        protected bool DoConvertFillValue(StyleBase style, object value, out PDFColor color)
        {
            if(null == value)
            {
                color = PDFColor.Transparent;
                return false;
            }
            else if(value is PDFColor c)
            {
                color = c;
                return true;
            }
            else if(SetColorValue(style, value.ToString(), out color))
            {
                return true;
            }
            else
            {
                color = PDFColor.Transparent;
                return false;
            }    
        }

        private bool SetColorValue(StyleBase onStyle, string val, out PDFColor color)
        {
            if (val.StartsWith("url("))
            {
                val = val.Substring(4);
                if (val.EndsWith(")"))
                    val = val.Substring(0, val.Length - 1);

                if (val.StartsWith("#"))
                {
                    throw new NotSupportedException("Setting background fills to patterns is not currently supported");
                }
                else
                    onStyle.SetValue(StyleKeys.FillImgSrcKey, val);

                color = PDFColor.Transparent;
            }
            else if (ParseCSSColor(val, out color, out double? opacity))
            {
                onStyle.SetValue(this.StyleAttribute, color);

                if (opacity.HasValue && null != this.OpacityStyleKey)
                    onStyle.SetValue(this.OpacityStyleKey, opacity.Value);

                return true;
            }
            else
                color = PDFColor.Transparent;


            return false;
        }
    }
}
