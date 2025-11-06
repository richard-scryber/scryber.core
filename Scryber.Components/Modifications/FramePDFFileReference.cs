using System;
using System.IO;
using System.Threading.Tasks;
using Scryber.PDF.Native;
using Scryber.Components;

namespace Scryber.Modifications;


public class FramePDFFileReference : FrameFileReference
{

    protected IRemoteRequest RemoteRequest { get; set; }
    private Func<Task> FunctionCallback { get; set; }

    protected object ThreadLock = new object();
    
    public FramePDFFileReference(IComponent firstOwner, string fullpath) : base(firstOwner, FrameFileType.DirectPDF, fullpath)
    {
    }

    protected override PDFFile DoGetOrCreateFile(ContextBase context, PDFFile baseFile, Component owner, Document topDoc)
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

    protected override async Task<bool> DoEnsureContentAsync(Component owner, Document topDoc, PDFFile appendTo, ContextBase context, Func<Task> callback  )
    {
        TimeSpan cacheDuration  = TimeSpan.Zero;
        this.FunctionCallback = callback;
        
        var asyncRemotes = (RemoteFileAsyncRequestSet)topDoc.RemoteRequests;

        var remotecallback = new RemoteRequestCallback(this.RemoteContentLoadedCallbackAsync);
        this.RemoteRequest  = topDoc.RegisterRemoteFileRequest(MimeType.Pdf.ToString(), this.FullPath, cacheDuration, remotecallback, owner, context);

        await asyncRemotes.EnsureRequestsFullfilledAsync();
        
        return null != this.RemoteRequest;
    }

    private bool RemoteContentLoadedCallbackAsync(IComponent raiser, IRemoteRequest request, System.IO.Stream response)
    {
        var ms = new MemoryStream();
        response.CopyTo(ms);
        ms.Position = 0;
        response.Dispose();

        var result = RemoteContentLoadedCallback(raiser, request, ms);

        if (null != this.FunctionCallback)
        {
            this.FunctionCallback();
            this.FunctionCallback = null;
        }

        return result;

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