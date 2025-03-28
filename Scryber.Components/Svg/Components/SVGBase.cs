using System;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.Styles;
using FontStyle = Scryber.Drawing.FontStyle;

namespace Scryber.Svg.Components
{
    public abstract class SVGBase : VisualComponent, ICloneable
    {
        [PDFAttribute("class")]
        public override string StyleClass
        {
            get => base.StyleClass;
            set => base.StyleClass = value;
        }

        [PDFAttribute("style")]
        public override Style Style
        {
            get => base.Style;
            set => base.Style = value;
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
        public string StrokeLineCap
        {
            get
            {
                if (this.Style.IsValueDefined(StyleKeys.StrokeEndingKey))
                    return this.Style.Stroke.LineCap.ToString().ToLower();
                else
                    return LineCaps.Square.ToString().ToLower();
            }
            set
            {
                LineCaps cap;
                if (Enum.TryParse<LineCaps>(value, true, out cap))
                    this.Style.Stroke.LineCap = cap;
                else
                    this.Style.RemoveValue(StyleKeys.StrokeEndingKey);
            }
        }

        [PDFAttribute("stroke-linejoin")]
        public string StrokeLineJoin
        {
            get
            {
                if (this.Style.IsValueDefined(StyleKeys.StrokeJoinKey))
                    return this.Style.Stroke.LineJoin.ToString().ToLower();
                else
                    return LineJoin.Bevel.ToString().ToLower();
            }
            set
            {
                LineJoin join;
                if (Enum.TryParse<LineJoin>(value, true, out join))
                    this.Style.Stroke.LineJoin = join;
                else
                    this.Style.RemoveValue(StyleKeys.StrokeJoinKey);
            }
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
        public SVGFillValue Fill
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.SVGFillKey, null);
                else
                {
                    return null;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGFillKey, value);
            }
        }

        [PDFAttribute("font-family")]
        public override FontSelector FontFamily
        {
            get => base.FontFamily;
            set => base.FontFamily = value;
        }

        [PDFAttribute("font-size")]
        public override Unit FontSize
        {
            get => base.FontSize;
            set => base.FontSize = value;
        }

        [PDFAttribute("font-weight")]
        public override int FontWeight
        {
            get => base.FontWeight;
            set => base.FontWeight = value;
        }

        [PDFAttribute("font-style")]
        public override FontStyle FontStyle
        {
            get => base.FontStyle;
            set => base.FontStyle = value;
        }

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

        [PDFAttribute("transform-origin")]
        public TransformOrigin TransformOrigin
        {
            get
            {
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.TransformOriginKey, out var value))
                    return value.Value(this.Style);
                else
                    return null;
            }
            set
            {
                this.Style.SetValue(StyleKeys.TransformOriginKey, value);
            }
        }
        


        public SVGBase(ObjectType type)
            : base(type)
        {
        }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.SetValue(StyleKeys.SVGGeometryInUseKey, true);
            return style;
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
                var referenceValue = value.GetValue(forStyle) as SVGFillReferenceValue;
                if (null != referenceValue)
                {
                    var svg = this.GetRootSVGCanvas();
                    SVGFillBase fill;
                    if (referenceValue.Value.StartsWith("#"))
                    {
                        fill = svg.FindAComponentById(referenceValue.Value.Substring(1)) as SVGFillBase;
                    }
                    else
                    {
                        fill = svg.FindAComponentByName(referenceValue.Value) as SVGFillBase;
                    }

                    if (null != fill)
                    {
                        var bounds = new Rect(Unit.Zero, Unit.Zero, svg.Width, svg.Height);
                        referenceValue.Adapter = fill.CreateBrush(bounds);
                    }
                }
            }
        }
        
        private SVGCanvas GetRootSVGCanvas()
        {
            SVGCanvas root = null;
            var parent = this.Parent;
            while (null != parent)
            {
                if (parent is SVGCanvas canvas)
                {
                    root = canvas;
                    if (root.IsDiscreetSVG)
                        return root;
                }
                parent = parent.Parent;
            }
            //should always be inside an SVG
            return root;
        }


        public virtual SVGBase Clone()
        {
            var c = this.MemberwiseClone() as SVGBase;
            c.Parent = null;
            if(this.Style.HasValues)
            {
                c.Style = new Style();
                this.Style.MergeInto(c.Style);
            }
            
            return c;
        }

        protected override void SetArrangement(ComponentArrangement arrange, PDFRenderContext context)
        {
            base.SetArrangement(arrange, context);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
        
        public static bool TryFindMarkerInParent(Component parent,  string nameOrId, out SVGMarker found)
        {
            var canvas = parent;
            while (!(canvas is SVGCanvas))
            {
                canvas = canvas.Parent;
                if (null == canvas)
                    throw new InvalidOperationException("Cannot find markers outside of the components heirarchy");

            }
            
            if (nameOrId.StartsWith("#"))
            {
                if (canvas.FindAComponentById(nameOrId.Substring(1)) is SVGMarker marker)
                {
                    found = marker;
                    return true;
                }
            }
            else if (Uri.IsWellFormedUriString(nameOrId, UriKind.Absolute))
            {
                throw new ArgumentException("Loading of markers from a uri is not (currently) supported");
            }
            else
            {
                if (canvas.FindAComponentByName(nameOrId) is SVGMarker marker)
                {
                    found = marker;
                    return true;
                }
            }

            found = null;
            return false;
        }

        
    }
}
