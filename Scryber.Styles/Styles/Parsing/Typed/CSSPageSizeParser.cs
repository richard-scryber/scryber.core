using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSPageSizeParser : CSSStyleValueParser
    {
        public CSSPageSizeParser() : base(CSSStyleItems.PageSize)
        { }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            bool result = true;
            if (reader.MoveToNextValue())
            {
                var val = reader.CurrentTextValue;

                PaperSize paper;
                PDFUnit width;
                PDFUnit height;
                PaperOrientation orient;
                string expr1 = null, expr2 = null;

                if (IsExpression(val))
                {
                    //first expression could be anything
                    expr1 = val;

                    if (!reader.MoveToNextValue())
                    {
                        //we have no second expression
                        result &= AttachExpressionBindingHandler(style, StyleKeys.PagePaperSizeKey, val, DoConvertPaper);
                        result &= AttachExpressionBindingHandler(style, StyleKeys.PageOrientationKey, val, DoConvertPageOrientation);
                        result &= AttachExpressionBindingHandler(style, StyleKeys.PageWidthKey, val, DoConvertPageSizeValue);
                        result &= AttachExpressionBindingHandler(style, StyleKeys.PageHeightKey, val, DoConvertPageSizeValue);
                    }
                    else
                    {
                        //we have a second value
                        val = reader.CurrentTextValue;
                        expr2 = val;

                        //Can either be a first expression of paper and second of orientation
                        //Or a first expression of width and a second of height

                        if (IsExpression(val))
                        {
                            //attach to expr1

                            result &= AttachExpressionBindingHandler(style, StyleKeys.PagePaperSizeKey, expr1, DoConvertPaper);
                            result &= AttachExpressionBindingHandler(style, StyleKeys.PageWidthKey, expr1, DoConvertPageSizeValue);

                            //can either be orientation or height
                            result &= AttachExpressionBindingHandler(style, StyleKeys.PageOrientationKey, expr2, DoConvertPageOrientation);
                            result &= AttachExpressionBindingHandler(style, StyleKeys.PageHeightKey, expr2, DoConvertPageSizeValue);
                        }
                        else if (Enum.TryParse<PaperOrientation>(expr2, true, out orient))
                        {
                            //Second is a paper orientation - so first is a paper size
                            result &= AttachExpressionBindingHandler(style, StyleKeys.PagePaperSizeKey, expr1, DoConvertPaper);

                            style.PageStyle.PaperOrientation = orient;
                            result &= true;
                        }
                        else if (ParseCSSUnit(reader.CurrentTextValue, out height))
                        {
                            //Second is a unit so first must evaluate to one too, for the width
                            result &= AttachExpressionBindingHandler(style, StyleKeys.PageWidthKey, expr1, DoConvertPageSizeValue);
                            style.PageStyle.Height = height;
                        }
                        else
                        {
                            //Don't know what this is.
                            //Be nice and bind the first value
                            result &= AttachExpressionBindingHandler(style, StyleKeys.PagePaperSizeKey, expr1, DoConvertPaper);
                            result &= AttachExpressionBindingHandler(style, StyleKeys.PageOrientationKey, expr1, DoConvertPageOrientation);
                            result &= AttachExpressionBindingHandler(style, StyleKeys.PageWidthKey, expr1, DoConvertPageSizeValue);
                            result &= AttachExpressionBindingHandler(style, StyleKeys.PageHeightKey, expr1, DoConvertPageSizeValue);

                            result = false;
                        }
                    }
                    return result;
                }
                else if (Enum.TryParse<PaperSize>(val, out paper))
                {
                    style.PageStyle.PaperSize = paper;

                    if (reader.MoveToNextValue())
                    {
                        val = reader.CurrentTextValue;

                        if (IsExpression(val))
                        {
                            //Can only be orientation
                            result &= AttachExpressionBindingHandler(style, StyleKeys.PageOrientationKey, val, DoConvertPageOrientation);
                        }
                        else if (Enum.TryParse<PaperOrientation>(reader.CurrentTextValue, true, out orient))
                        {
                            style.PageStyle.PaperOrientation = orient;
                            result &= true;
                        }
                        else
                            result = false;
                    }
                    return result;
                }
                else if (Enum.TryParse<PaperOrientation>(val, true, out orient))
                {
                    style.PageStyle.PaperOrientation = orient;
                    //No other values supported.
                    return true;
                }
                else if (ParseCSSUnit(val, out width))
                {
                    style.PageStyle.Width = width;

                    if (reader.MoveToNextValue())
                    {
                        //Must be an expression or a height
                        val = reader.CurrentTextValue;

                        if (IsExpression(val))
                        {
                            result &= AttachExpressionBindingHandler(style, StyleKeys.PageHeightKey, val, DoConvertPageSizeValue);
                        }
                        else if (ParseCSSUnit(val, out height))
                        {
                            style.PageStyle.Height = height;
                        }
                        else
                            result = false;
                    }

                    return result;
                }
                else
                    result = false;
            }
            //Could not parse the first item
            return false;
        }

        protected bool DoConvertPaper(StyleBase style, object value, out PaperSize pageSize)
        {
            if (null == value)
            {
                pageSize = PaperSize.A4;
                return false;
            }
            else if(value is PaperSize p)
            {
                pageSize = p;
                return true;
            }
            else if(Enum.TryParse(value.ToString(), out pageSize))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool DoConvertPageOrientation(StyleBase style, object value, out PaperOrientation orientation)
        {
            if (null == value)
            {
                orientation = PaperOrientation.Portrait;
                return false;
            }
            else if (value is PaperOrientation p)
            {
                orientation = p;
                return true;
            }
            else if (Enum.TryParse(value.ToString(), out orientation))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool DoConvertPageSizeValue(StyleBase style, object value, out PDFUnit size)
        {
            if(null == value)
            {
                size = PDFUnit.Empty;
                return false;
            }
            else if(value is PDFUnit u)
            {
                size = u;
                return true;
            }
            else if(ParseCSSUnit(value.ToString(), out size))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
