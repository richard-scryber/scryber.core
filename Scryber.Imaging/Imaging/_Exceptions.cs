using System;
namespace Scryber.Imaging
{

    /// <summary>
    /// Exception raised when an image format is not supported by the scryber imaging library
    /// </summary>
    [Serializable]
    public class PDFImageFormatException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:PDFImageFormatException"/> class
        /// </summary>
        public PDFImageFormatException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PDFImageFormatException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        public PDFImageFormatException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PDFImageFormatException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public PDFImageFormatException(string message, System.Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PDFImageFormatException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected PDFImageFormatException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
