using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Schema;
using Scryber.Components;
using Scryber.PDF;
using Scryber.PDF.Graphics;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.Styles;

namespace Scryber.Svg.Components;

using Scryber.Drawing;

[PDFParsableComponent("marker")]
public class SVGMarker : SVGAdorner, IStyledComponent, ICloneable
{
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
    public string StyleClass 
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
    public Unit X2 {
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
    public string Orient { get; set; }


    [PDFArray(typeof(Component))]
    [PDFElement("")]
    public Scryber.Components.ComponentList Contents
    {
        get { return base.InnerContent; }
        set { base.InnerContent = value; }
    }

    
    public SVGMarker() : this(ObjectTypes.Marker)
    {
    }

    protected SVGMarker(ObjectType type) : base(type)
    {
        // this.X1 = Unit.Zero;
        // this.X2 = new Unit(1.0);
        // this.Y1 = Unit.Zero;
        // this.Y2 = new Unit(1.0);
        // this.SpreadMode = GradientSpreadMode.Pad;
        // this.GradientUnits = GradientUnitType.ObjectBoundingBox;
    }

    public override PDFName OutputAdornment(PDFGraphics toGraphics, PathAdornmentInfo info, ContextBase context)
    {
        //TODO: put this in an xobject, with a viewbox and transform to the right place.
        var content = this.Contents[1] as SVGPath;
        
        if (null != content)
        {
            content.OutputToPDF(context as PDFRenderContext, toGraphics.Writer);
        }

        return null;
        
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
    
    
}