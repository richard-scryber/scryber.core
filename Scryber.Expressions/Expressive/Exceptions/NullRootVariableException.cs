using System;

namespace Scryber.Expressive.Exceptions
{
    /// <summary>
    /// Thrown when a property is read from the ROOT of an expression path (e.g. 'model' in model.missing.value)
    /// and that root variable has not been set, or is null. A null value part way along a path
    /// (e.g. 'missing' in model.missing.value) is not an error and evaluates to null.
    /// Inherits ArgumentNullException so existing handlers for a null parent continue to work.
    /// </summary>
    public class NullRootVariableException : ArgumentNullException
    {
        public string PropertyName { get; }

        public NullRootVariableException(string propertyName)
            : base("parent", "The root variable of the expression has not been set or is null, so the property '" + propertyName + "' could not be read")
        {
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Returns true if the exception, or any exception it wraps (e.g. an ExpressiveException), is a NullRootVariableException.
        /// </summary>
        public static bool IsCausedBy(Exception ex)
        {
            while (ex != null)
            {
                if (ex is NullRootVariableException)
                    return true;
                ex = ex.InnerException;
            }
            return false;
        }
    }
}
