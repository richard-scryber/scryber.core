using System;
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
            double xDeg, yDeg;
            if (values.IndexOf(',') > 0 || values.IndexOf(' ') > 0)
            {
                if (xOnly || yOnly)
                {
                    return null; //not allowed 2 values.
                }
                
                var all = values.Split(new char[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);
                var xS = all[0].Trim();
                var yS = all[1].Trim();

                if (!double.TryParse(xS, out xDeg))
                    return null;

                if (!double.TryParse(yS, out yDeg))
                    return null;
            }
            else
            {
                if (!double.TryParse(values, out xDeg))
                    return null;

                if (xOnly)
                {
                    yDeg = 0.0;
                }
                else if (yOnly)
                {
                    yDeg = xDeg;
                    xDeg = 0.0;
                }
                else
                {
                    yDeg = xDeg;
                }
            }

            var xRad = xDeg * DegressToRadians;
            var yRad = yDeg * DegressToRadians;
            
            return new TransformSkewOperation(xRad, yRad);

        }
        
        private static TransformOperation ParseRotateDegrees(string values, bool xOnly, bool yOnly)
        {
            values = values.Trim();
            double deg;
            if (!double.TryParse(values, out deg))
                    return null;

            var rad = deg * DegressToRadians;

            
            return new TransformRotateOperation(rad);

        }
    }
}