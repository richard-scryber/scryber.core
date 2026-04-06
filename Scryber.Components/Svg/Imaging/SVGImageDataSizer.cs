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

        // Relative (percentage) values belong to the SVG element's own attributes and have already
        // been resolved to absolute units in GetIntrinsicSize(). Don't treat them as img overrides.
        if (hasWidth  && widthValue.Value(this.AppliedStyle).IsRelative)  hasWidth  = false;
        if (hasHeight && heightValue.Value(this.AppliedStyle).IsRelative) hasHeight = false;

        if (hasWidth && hasHeight)
        {
            // Both dimensions explicitly set by the outer img element — use them as-is.
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
    /// Explicit SVG width/height take priority over the viewBox dimensions; percentage values
    /// are resolved against the viewBox (a viewBox is required when percentages are used).
    /// When only one explicit dimension is given and a viewBox is present the other is derived
    /// proportionally. Falls back to viewBox size when no explicit dims are set, or to the
    /// SVGCanvas defaults (300×150) when neither viewBox nor explicit dims exist.
    /// </summary>
    private Size GetIntrinsicSize()
    {
        // Read viewBox first — needed for percent resolution and as the no-explicit-dims fallback.
        Rect? viewBox = null;
        if (this.Canvas.Style.TryGetValue(StyleKeys.PositionViewPort, out var vp))
        {
            var rect = vp.Value(this.Canvas.Style);
            if (rect.Width > Unit.Zero && rect.Height > Unit.Zero)
                viewBox = rect;
        }

        bool hasWidth  = this.Canvas.Style.TryGetValue(StyleKeys.SizeWidthKey,  out var wValue);
        bool hasHeight = this.Canvas.Style.TryGetValue(StyleKeys.SizeHeightKey, out var hValue);

        Unit width  = SVGCanvas.DefaultWidth;
        Unit height = SVGCanvas.DefaultHeight;

        if (hasWidth)
        {
            var w = wValue.Value(this.Canvas.Style);
            if (w.IsRelative)
            {
                if (w.Units != PageUnits.Percent)
                    throw new NotSupportedException(
                        $"SVG width unit '{w.Units}' is not supported as a relative dimension on a discreet SVG image.");
                if (viewBox == null)
                    throw new InvalidOperationException(
                        "SVG width is expressed as a percentage but the SVG has no viewBox to resolve it against.");
                width = w.ToAbsolute(viewBox.Value.Width);
            }
            else
                width = w;
        }

        if (hasHeight)
        {
            var h = hValue.Value(this.Canvas.Style);
            if (h.IsRelative)
            {
                if (h.Units != PageUnits.Percent)
                    throw new NotSupportedException(
                        $"SVG height unit '{h.Units}' is not supported as a relative dimension on a discreet SVG image.");
                if (viewBox == null)
                    throw new InvalidOperationException(
                        "SVG height is expressed as a percentage but the SVG has no viewBox to resolve it against.");
                height = h.ToAbsolute(viewBox.Value.Height);
            }
            else
                height = h;
        }

        // No explicit dims at all → viewBox size is the intrinsic size (existing behaviour).
        if (!hasWidth && !hasHeight)
            return viewBox.HasValue ? viewBox.Value.Size : new Size(width, height);

        // One dimension only → derive the other proportionally from the viewBox aspect ratio.
        if (hasWidth && !hasHeight && viewBox.HasValue)
            height = width * (viewBox.Value.Height.PointsValue / viewBox.Value.Width.PointsValue);
        else if (!hasWidth && hasHeight && viewBox.HasValue)
            width = height * (viewBox.Value.Width.PointsValue / viewBox.Value.Height.PointsValue);

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