using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Scryber.Expressive.Expressions.Binary.Bitwise;

namespace Scryber.Expressive.Tokenisation
{
    public class Tokeniser2 : ITokeniser
    {
        public Context Context { get; set; }

        protected int CurrentIndex { get; set; }

        public TokenList Root { get; set; }

        public Tokeniser2(Context context)
        {
            this.Context = context;
            this.CurrentIndex = 0;
        }

        public IList<Token> Tokenise(string expression)
        {
            if (this.CurrentIndex > 0)
                throw new InvalidOperationException("This tokenizer is already running, get your own");

            this.CurrentIndex = 0;
            int length;
            this.Root = new TokenList();

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
                else if (IsOperator(expression, this.CurrentIndex, out length))
                {
                    TokeniseOperator(expression, length);
                }
                else if (IsNumber(expression, this.CurrentIndex, out length))
                {
                    TokeniseNumber(expression, length);
                }
                else if (IsSeparator(expression, this.CurrentIndex, out length))
                {
                    TokeniseSeparator(expression, length);
                }
                else if(IsString(expression, this.CurrentIndex, out length))
                {
                    TokeniseString(expression, length);
                }
                else if(IsDate(expression, this.CurrentIndex, out length))
                {
                    TokeniseDate(expression, length);
                }
                else
                    throw new Exceptions.UnrecognisedTokenException(expression.Substring(this.CurrentIndex));
            }

            this.CurrentIndex = 0;
            return Root;
        }

        private bool IsString(string expression, int index, out int length)
        {
            if (expression[index] == '\'' || expression[index] == '"')
            {
                char ending = expression[index];
                int start = index;
                index++;
                bool foundEnd = false;

                while (index < expression.Length)
                {
                    if (expression[index] == ending && expression[index-1] != '\\')
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

        private void TokeniseString(string expression, int length)
        {
            int start = this.CurrentIndex;
            this.CurrentIndex += length;
            this.AddNewToken(start, length, expression.Substring(start, length), ExpressionTokenType.Date);
        }

        private bool IsDate(string expression, int index, out int length)
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

        private void TokeniseDate(string expression, int length)
        {
            int start = this.CurrentIndex;
            this.CurrentIndex += length;
            this.AddNewToken(start, length, expression.Substring(start, length), ExpressionTokenType.Date);
        }

        private bool IsSeparator(string expression, int index, out int length)
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

        private void TokeniseSeparator(string expression, int length)
        {
            int start = this.CurrentIndex;
            this.CurrentIndex += length;
            this.AddNewToken(start, length, expression.Substring(start, length), ExpressionTokenType.Separator);
        }

        private bool IsWhiteSpace(string expression, int index, out int length)
        {
            int start = index;
            while(index < expression.Length)
            {
                if(char.IsWhiteSpace(expression, index))
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

        private void IgnoreToken(string expression, int length)
        {
            this.CurrentIndex += length;
        }

        private void TokeniseKeyword(string expression, int length)
        {
            int start = this.CurrentIndex;
            this.CurrentIndex+= length;
            var name = expression.Substring(start, length);


            Func<Expressions.IExpression[], IDictionary<string, object>, object> func;

            if (this.Context.TryGetFunction(name, out func))
            {
                this.AddNewToken(start, length, name , ExpressionTokenType.Function);
            }
            else if(this.IsConstant(name))
            {
                this.AddNewToken(start, length, name, ExpressionTokenType.Constant);
            }
            else
            {
                this.AddNewToken(start, length, "[" + name + "]", ExpressionTokenType.Variable);
            }
        }

        public bool IsConstant(string name)
        {
            bool result = false;
            var comparer = this.Context.ParsingStringComparer;


            if (this.Context.ParsingStringComparison == StringComparison.CurrentCultureIgnoreCase
                || this.Context.ParsingStringComparison == StringComparison.InvariantCultureIgnoreCase ||
                this.Context.ParsingStringComparison == StringComparison.OrdinalIgnoreCase)
            {
                if (name.Equals("true", this.Context.ParsingStringComparison))
                {
                    result = true;
                }
                else if (name.Equals("false", this.Context.ParsingStringComparison))
                {
                    result = true;
                }
                else if (name.Equals("null", this.Context.ParsingStringComparison))
                {
                    result = true;
                }
            }
            else
            {
                if (name.Equals("true", this.Context.ParsingStringComparison))
                {
                    result = true;
                }
                else if (name.Equals("TRUE", Context.ParsingStringComparison))
                {
                    result = true;
                }
                else if (name.Equals("false", this.Context.ParsingStringComparison))
                {
                    result = true;
                }
                else if (name.Equals("FALSE", this.Context.ParsingStringComparison))
                {
                    result = true;
                }
                else if (name.Equals("null", this.Context.ParsingStringComparison))
                {
                    result = true;
                }
                else if (name.Equals("NULL", this.Context.ParsingStringComparison))
                {
                    result = true;
                }
            }
            return result;
        }

        public bool IsKeyword(string expression, int index, out int length)
        {
            int start = index;
            while (index < expression.Length)
            {
                if(char.IsLetter(expression, index))
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

        public bool IsOperator(string expression, int index, out int length, bool secondCheck = false)
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
                    if (secondCheck)
                    {
                        length = 2;
                        return true;
                    }
                    else if((expression.Length > index+1) && IsOperator(expression, index+1, out length, secondCheck = true ))
                    {
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

        public void TokeniseOperator(string expression, int length)
        {
            this.AddNewToken(this.CurrentIndex, length, expression.Substring(this.CurrentIndex, length), ExpressionTokenType.Operator);
            this.CurrentIndex += length;
        }

        private bool IsNumber(string expression, int index, out int length)
        {
            int start = index;
            while (index < expression.Length)
            {
                if (char.IsDigit(expression, index))
                {
                    index++;
                }
                else if (expression[index] == this.Context.DecimalSeparator)
                {
                    index++;
                }
                else
                    break;
            }
            length = index - start;
            return length > 0;

        }

        private void TokeniseNumber(string expression, int length)
        {
            this.AddNewToken(this.CurrentIndex, length, expression.Substring(this.CurrentIndex, length), ExpressionTokenType.Number);
            this.CurrentIndex += length;
        }

        public void AddNewToken(int start, int length, string value, ExpressionTokenType type)
        {
            Token t = new Token(value, start, length);
            this.Root.Add(t);
        }
    }


    public enum ExpressionTokenType
    {
        Function,
        Variable,
        Operator,
        Number,
        Parenthese,
        Constant,
        Separator,
        Date,
        Other
    }

}
