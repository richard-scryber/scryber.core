using System;
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
    /// Gets the file the original page is from.
    /// </summary>
    public PDFFile OriginalFile { get; private set; }
    
    /// <summary>
    /// Gets the object reference within the overlay file for the page dictionary of the contents that will be rendered on top of the original page.
    /// Use the SetOverlay() method to populate these values.
    /// </summary>
    public PDFObjectRef OverlayPageReference { get; private set; }
    
    /// <summary>
    /// Gets the actual page dictionary for the overlay. Use the SetOverlay() method
    /// </summary>
    public PDFDictionary OverlayPageDictionary { get; private set; }
    
    
    /// <summary>
    /// Gets the source File any overlay page is from.
    /// </summary>
    public PDFFile OverlayFile { get; private set; }
    
    /// <summary>
    /// Returns true if this ModifyPage has both an underlying page and a page to render toe contents of over the top.
    /// </summary>
    public bool HasOverlay { get; private set; }

    /// <summary>
    /// Gets or sets the object reference to the page dictionary that was updated to within the new file / file section
    /// </summary>
    public PDFObjectRef NewPageRef { get; set; }

    public PDFModifyPageReference(PDFObjectRef originalPage, PDFDictionary pageDictionary, PDFFile originalFile)
    {
        this.OriginalPageRef = originalPage;
        this.PageDictionary = pageDictionary;
        this.OriginalFile = originalFile;
    }

    public bool SetOverlay(PDFObjectRef overlayRef, PDFDictionary overlayDict, PDFFile overlayFile)
    {
        this.OverlayPageReference = overlayRef ?? throw new ArgumentNullException(nameof(overlayRef));
        this.OverlayPageDictionary = overlayDict ?? throw new ArgumentNullException(nameof(overlayDict));
        this.OverlayFile = overlayFile ?? throw new ArgumentNullException(nameof(overlayFile));
        this.HasOverlay = true;

        return this.HasOverlay;
    }
}

public class PDFModifyPageReferenceList : List<PDFModifyPageReference>
{
    
}