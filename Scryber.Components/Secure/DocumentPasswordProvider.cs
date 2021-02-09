using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

namespace Scryber.Secure
{
    public class DocumentPasswordProvider : IPDFSecurePasswordProvider
    {

        /// <summary>
        /// Gets the Owner Password for the document. This is required to encrypt a document
        /// </summary>
        public SecureString OwnerPassword { get; set; }

        /// <summary>
        /// Gets the User Password for the document. Can be null
        /// </summary>
        public SecureString UserPassword { get; set; }

        

        public DocumentPasswordProvider()
            : this(null, null)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        public DocumentPasswordProvider(string owner)
            : this(GetSecureString(owner), null)
        {

        }

        /// <summary>
        /// Initializes the document passwords with 1 owner password.
        /// The user will not have to provide a password to open the document, but will have to provide a password to perform any
        /// of the restricted actions (e.g. print)
        /// </summary>
        /// <param name="owner">The owner password as a secure string</param>
        public DocumentPasswordProvider(SecureString owner)
            : this(owner, null)
        {

        }

        public DocumentPasswordProvider(SecureString owner, SecureString user)
        {
            this.OwnerPassword = owner;
            this.UserPassword = user;
        }

        public static SecureString GetSecureString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            SecureString all = new SecureString();
            foreach (char c in value)
            {
                all.AppendChar(c);
            }

            return all;
        }

        public bool IsSecure(string documentpath, out IDocumentPasswordSettings settings)
        {
            settings = new SecureDocumentPasswordSettings(this.OwnerPassword, this.UserPassword);
            return true;
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

                this.OwnerPassword = null;
                this.UserPassword = null;
            }
        }

        ~DocumentPasswordProvider()
        {
            this.Dispose(false);
        }

    }
}
