using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using System.Security;

namespace Scryber.Secure.Components
{
    [PDFParsableComponent("Writer")]
    public class PDFSecureWriterFactory : PDFWriterFactory
    {

        public const int DefaultEncryptionVersion = 1;
        public const int DefaultEncryptionRevision = 2;

        //properties

        /// <summary>
        /// Gets or sets the standard security type for these document permissions. This will alter the Version and Revision properties accordingly
        /// </summary>
        [PDFAttribute("type")]
        public SecurityType Type
        {
            get
            {
                return this.DeriveSecurityType();
            }
            set
            {
                this.ApplySecurityType(value);
            }
        }


        /// <summary>
        /// Gets the version of the documents security. This is based on the Security Type
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// Gets the revision of the documents security. This is based on the Security Type
        /// </summary>
        public int Revision { get; private set; }

        //
        // restriction properties (v1.2)
        //

        [PDFAttribute("allow-printing")]
        public bool AllowPrinting { get; set; }

        [PDFAttribute("allow-modification")]
        public bool AllowModification { get; set; }

        [PDFAttribute("allow-copying")]
        public bool AllowCopying { get; set; }

        [PDFAttribute("allow-annotations")]
        public bool AllowAnnotations { get; set; }

        //
        // Revision 2.3 properties
        //

        [PDFAttribute("allow-forms")]
        public bool AllowFormFilling { get; set; }

        [PDFAttribute("allow-accessibility")]
        public bool AllowAccessiblity { get; set; }

        [PDFAttribute("allow-document-assembly")]
        public bool AllowDocumentAssembly { get; set; }

        [PDFAttribute("allow-hq-printing")]
        public bool AllowHighQualityPrinting { get; set; }


        #region public SecureString OwnerPassword {get;set;}

        /// <summary>
        /// Gets the owner password for this document. This is a required value, and must be set before rendering.
        /// </summary>
        /// <remarks>It is also possible to use the SetOwnerPassword method to populate this value with a char[] that will be 
        /// cleared after use for reasons of security. It is up to you how this char[] is populated</remarks>
        public SecureString OwnerPassword
        {
            get;
            set;
        }

        #endregion

        #region public string OwnerPasswordUnsecure {get;set;}

        private string _ownerunsecure;

        /// <summary>
        /// Gets or sets the owner password for the document, from a string value.
        /// This is not best practice, or the most secure, as the string will be in memory on the server / machine, and could be inspected
        /// </summary>
        [PDFAttribute("owner-password")]
        public string OwnerPasswordUnsecure
        {
            get
            {
                return _ownerunsecure;
            }
            set
            {
                _ownerunsecure = value;
                if (string.IsNullOrEmpty(_ownerunsecure))
                    this.SetOwnerPassword(null);
                else
                    this.SetOwnerPassword(_ownerunsecure.ToCharArray());
            }
        }

        #endregion


        #region public SecureString UserPassword {get;set;}

        /// <summary>
        /// Gets the user password for this document (optional)
        /// </summary>
        /// <remarks>It is also possible to use the SetUserPassword method to populate this value with a char[] that will be 
        /// cleared after use for reasons of security. It is up to you how this char[] is populated</remarks>
        public SecureString UserPassword
        {
            get;
            set;
        }

        #endregion

        #region public string UserPasswordUnsecure {get;set;}

        private string _userunsecure;

        /// <summary>
        /// Gets or sets the user password for the document, from a string value.
        /// This is not best practice, or the most secure, as the string will be in memory on the server / machine, and could be inspected
        /// </summary>
        [PDFAttribute("user-password")]
        public string UserPasswordUnsecure
        {
            get
            {
                return _userunsecure;
            }
            set
            {
                _userunsecure = value;
                if (string.IsNullOrEmpty(_userunsecure))
                    this.SetUserPassword(null);
                else
                    this.SetUserPassword(_userunsecure.ToCharArray());
            }
        }

        #endregion

        public PDFSecureWriterFactory()
        {
            this.Revision = DefaultEncryptionRevision;
            this.Version = DefaultEncryptionVersion;
        }

        //public methods

        #region public void SetOwnerPassword(char[] password)

        /// <summary>
        /// Sets the owner password to the specified value, and then clears the characters on the array
        /// </summary>
        /// <param name="password"></param>
        public void SetOwnerPassword(char[] password)
        {
            if (null != this.OwnerPassword)
                this.OwnerPassword.Dispose();
            this.OwnerPassword = null;

            if (password != null && password.Length > 0)
            {
                SecureString str = new SecureString();
                PopulateAndClearPassword(password, str);
                this.OwnerPassword = str;
            }
        }

        #endregion

        #region public void SetUserPassword(char[] password)

        /// <summary>
        /// Sets the user password to the specified value, and then clears the characters in the array.
        /// </summary>
        /// <param name="password"></param>
        public void SetUserPassword(char[] password)
        {
            if (null != this.UserPassword)
                this.UserPassword.Dispose();
            this.UserPassword = null;

            if (password != null || password.Length > 0)
            {
                SecureString str = new SecureString();
                PopulateAndClearPassword(password, str);
                this.UserPassword = str;
            }
        }

        #endregion

        //implementation methods

        protected override PDFWriter DoGetInstance(Document forDoc, Stream stream, int generation, PDFDocumentRenderOptions options, PDFTraceLog log)
        {
            
            if (null == forDoc.DocumentID)
                forDoc.DocumentID = PDFDocumentID.Create();

            PDFEncrypterFactory factory = this.GetEncrypterFactory();

            IDocumentPasswordSettings settings;
            string path = forDoc.LoadedSource;
            SecureString owner;
            SecureString user;
            PermissionFlags protection;

            if (Options.SecurityOptions.TryGetPasswordSettings(path, out settings))
            {
                owner = settings.OwnerPassword;
                user = settings.UserPassword;
                protection = settings.DefaultPermissions;

                if (settings.AllowDocumentOverrides)
                {
                    if (null != this.OwnerPassword)
                        owner = this.OwnerPassword;
                    if (null != this.UserPassword)
                        user = this.UserPassword;
                    protection |= this.GetRestrictions();
                }
            }
            else
            {
                protection = this.GetRestrictions();
                owner = this.OwnerPassword;
                user = this.UserPassword;
            }

            PDFEncryter enc = factory.InitEncrypter(owner, user, forDoc.DocumentID, protection);
            PDFSecureWriter14 writer = new PDFSecureWriter14(stream, log, enc);
            return writer;
        }

        internal virtual PDFEncrypterFactory GetEncrypterFactory()
        {
            PDFEncrypterFactory factory = PDFEncrypterFactory.CreateFactory(this.Version, this.Revision);
            if (null == factory)
                throw new PDFSecurityException(string.Format("There is no available encryptor for the version and revision " + this.Version + "." + this.Revision));
            return factory;
        }

        internal PermissionFlags GetRestrictions()
        {
            SecurityType type = this.Type;
            if (type == SecurityType.Standard40Bit)
                return GetRestrictionsV2();
            else if (type == SecurityType.Standard128Bit)
                return GetRestrictionsV3();
            else
                return GetOtherRestrictions();
        }

        private SecurityType DeriveSecurityType()
        {
            if (this.Version == 1 && this.Revision == 2)
                return SecurityType.Standard40Bit;
            else if (this.Version == 2 && this.Revision == 3)
                return SecurityType.Standard128Bit;
            else
                return SecurityType.Other;
        }

        private void ApplySecurityType(SecurityType type)
        {
            if (type == SecurityType.Standard128Bit)
            {
                this.Version = 2;
                this.Revision = 3;
            }
            else if (type == SecurityType.Standard40Bit)
            {
                this.Version = 1;
                this.Revision = 2;
            }
        }

        /// <summary>
        /// Returns the restrictions for a non-standard version. Default returns the V3 restrictions, inheritors can override this to return their own
        /// </summary>
        /// <returns></returns>
        internal virtual PermissionFlags GetOtherRestrictions()
        {
            //Default is just to return V3
            return GetRestrictionsV3();
        }


        private PermissionFlags GetRestrictionsV2()
        {
            PermissionFlags restrict = GetEmptyRestrictions(this.Revision);

            if (AllowPrinting)
                restrict |= PermissionFlags.AllowPrinting;
            if (AllowModification)
                restrict |= PermissionFlags.AllowOtherModification;
            if (AllowCopying)
                restrict |= PermissionFlags.AllowCopyingOfTextAndGraphics;
            if (AllowAnnotations)
                restrict |= PermissionFlags.AllowR2AnnotationsAndForms;

            return restrict;
        }

        private PermissionFlags GetRestrictionsV3()
        {
            PermissionFlags restrict = GetEmptyRestrictions(this.Revision);

            if (AllowPrinting)
                restrict |= PermissionFlags.AllowPrinting;
            if (AllowModification)
                restrict |= PermissionFlags.AllowOtherModification;
            if (AllowCopying)
                restrict |= PermissionFlags.AllowCopyingOfTextAndGraphics;
            if (AllowAnnotations)
                restrict |= PermissionFlags.AllowR2AnnotationsAndForms;

            if (AllowFormFilling)
            {
                restrict |= PermissionFlags.AllowR2AnnotationsAndForms;
                restrict |= PermissionFlags.AllowFormFilling;
            }
            if (AllowAccessiblity)
                restrict |= PermissionFlags.AllowAccessibleTextAndGraphics;
            if (AllowDocumentAssembly)
            {
                restrict -= PermissionFlags.AllowDocumentAssembly;
            }
            if (AllowHighQualityPrinting)
            {
                restrict |= PermissionFlags.AllowPrinting;
                restrict |= PermissionFlags.AllowHighQualityPrinting;
            }
            return restrict;
        }


        private PermissionFlags GetEmptyRestrictions(int revision)
        {
            uint def = (uint)ushort.MaxValue;
            def = def << 16;

            //These are defined but must be one.
            PermissionFlags empty = PermissionFlags.Reserved7
                                            | PermissionFlags.Reserved8
                                            | PermissionFlags.Reserved13
                                            | PermissionFlags.Reserved14
                                            | PermissionFlags.Reserved15
                                            | PermissionFlags.Reserved16;
            empty |= (PermissionFlags)def;

            return empty;
        }

        // static methods

        #region private static void PopulateAndClearPassword(char[] password, SecureString str)

        /// <summary>
        /// Fills the secure string with the specified characters, and then resets the array, so the password is no longer retrieveable
        /// </summary>
        /// <param name="password"></param>
        /// <param name="str"></param>
        private static void PopulateAndClearPassword(char[] password, SecureString str)
        {
            if (null == password || password.Length == 0)
                throw new ArgumentNullException("password");
            int len = password.Length;
            for (int i = 0; i < len; i++)
            {
                char c = password[i];
                str.AppendChar(c);
            }
            Array.Clear(password, 0, len);
        }

        #endregion
    }
}
