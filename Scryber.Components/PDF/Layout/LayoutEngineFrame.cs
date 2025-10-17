using System.Collections.Generic;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using System;
using System.Diagnostics.CodeAnalysis;

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
            var overlay = this.Frame.OverReference?.FrameFile;
            var overlayRepeat = null == overlay ? FrameOverlayRepeat.None : this.Frame.OverlayRepeat;
                
            
            this.DoAddFramePageReferences(context, fullStyle, fromFile, overlay, overlayRepeat);
        }
        catch (Exception ex)
        {
            throw new PDFLayoutException(
                "Could not extract the page references from the frame file. See the inner exception for more details",
                ex);
        }
    }

    protected virtual void DoAddFramePageReferences(PDFLayoutContext context, Style fullStyle, PDFFile fromFile, PDFFile overlayFile, FrameOverlayRepeat overlayRepeat)
    {
        PDFObjectRef[] overlayReferences = null;
        bool hasOverlays = false;
        PDFObjectRef[] kids = null;
        if (null != overlayFile)
        {
            if (TryGetAllPageReferences(context, overlayFile, out var found))
            {
                overlayReferences = found;
                hasOverlays = true;
            }
        }
        
        
        if(!TryGetAllPageReferences(context, fromFile, out kids))
            return;
        
        
        var start = GetFirstPageIndex(context, kids);

        if (start >= kids.Length)
        {
            this.LogOrThrowWarning(context, "The start index is greater than the number of pages found in the document " + fromFile.ToString() + ", so no pages will be output for frame " + this.Frame.ToString());
            return;
        }
        
        var count = this.GetFramePageCount(context, start, kids);
        var last = start + count;
        var index = start;
        
        while (index < last)
        {
            var reference = GetPageReference(index, kids, context);
                
            if (null != reference)
            {
                var pageDict = GetPageDictionary(context, index, reference, fromFile);

                if (null != pageDict)
                {
                    var pageRef = new PDFModifyPageReference(reference, pageDict, fromFile);
                    this.LayoutDocument.OutputPages.Add(pageRef);

                    if (overlayRepeat != FrameOverlayRepeat.None && hasOverlays)
                    {
                        PDFObjectRef overlayRef = null;
                        PDFDictionary overlayDictionary = null;
                        var overlayPgIndex = 0;
                        
                        switch (overlayRepeat)
                        {
                            case FrameOverlayRepeat.All:
                                //A page from the overlay will be on every output page of the underlying pages.
                                //Repeating each of the pages from the overlay in order 
                                if (overlayReferences.Length > 1)
                                {
                                    overlayPgIndex = (index - start) % overlayReferences.Length;
                                }

                                overlayRef = overlayReferences[overlayPgIndex];
                                overlayDictionary = overlayFile.GetContent(overlayRef) as PDFDictionary;
                                
                                pageRef.SetOverlay(overlayRef, overlayDictionary, overlayFile);
                                break;
                            
                            case FrameOverlayRepeat.Once:
                                //All pages from the overlay will be displayed once and once only
                                //on each consecutive page of the underlying pages.
                                
                                var refIndex = (index - start);
                                if (refIndex < overlayReferences.Length)
                                {
                                    overlayRef = overlayReferences[refIndex];
                                    overlayDictionary = overlayFile.GetContent(overlayRef) as PDFDictionary;
                                    
                                    pageRef.SetOverlay(overlayRef, overlayDictionary, overlayFile);
                                }
                                break;
                            
                            case FrameOverlayRepeat.First:
                                //Only the first page of the overlay document will show on the first page of the underlying pages
                                if (index == start)
                                {
                                    overlayRef = overlayReferences[0];
                                    overlayDictionary = overlayFile.GetContent(overlayRef) as PDFDictionary;

                                    pageRef.SetOverlay(overlayRef, overlayDictionary, overlayFile);
                                }
                                break;
                            
                            case FrameOverlayRepeat.Last:
                                //Only the last page of the overlay document will show on the last page of the underlying pages.
                                if (index == last - 1)
                                {
                                    overlayRef = overlayReferences[overlayReferences.Length - 1];
                                    overlayDictionary = overlayFile.GetContent(overlayRef) as PDFDictionary;

                                    pageRef.SetOverlay(overlayRef, overlayDictionary, overlayFile);
                                }
                                break;
                            
                            default:
                                ; //Do Nothing
                                break;
                        }
                        
                    }
                }
            }
            else
            {
                LogOrThrowWarning(context, "The extract the page dictionary for " + reference + " or it could not be loaded");
            }

            index++;
        }
    }
    

    protected bool TryGetAllPageReferences(PDFLayoutContext context, PDFFile fromFile, out PDFObjectRef[] references)
    {
        references = null;
        var treeRef = fromFile.PageTree;
        var tree = fromFile.GetContent(treeRef) as PDFDictionary;
        if (null == tree)
        {
            this.LogOrThrowWarning(context, "Page tree could not be found in the document " + fromFile.ToString());
            return false;
        }
            
        var kids = tree["Kids"] as PDFArray;
        if (null == kids)
        {
            this.LogOrThrowWarning(context, "Page tree could not be found in the document " + fromFile.ToString());
            return false;
        }

        List<PDFObjectRef> all = new List<PDFObjectRef>(kids.Count);
        foreach (var kid in kids)
        {
            var oref = kid as PDFObjectRef;
            if(null != oref)
                all.Add(oref);
        }

        references = all.ToArray();
        return references.Length > 0;
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
    
    protected int GetFirstPageIndex(PDFLayoutContext context, PDFObjectRef[] pageTree)
    {
        var index = this.Frame.PageStartIndex;
        if (index < 0)
            index = 0;

        return index;
    }

    protected int GetFramePageCount(PDFLayoutContext context, int startIndex, PDFObjectRef[] pageTree)
    {
        var count = this.Frame.PageInsertCount;
        
        if (count <= 0)
            return 0;
        else if (count == HTMLFrame.AppendAllPageCount)
            count = pageTree.Length - startIndex;
        else if (startIndex + count > pageTree.Length)
        {
            this.LogOrThrowWarning(context, "The requested number of pages for frame " + this.Frame.ToString() + " is greater than the available number of pages in the document (based on starting index).");
            count = pageTree.Length - startIndex;
        }

        return count;
    }

    protected PDFObjectRef GetPageReference(int pgIndex, PDFObjectRef[] pageRefArray, PDFLayoutContext context)
    {
        if (pgIndex < 0)
        {
            return null;
        }
        else if (pgIndex >= pageRefArray.Length)
        {
            return null;
        }
        else
        {
            return pageRefArray[pgIndex];
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


