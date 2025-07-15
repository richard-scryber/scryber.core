using Scryber.Components;

namespace Scryber.Html.Components;

[PDFParsableComponent("frameset")]
public class HTMLFrameset : ContainerComponent
{
    
    private HTMLFrameList _frames;

    [PDFElement("")]
    [PDFArray(typeof(HTMLFrame))]
    public HTMLFrameList Frames
    {
        get
        {
            if (null == _frames)
                _frames = CreateFrameList();
            return _frames;
        }
        set
        {
            this._frames = value;
        }
    }
    
    public HTMLFrameset() : this(ObjectTypes.ModifyFrameSet)
    {}
    
    protected HTMLFrameset(ObjectType type)
        : base(type)
    {}
    
    
    protected virtual HTMLFrameList CreateFrameList()
    {
        return new HTMLFrameList(this.InnerContent);
    }
    
}