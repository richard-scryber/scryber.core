using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the box-decoration-break css property (and its -webkit- name) - clone (the default) or slice
    /// </summary>
    public class CSSBoxDecorationBreakParser : CSSStyleValueParser
    {
        public CSSBoxDecorationBreakParser(string item)
            : base(item)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var val = reader.CurrentTextValue;
            if (IsExpression(val))
                return AttachExpressionBindingHandler(onStyle, StyleKeys.BorderDecorationBreakKey, val, DoConvert);

            if (TryParse(val, out var result))
            {
                onStyle.SetValue(StyleKeys.BorderDecorationBreakKey, result);
                return true;
            }
            return false;
        }

        protected bool DoConvert(StyleBase style, object value, out BoxDecorationBreak result)
        {
            result = BoxDecorationBreak.Clone;
            if (null == value)
                return false;
            else if (value is BoxDecorationBreak b)
            {
                result = b;
                return true;
            }
            return TryParse(value.ToString(), out result);
        }

        public static bool TryParse(string value, out BoxDecorationBreak result)
        {
            switch (value.Trim().ToLowerInvariant())
            {
                case "clone": result = BoxDecorationBreak.Clone; return true;
                case "slice": result = BoxDecorationBreak.Slice; return true;
                default:
                    result = BoxDecorationBreak.Clone;
                    return false;
            }
        }
    }
}
