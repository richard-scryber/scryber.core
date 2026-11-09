using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses a css shorthand of one to four lengths (inset) or one to two (margin-block, inset-inline) onto the individual style keys.
    /// The keys are either a start and an end, or in the order top, right, bottom, left (with the standard css expansion of fewer values).
    /// </summary>
    public class CSSUnitSidesParser : CSSStyleValueParser
    {
        private static readonly int[][] FourSides = new int[][]
        {
            null,
            new[] { 0, 0, 0, 0 },
            new[] { 0, 1, 0, 1 },
            new[] { 0, 1, 2, 1 },
            new[] { 0, 1, 2, 3 }
        };

        private static readonly int[][] TwoSides = new int[][]
        {
            null,
            new[] { 0, 0 },
            new[] { 0, 1 }
        };

        private StyleKey<Unit>[] _keys;
        private bool _autoIsUnset;

        /// <param name="cssName">The name of the css property</param>
        /// <param name="autoIsUnset">If true, an auto value does not set the property (e.g. inset), otherwise it is the auto unit (e.g. margin)</param>
        /// <param name="keys">Either 2 keys (start, end), or 4 keys (top, right, bottom, left)</param>
        public CSSUnitSidesParser(string cssName, bool autoIsUnset, params StyleKey<Unit>[] keys)
            : base(cssName)
        {
            if (null == keys || (keys.Length != 2 && keys.Length != 4))
                throw new ArgumentException("There must be 2 or 4 keys", nameof(keys));

            _keys = keys;
            _autoIsUnset = autoIsUnset;
        }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            int max = _keys.Length;
            int count = 0;
            Unit?[] values = new Unit?[max];
            string[] exprs = new string[max];

            while (count < max && reader.ReadNextValue())
            {
                var str = reader.CurrentTextValue;

                if (IsExpression(str))
                {
                    exprs[count] = str;
                }
                else if (_autoIsUnset && string.Equals(str, "auto", StringComparison.OrdinalIgnoreCase))
                {
                    //leave unset
                }
                else if (CSSThicknessValueParser.ParseThicknessValue(str, Unit.AutoValue, out Unit found))
                {
                    values[count] = found;
                }
                else
                    return false;

                count++;
            }

            if (count == 0)
                return false;

            var map = (max == 4) ? FourSides[count] : TwoSides[count];

            for (int i = 0; i < max; i++)
            {
                int index = map[i];

                if (null != exprs[index])
                    AttachExpressionBindingHandler(style, _keys[i], exprs[index], this.DoConvertUnit);
                else if (values[index].HasValue)
                    style.SetValue(_keys[i], values[index].Value);
            }
            return true;
        }

        protected bool DoConvertUnit(StyleBase onStyle, object value, out Unit converted)
        {
            if (null == value)
            {
                converted = Unit.Empty;
                return false;
            }
            else if (value is Unit unit)
            {
                converted = unit;
                return true;
            }

            var str = (value is IFormattable formattable) ? formattable.ToString(null, System.Globalization.CultureInfo.InvariantCulture) : value.ToString();
            if (_autoIsUnset && string.Equals(str, "auto", StringComparison.OrdinalIgnoreCase))
            {
                converted = Unit.Empty;
                return false;
            }
            return CSSThicknessValueParser.ParseThicknessValue(str, Unit.AutoValue, out converted);
        }
    }

    /// <summary>
    /// A single length (or auto) css property, such as margin-block-start, set on the style key.
    /// </summary>
    public class CSSThicknessKeyParser : CSSThicknessValueParser
    {
        public CSSThicknessKeyParser(string cssName, StyleKey<Unit> key)
            : base(cssName, key)
        {
        }
    }

    /// <summary>
    /// Accepts a css property that has no meaning in a PDF (e.g. cursor), reading the values so there is no warning, and setting nothing.
    /// </summary>
    public class CSSIgnoredValueParser : CSSStyleValueParser
    {
        public CSSIgnoredValueParser(string cssName)
            : base(cssName)
        {
        }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            while (reader.ReadNextValue())
            {
                //just read
            }
            return true;
        }
    }
}
