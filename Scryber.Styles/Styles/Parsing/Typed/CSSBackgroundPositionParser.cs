using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSBackgroundPositionParser : CSSStyleValueParser
    {
        public CSSBackgroundPositionParser() : base(CSSStyleItems.BackgroundPosition)
        {
        }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            string h, v;
            PDFUnit uh, uv;
            bool set = true;

            if (reader.ReadNextValue())
            {
                h = reader.CurrentTextValue;

                if (IsExpression(h))
                {
                    if (reader.ReadNextValue())
                    {
                        //we have 2 values
                        v = reader.CurrentTextValue;
                        set &= this.AttachExpressionBindingHandler(style, StyleKeys.BgXPosKey, h, DoConvertBgPos);

                        if (IsExpression(v))
                        {
                            set &= this.AttachExpressionBindingHandler(style, StyleKeys.BgYPosKey, v, DoConvertBgPos);
                        }
                        else if (PDFUnit.TryParse(v, out uv))
                        {
                            style.Background.PatternYPosition = uv;
                            set &= true;
                        }
                        else
                            set = false;

                    }
                    else
                    {
                        //We have one expression for both - so attach to X and Y
                        set &= this.AttachExpressionBindingHandler(style, StyleKeys.BgXPosKey, h, DoConvertBgPos) &&
                              this.AttachExpressionBindingHandler(style, StyleKeys.BgYPosKey, h, DoConvertBgPos);
                    }

                    return set;

                }
                else
                {
                    if (reader.ReadNextValue())
                        v = reader.CurrentTextValue;
                    else
                        v = h;



                    if (PDFUnit.TryParse(h, out uh))
                    {
                        style.Background.PatternXPosition = uh;
                        set &= true;
                    }
                    else
                        set = false;

                    if(IsExpression(v))
                    {
                        //Just the second is an expression
                        set = set && this.AttachExpressionBindingHandler(style, StyleKeys.BgYPosKey, v, DoConvertBgPos);
                    }
                    else if (PDFUnit.TryParse(v, out uv))
                    {
                        style.Background.PatternYPosition = uv;
                        set &= true;
                    }
                    else
                        set = false;

                    return set;
                }
            }
            else
                return false;
        }

        protected bool DoConvertBgPos(StyleBase forstyle, object value, out PDFUnit size)
        {
            if (null == value)
            {
                size = PDFUnit.Empty;
                return false;
            }
            else if (value is PDFUnit unit)
            {
                size = unit;
                return true;
            }
            else
            {
                var str = value.ToString();

                if (PDFUnit.TryParse(str, out size))
                {
                    return true;
                }
                else
                {
                    size = PDFUnit.Zero;
                    return false;
                }
            }
        }
    }
}
