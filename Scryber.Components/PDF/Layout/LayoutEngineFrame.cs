using System.Collections.Generic;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;

namespace Scryber.PDF.Layout;

public class LayoutEngineFrame : IPDFLayoutEngine
{

    public IPDFLayoutEngine ParentEngine { get; }
    public bool ContinueLayout { get; set; }
    public PDFLayoutContext Context { get; }
    public HTMLFrame Frame { get; set; }
    
    
    public LayoutEngineFrame(LayoutEngineFrameset parent, HTMLFrame forFrame, PDFLayoutContext context)
    {
        this.ParentEngine = parent;
        this.Context = context;
        this.ContinueLayout = true;
        this.Frame = forFrame;
    }
    
    public virtual void Layout(PDFLayoutContext context, Style fullstyle)
    {
        
    }

    public bool MoveToNextPage(IComponent initiator, Style initiatorStyle, Stack<PDFLayoutBlock> depth, ref PDFLayoutRegion region,
        ref PDFLayoutBlock block)
    {
        return false;
    }

    public PDFLayoutBlock CloseCurrentBlockAndStartNewInRegion(PDFLayoutBlock blockToClose, PDFLayoutRegion joinToRegion)
    {
        return blockToClose;
    }

    protected virtual bool Dispose(bool disposing)
    {
        return true;
    }
    
    public void Dispose()
    {
        this.Dispose(true);
    }

    ~LayoutEngineFrame()
    {
        this.Dispose(false);
    }

}


