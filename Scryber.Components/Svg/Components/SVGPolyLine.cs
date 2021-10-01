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


        protected override void OnPreLayout(PDFLayoutContext context)
        {
            var bounds = this.GetBounds();
            this.X = bounds.X;
            this.Y = bounds.Y;
            this.Width = bounds.Width;
            this.Height = bounds.Height;
            base.OnPreLayout(context);
        }


        protected override PDFGraphicsPath CreatePath(PDFSize available, Style fullstyle)
        {
            var bounds = this.GetBounds();

            var xoffset = bounds.X.PointsValue;
            var yoffset = bounds.Y.PointsValue;

            PDFGraphicsPath path = new PDFGraphicsPath();

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


        protected override PDFRect GetBounds()
        {
            if (null == this.Points)
                return PDFRect.Empty;

            PDFUnit maxx = new PDFUnit(int.MinValue);
            PDFUnit maxy = new PDFUnit(int.MinValue);

            PDFUnit minx = new PDFUnit(int.MaxValue);
            PDFUnit miny = new PDFUnit(int.MaxValue);

            foreach (var pt in this.Points)
            {
                maxx = PDFUnit.Max(pt.X, maxx);
                maxy = PDFUnit.Max(pt.Y, maxy);

                minx = PDFUnit.Min(pt.X, minx);
                miny = PDFUnit.Min(pt.Y, miny);
            }

            return new PDFRect(minx, miny, maxx - minx, maxy - miny);

            
        }
    }


    [PDFParsableComponent("polygon")]
    public class SVGPolygon : SVGPolyLine
    {


        public SVGPolygon(): base()
        { }

        protected override PDFGraphicsPath CreatePath(PDFSize available, Style fullstyle)
        {
            PDFGraphicsPath path = base.CreatePath(available, fullstyle);
            path.ClosePath(true);

            return path;
        }
    }
}
