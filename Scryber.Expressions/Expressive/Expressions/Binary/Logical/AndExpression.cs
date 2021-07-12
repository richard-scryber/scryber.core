using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Expressions.Binary.Logical
{
    internal class AndExpression : BinaryExpressionBase
    {
        #region Constructors

        public AndExpression(IExpression lhs, IExpression rhs, Context context) : base(lhs, rhs, context)
        {
        }

        #endregion

        #region BinaryExpressionBase Members

        protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables) => 
            Convert.ToBoolean(lhsResult) &&
            Convert.ToBoolean(rightHandSide.Evaluate(variables));

        #endregion
    }
}
