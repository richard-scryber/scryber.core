using System;
using System.ComponentModel;
using Scryber.PDF.Layout;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.PDF;

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
            var textAnchor = full.GetValue(StyleKeys.TextAnchorKey, TextAnchor.Start);

            if (textAnchor == TextAnchor.Middle || textAnchor == TextAnchor.End)
            {
                var block = this.Context.DocumentLayout.CurrentPage.LastOpenBlock();
                var reg = block.CurrentRegion;
                var max = Unit.Zero;

                foreach (PDFLayoutLine line in reg.Contents)
                {
                    if (line.Runs.Count > 0)
                    {
                        var w = line.Width;
                        max = Unit.Max(max, w);
                    }
                }

                if (textAnchor == TextAnchor.Middle)
                    max = max / 2;

                block.Position.X = (block.Position.X ?? Unit.Zero) - max;
                var orig = block.TotalBounds;
                var update = new Rect(orig.X - max, orig.Y, orig.Width, orig.Height);
                block.TotalBounds = update;

            }
            
        }

        protected virtual void AdjustContainerForTextBaseline(PDFPositionOptions pos, IComponent comp, Style full)
        {
            var text = full.CreateTextOptions();

            if (text.DrawTextFromTop == false)
            {
                Unit y;
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
