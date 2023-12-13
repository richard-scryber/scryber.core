using System;
using System.Collections.Generic;
using Scryber.Expressive.Exceptions;

namespace Scryber.Expressive.Expressions.Binary.Conditional
{
    internal class NullCoalescingExpression : BinaryExpressionBase
    {
        #region Constructors

        public NullCoalescingExpression(IExpression lhs, IExpression rhs, Context context) : base(lhs, rhs, context)
        {
        }

        #endregion

        public override object Evaluate(IDictionary<string, object> variables)
        {
            if (this.leftHandSide is null)
            {
                throw new MissingParticipantException("The left hand side of the operation is missing.");
            }

            if (this.rightHandSide is null)
            {
                throw new MissingParticipantException("The right hand side of the operation is missing.");
            }

            object lhsResult;
            try
            {

                // We will evaluate the left hand side but hold off on the right hand side as it may not be necessary
                lhsResult = this.leftHandSide.Evaluate(variables);
            }
            catch(ArgumentNullException)
            {
                lhsResult = null;
            }
            catch(ArgumentOutOfRangeException)
            {
                lhsResult = null;
            }
            return this.EvaluateImpl(lhsResult, this.rightHandSide, variables);
        }
        #region BinaryExpressionBase Members

        /// <inheritdoc />
        protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables) =>
            EvaluateAggregates(lhsResult, rightHandSide, variables, (l, r, vars) => l ?? r);

        #endregion
    }
}
