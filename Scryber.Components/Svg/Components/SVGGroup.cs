using System;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.Styles;
using Scryber.PDF;
using Scryber.Svg.Layout;

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
   
        public SVGGroup() : base(ObjectTypes.Panel)
        {
        }

        public override SVGBase Clone()
        {
            SVGGroup clone = base.Clone() as SVGGroup;
            clone.Contents.Clear();

            for (int i = 0; i < this.Contents.Count; i++)
            {
                clone.Contents.Add(((SVGBase)this.Contents[i]).Clone());
            }
            return clone;
        }

        public override Style GetAppliedStyle(Component forComponent, Style baseStyle)
        {
            var style = base.GetAppliedStyle(forComponent, baseStyle);
            //style.Overflow.Action = Drawing.OverflowAction.Clip;
            style.Overflow.Action = Drawing.OverflowAction.None;
            //style.Overflow.Split = Drawing.OverflowSplit.Never;
            return style;
        }

        public IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDF.PDFLayoutContext context, Style fullstyle)
        {
            fullstyle.Overflow.Split = Drawing.OverflowSplit.Never;
            fullstyle.Overflow.Action = Drawing.OverflowAction.None; 
            //return new LayoutEnginePanel(this, parent);
            return new LayoutEngineSVG(this, parent);
        }
    }
}
