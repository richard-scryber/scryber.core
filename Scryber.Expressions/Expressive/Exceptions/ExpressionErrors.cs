using System;

namespace Scryber.Expressive.Exceptions
{
    /// <summary>
    /// Helpers for classifying the exceptions raised when an expression is compiled or evaluated.
    /// </summary>
    public static class ExpressionErrors
    {
        /// <summary>
        /// Returns true if the exception, or any exception it wraps (e.g. an ExpressiveException), shows the expression
        /// itself is not valid - a missing operand, missing or unrecognised token, or the wrong number of function parameters.
        /// Some of these (e.g. a missing operand in 'model.items[') are only discovered when the expression is evaluated,
        /// but they are errors in the template, not in the data, and should always be raised.
        /// </summary>
        public static bool IsSyntaxError(Exception ex)
        {
            while (ex != null)
            {
                //An expression supplied as data is not a template syntax error, whatever it wraps.
                if (ex is DynamicExpressionException)
                    return false;

                if (ex is MissingParticipantException
                    || ex is ParameterCountMismatchException
                    || ex is MissingTokenException
                    || ex is UnrecognisedTokenException)
                    return true;

                ex = ex.InnerException;
            }
            return false;
        }
    }
}
