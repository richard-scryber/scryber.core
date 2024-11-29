using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.Styles;

namespace Scryber.Svg.Components;

[PDFParsableComponentAttribute("image")]
public class SVGImage : Scryber.Components.Image
{
    [PDFAttribute("href")]
    public override string Source
    {
        get { return base.Source; }
        set { base.Source = value; }
    }
    
    [PDFAttribute("x")]
    public override Unit X {
        get
        {
            StyleValue<Unit> value;
            if (this.Style.TryGetValue(StyleKeys.SVGGeometryXKey, out value))
                return value.Value(this.Style);
            else
                return Unit.Zero;
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryXKey, value);
        }
    }

    [PDFAttribute("y")]
    public override Unit Y {
        get
        {
            StyleValue<Unit> value;
            if (this.Style.TryGetValue(StyleKeys.SVGGeometryYKey, out value))
                return value.Value(this.Style);
            else
                return Unit.Zero;
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryYKey, value);
        }
    }

    
    [PDFAttribute("class")]
    public override string StyleClass
    {
        get { return base.StyleClass; }
        set { base.StyleClass = value; }

    }

    #region public PDFUnit Width {get;set;} + public bool HasWidth {get;}

        /// <summary>
        /// Gets or Sets the Width of this page Component
        /// </summary>
        [PDFAttribute("width")]
        [PDFDesignable("Width", Category = "Size", Priority = 1, Type = "PDFUnit")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"width\"")]
        public override Unit Width
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.SizeWidthKey, Unit.Empty);
                else
                    return Unit.Empty;
            }
            set
            {
                this.Style.SetValue(StyleKeys.SizeWidthKey, value);
            }
        }

        

        /// <summary>
        /// Gets the flag to identify if the Width has been set for this Page Component
        /// </summary>
        public virtual bool HasWidth
        {
            get
            {
                StyleValue<Unit> width;
                return this.HasStyle && this.Style.TryGetValue(StyleKeys.SizeWidthKey, out width);
            }
        }

        #endregion

        #region public PDFUnit Height {get;set;} + public bool HasHeight {get;}

        /// <summary>
        /// Gets or sets the Height of the Page Component
        /// </summary>
        [PDFAttribute("height")]
        public override Unit Height
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.SizeHeightKey, Unit.Empty);
                else
                    return Unit.Empty;
            }
            set
            {
                this.Style.SetValue(StyleKeys.SizeHeightKey, value);
            }
        }

        /// <summary>
        /// Gets the flag to identifiy is the Height has been set on this page Component
        /// </summary>
        public virtual bool HasHeight
        {
            get
            {
                StyleValue<Unit> height;
                return this.HasStyle && this.Style.TryGetValue(StyleKeys.SizeHeightKey, out height);
            }
        }

        #endregion
        
        [PDFAttribute("transform")]
        public SVGTransformOperationSet Transform
        {
            get
            {
                if (this.Style.TryGetValue(StyleKeys.TransformOperationKey, out var value))
                    return value.Value(this.Style) as SVGTransformOperationSet;
                else
                    return null;
            }
            set
            {
                this.Style.SetValue(StyleKeys.TransformOperationKey, value);
            }
        }

        [PDFAttribute("opacity")]
        public override double FillOpacity
        {
            get { return base.FillOpacity; }
            set { base.FillOpacity = value; }
        }

        protected Rect TransformedBounds { get; set; }
        

    protected override Style GetBaseStyle()
    {
        var style = base.GetBaseStyle();
        style.Position.PositionMode = PositionMode.Absolute;
        style.Position.DisplayMode = DisplayMode.Block;

        return style;
    }

    public override PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
    {
        context.TraceLog.Add(TraceLevel.Message, "SVG", "Outputting the SVG Image with source " + this.Source);
        var cssW = context.FullStyle?.GetValue(StyleKeys.SizeWidthKey, Unit.Zero) ?? Unit.Zero;
        var cssH = context.FullStyle?.GetValue(StyleKeys.SizeHeightKey, Unit.Zero) ?? Unit.Zero;
        
        var w = this.Style.GetValue(StyleKeys.SizeWidthKey, cssW);
        var h = this.Style.GetValue(StyleKeys.SizeWidthKey, cssW);
        var x = this.X; //always explicit
        var y = this.Y; //always explicit

        var transform = context.FullStyle?.GetValue(StyleKeys.TransformOperationKey, null);
        if (null != transform)
        {
            var origin = context.FullStyle?.GetValue(StyleKeys.TransformOriginKey, null);

            var size = context.Graphics.ContainerSize;
            var matrix = transform.GetTransformationMatrix(size, origin);
            
            context.Graphics.SaveGraphicsState();
            context.Graphics.SetTransformationMatrix(matrix, true, true);

            var bounds = new Rect(x, y, w, h);
            var transformed = matrix.TransformBounds(bounds);
            this.TransformedBounds = transformed;

        }
        
        context.Offset = new Point(x, y);
        context.Space = new Size(w, h);
        
        var oref = base.OutputToPDF(context, writer);

        if (null != transform)
        {
            context.Graphics.RestoreGraphicsState();
        }

        return oref;
    }
}