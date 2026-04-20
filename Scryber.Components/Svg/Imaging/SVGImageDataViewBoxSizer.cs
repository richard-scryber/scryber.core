using System;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.PDF.Graphics;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging;

public class SVGImageDataViewBoxSizer : SVGImageDataSizer
{
    protected Rect ViewBox { get; set; }
    
    protected ViewPortAspectRatio AspectRatio {get; set;}
    
    
    
    public SVGImageDataViewBoxSizer(SVGCanvas forCanvas, Style appliedStyle, LayoutContext context)
        : base(forCanvas, appliedStyle, context)
    {
        var empty = (!appliedStyle.IsValueDefined(StyleKeys.SizeWidthKey) &&
                     !appliedStyle.IsValueDefined(StyleKeys.SizeHeightKey));
        
        var hasVb = appliedStyle.IsValueDefined(StyleKeys.PositionViewPort);
        
        if (!empty || !hasVb) 
            throw new ArgumentOutOfRangeException(@"This SVG Image data sizing strategy can only be used with canvas's and styles that have only a viewbox specified. Use the SVGImageDataSizer.CreateSizingStrategy to create the correct instance.", (Exception)null);
        
        this.ViewBox = appliedStyle.GetValue(StyleKeys.PositionViewPort, Rect.Empty);

        if (appliedStyle.TryGetValue(StyleKeys.ViewPortAspectRatioStyleKey, out var found))
        {
            var value = found.Value(appliedStyle);
            AspectRatio = value;
        }
        else
        {
            AspectRatio = ViewPortAspectRatio.Default;
        }
        
    }

    protected override Size DoGetLayoutSize()
    {
        var size = base.DoGetLayoutSize();
        return size;
    }

    protected virtual Size DoGetProportionalSize(Size layout, Size available, ContextBase context)
    {
        var scale = this.GetRenderScaleForContent(Point.Empty, available, context);
        
        var min = DoGetMinimumnImageScale();
        if (scale.Width.PointsValue < min || scale.Height.PointsValue < min)
        {
            scale.Width = available.Width.PointsValue / this.ViewBox.Width.PointsValue;
            scale.Height = scale.Width;
            //scale = new Size(1, 1);
        }

        var proportional = Size.Empty;
        
        //Use the minimum scale to fit
        if (scale.Width > scale.Height)
        {
            scale.Width = this.ViewBox.Width * scale.Height.PointsValue;
            proportional.Height = available.Height;
            proportional.Width = this.ViewBox.Width * scale.Height.PointsValue;
        }
        else
        {
            proportional.Width = available.Width;
            proportional.Height = this.ViewBox.Height * scale.Width.PointsValue;
        }
        return proportional;
    }

    protected virtual double DoGetMinimumnImageScale()
    {
        var services = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
        if (null != services)
        {
            return services.ImagingOptions.MinimumScaleReduction;
        }
        else
        {
            return 0.249;
        }
    }

    protected override Size DoGetOutputSizeForLayout(Size layout, Size available, Style applied, LayoutContext context)
    {
        var pos = applied.CreatePostionOptions(context.PositionDepth > 0);
        
        if(pos.Width.HasValue || pos.Height.HasValue)
            return base.DoGetOutputSizeForLayout(layout, available, applied, context);
        
        //scale proportionally to the available size
        if(layout == available)
            return layout;
        
        var proportional = this.DoGetProportionalSize(layout, available, context);
        
        return proportional;
    }

    protected override Point DoGetRenderOffsetForContent(Point offset, Size available, ContextBase context)
    {
        var newOffset = base.DoGetRenderOffsetForContent(offset, available, context);
        var scale = this.GetRenderScaleForContent(Point.Empty, available, context);
        
        //calculate the output size based on the view box, available space, scale, and the alignment
        var size = this.ViewBox.Size;
        
        //actual output size
        size.Width = size.Width * scale.Width.PointsValue;
        size.Height = size.Height * scale.Height.PointsValue;
        
        var adjust = GetOffsetsForAspecRatio(size, available, context);
        newOffset.X += adjust.X;
        newOffset.Y += adjust.Y;
        
        return newOffset;
    }

    private Point GetOffsetsForAspecRatio(Size contentSize, Size available, ContextBase context)
    {
        var spaceH = available.Width -  contentSize.Width;
        var spaceV = available.Height -  contentSize.Height;
        
        Point adjust = Point.Empty;
        
        //vertical first
        switch (this.AspectRatio.Align)
        {
            case AspectRatioAlign.None:
            case AspectRatioAlign.xMidYMid:
            case AspectRatioAlign.xMaxYMid:
            case AspectRatioAlign.xMinYMid:
                adjust.Y = spaceV / 2.0;
                break;
            
            case AspectRatioAlign.xMidYMin:
            case AspectRatioAlign.xMaxYMin:
            case AspectRatioAlign.xMinYMin:
                adjust.Y = 0;
                break;
            
            case AspectRatioAlign.xMidYMax:
            case AspectRatioAlign.xMaxYMax:
            case AspectRatioAlign.xMinYMax:
                adjust.Y = spaceV;
                break;
            
            default:
                throw new ArgumentOutOfRangeException("Unknown aspect ratio : "  + this.AspectRatio.Align);
        }
        
        //now each of the horizontal
        switch (this.AspectRatio.Align)
        {
            case AspectRatioAlign.None:
            case AspectRatioAlign.xMidYMin:
            case AspectRatioAlign.xMidYMid:
            case AspectRatioAlign.xMidYMax:
                adjust.X = spaceH / 2.0;
                break;
            
            case AspectRatioAlign.xMinYMin:
            case AspectRatioAlign.xMinYMid:
            case AspectRatioAlign.xMinYMax:
                adjust.X = 0;
                break;
            
            case AspectRatioAlign.xMaxYMin:
            case AspectRatioAlign.xMaxYMid:
            case AspectRatioAlign.xMaxYMax:
                adjust.X = spaceH;
                break;
            
            default:
                throw new ArgumentOutOfRangeException("Unknown aspect ratio : "  + this.AspectRatio.Align);
        }
        
        return adjust;
    }


    protected override Size DoGetRenderScaleForContent(Point offset, Size available, ContextBase context)
    {
        var scale = base.DoGetRenderScaleForContent(offset, available, context);
        if (this.AspectRatio.Align == AspectRatioAlign.None)
        {
            //None proportional scaling - keep as default
        }
        else if (this.AspectRatio.Meet == AspectRatioMeet.Meet)
        {
            //meet is the smallest
            if (scale.Width > scale.Height)
                scale.Width = scale.Height;
            else
                scale.Height = scale.Width;
        }
        else //slice takes the boggest proportion
        {
            if (scale.Width < scale.Height)
                scale.Width = scale.Height;
            else
                scale.Height = scale.Width;
        }

        return scale;
        
    }
}