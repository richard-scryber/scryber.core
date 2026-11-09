using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the transform-origin css property e.g. 'left top', '50% 100%', '10pt 20pt'.
    /// Any third (z) value is ignored.
    /// </summary>
    public class CSSTransformOriginParser : CSSStyleValueParser
    {
        public CSSTransformOriginParser()
            : base(CSSStyleItems.TransformOrigin)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            string all = null;

            while (reader.ReadNextValue())
            {
                var value = reader.CurrentTextValue;

                if (IsExpression(value) && null == all)
                    return AttachExpressionBindingHandler(onStyle, StyleKeys.TransformOriginKey, value, DoConvertOrigin);

                all = null == all ? value : all + " " + value;
            }

            if (DoConvertOrigin(onStyle, all, out var origin))
            {
                onStyle.SetValue(StyleKeys.TransformOriginKey, origin);
                return true;
            }
            else
                return false;
        }

        protected bool DoConvertOrigin(StyleBase style, object value, out TransformOrigin origin)
        {
            origin = null;
            var str = value?.ToString()?.Trim();

            if (string.IsNullOrEmpty(str))
                return false;

            try
            {
                origin = TransformOrigin.Parse(str.ToLowerInvariant());
                return null != origin;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
