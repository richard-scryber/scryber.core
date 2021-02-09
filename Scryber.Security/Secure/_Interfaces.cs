using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using Scryber.Secure.Configuration;

namespace Scryber.Secure
{
    /// <summary>
    /// Interface that implementers can use to load document owner and user passwords for specific paths (or other criteria).
    /// </summary>
    public interface IPDFSecurePasswordProvider
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
        /// Gets the path these settings were set up for.
        /// </summary>
        public string DocumentPath { get; }

        /// <summary>
        /// Gets the Owner Password for the document. 
        /// If not set, then it must be set in code before a secure document is output
        /// </summary>
        public SecureString OwnerPassword { get; }

        /// <summary>
        /// Gets the User Password for the document. Can be null
        /// </summary>
        public SecureString UserPassword { get; }

        /// <summary>
        /// Gets the default base set of permissions for this document
        /// </summary>
        public PermissionFlags DefaultPermissions { get; }

        /// <summary>
        /// If true then the set of permissions can be adjusted before the document is output. 
        /// If false then any attempt to change the configured values will raise an exception.
        /// </summary>
        public bool AllowDocumentOverrides { get; }

    }
    
}
