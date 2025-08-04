using Scryber.Components;
using Scryber.PDF.Native;
using Scryber.PDF.Parsing;

namespace Scryber.PDF.Layout;

public class PDFModifyLayoutDocument : PDFLayoutDocument
{

    public PDFFile OriginalFile { get; set; }
    
    public PDFParsedCatalog ExistingCatalog { get; set; }
    
    public PDFModifyLayoutDocument(Document root, LayoutEngineFrameset engine) : base(root, engine)
    {
        
    }
}