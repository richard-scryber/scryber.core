using System.Collections.Generic;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Expressions.Binary.Additive
{
    internal class SubtractExpression : BinaryExpressionBase
    {
        #region Constructors

        public SubtractExpression(IExpression lhs, IExpression rhs, Context context) : base(lhs, rhs, context)
        {
        }

        #endregion

        #region BinaryExpressionBase Members

        /// <inheritdoc />
        protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables) =>
            EvaluateAggregates(lhsResult, rightHandSide, variables, Numbers.Subtract);

        #endregion
    }
}
