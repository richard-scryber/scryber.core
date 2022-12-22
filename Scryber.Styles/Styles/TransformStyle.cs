/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber;
using Scryber.Drawing;
using System.ComponentModel;

namespace Scryber.Styles
{

    /// <summary>
    /// Defines a matrix transform style
    /// </summary>
    /// <remarks>The transformations on a single style are built in to a PDFTransformMatrix 
    /// in the order of Translate, Rotate, Scale and Skew as defined in the style</remarks>
    [PDFParsableComponent("Transform")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TransformStyle : StyleItemBase
    {

        #region public float ScaleX {get;set;} + RemoveScaleX()

        [PDFAttribute("scale-x")]
        public float ScaleX
        {
            get
            {
                var op = this.Operations;
                TransformOperation scale;

                if (null != op && op.TryGetType(TransformType.Scale, out scale))
                    return scale.Value1;
                else
                    return TransformOperation.NotSetValue();

            }
            set
            {
                var op = this.Operations;
                TransformOperation scale;

                if (null == op)
                    this.Operations = new TransformOperation(TransformType.Scale, value, TransformOperation.NotSetValue());
                else if (op.TryGetType(TransformType.Scale, out scale))
                    scale.Value1 = value;
                else
                    op.Append(new TransformOperation(TransformType.Scale, value, TransformOperation.NotSetValue()));

            }
        }

        public void RemoveScaleX()
        {
            var op = this.Operations;
            TransformOperation scale;

            if (op.TryGetType(TransformType.Scale, out scale))
            {
                scale.Value1 = TransformOperation.NotSetValue();

                if (TransformOperation.IsNotSet(scale.Value2))
                    this.Operations = TransformOperation.Remove(TransformType.Scale, op);
            }
        }

        #endregion

        #region public float ScaleY {get;set;} + RemoveScaleY()

        [PDFAttribute("scale-y")]
        public float ScaleY
        {
            get
            {
                var op = this.Operations;
                TransformOperation scale;

                if (null != op && op.TryGetType(TransformType.Scale, out scale))
                    return scale.Value2;
                else
                    return TransformOperation.NotSetValue();

            }
            set
            {
                var op = this.Operations;
                TransformOperation scale;

                if (null == op)
                    this.Operations = new TransformOperation(TransformType.Scale, TransformOperation.NotSetValue(), value);
                else if (op.TryGetType(TransformType.Scale, out scale))
                    scale.Value2 = value;
                else
                    op.Append(new TransformOperation(TransformType.Scale, TransformOperation.NotSetValue(), value));

            }
        }

        public void RemoveScaleY()
        {
            var op = this.Operations;
            TransformOperation scale;

            if (op.TryGetType(TransformType.Scale, out scale))
            {
                scale.Value2 = TransformOperation.NotSetValue();

                if (TransformOperation.IsNotSet(scale.Value1))
                    this.Operations = TransformOperation.Remove(TransformType.Scale, op);
            }
        }

        #endregion

        #region public float Rotate {get;set;} + RemoveRotate()

        [PDFAttribute("rotate")]
        public float Rotate
        {
            get
            {
                var op = this.Operations;
                TransformOperation rot;

                if (null != op && op.TryGetType(TransformType.Rotate, out rot))
                    return rot.Value1;
                else
                    return TransformOperation.NotSetValue();

            }
            set
            {
                var op = this.Operations;
                TransformOperation rot;

                if (null == op)
                    this.Operations = new TransformOperation(TransformType.Rotate, value, TransformOperation.NotSetValue());
                else if (op.TryGetType(TransformType.Rotate, out rot))
                    rot.Value1 = value;
                else
                    op.Append(new TransformOperation(TransformType.Rotate, value, TransformOperation.NotSetValue()));

            }
        }

        public void RemoveRotate()
        {
            var op = this.Operations;   
            TransformOperation scale;

            if (op.TryGetType(TransformType.Rotate, out scale))
            {
                this.Operations = TransformOperation.Remove(TransformType.Rotate, op);
            }
        }

#endregion

 #region public float SkewX {get;set;} + RemoveSkewX()

        [PDFAttribute("skew-x")]
        public float SkewX
        {
            get
            {
                var op = this.Operations;
                TransformOperation skew;

                if (null != op && op.TryGetType(TransformType.Skew, out skew))
                    return skew.Value1;
                else
                    return TransformOperation.NotSetValue();

            }
            set
            {
                var op = this.Operations;
                TransformOperation skew;

                if (null == op)
                    this.Operations = new TransformOperation(TransformType.Skew, value, TransformOperation.NotSetValue());
                else if (op.TryGetType(TransformType.Skew, out skew))
                    skew.Value1 = value;
                else
                    op.Append(new TransformOperation(TransformType.Skew, value, TransformOperation.NotSetValue()));

            }
        }

        public void RemoveSkewX()
        {
            var op = this.Operations;
            TransformOperation skew;

            if (op.TryGetType(TransformType.Skew, out skew))
            {
                skew.Value1 = TransformOperation.NotSetValue();

                if (TransformOperation.IsNotSet(skew.Value2))
                    this.Operations = TransformOperation.Remove(TransformType.Skew, op);
            }
        }

#endregion

        #region public float SkewY {get;set;} + RemoveSkewY()

        [PDFAttribute("skew-y")]
        public float SkewY
        {
            get
            {
                var op = this.Operations;
                TransformOperation skew;

                if (null != op && op.TryGetType(TransformType.Skew, out skew))
                    return skew.Value2;
                else
                    return TransformOperation.NotSetValue();

            }
            set
            {
                var op = this.Operations;
                TransformOperation skew;

                if (null == op)
                    this.Operations = new TransformOperation(TransformType.Skew, TransformOperation.NotSetValue(), value);
                else if (op.TryGetType(TransformType.Skew, out skew))
                    skew.Value2 = value;
                else
                    op.Append(new TransformOperation(TransformType.Skew, TransformOperation.NotSetValue(), value));
                    
            }
        }

        public void RemoveSkewY()
        {
            var op = this.Operations;
            TransformOperation skew;

            if (op.TryGetType(TransformType.Skew, out skew))
            {
                skew.Value2 = TransformOperation.NotSetValue();

                if (TransformOperation.IsNotSet(skew.Value1))
                    this.Operations = TransformOperation.Remove(TransformType.Skew, op);
            }
            
        }

        #endregion

        

        #region public float TranslateX {get;set;} + RemoveTranslateX()

        [PDFAttribute("translate-x")]
        public float TranslateX
        {
            get
            {
                var op = this.Operations;
                TransformOperation translate;

                if (null != op && op.TryGetType(TransformType.Translate, out translate))
                    return translate.Value1;
                else
                    return TransformOperation.NotSetValue();

            }
            set
            {
                var op = this.Operations;
                TransformOperation translate;

                if (null == op)
                    this.Operations = new TransformOperation(TransformType.Translate, value, TransformOperation.NotSetValue());
                else if (op.TryGetType(TransformType.Translate, out translate))
                    translate.Value1 = value;
                else
                    op.Append(new TransformOperation(TransformType.Translate, value, TransformOperation.NotSetValue()));

            }
        }

        public void RemoveTranslateX()
        {
            var op = this.Operations;
            TransformOperation trans;

            if (op.TryGetType(TransformType.Translate, out trans))
            {
                trans.Value1 = TransformOperation.NotSetValue();

                if (TransformOperation.IsNotSet(trans.Value2))
                    this.Operations = TransformOperation.Remove(TransformType.Translate, op);
            }
        }

        #endregion

        #region public float TranslateY {get;set;} + RemoveTranslateY()


        [PDFAttribute("translate-y")]
        public float TranslateY
        {
            get
            {
                var op = this.Operations;
                TransformOperation translate;

                if (null != op && op.TryGetType(TransformType.Translate, out translate))
                    return translate.Value2;
                else
                    return TransformOperation.NotSetValue();

            }
            set
            {
                var op = this.Operations;
                TransformOperation translate;

                if (null == op)
                    this.Operations = new TransformOperation(TransformType.Translate, TransformOperation.NotSetValue(), value);
                else if (op.TryGetType(TransformType.Translate, out translate))
                    translate.Value2 = value;
                else
                    op.Append(new TransformOperation(TransformType.Translate, TransformOperation.NotSetValue(), value));

            }
        }

        public void RemoveTranslateY()
        {
            var op = this.Operations;
            TransformOperation translate;

            if (op.TryGetType(TransformType.Translate, out translate))
            {
                translate.Value2 = TransformOperation.NotSetValue();

                if (TransformOperation.IsNotSet(translate.Value1))
                    this.Operations = TransformOperation.Remove(TransformType.Translate, op);
            }
        }

        #endregion

        /* Not currently supported
         * 
         * 
        #region public TransformationOrigin TransformationOrigin {get;set;} + RemoveOrigin()

        [PDFAttribute("origin")]
        public TransformationOrigin TransformationOrigin
        {
            get
            {
                TransformationOrigin origin = TransformationOrigin.CenterMiddle;
                if (this.TryGetValue(PDFStyleKeys.TransformOriginKey, out origin))
                    return origin;
                else
                    return TransformationOrigin.CenterMiddle;
            }

            set
            {
                this.SetValue(PDFStyleKeys.TransformOriginKey, value);
            }
        }

        #endregion


        *
        */
        


        public TransformOperation Operations
        {
            get
            {
                TransformOperation operation = null;
                if (this.TryGetValue(StyleKeys.TransformOperationKey, out operation))
                    return operation;
                else
                    return null;
            }
            set
            {
                this.SetValue(StyleKeys.TransformOperationKey, value);
            }
        }

        #region public bool IsIdentity {get;}

        /// <summary>
        /// Returns true if the matrix described by this style is the identity matrix (no change)
        /// </summary>
        public bool IsIdentity
        {
            get
            {
                return this.GetMatrix().IsIdentity;
            }
        }

        #endregion

        //
        // .ctor
        //

        public TransformStyle()
            : base(StyleKeys.TransformItemKey)
        {
        }

        //
        // public methods
        //

        #region public Scryber.Drawing.Drawing2D.MatrixOrder GetOrder()

        /// <summary>
        /// Returns the matrix drawing order
        /// </summary>
        /// <returns></returns>
        public Scryber.Drawing.MatrixOrder GetOrder()
        {
            return Scryber.Drawing.MatrixOrder.Append;
        }

        #endregion

        #region public PDFTransformationMatrix GetMatrix()

        /// <summary>
        /// Gets the matrix described by this transformation style
        /// </summary>
        /// <returns></returns>
        public PDFTransformationMatrix GetMatrix()
        {
            PDFTransformationMatrix current = new PDFTransformationMatrix();
            
            //current.SetTranslation(this.OffsetH, this.OffsetV);
            current.SetRotation(this.Rotate);
            current.SetScale(this.ScaleX, this.ScaleY);
            current.SetSkew(this.SkewX, this.SkewY);
            return current;
        }

        #endregion
    }


}
