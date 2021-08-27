using System;
using System.Collections.Generic;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive.Functions.CSS
{
    /// <summary>
    /// Implements the Var(name,default) expressive function
    /// </summary>
    public class VarFunction : FunctionBase
    {

        /// <summary>
        /// Gets the name of the function  - Var
        /// </summary>
        public override string Name
        {
            get { return "Var"; }
        }

        /// <summary>
        /// Creates a new instance of the var function
        /// </summary>
        public VarFunction()
        {
        }

        /// <summary>
        /// Evaluates the parameters in a Var expression returning either a parameter value, or the default if available.
        /// </summary>
        /// <param name="parameters">The expression parameters</param>
        /// <param name="context">The current expression context</param>
        /// <returns>The value of the parameter, or the default (second parameter) or null</returns>
        public override object Evaluate(IExpression[] parameters, IDictionary<string, object> variables, Context context)
        {
            this.ValidateParameterCount(parameters, -1, 1);
            object value = parameters[0].Evaluate(variables);

            //Allow the pass through of an optional second parameter with a fallback value

            if (null == value && parameters.Length > 1)
                value = parameters[1].Evaluate(variables);

            return value;
        }
    }
}
