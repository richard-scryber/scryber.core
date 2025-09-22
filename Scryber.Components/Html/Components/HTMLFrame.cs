using System;
using System.IO;
using System.Runtime.CompilerServices;
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
    /// Wraps the 3 properties into a single instance,
    /// as they are used together, but mimimal memory requirements.
    /// </summary>
    private Data.DataBindingContent _dataContent;
    
    /// <summary>
    /// Gets or sets the data content of this container as a string.
    /// Used for binding content to the Visual Container
    /// </summary>
    [PDFAttribute("data-content")]
    public virtual string DataContent
    {
        get
        {
            if (null == _dataContent)
                return string.Empty;
            else
                return _dataContent.Content;
        }
        set {
            if (null == _dataContent)
                _dataContent = DoCreateDataBindingContent();
            _dataContent.Content = value;
        }
    }
    
    /// <summary>
    /// Returns an initialized instance of the DataBindingContent for this component, inheritors can override to set their own defaults.
    /// </summary>
    /// <returns></returns>
    protected virtual Data.DataBindingContent DoCreateDataBindingContent()
    {
        return new Data.DataBindingContent(string.Empty, MimeType.xHtml, DataContentAction.Replace);
    }
    
    
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


    protected override void DoDataBind(DataContext context, bool includeChildren)
    {
        base.DoDataBind(context, includeChildren);
        
        if (null != this._dataContent && !string.IsNullOrEmpty(this._dataContent.Content))
        {
            this.DoBindContentIntoComponent(this._dataContent, context);
        }
        
    }

    protected override void OnDataBound(DataContext context)
    {
        base.OnDataBound(context);
            
        this.EnsureModificationType(context);
    }

    

    #region protected virtual void DoBindContentIntoComponent(Data.DataBindingContent data, DataContext context)

        /// <summary>
        /// Based on the DataBindingContent, this will parse the inner content, and add as specified the inner content
        /// </summary>
        /// <param name="data"></param>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected virtual void DoBindContentIntoComponent(Data.DataBindingContent data, DataContext context)
        {
            if (null == data)
                throw new ArgumentNullException(nameof(data));

            if (string.IsNullOrEmpty(data.Content))
            {
                if (context.ShouldLogVerbose)
                    context.TraceLog.Add(TraceLevel.Warning, "Binding", "Binding data-content value was null or empty on the '" + this.ID + " component, even though other options had been set");

                return;
            }


            if (null == data.Type)
                data.Type = this.Document.GetDefaultContentMimeType();

            //Load the parser from the document - will throw an error if not known
            var parser = this.Document.EnsureParser(data.Type);

            if (context.ShouldLogVerbose)
                context.TraceLog.Add(TraceLevel.Verbose, "Binding", "Binding data-content value onto the '" + this.ID + " component, with mime-type " + data.Type.ToString());

            using (var sr = new System.IO.StringReader(data.Content))
            {
                var component = parser.Parse(null, sr, ParseSourceType.Template);

                if (null == component)
                {
                    if (context.Conformance == ParserConformanceMode.Lax)
                    {
                        context.TraceLog.Add(TraceLevel.Warning, "Binding", "No component was returned for the binding of the frame " + this.ID);
                    }
                    else
                    {
                        throw new NullReferenceException(
                            "No content was returned for the binding of the data-content for frame " + this.ID);
                    }
                }
                var document = component as HTMLDocument;
                
                
                //We clear out even if the returned content is null
                if (null != document)
                {
                    this.InnerHtml = document;
                    
                    document.InitializeAndLoad(context.Format);
                    document.DataBind(context);
                }
                else if (context.ShouldLogVerbose)
                {
                    if (context.Conformance == ParserConformanceMode.Lax)
                        context.TraceLog.Add(TraceLevel.Error, "Binding",
                            "The parsed component that was returned from data-content value for frame " + this.ID +
                            " was not an HTMLDocument. Only fully qualified XHTML source documents are able to be used as dynamic content within a frame");
                    else
                    {
                        throw new NullReferenceException(
                            "The parsed component that was returned from data-content value for frame " + this.ID +
                            " was not an HTMLDocument. Only fully qualified XHTML source documents are able to be used as dynamic content within a frame");
                    }
                }
            }
        }

        #endregion

    
    
    
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
        else if (this.Visible)
        {
            if (context.Conformance == ParserConformanceMode.Lax)
                context.TraceLog.Add(TraceLevel.Warning, "Modifications",
                    "Not laying out " + this.ID + " as the frame has no content");
            else
                throw new PDFLayoutException(
                    "In strict mode, a frame must contain at lease one of a resolved source value or an inner content, or it should be marked as hidden.");

            return null;
        }
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