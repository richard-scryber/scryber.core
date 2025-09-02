using System;
using System.IO;
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
    protected Document TopDocument { get; set; }
    
    public PDFFile PrependFile { get; private set; }
    
    public FrameTemplateFileReference(IComponent firstOwner, string fullpath) : base(firstOwner, FrameFileType.ReferencedTemplate, fullpath)
    {}

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

        return null != this.RemoteRequest;
    }
    
    private bool RemoteContentLoadedCallback(IComponent raiser, IRemoteRequest request, System.IO.Stream response)
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
            
            return success;
        }
        else
        {
            return false;
        }
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
