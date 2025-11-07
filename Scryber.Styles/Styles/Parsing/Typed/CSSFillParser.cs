using System;
using Scryber.Html;
using Scryber.Drawing;
using Scryber.PDF.Graphics;
using Scryber.Svg;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the SVG fill value - can either be a color value e.g. '#AAAAA', or a gradient reference e.g. 'url(#gradientId)'.
    /// </summary>
    public class CSSFillParser : CSSStyleValueParser
    {
        private StyleKey<SVGFillValue> FillAttribute;
        
        public CSSFillParser()
            : this(CSSStyleItems.FillColor, StyleKeys.SVGFillKey)
        {
        }

        public CSSFillParser(string css, StyleKey<SVGFillValue> key)
            : base(css)
        {
            this.FillAttribute = key;
        }


        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {

            bool result = false;
            Color color;

            if (reader.ReadNextValue() && !string.IsNullOrEmpty(reader.CurrentTextValue))
            {

                var val = reader.CurrentTextValue;
                string valId;
                if (IsExpression(val))
                {
                    result = AttachExpressionBindingHandler(onStyle, this.FillAttribute, val, DoConvertFillValue);
                }
                else if (IsReferenceValue(val, out valId))
                {
                    SVGFillReferenceValue gradient = new SVGFillReferenceValue(null, valId);
                    onStyle.SetValue(this.FillAttribute, gradient);
                }
                else if (IsNamedValue(val, out PDFBrush brush))
                {
                    SVGFillNamedValue named = new SVGFillNamedValue(brush, val);
                    onStyle.SetValue(this.FillAttribute, named);
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

        protected bool IsReferenceValue(string value, out string refId)
        {
            if (value.StartsWith("url(", StringComparison.InvariantCulture))
            {
                if (value.EndsWith(")", StringComparison.InvariantCulture))
                {
                    value = value.Substring(4);
                    value = value.Substring(value.Length - 1);
                    refId = value;
                    return true;
                }
            }

            refId = string.Empty;
            return false;
        }

        
        

        protected bool DoConvertFillValue(StyleBase style, object value, out SVGFillValue fill)
        {
            Color color;
            fill = null;
            if(null == value)
            {
                color = Color.Transparent;
                return false;
            }
            else if(value is Color c)
            {
                color = c;
                fill = new SVGFillColorValue(color, color.ToString());
                return true;
            }
            else if (IsReferenceValue(value.ToString(), out string refId))
            {
                fill = new SVGFillReferenceValue(null, refId);
                return true;
            }
            else if (IsNamedValue(value.ToString(), out PDFBrush brush))
            {
                fill = new SVGFillNamedValue(brush, value.ToString());
                return true;
            }
            else if(SetColorValue(style, value.ToString(), out color))
            {
                fill = new SVGFillColorValue(color, value.ToString());
                return true;
            }
            else
            {
                color = Color.Transparent;
                return false;
            }    
        }

        protected virtual bool IsNamedValue(string value, out PDFBrush brush)
        {
            return SVGFillValue.IsNamedValue(value, out brush);
        }

        private bool SetColorValue(StyleBase onStyle, string val, out Color color)
        {
            if (ParseCSSColor(val, out color, out double? opacity))
            {
                var fill = new SVGFillColorValue(color, val);
                onStyle.SetValue(this.FillAttribute, fill);
                
                return true;
            }
            else
                color = Color.Transparent;


            return false;
        }
    }
}
