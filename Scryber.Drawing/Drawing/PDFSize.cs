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
using System.ComponentModel;

namespace Scryber.Drawing
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct PDFSize : IEquatable<PDFSize>, IComparable<PDFSize>, ICloneable
    {
        private PDFUnit _w;

        public PDFUnit Width
        {
            get { return _w; }
            set { _w = value; }
        }

        private PDFUnit _h;

        public PDFUnit Height
        {
            get { return _h; }
            set { _h = value; }
        }

        public bool IsEmpty
        {
            get { return this.Height.IsEmpty && this.Width.IsEmpty; }
        }

        public PDFSize(double width, double height)
            : this((PDFUnit)width,(PDFUnit)height)
        {
        }

        public PDFSize(PDFUnit width, PDFUnit height)
        {
            this._h = height;
            this._w = width;
        }

        public PDFSize ToPoints()
        {
            PDFUnit w = this.Width.ToPoints();
            PDFUnit h = this.Height.ToPoints();
            return new PDFSize(w, h);
        }

        public System.Drawing.SizeF ToDrawing()
        {
            PDFUnit w = this.Width.ToPoints();
            PDFUnit h = this.Height.ToPoints();
            return new System.Drawing.SizeF((float)w.Value, (float)h.Value);
        }

        public override string ToString()
        {
            return "[" + this.Width.ToString() + ", " + this.Height.ToString() + "]";
        }

        private static PDFSize _empty = new PDFSize(PDFUnit.Empty, PDFUnit.Empty);

        public static PDFSize Empty
        {
            get { return _empty; }
        }

        #region IEquatable Members

        public bool Equals(PDFSize other)
        {
            return this.Width.Equals(other.Width) && this.Height.Equals(other.Height);
        }

        public override bool Equals(object obj)
        {
            if (null == obj)
                return false;

            return this.Equals((PDFSize)obj);
        }

        #endregion

        public override int GetHashCode()
        {
            return this._h.GetHashCode() ^ this._w.GetHashCode();
        }

        #region IComparable<PDFSize> Members

        public int CompareTo(PDFSize other)
        {
            PDFUnit me = this.Width.ToPoints() + this.Height.ToPoints();
            PDFUnit them = other.Width.ToPoints() + other.Height.ToPoints();
            if (me.Equals(them))
            {
                me = this.Width.ToPoints();
                them = other.Width.ToPoints();
                if(me.Equals(them))
                {
                    me = this.Height.ToPoints();
                    them = other.Height.ToPoints();
                }
            }
            
            return me.CompareTo(them);
            
        }

        #endregion

        public PDFSize Subtract(PDFThickness thickness)
        {
            return new PDFSize(this.Width - thickness.Left - thickness.Right, this.Height - thickness.Top - thickness.Bottom);
        }

        public static bool operator ==(PDFSize one, PDFSize two)
        {
            return (one.Equals(two));
        }

        public static bool operator !=(PDFSize one, PDFSize two)
        {
            return !(one.Equals(two));
        }

        public PDFSize Clone()
        {
            return (PDFSize)this.MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        
    }
}
