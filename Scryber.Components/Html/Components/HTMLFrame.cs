using System;
using System.IO;
using Scryber.Components;
using Scryber.Modifications;
using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.Styles;

namespace Scryber.Html.Components;

[PDFParsableComponent("frame")]
public class HTMLFrame : ContainerComponent, IPDFViewPortComponent, INamingContainer
{
    /// <summary>
    /// Default Zero page index from the source to start inserting into the resultant document
    /// </summary>
    public const int AppendPageIndex = 0;
    
    /// <summary>
    /// Default all page count to insert into the resultant document.
    /// </summary>
    public const int AppendAllPageCount = int.MaxValue;
        

    /// <summary>
    /// Gets or sets the path to the pdf or the template document to use as the source for this frame.
    /// </summary>
    [PDFAttribute("src")]
    public string RemoteSource { get; set; }
        
    /// <summary>
    /// Gets or sets the Zero-Based index of the page in the document to include within the resultant document.
    /// </summary>
    [PDFAttribute("data-page-start")]
    public int PageStartIndex { get; set; }
        
    /// <summary>
    /// Gets or sets the Total count of pages from the document to include withing the resultant document.
    /// </summary>
    [PDFAttribute("data-page-count")]
    public int PageInsertCount { get; set; }

    [PDFElement("html")]
    public HTMLDocument InnerHtml { get; set; }

    [PDFAttribute("hidden")]
    public string Hidden
    {
        get
        {
            if (this.Visible)
                return string.Empty;
            else
                return "hidden";
        }
        set
        {
            if (string.Equals(value, "hidden"))
                this.Visible = false;
            else
                this.Visible = true;
        }
    }
        
    [PDFAttribute("mime-type")]
    public MimeType RemoteSourceMimeType { get; set; }
    
    
    
    
    /// <summary>
    /// Gets or sets the reference to the file referenced by this frame.
    /// </summary>
    public Modifications.FrameFileReference FileReference { get; set; }


    private ModificationType _modType = ModificationType.None;
        
    public ModificationType ModificationType
    {
        get { return _modType; }
        protected set { _modType = value; }
    }

    IComponent INamingContainer.Owner
    {
        get { return this.Document; }
        set
        {
            //Do nothing
        }
    }
        
    public HTMLFrame() : this(ObjectTypes.ModifyFrame)
    {}

    protected HTMLFrame(ObjectType type) 
        : base(type)
    {
        this.PageStartIndex = AppendPageIndex;
        this.PageInsertCount = AppendAllPageCount;
    }
    
    
    protected override void OnInitialized(InitContext context)
    {
        base.OnInitialized(context);

        // this.EnsureRemoteContent(context);
    }

    protected override void OnLoaded(LoadContext context)
    {
        base.OnLoaded(context);
    }

    protected override void OnDataBinding(DataContext context)
    {
        base.OnDataBinding(context);
    }

    protected override void OnDataBound(DataContext context)
    {
        base.OnDataBound(context);
            
        this.EnsureModificationType(context);
    }

    

    // protected virtual void EnsureRemoteContent(ContextBase context)
    // {
    //     if (!string.IsNullOrEmpty(this.RemoteSource))
    //     {
    //         if (null == this.RemoteFileLodRegistration)
    //         {
    //             string mimeType = InferResourceType(this.RemoteSourceMimeType);
    //             TimeSpan cacheDuration = TimeSpan.Zero;
    //             var callback = new RemoteRequestCallback(this.RemoteContentLoadedCallback);
    //             
    //             this.RemoteFileLodRegistration = this.Document.RegisterRemoteFileRequest(mimeType, RemoteSource, cacheDuration, callback, this, null);
    //         }
    //     }
    // }
    //
    // private bool RemoteContentLoadedCallback(IComponent raiser, IRemoteRequest request, Stream response)
    // {
    //     return false;
    //
    // }

    
    
    
    /// <summary>
    /// Checks and validates the remote sources any content, and also the page indexes 
    /// </summary>
    /// <param name="context"></param>
    protected virtual void EnsureModificationType(ContextBase context)
    {
        if (this.Visible)
        {
            if (string.IsNullOrEmpty(this.RemoteSource))
            {
                if (null == this.Document)
                {
                    this.ModificationType = ModificationType.None;
                    return; //we have nothing so there is nothing to do.
                }
                else //we have a inline html document.
                {
                    if (this.PageStartIndex == HTMLFrame.AppendAllPageCount)
                        this.ModificationType = ModificationType.Append; //We are at the end of the processing document.
                    else
                        this.ModificationType = ModificationType.Insert; //We should be inserted as we have an explicit page index
                }
            }
            else //we have a remote source
            {
                if (null != this.InnerHtml)
                {
                    this.ModificationType = ModificationType.Append;
                }
                else //just a remote source
                {
                    if (this.PageStartIndex == HTMLFrame.AppendAllPageCount)
                        this.ModificationType = ModificationType.Append; //We are at the end of the processing document.
                    else
                        this.ModificationType = ModificationType.Insert; //We should be inserted as we have an explicit page index
                }
            }
        }
        else
        {
            this.ModificationType = ModificationType.None;
        }
    }

    public IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style fullstyle)
    {
        if (null != this.FileReference)
        {
            var framesetEngine = (LayoutEngineFrameset)parent;
            if (this.FileReference.FileType == FrameFileType.DirectPDF)
            {
                
                return new LayoutEngineRootPDFFrame(framesetEngine, this, this.FileReference.FrameFile, context);
            }
            else if (this.FileReference.FileType == FrameFileType.ReferencedTemplate)
            {
                var file = framesetEngine.Frameset.CurrentFile;
                return new LayoutEngineTemplateFrame(framesetEngine, this, file, context);
            }
            else if (this.FileReference.FileType == FrameFileType.ContainedTemplate)
            {
                var file = framesetEngine.Frameset.CurrentFile;
                return new LayoutEngineTemplateFrame(framesetEngine, this, file, context);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Unknown reference file type for the frame");
            }
        }
        else if (null != this.InnerHtml)
        {
            var framesetEngine = (LayoutEngineFrameset)parent;
            var file = framesetEngine.Frameset.CurrentFile;
            return new LayoutEngineTemplateFrame(framesetEngine, this, file, context);
        }
        else if(this.Visible)
            throw new InvalidOperationException("The frame must be contained within a frameset to layout the pages");
        else
        {
            context.TraceLog.Add(TraceLevel.Message, "Modifications" ,"Not laying out " + this.ID + " as the frame is hidden");
            return null;
        }
    }
}

public class HTMLFrameList : ComponentWrappingList<HTMLFrame>
{

    public HTMLFrameList(ComponentList innerList) : base(innerList)
    {
            
    }
}