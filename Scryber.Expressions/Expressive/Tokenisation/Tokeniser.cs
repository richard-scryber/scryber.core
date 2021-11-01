using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Scryber.Expressive.Expressions.Binary.Bitwise;

namespace Scryber.Expressive.Tokenisation
{
    /// <summary>
    /// Converts an expression string into a list of tokens (variables, functions, separators, operators etc.)
    /// </summary>
    public class Tokeniser : ITokeniser
    {
        //
        // properties
        //

        #region public Context Context { get; }

        /// <summary>
        /// Gets the current context to tokenise an expression with
        /// </summary>
        public Context Context
        {
            get;
            private set;
        }

        #endregion

        #region public int CurrentIndex {get; protected set; }

        /// <summary>
        /// Gets or sets the current index in the string we are tokenising
        /// </summary>
        public int CurrentIndex
        {
            get;
            protected set;
        }

        #endregion

        #region public bool IsRunning { get; }

        /// <summary>
        /// Returns true if this tokenizer is currently executing
        /// </summary>
        public bool IsRunning
        {
            get { return this.CurrentIndex > 0; }
        }

        #endregion

        #region public TokenList Root {get;}

        /// <summary>
        /// The list of tokens (so far)
        /// </summary>
        public TokenList Root
        {
            get;
            private set;
        }

        #endregion

        //
        // .ctor
        //

        #region public Tokeniser(Context context)

        /// <summary>
        /// Creates a new instance of teh 
        /// </summary>
        /// <param name="context"></param>
        public Tokeniser(Context context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.CurrentIndex = 0;
        }

        #endregion

        //
        // public methods
        //

        #region public TokenList Tokenise(string expression)

        /// <summary>
        /// Tokenizes a single expression string into a list of tokens
        /// </summary>
        /// <param name="expression">The expression to tokenise</param>
        /// <returns>A complete list of all the tokens in the expression</returns>
        public TokenList Tokenise(string expression)
        {
            if (this.CurrentIndex > 0)
                throw new InvalidOperationException("This tokenizer is already running, get your own");

            this.CurrentIndex = 0;
            
            this.Root = new TokenList();

            try
            {
                this.DoTokenise(expression);
            }
            finally
            {
                this.CurrentIndex = 0;
            }

            return this.Root;
        }

        #endregion

        //
        // implementation
        //

        #region DoTokenise

        /// <summary>
        /// Main method that splits the expression string into the relevant tokens, adding them to the Root token list
        /// </summary>
        /// <param name="expression">The expression to split</param>
        protected virtual void DoTokenise(string expression)
        {
            int length;
            while (this.CurrentIndex < expression.Length)
            {
                if (IsWhiteSpace(expression, this.CurrentIndex, out length))
                {
                    this.IgnoreToken(expression, length);
                }
                else if (IsKeyword(expression, this.CurrentIndex, out length))
                {
                    TokeniseKeyword(expression, length);
                }
                else if(IsCssVariableName(expression, this.CurrentIndex, out length))
                {
                    TokeniseKeyword(expression, length);
                }
                else if (IsOperator(expression, this.CurrentIndex, out length))
                {
                    TokeniseOperator(expression, length);
                }
                else if (IsNumber(expression, this.CurrentIndex, out length, out bool isUnit))
                {
                    TokeniseNumber(expression, length, isUnit);
                }
                else if (IsSeparator(expression, this.CurrentIndex, out length))
                {
                    TokeniseSeparator(expression, length);
                }
                else if(IsString(expression, this.CurrentIndex, out length))
                {
                    TokeniseString(expression, length);
                }
                else if(IsColor(expression, this.CurrentIndex, out length))
                {
                    TokeniseColor(expression, length);
                }
                else if(IsDate(expression, this.CurrentIndex, out length))
                {
                    TokeniseDate(expression, length);
                }
                else
                    throw new Exceptions.UnrecognisedTokenException(expression.Substring(this.CurrentIndex));
            }
        }

        #endregion

        protected virtual bool IsColor(string expression, int index, out int length)
        {
            int start = index;
            length = 0;

            if (expression[index] == '#')
            {
                bool match = true;
                index++;
                while (match && index < expression.Length)
                {
                    switch (expression[index])
                    {
                        case ('0'):
                        case ('1'):
                        case ('2'):
                        case ('3'):
                        case ('4'):
                        case ('5'):
                        case ('6'):
                        case ('7'):
                        case ('8'):
                        case ('9'):
                        case ('A'):
                        case ('B'):
                        case ('C'):
                        case ('D'):
                        case ('E'):
                        case ('F'):
                        case ('a'):
                        case ('b'):
                        case ('c'):
                        case ('d'):
                        case ('e'):
                        case ('f'):
                            index++;
                            break;
                        default:
                            match = false;
                            break;
                    }
                }

                length = index - start;

                if (length == 4) // #FFF
                    return true;
                else if (length == 7) // #FFFFFF
                    return true;

            }

            return false;
        }

        protected void TokeniseColor(string expression, int length)
        {
            int start = this.CurrentIndex;
            this.CurrentIndex += length;
            var value = expression.Substring(start, length);

            this.AddNewToken(start, length, value, ExpressionTokenType.Color);
        }

        #region IsKeyword + TokeniseKeyword

        /// <summary>
        /// Checks if the characters at and following index in the expression are a keyword (a function, a constant or an expression)
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected virtual bool IsKeyword(string expression, int index, out int length)
        {
            int start = index;
            while (index < expression.Length)
            {
                if (char.IsLetter(expression, index))
                {
                    index++;
                }
                else if (index > start && char.IsDigit(expression, index))
                {
                    index++;
                }
                else if (expression[index] == '_')
                {
                    index++;
                }
                else
                    break;
            }
            length = index - start;
            return length > 0;
        }

        /// <summary>
        /// Extracts a keyword token from the expression based on the specified length -
        /// a keyword can either be a known function ('follwed by ('), a known constant or it will be declared as a variable.
        /// Increments the current index the length of the keyword.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="length"></param>
        protected virtual void TokeniseKeyword(string expression, int length)
        {
            int start = this.CurrentIndex;
            this.CurrentIndex+= length;
            var name = expression.Substring(start, length);


            ExecFunction func;
            int constLen;
            if (this.Context.TryGetFunction(name, out func) && expression.Length > (start + length) && expression[start + length] == '(')
            {
                this.AddNewToken(start, length, name, ExpressionTokenType.Function);
            }
            else if (this.IsConstant(name, out constLen))
            {
                this.AddNewToken(start, constLen, name, ExpressionTokenType.Constant);
            }
            else if (expression.Length > (start + length) && expression[start + length] == '(')
                throw new Exceptions.UnrecognisedTokenException(name);
            else
            {
                this.AddNewToken(start, length, "[" + name + "]", ExpressionTokenType.Variable);
            }
        }

        #endregion

        #region IsCSSVariableName

        /// <summary>
        /// Checks to see if the current token is a css variable name (start with -- and contains -_A-Z1-9)
        /// </summary>
        /// <param name="expression">The expression to check</param>
        /// <param name="index">The index to check at</param>
        /// <param name="length">Set to the length of the variable name</param>
        /// <returns>True if the following string is a css variable name</returns>
        protected virtual bool IsCssVariableName(string expression, int index, out int length)
        {
            int start = index;

            if (expression[index] == '-' && expression.Length > index + 1 && expression[index + 1] == '-')
            {
                index += 2;

                while (index < expression.Length)
                {
                    if (char.IsLetterOrDigit(expression, index))
                        index++;
                    else if (expression[index] == '_')
                        index++;
                    else if (expression[index] == '-')
                    {
                        index++;
                    }
                    else
                        break;
                }

                length = index - start;
                return length > 0;
            }
            else
            {
                length = 0;
                return false;

            }
        }

        #endregion

        #region IsConstant

        /// <summary>
        /// Returns true if the name represents a known constant expression - null, true, false, pi or e. The comparison is based on the contect parsing string comparison.
        /// </summary>
        /// <param name="name">THe name to check</param>
        /// <param name="length">Set to the length of the string </param>
        /// <returns>True if it is a known string</returns>
        protected virtual bool IsConstant(string name, out int length)
        {
            bool result = false;
            length = 0;

            var comparer = this.Context.ParsingStringComparer;


            if (this.Context.ParsingStringComparison == StringComparison.CurrentCultureIgnoreCase
                || this.Context.ParsingStringComparison == StringComparison.InvariantCultureIgnoreCase ||
                this.Context.ParsingStringComparison == StringComparison.OrdinalIgnoreCase)
            {
                if (name.Equals("true", this.Context.ParsingStringComparison))
                {
                    length = 4;
                    result = true;
                }
                else if (name.Equals("false", this.Context.ParsingStringComparison))
                {
                    length = 5;
                    result = true;
                }
                else if (name.Equals("null", this.Context.ParsingStringComparison))
                {
                    length = 4;
                    result = true;
                }
                else if(name.Equals("pi", this.Context.ParsingStringComparison))
                {
                    length = 2;
                    result = true;
                }
                else if(name.Equals("e", this.Context.ParsingStringComparison))
                {
                    length = 1;
                    result = true;
                }
            }
            else
            {
                if (name.Equals("true", this.Context.ParsingStringComparison))
                {
                    length = 4;
                    result = true;
                }
                else if (name.Equals("TRUE", Context.ParsingStringComparison))
                {
                    length = 4;
                    result = true;
                }
                else if (name.Equals("false", this.Context.ParsingStringComparison))
                {
                    length = 5;
                    result = true;
                }
                else if (name.Equals("FALSE", this.Context.ParsingStringComparison))
                {
                    length = 5;
                    result = true;
                }
                else if (name.Equals("null", this.Context.ParsingStringComparison))
                {
                    length = 4;
                    result = true;
                }
                else if (name.Equals("NULL", this.Context.ParsingStringComparison))
                {
                    length = 4;
                    result = true;
                }
                else if (name.Equals("PI", this.Context.ParsingStringComparison))
                {
                    length = 2;
                    result = true;
                }
                else if (name.Equals("E", this.Context.ParsingStringComparison))
                {
                    length = 4;
                    result = true;
                }
            }
            return result;
        }

        #endregion

        #region IsWhiteSpace + IgnoreToken

        /// <summary>
        /// Checks if the characters at index in expression are whitespace and to be ignored. If so, returns true and the length of the white space.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected virtual bool IsWhiteSpace(string expression, int index, out int length)
        {
            int start = index;
            while (index < expression.Length)
            {
                if (char.IsWhiteSpace(expression, index))
                {
                    index++;
                }
                else
                {
                    break;
                }
            }
            length = index - start;
            return length > 0;
        }

        /// <summary>
        /// Ignores the characters in the expression from the current index upto and including length
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="length"></param>
        protected virtual void IgnoreToken(string expression, int length)
        {
            this.CurrentIndex += length;
        }

        #endregion

        #region IsString + TokeniseString

        /// <summary>
        /// Checks if the characters following index in the expression represent a string literal e.g "This is a string" 'and so is this'.
        /// Returning true and the length of the string
        /// </summary>
        /// <param name="expression">The expression to check</param>
        /// <param name="index">The index to start checking</param>
        /// <param name="length">The output length of the string if found otherwise 0</param>
        /// <returns>True if it is a literal string, otherwise false</returns>
        protected virtual bool IsString(string expression, int index, out int length)
        {
            if (expression[index] == '\'' || expression[index] == '"')
            {
                char ending = expression[index];
                int start = index;
                index++;
                bool foundEnd = false;

                while (index < expression.Length)
                {
                    if (expression[index] == ending && expression[index - 1] != '\\')
                    {
                        index++;
                        foundEnd = true;
                        break;
                    }
                    else
                    {
                        index++;
                    }
                }
                if (!foundEnd)
                {
                    length = 0;
                    return false;
                }
                else
                {
                    length = index - start;
                    return true;
                }
            }
            else
            {
                length = 0;
                return false;
            }
        }

        /// <summary>
        /// Extracts the string literal token from expression (including the quotes) and adds it to the token list.
        /// Incrementing the current index by the required amount
        /// </summary>
        /// <param name="expression">The expresssion to extract from</param>
        /// <param name="length">The length of the string</param>
        protected virtual void TokeniseString(string expression, int length)
        {
            int start = this.CurrentIndex;
            this.CurrentIndex += length;
            this.AddNewToken(start, length, expression.Substring(start, length), ExpressionTokenType.Date);
        }


        #endregion

        #region IsDate + TokeniseDate

        /// <summary>
        /// Checks to see if the characters following index in the expression represent a date value (e.g. #12-30-2021 12:00:00#).
        /// If so then it will return true and the length of the date token
        /// </summary>
        /// <param name="expression">The expression to check</param>
        /// <param name="index">The index at which to start checking</param>
        /// <param name="length">The found length of the date token or 0</param>
        /// <returns>True if a date was found</returns>
        protected virtual bool IsDate(string expression, int index, out int length)
        {
            if (expression[index] == Context.DateSeparator)
            {
                int start = index;
                index++;
                bool foundEnd = false;

                while (index < expression.Length)
                {
                    if (expression[index] == Context.DateSeparator)
                    {
                        index++;
                        foundEnd = true;
                        break;
                    }
                    else
                    {
                        index++;
                    }
                }

                if (!foundEnd)
                {
                    length = 0;
                    return false;
                }
                else
                {
                    length = index - start;
                    return true;
                }
            }
            else
            {
                length = 0;
                return false;
            }
        }

        /// <summary>
        /// Extracts a date token and increments the current index by the specified length
        /// </summary>
        /// <param name="expression">The expression to take the date from</param>
        /// <param name="length">The full length of the date token</param>
        protected virtual void TokeniseDate(string expression, int length)
        {
            int start = this.CurrentIndex;
            this.CurrentIndex += length;
            this.AddNewToken(start, length, expression.Substring(start, length), ExpressionTokenType.Date);
        }

        #endregion

        #region IsSeparator + TokeniseSeparator

        /// <summary>
        /// Checks if the character at index in the expression is a separator (e.g. ( [ ; ,), returning true and the length if so.
        /// </summary>
        /// <param name="expression">The expression to check</param>
        /// <param name="index">The index at which to start checking</param>
        /// <param name="length">The final length of the separator</param>
        /// <returns>True if this is a separator character</returns>
        protected virtual bool IsSeparator(string expression, int index, out int length)
        {
            switch (expression[index])
            {
                case ('('):
                case (','):
                case (';'):
                case (')'):
                case ('['):
                case (']'):
                case ('.'):
                    length = 1;
                    return true;

                default:
                    length = 0;
                    return false;
            }
        }

        /// <summary>
        /// Creates a new Token in the list, from the expression, and increments the current index
        /// </summary>
        /// <param name="expression">The expression to take the token from</param>
        /// <param name="length">The length of the token</param>
        protected virtual void TokeniseSeparator(string expression, int length)
        {
            int start = this.CurrentIndex;
            this.CurrentIndex += length;
            var content = expression.Substring(start, length);
            if (content.Length == 1 && (content[0] == '(' || content[0] == ')'))
                this.AddNewToken(start, length, content, ExpressionTokenType.Parenthese);
            else
                this.AddNewToken(start, length, content, ExpressionTokenType.Separator);
        }

        #endregion

        #region IsOperator + TokeniseOperator

        /// <summary>
        /// Checks the character at the index in the expression to understand if it is an operator (e.g. + - / etc.)
        /// Returns true and the appropriate length of the operator.
        /// </summary>
        /// <param name="expression">The expression to check</param>
        /// <param name="index">The index within the expression to start checking</param>
        /// <param name="length">THe output length of the operator</param>
        /// <returns>True if it is an operator, otherwise false</returns>
        protected virtual bool IsOperator(string expression, int index, out int length)
        {
            switch (expression[index])
            {
                case ('+'):
                case ('-'):
                case ('*'):
                case ('/'):
                case ('%'):
                case ('^'):
                    length = 1;
                    return true;
                case ('&'):
                case ('|'):
                case ('<'):
                case ('>'):
                case ('?'):
                case ('='):
                case ('!'):
                    if(IsAllowedSecondOperatorChar(expression, index +1, expression[index]))
                    {
                        length = 2;
                        return true;
                    }
                    else
                    {
                        length = 1;
                        return true;
                    }
                default:
                    length = 0;
                    return false;
            }
        }

        /// <summary>
        /// Checks if the following character is an allowed second character in an operator e.g <> != &&
        /// </summary>
        /// <param name="expression">The expression to check</param>
        /// <param name="secondIndex">The index of the second character</param>
        /// <param name="firstOperator">The first operator character, to ensure it is valid</param>
        /// <returns></returns>
        private bool IsAllowedSecondOperatorChar(string expression, int secondIndex, char firstOperator)
        {
            if (expression.Length <= secondIndex)
                return false;

            var second = expression[secondIndex];
            switch (firstOperator)
            {
                case ('&'): //&&
                    if (second == '&')
                        return true;
                    else
                        return false;

                case ('|'): // ||
                    if (second == '|')
                        return true;
                    else
                        return false;

                case ('<'): // <= << <>
                    if (second == '=')
                        return true;
                    else if (second == '<')
                        return true;
                    else if (second == '>')
                        return true;
                    else
                        return false;

                case ('>'): // >= >>
                    if (second == '=')
                        return true;
                    else if (second == '>')
                        return true;
                    else
                        return false;

                case ('?'): // ?= ??
                    if (second == '=')
                        return true;
                    else if (second == '?')
                        return true;
                    else
                        return false;

                case ('='): // ==
                    if (second == '=')
                        return true;
                    else
                        return false;

                case ('!'): // !=
                    if (second == '=')
                        return true;
                    else
                        return false;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Adds a new Operator token to the list with the length and index, moving current idex along by the length
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="length"></param>
        protected virtual void TokeniseOperator(string expression, int length)
        {
            this.AddNewToken(this.CurrentIndex, length, expression.Substring(this.CurrentIndex, length), ExpressionTokenType.Operator);
            this.CurrentIndex += length;
        }



        #endregion

        #region IsNumber + TokeniseNumber

        /// <summary>
        /// Checks if the characters at the specified index is a number returning true, along with the length of the number string.
        /// </summary>
        /// <param name="expression">The expression to check</param>
        /// <param name="index">The index at whick to start checking</param>
        /// <param name="length">The output length of the string that constitutes the number</param>
        /// <returns>True if it is a number</returns>
        protected virtual bool IsNumber(string expression, int index, out int length, out bool isUnit)
        {
            int start = index;
            isUnit = false;

            while (index < expression.Length)
            {
                if (char.IsDigit(expression, index))
                {
                    index++;
                }
                else if (expression[index] == this.Context.DecimalSeparator)
                {
                    if (index > start)
                        index++;
                    else
                        break;
                }
                else if(expression[index] == 'p') //1234.5pt
                {
                    if (index+1 < expression.Length && expression[index + 1] == 't')
                    {
                        index+= 2;
                        isUnit = true;
                        break;
                    }
                    else
                        break;
                }
                else if(expression[index] == 'm') //1234.5mm
                {
                    if (index + 1 < expression.Length && expression[index + 1] == 'm')
                    {
                        index+= 2;
                        isUnit = true;
                        break;
                    }
                    else
                        break;
                }
                else if (expression[index] == 'c') //1234.5cm
                {
                    if (index + 1 < expression.Length && expression[index + 1] == 'm')
                    {
                        index+= 2;
                        isUnit = true;
                        break;
                    }
                    else
                        break;
                }
                else if (expression[index] == 'i') //1234.5mm
                {
                    if (index + 1 < expression.Length && expression[index + 1] == 'n')
                    {
                        index+= 2;
                        isUnit = true;
                        break;
                    }
                    else
                        break;
                }
                else
                    break;
            }

            length = index - start;
            return length > 0;

        }

        /// <summary>
        /// Creates a new number token, and adds it to the list,
        /// incrementing the CurrentIndex of the tokeniser at the same time.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="length"></param>
        protected virtual void TokeniseNumber(string expression, int length, bool isUnit)
        {
            this.AddNewToken(this.CurrentIndex, length, expression.Substring(this.CurrentIndex, length), ExpressionTokenType.Unit);
            this.CurrentIndex += length;
        }

        #endregion

        #region AddNewToken

        /// <summary>
        /// Adds a new token with the position, length and type to the token list
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        protected virtual void AddNewToken(int start, int length, string value, ExpressionTokenType type)
        {
            Token t = new Token(value, start, length, type);
            this.Root.Add(t);
        }

        #endregion
    }


    

}
