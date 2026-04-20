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
    private bool _allowLayoutOverflow;
    protected Unit SVGWidth { get; set; }
    protected Unit SVGHeight { get; set; }
    
    protected Rect ViewBox { get; set; }
    
    protected ViewPortAspectRatio AspectRatio {get; set;}

    public override bool AllowLayoutOverflow
    {
        get { return true; }
    }


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
            this.SVGWidth = EnsureAbsolute(w.Value(appliedStyle), this.ViewBox.Width);
            hasWidth = true;
        }

        if (appliedStyle.TryGetValue(StyleKeys.SizeHeightKey, out var h))
        {
            SVGHeight = EnsureAbsolute(h.Value(appliedStyle),  this.ViewBox.Height);
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
                this.SVGHeight = this.ViewBox.Height * scale;
            }
        }
    }

    private Unit EnsureAbsolute(Unit value, Unit reference)
    {
        if (value.IsRelative)
        {
            if (value.Units == PageUnits.Percent)
            {
                value = reference * (value.Value / 100.0);
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    "The SVG dimension units can only be absolute values, or a percent of the viewbox");
            }
        }
        return value;
    }

    
    protected override Size DoGetOutputSizeForLayout(Size layout, Size available, Style applied, LayoutContext context)
    {
        var width = this.SVGWidth;
        bool hasWidth = false;
        //TODO: Check applied for any size changes and scale
        if (applied.TryGetValue(StyleKeys.SizeWidthKey, out var w))
        {
            width = w.Value(applied);
            hasWidth = true;
        }
        
        var height = this.SVGHeight;
        bool hasHeight = false;
        if (applied.TryGetValue(StyleKeys.SizeHeightKey, out var h))
        {
            height = h.Value(applied);
            hasHeight = true;
        }

        if (hasWidth || hasHeight)
        {
            if (hasWidth && hasHeight)
            {
                return new Size(width, height);
            }
            else if (hasWidth)
            {
                var scale = width.PointsValue / this.SVGWidth.PointsValue;
                height = this.SVGHeight * scale;
            }
            else
            {
                var scale = height.PointsValue / this.SVGHeight.PointsValue;
                width = this.SVGWidth * scale;
            }
        }
        
        return new Size(width, height);
    }

    protected override Size DoGetRenderScaleForContent(Point offset, Size available, ContextBase context)
    {
        var scaleH = 1.0;
        var scaleV = 1.0;
        
        //TODO: Adjust here for available not matching the SVG width and height.
        if (available.Width != this.SVGWidth || available.Height != this.SVGHeight)
        {
            scaleH = available.Width.PointsValue / this.SVGWidth.PointsValue;
            scaleV = available.Height.PointsValue / this.SVGHeight.PointsValue;

            if (this.AspectRatio.Meet == AspectRatioMeet.Meet)
            {
                scaleH = Math.Min(scaleH, scaleV);
                scaleV = scaleH;
            }
            else if (this.AspectRatio.Meet == AspectRatioMeet.Slice)
            {
                scaleH = Math.Max(scaleH, scaleV);
                scaleV = scaleH;
            }
            //Alternative = None with non-proportional scaling.
        }
        
        return new Size(scaleH, scaleV);
        return base.DoGetRenderScaleForContent(offset, available, context);
    }

    protected override Point DoGetRenderOffsetForContent(Point offset, Size available, ContextBase context)
    {
        var scale = this.DoGetRenderScaleForContent(offset, available, context);
        var newSize = new Size(this.SVGWidth * scale.Width.PointsValue, this.SVGHeight * scale.Height.PointsValue);
        if (available.Width != newSize.Width || available.Height != newSize.Height)
        {
            var offsetH = available.Width.PointsValue - newSize.Width.PointsValue;
            var offsetV = available.Height.PointsValue - newSize.Height.PointsValue;

            if (this.AspectRatio.Meet != AspectRatioMeet.None)
            {
                switch (this.AspectRatio.Align)
                {
                    case(AspectRatioAlign.xMinYMid):
                    case(AspectRatioAlign.xMinYMax):
                    case(AspectRatioAlign.xMinYMin):
                        offsetH = 0.0;
                        break;
                    case(AspectRatioAlign.xMidYMid):
                        case(AspectRatioAlign.xMidYMax):
                        case(AspectRatioAlign.xMidYMin):
                            offsetH = offsetH / 2;
                        break;
                    case(AspectRatioAlign.xMaxYMid):
                    case(AspectRatioAlign.xMaxYMax):
                    case(AspectRatioAlign.xMaxYMin):
                    default:
                        //No change
                        break;
                }
                
                switch (this.AspectRatio.Align)
                {
                    case(AspectRatioAlign.xMinYMin):
                    case(AspectRatioAlign.xMidYMin):
                    case(AspectRatioAlign.xMaxYMin):
                        offsetV = 0.0;
                        break;
                    case(AspectRatioAlign.xMinYMid):
                    case(AspectRatioAlign.xMidYMid):
                    case(AspectRatioAlign.xMaxYMid):
                        offsetV = offsetV / 2;
                        break;
                    case(AspectRatioAlign.xMinYMax):
                    case(AspectRatioAlign.xMidYMax):
                    case(AspectRatioAlign.xMaxYMax):
                    default:
                        //No change
                        break;
                }
                
                offset = new Point(offset.X + offsetH, offset.Y + offsetV);
                
            }
        }
        var def =  base.DoGetRenderOffsetForContent(offset, available, context);
        
        return def;
    }

    protected override PDFTransformationMatrix DoGetCanvasToImageMatrix(Size layoutSize, Point layoutOffset, ContextBase context)
    {
        return PDFTransformationMatrix.Identity();
        var matrix =  base.DoGetCanvasToImageMatrix(layoutSize, layoutOffset, context);
        
        return matrix;
    }
}