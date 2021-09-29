using System;
using Scryber.Html;
using Scryber.Drawing;

//TODO: Add support for expressions

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSFontParser : CSSStyleValueParser
    {
        public static readonly PDFFont CaptionFont = new PDFFont("Helvetica", 12, FontWeights.Bold, Drawing.FontStyle.Regular);
        public static readonly PDFFont IconFont = new PDFFont("Helvetica", 8, FontWeights.Bold, Drawing.FontStyle.Regular);
        public static readonly PDFFont MenuFont = new PDFFont("Times", 10, FontWeights.Regular, Drawing.FontStyle.Regular);
        public static readonly PDFFont MessageBoxFont = new PDFFont("Times", 10, FontWeights.Bold, Drawing.FontStyle.Regular);
        public static readonly PDFFont SmallCaptionFont = new PDFFont("Helvetica", 8, FontWeights.Regular, Drawing.FontStyle.Italic);
        public static readonly PDFFont StatusBarFont = new PDFFont("Courier", 10, FontWeights.Bold, Drawing.FontStyle.Regular);

        public CSSFontParser()
            : base(CSSStyleItems.Font)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = false;
            if (!reader.ReadNextValue())
                return result;

            if (IsExpression(reader.CurrentTextValue))
                throw new InvalidOperationException("The compound 'font' css selector does not currently support expressions");

            //compound values - caption|icon|menu|message-box|small-caption|status-bar
            // invalid values - initial|inherit
            switch (reader.CurrentTextValue.ToLower())
            {
                case ("caption"):
                    ApplyFont(onStyle, CaptionFont);
                    return true;

                case ("icon"):
                    ApplyFont(onStyle, IconFont);
                    return true;

                case ("menu"):
                    ApplyFont(onStyle, MenuFont);
                    return true;

                case ("message-box"):
                    ApplyFont(onStyle, MessageBoxFont);
                    return true;

                case ("small-caption"):
                    ApplyFont(onStyle, SmallCaptionFont);
                    return true;

                case ("status-bar"):
                    ApplyFont(onStyle, StatusBarFont);
                    return true;

                default:
                    //Just fall back to parsing each individual item
                    break;
            }

            result = true;

            //discreet values - font-style font-variant font-weight font-size/line-height font-family
            Drawing.FontStyle italic;
            int weight;
            PDFUnit fsize, lineheight;
            double relativeLeading;



            if (CSSFontStyleParser.TryGetFontStyle(reader.CurrentTextValue, out italic))
            {
                onStyle.Font.FontFaceStyle = italic;
                if (!reader.MoveToNextValue())
                    return result;
            }

            if (IsDefinedFontVariant(reader.CurrentTextValue))
            {
                //We dont support variant but we should honour it being there.
                if (!reader.MoveToNextValue())
                    return result;
            }
            if (CSSFontWeightParser.TryGetFontWeight(reader.CurrentTextValue, out weight))
            {
                onStyle.Font.FontWeight = weight;
                if (!reader.MoveToNextValue())
                    return result;
            }

            //font-size/line height are special and will return the both values from the reader if both are present
            //other wise it is just the font-size
            if (reader.CurrentTextValue.IndexOf('/') > 0)
            {
                // we have both
                string part = reader.CurrentTextValue.Substring(0, reader.CurrentTextValue.IndexOf('/'));
                if (ParseCSSUnit(part, out fsize))
                    onStyle.Font.FontSize = fsize;
                else
                    result = false;

                part = reader.CurrentTextValue.Substring(reader.CurrentTextValue.IndexOf('/') + 1);

                //if we have a simple double value then it is relative leading based on font size.
                if (double.TryParse(part, out relativeLeading))
                    onStyle.Text.Leading = onStyle.Font.FontSize * relativeLeading;
                else if (ParseCSSUnit(part, out lineheight))
                    onStyle.Text.Leading = lineheight;
                else
                    result = false;

                if (!reader.MoveToNextValue())
                    return result;
            }
            else if (ParseCSSUnit(reader.CurrentTextValue, out fsize))
            {
                onStyle.Font.FontSize = fsize;
                if (!reader.MoveToNextValue())
                    return result;
            }

            //last one is multiple font families
            bool foundFamily = false;
            PDFFontSelector root = null;

            do
            {
                if (IsExpression(reader.CurrentTextValue))
                    throw new InvalidOperationException("The compound 'font' css selector does not currently support expressions");

                PDFFontSelector selector;

                PDFFontSelector curr = null;

                if (CSSFontFamilyParser.TryGetActualFontFamily(reader.CurrentTextValue, out selector))
                {
                    if (null == root)
                        root = selector;

                    if (null == curr)
                        curr = selector;
                    else
                    {
                        curr.Next = selector;
                        curr = selector;
                    }
                    foundFamily = true;
                }

            }
            while (reader.MoveToNextValue());

            if (foundFamily)
            {
                onStyle.SetValue(StyleKeys.FontFamilyKey, root);
            }
            else
                result = false;

            return result;
        }

        private void ApplyFont(Style onStyle, PDFFont font)
        {
            onStyle.SetValue(StyleKeys.FontFamilyKey, font.Selector);
            onStyle.SetValue(StyleKeys.FontSizeKey, font.Size);
            onStyle.SetValue(StyleKeys.FontWeightKey, font.FontWeight);
            onStyle.SetValue(StyleKeys.FontStyleKey, font.FontStyle);
        }



        private bool IsDefinedFontVariant(string value)
        {
            // font-variant: normal|small-caps|initial|inherit;
            switch (value.ToLower())
            {
                case ("normal"):
                case ("small-caps"):
                case ("initial"):
                case ("inherit"):
                    return true;
                default:
                    return false;
            }
        }
    }
}
