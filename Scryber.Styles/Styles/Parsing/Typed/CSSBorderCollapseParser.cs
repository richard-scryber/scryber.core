using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the border-collapse css property - separate (default) or collapse
    /// </summary>
    public class CSSBorderCollapseParser : CSSStyleValueParser
    {
        public CSSBorderCollapseParser()
            : base(CSSStyleItems.BorderCollapse)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var val = reader.CurrentTextValue;

            if (IsExpression(val))
                return AttachExpressionBindingHandler(onStyle, StyleKeys.TableBorderCollapseKey, val, DoConvertCollapse);

            if (TryParseCollapse(val, out var collapse))
            {
                onStyle.SetValue(StyleKeys.TableBorderCollapseKey, collapse);
                return true;
            }

            return false;
        }

        protected bool DoConvertCollapse(StyleBase style, object value, out BorderCollapse collapse)
        {
            if (null == value)
            {
                collapse = BorderCollapse.Separate;
                return false;
            }
            else if (value is BorderCollapse b)
            {
                collapse = b;
                return true;
            }
            else
                return TryParseCollapse(value.ToString(), out collapse);
        }

        public static bool TryParseCollapse(string value, out BorderCollapse collapse)
        {
            switch (value.Trim().ToLowerInvariant())
            {
                case "separate":
                    collapse = BorderCollapse.Separate;
                    return true;
                case "collapse":
                    collapse = BorderCollapse.Collapse;
                    return true;
                default:
                    collapse = BorderCollapse.Separate;
                    return false;
            }
        }
    }
}
