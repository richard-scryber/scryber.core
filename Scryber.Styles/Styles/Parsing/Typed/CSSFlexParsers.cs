using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSFlexDirectionParser : CSSStyleAttributeParser<FlexDirection>
    {
        public CSSFlexDirectionParser() : base(CSSStyleItems.FlexDirection, StyleKeys.FlexDirectionKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (reader.ReadNextValue() && TryGetDirection(reader.CurrentTextValue, out var dir))
            {
                this.SetValue(onStyle, dir);
                return true;
            }
            return false;
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
            if (reader.ReadNextValue() && TryGetWrap(reader.CurrentTextValue, out var wrap))
            {
                this.SetValue(onStyle, wrap);
                return true;
            }
            return false;
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
            if (reader.ReadNextValue() && TryGetJustify(reader.CurrentTextValue, out var justify))
            {
                this.SetValue(onStyle, justify);
                return true;
            }
            return false;
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
            if (reader.ReadNextValue() && TryGetAlign(reader.CurrentTextValue, out var align))
            {
                this.SetValue(onStyle, align);
                return true;
            }
            return false;
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
            if (reader.ReadNextValue() && CSSAlignItemsParser.TryGetAlign(reader.CurrentTextValue, out var align))
            {
                this.SetValue(onStyle, align);
                return true;
            }
            return false;
        }
    }

    public class CSSAlignSelfParser : CSSStyleAttributeParser<FlexAlignMode>
    {
        public CSSAlignSelfParser() : base(CSSStyleItems.AlignSelf, StyleKeys.FlexAlignSelfKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (reader.ReadNextValue() && CSSAlignItemsParser.TryGetAlign(reader.CurrentTextValue, out var align))
            {
                this.SetValue(onStyle, align);
                return true;
            }
            return false;
        }
    }

    public class CSSFlexGrowParser : CSSStyleAttributeParser<double>
    {
        public CSSFlexGrowParser() : base(CSSStyleItems.FlexGrow, StyleKeys.FlexGrowKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (reader.ReadNextValue() && double.TryParse(reader.CurrentTextValue, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var v))
            {
                this.SetValue(onStyle, Math.Max(0.0, v));
                return true;
            }
            return false;
        }
    }

    public class CSSFlexShrinkParser : CSSStyleAttributeParser<double>
    {
        public CSSFlexShrinkParser() : base(CSSStyleItems.FlexShrink, StyleKeys.FlexShrinkKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (reader.ReadNextValue() && double.TryParse(reader.CurrentTextValue, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var v))
            {
                this.SetValue(onStyle, Math.Max(0.0, v));
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
            if (Unit.TryParse(text, out var unit))
            {
                this.SetValue(onStyle, unit);
                onStyle.SetValue(StyleKeys.FlexBasisAutoKey, false);
                return true;
            }
            return false;
        }
    }

    public class CSSGapParser : CSSStyleAttributeParser<Unit>
    {
        public CSSGapParser() : base(CSSStyleItems.Gap, StyleKeys.FlexGapKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (reader.ReadNextValue() && Unit.TryParse(reader.CurrentTextValue, out var gap))
            {
                this.SetValue(onStyle, gap);
                return true;
            }
            return false;
        }
    }

    public class CSSRowGapParser : CSSStyleAttributeParser<Unit>
    {
        public CSSRowGapParser() : base(CSSStyleItems.RowGap, StyleKeys.FlexRowGapKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (reader.ReadNextValue() && Unit.TryParse(reader.CurrentTextValue, out var gap))
            {
                this.SetValue(onStyle, gap);
                return true;
            }
            return false;
        }
    }

    public class CSSFlexOrderParser : CSSStyleAttributeParser<int>
    {
        public CSSFlexOrderParser() : base(CSSStyleItems.Order, StyleKeys.FlexOrderKey) { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (reader.ReadNextValue() && int.TryParse(reader.CurrentTextValue, out var order))
            {
                this.SetValue(onStyle, order);
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Parses the flex shorthand: flex: [grow] [shrink] [basis] | none | auto
    /// </summary>
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
