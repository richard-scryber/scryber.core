using System.Collections.Generic;
using Scryber.Components;
using Scryber.PDF.Native;
using Scryber.PDF.Parsing;

namespace Scryber.PDF.Layout;

public class PDFModifyLayoutDocument : PDFLayoutDocument
{

    public PDFFile OriginalFile { get; set; }
    
    public PDFParsedCatalog ExistingCatalog { get; set; }
    
    
    public PDFModifyPageReferenceList OutputPages { get; set; }
    
    public PDFModifyLayoutDocument(Document root, LayoutEngineFrameset engine) : base(root, engine)
    {
        this.OutputPages = new PDFModifyPageReferenceList();
    }

    protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
    {
        
        context.TraceLog.Begin(TraceLevel.Message, "Layout Document", "Outputting document to the PDFWriter");
        writer.OpenDocument(this.OriginalFile, true);
        writer.WriteLine();
        writer.WriteCommentLine("--- START OF THE NEXT DOCUMENT ---");
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
        
        writer.WriteDictionaryObjectRefEntry("Names", ExistingCatalog.Entries["Names"].OriginalData as PDFObjectRef);
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