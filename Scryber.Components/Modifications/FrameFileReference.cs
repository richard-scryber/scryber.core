using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.PDF.Native;
using Scryber.Styles;

namespace Scryber.Modifications;



public abstract class FrameFileReference : IDisposable
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

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if(null != this.FrameFile)
                this.FrameFile.Dispose();
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
    }

    ~FrameFileReference()
    {
        this.Dispose(false);
    }
}





