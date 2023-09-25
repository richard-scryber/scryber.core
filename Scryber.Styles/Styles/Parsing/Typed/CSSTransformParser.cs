using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSTransformParser : CSSStyleValueParser
    {
        public CSSTransformParser()
            : base(CSSStyleItems.Transform)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            TransformOperation full = null;
            bool bound = false;

            while (reader.ReadNextValue())
            {
                string value = reader.CurrentTextValue;

                TransformOperation parsed;
                if (IsExpression(value))
                {
                    if (null != full)
                        throw new ArgumentOutOfRangeException("Cannot bind individual transforms within multiple operations. Consider using a concat expression to build the string");

                    result &= AttachExpressionBindingHandler(onStyle, StyleKeys.TransformOperationKey, value, DoConvertTransform);
                    bound = true;
                }
                else if (DoConvertTransform(onStyle, value, out parsed))
                {
                    if(bound)
                        throw new ArgumentOutOfRangeException("Cannot bind individual transforms within multiple operations. Consider using a concat expression to build the string");

                    if (null == full)
                        full = parsed;
                    else
                        full.Append(parsed);


                    result &= true;
                }
                else
                    result = false;
            }

            if (null != full)
            {
                onStyle.SetValue(StyleKeys.TransformOperationKey, full);
                return result;
            }
            else
            {
                return false;
            }
        }

        


        protected bool DoConvertTransform(StyleBase style, object value, out TransformOperation operation)
        {
            TransformType type;
            operation = null;
            TransformOperation first = null;
            float value1 = TransformOperation.NotSetValue();
            float value2 = TransformOperation.NotSetValue();

            int valueCount;
            int opLength;
            bool useDegrees = false;
            bool negative1 = false;
            bool negative2 = false;

            if (null == value)
            {
                operation = null;
                return false;
            }

            var str = value.ToString().Trim();

            if (str.StartsWith("rotate("))
            {
                opLength = 7;
                type = TransformType.Rotate;
                useDegrees = true;
                negative1 = true;
                valueCount = 1;
            }
            else if (str.StartsWith("skew("))
            {
                opLength = 5;
                type = TransformType.Skew;
                useDegrees = true;
                negative1 = true;
                negative2 = true;
                valueCount = 2;
            }
            else if (str.StartsWith("scale("))
            {
                opLength = 6;
                type = TransformType.Scale;
                useDegrees = false;
                valueCount = 2;
            }
            else if (str.StartsWith("translate("))
            {
                opLength = 10;
                type = TransformType.Translate;
                useDegrees = false;
                negative2 = true;
                valueCount = 2;
            }
            else
            {
                throw new NotSupportedException("The transform operation " + str + " is not known or not currently supported");
            }

            var end = str.IndexOf(")", opLength);

            if (end <= opLength + 1)
            {
                return false;
            }

            var values = str.Substring(opLength, end - opLength).Trim();
            str = str.Substring(end + 1);

            if (valueCount == 2)
            {
                var parts = values.Split(',');
                if (parts.Length != 2)
                {
                    operation = null;
                    return false;
                }

                if (useDegrees)
                {
                    value1 = GetDegreesValue(parts[0], negative1);
                    value2 = GetDegreesValue(parts[1], negative2);
                }
                else
                {
                    value1 = GetUnitValue(parts[0], negative1);
                    value2 = GetUnitValue(parts[1], negative2);
                }

                operation = new TransformOperation(type, value1, value2);
                return true;

            }
            else
            {
                if (useDegrees)
                {
                    value1 = GetDegreesValue(values, negative1);
                }
                else
                {
                    value1 = GetUnitValue(values, negative1);
                }

                operation = new TransformOperation(type, value1, TransformOperation.NotSetValue());
                return true;
            }
        }

        private float GetDegreesValue(string deg, bool negative)
        {
            deg = deg.Trim();
            if (deg.EndsWith("deg"))
            {
                deg = deg.Substring(0, deg.Length - 3);
                float value;
                if(float.TryParse(deg, out value))
                {
                    if (negative)
                        return -((float)(Math.PI / 180.0) * value);
                    else
                        return (float)(Math.PI / 180.0) * value;

                }
            }

            throw new ArgumentOutOfRangeException("Value " + deg + " could not be converted to degrees");
        }

        private float GetUnitValue(string unit, bool negative)
        { 
            Unit parsed;
            if (ParseCSSUnit(unit, out parsed))
            {
                if (negative)
                    return -(float)parsed.PointsValue;
                else
                    return (float)parsed.PointsValue;
            }

            throw new ArgumentOutOfRangeException("Value " + unit + " could not be converted to unit value");
        }

        protected bool DoConvertFullWidth(StyleBase style, object value, out bool fullWidth)
        {
            if(null == value)
            {
                fullWidth = false;
                return false;
            }
            else if(value.ToString() == "100%")
            {
                fullWidth = true;
                return true;
            }
            else
            {
                fullWidth = false;
                return false;
            }
        }

    }
}
