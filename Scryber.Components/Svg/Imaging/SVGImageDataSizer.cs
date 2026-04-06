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
    /// Intrinsic size is resolved from Canvas.Style (viewBox → explicit SVG dims → defaults).
    /// If the outer img element has overridden one or both dimensions via AppliedStyle those are
    /// respected, with proportional scaling applied when only one dimension is specified.
    /// </summary>
    /// <returns>The appropriate size</returns>
    protected virtual Size DoGetLayoutSize()
    {
        var intrinsic = this.GetIntrinsicSize();

        bool hasWidth  = this.AppliedStyle.TryGetValue(StyleKeys.SizeWidthKey,  out var widthValue);
        bool hasHeight = this.AppliedStyle.TryGetValue(StyleKeys.SizeHeightKey, out var heightValue);

        if (hasWidth && hasHeight)
        {
            // Both dimensions explicitly set — use them as-is.
            return new Size(widthValue.Value(this.AppliedStyle), heightValue.Value(this.AppliedStyle));
        }
        else if (hasWidth)
        {
            // Width only — scale height proportionally from the intrinsic aspect ratio.
            var w = widthValue.Value(this.AppliedStyle);
            var h = w * (intrinsic.Height.PointsValue / intrinsic.Width.PointsValue);
            return new Size(w, h);
        }
        else if (hasHeight)
        {
            // Height only — scale width proportionally from the intrinsic aspect ratio.
            var h = heightValue.Value(this.AppliedStyle);
            var w = h * (intrinsic.Width.PointsValue / intrinsic.Height.PointsValue);
            return new Size(w, h);
        }
        else
        {
            return intrinsic;
        }
    }

    /// <summary>
    /// Returns the intrinsic (natural) size of the SVG by inspecting Canvas.Style.
    /// Priority: viewBox dimensions → explicit SVG width/height → SVGCanvas defaults (300×150).
    /// </summary>
    private Size GetIntrinsicSize()
    {
        // viewBox drives intrinsic size when present.
        if (this.Canvas.Style.TryGetValue(StyleKeys.PositionViewPort, out var vp))
        {
            var rect = vp.Value(this.Canvas.Style);
            if (rect.Width > Unit.Zero && rect.Height > Unit.Zero)
                return rect.Size;
        }

        Unit width  = SVGCanvas.DefaultWidth;
        Unit height = SVGCanvas.DefaultHeight;

        if (this.Canvas.Style.TryGetValue(StyleKeys.SizeWidthKey, out var w))
            width = w.Value(this.Canvas.Style);

        if (this.Canvas.Style.TryGetValue(StyleKeys.SizeHeightKey, out var h))
            height = h.Value(this.Canvas.Style);

        return new Size(width, height);
    }

    /// <summary>
    /// Gets the actual rendered size of the image in the page (set by SetRenderSize after layout).
    /// Used as the dest for the canvas-to-image matrix so the transform matches the placed dimensions.
    /// </summary>
    public Size? RenderSize { get; private set; }

    /// <summary>
    /// Records the rendered size of the image once the layout engine has determined the final placement.
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
        // No viewBox → content maps 1:1, no transform needed.
        if (!this.Canvas.Style.TryGetValue(StyleKeys.PositionViewPort, out var vpValue))
            return PDFTransformationMatrix.Identity();

        var viewBox = vpValue.Value(this.Canvas.Style);
        if (viewBox.Width <= Unit.Zero || viewBox.Height <= Unit.Zero)
            return PDFTransformationMatrix.Identity();

        // Use the actual rendered size as the dest if the layout engine has set it;
        // otherwise fall back to the intrinsic layout size.
        var dest = this.RenderSize ?? this.GetLayoutSize();

        // Resolve preserveAspectRatio from the canvas (default: xMidYMid meet).
        ViewPortAspectRatio par;
        if (this.Canvas.Style.TryGetValue(StyleKeys.ViewPortAspectRatioStyleKey, out var parValue))
            par = parValue.Value(this.Canvas.Style);
        else
            par = ViewPortAspectRatio.Default;

        var matrix = new PDFTransformationMatrix();

        if (par.Align == AspectRatioAlign.None)
            ViewPortAspectRatio.ApplyMaxNonUniformScaling(matrix, dest, viewBox);
        else if (par.Meet == AspectRatioMeet.Slice)
            ViewPortAspectRatio.ApplyUniformStretching(matrix, dest, viewBox, par.Align);
        else
            ViewPortAspectRatio.ApplyUniformScaling(matrix, dest, viewBox, par.Align);

        // Shift for non-zero viewBox origin: content starting at (minX, minY) must map to (0,0).
        if (viewBox.X != Unit.Zero || viewBox.Y != Unit.Zero)
        {
            double scaleX = matrix.Components[0];
            double scaleY = matrix.Components[3];
            matrix.SetTranslation(
                -viewBox.X.PointsValue * scaleX,
                -viewBox.Y.PointsValue * scaleY);
        }

        return matrix;
    }
    

    public Rect GetImageToCanvasBBox(ContextBase context)
    {
        var size = this.GetLayoutSize();
        return new Rect(Point.Empty, size);
    }

}