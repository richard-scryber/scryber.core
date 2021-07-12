using System.Globalization;

namespace Scryber.Expressive.Tokenisation
{
    internal class NumericTokenExtractor : ITokenExtractor
    {
        public Token ExtractToken(string expression, int currentIndex, Context context)
        {
            var character = expression[currentIndex];

            if (!IsValidStart(character, context, NumberStyles.Any))
            {
                return null;
            }

            var location = GetNumberLocation(expression, currentIndex, context, NumberStyles.Any);

            return new Token(expression.Substring(location.Start, location.Length), currentIndex);
        }

        private static Location GetNumberLocation(string expression, int startIndex, Context context, NumberStyles numberStyles)
        {
            var index = startIndex;
            var character = expression[index];
            var expressionLength = expression.Length;

            while (IsAllowableCharacter(character, expression, index, startIndex, expressionLength, context, ref numberStyles, out var exponentialLocation) &&
                index < expressionLength)
            {
                if (exponentialLocation != null)
                {
                    return new Location(startIndex, exponentialLocation.End);
                }

                index++;

                if (index == expression.Length)
                {
                    break;
                }

                character = expression[index];
            }

            return new Location(startIndex, index);
        }

        private static bool IsAllowableCharacter(
            char character,
            string expression,
            int index,
            int startIndex,
            int expressionLength,
            Context context,
            ref NumberStyles numberStyles,
            out Location exponentialLocation)
        {
            exponentialLocation = null;

            if (char.IsDigit(character) || IsValidStart(character, context, numberStyles) && index == startIndex)
            {
                return true;
            }

            // No more exponential or decimal chars after our first exponential.
            if (!numberStyles.HasFlag(NumberStyles.AllowExponent))
            {
                return false;
            }

            // If we find an exponential character then carry on to find a valid integer.
            if ((character == 'e' || character == 'E') &&
                char.IsDigit(expression[index - 1]) && // make sure the e is preceded by an actual number.
                index + 1 < expressionLength &&
                IsValidStart(expression[index + 1], context, NumberStyles.Integer))
            {
                exponentialLocation = GetNumberLocation(expression, index + 1, context, NumberStyles.Integer);

                if (exponentialLocation != null)
                {
                    // No need to interact with the numberStyles as supplying the location short circuits.
                    return true;
                }
            }

            if (numberStyles.HasFlag(NumberStyles.AllowDecimalPoint) && character == context.DecimalSeparator)
            {
                numberStyles &= ~NumberStyles.AllowDecimalPoint;
                return true;
            }

            return false;
        }

        private static bool IsSignCharacter(char character) => character == '-' || character == '\u2212' || character == '+';

        private static bool IsValidStart(char character, Context context, NumberStyles numberStyles) =>
            char.IsDigit(character) ||
            numberStyles.HasFlag(NumberStyles.AllowLeadingSign) && IsSignCharacter(character) ||
            numberStyles.HasFlag(NumberStyles.AllowDecimalPoint) && character == context.DecimalSeparator;

        private class Location
        {
            public int End { get; }

            public int Length => End - Start;

            public int Start { get; }

            public Location(int start, int end)
            {
                Start = start;
                End = end;
            }
        }
    }
}