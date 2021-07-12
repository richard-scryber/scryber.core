using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;
using System;

namespace Scryber.Expressive.Functions.Mathematical
{
    internal class AbsFunction : FunctionBase
    {
        #region IFunction Members

        public override string Name { get { return "Abs"; } }

        public override object Evaluate(IExpression[] parameters, Context context)
        {
            this.ValidateParameterCount(parameters, 1, 1);

            var value = parameters[0].Evaluate(Variables);

            if (value != null)
            {
                var valueType = TypeHelper.GetTypeCode(value);

                switch (valueType)
                {
                    case TypeCode.Decimal:
                        return Math.Abs(Convert.ToDecimal(value));
                    case TypeCode.Double:
                        return Math.Abs(Convert.ToDouble(value));
                    case TypeCode.Int16:
                        return Math.Abs(Convert.ToInt16(value));
                    case TypeCode.UInt16:
                        return Math.Abs(Convert.ToUInt16(value));
                    case TypeCode.Int32:
                        return Math.Abs(Convert.ToInt32(value));
                    case TypeCode.UInt32:
                        return Math.Abs(Convert.ToUInt32(value));
                    case TypeCode.Int64:
                        return Math.Abs(Convert.ToInt64(value));
                    case TypeCode.SByte:
                        return Math.Abs(Convert.ToSByte(value));
                    case TypeCode.Single:
                        return Math.Abs(Convert.ToSingle(value));
                    default:
                        break;
                }
            }

            return null;
        }

        #endregion
    }
}
