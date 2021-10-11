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
            Unit uh, uv;
            bool set = true;
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
                        else if (Unit.TryParse(v, out uv))
                        {
                            //Not an expression so just set the value
                            style.Background.PatternYSize = uv;
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

                    
                    if (Unit.TryParse(h, out uh))
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
                    else if (Unit.TryParse(v, out uv))
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


        protected bool DoConvertBgSize(StyleBase forstyle, object value, out Unit size)
        {
            if(null == value)
            {
                size = Unit.Empty;
                return false;
            }
            else if(value is Unit unit)
            {
                size = unit;
                return true;
            }
            else
            {
                var str = value.ToString();
                if(str == "cover")
                {
                    size = Unit.Zero;
                    forstyle.SetValue(StyleKeys.BgRepeatKey, PatternRepeat.Fill);
                    return true;
                }
                else if(Unit.TryParse(str, out size))
                {
                    return true;
                }
                else
                {
                    size = Unit.Zero;
                    return false;
                }
            }
        }



    }
}
