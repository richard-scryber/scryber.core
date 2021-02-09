using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Secure.Components;

namespace Scryber.Secure
{
    /// <summary>
    /// Defines the permissions and security type for a PDFSecureDocument.
    /// </summary>
    public class PDFDocumentPermissions
    {
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

        //
        // .ctor
        //

        public PDFDocumentPermissions()
        {
            this.Revision = PDFSecureDocument.DefaultEncryptionRevision;
            this.Version = PDFSecureDocument.DefaultEncryptionVersion;
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
                    throw new PDFSecurityException(string.Format(Errors.NoAvailableEncryptorFactoryForVersionAndRevision, this.Version, this.Revision));
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
    }
}
