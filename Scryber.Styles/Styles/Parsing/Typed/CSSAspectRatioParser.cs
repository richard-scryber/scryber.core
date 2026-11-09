using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the CSS aspect-ratio property e.g. 'aspect-ratio: 566/311;' or 'aspect-ratio: 1.82;'.
    /// The 'auto' keyword (on its own) removes any explicit ratio and reverts to the natural/intrinsic size.
    /// </summary>
    public class CSSAspectRatioParser : CSSStyleAttributeParser<double>
    {
        public CSSAspectRatioParser()
            : base(CSSStyleItems.AspectRatio, StyleKeys.SizeAspectRatioKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (reader.ReadNextValue())
            {
                var value = reader.CurrentTextValue;

                if (IsExpression(value))
                    return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, value, DoConvertAspectRatio);

                if (DoParseAspectRatio(value, out double ratio))
                {
                    if (double.IsNaN(ratio))
                        onStyle.RemoveValue(this.StyleAttribute);
                    else
                        this.SetValue(onStyle, ratio);

                    return true;
                }
            }
            return false;
        }

        protected bool DoConvertAspectRatio(StyleBase style, object value, out double ratio)
        {
            if (null == value)
            {
                ratio = double.NaN;
                return false;
            }
            else if (value is double d)
            {
                ratio = d;
                return true;
            }
            else
                return DoParseAspectRatio(value.ToString(), out ratio);
        }

        private static bool DoParseAspectRatio(string value, out double ratio)
        {
            value = value.Trim();

            if (string.Equals(value, "auto", System.StringComparison.OrdinalIgnoreCase))
            {
                ratio = double.NaN;
                return true;
            }

            var slash = value.IndexOf('/');

            if (slash > 0)
            {
                var wpart = value.Substring(0, slash).Trim();
                var hpart = value.Substring(slash + 1).Trim();

                if (ParseDouble(wpart, out double w) && ParseDouble(hpart, out double h) && h != 0)
                {
                    ratio = w / h;
                    return true;
                }
            }
            else if (ParseDouble(value, out double single) && single > 0)
            {
                ratio = single;
                return true;
            }

            ratio = double.NaN;
            return false;
        }
    }
}
