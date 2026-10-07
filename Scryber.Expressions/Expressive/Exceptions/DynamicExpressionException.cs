using System;

namespace Scryber.Expressive.Exceptions
{
    /// <summary>
    /// Thrown when an expression that is supplied as DATA (e.g. eval(.expr)) cannot be compiled or evaluated.
    /// It is a problem with the data, not with the template, so it is not treated as invalid template syntax
    /// by <see cref="ExpressionErrors.IsSyntaxError(Exception)"/>, even though it wraps the original syntax error.
    /// </summary>
    public class DynamicExpressionException : Exception
    {
        public DynamicExpressionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
