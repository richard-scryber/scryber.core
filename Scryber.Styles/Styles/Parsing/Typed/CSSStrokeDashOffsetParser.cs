using System;
using Scryber.Html;
using Scryber.Drawing;
using System.Collections.Generic;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSStrokeDashOffsetParser : CSSStyleValueParser
    {

        public CSSStrokeDashOffsetParser()
            : base(CSSStyleItems.StrokeDashOffset)
        { }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            if(reader.ReadNextValue())
            {
                if(IsExpression(reader.CurrentTextValue))
                {
                    return AttachExpressionBindingHandler(style, StyleKeys.StrokeDashOffsetKey, reader.CurrentTextValue, DoConvertDashOffset);
                }
                else
                {
                    
                    if (TryConvertToUnit(reader.CurrentTextValue, out var offset))
                    {
                        if (offset.IsRelative && offset.Units != PageUnits.Percent)
                            throw new ArgumentOutOfRangeException("value",
                                "The dash offset values can only be absolute values, or a percentage value - " + reader.CurrentTextValue + " is not usable as a dash offset value");
                        
                        style.SetValue(StyleKeys.StrokeDashOffsetKey, offset );
                        return true;
                    }
                    else
                        return false;
                }
            }
            else
                return false;
        }

        protected bool DoConvertDashOffset(StyleBase style, object value, out Unit dashoffset)
        {
            if(null == value)
            {
                dashoffset = Unit.Zero;
                return false;
            }
            else if (TryConvertToUnit(value, out var offset))
            {
                if (offset.IsRelative && offset.Units != PageUnits.Percent)
                    throw new ArgumentOutOfRangeException("value",
                        "The dash offset values can only be absolute values, or a percentage value - " + value + " is not usable as a dash offset value");

                dashoffset = offset;
                return true;
            }
            else
            {
                dashoffset = Unit.Zero;
                return false;
            }
        }

    }
}
