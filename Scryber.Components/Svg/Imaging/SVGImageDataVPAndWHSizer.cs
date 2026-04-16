using System;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.PDF.Graphics;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging;

/// <summary>
/// Implements the sizing for an SVG Image Reference when the referenced image has both a ViewPort and explicit width and or height.
/// </summary>
public class SVGImageDataVPAndWHSizer : SVGImageDataSizer
{
    protected Unit SVGWidth { get; set; }
    protected Unit SVGHeight { get; set; }
    
    protected Rect ViewBox { get; set; }
    
    protected ViewPortAspectRatio AspectRatio {get; set;}
    

    public SVGImageDataVPAndWHSizer(SVGCanvas forCanvas, Style appliedStyle, LayoutContext context)
        : base(forCanvas, appliedStyle, context)
    {
        if (appliedStyle.TryGetValue(StyleKeys.PositionViewPort, out var vp))
        {
            this.ViewBox = vp.Value(appliedStyle);
        }
        else
        {
            throw new ArgumentNullException(
                "This SVG Image Data Strategy requires a viewBox to be set on the source image.");
        }
        
        if (appliedStyle.TryGetValue(StyleKeys.ViewPortAspectRatioStyleKey, out var found))
        {
            var value = found.Value(appliedStyle);
            AspectRatio = value;
        }
        else
        {
            AspectRatio = ViewPortAspectRatio.Default;
        }
        
        bool hasWidth = false;
        bool hasHeight = false;

        if (appliedStyle.TryGetValue(StyleKeys.SizeWidthKey, out var w))
        {
            this.SVGWidth = w.Value(appliedStyle);
            hasWidth = true;
        }

        if (appliedStyle.TryGetValue(StyleKeys.SizeHeightKey, out var h))
        {
            SVGHeight = h.Value(appliedStyle);
            hasHeight = true;
        }
        
        if(!hasWidth && !hasHeight)
            throw new ArgumentNullException("Width and Height must be set on the source svg for this strategy.");
        else if (!hasHeight || !hasWidth)
        {
            if (!hasWidth)
            {
                //We don't have both, so calculate the missing based on set.
                var scale = this.SVGHeight.PointsValue / this.ViewBox.Height.PointsValue;
                this.SVGWidth = this.ViewBox.Width * scale;
            }
            else
            {
                var scale = this.SVGWidth.PointsValue / this.ViewBox.Width.PointsValue;
                this.SVGWidth = this.ViewBox.Height * scale;
            }
        }
    }

    protected override Size DoGetOutputSizeForLayout(Size layout, Size available, Style applied, LayoutContext context)
    {

        return new Size(this.SVGWidth, this.SVGHeight);
    }

    protected override Size DoGetRenderScaleForContent(Point offset, Size available, ContextBase context)
    {
        return new Size(0.5, 0.5);
        return base.DoGetRenderScaleForContent(offset, available, context);
    }

    protected override PDFTransformationMatrix DoGetCanvasToImageMatrix(Size layoutSize, Point layoutOffset, ContextBase context)
    {
        //return PDFTransformationMatrix.Identity();
        return base.DoGetCanvasToImageMatrix(layoutSize, layoutOffset, context);
    }
}