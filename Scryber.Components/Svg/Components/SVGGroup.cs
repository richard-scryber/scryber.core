using System;
using Scryber.Components;
using Scryber.Layout;
using Scryber.Styles;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("g")]
    public class SVGGroup : SVGBase, IPDFViewPortComponent
    {
        [PDFElement()]
        [PDFArray(typeof(Component))]
        public ComponentList Contents
        {
            get { return this.InnerContent; }
        }
   
        public SVGGroup() : base(PDFObjectTypes.Panel)
        {
        }

        public override SVGBase Clone()
        {
            SVGGroup clone = base.Clone() as SVGGroup;
            //TODO: Clone the inner contents
            return clone;
        }

        public override Style GetAppliedStyle(Component forComponent, Style baseStyle)
        {
            var style = base.GetAppliedStyle(forComponent, baseStyle);
            style.Overflow.Action = Drawing.OverflowAction.None;
            return style;
        }

        public IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style fullstyle)
        {
            return new LayoutEngineCanvas(this, parent);
        }
    }
}
