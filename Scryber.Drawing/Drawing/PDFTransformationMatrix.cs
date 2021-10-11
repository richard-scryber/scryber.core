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
using System.Text;
using Scryber.PDF.Graphics;

namespace Scryber.Drawing
{
    public class PDFTransformationMatrix : IPDFGraphicsAdapter, ICloneable
    {

        private System.Drawing.Drawing2D.Matrix _matrix;
        
        
        public float[] Components
        {
            get { return _matrix.Elements; }
        }

        
        
        public bool IsIdentity
        {
            get { return _matrix.IsIdentity; }
        }

        public PDFTransformationMatrix()
        {
            _matrix = new System.Drawing.Drawing2D.Matrix();
            _matrix.Reset();
        }

        public PDFTransformationMatrix(float offsetX, float offsetY, float angle, float scaleX, float scaleY) 
            : this()
        {
            _matrix.Translate(offsetX, offsetY);
            _matrix.Rotate(angle);
            _matrix.Scale(scaleX, scaleY);
        }

        protected PDFTransformationMatrix(System.Drawing.Drawing2D.Matrix innerMatrix)
        {
            this._matrix = innerMatrix;
        }

        public void SetTranslation(Unit offsetX, Unit offsetY)
        {
            SetTranslation((float)offsetX.PointsValue, (float)offsetY.PointsValue);
        }

        public void SetTranslation(float offsetX, float offsetY)
        {
            _matrix.Translate(offsetX, offsetY);
        }

        public void SetRotation(float angle)
        {
            _matrix.Rotate(angle);
        }

        public void SetScale(float scaleX, float scaleY)
        {
            _matrix.Scale(scaleX, scaleY);
        }

        public void SetSkew(float skewX, float skewY)
        {
            _matrix.Shear(skewX, skewY);
        }

        public Rect TransformBounds(Rect bounds, TransformationOrigin origin)
        {
            Unit xoffset = 0;
            Unit yoffset = 0;

            switch (origin)
            {
                case TransformationOrigin.BottomLeft:
                    //Do nothing - this is the default
                    break;
                case TransformationOrigin.TopLeft:
                    yoffset = (bounds.Y + bounds.Height);
                    break;
                case TransformationOrigin.TopRight:
                    xoffset = (bounds.X + bounds.Width);
                    yoffset = (bounds.Y + bounds.Height);
                    break;
                case TransformationOrigin.BottomRight:
                    xoffset = (bounds.X + bounds.Width);
                    break;
                case TransformationOrigin.CenterMiddle:
                    xoffset = (bounds.X + (bounds.Width / 2));
                    yoffset = (bounds.Y + (bounds.Height / 2));
                    break;
                case TransformationOrigin.Origin:
                    break;
                default:
                    break;
            }
            bounds.X -= xoffset;
            bounds.Y -= yoffset;

            Rect transformed = TransformBounds(bounds);

            transformed.X += xoffset;
            transformed.Y += yoffset;

            return transformed;
        }

        public Rect TransformBounds(Rect bounds)
        {
            if (this.IsIdentity)
                return bounds;

            float width = (float)bounds.Width.PointsValue;
            float height = (float)bounds.Height.PointsValue;

            System.Drawing.PointF[] all = new System.Drawing.PointF[4];

            all[0] = new System.Drawing.PointF((float)bounds.X.PointsValue, (float)bounds.Y.PointsValue);
            all[1] = new System.Drawing.PointF((float)bounds.X.PointsValue, (float)(bounds.Y.PointsValue + bounds.Height.PointsValue));
            all[2] = new System.Drawing.PointF((float)(bounds.X.PointsValue + bounds.Width.PointsValue), (float)(bounds.Y.PointsValue + bounds.Height.PointsValue));
            all[3] = new System.Drawing.PointF((float)(bounds.X.PointsValue + bounds.Width.PointsValue), (float)bounds.Y.PointsValue);

            

            this._matrix.TransformPoints(all);

            double maxX = all[0].X;
            double minX = all[0].X;
            double maxY = all[0].Y;
            double minY = all[0].Y;

            for (int i = 1; i < 4; i++)
            {
                maxX = Math.Max(maxX, all[i].X);
                minX = Math.Min(minX, all[i].X);
                maxY = Math.Max(maxY, all[i].Y);
                minY = Math.Min(minY, all[i].Y);
            }

            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }

        public Point TransformPoint(Point pt)
        {
            if (IsIdentity)
                return pt;

            System.Drawing.PointF[] ptf = new System.Drawing.PointF[] { new System.Drawing.PointF((float)pt.X.PointsValue, (float)pt.Y.PointsValue) };
            this._matrix.TransformPoints(ptf);
            return new Point(ptf[0].X, ptf[0].Y);
        }

        //
        // calculations
        //

        public static PDFTransformationMatrix Multiply(PDFTransformationMatrix one, PDFTransformationMatrix two)
        {
            if (null == one)
                throw new ArgumentNullException("one");
            if (null == two)
                throw new ArgumentNullException("two");

            one = new PDFTransformationMatrix(one._matrix.Clone());
            one._matrix.Multiply(two._matrix);
            
            return one;
        }

        public static PDFTransformationMatrix operator *(PDFTransformationMatrix one, PDFTransformationMatrix two)
        {
            return Multiply(one, two);
        }

        public static PDFTransformationMatrix Identity()
        {
            
            return new PDFTransformationMatrix(0, 0, 0, 1, 1);
        }
        //
        // graphics adapters
        //

        public bool SetUpGraphics(PDFGraphics graphics, Rect bounds)
        {
            graphics.SetTransformationMatrix(this, true, true);
            return true;
        }

        public void ReleaseGraphics(PDFGraphics g, Rect bounds)
        {
            //Do Nothing
        }

        //
        // cloneing
        //

        public PDFTransformationMatrix Clone()
        {
            return new PDFTransformationMatrix(this._matrix);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}
