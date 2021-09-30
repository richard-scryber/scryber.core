using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

namespace Scryber.PDF.Secure
{
    public class DocumentPasswordProvider : IPDFSecurePasswordProvider
    {

        /// <summary>
        /// Gets the Owner Password for the document. This is required to encrypt a document, and perform all restricted actions.
        /// </summary>
        public SecureString OwnerPassword { get; set; }

        /// <summary>
        /// Gets the User Password for the document. This is used for opening the document. Can be null
        /// </summary>
        public SecureString UserPassword { get; set; }

        
        /// <summary>
        /// Creates a new empty instance.
        /// The owner password, as a minimum, must be set later, for this instance to be of any value.
        /// </summary>
        public DocumentPasswordProvider()
            : this(String.Empty, String.Empty)
        {
        }

        /// <summary>
        /// INSECURE - setting the owner password from a string. The document can be opened,
        /// but as an end user, to perform any of the restricted functions this password will need to be provided.
        /// </summary>
        /// <param name="owner"></param>
        public DocumentPasswordProvider(string owner)
            : this(GetSecureString(owner), null)
        {

        }

        /// <summary>
        /// INSECURE - setting the owner and user password from a string. The document **cannot** be opened
        /// without entering the user password. If this is the same as the owner password all other functions
        /// can then be performed.
        /// </summary>
        /// <param name="owner">The password to unlock any restrictions.</param>
        /// <param name="user">The password to open the document</param>
        public DocumentPasswordProvider(string owner, string user)
            : this(GetSecureString(owner), GetSecureString(user))
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

        /// <summary>
        /// Main constructor that initialaze the instance with its secure owner and user password.
        /// </summary>
        /// <param name="owner">The owner password to force the restrictions to a document</param>
        /// <param name="user">The optional user password that if specified will require the end user to enter a password to open the document</param>
        public DocumentPasswordProvider(SecureString owner, SecureString user)
        {
            this.OwnerPassword = owner;
            this.UserPassword = user;
        }




        /// <summary>
        /// IPDFSecurePasswordProvider interface implementation for the IsSecure method.
        /// </summary>
        /// <param name="documentpath">The path to the document</param>
        /// <param name="settings">Set to the passwords for the document if IsSecure returns true</param>
        /// <returns>True if the document should be secured</returns>
        public virtual bool IsSecure(string documentpath, out IDocumentPasswordSettings settings)
        {
            settings = new SecureDocumentPasswordSettings(this.OwnerPassword, this.UserPassword);
            return true;
        }


        /// <summary>
        /// IDisposable implementation
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Internal implementation of the object disposal, to release any unmanaged resources.
        /// May be called multiple times.
        /// </summary>
        /// <param name="disposing">True if this is an explicit dispose</param>
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                if (null != this.OwnerPassword)
                    this.OwnerPassword.Dispose();
                if (null != this.UserPassword)
                    this.UserPassword.Dispose();
            }
            this.OwnerPassword = null;
            this.UserPassword = null;
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~DocumentPasswordProvider()
        {
            this.Dispose(false);
        }


        /// <summary>
        /// Converts a string into a secure string
        /// </summary>
        /// <param name="value">The string to convert. Returns null if the string is null or empty</param>
        /// <returns>A secured string of all the characters</returns>
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

    }
}
