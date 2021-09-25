using System;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses a css dimension value into a PDFUnit for a style key
    /// </summary>
    public class CSSUnitStyleParser : CSSStyleAttributeParser<PDFUnit>
    {


        public CSSUnitStyleParser(string styleItemKey, PDFStyleKey<Scryber.Drawing.PDFUnit> pdfAttr)
            : base(styleItemKey, pdfAttr)
        {

        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = false;
            if (reader.ReadNextValue())
            {
                string value = reader.CurrentTextValue;

                PDFUnit parsed;
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

        protected virtual bool DoConvertUnit(StyleBase onStyle, object value, out PDFUnit result)
        {
            if(null == value)
            {
                result = PDFUnit.Zero;
                return false;
            }
            else if(value is PDFUnit)
            {
                result = (PDFUnit)value;
                return true;
            }
            else if (PDFUnit.TryParse(value.ToString(), out result))
                return true;
            else
                return false;
        }
    }
}
