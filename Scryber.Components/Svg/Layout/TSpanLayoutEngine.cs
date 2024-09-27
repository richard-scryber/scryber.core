using System;
using System.ComponentModel;
using Scryber.PDF.Layout;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.PDF;
using Scryber.Svg.Components;

namespace Scryber.Svg.Layout
{
    public class TSpanLayoutEngine : LayoutEnginePanel
    {
        public SVGText TextContainer { get; set; }
        
        public TSpanLayoutEngine(ContainerComponent container, IPDFLayoutEngine parent)
            : base(container, parent)
        {
            this.TextContainer = (SVGText)container;
            
        }

        protected override void DoLayoutChildren()
        {
            var full = this.FullStyle;
            
            var textAnchor = full.GetValue(StyleKeys.TextAnchorKey, TextAnchor.Start);
            var domBase = full.GetValue(StyleKeys.DominantBaselineKey, DominantBaseline.Auto);

            Unit yVal = full.GetValue(StyleKeys.SVGGeometryYKey, Unit.Zero);
            Unit xVal = full.GetValue(StyleKeys.SVGGeometryXKey, Unit.Zero);
            
            var block = this.Context.DocumentLayout.CurrentPage.LastOpenBlock();
            var reg = block.CurrentRegion;
            
            base.DoLayoutChildren();
            
            if (!reg.IsClosed)
                reg.Close();

            block.Close();
            this.TextContainer.TextBlock = block;

            var b = block.Height;

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

                //block.Position.X = (block.Position.X ?? Unit.Zero) - maxW;
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

                var baseline = first.BaseLineOffset;
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

                var space = (first.Height - begin.TextRenderOptions.GetSize()) / 2;

                //offsetY is by default expressed as the actual Y position for the baseline of the text.
                //our block is top left - so offsetY is the position - the top to the baseline.
                Unit offsetY = yVal - first.BaseLineOffset + space;
                
                Unit offsetX = xVal;

                switch (domBase)
                {
                    case DominantBaseline.Central:
                        //central to the M height
                        offsetY += met.Ascent / 2.0;
                        break;
                    case DominantBaseline.Middle:
                        //middle to the x height
                        offsetY += met.ExHeight / 2.0;
                        break;
                    case DominantBaseline.Hanging:
                        //top of the bounding box
                        offsetY += met.Ascent;
                        break;
                    case DominantBaseline.Mathematical:
                        //top of tghe x height
                        offsetY += met.ExHeight;
                        break;
                    case DominantBaseline.Text_After_Edge:
                    case DominantBaseline.Ideographic:
                        //include the descender
                        offsetY += -met.Descent;
                        break;
                    case DominantBaseline.Alphabetic:
                        //same as auto or text top - normal position of the default shift
                        break;
                        
                    case DominantBaseline.Text_Before_Edge:
                        //full ascender height 
                        offsetY += met.EmHeight;
                        break;

                    case DominantBaseline.Text_Top:
                    case DominantBaseline.Auto:
                    default:
                        //normal position of the default shift
                        break;
                }

                //Add the Y position as the baseline.
                

                
                var orig = block.TotalBounds;
                var update = new Rect(orig.X + offsetX, orig.Y + offsetY + defaultShift, orig.Width, orig.Height);

                //check the textLength

                if (this.TextContainer.TextLength != Unit.Auto)
                {
                    update = this.UpdateCharsWidthForTextLength(this.TextContainer, first, update);
                    Unit required;
                    
                    
                }
                
                if (block.Position.TransformMatrix != null)
                {
                    if (yVal != Unit.Zero)
                        update.Y += yVal;
                    
                    if (xVal != Unit.Zero)
                        update.X += xVal;
                }
                block.TotalBounds = update;
                
            }

            
        }

        protected virtual Rect UpdateCharsWidthForTextLength(SVGText text, PDFLayoutLine onLine, Rect origBounds)
        {
            Rect newBounds = origBounds;
            Unit requiredWidth = text.TextLength;
            
            if (text.TextLength.IsRelative)
            {
                if (text.TextLength.Units != PageUnits.Percent)
                {
                    this.Context.TraceLog.Add(TraceLevel.Warning, "TSpan Layout", "The only supported relative units to TSpan is percent");
                    return newBounds;
                }
                else
                {
                    requiredWidth = requiredWidth * (text.TextLength.Value / 100.0);
                }
            }

            Unit currWidth = origBounds.Width;
            int count = CountCharsOnLine(onLine);

            if (text.LengthAdjust == TextLengthAdjustType.Spacing)
            {
                Unit extra = (requiredWidth - currWidth) / count;
                foreach (var run in onLine.Runs)
                {
                    if (run is PDFTextRunCharacter chars)
                    {
                        chars.ExtraSpace = extra * chars.Characters.Length;
                    }
                    else if (run is PDFTextRunBegin begin)
                    {
                        begin.SetTextSpacing(extra, extra);
                    }
                    
                }
            }
            else
            {
                var stretch = requiredWidth.PointsValue / currWidth.PointsValue;
                foreach (var run in onLine.Runs)
                {
                    if (run is PDFTextRunCharacter chars)
                    {
                        chars.ExtraSpace = chars.Characters.Length * (stretch - 1.0);
                    }
                    else if (run is PDFTextRunBegin begin)
                    {
                        begin.TextRenderOptions.CharacterHScale = stretch;
                    }
                    
                }
            }

            newBounds.Width = requiredWidth;
            return newBounds;
        }

        private int CountCharsOnLine(PDFLayoutLine onLine)
        {
            int total = 0;
            foreach (var run in onLine.Runs)
            {
                if (run is PDFTextRunCharacter chars)
                {
                    total += chars.Characters.Length;
                }
            }

            return total;
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

        
    }

}
