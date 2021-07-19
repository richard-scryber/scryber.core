using System.Collections.Generic;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Operators.Grouping
{
    public class ParenthesisCloseOperator : OperatorBase
    {
        #region OperatorBase Members

        public override IEnumerable<string> Tags => new[] { ")" };

        public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
        {
            return expressions[0] ?? expressions[1];
        }

        public override OperatorPrecedence GetPrecedence(Token previousToken)
        {
            return OperatorPrecedence.ParenthesisClose;
        }

        #endregion
    }
}
