using System;
using System.Text;

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
    }
}