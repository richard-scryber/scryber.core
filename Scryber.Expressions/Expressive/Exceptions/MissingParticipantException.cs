using System;

namespace Scryber.Expressive.Exceptions
{
    /// <summary>
    /// Represents an error that is thrown when one side of an operation is missing inside an <see cref="Expression"/>.
    /// </summary>
#if NET45
    [Serializable]
#endif
    public sealed class MissingParticipantException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingParticipantException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        internal MissingParticipantException(string message) : base(message)
        {

        }
    }
}
