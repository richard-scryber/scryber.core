using System;
using System.IO;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.PDF.Native;


namespace Scryber.Modifications;

/// <summary>
/// A frame content reference for content that is loaded remotely as a template from another source. This is usually done directly by the HTMLFrame which uses this class.
/// </summary>
public class FrameTemplateFileReference : FrameFileReference
{
    /// <summary>
    /// Gets or sets the actual document that was loaded remotely by this reference
    /// </summary>
    public HTMLDocument ParsedDocument { get; set; }
    protected IRemoteRequest RemoteRequest { get; set; }
    
    private Func<Task> FunctionCallback { get; set; }
    protected Document TopDocument { get; set; }
    
    public PDFFile PrependFile { get; private set; }
    


    public FrameTemplateFileReference(IComponent firstOwner, string fullpath) : base(firstOwner,
        FrameFileType.ReferencedTemplate, fullpath)
    {
        var frame = firstOwner as HTMLFrame;
        if (null != frame && null != frame.Parent)
        {
            var frameset = frame.Parent as HTMLFrameset;
        }

    }

    protected override PDFFile DoGetOrCreateFile(ContextBase context, PDFFile prependFile, Component owner, Document topDoc)
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

    private bool LoadTemplateContent(ContextBase context, PDFFile prependFile, Component owner, Document topDoc)
    {
        this.TopDocument = topDoc;
        this.PrependFile = prependFile;
        TimeSpan cacheDuration  = TimeSpan.Zero;
        var callback = new RemoteRequestCallback(this.RemoteContentLoadedCallback);
        this.RemoteRequest  = topDoc.RegisterRemoteFileRequest(MimeType.Html.ToString(), this.FullPath, cacheDuration, callback, owner, context);
        
        if(context.ShouldLogVerbose)
            context.TraceLog.Add(TraceLevel.Verbose, "Modifications", "The remote request has bee registered for the template " + this.FullPath);
        
        return null != this.RemoteRequest;
    }
    
    private bool RemoteContentLoadedCallback(IComponent raiser, IRemoteRequest request, System.IO.Stream response)
    {
        if (null != response)
        {
            bool success = false;
            
                PDF.Native.PDFFile file = null;
                Exception error = null;

                if (request.IsCompleted)
                {
                    //Already done - don't process again.
                    success = request.IsSuccessful;
                }
                else
                {
                    try
                    {
                        //We need a seekable stream
                        if (!response.CanSeek)
                        {
                            var ms = new MemoryStream();
                            response.CopyTo(ms);
                            ms.Position = 0;

                            response.Dispose();
                            response = ms;
                        }

                        ContextBase context = (ContextBase)request.Arguments;

                        var doc = Document.ParseHtmlDocument(response, request.FilePath, ParseSourceType.RemoteFile);
                        doc.PrependedFile = this.PrependFile;
                        doc.AppendTraceLog = false;
                        doc.Parent = this.Owner as Component;
                        this.ParsedDocument = doc as HTMLDocument;

                        this.InitDoc(doc, context);
                        this.LoadDoc(doc, context);
                        this.BindDoc(doc, context);

                        var stream = new MemoryStream();

                        doc.PrependedFile = this.PrependFile;
                        doc.RenderToPDF(stream);
                        stream.Flush();

                        stream.Position = 0;
                        file = PDFFile.Load(stream, context.TraceLog);



                        success = this.FileLoaded(file, context);

                    }
                    catch (Exception ex)
                    {
                        success = false;
                        error = ex;
                    }

                    request.CompleteRequest(file, success, error);
                }

                return success;
        }
        else
        {
            return false;
        }
    }
    
    protected virtual void InitDoc(Document doc, ContextBase baseContext)
    {
        var initContext = new InitContext(baseContext.Items, baseContext.TraceLog, baseContext.PerformanceMonitor, doc,
            baseContext.Format);
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
    
    //
    // async methods
    //

    protected override async Task<bool> DoEnsureContentAsync(Component owner, Document topDoc, PDFFile appendTo,
        ContextBase context, Func<Task> callback)
    {
        if (null == this.FrameFile)
        {
            this.PrependFile = appendTo;
            this.FunctionCallback = callback;
            
            if (await this.LoadTemplateContentAsync(context, appendTo, owner, topDoc))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
            return true;
    }
    
    private async Task<bool> LoadTemplateContentAsync(ContextBase context, PDFFile prependFile, Component owner, Document topDoc)
    {
        this.TopDocument = topDoc;
        this.PrependFile = prependFile;
        TimeSpan cacheDuration  = TimeSpan.Zero;
        var callback = new RemoteRequestCallback(this.RemoteContentLoadedCallbackAsync);
        this.RemoteRequest  = topDoc.RegisterRemoteFileRequest(MimeType.Html.ToString(), this.FullPath, cacheDuration, callback, owner, context);
        
        if(context.ShouldLogVerbose)
            context.TraceLog.Add(TraceLevel.Verbose, "Modifications", "The remote request has bee registered ASYNCHRONOUSLY for the template " + this.FullPath);

        var requestsAsync = (RemoteFileAsyncRequestSet)topDoc.RemoteRequests;
        await requestsAsync.EnsureRequestsFullfilledAsync();
        
        return null != this.RemoteRequest;
    }

    private bool RemoteContentLoadedCallbackAsync(IComponent raiser, IRemoteRequest request, System.IO.Stream response)
    {
        if (null != response)
        {
            bool success = false;

            PDF.Native.PDFFile file = null;
            Exception error = null;
            try
            {
                //We need a seekable stream
                if (!response.CanSeek)
                {
                    var ms = new MemoryStream();
                    response.CopyTo(ms);
                    ms.Position = 0;

                    response.Dispose();
                    response = ms;
                }

                ContextBase context = (ContextBase)request.Arguments;

                var doc = Document.ParseHtmlDocument(response, request.FilePath, ParseSourceType.RemoteFile);
                //string name = new UriBuilder(request.FilePath).Path;
                //name = System.IO.Path.GetFileName(name);
                var log = new FrameTraceLogCollector("Frame" + this.DocumentFileIndex, context.TraceLog);
                ((IParsedDocument)doc).SetTraceLog(log);
                doc.TraceLog.SetRecordLevel(context.TraceLog.RecordLevel);
                doc.RemoteRequests = new RemoteFileAsyncRequestSet(doc);
                doc.CacheProvider = ((Document)context.Document).CacheProvider;
                doc.PrependedFile = this.PrependFile;
                doc.AppendTraceLog = false; //all log entries will be captured by the outer tracelog.

                doc.Parent = this.Owner as Component;
                this.ParsedDocument = doc as HTMLDocument;

                Task.Run(async () =>
                {
                    success = await SaveTemplateDocumentAsync(context, this.ParsedDocument, request);

                    if (null != this.FunctionCallback)
                    {
                        await this.FunctionCallback();
                    }

                });

                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                throw new PDFLayoutException(
                    "Could not load the referenced template file from " + request.FilePath +
                    " - see the inner exception for more details", ex);
            }



            return success;
        }
        else
        {
            return false;
        }
    }

    private async Task<bool> SaveTemplateDocumentAsync(ContextBase baseContext, Document processingDocument, IRemoteRequest request)
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
            
            request.CompleteRequest(file, result, null);
        }
        catch (Exception e)
        {
            request.CompleteRequest(null, result, new InvalidOperationException(
                "The asynchronous processing of the template document could not be completed. See the inner exception for more details.",
                e));
        }

        return result;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if(null != this.ParsedDocument)
                this.ParsedDocument.Dispose();
        }
        
        base.Dispose(disposing);
    }
}
