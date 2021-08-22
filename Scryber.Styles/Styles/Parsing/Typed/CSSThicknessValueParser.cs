using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Generic CSSUnit Parser for thicknesses - supports the 'Auto' option as well as units.
    /// </summary>
    public abstract class CSSThicknessValueParser : CSSUnitStyleParser
    {
        protected PDFUnit AutoValue { get; set; }

        public CSSThicknessValueParser(string cssName, PDFStyleKey<PDFUnit> pdfAttr)
            : base(cssName, pdfAttr)
        {
            AutoValue = PDFUnit.Zero;
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (reader.ReadNextValue())
            {
                PDFUnit found;
                if(IsExpression(reader.CurrentTextValue))
                {
                    if (this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, reader.CurrentTextValue, DoConvertThicknessValue))
                        return true;
                }
                else if (ParseThicknessValue(reader.CurrentTextValue, this.AutoValue, out found))
                {
                    this.SetValue(onStyle, found);
                    return true;
                }

            }
            return false;

        }

        protected bool DoConvertThicknessValue(StyleBase onStyle, object value, out PDFUnit found)
        {
            if (null == value)
            {
                found = PDFUnit.Empty;
                return false;
            }
            else if (value is PDFUnit unit)
            {
                found = unit;
                return true;
            }
            else if (ParseThicknessValue(value.ToString(), this.AutoValue, out found))
            {
                return true;
            }
            else
                return false;
        }

        public static bool ParseThicknessValue(string value, PDFUnit auto, out PDFUnit found)
        {
            if (value.Equals("auto", StringComparison.OrdinalIgnoreCase))
            {
                found = auto;
                return true;
            }
            else if (ParseCSSUnit(value, out found))
            {
                return true;
            }

            return false;
        }
    }
}
