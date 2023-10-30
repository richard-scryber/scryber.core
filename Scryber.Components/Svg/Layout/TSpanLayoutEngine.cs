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
            var textAnchor = full.GetValue(StyleKeys.TextAnchorKey, TextAnchor.Start);
            var domBase = full.GetValue(StyleKeys.DominantBaselineKey, DominantBaseline.Auto);
            StyleValue<Unit> ypos;
            full.TryGetValue(StyleKeys.PositionYKey, out ypos);

            base.DoLayoutAChild(comp, full);
            
           

            
            var block = this.Context.DocumentLayout.CurrentPage.LastOpenBlock();
            var reg = block.CurrentRegion;

            if (textAnchor == TextAnchor.Middle || textAnchor == TextAnchor.End)
            {
                var maxW = Unit.Zero;

                foreach (PDFLayoutLine line in reg.Contents)
                {
                    if (line.Runs.Count > 0)
                    {
                        var w = line.Width;
                        maxW = Unit.Max(maxW, w);
                    }
                }

                if (textAnchor == TextAnchor.Middle)
                    maxW = maxW / 2;

                block.Position.X = (block.Position.X ?? Unit.Zero) - maxW;
                var orig = block.TotalBounds;
                var update = new Rect(orig.X - maxW, orig.Y, orig.Width, orig.Height);
                block.TotalBounds = update;

            }


            PDFTextRenderOptions txt = null;
            //Ensure we get the first TextRunBegin from the layout region on the first line.
            if (reg.Contents.Count > 0 && reg.Contents[0] is PDFLayoutLine first && first.Runs.Count > 0 && first.Runs[0] is PDFTextRunBegin begin)
            {
                txt = begin.TextRenderOptions;
                var met = txt.Font.FontMetrics;

                //In SVG the x,y position is based on the baseline of the text
                //So we need to adjust for this (rather than the x,y being for the bounding box

                Unit defaultShift; 

                if (block.Position.TransformMatrix != null && block.Position.TransformMatrix.IsIdentity == false){
                    //case for the transform only where we should consider the full block height that is transformed.
                    defaultShift = met.Descent;
                }
                else
                {
                    //otherwise we use the the space between the descender and the bottom of the bounding box
                    defaultShift = -(met.TotalLineHeight - (met.BaseLineOffset + met.Descent));
                }


                Unit offset = Unit.Zero;

                switch (domBase)
                {
                    case DominantBaseline.Central:
                        //central to the M height
                        offset = met.Ascent / 2.0;
                        break;
                    case DominantBaseline.Middle:
                        //middle to the x height
                        offset = met.ExHeight / 2.0;
                        break;
                    case DominantBaseline.Hanging:
                        //top of the bounding box
                        offset = met.Ascent;
                        break;
                    case DominantBaseline.Mathematical:
                        //top of tghe x height
                        offset = met.ExHeight;
                        break;
                    case DominantBaseline.Text_After_Edge:
                    case DominantBaseline.Ideographic:
                        //include the descender
                        offset = -met.Descent;
                        break;
                    case DominantBaseline.Alphabetic:
                        //same as auto or text top - normal position of the default shift
                        break;
                        
                    case DominantBaseline.Text_Before_Edge:
                        //full ascender height 
                        offset = met.EmHeight;
                        break;

                    case DominantBaseline.Text_Top:
                    case DominantBaseline.Auto:
                    default:
                        //normal position of the default shift
                        break;
                }

                block.Position.Y = (block.Position.Y ?? Unit.Zero) + offset;
                var orig = block.TotalBounds;
                var update = new Rect(orig.X, orig.Y + offset + defaultShift, orig.Width, orig.Height);
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
