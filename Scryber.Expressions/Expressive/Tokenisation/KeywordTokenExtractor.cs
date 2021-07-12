using System;
using System.Collections.Generic;

namespace Scryber.Expressive.Tokenisation
{
    internal class KeywordTokenExtractor : ITokenExtractor
    {
        private readonly IEnumerable<string> keywords;

        public KeywordTokenExtractor(IEnumerable<string> keywords)
        {
            
            if (keywords is null)
            {
                throw new ArgumentNullException(nameof(keywords));
            }

            this.keywords = keywords;
        }

        public Token ExtractToken(string expression, int currentIndex, Context context)
        {
            var expressionLength = expression.Length;

            foreach (var possibleName in this.keywords)
            {
                var lookAhead = expression.Substring(currentIndex, Math.Min(possibleName.Length, expressionLength - currentIndex));

                if (!string.Equals(lookAhead, possibleName, context.ParsingStringComparison) || HasContinuationCharacter(expression,possibleName, currentIndex))
                {
                    continue;
                }

                return new Token(lookAhead, currentIndex);
            }

            return null;
        }

        //Checks to see if the next character in the string continues a name beyond possible name (e.g. model and mode)
        private bool HasContinuationCharacter(string expression, string possibleName, int currentIndex)
        {
            if (expression.Length == possibleName.Length + currentIndex)
                return false;
            char c = expression[possibleName.Length + currentIndex]; // character after
            if (char.IsLetterOrDigit(c) || c == '_')
                return true;
            else
                return false;
        }
    }
}