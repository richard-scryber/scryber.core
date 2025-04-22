using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPaddingInlineBothParser : CSSStyleValueParser
    {
        public CSSPaddingInlineBothParser()
            : base(CSSStyleItems.PaddingInlineAll)
        {

        }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            int count = 0;
            Unit? start = null;
            Unit? end = null;
            string exprStart = null;
            string exprEnd = null;

            while (reader.ReadNextValue())
            {
                Unit found;
                var str = reader.CurrentTextValue;

                if(IsExpression(str))
                {
                    if(count == 0)
                    {
                        exprStart = str;
                    }
                    else if(count == 1)
                    {
                        exprEnd = str;
                    }
                    count++;
                }
                else if(CSSUnitStyleParser.ParseCSSUnit(str, out found))
                {
                    if(count == 0)
                    {
                        start = found;
                    }
                    else if(count == 1)
                    {
                        end = found;
                    }
                    count++;
                }
            }
            if (count == 0)
            {
                return false;
            }
            else
            {
                bool result = true;

                if (count == 1)
                {
                    end = start;
                    exprEnd = exprStart;
                }

                if (!string.IsNullOrEmpty(exprStart))
                {
                    result &= AttachExpressionBindingHandler(style, StyleKeys.PaddingInlineStart, exprStart, this.DoConvertUnit);
                }
                else if (start.HasValue)
                {
                    style.SetValue(StyleKeys.PaddingInlineStart, start.Value);
                    result &= true; //redundent, but explicit.
                }

                if (!string.IsNullOrEmpty(exprEnd))
                {
                    result &= AttachExpressionBindingHandler(style, StyleKeys.PaddingInlineEnd, exprEnd, this.DoConvertUnit);
                }
                else if (end.HasValue)
                {
                    style.SetValue(StyleKeys.PaddingInlineEnd, end.Value);
                    result &= true; //redundent, but explicit.
                }

                return result;
            }
        }

        protected virtual bool DoConvertUnit(StyleBase onStyle, object value, out Unit result)
        {
            if (null == value)
            {
                result = Unit.Zero;
                return false;
            }
            else if (value is Unit)
            {
                result = (Unit)value;
                return true;
            }
            else if (TryConvertToUnit(value, out result))
                return true;
            else
                return false;
        }
    }


}
