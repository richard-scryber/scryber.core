using System;
using System.Collections.Generic;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Expressions.Binary.Multiplicative
{
    internal class ExponentExpression : BinaryExpressionBase
    {
        #region Constructors

        public ExponentExpression(IExpression lhs, IExpression rhs, Context context) : base(lhs, rhs, context)
        {
        }

        #endregion

        #region BinaryExpressionBase Members

        protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables) =>
            EvaluateAggregates(lhsResult, rightHandSide, variables, (l, r, vars) =>
                Math.Pow(Convert.ToDouble(l), Convert.ToDouble(r)));

        #endregion
    }
}
