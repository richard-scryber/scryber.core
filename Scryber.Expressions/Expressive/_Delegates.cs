using System.Collections.Generic;
using Scryber.Expressive.Expressions;

namespace Scryber.Expressive
{
    
    /// <summary>
    /// Performs the execution of a function to return the resulkt based on parameters and variables.
    /// All Declared functions to be used by the expressive library must have a matching method signature.
    /// </summary>
    public delegate object ExecFunction(IExpression[] parameters, IDictionary<string, object> variables, ExpressionContext context);

}