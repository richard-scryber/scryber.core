using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.PDF;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("circle")]
    public class SVGCircle : SVGShape
    {

        [PDFAttribute("cx")]
        public Unit CentreX { get; set; }

        [PDFAttribute("cy")]
        public Unit CenterY { get; set; }

        [PDFAttribute("r")]
        public Unit Radius { get; set; }


        public SVGCircle()
            : base(ObjectTypes.ShapeCircle)
        {

        }




        protected override void OnPreLayout(LayoutContext context)
        {
            Rect rect = GetRectBounds();

            this.X = rect.X;
            this.Y = rect.Y;
            this.Width = rect.Width;
            this.Height = rect.Height;

            base.OnPreLayout(context);
        }

        protected override GraphicsPath CreatePath(Size available, Style fullstyle)
        {
            var bounds = this.GetRectBounds();
            var path = new GraphicsPath();
            Ellipse.BuildElipse(path, bounds, true, 0);

            return path;
        }

        private Rect GetRectBounds()
        {
            var rect = new Rect();
            rect.X = this.CentreX - this.Radius;
            rect.Width = this.Radius * 2;
            rect.Y = this.CenterY - this.Radius;
            rect.Height = this.Radius * 2;
            return rect;
        }
    }
}
