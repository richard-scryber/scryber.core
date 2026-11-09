using System;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Graphics;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging;

/// <summary>
/// Implements the sizing for an SVG Image Reference when the referenced
/// image has no explicit sizes or viewport.
/// </summary>
/// <remarks>When a referenced SVG Image has no sizing,
/// then it is always rendered at the default SVG Size (300x150)
/// and any img tag size is used to clip (or extend beyond) the content.</remarks>
public class SVGImageDataEmptySizer_old : SVGImageDataSizer
{

    public SVGImageDataEmptySizer_old(SVGCanvas forCanvas, Style appliedStyle, LayoutContext context)
        : base(forCanvas, appliedStyle, context)
    {
        //check the style values to make sure
        var empty = (!appliedStyle.IsValueDefined(StyleKeys.SizeWidthKey) &&
                     !appliedStyle.IsValueDefined(StyleKeys.SizeHeightKey) &&
                     !appliedStyle.IsValueDefined(StyleKeys.PositionViewPort));
        
        if (!empty) 
            throw new ArgumentOutOfRangeException("This SVG Image data sizing strategy can only be used with canvas's and styles that have no dimensions specified. Use the SVGImageDataSizer.CreateSizingStrategy to create the correct instance.", (Exception)null);
        
    }

    /// <summary>
    /// Overrides the base implementation to return the Default Canvas Size. THis is always used for img tags that reference an image that does not have any internal dimensions.
    /// </summary>
    /// <returns></returns>
    protected override Size DoGetLayoutSize()
    {
        return new Size(SVGCanvas.DefaultWidth, SVGCanvas.DefaultHeight);
    }

    /// <summary>
    /// Overrides the base implementation (which would otherwise just return GetLayoutSize(), the
    /// 300x150 default) so the SVG's own content is laid out into the current page's size instead -
    /// generously large, so this is never the bottleneck - rather than being permanently clipped to
    /// 300x150 regardless of how big any individual &lt;img&gt; referencing it ends up being.
    /// Confirmed against real browser rendering: content beyond 300x150 draws in full once the box
    /// is bigger, rather than a fixed-size render just being repositioned within a bigger box.
    /// GetLayoutSize() itself deliberately stays at the 300x150 default - it is also used as the
    /// *default output box* size (DoGetOutputSizeForLayout) and for the render offset calculation,
    /// both of which must keep matching real browser behaviour for an unstyled &lt;img&gt;.
    /// </summary>
    public override Size GetContentLayoutSize(LayoutContext context)
    {
        var pdfContext = context as PDFLayoutContext;
        var pageSize = pdfContext?.DocumentLayout?.CurrentPage?.Size.Size;
        if (pageSize.HasValue && pageSize.Value.Width > Unit.Zero && pageSize.Value.Height > Unit.Zero)
            return pageSize.Value;

        return this.GetLayoutSize();
    }

    /// <summary>
    /// Overrides the base implementation to return an identity matrix
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected override PDFTransformationMatrix DoGetCanvasToImageMatrix(Size layoutSize, Point layoutLocation, ContextBase context)
    {
        //var matrix = base.DoGetCanvasToImageMatrix(context);
        var matrix =  PDFTransformationMatrix.Identity();
        return matrix;
    }

    protected override Rect? DoGetClippingRect(Point offset, Size available, ContextBase context)
    {
        var rect = base.DoGetClippingRect(offset, available, context);
        rect = new Rect(offset, available);
        return rect;
    }

    /// <summary>
    /// Overrides the base implementation, which would otherwise use GetLayoutSize() (the 300x150
    /// default) as the BBox for the shared PDF Form XObject wrapping this SVG's content. That
    /// default is only meaningful as the *outer box* size when nothing else specifies one - it is
    /// not a real, author-declared clip boundary the way a viewBox or the SVG's own declared
    /// width/height is. Using it as the XObject's BBox permanently clips anything drawn beyond it
    /// at the PDF spec level, regardless of how big any individual &lt;img&gt; referencing this SVG
    /// ends up being - confirmed against real browser rendering, where a bigger box lets content
    /// beyond 300x150 draw in full rather than just repositioning a fixed-size render.
    /// Use the current page size instead - generously large, so the BBox is never the bottleneck -
    /// and let the render-time clipping rect (already computed per &lt;img&gt; instance in
    /// DoGetClippingRect above) constrain what each individual instance actually shows.
    /// </summary>
    protected override Rect DoGetImageToCanvasBBox(ContextBase context)
    {
        var pageSize = (context as PDFRenderContext)?.PageSize;
        var size = (pageSize.HasValue && pageSize.Value.Width > Unit.Zero && pageSize.Value.Height > Unit.Zero)
            ? pageSize.Value
            : new Size(SVGCanvas.DefaultWidth, SVGCanvas.DefaultHeight);

        return new Rect(Point.Empty, size);
    }

    protected override Size DoGetOutputSizeForLayout(Size layout, Size available, Style applied, LayoutContext context)
    {
        var pos = applied.CreatePostionOptions(context.PositionDepth > 0);
        if (pos.Height.HasValue || pos.Width.HasValue)
        {
            Unit width;
            Unit height;

            if (pos.Width.HasValue && pos.Height.HasValue)
            {
                width = pos.Width.Value;
                height = pos.Height.Value;
            }
            else if (pos.Width.HasValue)
            {
                width = pos.Width.Value;
                height = pos.AspectRatio.HasValue && pos.AspectRatio.Value > 0
                    ? width / pos.AspectRatio.Value
                    : SVGCanvas.DefaultHeight;
            }
            else
            {
                height = pos.Height.Value;
                width = pos.AspectRatio.HasValue && pos.AspectRatio.Value > 0
                    ? height * pos.AspectRatio.Value
                    : SVGCanvas.DefaultWidth;
            }

            return new Size(width, height);

        }
        return base.DoGetOutputSizeForLayout(layout, available, applied, context);
    }

    protected override Size DoGetRenderScaleForContent(Point offset, Size available, ContextBase context)
    {
        //var scale = base.DoGetRenderScaleForContent(offset, available, context);
        var scale = new Size(1.0, 1.0);
        return scale;
    }

    protected override Point DoGetRenderOffsetForContent(Point offset, Size available, ContextBase context)
    {
        var layout = GetLayoutSize();
        var diffh = layout.Height - available.Height;
        var pt = new Point(offset.X, offset.Y + available.Height + diffh);
        return pt;
    }
}