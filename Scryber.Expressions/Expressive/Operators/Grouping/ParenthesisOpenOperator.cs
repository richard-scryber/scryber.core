using Scryber.Expressive.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scryber.Expressive.Operators.Grouping
{
    internal class ParenthesisOpenOperator : IOperator
    {
        #region IOperator Members

        public IEnumerable<string> Tags => new[] { "(" };

        public IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
        {
            return new ParenthesisedExpression(expressions[0] ?? expressions[1]);
        }

        public bool CanGetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
        {
            var remainingTokensCopy = new Queue<Token>(remainingTokens.ToArray());

            return this.GetCaptiveTokens(previousToken, token, remainingTokensCopy).Any();
        }

        public Token[] GetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
        {
            IList<Token> result = new List<Token>();

            result.Add(token);

            var parenCount = 1;

            while (remainingTokens.Any())
            {
                var nextToken = remainingTokens.Dequeue();

                result.Add(nextToken);

                if (string.Equals(nextToken.CurrentToken, "(", StringComparison.Ordinal))
                {
                    parenCount++;
                }
                else if (string.Equals(nextToken.CurrentToken, ")", StringComparison.Ordinal))
                {
                    parenCount--;
                }

                if (parenCount <= 0)
                {
                    break;
                }
            }

            return result.ToArray();
        }

        public Token[] GetInnerCaptiveTokens(Token[] allCaptiveTokens)
        {
            return allCaptiveTokens.Skip(1).Take(allCaptiveTokens.Length - 2).ToArray();
        }

        public OperatorPrecedence GetPrecedence(Token previousToken)
        {
            return OperatorPrecedence.ParenthesisOpen;
        }

        #endregion
    }
}
