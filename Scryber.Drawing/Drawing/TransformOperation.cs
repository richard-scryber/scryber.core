using System;
using System.Text;
using Scryber.PDF.Graphics;

namespace Scryber.Drawing
{
    public abstract class TransformOperation
    {
        public virtual bool IsIdentity
        {
            get
            {
                if (this.OperationType == MatrixTransformTypes.Identity)
                {
                    if (null == this.NextOp)
                        return true;
                    else
                    {
                        return this.NextOp.IsIdentity;
                    }
                }

                return false;
            }
        }
        public MatrixTransformTypes OperationType { get; protected set; }
        
        public TransformOperation NextOp { get; private set; }

        protected TransformOperation(MatrixTransformTypes op)
        {
            this.OperationType = op;
        }

        public void AppendTransformation(TransformOperation toAppend)
        {
            if (null == this.NextOp)
            {
                this.NextOp = toAppend;
            }
            else
            {
                this.NextOp.AppendTransformation(toAppend);
            }
        }

        public Matrix2D GetMatrix(Matrix2D mapping, DrawingOrigin origin)
        {
            //TODO: check the order of application.
            mapping = this.DoGetMatrix(mapping, origin);
            
            if (null != this.NextOp)
                mapping = this.NextOp.GetMatrix(mapping, origin);
            
            return mapping;

        }

        protected virtual Matrix2D DoGetMatrix(Matrix2D matrix, DrawingOrigin origin)
        {
            return matrix;
        }

        public static bool ValuesAreIdentity(double[] values)
        {
            if (values.Length == 6 && values[0] == 1.0 && values[3] == 1.0)
                return true;
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            this.ToString(sb);
            return sb.ToString();
        }

        public virtual void ToString(StringBuilder appendTo)
        {
            if (null != this.NextOp)
            {
                appendTo.Append(" ");
                this.NextOp.ToString(appendTo);
            }
        }

        public TransformOperation CloneValuesAndFlatten(Size page, Size container, Size font, Unit rootFont)
        {
            var clone = this.CloneAndFlatten(page, container, font, rootFont);
            if (null != this.NextOp)
                this.NextOp = this.NextOp.CloneValuesAndFlatten(page, container, font, rootFont);

            return clone;
        }

        protected abstract TransformOperation CloneAndFlatten(Size page, Size container, Size font, Unit rootFont);
    }

    public class TransformRotateOperation : TransformOperation
    {
        public double AngleRadians { get; set; }
        
        public TransformRotateOperation(double angleRadians) : base(MatrixTransformTypes.Rotate)
        {
            this.AngleRadians = angleRadians;
        }

        public override void ToString(StringBuilder appendTo)
        {
            appendTo.Append("rotate(");
            appendTo.Append(this.AngleRadians);
            appendTo.Append(TransformOperationSet.RadiansIdentifier);
            appendTo.Append(")");

            base.ToString(appendTo);
        }

        protected override Matrix2D DoGetMatrix(Matrix2D matrix, DrawingOrigin origin)
        {
            var rotate = Matrix2D.Identity;
            var angle = origin == DrawingOrigin.BottomLeft ? (0 - this.AngleRadians) :  this.AngleRadians;
            rotate.Rotate(angle);
            matrix = Matrix2D.Multiply(matrix, rotate);
            
            return base.DoGetMatrix(matrix, origin);
        }

        protected override TransformOperation CloneAndFlatten(Size page, Size container, Size font, Unit rootFont)
        {
            var clone = (TransformRotateOperation) this.MemberwiseClone();
            return clone;
        }
    }

    public class TransformTranslateOperation : TransformOperation
    {
        public Unit XOffset { get; set; }
        
        public Unit YOffset { get; set; }

        public TransformTranslateOperation(Unit x, Unit y) : base(MatrixTransformTypes.Translation)
        {
            this.XOffset = x;
            this.YOffset = y;
        }

        protected override TransformOperation CloneAndFlatten(Size page, Size container, Size font, Unit rootFont)
        {
            var clone = (TransformTranslateOperation)this.MemberwiseClone();
            if (XOffset.IsRelative)
                clone.XOffset = Unit.FlattenHorizontalValue(this.XOffset, page, container, font, rootFont);
            if (YOffset.IsRelative)
                clone.YOffset = Unit.FlattenVerticalValue(this.YOffset, page, container, font, rootFont);

            return clone;
        }

        protected override Matrix2D DoGetMatrix(Matrix2D matrix, DrawingOrigin origin)
        {
            Matrix2D translate = Matrix2D.Identity;
            double x, y;
            
            x = this.XOffset.PointsValue;

            y = (origin == DrawingOrigin.BottomLeft) ? this.YOffset.PointsValue : 0 - this.YOffset.PointsValue;
            
            translate.Translate(x,y);
            matrix = Matrix2D.Multiply(matrix, translate);
            
            return base.DoGetMatrix(matrix, origin);
        }

        public override void ToString(StringBuilder appendTo)
        {
            appendTo.Append("translate(");
            appendTo.Append(this.XOffset.ToString());
            appendTo.Append(", ");
            appendTo.Append(this.YOffset.ToString());
            appendTo.Append(")");
            
            base.ToString(appendTo);
        }
    }

    public class TransformSkewOperation : TransformOperation
    {
        public double XAngleRadians { get; set; }
        public double YAngleRadians { get; set; }

        public TransformSkewOperation(double xRadians, double yRadians) : base(MatrixTransformTypes.Skew)
        {
            this.XAngleRadians = xRadians;
            this.YAngleRadians = yRadians;
        }

        public override void ToString(StringBuilder appendTo)
        {
            appendTo.Append("skew(");
            appendTo.Append(Math.Round(this.XAngleRadians, 6));
            appendTo.Append(", ");
            appendTo.Append(Math.Round(this.YAngleRadians, 6));
            
            base.ToString(appendTo);
        }

        protected override TransformOperation CloneAndFlatten(Size page, Size container, Size font, Unit rootFont)
        {
            return (TransformOperation)this.MemberwiseClone();
        }
    }

    public class TransformScaleOperation : TransformOperation
    {
        public double XScaleValue { get; set; }
        
        public double YScaleValue { get; set; }

        public TransformScaleOperation(double xScale, double yScale) : base(MatrixTransformTypes.Scaling)
        {
            this.XScaleValue = xScale;
            this.YScaleValue = yScale;
        }
        
        public override void ToString(StringBuilder appendTo)
        {
            appendTo.Append("scale(");
            appendTo.Append(this.XScaleValue);
            if (this.XScaleValue != this.YScaleValue)
            {
                appendTo.Append(", ");
                appendTo.Append(this.YScaleValue);
            }
            appendTo.Append(")");

            base.ToString(appendTo);
        }

        protected override TransformOperation CloneAndFlatten(Size page, Size container, Size font, Unit rootFont)
        {
            return (TransformOperation)this.MemberwiseClone();
        }
    }

    public class TransformMatrixOperation : TransformOperation
    {
        public double[] MatrixValues { get; set; }

        public override bool IsIdentity
        {
            get
            {
                if (!ValuesAreIdentity(MatrixValues)) return false;
                else
                {
                    if (null == this.NextOp)
                        return true;
                    else
                        return this.NextOp.IsIdentity;
                }

            }
        }

        public TransformMatrixOperation(double[] values) : base(MatrixTransformTypes.Matrix)
        {
            if (null == values || values.Length != 6)
                throw new ArgumentOutOfRangeException(nameof(values),
                    "The length of a matrix transform operation should have 6 values.");
            this.MatrixValues = values;
            
        }

        protected override TransformOperation CloneAndFlatten(Size page, Size container, Size font, Unit rootFont)
        {
            TransformMatrixOperation matrix = (TransformMatrixOperation)this.MemberwiseClone();
            matrix.MatrixValues = new double[6];
            Array.Copy(this.MatrixValues, matrix.MatrixValues, 6);

            return matrix;
        }

        public override void ToString(StringBuilder appendTo)
        {
            appendTo.Append("matrix(");
            appendTo.Append(string.Join(", ", this.MatrixValues));
            appendTo.Append(")");
            base.ToString(appendTo);
        }
    }
}