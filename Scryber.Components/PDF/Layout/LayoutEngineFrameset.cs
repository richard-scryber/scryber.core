using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        //if (this.Document.RemoteRequests.ExecMode != DocumentExecMode.Immediate)
        //{
        //    this.WaitAndEnsureFramesReady();
        //}
        
        var doc = new PDFFramesetLayoutDocument(this.Document, this);
        context.DocumentLayout = doc;
        doc.PrependFile = this.Frameset.CurrentFile;

        if (null == doc.PrependFile || this.Frameset.RootReference.Status != FrameFileStatus.Ready)
            throw new InvalidOperationException(
                "The generated file is not loaded or ready - cannot proceed with the layout until this is completed");
        
        doc.ExistingCatalog = doc.PrependFile.GetCatalog();
        this.AddFramePages(context, fullstyle);
        this.AddNames(doc, context, fullstyle);
    }

    protected virtual void WaitAndEnsureFramesReady()
    {
        int count = 0;
        int maxDuration = 40; //20 seconds
        int delayMs = 500;
        
        while (count < maxDuration && !AllFramesAreReady())
        {
            Task.Delay(delayMs).GetAwaiter().GetResult();
            count++;
        }
        
    }

    private bool AllFramesAreReady()
    {
        
        var root = this.Frameset.RootReference;
        
        if (root.Status != FrameFileStatus.Ready && root.Status != FrameFileStatus.Invalid)
            return false;
        
        if (this.Frameset.DependantReferences.Count > 0)
        {
            foreach (var fref in this.Frameset.DependantReferences)
            {
                if (fref.Status != FrameFileStatus.Ready && fref.Status != FrameFileStatus.Invalid)
                    return false;
            }
        }

        return true;
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

    protected virtual void AddNames(PDFFramesetLayoutDocument doc, PDFLayoutContext context, Style fullStyle)
    {
        if (this.Frameset.DependantReferences.Count > 0)
        {
            //We have more than one file so merge all the names.
            var root = this.Frameset.RootReference.FrameFile;

            this.CopyNamesToFramesetDocument(root, doc);

            if (this.Frameset.DependantReferences.Count > 0)
            {
                foreach (var fRef in this.Frameset.DependantReferences)
                {
                    var file = fRef.FrameFile;

                    if (null != file)
                        this.CopyNamesToFramesetDocument(file, doc);
                }
            }
        }
    }

    private void CopyNamesToFramesetDocument(PDFFile file, PDFFramesetLayoutDocument doc)
    {
        PDFArray namesArray;
        
        if (this.TryGetFileNamesArray(file, out namesArray))
        {
            //An array of name string and then an object reference
            var index = 0;
            if (null == doc.SourceNames)
                doc.SourceNames = new SortedDictionary<string, IPDFFileObject>();

            while (index < namesArray.Count)
            {
                var name = (PDFString)namesArray[index++];
                var orref = namesArray[index++];

                doc.SourceNames.Add(name.Value, orref);
            }
        }
        
    }

    private bool TryGetFileNamesArray(PDFFile file, out PDFArray namesArray)
    {
        var catalog = file.DocumentCatalog;
        PDFObjectRef namesRef;

        catalog.TryGetValue("Names", out var value);
        namesRef = value as PDFObjectRef;
        
        if (null != namesRef)
        {
            var namesDict = file.AssertGetContent(namesRef) as PDFDictionary;
            if (null != namesDict && namesDict.TryGetValue("Dests", out var found))
            {
                var destsEntry = found as PDFObjectRef;
                if (null != destsEntry)
                {
                    var destsDict = file.AssertGetContent(destsEntry) as PDFDictionary;

                    if (null != destsDict && destsDict.TryGetValue("Names", out var found2))
                    {
                        namesArray = found2 as PDFArray;
                        return (null != namesArray);
                    }
                }
            }
        }

        namesArray = null;
        return false;
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