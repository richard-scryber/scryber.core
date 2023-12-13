using System;
using System.Collections.Generic;
using Scryber.Drawing;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Expressions.Binary.Multiplicative
{
    internal class DivideExpression : BinaryExpressionBase
    {
        #region Constructors

        public DivideExpression(IExpression lhs, IExpression rhs, Context context) : base(lhs, rhs, context)
        {
        }

        #endregion

        #region BinaryExpressionBase Members

        /// <inheritdoc />
        protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables) =>
            EvaluateAggregates(lhsResult, rightHandSide, variables, (l, r, vars) => 
                l is null || r is null || IsReal(l) || IsReal(r)
                    ? Numbers.Divide(l, r, vars)
                    : Numbers.Divide(ConvertToDouble(l, vars), r, vars));

        #endregion

        /// <summary>
        /// Converts the value to a double so we know we have a floating point division rather than int/int = int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object ConvertToDouble(object value, IDictionary<string, object> variables)
        {
            if (value is Unit unit)
            {
                if (unit.IsRelative)
                    unit = Numbers.ConvertRelativeUnit(unit, variables);
                return unit.PointsValue;
            }
            else if (value is string s)
            {
                if (Double.TryParse(s, out double d))
                    return d;
                else if (Unit.TryParse(s, out unit))
                {
                    if (unit.IsRelative)
                        unit = Numbers.ConvertRelativeUnit(unit, variables);

                    return unit.PointsValue;
                }
            }
            return Convert.ToDouble(value);

        }
        private static bool IsReal(object value)
        {
            var typeCode = TypeHelper.GetTypeCode(value);

            return typeCode == TypeCode.Decimal || typeCode == TypeCode.Double || typeCode == TypeCode.Single;
        }
    }
}
