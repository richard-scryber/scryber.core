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
            //We use top left zero based moving and line
            var bounds = this.GetBounds();
            var x1 = this.X1 - bounds.X;
            var x2 = this.X2 - bounds.X;
            var y1 = this.Y1 - bounds.Y;
            var y2 = this.Y2 - bounds.Y;

            var path = new GraphicsPath();
            path.MoveTo(new Point(x1,y1));
            path.LineTo(new Point(x2, y2));

            return path;
        }
    }
}
