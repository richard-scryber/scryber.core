using System;

namespace Scryber.Expressive.Helpers
{
    internal static class TypeHelper
    {
        internal static TypeCode GetTypeCode(object value)
        {
            TypeCode typeCode = TypeCode.Object;

#if NETSTANDARD1_4
            // TODO: Explore converting all numbers to decimal and simplifying all of this.

            if (value is Boolean)
            {
                typeCode = TypeCode.Boolean;
            }
            else if (value is Byte)
            {
                typeCode = TypeCode.Byte;
            }
            else if (value is Char)
            {
                typeCode = TypeCode.Char;
            }
            else if (value is DateTime)
            {
                typeCode = TypeCode.DateTime;
            }
            else if (value is Decimal)
            {
                typeCode = TypeCode.Decimal;
            }
            else if (value is Double)
            {
                typeCode = TypeCode.Double;
            }
            else if (value is Int64)
            {
                typeCode = TypeCode.Int64;
            }
            else if (value is Int32)
            {
                typeCode = TypeCode.Int32;
            }
            else if (value is Int16)
            {
                typeCode = TypeCode.Int16;
            }
            else if (value is SByte)
            {
                typeCode = TypeCode.SByte;
            }
            else if (value is Single)
            {
                typeCode = TypeCode.Single;
            }
            else if (value is String)
            {
                typeCode = TypeCode.String;
            }
            else if (value is UInt16)
            {
                typeCode = TypeCode.UInt16;
            }
            else if (value is UInt32)
            {
                typeCode = TypeCode.UInt32;
            }
            else if (value is UInt64)
            {
                typeCode = TypeCode.UInt64;
            }

#else
            typeCode = Type.GetTypeCode(value.GetType());
#endif

            return typeCode;
        }
    }
}
