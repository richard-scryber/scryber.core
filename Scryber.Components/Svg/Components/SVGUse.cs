using System;
using System.Collections.Generic;
using System.Diagnostics;
using Scryber.Components;
using Scryber.Data;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("use")]
    public class SVGUse : Scryber.Components.PlaceHolder
    {

        #region public string HRef {get;set;}

        private string _href;
        
        /// <summary>
        /// Gets or sets the 
        /// </summary>
        [PDFAttribute("href")]
        public string HRef
        {
            get { return _href; }
            set
            {
                _href = value;
                this.ResetContents();
            }
        }

        #endregion

        #region public PDFStyle Style {get;set;} + public bool HasStyle{get;}

        private Style _style;

        /// <summary>
        /// Gets the applied style for this page Component
        /// </summary>
        [PDFAttribute("style")]
        [PDFElement("Style")]
        public virtual Style Style
        {
            get
            {
                if (_style == null)
                {
                    _style = new Style();
                    _style.Priority = Style.DirectStylePriority;
                }
                return _style;
            }
            set
            {
                if (this._style != null && this._style.HasValues)
                {
                    if (null == value)
                        this._style = null;
                    else
                    {
                        this._style.MergeInto(value);
                        this._style = value;
                    }
                }
                else
                    this._style = value;
            }
        }



        /// <summary>
        /// Gets the flag to indicate if this page Component has style 
        /// information associated with it.
        /// </summary>
        public virtual bool HasStyle
        {
            get { return this._style != null && this._style.HasValues; }
        }

        #endregion

        #region public PDFUnit X {get;set;} + public bool HasX {get;}

        /// <summary>
        /// Gets or Sets the X (Horizontal) position of this page Component
        /// </summary>
        [PDFAttribute("x")]
        [PDFDesignable("X", Ignore = true, Category = "Position", Priority = 1, Type = "PDFUnit")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"left\"")]
        public virtual Unit X
        {
            get
            {
                StyleValue<Unit> x;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.SVGGeometryXKey, out x))
                    return x.Value(this.Style);
                else
                    return Unit.Empty;
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGGeometryXKey, value);
            }
        }

        /// <summary>
        /// Gets the flag to identify if the X position has been set for this Page Component
        /// </summary>
        public virtual bool HasX => this.HasStyle && this._style.IsValueDefined(StyleKeys.SVGGeometryXKey);

        #endregion

        #region public PDFUnit Y {get;set;} + public bool HasY {get;}

        /// <summary>
        /// Gets or sets the Y (vertical) position of the Page Component
        /// </summary>
        [PDFAttribute("y")]
        [PDFDesignable("Y", Ignore = true, Category = "Position", Priority = 1, Type = "PDFUnit")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"top\"")]
        public virtual Unit Y
        {
            get
            {
                StyleValue<Unit> y;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.SVGGeometryYKey, out y))
                    return y.Value(this.Style);
                else
                    return Unit.Empty;
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGGeometryYKey, value);
            }
        }

        /// <summary>
        /// Gets the flag to identifiy is the Y value has been set on this page Component
        /// </summary>
        public virtual bool HasY
        {
            get
            {
                StyleValue<Unit> x;
                return this.HasStyle && this._style.IsValueDefined(StyleKeys.SVGGeometryYKey);
            }
        }

        #endregion

        #region public PDFUnit Width {get;set;} + public bool HasWidth {get;}

        /// <summary>
        /// Gets or Sets the Width of this page Component
        /// </summary>
        [PDFAttribute("width")]
        [PDFDesignable("Width", Category = "Size", Priority = 1, Type = "PDFUnit")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"width\"")]
        public virtual Unit Width
        {
            get
            {
                StyleValue<Unit> width;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.SizeWidthKey, out width))
                    return width.Value(this.Style);
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
                return this.HasStyle && this._style.IsValueDefined(StyleKeys.SizeWidthKey);
            }
        }

        #endregion

        #region public PDFUnit Height {get;set;} + public bool HasHeight {get;}

        /// <summary>
        /// Gets or sets the Height of the Page Component
        /// </summary>
        [PDFAttribute("height")]
        [PDFDesignable("Height", Category = "Size", Priority = 1, Type = "PDFUnit")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"height\"")]
        public virtual Unit Height
        {
            get
            {
                StyleValue<Unit> height;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.SizeHeightKey, out height))
                    return height.Value(this.Style);
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
                return this.HasStyle && this._style.IsValueDefined(StyleKeys.SizeHeightKey);
            }
        }

        #endregion

        [PDFAttribute("rx")]
        public Unit CornerRadiusX
        {
            get
            {
                StyleValue<Unit> value;
                if (this.Style.TryGetValue(StyleKeys.SVGGeometryRadiusXKey, out value))
                    return value.Value(this.Style);
                else
                    return Unit.Zero;
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGGeometryRadiusXKey, value);
            }
        }

        [PDFAttribute("ry")]
        public Unit CornerRadiusY
        {
            get
            {
                if (this.Style.TryGetValue(StyleKeys.SVGGeometryRadiusYKey, out var value))
                    return value.Value(this.Style);
                else
                    return Unit.Zero;
            }
            set => this.Style.SetValue(StyleKeys.SVGGeometryRadiusYKey, value);
        }

        // stroke

        [PDFAttribute("stroke")]
        public Color StrokeColor
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.StrokeColorKey, StandardColors.Transparent);
                else
                {
                    return StandardColors.Transparent;
                }
            }
            set
            { 
                this.Style.SetValue(StyleKeys.StrokeColorKey, value);
            }
        }

        [PDFAttribute("stroke-width")]
        public Unit StrokeWidth 
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.StrokeWidthKey, Unit.Zero);
                else
                {
                    return Unit.Zero;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.StrokeWidthKey, value);
            }
        }

        
        [PDFAttribute("stroke-linecap")]
        public LineCaps StrokeLineCap
        {
            get { return this.Style.Stroke.LineCap; }
            set { this.Style.Stroke.LineCap = value; }
        }

        [PDFAttribute("stroke-dasharray")]
        public Dash StrokeDashPattern
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.StrokeDashKey, Dash.None);
                else
                {
                    return Dash.None;
                }
            }
            set
            {
                if (null != value)
                    this.Style.SetValue(StyleKeys.StrokeDashKey, value);
                else if(this.HasStyle)
                {
                    this.Style.RemoveValue(StyleKeys.StrokeDashKey);
                }
            }
        }



        // fill

        [PDFAttribute("fill")]
        public SVGFillValue FillValue
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.SVGFillKey, SVGFillColorValue.Black);
                else
                {
                    return SVGFillColorValue.Black;
                    
                }
            }
            set
            { 
                this.Style.SetValue(StyleKeys.SVGFillKey, value);
            }
        }

        [PDFAttribute("fill-opacity")]
        public double FillOpacity
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.FillOpacityKey, 1.0);
                else
                {
                    return 1.0;
                }
            }
            set
            { 
                this.Style.SetValue(StyleKeys.FillOpacityKey, value);
            }
        }

        [PDFAttribute("fill-rule")]
        public string FillRule
        {
            get
            {

                if (this.Style.TryGetValue(StyleKeys.GraphicFillModeKey, out StyleValue<GraphicFillMode> mode))
                {
                    switch (mode.Value(this.Style))
                    {
                        case GraphicFillMode.EvenOdd: return "evenodd";
                        default: return "nonzero";
                    }
                }
                else
                    return "nonzero";
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.Style.RemoveValue(StyleKeys.GraphicFillModeKey);
                else if(value == "evenodd")
                    this.Style.SetValue(StyleKeys.GraphicFillModeKey, GraphicFillMode.EvenOdd);
                else if(value == "nonzero")
                    this.Style.SetValue(StyleKeys.GraphicFillModeKey, GraphicFillMode.Winding);

            }
        }

        [PDFAttribute("transform")]
        public TransformOperation TransformMatrix
        {
            get
            {
                if (this.Style.TryGetValue(StyleKeys.TransformOperationKey, out var value))
                    return value.Value(this.Style);
                else
                    return null;
            }
            set
            {
                this.Style.SetValue(StyleKeys.TransformOperationKey, value);
            }
        }

        [PDFAttribute("display")]
        public string Display
        {
            get
            {
                if (this.Visible == false)
                    return "none";
                else if (this.Style.TryGetValue(StyleKeys.PositionDisplayKey, out StyleValue<DisplayMode> posValue))
                    return posValue.Value(this.Style).ToString().ToLower();
                else
                    return "inline";
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value == "inherit")
                    this.Style.RemoveValue(StyleKeys.PositionModeKey);
                else if (value == "none")
                    this.Visible = false;
                else if (value == "initial")
                    this.Style.SetValue(StyleKeys.PositionDisplayKey, DisplayMode.Inline);

                else if (DisplayMode.TryParse(value, true, out DisplayMode parsed))
                    this.Style.SetValue(StyleKeys.PositionDisplayKey, parsed);
                else
                    throw new ArgumentOutOfRangeException("The value '" + value + "' is not supported for the display");

            }

        }

        public SVGUse()
        {
        }



        protected override IEnumerable<IComponent> DoParseContents(ContextBase context)
        {
            if (!string.IsNullOrEmpty(this.HRef))
            {
                return GetContentFromHRef(this.HRef, context);
            }
            else
                return base.DoParseContents(context);
        }

        protected virtual IEnumerable<IComponent> GetContentFromHRef(string href, ContextBase context)
        {
            if (string.IsNullOrEmpty(href))
                throw new ArgumentNullException(nameof(href));

            if (href.StartsWith("#"))
            {
                var id = href.Substring(1);
                var parent = this.Parent;
                while (null != parent)
                {
                    IComponent found;
                    if (parent is SVGCanvas && (parent as SVGCanvas).TryFindComponentByID(id, out found))
                    {
                        return this.GetClonedContent(found);
                    }
                    parent = parent.Parent;
                }

                var comp = this.Document.FindAComponentById(id);
                if (null != comp)
                {
                    return GetClonedContent(comp);
                }

                return new IComponent[] { };
            }
            else throw new NotSupportedException("Full or relative URLs are not currently supported on the SVG Use component");
        }


        protected IEnumerable<IComponent> GetClonedContent(IComponent found)
        {
            IComponent copy;
            if (found is ICloneable)
            {
                copy = (IComponent)(found as ICloneable).Clone();

                if (copy is IStyledComponent)
                    this.ApplyStyle(copy as IStyledComponent);
                
                copy.ID = Document.GetIncrementID(copy.Type);

                return new IComponent[] { copy };
            }
            else
            {
                throw new ArgumentException(
                    "The provided component is not cloneable - so cannot be referenced in a use component", nameof(found));
            }
        }

        private void ApplyStyle(IStyledComponent tocomponent)
        {
            StyleValue<Unit> val;

            if (this.HasStyle)
            {
                foreach (var key in this.Style.Keys)
                {
                    if (key.Equals(StyleKeys.SVGGeometryXKey))
                    {
                        this.AddStyleValues(StyleKeys.SVGGeometryXKey, tocomponent);
                    }
                    else if (key.Equals(StyleKeys.SVGGeometryYKey))
                    {
                        this.AddStyleValues(StyleKeys.SVGGeometryYKey, tocomponent);
                    }
                    else
                    {
                        if (tocomponent.Style.IsValueDefined(key) == false)
                        {
                            //unlike css if the value is not defined on the cloned component, then we copy it across.
                            //This cloned component does not have the value, so we apply it.
                            key.CopyValue(this.Style, tocomponent.Style);
                        }
                        
                    }
                }
            }
        }

        private void AddStyleValues(StyleKey<Unit> key, IStyledComponent toComponent)
        {
            StyleValue<Unit> value;
            StyleValue<Unit> toAdd;
            if (toComponent.Style.TryGetValue(key, out value))
            {
                Unit u1 = value.Value(toComponent.Style);
                
                if (this.Style.TryGetValue(key, out toAdd))
                {
                    //we have both so add them together
                    Unit u2 = toAdd.Value(this.Style);
                    Unit total = u1 + u2;
                    toComponent.Style.SetValue(key, total);
                }
                else
                {
                    //only the component has the value
                    toComponent.Style.SetValue(key, u1);
                }
            }
            else if (this.Style.TryGetValue(key, out toAdd))
            {
                //only we have the value
                toComponent.Style.SetValue(key, toAdd.Value(this.Style));
            }
        }
    }

    
}
