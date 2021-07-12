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
        internal ExpressiveException(string message) : base(message)
        {

        }

        internal ExpressiveException(Exception innerException) : base(innerException.Message, innerException)
        {

        }
    }
}
