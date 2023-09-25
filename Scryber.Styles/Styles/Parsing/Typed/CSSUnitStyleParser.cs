using System;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses a css dimension value into a PDFUnit for a style key
    /// </summary>
    public class CSSUnitStyleParser : CSSStyleAttributeParser<Unit>
    {


        public CSSUnitStyleParser(string styleItemKey, StyleKey<Scryber.Drawing.Unit> attr)
            : base(styleItemKey, attr)
        {

        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = false;
            if (reader.ReadNextValue())
            {
                string value = reader.CurrentTextValue;

                Unit parsed;
                if (IsExpression(value))
                {
                    result = AttachExpressionBindingHandler(onStyle, this.StyleAttribute, value, this.DoConvertUnit);
                }
                if (this.DoConvertUnit(onStyle, value, out parsed))
                {
                    onStyle.SetValue(this.StyleAttribute, parsed);
                    result = true;
                }
            }
            return result;
        }

        protected virtual bool DoConvertUnit(StyleBase onStyle, object value, out Unit result)
        {
            if(null == value)
            {
                result = Unit.Zero;
                return false;
            }
            else if(value is Unit)
            {
                result = (Unit)value;
                return true;
            }
            else if (Unit.TryParse(value.ToString(), out result))
                return true;
            else
                return false;
        }
    }
}
