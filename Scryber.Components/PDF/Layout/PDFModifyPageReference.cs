using System.Collections.Generic;
using Scryber.PDF.Native;

namespace Scryber.PDF.Layout;

public class PDFModifyPageReference
{
    public PDFObjectRef OriginalPageRef { get; private set; }


    public PDFModifyPageReference(PDFObjectRef originalPage)
    {
        this.OriginalPageRef = originalPage;
    }
}

public class PDFModifyPageReferenceList : List<PDFModifyPageReference>
{
    
}