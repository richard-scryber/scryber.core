using System.Collections.Generic;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;

namespace Scryber.PDF.Layout;


/// <summary>
/// The frame layout engine for pages from a root pdf file
/// </summary>
public class LayoutEngineRootPDFFrame : LayoutEngineFrame
{
    protected PDFFile RootFile { get; set; }

    protected PDFFramesetLayoutDocument LayoutDocument { get; set; }

    public LayoutEngineRootPDFFrame(LayoutEngineFrameset parent, HTMLFrame forFrame, PDFFile rootFile,
        PDFLayoutContext context)
        : base(parent, forFrame, context)
    {
        this.RootFile = rootFile;
        this.LayoutDocument = (PDFFramesetLayoutDocument)context.DocumentLayout;
    }

    public override void Layout(PDFLayoutContext context, Style fullStyle)
    {
        if (this.Frame.Visible)
        {
            var treeRef = this.RootFile.PageTree;
            var tree = this.RootFile.GetContent(treeRef) as PDFDictionary;
            if (null == tree)
            {
                if (context.Conformance == ParserConformanceMode.Strict)
                    throw new PDFLayoutException(
                        "The page tree for the source pdf document could not be loaded. Cannot insert the requested pages from the original file.");
                else
                {
                    context.TraceLog.Add(TraceLevel.Error, "Modifications", "The page tree for the source pdf document could not be loaded. Cannot insert the requested pages from the original file. Skipping the frame '" + this.Frame.UniqueID + "'.");
                    return;
                }
            }
            
            var kids = tree["Kids"] as PDFArray;
            
            var index = GetFirstPageIndex(context, kids);
            var count = this.GetFramePageCount(context, index, kids);
            var last = index + count;
            
            while (index < last)
            {
                var reference = GetRootPageReference(index, kids, context);
                
                if (null != reference)
                {
                    var pageRef = new PDFModifyPageReference(reference);
                    this.LayoutDocument.OutputPages.Add(pageRef);
                }

                index++;
            }
        }
    }

    protected int GetFirstPageIndex(PDFLayoutContext context, PDFArray pageTree)
    {
        var index = this.Frame.PageStartIndex;
        if (index < 0)
            index = 0;
        else if (index >= pageTree.Count)
        {
            context.TraceLog.Add(TraceLevel.Warning, "Modifications", "The start page index for frame " + this.Frame.UniqueID + " is greater than the total number of pages within the source document. No pages will be included for this frame.");
            index = int.MaxValue;
        }

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
            count = pageTree.Count - startIndex;
        
        return count;
    }

    protected PDFObjectRef GetRootPageReference(int pgIndex, PDFArray pageRefArray, PDFLayoutContext context)
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
                if (context.Conformance == ParserConformanceMode.Strict)
                    throw new PDFLayoutException(
                        "Page " + pgIndex +
                        " was not an object reference within the page tree for the source pdf document. Cannot insert the requested page from the original file.");
                else
                {
                    context.TraceLog.Add(TraceLevel.Error, "Modifications",
                        "Page " + pgIndex +
                        " was not an object reference within the page tree for the source pdf document. Cannot insert the requested page from the original file. Skipping the page in frame '" +
                        this.Frame.UniqueID + "'.");
                    return null;
                }
            }

        }

    }
}