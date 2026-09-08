using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the components text decoration option based on the CSS names
    /// </summary>
    public class CSSDisplayParser : CSSEnumStyleParser<DisplayMode>
    {
        public CSSDisplayParser()
            : base(CSSStyleItems.Display, StyleKeys.PositionDisplayKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            DisplayMode display;
            if (reader.ReadNextValue())
            {
                string value = reader.CurrentTextValue;

                if (IsExpression(value))
                {
                    result = AttachExpressionBindingHandler(onStyle, this.StyleAttribute, value, DoConvertPosition);
                }
                else if (reader.ReadNextValue())
                {
                    // CSS Display Module Level 3 two-value syntax, e.g. "inline flex" -
                    // <display-outside> <display-inside>. Only valid if it maps onto one of
                    // the legacy single-keyword modes we actually support (see
                    // TryGetTwoValueDisplayEnum) - an unrecognised pairing is a parse failure,
                    // not a silent fallback to just the first word.
                    string second = reader.CurrentTextValue;
                    if (TryGetTwoValueDisplayEnum(value, second, out display))
                    {
                        this.SetValue(onStyle, display);
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else if (TryGetDisplayEnum(value, out display))
                {
                    this.SetValue(onStyle, display);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
                result = false;

            return result;
        }


        protected bool DoConvertPosition(StyleBase style, object value, out DisplayMode display)
        {
            if(null == value)
            {
                display = DisplayMode.Block;
                return false;
            }
            else if(value is DisplayMode p)
            {
                display = p;
                return true;
            }
            else if(TryGetDisplayEnum(value.ToString(), out display))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryGetDisplayEnum(string value, out DisplayMode display)
        {
            switch (value.ToLower())
            {
                case ("inline"):
                    display = DisplayMode.Inline;
                    return true;
                case("inline-block"):
                    display = DisplayMode.InlineBlock;
                    return true;
                case ("block"):
                    display = DisplayMode.Block;
                    return true;
                case("table-cell"):
                    display = DisplayMode.TableCell;
                    return true;
                case ("none"):
                    display = DisplayMode.Invisible;
                    return true;
                case ("flex"):
                    display = DisplayMode.FlexBox;
                    return true;
                case ("grid"):
                    display = DisplayMode.FlexGrid;
                    return true;
                case ("table"):
                    display = DisplayMode.Table;
                    return true;
                case ("table-row"):
                    display = DisplayMode.TableRow;
                    return true;
                default:
                    display = DisplayMode.Block;
                    return false;

            }
        }

        /// <summary>
        /// Maps the CSS Display Module Level 3 two-value syntax
        /// (&lt;display-outside&gt; &lt;display-inside&gt;, e.g. "block flex", "inline flow-root")
        /// onto the single legacy DisplayMode it's equivalent to, for each mode we actually
        /// support. Only the "outside inside" order is recognised, matching the order every
        /// real stylesheet and browser devtools panel actually emits.
        ///
        /// table-cell and table-row have no two-value form in the spec (they're "internal"
        /// display types, not composed from an outside+inside pair), so they're intentionally
        /// absent here - only reachable via the single-keyword TryGetDisplayEnum.
        ///
        /// "inline flex", "inline grid" and "inline table" are recognised leniently: their
        /// strict CSS3 equivalents are the distinct inline-flex/inline-grid/inline-table modes,
        /// which we don't model separately (see DisplayMode), so they resolve to the same
        /// FlexBox/FlexGrid/Table mode as their block-outside counterpart - the "inline"
        /// outer behaviour (flowing with surrounding inline content) isn't applied.
        /// </summary>
        public static bool TryGetTwoValueDisplayEnum(string outside, string inside, out DisplayMode display)
        {
            switch (outside.ToLower(), inside.ToLower())
            {
                case ("inline", "flow"):
                    display = DisplayMode.Inline;
                    return true;
                case ("block", "flow"):
                    display = DisplayMode.Block;
                    return true;
                case ("inline", "flow-root"):
                    display = DisplayMode.InlineBlock;
                    return true;
                case ("block", "flow-root"):
                    // flow-root's distinguishing feature is establishing a new block
                    // formatting context - we have no distinct mode for that, so the
                    // nearest supported equivalent is plain Block.
                    display = DisplayMode.Block;
                    return true;
                case ("block", "flex"):
                case ("inline", "flex"):
                    display = DisplayMode.FlexBox;
                    return true;
                case ("block", "grid"):
                case ("inline", "grid"):
                    display = DisplayMode.FlexGrid;
                    return true;
                case ("block", "table"):
                case ("inline", "table"):
                    display = DisplayMode.Table;
                    return true;
                default:
                    display = DisplayMode.Block;
                    return false;
            }
        }

    }
}
