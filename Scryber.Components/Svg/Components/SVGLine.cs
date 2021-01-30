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
        public PDFUnit X1 { get; set; }

        [PDFAttribute("x2")]
        public PDFUnit X2 { get; set; }

        [PDFAttribute("y1")]
        public PDFUnit Y1 { get; set; }

        [PDFAttribute("y2")]
        public PDFUnit Y2 { get; set; }

        public SVGLine() : base(PDFObjectTypes.ShapeLine)
        {
        }


        protected override PDFRect GetBounds()
        {
            var minx = PDFUnit.Min(X1, X2);
            var maxx = PDFUnit.Max(X1, X2);
            var miny = PDFUnit.Min(Y1, Y2);
            var maxy = PDFUnit.Max(Y1, Y2);

            return new PDFRect(minx, miny, maxx - minx, maxy - maxx);
        }

        protected override PDFGraphicsPath CreatePath(PDFSize available, Style fullstyle)
        {
            //We use top left zero based moving and line
            var bounds = this.GetBounds();
            var x1 = this.X1 - bounds.X;
            var x2 = this.X2 - bounds.X;
            var y1 = this.Y1 - bounds.Y;
            var y2 = this.Y2 - bounds.Y;

            var path = new PDFGraphicsPath();
            path.MoveTo(new PDFPoint(x1,y1));
            path.LineTo(new PDFPoint(x2, y2));

            return path;
        }
    }
}
