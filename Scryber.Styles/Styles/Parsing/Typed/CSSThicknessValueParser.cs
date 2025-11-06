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
        protected Unit AutoValue { get; set; }

        public CSSThicknessValueParser(string cssName, StyleKey<Unit> pdfAttr)
            : base(cssName, pdfAttr)
        {
            AutoValue = Unit.AutoValue;
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (reader.ReadNextValue())
            {
                Unit found;
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

        protected bool DoConvertThicknessValue(StyleBase onStyle, object value, out Unit found)
        {
            if (null == value)
            {
                found = Unit.Empty;
                return false;
            }
            else if (value is Unit unit)
            {
                found = unit;
                return true;
            }
            else if(value is IFormattable)
            {
                var str = ((IFormattable)value).ToString(null, System.Globalization.CultureInfo.InvariantCulture);
                return ParseThicknessValue(str, this.AutoValue, out found);
            }
            else if (ParseThicknessValue(value.ToString(), this.AutoValue, out found))
            {
                return true;
            }
            else
                return false;
        }

        public static bool ParseThicknessValue(string value, Unit auto, out Unit found)
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
