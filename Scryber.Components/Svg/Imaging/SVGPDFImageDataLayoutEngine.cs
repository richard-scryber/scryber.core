using System;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Graphics;
using Scryber.PDF.Layout;
using Scryber.Styles;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging;

/// <summary>
/// Class that performs the actual layout of the content within a discreet SVGImage
/// </summary>
public class SVGPDFImageDataLayoutEngine
{

    public PDFLayoutBlock TryLayoutCanvas(SVGCanvas canvas, SVGImageDataSizer sizer, LayoutContext context, Style appliedstyle)
    {
        PDFLayoutBlock result = null;
        
        PDFLayoutContext pdfContext = context as PDFLayoutContext;

        if (pdfContext == null)
        {
            context.TraceLog.Add(TraceLevel.Error, "SVG Image", "The layout context was not a PDF layout context. Need the PDFContext to layout an SVG Canvas");
            return null;
        }

        var size = sizer.GetLayoutSize();

        if (!canvas.Style.IsValueDefined(StyleKeys.SizeWidthKey))
        {
            canvas.Width = size.Width;
            appliedstyle.SetValue(StyleKeys.SizeWidthKey, canvas.Width);
        }

        if (!canvas.Style.IsValueDefined(StyleKeys.SizeHeightKey))
        {
            canvas.Height = size.Height;
            appliedstyle.SetValue(StyleKeys.SizeHeightKey, size.Height);
        }

        var prevStyleStack = context.StyleStack;
        var prevItems = context.Items;
        
        PDFGraphics newGraphics = null;
        
        var closeContainerAfter = false;
        
        var pg = pdfContext.DocumentLayout.CurrentPage;

        try
        {
            
            var open = pg.LastOpenBlock();

            if (open == null)
            {
                //Document layout is complete and done - special case for SVG images as backgrounds
                //re-open and then we can close at the end.
                pg.ContentBlock.ReOpen();
                open = pg.ContentBlock;
                closeContainerAfter = true;
            }

            if (null == pdfContext.Graphics)
            {
                //We don't have a current graphics, so 
                //TODO: Check the size here: one.Size is very specific to the Graphics
                newGraphics = PDFGraphics.Create(null, false, pg, DrawingOrigin.TopLeft, open.Size, pdfContext);
                pdfContext.Graphics = newGraphics;
            }
            
            //Clear the stack and the items so we are a clean layout.
            var baseStyle = SVGCanvas.GetDefaultBaseStyle();
            context.StyleStack = new StyleStack(baseStyle);
            context.Items = new ItemCollection(canvas);

            
            //Apply specific styling for the XObject rendering and then pull a full style.
            this.PushXObjectStyle(appliedstyle, sizer);
            context.StyleStack.Push(appliedstyle);
            var full = GetFullStyle(canvas, pg.Size, open.AvailableBounds.Size, pdfContext, appliedstyle);


            //We can now safely perform the sizing and layout.
            result = this.DoLayoutCanvas(open, canvas, full, pdfContext);
            
            //not needed, but nice to clean up
            context.StyleStack.Pop();
        }
        catch (Exception e)
        {
            if(context.Conformance == ParserConformanceMode.Strict)
                throw new PDFLayoutException("Could not layout the SVG Image contents. See the inner exception for details.", e);
            else
            {
                context.TraceLog.Add(TraceLevel.Error, "SVG Image", "The layout of the SVG Image failed : " + e.Message, e);
                result = null;
            }
        }
        finally
        {
            //restore the stack and the items
            pdfContext.StyleStack = prevStyleStack;
            pdfContext.Items = prevItems;
            
            //check any created graphics;
            if (null != newGraphics)
            {
                newGraphics.Dispose();
                newGraphics = null;
            }
            
            //close any block we re-opened
            
            if(closeContainerAfter)
                pg.ContentBlock.Close();
            
        }
        
        return result;
    }

    /// <summary>
    /// Applies the explicit XOject styles (fixed layout, and position, etc) to the canvas style
    /// </summary>
    /// <param name="appliedstyle"></param>
    /// <param name="sizer"></param>
    protected virtual void PushXObjectStyle(Style appliedstyle, SVGImageDataSizer sizer)
    {
        appliedstyle.SetValue(StyleKeys.PositionModeKey, PositionMode.Fixed);
        appliedstyle.SetValue(StyleKeys.PositionXKey, Unit.Zero);
        appliedstyle.SetValue(StyleKeys.PositionYKey, Unit.Zero);
    }


    /// <summary>
    /// Creates the layout block for the SVGCanvas and lays out the individual components within it. Returning the actual block.
    /// All the related contents are removed so the SVG will not be rendered as part of the document flow.
    /// </summary>
    /// <param name="intoBlock">The block we can use to put the contents in.</param>
    /// <param name="canvas">The SVG Canvas to actually lay out.</param>
    /// <param name="fullStyle">The style associated with the canvas</param>
    /// <param name="context">The current layout context</param>
    /// <returns>A PDFLayoutBlock that is no longer part of any page.</returns>
    /// <exception cref="PDFLayoutException"></exception>
    protected virtual PDFLayoutBlock DoLayoutCanvas(PDFLayoutBlock intoBlock, SVGCanvas canvas,
        Style fullStyle, PDFLayoutContext context)
    {
        var pos = fullStyle.CreatePostionOptions(false);
        var pg = intoBlock.GetLayoutPage();
        
        bool isFloating = false;
        bool addRun = false;
        
        var posRegion = intoBlock.BeginNewPositionedRegion(pos, pg, canvas, fullStyle, isFloating, addRun);
        
        this.DoLayoutViewPort(canvas, null, fullStyle, context);

        //Extract the layout and remove any associations
        //We can render this independent of the layout itself
        
        var layout = posRegion.Contents[0] as PDFLayoutBlock;

        if (null == layout)
            throw new PDFLayoutException("Could not render the SVG Content as a layout block was not returned.");
        
        if(!layout.IsClosed)
            layout.Close();
        
        var parent = (PDFLayoutBlock)posRegion.Parent;
        parent.PositionedRegions.Remove(posRegion);
        
        return layout;
    }

    /// <summary>
    /// Performs the actual layout of the SVGCanvas
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="parent"></param>
    /// <param name="fullStyle"></param>
    /// <param name="context"></param>
    protected virtual void DoLayoutViewPort(SVGCanvas canvas, IPDFLayoutEngine parent, Style fullStyle, PDFLayoutContext context)
    {
        var vpComponent = (IPDFViewPortComponent)canvas;
        using (var engine = vpComponent.GetEngine(parent, context, fullStyle))
        {
            engine.Layout(context, fullStyle);
        }
    }

    protected virtual Style GetFullStyle(SVGCanvas canvas, Size pageSize, Size container, PDFLayoutContext layoutContext, Style appliedStyle)
    {
        //We have no reference back to the document style sheet. Use default values for all.
        var defSize = (Unit)Font.DefaultFontSize;
        var txtSize = new Size(defSize * 0.5, defSize);
        
        
        var fullStyle = layoutContext.StyleStack.GetFullStyle(canvas, pageSize,
            sizer: (IComponent forComponent, Style withStyle, PositionMode withPos) => new Size(container.Width, container.Height),
            txtSize, defSize);
        
        return fullStyle;
    }
    
}