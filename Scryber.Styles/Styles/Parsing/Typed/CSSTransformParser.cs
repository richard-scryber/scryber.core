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
            bool result = true;
            TransformOperationSet full = null;
            bool bound = false;

            while (reader.ReadNextValue())
            {
                string value = reader.CurrentTextValue;

                TransformOperationSet parsed;
                if (IsExpression(value))
                {
                    if (null != full)
                        throw new ArgumentOutOfRangeException("Cannot bind individual transforms within multiple operations. Consider using a concat expression to build the string");

                    result &= AttachExpressionBindingHandler(onStyle, StyleKeys.TransformOperationKey, value, DoConvertTransform);
                    bound = true;
                }
                else if (DoConvertTransform(onStyle, value, out parsed))
                {
                    if(bound)
                        throw new ArgumentOutOfRangeException("Cannot bind individual transforms within multiple operations. Consider using a concat expression to build the string");

                    if (null != full)
                        full.AppendOperation(parsed.Root);
                    else
                        full = parsed;
                    
                    result = true;
                }
                else
                    result = false;
            }

            if (null != full)
            {
                onStyle.SetValue(StyleKeys.TransformOperationKey, full);
                return result;
            }
            else
            {
                return false;
            }
        }

        private static readonly char[] _separators = { ',', ' ' };


        protected bool DoConvertTransform(StyleBase style, object value, out TransformOperationSet operation)
        {
            return TransformOperationSet.TryParse(value.ToString(), out operation);
        }

        

    }
}
