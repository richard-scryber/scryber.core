using System.Collections.Generic;
using Scryber.PDF.Native;

namespace Scryber.PDF.Layout;

public class PDFModifyPageReference
{
    /// <summary>
    /// Gets the object reference within the file for the original page dictionary
    /// </summary>
    public PDFObjectRef OriginalPageRef { get; private set; }
    
    /// <summary>
    /// The actual page dictionary contents
    /// </summary>
    public PDFDictionary PageDictionary { get; private set; }

    /// <summary>
    /// Gets or sets the object reference the page was updated to within the new file / file section
    /// </summary>
    public PDFObjectRef NewPageRef { get; set; }

    public PDFModifyPageReference(PDFObjectRef originalPage, PDFDictionary pageDictionary)
    {
        this.OriginalPageRef = originalPage;
        this.PageDictionary = pageDictionary;
    }
}

public class PDFModifyPageReferenceList : List<PDFModifyPageReference>
{
    
}