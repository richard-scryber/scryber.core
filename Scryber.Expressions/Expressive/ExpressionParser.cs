using Scryber.Expressive.Exceptions;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Operators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Scryber.Expressive.Tokenisation;
using System.Reflection.Metadata;
using System.Diagnostics;

namespace Scryber.Expressive
{
    public class ExpressionParser
    {
        #region Fields

        private readonly Context context;
        private readonly ITokeniser tokeniser;

        #endregion

        #region Constructors

        public ExpressionParser(Context context)
            : this(context, GetDefaultTokenExtractors(context))
        {
        }

        public ExpressionParser(Context context, List<ITokenExtractor> tokenExtractors)
            : this(context, new Tokeniser(context, tokenExtractors))
        { }


        public ExpressionParser(Context context, ITokeniser tokeniser)
        {
            this.context = context;
            this.tokeniser = tokeniser;
        }

        public static List<ITokenExtractor> GetDefaultTokenExtractors(Context context)
        {
            if (null == context)
            {
                throw new NullReferenceException(nameof(context));
            }
            return new List<ITokenExtractor>
                {
                    new KeywordTokenExtractor(context.FunctionNames),
                    new OperatorTokenExtractor(context.OperatorNames),
                    // Variables
                    new ParenthesisedTokenExtractor('[', ']'),
                    new NumericTokenExtractor(),
                    new ValueTokenExtractor("."),
                    // Dates
                    new ParenthesisedTokenExtractor('#'),
                    new ValueTokenExtractor(","),
                    new ParenthesisedTokenExtractor('"'),
                    new ParenthesisedTokenExtractor('\''),      
                    // TODO: Probably a better way to achieve this.
                    new ValueTokenExtractor("true"),
                    new ValueTokenExtractor("TRUE"),
                    new ValueTokenExtractor("false"),
                    new ValueTokenExtractor("FALSE"),
                    new ValueTokenExtractor("null"),
                    new ValueTokenExtractor("NULL"),
                    new VariableTokenExtractor()
                } ;
        }


        #endregion

#region Public Methods


        public IExpression CompileExpression(string expression, IList<string> variables)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ExpressiveException("An Expression cannot be empty.");
            }

            if(null == variables)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            var tokens = this.tokeniser.Tokenise(expression);

            var openCount = tokens.Select(t => t.CurrentToken).Count(t => string.Equals(t, "(", StringComparison.Ordinal));
            var closeCount = tokens.Select(t => t.CurrentToken).Count(t => string.Equals(t, ")", StringComparison.Ordinal));

            // Bail out early if there isn't a matching set of ( and ) characters.
            if (openCount > closeCount)
            {
                throw new ArgumentException("There aren't enough ')' symbols. Expected " + openCount + " but there is only " + closeCount);
            }
            if (openCount < closeCount)
            {
                throw new ArgumentException("There are too many ')' symbols. Expected " + openCount + " but there is " + closeCount);
            }

            var expr = this.CompileExpression(new Queue<Token>(tokens), OperatorPrecedence.Minimum, variables, false);

            return expr;
        }

#endregion

#region Private Methods

        private IExpression CompileExpression(Queue<Token> tokens, OperatorPrecedence minimumPrecedence, IList<string> variables, bool isWithinFunction)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens), "You must call Tokenise before compiling");
            }
            
            IExpression leftHandSide = null;
            var currentToken = tokens.PeekOrDefault();
            Token previousToken = null;

            while (currentToken != null)
            {
                if (this.context.TryGetOperator(currentToken.CurrentToken, out var op)) // Are we an IOperator?
                {
                    var precedence = op.GetPrecedence(previousToken);

                    if (precedence > minimumPrecedence)
                    {
                        tokens.Dequeue();

                        if (!op.CanGetCaptiveTokens(previousToken, currentToken, tokens))
                        {
                            // Do it anyway to update the list of tokens
                            op.GetCaptiveTokens(previousToken, currentToken, tokens);
                            break;
                        }

                        IExpression rightHandSide = null;

                        var captiveTokens = op.GetCaptiveTokens(previousToken, currentToken, tokens);

                        if (captiveTokens.Length > 1)
                        {
                            var innerTokens = op.GetInnerCaptiveTokens(captiveTokens);
                            rightHandSide = this.CompileExpression(new Queue<Token>(innerTokens), OperatorPrecedence.Minimum, variables, isWithinFunction);

                            currentToken = captiveTokens[captiveTokens.Length - 1];
                        }
                        else
                        {
                            rightHandSide = this.CompileExpression(tokens, precedence, variables, isWithinFunction);
                            // We are at the end of an expression so fake it up.
                            currentToken = new Token(")", -1);
                        }

                        leftHandSide = op.BuildExpression(previousToken, new[] { leftHandSide, rightHandSide }, this.context);
                    }
                    else
                    {
                        break;
                    }
                }
                else if(this.TryGetConstant(currentToken.CurrentToken, out var constant))
                {
                    CheckForExistingParticipant(leftHandSide, currentToken, isWithinFunction);

                    tokens.Dequeue();
                    leftHandSide = constant;
                }
                else if (this.context.TryGetFunction(currentToken.CurrentToken, out var function)) // or an IFunction?
                {
                    CheckForExistingParticipant(leftHandSide, currentToken, isWithinFunction);

                    var expressions = new List<IExpression>();
                    var captiveTokens = new Queue<Token>();
                    var parenCount = 0;
                    tokens.Dequeue();

                    // Loop through the list of tokens and split by ParameterSeparator character
                    while (tokens.Count > 0)
                    {
                        var nextToken = tokens.Dequeue();

                        if (string.Equals(nextToken.CurrentToken, "(", StringComparison.Ordinal))
                        {
                            parenCount++;
                        }
                        else if (string.Equals(nextToken.CurrentToken, ")", StringComparison.Ordinal))
                        {
                            parenCount--;
                        }

                        if (!(parenCount == 1 && nextToken.CurrentToken == "(") &&
                                !(parenCount == 0 && nextToken.CurrentToken == ")"))
                        {
                            captiveTokens.Enqueue(nextToken);
                        }

                        if (parenCount == 0 &&
                            captiveTokens.Any())
                        {
                            expressions.Add(this.CompileExpression(captiveTokens, minimumPrecedence: OperatorPrecedence.Minimum, variables: variables, isWithinFunction: true));
                            captiveTokens.Clear();
                        }
                        else if (string.Equals(nextToken.CurrentToken, Context.ParameterSeparator.ToString(), StringComparison.Ordinal) && parenCount == 1)
                        {
                            // TODO: Should we expect expressions to be null???
                            expressions.Add(this.CompileExpression(captiveTokens, minimumPrecedence: 0, variables: variables, isWithinFunction: true));
                            captiveTokens.Clear();
                        }

                        if (parenCount <= 0)
                        {
                            break;
                        }
                    }

                    leftHandSide = new FunctionExpression(currentToken.CurrentToken, function, expressions.ToArray());
                }
                else if (currentToken.CurrentToken.IsNumeric(this.context.DecimalCurrentCulture)) // Or a number
                {
                    CheckForExistingParticipant(leftHandSide, currentToken, isWithinFunction);

                    tokens.Dequeue();

                    if (int.TryParse(currentToken.CurrentToken, NumberStyles.Any, this.context.DecimalCurrentCulture, out var intValue))
                    {
                        leftHandSide = new ConstantValueExpression(intValue);
                    }
                    else if (decimal.TryParse(currentToken.CurrentToken, NumberStyles.Any, this.context.DecimalCurrentCulture, out var decimalValue))
                    {
                        leftHandSide = new ConstantValueExpression(decimalValue);
                    }
                    else if (double.TryParse(currentToken.CurrentToken, NumberStyles.Any, this.context.DecimalCurrentCulture, out var doubleValue))
                    {
                        leftHandSide = new ConstantValueExpression(doubleValue);
                    }
                    else if (float.TryParse(currentToken.CurrentToken, NumberStyles.Any, this.context.DecimalCurrentCulture, out var floatValue))
                    {
                        leftHandSide = new ConstantValueExpression(floatValue);
                    }
                    else if (long.TryParse(currentToken.CurrentToken, NumberStyles.Any, this.context.DecimalCurrentCulture, out var longValue))
                    {
                        leftHandSide = new ConstantValueExpression(longValue);
                    }
                }
                else if (currentToken.CurrentToken.StartsWith("[") && currentToken.CurrentToken.EndsWith("]")) // or a variable?
                {
                    tokens.Dequeue();
                    var variableName = currentToken.CurrentToken.Replace("[", "").Replace("]", "");


                    CheckForExistingParticipant(leftHandSide, currentToken, isWithinFunction);

                    leftHandSide = CreateVariableExpression(variableName, this.context);

                    if (!variables.Contains(variableName, this.context.ParsingStringComparer))
                    {
                        variables.Add(variableName);
                    }

                }
                else if (currentToken.CurrentToken.StartsWith("{") && currentToken.CurrentToken.EndsWith("}")) // or a variable?
                {
                    tokens.Dequeue();
                    var variableName = currentToken.CurrentToken.Replace("{", "").Replace("}", "");


                    CheckForExistingParticipant(leftHandSide, currentToken, isWithinFunction);

                    leftHandSide = CreateVariableExpression(variableName, this.context);

                    if (!variables.Contains(variableName, this.context.ParsingStringComparer))
                    {
                        variables.Add(variableName);
                    }

                }
                else if (currentToken.CurrentToken.StartsWith(Context.DateSeparator.ToString()) && currentToken.CurrentToken.EndsWith(Context.DateSeparator.ToString())) // or a date?
                {
                    CheckForExistingParticipant(leftHandSide, currentToken, isWithinFunction);

                    tokens.Dequeue();

                    var dateToken = currentToken.CurrentToken.Replace(Context.DateSeparator.ToString(), "");

                    // If we can't parse the date let's check for some known tags.
                    if (!DateTime.TryParse(dateToken, out var date))
                    {
                        if (string.Equals("TODAY", dateToken, StringComparison.OrdinalIgnoreCase))
                        {
                            date = DateTime.Today;
                        }
                        else if (string.Equals("NOW", dateToken, StringComparison.OrdinalIgnoreCase))
                        {
                            date = DateTime.Now;
                        }
                        else
                        {
                            throw new UnrecognisedTokenException(dateToken);
                        }
                    }

                    leftHandSide = new ConstantValueExpression(date);
                }
                else if ((currentToken.CurrentToken.StartsWith("'") && currentToken.CurrentToken.EndsWith("'")) ||
                    (currentToken.CurrentToken.StartsWith("\"") && currentToken.CurrentToken.EndsWith("\"")))
                {
                    CheckForExistingParticipant(leftHandSide, currentToken, isWithinFunction);

                    tokens.Dequeue();
                    leftHandSide = new ConstantValueExpression(CleanString(currentToken.CurrentToken.Substring(1, currentToken.Length - 2)));
                }
                else if (string.Equals(currentToken.CurrentToken, Context.ParameterSeparator.ToString(), StringComparison.Ordinal)) // Make sure we ignore the parameter separator
                {
                    if (!isWithinFunction)
                    {
                        throw new ExpressiveException("Unexpected parameter separator token '" + Context.ParameterSeparator + "' or unnrecognised outer function.");
                    }
                    tokens.Dequeue();
                }
                else
                {
                    tokens.Dequeue();

                    throw new UnrecognisedTokenException(currentToken.CurrentToken);
                }

                previousToken = currentToken;
                currentToken = tokens.PeekOrDefault();
            }

            return leftHandSide;
        }

        private bool TryGetConstant(string currentToken, out IExpression constant)
        {
            if(currentToken.Equals("true", this.context.ParsingStringComparison))
            {
                constant = new ConstantValueExpression(true);
            }
            else if(currentToken.Equals("false", this.context.ParsingStringComparison))
            {
                constant = new ConstantValueExpression(false);
            }
            else if (currentToken.Equals("null", this.context.ParsingStringComparison))
            {
                constant = new ConstantValueExpression(null);
            }
            else if (currentToken.Equals("pi", this.context.ParsingStringComparison))
            {
                constant = new ConstantValueExpression(Math.PI);
            }
            else if(currentToken.Equals("e", this.context.ParsingStringComparison))
            {
                constant = new ConstantValueExpression(Math.E);
            }
            else
            {
                constant = null;
            }

            return null != constant;
        }

        private static string CleanString(string input)
        {
            if (input.Length <= 1) { return input; }

            // the input string can only get shorter
            // so init the buffer so we won't have to reallocate later
            var buffer = new char[input.Length];
            var outIdx = 0;
            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (c == '\\')
                {
                    if (i < input.Length - 1)
                    {
                        switch (input[i + 1])
                        {
                            case 'n':
                                buffer[outIdx++] = '\n';
                                i++;
                                continue;
                            case 'r':
                                buffer[outIdx++] = '\r';
                                i++;
                                continue;
                            case 't':
                                buffer[outIdx++] = '\t';
                                i++;
                                continue;
                            case '\'':
                                buffer[outIdx++] = '\'';
                                i++;
                                continue;
                            case '\"':
                                buffer[outIdx++] = '\"';
                                i++;
                                continue;
                            case '\\':
                                buffer[outIdx++] = '\\';
                                i++;
                                continue;
                        }
                    }
                }

                buffer[outIdx++] = c;
            }

            return new string(buffer, 0, outIdx);
        }

        protected virtual IExpression CreateVariableExpression(string token, Context context)
        {
            return new VariableExpression(token);
        }

        

        private static void CheckForExistingParticipant(IExpression participant, Token token, bool isWithinFunction)
        {
            if (participant != null)
            {
                if (isWithinFunction)
                {
                    throw new MissingTokenException("Missing token, expecting ','.", ',');
                }
                
                throw new ExpressiveException($"Unexpected token '{token.CurrentToken}' at index {token.StartIndex}");
            }
        }

#endregion
    }
}
