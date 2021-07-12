using Scryber.Expressive.Expressions;
using System.Collections.Generic;

namespace Scryber.Expressive.Operators
{
    /// <summary>
    /// Base class implementation of <see cref="IOperator"/>.
    /// </summary>
    public abstract class OperatorBase : IOperator
    {
        #region IOperator Members

        /// <inheritdoc />
        public abstract IEnumerable<string> Tags { get; }

        /// <inheritdoc />
        public abstract IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context);

        /// <inheritdoc />
        public virtual bool CanGetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
        {
            return true;
        }

        /// <inheritdoc />
        public virtual Token[] GetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
        {
            return new[] { token };
        }

        /// <inheritdoc />
        public virtual Token[] GetInnerCaptiveTokens(Token[] allCaptiveTokens)
        {
#pragma warning disable CA1825 // Avoid zero-length array allocations. - Array.Empty does not exist in net 4.5
            return new Token[0];
#pragma warning restore CA1825 // Avoid zero-length array allocations.
        }

        /// <inheritdoc />
        public abstract OperatorPrecedence GetPrecedence(Token previousToken);

        #endregion
    }
}
