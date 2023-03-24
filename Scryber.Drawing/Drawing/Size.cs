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
    public struct Size : IEquatable<Size>, IComparable<Size>, ICloneable
    {
        private Unit _w;

        public Unit Width
        {
            get { return _w; }
            set { _w = value; }
        }

        private Unit _h;

        public Unit Height
        {
            get { return _h; }
            set { _h = value; }
        }

        public bool IsEmpty
        {
            get { return this.Height.IsZero && this.Width.IsZero; }
        }

        /// <summary>
        /// Returns true if one or more of the units in this point are relative units.
        /// </summary>
        public bool IsRelative
        {
            get
            {
                return this._w.IsRelative
                    || this._h.IsRelative;
            }
        }

        public Size(double width, double height, PageUnits units)
            : this(new Unit(width, units), new Unit(height, units))
        {

        }

        public Size(double width, double height)
            : this((Unit)width,(Unit)height)
        {
        }

        public Size(Unit width, Unit height)
        {
            this._h = height;
            this._w = width;
        }

        public Size ToPoints()
        {
            Unit w = this.Width.ToPoints();
            Unit h = this.Height.ToPoints();
            return new Size(w, h);
        }

        public override string ToString()
        {
            return "[" + this.Width.ToString() + ", " + this.Height.ToString() + "]";
        }

        private static readonly Size _empty = new Size(Unit.Empty, Unit.Empty);

        public static Size Empty
        {
            get { return _empty; }
        }

        #region IEquatable Members

        public bool Equals(Size other)
        {
            return this.Width.Equals(other.Width) && this.Height.Equals(other.Height);
        }

        public override bool Equals(object obj)
        {
            if (null == obj)
                return false;

            return this.Equals((Size)obj);
        }

        #endregion

        public override int GetHashCode()
        {
            return this._h.GetHashCode() ^ this._w.GetHashCode();
        }

        #region IComparable<PDFSize> Members

        public int CompareTo(Size other)
        {

            var one = this.Width.CompareTo(other.Width);
            if (one == 0)
                one = this.Height.CompareTo(other.Height);

            return one;
            
        }

        #endregion

        public Size Subtract(Thickness thickness)
        {
            return new Size(this.Width - thickness.Left - thickness.Right, this.Height - thickness.Top - thickness.Bottom);
        }

        public static bool operator ==(Size one, Size two)
        {
            return (one.Equals(two));
        }

        public static bool operator !=(Size one, Size two)
        {
            return !(one.Equals(two));
        }

        public Size Clone()
        {
            return (Size)this.MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        
    }
}
