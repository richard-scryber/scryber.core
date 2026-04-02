using System;
using Scryber.Drawing;
using Scryber.Expressive;
using Scryber.PDF.Graphics;
using Scryber.Styles;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging;

/// <summary>
/// Manages the sizing of a single SVG Disceet Image.
/// </summary>
public class SVGImageDataSizer
{
    //
    // properties
    //
    
    /// <summary>
    /// Gets the canvas for this sizer to work on
    /// </summary>
    public SVGCanvas Canvas { get; set; }
    
    /// <summary>
    /// Gets the style applied directly to the canvas (either via properties of the style attribute).
    /// </summary>
    private Style AppliedStyle { get; set; }

    /// <summary>
    /// Gets the current context (for logging)
    /// </summary>
    public LayoutContext Context { get; set; }
    
    /// <summary>
    /// Gets the size available to place any SVG Image.
    /// </summary>
    public Size AvailableSpace { get; set; }
    
    
    /// <summary>
    /// Gets the size the canvas should be rendered
    /// </summary>
    public Size RenderSize { get; private set; }
    
    //
    // .ctor
    //

    public SVGImageDataSizer(SVGCanvas canvas, Size available, Style appliedStyle, LayoutContext context)
    {
        this.Canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
        
        if(!this.Canvas.IsDiscreetSVG)
            throw new ArgumentException("SVG canvas must be a discreet SVG Image for this sizer");
        
        this.AppliedStyle = appliedStyle ?? throw new ArgumentNullException(nameof(appliedStyle));
        this.AvailableSpace = available;
        this.Context = context;
    }
    
    //
    // public methods
    //

    /// <summary>
    /// Returns the size that should be used to lay out the SVG in
    /// </summary>
    /// <returns></returns>
    public Size GetLayoutSize()
    {
        return this.DoGetLayoutSize();
    }

    /// <summary>
    /// Calculates and returns the size of the layout that should be used for this instances canvas.
    /// </summary>
    /// <returns>The appropriate size</returns>
    protected virtual Size DoGetLayoutSize()
    {
        //For layout - viewport (viewBox) overrides all
        if (this.AppliedStyle.TryGetValue(StyleKeys.PositionViewPort, out var vp))
        {
            var rect = vp.Value(this.AppliedStyle);
            return rect.Size;
        }

        // no viewport, so we start with the default size and apply any specific width or height.

        Unit width = SVGCanvas.DefaultWidth;
        Unit height = SVGCanvas.DefaultWidth;

        if (this.AppliedStyle.TryGetValue(StyleKeys.SizeWidthKey, out var widthValue))
        {
            width = widthValue.Value(this.AppliedStyle);
        }

        if (this.AppliedStyle.TryGetValue(StyleKeys.SizeHeightKey, out var heightValue))
        {
            height = heightValue.Value(this.AppliedStyle);
        }

        return new Size(width, height);

    }

    /// <summary>
    /// Updates the size of the canvas render size.
    /// </summary>
    /// <param name="size"></param>
    public void SetRenderSize(Size size)
    {
        this.RenderSize = size;
    }


    /// <summary>
    /// Calculate the unit scale for rendering laid out image into the required box. 1,1 will be natural size,
    /// altering this will scale the image as required in the horizontal and vertical (up) axes.
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="available"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Size GetRenderScaleForContent(Point offset, Size available, ContextBase context)
    {
        return new Size(1, 1);
    }

    public Point GetRenderOffsetForContent(Point offset, Size available, ContextBase context)
    {
        //Are we always render at 0,0 (bottom left) we offset to the position.
        var size = this.GetLayoutSize();
        offset.Y += size.Height;
        return offset;
    }



    public PDFTransformationMatrix GetCanvasToImageMatrix(ContextBase context)
    {
        return PDFTransformationMatrix.Identity();
    }
    

    public Rect GetImageToCanvasBBox(ContextBase context)
    {
        var size = this.GetLayoutSize();
        return new Rect(Point.Empty, size);
    }

}