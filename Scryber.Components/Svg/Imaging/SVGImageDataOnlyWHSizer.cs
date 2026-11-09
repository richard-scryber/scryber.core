using System;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.PDF.Graphics;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging;

/// <summary>
/// Implements the sizing for an SVG Image Reference when the referenced image has only explicit width and or height.
/// </summary>
public class SVGImageDataOnlyWHSizer : SVGImageDataSizer
{
    
    protected Unit? SVGWidth { get; set; }
    protected Unit? SVGHeight { get; set; }
    
    protected ViewPortAspectRatio AspectRatio {get; set;}

    public SVGImageDataOnlyWHSizer(SVGCanvas forCanvas, Style appliedStyle, LayoutContext context)
        : base(forCanvas, appliedStyle, context)
    {
        if (appliedStyle.TryGetValue(StyleKeys.ViewPortAspectRatioStyleKey, out var found))
        {
            var value = found.Value(appliedStyle);
            AspectRatio = value;
        }
        else
        {
            AspectRatio = ViewPortAspectRatio.Default;
        }
        

        if (appliedStyle.TryGetValue(StyleKeys.SizeWidthKey, out var w))
        {
            this.SVGWidth = w.Value(appliedStyle);
        }

        if (appliedStyle.TryGetValue(StyleKeys.SizeHeightKey, out var h))
        {
            SVGHeight = h.Value(appliedStyle);
        }
    }

    protected override Size DoGetOutputSizeForLayout(Size layout, Size available, Style applied, LayoutContext context)
    {
        var width = SVGCanvas.DefaultWidth;
        var height = SVGCanvas.DefaultHeight;
        
        bool hasWidth = false;
        bool hasHeight = false;

        if (applied.TryGetValue(StyleKeys.SizeWidthKey, out var w))
        {
            width = w.Value(applied);
            hasWidth = true;
        }

        if (applied.TryGetValue(StyleKeys.SizeHeightKey, out var h))
        {
            height = h.Value(applied);
            hasHeight = true;
        }
        
        // if (this.AspectRatio.Align == AspectRatioAlign.None)
        // {
        //     //Not sure - but matches chrome.
        //     if(SVGWidth.HasValue && SVGHeight.HasValue)
        //         return new Size(SVGWidth.Value, SVGHeight.Value);
        //     
        //
        //     return new Size(width, height);
        //     
        // }

        if (hasWidth || hasHeight)
        {
            if(hasWidth &&  hasHeight)
                return new Size(width, height);

            double? aspectRatio = null;
            if (applied.TryGetValue(StyleKeys.SizeAspectRatioKey, out var arValue))
                aspectRatio = arValue.Value(applied);

            if (aspectRatio.HasValue && aspectRatio.Value > 0)
            {
                //An explicit aspect-ratio (or one derived from the outer img's intrinsic width/height
                //attributes) takes priority over the referenced SVG's own width/height ratio.
                if (hasHeight)
                    width = height * aspectRatio.Value;
                else
                    height = width / aspectRatio.Value;
            }
            else if (SVGWidth.HasValue && SVGHeight.HasValue)
            {
                //We have both the SVG Sizes, but only 1 img size so calculate the correct ratio and apply

                if (hasHeight)
                {
                    var ratio = height.PointsValue / SVGHeight.Value.PointsValue;
                    width = SVGWidth.Value * ratio;
                }
                else
                {
                    var ratio = width.PointsValue / SVGWidth.Value.PointsValue;
                    height = SVGHeight.Value * ratio;
                }
            }
            else
            {

                if (!hasWidth && SVGWidth.HasValue)
                    width = SVGWidth.Value;

                if (!hasHeight && SVGHeight.HasValue)
                    height = SVGHeight.Value;

                // if (width > available.Width)
                // {
                //     var scale = available.Width.PointsValue / width.PointsValue;
                //     width = available.Width;
                //     height = available.Height.PointsValue  * scale;
                //     
                // }
            }

            return new Size(width, height);
        }
        else
        {
            // Neither explicit - start from the SVG's own natural declared size (layout, from the
            // base GetLayoutSize() - NOT the local width/height above, which are still stuck at
            // the arbitrary 300x150 UA default here since nothing has set them from SVGWidth/
            // SVGHeight in this branch), shrink to fit if wider than available (existing
            // behaviour), then apply min/max on top, deriving whichever axis isn't independently
            // constrained from the SVG's own declared ratio (confirmed against real browser
            // rendering - mirrors the equivalent fix in SVGImageDataEmptySizer /
            // SVGImageDataViewBoxSizer). Max is applied before min so min wins on conflict (also
            // confirmed against real browser rendering). If both axes end up independently
            // constrained, the ratio is allowed to break.
            Size calculated = layout;

            if (calculated.Width > available.Width)
            {
                var scale = available.Width.PointsValue / calculated.Width.PointsValue;
                calculated.Width = available.Width;
                calculated.Height = calculated.Height.PointsValue * scale;
            }

            var pos = applied.CreatePostionOptions(context.PositionDepth > 0);
            bool hasW = false, hasH = false;

            if (pos.MaximumHeight.HasValue && calculated.Height > pos.MaximumHeight.Value)
            {
                calculated.Height = pos.MaximumHeight.Value;
                hasH = true;
            }

            if (pos.MaximumWidth.HasValue && calculated.Width > pos.MaximumWidth.Value)
            {
                calculated.Width = pos.MaximumWidth.Value;
                hasW = true;
            }

            if (pos.MinimumHeight.HasValue && calculated.Height < pos.MinimumHeight.Value)
            {
                calculated.Height = pos.MinimumHeight.Value;
                hasH = true;
            }

            if (pos.MinimumWidth.HasValue && calculated.Width < pos.MinimumWidth.Value)
            {
                calculated.Width = pos.MinimumWidth.Value;
                hasW = true;
            }

            if (hasW && !hasH)
                calculated.Height = calculated.Width * (layout.Height.PointsValue / layout.Width.PointsValue);
            else if (hasH && !hasW)
                calculated.Width = calculated.Height * (layout.Width.PointsValue / layout.Height.PointsValue);

            return calculated;
        }
    }


    protected override Size DoGetRenderScaleForContent(Point offset, Size available, ContextBase context)
    {
        var scale = base.DoGetRenderScaleForContent(offset, available, context);
        
        return scale;
    }

    protected override PDFTransformationMatrix DoGetCanvasToImageMatrix(Size layoutSize, Point layoutOffset, ContextBase context)
    {
        var matrix = base.DoGetCanvasToImageMatrix(layoutSize, layoutOffset, context);
        return matrix;
    }
}