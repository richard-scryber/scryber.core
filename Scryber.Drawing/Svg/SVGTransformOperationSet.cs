using System;
using System.Collections.Generic;
using System.Resources;
using Scryber.Drawing;

namespace Scryber.Svg
{
    /// <summary>
    /// The SVGTransformOperationSet implements the base TransformOperationSet, but parses the explicit
    /// content for the svg element transform attribute values
    /// </summary>
    [PDFParsableValue]
    public class SVGTransformOperationSet : Scryber.Drawing.TransformOperationSet
    {

        public SVGTransformOperationSet() : base()
        {

        }

        public SVGTransformOperationSet(TransformOperation operation) : this()
        {
            if(null != operation)
                this.AppendOperation(operation);
        }
        
        public SVGTransformOperationSet(IEnumerable<TransformOperation> operation) : this()
        {
            if (null != operation)
            {
                foreach (var op in operation)
                {
                    this.AppendOperation(op);
                }
            }
        }

        public SVGTransformOperationSet(params TransformOperation[] operations) : this()
        {
            if (null != operations)
            {
                foreach (var op in operations)
                {
                    this.AppendOperation(op);
                }
            }
        }


        public static SVGTransformOperationSet Parse(string value)
        {
            SVGTransformOperationSet parsed = new SVGTransformOperationSet();

            if (TransformOperationSet.ParseIntoSet(parsed, value, ParseRotateDegrees, null, null, ParseSkewDegrees))
            {
                return parsed;
            }
            else
                return null;
        }

        private static readonly TransformOperationParser SkewXParser = new TransformOperationParser(ParseSkewDegrees);
        private static TransformOperation ParseSkewDegrees(string values, bool xOnly, bool yOnly)
        {
            values = values.Trim();
            double xRad, yRad;
            if (values.IndexOf(',') > 0 || values.IndexOf(' ') > 0)
            {
                if (xOnly || yOnly)
                {
                    return null; //not allowed 2 values.
                }
                
                var all = values.Split(new char[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);
                var xS = all[0].Trim();
                var yS = all[1].Trim();

                if (!TryParseAngleToRadians(xS, out xRad))
                    return null;

                if (!TryParseAngleToRadians(yS, out yRad))
                    return null;
            }
            else
            {
                if (!TryParseAngleToRadians(values, out xRad))
                    return null;

                if (xOnly)
                {
                    yRad = 0.0;
                }
                else if (yOnly)
                {
                    yRad = xRad;
                    xRad = 0.0;
                }
                else
                {
                    yRad = xRad;
                }
            }

            
            return new TransformSkewOperation(xRad, yRad);

        }
        
        private static TransformOperation ParseRotateDegrees(string values, bool xOnly, bool yOnly)
        {
            values = values.Trim();
            double radians = 0.0;

            if (!TryParseAngleToRadians(values, out radians))
                return null;
            else
                return new TransformRotateOperation(radians);

        }

        private static bool TryParseAngleToRadians(string values, out double radians)
        {
            radians = 0.0;
            if (values.EndsWith("rad"))
            {
                values = values.Substring(0, values.Length - 3);
                
                if (!double.TryParse(values, out radians))
                    return false;
                
                
            }
            else if (values.EndsWith("turn"))
            {
                values = values.Substring(0, values.Length - 4);
                var turns = double.Parse(values);
                var deg = 360 * turns;
                radians = deg * DegressToRadians;
            }
            else
            {
                if (values.EndsWith("deg"))
                    values = values.Substring(0, values.Length - 3);

                if (!double.TryParse(values, out var deg))
                    return false;
                
                radians = deg * DegressToRadians;
            }
            
            return true;
        }
    }
}