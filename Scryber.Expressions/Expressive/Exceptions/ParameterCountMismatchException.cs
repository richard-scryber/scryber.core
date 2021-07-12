using System;

namespace Scryber.Expressive.Exceptions
{
    /// <summary>
    /// Represents an error that is thrown when a function has an incorrect number of parameters.
    /// </summary>
#if NET45
    [Serializable]
#endif
    public sealed class ParameterCountMismatchException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterCountMismatchException"/> class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        internal ParameterCountMismatchException(string message)
            : base(message)
        {

        }
    }
}
