using System;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Components;
using Scryber.PDF;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("path")]
    public class SVGPath : SVGIrregularShape
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
            var rect = this.PathData.Bounds;
            rect = rect.Offset(this.ShapeOffset);
            return rect;
        }

        protected override Drawing.GraphicsPath CreatePath(Drawing.Size available, Style fullstyle)
        {
            if (!this.ShapeOffset.IsZero)
            {
                if (null == this.PathData.PathMatrix)
                    this.PathData.PathMatrix = new PDFTransformationMatrix();
                
                this.PathData.PathMatrix.SetTranslation(this.ShapeOffset.X, 0 - this.ShapeOffset.Y);
            }
            
            return this.PathData;
        }


        protected override void SetArrangement(ComponentArrangement arrange, PDFRenderContext context)
        {
            var path = this.PathData;
            
            //override the default to use the path
            if(null != path)
            {
                var bounds = path.Bounds;
                bounds = bounds.Offset(this.ShapeOffset);
                if (null != context.RenderMatrix)
                    bounds = context.RenderMatrix.TransformBounds(bounds);
                
                bounds.X += arrange.RenderBounds.X;
                bounds.Y += arrange.RenderBounds.Y;
                arrange.RenderBounds = bounds;
            }
            
            base.SetArrangement(arrange, context);
        }

        public override SVGBase Clone()
        {
            var path = (SVGPath)base.Clone();
            path.ShapeOffset = this.ShapeOffset;
            path.PathData = this.PathData.Clone();
            return path;
        }
    }
}
