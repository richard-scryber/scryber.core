using System.Collections.Generic;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.Conversion
{
    public class BoolFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name => "Bool";

        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 1);

            var objectToConvert = parameters[0].Evaluate(variables);

            return ConvertToBoolean(objectToConvert);
        }

        #endregion

        public static bool ConvertToBoolean(object objectToConvert)
        {
            if (objectToConvert is null) { return false; }
            else if (objectToConvert is bool) { return (bool)objectToConvert; }
            else
            {
                var type = System.Type.GetTypeCode(objectToConvert.GetType());
                bool result;
                switch (type)
                {
                    case System.TypeCode.Boolean:
                        result = (bool)(objectToConvert);
                        break;

                    case System.TypeCode.Byte:
                        var b = (byte)objectToConvert;
                        result = !(b == 0);
                        break;

                    case System.TypeCode.Decimal:
                        var dec = (decimal)objectToConvert;
                        result = !(dec == 0.0M);
                        break;

                    case System.TypeCode.Double:
                        var dou = (double)objectToConvert;
                        result = !(bool)(dou == 0.0 || dou == double.NaN);
                        break;

                    case System.TypeCode.SByte:
                        var sb = (sbyte)objectToConvert;
                        result = !(bool)(sb == 0);
                        break;

                    case System.TypeCode.Single:
                        var sin = (float)objectToConvert;
                        result = !(bool)(sin == 0.0F || sin == float.NaN);
                        break;

                    case System.TypeCode.Int16:
                        var i16 = (short)objectToConvert;
                        result = !(bool)(i16 == 0);
                        break;

                    case System.TypeCode.Int32:
                        var i32 = (int)objectToConvert;
                        result = !(bool)(i32 == 0);
                        break;

                    case System.TypeCode.Int64:
                        var i64 = (long)objectToConvert;
                        result = !(bool)(i64 == 0);
                        break;

                    case System.TypeCode.UInt16:
                        var u16 = (ushort)objectToConvert;
                        result = !(bool)(u16 == 0);
                        break;

                    case System.TypeCode.UInt32:
                        var u32 = (ushort)objectToConvert;
                        result = !(bool)(u32 == 0);
                        break;

                    case System.TypeCode.UInt64:
                        var u64 = (ulong)objectToConvert;
                        result = !(bool)(u64 == 0);
                        break;

                    case System.TypeCode.Char:
                        var c = (char)objectToConvert;
                        result = !(bool)(c == 0);
                        break;

                    case System.TypeCode.DateTime:
                        var dt = (System.DateTime)objectToConvert;
                        result = !(bool)(dt.Ticks == 0);
                        break;

                    case System.TypeCode.DBNull:
                        result = false;
                        break;

                    case System.TypeCode.Empty:
                        result = false;
                        break;

                    case System.TypeCode.String:
                    case System.TypeCode.Object:
                    default:
                        var s = (objectToConvert).ToString();
                        result = !string.IsNullOrEmpty(s);

                        bool parsed;
                        if (result && bool.TryParse(s, out parsed))
                            result = parsed;

                        break;
                }

                return result;
            }
        }
    }


    public class BooleanFunction : BoolFunction
    {
        
        public override string Name => "Boolean";
        
        public BooleanFunction() : base()
        {}
    }
}
