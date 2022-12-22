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

                TransformOperation parsed;
                if(IsExpression(value))
                {
                    //Attach to both the Width and the FullWidth flag
                    result = AttachExpressionBindingHandler(onStyle, StyleKeys.TransformOperationKey, value, DoConvertTransform);
                    //result &= AttachExpressionBindingHandler(onStyle, StyleKeys.SizeFullWidthKey, value, DoConvertFullWidth);
                }
                else if (DoConvertTransform(onStyle, value, out parsed))
                {
                    onStyle.SetValue(StyleKeys.TransformOperationKey, parsed);
                    result = true;
                }
                
            }
            return result;
        }

        protected bool DoConvertTransform(StyleBase style, object value, out TransformOperation operation)
        {
            TransformType type;
            float value1 = TransformOperation.NotSetValue();
            float value2 = TransformOperation.NotSetValue();

            if(null == value)
            {
                operation = null;
                return false;
            }

            var str = value.ToString().Trim();
            if(str.StartsWith("rotate("))
            {
                var end = str.IndexOf(")", 7);
                if(end <= 8)
                {
                    operation = null;
                    return false;
                }
                str = str.Substring(7, end - 7).Trim();
                type = TransformType.Rotate;
            }
            else if (str.StartsWith("skew("))
            {
                throw new NotSupportedException("Skew not currently supported");
            }
            else if (str.StartsWith("scale("))
            {
                throw new NotSupportedException("Scale not currently supported");
            }
            else if (str.StartsWith("translate("))
            {
                throw new NotSupportedException("Translate not currently supported");
            }
            else
            {
                throw new NotSupportedException("The transform operation " + str + " is not known or not currently supported");
            }

            if (str.EndsWith("deg"))
                str = str.Substring(0, str.Length - 3);

            if(float.TryParse(str, out value1))
            {
                value1 = -((float)((Math.PI / 180.0) * value1));
                operation = new TransformOperation(type, value1, value2);
                return true;
            }
            else
            {
                operation = null;
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
