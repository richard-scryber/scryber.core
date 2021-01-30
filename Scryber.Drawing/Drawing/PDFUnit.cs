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
using Scryber.Native;
using System.CodeDom;

namespace Scryber.Drawing
{
    [PDFParsableValue()]
    public struct PDFUnit : IComparable, IComparable<PDFUnit>, IEquatable<PDFUnit>, IPDFSimpleExpressionValue
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
                return ConvertToUnits(_val, this.Units);
            }
        }

        public double PointsValue
        {
            get { return _val; }
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
        /// Gets the value of the unit in points (Native PDF Measurement) as a PDFReal value
        /// </summary>
        public PDFReal RealValue
        {
            get { return new Scryber.Native.PDFReal(this._val); }
        }

        
        public bool IsEmpty
        {
            get { return this._val == 0; }
        }

        public PDFUnit(double value, PageUnits units)
        {
            this._val = ConvertToPoints(value, units);
            this._units = units;
        }

        public PDFUnit(double pointvalue)
            : this(pointvalue, PageUnits.Points)
        {

        }

        public PDFUnit(int pointvalue)
            : this((double)pointvalue)
        {
        }

        


        private static double ConvertToPoints(double val, PageUnits currentUnits)
        {
            switch (currentUnits)
            {
                case PageUnits.Points:
                    return val;
                    
                case PageUnits.Millimeters:
                    return val * PointsPerMM;
                    
                case PageUnits.Inches:
                    return val * PointsPerInch;
                    
                default:
                    throw new ArgumentOutOfRangeException("currentUnits", String.Format(Errors.UnknownPageUnits, currentUnits));
            }
        }

        private static double ConvertToUnits(double ptVal, PageUnits toconvertto)
        {
            switch (toconvertto)
            {
                case PageUnits.Points:
                    return ptVal;
                    
                case PageUnits.Millimeters:
                    return ptVal * MMPerPoint;
                    
                case PageUnits.Inches:
                    return ptVal * InchesPerPoint;
                    
                default:
                    throw new ArgumentOutOfRangeException("toconvertto", String.Format(Errors.UnknownPageUnits, toconvertto));
            }
        }

        public PDFUnit ToPoints()
        {
            return new PDFUnit(this._val, PageUnits.Points);
        }

        public PDFUnit ToMillimeters()
        {
            if (this._units == PageUnits.Millimeters)
                return this;
            else
                return new PDFUnit(ConvertToUnits(this._val, PageUnits.Millimeters), PageUnits.Millimeters);
        }

        public PDFUnit ToInches()
        {
            if (this._units == PageUnits.Inches)
                return this;
            else
                return new PDFUnit(ConvertToUnits(this._val, PageUnits.Inches), PageUnits.Inches);
        }


        public override int GetHashCode()
        {
            return this._val.GetHashCode();
        }
        
        

        #region override ToString()

        public override string ToString()
        {
            return this.Value.ToString() + GetUnitString(this.Units);
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

                case PageUnits.Points:
                default:
                    s = PointPostFix;
                    break;
            }
            return s;
        }

        #endregion

        #region bool Equals(PDFUnit unit) + 1 overload

        public bool Equals(PDFUnit unit)
        {
            return DoubleApproximatelyEquals(this._val, unit._val);
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
            return this.Equals((PDFUnit)obj);
        }

        #endregion

        #region int CompareTo(PDFUnit unit) + 1 overload

        public int CompareTo(PDFUnit unit)
        {
            return this._val.CompareTo(unit._val);
        }

        public int CompareTo(object obj)
        {
            return this.CompareTo((PDFUnit)obj);
        }

        #endregion

        #region public static PDFUnit Empty {get;}

        private static PDFUnit _empty = new PDFUnit();
        public static PDFUnit Empty
        {
            get { return _empty; }
        }

        #endregion

        #region public static PDFUnit Zero {get;}

        private static PDFUnit _zero = new PDFUnit(0, PageUnits.Points);

        public static PDFUnit Zero
        {
            get { return _zero; }
        }

        #endregion

        #region public static int Compare(PDFUnit one, PDFUnit two)

        public static int Compare(PDFUnit one, PDFUnit two)
        {
            return one._val.CompareTo(two._val);
        }

        #endregion


        #region + operator

        public static PDFUnit operator +(PDFUnit left, PDFUnit right)
        {
            return new PDFUnit(left._val + right._val);
        }

        public static PDFUnit Add(PDFUnit left, PDFUnit right)
        {
            return left + right;
        }

        #endregion

        #region - operator

        public static PDFUnit operator -(PDFUnit left, PDFUnit right)
        {
            return new PDFUnit(left._val - right._val);
            
        }

        public static PDFUnit Subtract(PDFUnit left, PDFUnit right)
        {
            return left - right;
        }

        #endregion

        #region / operator

        public static PDFUnit Divide(PDFUnit left, double right)
        {
            return new PDFUnit(left.Value / right, left.Units);
        }

        public static PDFUnit Divide(PDFUnit left, int right)
        {
            return new PDFUnit(left.Value / (double)right, left._units);
        }

        public static PDFUnit operator /(PDFUnit left, int right)
        {
            double val = left.Value / (double)right;
            return new PDFUnit(val, left.Units);
        }

        public static PDFUnit operator /(PDFUnit left, double right)
        {
            double val = left.Value / right;
            return new PDFUnit(val, left.Units);
        }

        #endregion

        #region * operator

        public static PDFUnit operator *(PDFUnit left, double right)
        {
            return new PDFUnit(left.Value * right, left.Units);
        }

        public static PDFUnit Multiply(PDFUnit left, double right)
        {
            return new PDFUnit(left.Value * right, left.Units);
        }

        public static PDFUnit operator *(PDFUnit left, int right)
        {
            return new PDFUnit(left.Value * (double)right, left.Units);
        }

        public static PDFUnit Multiply(PDFUnit left, int right)
        {
            return new PDFUnit(left.Value * (double)right, left.Units);
        }

        #endregion

        #region ==, != operators

        public static bool operator ==(PDFUnit left, PDFUnit right)
        {
            return left._val == right._val;
        }

        public static bool Equals(PDFUnit left, PDFUnit right)
        {
            return left._val == right._val;
        }

        public static bool operator !=(PDFUnit left, PDFUnit right)
        {
            return left._val != right._val;
        }

        public static bool NotEquals(PDFUnit left, PDFUnit right)
        {
            return left._val != right._val;
        }

        #endregion

        #region >, < operators

        public static bool operator >(PDFUnit left, PDFUnit right)
        {
            return left._val > right._val;
        }

        public static bool operator <(PDFUnit left, PDFUnit right)
        {
            return left._val < right._val;
        }

        public static bool GreaterThan(PDFUnit left, PDFUnit right)
        {
            return left._val > right._val;
        }

        public static bool LessThan(PDFUnit left, PDFUnit right)
        {
            return left._val < right._val;
        }

        #endregion

        #region <=, >= operators

        public static bool operator <=(PDFUnit left, PDFUnit right)
        {
            return left._val <= right._val;
        }

        public static bool operator >=(PDFUnit left, PDFUnit right)
        {
            return left._val >= right._val;
        }

        public static bool GreaterThanEqual(PDFUnit left, PDFUnit right)
        {
            return left._val >= right._val;
        }

        public static bool LessThanEqual(PDFUnit left, PDFUnit right)
        {
            return left._val <= right._val;
        }

        #endregion

        #region cast - double to unit, int to unit, unit to double


        public static implicit operator PDFUnit(double value)
        {
            return new PDFUnit(value, PageUnits.Points);
        }

        public static implicit operator PDFUnit(int value)
        {
            return new PDFUnit(value, PageUnits.Points);
        }

        #endregion

        #region public static PDFUnit Convert(PDFUnit unit, PageUnits tounits)

        public static PDFUnit Convert(PDFUnit unit, PageUnits tounits)
        {
            PDFUnit newunit = new PDFUnit(unit._val);
            newunit._units = tounits;
            return newunit;
        }

        #endregion

        #region public static Min + Max

        public static PDFUnit Max(PDFUnit a, PDFUnit b)
        {
            if (a._val > b._val)
                return a;
            else
                return b;
        }

        public static PDFUnit Min(PDFUnit a, PDFUnit b)
        {
            if (a._val < b._val)
                return a;
            else
                return b;
        }

        #endregion

        #region public static Parse(string value)

        public static bool TryParse(string value, out PDFUnit unit)
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
                unit = PDFUnit.Empty;
            }
            return b;
        }

        public static PDFUnit Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                return PDFUnit.Empty;
            
            PageUnits unit;
            double val;
            int offset = 0;
            //if (IsRelativeValue(value, out val, out unit))
            //    return new PDFUnit(val, unit);

            val = GetNumber(ref offset, value);
            if (offset < value.Length)
                unit = GetUnit(ref offset, value, ref val);
            else
                unit = PageUnits.Points;

            return new PDFUnit(val, unit);
        }

        private static bool IsRelativeValue(string value, out double val, out PageUnits unit)
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

        private static PageUnits GetUnit(ref int offset, string value, ref double number)
        {
            string end = value.Substring(offset).Trim().ToLower();
            if (end == MillimeterPostFix)
                return PageUnits.Millimeters;
            else if (end == InchPostFix)
                return PageUnits.Inches;
            else if (end == ExplicitPointPostFix)
                return PageUnits.Points;
            else if (end == ExplicitPixelPostFix)
            {
                number = number * (72.0 / 96.0);
                return PageUnits.Points;
            }
            else
                throw new ArgumentException(String.Format(Errors.CouldNotParseValue_3, value, "PDFUnit", "nnn[.nnn](mm|in|pt)"), "value");

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
                throw new ArgumentException(String.Format(Errors.CouldNotParseValue_3, value, "PDFUnit", "nnn[.nnn](mm|in|pt)"), "value");

            offset = end;
            
            double d;
            if (double.TryParse(value.Substring(start, end - start), out d))
                return d;
            else
                throw new ArgumentException(String.Format(Errors.CouldNotParseValue_3, value, "PDFUnit", "nnn[.nnn](mm|in|pt)"), "value");

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
        public static PDFUnit Pt(double value)
        {
            return new PDFUnit(value, PageUnits.Points);
        }

        /// <summary>
        /// Creates a new PDFUnit with the specified value in Points
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PDFUnit Pt(int value)
        {
            return new PDFUnit(value, PageUnits.Points);
        }

        #endregion


        #region public static PDFUnit Inch(double value)

        /// <summary>
        /// Creates a new PDFUnit with the specified value in Inches
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PDFUnit Inch(double value)
        {
            return new PDFUnit(value, PageUnits.Inches);
        }

        /// <summary>
        /// Creates a new PDFUnit with the specified value in Inches
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PDFUnit Inch(int value)
        {
            return new PDFUnit(value, PageUnits.Inches);
        }

        #endregion

        #region public static PDFUnit Mm(double value)

        /// <summary>
        /// Creates a new PDFUnit with the specified value in Millimeters
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PDFUnit Mm(double value)
        {
            return new PDFUnit(value, PageUnits.Millimeters);
        }

        /// <summary>
        /// Creates a new PDFUnit with the specified value in Millimeters
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PDFUnit Mm(int value)
        {
            return new PDFUnit(value, PageUnits.Millimeters);
        }

        #endregion

        //
        // conversion constants
        //

        public const double PointsPerInch = 72;
        public const double PointsPerMM = PointsPerInch / 25.4;
        public const double InchesPerPoint = 1.0 / PointsPerInch;
        public const double MMPerPoint = 1.0 / PointsPerMM;

        public const string MillimeterPostFix = "mm";
        public const string InchPostFix = "in";
        public const string ExplicitPointPostFix = "pt";
        public const string ExplicitPixelPostFix = "px";
        public const string PointPostFix = "";



        #region IPDFSimpleCodeDomValue Members

        private static readonly System.Reflection.ConstructorInfo ctorRef = typeof(PDFUnit).GetConstructor(new Type[] { typeof(double), typeof(PageUnits) });

        public System.Linq.Expressions.Expression GetConstructorExpression()
        {
            System.Linq.Expressions.ConstantExpression value = System.Linq.Expressions.Expression.Constant(this.Value, typeof(double));
            System.Linq.Expressions.ConstantExpression units = System.Linq.Expressions.Expression.Constant(this.Units, typeof(PageUnits));
            System.Linq.Expressions.NewExpression ctor = System.Linq.Expressions.Expression.New(ctorRef, value, units);
            return ctor;
        }

        //public System.CodeDom.CodeExpression GetConstructorExpression()
        //{
        //    //new PDFUnit(PageUnits.Units,double);

        //    return new CodeObjectCreateExpression(typeof(PDFUnit),
        //        new CodePrimitiveExpression(this.Value), 
        //        new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(PageUnits)),this.Units.ToString())
        //        );

        //}

        #endregion
    }
}
