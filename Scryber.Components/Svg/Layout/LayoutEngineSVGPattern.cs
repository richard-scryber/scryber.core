using System.Collections.Generic;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.PDF.Resources;
using Scryber.Styles;
using Scryber.Svg.Components;

namespace Scryber.Svg.Layout;

/// <summary>
/// Implements the layout of patterns - that are contained in a separate svg canvas,
/// and laid out indpendently of the main canvas so they can be rendered once and referred to by their name
/// </summary>
public class LayoutEngineSVGPattern : IPDFLayoutEngine
{

    public IPDFLayoutEngine ParentEngine { get; }
    
    public bool ContinueLayout { get; set; }
    
    public PDFLayoutContext Context { get; }
    
    public SVGPattern Pattern { get; }
    
    protected SVGCanvas InnerCanvas { get; }


    public LayoutEngineSVGPattern(IPDFLayoutEngine parentEngine, SVGPattern pattern, SVGCanvas innerCanvas, PDFLayoutContext context)
    {
        this.ParentEngine = parentEngine;
        this.Context = context;
        this.ContinueLayout = true;
        this.Pattern = pattern;
        this.InnerCanvas = innerCanvas;
    }
    
    public void Layout(PDFLayoutContext context, Style fullstyle)
    {
        var pos = fullstyle.CreatePostionOptions(false);

        if (this.DoLayoutCanvas(context, fullstyle, pos, out PDFLayoutPositionedRegion region))
        {
            var layoutKey = PDFPatternLayoutResource.GetLayoutResourceKey(this.Pattern);
            var patternRsrc = new PDFPatternLayoutResource(this.Pattern, region, this.Pattern.GetTilingPattern(), layoutKey);
            patternRsrc.Name = (Scryber.PDF.Native.PDFName)this.Pattern.Document.GetIncrementID(ObjectTypes.Pattern);
            this.Pattern.Document.SharedResources.Add(patternRsrc);
            
            //This is used the same way as in the engine but needs more formal mechanism.
            this.Pattern.XObjectRendererKey = layoutKey;
        }


    }

    protected virtual bool DoLayoutCanvas(PDFLayoutContext context, Style fullstyle, PDFPositionOptions pos, out PDFLayoutPositionedRegion contentRegion)
    {
        var viewport = (IPDFViewPortComponent)this.InnerCanvas;
        contentRegion = null;
        
        using (var engine = viewport.GetEngine(this, context, fullstyle))
        {
            
            engine.Layout(context, fullstyle);
            var block = context.DocumentLayout.CurrentPage.LastOpenBlock();
            
            if (block.HasPositionedRegions)
            {
                foreach (var region in block.PositionedRegions)
                {
                    if (region.Owner == this.Pattern)
                    {
                        contentRegion = region as PDFLayoutPositionedRegion;
                        if (contentRegion != null && contentRegion.AssociatedRun != null)
                        {
                            contentRegion.AssociatedRun.Line.RemoveRun(contentRegion.AssociatedRun);
                        }
                        break;
                    }
                }
            }
        }
        
        return null != contentRegion;
    }

    public bool MoveToNextPage(IComponent initiator, Style initiatorStyle, Stack<PDFLayoutBlock> depth, ref PDFLayoutRegion region,
        ref PDFLayoutBlock block)
    {
        return false;
    }

    public PDFLayoutBlock CloseCurrentBlockAndStartNewInRegion(PDFLayoutBlock blockToClose, PDFLayoutRegion joinToRegion)
    {
        return blockToClose;
    }
    
    
    
    protected virtual void Dispose(bool disposing)
    {
    }
    
    public void Dispose()
    {
        this.Dispose(true);
    }

    ~LayoutEngineSVGPattern()
    {
        this.Dispose(false);
    }

    
}