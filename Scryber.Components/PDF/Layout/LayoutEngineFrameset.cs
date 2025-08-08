using System;
using System.Collections.Generic;
using Scryber.Html.Components;
using Scryber.Modifications;
using Scryber.PDF.Native;
using Scryber.PDF.Parsing;
using Scryber.Styles;

namespace Scryber.PDF.Layout;

public class LayoutEngineFrameset : IPDFLayoutEngine
{
    

    public IPDFLayoutEngine ParentEngine { get; private set; }
    
    public bool ContinueLayout { get; set; }
    public PDFLayoutContext Context { get; private set; }
    
    /// <summary>
    /// Gets the root document that contains the frameset.
    /// </summary>
    protected HTMLDocument Document { get; private set; }
    
    public HTMLFrameset Frameset { get; private set; }


    public LayoutEngineFrameset(HTMLDocument document, IPDFLayoutEngine parent,
        PDFLayoutContext context)
    {
        this.ParentEngine = parent;
        this.Context = context;
        this.Document = document;
        this.Frameset = document.Frameset ?? throw new ArgumentNullException("The provided document does not contain a frameset");
    }
    
    public void Layout(PDFLayoutContext context, Style fullstyle)
    {
        var doc = new PDFModifyLayoutDocument(this.Document, this);
        context.DocumentLayout = doc;
        doc.OriginalFile = this.Frameset.RootReference.ReferencedFile;

        if (null == doc.OriginalFile || this.Frameset.RootReference.Status != FrameFileStatus.Ready)
            throw new InvalidOperationException(
                "The root file is not loaded or ready - cannot proceed with the layout until this is completed");
        
        doc.ExistingCatalog = doc.OriginalFile.GetCatalog();
        this.AddFramePages(context, fullstyle);
        this.AddNames(doc, context, fullstyle);
    }

    protected void AddFramePages(PDFLayoutContext context, Style fullStyle)
    {
        
        foreach (var frame in this.Frameset.Frames)
        {
            this.DoAddAFramesPages(frame, context, fullStyle);
        }
    }

    protected virtual void DoAddAFramesPages(HTMLFrame frame, PDFLayoutContext context, Style fullStyle)
    {
        var engine = frame.GetEngine(this, context, fullStyle) as IPDFLayoutEngine;
        engine.Layout(context, fullStyle);
    }

    protected virtual void AddNames(PDFModifyLayoutDocument doc, PDFLayoutContext context, Style fullStyle)
    {
        if (this.Frameset.RootReference.FileType == FrameFileType.DirectPDF)
        {
            var root = this.Frameset.RootReference.ReferencedFile;
            var catalog = root.DocumentCatalog;
            var namesRef = catalog["Names"] as PDFObjectRef;

            
            if (null != namesRef)
            {
                //doc.OriginalNameDictionary = namesRef;
                //var namesDict = root.AssertGetContent(namesRef);
            }
        }
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

    ~LayoutEngineFrameset()
    {
        this.Dispose(false);
    }
}