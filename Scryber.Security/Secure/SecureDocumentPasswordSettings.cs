using System;
using System.Security;

namespace Scryber.Secure
{
    public class SecureDocumentPasswordSettings : IDocumentPasswordSettings
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

        /// <summary>
        /// Gets the level of encryption
        /// </summary>
        public SecurityType SecurityType { get; private set; }

        public SecureDocumentPasswordSettings(string path, SecureString owner, SecureString user, PermissionFlags perms, bool overridable, SecurityType securityType)
        {
            this.DocumentPath = path;
            this.OwnerPassword = owner;
            this.UserPassword = user;
            this.DefaultPermissions = perms;
            this.AllowDocumentOverrides = overridable;
            this.SecurityType = securityType;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                if (null != this.OwnerPassword)
                    this.OwnerPassword.Dispose();
                if (null != this.UserPassword)
                    this.UserPassword.Dispose();
            }
        }
    }
}
