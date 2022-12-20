using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSTransformParser : CSSStyleValueParser
    {
        public CSSTransformParser()
            : base(CSSStyleItems.Transform)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = false;
            if (reader.ReadNextValue())
            {
                string value = reader.CurrentTextValue;

                float parsed;
                if(IsExpression(value))
                {
                    //Attach to both the Width and the FullWidth flag
                    result = AttachExpressionBindingHandler(onStyle, StyleKeys.TransformRotateKey, value, DoConvertTransformRotate);
                    //result &= AttachExpressionBindingHandler(onStyle, StyleKeys.SizeFullWidthKey, value, DoConvertFullWidth);
                }
                else if (DoConvertTransformRotate(onStyle, value, out parsed))
                {
                    onStyle.SetValue(StyleKeys.TransformRotateKey, parsed);
                    result = true;
                }
                
            }
            return result;
        }

        protected bool DoConvertTransformRotate(StyleBase style, object value, out float angle)
        {
            if(null == value)
            {
                angle = 0;
                return false;
            }

            var str = value.ToString().Trim();
            if(str.StartsWith("rotate("))
            {
                var end = str.IndexOf(")", 7);
                if(end <= 8)
                {
                    angle = 0;
                    return false;
                }
                str = str.Substring(7, end - 7).Trim();
            }

            if (str.EndsWith("deg"))
                str = str.Substring(0, str.Length - 3);

            if(float.TryParse(str, out angle))
            {
                angle = (float)((Math.PI / 180.0) * angle);
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
