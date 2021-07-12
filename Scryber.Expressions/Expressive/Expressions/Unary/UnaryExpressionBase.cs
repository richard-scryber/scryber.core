using System.Collections.Generic;

namespace Scryber.Expressive.Expressions.Unary
{
    internal abstract class UnaryExpressionBase : IExpression
    {
        #region Fields

        protected readonly IExpression expression;

        #endregion

        internal UnaryExpressionBase(IExpression expression)
        {
            this.expression = expression;
        }

        #region IExpression Members

        public abstract object Evaluate(IDictionary<string, object> variables);

        #endregion
    }
}
