using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Scryber.Expressive.Tokenisation
{
    public class VariableTokenExtractor : ITokenExtractor
    {
        StringBuilder buffer = new StringBuilder();

        public VariableTokenExtractor()
        {
        }

        public Token ExtractToken(string expression, int currentIndex, Context context)
        {
            if (String.IsNullOrEmpty(expression))
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if(char.IsLetter(expression, currentIndex))
            {
                this.buffer.Clear();
                this.buffer.Append(expression[currentIndex]);

                int lastIndex = currentIndex + 1;

                while (lastIndex < expression.Length)
                {
                    if(char.IsLetterOrDigit(expression, lastIndex) || expression[lastIndex] == '_')
                    {
                        this.buffer.Append(expression[lastIndex]);
                        lastIndex++;
                    }
                    else
                    {
                        break;
                    }
                
                }
                return new Token("[" + this.buffer.ToString() + "]", currentIndex, lastIndex - currentIndex);
            }
            else
            {
                return null;
            }
        }

    }
}
