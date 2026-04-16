using System;
using Scryber.Styles;
using Scryber.Drawing;
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

    public SVGImageDataEmptySizer(SVGCanvas forCanvas, Style appliedStyle, LayoutContext context)
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
        //var rect = base.DoGetClippingRect(offset, available, context);
        var rect = new Rect(offset, available);
        return rect;
    }

    protected override Size DoGetOutputSizeForLayout(Size layout, Size available, Style applied, LayoutContext context)
    {
        var pos = applied.CreatePostionOptions(context.PositionDepth > 0);
        if (pos.Height.HasValue || pos.Width.HasValue)
        {
            var width = pos.Width ?? SVGCanvas.DefaultWidth;
            var height = pos.Height ?? SVGCanvas.DefaultHeight;
            
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