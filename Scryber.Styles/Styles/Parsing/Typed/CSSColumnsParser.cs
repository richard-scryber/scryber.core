using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSColumnsParser : CSSStyleValueParser
    {

        public CSSColumnsParser() : base(CSSStyleItems.Columns)
        {
        }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            int index = 0;


            string[] both = new string[2];

            while (reader.ReadNextValue())
            {
                both[index] = reader.CurrentTextValue;

                index++;

                if (index > 1)
                    break;
            }
            
            return DoParseColumns(style, both);
        }
        
        protected bool DoParseColumns(Style style, string[] columns)
        {
            int failed = 0;
            int index = 0;
            
            Unit? firstUnit =  null;
            Unit? secondUnit =  null;

            int? firstNumber =  null;
            int? secondNumber =   null;
            
            bool firstIsBound = false;
            bool secondIsBound = false;
            string firstExpression = String.Empty;
            string secondExpression = String.Empty;
            
            bool firstIsAuto = false;
            bool secondIsAuto = false;
            
            foreach (var one in columns)
            {
                if (!string.IsNullOrEmpty(one))
                {
                    if (IsExpression(one))
                    {
                        if (index == 0)
                        {
                            firstExpression = one;
                            firstIsBound = true;

                        }
                        else if (index == 1)
                        {
                            secondExpression = one;
                            secondIsBound = true;
                        }
                    }
                    else if (string.Equals("auto", one))
                    {
                        if (index == 0)
                            firstIsAuto = true;
                        else if (index == 1)
                            secondIsAuto = true;
                    }
                    else if (IsNumber(one) && int.TryParse(one, out var value))
                    {
                        if (index == 0)
                            firstNumber = value;
                        else if (index == 1)
                            secondNumber = value;
                    }
                    else if (Unit.TryParse(one, out var unit))
                    {
                        if (index == 0)
                            firstUnit = unit;
                        else if (index == 1)
                            secondUnit = unit;
                    }
                    else
                    {
                        failed++;
                    }
                }

                index++;
                
            }
            

            if (firstIsAuto && secondIsAuto)
            {
                style.SetValue(StyleKeys.ColumnWidthKey, ColumnWidths.Empty);
                style.SetValue(StyleKeys.ColumnCountKey, 1);
            }
            else if (firstIsAuto)
            {
                if (secondIsBound)
                {
                    // auto count with data-bound widths
                    style.SetValue(StyleKeys.ColumnCountKey, 0);
                    if (!AttachExpressionBindingHandler(style, StyleKeys.ColumnWidthKey, secondExpression,
                            DoConvertColumnWidths))
                    {
                        failed++;
                    }
                }
                else if (secondNumber.HasValue)
                {
                    //auto widths, specific count
                    style.SetValue(StyleKeys.ColumnCountKey, secondNumber.Value);
                    style.SetValue(StyleKeys.ColumnWidthKey, ColumnWidths.Empty);
                }
                else if (secondUnit.HasValue)
                {
                    //auto count, specific width
                    style.SetValue(StyleKeys.ColumnWidthKey, new ColumnWidths(secondUnit.Value));
                    style.SetValue(StyleKeys.ColumnCountKey, 0);
                }
                else
                {
                    //it's just auto - equivalent to auto auto
                    style.SetValue(StyleKeys.ColumnWidthKey, ColumnWidths.Empty);
                    style.SetValue(StyleKeys.ColumnCountKey, 1);
                }
            }
            else if (firstNumber.HasValue)
            {
                if (secondIsBound)
                {
                    //explicit number column, data-bound widths
                    style.SetValue(StyleKeys.ColumnCountKey, firstNumber.Value);
                    if (!AttachExpressionBindingHandler(style, StyleKeys.ColumnWidthKey, secondExpression,
                            DoConvertColumnWidths))
                    {
                        failed++;
                    }
                }
                else if (secondIsAuto)
                {
                    // specific count, auto widths
                    style.SetValue(StyleKeys.ColumnCountKey, firstNumber.Value);
                    style.SetValue(StyleKeys.ColumnWidthKey, ColumnWidths.Empty);
                }
                else if (secondUnit.HasValue)
                {
                    //specific count, specific width
                    style.SetValue(StyleKeys.ColumnCountKey, firstNumber.Value);
                    style.SetValue(StyleKeys.ColumnWidthKey, new ColumnWidths(secondUnit.Value));
                }
                else if (secondNumber.HasValue)
                {
                    //could be any order but default to
                    //specific count, specific width
                    style.SetValue(StyleKeys.ColumnCountKey, firstNumber.Value);
                    style.SetValue(StyleKeys.ColumnWidthKey, new ColumnWidths(secondNumber.Value));
                }
                else
                {
                    //just a count - same as 'number auto'
                    style.SetValue(StyleKeys.ColumnCountKey, firstNumber.Value);
                    style.SetValue(StyleKeys.ColumnWidthKey, ColumnWidths.Empty);
                }
            }
            else if (firstUnit.HasValue)
            {
                if (secondIsBound)
                {
                    //explicit column width, data-bound count
                    style.SetValue(StyleKeys.ColumnWidthKey, new ColumnWidths(firstUnit.Value));
                    if (!AttachExpressionBindingHandler(style, StyleKeys.ColumnCountKey, secondExpression,
                            DoConvertColumnCount))
                    {
                        failed++;
                    }
                }
                else if (secondIsAuto)
                {
                    //specific width, auto column count.
                    style.SetValue(StyleKeys.ColumnCountKey, 0);
                    style.SetValue(StyleKeys.ColumnWidthKey, new ColumnWidths(firstUnit.Value));
                }
                else if (secondNumber.HasValue)
                {
                    //specific width, specific column count
                    style.SetValue(StyleKeys.ColumnCountKey, secondNumber.Value);
                    style.SetValue(StyleKeys.ColumnWidthKey, new ColumnWidths(firstUnit.Value));
                }
                else if (secondUnit.HasValue)
                {
                    // we can treat it as 2 column widths.
                    style.SetValue(StyleKeys.ColumnCountKey, 2);
                    style.SetValue(StyleKeys.ColumnWidthKey, new ColumnWidths(new double[] { firstUnit.Value.PointsValue, secondUnit.Value.PointsValue }));
                }
                else
                {
                    //just widths
                    style.SetValue(StyleKeys.ColumnCountKey, 0);
                    style.SetValue(StyleKeys.ColumnWidthKey, new ColumnWidths(firstUnit.Value));
                }
            }
            else if (firstIsBound)
            {
                if (secondIsBound)
                {
                    //cannot know, but default is count then width.
                    if (!AttachExpressionBindingHandler(style, StyleKeys.ColumnCountKey, firstExpression,
                            DoConvertColumnCount))
                    {
                        failed++;
                    }
                    if (!AttachExpressionBindingHandler(style, StyleKeys.ColumnWidthKey, secondExpression,
                            DoConvertColumnWidths))
                    {
                        failed++;
                    }
                }
                else if (secondIsAuto)
                {
                    // data-binding expression then auto - assume count then widths.
                    if (!AttachExpressionBindingHandler(style, StyleKeys.ColumnCountKey, firstExpression,
                            DoConvertColumnCount))
                    {
                        failed++;
                    }
                    style.SetValue(StyleKeys.ColumnWidthKey, ColumnWidths.Empty);
                }
                else if (secondNumber.HasValue)
                {
                    //binding then an integer value - widths then count
                    if (!AttachExpressionBindingHandler(style, StyleKeys.ColumnWidthKey, firstExpression,
                            DoConvertColumnWidths))
                    {
                        failed++;
                    }
                    style.SetValue(StyleKeys.ColumnCountKey, secondNumber.Value);
                }
                else if (secondUnit.HasValue)
                {
                    //binding then unit - count widths
                    if (!AttachExpressionBindingHandler(style, StyleKeys.ColumnCountKey, firstExpression,
                            DoConvertColumnCount))
                    {
                        failed++;
                    }
                    style.SetValue(StyleKeys.ColumnWidthKey, new  ColumnWidths(secondUnit.Value));
                }
                else
                {
                    //we just have a binding expression.
                    //could turn out to be any of the above.
                    //so attach both.
                    if (!AttachExpressionBindingHandler(style, StyleKeys.ColumnWidthKey, firstExpression,
                            DoConvertColumnWidths))
                    {
                        failed++;
                    }
                    if (!AttachExpressionBindingHandler(style, StyleKeys.ColumnCountKey, firstExpression,
                            DoConvertColumnCount))
                    {
                        failed++;
                    }
                }
            }

            return failed == 0;
        }

        protected bool DoConvertColumnWidths(StyleBase onStyle, object value, out ColumnWidths result)
        {

            if (null != value)
            {
                if (!(value is Unit) && Unit.TryParse(value.ToString(), out var parsed))
                {
                    value = parsed;
                }
                else if (value.ToString() == "auto")
                {
                    result = ColumnWidths.Empty;
                    return true;
                }

                if (value is Unit)
                {
                    result = new ColumnWidths((Unit)value);
                    return true;
                }
            }
            result = ColumnWidths.Empty;
            return false;
        }

        protected bool DoConvertColumnCount(StyleBase onStyle, object value, out int result)
        {
            if (null != value)
            {
                if (!(value is int) && int.TryParse(value.ToString(), out var parsed))
                {
                    value = parsed;
                }
                else if (value.ToString() == "auto")
                {
                    result = 0;
                    return true;
                }
                

                if (value is int)
                {
                    result = (int)value;
                    return true;
                }
            }

            result = -1;
            return false;
        }
    }
}