using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Expressions.Binary.Logical
{
    internal class OrExpression : BinaryExpressionBase
    {
        #region Constructors

        public OrExpression(IExpression lhs, IExpression rhs, ExpressionContext context) : base(lhs, rhs, context)
        {
        }

        #endregion

        #region BinaryExpressionBase Members

        protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables) =>
            Helpers.Truth.IsTrue(lhsResult) ||
            Helpers.Truth.IsTrue(rightHandSide.Evaluate(variables));

        #endregion
    }
}
