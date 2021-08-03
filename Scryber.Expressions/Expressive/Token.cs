using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using Scryber.Expressive.Tokenisation;

namespace Scryber.Expressive
{ 
    /// <summary>
    /// Represents a chunk of expression that has been identified as something compilable.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Gets the text from the expression.
        /// </summary>
        public string CurrentToken { get; }

        /// <summary>
        /// Gets the length of the text.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets the index where it was discovered in the text.
        /// </summary>
        public int StartIndex { get; }

        /// <summary>
        /// Gets the type of expression token this refers to.
        /// </summary>
        public ExpressionTokenType Type { get; }
        

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="currentToken">The text from the expression.</param>
        /// <param name="startIndex">The index where it was discovered in the text.</param>
        public Token(string currentToken, int startIndex, ExpressionTokenType type)
            : this(currentToken, startIndex, currentToken?.Length ?? 0, type)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="currentToken">The text from the expression.</param>
        /// <param name="startIndex">The index where it was discovered in the text.</param>
        /// <param name="length">The length of the token in the original expression</param>
        public Token(string currentToken, int startIndex, int length, ExpressionTokenType type)
        {
            this.CurrentToken = currentToken;
            this.StartIndex = startIndex;
            this.Length = length;
            this.Type = type;
        }

        public override string ToString()
        {
            return this.CurrentToken;
        }
    }

    public class TokenList : List<Token>
    {

        public TokenList(): base()
        {

        }

        /// <summary>
        /// Makes sure there are an equal number of ( and ) in the token list. 0 = equal, -1 = more open, 1 = more close
        /// </summary>
        /// <returns></returns>
        public int CompareParenthese()
        {
            int open = 0;
            int close = 0;

            foreach (var token in this)
            {
                if(token.Type == ExpressionTokenType.Parenthese)
                {
                    switch (token.CurrentToken[0])
                    {
                        case ('('):
                            open++;
                            break;
                        case (')'):
                            close++;
                            break;
                        default:
                            break;
                    }
                }
            }
            return open.CompareTo(close);
        }
    }

}
