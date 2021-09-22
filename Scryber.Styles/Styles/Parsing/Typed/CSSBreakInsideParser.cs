using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSBreakInsideParser : CSSStyleValueParser
    {
        public PDFStyleKey<OverflowSplit> SplitKey { get; set; }

        public PDFStyleKey<OverflowAction> ActionKey { get; set; }

        public CSSBreakInsideParser(string cssName, PDFStyleKey<OverflowAction> actionKey, PDFStyleKey<OverflowSplit> splitKey)
            : base(cssName)
        {
            this.ActionKey = actionKey;
            this.SplitKey = splitKey;
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result;

            if (!reader.ReadNextValue())
                result = false;
            else
            {
                var val = reader.CurrentTextValue;
                OverflowAction? action;
                OverflowSplit? split;

                if (IsExpression(val))
                {
                    result = AttachExpressionBindingHandler(onStyle, this.SplitKey, val, DoConvertSplit);
                    result &= AttachExpressionBindingHandler(onStyle, this.ActionKey, val, DoConvertAction);

                }
                else if (TryParseOverflow(val, out split, out action))
                {
                    if(split.HasValue)
                        onStyle.SetValue(this.SplitKey, split.Value);
                    if(action.HasValue)
                        onStyle.SetValue(this.ActionKey, action.Value);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;

        }

        protected bool DoConvertSplit(StyleBase style, object value, out OverflowSplit split)
        {
            OverflowAction? actions;
            OverflowSplit? splits;
            if(null == value)
            {
                split = OverflowSplit.Any;
                return false;
            }
            else if(value is OverflowSplit s)
            {
                split = s;
                return true;
            }
            else if(TryParseOverflow(value.ToString(), out splits, out actions) && splits.HasValue)
            {
                split = splits.Value;
                return true;
            }
            else
            {
                split = OverflowSplit.Any;
                return false;
            }
        }

        protected bool DoConvertAction(StyleBase style, object value, out OverflowAction action)
        {
            OverflowAction? actions;
            OverflowSplit? splits;

            if (null == value)
            {
                action = OverflowAction.None;
                return false;
            }
            else if (value is OverflowAction a)
            {
                action = a;
                return true;
            }
            else if (TryParseOverflow(value.ToString(), out splits, out actions) && actions.HasValue)
            {
                action = actions.Value;
                return true;
            }
            else
            {
                action = OverflowAction.None;
                return false;
            }
        }


        public static bool TryParseOverflow(string value, out OverflowSplit? split, out OverflowAction? action)
        {
            bool result;
            if (value == "auto")
            {
                split = OverflowSplit.Any;
                action = null;

                result = true;
            }
            else if (value == "avoid")
            {
                split = OverflowSplit.Never;
                action = OverflowAction.NewPage;

                result = true;
            }
            else if(value == "initial")
            {
                action = OverflowAction.Truncate;
                split = OverflowSplit.Never;

                result = true;
            }
            else
            {
                split = null;
                action = null;

                result = false;
            }

            return result;
        }
    }
}
