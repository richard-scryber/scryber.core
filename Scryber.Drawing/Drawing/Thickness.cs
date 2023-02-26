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
using System.Drawing;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Scryber.Drawing
{
    /// <summary>
    /// Defines the thickness of edges (Margins, Patting, Spacing)
    /// </summary>
    [PDFParsableValue()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct Thickness : IComparable<Thickness>, IEquatable<Thickness>, ICloneable
    {
        public const char ThicknessStartChar = '[';
        public const char ThicknessSeparatorChar = ' ';
        public const char ThicknessEndChar = ']';
        public const bool ThicknessStartAndEndRequired = false;

        #region Top{get;set}, Left{get;set}, Bottom{get;set}, Right{get;set}

        private Unit _top;
        /// <summary>
        /// Gets or Sets the top thickness
        /// </summary>
        /// <value>The new thickness</value>
        public Unit Top
        {
            get { return _top; }
            set { _top = value; }
        }

        private Unit _left;
        /// <summary>
        /// Gets or Sets the left thickness
        /// </summary>
        /// <value>The new thickness</value>
        public Unit Left
        {
            get { return _left; }
            set { _left = value; }
        }

        private Unit _bottom;
        /// <summary>
        /// Gets or sets the Botton thickness
        /// </summary>
        /// <value>The new thickness</value>
        public Unit Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }

        private Unit _right;
        /// <summary>
        /// Gets or sets the Right thickness
        /// </summary>
        /// <value>The new thickness</value>
        public Unit Right
        {
            get { return _right; }
            set { _right = value; }
        }

        #endregion

        /// <summary>
        /// Returns true if and only if all the units are zero
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (this._top.Value == 0.0 && this._left.Value == 0.0 && this._bottom == 0.0 && this._right == 0.0)
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
                return this._top.IsRelative
                    || this._left.IsRelative
                    || this._bottom.IsRelative
                    || this._right.IsRelative;
            }
        }

        #region SetAll(all)

        /// <summary>
        /// Sets all edges in the thickness to the value of all
        /// </summary>
        /// <param name="all">The new value of all edges</param>
        public void SetAll(Unit all)
        {
            this._bottom = this._left = this._right = this._top = all;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PDFThickness with the edges set to all
        /// </summary>
        /// <param name="all">The thickness of each edge</param>
        public Thickness(Unit all)
            : this(all, all, all, all)
        {
        }

        /// <summary>
        /// Creates a new instance of a PDFThickness
        /// </summary>
        /// <param name="top">The top thickness</param>
        /// <param name="left">The left thickness</param>
        /// <param name="bottom">The bottom thickness</param>
        /// <param name="right">The right thickness</param>
        public Thickness(Unit top, Unit right, Unit bottom, Unit left)
        {
            this._left = left;
            this._top = top;
            this._right = right;
            this._bottom = bottom;
        }

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Returns an integer value representing the PDFThickness. This is not guarunteed to be unique for any specified thickness
        /// </summary>
        /// <returns>A representative thickness</returns>
        public override int GetHashCode()
        {
            return (int)(((((uint)this.Top.GetHashCode()) ^ ((((uint)this.Left.GetHashCode()) << 13)
                | (((uint)this.Left.GetHashCode()) >> 0x13))) ^ ((((uint)this.Bottom.GetHashCode()) << 0x1a)
                | (((uint)this.Bottom.GetHashCode()) >> 6))) ^ ((((uint)this.Right.GetHashCode()) << 7)
                | (((uint)this.Right.GetHashCode()) >> 0x19)));
        }

        #endregion

        #region Equals()
        /// <summary>
        /// Compares this instance with the other an returns true if they have the same edges
        /// </summary>
        /// <param name="other">The other thickness to compare</param>
        /// <returns>True if the two instances are the same</returns>
        public bool Equals(Thickness other)
        {
            return this.Top.Equals(other.Top)
                    && this.Left.Equals(other.Left)
                    && this.Bottom.Equals(other.Bottom)
                    && this.Right.Equals(other.Right);
        }

        /// <summary>
        /// Compares this instance with the other an returns true if the other is a PDFTickness and they have the same edges
        /// </summary>
        /// <param name="obj">The other thickness to compare</param>
        /// <returns>True if the two instances are the same</returns>
        public override bool Equals(object obj)
        {
            return (null == obj) ? false : Equals((Thickness)obj);
        }

        #endregion

        #region Inflate()

        
        /// <summary>
        /// Increases the thickness of the edges by the specified amount (Top &amp; Bottom += h, Left &amp; Right += w)
        /// </summary>
        /// <param name="w">The value to increase the horizontal sizes by</param>
        /// <param name="h">The value to increase the vertical sizes by</param>
        public void Inflate(Unit w, Unit h)
        {
            this.Top += h;
            this.Bottom += h;
            this.Left += w;
            this.Right += w;
        }

        /// <summary>
        /// Increases the thickness of the edges by the specified amount (Top & Bottom & Left & Right += all)
        /// </summary>
        /// <param name="all">The thickness to increase all edges by</param>
        public void Inflate(Unit all)
        {
            this.Top += all;
            this.Bottom += all;
            this.Left += all;
            this.Right += all;
        }

        /// <summary>
        /// Increases the thickness of the edges by the individually specified amounts
        /// </summary>
        /// <param name="top">The value to increase the top edge by</param>
        /// <param name="left">The value to increase the left edge by</param>
        /// <param name="bottom">The value to increase the bottom edge by</param>
        /// <param name="right">The value to increase the right edge by</param>
        public void Inflate(Unit top, Unit right, Unit bottom, Unit left)
        {
            this.Top += top;
            this.Bottom += bottom;
            this.Left += left;
            this.Right += right;
        }

        #endregion

        #region Empty()

        /// <summary>
        /// Returns a new Empty PDFThickness
        /// </summary>
        /// <returns>The new thickness with 0.0 edges</returns>
        public static Thickness Empty()
        {
            return new Thickness();
        }

        #endregion

        

        #region operator cast double
        
        /// <summary>
        /// Returns a new thickness where all units are the value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Thickness(double value)
        {
            var unit = (Unit) value;
            return new Thickness(unit);
        }
        
        #endregion
        
        #region operator +, -

        public static Thickness operator +  (Thickness one, Thickness two)
        {
            return Add(one, two);
        }

        public static Thickness operator -(Thickness one, Thickness two)
        {
            return Subtract(one, two);
        }

        #endregion

        #region Add, Subtract

        /// <summary>
        /// Adds two thicknesses and returns the combined edges
        /// </summary>
        /// <param name="one">The first thickness</param>
        /// <param name="two">The second thickness</param>
        /// <returns>The resultant thickness</returns>
        public static Thickness Add(Thickness one, Thickness two)
        {
            return new Thickness(one.Top + two.Top,
                one.Right + two.Right, one.Bottom + two.Bottom, one.Left + two.Left);

        }

        /// <summary>
        /// Subtracts two thicknesses and returns the diferences in each of the edges
        /// </summary>
        /// <param name="one">The first thickness</param>
        /// <param name="two">The second thickness</param>
        /// <returns>The resultant thickness</returns>
        public static Thickness Subtract(Thickness one, Thickness two)
        {
            return new Thickness(one.Top - two.Top,
                one.Right - two.Right, one.Bottom - two.Bottom, one.Left - two.Left);
        }

        #endregion


        #region IComparable<PDFThickness> Members

        
        /// <summary>
        /// Compares this thickness to another thickness and returns true if they have the same values
        /// </summary>
        /// <param name="other">The other thickness</param>
        /// <returns>True if they are the same</returns>
        public int CompareTo(Thickness other)
        {
            var comp = this.Top.CompareTo(other.Top);
            if (comp != 0) return comp;

            comp = this.Bottom.CompareTo(other.Bottom);
            if (comp != 0) return comp;

            comp = this.Left.CompareTo(other.Left);
            if (comp != 0) return comp;

            return this.Right.CompareTo(other.Right);

        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        #region ICloneable<PDFThickness> Members
        /// <summary>
        /// Rturns a new Thickness with the same edge sizes
        /// </summary>
        /// <returns>The new thickness</returns>
        public Thickness Clone()
        {
            return new Thickness(this.Top, this.Right, this.Bottom, this.Left);
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Converts this instance into its string representation in the format [T L B R]
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(20);
            sb.Append(ThicknessStartChar);
            sb.Append(this.Top);
            sb.Append(ThicknessSeparatorChar);
            sb.Append(this.Right);
            sb.Append(ThicknessSeparatorChar);
            sb.Append(this.Bottom);
            sb.Append(ThicknessSeparatorChar);
            sb.Append(this.Left);
            sb.Append(ThicknessEndChar);
            return sb.ToString();
        }

        #endregion

        public static bool TryParse(string value, out Thickness parsed)
        {
            if (string.IsNullOrEmpty(value))
            {
                parsed = Thickness.Empty();
                return false;
            }
            bool success = false;
            try
            {
                parsed = Parse(value);
                success = true;
            }
            catch
            {
                parsed = Thickness.Empty();
                success = false;
            }
            return success;
        }

        #region static Parse(string)

        /// <summary>
        /// Parses a string value in the format [T L B R] or [All] into a PDFThickness instance
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <returns>A new PDFSize instance</returns>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentException" />
        public static Thickness Parse(string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentNullException("value", String.Format(Errors.CouldNotParseValue_3, value, "PDFThickness", "[T L B R], [TB RL] OR [All]"));

            if (value.StartsWith(ThicknessStartChar.ToString()) || value.EndsWith(ThicknessEndChar.ToString()))
                value = value.Substring(1, value.Length - 2);
            else if(ThicknessStartAndEndRequired)
                throw new ArgumentNullException("value", String.Format(Errors.CouldNotParseValue_3, value, "PDFThickness", "[T R B L], [TB RL] OR [All]"));

            Unit t, l, b, r;

            string[] thick = value.Split(ThicknessSeparatorChar);
            if (thick.Length == 1)
            {
                if (Unit.TryParse(thick[0], out t) == false)
                    throw new ArgumentException("value", String.Format(Errors.CouldNotParseValue_3, value, "PDFThickness", "[T R B L], [TB RL] OR [All]"));
                else
                {
                    l = b = r = t;
                }
            }
            else if(thick.Length == 2)
            {
                if (Unit.TryParse(thick[0], out t) == false ||
                    Unit.TryParse(thick[1], out r) == false)
                    throw new ArgumentException("value", String.Format(Errors.CouldNotParseValue_3, value, "PDFThickness", "[T R B L], [TB RL] OR [All]"));
                b = t;
                l = r;
            }
            else if (thick.Length != 4)
                throw new ArgumentException("value", String.Format(Errors.CouldNotParseValue_3, value, "PDFThickness", "[T R B L], [TB RL] OR [All]"));
            else
            {

                if (Unit.TryParse(thick[0], out t) == false ||
                    Unit.TryParse(thick[1], out r) == false ||
                    Unit.TryParse(thick[2], out b) == false ||
                    Unit.TryParse(thick[3], out l) == false)
                    throw new ArgumentException("value", String.Format(Errors.CouldNotParseValue_3, value, "PDFThickness", "[T R B L], [TB RL] OR [All]"));
            }
            return new Thickness(t, r, b, l);
        }



        #endregion

        
    }
}
