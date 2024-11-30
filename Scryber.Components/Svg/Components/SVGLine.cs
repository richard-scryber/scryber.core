using System;
using System.Runtime.InteropServices.ComTypes;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.PDF;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("line")]
    public class SVGLine : SVGShape
    {
        [PDFAttribute("x1")]
        public Unit X1 
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.SVGGeometryXKey, 0.0);
                else
                {
                    return 0.0;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGGeometryXKey, value);
            }
        }

        [PDFAttribute("y1")]
        public Unit Y1 { 
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.SVGGeometryYKey, 0.0);
                else
                {
                    return 0.0;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGGeometryYKey, value);
            } 
        }

        [PDFAttribute("x2")]
        public Unit X2
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.SVGGeometryX2Key, 0.0);
                else
                {
                    return 0.0;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGGeometryX2Key, value);
            } 
        }
        
        [PDFAttribute("y2")]
        public Unit Y2
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.SVGGeometryY2Key, 0.0);
                else
                {
                    return 0.0;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGGeometryY2Key, value);
            } 
        }

        [PDFAttribute("marker-start")]
        public SVGMarkerValue MarkerStart
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.SVGMarkerStartKey, SVGMarkerValue.Empty);
                else
                {
                    return SVGMarkerValue.Empty;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGMarkerStartKey, value);
            } 
        }

        [PDFAttribute("marker-end")]
        public SVGMarkerValue MarkerEnd
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.SVGMarkerEndKey, SVGMarkerValue.Empty);
                else
                {
                    return SVGMarkerValue.Empty;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGMarkerEndKey, value);
            } 
        }

        
        public SVGLine() : base(ObjectTypes.ShapeLine)
        {
        }


        protected override Rect GetBounds()
        {
            var minx = Unit.Min(X1, X2);
            var maxx = Unit.Max(X1, X2);
            var miny = Unit.Min(Y1, Y2);
            var maxy = Unit.Max(Y1, Y2);

            return new Rect(minx, miny, maxx - minx, maxy - maxx);
        }

        protected override GraphicsPath CreatePath(Size available, Style fullstyle)
        {
            Unit x1, x2, y1, y2;
            StyleValue<Unit> found;
            if (fullstyle.TryGetValue(StyleKeys.SVGGeometryXKey, out found))
                x1 = found.Value(fullstyle);
            else
                return null;
            
            if (fullstyle.TryGetValue(StyleKeys.SVGGeometryX2Key, out found))
                x2 = found.Value(fullstyle);
            else
                return null;
            
            if (fullstyle.TryGetValue(StyleKeys.SVGGeometryYKey, out found))
                y1 = found.Value(fullstyle);
            else
                return null;
            
            if (fullstyle.TryGetValue(StyleKeys.SVGGeometryY2Key, out found))
                y2 = found.Value(fullstyle);
            else
                return null;
            
            
            var path = new GraphicsPath();
            path.MoveTo(new Point(x1,y1));
            path.LineTo(new Point(x2, y2));

            if (fullstyle.TryGetValue(StyleKeys.SVGMarkerStartKey, out var start))
            {
                var adorner = start.Value(fullstyle);
                if (!string.IsNullOrEmpty(adorner.MarkerReference) && this.TryFindMarker(adorner.MarkerReference, out var marker))
                {
                    path.AddAdornment(marker, AdornmentOrder.After, AdornmentPlacements.Start);
                }
            }

            if (fullstyle.TryGetValue(StyleKeys.SVGMarkerEndKey, out var end))
            {
                var adorner = end.Value(fullstyle);
                if (!string.IsNullOrEmpty(adorner.MarkerReference) && this.TryFindMarker(adorner.MarkerReference, out var marker))
                {
                    path.AddAdornment(marker, AdornmentOrder.After, AdornmentPlacements.End);
                }
            }

            return path;
        }

        protected bool TryFindMarker(string nameOrId, out SVGMarker marker)
        {
            return SVGBase.TryFindMarkerInParent(this.Parent,  nameOrId, out marker);
        }

        protected override void SetArrangement(ComponentArrangement arrange, PDFRenderContext context)
        {
            var path = this.Path;
            
            //override the default to use the path
            if(null != path)
            {
                var bounds = path.Bounds;
                bounds.X += arrange.RenderBounds.X;
                bounds.Y += arrange.RenderBounds.Y;

                if (bounds.Width < 1.0)
                {
                    StyleValue<Unit> strokeWidth;
                    if (arrange.FullStyle.TryGetValue(StyleKeys.StrokeWidthKey, out strokeWidth))
                        bounds.Width = strokeWidth.Value(arrange.FullStyle);
                    else
                        bounds.Width = 1; //vertical line - so give it a nominal width
                }
                else if (bounds.Height < 1.0)
                {  
                    StyleValue<Unit> strokeWidth;
                    if (arrange.FullStyle.TryGetValue(StyleKeys.StrokeWidthKey, out strokeWidth))
                        bounds.Height = strokeWidth.Value(arrange.FullStyle);
                    else
                        bounds.Height = 1; //horizontal line - so give it a nominal width
                    
                }
                
                arrange.RenderBounds = bounds;
            }
            
            base.SetArrangement(arrange, context);
        }
    }
}
