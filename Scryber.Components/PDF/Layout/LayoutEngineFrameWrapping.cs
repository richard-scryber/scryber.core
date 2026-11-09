using System;
using System.Collections.Generic;
using Scryber.Html.Components;
using Scryber.Styles;

namespace Scryber.PDF.Layout;

/// <summary>
/// Wraps any inner engine and allows the IFrame to apply security policies to the layout beforehand and relieve after.
/// </summary>
public class LayoutEngineFrameWrapping : IPDFLayoutEngine
{
    
    public LayoutEngineFrameWrapping(HTMLiFrame frame, IPDFLayoutEngine innerEngine)
    {
        this._innerEngine = innerEngine ?? throw new ArgumentNullException(nameof(innerEngine));
        this._innerFrame = frame ?? throw new ArgumentNullException(nameof(frame));
        
    }
    
    private IPDFLayoutEngine _innerEngine;
    private HTMLiFrame _innerFrame;

    public IPDFLayoutEngine ParentEngine { get { return _innerEngine.ParentEngine; } }

    public bool ContinueLayout
    {
        get  { return _innerEngine.ContinueLayout; }
        set  { _innerEngine.ContinueLayout = value; }
    }

    public PDFLayoutContext Context  { get { return _innerEngine.Context; } }

    public void Layout(PDFLayoutContext context, Style fullstyle)
    {
        this._innerFrame.ApplyLayoutPolicy(context, ref fullstyle);

        try
        {
            this._innerEngine.Layout(context, fullstyle);
        }
        finally
        {
            this._innerFrame.ReleaseLayoutPolicy(context, ref fullstyle);
        }
    }
    
    

    public bool MoveToNextPage(IComponent initiator, Style initiatorStyle, Stack<PDFLayoutBlock> depth, ref PDFLayoutRegion region,
        ref PDFLayoutBlock block)
    {
        return this._innerEngine.MoveToNextPage(initiator, initiatorStyle, depth, ref region, ref block);
    }

    public PDFLayoutBlock CloseCurrentBlockAndStartNewInRegion(PDFLayoutBlock blockToClose, PDFLayoutRegion joinToRegion)
    {
        return this._innerEngine.CloseCurrentBlockAndStartNewInRegion(blockToClose, joinToRegion);
    }

    ~LayoutEngineFrameWrapping()
    {
        this.Dispose(false);
    }
    
    public void Dispose()
    {
        this.Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (null != this._innerEngine)
                this._innerEngine.Dispose();
        }
    }
}