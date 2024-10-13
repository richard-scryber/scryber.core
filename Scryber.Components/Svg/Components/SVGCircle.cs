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
        public Unit CentreX 
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.SVGGeometryCentreXKey, 0.0);
                else
                {
                    return 0.0;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGGeometryCentreXKey, value);
            }
        }

        [PDFAttribute("cy")]
        public Unit CenterY { get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.SVGGeometryCentreYKey, 0.0);
                else
                {
                    return 0.0;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGGeometryCentreYKey, value);
            } 
        }

        [PDFAttribute("r")]
        public Unit Radius
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.SVGGeometryRadiusKey, 0.0);
                else
                {
                    return 0.0;
                }
            }
            set
            {
                this.Style.SetValue(StyleKeys.SVGGeometryRadiusKey, value);
            } 
        }


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

        protected override void SetArrangement(ComponentArrangement arrange, PDFRenderContext context)
        {
            var path = this.Path;
            
            //override the default to use the path
            if(null != path)
            {
                var bounds = path.Bounds;
                
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
