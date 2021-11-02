using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

namespace Scryber.PDF.Secure
{
    /// <summary>
    /// Interface that implementers can use to load document owner and user passwords for specific paths (or other criteria).
    /// </summary>
    public interface IPDFSecurePasswordProvider : IDisposable
    {
        /// <summary>
        /// Implementers should use this method to load specific security settings pased on a specific path and return required values 
        /// </summary>
        /// <param name="documentpath">The source path the document was loaded from</param>
        /// <param name="settings">Set to the security settings to be associated with this document if IsSecure returns true.</param>
        /// <returns>Return true to assign security settings, or false to not.</returns>
        bool IsSecure(string documentpath, out IDocumentPasswordSettings settings);

    }


    public interface IDocumentPasswordSettings : IDisposable
    {
        /// <summary>
        /// Gets the Owner Password for the document. 
        /// If not set, then it must be set in code before a secure document is output
        /// </summary>
        SecureString OwnerPassword { get; }

        /// <summary>
        /// Gets the User Password for the document. If null then NO password is required to open and view the document
        /// </summary>
        SecureString UserPassword { get; }

    }
    
}
