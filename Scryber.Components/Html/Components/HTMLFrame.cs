using Scryber.Components;

namespace Scryber.Html.Components;

[PDFParsableComponent("frame")]
public class HTMLFrame : ContainerComponent
{
    public const int AppendPageIndex = int.MaxValue;
    public const int AppendAllPageCount = int.MaxValue;
        

    [PDFAttribute("src")]
    public string RemoteSource { get; set; }
        
    [PDFAttribute("page-start")]
    public int PageStartIndex { get; set; }
        
    [PDFAttribute("page-count")]
    public int PageInsertCount { get; set; }

    [PDFElement("html")]
    public HTMLDocument InnerHtml { get; set; }

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
        


    private ModificationType _modType = ModificationType.None;
        
    public ModificationType ModificationType
    {
        get { return _modType; }
        protected set { _modType = value; }
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
                    
            }
        }
        else
        {
            this.ModificationType = ModificationType.None;
        }
    }
}

public class HTMLFrameList : ComponentWrappingList<HTMLFrame>
{

    public HTMLFrameList(ComponentList innerList) : base(innerList)
    {
            
    }
}