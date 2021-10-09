using System;
using Scryber.Html;
using Scryber.Drawing;
using System.Text;

namespace Scryber.Styles.Parsing.Typed
{

    public class CSSColumnWidthParser : CSSStyleAttributeParser<ColumnWidths>
    {
        public CSSColumnWidthParser()
            : base(CSSStyleItems.ColumnWidths, StyleKeys.ColumnWidthKey)
        {

        }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            string all = string.Empty;
            StringBuilder buffer = new StringBuilder();
            ColumnWidths widths;

            while (reader.ReadNextValue())
            {
                if (buffer.Length > 0)
                    buffer.Append(" ");
                buffer.Append(reader.CurrentTextValue);
            }

            if (buffer.Length > 0)
            {
                all = buffer.ToString();

                if (IsExpression(all))
                {
                    return this.AttachExpressionBindingHandler(style, this.StyleAttribute, all, DoConvertColumnWidths);
                }
                else if (TryParseWidths(all, out widths))
                {
                    this.SetValue(style, widths);
                    return true;
                }
            }

            return false;
        }

        protected bool DoConvertColumnWidths(StyleBase style, object value, out ColumnWidths widths)
        {
            if(null == value)
            {
                widths = default;
                return false;
            }
            else if(value is ColumnWidths w)
            {
                widths = w;
                return true;
            }
            else if(this.TryParseWidths(value.ToString(), out widths))
            {
                return true;
            }
            else
            {
                widths = default;
                return false;
            }
        }


        protected bool TryParseWidths(string value, out ColumnWidths widths)
        {
            try
            {
                widths = ColumnWidths.Parse(value);
            }
            catch
            {
                widths = default;
                return false;
            }
            return true;
        }
    }
}
