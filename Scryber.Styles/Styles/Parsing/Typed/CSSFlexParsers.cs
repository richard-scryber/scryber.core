using System;
using System.Globalization;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSFlexDirectionParser : CSSStyleAttributeParser<FlexDirection>
    {
        public CSSFlexDirectionParser() : base(CSSStyleItems.FlexDirection, StyleKeys.FlexDirectionKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var text = reader.CurrentTextValue;
            if (IsExpression(text))
                return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, text, this.DoConvertDirection);
            else if (TryGetDirection(text, out var dir))
            {
                this.SetValue(onStyle, dir);
                return true;
            }
            return false;
        }

        protected virtual bool DoConvertDirection(StyleBase onStyle, object value, out FlexDirection result)
        {
            if (value is FlexDirection fd) { result = fd; return true; }
            return TryGetDirection(value?.ToString() ?? string.Empty, out result);
        }

        public static bool TryGetDirection(string value, out FlexDirection direction)
        {
            switch (value.ToLower())
            {
                case "row":            direction = FlexDirection.Row;           return true;
                case "row-reverse":    direction = FlexDirection.RowReverse;    return true;
                case "column":         direction = FlexDirection.Column;        return true;
                case "column-reverse": direction = FlexDirection.ColumnReverse; return true;
                default:               direction = FlexDirection.Row;           return false;
            }
        }
    }

    public class CSSFlexWrapParser : CSSStyleAttributeParser<FlexWrap>
    {
        public CSSFlexWrapParser() : base(CSSStyleItems.FlexWrap, StyleKeys.FlexWrapKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var text = reader.CurrentTextValue;
            if (IsExpression(text))
                return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, text, this.DoConvertWrap);
            else if (TryGetWrap(text, out var wrap))
            {
                this.SetValue(onStyle, wrap);
                return true;
            }
            return false;
        }

        protected virtual bool DoConvertWrap(StyleBase onStyle, object value, out FlexWrap result)
        {
            if (value is FlexWrap fw) { result = fw; return true; }
            return TryGetWrap(value?.ToString() ?? string.Empty, out result);
        }

        public static bool TryGetWrap(string value, out FlexWrap wrap)
        {
            switch (value.ToLower())
            {
                case "nowrap":       wrap = FlexWrap.Nowrap;      return true;
                case "wrap":         wrap = FlexWrap.Wrap;        return true;
                case "wrap-reverse": wrap = FlexWrap.WrapReverse; return true;
                default:             wrap = FlexWrap.Nowrap;      return false;
            }
        }
    }

    public class CSSJustifyContentParser : CSSStyleAttributeParser<FlexJustify>
    {
        public CSSJustifyContentParser() : base(CSSStyleItems.JustifyContent, StyleKeys.FlexJustifyKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var text = reader.CurrentTextValue;
            if (IsExpression(text))
                return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, text, this.DoConvertJustify);
            else if (TryGetJustify(text, out var justify))
            {
                this.SetValue(onStyle, justify);
                return true;
            }
            return false;
        }

        protected virtual bool DoConvertJustify(StyleBase onStyle, object value, out FlexJustify result)
        {
            if (value is FlexJustify fj) { result = fj; return true; }
            return TryGetJustify(value?.ToString() ?? string.Empty, out result);
        }

        public static bool TryGetJustify(string value, out FlexJustify justify)
        {
            switch (value.ToLower())
            {
                case "flex-start":    justify = FlexJustify.FlexStart;    return true;
                case "flex-end":      justify = FlexJustify.FlexEnd;      return true;
                case "center":        justify = FlexJustify.Center;       return true;
                case "space-between": justify = FlexJustify.SpaceBetween; return true;
                case "space-around":  justify = FlexJustify.SpaceAround;  return true;
                case "space-evenly":  justify = FlexJustify.SpaceEvenly;  return true;
                default:              justify = FlexJustify.FlexStart;    return false;
            }
        }
    }

    public class CSSAlignItemsParser : CSSStyleAttributeParser<FlexAlignMode>
    {
        public CSSAlignItemsParser() : base(CSSStyleItems.AlignItems, StyleKeys.FlexAlignItemsKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var text = reader.CurrentTextValue;
            if (IsExpression(text))
                return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, text, this.DoConvertAlign);
            else if (TryGetAlign(text, out var align))
            {
                this.SetValue(onStyle, align);
                return true;
            }
            return false;
        }

        protected virtual bool DoConvertAlign(StyleBase onStyle, object value, out FlexAlignMode result)
        {
            if (value is FlexAlignMode fa) { result = fa; return true; }
            return TryGetAlign(value?.ToString() ?? string.Empty, out result);
        }

        public static bool TryGetAlign(string value, out FlexAlignMode align)
        {
            switch (value.ToLower())
            {
                case "stretch":       align = FlexAlignMode.Stretch;      return true;
                case "flex-start":    align = FlexAlignMode.FlexStart;    return true;
                case "flex-end":      align = FlexAlignMode.FlexEnd;      return true;
                case "center":        align = FlexAlignMode.Center;       return true;
                case "baseline":      align = FlexAlignMode.Baseline;     return true;
                case "auto":          align = FlexAlignMode.Auto;         return true;
                case "space-between": align = FlexAlignMode.SpaceBetween; return true;
                case "space-around":  align = FlexAlignMode.SpaceAround;  return true;
                case "space-evenly":  align = FlexAlignMode.SpaceEvenly;  return true;
                default:              align = FlexAlignMode.Stretch;      return false;
            }
        }
    }

    public class CSSAlignContentParser : CSSStyleAttributeParser<FlexAlignMode>
    {
        public CSSAlignContentParser() : base(CSSStyleItems.AlignContent, StyleKeys.FlexAlignContentKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var text = reader.CurrentTextValue;
            if (IsExpression(text))
                return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, text, this.DoConvertAlign);
            else if (CSSAlignItemsParser.TryGetAlign(text, out var align))
            {
                this.SetValue(onStyle, align);
                return true;
            }
            return false;
        }

        protected virtual bool DoConvertAlign(StyleBase onStyle, object value, out FlexAlignMode result)
        {
            if (value is FlexAlignMode fa) { result = fa; return true; }
            return CSSAlignItemsParser.TryGetAlign(value?.ToString() ?? string.Empty, out result);
        }
    }

    public class CSSAlignSelfParser : CSSStyleAttributeParser<FlexAlignMode>
    {
        public CSSAlignSelfParser() : base(CSSStyleItems.AlignSelf, StyleKeys.FlexAlignSelfKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var text = reader.CurrentTextValue;
            if (IsExpression(text))
                return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, text, this.DoConvertAlign);
            else if (CSSAlignItemsParser.TryGetAlign(text, out var align))
            {
                this.SetValue(onStyle, align);
                return true;
            }
            return false;
        }

        protected virtual bool DoConvertAlign(StyleBase onStyle, object value, out FlexAlignMode result)
        {
            if (value is FlexAlignMode fa) { result = fa; return true; }
            return CSSAlignItemsParser.TryGetAlign(value?.ToString() ?? string.Empty, out result);
        }
    }

    public class CSSFlexGrowParser : CSSStyleAttributeParser<double>
    {
        public CSSFlexGrowParser() : base(CSSStyleItems.FlexGrow, StyleKeys.FlexGrowKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var text = reader.CurrentTextValue;
            if (IsExpression(text))
                return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, text, this.DoConvertGrow);
            else if (DoConvertGrow(onStyle, text, out var v))
            {
                this.SetValue(onStyle, v);
                return true;
            }
            return false;
        }

        protected virtual bool DoConvertGrow(StyleBase onStyle, object value, out double result)
        {
            if (TryParseDouble(value, out result))
            {
                result = Math.Max(0.0, result);
                return true;
            }
            return false;
        }

        internal static bool TryParseDouble(object value, out double result)
        {
            if (value is double d) { result = d; return true; }
            if (value != null && double.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                return true;
            result = 0.0;
            return false;
        }
    }

    public class CSSFlexShrinkParser : CSSStyleAttributeParser<double>
    {
        public CSSFlexShrinkParser() : base(CSSStyleItems.FlexShrink, StyleKeys.FlexShrinkKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var text = reader.CurrentTextValue;
            if (IsExpression(text))
                return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, text, this.DoConvertShrink);
            else if (DoConvertShrink(onStyle, text, out var v))
            {
                this.SetValue(onStyle, v);
                return true;
            }
            return false;
        }

        protected virtual bool DoConvertShrink(StyleBase onStyle, object value, out double result)
        {
            if (CSSFlexGrowParser.TryParseDouble(value, out result))
            {
                result = Math.Max(0.0, result);
                return true;
            }
            return false;
        }
    }

    public class CSSFlexBasisParser : CSSStyleAttributeParser<Unit>
    {
        public CSSFlexBasisParser() : base(CSSStyleItems.FlexBasis, StyleKeys.FlexBasisKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var text = reader.CurrentTextValue;
            if (string.Equals(text, "auto", StringComparison.OrdinalIgnoreCase))
            {
                onStyle.SetValue(StyleKeys.FlexBasisAutoKey, true);
                return true;
            }

            if (IsExpression(text))
            {
                onStyle.SetValue(StyleKeys.FlexBasisAutoKey, false);
                return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, text, this.DoConvertBasis);
            }

            if (DoConvertBasis(onStyle, text, out var unit))
            {
                this.SetValue(onStyle, unit);
                onStyle.SetValue(StyleKeys.FlexBasisAutoKey, false);
                return true;
            }
            return false;
        }

        protected virtual bool DoConvertBasis(StyleBase onStyle, object value, out Unit result)
        {
            if (value is Unit u) { result = u; return true; }
            return TryConvertToUnit(value, out result);
        }
    }

    public class CSSGapParser : CSSStyleAttributeParser<Unit>
    {
        public CSSGapParser() : base(CSSStyleItems.Gap, StyleKeys.FlexGapKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var first = reader.CurrentTextValue;

            if (IsExpression(first))
            {
                // Whole gap value is a single var()/calc() expression - mirror it onto both
                // row-gap and column-gap so it resolves independently at data-bind time.
                bool r1 = this.AttachExpressionBindingHandler(onStyle, StyleKeys.FlexRowGapKey, first, this.DoConvertUnit);
                bool r2 = this.AttachExpressionBindingHandler(onStyle, StyleKeys.FlexColumnGapKey, first, this.DoConvertUnit);
                return r1 || r2;
            }

            if (!Unit.TryParse(first, out var firstUnit))
                return false;

            // CSS gap: <row-gap> [<column-gap>]
            // gap is a shorthand that expands to row-gap + column-gap, writing the same keys
            // as those individual properties.  This lets CSS declaration order determine which
            // wins: e.g. "gap:10pt; column-gap:20pt" → column-gap overwrites gap's column value.
            if (reader.ReadNextValue())
            {
                var second = reader.CurrentTextValue;
                onStyle.SetValue(StyleKeys.FlexRowGapKey, firstUnit);

                if (IsExpression(second))
                    this.AttachExpressionBindingHandler(onStyle, StyleKeys.FlexColumnGapKey, second, this.DoConvertUnit);
                else if (Unit.TryParse(second, out var secondUnit))
                    onStyle.SetValue(StyleKeys.FlexColumnGapKey, secondUnit);
                else
                    onStyle.SetValue(StyleKeys.FlexColumnGapKey, firstUnit);
            }
            else
            {
                onStyle.SetValue(StyleKeys.FlexRowGapKey, firstUnit);
                onStyle.SetValue(StyleKeys.FlexColumnGapKey, firstUnit);
            }
            return true;
        }

        protected virtual bool DoConvertUnit(StyleBase onStyle, object value, out Unit result)
        {
            if (value is Unit u) { result = u; return true; }
            return TryConvertToUnit(value, out result);
        }
    }

    public class CSSRowGapParser : CSSStyleAttributeParser<Unit>
    {
        public CSSRowGapParser() : base(CSSStyleItems.RowGap, StyleKeys.FlexRowGapKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var text = reader.CurrentTextValue;
            if (IsExpression(text))
                return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, text, this.DoConvertUnit);
            else if (Unit.TryParse(text, out var gap))
            {
                this.SetValue(onStyle, gap);
                return true;
            }
            return false;
        }

        protected virtual bool DoConvertUnit(StyleBase onStyle, object value, out Unit result)
        {
            if (value is Unit u) { result = u; return true; }
            return TryConvertToUnit(value, out result);
        }
    }

    public class CSSFlexOrderParser : CSSStyleAttributeParser<int>
    {
        public CSSFlexOrderParser() : base(CSSStyleItems.Order, StyleKeys.FlexOrderKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var text = reader.CurrentTextValue;
            if (IsExpression(text))
                return this.AttachExpressionBindingHandler(onStyle, this.StyleAttribute, text, this.DoConvertOrder);
            else if (int.TryParse(text, out var order))
            {
                this.SetValue(onStyle, order);
                return true;
            }
            return false;
        }

        protected virtual bool DoConvertOrder(StyleBase onStyle, object value, out int result)
        {
            if (value is int i) { result = i; return true; }
            if (value != null && int.TryParse(value.ToString(), out result))
                return true;
            result = 0;
            return false;
        }
    }

    /// <summary>
    /// Parses the flex shorthand: flex: [grow] [shrink] [basis] | none | auto
    /// </summary>
    /// <remarks>
    /// var()/calc() is not supported for the shorthand itself (grow/shrink/basis packed into
    /// one expression is ambiguous to unpack generically) - use the flex-grow, flex-shrink and
    /// flex-basis longhand properties individually, which all support expressions.
    /// </remarks>
    public class CSSFlexShorthandParser : CSSStyleValueParser
    {
        public CSSFlexShorthandParser() : base(CSSStyleItems.Flex) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (!reader.ReadNextValue())
                return false;

            var first = reader.CurrentTextValue;

            if (string.Equals(first, "none", StringComparison.OrdinalIgnoreCase))
            {
                onStyle.SetValue(StyleKeys.FlexGrowKey, 0.0);
                onStyle.SetValue(StyleKeys.FlexShrinkKey, 0.0);
                onStyle.SetValue(StyleKeys.FlexBasisAutoKey, true);
                return true;
            }

            if (string.Equals(first, "auto", StringComparison.OrdinalIgnoreCase))
            {
                onStyle.SetValue(StyleKeys.FlexGrowKey, 1.0);
                onStyle.SetValue(StyleKeys.FlexShrinkKey, 1.0);
                onStyle.SetValue(StyleKeys.FlexBasisAutoKey, true);
                return true;
            }

            if (!double.TryParse(first, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var grow))
                return false;

            onStyle.SetValue(StyleKeys.FlexGrowKey, Math.Max(0.0, grow));
            onStyle.SetValue(StyleKeys.FlexShrinkKey, 1.0);
            onStyle.SetValue(StyleKeys.FlexBasisAutoKey, false);

            if (reader.ReadNextValue())
            {
                if (double.TryParse(reader.CurrentTextValue, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var shrink))
                {
                    onStyle.SetValue(StyleKeys.FlexShrinkKey, Math.Max(0.0, shrink));

                    if (reader.ReadNextValue())
                    {
                        var basisText = reader.CurrentTextValue;
                        if (string.Equals(basisText, "auto", StringComparison.OrdinalIgnoreCase))
                            onStyle.SetValue(StyleKeys.FlexBasisAutoKey, true);
                        else if (Unit.TryParse(basisText, out var basis))
                            onStyle.SetValue(StyleKeys.FlexBasisKey, basis);
                    }
                }
            }

            return true;
        }
    }
}
