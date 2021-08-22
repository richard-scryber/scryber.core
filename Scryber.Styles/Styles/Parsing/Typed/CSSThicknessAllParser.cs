using System;
using Scryber.Drawing;

namespace Scryber.Styles.Parsing.Typed
{
    public abstract class CSSThicknessAllParser : CSSStyleValueParser
    {
        private PDFStyleKey<PDFUnit> _all;
        private PDFStyleKey<PDFUnit> _left;
        private PDFStyleKey<PDFUnit> _right;
        private PDFStyleKey<PDFUnit> _top;
        private PDFStyleKey<PDFUnit> _bottom;

        public CSSThicknessAllParser(string cssAttr, PDFStyleKey<PDFUnit> all, PDFStyleKey<PDFUnit> left, PDFStyleKey<PDFUnit> top, PDFStyleKey<PDFUnit> right, PDFStyleKey<PDFUnit> bottom)
            : base(cssAttr)
        {
            _all = all;
            _left = left;
            _right = right;
            _top = top;
            _bottom = bottom;
        }


        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            int count = 0;
            PDFUnit[] all = new PDFUnit[4];
            string[] exprs = new string[4];

            while (reader.ReadNextValue())
            {
                PDFUnit found;
                var str = reader.CurrentTextValue;
                if (IsExpression(str))
                {
                    exprs[count] = str;
                    count++;
                }
                else if (CSSThicknessValueParser.ParseThicknessValue(str, PDFUnit.Zero, out found))
                {
                    all[count] = found;
                    count++;

                    if (count == 4)
                        break;
                }
                else //We have failed, so just return
                    return false;
            }

            bool result = false;

            if (count == 1)
            {
                if (!string.IsNullOrEmpty(exprs[0])) // we have a single expression for this set of values - so set it on all properties
                {
                    result = true;
                    result &= AttachExpressionBindingHandler(onStyle, _all, exprs[0], DoConvertThicknessValue);
                    result &= AttachExpressionBindingHandler(onStyle, _top, exprs[0], DoConvertThicknessValue);
                    result &= AttachExpressionBindingHandler(onStyle, _left, exprs[0], DoConvertThicknessValue);
                    result &= AttachExpressionBindingHandler(onStyle, _bottom, exprs[0], DoConvertThicknessValue);
                    result &= AttachExpressionBindingHandler(onStyle, _right, exprs[0], DoConvertThicknessValue);
                }
                else
                {
                    onStyle.SetValue(_all, all[0]);
                    onStyle.SetValue(_top, all[0]);
                    onStyle.SetValue(_bottom, all[0]);

                    onStyle.SetValue(_left, all[0]);
                    onStyle.SetValue(_right, all[0]);

                    result = true;
                }
            }
            else if (count == 2)
            {
                result = true;

                // first top and bottom then left and right
                if (!string.IsNullOrEmpty(exprs[0]))
                {
                    result &= AttachExpressionBindingHandler(onStyle, _top, exprs[0], DoConvertThicknessValue);
                    result &= AttachExpressionBindingHandler(onStyle, _bottom, exprs[0], DoConvertThicknessValue);
                }
                else
                {
                    onStyle.SetValue(_top, all[0]);
                    onStyle.SetValue(_bottom, all[0]);
                    result &= true;
                }

                if (!string.IsNullOrEmpty(exprs[1]))
                {
                    result &= AttachExpressionBindingHandler(onStyle, _left, exprs[1], DoConvertThicknessValue);
                    result &= AttachExpressionBindingHandler(onStyle, _right, exprs[1], DoConvertThicknessValue);
                }
                else
                {
                    onStyle.SetValue(_left, all[1]);
                    onStyle.SetValue(_right, all[1]);
                    result &= true;
                }
            }
            else if (count == 3)
            {
                // top then left and right and finally bottom
                if (!string.IsNullOrEmpty(exprs[0]))
                {
                    result &= AttachExpressionBindingHandler(onStyle, _top, exprs[0], DoConvertThicknessValue);
                }
                else
                {
                    onStyle.SetValue(_top, all[0]);
                    result &= true;
                }

                if (!string.IsNullOrEmpty(exprs[1]))
                {
                    result &= AttachExpressionBindingHandler(onStyle, _left, exprs[1], DoConvertThicknessValue);
                    result &= AttachExpressionBindingHandler(onStyle, _right, exprs[1], DoConvertThicknessValue);
                }
                else
                {
                    onStyle.SetValue(_left, all[1]);
                    onStyle.SetValue(_right, all[1]);
                    result &= true;
                }

                if (!string.IsNullOrEmpty(exprs[2]))
                {
                    result &= AttachExpressionBindingHandler(onStyle, _bottom, exprs[2], DoConvertThicknessValue);
                }
                else
                {
                    onStyle.SetValue(_bottom, all[2]);
                    result &= true;
                }
            }
            else if (count == 4)
            {
                // all 4 individually top then right and then bottom and finally left
                if (!string.IsNullOrEmpty(exprs[0]))
                {
                    result &= AttachExpressionBindingHandler(onStyle, _top, exprs[0], DoConvertThicknessValue);
                }
                else
                {
                    onStyle.SetValue(_top, all[0]);
                    result &= true;
                }

                if (!string.IsNullOrEmpty(exprs[1]))
                {
                    result &= AttachExpressionBindingHandler(onStyle, _right, exprs[1], DoConvertThicknessValue);
                }
                else
                {
                    onStyle.SetValue(_right, all[1]);
                    result &= true;
                }

                if (!string.IsNullOrEmpty(exprs[2]))
                {
                    result &= AttachExpressionBindingHandler(onStyle, _bottom, exprs[2], DoConvertThicknessValue);
                }
                else
                {
                    onStyle.SetValue(_bottom, all[2]);
                    result &= true;
                }

                if (!string.IsNullOrEmpty(exprs[3]))
                {
                    result &= AttachExpressionBindingHandler(onStyle, _left, exprs[3], DoConvertThicknessValue);
                }
                else
                {
                    onStyle.SetValue(_left, all[3]);
                    result &= true;
                }
            }

            return result;
        }

        protected bool DoConvertThicknessValue(StyleBase style, object value, out PDFUnit converted)
        {
            if (null == value)
            {
                converted = PDFUnit.Empty;
                return false;
            }
            else if (value is PDFUnit unit)
            {
                converted = unit;
                return true;
            }
            else if (CSSThicknessValueParser.ParseThicknessValue(value.ToString(), PDFUnit.Zero, out converted))
            {
                return true;
            }
            else
                return false;
        }
    }
}
