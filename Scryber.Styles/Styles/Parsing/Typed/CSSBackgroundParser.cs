using System;
using Scryber.Html;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the background values for a component based on the shorthand css background property
    /// </summary>
    public class CSSBackgroundParser : CSSStyleValueParser
    {
        public CSSBackgroundParser()
            : base(CSSStyleItems.Background)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {


            int count = 0;
            int failed = 0;
            PatternRepeat repeat;

            while (reader.ReadNextValue())
            {
                count++;
                if (IsExpression(reader.CurrentTextValue))
                {
                    if (count == 1)
                    {
                        if (!this.AttachExpressionBindingHandler(onStyle, StyleKeys.BgColorKey, reader.CurrentTextValue, DoConvertBackgroundColor))
                            failed++;
                    }
                    else if(count == 2)
                    {
                        if (!this.AttachExpressionBindingHandler(onStyle, StyleKeys.BgImgSrcKey, reader.CurrentTextValue, DoConvertBackgroundImage))
                            failed++;
                    }
                    else if(count == 3)
                    {
                        if (!this.AttachExpressionBindingHandler(onStyle, StyleKeys.BgRepeatKey, reader.CurrentTextValue, DoConvertBackgroundRepeat))
                            failed++;
                    }
                }
                else if (IsColor(reader.CurrentTextValue))
                {
                    PDFColor color;
                    double? opacity;
                    if (ParseCSSColor(reader.CurrentTextValue, out color, out opacity))
                    {
                        onStyle.Background.Color = color;
                        onStyle.Background.FillStyle = Drawing.FillType.Solid;
                        if (opacity.HasValue)
                            onStyle.Background.Opacity = opacity.Value;
                    }
                    else
                        failed++;
                }
                else if (IsUrl(reader.CurrentTextValue))
                {
                    string url;
                    if (ParseCSSUrl(reader.CurrentTextValue, out url))
                    {
                        onStyle.Background.ImageSource = url;
                        onStyle.Background.FillStyle = Drawing.FillType.Image;
                    }
                    else
                        failed++;
                }
                else if (CSSBackgroundRepeatParser.TryGetRepeatEnum(reader.CurrentTextValue, out repeat))
                {
                    onStyle.Background.PatternRepeat = repeat;
                }
                else
                    failed++;

                //TODO: Parse sizes and positions
            }

            return count > 0 && failed == 0;
        }

        protected bool DoConvertBackgroundColor(StyleBase onStyle, object value, out PDFColor result)
        {
            double? opacity;

            if(null == value)
            {
                result = PDFColor.Transparent;
                return false;
            }
            else if(value is PDFColor color)
            {
                result = color;
                return true;
            }
            else if(ParseCSSColor(value.ToString(), out result, out opacity))
            {
                if (opacity.HasValue)
                    onStyle.SetValue(StyleKeys.BgOpacityKey, opacity.Value);
                return true;
            }
            else
            {
                result = PDFColor.Transparent;
                return false;
            }
        }

        protected bool DoConvertBackgroundImage(StyleBase onStyle, object value, out string result)
        {
            if (null == value)
            {
                result = string.Empty;
                return false;
            }
            else
            {
                var str = value.ToString();
                result = str;
                return true;
            }
        }

        protected bool DoConvertBackgroundRepeat(StyleBase onStyle, object value, out PatternRepeat result)
        {
            if (null == value)
            {
                result = PatternRepeat.RepeatBoth;
                return false;
            }
            else if(CSSBackgroundRepeatParser.TryGetRepeatEnum(value.ToString(), out result))
            {
                return true;
            }
            else
            {
                result = PatternRepeat.RepeatBoth;
                return false;
            }
        }
    }
}
