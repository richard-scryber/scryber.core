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
using System.CodeDom;
using System.Globalization;

namespace Scryber.Drawing
{
    [PDFParsableValue()]
    public struct Unit : IComparable, IComparable<Unit>, IEquatable<Unit>
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
                if (this.IsRelative)
                    return _val;
                else
                    return ConvertToUnits(_val, this.Units);
            }
        }

        public double PointsValue
        {
            get
            {
                if(this.IsRelative)
                    throw new InvalidOperationException("The Unit must be absolute to be converted to another absolute dimension");
                return _val;
            }
        }

        


        private PageUnits _units;
        /// <summary>
        /// Gets the defined measurement units of the PDFUnit
        /// </summary>
        public PageUnits Units
        {
            get { return _units; }
        }

        /// <summary>
        /// Gets the value of the unit in points as a real value
        /// </summary>
        public PDFReal RealValue
        {
            get {
                if (this.IsRelative)
                    throw new InvalidOperationException("The Unit must be absolute to be converted to another absolute dimension");
                return new PDFReal(this._val);
            }
        }

        /// <summary>
        /// Returns true if this Unit has a value of zero
        /// </summary>
        public bool IsZero
        {
            get { return this._val == 0; }
        }


        public bool IsRelative
        {
            get
            {
                return IsRelativeUnit(this.Units);
            }
        }


        //
        // constructors
        //

        public Unit(double value, PageUnits units)
        {
            if (IsRelativeUnit(units))
            {
                this._val = value;
                this._units = units;
            }
            else
            {
                this._val = ConvertToPoints(value, units);
                this._units = units;
            }
        }

        public Unit(double pointvalue)
            : this(pointvalue, PageUnits.Points)
        {

        }

        public Unit(int pointvalue)
            : this((double)pointvalue)
        {
        }

        public Unit ToPoints()
        {
            if (this.IsRelative)
                throw new InvalidOperationException("The Unit must be absolute to be converted to another absolute dimension");

            return new Unit(this._val, PageUnits.Points);
        }

        public Unit ToMillimeters()
        {
            if (this.IsRelative)
                throw new InvalidOperationException("The Unit must be absolute to be converted to another absolute dimension");

            if (this._units == PageUnits.Millimeters)
                return this;
            else
                return new Unit(ConvertToUnits(this._val, PageUnits.Millimeters), PageUnits.Millimeters);
        }

        public Unit ToInches()
        {
            if (this.IsRelative)
                throw new InvalidOperationException("The Unit must be absolute to be converted to another absolute dimension");

            if (this._units == PageUnits.Inches)
                return this;
            else
                return new Unit(ConvertToUnits(this._val, PageUnits.Inches), PageUnits.Inches);
        }


        public Unit ToAbsolute(Unit referenceUnit)
        {
            if (referenceUnit.IsRelative)
                throw new InvalidOperationException("The reference Unit must be absolute to convert this to an absolute dimension");

            if (this.IsZero)
                return Unit.Zero;
            
            Unit result;

            switch (this._units)
            {
                case (PageUnits.Percent):
                case (PageUnits.ViewPortHeight):
                case (PageUnits.ViewPortWidth):
                case (PageUnits.ViewPortMax):
                case (PageUnits.ViewPortMin):
                    result = referenceUnit * (this._val / 100.0);
                    break;
                case (PageUnits.EMHeight):
                case (PageUnits.EXHeight):
                case (PageUnits.RootEMHeight):
                case (PageUnits.ZeroWidth):
                    result = referenceUnit * this._val;
                    break;
                default:
                    result = this;
                    break;
            }
            return result;
        }


        public override int GetHashCode()
        {
            return this._val.GetHashCode();
        }



        #region override ToString()

        public override string ToString()
        {
            return this.ToString(null, System.Globalization.CultureInfo.InvariantCulture);
        }

        public string ToString(IFormatProvider provider)
        {
            return this.ToString(null, provider);
        }

        public string ToString(string format)
        {
            return this.ToString(format, System.Globalization.CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return this.Value.ToString(format, provider) + GetUnitString(this.Units);
        }

        private static string GetUnitString(PageUnits pageUnits)
        {
            string s;
            switch (pageUnits)
            {
                case PageUnits.Millimeters:
                    s = MillimeterPostFix;
                    break;

                case PageUnits.Inches:
                    s = InchPostFix;
                    break;

                case PageUnits.Pixel:
                    s = ExplicitPixelPostFix;
                    break;

                case PageUnits.Percent:
                    s = RelativePercentPostfix;
                    break;

                case PageUnits.EMHeight:
                    s = RelativeEMHeightPostfix;
                    break;

                case PageUnits.EXHeight:
                    s = RelativeExHeightPostfix;
                    break;

                case PageUnits.ZeroWidth:
                    s = RelativeZeroWidthPostfix;
                    break;

                case PageUnits.RootEMHeight:
                    s = RelativeRootEMHeightPostfix;
                    break;

                case PageUnits.ViewPortWidth:
                    s = RelativeViewPortWidthPostfix;
                    break;

                case PageUnits.ViewPortHeight:
                    s = RelativeViewPortHeightPostfix;
                    break;

                case PageUnits.ViewPortMin:
                    s = RelativeViewPortMinPostfix;
                    break;

                case PageUnits.ViewPortMax:
                    s = RelativeViewPortMaxPostfix;
                    break;


                case PageUnits.Points:
                default:
                    s = PointPostFix;
                    break;
            }
            return s;
        }

        #endregion


        private static double ConvertToPoints(double val, PageUnits currentUnits)
        {
            switch (currentUnits)
            {
                case PageUnits.Points:
                    return val;
                    
                case PageUnits.Millimeters:
                    return Math.Round(val * PointsPerMM, DoublePrecisionLength);
                    
                case PageUnits.Inches:
                    return Math.Round(val * PointsPerInch, DoublePrecisionLength);

                case PageUnits.Pixel:
                    return Math.Round(val * PointsPerPixel, DoublePrecisionLength);

                default:
                    throw new InvalidOperationException("The Unit must be absolute to be converted to another absolute dimension");
            }
        }

        private static double ConvertToUnits(double ptVal, PageUnits toconvertto)
        {
            switch (toconvertto)
            {
                case PageUnits.Points:
                    return ptVal;
                    
                case PageUnits.Millimeters:
                    return Math.Round(ptVal * MMPerPoint, DoublePrecisionLength);
                    
                case PageUnits.Inches:
                    return Math.Round(ptVal * InchesPerPoint, DoublePrecisionLength);

                case PageUnits.Pixel:
                    return Math.Round(ptVal * PixelsPerPoint, DoublePrecisionLength);

                default:
                    throw new InvalidOperationException("The Unit must be absolute to be converted to another absolute dimension");
            }
        }

        

        #region bool Equals(PDFUnit unit) + 1 overload

        public bool Equals(Unit unit)
        {
            if (this.IsRelative)
            {
                if (!unit.IsRelative)
                    return false;
                else if (this.Units != unit.Units)
                    return false;
                else
                    return DoubleApproximatelyEquals(this._val, unit._val);
            }
            else if (unit.IsRelative)
            {
                return false;
            }
            else
                return DoubleApproximatelyEquals(this._val, unit._val);
        }


        

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
            return this.Equals((Unit)obj);
        }

        #endregion

        #region int CompareTo(PDFUnit unit) + 1 overload

        public int CompareTo(Unit unit)
        {
            if (this.IsRelative || unit.IsRelative)
            {
                if (this.Units != unit.Units)
                    throw new InvalidOperationException("Cannot compare different relative units");
            }

            return this._val.CompareTo(unit._val);
        }

        public int CompareTo(object obj)
        {
            return this.CompareTo((Unit)obj);
        }

        #endregion

        #region public static PDFUnit Empty {get;}

        private static Unit _empty = new Unit();
        public static Unit Empty
        {
            get { return _empty; }
        }

        #endregion

        #region public static PDFUnit Zero {get;}

        private static Unit _zero = new Unit(0, PageUnits.Points);

        public static Unit Zero
        {
            get { return _zero; }
        }

        #endregion

        #region public static int Compare(PDFUnit one, PDFUnit two)

        public static int Compare(Unit one, Unit two)
        {
            if (one.IsRelative || two.IsRelative)
            {
                if (one.Units != two.Units)
                    throw new InvalidOperationException("Cannot compare different relative units");
            }

            return one._val.CompareTo(two._val);
        }

        #endregion


        #region + operator

        public static Unit operator +(Unit left, Unit right)
        {
            if (left.IsRelative || right.IsRelative)
            {
                if (left.Units != right.Units)
                    throw new InvalidOperationException("Cannot calculate different relative units for " + left.ToString() + " + " + right.ToString());
            }

            var newUnit = new Unit(left._val + right._val);
            newUnit._units = left.Units;
            return newUnit;
        }

        public static Unit Add(Unit left, Unit right)
        {
            return left + right;
        }

        #endregion

        #region - operator

        public static Unit operator -(Unit left, Unit right)
        {
            if (left.IsRelative || right.IsRelative)
            {
                if (left.Units != right.Units)
                    throw new InvalidOperationException("Cannot calculate different relative units for " + left.ToString() + " - " + right.ToString());
            }

            var newUnit = new Unit(left._val - right._val);
            newUnit._units = left.Units;
            return newUnit;
        }

        public static Unit Subtract(Unit left, Unit right)
        {
            return left - right;
        }

        #endregion

        #region / operator

        public static Unit Divide(Unit left, double right)
        {
            return new Unit(left.Value / right, left.Units);
        }

        public static Unit Divide(Unit left, int right)
        {
            return new Unit(left.Value / (double)right, left._units);
        }

        public static Unit operator /(Unit left, int right)
        {
            double val = left.Value / (double)right;
            return new Unit(val, left.Units);
        }

        public static Unit operator /(Unit left, double right)
        {
            double val = left.Value / right;
            return new Unit(val, left.Units);
        }

        #endregion

        #region * operator

        public static Unit operator *(Unit left, double right)
        {
            return new Unit(left.Value * right, left.Units);
        }

        public static Unit Multiply(Unit left, double right)
        {
            return new Unit(left.Value * right, left.Units);
        }

        public static Unit operator *(Unit left, int right)
        {
            return new Unit(left.Value * (double)right, left.Units);
        }

        public static Unit Multiply(Unit left, int right)
        {
            return new Unit(left.Value * (double)right, left.Units);
        }

        #endregion

        #region ==, != operators

        public static bool operator ==(Unit left, Unit right)
        {
            if (left.IsRelative || right.IsRelative)
                return left.Equals(right);
            else
                return left._val == right._val;
        }

        public static bool Equals(Unit left, Unit right)
        {
            if (left.IsRelative || right.IsRelative)
                return left.Equals(right);
            else
                return left._val == right._val;
        }

        public static bool operator !=(Unit left, Unit right)
        {
            if (left.IsRelative || right.IsRelative)
                return !left.Equals(right);
            else
                return left._val != right._val;
        }

        public static bool NotEquals(Unit left, Unit right)
        {
            if (left.IsRelative || right.IsRelative)
                return !left.Equals(right);
            else
                return left._val != right._val;
        }

        #endregion

        #region >, < operators

        public static bool operator >(Unit left, Unit right)
        {
            if (left.IsRelative || right.IsRelative)
            {
                if (left.Units == right.Units)
                    return left._val > right._val;
                else
                    throw new InvalidOperationException("The Units must be absolute or the same, to be compared to another dimension");
            }
            else
                return left._val > right._val;
        }

        public static bool operator <(Unit left, Unit right)
        {
            if (left.IsRelative || right.IsRelative)
            {
                if (left.Units == right.Units)
                    return left._val < right._val;
                else
                    throw new InvalidOperationException("The Units must be absolute or the same, to be compared to another dimension");
            }
            else
                return left._val < right._val;
        }

        public static bool GreaterThan(Unit left, Unit right)
        {
            if (left.IsRelative || right.IsRelative)
            {
                if (left.Units == right.Units)
                    return left._val > right._val;
                else
                    throw new InvalidOperationException("The Units must be absolute or the same, to be compared to another dimension");
            }
            else
                return left._val > right._val;
        }

        public static bool LessThan(Unit left, Unit right)
        {
            if (left.IsRelative || right.IsRelative)
            {
                if (left.Units == right.Units)
                    return left._val < right._val;
                else
                    throw new InvalidOperationException("The Units must be absolute or the same, to be compared to another dimension");
            }
            else
                return left._val < right._val;
        }

        #endregion

        #region <=, >= operators

        public static bool operator <=(Unit left, Unit right)
        {
            if (left.IsRelative || right.IsRelative)
            {
                if (left.Units == right.Units)
                    return left._val <= right._val;
                else
                    throw new InvalidOperationException("The Units must be absolute or the same, to be compared to another dimension");
            }
            else
                return left._val <= right._val;
        }

        public static bool operator >=(Unit left, Unit right)
        {
            if (left.IsRelative || right.IsRelative)
            {
                if (left.Units == right.Units)
                    return left._val >= right._val;
                else
                    throw new InvalidOperationException("The Units must be absolute or the same, to be compared to another dimension");
            }
            else
                return left._val >= right._val;
        }

        public static bool GreaterThanEqual(Unit left, Unit right)
        {
            if (left.IsRelative || right.IsRelative)
            {
                if (left.Units == right.Units)
                    return left._val >= right._val;
                else
                    throw new InvalidOperationException("The Units must be absolute or the same, to be compared to another dimension");
            }
            else
                return left._val >= right._val;
        }

        public static bool LessThanEqual(Unit left, Unit right)
        {
            if (left.IsRelative || right.IsRelative)
            {
                if (left.Units == right.Units)
                    return left._val <= right._val;
                else
                    throw new InvalidOperationException("The Units must be absolute or the same, to be compared to another dimension");
            }
            else
                return left._val <= right._val;
        }

        #endregion

        #region cast - double to unit, int to unit, unit to double


        public static implicit operator Unit(double value)
        {
            return new Unit(value, PageUnits.Points);
        }

        public static implicit operator Unit(int value)
        {
            return new Unit(value, PageUnits.Points);
        }

        #endregion

        #region public static PDFUnit Convert(PDFUnit unit, PageUnits tounits)

        public static Unit Convert(Unit unit, PageUnits tounits)
        {
            if (unit.IsRelative)
            {
                if (unit.Units != tounits)
                    throw new InvalidOperationException("Cannot convert relative units. Use the ToAbsolute method to fix the size before conversion");
            }

            Unit newunit = new Unit(unit._val);
            newunit._units = tounits;
            return newunit;
        }

        #endregion

        #region public static Min + Max

        public static Unit Max(Unit a, Unit b)
        {
            if (a.IsRelative || b.IsRelative)
            {
                if (a.Units != b.Units)
                    throw new InvalidOperationException("Cannot compare different relative units");
            }

            if (a._val > b._val)
                return a;
            else
                return b;
        }

        public static Unit Min(Unit a, Unit b)
        {
            if (a.IsRelative || b.IsRelative)
            {
                if (a.Units != b.Units)
                    throw new InvalidOperationException("Cannot compare different relative units");
            }

            if (a._val < b._val)
                return a;
            else
                return b;
        }

        #endregion

        #region public static Parse(string value)

        public static bool TryParse(string value, out Unit unit)
        {
            bool b = true;
            try
            {
                unit = Parse(value);
            }
            catch (OutOfMemoryException) { throw; }
            catch (System.Threading.ThreadAbortException) { throw; }
            catch (StackOverflowException) { throw; }
            catch (Exception)
            {
                b = false;
                unit = Unit.Empty;
            }
            return b;
        }

        public static Unit Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Unit.Empty;
            
            PageUnits unit;
            double val;
            int offset = 0;
            //if (IsRelativeValue(value, out val, out unit))
            //    return new PDFUnit(val, unit);

            val = GetNumber(ref offset, value);
            if (offset < value.Length)
                unit = GetUnit(ref offset, value, val);
            else
                unit = PageUnits.Points;

            return new Unit(val, unit);
        }

        private static bool IsRelativeFontSizeValue(string value, out double val, out PageUnits unit)
        {
            bool parsed;
            switch (value)
            {
                case ("xx-small"):
                    val = 6;
                    unit = PageUnits.Points;
                    parsed = true;
                    break;
                case ("x-small"):
                    val = 7.5;
                    unit = PageUnits.Points;
                    parsed = true;
                    break;
                case ("small"):
                    val = 10;
                    unit = PageUnits.Points;
                    parsed = true;
                    break;
                case ("medium"):
                    val = 12;
                    unit = PageUnits.Points;
                    parsed = true;
                    break;
                case ("large"):
                    val = 14;
                    unit = PageUnits.Points;
                    parsed = true;
                    break;
                case ("x-large"):
                    val = 18;
                    unit = PageUnits.Points;
                    parsed = true;
                    break;
                case ("xx-large"):
                    val = 24;
                    unit = PageUnits.Points;
                    parsed = true;
                    break;
                case ("xxx-large"):
                    val = 32;
                    unit = PageUnits.Points;
                    parsed = true;
                    break;
                default:
                    val = 0;
                    unit = PageUnits.Points;
                    parsed = false;
                    break;
            }
            return parsed;
        }

        private static PageUnits GetUnit(ref int offset, string value, double number)
        {
            string end = value.Substring(offset).Trim().ToLower();
            switch (end)
            {
                case (MillimeterPostFix):
                    return PageUnits.Millimeters;

                case (InchPostFix):
                    return PageUnits.Inches;

                case (ExplicitPointPostFix):
                    return PageUnits.Points;

                case (ExplicitPixelPostFix):
                    return PageUnits.Pixel;

                case (RelativePercentPostfix):
                    return PageUnits.Percent;

                case (RelativeEMHeightPostfix):
                    return PageUnits.EMHeight;

                case (RelativeExHeightPostfix):
                    return PageUnits.EXHeight;

                case (RelativeZeroWidthPostfix):
                    return PageUnits.ZeroWidth;

                case (RelativeRootEMHeightPostfix):
                    return PageUnits.RootEMHeight;

                case (RelativeViewPortWidthPostfix):
                    return PageUnits.ViewPortWidth;

                case (RelativeViewPortHeightPostfix):
                    return PageUnits.ViewPortHeight;

                case (RelativeViewPortMinPostfix):
                    return PageUnits.ViewPortMin;

                case (RelativeViewPortMaxPostfix):
                    return PageUnits.ViewPortMax;
                default:
                    throw new ArgumentException(String.Format(Errors.CouldNotParseValue_3, value, "PDFUnit", "nnn[.nnn](mm|in|pt..)"), "value");

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
                if (value[end] == '%')
                    break;
                else if (char.IsDigit(value, end) || char.IsPunctuation(value,end))
                    end++;
                else
                    break;
            }
            while (end < value.Length);

            if (end == start)
                throw new ArgumentException(String.Format(Errors.CouldNotParseValue_3, value, "PDFUnit", "nnn[.nnn](mm|in|pt)"), "value");

            offset = end;

            var valueForParsing = value.Substring(start, end - start);

            if (double.TryParse(valueForParsing, NumberStyles.Any, CultureInfo.InvariantCulture, out var d2))
            {
                return d2;
            }
            if (double.TryParse(valueForParsing, out var d1))
            {
                return d1;
            }
            
            
            throw new ArgumentException(String.Format(Errors.CouldNotParseValue_3, value, "PDFUnit", "nnn[.nnn](mm|in|pt)"), "value");

        }

        #endregion

        #region IsRelativeUnit(Unit) + 1 overload

        /// <summary>
        /// Returns true if the unit has a PageUnit dimension that is defined as relative to other graphic content
        /// </summary>
        /// <param name="unit">The unit to check</param>
        /// <returns>True if the units dimension is relative</returns>
        public static bool IsRelativeUnit(Unit unit)
        {
            if (unit.Units >= RelativeStart)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if the PageUnit dimension is defined as relative to other graphic content
        /// </summary>
        /// <param name="unit">The PageUnit to check</param>
        /// <returns>True if the dimension is relative</returns>
        public static bool IsRelativeUnit(PageUnits units)
        {
            if (units >= RelativeStart)
                return true;
            else
                return false;
        }

        #endregion

        //
        // factory methods
        //

        #region public static PDFUnit Pt(double value)

        /// <summary>
        /// Creates a new PDFUnit with the specified value in Points
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Pt(double value)
        {
            return new Unit(value, PageUnits.Points);
        }

        /// <summary>
        /// Creates a new PDFUnit with the specified value in Points
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Pt(int value)
        {
            return new Unit(value, PageUnits.Points);
        }

        #endregion

        public static Unit Px(double value)
        {
            return new Unit(value, PageUnits.Pixel);
        }

        #region public static PDFUnit Inch(double value)

        /// <summary>
        /// Creates a new PDFUnit with the specified value in Inches
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Inch(double value)
        {
            return new Unit(value, PageUnits.Inches);
        }

        /// <summary>
        /// Creates a new PDFUnit with the specified value in Inches
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Inch(int value)
        {
            return new Unit(value, PageUnits.Inches);
        }

        #endregion

        #region public static PDFUnit Mm(double value)

        /// <summary>
        /// Creates a new PDFUnit with the specified value in Millimeters
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Mm(double value)
        {
            return new Unit(value, PageUnits.Millimeters);
        }

        /// <summary>
        /// Creates a new PDFUnit with the specified value in Millimeters
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Mm(int value)
        {
            return new Unit(value, PageUnits.Millimeters);
        }

        #endregion

        #region public static PDFUnit Percent(double value)

        /// <summary>
        /// Creates a new relative PDFUnit with the specified percentage value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Percent(double value)
        {
            return new Unit(value, PageUnits.Percent);
        }

        /// <summary>
        /// Creates a new relative PDFUnit with the specified percentage value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Percent(int value)
        {
            return new Unit(value, PageUnits.Percent);
        }

        #endregion

        #region public static PDFUnit Ex(double value)

        /// <summary>
        /// Creates a new relative PDFUnit with the specified lowercase ex height value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Ex(double value)
        {
            return new Unit(value, PageUnits.EXHeight);
        }

        /// <summary>
        /// Creates a new relative PDFUnit with the specified lowercase ex height value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Ex(int value)
        {
            return new Unit(value, PageUnits.EXHeight);
        }

        #endregion

        #region public static PDFUnit Em(double value)

        /// <summary>
        /// Creates a new relative PDFUnit with the specified uppercase em height value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Em(double value)
        {
            return new Unit(value, PageUnits.EMHeight);
        }

        /// <summary>
        /// Creates a new relative PDFUnit with the specified uppercase em height value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Em(int value)
        {
            return new Unit(value, PageUnits.EMHeight);
        }

        #endregion

        #region public static PDFUnit RootEm(double value)

        /// <summary>
        /// Creates a new relative PDFUnit with the specified uppercase em height value based on the default font size
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit RootEm(double value)
        {
            return new Unit(value, PageUnits.RootEMHeight);
        }

        /// <summary>
        /// Creates a new relative PDFUnit with the specified uppercase em height value based on the default font size
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit RootEm(int value)
        {
            return new Unit(value, PageUnits.RootEMHeight);
        }

        #endregion

        #region public static PDFUnit Ch(double value)

        /// <summary>
        /// Creates a new relative PDFUnit with the specified zero char width value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Ch(double value)
        {
            return new Unit(value, PageUnits.ZeroWidth);
        }

        /// <summary>
        /// Creates a new relative PDFUnit with the specified zero char value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Ch(int value)
        {
            return new Unit(value, PageUnits.ZeroWidth);
        }

        #endregion

        #region public static PDFUnit Vh(double value)

        /// <summary>
        /// Creates a new relative PDFUnit with the specified viewport height value (page height
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Vh(double value)
        {
            return new Unit(value, PageUnits.ViewPortHeight);
        }

        /// <summary>
        /// Creates a new relative PDFUnit with the specified viewport height value (page height)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Vh(int value)
        {
            return new Unit(value, PageUnits.ViewPortHeight);
        }

        #endregion

        #region public static PDFUnit Vh(double value)

        /// <summary>
        /// Creates a new relative PDFUnit with the specified viewport width value (page width)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Vw(double value)
        {
            return new Unit(value, PageUnits.ViewPortWidth);
        }

        /// <summary>
        /// Creates a new relative PDFUnit for the specified viewport width value (page width)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Vw(int value)
        {
            return new Unit(value, PageUnits.ViewPortWidth);
        }

        #endregion

        #region public static PDFUnit Vmin(double value)

        /// <summary>
        /// Creates a new relative PDFUnit with the specified smallest viewport value (page width or height)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Vmin(double value)
        {
            return new Unit(value, PageUnits.ViewPortMin);
        }

        /// <summary>
        /// Creates a new relative PDFUnit for the specified smallest viewport value (page width or height)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Vmin(int value)
        {
            return new Unit(value, PageUnits.ViewPortMin);
        }

        #endregion

        #region public static PDFUnit Vmax(double value)

        /// <summary>
        /// Creates a new relative PDFUnit with the specified largest viewport value (page width or height)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Vmax(double value)
        {
            return new Unit(value, PageUnits.ViewPortMax);
        }

        /// <summary>
        /// Creates a new relative PDFUnit for the specified largest viewport value (page width or height)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Unit Vmax(int value)
        {
            return new Unit(value, PageUnits.ViewPortMax);
        }

        #endregion

        //
        // conversion constants
        //

        public const double DoublePrecision = 0.000000001;
        public const int DoublePrecisionLength = 8;

        public const double PointsPerInch = 72;
        public const double PointsPerMM = PointsPerInch / 25.4;
        public const double InchesPerPoint = 1.0 / PointsPerInch;
        public const double MMPerPoint = 1.0 / PointsPerMM;
        public const double PointsPerPixel = 72.0 / 96.0;
        public const double PixelsPerPoint = 1.0 / PointsPerPixel;

        public const string MillimeterPostFix = "mm";
        public const string InchPostFix = "in";
        public const string ExplicitPointPostFix = "pt";
        public const string ExplicitPixelPostFix = "px";
        public const string PointPostFix = "pt";


        public const PageUnits RelativeStart = PageUnits.Percent;

        public const string RelativePercentPostfix = "%";
        public const string RelativeEMHeightPostfix = "em";
        public const string RelativeExHeightPostfix = "ex";
        public const string RelativeZeroWidthPostfix = "ch";
        public const string RelativeRootEMHeightPostfix = "rem";
        public const string RelativeViewPortWidthPostfix = "vw";
        public const string RelativeViewPortHeightPostfix = "vh";
        public const string RelativeViewPortMinPostfix = "vmin";
        public const string RelativeViewPortMaxPostfix = "vmax";



    }
}
