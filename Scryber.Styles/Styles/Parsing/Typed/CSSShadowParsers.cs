using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Base for the shadow parsers. A list of comma separated shadows, each with 2 to 4 lengths (x, y, blur, spread) and an optional color (and inset).
    /// </summary>
    public abstract class CSSShadowParser : CSSStyleValueParser
    {
        private readonly StyleKey<BoxShadow[]> _key;
        private readonly bool _isBox;

        protected CSSShadowParser(string item, StyleKey<BoxShadow[]> key, bool isBox)
            : base(item)
        {
            _key = key;
            _isBox = isBox;
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            var all = new StringBuilder();

            //read all the comma separated shadows, each is one value
            while (reader.ReadNextValue(',', ';', true))
            {
                if (all.Length > 0)
                    all.Append(',');
                all.Append(reader.CurrentTextValue);

                //move past the comma separating the shadows, as the white space is ignored when reading
                if (!reader.InnerEnumerator.EOS && reader.InnerEnumerator.Current == ',')
                    reader.InnerEnumerator.MoveNext();
            }

            if (all.Length == 0)
                return false;

            var value = all.ToString().Trim();

            if (IsExpression(value))
                return AttachExpressionBindingHandler(onStyle, _key, value, DoConvertShadows);

            if (TryParseShadows(value, _isBox, out var shadows))
            {
                onStyle.SetValue(_key, shadows);
                return true;
            }

            return false;
        }

        protected bool DoConvertShadows(StyleBase style, object value, out BoxShadow[] shadows)
        {
            shadows = new BoxShadow[0];
            if (null == value)
                return false;
            else if (value is BoxShadow[] already)
            {
                shadows = already;
                return true;
            }
            return TryParseShadows(value.ToString(), _isBox, out shadows);
        }

        /// <summary>
        /// Parses the list of shadows. 'none' is an empty list.
        /// </summary>
        public static bool TryParseShadows(string value, bool allowSpread, out BoxShadow[] shadows)
        {
            shadows = new BoxShadow[0];

            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (string.Equals(value.Trim(), "none", StringComparison.OrdinalIgnoreCase))
                return true;

            var list = new List<BoxShadow>();

            foreach (var part in SplitOutsideParentheses(value, ','))
            {
                if (!TryParseShadow(part, allowSpread, out var shadow))
                    return false;
                list.Add(shadow);
            }

            shadows = list.ToArray();
            return shadows.Length > 0;
        }

        private static bool TryParseShadow(string value, bool allowSpread, out BoxShadow shadow)
        {
            shadow = new BoxShadow();
            var lengths = new List<Unit>();
            bool hasColor = false;

            foreach (var token in SplitOutsideParentheses(value, ' '))
            {
                if (string.Equals(token, "inset", StringComparison.OrdinalIgnoreCase))
                {
                    shadow.Inset = true;
                }
                else if (CSSStyleValueParser.ParseCSSColor(token, out var color, out var opacity) && !IsLength(token))
                {
                    if (hasColor)
                        return false;

                    hasColor = true;
                    shadow.Color = color;
                    if (opacity.HasValue)
                        shadow.Opacity = opacity.Value;
                }
                else if (Unit.TryParse(token, out var unit) && !unit.IsRelative)
                {
                    lengths.Add(unit);
                }
                else
                    return false;
            }

            if (lengths.Count < 2 || lengths.Count > (allowSpread ? 4 : 3))
                return false;

            shadow.OffsetX = lengths[0];
            shadow.OffsetY = lengths[1];
            shadow.Blur = lengths.Count > 2 ? lengths[2] : Unit.Zero;
            shadow.Spread = lengths.Count > 3 ? lengths[3] : Unit.Zero;

            //a blur cannot be negative
            if (shadow.Blur < Unit.Zero)
                return false;

            return true;
        }

        private static bool IsLength(string token)
        {
            return token.Length > 0 && (char.IsDigit(token[0]) || token[0] == '-' || token[0] == '.' || token[0] == '+');
        }

        /// <summary>
        /// Splits on the separator, ignoring any inside parentheses (e.g. the commas in rgba(0,0,0,0.5))
        /// </summary>
        private static IEnumerable<string> SplitOutsideParentheses(string value, char separator)
        {
            int depth = 0;
            var current = new StringBuilder();

            foreach (var c in value)
            {
                if (c == '(')
                    depth++;
                else if (c == ')')
                    depth--;

                bool isSeparator = depth == 0 && (c == separator || (separator == ' ' && char.IsWhiteSpace(c)));
                if (isSeparator)
                {
                    if (current.Length > 0)
                        yield return current.ToString().Trim();
                    current.Clear();
                }
                else
                    current.Append(c);
            }

            if (current.Length > 0)
                yield return current.ToString().Trim();
        }
    }

    /// <summary>
    /// Parses the box-shadow css property
    /// </summary>
    public class CSSBoxShadowParser : CSSShadowParser
    {
        public CSSBoxShadowParser()
            : base(CSSStyleItems.BoxShadow, StyleKeys.BorderBoxShadowKey, true)
        {
        }
    }

    /// <summary>
    /// Parses the text-shadow css property
    /// </summary>
    public class CSSTextShadowParser : CSSShadowParser
    {
        public CSSTextShadowParser()
            : base(CSSStyleItems.TextShadow, StyleKeys.TextShadowKey, false)
        {
        }
    }
}
