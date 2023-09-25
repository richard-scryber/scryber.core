﻿//#define OLD
//Copyright(c) 2019 Shaun Lawrence

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NON INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using Scryber.Expressive.Exceptions;
using Scryber.Expressive.Expressions;
using Scryber.Expressive.Functions;
using Scryber.Expressive.Operators;
using Scryber.Expressive.Tokenisation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Scryber.Expressive
{
    /// <summary>
    /// Class definition for an Expression that can be evaluated.
    /// </summary>
    public class Expression : IExpression
    {
        #region Fields

        private IExpression compiledExpression;
        private readonly Context context;
        private readonly string originalExpression;
        private readonly ExpressionParser parser;
        private string[] referencedVariables;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list of the Variable names that are contained within this Expression.
        /// </summary>
        public IReadOnlyCollection<string> ReferencedVariables
        {
            get
            {
                this.CompileExpression();

                return this.referencedVariables;
            }
        }

        /// <summary>
        /// Gets the current context for the expression evaluation
        /// </summary>
        public Context ExpressionContext
        {
            get { return this.context; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Expression"/> class with the specified <paramref name="context"/>.
        /// </summary>
        /// <param name="expression">The expression to be evaluated.</param>
        /// <param name="context">The <see cref="Context"/> to use when evaluating.</param>
        public Expression(string expression, Context context)
            : this(expression, new ExpressionParser(context, new Tokeniser(context)), context)
        {

        }


        public Expression(string expression, ExpressionParser parser, Context context)
        {
            this.originalExpression = expression;
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }
        #endregion

        #region Public Methods

        const int SignificantDigits = 10;

        /// <summary>
        /// Evaluates the expression using the supplied <paramref name="variables"/> and returns the result.
        /// </summary>
        /// <exception cref="Exceptions.ExpressiveException">Thrown when there is a break in the evaluation process, check the InnerException for further information.</exception>
        /// <param name="variables">The variables to be used in the evaluation.</param>
        /// <returns>The result of the evaluation.</returns>
        public object Evaluate(IDictionary<string, object> variables = null)
        {
            try
            {
                this.CompileExpression();

                var result = this.compiledExpression?.Evaluate(ApplyStringComparerSettings(variables, this.context.ParsingStringComparer));
                if(result is double)
                {
                    result = Math.Round((double)result, SignificantDigits);
                }
                else if(result is decimal)
                {
                    result = Math.Round((decimal)result, SignificantDigits);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new ExpressiveException(ex);
            }
        }

        /// <summary>
        /// Evaluates the expression using the supplied <paramref name="variables"/> and returns the result.
        /// </summary>
        /// <exception cref="Exceptions.ExpressiveException">Thrown when there is a break in the evaluation process, check the InnerException for further information.</exception>
        /// <param name="variables">The variables to be used in the evaluation.</param>
        /// <returns>The result of the evaluation.</returns>
        public T Evaluate<T>(IDictionary<string, object> variables = null)
        {
            try
            {
                object value = this.Evaluate(variables);
                if (null == value)
                    return default;
                else if (value is T t)
                    return t;
                else
                    return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (ExpressiveException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ExpressiveException(ex);
            }
        }

        /// <summary>
        /// Evaluates the expression using the supplied <paramref name="variableProvider"/> and returns the result.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="variableProvider"/> is null.</exception>
        /// <exception cref="Exceptions.ExpressiveException">Thrown when there is a break in the evaluation process, check the InnerException for further information.</exception>
        /// <param name="variableProvider">The <see cref="IVariableProvider"/> implementation to provide variable values during evaluation.</param>
        /// <returns>The result of the evaluation.</returns>
        public object Evaluate(IVariableProvider variableProvider)
        {
            if (variableProvider is null)
            {
                throw new ArgumentNullException(nameof(variableProvider));
            }

            return this.Evaluate(new VariableProviderDictionary(variableProvider));
        }

        /// <summary>
        /// Evaluates the expression using the supplied <paramref name="variableProvider"/> and returns the result.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="variableProvider"/> is null.</exception>
        /// <exception cref="Exceptions.ExpressiveException">Thrown when there is a break in the evaluation process, check the InnerException for further information.</exception>
        /// <param name="variableProvider">The <see cref="IVariableProvider"/> implementation to provide variable values during evaluation.</param>
        /// <returns>The result of the evaluation.</returns>
        public T Evaluate<T>(IVariableProvider variableProvider)
        {
            if (variableProvider is null)
            {
                throw new ArgumentNullException(nameof(variableProvider));
            }

            try
            {
                return (T)this.Evaluate(variableProvider);
            }
            catch (ExpressiveException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ExpressiveException(ex);
            }
        }

        /// <summary>
        /// Evaluates the expression using the supplied variables asynchronously and returns the result via the callback.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown if the callback is not supplied.</exception>
        /// <param name="callback">Provides the result once the evaluation has completed.</param>
        /// <param name="variables">The variables to be used in the evaluation.</param>
        public void EvaluateAsync(Action<string, object> callback, IDictionary<string, object> variables = null)
        {
            this.EvaluateAsync<object>(callback, variables);
        }

        /// <summary>
        /// Evaluates the expression using the supplied variables asynchronously and returns the result via the callback.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown if the callback is not supplied.</exception>
        /// <param name="callback">Provides the result once the evaluation has completed.</param>
        /// <param name="variables">The variables to be used in the evaluation.</param>
        public void EvaluateAsync<T>(Action<string, T> callback, IDictionary<string, object> variables = null)
        {
            if (callback is null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

#if NETSTANDARD1_4
            Task.Run(() =>
#else
            ThreadPool.QueueUserWorkItem((o) =>
#endif
            {
                var result = default(T);
                string message = null;

                try
                {
                    result = this.Evaluate<T>(variables);
                }
                catch (ExpressiveException ex)
                {
                    message = ex.Message;
                }

                callback.Invoke(message, result);
            });
        }

        

#endregion

        

        public void CompileExpression(bool force = false)
        {
            if (!force)
            {
                // Cache the expression to save us having to recompile.
                if (!(this.compiledExpression is null) && !this.context.Options.HasFlag(ExpressiveOptions.NoCache))
                {
                    return;
                }
            }

            var variables = new List<string>();

            this.compiledExpression = this.parser.CompileExpression(this.originalExpression, variables);

            this.referencedVariables = variables.ToArray();
        }

#region Private Methods

        private static IDictionary<string, object> ApplyStringComparerSettings(IDictionary<string, object> variables, IEqualityComparer<string> desiredStringComparer)
        {
            switch (variables)
            {
                case null:
                    return null;
                case Dictionary<string, object> dictionary when dictionary.Comparer.Equals(desiredStringComparer):
                    return dictionary;
                case VariableProviderDictionary _:
                    return variables;
                default:
                    return new Dictionary<string, object>(variables, desiredStringComparer);
            }
        }

#endregion

#region object overrides

        public override string ToString()
        {
            return "{" + this.originalExpression + "}";
        }

#endregion
    }
}
