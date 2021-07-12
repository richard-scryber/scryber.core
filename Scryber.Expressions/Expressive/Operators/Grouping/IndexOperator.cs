using System;
using Scryber.Expressive.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace Scryber.Expressive.Operators.Grouping
{
    public class IndexOpenOperator : IOperator
    {
        #region IOperator Members

        public IEnumerable<string> Tags => new[] { "[" };

        public IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
        {
            if (null == expressions)
                throw new ArgumentNullException(nameof(expressions));

            return new IndexorExpression(expressions[0], expressions[1], context);
        }

        public bool CanGetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
        {
            if (null == remainingTokens)
                throw new ArgumentNullException(nameof(remainingTokens));

            var remainingTokensCopy = new Queue<Token>(remainingTokens.ToArray());

            return this.GetCaptiveTokens(previousToken, token, remainingTokensCopy).Any();
        }

        public Token[] GetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
        {
            if (null == remainingTokens)
                throw new ArgumentNullException(nameof(remainingTokens));

            IList<Token> result = new List<Token>();

            result.Add(token);

            var parenCount = 1;

            while (remainingTokens.Any())
            {
                var nextToken = remainingTokens.Dequeue();

                result.Add(nextToken);

                if (string.Equals(nextToken.CurrentToken, "[", StringComparison.Ordinal))
                {
                    parenCount++;
                }
                else if (string.Equals(nextToken.CurrentToken, "]", StringComparison.Ordinal))
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
            if (null == allCaptiveTokens)
                throw new ArgumentNullException(nameof(allCaptiveTokens));

            return allCaptiveTokens.Skip(1).Take(allCaptiveTokens.Length - 2).ToArray();
        }

        public OperatorPrecedence GetPrecedence(Token previousToken)
        {
            return OperatorPrecedence.IndexorOpen;
        }

        #endregion

        public IndexOpenOperator()
        {
        }
    }


    public class IndexCloseOperator : OperatorBase
    {
        #region OperatorBase Members

        public override IEnumerable<string> Tags => new[] { "]" };

        public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
        {
            if (null == expressions)
                throw new ArgumentNullException(nameof(expressions));

            return expressions[0] ?? expressions[1];
        }

        public override OperatorPrecedence GetPrecedence(Token previousToken)
        {
            return OperatorPrecedence.IndexorClose;
        }

        #endregion
    }
}
