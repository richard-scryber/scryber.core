using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Components;
using Scryber.PDF.Native;

namespace Scryber.PDF.Layout
{
    public class PDFLayoutInlineBegin : PDFLayoutRun
    {

        public PDFPositionOptions PositionOptions { get; protected set; }

        public PDFTextRenderOptions TextOptions { get; protected set; }

        public PDFLayoutInlineEnd EndMarker
        {
            get;
            set;
        }

        public Style FullStyle
        {
            get;
            set;
        }

        private Unit _width;
        
        public override Drawing.Unit Height
        {
            get { return 0; }
        }

        public override Drawing.Unit Width
        {
            get { return _width; }
        }

        public PDFLayoutInlineBegin(PDFLayoutLine line, IComponent owner, PDFPositionOptions pos, PDFTextRenderOptions text, Style fullStyle)
            : base(line, owner)
        {
            
            this.PositionOptions = pos ?? throw new ArgumentNullException(nameof(pos));
            this.TextOptions = text ?? throw new ArgumentNullException(nameof(text));
            this.FullStyle = fullStyle;

            //Set the margin inline start width
            if (fullStyle.TryGetValue(StyleKeys.MarginsInlineStart, out StyleValue<Unit> found))
                this._width = found.Value(fullStyle);
            else
                this._width = Unit.Zero;

        }

        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, Drawing.Unit xoffset, Drawing.Unit yoffset)
        {
            
        }

        protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            
            PDFObjectRef oref = base.DoOutputToPDF(context, writer);

            if (null != this.Owner && this.Owner is Component comp)
            {
                var bounds = this.GetContainingBounds(context);
                comp.SetArrangement(context, this.FullStyle, bounds);
            }

            return oref; 
        }

        protected Rect GetContainingBounds(PDFRenderContext context)
        {
            var line = this.Line;
            var index = this.Line.Runs.IndexOf(this);
            var maxed = Drawing.Rect.Empty;

            try
            {
                if (index >= 0)
                {
                    index++;
                    while (index < line.Runs.Count)
                    {
                        PDFLayoutRun run = line.Runs[index];

                        if (run is PDFTextRunBegin begin)
                        {
                            var calc = begin.CalculatedBounds;
                            maxed = OutsetRectToInclude(maxed, calc[0]);
                            if (calc.Length > 1)
                                maxed = OutsetRectToInclude(maxed, calc[1]);
                            if (calc.Length > 2)
                                maxed = OutsetRectToInclude(maxed, calc[2]);
                        }
                        else if (run is PDFLayoutComponentRun componentRun)
                        {
                            maxed = OutsetRectToInclude(maxed, componentRun.TotalBounds);
                        }
                        else if (run is PDFLayoutPositionedRegionRun posRun)
                        {
                            var reg = posRun.Owner as Component;
                            if (null != reg)
                            {
                                var arrange = reg.GetFirstArrangement();
                                if (null != arrange)
                                    maxed = OutsetRectToInclude(maxed, arrange.RenderBounds);
                            }
                        }
                        else if (run is PDFLayoutInlineEnd)
                            break;

                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                if (context.TraceLog.RecordLevel >= TraceRecordLevel.Verbose)
                    context.TraceLog.Add(TraceLevel.Warning, "Inline Layout", "Could not calculate the bounds of the inline component '" + this.Owner.ID + "' as an exception was raised: " + ex.Message);
                maxed = Rect.Empty;
            }

            

            return maxed;
        }

        protected Rect OutsetRectToInclude(Drawing.Rect orig, Drawing.Rect include)
        {
            if (orig.IsEmpty)
                return include;

            if (include.IsEmpty)
                return orig;

            var minx = Unit.Min(orig.X, include.X);
            var miny = Unit.Min(orig.Y, include.Y);
            var maxx = Unit.Max(orig.X + orig.Width, include.X + include.Width);
            var maxy = Unit.Max(orig.Y + orig.Height, include.Y + include.Height);

            

            return new Rect(minx, miny, maxx - minx, maxy - miny);

        }
    }

    
}
