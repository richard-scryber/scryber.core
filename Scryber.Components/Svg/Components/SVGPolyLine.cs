using System;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.PDF;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("polyline")]
    public class SVGPolyLine : SVGShape
    {
        [PDFAttribute("points")]
        public PDFPointList Points { get; set; }

        public SVGPolyLine()
            : base(ObjectTypes.ShapePolygon)
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

            var xoffset = bounds.X.PointsValue;
            var yoffset = bounds.Y.PointsValue;

            GraphicsPath path = new GraphicsPath();

            if (null != this.Points)
            {
                for (int i = 0; i < this.Points.Count; i++)
                {
                    var pt = this.Points[i];
                    pt = pt.Offset(-xoffset, -yoffset);

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
    }


    [PDFParsableComponent("polygon")]
    public class SVGPolygon : SVGPolyLine
    {


        public SVGPolygon(): base()
        { }

        protected override GraphicsPath CreatePath(Size available, Style fullstyle)
        {
            GraphicsPath path = base.CreatePath(available, fullstyle);
            path.ClosePath(true);

            return path;
        }
    }
}
