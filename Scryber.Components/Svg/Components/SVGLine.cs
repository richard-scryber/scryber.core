using System;
using System.Runtime.InteropServices.ComTypes;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("line")]
    public class SVGLine : SVGShape
    {
        [PDFAttribute("x1")]
        public Unit X1 { get; set; }

        [PDFAttribute("x2")]
        public Unit X2 { get; set; }

        [PDFAttribute("y1")]
        public Unit Y1 { get; set; }

        [PDFAttribute("y2")]
        public Unit Y2 { get; set; }

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
            var x1 = this.X1;
            var x2 = this.X2;
            var y1 = this.Y1;
            var y2 = this.Y2;

            var path = new GraphicsPath();
            path.MoveTo(new Point(x1,y1));
            path.LineTo(new Point(x2, y2));

            return path;
        }

        protected override void SetArrangement(ComponentArrangement arrange)
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
            
            base.SetArrangement(arrange);
        }
    }
}
