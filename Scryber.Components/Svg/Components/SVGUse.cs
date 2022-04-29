using System;
using System.Collections.Generic;
using Scryber.Components;
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
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.PositionXKey, out x))
                    return x.Value(this.Style);
                else
                    return Unit.Empty;
            }
            set
            {
                this.Style.SetValue(StyleKeys.PositionXKey, value);
            }
        }

        /// <summary>
        /// Gets the flag to identify if the X position has been set for this Page Component
        /// </summary>
        public virtual bool HasX => this.HasStyle && this._style.IsValueDefined(StyleKeys.PositionXKey);

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
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.PositionYKey, out y))
                    return y.Value(this.Style);
                else
                    return Unit.Empty;
            }
            set
            {
                this.Style.SetValue(StyleKeys.PositionYKey, value);
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
                return this.HasStyle && this._style.IsValueDefined(StyleKeys.PositionYKey);
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
            if (found is ICloneable)
                found = (found as ICloneable).Clone() as IComponent;

            if (found is IStyledComponent)
                this.ApplySizeStyle(found as IStyledComponent);
            
            
            return new IComponent[] { found };
        }

        private void ApplySizeStyle(IStyledComponent found)
        {
            StyleValue<Unit> val;

            if(this.HasX)
            {
                if (found.HasStyle && found.Style.TryGetValue(StyleKeys.PositionXKey, out val))
                    found.Style.SetValue(StyleKeys.PositionXKey, val.Value(found.Style) + this.X);
                else
                    found.Style.SetValue(StyleKeys.PositionXKey, this.X);
            }

            if (this.HasY)
            {
                if (found.HasStyle && found.Style.TryGetValue(StyleKeys.PositionYKey, out val))
                    found.Style.SetValue(StyleKeys.PositionYKey, val.Value(found.Style) + this.Y);
                else
                    found.Style.SetValue(StyleKeys.PositionYKey, this.Y);
            }

            if (this.HasWidth)
            {
                if (found.HasStyle && found.Style.TryGetValue(StyleKeys.SizeWidthKey, out val))
                    found.Style.SetValue(StyleKeys.SizeWidthKey, val.Value(found.Style) + this.Width);
                else
                    found.Style.SetValue(StyleKeys.SizeWidthKey, this.Width);
            }

            if (this.HasHeight)
            {
                if (found.HasStyle && found.Style.TryGetValue(StyleKeys.SizeHeightKey, out val))
                    found.Style.SetValue(StyleKeys.SizeHeightKey, val.Value(found.Style) + this.Height);
                else
                    found.Style.SetValue(StyleKeys.SizeHeightKey, this.Height);
            }
        }
    }

    
}
