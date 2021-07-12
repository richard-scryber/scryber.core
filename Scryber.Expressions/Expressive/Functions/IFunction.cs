using Scryber.Expressive.Expressions;
using System.Collections.Generic;

namespace Scryber.Expressive.Functions
{
    /// <summary>
    /// Interface definition for a Function that can be evaluated.
    /// </summary>
    public interface IFunction
    {
        /// <summary>
        /// Gets or sets the Variables and their values to be used in evaluating an <see cref="Expression"/>.
        /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only - it is likely this can be passed in to Evaluate but it will need to be done carefully (e.g. mark this setter as obsolete first).
        IDictionary<string, object> Variables { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

        /// <summary>
        /// Gets the name of the Function.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Forces the Function to evaluate itself using the supplied parameters.
        /// </summary>
        /// <param name="parameters">The list of parameters inside the Function.</param>
        /// <param name="context">The evaluation context to be used.</param>
        /// <returns>The result of the Function.</returns>
        object Evaluate(IExpression[] parameters, Context context);
    }
}
