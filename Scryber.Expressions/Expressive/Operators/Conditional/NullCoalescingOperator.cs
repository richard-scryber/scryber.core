using System.Collections.Generic;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Expressions.Binary.Conditional;

namespace Scryber.Expressive.Operators.Conditional
{
    internal class NullCoalescingOperator : OperatorBase
    {
        #region OperatorBase Members

        public override IEnumerable<string> Tags => new[] { "??" };

        public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
        {
            return new NullCoalescingExpression(expressions[0], expressions[1], context);
        }

        public override OperatorPrecedence GetPrecedence(Token previousToken)
        {
            return OperatorPrecedence.NullCoalescing;
        }

        #endregion
    }
}
