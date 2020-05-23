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

namespace Scryber.Native
{
    /// <summary>
    /// A pdf representation of a boolean value.
    /// </summary>
    public struct PDFBoolean : IFileObject
    {
        public PDFObjectType Type { get { return PDFObjectTypes.Boolean; } }
        
        private bool _val;

        /// <summary>
        /// Gets or sets the boolean value.
        /// </summary>
        public bool Value
        {
            get { return _val; }
        }

        /// <summary>
        /// Creates a new instance with either true or false value set.
        /// </summary>
        /// <param name="val">The true or false value</param>
        public PDFBoolean(bool val)
        {
            this._val = val;
        }

        

        /// <summary>
        /// Writes the underlying data of the file object to the passed text writer
        /// </summary>
        /// <param name="tw">The text writer object to write data to</param>
        public void WriteData(PDFWriter writer)
        {
            writer.WriteBooleanS(this.Value);
        }


        public override string ToString()
        {
            return this.Value.ToString();
        }

        public static readonly string TrueString = bool.TrueString;

        public static readonly string FalseString = bool.FalseString;


        public bool Equals(PDFBoolean val)
        {
            return this.Value == val.Value;
        }

        public override bool Equals(object obj)
        {
            if (null == obj || !(obj is PDFBoolean))
                return false;
            else
                return this.Equals((PDFBoolean)obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static PDFBoolean Parse(string value)
        {
            int end;
            return Parse(value, 0, out end);

        }

        public static PDFBoolean Parse(string value, int offset, out int end)
        {
            return PDFParserHelper.ParseBoolean(value, offset, out end);
        }


        public static explicit operator PDFBoolean(bool value)
        {
            return new PDFBoolean(value);
        }

        public static explicit operator bool(PDFBoolean value)
        {
            return value.Value;
        }

        public static bool operator ==(PDFBoolean one, PDFBoolean two)
        {
            return one.Value == two.Value;
        }

        public static bool operator !=(PDFBoolean one, PDFBoolean two)
        {
            return one.Value != two.Value;
        }

        public static bool Equals(PDFBoolean one, PDFBoolean two)
        {
            return one.Value == two.Value;
        }
    }
}
