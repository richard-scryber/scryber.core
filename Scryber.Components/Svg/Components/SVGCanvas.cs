using System;
using Scryber.Styles;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.Svg.Layout;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("svg")]
    public class SVGCanvas : Scryber.Components.Canvas
    {

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
        public SVGAspectRatio PreserveAspectRatio
        {
            get
            {
                if (this.HasStyle)
                {
                    return this.Style.GetValue(SVGAspectRatio.AspectRatioStyleKey, SVGAspectRatio.Default);
                }
                else
                    return SVGAspectRatio.Default;
            }
            set
            {
                this.Style.SetValue(SVGAspectRatio.AspectRatioStyleKey, value);
            }
        }

        public void RemoveAspectRatio()
        {
            this.Style.RemoveValue(SVGAspectRatio.AspectRatioStyleKey);
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

        public SVGCanvas()
        {
        }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Position.PositionMode = PositionMode.InlineBlock;
            style.Overflow.Action = OverflowAction.Clip;
            style.Overflow.Split = OverflowSplit.Never;
            style.Size.Width = 300;
            style.Size.Height = 150;
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
            return base.GetAppliedStyle(forComponent, baseStyle);
        }
    }
}
