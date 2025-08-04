using System;
using System.IO;
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

    protected virtual bool FileLoaded(PDFFile file, ContextBase context)
    {
        this.ReferencedFile = file ?? throw new ArgumentNullException("The file cannot be null to register file loaded");
        this.Status = FrameFileStatus.Ready;
        return true;
    }
}

public class FramePDFFileReference : FrameFileReference
{

    protected IRemoteRequest RemoteRequest { get; set; }

    
    
    public FramePDFFileReference(string fullpath) : base(FrameFileType.DirectPDF, fullpath)
    {
    }

    protected override bool DoEnsureContentLoadedAndBound(Component owner, Document topDoc, DataContext dataContext)
    {
        TimeSpan cacheDuration  = TimeSpan.Zero;
        var callback = new RemoteRequestCallback(this.RemoteContentLoadedCallback);
        this.RemoteRequest  = topDoc.RegisterRemoteFileRequest(MimeType.Pdf.ToString(), this.FullPath, cacheDuration, callback, owner, dataContext);

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



