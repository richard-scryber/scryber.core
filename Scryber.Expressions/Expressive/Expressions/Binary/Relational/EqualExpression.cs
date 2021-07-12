using System.Collections.Generic;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Expressions.Binary.Relational
{
    internal class EqualExpression : BinaryExpressionBase
    {
        #region Constructors

        public EqualExpression(IExpression lhs, IExpression rhs, Context context) : base(lhs, rhs, context)
        {
        }

        #endregion

        #region BinaryExpressionBase Members

        protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
        {
            // Use the type of the left operand to make the comparison
            if (lhsResult is null)
            {
                return rightHandSide.Evaluate(variables) is null;
            }

            var rhsResult = rightHandSide.Evaluate(variables);

            // If we got here then the lhsResult is not null.
            if (rhsResult is null)
            {
                return false;
            }

            return Comparison.CompareUsingMostPreciseType(lhsResult, rhsResult, this.Context) == 0;
        }

        #endregion
    }
}
