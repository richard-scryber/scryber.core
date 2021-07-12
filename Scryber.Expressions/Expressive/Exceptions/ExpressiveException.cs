using System;

namespace Scryber.Expressive.Exceptions
{
    /// <summary>
    /// The main exposed <see cref="Exception"/> for users of an Expression. Check the InnerException for more information.
    /// </summary>
#if NET45
    [Serializable]
#endif
    public sealed class ScryberException : Exception
    {
        internal ScryberException(string message) : base(message)
        {

        }

        internal ScryberException(Exception innerException) : base(innerException.Message, innerException)
        {

        }
    }
}
