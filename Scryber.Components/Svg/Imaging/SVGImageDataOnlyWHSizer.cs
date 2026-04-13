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
        
        if (this.AspectRatio.Align == AspectRatioAlign.None)
        {
            //Not sure - but matches chrome.
            if(SVGWidth.HasValue && SVGHeight.HasValue)
                return new Size(SVGWidth.Value, SVGHeight.Value);
            

            return new Size(width, height);
            
        }

        if (hasWidth || hasHeight)
        {
            if(!hasWidth && SVGWidth.HasValue)
                width = SVGWidth.Value;
            
            if(!hasHeight && SVGHeight.HasValue)
                height = SVGHeight.Value;
            
            return new Size(width, height);
        }
        
            
        
        return base.DoGetOutputSizeForLayout(layout, available, applied, context);
    }


    protected override Size DoGetRenderScaleForContent(Point offset, Size available, ContextBase context)
    {
        var scale = base.DoGetRenderScaleForContent(offset, available, context);

        return scale;
    }

    protected override PDFTransformationMatrix DoGetCanvasToImageMatrix(ContextBase context)
    {
        var matrix = base.DoGetCanvasToImageMatrix(context);
        return matrix;
    }
}