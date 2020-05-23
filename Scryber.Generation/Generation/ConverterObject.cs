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
using System.Reflection;
using System.Xml;

namespace Scryber.Generation
{
    /// <summary>
    /// Wrapper with a single public method that will return a delegate that can convert a 
    /// string value to an object value of the appropriate type.
    /// </summary>
    public static class ConvertObjects
    {
        private static readonly PDFValueConverter ConvertToInt32 = new PDFValueConverter(ToInt32);
        private static object ToInt32(string value, Type requiredType, IFormatProvider provider)
        {
            return int.Parse(value, provider);
        }

        private static readonly PDFValueConverter ConvertToInt16 = new PDFValueConverter(ToInt16);
        private static object ToInt16(string value, Type requiredType, IFormatProvider provider)
        {
            return short.Parse(value, provider);
        }

        private static readonly PDFValueConverter ConvertToInt64 = new PDFValueConverter(ToInt64);
        private static object ToInt64(string value, Type requiredType, IFormatProvider provider)
        {
            return long.Parse(value, provider);
        }

        private static readonly PDFValueConverter ConvertToUInt32 = new PDFValueConverter(ToUInt32);
        private static object ToUInt32(string value, Type requiredType, IFormatProvider provider)
        {
            return uint.Parse(value);
        }

        private static readonly PDFValueConverter ConvertToUInt16 = new PDFValueConverter(ToUInt16);
        private static object ToUInt16(string value, Type requiredType, IFormatProvider provider)
        {
            return ushort.Parse(value);
        }

        private static readonly PDFValueConverter ConvertToUInt64 = new PDFValueConverter(ToUInt64);
        private static object ToUInt64(string value, Type requiredType, IFormatProvider provider)
        {
            return ulong.Parse(value, provider);
        }

        private static readonly PDFValueConverter ConvertToFloat = new PDFValueConverter(ToFloat);
        private static object ToFloat(string value, Type requiredType, IFormatProvider provider)
        {
            return float.Parse(value, provider);
        }

        private static readonly PDFValueConverter ConvertToDouble = new PDFValueConverter(ToDouble);
        private static object ToDouble(string value, Type requiredType, IFormatProvider provider)
        {
            return double.Parse(value, provider);
        }

        private static readonly PDFValueConverter ConvertToDecimal = new PDFValueConverter(ToDecimal);
        private static object ToDecimal(string value, Type requiredType, IFormatProvider provider)
        {
            return decimal.Parse(value, provider);
        }

        private static readonly PDFValueConverter ConvertToString = new PDFValueConverter(ToString);
        private static object ToString(string value, Type requiredType, IFormatProvider provider)
        {
            return value;
        }

        private static readonly PDFValueConverter ConvertToDateTime = new PDFValueConverter(ToDateTime);
        private static object ToDateTime(string value, Type requiredType, IFormatProvider provider)
        {
            return DateTime.Parse(value, provider);
        }

        private static readonly PDFValueConverter ConvertToTimeSpan = new PDFValueConverter(ToTimeSpan);
        private static object ToTimeSpan(string value, Type requiredType, IFormatProvider provider)
        {
            return TimeSpan.Parse(value, provider);
        }

        private static readonly PDFValueConverter ConvertToEnum = new PDFValueConverter(ToEnum);
        private static object ToEnum(string value, Type requiredType, IFormatProvider provider)
        {
            if (value.IndexOf(' ') > -1)
                value = value.Replace(' ', ',');
            return Enum.Parse(requiredType, value);
        }

        private static readonly PDFValueConverter ConvertToByte = new PDFValueConverter(ToByte);
        private static object ToByte(string value, Type requiredType, IFormatProvider provider)
        {
            return byte.Parse(value, provider);
        }

        private static readonly PDFValueConverter ConvertToSByte = new PDFValueConverter(ToSByte);
        private static object ToSByte(string value, Type requiredType, IFormatProvider provider)
        {
            return sbyte.Parse(value, provider);
        }

        private static readonly PDFValueConverter ConvertToChar = new PDFValueConverter(ToChar);
        private static object ToChar(string value, Type requiredType, IFormatProvider provider)
        {
            return char.Parse(value);
        }

        private static readonly PDFValueConverter ConvertToGuid = new PDFValueConverter(ToGuid);
        private static object ToGuid(string value, Type requiredType, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(value))
                return Guid.Empty;
            else
                return new Guid(value);
        }

        private static readonly PDFValueConverter ConvertToBool = new PDFValueConverter(ToBool);
        private static object ToBool(string value, Type requiredType, IFormatProvider provider)
        {
            return bool.Parse(value);
        }

        private static readonly PDFValueConverter ConvertToDBNull = new PDFValueConverter(ToDBNull);
        private static object ToDBNull(string value, Type requiredType, IFormatProvider provider)
        {
            return DBNull.Value;
        }

        private static readonly PDFValueConverter ConvertToUri = new PDFValueConverter(ToUri);
        private static object ToUri(string value, Type requiredType, IFormatProvider provider)
        {
            return new Uri(value);
        }

        public static bool IsSimpleObjectType(Type type, out PDFValueConverter convert)
        {
            bool result = false;
            if (type.IsEnum)
            {
                convert = new PDFValueConverter(ConvertObjects.ToEnum);
                return true;
            }
            TypeCode code = Type.GetTypeCode(type);

            switch (code)
            {
                case TypeCode.Boolean:
                    convert = ConvertToBool;
                    result = true;
                    break;
                case TypeCode.Byte:
                    convert = ConvertToByte;
                    result = true;
                    break;
                case TypeCode.Char:
                    convert = ConvertToChar;
                    result = true;
                    break;
                case TypeCode.DBNull:
                    convert = ConvertToDBNull;
                    result = true;
                    break;
                case TypeCode.DateTime:
                    convert = ConvertToDateTime;
                    result = true;
                    break;
                case TypeCode.Decimal:
                    convert = ConvertToDecimal;
                    result = true;
                    break;
                case TypeCode.Double:
                    convert = ConvertToDouble;
                    result = true;
                    break;
                case TypeCode.Int16:
                    convert = ConvertToInt16;
                    result = true;
                    break;
                case TypeCode.Int32:
                    convert = ConvertToInt32;
                    result = true;
                    break;
                case TypeCode.Int64:
                    convert = ConvertToInt64;
                    result = true;
                    break;
                case TypeCode.SByte:
                    convert = ConvertToSByte;
                    result = true;
                    break;
                case TypeCode.Single:
                    convert = ConvertToFloat;
                    result = true;
                    break;
                case TypeCode.String:
                    convert = ConvertToString;
                    result = true;
                    break;
                case TypeCode.UInt16:
                    convert = ConvertToUInt16;
                    result = true;
                    break;
                case TypeCode.UInt32:
                    convert = ConvertToUInt32;
                    result = true;
                    break;
                case TypeCode.UInt64:
                    convert = ConvertToUInt64;
                    result = true;
                    break;
                case TypeCode.Object:
                    if (type == typeof(Guid))
                    {
                        convert = ConvertToGuid;
                        result = true;
                    }
                    else if (type == typeof(DateTime))
                    {
                        convert = ConvertToDateTime;
                        result = true;
                    }
                    else if (type == typeof(TimeSpan))
                    {
                        convert = ConvertToTimeSpan;
                        result = true;
                    }
                    else if (type == typeof(Uri))
                    {
                        convert = ConvertToUri;
                        result = true;
                    }
                    else
                    {
                        convert = null;
                        result = false;
                    }
                    break;
                case TypeCode.Empty:
                default:
                    convert = null;
                    result = false;
                    break;
            }
            return result;
        }

        internal static bool IsCustomParsableObjectType(Type type, out PDFValueConverter convert)
        {
            PDFParsableValueAttribute valattr = ParserDefintionFactory.GetCustomAttribute<PDFParsableValueAttribute>(type, true);
            if (null != valattr)
            {
                convert = ConvertObjects.GetParsableValueConverter(type);
                return null != convert;
            }
            else
            {
                convert = null;
                return false;
            }
        }

        /// <summary>
        /// Returns the PDFValueConverter method for a type.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static PDFValueConverter GetParsableValueConverter(Type t)
        {
            ConverterXml.ParseableConverter conv = ConverterXml.GetParserConverter(t);
            return conv.ValueConverter;
        }
    }
}
