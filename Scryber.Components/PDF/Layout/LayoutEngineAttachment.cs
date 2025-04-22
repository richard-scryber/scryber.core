using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.Styles;

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
                var icon = this.FullStyle.GetValue(StyleKeys.AttachmentDisplayIconKey, AttachmentDisplayIcon.None);
                if (icon != AttachmentDisplayIcon.None)
                {
                    PDFPositionOptions pos = this.FullStyle.CreatePostionOptions(this.Context.PositionDepth > 0);
                    PDFTextRenderOptions opts = this.FullStyle.CreateTextOptions();
                    FontMetrics metrics = opts.Font.FontMetrics;
                    PDFLayoutRegion curReg = this.CurrentBlock.LastOpenBlock().CurrentRegion;

                    

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
                        y = Unit.Zero;

                    var offset = Unit.Zero;

                    PDFLayoutLine curLine = this.GetOpenLine(w, curReg, DisplayMode.Inline);
                    Rect total = new Rect(0, y, w, h);
                    Rect content = new Rect(0, y, w, h);
                    Rect border = new Rect(0, y, w, h);
                    
                    if (pos.DisplayMode == DisplayMode.InlineBlock)
                    {
                        if (pos.Padding.IsEmpty == false)
                        {
                            total = new Rect(total.X, total.Y,
                                total.Width + pos.Padding.Left + pos.Padding.Right,
                                total.Height + pos.Padding.Top + pos.Padding.Bottom);

                            border = new Rect(border.X, border.Y,
                                border.Width + pos.Padding.Left + pos.Padding.Right,
                                border.Height + pos.Padding.Top + pos.Padding.Bottom);

                            // content.X += pos.Padding.Left;
                            // content.Y += pos.Padding.Top;
                        }

                        if (pos.Margins.IsEmpty == false)
                        {
                            total.Size = new Size(total.Width + pos.Margins.Left + pos.Margins.Right, total.Height + pos.Margins.Top + pos.Margins.Bottom);
                            
                            
                            
                        }
                    }
                    else if (pos.DisplayMode == DisplayMode.Block)
                    {
                        if (pos.Padding.IsEmpty == false)
                        {
                            total = new Rect(total.X, total.Y,
                                total.Width + pos.Padding.Left + pos.Padding.Right,
                                total.Height + pos.Padding.Top + pos.Padding.Bottom);

                            border = new Rect(border.X, border.Y,
                                border.Width + pos.Padding.Left + pos.Padding.Right,
                                border.Height + pos.Padding.Top + pos.Padding.Bottom);

                            content.X += pos.Padding.Left;
                            content.Y += pos.Padding.Top;
                        }
                        
                        if (pos.Margins.IsEmpty == false)
                        {
                            total = new Rect(total.X, total.Y,
                                total.Width + pos.Margins.Left + pos.Margins.Right,
                                total.Height + pos.Margins.Top + pos.Margins.Bottom);
                        }

                        
                    }
                    else if(pos.DisplayMode == DisplayMode.Inline)
                    {
                        var marginStart = FullStyle.GetValue(StyleKeys.MarginsInlineStart, Unit.Empty);
                        var marginEnd = FullStyle.GetValue(StyleKeys.MarginsInlineEnd, Unit.Empty);
                        
                        total.Width += marginStart + marginEnd;
                        border.X += marginStart;
                        content.X += marginStart;
                        
                        var paddingStart = FullStyle.GetValue(StyleKeys.PaddingInlineStart, Unit.Empty);
                        var paddingEnd = FullStyle.GetValue(StyleKeys.PaddingInlineEnd, Unit.Empty);

                        if (paddingStart != Unit.Zero)
                        {
                            total.Width += paddingStart;
                            border.Width += paddingStart;
                            content.X += paddingStart;
                        }

                        if (paddingEnd != Unit.Zero)
                        {
                            total.Width += paddingEnd;
                            border.Width += paddingEnd;
                        }
                    }
                    
                    if (curReg.CurrentItem == null)
                        curReg.BeginNewLine();

                    //pos.VAlign = VerticalAlignment.Baseline;
                    // total = new Rect(0, 0, 9, 12);
                    // border = total;
                    // content = total;
                    //offset = 0;
                    //pos.VAlign = VerticalAlignment.Baseline;
                   
                    var comp = this.Component;
                    var style = this.FullStyle;
                    
                    var run = curLine.AddComponentRun(comp, total, border, content, offset, pos,
                        style);
                    
                    
                }
            }

        }
    }
}
