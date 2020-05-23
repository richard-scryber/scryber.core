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
    public sealed class PDFNull : IFileObject
    {
        public PDFObjectType Type { get { return PDFObjectTypes.Null; } }

        private PDFNull()
        {
        }

        private static PDFNull _null = new PDFNull();
        /// <summary>
        /// Gets the one and only instance of the PDFNull class
        /// </summary>
        public static PDFNull Value
        {
            get { return _null; }
        }

        /// <summary>
        /// Checks to see if the other object is also PDFNull.Value if so returns true.
        /// </summary>
        /// <param name="obj">The object to comapre to</param>
        /// <returns>True if both are PDFNull</returns>
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public const string NullString = "null";
        
        /// <summary>
        /// Returns a string representation of the PDFNull class
        /// </summary>
        /// <returns>The string</returns>
        public override string ToString()
        {
            return NullString;
        }

        /// <summary>
        /// Writes the null string to the writer
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        public void WriteData(PDFWriter writer)
        {
            writer.WriteNullS();
        }


        public static PDFNull Parse(string value)
        {
            int end;
            return Parse(value, 0, out end);
        }

        public static PDFNull Parse(string value, int offset, out int end)
        {
            return PDFParserHelper.ParseNull(value, offset, out end);
        }
    }
}
