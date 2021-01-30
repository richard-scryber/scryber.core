using System;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Components;
using Scryber.Html.Components;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("a")]
    public class SVGAnchor : HTMLAnchor, ICloneable
    {

        public SVGAnchor()
        {
        }


        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Fill.RemoveColor();
            style.Text.RemoveDecoration();
            return style;
        }

        public SVGAnchor Clone()
        {
            var a = this.MemberwiseClone() as SVGAnchor;
            a.Parent = null;
            if(this.Style.HasValues)
            {
                a.Style = new Style();
                this.Style.MergeInto(a.Style);
            }
            return a;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}
