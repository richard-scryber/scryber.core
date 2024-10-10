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
        public Unit CentreX { get; set; }

        [PDFAttribute("cy")]
        public Unit CenterY { get; set; }

        [PDFAttribute("rx")]
        public Unit RadiusX { get; set; }

        [PDFAttribute("ry")]
        public Unit RadiusY { get; set; }


        public SVGEllipse()
            : base(ObjectTypes.ShapeElipse)
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

            Rect rect = this.GetRectBounds();
            GraphicsPath path = new GraphicsPath();

            Ellipse.BuildElipse(path, rect, true, 0);

            return path;
        }

        protected virtual Rect GetRectBounds()
        {
            var rect = new Rect();
            rect.X = this.CentreX - this.RadiusX;
            rect.Width = this.RadiusX * 2;
            rect.Y = this.CenterY - this.RadiusY;
            rect.Height = this.RadiusY * 2;

            return rect;
        }


        protected override void SetArrangement(ComponentArrangement arrange, PDFRenderContext context)
        {
            var path = this.Path;

            if (null != path)
            {
                var bounds = path.Bounds;
                bounds.X += arrange.RenderBounds.X;
                bounds.Y += arrange.RenderBounds.Y;
                arrange.RenderBounds = bounds;
            }
            
            base.SetArrangement(arrange, context);
        }
    }
}
