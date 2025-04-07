using System;
using Scryber.Styles;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.Svg.Layout;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("svg")]
    public class SVGCanvas : Scryber.Components.Canvas, IResourceContainer, ICanvas, INamingContainer
    {

        //pre-defined width and heights
        //based on html defaults
        
        public static readonly Unit DefaultWidth = new Unit(300);
        public static readonly Unit DefaultHeight = new Unit(150);

        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        [PDFAttribute("hidden")]
        public string Hidden
        {
            get
            {
                if (this.Visible)
                    return string.Empty;
                else
                    return "hidden";
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value != "hidden")
                    this.Visible = true;
                else
                    this.Visible = false;
            }
        }

        [PDFElement("")]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents
        {
            get { return base.Contents; }
        }

        [PDFAttribute("x")]
        public override Unit X { get => base.X; set => base.X = value; }

        [PDFAttribute("y")]
        public override Unit Y { get => base.Y; set => base.Y = value; }

        private ComponentList _definitions;

        [PDFElement("defs")]
        [PDFArray(typeof(Component))]
        public ComponentList Definitions
        {
            get
            {
                if (_definitions == null)
                    _definitions = new ComponentList(this, ObjectTypes.ShapePath);
                return _definitions;
            }
        }

        public bool HasDefinitions
        {
            get
            {
                if(_definitions != null && _definitions.Count > 0)
                    return true;
                else
                {
                    return false;
                }
            }
        }


        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        [PDFAttribute("stroke")]
        public override Color StrokeColor
        {
            get => base.StrokeColor;
            set => base.StrokeColor = value;
        }


        [PDFAttribute("stroke-width")]
        public override Unit StrokeWidth
        {
            get => base.StrokeWidth;
            set => base.StrokeWidth = value;
        }

        [PDFAttribute("stroke-linecap")]
        public LineCaps StrokeLineCap
        {
            get { return this.Style.Stroke.LineCap; }
            set { this.Style.Stroke.LineCap = value; }
        }

        [PDFAttribute("stroke-linejoin")]
        public LineJoin StrokeLineJoin
        {
            get { return this.Style.Stroke.LineJoin; }
            set { this.Style.Stroke.LineJoin = value; }
        }

        [PDFAttribute("stroke-dasharray")]
        public override Dash StrokeDashPattern
        {
            get => base.StrokeDashPattern;
            set => base.StrokeDashPattern = value;
        }

        [PDFAttribute("stroke-opacity")]
        public override double StrokeOpacity { get => base.StrokeOpacity; set => base.StrokeOpacity = value; }

        [PDFAttribute("fill-opacity")]
        public override double FillOpacity { get => base.FillOpacity; set => base.FillOpacity = value; }

        // fill

        [PDFAttribute("fill")]
        public override Color FillColor
        {
            get => base.FillColor;
            set => base.FillColor = value;
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

        #region public SVGAspectRatio PreserveAspectRatio

        [PDFAttribute("preserveAspectRatio")]
        public ViewPortAspectRatio PreserveAspectRatio
        {
            get
            {
                if (this.HasStyle)
                {
                    return this.Style.GetValue(StyleKeys.ViewPortAspectRatioStyleKey, ViewPortAspectRatio.Default);
                }
                else
                    return ViewPortAspectRatio.Default;
            }
            set
            {
                this.Style.SetValue(StyleKeys.ViewPortAspectRatioStyleKey, value);
            }
        }

        public void RemoveAspectRatio()
        {
            this.Style.RemoveValue(StyleKeys.ViewPortAspectRatioStyleKey);
        }

        #endregion

        //style attributes
        

        [PDFAttribute("width")]
        public override Unit Width { get => base.Width; set => base.Width = value; }

        [PDFAttribute("height")]
        public override Unit Height { get => base.Height; set => base.Height = value; }

        //Style Collection

        #region public StyleCollection Styles

        private StyleCollection _innerStyles;

        /// <summary>
        /// Gets the inner set of styles defined within the SVGCanvas.
        /// </summary>
        /// <remarks>This collection is normally populated from the SVGStyleElement contents during processing</remarks>
        public StyleCollection Styles
        {
            get
            {
                if (this._innerStyles == null)
                    this._innerStyles = new StyleCollection(this);
                return this._innerStyles;
            }
        }

        public bool HasInnerStyles
        {
            get { return null != this._innerStyles && this._innerStyles.Count > 0; }
        }

        #endregion

        /// <summary>
        /// Gets the flag to indicate if this SVG is contained in another canvas, or a root SVG canvas.
        /// Default is false, set by the LayoutEngineSVG to true its the parent engine is another SVG.
        /// </summary>
        public bool ContainedInParentSVG { get; internal set; }
        
        /// <summary>
        /// Gets or sets the flag to indicate if this SVG is discreet - i.e. referenced from an image url or similar
        /// </summary>
        /// <remarks>
        /// If this is a discree SVG then the Style building will block up to the parent. If not then styles will be built including other css references.
        /// </remarks>
        public bool IsDiscreetSVG { get; set; }

        IComponent INamingContainer.Owner
        {
            get{ return this.Parent; }
            set{ this.Parent = value as Component; }
        }
        
        public SVGCanvas()
        {
            this.Styles.Add(SVGBaseStyleSheet.Default);
        }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Position.PositionMode = PositionMode.Static;
            style.Position.DisplayMode = DisplayMode.InlineBlock;
            style.Overflow.Action = OverflowAction.Clip;
            style.Overflow.Split = OverflowSplit.Never;
            style.Size.Width = SVGCanvas.DefaultWidth;
            style.Size.Height = SVGCanvas.DefaultHeight;
            style.Position.XObject= true;
            
            return style;
        }

        public bool TryFindComponentByID(string id, out IComponent found)
        {
            if(null != this._definitions)
            {
                foreach (var item in this._definitions)
                {
                    if (item.ID == id)
                    {
                        found = item;
                        return true;
                    }
                }
            }
            foreach (var item in this.Contents)
            {
                if (item.ID == id)
                {
                    found = item;
                    return true;
                }
                else if (item is SVGCanvas)
                {
                    if ((item as SVGCanvas).TryFindComponentByID(id, out found))
                        return true;
                }
            }

            found = null;
            return false;
        }
        
        //
        // resources and artefacts
        //

        #region public PDFResourceList Resources {get;set;} + DoInitResources()

        /// <summary>
        /// private instance variable to hold the list of resources
        /// </summary>
        private PDFResourceList _resources;
        
        /// <summary>
        /// Gets the list of resources this page and its contents use 
        /// </summary>
        /// <remarks>Also implements the IPDFResourceContainer interface</remarks>
        [System.ComponentModel.Browsable(false)]
        public PDFResourceList Resources
        {
            get 
            {
                if (_resources == null)
                    _resources = this.DoInitResources();
                return _resources;
            }
            protected set { _resources = value; }
        }

        /// <summary>
        /// Virtual method that creates a new PDFResourceList for holding a pages resources.
        /// </summary>
        /// <returns>A new PDFResourceList</returns>
        protected virtual PDFResourceList DoInitResources()
        {
            PDFResourceList list = new PDFResourceList(this);
            return list;
        }

        #endregion

        IDocument IResourceContainer.Document
        {
            get
            {
                return this.Document;
            }
        }

        public string Register(ISharedResource rsrc)
        {
            return this.Register((PDFResource)rsrc).Value;
        }
        
        public PDFName Register(PDFResource reference)
        {
            if (null == reference.Name || string.IsNullOrEmpty(reference.Name.Value))
            {
                string name = this.Document.GetIncrementID(reference.Type);
                reference.Name = (PDFName)name;
            }
            reference.RegisterUse(this.Resources,this);
            return reference.Name;
        }

        public override Component FindAComponentById(string id)
        {
            IComponent found;
            if (this.TryFindComponentByID(id, out found))
                return found as Component;
            else
                return base.FindAComponentById(id);
        }

        protected override IPDFLayoutEngine CreateLayoutEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        {
            return new LayoutEngineSVG(this, parent);
            //return base.CreateLayoutEngine(parent, context, style);
        }

        public override Style GetAppliedStyle(Component forComponent, Style baseStyle)
        {
            if (this.HasInnerStyles)
            {
                this.Styles.MergeInto(baseStyle, forComponent);
            }

            if (this.IsDiscreetSVG)
                return baseStyle;
            else
                return base.GetAppliedStyle(forComponent, baseStyle);
        }

        public PDFObjectRef RenderToPDF(RenderContext context, PDFWriter writer)
        {
            if (this.IsDiscreetSVG)
            {
                
            }

            return null;
        }

        public static Style GetDefaultBaseStyle()
        {
            var style = new Style();
            Styles.FillStyle fill = new Styles.FillStyle();
            style.StyleItems.Add(fill);
            fill.Color = StandardColors.Black;


            PageStyle defpaper = new PageStyle();
            style.StyleItems.Add(defpaper);
            defpaper.PaperSize = PaperSize.A4;
            defpaper.PaperOrientation = PaperOrientation.Portrait;

            Styles.FontStyle fs = new Styles.FontStyle();
            style.StyleItems.Add(fs);
            fs.FontFamily = (FontSelector)ServiceProvider.GetService<IScryberConfigurationService>().FontOptions.DefaultFont;
            fs.FontSize = new Unit(24.0, PageUnits.Points);
            
            return style;
        }
    }
}
