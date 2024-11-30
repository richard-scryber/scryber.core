using System;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.PDF;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("polyline")]
    public class SVGPolyLine : SVGIrregularShape
    {
        [PDFAttribute("points")]
        public PDFPointList Points { get; set; }

        public SVGPolyLine()
            : this(ObjectTypes.ShapePolyline)
        {
        }

        protected SVGPolyLine(ObjectType type) : base(type)
        {
            
        }

        protected override void OnPreLayout(LayoutContext context)
        {
            var bounds = this.GetBounds();
            this.X = bounds.X;
            this.Y = bounds.Y;
            this.Width = bounds.Width;
            this.Height = bounds.Height;
            base.OnPreLayout(context);
        }


        protected override GraphicsPath CreatePath(Size available, Style fullstyle)
        {
            var bounds = this.GetBounds();

            var xoffset = this.ShapeOffset.X;
            var yoffset = this.ShapeOffset.Y;
            

            GraphicsPath path = new GraphicsPath();

            if (null != this.Points)
            {
                for (int i = 0; i < this.Points.Count; i++)
                {
                    var pt = this.Points[i];
                    
                    pt = pt.Offset(xoffset, yoffset);

                    if (i == 0)
                        path.MoveTo(pt);
                    else
                        path.LineTo(pt);
                }
            }
            return path;
        }


        protected override Rect GetBounds()
        {
            if (null == this.Points)
                return Rect.Empty;

            Unit maxx = new Unit(int.MinValue);
            Unit maxy = new Unit(int.MinValue);

            Unit minx = new Unit(int.MaxValue);
            Unit miny = new Unit(int.MaxValue);

            foreach (var pt in this.Points)
            {
                maxx = Unit.Max(pt.X, maxx);
                maxy = Unit.Max(pt.Y, maxy);

                minx = Unit.Min(pt.X, minx);
                miny = Unit.Min(pt.Y, miny);
            }

            return new Rect(minx, miny, maxx - minx, maxy - miny);
            
        }

        protected override void SetArrangement(ComponentArrangement arrange, PDFRenderContext context)
        {
            var path = this.Path;
            
            //override the default to use the path
            if(null != path)
            {
                var bounds = path.Bounds;

                if (null != this.DrawingTransformMatrix)
                {
                    bounds = this.DrawingTransformBounds;
                    var tl = new Point(bounds.X, bounds.Y);
                    var tr = new Point(bounds.X + bounds.Width, bounds.Y);
                    var bl = new Point(bounds.X, bounds.Y + bounds.Height);
                    var br = new Point(bounds.X + bounds.Width, bounds.Y + bounds.Height);

                    bounds = Rect.Bounds(tl, tr, bl, br);
                }

                if (null != context.RenderMatrix)
                    bounds = context.RenderMatrix.TransformBounds(bounds);
                
                bounds.X += arrange.RenderBounds.X;
                bounds.Y += arrange.RenderBounds.Y;
                arrange.RenderBounds = bounds;
            }
            
            base.SetArrangement(arrange, context);
        }
    }


    
}
