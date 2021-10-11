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
    public struct Point : IEquatable<Point>, IComparable<Point>, ICloneable
    {
        private Unit _x;

        private Unit _y;

        public Unit Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public Unit X
        {
            get { return this._x; }
            set { this._x = value; }
        }

        public bool IsEmpty
        {
            get { return this.X.IsEmpty && this.Y.IsEmpty; }
        }

        public Point(double x, double y)
            : this((Unit)x, (Unit)y)
        { }

        public Point(Unit x, Unit y)
        {
            this._x = x;
            this._y = y;
        }

        public Point ToPoints()
        {
            Unit x = this.X.ToPoints();
            Unit y = this.Y.ToPoints();
            return new Point(x,y);
        }


        public override string ToString()
        {
            return "[" + this.X.ToString() + ", " + this.Y.ToString() + "]";
        }

        private static Point _empty = new Point(Unit.Empty, Unit.Empty);

        public static Point Empty
        {
            get { return _empty; }
        }

        #region IEquatable Members

        public bool Equals(Point other)
        {
            return this.X.Equals(other.X) && this.Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            return this.Equals((Point)obj);
        }

        #endregion


        public Point Offset(Point pt)
        {
            return this.Offset(pt.X, pt.Y);
        }

        public Point Offset(Unit x, Unit y)
        {
            return new Point(this.X + x, this.Y + y);
        }

        public override int GetHashCode()
        {
            return this._x.GetHashCode() ^ this._y.GetHashCode();
        }

        #region IComparable<PDFSize> Members

        public int CompareTo(Point other)
        {
            Unit me = this.X.ToPoints() + this.Y.ToPoints();
            Unit them = other.X.ToPoints() + other.Y.ToPoints();
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

        public static bool operator ==(Point one, Point two)
        {
            return (one.Equals(two));
        }

        public static bool operator !=(Point one, Point two)
        {
            return !(one.Equals(two));
        }

        #region ICloneable<PDFPoint> Members

        public Point Clone()
        {
            return (Point)this.MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        // parsing

        private const char Separator = ',';

        public static Point Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
                return Point.Empty;
            else
            {
                try
                {
                    input = input.Trim();
                    Unit x, y;
                    if (input.IndexOf(Separator) > 0)
                    {
                        string[] all = input.Split(Separator);
                        x = Unit.Parse(all[0]);
                        y = Unit.Parse(all[1]);
                    }
                    else
                    {
                        x = Unit.Parse(input);
                        y = x;
                    }

                    Point pt = new Point(x, y);
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
