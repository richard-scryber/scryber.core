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
public class SVGImageDataEmptySizer : SVGImageDataSizer
{

    /// <summary>
    /// Stores the available loyout space as a whole page. As we have no explicit sizing on the svg
    /// </summary>
    private Size _pageLayoutSize;
    
    public SVGImageDataEmptySizer(SVGCanvas forCanvas, Style appliedStyle, LayoutContext context)
        : base(forCanvas, appliedStyle, context)
    {
        //check the style values to make sure
        var empty = (!appliedStyle.IsValueDefined(StyleKeys.SizeWidthKey) &&
                     !appliedStyle.IsValueDefined(StyleKeys.SizeHeightKey) &&
                     !appliedStyle.IsValueDefined(StyleKeys.PositionViewPort));
        
        if (!empty) 
            throw new ArgumentOutOfRangeException("This SVG Image data sizing strategy can only be used with canvas's and styles that have no dimensions specified. Use the SVGImageDataSizer.CreateSizingStrategy to create the correct instance.", (Exception)null);
        
        // Null-safe: low-level unit tests construct sizers with context: null (no full layout
        // pipeline needed for what they're testing), so this can't assume a page is available.
        var pageSize = (context as PDFLayoutContext)?.DocumentLayout?.CurrentPage?.Size.Size;
        this._pageLayoutSize = (pageSize.HasValue && pageSize.Value.Width > Unit.Zero && pageSize.Value.Height > Unit.Zero)
            ? pageSize.Value
            : new Size(SVGCanvas.DefaultWidth, SVGCanvas.DefaultHeight);
    }

    /// <summary>
    /// Overrides the base implementation to return the Default Canvas Size. THis is always used for img tags that reference an image that does not have any internal dimensions.
    /// </summary>
    /// <returns></returns>
    protected override Size DoGetLayoutSize()
    {
        return this._pageLayoutSize;
        //return new Size(SVGCanvas.DefaultWidth, SVGCanvas.DefaultHeight);
    }


    // No need to override as just calls as base just calls DoGetLayoutSize
    // public override Size GetContentLayoutSize(LayoutContext context)
    // {
    //     return this.GetLayoutSize();
    // }

    /// <summary>
    /// Overrides the base implementation to return an identity matrix.
    /// Without sizing an svg is always rendered at 1:1 scale.
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
        var rect = new Rect(offset, available);
        return rect;
    }

    /// <summary>
    /// Overrides the base implementation, to return the layout page size (as we have no knowledge of how big it will be.
    /// </summary>
    protected override Rect DoGetImageToCanvasBBox(ContextBase context)
    {
        return new Rect(Point.Empty, _pageLayoutSize);
    }

    /// <summary>
    /// Caclulates the actual required image size based on the applied style, taken from a canvas default size.
    /// </summary>
    /// <param name="layout"></param>
    /// <param name="available"></param>
    /// <param name="applied"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    protected override Size DoGetOutputSizeForLayout(Size layout, Size available, Style applied, LayoutContext context)
    {
        Size calculated = new Size(SVGCanvas.DefaultWidth, SVGCanvas.DefaultHeight);
        bool hasWidth = false;
        bool hasHeight = false;
        
        var pos = applied.CreatePostionOptions(context.PositionDepth > 0);

        if (pos.Height.HasValue)
        {
            calculated.Height = pos.Height.Value;
            hasHeight = true;
        }

        if (pos.Width.HasValue)
        {
            calculated.Width = pos.Width.Value;
            hasWidth = true;
        }
        
        if (pos.MaximumHeight.HasValue && calculated.Height > pos.MaximumHeight.Value)
        {
            calculated.Height = pos.MaximumHeight.Value;
            hasHeight = true;
        }

        if (pos.MaximumWidth.HasValue && calculated.Width > pos.MaximumWidth.Value)
        {
            calculated.Width = pos.MaximumWidth.Value;
            hasWidth = true;
        }

        if (pos.MinimumHeight.HasValue && calculated.Height < pos.MinimumHeight.Value)
        {
            calculated.Height = pos.MinimumHeight.Value;
            hasHeight = true;
        }

        if (pos.MinimumWidth.HasValue && calculated.Width < pos.MinimumWidth.Value)
        {
            calculated.Width = pos.MinimumWidth.Value;
            hasWidth = true;
        }
        

        if (pos.AspectRatio.HasValue)
        {
            if (hasWidth && hasHeight)
                ; //Do nothing.
            else if (hasWidth)
            {
                calculated.Height = calculated.Width * (1/pos.AspectRatio.Value);
            }
            else if (hasHeight)
            {
                calculated.Width = calculated.Height * pos.AspectRatio.Value;
            }
        }

        return calculated;
    }

    /// <summary>
    /// No sizing always rendered at 1:1 scale
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="available"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    protected override Size DoGetRenderScaleForContent(Point offset, Size available, ContextBase context)
    {
        //var scale = base.DoGetRenderScaleForContent(offset, available, context);
        var scale = new Size(1.0, 1.0);
        return scale;
    }

    /// <summary>
    /// We have our page layout size, so we need to update offset based on the layout size and the output height.
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="available"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    protected override Point DoGetRenderOffsetForContent(Point offset, Size available, ContextBase context)
    {
        var layout = GetLayoutSize();
        //var diffh = layout.Height - available.Height;
        var pt = new Point(offset.X, offset.Y + layout.Height);
        return pt;
    }
}