using Scryber.Expressive.Expressions;
using System.Collections.Generic;

namespace Scryber.Expressive.Operators
{
    /// <summary>
    /// Definition for all Operators (i.e. +, -, etc.) that are available in Scryber.Expressive.
    /// </summary>
    public interface IOperator
    {
        /// <summary>
        /// Gets the list of tags that can be used to identify this IOperator.
        /// </summary>
        IEnumerable<string> Tags { get; }

        /// <summary>
        /// Builds the operator in to an <see cref="IExpression"/> ready for evaluation.
        /// </summary>
        /// <param name="previousToken">The previous <see cref="Token"/>.</param>
        /// <param name="expressions">The <see cref="IExpression"/>s to use within the operation (e.g. left hand side and right hand side).</param>
        /// <param name="context">The <see cref="Context"/> to use within the operation.</param>
        /// <returns>An <see cref="IExpression"/> that can be evaluated.</returns>
        IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context);

        /// <summary>
        /// Determines whether this implementation can consume the supplied <see cref="Token"/>s.
        /// </summary>
        /// <param name="previousToken">The previous <see cref="Token"/>.</param>
        /// <param name="token">The current <see cref="Token"/>.</param>
        /// <param name="remainingTokens">A remaining <see cref="Token"/>s.</param>
        /// <returns>True if this implementation can consume the supplied <see cref="Token"/>s, false otherwise.</returns>
        bool CanGetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens);

        /// <summary>
        /// Consume the supplied <see cref="Token"/>s.
        /// </summary>
        /// <param name="previousToken">The previous <see cref="Token"/>.</param>
        /// <param name="token">The current <see cref="Token"/>.</param>
        /// <param name="remainingTokens">A remaining <see cref="Token"/>s.</param>
        /// <returns>The consumed <see cref="Token"/>s.</returns>
        Token[] GetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens);

        /// <summary>
        /// Gets any nested <see cref="Token"/>s that can be consumed from within.
        /// </summary>
        /// <param name="allCaptiveTokens">All possible <see cref="Token"/>s to consume.</param>
        /// <returns>The consumed <see cref="Token"/>s.</returns>
        Token[] GetInnerCaptiveTokens(Token[] allCaptiveTokens);

        /// <summary>
        /// Gets the <see cref="OperatorPrecedence"/>. Basically how important it is over the other implementations.
        /// </summary>
        /// <param name="previousToken">The previous <see cref="Token"/>. Useful for determining whether the operator is unary or binary.</param>
        /// <returns>The <see cref="OperatorPrecedence"/> of this implementation.</returns>
        OperatorPrecedence GetPrecedence(Token previousToken);
    }
}
