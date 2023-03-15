using System;
namespace Scryber.Styles
{
    /// <summary>
    /// A linked list of transformation operations that can be used to build a transformation matrix
    /// </summary>
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

        public Scryber.Drawing.PDFTransformationMatrix GetMatrix(Scryber.Drawing.MatrixOrder order)
        {
            var matrix = new Drawing.PDFTransformationMatrix();

            switch (order)
            {
                case Drawing.MatrixOrder.Append:
                    this.AppendTransformations(matrix);
                    break;
                default:
                    this.PrependTransformations(matrix);
                    break;
            }

            return matrix;
        }

        protected virtual void AppendTransformations(Drawing.PDFTransformationMatrix matrix)
        {
            this.ApplyTransformation(matrix);
            if (null != this.Next)
                this.Next.AppendTransformations(matrix);
        }

        protected virtual void PrependTransformations(Drawing.PDFTransformationMatrix matrix)
        {
            if (null != this.Next)
                this.Next.PrependTransformations(matrix);
            this.ApplyTransformation(matrix);
        }

        protected virtual void ApplyTransformation(Drawing.PDFTransformationMatrix matrix)
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


    }
}

