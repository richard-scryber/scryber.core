using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSBackgroundSizeParser : CSSStyleValueParser
    {
        public CSSBackgroundSizeParser() : base(CSSStyleItems.BackgroundSize)
        {
        }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            string h, v;
            PDFUnit uh, uv;
            bool set = false;
            if (reader.ReadNextValue())
            {
                h = reader.CurrentTextValue;

                if (IsExpression(h))
                {
                    if(reader.ReadNextValue())
                    {
                        //we have 2 values
                        v = reader.CurrentTextValue;
                        set &= this.AttachExpressionBindingHandler(style, StyleKeys.BgXSizeKey, h, DoConvertBgSize);

                        if (IsExpression(v))
                        {
                            //Second is an expression so attach to the Y-pos ket
                            set &= this.AttachExpressionBindingHandler(style, StyleKeys.BgYSizeKey, v, DoConvertBgSize);
                        }
                        else if (PDFUnit.TryParse(v, out uv))
                        {
                            //Not an expression so just set the value
                            style.Background.PatternYPosition = uv;
                            set &= true;
                        }
                        else
                            set = false;
                    }
                    else
                    {
                        //We have only one - so set both
                        set &= this.AttachExpressionBindingHandler(style, StyleKeys.BgXSizeKey, h, DoConvertBgSize) &&
                              this.AttachExpressionBindingHandler(style, StyleKeys.BgYSizeKey, h, DoConvertBgSize);
                    }
                    return set;
                }
                else if (h.ToLower() == "cover")
                {
                    style.Background.PatternRepeat = PatternRepeat.Fill;
                    return true;
                }
                else
                {
                    if (reader.ReadNextValue())
                        v = reader.CurrentTextValue;
                    else
                        v = h;

                    
                    if (PDFUnit.TryParse(h, out uh))
                    {
                        style.Background.PatternXSize = uh;
                        set &= true;
                    }
                    else
                        set = false;

                    if(IsExpression(v))
                    {
                        //Just the second one is an expression
                        set &= this.AttachExpressionBindingHandler(style, StyleKeys.BgYSizeKey, v, DoConvertBgSize);
                    }
                    else if (PDFUnit.TryParse(v, out uv))
                    {
                        style.Background.PatternYSize = uv;
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


        protected bool DoConvertBgSize(StyleBase forstyle, object value, out PDFUnit size)
        {
            if(null == value)
            {
                size = PDFUnit.Empty;
                return false;
            }
            else if(value is PDFUnit unit)
            {
                size = unit;
                return true;
            }
            else
            {
                var str = value.ToString();
                if(str == "cover")
                {
                    size = PDFUnit.Zero;
                    forstyle.SetValue(StyleKeys.BgRepeatKey, PatternRepeat.Fill);
                    return true;
                }
                else if(PDFUnit.TryParse(str, out size))
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
