using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Scryber.Drawing;
using Scryber.PDF.Graphics;
using Scryber.PDF.Layout;
using Scryber.PDF.Native;
using Scryber.Svg.Components;
using Point = System.Drawing.Point;

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

    
    protected PDFGraphicTilingPattern TilingPattern
    {
        get;
        set;
    }


    public bool Rendered { get; private set; }
    public PDFObjectRef RenderReference { get; private set; }
    
    public PDFPatternLayoutResource(IComponent pattern, PDFLayoutPositionedRegion region, PDFGraphicTilingPattern tilingPattern, string resourceKey) : base(ObjectTypes.PatternLayout)
    {
        this._layoutKey = resourceKey;
        this._layoutItem = region;
        this.Pattern = pattern;
        this.TilingPattern = tilingPattern;
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
            
            var prevSize = renderContext.Space;
            var prevOffset = renderContext.Offset;
            var prevGraphics = renderContext.Graphics;
            
            
            var newSize = this.TilingPattern.CalculateStepSize(context);
            
            if(newSize.IsRelative)
                throw new ArgumentException("Tiling pattern must have an absolute size");
                
            var newGraphics = PDFGraphics.Create(writer, false, this.TilingPattern, DrawingOrigin.TopLeft,
                newSize, context);

            renderContext.Offset = Drawing.Point.Empty;
            renderContext.Space = this.TilingPattern.Step;
            renderContext.Graphics = newGraphics;

            try
            {
                this.RenderReference = this._layoutItem.OutputToPDF(renderContext, writer);
            }
            catch (Exception e)
            {
                this.RenderReference = null;
                throw;
            }
            finally
            {
                renderContext.Offset = prevOffset;
                renderContext.Space = prevSize;
                renderContext.Graphics = prevGraphics;
                
                //Set this to true, whatever the result as we fail once.
                this.Rendered = false;
            }
        }

        return this.RenderReference;
    }

    public static string GetLayoutResourceKey(string forPatternResource)
    {
        var key = forPatternResource;
        key += "_layout";
        return key;
    }
}