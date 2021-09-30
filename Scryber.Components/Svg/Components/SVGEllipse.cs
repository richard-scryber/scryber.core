using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.PDF;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("ellipse")]
    public class SVGEllipse : SVGShape
    {

        [PDFAttribute("cx")]
        public PDFUnit CentreX { get; set; }

        [PDFAttribute("cy")]
        public PDFUnit CenterY { get; set; }

        [PDFAttribute("rx")]
        public PDFUnit RadiusX { get; set; }

        [PDFAttribute("ry")]
        public PDFUnit RadiusY { get; set; }


        public SVGEllipse()
            : base(PDFObjectTypes.ShapeElipse)
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

            PDFRect rect = this.GetRectBounds();
            PDFGraphicsPath path = new PDFGraphicsPath();

            Ellipse.BuildElipse(path, rect, true, 0);

            return path;
        }

        protected virtual PDFRect GetRectBounds()
        {
            var rect = new PDFRect();
            rect.X = this.CentreX - this.RadiusX;
            rect.Width = this.RadiusX * 2;
            rect.Y = this.CenterY - this.RadiusY;
            rect.Height = this.RadiusY * 2;

            return rect;
        }

        
    }
}
