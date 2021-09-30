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
using System.Runtime.InteropServices;

namespace Scryber
{
    /// <summary>
    /// A 4 character object type that can be quickly compared but also has a 'human readable' form. Each PDFObject has a 
    /// PDFObjectType that identifies the type of PDF object it is (irresspective of the runtime type).
    /// </summary>
    /// <remarks>The object type is castable from a string value for ease of creation, and helps in unique id generation</remarks>
    [StructLayout(LayoutKind.Explicit)]
    public struct ObjectType : IEquatable<ObjectType>, IComparable, IComparable<ObjectType>
    {

        #region ivars

        [FieldOffset(0)]
        private byte _zero;

        [FieldOffset(1)]
        private byte _one;

        [FieldOffset(2)]
        private byte _two;

        [FieldOffset(3)]
        private byte _three;

        /// <summary>
        /// Offset 0 means that this object type will take on each of the 4 bytes for a unique integer
        /// </summary>
        [FieldOffset(0)]
        private int _value;

        #endregion

        //
        // ctor(s)
        //

        #region public PDFObjectType(string type)

        /// <summary>
        /// Create a new PDFObjectType with the specified string type (must be 4 ANSII string characters long)
        /// </summary>
        /// <param name="type"></param>
        public ObjectType(string type)
            : this(GetValidType(type))
        {
        }

        #endregion

        #region public PDFObjectType (char[] type)

        /// <summary>
        /// Creates a new PDFObjectType with the specified 4 length character array
        /// </summary>
        /// <param name="type"></param>
        public ObjectType(char[] type) : this(ConvertToBytes(type))
        {
        }

        #endregion

        #region private PDFObjectType(byte[] bytes)

        /// <summary>
        /// Creates a new PDFObjectType with the specified 4 byte characters.
        /// </summary>
        /// <param name="bytes"></param>
        private ObjectType(byte[] bytes)
        {
            this._value = 0;
            this._zero = bytes[0];
            this._one = bytes[1];
            this._two = bytes[2];
            this._three = bytes[3];
        }

        #endregion

        //
        // methods
        //

        #region public override string ToString()

        /// <summary>
        /// Converts this type to its string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return new string(new char[4] { (char)_zero, (char)_one, (char)_two, (char)_three });
        }

        #endregion

        #region public override bool Equals(object obj)

        /// <summary>
        /// Returns true if this instance equals (is equvalent to) the passed object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (null == obj || !(obj is ObjectType))
                return false;
            else
                return this.Equals((ObjectType)obj);
        }

        #endregion

        #region public bool Equals(PDFObjectType type)

        /// <summary>
        /// Returns true if this instance is equvalent to the passed object type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Equals(ObjectType type)
        {
            return this._value.Equals(type._value);
        }

        #endregion

        #region public override int GetHashCode()

        /// <summary>
        /// Gets the unique hash code that represents this object type
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _value;
        }

        #endregion

        #region int IComparable.CompareTo(object obj)

        /// <summary>
        /// Compares this instance with the specified object. If the object is not a PDFObjectType then returns -1
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int IComparable.CompareTo(object obj)
        {
            if (null == obj || !(obj is ObjectType))
                return -1;
            else
                return CompareTo((ObjectType)obj);
        }

        #endregion

        #region public int CompareTo(PDFObjectType other)

        /// <summary>
        /// compares this PDFObjectType with the specified object type, returning zero if they are the same
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ObjectType other)
        {
            return this._value.CompareTo(other._value);
        }

        #endregion

        //
        // static methods
        //

        #region private static char[] GetValidType(string s)

        /// <summary>
        /// Converts the string to a character array. Throws ArgumentException is the string is not 4 characters long
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static char[] GetValidType(string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s", String.Format(CommonErrors.TypeStringOnlyNChars, 4, ""));
            else if (s.Length != 4)
                throw new ArgumentOutOfRangeException("s", String.Format(CommonErrors.TypeStringOnlyNChars, 4, s));
            else
                return new char[] { s[0], s[1], s[2], s[3] };
        }

        #endregion

        #region public static byte[] ConvertToBytes(char[] chars)

        /// <summary>
        /// Converts the character array to a byte array. 
        /// Throws ArgumentException if the character array is not 4 characters in length
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static byte[] ConvertToBytes(char[] chars)
        {
            if (chars == null)
                throw new ArgumentNullException("chars", String.Format(CommonErrors.TypeStringOnlyNChars, 4, ""));
            else if (chars.Length != 4)
                throw new ArgumentOutOfRangeException("chars", String.Format(CommonErrors.TypeStringOnlyNChars, 4, new string(chars)));
            else
            {
                byte[] bytes = new byte[4];
                bytes[0] = (byte)chars[0];
                bytes[1] = (byte)chars[1];
                bytes[2] = (byte)chars[2];
                bytes[3] = (byte)chars[3];
                return bytes;
            }
        }

        #endregion

        #region public static PDFObjectType FromString(string s)

        /// <summary>
        /// Converts a string to a PDFObjectType - must be 4 characters in length
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ObjectType FromString(string s)
        {
            return new ObjectType(s);
        }

        #endregion

        #region public static bool Equals(PDFObjectType one, PDFObjectType two)

        /// <summary>
        /// Compares two object types and returns true if they are equvalient
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static bool Equals(ObjectType one, ObjectType two)
        {
            return one.Equals(two);
        }

        #endregion

        #region public static readonly PDFObjectType Empty

        /// <summary>
        /// An empty PDFObjectType
        /// </summary>
        public static readonly ObjectType Empty = new ObjectType();

        #endregion

        //
        // operator overloads
        //

        #region public static bool operator ==(PDFObjectType one, PDFObjectType two)

        /// <summary>
        /// True if the PDFObjectType(s) are equal
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static bool operator ==(ObjectType one, ObjectType two)
        {
            return one.Equals(two);
        }

        #endregion

        #region public static bool operator !=(PDFObjectType one, PDFObjectType two)

        /// <summary>
        /// True if the PDFObjectType(s) are not equal
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static bool operator !=(ObjectType one, ObjectType two)
        {
            return one.Equals(two) == false;
        }

        #endregion

        #region public static explicit operator PDFObjectType(string s)

        /// <summary>
        /// Explicitly converts the string to a PDFObjectType
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static explicit operator ObjectType(string s)
        {
            return new ObjectType(s);
        }

        #endregion

    }
}
