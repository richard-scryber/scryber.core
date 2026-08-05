using System;
using Scryber.Drawing;
using Scryber.Expressive;
using Scryber.PDF.Graphics;
using Scryber.Styles;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging;

/// <summary>
/// Manages the sizing of a single SVG Discreet Image. The SVG itself should handle all the internal sizing.
/// This class is concerned with sizing appropriate to it's img tag, and any width of height associated with that.
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
    /// Gets or sets the flag that identifies if the canvas does not have any explicit dimensions or viewbox
    /// This should always use a 1:1 ratio scaling with the default sizes (300x150)
    /// </summary>
    public bool IsNotDimensioned { get; set; }
    
    
    /// <summary>
    /// cached nullable layout size value.
    /// If not set then DoGetLayoutSize will be called to (re-)calculate.
    /// </summary>
    protected Size? LayoutSize { get; set; }
    
    /// <summary>
    /// Gets or sets the actual rendered size of the image in the page (set by SetRenderSize after layout).
    /// Used as the dest for the canvas-to-image matrix so the transform matches the placed dimensions.
    /// </summary>
    protected Size? RenderSize { get; set; }

    public virtual bool AllowLayoutOverflow
    {
        get{ return false; }
    }
    
    //
    // .ctor
    //

    public SVGImageDataSizer(SVGCanvas canvas, Style appliedStyle, LayoutContext context)
    {
        this.Canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
        this.IsNotDimensioned = (!appliedStyle.IsValueDefined(StyleKeys.SizeWidthKey) &&
                                 !appliedStyle.IsValueDefined(StyleKeys.SizeHeightKey) &&
                                 !appliedStyle.IsValueDefined(StyleKeys.PositionViewPort));
        if(!this.Canvas.IsDiscreetSVG)
            throw new ArgumentException("SVG canvas must be a discreet SVG Image for this sizer");
        
        this.AppliedStyle = appliedStyle ?? throw new ArgumentNullException(nameof(appliedStyle));
        this.Context = context;

        
    }
    
    //
    // Static factory for concrete instances
    //

    public static SVGImageDataSizer CreateSizingStrategy(SVGCanvas forCanvas, Style appliedStyle, LayoutContext context)
    {
        //return new SVGImageDataSizer(forCanvas, appliedStyle, context);
        var w = appliedStyle.GetValue(StyleKeys.SizeWidthKey, Unit.AutoValue);
        var h = appliedStyle.GetValue(StyleKeys.SizeHeightKey, Unit.AutoValue);
        var vp = appliedStyle.IsValueDefined(StyleKeys.PositionViewPort);

        if (w != Unit.AutoValue || h != Unit.AutoValue)
        {
            //we have an SVG size
            if(vp)
                return new SVGImageDataVPAndWHSizer(forCanvas, appliedStyle, context);
            else
            {
                return new SVGImageDataOnlyWHSizer(forCanvas, appliedStyle, context);
            }
        }
        else if (vp)
        {
            //we have just a viewport
            return new SVGImageDataViewBoxSizer(forCanvas, appliedStyle, context);
        }
        else
        {
            return new SVGImageDataEmptySizer(forCanvas, appliedStyle, context);
        }
    }
    
    //
    // public methods
    //

    /// <summary>
    /// Returns the size that should be used internally to lay out the SVG in the Canvas (not the output size)
    /// </summary>
    /// <returns></returns>
    public Size GetLayoutSize()
    {
        if (!this.LayoutSize.HasValue)
            this.LayoutSize = this.DoGetLayoutSize();
        
        return this.LayoutSize.Value;
    }


    public Size GetOutputSizeForLayout(Size layout, Size available, Style applied, LayoutContext context)
    {
        var size = this.DoGetOutputSizeForLayout(layout, available, applied, context);
        return ApplyMinMaxConstraints(size, applied, context);
    }

    /// <summary>
    /// Applies min-width/max-width/min-height/max-height to whatever size DoGetOutputSizeForLayout
    /// (or a subclass override) computed. None of the sizing strategies otherwise consider these -
    /// unlike raster images, which apply them via ImageBase.GetRequiredSizeForLayout - so they were
    /// silently ignored for SVG images. This deliberately only adds the min/max clamp on top of each
    /// strategy's own result, rather than replacing how an unsized SVG is sized in the first place:
    /// several strategies have their own correct, spec-aligned behaviour for that case (e.g. a
    /// viewBox-only SVG fills its container per SVGImageDataViewBoxSizer, and an SVG with no sizing
    /// information at all always renders at the SVG default per SVGImageDataEmptySizer's own doc
    /// comment) that a generic "shrink/grow to fit" replacement would have broken.
    /// </summary>
    private static Size ApplyMinMaxConstraints(Size size, Style applied, LayoutContext context)
    {
        var pos = applied.CreatePostionOptions(context.PositionDepth > 0);

        // Both explicit - matches ImageBase, which doesn't clamp this combination either.
        if (pos.Width.HasValue && pos.Height.HasValue)
            return size;

        if (!pos.MinimumWidth.HasValue && !pos.MaximumWidth.HasValue &&
            !pos.MinimumHeight.HasValue && !pos.MaximumHeight.HasValue)
            return size;

        Unit w = size.Width;
        Unit h = size.Height;

        if (pos.Width.HasValue)
        {
            // Width is explicit/derived already - only the derived height is clamped, matching
            // ImageBase's width-only branch (which only ever adjusts the derived height).
            if (pos.MaximumHeight.HasValue && h > pos.MaximumHeight.Value) h = pos.MaximumHeight.Value;
            if (pos.MinimumHeight.HasValue && h < pos.MinimumHeight.Value) h = pos.MinimumHeight.Value;
            return new Size(w, h);
        }

        if (pos.Height.HasValue)
        {
            if (pos.MaximumWidth.HasValue && w > pos.MaximumWidth.Value) w = pos.MaximumWidth.Value;
            if (pos.MinimumWidth.HasValue && w < pos.MinimumWidth.Value) w = pos.MinimumWidth.Value;
            return new Size(w, h);
        }

        // Neither explicit - clamp width and height independently, with NO proportional coupling
        // between them. Confirmed against real browser rendering: e.g. min-width alone on an
        // unsized SVG does not scale the content up and derive a taller height - it only widens
        // the box (300x150 natural -> 400x150 with min-width:400), and the SVG content still
        // renders at its natural scale within that box (padded or clipped as needed). This mirrors
        // how an unsized SVG has no inherent width/height coupling to begin with - unlike the
        // width-only/height-only branches above, where DoGetOutputSizeForLayout already derived
        // the other dimension from an explicit aspect-ratio/viewBox, so clamping is appropriate.
        if (pos.MaximumWidth.HasValue && w > pos.MaximumWidth.Value)
            w = pos.MaximumWidth.Value;
        else if (pos.MinimumWidth.HasValue && w < pos.MinimumWidth.Value)
            w = pos.MinimumWidth.Value;

        if (pos.MaximumHeight.HasValue && h > pos.MaximumHeight.Value)
            h = pos.MaximumHeight.Value;
        else if (pos.MinimumHeight.HasValue && h < pos.MinimumHeight.Value)
            h = pos.MinimumHeight.Value;

        return new Size(w, h);
    }
    

    public Rect? GetClippingRect(Point offset, Size available, ContextBase context)
    {
        return this.DoGetClippingRect(offset, available, context);
    }

    /// <summary>
    /// Calculates and returns the unit scale for rendering laid out image into the required box. 1,1 will be natural size,
    /// altering this will scale the image as required in the horizontal and vertical (up) axes.
    /// </summary>
    /// <param name="offset">The current offset on the page</param>
    /// <param name="available">The space available to layout teh SVG Image in</param>
    /// <param name="context">The current context</param>
    /// <returns>The scale that should be used to render the image</returns>
    public Size GetRenderScaleForContent(Point offset, Size available, ContextBase context)
    {
        return this.DoGetRenderScaleForContent(offset, available, context);
    }

    /// <summary>
    /// Calculates and returns the required offset (from top left) on the page for the SVGCanvas this sizer references.
    /// </summary>
    /// <param name="offset">The current offset on the page</param>
    /// <param name="available">The available space this image will be rendered in.</param>
    /// <param name="context">The current context</param>
    /// <returns>The actual offset position </returns>
    public Point GetRenderOffsetForContent(Point offset, Size available, ContextBase context)
    {
        return this.DoGetRenderOffsetForContent(offset, available, context);
    }

    /// <summary>
    /// Calculates and returns the transformation matrix that should be used when invoking the rendering of the Do img render.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public PDFTransformationMatrix GetCanvasToImageMatrix(Size layoutSize, Point layoutLocation, ContextBase context)
    {
        return this.DoGetCanvasToImageMatrix(layoutSize, layoutLocation, context);
    }
    
    public Rect GetImageToCanvasBBox(ContextBase context)
    {
        return this.DoGetImageToCanvasBBox(context);
    }
    
    protected virtual Size DoGetRenderScaleForContent(Point offset, Size available, ContextBase context)
    {
        var layout = this.GetLayoutSize();
        
        double scaleX = available.Width.PointsValue / layout.Width.PointsValue;
        double scaleY = available.Height.PointsValue / layout.Height.PointsValue;
        
        if (IsNotDimensioned)
        {
            double scaleMax = Math.Max(scaleX, scaleY);
            return new Size(scaleMax, scaleMax);
        }
        
        return new Size(scaleX, scaleY);
    }
    
    protected virtual Size DoGetOutputSizeForLayout(Size layout, Size available, Style applied, LayoutContext context)
    {
        var pos = applied.CreatePostionOptions(context.PositionDepth > 0);

        Size output;

        if (pos.Width.HasValue && pos.Height.HasValue)
            output = new Size(pos.Width.Value, pos.Height.Value);
        else if (pos.Width.HasValue)
        {
            var w = pos.Width.Value;
            var h = pos.AspectRatio.HasValue && pos.AspectRatio.Value > 0
                ? w / pos.AspectRatio.Value
                : layout.Height * (w.PointsValue / layout.Width.PointsValue);
            output = new Size(w, h);
        }
        else if (pos.Height.HasValue)
        {
            var h = pos.Height.Value;
            var w = pos.AspectRatio.HasValue && pos.AspectRatio.Value > 0
                ? h * pos.AspectRatio.Value
                : layout.Width * (h.PointsValue / layout.Height.PointsValue);
            output = new Size(w, h);
        }
        else
            output = layout;
        
        return output;
    }
    
    protected virtual Point DoGetRenderOffsetForContent(Point offset, Size available, ContextBase context)
    {
        //Are we always render at 0,0 (bottom left) we offset to the position.
        var size = this.GetLayoutSize();
        var scale = this.GetRenderScaleForContent(offset, available, context);
        var y = size.Height * scale.Height.PointsValue;
        offset.Y += y;
        return offset;
    }


    protected virtual Rect? DoGetClippingRect(Point offset, Size available, ContextBase context)
    {
        return new Rect(offset, available);
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

        bool hasAspectRatio = this.AppliedStyle.TryGetValue(StyleKeys.SizeAspectRatioKey, out var aspectValue);
        double aspectRatio = hasAspectRatio ? aspectValue.Value(this.AppliedStyle) : double.NaN;

        if (hasWidth && hasHeight)
        {
            // Both dimensions explicitly set by the outer img element — use them as-is.
            return new Size(widthValue.Value(this.AppliedStyle), heightValue.Value(this.AppliedStyle));
        }
        else if (hasWidth)
        {
            // Width only — scale height from an explicit aspect-ratio if set, otherwise the intrinsic aspect ratio.
            var w = widthValue.Value(this.AppliedStyle);
            var h = hasAspectRatio && aspectRatio > 0
                ? w / aspectRatio
                : w * (intrinsic.Height.PointsValue / intrinsic.Width.PointsValue);
            return new Size(w, h);
        }
        else if (hasHeight)
        {
            // Height only — scale width from an explicit aspect-ratio if set, otherwise the intrinsic aspect ratio.
            var h = heightValue.Value(this.AppliedStyle);
            var w = hasAspectRatio && aspectRatio > 0
                ? h * aspectRatio
                : h * (intrinsic.Width.PointsValue / intrinsic.Height.PointsValue);
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

    


    



    protected virtual PDFTransformationMatrix DoGetCanvasToImageMatrix(Size layoutSize, Point layoutLocation, ContextBase context)
    {
        
        // No viewBox → content maps 1:1, no transform needed.
        if (!this.Canvas.Style.TryGetValue(StyleKeys.PositionViewPort, out var vpValue))
            return PDFTransformationMatrix.Identity();

        var viewBox = vpValue.Value(this.Canvas.Style);
        if (viewBox.Width <= Unit.Zero || viewBox.Height <= Unit.Zero)
            return PDFTransformationMatrix.Identity();

        // Use the actual rendered size as the dest if the layout engine has set it;
        // otherwise fall back to the intrinsic layout size.
        var dest = this.GetLayoutSize();

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
    

    protected Rect DoGetImageToCanvasBBox(ContextBase context)
    {
        var size = this.GetLayoutSize();
        return new Rect(Point.Empty, size);
    }

}