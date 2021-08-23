using System;
using Scryber.Html;
using Scryber.Drawing;
using System.Reflection.Metadata.Ecma335;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the components text decoration option based on the CSS names
    /// </summary>
    public class CSSListStyleTypeParser : CSSEnumStyleParser<ListNumberingGroupStyle>
    {
        public CSSListStyleTypeParser()
            : base(CSSStyleItems.ListStyleType, StyleKeys.ListNumberStyleKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            ListNumberingGroupStyle type;
            if (reader.ReadNextValue())
            {
                var val = reader.CurrentTextValue;

                if (IsExpression(val))
                {
                    result = AttachExpressionBindingHandler(onStyle, this.StyleAttribute, val, DoConvertListType);
                }
                else if (TryGetListTypeEnum(reader.CurrentTextValue, out type))
                {
                    this.SetValue(onStyle, type);
                    result = true;
                }
                else
                    result = false;
            }
            else
                result = false;

            return result;
        }

        protected bool DoConvertListType(StyleBase style, object value, out ListNumberingGroupStyle type)
        {
            if(null == value)
            {
                type = ListNumberingGroupStyle.None;
                return false;
            }
            else if(value is ListNumberingGroupStyle l)
            {
                type = l;
                return true;
            }
            else if(TryGetListTypeEnum(value.ToString(), out type))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool IsDecorationEnum(string value)
        {
            ListNumberingGroupStyle type;
            return TryGetListTypeEnum(value, out type);
        }

        public static bool TryGetListTypeEnum(string value, out ListNumberingGroupStyle type)
        {
            switch (value.ToLower())
            {
                case ("disc"):
                case ("circle"):
                    type = ListNumberingGroupStyle.Bullet;
                    return true;

                case ("decimal"):
                    type = ListNumberingGroupStyle.Decimals;
                    return true;

                case ("none"):
                    type = ListNumberingGroupStyle.None;
                    return true;

                case ("lower-roman"):
                    type = ListNumberingGroupStyle.LowercaseRoman;
                    return true;

                case ("lower-alpha"):
                    type = ListNumberingGroupStyle.LowercaseLetters;
                    return true;

                case ("upper-roman"):
                    type = ListNumberingGroupStyle.UppercaseRoman;
                    return true;

                case ("upper-alpha"):
                    type = ListNumberingGroupStyle.UppercaseLetters;
                    return true;
                default:
                    type = ListNumberingGroupStyle.Decimals;
                    return false;

            }
        }

    }
}
