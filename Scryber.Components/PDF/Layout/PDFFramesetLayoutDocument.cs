using System;
using System.Collections.Generic;
using Scryber.Components;
using Scryber.PDF.Native;
using Scryber.PDF.Parsing;

namespace Scryber.PDF.Layout;

public class PDFFramesetLayoutDocument : PDFLayoutDocument
{

    
    
    public PDFParsedCatalog ExistingCatalog { get; set; }
    
    
    public SortedDictionary<string, IPDFFileObject> SourceNames { get; set; }
    
    /// <summary>
    /// Gets the mapping between original page object references and new page object references.
    /// </summary>
    public Dictionary<PDFObjectRef,PDFObjectRef> PageReferenceMappings { get; private set; }
    
    
    public PDFModifyPageReferenceList OutputPages { get; set; }

    public override int TotalPageCount
    {
        get { return this.OutputPages.Count; }
    }

    public PDFFramesetLayoutDocument(Document root, LayoutEngineFrameset engine) : base(root, engine)
    {
        this.OutputPages = new PDFModifyPageReferenceList();
        this.PageReferenceMappings = new Dictionary<PDFObjectRef, PDFObjectRef>();
    }

    protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
    {
        
        context.TraceLog.Begin(TraceLevel.Message, "Layout Document", "Outputting document to the PDFWriter");
        writer.OpenDocument(this.PrependFile, true);
        writer.WriteLine();
        writer.WriteCommentLine("--- START OF THE FRAMESET DOCUMENT ---");
        writer.WriteLine();
        PDFObjectRef catalog = this.WriteCatalog(context, writer);

        this.WriteInfo(context, writer);

        PDFDocumentID id = this.DocumentComponent.DocumentID;
        if (null == id)
            id = PDFDocumentID.Create();

        writer.CloseDocument(id);

        context.TraceLog.End(TraceLevel.Message, "Layout Document", "Completed output of the document to the PDFWriter");

        return catalog;
        
    }

    protected override void WriteCatalogEntries(PDFRenderContext context, PDFWriter writer)
    {
        base.WriteCatalogEntries(context, writer);

        if (null != this.SourceNames)
        {
            var namesObj = writer.BeginObject();
            writer.BeginDictionary();
            this.WriteSourceNames(context, writer);
            writer.EndDictionary();
            writer.EndObject();

            //back to the catalog
            writer.WriteDictionaryObjectRefEntry("Names", namesObj);

        }
        else if (null != ExistingCatalog)
        {
            //We dont have an explict source names list so we can just try and reference the original entry.
            var origNames = ExistingCatalog.Entries.TryGetEntry("Names", out var entry);

            if (origNames)
                writer.WriteDictionaryObjectRefEntry("Names",
                    entry.OriginalData as PDFObjectRef);
        }
    }


    /// <summary>
    /// Writes a name tree of the Destinations within the document from the sorted dictionary, that has been populated from the referenced files.
    /// </summary>
    /// <returns></returns>
    private bool WriteSourceNames(PDFRenderContext context, PDFWriter writer)
    {
        writer.BeginDictionaryEntry("Dests");
        
        var dests = writer.BeginObject();

        writer.BeginDictionary();
        
        writer.BeginDictionaryEntry("Names");
        
        string firstName = null;

        string lastName = null;
        
        //Name Tree - ordered list of a name with a destination
        //Followed by the first and last limits.
        writer.BeginArray();

        foreach (var str in this.SourceNames.Keys)
        {
            var name = str;
            var value = this.SourceNames[str];

            if (null == firstName)
            {
                firstName = name;
            }
            // we swap the value for a named destination with the new page reference
            // the array is populated when we output the pages (below)
            // if the updated page is not found, then it is a missing page, and we skip the link - (If we don't it causes error in Acrobat)
            if (value is PDFArray arry && arry.Count > 0)
            {
                var oref = arry[0] as PDFObjectRef;
                if (null != oref && this.PageReferenceMappings.TryGetValue(oref, out var newOref))
                    arry[0] = newOref;
                else
                {
                    context.TraceLog.Add(TraceLevel.Verbose, "Modifications", "Skipping the named destination for " + name + " as it refers to page " + oref +" that is no longer in the tree.");
                    continue;
                }
            }
            
            writer.BeginArrayEntry();
            writer.WriteStringLiteral(name);
            writer.EndArrayEntry();
            writer.BeginArrayEntry();
            
            
            
            
            value.WriteData(writer);
            writer.EndArrayEntry();

            lastName = name;

        }
        writer.EndArray();
        
        writer.EndDictionaryEntry(); //names
        
        writer.BeginDictionaryEntry("Limits");
        writer.WriteArrayStringEntries(true, firstName, lastName);
        writer.EndDictionaryEntry(); //limits
        
        writer.EndDictionary();
        writer.EndObject(); //dests
        
        writer.WriteObjectRef(dests);
        writer.EndDictionaryEntry();
        
        return false;
    }
    

    protected override PDFObjectRef OutputPageTree(PDFRenderContext context, PDFWriter writer)
    {
        return base.OutputPageTree(context, writer);
    }

    /// <summary>
    /// Flag to only raise the warning once per document, if duplicate pages are used through the frames.
    /// </summary>
    private bool warnedOfDuplicates = false;

    protected override List<PDFObjectRef> OutputAllPages(PDFObjectRef parent, PDFRenderContext context, PDFWriter writer)
    {
        List<PDFObjectRef> all = new List<PDFObjectRef>();
        
        foreach (var outputPage in this.OutputPages)
        {
            var oref = this.OutputModificationPageDictionary(parent, outputPage, context, writer);
            outputPage.NewPageRef = oref;

            if (this.PageReferenceMappings.TryGetValue(outputPage.OriginalPageRef, out var existingPg))
            {
                if (!warnedOfDuplicates && !existingPg.Equals(oref))
                {
                    context.TraceLog.Add(TraceLevel.Warning, "Modifications",
                        "There is already an existing entry for page " + existingPg +
                        " any links will refer to the original entry, rather than the new set of duplicate pages.");
                    warnedOfDuplicates = true;
                }
            }
            else
            {
                this.PageReferenceMappings.Add(outputPage.OriginalPageRef, oref);
            }

            all.Add(oref);
        }

        return all;
    }

    protected virtual PDFObjectRef OutputModificationPageDictionary(PDFObjectRef parent, PDFModifyPageReference modify,
        PDFRenderContext context, PDFWriter writer)
    {
        var dict = modify.PageDictionary.Clone();

        if (modify.HasOverlay)
        {
            try
            {


                var origContent = dict["Contents"];
                var overContent = modify.OverlayPageDictionary["Contents"];
                var array = new PDFArray(origContent, overContent);

                dict["Contents"] = array;

                foreach (var pdfName in modify.OverlayPageDictionary.Keys)
                {
                    if (!dict.ContainsKey(pdfName))
                        dict.Add(pdfName, modify.OverlayPageDictionary[pdfName]);
                    else if (pdfName.Value == "Resources")
                        this.CopyPageResources(modify.OverlayPageDictionary, dict, writer, modify);
                    else if (pdfName.Value == "Annots")
                        this.CopyPageAnnotations(modify.OverlayPageDictionary, dict, writer, modify);
                    else
                    {
                        //Just skip for unknown types in the page dictionary.
                    }

                }
            }
            catch (Exception ex)
            {
                if (context.Conformance == ParserConformanceMode.Strict)
                    throw new InvalidOperationException(
                        "Could not copy the page dictionary content for the page " + modify.OverlayPageReference +
                        ": " + ex.Message, ex);

                context.TraceLog.Add(TraceLevel.Error, "Modifications", "Could not copy the page dictionary for the overlay page " + modify.OverlayPageReference + " to the main page dictionary, restoring back to normal. : " + ex.Message, ex);
                dict = modify.PageDictionary;
            }

        }


        var pgRef = writer.BeginObject();
        dict["Parent"] = parent;
        dict.WriteData(writer);
        writer.EndObject();

        return pgRef;
    }


    private void CopyPageResources(PDFDictionary fromDict, PDFDictionary toDict, PDFWriter writer,
        PDFModifyPageReference forPage)
    {
        bool isReference = false;
        var toRsrc = toDict["Resources"];
        if (toRsrc is PDFObjectRef rsrcRef)
        {
            toRsrc = forPage.OriginalFile.GetContent(rsrcRef);
            isReference = true;
        }

        var rsrcDict = toRsrc as PDFDictionary;
        if (null == rsrcDict)
        {
            throw new NullReferenceException(
                "The Resources entry in the original file could not be loaded - expected a dictionary");
        }

        var fromRsrc = fromDict["Resources"];
        if (fromRsrc is PDFObjectRef rsrcRef2)
        {
            fromRsrc = forPage.OverlayFile.GetContent(rsrcRef2);
        }

        var rsrcDict2 = fromRsrc as PDFDictionary;
        if (null == rsrcDict2)
        {
            throw new NullReferenceException(
                "The Resources entry in the overlay file could not be loaded - expected a dictionary");
        }

        var newRsrc = new PDFDictionary();
        foreach (var entry in rsrcDict)
        {
            var data1 = entry.Value;

            if (rsrcDict2.TryGetValue(entry.Key, out var data2))
            {
                switch (entry.Key.Value)
                {
                    case "ProcSet":
                        // Do nothing
                        break;
                    case "Font":
                    case "XObject":
                    case "ExtGState":
                        //Copy to a new full single dictionary
                        data1 = MergeResourceReferenceDictionaries(data1, data2);
                        break;
                    default:
                        //no know way to copy - ignore
                        break;
                }
            }

            newRsrc.Add(entry.Key, data1);

        }

        if (isReference)
        {
            var oref = writer.BeginObject();
            newRsrc.WriteData(writer);
            writer.EndObject();
            toDict["Resources"] = oref;
        }
        else
        {
            toDict["Resources"] = newRsrc;
        }
    }

    private IPDFFileObject MergeResourceReferenceDictionaries(IPDFFileObject one, IPDFFileObject two)
    {
        var dict1 = one as PDFDictionary;
        if (null == dict1)
            return one;
        
        var dict2 = two as PDFDictionary;
        if (null == dict2)
            return one;

        PDFDictionary copy = new PDFDictionary();
        foreach (var entry in dict1)
        {
            copy.Add(entry.Key, entry.Value);
        }
        
        foreach (var entry in dict2)
        {
            if (!copy.ContainsKey(entry.Key))
                copy.Add(entry.Key, entry.Value);
        }

        return copy;
    }
    
    private void CopyPageAnnotations(PDFDictionary fromDict, PDFDictionary toDict, PDFWriter writer,
        PDFModifyPageReference forPage)
    {
        bool isReference = false;
        var toArray = toDict["Annots"];
        if (toArray is PDFObjectRef annotsRef)
        {
            toArray = forPage.OriginalFile.GetContent(annotsRef);
            isReference = true;
        }

        var annots = toArray as PDFArray;
        if (null == annots)
        {
            throw new NullReferenceException(
                "The Annotations entry in the original file could not be loaded - expected an array");
        }

        var newAnnots = new PDFArray();
        foreach (var item in annots)
        {
            var oref = item as PDFObjectRef;
            if(null != oref)
                newAnnots.Add(oref);
        }

        var fromArray = fromDict["Annots"];
        if (fromArray is PDFObjectRef annotsRef2)
            fromArray = forPage.OverlayFile.GetContent(annotsRef2);
        
        annots = fromArray as PDFArray;
        if(null == annots)
            throw new NullReferenceException(
                "The Annotations entry in the overlay file could not be loaded - expected an array");

        foreach (var item in annots)
        {
            var oref = item as PDFObjectRef;
            if(null != oref)
                newAnnots.Add(oref);
        }

        if (isReference)
        {
            var oref = writer.BeginObject();
            newAnnots.WriteData(writer);
            writer.EndObject();
            toDict["Annots"] = oref;
        }
        else
            toDict["Annots"] = newAnnots;

    }
    

    protected override void WriteInfo(PDFRenderContext context, PDFWriter writer)
    {
        base.WriteInfo(context, writer);
    }
}