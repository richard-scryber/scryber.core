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
    public struct PDFNumber : IFileObject
    {

        public ObjectType Type { get { return ObjectTypes.Number; } }

        private long _value;

        /// <summary>
        /// Gets the long value of this number
        /// </summary>
        public long Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Constructs a new instance of the PDFNumber with the specified intial value
        /// </summary>
        /// <param name="value">The inital value of the PDFNumber</param>
        public PDFNumber(int value)
            : this((long)value)
        {

        }

        /// <summary>
        /// Constructs a new instance of the PDFNumber with the specified intial value
        /// </summary>
        /// <param name="value">The inital value of the PDFNumber</param>
        public PDFNumber(long value)
        {
            this._value = value;
        }

        /// <summary>
        /// Writes the value of the number to the current PDFWriter
        /// </summary>
        /// <param name="writer">The PDFWriter to write to</param>
        public void WriteData(PDFWriter writer)
        {
            writer.WriteNumberS(this.Value);
        }

        public static PDFNumber Zero
        {
            get { return new PDFNumber(0); }
        }

        public override bool Equals(object obj)
        {
            if (null == obj)
                return false;
            else if (obj is PDFNumber)
                return this.Equals((PDFNumber)obj);
            else
                return false;
        }

        public bool Equals(PDFNumber num)
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

        public static PDFNumber Parse(string value)
        {
            return (PDFNumber)long.Parse(value);
        }


        public static PDFNumber Parse(string value, int offset, out int end)
        {
            IFileObject obj = PDFParserHelper.ParseNumericValue(value, offset, out end);
            if (obj.Type == ObjectTypes.Number)
                return (PDFNumber)obj;
            else
                throw new PDFNativeParserException(CommonErrors.ParsedValueWasNotAnItegralNumber);

        }

        #region PDFNumber Operator Overloads

        public static explicit operator long(PDFNumber number)
        {
            return number.Value;
        }

        public static explicit operator int(PDFNumber number)
        {
            return (int)number.Value;
        }

        public static explicit operator PDFNumber(int num)
        {
            return new PDFNumber(num);
        }

        public static explicit operator PDFNumber(long num)
        {
            return new PDFNumber(num);
        }

        public static PDFNumber operator +(PDFNumber one, PDFNumber two)
        {
            return new PDFNumber(one.Value + two.Value);
        }

        public static PDFNumber operator -(PDFNumber one, PDFNumber two)
        {
            return new PDFNumber(one.Value - two.Value);
        }

        public static PDFNumber operator /(PDFNumber one, PDFNumber two)
        {
            return new PDFNumber(one.Value / two.Value);
        }

        public static PDFNumber operator *(PDFNumber one, PDFNumber two)
        {
            return new PDFNumber(one.Value * two.Value);
        }

        public static bool operator ==(PDFNumber one, PDFNumber two)
        {
            return one.Value == two.Value;
        }

        public static bool operator !=(PDFNumber one, PDFNumber two)
        {
            return one.Value != two.Value;
        }

        public static bool operator >(PDFNumber one, PDFNumber two)
        {
            return one.Value > two.Value;
        }

        public static bool operator <(PDFNumber one, PDFNumber two)
        {
            return one.Value < two.Value;
        }

        public static bool operator >=(PDFNumber one, PDFNumber two)
        {
            return one.Value >= two.Value;
        }

        public static bool operator <=(PDFNumber one, PDFNumber two)
        {
            return one.Value <= two.Value;
        }

        #endregion
    }
}
