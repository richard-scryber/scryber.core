using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Schema;
using HtmlAgilityPack;
using Scryber.Components;
using Scryber.PDF;
using Scryber.PDF.Graphics;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.Styles;
using SixLabors.ImageSharp.ColorSpaces.Companding;

namespace Scryber.Svg.Components;

using Scryber.Drawing;

[PDFParsableComponent("marker")]
public class SVGMarker : SVGAdorner, IStyledComponent, ICloneable, IResourceContainer
{
    internal const double DefaultMarkerOutputWidth = 3.0;
    internal const double DefaultMarkerOutputHeight = 3.0;
    
    private Style _style;

    [PDFAttribute("style")]
    public Style Style
    {
        get
        {
            if (null == this._style)
                this._style = new Style();
            return this._style;
        }
    }

    public bool HasStyle
    {
        get
        {
            return null != this._style && this._style.HasValues;
        }
    }

    [PDFAttribute("class")]
    public override string StyleClass 
    { 
        get; 
        set;
    }
    
    
    [PDFAttribute("refX")]
    public Unit RefX
    {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryXKey, Unit.Zero);
            else
                return Unit.Zero;
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryXKey, value);
        } 
    }
    
    [PDFAttribute("refY")]
    public Unit RefY {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGGeometryYKey, new Unit(0));
            else
                return new Unit(0.0);
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGGeometryYKey, value);
        } 
    }
    
    #region public PDFRect ViewBox {get; set;}

    [PDFAttribute("viewBox")]
    public Rect ViewBox
    {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.PositionViewPort, Rect.Empty);
            else
                return Rect.Empty;
        }
        set
        {
            this.Style.SetValue(StyleKeys.PositionViewPort, value);
        }
    }

    public void RemoveViewBox()
    {
        this.Style.RemoveValue(StyleKeys.PositionViewPort);
    }

    #endregion
    
    
    [PDFAttribute("markerWidth")]
    public Unit MarkerWidth {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SizeWidthKey, Unit.Auto);
            else
                return Unit.Auto;
        }
        set
        {
            this.Style.SetValue(StyleKeys.SizeWidthKey, value);
        } 
    }
    
    [PDFAttribute("markerHeight")]
    public Unit MarkerHeight{
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SizeHeightKey, Unit.Auto);
            else
                return Unit.Auto;
        }
        set
        {
            this.Style.SetValue(StyleKeys.SizeHeightKey, value);
        } 
    }

    [PDFAttribute("orient")]
    public override AdornmentOrientationValue Orientation
    {
        get
        {
            if (this.HasStyle)
                return this.Style.GetValue(StyleKeys.SVGMarkerOrientationKey, AdornmentOrientationValue.Default);
            else
            {
                return AdornmentOrientationValue.Default;
            }
        }
        set
        {
            this.Style.SetValue(StyleKeys.SVGMarkerOrientationKey, value);
        } 
    }


    [PDFArray(typeof(Component))]
    [PDFElement("")]
    public Scryber.Components.ComponentList Contents
    {
        get { return base.InnerContent; }
        set { base.InnerContent = value; }
    }

    private PDFMarkerXObjectResource _markerResource;
    

    IDocument IResourceContainer.Document => this.Parent.Document as IDocument;

    private PDFResourceList _resources;

    PDFResourceList IResourceContainer.Resources
    {
        get
        {
            if (null == this._resources)
                this._resources = new PDFResourceList(this, false);
            return this._resources;
        }
    }

    public SVGMarker() : this(ObjectTypes.Marker)
    {
    }

    protected SVGMarker(ObjectType type) : base(type)
    {
        
    }

   
    public override PDFName OutputAdornment(PDFGraphics toGraphics, PathAdornmentInfo info, ContextBase context)
    {
        var renderContext = (PDFRenderContext)context;
        var size = this.GetMarkerSize(toGraphics);
        
        if (this._markerResource == null)
        {
            var canvas = this.GetParentCanvas();
            this._markerResource = new PDFMarkerXObjectResource(this, canvas);
        }
        RegisterXObject(toGraphics, renderContext, info);
        size = this.GetOutputSize(toGraphics);
        this.OutputMarkerDraw(size, info, toGraphics, context);
        
        
        return this._markerResource.Name;
    }

    protected virtual PDFObjectRef RenderInnerContent(PDFRenderContext context, PDFWriter writer)
    {
        PDFObjectRef oref = null;
        foreach (var component in this.Contents)
        {
            if(component is Whitespace)
                continue;
            else if (component is IPDFRenderComponent renderer)
            {
                Style style = null;
                
                if (component is IStyledComponent styled)
                {
                    style = styled.GetAppliedStyle();
                    context.StyleStack.Push(style);
                    var fh = context.FullStyle.GetValue(StyleKeys.FontSizeKey, Font.DefaultFontSize);
                    var fw = fh / 2.0;
                    var full = context.StyleStack.GetFullStyle(component, context.PageSize,
                        (c, s, p) => context.Space,
                        new Size(fw, fh), Font.DefaultFontSize);
                    
                    context.FullStyle = full;
                }
                
                oref = renderer.OutputToPDF(context, writer);

                if (null != style)
                    context.StyleStack.Pop();
            }
        }

        return oref;

    }

    private SVGCanvas GetParentCanvas()
    {
        var parent = this.Parent;
        while (null != parent)
        {
            if (parent is SVGCanvas canvas)
                return canvas;
        }

        return null;
    }
    
    /// <summary>
    /// Returns the size of the marker view
    /// </summary>
    /// <param name="src"></param>
    /// <returns></returns>
    protected virtual Size GetMarkerSize(PDFGraphics src)
    {
        var w = this.Style.GetValue(StyleKeys.SizeWidthKey, DefaultMarkerOutputWidth); //default as per spec is 3
        var h = this.Style.GetValue(StyleKeys.SizeHeightKey, DefaultMarkerOutputHeight); //default as per spec is 3
        if (this.ViewBox != Rect.Empty)
        {
            w = this.ViewBox.Width - this.ViewBox.X;
            h = this.ViewBox.Height - this.ViewBox.Y;
        }
        return new Size(w, h);
    }

    protected virtual Size GetOutputSize(PDFGraphics content)
    {
         var w = this.Style.GetValue(StyleKeys.SizeWidthKey, DefaultMarkerOutputWidth); //default as per spec is 3
         var h = this.Style.GetValue(StyleKeys.SizeHeightKey, DefaultMarkerOutputHeight); //default as per spec is 3
         return new Size(w, h);
    }

    protected virtual PDFGraphics DoSetUpXObject(PDFWriter writer, Size size, ContextBase context)
    {
        var oref = writer.BeginObject();
        writer.BeginStream(oref);
        var graphics = PDFGraphics.Create(writer, false, this, DrawingOrigin.BottomLeft, size, context);
        
        return graphics;
    }


    protected virtual void OutputMarkerDraw(Size size, PathAdornmentInfo info, PDFGraphics graphics,
        ContextBase context)
    {
        this._markerResource.EnsureRendered(context, graphics.Writer);


        //info.Location = new Point(90, 90);
        graphics.SaveGraphicsState();
        var matrix = this.ApplyTransformation(graphics, size, info);
        graphics.PaintXObject(this._markerResource);
        graphics.RestoreGraphicsState();





    }

    protected virtual PDFTransformationMatrix ApplyTransformation(PDFGraphics graphics, Size size, PathAdornmentInfo info)
    {
        
        var transform = new SVGTransformOperationSet();
        var origin = new TransformOrigin(0,  graphics.ContainerSize.Height );

        
        transform.AppendOperation(new TransformRotateOperation(info.AngleRadians));
        transform.AppendOperation(new TransformTranslateOperation(info.Location.X,
            info.Location.Y - graphics.ContainerSize.Height));
        
        var matrix = transform.GetTransformationMatrix(graphics.ContainerSize, origin);
        
        graphics.SetTransformationMatrix(matrix, true, true);
        return matrix;
    }
    

    protected virtual PDFName RegisterXObject(PDFGraphics srcGraphics, PDFRenderContext context, PathAdornmentInfo info)
    {
        srcGraphics.Container.Register(this._markerResource);
        return (PDFName)this._markerResource.Name;
    }

    public virtual SVGMarker Clone()
    {
        var clone = (SVGMarker)this.MemberwiseClone();
        clone.InnerContent.Clear();
        
        foreach(var inner in this.Contents)
        {
            if (inner is ICloneable cloneable)
                clone.InnerContent.Add((Component)cloneable.Clone());
            else
            {
                clone.InnerContent.Add(inner);
            }
        }
        
        if (!string.IsNullOrEmpty(this.ID) && null != this.Document)
            clone.ID = this.Document.GetIncrementID(ObjectTypes.Marker);
        
        return clone;

    }

    object ICloneable.Clone()
    {
        return this.Clone();
    }

    public string Register(ISharedResource resource)
    {
        if (null == this._resources)
            this._resources = new PDFResourceList(this, false);
        var rsrc = resource as PDFResource;
        
        if (null != rsrc)
        {
            rsrc.RegisterUse(this._resources, this);
            this._resources.EnsureInList(rsrc);
            return rsrc.Name.Value;
        }
        else
        {
            return string.Empty;
        }
    }
    
    private class PDFMarkerXObjectResource : PDFResource
    {
        private SVGMarker _marker;
        public PDFMarkerXObjectResource(SVGMarker marker, SVGCanvas canvas) : base(ObjectTypes.Marker)
        {
            this._marker = marker ?? throw new ArgumentNullException(nameof(marker));
        }

        public override string ResourceKey
        {
            get { return this._marker.UniqueID; }
        }

        public override string ResourceType
        {
            get { return PDFResource.XObjectResourceType; }
        }

        protected override PDFObjectRef DoRenderToPDF(ContextBase context, PDFWriter writer)
        {
            if (null == this.RenderReference)
            {
                var renderContext = (PDFRenderContext)context;
                var size = this._marker.GetMarkerSize(renderContext.Graphics);
                var origGraphics = renderContext.Graphics;
                var origOffset = renderContext.Offset;
                var origSize = renderContext.Space;
                
                using (var graphics = this.DoSetUpXObject(writer, size, renderContext))
                {
                    renderContext.Graphics = graphics;
                    renderContext.Offset = Point.Empty;
                    renderContext.Space = size;
                    this._marker.RenderInnerContent(renderContext, writer);
                }

                this.OutputXObjectDictionary(size, writer, renderContext);

                renderContext.Graphics = origGraphics;
                renderContext.Space = origSize;
                renderContext.Offset = origOffset;
            }
            

            return this.RenderReference;
            
        }

        private PDFGraphics DoSetUpXObject(PDFWriter writer, Size size, PDFRenderContext context)
        {
            var oref = writer.BeginObject();
            this.RenderReference = oref;
            writer.BeginStream(oref);

            PDFGraphics g = PDFGraphics.Create(writer, false, this._marker, DrawingOrigin.TopLeft, size, context);
            return g;
        }

        private void OutputXObjectDictionary(Size size, PDFWriter writer, PDFRenderContext context)
        {
            //Need the viewport info.
            var len = writer.EndStream();
            writer.WriteCommentLine("Beginning output of marker xObject from the resource");
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "XObject");
            writer.WriteDictionaryNameEntry("Subtype", "Form");
            writer.BeginDictionaryEntry("Matrix");
            
            //TODO - set up the transform matrix
            var matrix = new PDFTransformationMatrix();
            var xScale = 1.0;
            var yScale = 1.0;
            if (this._marker.ViewBox != Rect.Empty)
            {
                var vbw = this._marker.ViewBox.Width.PointsValue - this._marker.ViewBox.X.PointsValue;
                var vbh = this._marker.ViewBox.Height.PointsValue - this._marker.ViewBox.Y.PointsValue;
                
                var mkw = this._marker.MarkerWidth.PointsValue;
                
                if (mkw == 0.0)
                    mkw = 3.0;
                
                var mkh = this._marker.MarkerHeight.PointsValue;
                
                if (mkh == 0.0)
                    mkh = 3.0;
                
                xScale = mkw / vbw;
                yScale = mkh / vbh;

                var offsetX = 0.0 - this._marker.RefX;
                if (offsetX != 0.0)
                    offsetX *= xScale;

                var offsetY = 0.0 - this._marker.RefY;
                if (offsetY != 0.0)
                    offsetY *= yScale;

                matrix.SetScale((float)xScale, (float)yScale);
                matrix.SetTranslation(offsetX, offsetY);
            }
            
            writer.WriteArrayRealEntries(matrix.Components);
            writer.EndDictionaryEntry();
            
            if (null != this._marker._resources)
            {
                var rsrc = this._marker._resources.WriteResourceList(context, writer);
                if (null != rsrc)
                {
                    writer.WriteDictionaryObjectRefEntry("Resources", rsrc);
                }
            }

            var bbox = new double[4];
            if (this._marker.ViewBox != Rect.Empty)
            {
                bbox[0] = this._marker.ViewBox.X.PointsValue;
                bbox[1] = this._marker.ViewBox.Y.PointsValue;
                bbox[2] = this._marker.ViewBox.Width.PointsValue;
                bbox[3] = this._marker.ViewBox.Height.PointsValue;
            }
            else
            {
                bbox[0] = 0.0;
                bbox[1] = 0.0;
                bbox[2] = size.Width.PointsValue;
                bbox[3] = size.Height.PointsValue;
            }
            
            writer.BeginDictionaryEntry("BBox");
            writer.WriteArrayRealEntries(bbox);
            writer.EndDictionaryEntry();
            

            writer.WriteDictionaryNumberEntry("Length", len);
            writer.EndDictionary();
            writer.EndObject();
        }
    }
    
}