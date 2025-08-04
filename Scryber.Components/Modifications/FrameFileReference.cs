using System;
using Scryber.Components;
using Scryber.PDF.Native;

namespace Scryber.Modifications;



public abstract class FrameFileReference
{
    public FrameFileType FileType { get; private set; }

    public string FullPath { get; private set; }
    
    public FrameFileStatus Status { get; set; }
    
    public Scryber.PDF.Native.PDFFile ReferencedFile { get; set; }
    
    protected FrameFileReference(FrameFileType type, string fullpath)
    {
        this.FileType = type;
        this.FullPath = fullpath;
        this.Status = FrameFileStatus.NotLoaded;
    }

    public bool EnsureContentLoadedAndBound(Component owner, Document topDoc, DataContext dataContext)
    {
        bool result = true;
        
        this.Status = FrameFileStatus.Loading;

        if (!this.DoEnsureContentLoadedAndBound(owner, topDoc, dataContext))
        {
            this.Status = FrameFileStatus.Invalid;
            result = false;
        }

        return result;
    }

    protected virtual bool DoEnsureContentLoadedAndBound(Component owner, Document topDoc, DataContext dataContext)
    {
        return false;
    }

    protected virtual void FileLoaded(PDFFile file, ContextBase context)
    {
        this.ReferencedFile = file ?? throw new ArgumentNullException("The file cannot be null to register file loaded");
        this.Status = FrameFileStatus.Ready;
    }
}

public class FramePDFFileReference : FrameFileReference
{

    protected IRemoteRequest RemoteRequest { get; set; }

    private DataContext _context;
    
    public FramePDFFileReference(string fullpath) : base(FrameFileType.DirectPDF, fullpath)
    {
    }

    protected override bool DoEnsureContentLoadedAndBound(Component owner, Document topDoc, DataContext dataContext)
    {
        TimeSpan cacheDuration  = TimeSpan.Zero;
        var callback = new RemoteRequestCallback(this.RemoteContentLoadedCallback);
        this._context = dataContext;
        this.RemoteRequest  = topDoc.RegisterRemoteFileRequest(MimeType.Pdf.ToString(), this.FullPath, cacheDuration, callback, owner, null);

        return null != this.RemoteRequest;
    }
    
    private bool RemoteContentLoadedCallback(IComponent raiser, IRemoteRequest request, System.IO.Stream response)
    {
        if (request.IsCompleted && request.IsSuccessful)
        {
            PDF.Native.PDFFile file = PDFFile.Load(response, _context.TraceLog);
            this.FileLoaded(file, _context);

            return null != file;
        }
        else
        {
            return false;
        }
    }
}

public class FrameTemplateFileReference : FrameFileReference
{
    public FrameTemplateFileReference(string fullpath) : base(FrameFileType.ReferencedTemplate, fullpath)
    {}
}

public class FrameContentTemplateReference : FrameFileReference
{
    public Components.Document Document { get; set; }

    public FrameContentTemplateReference(Scryber.Components.Document content) : base(FrameFileType.ContainedTemplate,
        string.Empty)
    {
        this.Document = content ?? throw new ArgumentNullException("The document content cannot be null");
    }
}



