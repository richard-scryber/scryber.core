using System;
using System.Security;

namespace Scryber.PDF.Secure
{
    /// <summary>
    /// Keeps a copy of the user name and password. Should be disposed of after use.
    /// </summary>
    internal class SecureDocumentPasswordSettings : IDocumentPasswordSettings
    {
        /// <summary>
        /// Gets the Owner Password for the document. 
        /// If not set, then it must be set in code before a secure document is output
        /// </summary>
        public SecureString OwnerPassword { get; private set; }

        /// <summary>
        /// Gets the User Password for the document. Can be null
        /// </summary>
        public SecureString UserPassword { get; private set; }


        
        public SecureDocumentPasswordSettings(SecureString owner, SecureString user)
        {
            this.OwnerPassword = owner;
            this.UserPassword = user;
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
