using System;

namespace Scryber.Expressive.Exceptions
{
    /// <summary>
    /// The main exposed <see cref="Exception"/> for users of an Expression. Check the InnerException for more information.
    /// </summary>
#if NET45
    [Serializable]
#endif
    public sealed class ExpressiveException : Exception
    {
        public ExpressiveException(string message) : base(message)
        {

        }

        public ExpressiveException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public ExpressiveException(Exception innerException) : base(innerException.Message, innerException)
        {

        }
    }
}
