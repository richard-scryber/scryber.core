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
    public class PDFString : IPDFFileObject, IEquatable<PDFString>
    {
        public ObjectType Type { get { return ObjectTypes.String; } }

        private string _value;
        /// <summary>
        /// Gets or sets the literal value for this string
        /// </summary>
        public string Value
        {
            get { return _value; }
            set 
            { 
                _value = value;
            }
        }

         /// <summary>
        /// Constructs a new PDFString whose value has not been set. Value will be null
        /// </summary>
        public PDFString()
            : this(null)
        {
        }

        /// <summary>
        /// Constructs a new PDFString with the specified initial value
        /// </summary>
        /// <param name="value">The initial value of the string</param>
        public PDFString(string value)
        {
            this._value = value;
        }

        
        public void WriteData(PDFWriter writer)
        {
            if (null == this._value)
                writer.WriteNullS();
            else
                writer.WriteStringLiteralS(this.Value);
        }

        /// <summary>
        /// Compares two PDFStrings for equality
        /// </summary>
        /// <param name="obj">The object to compare this string to</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            PDFString str = obj as PDFString;
            return this.Equals(str);
        }

        /// <summary>
        /// Gets the string HashCode for this items Value
        /// </summary>
        /// <returns>The strings hash code</returns>
        public override int GetHashCode()
        {
            if (this.Value != null)
                return this.Value.GetHashCode();
            else
                return String.Empty.GetHashCode();
        }

        public override string ToString()
        {
            return "(" + this.Value + ")";
        }

        #region IEquatable<PDFString> Members

        /// <summary>
        /// Returns true if the other PDFString has an equal value to this PDFString
        /// </summary>
        /// <param name="other">The other PDFString to compare</param>
        /// <returns>True if theor values are the same</returns>
        public bool Equals(PDFString other)
        {
            return this.Value == null ? other.Value == null : this.Value.Equals(other.Value);
        }

        #endregion

        public static PDFString Parse(string value)
        {
            int end = 0;
            return Parse(value, 0, out end);
        }

        public static PDFString Parse(string value, int offset, out int end)
        {
            return PDFParserHelper.ParseString(value, offset, out end);
        }

        public static explicit operator string(PDFString str)
        {
            return str.Value;
        }

        public static explicit operator PDFString(string str)
        {
            return new PDFString(str);
        }
    }
}
