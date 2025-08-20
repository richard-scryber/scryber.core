using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.PDF.Native;
using Scryber.Styles;

namespace Scryber.Modifications;



public abstract class FrameFileReference
{
    public FrameFileType FileType { get; private set; }

    public string FullPath { get; private set; }
    
    public FrameFileStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the PDFFile for this frame
    /// </summary>
    public Scryber.PDF.Native.PDFFile FrameFile { get; set; }
    
    protected FrameFileReference(FrameFileType type, string fullpath)
    {
        this.FileType = type;
        this.FullPath = fullpath;
        this.Status = FrameFileStatus.NotLoaded;
    }

    public abstract PDFFile GetOrCreateFile(ContextBase context, PDFFile baseFile, Component owner, Document topDoc);

    public virtual bool EnsureContent(Component owner, Document topDoc, PDFFile appendTo, ContextBase context)
    {
        bool result = true;
        
        this.Status = FrameFileStatus.Loading;

        if (!this.DoEnsureContent(owner, topDoc, appendTo, context))
        {
            this.Status = FrameFileStatus.Invalid;
            result = false;
        }

        return result;
    }

    protected virtual bool DoEnsureContent(Component owner, Document topDoc, PDFFile appendTo, ContextBase context)
    {
        return false;
    }

    protected virtual bool FileLoaded(PDFFile file, ContextBase context)
    {
        this.FrameFile = file ?? throw new ArgumentNullException("The file cannot be null to register file loaded");
        this.Status = FrameFileStatus.Ready;
        return true;
    }
}

public class FramePDFFileReference : FrameFileReference
{

    protected IRemoteRequest RemoteRequest { get; set; }

    protected object ThreadLock = new object();
    
    public FramePDFFileReference(string fullpath) : base(FrameFileType.DirectPDF, fullpath)
    {
    }

    public override PDFFile GetOrCreateFile(ContextBase context, PDFFile baseFile, Component owner, Document topDoc)
    {
        lock (this.ThreadLock)
        {
            if (null == this.FrameFile)
            {
                if (null == this.RemoteRequest)
                {
                    this.DoEnsureContent(owner, topDoc, baseFile, context);
                }
            }
            
        }
        
        if(topDoc.RemoteRequests.ExecMode == DocumentExecMode.Asyncronous || topDoc.RemoteRequests.ExecMode == DocumentExecMode.Phased)
            topDoc.RemoteRequests.EnsureRequestsFullfilled();

        return this.FrameFile;

    }

    protected override bool DoEnsureContent(Component owner, Document topDoc, PDFFile appendTo, ContextBase context)
    {
        TimeSpan cacheDuration  = TimeSpan.Zero;
        var callback = new RemoteRequestCallback(this.RemoteContentLoadedCallback);
        this.RemoteRequest  = topDoc.RegisterRemoteFileRequest(MimeType.Pdf.ToString(), this.FullPath, cacheDuration, callback, owner, context);

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
                
                DataContext context = (DataContext)request.Arguments;
                file = PDFFile.Load(response, context.TraceLog);
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
}

public class FrameTemplateFileReference : FrameFileReference
{
    
    protected IRemoteRequest RemoteRequest { get; set; }
    protected Document TopDocument { get; set; }
    
    public PDFFile PrependFile { get; private set; }
    
    public FrameTemplateFileReference(string fullpath) : base(FrameFileType.ReferencedTemplate, fullpath)
    {}

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
            return base.DoEnsureContent(owner, topDoc, appendTo, context);
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
}

public class FrameContentTemplateReference : FrameFileReference
{
    public Components.Document Document { get; set; }

    public FrameContentTemplateReference(Scryber.Components.Document content) : base(FrameFileType.ContainedTemplate,
        string.Empty)
    {
        this.Document = content ?? throw new ArgumentNullException("The document content cannot be null");
    }

    public override PDFFile GetOrCreateFile(ContextBase context, PDFFile baseFile, Component owner, Document topDoc)
    {
        throw new NotImplementedException();
    }
}



