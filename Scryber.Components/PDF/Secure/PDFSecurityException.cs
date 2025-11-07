using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber.PDF.Secure
{
    /// <summary>
    /// Raised if there is an error or issue with the security of the application
    /// </summary>
    [Serializable]
    public class PDFSecurityException : PDFException
    {
        public PDFSecurityException() { }

        public PDFSecurityException(string message) : base(message) { }

        public PDFSecurityException(string message, Exception inner) : base(message, inner) { }

        
    }
}
