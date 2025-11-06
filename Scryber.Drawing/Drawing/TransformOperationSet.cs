using System;
using System.Text;
using Scryber.OpenType;
using Scryber.PDF.Graphics;

namespace Scryber.Drawing
{

    public delegate TransformOperation TransformOperationParser(string arguments, bool xOnly, bool yOnly);
    
    /// <summary>
    /// A wrapper for a set of individual matrix operations on a component or graphic element such as rotate, translate, scale, skew.
    /// The values of the operations are held as a linked list of TransformOperations. Use the SVGTransformOperationSet or the CSSTransformOperationSet for the Parsing static methods.
    /// To get the actual 2D matrix - It can be converted to a <see cref="Scryber.PDF.Graphics.PDFTransformationMatrix"/> using the GetMatrixMethod.
    /// </summary>
    public class TransformOperationSet
    {
        /// <summary>
        /// Gets or sets the root of the linked list of TransformOperations
        /// </summary>
        public TransformOperation Root { get; set; }
        

        public bool IsIdentity
        {
            get
            {
                if (null == Root)
                    return true;
                else
                {
                    return this.Root.IsIdentity;
                }
            }
        }

        public void AppendOperation(TransformOperation op)
        {
            var curr = this.Root;
            if (null == curr)
            {
                this.Root = op;
            }
            else
            {
                curr.AppendTransformation(op);
            }
        }

        public TransformOperationSet CloneAndFlatten(Size pageSize, Size containerSize, Size font, Unit rootFont)
        {
            TransformOperationSet clone = (TransformOperationSet)this.MemberwiseClone();
            if (null != this.Root)
            {
                clone.Root = this.Root.CloneValuesAndFlatten(pageSize, containerSize, font, rootFont);
            }

            return clone;
        }

        public Scryber.PDF.Graphics.PDFTransformationMatrix GetTransformationMatrix(Size containerSize, TransformOrigin origin)
        {
            Matrix2D matrix = Matrix2D.Identity;
            DrawingOrigin drawFrom = DrawingOrigin.BottomLeft;

            
            Unit centreX, centerY;
            
            if (origin != null)
            {
                //TODO apply the offset to 
                centreX = origin.GetHorizontalOffset(containerSize, drawFrom);
                centerY = origin.GetVerticalOffset(containerSize, drawFrom);
            }
            else
            {
                //default PDF Origin is bottom left.
                //default SVG Origin in Top left
                //so move there
                centreX = 0;
                centerY = containerSize.Height;
            }
            
            //matrix.Translate(0, containerSize.Height.PointsValue); - working top left
            matrix.Translate(centreX.PointsValue, centerY.PointsValue);

            if (null != this.Root)
            {
                
                matrix = this.Root.GetMatrix(matrix, drawFrom);
            }

            origin = null;

            if (origin != null)
            {
                //TODO: re-apply the offset back to the position.
                
            }
            else
            {
                var moveBack = Matrix2D.Identity;
                moveBack.Translate(0 - centreX.PointsValue, 0- centerY.PointsValue);
                matrix = Matrix2D.Multiply(matrix, moveBack);
            }

            return new PDFTransformationMatrix(matrix);
        }

        #region Parsing
        

        public static bool TryParse(string value, out TransformOperationSet operations)
        {
            operations = new TransformOperationSet();
            bool success = false;
            try
            {
                success = TransformOperationSet.ParseIntoSet(operations, value);
            }
            catch (Exception ex)
            {
                operations = null;
                success = false;
            }

            return success;
        }

        

        protected static bool ParseIntoSet(TransformOperationSet set, string value, TransformOperationParser rotate = null, TransformOperationParser translate = null, TransformOperationParser scale = null, TransformOperationParser skew = null, TransformOperationParser matrix = null)
        {
            MatrixTransformTypes type;
            TransformOperation operation = null;
            TransformOperation first = null;
            
            int opLength;
            bool xOnly = false;
            bool yOnly = false;

            if (null == value)
            {
                return false;
            }

            var str = value.ToString().Trim();
            
            if (str == "none")
            {
                operation = new TransformMatrixOperation(Matrix2D.Identity.Elements);
                set.AppendOperation(operation);
                return true;
            }

            while (string.IsNullOrEmpty(str) == false)
            {
                xOnly = false;
                yOnly = false;

                if (str.StartsWith("rotate("))
                {
                    opLength = 7;
                    type = MatrixTransformTypes.Rotate;
                    
                }
                else if (str.StartsWith("skew("))
                {
                    opLength = 5;
                    type = MatrixTransformTypes.Skew;
                    
                }
                else if (str.StartsWith("skewX("))
                {
                    opLength = 6;
                    type = MatrixTransformTypes.Skew;
                    xOnly = true;
                }
                else if (str.StartsWith("skewY("))
                {
                    opLength = 6;
                    type = MatrixTransformTypes.Skew;
                    yOnly = true;
                }
                else if (str.StartsWith("scale("))
                {
                    opLength = 6;
                    type = MatrixTransformTypes.Scaling;
                    
                }
                else if (str.StartsWith("scaleX("))
                {
                    opLength = 7;
                    type = MatrixTransformTypes.Scaling;
                    xOnly = true;
                }
                else if (str.StartsWith("scaleY("))
                {
                    opLength = 7;
                    type = MatrixTransformTypes.Scaling;
                    yOnly = true;
                }
                else if (str.StartsWith("translate("))
                {
                    opLength = 10;
                    type = MatrixTransformTypes.Translation;
                    
                }
                else if (str.StartsWith("translateX("))
                {
                    opLength = 11;
                    type = MatrixTransformTypes.Translation;
                    xOnly = true;
                }
                else if (str.StartsWith("translateY"))
                {
                    opLength = 11;
                    type = MatrixTransformTypes.Translation;
                    yOnly = true;
                }
                else if (str.StartsWith("matrix("))
                {
                    opLength = 7;
                    type = MatrixTransformTypes.Matrix;
                   
                }
                else
                {
                    //Just skip - could look at extracting and ignoring, but it would mess up anyway.
                    return false;
                    // throw new NotSupportedException("The transform operation " + str +
                    //                                 " is not known or not currently supported");
                }

                var end = str.IndexOf(")", opLength);

                if (end <= opLength)
                {
                    return false;
                }


                var values = str.Substring(opLength, end - opLength).Trim();
                str = str.Substring(end + 1).Trim();
                TransformOperation op;
                
                switch (type)
                {
                    case(MatrixTransformTypes.Rotate):
                        if (rotate != null)
                            op = rotate(values, xOnly, yOnly);
                        else
                            op = ParseRotateOperation(values);
                        break;
                    case(MatrixTransformTypes.Scaling):
                        if (scale != null)
                            op = scale(values, xOnly, yOnly);
                        else
                            op = ParseScalingOperation(values, xOnly, yOnly);
                        break;
                    case(MatrixTransformTypes.Skew):
                        if (skew != null)
                            op = skew(values, xOnly, yOnly);
                        else
                            op = ParseSkewOperation(values, xOnly, yOnly);
                        break;
                    case(MatrixTransformTypes.Translation):
                        if (translate != null)
                            op = translate(values, xOnly, yOnly);
                        else
                            op = ParseTranslateOperation(values, xOnly, yOnly);
                        break;
                    case(MatrixTransformTypes.Matrix):
                        if (matrix != null)
                            op = matrix(values, xOnly, yOnly);
                        else
                            op = ParseMatrixOperation(values);
                        break;
                    default: 
                        op = null;
                        break;
                }
                
                
                
                if (op != null)
                {
                    set.AppendOperation(op);
                }
                else
                {
                    throw new ArgumentException("The transform value could not be understood - " + values +
                                                " could not be read as a valid transform operation");
                }
            }

            return true;

            
        }
        
        
        private static readonly char[] paramSplitters = new char[] { ',', ' ' };

        private static TransformOperation ParseMatrixOperation(string values)
        {
            var all = values.Trim().Split(paramSplitters, StringSplitOptions.RemoveEmptyEntries);
            if (all.Length != 6)
                return null;
            double[] parsed = new double[6];

            for (var i = 0; i < all.Length; i++)
            {
                if (double.TryParse(all[i], out var d))
                    parsed[i] = d;
                else
                {
                    return null;
                }
            }

            return new TransformMatrixOperation(parsed);
        }

        private static TransformOperation ParseTranslateOperation(string values, bool xOnly, bool yOnly)
        {
            values = values.Trim();
            Unit xUnit, yUnit;
            if (values.IndexOf(',') > 0 || values.IndexOf(' ') > 0)
            {
                if (xOnly || yOnly)
                    return null; //comma not supported
                
                var all = values.Split(paramSplitters, StringSplitOptions.RemoveEmptyEntries);
                var xS = all[0].Trim();
                var yS = all[1].Trim();

                if (!Unit.TryParse(xS, out xUnit))
                    return null;

                if (!Unit.TryParse(yS, out yUnit))
                    return null;
            }
            
            else
            {
                if (!Unit.TryParse(values, out xUnit))
                    return null;

                if (yOnly) //swap
                {
                    yUnit = xUnit;
                    xUnit = Unit.Zero;
                }
                else //only 1 value so always xOnly
                {
                    yUnit = Unit.Zero;
                }
            }

            return new TransformTranslateOperation(xUnit, yUnit);
        }

        private static TransformOperation ParseSkewOperation(string values, bool xOnly, bool yOnly)
        {
            values = values.Trim();
            double xRad, yRad;
            if (values.IndexOf(',') > 0 || values.IndexOf(' ') > 0)
            {
                if (xOnly || yOnly)
                {
                    return null; //not allowed 2 values.
                }
                
                var all = values.Split(paramSplitters, StringSplitOptions.RemoveEmptyEntries);
                var xS = all[0].Trim();
                var yS = all[1].Trim();

                if (!ParseAngleValue(xS, out xRad))
                    return null;

                if (!ParseAngleValue(yS, out yRad))
                    return null;
            }
            else
            {
                if (!ParseAngleValue(values, out xRad))
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

        

        private static TransformOperation ParseScalingOperation(string values, bool xOnly, bool yOnly)
        {
            values = values.Trim();
            double x;
            double y;
            bool xPcent = false;
            bool yPcent = false;
            
            if (values.IndexOf(',') > 0 || values.IndexOf(' ') > 0)
            {
                if (xOnly || yOnly)
                {
                    return null; //not allowed 2 values.
                }
                var all = values.Split(paramSplitters, StringSplitOptions.RemoveEmptyEntries);
                var xS = all[0].Trim();
                var yS = all[1].Trim();

                if (xS.EndsWith("%"))
                {
                    xPcent = true;
                    xS = xS.Substring(0, xS.Length - 1);
                }

                if (yS.EndsWith("%"))
                {
                    yPcent = true;
                    yS = yS.Substring(0, yS.Length -1);
                }

                if (!double.TryParse(xS, out x))
                    return null;

                if (!double.TryParse(yS, out y))
                    return null;
            }
            else
            {
                if (values.EndsWith("%"))
                {
                    values = values.Substring(0, values.Length - 1);
                    xPcent = true;
                    yPcent = true;
                }
                

                if (!double.TryParse(values, out x))
                    return null;

                if (yOnly)
                {
                    y = x;
                    x = 1.0;
                }
                else if (xOnly)
                {
                    y = 1;
                }
                else
                {
                    y = x;
                }
            }

            if (xPcent)
            {
                x = x / 100.0;
            }

            if (yPcent)
            {
                y = y / 100.0;
            }

            return new TransformScaleOperation(x, y);
        }

        public const string DegreesIdentifier = "deg";
        public const string TurnsIdentifier = "turn";
        public const string RadiansIdentifier = "rad";

        protected const double DegressToRadians = Math.PI / 180;
        private static TransformOperation ParseRotateOperation(string values)
        {
            values = values.Trim();
            double value;
            if (ParseAngleValue(values, out value))
                return new TransformRotateOperation(value);
            else
                return null;
        }


        protected static bool ParseAngleValue(string value, out double radians)
        {
            if (value.EndsWith(DegreesIdentifier))
            {
                value = value.Substring(0, value.Length - DegreesIdentifier.Length);
                if (!double.TryParse(value, out radians))
                    return false;
                else
                {
                    radians = DegressToRadians * radians;
                    return true;
                }
            }
            else if (value.EndsWith(TurnsIdentifier))
            {
                value = value.Substring(0, value.Length - TurnsIdentifier.Length);
                if (!double.TryParse(value, out radians))
                    return false;
                else
                {
                    radians = DegressToRadians * (radians * 360);
                    return true;
                }
            }
            else
            {
                if (value.EndsWith(RadiansIdentifier))
                {
                    value = value.Substring(0, value.Length - RadiansIdentifier.Length);
                }

                if (!double.TryParse(value, out radians))
                    return false;
                else
                {
                    return true;
                }
            }
        }
        
        
#endregion


        public override string ToString()
        {
            var sb = new StringBuilder();
            this.Root.ToString(sb);
            
            return sb.ToString();
        }
        
    }
}