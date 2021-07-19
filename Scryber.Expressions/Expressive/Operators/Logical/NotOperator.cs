using System.Collections.Generic;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Expressions.Unary.Logical;

namespace Scryber.Expressive.Operators.Logical
{
    public class NotOperator : OperatorBase
    {
        #region IOperator Members

        public override IEnumerable<string> Tags => new[] { "!", "not" };

        public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
        {
            return new NotExpression(expressions[0] ?? expressions[1]);
        }

        public override OperatorPrecedence GetPrecedence(Token previousToken)
        {
            return OperatorPrecedence.Not;
        }

        #endregion
    }
}
