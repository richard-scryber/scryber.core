using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Expressive.Helpers;
using Scryber.Expressive.Exceptions;

namespace Scryber.Expressive.Expressions.Binary.Relational
{
    internal class NotEqualExpression : BinaryExpressionBase
    {
        #region Constructors

        public NotEqualExpression(IExpression lhs, IExpression rhs, Context context) : base(lhs, rhs, context)
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
            catch (ArgumentNullException)
            {
                lhsResult = null;
            }
            catch (ArgumentOutOfRangeException)
            {
                lhsResult = null;
            }

            return this.EvaluateImpl(lhsResult, this.rightHandSide, variables);
        }

        #region BinaryExpressionBase Members

        protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
        {
            // Use the type of the left operand to make the comparison
            if (lhsResult is null)
            {
                return rightHandSide.Evaluate(variables) != null;
            }

            var rhsResult = rightHandSide.Evaluate(variables);

            // If we got here then the lhsResult is not null.
            if (rhsResult is null)
            {
                return true;
            }

            return Comparison.CompareUsingMostPreciseType(lhsResult, rhsResult, this.Context) != 0;
        }

        #endregion
    }
}
