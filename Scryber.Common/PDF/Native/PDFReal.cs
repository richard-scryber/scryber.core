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

namespace Scryber.PDF.Native
{
    [PDFParsableValue()]
    public struct PDFReal : IFileObject, IEquatable<PDFReal>
    {
        public ObjectType Type { get { return PDFObjectTypes.Real; } }

        private double _value;
        /// <summary>
        /// Gets or Sets the double value associated with this PDFReal instance.
        /// </summary>
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
            }
        }



        /// <summary>
        /// Constructs a new PDFReal instance whose value is set to the value parameter
        /// </summary>
        /// <param name="value">The value to set</param>
        public PDFReal(double value)
        {
            this._value = value;
        }

        public static PDFReal Zero
        {
            get { return new PDFReal(0.0); }
        }

        public void WriteData(PDFWriter writer)
        {
            writer.WriteRealS(this.Value);
        }


        public override bool Equals(object obj)
        {
            if (null == obj)
                return false;
            else if (obj is PDFReal)
                return this.Equals((PDFReal)obj);
            else
                return false;
        }

        public bool Equals(PDFReal num)
        {
            return this.Value.Equals(num.Value);
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static PDFReal Parse(string value)
        {
            int end;
            return Parse(value, 0, out end);
        }

        public static PDFReal Parse(string value, int offset, out int end)
        {
            IFileObject obj = PDFParserHelper.ParseNumericValue(value, offset, out end);

            if (obj.Type == PDFObjectTypes.Real)
                return (PDFReal)obj;
            else if (obj.Type == PDFObjectTypes.Number)
                return new PDFReal(((PDFNumber)obj).Value);
            else
                throw new PDFNativeParserException(CommonErrors.ParsedValueWasNotANumericValue);
        }

        #region PDFReal Operator Overloads

        public static explicit operator double(PDFReal number)
        {
            return number.Value;
        }

        public static explicit operator float(PDFReal number)
        {
            return (float)number.Value;
        }

        public static implicit operator PDFReal(float val)
        {
            return new PDFReal(val);
        }

        public static implicit operator PDFReal(double val)
        {
            return new PDFReal(val);
        }

        public static PDFReal operator +(PDFReal one, PDFReal two)
        {
            double oned = one.Value;
            double twod = two.Value;

            return new PDFReal(oned + twod);
        }

        public static PDFReal operator -(PDFReal one, PDFReal two)
        {
            return new PDFReal(one.Value - two.Value);
        }

        public static PDFReal operator /(PDFReal one, PDFReal two)
        {
            return new PDFReal(one.Value / two.Value);
        }

        public static PDFReal operator *(PDFReal one, PDFReal two)
        {
            return new PDFReal(one.Value * two.Value);
        }

        public static bool operator ==(PDFReal one, PDFReal two)
        {
            return one.Value == two.Value;
        }

        public static bool operator !=(PDFReal one, PDFReal two)
        {
            return one.Value != two.Value;
        }

        public static bool operator >(PDFReal one, PDFReal two)
        {
            return one.Value > two.Value;
        }

        public static bool operator <(PDFReal one, PDFReal two)
        {
            return one.Value < two.Value;
        }

        public static bool operator >=(PDFReal one, PDFReal two)
        {
            return one.Value >= two.Value;
        }

        public static bool operator <=(PDFReal one, PDFReal two)
        {
            return one.Value <= two.Value;
        }

        #endregion
    }
}
