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
using System.Linq;
using System.Text;

namespace Scryber.Data
{
    internal static class TypeConverter
    {
        public static object GetNativeValue(System.Type type, string svalue)
        {
            object value;
            TypeCode tc = Type.GetTypeCode(type);
            switch (tc)
            {
                case TypeCode.Boolean:
                    value = bool.Parse(svalue);
                    break;
                case TypeCode.Byte:
                    value = byte.Parse(svalue);
                    break;
                case TypeCode.Char:
                    value = svalue[0];
                    break;
                case TypeCode.DBNull:
                    value = DBNull.Value;
                    break;
                case TypeCode.DateTime:
                    value = DateTime.Parse(svalue);
                    break;
                case TypeCode.Decimal:
                    value = Decimal.Parse(svalue);
                    break;
                case TypeCode.Double:
                    value = Double.Parse(svalue);
                    break;
                case TypeCode.Empty:
                    throw new NotSupportedException("Cannot parse an Empty object value");

                case TypeCode.Int16:
                    value = short.Parse(svalue);
                    break;
                case TypeCode.Int32:
                    value = int.Parse(svalue);
                    break;
                case TypeCode.Int64:
                    value = long.Parse(svalue);
                    break;
                case TypeCode.SByte:
                    value = sbyte.Parse(svalue);
                    break;
                case TypeCode.Single:
                    value = float.Parse(svalue);
                    break;
                case TypeCode.String:
                    value = svalue;
                    break;
                case TypeCode.UInt16:
                    value = ushort.Parse(svalue);
                    break;
                case TypeCode.UInt32:
                    value = uint.Parse(svalue);
                    break;
                case TypeCode.UInt64:
                    value = ulong.Parse(svalue);
                    break;
                case TypeCode.Object:
                default:
                    if (type == typeof(Guid))
                        value = new Guid(svalue);
                    else if (type == typeof(byte[]))
                        value = Convert.FromBase64String(svalue);
                    else if (type == typeof(TimeSpan))
                        value = TimeSpan.Parse(svalue);
                    else if (type.IsAssignableFrom(typeof(System.Xml.XmlNode)))
                    {
                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.LoadXml(svalue);
                        value = doc.DocumentElement;
                    }
                    else 
                        throw new ArgumentOutOfRangeException("type","Could not parse the value to a " + type.FullName);
                    break;
            }

            return value;
        }

        public static object GetNativeValue(System.Data.DbType type, string value)
        {
            object parsed;
            switch (type)
            {
                case System.Data.DbType.AnsiString:
                    parsed = value;
                    break;
                case System.Data.DbType.AnsiStringFixedLength:
                    parsed = value;
                    break;
                case System.Data.DbType.Binary:
                    parsed = System.Convert.FromBase64String(value);
                    break;
                case System.Data.DbType.Boolean:
                    parsed = bool.Parse(value);
                    break;
                case System.Data.DbType.Byte:
                    parsed = byte.Parse(value);
                    break;
                case System.Data.DbType.Currency:
                    parsed = int.Parse(value, System.Globalization.NumberStyles.Currency);
                    break;
                case System.Data.DbType.Date:
                case System.Data.DbType.DateTime:
                case System.Data.DbType.DateTime2:
                    parsed = DateTime.Parse(value);
                    break;
                case System.Data.DbType.DateTimeOffset:
                    parsed = TimeSpan.Parse(value);
                    break;
                case System.Data.DbType.Decimal:
                    parsed = Decimal.Parse(value);
                    break;
                case System.Data.DbType.Double:
                    parsed = Double.Parse(value);
                    break;
                case System.Data.DbType.Guid:
                    parsed = new Guid(value);
                    break;
                case System.Data.DbType.Int16:
                    parsed = Int16.Parse(value);
                    break;
                case System.Data.DbType.Int32:
                    parsed = Int32.Parse(value);
                    break;
                case System.Data.DbType.Int64:
                    parsed = Int64.Parse(value);
                    break;
                case System.Data.DbType.Object:
                    parsed = value;
                    break;
                case System.Data.DbType.SByte:
                    parsed = SByte.Parse(value);
                    break;
                case System.Data.DbType.Single:
                    parsed = Single.Parse(value);
                    break;
                case System.Data.DbType.String:
                    parsed = value;
                    break;
                case System.Data.DbType.StringFixedLength:
                    parsed = value;
                    break;
                case System.Data.DbType.Time:
                    parsed = DateTime.Parse(value);
                    break;
                case System.Data.DbType.UInt16:
                    parsed = UInt16.Parse(value);
                    break;
                case System.Data.DbType.UInt32:
                    parsed = UInt32.Parse(value);
                    break;
                case System.Data.DbType.UInt64:
                    parsed = UInt64.Parse(value);
                    break;
                case System.Data.DbType.VarNumeric:
                    parsed = double.Parse(value);
                    break;
                case System.Data.DbType.Xml:
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.LoadXml(value);
                    parsed = doc.DocumentElement;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            return parsed;
        }
    }
}
