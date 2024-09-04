using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.Drawing;
using Scryber.PDF;

namespace Scryber.PDF.Layout
{
    public class LayoutEngineAttachment : LayoutEngineBase
    {
        private static double DefaulWidthFactor = 0.75;
        private double WidthFactor { get; set; }

        public LayoutEngineAttachment(IconAttachment attach, IPDFLayoutEngine parent) : base(attach, parent)
        {
            WidthFactor = DefaulWidthFactor;
        }

        
        protected override void DoLayoutComponent()
        {
            //Attachments can be outside of the pages collection and are not shown with an icon.
            
            if (this.Component.Page != null)
            {
                PDFPositionOptions pos = this.FullStyle.CreatePostionOptions(this.Context.PositionDepth > 0);
                PDFTextRenderOptions opts = this.FullStyle.CreateTextOptions();
                FontMetrics metrics = opts.Font.FontMetrics;
                PDFLayoutRegion curReg = this.CurrentBlock.LastOpenBlock().CurrentRegion;

                if (curReg.CurrentItem == null)
                    curReg.BeginNewLine();

                Unit y = Unit.Zero;
                Unit h;
                Unit w;

                if (pos.Height.HasValue)
                    h = pos.Height.Value;
                
                else
                    h = opts.GetAscent(); //set the height as the height of the A


                if (pos.Width.HasValue)
                    w = pos.Width.Value;
                
                else
                    w = h * this.WidthFactor;


                if (pos.Y.HasValue)
                    y = pos.Y.Value;
                else
                    y = (opts.GetLineHeight() - h) - opts.GetDescender();
                

                PDFLayoutLine curLine = this.GetOpenLine(w, curReg, DisplayMode.Inline);
                Rect total = new Rect(0, y, w, h);

                curLine.AddComponentRun(this.Component, total, Rect.Empty, total, total.Height, pos, this.FullStyle);
            }

        }
    }
}
