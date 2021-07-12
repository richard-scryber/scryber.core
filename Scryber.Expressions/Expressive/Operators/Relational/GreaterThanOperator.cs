using System.Collections.Generic;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Expressions.Binary.Relational;

namespace Scryber.Expressive.Operators.Relational
{
    internal class GreaterThanOperator : OperatorBase
    {
        #region OperatorBase Members

        public override IEnumerable<string> Tags => new[] { ">" };

        public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
        {
            return new GreaterThanExpression(expressions[0], expressions[1], context);
        }

        public override OperatorPrecedence GetPrecedence(Token previousToken)
        {
            return OperatorPrecedence.GreaterThan;
        }

        #endregion
    }
}
