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
    /// <summary>
    /// 
    /// </summary>
    [PDFParsableValue()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct Rect : IEquatable<Rect>, IComparable<Rect>, ICloneable
    {
        public const char RectangleStartChar = '[';
        public const char RectangleEndChar = ']';
        public static readonly char[] RectangleSeparatorChars = new char[] {' ',','};
        public const bool RectangleStartAndEndCharRequired = false;


        private Unit _x;

        public Unit X
        {
            get { return _x; }
            set { _x = value; }
        }

        private Unit _y;

        public Unit Y
        {
            get { return _y; }
            set { _y = value; }
        }

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

        public Point Location
        {
            get { return new Point(this.X, this.Y); }
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        public Size Size
        {
            get { return new Size(this.Width, this.Height); }
            set
            {
                this.Width = value.Width;
                this.Height = value.Height;
            }
        }

        /// <summary>
        /// Returns true if and only if all the units are zero
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (this._x.Value == 0.0 && this._y.Value == 0.0 && this._w == 0.0 && this._h == 0.0)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Returns true if one or more of the units in this rect are relative units.
        /// </summary>
        public bool IsRelative
        {
            get
            {
                return this._x.IsRelative || this._y.IsRelative || this._w.IsRelative || this._h.IsRelative;
            }
        }

        public Rect(double x, double y, double width, double height)
            : this((Unit)x, (Unit)y, (Unit)width, (Unit)height)
        {
        }

        public Rect(Unit x, Unit y, Unit width, Unit height)
        {
            this._x = x;
            this._y = y;
            this._w = width;
            this._h = height;
        }

        public Rect(Point location, Size size)
        {
            this._x = location.X;
            this._y = location.Y;
            this._w = size.Width;
            this._h = size.Height;
        }

        public override bool Equals(object obj)
        {
            if ((obj is Rect) == false)
                return false;
            else
                return Equals((Rect)obj);
        }

        public bool Equals(Rect rect)
        {
            return Equal(this, rect);
        }

        public static bool Equal(Rect one, Rect two)
        {
            return (one.X == two.X) && (one.Y == two.Y) && (one.Width == two.Width) && (one.Height == two.Height);
        }

        public static bool operator ==(Rect left, Rect right)
        {
            return Equal(left, right);
        }

        public static bool operator !=(Rect left, Rect right)
        {
            return Equal(left, right) == false;
        }

        public override int GetHashCode()
        {
            int x = this.X.GetHashCode();
            int y = this.Y.GetHashCode();
            int w = this.Width.GetHashCode();
            int h = this.Height.GetHashCode();

            return (((x ^ ((y << 13) | (y >> 0x13))) ^ ((w << 0x1a) | (w >> 6))) ^ ((h << 7) | (h >> 0x19)));
        }

        public bool Contains(Unit x, Unit y)
        {
            return (this.X <= x) && (x < (this.X + this.Width)) && (this.Y <= y) && (y < (this.Y + this.Height));
        }

        public bool Contains(Point point)
        {
            return this.Contains(point.X, point.Y);
        }

        /// <summary>
        /// Increases the size of this rectagle by the specified width and height. Returning the resulting rectangle
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Rect Inflate(Unit width, Unit height)
        {
            Rect r = this.Clone();
            r.Width += width;
            r.Height += height;
            return r;
        }

        /// <summary>
        /// Insets this rectangle by the specified thickness. Offsetting the location by Top and Left values
        /// and reducing the size by all values
        /// </summary>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public Rect Inset(Thickness thickness)
        {
            Rect r = this.Clone();
            r.X += thickness.Left;
            r.Y += thickness.Top;

            r.Width -= thickness.Left + thickness.Right;
            r.Height -= thickness.Top + thickness.Bottom;

            return r;
        }

        /// <summary>
        /// Outsets this rectangle by adding the thickness values and subtacting from the location
        /// </summary>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public Rect Outset(Thickness thickness)
        {
            Rect r = this.Clone();
            r.X -= thickness.Left;
            r.Y -= thickness.Top;

            r.Width += thickness.Left + thickness.Right;
            r.Height += thickness.Top + thickness.Bottom;

            return r;
        }

        /// <summary>
        /// Increases the size of this rectagle by the specified width and height. Returning the resulting rectangle
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Rect Inflate(Size size)
        {
            return this.Inflate(size.Width, size.Height);
        }

        public Rect Inflate(Thickness thickness)
        {
            Rect rect2 = this.Clone();
            rect2.X -= thickness.Left;
            rect2.Y -= thickness.Top;
            rect2.Width += thickness.Left + thickness.Right;
            rect2.Height += thickness.Top + thickness.Bottom;
            return rect2;
        }

        public static Rect Inflate(Rect rect, Unit x, Unit y)
        {
            Rect rect2 = rect.Clone();
            rect2 = rect2.Inflate(x, y);
            return rect2;
        }

        public Rect Intersect(Rect rect)
        {
            return Rect.Intersect(this, rect);
        }

        public bool IntersectsWith(Rect rect)
        {
            return ((((rect.X < (this.X + this.Width)) && (this.X < (rect.X + rect.Width))) && (rect.Y < (this.Y + this.Height))) && (this.Y < (rect.Y + rect.Height)));
        }

        public static Rect Intersect(Rect a, Rect b)
        {
            Unit x1 = Unit.Max(a.X, b.X);
            Unit x2 = Unit.Min(a.X + a.Width, b.X + b.Width);
            Unit y1 = Unit.Max(a.Y, b.Y);
            Unit y2 = Unit.Min(a.Y + a.Height, b.Y + b.Height);

            if ((x2 >= x1) && (y2 >= y1))
                return new Rect(x1, y1, x2 - x1, y2 - y1);
            else
                return Rect.Empty;
        }

        public static Rect Union(Rect a, Rect b)
        {
            Unit x = Unit.Min(a.X, b.X);
            Unit x2 = Unit.Max(a.X + a.Width, b.X + b.Width);
            Unit y = Unit.Min(a.Y, b.Y);
            Unit y2 = Unit.Max(a.Y + a.Height, b.Y + b.Height);

            return new Rect(x, y, x2 - x, y2 - y);
        }

        /// <summary>
        /// Finds the rect that encloses all the points provided returning the minimum x, minimum y and required width and height to encompass. e.g. [2,4], [3, 2], [1, 2] = [1,2,2,3]
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static Rect Bounds(params Point[] pts)
        {
            if (pts.Length == 0)
                return Rect.Empty;

            var minx = pts[0].X;
            var miny = pts[0].Y;
            var maxx = minx;
            var maxy = miny;

            for (var i = 1; i < pts.Length; i++)
            {
                minx = Unit.Min(minx, pts[i].X);
                miny = Unit.Min(miny, pts[i].Y);
                maxx = Unit.Max(maxx, pts[i].X);
                maxy = Unit.Max(maxy, pts[i].Y);
            }

            Rect bounds = new Rect(minx, miny, maxx - minx, maxy - miny);
            return bounds;

        }

        public Rect Offset(Unit x, Unit y)
        {
            Rect rect2 = this.Clone();
            rect2.X += x;
            rect2.Y += y;
            return rect2;
        }

        public Rect Offset(Point pt)
        {
            return this.Offset(pt.X, pt.Y);
        }

        public override string ToString()
        {
            return "[" + this.X.ToString() + ", " + this.Y.ToString() + ", " + this.Width.ToString() + ", " + this.Height.ToString() + "]";
        }


        public static Rect Empty
        {
            get { return new Rect(); }
        }


        #region IComparable<PDFRectangle> Members

        public int CompareTo(Rect other)
        {
            int i = this.Location.CompareTo(other.Location);
            if (i == 0)
                i = this.Size.CompareTo(other.Size);
            return i;
        }

        #endregion

        #region ICloneable<PDFRectangle> Members

        public Rect Clone()
        {
            return this;
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion


        #region public static PDFRect Parse(string)

        /// <summary>
        /// Parses a string value in the format [T,L,W,H] or [All] into a PDFRect instance
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <returns>A new PDFRect instance</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentException" />
        public static Rect Parse(string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentNullException("value", String.Format(Errors.CouldNotParseValue_3, value, "PDFRect", "[T L W H] OR [All]"));

            if (value.StartsWith(RectangleStartChar.ToString()) || value.EndsWith(RectangleEndChar.ToString()))
                value = value.Substring(1, value.Length - 2);

            else if (RectangleStartAndEndCharRequired)
                throw new ArgumentNullException("value", String.Format(Errors.CouldNotParseValue_3, value, "PDFRect", "[T L W H] OR [All]"));

            Unit t, l, w, h;

            string[] rect = value.Split(RectangleSeparatorChars);
            if (rect.Length == 1)
            {
                if (Unit.TryParse(rect[0], out t) == false)
                    throw new ArgumentException("value", String.Format(Errors.CouldNotParseValue_3, value, "PDFRect", "[T L W H] OR [All]"));
                else
                {
                    l = w = h = t;
                }
            }
            else if (rect.Length != 4)
                throw new ArgumentException("value", String.Format(Errors.CouldNotParseValue_3, value, "PDFRect", "[T L W H] OR [All]"));
            else
            {

                if (Unit.TryParse(rect[0], out t) == false ||
                    Unit.TryParse(rect[1], out l) == false ||
                    Unit.TryParse(rect[2], out w) == false ||
                    Unit.TryParse(rect[3], out h) == false)
                    throw new ArgumentException("value", String.Format(Errors.CouldNotParseValue_3, value, "PDFRect", "[T L W H] OR [All]"));
            }
            return new Rect(t, l, w, h);
        }

        #endregion
    }
}
