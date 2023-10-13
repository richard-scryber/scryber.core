﻿using System;
using Scryber.Styles;
using Scryber.Components;
using Scryber.Drawing;

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


        public SVGCanvas()
        {
        }

        protected override Style GetBaseStyle()
        {
            return base.GetBaseStyle();
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

        protected override void DoInitChildren(InitContext context)
        {
            base.DoInitChildren(context);

            if(null != this._definitions)
            {
                foreach (var item in this._definitions)
                {
                    item.Init(context);
                }
            }
        }

        protected override void DoLoadChildren(LoadContext context)
        {
            base.DoLoadChildren(context);

            if( null != this._definitions)
            {
                foreach (var item in this._definitions)
                {
                    item.Load(context);
                }
            }
        }

        protected override void DoDataBindChildren(DataContext context)
        {
            base.DoDataBindChildren(context);

            if (null != this._definitions)
            {
                foreach (var item in this._definitions)
                {
                    item.DataBind(context);
                }
            }
        }
    }
}
