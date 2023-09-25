﻿using System;
using Scryber.Drawing;

namespace Scryber.Expressive.Helpers
{
    //  Shout out to https://ncalc.codeplex.com/ for the bulk of this implementation.
    internal static class Numbers
    {
        private static object ConvertIfStringOrUnit(object s)
        {
            if(s is Unit unit)
            {
                return unit.PointsValue;
            }
            else if (s is String || s is char)
            {
                if (Decimal.TryParse(s.ToString(), out Decimal result))
                    return result;
                else if (Unit.TryParse(s.ToString(), out unit))
                    return unit.PointsValue;
                else
                    throw new InvalidCastException("Cannot convert " + s.ToString() + " to a decimal");

            }

            return s;
        }

        internal static object Add(object a, object b)
        {
            if (a is null || b is null)
            {
                return null;
            }

            if (a is Newtonsoft.Json.Linq.JToken token) {
                if(token.Type == Newtonsoft.Json.Linq.JTokenType.Integer
                    || token.Type == Newtonsoft.Json.Linq.JTokenType.Float
                    || token.Type == Newtonsoft.Json.Linq.JTokenType.String)
                {
                    a = (token as Newtonsoft.Json.Linq.JValue).Value;
                }
            }
#if NET6_0_OR_GREATER
            else if(a is System.Text.Json.JsonElement element)
            {
                if(element.ValueKind == System.Text.Json.JsonValueKind.Number)
                {
                    a = element.GetDouble();
                }
                else if(element.ValueKind == System.Text.Json.JsonValueKind.String)
                {
                    a = element.GetString();
                }
            }
#endif
            if (b is Newtonsoft.Json.Linq.JToken btoken)
            {
                if (btoken.Type == Newtonsoft.Json.Linq.JTokenType.Integer
                    || btoken.Type == Newtonsoft.Json.Linq.JTokenType.Float
                    || btoken.Type == Newtonsoft.Json.Linq.JTokenType.String)
                {
                    b = (btoken as Newtonsoft.Json.Linq.JValue).Value;
                }
            }
#if NET6_0_OR_GREATER
            else if (b is System.Text.Json.JsonElement belement)
            {
                if (belement.ValueKind == System.Text.Json.JsonValueKind.Number)
                {
                    b = belement.GetDouble();
                }
                else if (belement.ValueKind == System.Text.Json.JsonValueKind.String)
                {
                    b = belement.GetString();
                }
            }
#endif


            a = ConvertIfStringOrUnit(a);
            b = ConvertIfStringOrUnit(b);

            if (a is double && double.IsNaN((double)a))
            {
                return a;
            }
            if (b is double && double.IsNaN((double)b))
            {
                return b;
            }

            TypeCode typeCodeA = TypeHelper.GetTypeCode(a);
            TypeCode typeCodeB = TypeHelper.GetTypeCode(b);

            switch (typeCodeA)
            {
                case TypeCode.Boolean:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'bool'");
                        case TypeCode.Byte: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.SByte: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.Int16: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.UInt16: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.Int32: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.UInt32: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.Int64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.Single: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.Double: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.Decimal: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                    }
                    break;
                case TypeCode.Byte:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'byte' and 'bool'");
                        case TypeCode.Byte: return (Byte)a + (Byte)b;
                        case TypeCode.SByte: return (Byte)a + (SByte)b;
                        case TypeCode.Int16: return (Byte)a + (Int16)b;
                        case TypeCode.UInt16: return (Byte)a + (UInt16)b;
                        case TypeCode.Int32: return (Byte)a + (Int32)b;
                        case TypeCode.UInt32: return (Byte)a + (UInt32)b;
                        case TypeCode.Int64: return (Byte)a + (Int64)b;
                        case TypeCode.UInt64: return (Byte)a + (UInt64)b;
                        case TypeCode.Single: return (Byte)a + (Single)b;
                        case TypeCode.Double: return (Byte)a + (Double)b;
                        case TypeCode.Decimal: return (Byte)a + (Decimal)b;
                    }
                    break;
                case TypeCode.SByte:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'sbyte' and 'bool'");
                        case TypeCode.Byte: return (SByte)a + (Byte)b;
                        case TypeCode.SByte: return (SByte)a + (SByte)b;
                        case TypeCode.Int16: return (SByte)a + (Int16)b;
                        case TypeCode.UInt16: return (SByte)a + (UInt16)b;
                        case TypeCode.Int32: return (SByte)a + (Int32)b;
                        case TypeCode.UInt32: return (SByte)a + (UInt32)b;
                        case TypeCode.Int64: return (SByte)a + (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'sbyte' and 'ulong'");
                        case TypeCode.Single: return (SByte)a + (Single)b;
                        case TypeCode.Double: return (SByte)a + (Double)b;
                        case TypeCode.Decimal: return (SByte)a + (Decimal)b;
                    }
                    break;

                case TypeCode.Int16:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'short' and 'bool'");
                        case TypeCode.Byte: return (Int16)a + (Byte)b;
                        case TypeCode.SByte: return (Int16)a + (SByte)b;
                        case TypeCode.Int16: return (Int16)a + (Int16)b;
                        case TypeCode.UInt16: return (Int16)a + (UInt16)b;
                        case TypeCode.Int32: return (Int16)a + (Int32)b;
                        case TypeCode.UInt32: return (Int16)a + (UInt32)b;
                        case TypeCode.Int64: return (Int16)a + (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'short' and 'ulong'");
                        case TypeCode.Single: return (Int16)a + (Single)b;
                        case TypeCode.Double: return (Int16)a + (Double)b;
                        case TypeCode.Decimal: return (Int16)a + (Decimal)b;
                    }
                    break;

                case TypeCode.UInt16:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ushort' and 'bool'");
                        case TypeCode.Byte: return (UInt16)a + (Byte)b;
                        case TypeCode.SByte: return (UInt16)a + (SByte)b;
                        case TypeCode.Int16: return (UInt16)a + (Int16)b;
                        case TypeCode.UInt16: return (UInt16)a + (UInt16)b;
                        case TypeCode.Int32: return (UInt16)a + (Int32)b;
                        case TypeCode.UInt32: return (UInt16)a + (UInt32)b;
                        case TypeCode.Int64: return (UInt16)a + (Int64)b;
                        case TypeCode.UInt64: return (UInt16)a + (UInt64)b;
                        case TypeCode.Single: return (UInt16)a + (Single)b;
                        case TypeCode.Double: return (UInt16)a + (Double)b;
                        case TypeCode.Decimal: return (UInt16)a + (Decimal)b;
                    }
                    break;

                case TypeCode.Int32:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'int' and 'bool'");
                        case TypeCode.Byte: return (Int32)a + (Byte)b;
                        case TypeCode.SByte: return (Int32)a + (SByte)b;
                        case TypeCode.Int16: return (Int32)a + (Int16)b;
                        case TypeCode.UInt16: return (Int32)a + (UInt16)b;
                        case TypeCode.Int32: return (Int32)a + (Int32)b;
                        case TypeCode.UInt32: return (Int32)a + (UInt32)b;
                        case TypeCode.Int64: return (Int32)a + (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'int' and 'ulong'");
                        case TypeCode.Single: return (Int32)a + (Single)b;
                        case TypeCode.Double: return (Int32)a + (Double)b;
                        case TypeCode.Decimal: return (Int32)a + (Decimal)b;
                    }
                    break;

                case TypeCode.UInt32:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'unit' and 'bool'");
                        case TypeCode.Byte: return (UInt32)a + (Byte)b;
                        case TypeCode.SByte: return (UInt32)a + (SByte)b;
                        case TypeCode.Int16: return (UInt32)a + (Int16)b;
                        case TypeCode.UInt16: return (UInt32)a + (UInt16)b;
                        case TypeCode.Int32: return (UInt32)a + (Int32)b;
                        case TypeCode.UInt32: return (UInt32)a + (UInt32)b;
                        case TypeCode.Int64: return (UInt32)a + (Int64)b;
                        case TypeCode.UInt64: return (UInt32)a + (UInt64)b;
                        case TypeCode.Single: return (UInt32)a + (Single)b;
                        case TypeCode.Double: return (UInt32)a + (Double)b;
                        case TypeCode.Decimal: return (UInt32)a + (Decimal)b;
                    }
                    break;

                case TypeCode.Int64:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'long' and 'bool'");
                        case TypeCode.Byte: return (Int64)a + (Byte)b;
                        case TypeCode.SByte: return (Int64)a + (SByte)b;
                        case TypeCode.Int16: return (Int64)a + (Int16)b;
                        case TypeCode.UInt16: return (Int64)a + (UInt16)b;
                        case TypeCode.Int32: return (Int64)a + (Int32)b;
                        case TypeCode.UInt32: return (Int64)a + (UInt32)b;
                        case TypeCode.Int64: return (Int64)a + (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'long' and 'ulong'");
                        case TypeCode.Single: return (Int64)a + (Single)b;
                        case TypeCode.Double: return (Int64)a + (Double)b;
                        case TypeCode.Decimal: return (Int64)a + (Decimal)b;
                    }
                    break;

                case TypeCode.UInt64:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'bool'");
                        case TypeCode.Byte: return (UInt64)a + (Byte)b;
                        case TypeCode.SByte: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'sbyte'");
                        case TypeCode.Int16: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'short'");
                        case TypeCode.UInt16: return (UInt64)a + (UInt16)b;
                        case TypeCode.Int32: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'int'");
                        case TypeCode.UInt32: return (UInt64)a + (UInt32)b;
                        case TypeCode.Int64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'long'");
                        case TypeCode.UInt64: return (UInt64)a + (UInt64)b;
                        case TypeCode.Single: return (UInt64)a + (Single)b;
                        case TypeCode.Double: return (UInt64)a + (Double)b;
                        case TypeCode.Decimal: return (UInt64)a + (Decimal)b;
                    }
                    break;

                case TypeCode.Single:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'float' and 'bool'");
                        case TypeCode.Byte: return (Single)a + (Byte)b;
                        case TypeCode.SByte: return (Single)a + (SByte)b;
                        case TypeCode.Int16: return (Single)a + (Int16)b;
                        case TypeCode.UInt16: return (Single)a + (UInt16)b;
                        case TypeCode.Int32: return (Single)a + (Int32)b;
                        case TypeCode.UInt32: return (Single)a + (UInt32)b;
                        case TypeCode.Int64: return (Single)a + (Int64)b;
                        case TypeCode.UInt64: return (Single)a + (UInt64)b;
                        case TypeCode.Single: return (Single)a + (Single)b;
                        case TypeCode.Double: return (Single)a + (Double)b;
                        case TypeCode.Decimal: return Convert.ToDecimal(a) + (Decimal)b;
                    }
                    break;

                case TypeCode.Double:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'double' and 'bool'");
                        case TypeCode.Byte: return (Double)a + (Byte)b;
                        case TypeCode.SByte: return (Double)a + (SByte)b;
                        case TypeCode.Int16: return (Double)a + (Int16)b;
                        case TypeCode.UInt16: return (Double)a + (UInt16)b;
                        case TypeCode.Int32: return (Double)a + (Int32)b;
                        case TypeCode.UInt32: return (Double)a + (UInt32)b;
                        case TypeCode.Int64: return (Double)a + (Int64)b;
                        case TypeCode.UInt64: return (Double)a + (UInt64)b;
                        case TypeCode.Single: return (Double)a + (Single)b;
                        case TypeCode.Double: return (Double)a + (Double)b;
                        case TypeCode.Decimal: return Convert.ToDecimal(a) + (Decimal)b;
                    }
                    break;

                case TypeCode.Decimal:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'decimal' and 'bool'");
                        case TypeCode.Byte: return (Decimal)a + (Byte)b;
                        case TypeCode.SByte: return (Decimal)a + (SByte)b;
                        case TypeCode.Int16: return (Decimal)a + (Int16)b;
                        case TypeCode.UInt16: return (Decimal)a + (UInt16)b;
                        case TypeCode.Int32: return (Decimal)a + (Int32)b;
                        case TypeCode.UInt32: return (Decimal)a + (UInt32)b;
                        case TypeCode.Int64: return (Decimal)a + (Int64)b;
                        case TypeCode.UInt64: return (Decimal)a + (UInt64)b;
                        case TypeCode.Single: return (Decimal)a + Convert.ToDecimal(b);
                        case TypeCode.Double: return (Decimal)a + Convert.ToDecimal(b);
                        case TypeCode.Decimal: return (Decimal)a + (Decimal)b;
                    }
                    break;
            }

            return null;
        }

        internal static object Divide(object a, object b)
        {
            if (a is null || b is null)
            {
                return null;
            }

            a = ConvertIfStringOrUnit(a);
            b = ConvertIfStringOrUnit(b);

            if (a is double && double.IsNaN((double)a))
            {
                return a;
            }
            if (b is double && double.IsNaN((double)b))
            {
                return b;
            }

            TypeCode typeCodeA = TypeHelper.GetTypeCode(a);
            TypeCode typeCodeB = TypeHelper.GetTypeCode(b);

            switch (typeCodeA)
            {
                case TypeCode.Byte:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'byte' and 'bool'");
                        case TypeCode.SByte: return (Byte)a / (SByte)b;
                        case TypeCode.Int16: return (Byte)a / (Int16)b;
                        case TypeCode.UInt16: return (Byte)a / (UInt16)b;
                        case TypeCode.Int32: return (Byte)a / (Int32)b;
                        case TypeCode.UInt32: return (Byte)a / (UInt32)b;
                        case TypeCode.Int64: return (Byte)a / (Int64)b;
                        case TypeCode.UInt64: return (Byte)a / (UInt64)b;
                        case TypeCode.Single: return (Byte)a / (Single)b;
                        case TypeCode.Double: return (Byte)a / (Double)b;
                        case TypeCode.Decimal: return (Byte)a / (Decimal)b;
                    }
                    break;
                case TypeCode.SByte:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'sbyte' and 'bool'");
                        case TypeCode.SByte: return (SByte)a / (SByte)b;
                        case TypeCode.Int16: return (SByte)a / (Int16)b;
                        case TypeCode.UInt16: return (SByte)a / (UInt16)b;
                        case TypeCode.Int32: return (SByte)a / (Int32)b;
                        case TypeCode.UInt32: return (SByte)a / (UInt32)b;
                        case TypeCode.Int64: return (SByte)a / (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'sbyte' and 'ulong'");
                        case TypeCode.Single: return (SByte)a / (Single)b;
                        case TypeCode.Double: return (SByte)a / (Double)b;
                        case TypeCode.Decimal: return (SByte)a / (Decimal)b;
                    }
                    break;

                case TypeCode.Int16:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'short' and 'bool'");
                        case TypeCode.SByte: return (Int16)a / (SByte)b;
                        case TypeCode.Int16: return (Int16)a / (Int16)b;
                        case TypeCode.UInt16: return (Int16)a / (UInt16)b;
                        case TypeCode.Int32: return (Int16)a / (Int32)b;
                        case TypeCode.UInt32: return (Int16)a / (UInt32)b;
                        case TypeCode.Int64: return (Int16)a / (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'short' and 'ulong'");
                        case TypeCode.Single: return (Int16)a / (Single)b;
                        case TypeCode.Double: return (Int16)a / (Double)b;
                        case TypeCode.Decimal: return (Int16)a / (Decimal)b;
                    }
                    break;

                case TypeCode.UInt16:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ushort' and 'bool'");
                        case TypeCode.SByte: return (UInt16)a / (SByte)b;
                        case TypeCode.Int16: return (UInt16)a / (Int16)b;
                        case TypeCode.UInt16: return (UInt16)a / (UInt16)b;
                        case TypeCode.Int32: return (UInt16)a / (Int32)b;
                        case TypeCode.UInt32: return (UInt16)a / (UInt32)b;
                        case TypeCode.Int64: return (UInt16)a / (Int64)b;
                        case TypeCode.UInt64: return (UInt16)a / (UInt64)b;
                        case TypeCode.Single: return (UInt16)a / (Single)b;
                        case TypeCode.Double: return (UInt16)a / (Double)b;
                        case TypeCode.Decimal: return (UInt16)a / (Decimal)b;
                    }
                    break;

                case TypeCode.Int32:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'int' and 'bool'");
                        case TypeCode.SByte: return (Int32)a / (SByte)b;
                        case TypeCode.Int16: return (Int32)a / (Int16)b;
                        case TypeCode.UInt16: return (Int32)a / (UInt16)b;
                        case TypeCode.Int32: return (Int32)a / (Int32)b;
                        case TypeCode.UInt32: return (Int32)a / (UInt32)b;
                        case TypeCode.Int64: return (Int32)a / (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'int' and 'ulong'");
                        case TypeCode.Single: return (Int32)a / (Single)b;
                        case TypeCode.Double: return (Int32)a / (Double)b;
                        case TypeCode.Decimal: return (Int32)a / (Decimal)b;
                    }
                    break;

                case TypeCode.UInt32:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'uint' and 'bool'");
                        case TypeCode.SByte: return (UInt32)a / (SByte)b;
                        case TypeCode.Int16: return (UInt32)a / (Int16)b;
                        case TypeCode.UInt16: return (UInt32)a / (UInt16)b;
                        case TypeCode.Int32: return (UInt32)a / (Int32)b;
                        case TypeCode.UInt32: return (UInt32)a / (UInt32)b;
                        case TypeCode.Int64: return (UInt32)a / (Int64)b;
                        case TypeCode.UInt64: return (UInt32)a / (UInt64)b;
                        case TypeCode.Single: return (UInt32)a / (Single)b;
                        case TypeCode.Double: return (UInt32)a / (Double)b;
                        case TypeCode.Decimal: return (UInt32)a / (Decimal)b;
                    }
                    break;

                case TypeCode.Int64:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'long' and 'bool'");
                        case TypeCode.SByte: return (Int64)a / (SByte)b;
                        case TypeCode.Int16: return (Int64)a / (Int16)b;
                        case TypeCode.UInt16: return (Int64)a / (UInt16)b;
                        case TypeCode.Int32: return (Int64)a / (Int32)b;
                        case TypeCode.UInt32: return (Int64)a / (UInt32)b;
                        case TypeCode.Int64: return (Int64)a / (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'long' and 'ulong'");
                        case TypeCode.Single: return (Int64)a / (Single)b;
                        case TypeCode.Double: return (Int64)a / (Double)b;
                        case TypeCode.Decimal: return (Int64)a / (Decimal)b;
                    }
                    break;

                case TypeCode.UInt64:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'bool'");
                        case TypeCode.SByte: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'sbyte'");
                        case TypeCode.Int16: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'short'");
                        case TypeCode.UInt16: return (UInt64)a / (UInt16)b;
                        case TypeCode.Int32: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'int'");
                        case TypeCode.UInt32: return (UInt64)a / (UInt32)b;
                        case TypeCode.Int64: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'long'");
                        case TypeCode.UInt64: return (UInt64)a / (UInt64)b;
                        case TypeCode.Single: return (UInt64)a / (Single)b;
                        case TypeCode.Double: return (UInt64)a / (Double)b;
                        case TypeCode.Decimal: return (UInt64)a / (Decimal)b;
                    }
                    break;

                case TypeCode.Single:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'float' and 'bool'");
                        case TypeCode.SByte: return (Single)a / (SByte)b;
                        case TypeCode.Int16: return (Single)a / (Int16)b;
                        case TypeCode.UInt16: return (Single)a / (UInt16)b;
                        case TypeCode.Int32: return (Single)a / (Int32)b;
                        case TypeCode.UInt32: return (Single)a / (UInt32)b;
                        case TypeCode.Int64: return (Single)a / (Int64)b;
                        case TypeCode.UInt64: return (Single)a / (UInt64)b;
                        case TypeCode.Single: return (Single)a / (Single)b;
                        case TypeCode.Double: return (Single)a / (Double)b;
                        case TypeCode.Decimal: return Convert.ToDecimal(a) / (Decimal)b;
                    }
                    break;

                case TypeCode.Double:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'double' and 'bool'");
                        case TypeCode.SByte: return (Double)a / (SByte)b;
                        case TypeCode.Int16: return (Double)a / (Int16)b;
                        case TypeCode.UInt16: return (Double)a / (UInt16)b;
                        case TypeCode.Int32: return (Double)a / (Int32)b;
                        case TypeCode.UInt32: return (Double)a / (UInt32)b;
                        case TypeCode.Int64: return (Double)a / (Int64)b;
                        case TypeCode.UInt64: return (Double)a / (UInt64)b;
                        case TypeCode.Single: return (Double)a / (Single)b;
                        case TypeCode.Double: return (Double)a / (Double)b;
                        case TypeCode.Decimal: return Convert.ToDecimal(a) / (Decimal)b;
                    }
                    break;

                case TypeCode.Decimal:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'decimal' and 'bool'");
                        case TypeCode.SByte: return (Decimal)a / (SByte)b;
                        case TypeCode.Int16: return (Decimal)a / (Int16)b;
                        case TypeCode.UInt16: return (Decimal)a / (UInt16)b;
                        case TypeCode.Int32: return (Decimal)a / (Int32)b;
                        case TypeCode.UInt32: return (Decimal)a / (UInt32)b;
                        case TypeCode.Int64: return (Decimal)a / (Int64)b;
                        case TypeCode.UInt64: return (Decimal)a / (UInt64)b;
                        case TypeCode.Single: return (Decimal)a / Convert.ToDecimal(b);
                        case TypeCode.Double: return (Decimal)a / Convert.ToDecimal(b);
                        case TypeCode.Decimal: return (Decimal)a / (Decimal)b;
                    }
                    break;
            }

            return null;
        }

        internal static object Multiply(object a, object b)
        {
            if (a is null || b is null)
            {
                return null;
            }

            a = ConvertIfStringOrUnit(a);
            b = ConvertIfStringOrUnit(b);

            if (a is double && double.IsNaN((double)a))
            {
                return a;
            }
            if (b is double && double.IsNaN((double)b))
            {
                return b;
            }

            TypeCode typeCodeA = TypeHelper.GetTypeCode(a);
            TypeCode typeCodeB = TypeHelper.GetTypeCode(b);

            switch (typeCodeA)
            {
                case TypeCode.Byte:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'byte' and 'bool'");
                        case TypeCode.SByte: return (Byte)a * (SByte)b;
                        case TypeCode.Int16: return (Byte)a * (Int16)b;
                        case TypeCode.UInt16: return (Byte)a * (UInt16)b;
                        case TypeCode.Int32: return (Byte)a * (Int32)b;
                        case TypeCode.UInt32: return (Byte)a * (UInt32)b;
                        case TypeCode.Int64: return (Byte)a * (Int64)b;
                        case TypeCode.UInt64: return (Byte)a * (UInt64)b;
                        case TypeCode.Single: return (Byte)a * (Single)b;
                        case TypeCode.Double: return (Byte)a * (Double)b;
                        case TypeCode.Decimal: return (Byte)a * (Decimal)b;
                    }
                    break;
                case TypeCode.SByte:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'sbyte' and 'bool'");
                        case TypeCode.SByte: return (SByte)a * (SByte)b;
                        case TypeCode.Int16: return (SByte)a * (Int16)b;
                        case TypeCode.UInt16: return (SByte)a * (UInt16)b;
                        case TypeCode.Int32: return (SByte)a * (Int32)b;
                        case TypeCode.UInt32: return (SByte)a * (UInt32)b;
                        case TypeCode.Int64: return (SByte)a * (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'sbyte' and 'ulong'");
                        case TypeCode.Single: return (SByte)a * (Single)b;
                        case TypeCode.Double: return (SByte)a * (Double)b;
                        case TypeCode.Decimal: return (SByte)a * (Decimal)b;
                    }
                    break;

                case TypeCode.Int16:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'short' and 'bool'");
                        case TypeCode.SByte: return (Int16)a * (SByte)b;
                        case TypeCode.Int16: return (Int16)a * (Int16)b;
                        case TypeCode.UInt16: return (Int16)a * (UInt16)b;
                        case TypeCode.Int32: return (Int16)a * (Int32)b;
                        case TypeCode.UInt32: return (Int16)a * (UInt32)b;
                        case TypeCode.Int64: return (Int16)a * (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'short' and 'ulong'");
                        case TypeCode.Single: return (Int16)a * (Single)b;
                        case TypeCode.Double: return (Int16)a * (Double)b;
                        case TypeCode.Decimal: return (Int16)a * (Decimal)b;
                    }
                    break;

                case TypeCode.UInt16:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ushort' and 'bool'");
                        case TypeCode.SByte: return (UInt16)a * (SByte)b;
                        case TypeCode.Int16: return (UInt16)a * (Int16)b;
                        case TypeCode.UInt16: return (UInt16)a * (UInt16)b;
                        case TypeCode.Int32: return (UInt16)a * (Int32)b;
                        case TypeCode.UInt32: return (UInt16)a * (UInt32)b;
                        case TypeCode.Int64: return (UInt16)a * (Int64)b;
                        case TypeCode.UInt64: return (UInt16)a * (UInt64)b;
                        case TypeCode.Single: return (UInt16)a * (Single)b;
                        case TypeCode.Double: return (UInt16)a * (Double)b;
                        case TypeCode.Decimal: return (UInt16)a * (Decimal)b;
                    }
                    break;

                case TypeCode.Int32:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'int' and 'bool'");
                        case TypeCode.SByte: return (Int32)a * (SByte)b;
                        case TypeCode.Int16: return (Int32)a * (Int16)b;
                        case TypeCode.UInt16: return (Int32)a * (UInt16)b;
                        case TypeCode.Int32: return (Int32)a * (Int32)b;
                        case TypeCode.UInt32: return (Int32)a * (UInt32)b;
                        case TypeCode.Int64: return (Int32)a * (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'int' and 'ulong'");
                        case TypeCode.Single: return (Int32)a * (Single)b;
                        case TypeCode.Double: return (Int32)a * (Double)b;
                        case TypeCode.Decimal: return (Int32)a * (Decimal)b;
                    }
                    break;

                case TypeCode.UInt32:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'uint' and 'bool'");
                        case TypeCode.SByte: return (UInt32)a * (SByte)b;
                        case TypeCode.Int16: return (UInt32)a * (Int16)b;
                        case TypeCode.UInt16: return (UInt32)a * (UInt16)b;
                        case TypeCode.Int32: return (UInt32)a * (Int32)b;
                        case TypeCode.UInt32: return (UInt32)a * (UInt32)b;
                        case TypeCode.Int64: return (UInt32)a * (Int64)b;
                        case TypeCode.UInt64: return (UInt32)a * (UInt64)b;
                        case TypeCode.Single: return (UInt32)a * (Single)b;
                        case TypeCode.Double: return (UInt32)a * (Double)b;
                        case TypeCode.Decimal: return (UInt32)a * (Decimal)b;
                    }
                    break;

                case TypeCode.Int64:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'long' and 'bool'");
                        case TypeCode.SByte: return (Int64)a * (SByte)b;
                        case TypeCode.Int16: return (Int64)a * (Int16)b;
                        case TypeCode.UInt16: return (Int64)a * (UInt16)b;
                        case TypeCode.Int32: return (Int64)a * (Int32)b;
                        case TypeCode.UInt32: return (Int64)a * (UInt32)b;
                        case TypeCode.Int64: return (Int64)a * (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'long' and 'ulong'");
                        case TypeCode.Single: return (Int64)a * (Single)b;
                        case TypeCode.Double: return (Int64)a * (Double)b;
                        case TypeCode.Decimal: return (Int64)a * (Decimal)b;
                    }
                    break;

                case TypeCode.UInt64:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'bool'");
                        case TypeCode.SByte: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'sbyte'");
                        case TypeCode.Int16: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'short'");
                        case TypeCode.UInt16: return (UInt64)a * (UInt16)b;
                        case TypeCode.Int32: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'int'");
                        case TypeCode.UInt32: return (UInt64)a * (UInt32)b;
                        case TypeCode.Int64: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'long'");
                        case TypeCode.UInt64: return (UInt64)a * (UInt64)b;
                        case TypeCode.Single: return (UInt64)a * (Single)b;
                        case TypeCode.Double: return (UInt64)a * (Double)b;
                        case TypeCode.Decimal: return (UInt64)a * (Decimal)b;
                    }
                    break;

                case TypeCode.Single:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'float' and 'bool'");
                        case TypeCode.SByte: return (Single)a * (SByte)b;
                        case TypeCode.Int16: return (Single)a * (Int16)b;
                        case TypeCode.UInt16: return (Single)a * (UInt16)b;
                        case TypeCode.Int32: return (Single)a * (Int32)b;
                        case TypeCode.UInt32: return (Single)a * (UInt32)b;
                        case TypeCode.Int64: return (Single)a * (Int64)b;
                        case TypeCode.UInt64: return (Single)a * (UInt64)b;
                        case TypeCode.Single: return (Single)a * (Single)b;
                        case TypeCode.Double: return (Single)a * (Double)b;
                        case TypeCode.Decimal: return Convert.ToDecimal(a) * (Decimal)b;
                    }
                    break;

                case TypeCode.Double:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'double' and 'bool'");
                        case TypeCode.SByte: return (Double)a * (SByte)b;
                        case TypeCode.Int16: return (Double)a * (Int16)b;
                        case TypeCode.UInt16: return (Double)a * (UInt16)b;
                        case TypeCode.Int32: return (Double)a * (Int32)b;
                        case TypeCode.UInt32: return (Double)a * (UInt32)b;
                        case TypeCode.Int64: return (Double)a * (Int64)b;
                        case TypeCode.UInt64: return (Double)a * (UInt64)b;
                        case TypeCode.Single: return (Double)a * (Single)b;
                        case TypeCode.Double: return (Double)a * (Double)b;
                        case TypeCode.Decimal: return Convert.ToDecimal(a) * (Decimal)b;
                    }
                    break;

                case TypeCode.Decimal:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'decimal' and 'bool'");
                        case TypeCode.SByte: return (Decimal)a * (SByte)b;
                        case TypeCode.Int16: return (Decimal)a * (Int16)b;
                        case TypeCode.UInt16: return (Decimal)a * (UInt16)b;
                        case TypeCode.Int32: return (Decimal)a * (Int32)b;
                        case TypeCode.UInt32: return (Decimal)a * (UInt32)b;
                        case TypeCode.Int64: return (Decimal)a * (Int64)b;
                        case TypeCode.UInt64: return (Decimal)a * (UInt64)b;
                        case TypeCode.Single: return (Decimal)a * Convert.ToDecimal(b);
                        case TypeCode.Double: return (Decimal)a * Convert.ToDecimal(b);
                        case TypeCode.Decimal: return (Decimal)a * (Decimal)b;
                    }
                    break;
            }

            return null;
        }

        internal static object Subtract(object a, object b)
        {
            if (a is null || b is null)
            {
                return null;
            }

            a = ConvertIfStringOrUnit(a);
            b = ConvertIfStringOrUnit(b);

            if (a is double && double.IsNaN((double)a))
            {
                return a;
            }
            if (b is double && double.IsNaN((double)b))
            {
                return b;
            }

            TypeCode typeCodeA = TypeHelper.GetTypeCode(a);
            TypeCode typeCodeB = TypeHelper.GetTypeCode(b);

            switch (typeCodeA)
            {
                case TypeCode.Boolean:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'bool'");
                        case TypeCode.Byte: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.SByte: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.Int16: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.UInt16: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.Int32: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.UInt32: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.Int64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.Single: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.Double: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case TypeCode.Decimal: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                    }
                    break;
                case TypeCode.Byte:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'byte' and 'bool'");
                        case TypeCode.SByte: return (Byte)a - (SByte)b;
                        case TypeCode.Int16: return (Byte)a - (Int16)b;
                        case TypeCode.UInt16: return (Byte)a - (UInt16)b;
                        case TypeCode.Int32: return (Byte)a - (Int32)b;
                        case TypeCode.UInt32: return (Byte)a - (UInt32)b;
                        case TypeCode.Int64: return (Byte)a - (Int64)b;
                        case TypeCode.UInt64: return (Byte)a - (UInt64)b;
                        case TypeCode.Single: return (Byte)a - (Single)b;
                        case TypeCode.Double: return (Byte)a - (Double)b;
                        case TypeCode.Decimal: return (Byte)a - (Decimal)b;
                    }
                    break;
                case TypeCode.SByte:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'sbyte' and 'bool'");
                        case TypeCode.SByte: return (SByte)a - (SByte)b;
                        case TypeCode.Int16: return (SByte)a - (Int16)b;
                        case TypeCode.UInt16: return (SByte)a - (UInt16)b;
                        case TypeCode.Int32: return (SByte)a - (Int32)b;
                        case TypeCode.UInt32: return (SByte)a - (UInt32)b;
                        case TypeCode.Int64: return (SByte)a - (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'sbyte' and 'ulong'");
                        case TypeCode.Single: return (SByte)a - (Single)b;
                        case TypeCode.Double: return (SByte)a - (Double)b;
                        case TypeCode.Decimal: return (SByte)a - (Decimal)b;
                    }
                    break;

                case TypeCode.Int16:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'short' and 'bool'");
                        case TypeCode.SByte: return (Int16)a - (SByte)b;
                        case TypeCode.Int16: return (Int16)a - (Int16)b;
                        case TypeCode.UInt16: return (Int16)a - (UInt16)b;
                        case TypeCode.Int32: return (Int16)a - (Int32)b;
                        case TypeCode.UInt32: return (Int16)a - (UInt32)b;
                        case TypeCode.Int64: return (Int16)a - (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'short' and 'ulong'");
                        case TypeCode.Single: return (Int16)a - (Single)b;
                        case TypeCode.Double: return (Int16)a - (Double)b;
                        case TypeCode.Decimal: return (Int16)a - (Decimal)b;
                    }
                    break;

                case TypeCode.UInt16:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ushort' and 'bool'");
                        case TypeCode.SByte: return (UInt16)a - (SByte)b;
                        case TypeCode.Int16: return (UInt16)a - (Int16)b;
                        case TypeCode.UInt16: return (UInt16)a - (UInt16)b;
                        case TypeCode.Int32: return (UInt16)a - (Int32)b;
                        case TypeCode.UInt32: return (UInt16)a - (UInt32)b;
                        case TypeCode.Int64: return (UInt16)a - (Int64)b;
                        case TypeCode.UInt64: return (UInt16)a - (UInt64)b;
                        case TypeCode.Single: return (UInt16)a - (Single)b;
                        case TypeCode.Double: return (UInt16)a - (Double)b;
                        case TypeCode.Decimal: return (UInt16)a - (Decimal)b;
                    }
                    break;

                case TypeCode.Int32:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'int' and 'bool'");
                        case TypeCode.SByte: return (Int32)a - (SByte)b;
                        case TypeCode.Int16: return (Int32)a - (Int16)b;
                        case TypeCode.UInt16: return (Int32)a - (UInt16)b;
                        case TypeCode.Int32: return (Int32)a - (Int32)b;
                        case TypeCode.UInt32: return (Int32)a - (UInt32)b;
                        case TypeCode.Int64: return (Int32)a - (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'int' and 'ulong'");
                        case TypeCode.Single: return (Int32)a - (Single)b;
                        case TypeCode.Double: return (Int32)a - (Double)b;
                        case TypeCode.Decimal: return (Int32)a - (Decimal)b;
                    }
                    break;

                case TypeCode.UInt32:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'uint' and 'bool'");
                        case TypeCode.SByte: return (UInt32)a - (SByte)b;
                        case TypeCode.Int16: return (UInt32)a - (Int16)b;
                        case TypeCode.UInt16: return (UInt32)a - (UInt16)b;
                        case TypeCode.Int32: return (UInt32)a - (Int32)b;
                        case TypeCode.UInt32: return (UInt32)a - (UInt32)b;
                        case TypeCode.Int64: return (UInt32)a - (Int64)b;
                        case TypeCode.UInt64: return (UInt32)a - (UInt64)b;
                        case TypeCode.Single: return (UInt32)a - (Single)b;
                        case TypeCode.Double: return (UInt32)a - (Double)b;
                        case TypeCode.Decimal: return (UInt32)a - (Decimal)b;
                    }
                    break;

                case TypeCode.Int64:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'long' and 'bool'");
                        case TypeCode.SByte: return (Int64)a - (SByte)b;
                        case TypeCode.Int16: return (Int64)a - (Int16)b;
                        case TypeCode.UInt16: return (Int64)a - (UInt16)b;
                        case TypeCode.Int32: return (Int64)a - (Int32)b;
                        case TypeCode.UInt32: return (Int64)a - (UInt32)b;
                        case TypeCode.Int64: return (Int64)a - (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'long' and 'ulong'");
                        case TypeCode.Single: return (Int64)a - (Single)b;
                        case TypeCode.Double: return (Int64)a - (Double)b;
                        case TypeCode.Decimal: return (Int64)a - (Decimal)b;
                    }
                    break;

                case TypeCode.UInt64:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'bool'");
                        case TypeCode.SByte: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'double'");
                        case TypeCode.Int16: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'short'");
                        case TypeCode.UInt16: return (UInt64)a - (UInt16)b;
                        case TypeCode.Int32: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'int'");
                        case TypeCode.UInt32: return (UInt64)a - (UInt32)b;
                        case TypeCode.Int64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'long'");
                        case TypeCode.UInt64: return (UInt64)a - (UInt64)b;
                        case TypeCode.Single: return (UInt64)a - (Single)b;
                        case TypeCode.Double: return (UInt64)a - (Double)b;
                        case TypeCode.Decimal: return (UInt64)a - (Decimal)b;
                    }
                    break;

                case TypeCode.Single:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'float' and 'bool'");
                        case TypeCode.SByte: return (Single)a - (SByte)b;
                        case TypeCode.Int16: return (Single)a - (Int16)b;
                        case TypeCode.UInt16: return (Single)a - (UInt16)b;
                        case TypeCode.Int32: return (Single)a - (Int32)b;
                        case TypeCode.UInt32: return (Single)a - (UInt32)b;
                        case TypeCode.Int64: return (Single)a - (Int64)b;
                        case TypeCode.UInt64: return (Single)a - (UInt64)b;
                        case TypeCode.Single: return (Single)a - (Single)b;
                        case TypeCode.Double: return (Single)a - (Double)b;
                        case TypeCode.Decimal: return Convert.ToDecimal(a) - (Decimal)b;
                    }
                    break;

                case TypeCode.Double:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'double' and 'bool'");
                        case TypeCode.SByte: return (Double)a - (SByte)b;
                        case TypeCode.Int16: return (Double)a - (Int16)b;
                        case TypeCode.UInt16: return (Double)a - (UInt16)b;
                        case TypeCode.Int32: return (Double)a - (Int32)b;
                        case TypeCode.UInt32: return (Double)a - (UInt32)b;
                        case TypeCode.Int64: return (Double)a - (Int64)b;
                        case TypeCode.UInt64: return (Double)a - (UInt64)b;
                        case TypeCode.Single: return (Double)a - (Single)b;
                        case TypeCode.Double: return (Double)a - (Double)b;
                        case TypeCode.Decimal: return Convert.ToDecimal(a) - (Decimal)b;
                    }
                    break;

                case TypeCode.Decimal:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'decimal' and 'bool'");
                        case TypeCode.SByte: return (Decimal)a - (SByte)b;
                        case TypeCode.Int16: return (Decimal)a - (Int16)b;
                        case TypeCode.UInt16: return (Decimal)a - (UInt16)b;
                        case TypeCode.Int32: return (Decimal)a - (Int32)b;
                        case TypeCode.UInt32: return (Decimal)a - (UInt32)b;
                        case TypeCode.Int64: return (Decimal)a - (Int64)b;
                        case TypeCode.UInt64: return (Decimal)a - (UInt64)b;
                        case TypeCode.Single: return (Decimal)a - Convert.ToDecimal(b);
                        case TypeCode.Double: return (Decimal)a - Convert.ToDecimal(b);
                        case TypeCode.Decimal: return (Decimal)a - (Decimal)b;
                    }
                    break;
            }

            return null;
        }

        internal static object Modulus(object a, object b)
        {
            if (a is null || b is null)
            {
                return null;
            }

            a = ConvertIfStringOrUnit(a);
            b = ConvertIfStringOrUnit(b);

            if (a is double && double.IsNaN((double)a))
            {
                return a;
            }
            if (b is double && double.IsNaN((double)b))
            {
                return b;
            }

            TypeCode typeCodeA = TypeHelper.GetTypeCode(a);
            TypeCode typeCodeB = TypeHelper.GetTypeCode(b);

            switch (typeCodeA)
            {
                case TypeCode.Byte:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'byte' and 'bool'");
                        case TypeCode.SByte: return (Byte)a % (SByte)b;
                        case TypeCode.Int16: return (Byte)a % (Int16)b;
                        case TypeCode.UInt16: return (Byte)a % (UInt16)b;
                        case TypeCode.Int32: return (Byte)a % (Int32)b;
                        case TypeCode.UInt32: return (Byte)a % (UInt32)b;
                        case TypeCode.Int64: return (Byte)a % (Int64)b;
                        case TypeCode.UInt64: return (Byte)a % (UInt64)b;
                        case TypeCode.Single: return (Byte)a % (Single)b;
                        case TypeCode.Double: return (Byte)a % (Double)b;
                        case TypeCode.Decimal: return (Byte)a % (Decimal)b;
                    }
                    break;
                case TypeCode.SByte:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'sbyte' and 'bool'");
                        case TypeCode.SByte: return (SByte)a % (SByte)b;
                        case TypeCode.Int16: return (SByte)a % (Int16)b;
                        case TypeCode.UInt16: return (SByte)a % (UInt16)b;
                        case TypeCode.Int32: return (SByte)a % (Int32)b;
                        case TypeCode.UInt32: return (SByte)a % (UInt32)b;
                        case TypeCode.Int64: return (SByte)a % (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'sbyte' and 'ulong'");
                        case TypeCode.Single: return (SByte)a % (Single)b;
                        case TypeCode.Double: return (SByte)a % (Double)b;
                        case TypeCode.Decimal: return (SByte)a % (Decimal)b;
                    }
                    break;

                case TypeCode.Int16:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'short' and 'bool'");
                        case TypeCode.SByte: return (Int16)a % (SByte)b;
                        case TypeCode.Int16: return (Int16)a % (Int16)b;
                        case TypeCode.UInt16: return (Int16)a % (UInt16)b;
                        case TypeCode.Int32: return (Int16)a % (Int32)b;
                        case TypeCode.UInt32: return (Int16)a % (UInt32)b;
                        case TypeCode.Int64: return (Int16)a % (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'short' and 'ulong'");
                        case TypeCode.Single: return (Int16)a % (Single)b;
                        case TypeCode.Double: return (Int16)a % (Double)b;
                        case TypeCode.Decimal: return (Int16)a % (Decimal)b;
                    }
                    break;

                case TypeCode.UInt16:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ushort' and 'bool'");
                        case TypeCode.SByte: return (UInt16)a % (SByte)b;
                        case TypeCode.Int16: return (UInt16)a % (Int16)b;
                        case TypeCode.UInt16: return (UInt16)a % (UInt16)b;
                        case TypeCode.Int32: return (UInt16)a % (Int32)b;
                        case TypeCode.UInt32: return (UInt16)a % (UInt32)b;
                        case TypeCode.Int64: return (UInt16)a % (Int64)b;
                        case TypeCode.UInt64: return (UInt16)a % (UInt64)b;
                        case TypeCode.Single: return (UInt16)a % (Single)b;
                        case TypeCode.Double: return (UInt16)a % (Double)b;
                        case TypeCode.Decimal: return (UInt16)a % (Decimal)b;
                    }
                    break;

                case TypeCode.Int32:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'int' and 'bool'");
                        case TypeCode.SByte: return (Int32)a % (SByte)b;
                        case TypeCode.Int16: return (Int32)a % (Int16)b;
                        case TypeCode.UInt16: return (Int32)a % (UInt16)b;
                        case TypeCode.Int32: return (Int32)a % (Int32)b;
                        case TypeCode.UInt32: return (Int32)a % (UInt32)b;
                        case TypeCode.Int64: return (Int32)a % (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'int' and 'ulong'");
                        case TypeCode.Single: return (Int32)a % (Single)b;
                        case TypeCode.Double: return (Int32)a % (Double)b;
                        case TypeCode.Decimal: return (Int32)a % (Decimal)b;
                    }
                    break;

                case TypeCode.UInt32:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'uint' and 'bool'");
                        case TypeCode.SByte: return (UInt32)a % (SByte)b;
                        case TypeCode.Int16: return (UInt32)a % (Int16)b;
                        case TypeCode.UInt16: return (UInt32)a % (UInt16)b;
                        case TypeCode.Int32: return (UInt32)a % (Int32)b;
                        case TypeCode.UInt32: return (UInt32)a % (UInt32)b;
                        case TypeCode.Int64: return (UInt32)a % (Int64)b;
                        case TypeCode.UInt64: return (UInt32)a % (UInt64)b;
                        case TypeCode.Single: return (UInt32)a % (Single)b;
                        case TypeCode.Double: return (UInt32)a % (Double)b;
                        case TypeCode.Decimal: return (UInt32)a % (Decimal)b;
                    }
                    break;

                case TypeCode.Int64:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'long' and 'bool'");
                        case TypeCode.SByte: return (Int64)a % (SByte)b;
                        case TypeCode.Int16: return (Int64)a % (Int16)b;
                        case TypeCode.UInt16: return (Int64)a % (UInt16)b;
                        case TypeCode.Int32: return (Int64)a % (Int32)b;
                        case TypeCode.UInt32: return (Int64)a % (UInt32)b;
                        case TypeCode.Int64: return (Int64)a % (Int64)b;
                        case TypeCode.UInt64: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'long' and 'ulong'");
                        case TypeCode.Single: return (Int64)a % (Single)b;
                        case TypeCode.Double: return (Int64)a % (Double)b;
                        case TypeCode.Decimal: return (Int64)a % (Decimal)b;
                    }
                    break;

                case TypeCode.UInt64:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'bool'");
                        case TypeCode.SByte: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'sbyte'");
                        case TypeCode.Int16: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'short'");
                        case TypeCode.UInt16: return (UInt64)a % (UInt16)b;
                        case TypeCode.Int32: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'int'");
                        case TypeCode.UInt32: return (UInt64)a % (UInt32)b;
                        case TypeCode.Int64: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'long'");
                        case TypeCode.UInt64: return (UInt64)a % (UInt64)b;
                        case TypeCode.Single: return (UInt64)a % (Single)b;
                        case TypeCode.Double: return (UInt64)a % (Double)b;
                        case TypeCode.Decimal: return (UInt64)a % (Decimal)b;
                    }
                    break;

                case TypeCode.Single:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'float' and 'bool'");
                        case TypeCode.SByte: return (Single)a % (SByte)b;
                        case TypeCode.Int16: return (Single)a % (Int16)b;
                        case TypeCode.UInt16: return (Single)a % (UInt16)b;
                        case TypeCode.Int32: return (Single)a % (Int32)b;
                        case TypeCode.UInt32: return (Single)a % (UInt32)b;
                        case TypeCode.Int64: return (Single)a % (Int64)b;
                        case TypeCode.UInt64: return (Single)a % (UInt64)b;
                        case TypeCode.Single: return (Single)a % (Single)b;
                        case TypeCode.Double: return (Single)a % (Double)b;
                        case TypeCode.Decimal: return Convert.ToDecimal(a) % (Decimal)b;
                    }
                    break;

                case TypeCode.Double:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'double' and 'bool'");
                        case TypeCode.SByte: return (Double)a % (SByte)b;
                        case TypeCode.Int16: return (Double)a % (Int16)b;
                        case TypeCode.UInt16: return (Double)a % (UInt16)b;
                        case TypeCode.Int32: return (Double)a % (Int32)b;
                        case TypeCode.UInt32: return (Double)a % (UInt32)b;
                        case TypeCode.Int64: return (Double)a % (Int64)b;
                        case TypeCode.UInt64: return (Double)a % (UInt64)b;
                        case TypeCode.Single: return (Double)a % (Single)b;
                        case TypeCode.Double: return (Double)a % (Double)b;
                        case TypeCode.Decimal: return Convert.ToDecimal(a) % (Decimal)b;
                    }
                    break;

                case TypeCode.Decimal:
                    switch (typeCodeB)
                    {
                        case TypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'decimal' and 'bool'");
                        case TypeCode.SByte: return (Decimal)a % (SByte)b;
                        case TypeCode.Int16: return (Decimal)a % (Int16)b;
                        case TypeCode.UInt16: return (Decimal)a % (UInt16)b;
                        case TypeCode.Int32: return (Decimal)a % (Int32)b;
                        case TypeCode.UInt32: return (Decimal)a % (UInt32)b;
                        case TypeCode.Int64: return (Decimal)a % (Int64)b;
                        case TypeCode.UInt64: return (Decimal)a % (UInt64)b;
                        case TypeCode.Single: return (Decimal)a % Convert.ToDecimal(b);
                        case TypeCode.Double: return (Decimal)a % Convert.ToDecimal(b);
                        case TypeCode.Decimal: return (Decimal)a % (Decimal)b;
                    }
                    break;
            }

            return null;
        }

        internal static object Max(object a, object b)
        {
            a = ConvertIfStringOrUnit(a);
            b = ConvertIfStringOrUnit(b);

            if (a is null || b is null)
            {
                return null;
            }

            TypeCode typeCodeA = TypeHelper.GetTypeCode(a);

            switch (typeCodeA)
            {
                case TypeCode.Byte:
                    return Math.Max((Byte)a, Convert.ToByte(b));
                case TypeCode.SByte:
                    return Math.Max((SByte)a, Convert.ToSByte(b));
                case TypeCode.Int16:
                    return Math.Max((Int16)a, Convert.ToInt16(b));
                case TypeCode.UInt16:
                    return Math.Max((UInt16)a, Convert.ToUInt16(b));
                case TypeCode.Int32:
                    return Math.Max((Int32)a, Convert.ToInt32(b));
                case TypeCode.UInt32:
                    return Math.Max((UInt32)a, Convert.ToUInt32(b));
                case TypeCode.Int64:
                    return Math.Max((Int64)a, Convert.ToInt64(b));
                case TypeCode.UInt64:
                    return Math.Max((UInt64)a, Convert.ToUInt64(b));
                case TypeCode.Single:
                    return Math.Max((Single)a, Convert.ToSingle(b));
                case TypeCode.Double:
                    return Math.Max((Double)a, Convert.ToDouble(b));
                case TypeCode.Decimal:
                    return Math.Max((Decimal)a, Convert.ToDecimal(b));
            }

            return null;
        }

        internal static object Min(object a, object b)
        {
            a = ConvertIfStringOrUnit(a);
            b = ConvertIfStringOrUnit(b);

            if (a is null && b is null)
            {
                return null;
            }

            if (a is null)
            {
                return b;
            }

            if (b is null)
            {
                return a;
            }

            TypeCode typeCodeA = TypeHelper.GetTypeCode(a);

            switch (typeCodeA)
            {
                case TypeCode.Byte:
                    return Math.Min((Byte)a, Convert.ToByte(b));
                case TypeCode.SByte:
                    return Math.Min((SByte)a, Convert.ToSByte(b));
                case TypeCode.Int16:
                    return Math.Min((Int16)a, Convert.ToInt16(b));
                case TypeCode.UInt16:
                    return Math.Min((UInt16)a, Convert.ToUInt16(b));
                case TypeCode.Int32:
                    return Math.Min((Int32)a, Convert.ToInt32(b));
                case TypeCode.UInt32:
                    return Math.Min((UInt32)a, Convert.ToUInt32(b));
                case TypeCode.Int64:
                    return Math.Min((Int64)a, Convert.ToInt64(b));
                case TypeCode.UInt64:
                    return Math.Min((UInt64)a, Convert.ToUInt64(b));
                case TypeCode.Single:
                    return Math.Min((Single)a, Convert.ToSingle(b));
                case TypeCode.Double:
                    return Math.Min((Double)a, Convert.ToDouble(b));
                case TypeCode.Decimal:
                    return Math.Min((Decimal)a, Convert.ToDecimal(b));
            }

            return null;
        }
    }
}
