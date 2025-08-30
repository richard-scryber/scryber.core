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
    
    
    public PDFModifyPageReferenceList OutputPages { get; set; }
    
    public PDFFramesetLayoutDocument(Document root, LayoutEngineFrameset engine) : base(root, engine)
    {
        this.OutputPages = new PDFModifyPageReferenceList();
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
        else
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

    protected override List<PDFObjectRef> OutputAllPages(PDFObjectRef parent, PDFRenderContext context, PDFWriter writer)
    {
        List<PDFObjectRef> all = new List<PDFObjectRef>();
        
        foreach (var outputPage in this.OutputPages)
        {
            all.Add(outputPage.OriginalPageRef);
        }

        return all;
    }
    

    protected override void WriteInfo(PDFRenderContext context, PDFWriter writer)
    {
        base.WriteInfo(context, writer);
    }
}