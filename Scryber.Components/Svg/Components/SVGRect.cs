using System;
using System.Diagnostics;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.PDF;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("rect")]
    public class SVGRect : Scryber.Components.Rectangle, ICloneable
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

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        //
        //style attributes
        //

        // position

        [PDFAttribute("width")]
        public override Unit Width { get { return base.Width; } set { base.Width = value; } }

        [PDFAttribute("height")]
        public override Unit Height { get { return base.Height; } set { base.Height = value; } }

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
        public override Color StrokeColor
        {
            get => base.StrokeColor; 
            set => base.StrokeColor = value;
        }

        [PDFAttribute("stroke-width")]
        public override Unit StrokeWidth { get => base.StrokeWidth; set => base.StrokeWidth = value; }

        
        [PDFAttribute("stroke-linecap")]
        public LineCaps StrokeLineCap
        {
            get { return this.Style.Stroke.LineCap; }
            set { this.Style.Stroke.LineCap = value; }
        }

        [PDFAttribute("stroke-dasharray")]
        public override Dash StrokeDashPattern { get => base.StrokeDashPattern; set => base.StrokeDashPattern = value; }



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
        public override double FillOpacity { get => base.FillOpacity; set => base.FillOpacity = value; }

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
        public Scryber.Drawing.TransformOperationSet TransformMatrix
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
        //
        // .ctor
        //

        public SVGRect()
        {
        }


        protected override Point[] GetPoints(Rect bounds, Style style)
        {
            var x = style.GetValue(StyleKeys.SVGGeometryXKey, Unit.Zero);
            var y = style.GetValue(StyleKeys.SVGGeometryYKey, Unit.Zero);
            var w = style.GetValue(StyleKeys.SizeWidthKey, bounds.Width);
            var h = style.GetValue(StyleKeys.SizeHeightKey, bounds.Height);

            Point[] all = new Point[4];
            all[0] = new Point(x, y);
            all[1] = new Point(x + w, y);
            all[2] = new Point(x + w, y + h);
            all[3] = new Point(x, y + h);
            return all;
        }


        protected override void BuildPath(GraphicsPath path, Point[] points, Style style, bool end)
        {
            if (this.Style.IsValueDefined(StyleKeys.SVGGeometryRadiusXKey))
                this.BuildRoundRectPath(path, points, style, end);
            else if (this.Style.IsValueDefined(StyleKeys.SVGGeometryRadiusYKey))
                this.BuildRoundRectPath(path, points, style, end);
            else
                base.BuildPath(path, points, style, end);
        }

        private void BuildRoundRectPath(GraphicsPath path, Point[] points, Unit xRadius, Unit yRadius, Style style,
            bool end)
        {
            if(path.HasCurrentPath == false)
                path.BeginPath();

            Unit xHandleOffset = xRadius * PDF.Graphics.PDFGraphics.CircularityFactor;
            Unit yHandleOffset = yRadius * PDF.Graphics.PDFGraphics.CircularityFactor;

            if (points.Length != 4)
                throw new ArgumentOutOfRangeException(nameof(points),"The number of points for a rect must always be 4");

            //top left, top right, bottom right, bottom left
            var tl = points[0];
            var tr = points[1];
            var br = points[2];
            var bl = points[3];
            path.MoveTo(new Point(tl.X + xRadius, tl.Y));
            
            path.LineTo(new Point(tr.X - xRadius, tr.Y));
            
            var hStart = new Point((tr.X - xRadius) + xHandleOffset, tr.Y);
            var hEnd = new Point(tr.X, (tr.Y + yRadius) - yHandleOffset);
            path.CubicCurveTo(new Point(tr.X, tr.Y + yRadius), hStart, hEnd );
            
            path.LineTo(new Point(br.X, br.Y - yRadius));

            hStart = new Point(br.X, (br.Y - yRadius) + yHandleOffset);
            hEnd = new Point((br.X - xRadius) + xHandleOffset, br.Y);
            path.CubicCurveTo(new Point(br.X - xRadius, br.Y), hStart, hEnd);
            
            path.LineTo(new Point(bl.X + xRadius, bl.Y));

            hStart = new Point((bl.X + xRadius) - xHandleOffset, bl.Y);
            hEnd = new Point(bl.X, (bl.Y - yRadius) + yHandleOffset);
            path.CubicCurveTo(new Point(bl.X, bl.Y - yRadius), hStart, hEnd);
            
            path.LineTo(new Point(tl.X, tl.Y + yRadius));

            hStart = new Point(tl.X, (tl.Y + yRadius) - yHandleOffset);
            hEnd = new Point((tl.X + xRadius) - xHandleOffset, tl.Y);
            path.CubicCurveTo(new Point(tl.X + xRadius, tl.Y), hStart, hEnd);
            
            if(end)
                path.EndPath();
        }

        private void BuildRoundRectPath(GraphicsPath path, Point[] points, Style style, bool end)
        {
            Unit xRadius = 0;
            Unit yRadius = 0;
            
            StyleValue<Unit> found;
            
            if (style.TryGetValue(StyleKeys.SVGGeometryRadiusXKey, out found))
                xRadius = found.Value(style);

            if (style.TryGetValue(StyleKeys.SVGGeometryRadiusYKey, out found))
                yRadius = found.Value(style);
            else
                yRadius = xRadius;

            if (xRadius.IsZero && yRadius.IsZero)
            {
                //values are zero, so just build a rect
                BuildPath(path, points, style, end);
            }
            else
            {
                BuildRoundRectPath(path, points, xRadius, yRadius, style, end);
            }
        }

        protected override void SetArrangement(ComponentArrangement arrange , PDFRenderContext context)
        {
            var path = this.Path;
            
            //override the default to use the path
            if(null != path)
            {
                var bounds = path.Bounds;
                
                if (null != context.RenderMatrix)
                    bounds = context.RenderMatrix.TransformBounds(bounds);
                
                bounds.X += arrange.RenderBounds.X;
                bounds.Y += arrange.RenderBounds.Y;

                
                arrange.RenderBounds = bounds;
            }
            

            base.SetArrangement(arrange, context);
        }

        private Style _applied;
        
        public override Style GetAppliedStyle()
        {
            if (null == _applied)
            {
                var style = base.GetAppliedStyle();
                this.ResolveStyleReferences(style);
                _applied = style;
            }

            return _applied;
        }

        protected void ResolveStyleReferences(Style forStyle)
        {
            StyleValue<SVGFillValue> value;
            if (forStyle.TryGetValue(StyleKeys.SVGFillKey, out value))
            {
                var gradient = value.GetValue(forStyle) as SVGFillReferenceValue;
                if (null != gradient)
                {
                    var svg = this.GetRootSVGCanvas();
                    SVGFillBase fill;
                    if (gradient.Value.StartsWith("#"))
                    {
                        fill = svg.FindAComponentById(gradient.Value.Substring(1)) as SVGFillBase;
                    }
                    else
                    {
                        fill = svg.FindAComponentByName(gradient.Value) as SVGFillBase;
                    }

                    if (null != fill)
                    {
                        var bounds = new Rect(Unit.Zero, Unit.Zero, svg.Width, svg.Height);
                        gradient.Adapter = fill.CreateBrush(bounds);
                    }
                }
            }
        }
        
        

        private SVGCanvas GetRootSVGCanvas()
        {
            var parent = this.Parent;
            while (null != parent)
            {
                if (parent is SVGCanvas canvas)
                    return canvas; //TODO: need a reference to the root, not just the parent.
                else parent = parent.Parent;
            }
            //should always be inside an SVG
            return null;
        }

        public SVGRect Clone()
        {
            var clone = (SVGRect)this.MemberwiseClone();
            clone.Style = new Style();
            
            if(this.HasStyle)
               this.Style.MergeInto(clone.Style);

            return clone;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        // protected override Style GetBaseStyle()
        // {
        //     var style = base.GetBaseStyle();
        //     style.SetValue(StyleKeys.SVGGeometryInUseKey, true);
        //     return style;
        // }
    }
}
