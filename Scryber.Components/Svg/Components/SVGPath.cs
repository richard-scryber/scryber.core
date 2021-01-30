using System;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Components;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("path")]
    public class SVGPath : SVGShape
    {

        

        [PDFElement()]
        [PDFAttribute("d")]
        public PDFGraphicsPath PathData
        {
            get { return this.Path; }
            set { this.Path = value; }
        }

        public SVGPath() : base(PDFObjectTypes.ShapePath)
        {
        }


        protected override PDFRect GetBounds()
        {
            return this.PathData.Bounds;
        }

        protected override Drawing.PDFGraphicsPath CreatePath(Drawing.PDFSize available, Style fullstyle)
        {
            return this.PathData;
        }

        public override Style GetAppliedStyle(Component forComponent, Style baseStyle)
        {
            return base.GetAppliedStyle(forComponent, baseStyle);
        }
    }
}
