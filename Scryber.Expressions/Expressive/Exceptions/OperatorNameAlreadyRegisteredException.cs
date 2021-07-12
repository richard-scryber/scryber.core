using System;
using Scryber.Expressive.Operators;

#if NET45
using System.Runtime.Serialization;
using System.Security.Permissions;
#endif

namespace Scryber.Expressive.Exceptions
{
    /// <summary>
    /// Represents an error that is thrown when registering an <see cref="IOperator"/> and the name is already used.
    /// </summary>
#if NET45
    [Serializable]
#endif
    public sealed class OperatorNameAlreadyRegisteredException : Exception
    {
        /// <summary>
        /// Gets the tag of the operator already used.
        /// </summary>
        public string Tag { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNameAlreadyRegisteredException"/> class.
        /// </summary>
        /// <param name="tag">The tag of the operator.</param>
        internal OperatorNameAlreadyRegisteredException(string tag)
            : base($"An operator has already been registered '{tag}'")
        {
            this.Tag = tag;
        }

#if NET45
        /// <summary>
        /// Set the <see cref="SerializationInfo"/> with information about this exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Tag", Tag);
        }
#endif
    }
}
