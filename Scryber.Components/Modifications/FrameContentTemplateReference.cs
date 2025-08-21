using System;
using System.IO;
using Scryber.Components;
using Scryber.PDF.Native;



namespace Scryber.Modifications;


public class FrameContentTemplateReference : FrameFileReference
{
    public Document InnerDocument { get; set; }
    
    protected Document TopDocument { get; set; }
    
    public PDFFile PrependFile { get; private set; }

    public FrameContentTemplateReference(Scryber.Components.Document content) : base(FrameFileType.ContainedTemplate,
        string.Empty)
    {
        this.InnerDocument = content ?? throw new ArgumentNullException("The document content cannot be null");
    }

    public override PDFFile GetOrCreateFile(ContextBase context, PDFFile prependFile, Component owner, Document topDoc)
    {
        if (null == this.FrameFile)
        {
            this.LoadTemplateContent(context, prependFile, owner, topDoc);
            
        }

        return this.FrameFile;
    }

    protected override bool DoEnsureContent(Component owner, Document topDoc, PDFFile appendTo, ContextBase context)
    {
        if (null == this.FrameFile)
        {
            this.PrependFile = appendTo;
            return this.LoadTemplateContent(context, appendTo, owner, topDoc);
        }
        else
            return true;
    }

    protected bool LoadTemplateContent(ContextBase context, PDFFile baseFile, Component owner, Document topDoc)
    {
        this.TopDocument = topDoc;
        bool success = false;
        PDF.Native.PDFFile file = null;
        MemoryStream stream = null;
        try
        {
            this.PrependFile = baseFile;
            var doc = this.InnerDocument ??
                      throw new ArgumentNullException("The inner document content cannot be null");

            this.InitDoc(doc, context);
            this.LoadDoc(doc, context);
            this.BindDoc(doc, context);

            stream = new MemoryStream(); //Don't dispose as used in the PDF file.
            doc.PrependedFile = this.PrependFile;
            doc.AppendTraceLog = false; //Using the outer document

            doc.RenderToPDF(stream);
            stream.Flush();
            stream.Position = 0;
            
            file = PDFFile.Load(stream, context.TraceLog);

            success = this.FileLoaded(file, context);

        }
        catch (Exception ex)
        {
            if(null != stream)
                stream.Dispose();

            throw new Scryber.PDFLayoutException("Could not create the fame content from the inner template", ex);
        }

        return success;
    }

    protected virtual void InitDoc(Document doc, ContextBase baseCOntext)
    {
        var initContext = new InitContext(baseCOntext.Items, baseCOntext.TraceLog, baseCOntext.PerformanceMonitor, doc,
            baseCOntext.Format);
        doc.Init(initContext);
    }

    protected virtual void LoadDoc(Document doc, ContextBase baseContext)
    {
        var loadContext = new LoadContext(baseContext.Items, baseContext.TraceLog, baseContext.PerformanceMonitor, doc,
            baseContext.Format);
        doc.Load(loadContext);
    }

    protected virtual void BindDoc(Document doc, ContextBase baseContext)
    {
        var dataContext = new DataContext(baseContext.Items, baseContext.TraceLog, baseContext.PerformanceMonitor, doc,
            baseContext.Format);
        doc.Params.Merge(this.TopDocument.Params);
        doc.DataBind(dataContext);
    }
}