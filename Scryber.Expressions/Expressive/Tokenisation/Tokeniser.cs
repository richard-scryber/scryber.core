using System.Collections.Generic;
using System.Linq;

namespace Scryber.Expressive.Tokenisation
{
    public class Tokeniser
    {
        #region Fields

        private readonly Context context;
        private readonly IEnumerable<ITokenExtractor> tokenExtractors;

        #endregion

        #region Constructors

        public Tokeniser(Context context, IEnumerable<ITokenExtractor> tokenExtractors)
        {
            this.context = context;

            this.tokenExtractors = tokenExtractors;
        }

        #endregion

        #region Internal Methods

        public IList<Token> Tokenise(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return null;
            }

            var expressionLength = expression.Length;
            var tokens = new List<Token>();
            IList<char> unrecognised = null;

            var index = 0;

            while (index < expressionLength)
            {
                var token = this.tokenExtractors.Select(t => t.ExtractToken(expression, index, this.context)).FirstOrDefault(t => t != null);

                if (token != null)
                {
                    CheckForUnrecognised(unrecognised, tokens, index);
                    unrecognised = null;

                    tokens.Add(token);
                }
                else
                {
                    var character = expression[index];

                    if (!char.IsWhiteSpace(character))
                    {
                        // If we don't recognise this item then we had better keep going until we find something we know about.
                        if (unrecognised is null)
                        {
                            unrecognised = new List<char>();
                        }
                        unrecognised.Add(character);
                    }
                    else
                    {
                        CheckForUnrecognised(unrecognised, tokens, index);
                        unrecognised = null;
                    }
                }

                index += token?.Length ?? 1;
            }

            // Double check whether the last part is unrecognised.
            CheckForUnrecognised(unrecognised, tokens, index);

            return tokens;
        }

        #endregion

        #region Private Methods

        private static void CheckForUnrecognised(IList<char> unrecognised, ICollection<Token> tokens, int index)
        {
            if (unrecognised is null)
            {
                return;
            }

            var currentToken = new string(unrecognised.ToArray());
            tokens.Add(new Token(currentToken, index - currentToken.Length)); // The index supplied is the current location not the start of the unrecognised token.
        }

        #endregion
    }
}