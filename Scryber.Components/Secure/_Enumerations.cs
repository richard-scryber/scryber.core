using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber.Secure
{
    /// <summary>
    /// Defines the standard encryption types for the RC4 algorithm
    /// </summary>
    public enum SecurityType
    {
        /// <summary>
        /// 40bit encryption - Version 1, revision 2
        /// </summary>
        Standard40Bit,
        
        /// <summary>
        /// 128bit encryption - Version 2, revision 3
        /// </summary>
        Standard128Bit,

        /// <summary>
        /// A non-standard type
        /// </summary>
        Other
    }

    /// <summary>
    /// The flags for specifying premissions (restrictions) on a document
    /// </summary>
    [Flags()]
    public enum PermissionFlags : int
    {
        /// <summary>
        /// Do not use - must be 0
        /// </summary>
        Reserved1 = 1,
        /// <summary>
        /// Do not use - must be 0
        /// </summary>
        Reserved2 = 2,

        /// <summary>
        /// Bit 3. Print the document (possibly not at the high- est quality level, depending on whether bit 12 is also set - HighQualityPrinting).
        /// </summary>
        AllowPrinting = 4,

        /// <summary>
        /// Bit 4. Allow modification to the document other than those specificied in bits 6,9 and 11 (6=AllowAnnotations, 9=AllowFormFilling, 11=AssembeTheDocument)
        /// </summary>
        AllowOtherModification = 8,

        /// <summary>
        /// Bit 5. Allow users to copy text and graphical data (excluding for accessibility)
        /// </summary>
        AllowCopyingOfTextAndGraphics = 16,

        /// <summary>
        /// Bit 6. Allow the creation of annotations and also filling interactive forms
        /// </summary>
        AllowR2AnnotationsAndForms = 32,

        /// <summary>
        /// Bit 7. Reserved, mut be 1
        /// </summary>
        Reserved7 = 64,

        /// <summary>
        /// Bit 8. Reserved must be 1
        /// </summary>
        Reserved8 = 128,

        /// <summary>
        /// Bit 9. Allow filling of form fields (even if bit 6 is clear)
        /// </summary>
        AllowFormFilling = 256,

        /// <summary>
        /// Bit 10. Allow the extraction of text and graphics for accessibility purposes.
        /// </summary>
        AllowAccessibleTextAndGraphics = 512,

        /// <summary>
        /// Bit 11. Allow assembly of document including insert, rotate, delete pages and creation of bookmarks and thumbnails
        /// </summary>
        AllowDocumentAssembly = 1024,

        /// <summary>
        /// If set and bit 3 is set then this document can be printed at a high quality, otherwise it can only be low quality if bit 3 is set.
        /// </summary>
        AllowHighQualityPrinting = 2048,

        /// <summary>
        /// Bit 13. Reserved, must be 1
        /// </summary>
        Reserved13 = 4096,

        /// <summary>
        /// Bit 14. Reserved, must be 1
        /// </summary>
        Reserved14 = 8192,

        /// <summary>
        /// Bit 15. Reserved, must be 1
        /// </summary>
        Reserved15 = 16384,

        /// <summary>
        /// Bit 16. Reserved, must be 1
        /// </summary>
        Reserved16 = 32768,
    }
}
