using System;
using System.ComponentModel;
using Scryber.PDF.Layout;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Svg.Layout
{
    public class TSpanLayoutEngine : LayoutEnginePanel
    {
        public TSpanLayoutEngine(ContainerComponent container, IPDFLayoutEngine parent)
            : base(container, parent)
        {
        }


        protected override void DoLayoutAChild(IComponent comp, Style full)
        {
            base.DoLayoutAChild(comp, full);
        }

        protected virtual void AdjustContainerForTextBaseline(PDFPositionOptions pos, IComponent comp, Style full)
        {
            var text = full.CreateTextOptions();

            if (text.DrawTextFromTop == false)
            {
                PDFUnit y;
                var font = full.CreateFont();
                if (pos.Y.HasValue)
                    y = pos.Y.Value;
                else
                    y = 0;

                var doc = this.Component.Document;
                var frsrc = doc.GetFontResource(font, true);
                var metrics = frsrc.Definition.GetFontMetrics(font.Size);

                //TODO: Register the font so that we can get the metrics. Or call later on and move
                // But for now it works (sort of).

                if (null != metrics)
                    y -= metrics.Ascent;
                else
                    y -= font.Size * 0.8;

                pos.Y = y;

                full.Position.Y = y;


                if (full is StyleFull)
                    (full as StyleFull).ClearFullRefs();
            }
        }

        protected override PDFLayoutRegion BeginNewRelativeRegionForChild(PDFPositionOptions pos, IComponent comp, Style full)
        {
            this.AdjustContainerForTextBaseline(pos, comp, full);
            return base.BeginNewRelativeRegionForChild(pos, comp, full);
        }
    }

}
