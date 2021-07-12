using System.Collections.Generic;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Expressions.Binary.Additive
{
    internal class AddExpression : BinaryExpressionBase
    {
        #region Constructors

        public AddExpression(IExpression lhs, IExpression rhs, Context context) : base(lhs, rhs, context)
        {
        }

        #endregion

        #region BinaryExpressionBase Members

        /// <inheritdoc />
        protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
        {
            if (lhsResult is string stringValue)
            {
                return stringValue + rightHandSide.Evaluate(variables);
            }

            return EvaluateAggregates(lhsResult, rightHandSide, variables, Numbers.Add);
        }

        #endregion
    }
}
