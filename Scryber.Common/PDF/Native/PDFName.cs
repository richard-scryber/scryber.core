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
    public class PDFName : IFileObject, IEquatable<PDFName>
    {

        public ObjectType Type { get { return ObjectTypes.Name; } }


        private string _name = String.Empty;

        /// <summary>
        /// Gets the name of the PDFName
        /// </summary>
        public string Value
        {
            get { return _name; }
        }

        /// <summary>
        /// Returns true if both objects are equal
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if both are non null and Equal</returns>
        public override bool Equals(object obj)
        {
            if (null == obj)
                return false;
            else if (obj is PDFName)
                return this.Equals((PDFName)obj);
            else
                return false; //not a PDFName
        }

        /// <summary>
        /// Compares two PDFNames and returns true if they are equal. Overriden to compare only their Name values
        /// </summary>
        /// <param name="other">The other name ot compare to</param>
        /// <returns>True if the names are equal</returns>
        public bool Equals(PDFName other)
        {
            if (null == other)
                return false;
            else
                return this.Value.Equals(other.Value, StringComparison.Ordinal);
        }


        /// <summary>
        /// Overriden to return the hashcode of the inner Name value
        /// </summary>
        /// <returns>The hash code of the Name value</returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }


        /// <summary>
        /// Constructs a new instance of a PDFName with the specified name value
        /// </summary>
        /// <param name="name">The name value string</param>
        public PDFName(string name)
        {
            ValidateName(name);
            this._name = name;
        }

        public override string ToString()
        {
            return "/" + this._name;
        }
        

        /// <summary>
        /// Writes the name data to the current PDFWriter. If the Name value has not been set or is null then PDFNull will be written to the writer
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        public void WriteData(PDFWriter writer)
        {
            writer.WriteNameS(this.Value);
        }

        private const char NameStartCharacter = '/';
        private const char HexStartChar = '#';

        private static char[] _invalidnamechars = new char[] {'[', ']', '%', '\\', '<', '>', '/', ' ' };
        public static char[] InvalidNameChars
        {
            get
            {
                return _invalidnamechars;
            }
        }

        public static void ValidateName(string namevalue)
        {
            if (string.IsNullOrEmpty(namevalue))
                throw new ArgumentNullException("namevalue");
            if (namevalue.IndexOfAny(InvalidNameChars) > -1)
                throw new ArgumentException(string.Format(CommonErrors.InvalidPDFName, namevalue), "namevalue");

        }

        public static PDFName Parse(string name)
        {
            int end;
            return Parse(name, 0, out end);
        }

        public static PDFName Parse(string value, int startoffset, out int end)
        {
            return PDFParserHelper.ParseName(value, startoffset, out end);
        }



        public static explicit operator string(PDFName name)
        {
            return name.Value;
        }

        public static explicit operator PDFName(string name)
        {
            return new PDFName(name);
        }
    }
}
