using System.Collections.Generic;
using Scryber.Expressive.Exceptions;

namespace Scryber.Expressive.Tokenisation
{
    internal class ParenthesisedTokenExtractor : ITokenExtractor
    {
        private readonly char endingCharacter;
        private readonly char startingCharacter;

        public ParenthesisedTokenExtractor(char singleCharacter) : this(singleCharacter, singleCharacter)
        {
        }

        public ParenthesisedTokenExtractor(char startingCharacter, char endingCharacter)
        {
            this.startingCharacter = startingCharacter;
            this.endingCharacter = endingCharacter;
        }

        public Token ExtractToken(string expression, int currentIndex, Context context)
        {
            var character = expression[currentIndex];

            if (character != this.startingCharacter)
            {
                return null;
            }

            var extracted = GetString(expression, currentIndex, this.endingCharacter);

            if (string.IsNullOrWhiteSpace(extracted))
            {
                throw new MissingTokenException($"Missing closing token '{this.endingCharacter}'", this.endingCharacter);
            }

            return new Token(extracted, currentIndex);
        }

        private static string GetString(string expression, int startIndex, char expectedEndingCharacter)
        {
            var index = startIndex;
            var foundEndingCharacter = false;
            var character = expression[index];
            var isEscape = false;

            while (index < expression.Length && !foundEndingCharacter)
            {
                if (index != startIndex &&
                    character == expectedEndingCharacter &&
                    !isEscape) // Make sure the escape character wasn't previously used.
                {
                    foundEndingCharacter = true;
                }

                if (character == '\\' && !isEscape)
                {
                    isEscape = true;
                }
                else
                {
                    isEscape = false;
                }

                index++;

                if (index == expression.Length)
                {
                    break;
                }

                character = expression[index];
            }

            return foundEndingCharacter ? expression.Substring(startIndex, index - startIndex) : null;
        }
    }
}