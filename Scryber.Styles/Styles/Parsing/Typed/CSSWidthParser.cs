using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSWidthParser : CSSUnitStyleParser
    {
        public CSSWidthParser()
            : base(CSSStyleItems.Width, StyleKeys.SizeWidthKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = false;
            if (reader.ReadNextValue())
            {
                string value = reader.CurrentTextValue;

                Unit parsed;
                if(IsExpression(value))
                {
                    //Attach to both the Width and the FullWidth flag
                    result = AttachExpressionBindingHandler(onStyle, StyleKeys.SizeWidthKey, value, DoConvertExplicitWidth);
                    result &= AttachExpressionBindingHandler(onStyle, StyleKeys.SizeFullWidthKey, value, DoConvertFullWidth);
                }
                //else if (value == "100%")
                //{
                //    onStyle.SetValue(StyleKeys.SizeFullWidthKey, true);
                //    result = true;
                //}
                else if (ParseCSSUnit(value, out parsed))
                {
                    onStyle.SetValue(this.StyleAttribute, parsed);
                    result = true;
                }
            }
            return result;
        }

        protected bool DoConvertExplicitWidth(StyleBase style, object value, out Unit width)
        {
            if(null == value)
            {
                width = Unit.Empty;
                return false;
            }
            else if(value is Unit unit)
            {
                width = unit;
                return true;
            }
            else if(TryConvertToUnit(value, out width))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool DoConvertFullWidth(StyleBase style, object value, out bool fullWidth)
        {
            if(null == value)
            {
                fullWidth = false;
                return false;
            }
            else if(value.ToString() == "100%")
            {
                fullWidth = true;
                return true;
            }
            else
            {
                fullWidth = false;
                return false;
            }
        }

    }
}
