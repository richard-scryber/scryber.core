using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

namespace Scryber.Secure.Configuration
{
    /// <summary>
    /// Returned to the PDFSecureDocument from the ISecurePasswordProvider based on the current configuration.
    /// Using this documents can be output with specific security settings.
    /// </summary>
    public class PDFDocumentPasswordSettings : IDocumentPasswordSettings
    {
        /// <summary>
        /// Gets the path these settings were set up for.
        /// </summary>
        public string DocumentPath { get; private set; }

        /// <summary>
        /// Gets the Owner Password for the document. 
        /// If not set, then it must be set in code before a secure document is output
        /// </summary>
        public SecureString OwnerPassword { get; private set; }

        /// <summary>
        /// Gets the User Password for the document. Can be null
        /// </summary>
        public SecureString UserPassword { get; private set; }

        /// <summary>
        /// Gets the default base set of permissions for this document
        /// </summary>
        public PermissionFlags DefaultPermissions { get; private set; }

        /// <summary>
        /// If true then the set of permissions can be adjusted before the document is output. 
        /// If false then any attempt to change the configured values will raise an exception.
        /// </summary>
        public bool AllowDocumentOverrides { get; private set; }

        //
        // ctor
        //

        /// <summary>
        /// Creates a new DocumentPasswordSettings
        /// </summary>
        /// <param name="path"></param>
        /// <param name="owner"></param>
        /// <param name="user"></param>
        /// <param name="perms"></param>
        /// <param name="overridable"></param>
        public PDFDocumentPasswordSettings(string path, SecureString owner, SecureString user, PermissionFlags perms, bool overridable)
        {
            this.DocumentPath = path;
            this.OwnerPassword = owner;
            this.UserPassword = user;
            this.DefaultPermissions = perms;
            this.AllowDocumentOverrides = overridable;
        }



        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != this.OwnerPassword)
                    this.OwnerPassword.Dispose();
                if (null != this.UserPassword)
                    this.UserPassword.Dispose();
            }
        }
    }
}
