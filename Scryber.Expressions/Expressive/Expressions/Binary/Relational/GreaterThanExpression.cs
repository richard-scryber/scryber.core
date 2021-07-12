using System.Collections.Generic;
using Scryber.Expressive.Helpers;

namespace Scryber.Expressive.Expressions.Binary.Relational
{
    internal class GreaterThanExpression : BinaryExpressionBase
    {
        #region Constructors

        public GreaterThanExpression(IExpression lhs, IExpression rhs, Context context) : base(lhs, rhs, context)
        {
        }

        #endregion

        #region BinaryExpressionBase Members

        protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables) => 
            Comparison.CompareUsingMostPreciseType(lhsResult, rightHandSide.Evaluate(variables), this.Context) > 0;

        #endregion
    }
}
