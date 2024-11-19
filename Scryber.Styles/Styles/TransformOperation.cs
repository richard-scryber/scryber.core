using System;
using System.Text;
using Scryber.Drawing;

namespace Scryber.Styles
{
    /// <summary>
    /// A linked list of transformation operations that can be used to build a transformation matrix
    /// </summary>
    [PDFParsableValue()]
    public class TransformOperation
    {
        protected static readonly float _notSet = float.NaN;

        public static bool IsNotSet(float value)
        {
            return float.IsNaN(value);
        }

        public static bool IsSet(float value)
        {
            return !float.IsNaN(value);
        }

        public static float NotSetValue()
        {
            return _notSet;
        }

        public TransformType Type { get; set; }

        public float Value1 { get; set; }

        public float Value2 { get; set; }

        public TransformOperation Next { get; set; }


        public TransformOperation(TransformType type, float value1, float value2)
        {
            this.Type = type;
            this.Value1 = value1;
            this.Value2 = value2;
        }

        public void Append(TransformOperation op)
        {
            if (null == this.Next)
                this.Next = op;
            else
                this.Next.Append(op);
        }

        public bool TryGetType(TransformType type, out TransformOperation operation)
        {
            if (this.Type == type)
            {
                operation = this;
                return true;
            }
            else if (null != this.Next)
                return this.Next.TryGetType(type, out operation);
            else
            {
                operation = null;
                return false;
            }
        }

        public static TransformOperation Remove(TransformType type, TransformOperation from)
        {
            var first = from;
            if (first.Type == type)
                return first.Next;
            else
            {
                var curr = first;
                while(null != curr)
                {
                    var next = curr.Next;
                    if(null != next && next.Type == type)
                    {
                        curr.Next = next.Next;
                    }
                    curr = curr.Next;
                }
                return first;
            }
        }

        public virtual Scryber.PDF.Graphics.PDFTransformationMatrix GetResolvedMatrix(
            Scryber.PDF.Graphics.PDFGraphics graphics, Scryber.Drawing.MatrixOrder order)
        {
            var root = this;
            if (root.Type == TransformType.Rotate)
            {
                var move = Matrix2D.Identity;
                move.Translate(0, graphics.ContainerSize.Height.PointsValue);
                var rotate = Matrix2D.Identity;
                rotate.Rotate(root.Value1);

                var returnTo = Matrix2D.Identity;
                returnTo.Translate(0, 0 - graphics.ContainerSize.Height.PointsValue);

                var full = move.Multiply(rotate).Multiply(returnTo);

                return null; // new PDFTransformationMatrix(full);
            }
            else
            {
                return null;// this.GetMatrix(order);
            }
            
        }
        
        
        public virtual Scryber.PDF.Graphics.PDFTransformationMatrix GetMatrix(Rect containerBounds, Rect itemBounds, Point transformOrigin, Scryber.Drawing.MatrixOrder order)
        {
            var matrix = Matrix2D.Identity;

            switch (order)
            {
                case Drawing.MatrixOrder.Append:
                    matrix = this.ApplyAppendedTransformations(containerBounds, itemBounds, transformOrigin, matrix);
                    break;
                default:
                    matrix = this.ApplyPrependedTransformations(containerBounds, itemBounds, transformOrigin, matrix);
                    break;
            }

            return null; // new PDFTransformationMatrix(matrix);
        }

        protected virtual Matrix2D ApplyAppendedTransformations(
            Rect containerBounds, 
            Rect itemBounds, Point transformOrigin, Matrix2D matrix)
        {
            var root = Matrix2D.Identity;
            var currOp = this;
            //ApplyTransformation(root, currOp, containerBounds, itemBounds, );

            return root; // new PDFTransformationMatrix(root);
        }

        protected virtual Matrix2D ApplyPrependedTransformations(
            Rect containerBounds, 
            Rect itemBounds, Point transformOrigin, 
            Matrix2D matrix)
        {
            
            //if (null != this.Next)
            //    this.Next.PrependTransformations(matrix);
            //this.ApplyTransformation(matrix);

            return matrix;
        }

        protected virtual void ApplyTransformation(PDF.Graphics.PDFTransformationMatrix matrix)
        {
            switch (this.Type)
            {
                case TransformType.Rotate:

                    if (IsSet(this.Value1))
                        matrix.SetRotation(this.Value1);

                    break;

                case TransformType.Scale:
                    if(IsNotSet(this.Value1))
                    {
                        if(IsSet(this.Value2))
                            matrix.SetScale(1.0F, this.Value2);
                    }
                    else if (IsNotSet(this.Value2))
                        matrix.SetScale(this.Value1, 1.0F);
                    else
                        matrix.SetScale(this.Value1, this.Value2);
                    break;

                case TransformType.Skew:
                    if(IsNotSet(this.Value1))
                    {
                        if (IsSet(this.Value2))
                            matrix.SetSkew(0.0F, this.Value2);
                    }
                    else if (IsNotSet(this.Value2))
                        matrix.SetSkew(this.Value1, 0.0F);
                    else
                        matrix.SetSkew(this.Value1, this.Value2);
                    break;

                case TransformType.Translate:
                    if(IsNotSet(this.Value1))
                    {
                        if (IsSet(this.Value2))
                            matrix.SetTranslation(0.0F, this.Value2);
                    }
                    else if (IsNotSet(this.Value2))
                        matrix.SetTranslation(this.Value1, 0.0F);
                    else
                        matrix.SetTranslation(this.Value1, this.Value2);
                    break;
            }
        }


        //ToString and Parsing

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            this.ToString(sb);
            return sb.ToString();
        }

        protected virtual void ToString(StringBuilder sb)
        {
            if(sb.Length > 0) { sb.Append(" "); }

            switch (this.Type)
            {
                case TransformType.Rotate:
                    sb.Append("rotate(");
                    sb.Append(this.Value1);
                    sb.Append(")");
                    break;

                case TransformType.Scale:
                    sb.Append("scale(");
                    if (IsSet(this.Value1))
                        sb.Append(this.Value1);
                    else
                        sb.Append("1.0");

                    sb.Append(", ");
                    if (IsSet(this.Value2))
                        sb.Append(this.Value2);
                    else
                        sb.Append("1.0");

                    sb.Append(")");
                    break;

                case TransformType.Skew:
                    sb.Append("skew(");
                    if (IsSet(this.Value1))
                        sb.Append(this.Value1);
                    else
                        sb.Append("1.0");

                    sb.Append(", ");
                    if (IsSet(this.Value2))
                        sb.Append(this.Value2);
                    else
                        sb.Append("1.0");

                    sb.Append(")");
                    break;

                case TransformType.Translate:
                    sb.Append("translate(");
                    if (IsSet(this.Value1))
                        sb.Append(this.Value1);
                    else
                        sb.Append("1.0");

                    sb.Append(", ");
                    if (IsSet(this.Value2))
                        sb.Append(this.Value2);
                    else
                        sb.Append("1.0");

                    sb.Append(")");
                    break;
                
                default:
                    throw new PDFParserException("Could not understand the Transformation Type " + this.Type.ToString());
            }

            if(null != this.Next)
            {
                this.Next.ToString(sb);
            }

        }

        private static char[] _separators = { ',', ' ' };

        public static TransformOperation Parse(string value)
        {
            TransformType type;
            TransformOperation operation = null;
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
                return operation;
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
            else if(str.StartsWith("matrix("))
            {
                opLength = 7;
                type = TransformType.Matrix;
                useDegrees = false;
                valueCount = 6;
            }
            else
            {
                throw new NotSupportedException("The transform operation " + str + " is not known or not currently supported");
            }

            var end = str.IndexOf(")", opLength);

            if (end <= opLength + 1)
            {
                return null;
            }

            var values = str.Substring(opLength, end - opLength).Trim();
            str = str.Substring(end + 1);

            if (valueCount == 2)
            {
                var parts = values.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                {
                    operation = null;
                    return operation;
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
                return operation;

            }
            else if(valueCount == 6)
            {
                //A full matrix
                var parts = values.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
                if(parts.Length != 6)
                {
                    operation = null;
                    return operation;
                    
                }
                double[] all = new double[6];
                for (var i = 0; i < all.Length; i++)
                    all[i] = GetUnitValue(parts[i], false);

                operation = new TransformMatrixOperation(all);
                return operation;
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
                return operation;
            }
        }

        private static float GetDegreesValue(string deg, bool negative)
        {
            deg = deg.Trim();
            if (deg.EndsWith("deg"))
            {
                deg = deg.Substring(0, deg.Length - 3);
                float value;
                if (float.TryParse(deg, out value))
                {
                    if (negative)
                        return -((float)(Math.PI / 180.0) * value);
                    else
                        return (float)(Math.PI / 180.0) * value;

                }
            }

            throw new ArgumentOutOfRangeException("Value " + deg + " could not be converted to degrees");
        }

        private static float GetUnitValue(string unit, bool negative)
        {
            Drawing.Unit parsed;
            if (Drawing.Unit.TryParse(unit, out parsed))
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
            if (null == value)
            {
                fullWidth = false;
                return false;
            }
            else if (value.ToString() == "100%")
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


    public class TransformMatrixOperation : TransformOperation
    {
        private double[] _values;

        // protected override void AppendTransformations(PDFTransformationMatrix matrix)
        // {
        //     throw new NotSupportedException("A Matrix operation cannot append further transformations");
        // }
        //
        // protected override void PrependTransformations(PDFTransformationMatrix matrix)
        // {
        //     throw new NotSupportedException("A Matrix operation cannot prepend further transformations");
        // }

        public TransformMatrixOperation(double[] values)
            : base(TransformType.Matrix, 0, 0)
        {

            if (values == null)
                throw new ArgumentNullException("values", "The values paramerter must be an array of 6 floating points");
            if (values.Length != 6)
                throw new ArgumentOutOfRangeException("values", "The values paramerter must be an array of 6 floating points");

            _values = values;
        }

        // public override PDFTransformationMatrix GetMatrix(MatrixOrder order)
        // {
        //     Matrix2D d = new Matrix2D(_values[0], _values[1], _values[2], _values[3], _values[4], -(_values[5]));
        //
        //     return new PDFTransformationMatrix(d);
        // }
    }
}

