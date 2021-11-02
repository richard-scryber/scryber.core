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
                float f;
                if (this.TryGetValue(StyleKeys.TransformXScaleKey, out f))
                    return f;
                else
                    return 1.0F;

            }
            set
            {
                this.SetValue(StyleKeys.TransformXScaleKey, value);
            }
        }

        public void RemoveScaleX()
        {
            this.RemoveValue(StyleKeys.TransformXScaleKey);
        }

        #endregion

        #region public float ScaleY {get;set;} + RemoveScaleY()

        [PDFAttribute("scale-y")]
        public float ScaleY
        {
            get
            {
                float f;
                if (this.TryGetValue(StyleKeys.TransformYScaleKey, out f))
                    return f;
                else
                    return 1.0F;

            }
            set
            {
                this.SetValue(StyleKeys.TransformYScaleKey, value);
            }
        }

        public void RemoveScaleY()
        {
            this.RemoveValue(StyleKeys.TransformYScaleKey);
        }

        #endregion

        #region public float Rotate {get;set;} + RemoveRotate()

        [PDFAttribute("rotate")]
        public float Rotate
        {
            get
            {
                float f;
                if (this.TryGetValue(StyleKeys.TransformRotateKey, out f))
                    return f;
                else
                    return 0.0F;

            }
            set
            {
                this.SetValue(StyleKeys.TransformRotateKey, value);
            }
        }

        public void RemoveRotate()
        {
            this.RemoveValue(StyleKeys.TransformRotateKey);
        }

#endregion

        #region public float SkewX {get;set;} + RemoveSkewX()

        [PDFAttribute("skew-x")]
        public float SkewX
        {
            get
            {
                float f;
                if (this.TryGetValue(StyleKeys.TransformXSkewKey, out f))
                    return f;
                else
                    return 0.0F;

            }
            set
            {
                this.SetValue(StyleKeys.TransformXSkewKey, value);
            }
        }

        public void RemoveSkewX()
        {
            this.RemoveValue(StyleKeys.TransformXSkewKey);
        }

#endregion

        #region public float SkewY {get;set;} + RemoveSkewY()

        [PDFAttribute("skew-y")]
        public float SkewY
        {
            get
            {
                float f;
                if (this.TryGetValue(StyleKeys.TransformYSkewKey, out f))
                    return f;
                else
                    return 0.0F;

            }
            set
            {
                this.SetValue(StyleKeys.TransformYSkewKey, value);
            }
        }

        public void RemoveSkewY()
        {
            this.RemoveValue(StyleKeys.TransformYSkewKey);
        }

#endregion

        /* OffsetX, Y and the origin are not supported 
         * 
        
        #region public float OffsetH {get;set;} + RemoveOffsetH()

        [PDFAttribute("offset-h")]
        public float OffsetH
        {
            get
            {
                float f;
                if (this.TryGetValue(PDFStyleKeys.TransformXOffsetKey, out f))
                    return f;
                else
                    return 0.0F;

            }
            set
            {
                this.SetValue(PDFStyleKeys.TransformXOffsetKey, value);
            }
        }

        public void RemoveOffsetH()
        {
            this.RemoveValue(PDFStyleKeys.TransformXOffsetKey);
        }

#endregion

        #region public float OffsetV {get;set;} + RemoveOffsetV()

        [PDFAttribute("offset-v")]
        public float OffsetV
        {
            get
            {
                float f;
                if (this.TryGetValue(PDFStyleKeys.TransformYOffsetKey, out f))
                    return f;
                else
                    return 0.0F;

            }
            set
            {
                this.SetValue(PDFStyleKeys.TransformYOffsetKey, value);
            }
        }

        public void RemoveOffsetV()
        {
            this.RemoveValue(PDFStyleKeys.TransformYOffsetKey);
        }

        #endregion

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

        */


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
