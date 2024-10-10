using System;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Components;
using Scryber.PDF;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("path")]
    public class SVGPath : SVGShape
    {

        

        [PDFElement()]
        [PDFAttribute("d")]
        public GraphicsPath PathData
        {
            get { return this.Path; }
            set { this.Path = value; }
        }

        

        public SVGPath() : base(ObjectTypes.ShapePath)
        {
        }


        protected override Rect GetBounds()
        {
            return this.PathData.Bounds;
        }

        protected override Drawing.GraphicsPath CreatePath(Drawing.Size available, Style fullstyle)
        {
            return this.PathData;
        }


        protected override void SetArrangement(ComponentArrangement arrange, PDFRenderContext context)
        {
            var path = this.PathData;
            
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
