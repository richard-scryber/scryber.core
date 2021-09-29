using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the components italic option based on the CSS names
    /// </summary>
    public class CSSFontWeightParser : CSSStyleValueParser
    {
        public CSSFontWeightParser()
            : base(CSSStyleItems.FontWeight)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            int weight = FontWeights.Regular;
            bool result = true;
            if (reader.ReadNextValue())
            {
                var str = reader.CurrentTextValue;
                if (IsExpression(str))
                {
                    result = AttachExpressionBindingHandler(onStyle, StyleKeys.FontWeightKey, str, DoConvertFontWeight);
                }
                else if (TryGetFontWeight(reader.CurrentTextValue, out weight))
                {
                    onStyle.SetValue(StyleKeys.FontWeightKey, weight);
                }
                else
                    result = false;
            }
            else
                result = false;

            return result;
        }

        public bool DoConvertFontWeight(StyleBase onStyle, object value, out int fontWeight)
        {
            if(null == value)
            {
                fontWeight = FontWeights.Regular;
                return false;
            }
            else if(value is bool bold)
            {
                fontWeight = FontWeights.Bold;
                return true;
            }
            else if(value is int)
            {
                fontWeight = (int)value;
                return true;
            }
            else if(TryGetFontWeight(value.ToString(), out fontWeight))
            {
                return true;
            }
            else
            {
                fontWeight = FontWeights.Regular;
                return false;
            }
        }

        public static bool IsFontWeight(string value)
        {
            int val;
            return TryGetFontWeight(value, out val);
        }

        public static bool TryGetFontWeight(string value, out int weight)
        {
            switch (value.ToLower())
            {
                case ("black"):
                    //900
                    weight = FontWeights.Black;
                    return true;

                case ("bolder"):
                case ("extra-bold"):
                    //800
                    weight = FontWeights.ExtraBold;
                    return true;

                case ("bold"):
                    //700
                    weight = FontWeights.Bold;
                    return true;

                case ("semi-bold"):
                    //600
                    weight = FontWeights.SemiBold;
                    return true;

                case ("medium"):
                    //500
                    weight = FontWeights.Medium;
                    return true;

                case ("normal"):
                case ("regular"):
                    //400
                    weight = FontWeights.Regular;
                    return true;

                case ("lighter"):
                    //300
                    weight = FontWeights.Light;
                    return true;

                case ("extra-light"):
                    //200
                    weight = FontWeights.ExtraLight;
                    return true;
                    
                case ("thin"):
                    //100
                    weight = FontWeights.Thin;
                    return true;
                case ("100"):
                case ("200"):
                case ("300"):
                case ("400"):
                case ("500"):
                case ("600"):
                case ("700"):
                case ("800"):
                case ("900"):
                    weight = int.Parse(value);
                    return true;

                default:

                    weight = FontWeights.Regular;
                    return false;


            }
        }

    }
}
