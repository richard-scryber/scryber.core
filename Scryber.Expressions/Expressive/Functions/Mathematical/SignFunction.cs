using Scryber.Expressive.Expressions;
using Scryber.Expressive.Helpers;
using System;

namespace Scryber.Expressive.Functions.Mathematical
{
    public class SignFunction : FunctionBase
    {
        #region FunctionBase Members

        public override string Name { get { return "Sign"; } }

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
                        return Math.Sign(Convert.ToDecimal(value));
                    case TypeCode.Double:
                        return Math.Sign(Convert.ToDouble(value));
                    case TypeCode.Int16:
                        return Math.Sign(Convert.ToInt16(value));
                    case TypeCode.UInt16:
                        return Math.Sign(Convert.ToUInt16(value));
                    case TypeCode.Int32:
                        return Math.Sign(Convert.ToInt32(value));
                    case TypeCode.UInt32:
                        return Math.Sign(Convert.ToUInt32(value));
                    case TypeCode.Int64:
                        return Math.Sign(Convert.ToInt64(value));
                    case TypeCode.SByte:
                        return Math.Sign(Convert.ToSByte(value));
                    case TypeCode.Single:
                        return Math.Sign(Convert.ToSingle(value));
                    default:
                        break;
                }
            }

            return null;
        }

        #endregion
    }
}
