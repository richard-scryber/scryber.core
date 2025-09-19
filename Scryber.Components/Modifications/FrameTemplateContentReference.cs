using System;
using System.IO;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.PDF.Native;
using Document = Scryber.Components.Document;


namespace Scryber.Modifications;


/// <summary>
/// A frame content reference for content that is not loaded from another source but passed directly to the reference in the constructor. This is usually done directly by the HTMLFrame which uses this class.
/// </summary>
public class FrameTemplateContentReference : FrameFileReference
{
    /// <summary>
    /// Gets or sets the actual document to be processed
    /// </summary>
    public Document InnerDocument { get; set; }
    
    protected Document TopDocument { get; set; }
    
    public PDFFile PrependFile { get; private set; }

    private Func<Task> FunctionCallback { get; set; }
    
    public FrameTemplateContentReference(IComponent owner, Document content) : base(owner,FrameFileType.ContainedTemplate,
        string.Empty)
    {
        this.InnerDocument = content ?? throw new ArgumentNullException("The document content cannot be null");
    }

    protected override PDFFile DoGetOrCreateFile(ContextBase context, PDFFile prependFile, Component owner, Document topDoc)
    {
        if (null == this.FrameFile)
        {
            this.LoadTemplateContent(context, prependFile, owner, topDoc);
        }

        return this.FrameFile;
    }
    
    
    #region synchronous operation

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

            this.InnerDocument.Parent = this.Owner as Component;
            
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
    
    #endregion
    
    #region asynchronous operation

    protected override async Task<bool> DoEnsureContentAsync(Component owner, Document topDoc, PDFFile appendTo, ContextBase context, Func<Task> callback)
    {
        
        if (null == this.FrameFile)
        {
            this.PrependFile = appendTo;
            this.TopDocument = topDoc;
            this.FunctionCallback = callback;

            var success = await this.RenderTemplateDocumentAsync(context, this.InnerDocument);

            if (success && null != this.FunctionCallback)
            {
                await this.FunctionCallback();
            }

            return success;
        }
        else
        {
            return true;
        }
    }

    protected virtual async Task<bool> RenderTemplateDocumentAsync(ContextBase context, Document doc)
    {
        var log = new FrameTraceLogCollector("Frame" + this.DocumentFileIndex, context.TraceLog);
        ((IParsedDocument)doc).SetTraceLog(log);
        doc.TraceLog.SetRecordLevel(context.TraceLog.RecordLevel);
        doc.RemoteRequests = new RemoteFileAsyncRequestSet(doc);
        doc.CacheProvider = ((Document)context.Document).CacheProvider;
        doc.PrependedFile = this.PrependFile;
        doc.AppendTraceLog = false;

        bool success = await SaveTemplateDocumentAsync(context, doc);

        if (null != this.FunctionCallback)
        {
            await this.FunctionCallback();
        }
        
        return success;
    }

    private async Task<bool> SaveTemplateDocumentAsync(ContextBase baseContext, Document processingDocument)
    {
        var remotes = (RemoteFileAsyncRequestSet)processingDocument.RemoteRequests;
        
        var initContext = new InitContext(baseContext.Items, baseContext.TraceLog, baseContext.PerformanceMonitor, processingDocument,
            baseContext.Format);
        
        var loadContext = new LoadContext(baseContext.Items, baseContext.TraceLog, baseContext.PerformanceMonitor, processingDocument,
            baseContext.Format);
        
        var dataContext = new DataContext(baseContext.Items, baseContext.TraceLog, baseContext.PerformanceMonitor, processingDocument,
            baseContext.Format);
        
        processingDocument.Params.Merge(this.TopDocument.Params);
        
        var result = false;

        var outputStream = new MemoryStream();
        try
        {
            if (processingDocument.GenerationStage < DocumentGenerationStage.Initialized)
                processingDocument.Init(initContext);

            await remotes.EnsureRequestsFullfilledAsync();

            if (processingDocument.GenerationStage < DocumentGenerationStage.Loaded)
                processingDocument.Load(loadContext);
            
            await remotes.EnsureRequestsFullfilledAsync();

            if (processingDocument.GenerationStage < DocumentGenerationStage.Bound)
                processingDocument.DataBind(dataContext);
            
            await remotes.EnsureRequestsFullfilledAsync();
            
            processingDocument.RenderToPDF(outputStream);
            
            
            outputStream.Flush();
                
            outputStream.Position = 0;
            PDFFile file = PDFFile.Load(outputStream, baseContext.TraceLog);
                
            result = this.FileLoaded(file, baseContext);
            
        }
        catch (Exception e)
        {
            if(baseContext.Conformance == ParserConformanceMode.Lax)
                baseContext.TraceLog.Add(TraceLevel.Error, "Modifications", "The processing of the Frame content failed with message : " + e.Message, e);
            else
            {
                throw new InvalidOperationException(
                    "Could not processes the frame template content, failed with message " + e.Message, e);
            }
        }

        return result;
    }

    #endregion

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (null != this.InnerDocument)
                this.InnerDocument.Dispose();
        }

        base.Dispose(disposing);
    }
}