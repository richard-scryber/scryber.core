using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPaintOrderParser : CSSStyleAttributeParser<PaintOrder>
    {
        public CSSPaintOrderParser()
            : base(CSSStyleItems.PaintOrder, StyleKeys.SVGGeometryPaintOrderKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            PaintOrder paintOrder;

            string full = "";
            while (reader.ReadNextValue())
            {
                if(full.Length > 0)
                    full += " ";
                full += reader.CurrentTextValue;
            }

            if (IsExpression(full))
            {
                return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, reader.CurrentTextValue,
                    DoConvertPaintOrder);
            }
            else if (ParsePaintOrder(full, out paintOrder))
            {
                this.SetValue(onStyle, paintOrder);
                return true;
            }

            return false;
        }

        protected bool DoConvertPaintOrder(StyleBase style, object value, out PaintOrder po)
        {
            if(null == value)
            {
                po = PaintOrder.Default;
                return false;
            }
            else if(ParsePaintOrder(value.ToString(), out po))
            {
                return true;
            }
            else
            {
                po = PaintOrder.Default;
                return false;
            }
        }

        protected bool ParsePaintOrder(string value, out PaintOrder po)
        {
            return PaintOrder.TryParse(value, out po);
        }
    }
}
