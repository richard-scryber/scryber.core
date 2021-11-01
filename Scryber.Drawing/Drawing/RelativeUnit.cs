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
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;

namespace Scryber.Drawing
{
    /// <summary>
    /// A unit that supports relative dimensions - can be implicitly case from a string, double or an Absolute Unit
    /// </summary>
    [PDFParsableValue()]
    public struct RelativeUnit
    {
        /// <summary>
        /// Stores the actual value of the units in Points measurement
        /// </summary>
        private double _val;

        /// <summary>
        /// Gets the Value of the Unit in its PageUnit measurement
        /// </summary>
        public double Value
        {
            get 
            {
                return _val;
            }
        }

        /// <summary>
        /// Returns true if the units of this dimension are relative (vary based on the current context)
        /// </summary>
        public bool IsRelative
        {
            get { return this._units >= RelativeUnits.Pixel; }
        }
       

        private RelativeUnits _units;
        /// <summary>
        /// Gets the defined measurement units of the RelativeUnit
        /// </summary>
        public RelativeUnits Units
        {
            get { return _units; }
        }

        
        public bool IsEmpty
        {
            get { return this._val == 0; }
        }

        public RelativeUnit(double value, RelativeUnits units)
        {
            this._val = value;
            this._units = units;
        }

        public RelativeUnit(double pointvalue)
            : this(pointvalue, RelativeUnits.Points)
        {

        }

        public RelativeUnit(int pointvalue)
            : this((double)pointvalue)
        {
        }

        

        /// <summary>
        /// Converts this relative unit to an absolute unit with the specified absolute units for reference
        /// </summary>
        /// <param name="containerDimension">The width or height of the container, based on the relative unit required</param>
        /// <param name="pageSize">The full size of the view</param>
        /// <param name="fontMetrics">The mterics associated with the font to be used</param>
        /// <param name="defaultFontSize">The size of the default font</param>
        /// <returns>The converted unit</returns>
        public Unit ToAbsolute(Unit containerDimension, Size pageSize, FontMetrics fontMetrics, Unit defaultFontSize)
        {
            var val = _val;

            if (!this.IsRelative)
                return new Unit(val, (PageUnits)this.Units);
            else
            {
                val = ConvertToPoints(this._units, _val, containerDimension, pageSize, fontMetrics, defaultFontSize);

                return new Unit(val, PageUnits.Points);
            }
        }


        public override int GetHashCode()
        {
            return this._val.GetHashCode() * (int)_units;
        }
        
        

        #region override ToString()

        public override string ToString()
        {
            return this.Value.ToString() + GetUnitString(this.Units);
        }

        private static string GetUnitString(RelativeUnits pageUnits)
        {
            string s;
            switch (pageUnits)
            {
                case RelativeUnits.ViewPortMax:
                    s = ViewMaxPostfix;
                    break;

                case RelativeUnits.ViewPortMin:
                    s = ViewMinPostfix;
                    break;
                    
                case RelativeUnits.ViewPortHeight:
                    s = ViewHeightPostfix;
                    break;

                case RelativeUnits.ViewPortWidth:
                    s = ViewWidthPostfix;
                    break;

                case RelativeUnits.RootEMHeight:
                    s = RootEmPostfix;
                    break;

                case RelativeUnits.ZeroWidth:
                    s = ZeroWPostfix;
                    break;

                case RelativeUnits.EXHeight:
                    s = EXPostfix;
                    break;

                case RelativeUnits.EMHeight:
                    s = EMPostfix;
                    break;

                case RelativeUnits.Percent:
                    s = PercentPostFix;
                    break;

                case RelativeUnits.Millimeters:
                    s = MillimeterPostFix;
                    break;

                case RelativeUnits.Inches:
                    s = InchPostFix;
                    break;

                case RelativeUnits.Points:
                default:
                    s = PointPostFix;
                    break;

            }
            return s;
        }

        #endregion

        #region bool Equals(RelativeUnit unit) + 1 overload

        public bool Equals(RelativeUnit unit)
        {
            if (this._units == unit._units)
                return DoubleApproximatelyEquals(this._val, unit._val);
            else
                return false;
        }


        private const double DoublePrecision = 0.000000001;

        /// <summary>
        /// Compares 2 doubles for approximage equality.
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        private bool DoubleApproximatelyEquals(double one, double two)
        {
            return Math.Abs(one - two) < DoublePrecision;
        }

        public override bool Equals(object obj)
        {
            return this.Equals((RelativeUnit)obj);
        }

        #endregion

        #region int CompareTo(RelativeUnit unit) + 1 overload

        public int CompareTo(RelativeUnit unit)
        {
            int comp = this._val.CompareTo(unit._val);
            if (comp == 0)
                comp = this._units.CompareTo(unit._units);
            return comp;
        }

        public int CompareTo(object obj)
        {
            return this.CompareTo((RelativeUnit)obj);
        }

        #endregion

        #region public static RelativeUnit Empty {get;}

        private static RelativeUnit _empty = new RelativeUnit();

        public static RelativeUnit Empty
        {
            get { return _empty; }
        }

        #endregion

        #region public static RelativeUnit Zero {get;}

        private static RelativeUnit _zero = new RelativeUnit(0, RelativeUnits.Points);

        public static RelativeUnit Zero
        {
            get { return _zero; }
        }

        #endregion

        #region cast - Unit to Relative Unit, string to relative, double to relative

        public static implicit operator RelativeUnit(Unit value)
        {
            return new RelativeUnit(value.Value, (RelativeUnits)value.Units);
        }

        public static implicit operator RelativeUnit(double value)
        {
            return new RelativeUnit(value);
        }

        public static implicit operator RelativeUnit(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Equals("0"))
                return new RelativeUnit(0, RelativeUnits.Points);
            else
                return Parse(value);
        }

        #endregion

        public static double ConvertToPoints(RelativeUnits units, double value, Unit containerDimension, Size viewSize, FontMetrics metrics, Unit defaultEMSize)
        {
            switch (units)
            {
                case (RelativeUnits.Points):
                    return value;

                case (RelativeUnits.Millimeters):
                    return value * PointsPerMM;

                case (RelativeUnits.Inches):
                    return value * PointsPerInch;

                case (RelativeUnits.Pixel):
                    return value * PointsPerPixel;

                case (RelativeUnits.Percent):
                    return containerDimension.PointsValue * (value / 100.0);

                case (RelativeUnits.EMHeight):
                    return value * metrics.EmHeight;

                case (RelativeUnits.EXHeight):
                    return value * metrics.ExHeight;

                case (RelativeUnits.ZeroWidth):
                    return value * metrics.ZeroWidth;

                case (RelativeUnits.RootEMHeight):
                    return value * defaultEMSize.PointsValue;

                case (RelativeUnits.ViewPortWidth):
                    return value * viewSize.Width.PointsValue;

                case (RelativeUnits.ViewPortHeight):
                    return value * viewSize.Height.PointsValue;

                case (RelativeUnits.ViewPortMin):
                    return value * Math.Min(viewSize.Width.PointsValue, viewSize.Height.PointsValue);

                case (RelativeUnits.ViewPortMax):
                    return value * Math.Min(viewSize.Width.PointsValue, viewSize.Height.PointsValue);

                default:
                    throw new ArgumentOutOfRangeException(nameof(units));

            }
        }
        

        #region public static bool TryParse(string value, out unit)

        public static bool TryParse(string value, out RelativeUnit unit)
        {
            double val;
            RelativeUnits post;
            int offset = 0;
            if (IsKnownValue(value, out val, out post))
            {
                unit = new RelativeUnit(val, post);
                return true;
            }
            else if (TryGetNumber(ref offset, value, out val) && TryGetUnit(ref offset, value, out post))
            {
                unit = new RelativeUnit(val, post);
                return true;
            }
            else
            {
                unit = RelativeUnit.Zero;
                return false;
            }

        }

        #endregion

        #region public static Parse(string value)

        public static RelativeUnit Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                return RelativeUnit.Empty;

            RelativeUnit parsed;
            if (TryParse(value, out parsed))
                return parsed;
            else
                throw new ArgumentException(String.Format(Errors.CouldNotParseValue_3, value, "RelativeUnit", "nnn[.nnn](mm|in|pt)"), "value");
        }

        private static bool IsKnownValue(string value, out double val, out RelativeUnits unit)
        {
            bool parsed;
            switch (value)
            {
                case ("xx-small"):
                    val = 6;
                    unit = RelativeUnits.Points;
                    parsed = true;
                    break;
                case ("x-small"):
                    val = 7.5;
                    unit = RelativeUnits.Points;
                    parsed = true;
                    break;
                case ("small"):
                    val = 10;
                    unit = RelativeUnits.Points;
                    parsed = true;
                    break;
                case ("medium"):
                    val = 12;
                    unit = RelativeUnits.Points;
                    parsed = true;
                    break;
                case ("large"):
                    val = 14;
                    unit = RelativeUnits.Points;
                    parsed = true;
                    break;
                case ("x-large"):
                    val = 18;
                    unit = RelativeUnits.Points;
                    parsed = true;
                    break;
                case ("xx-large"):
                    val = 24;
                    unit = RelativeUnits.Points;
                    parsed = true;
                    break;
                case ("xxx-large"):
                    val = 32;
                    unit = RelativeUnits.Points;
                    parsed = true;
                    break;
                default:
                    val = 0;
                    unit = RelativeUnits.Points;
                    parsed = false;
                    break;
            }
            return parsed;
        }

        private static bool TryGetUnit(ref int offset, string value, out RelativeUnits units)
        {
            units = RelativeUnits.Points;

            string end = value.Substring(offset).Trim();
            switch (end)
            {
                case (MillimeterPostFix):
                    units = RelativeUnits.Millimeters;
                    return true;

                case (InchPostFix):

                    units = RelativeUnits.Inches;
                    return true;

                case (ExplicitPointPostFix):
                    units = RelativeUnits.Points;
                    return true;

                case (ExplicitPixelPostFix):
                    units = RelativeUnits.Pixel;
                    return true;

                case (PercentPostFix):
                    units = RelativeUnits.Percent;
                    return true;

                case (EMPostfix):
                    units = RelativeUnits.EMHeight;
                    return true;

                case (EXPostfix):
                    units = RelativeUnits.EXHeight;
                    return true;

                case (ZeroWPostfix):
                    units = RelativeUnits.ZeroWidth;
                    return true;

                case (RootEmPostfix):
                    units = RelativeUnits.RootEMHeight;
                    return true;

                case (ViewWidthPostfix):
                    units = RelativeUnits.ViewPortWidth;
                    return true;

                case (ViewHeightPostfix):
                    units = RelativeUnits.ViewPortHeight;
                    return true;

                case (ViewMaxPostfix):
                    units = RelativeUnits.ViewPortMax;
                    return true;

                case (ViewMinPostfix):
                    units = RelativeUnits.ViewPortMin;
                    return true;

                default:
                    return false;

            }
            

        }

        private static RelativeUnits GetUnit(ref int offset, string value)
        {
            RelativeUnits units;
            if (TryGetUnit(ref offset, value, out units))
                return units;
            else
                throw new ArgumentException(String.Format(Errors.CouldNotParseValue_3, value, "RelativeUnit", "nnn[.nnn](mm|in|pt)"), "value");
        }


        private static bool TryGetNumber(ref int offset, string value, out double found)
        {
            int start = offset;
            int end = start;
            do
            {
                if (end == start && value[end] == '-')
                    end++;
                if (char.IsDigit(value, end) || char.IsPunctuation(value, end))
                    end++;
                else
                    break;
            }
            while (end < value.Length);

            if (end == start)
            {
                found = 0.0;
                return false;
            }
            offset = end;

            double d;

            if (double.TryParse(value.Substring(start, end - start), out d))
            {
                found = d;
                return true;
            }
            else
            {
                found = 0;
                return false;
            }

        }

        private static double GetNumber(ref int offset, string value)
        {
            int start = offset;
            int end = start;
            do
            {
                if (end == start && value[end] == '-')
                    end++;
                if (char.IsDigit(value, end) || char.IsPunctuation(value,end))
                    end++;
                else
                    break;
            }
            while (end < value.Length);

            if (end == start)
                throw new ArgumentException(String.Format(Errors.CouldNotParseValue_3, value, "RelativeUnit", "nnn[.nnn](mm|in|pt)"), "value");

            offset = end;
            
            double d;
            if (double.TryParse(value.Substring(start, end - start), out d))
                return d;
            else
                throw new ArgumentException(String.Format(Errors.CouldNotParseValue_3, value, "RelativeUnit", "nnn[.nnn](mm|in|pt)"), "value");

        }

        #endregion


        //
        // factory methods
        //

        #region public static RelativeUnit Pt(double value)
        
        /// <summary>
        /// Creates a new RelativeUnit with the specified value in Points
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RelativeUnit Pt(double value)
        {
            return new RelativeUnit(value, RelativeUnits.Points);
        }

        /// <summary>
        /// Creates a new RelativeUnit with the specified value in Points
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RelativeUnit Pt(int value)
        {
            return new RelativeUnit(value, RelativeUnits.Points);
        }

        #endregion


        #region public static RelativeUnit Inch(double value)

        /// <summary>
        /// Creates a new RelativeUnit with the specified value in Inches
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RelativeUnit Inch(double value)
        {
            return new RelativeUnit(value, RelativeUnits.Inches);
        }

        /// <summary>
        /// Creates a new RelativeUnit with the specified value in Inches
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RelativeUnit Inch(int value)
        {
            return new RelativeUnit(value, RelativeUnits.Inches);
        }

        #endregion

        #region public static RelativeUnit Mm(double value)

        /// <summary>
        /// Creates a new RelativeUnit with the specified value in Millimeters
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RelativeUnit Mm(double value)
        {
            return new RelativeUnit(value, RelativeUnits.Millimeters);
        }

        /// <summary>
        /// Creates a new RelativeUnit with the specified value in Millimeters
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RelativeUnit Mm(int value)
        {
            return new RelativeUnit(value, RelativeUnits.Millimeters);
        }

        #endregion

        //
        // conversion constants
        //

        public const double PointsPerInch = 72;
        public const double PointsPerMM = PointsPerInch / 25.4;
        public const double InchesPerPoint = 1.0 / PointsPerInch;
        public const double MMPerPoint = 1.0 / PointsPerMM;
        public const double PointsPerPixel = PointsPerInch / 96.0;
        public const double PixelsPerPoint = 1.0 / PointsPerPixel;

        public const string MillimeterPostFix = "mm";
        public const string InchPostFix = "in";
        public const string ExplicitPointPostFix = "pt";
        public const string ExplicitPixelPostFix = "px";
        public const string PointPostFix = "pt";
        public const string PercentPostFix = "%";
        public const string EMPostfix = "em";
        public const string EXPostfix = "ex";
        public const string ZeroWPostfix = "ch";
        public const string RootEmPostfix = "rem";
        public const string ViewWidthPostfix = "vw";
        public const string ViewHeightPostfix = "vh";
        public const string ViewMinPostfix = "vmin";
        public const string ViewMaxPostfix = "vmax";


    }
}
