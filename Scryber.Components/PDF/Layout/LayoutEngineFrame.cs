using System.Collections.Generic;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using System;

namespace Scryber.PDF.Layout;

public abstract class LayoutEngineFrame : IPDFLayoutEngine
{

    public IPDFLayoutEngine ParentEngine { get; }
    public bool ContinueLayout { get; set; }
    public PDFLayoutContext Context { get; }
    public HTMLFrame Frame { get; set; }
    
    protected PDFFramesetLayoutDocument LayoutDocument { get; set; }
    
    
    public LayoutEngineFrame(LayoutEngineFrameset parent, HTMLFrame forFrame, PDFLayoutContext context)
    {
        this.ParentEngine = parent;
        this.Context = context;
        this.ContinueLayout = true;
        this.Frame = forFrame;
        
        this.LayoutDocument = (PDFFramesetLayoutDocument)context.DocumentLayout;
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

    protected void AddFramePageReferences(PDFLayoutContext context, Style fullStyle, PDFFile fromFile)
    {
        try
        {
            this.DoAddFramePageReferences(context, fullStyle, fromFile);
        }
        catch (Exception ex)
        {
            throw new PDFLayoutException(
                "Could not extract the page references from the frame file. See the inner exception for more details",
                ex);
        }
    }

    protected virtual void DoAddFramePageReferences(PDFLayoutContext context, Style fullStyle, PDFFile fromFile)
    {
        var treeRef = fromFile.PageTree;
        var tree = fromFile.GetContent(treeRef) as PDFDictionary;
        if (null == tree)
        {
            this.LogOrThrowWarning(context, "Page tree could not be found in the document " + fromFile.ToString());
            return;
        }
            
        var kids = tree["Kids"] as PDFArray;
        if (null == kids)
        {
            this.LogOrThrowWarning(context, "Page tree could not be found in the document " + fromFile.ToString());
            return;
        }
        
        var index = GetFirstPageIndex(context, kids);

        if (index >= kids.Count)
        {
            this.LogOrThrowWarning(context, "The start index is greater than the number of pages found in the document " + fromFile.ToString() + ", so no pages will be output for frame " + this.Frame.ToString());
            return;
        }
        
        var count = this.GetFramePageCount(context, index, kids);
        var last = index + count;
            
        while (index < last)
        {
            var reference = GetPageReference(index, kids, context);
                
            if (null != reference)
            {
                var pageDict = GetPageDictionary(context, index, reference, fromFile);

                if (null != pageDict)
                {
                    var pageRef = new PDFModifyPageReference(reference, pageDict);
                    this.LayoutDocument.OutputPages.Add(pageRef);
                }
            }
            else
            {
                LogOrThrowWarning(context, "The extract the page dictionary for " + reference + " or it could not be loaded");
            }

            index++;
        }
    }
    

    protected PDFDictionary GetPageDictionary(PDFLayoutContext context, int index, PDFObjectRef pageRef, PDFFile file)
    {
        var obj = file.GetContent(pageRef);
        if (null == obj)
            return null;
        
        var dict = obj as PDFDictionary;
        if (null == dict)
        {
            LogOrThrowWarning(context, "The referenced page for " + pageRef.ToString() + " was not a page dictionary");
            return null;
        }

        IPDFFileObject found;
        if (dict.TryGetValue("Type", out found) && found is PDFName pname)
        {
            if (pname.Value == "Page")
            {
                if(context.TraceLog.ShouldLog(TraceLevel.Verbose))
                    context.TraceLog.Add(TraceLevel.Verbose, "Modifications", "Found and returning the page dictionary for page index '" + index + "' with reference " + pageRef);
                return dict;
            }
        }
        LogOrThrowWarning(context, "The returned type was not a page dictionary type for object " + pageRef.ToString());

        return null;
    }
    
    protected int GetFirstPageIndex(PDFLayoutContext context, PDFArray pageTree)
    {
        var index = this.Frame.PageStartIndex;
        if (index < 0)
            index = 0;

        return index;
    }

    protected int GetFramePageCount(PDFLayoutContext context, int startIndex, PDFArray pageTree)
    {
        var count = this.Frame.PageInsertCount;
        
        if (count <= 0)
            return 0;
        else if (count == HTMLFrame.AppendAllPageCount)
            count = pageTree.Count - startIndex;
        else if (startIndex + count > pageTree.Count)
        {
            this.LogOrThrowWarning(context, "The requested number of pages for frame " + this.Frame.ToString() + " is greater than the available number of pages in the document (based on starting index).");
            count = pageTree.Count - startIndex;
        }

        return count;
    }

    protected PDFObjectRef GetPageReference(int pgIndex, PDFArray pageRefArray, PDFLayoutContext context)
    {
        if (pgIndex < 0)
        {
            return null;
        }
        else if (pgIndex >= pageRefArray.Count)
        {
            return null;
        }
        else
        {
            var found = pageRefArray[pgIndex];
            if (found is PDFObjectRef objectRef)
                return objectRef;
            else
            {
                LogOrThrowWarning(context, "Page " + pgIndex +
                                           " was not an object reference within the page tree for the source pdf document. Cannot insert the requested page from the original file.");
                return null;
            }
        }
    }

    protected void LogOrThrowWarning(ContextBase context, string message)
    {
        if (context.Conformance == ParserConformanceMode.Strict)
            throw new PDFLayoutException(message);
        else
        {
            context.TraceLog.Add(TraceLevel.Warning, "Frame Layout", message);
        }
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


