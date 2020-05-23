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
    [PDFParsableValue()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct PDFPoint : IEquatable<PDFPoint>, IComparable<PDFPoint>, ICloneable
    {
        private PDFUnit _x;

        private PDFUnit _y;

        public PDFUnit Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public PDFUnit X
        {
            get { return this._x; }
            set { this._x = value; }
        }

        public bool IsEmpty
        {
            get { return this.X.IsEmpty && this.Y.IsEmpty; }
        }

        public PDFPoint(double x, double y)
            : this((PDFUnit)x, (PDFUnit)y)
        { }

        public PDFPoint(PDFUnit x, PDFUnit y)
        {
            this._x = x;
            this._y = y;
        }

        public PDFPoint ToPoints()
        {
            PDFUnit x = this.X.ToPoints();
            PDFUnit y = this.Y.ToPoints();
            return new PDFPoint(x,y);
        }

        public System.Drawing.PointF ToDrawing()
        {
            PDFUnit x = this.X.ToPoints();
            PDFUnit y = this.Y.ToPoints();
            return new System.Drawing.PointF((float)x.Value, (float)y.Value);
        }

        public override string ToString()
        {
            return "[" + this.X.ToString() + ", " + this.Y.ToString() + "]";
        }

        private static PDFPoint _empty = new PDFPoint(PDFUnit.Empty, PDFUnit.Empty);

        public static PDFPoint Empty
        {
            get { return _empty; }
        }

        #region IEquatable Members

        public bool Equals(PDFPoint other)
        {
            return this.X.Equals(other.X) && this.Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            return this.Equals((PDFPoint)obj);
        }

        #endregion


        public PDFPoint Offset(PDFPoint pt)
        {
            return this.Offset(pt.X, pt.Y);
        }

        public PDFPoint Offset(PDFUnit x, PDFUnit y)
        {
            return new PDFPoint(this.X + x, this.Y + y);
        }

        public override int GetHashCode()
        {
            return this._x.GetHashCode() ^ this._y.GetHashCode();
        }

        #region IComparable<PDFSize> Members

        public int CompareTo(PDFPoint other)
        {
            PDFUnit me = this.X.ToPoints() + this.Y.ToPoints();
            PDFUnit them = other.X.ToPoints() + other.Y.ToPoints();
            if (me.Equals(them))
            {
                me = this.X.ToPoints();
                them = other.X.ToPoints();
                if(me.Equals(them))
                {
                    me = this.Y.ToPoints();
                    them = other.Y.ToPoints();
                }
            }
            
            return me.CompareTo(them);
            
        }

        #endregion

        public static bool operator ==(PDFPoint one, PDFPoint two)
        {
            return (one.Equals(two));
        }

        public static bool operator !=(PDFPoint one, PDFPoint two)
        {
            return !(one.Equals(two));
        }

        #region ICloneable<PDFPoint> Members

        public PDFPoint Clone()
        {
            return (PDFPoint)this.MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        // parsing

        private const char Separator = ',';

        public static PDFPoint Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
                return PDFPoint.Empty;
            else
            {
                try
                {
                    input = input.Trim();
                    PDFUnit x, y;
                    if (input.IndexOf(Separator) > 0)
                    {
                        string[] all = input.Split(Separator);
                        x = PDFUnit.Parse(all[0]);
                        y = PDFUnit.Parse(all[1]);
                    }
                    else
                    {
                        x = PDFUnit.Parse(input);
                        y = x;
                    }

                    PDFPoint pt = new PDFPoint(x, y);
                    return pt;
                }
                catch (Exception ex)
                {
                    string msg = String.Format(Errors.CouldNotParseValue_3,input,"PDFPoint","Unit,Unit");
                    throw new PDFException(msg, ex);
                }
            }
        }
    }
}
