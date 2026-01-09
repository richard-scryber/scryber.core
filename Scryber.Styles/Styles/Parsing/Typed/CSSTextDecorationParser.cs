using System;
using Scryber.Html;
using Scryber.Text;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the components text decoration option based on the CSS names
    /// </summary>
    public class CSSTextDecorationParser : CSSEnumStyleParser<Text.TextDecoration>
    {
        public CSSTextDecorationParser()
            : base(CSSStyleItems.TextDecoration, StyleKeys.TextDecorationKey)
        {
        }


        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            Text.TextDecoration decor;
            TextDecoration final = TextDecoration.None;
            bool hasExpr = false;

            while (reader.ReadNextValue())
            {
                string val = reader.CurrentTextValue;

                if (IsExpression(val))
                {
                    if(final != TextDecoration.None)
                        throw new InvalidOperationException("Cannot apply expressions and text decoration values in the same style selector");
                    
                    result = AttachExpressionBindingHandler(onStyle, this.StyleAttribute, val, DoConvertDecoration);
                    hasExpr = result;
                }
                else if (TryGetOneDecoration(val, out decor))
                {
                    if (hasExpr)
                        throw new InvalidOperationException("Cannot apply expressions and text decoration values in the same style selector");

                    final |= decor;
                    result &= true;
                }
                else
                {
                    result = false; //not for any
                }
            }

            if (result && !hasExpr)
                onStyle.SetValue(StyleKeys.TextDecorationKey, final);

            return result;
        }

        protected bool DoConvertDecoration(StyleBase style, object value, out TextDecoration decor)
        {
            if(null == value)
            {
                decor = TextDecoration.None;
                return false;
            }
            else if(value is TextDecoration d)
            {
                decor = d;
                return true;
            }
            else if(TryGetDecorations(value.ToString(), out decor))
            {
                return true;
            }
            else
            {
                decor = TextDecoration.None;
                return false;
            }
        }

        public static bool IsDecorationEnum(string value)
        {
            Text.TextDecoration decor;
            return TryGetOneDecoration(value, out decor);
        }

        public static bool TryGetDecorations(string value, out TextDecoration decoration)
        {
            decoration = TextDecoration.None;

            if (value.IndexOf(' ') > 0)
            {
                var all = value.Split(' ');
                TextDecoration d;
                bool result = true;

                foreach (var one in all)
                {
                    if (!string.IsNullOrEmpty(one))
                    {
                        if (TryGetOneDecoration(one, out d))
                            decoration |= d;
                        else
                            result = false;
                    }
                }
                return result;
            }
            else if (TryGetOneDecoration(value, out decoration))
                return true;
            else
                return false;
        }


        public static bool TryGetOneDecoration(string value, out TextDecoration decoration)
        {
            switch (value.ToLower())
            {
                case ("underline"):
                    decoration = Text.TextDecoration.Underline;
                    return true;

                case ("overline"):
                    decoration = Text.TextDecoration.Overline;
                    return true;

                case ("line-through"):
                    decoration = Text.TextDecoration.StrikeThrough;
                    return true;

                case ("none"):
                    decoration = Text.TextDecoration.None;
                    return true;

                default:
                    decoration = Text.TextDecoration.None;
                    return false;

            }
        }

    }
}
