using System;
using Scryber.Expressive.Exceptions;
using Scryber.Expressive.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace Scryber.Expressive.Functions
{
    /// <summary>
    /// Base class implementation for providing a function that can be registered and evaluated.
    /// </summary>
    public abstract class FunctionBase : IFunction
    {
        #region IFunction Members

        /// <inheritdoc />
#pragma warning disable CA2227 // Collection properties should be read only - it is likely this can be passed in to Evaluate but it will need to be done carefully (e.g. mark this setter as obsolete first).
        public IDictionary<string, object> Variables { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

        /// <inheritdoc />
        public abstract string Name { get; }

        /// <inheritdoc />
        public abstract object Evaluate(IExpression[] parameters, Context context);

        #endregion

        /// <summary>
        /// Validates whether the expected number of parameters are present.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="expectedCount">The expected number of parameters, use -1 for an unknown number.</param>
        /// <param name="minimumCount">The minimum number of parameters.</param>
        protected void ValidateParameterCount(IExpression[] parameters, int expectedCount, int minimumCount)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (expectedCount == 0 && (parameters.Any() || parameters.Length != expectedCount))
            {
                throw new ParameterCountMismatchException($"{this.Name}() does not take any arguments");
            }

            if (expectedCount > 0 && (!parameters.Any() || parameters.Length != expectedCount))
            {
                throw new ParameterCountMismatchException($"{this.Name}() takes only {expectedCount} argument(s)");
            }

            if (minimumCount > 0 && (!parameters.Any() || parameters.Length < minimumCount))
            {
                throw new ParameterCountMismatchException($"{this.Name}() expects at least {minimumCount} argument(s)");
            }
        }
    }
}
