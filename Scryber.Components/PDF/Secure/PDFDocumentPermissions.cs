using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber.PDF.Secure
{
    /// <summary>
    /// Defines the permissions and security type for a PDFSecureDocument.
    /// </summary>
    public class PDFDocumentPermissions
    {

        private const int DefaultEncryptionRevision = 3;

        private const int DefaultEncryptionVersion = 2;

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

        /// <summary>
        /// Standard40Bit permission to allow printing
        /// </summary>
        [PDFAttribute("allow-printing")]
        public bool AllowPrinting { get; set; }

        /// <summary>
        /// Standard40Bit permission to allow modifications
        /// </summary>
        [PDFAttribute("allow-modification")]
        public bool AllowModification { get; set; }

        /// <summary>
        /// Standard40Bit permission to allow copying
        /// </summary>
        [PDFAttribute("allow-copying")]
        public bool AllowCopying { get; set; }

        /// <summary>
        /// Standard40Bit permission to allow annotations
        /// </summary>
        [PDFAttribute("allow-annotations")]
        public bool AllowAnnotations { get; set; }

        //
        // Revision 2.3 properties
        //

        /// <summary>
        /// Standard128Bit permission to allow form filling
        /// </summary>
        [PDFAttribute("allow-forms")]
        public bool AllowFormFilling { get; set; }

        /// <summary>
        /// Standard128Bit permission to allow accessibility
        /// </summary>
        [PDFAttribute("allow-accessibility")]
        public bool AllowAccessiblity { get; set; }

        /// <summary>
        /// Standard128Bit permission to allow document assembly
        /// </summary>
        [PDFAttribute("allow-document-assembly")]
        public bool AllowDocumentAssembly { get; set; }

        /// <summary>
        /// Standard128Bit permission to allow high quality printing
        /// </summary>
        [PDFAttribute("allow-hq-printing")]
        public bool AllowHighQualityPrinting { get; set; }

        private static readonly PermissionFlags allAllowedR3 = (PermissionFlags)(-4100);
        private static readonly PermissionFlags allAllowedR2 = (PermissionFlags)(-3844);

        /// <summary>
        /// Returns true if any of the restrictions are set.
        /// </summary>
        public bool HasRestrictions
        {
            get
            {
                var curr = this.GetRestrictions();

                var type = this.Type;
                PermissionFlags allAllowed;
                if (type == SecurityType.Standard40Bit)
                    allAllowed = allAllowedR2;
                else if (type == SecurityType.Standard128Bit)
                    allAllowed = allAllowedR3;
                else
                    allAllowed = (PermissionFlags)(-1);

                return curr != allAllowed;
            }
        }
        //
        // .ctor
        //

        public PDFDocumentPermissions()
        {
            this.Revision = DefaultEncryptionRevision;
            this.Version = DefaultEncryptionVersion;
            this.AllowAccessiblity = true;
            this.AllowAnnotations = true;
            this.AllowCopying = true;
            this.AllowDocumentAssembly = true;
            this.AllowFormFilling = true;
            this.AllowHighQualityPrinting = true;
            this.AllowModification = true;
            this.AllowPrinting = true;
        }

        //
        // methods
        //

        private PDFEncrypterFactory _factory = null;

        /// <summary>
        /// Returns the appropriate encryptor factory for the Version and Revision
        /// </summary>
        /// <returns></returns>
        internal virtual PDFEncrypterFactory GetFactory()
        {
            if (null == _factory)
            {
                PDFEncrypterFactory factory = PDFEncrypterFactory.CreateFactory(this.Version, this.Revision);
                if (null == factory)
                    throw new PDFSecurityException(string.Format("There is no encrypter factory for the version " + this.Version + " and revision " + this.Revision));
                _factory = factory;
            }
            return _factory;
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
            PermissionFlags restrict = GetEmptyRestrictions();

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
            PermissionFlags restrict = GetEmptyRestrictions();

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


        private PermissionFlags GetEmptyRestrictions()
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
    }
}
