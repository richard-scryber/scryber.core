using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Scryber.Drawing;
using Scryber.PDF.Layout;
using Scryber.PDF.Native;
using Scryber.Svg.Components;

namespace Scryber.PDF.Resources;

/// <summary>
/// The PDFPatternLayoutResource holds a reference to the layout (items) of the pattern, that have been laid out.
/// These will then be rendered (once)
/// </summary>
public class PDFPatternLayoutResource : PDFResource
{

    public override string ResourceType
    {
        get { return PDFResource.XObjectResourceType; }
    }

    private string _layoutKey;
    private PDFLayoutPositionedRegion _layoutItem;

    public override string ResourceKey
    {
        get { return _layoutKey; }
    }

    public IComponent Pattern
    {
        get;
        set;
    }


    public bool Rendered { get; private set; }
    public PDFObjectRef RenderReference { get; private set; }
    
    public PDFPatternLayoutResource(IComponent pattern, PDFLayoutPositionedRegion region, string resourceKey) : base(ObjectTypes.PatternLayout)
    {
        this._layoutKey = resourceKey;
        this._layoutItem = region;
        this.Pattern = pattern;
    }

    public override bool Equals(string resourcetype, string key)
    {
        return String.Equals(this.ResourceType, resourcetype, StringComparison.InvariantCulture) && String.Equals(this.ResourceKey, key, StringComparison.InvariantCulture);
    }

    protected override PDFObjectRef DoRenderToPDF(ContextBase context, PDFWriter writer)
    {
        if (!this.Rendered)
        {
            var renderContext = context as PDF.PDFRenderContext ??
                                throw new ArgumentException("context must be of type PDFRenderContext",
                                    nameof(context));
            //we should be outputting directly to the 
            this.RenderReference = this._layoutItem.OutputToPDF(renderContext, writer);
            this.Rendered = true;
        }

        return this.RenderReference;
    }

    public static string GetLayoutResourceKey(SVGPattern forPattern)
    {
        var key = forPattern.UniqueID;
        key += "_layout";
        return key;
    }
}