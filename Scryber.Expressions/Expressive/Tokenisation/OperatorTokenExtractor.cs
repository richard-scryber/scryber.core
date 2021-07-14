using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Tokenisation
{
    public class OperatorTokenExtractor : ITokenExtractor
    {
        private readonly IEnumerable<string> operators;

        
        public OperatorTokenExtractor(IEnumerable<string> operators)
        {
            if (operators is null)
            {
                throw new ArgumentNullException(nameof(operators));
            }

            this.operators = operators;
        }

        public Token ExtractToken(string expression, int currentIndex, Context context)
        {
            
            var expressionLength = expression.Length;

            foreach (var possibleName in this.operators)
            {

                var lookAhead = expression.Substring(currentIndex, Math.Min(possibleName.Length, expressionLength - currentIndex));

                if (!string.Equals(lookAhead, possibleName, context.ParsingStringComparison) || HasContinuationCharacter(expression, possibleName, currentIndex))
                {
                    continue;
                }

                return new Token(lookAhead, currentIndex);
            }

            return null;
        }

        //Checks to see if the next character in the string continues a name beyond possible name (e.g. mod and model)
        //whilst making sure that the +var is not considered a continuation

        private bool HasContinuationCharacter(string expression, string possibleName, int currentIndex)
        {
            if (expression.Length == possibleName.Length + currentIndex)
                return false;
            char last = expression[possibleName.Length + currentIndex - 1]; // last character
            char next = expression[possibleName.Length + currentIndex]; //character after
            if (char.IsLetterOrDigit(last) && char.IsLetterOrDigit(next)) //we have a continuation after the last letter
                return true;
            else
                return false;
        }
    }
}
