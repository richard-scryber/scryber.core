using System.Collections.Generic;

namespace Scryber.Expressive.Expressions.Binary.Conditional
{
    internal class NullCoalescingExpression : BinaryExpressionBase
    {
        #region Constructors

        public NullCoalescingExpression(IExpression lhs, IExpression rhs, Context context) : base(lhs, rhs, context)
        {
        }

        #endregion

        #region BinaryExpressionBase Members

        /// <inheritdoc />
        protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables) =>
            EvaluateAggregates(lhsResult, rightHandSide, variables, (l, r) => l ?? r);

        #endregion
    }
}
