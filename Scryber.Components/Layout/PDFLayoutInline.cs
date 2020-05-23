using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Styles;
using Scryber.Components;

namespace Scryber.Layout
{
    public class PDFLayoutInlineBegin : PDFLayoutRun
    {

        public PDFPositionOptions InlinePosition { get; set; }


        public PDFLayoutInlineEnd EndMarker
        {
            get;
            set;
        }

        public PDFStyle FullStyle
        {
            get;
            set;
        }


        
        public override Drawing.PDFUnit Height
        {
            get { return 0; }
        }

        public override Drawing.PDFUnit Width
        {
            get { return 0; }
        }

        public PDFLayoutInlineBegin(PDFLayoutLine line, IPDFComponent owner, PDFPositionOptions pos, PDFStyle fullStyle)
            : base(line, owner)
        {
            this.InlinePosition = pos;
            this.FullStyle = fullStyle;
        }

        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, Drawing.PDFUnit xoffset, Drawing.PDFUnit yoffset)
        {
            
        }

        protected override Native.PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            Scryber.Drawing.PDFBrush bg = null;
            Scryber.Drawing.PDFPen border = null;
            Drawing.PDFUnit corner = Drawing.PDFUnit.Zero;
            Drawing.Sides sides = Drawing.Sides.Top | Drawing.Sides.Left | Drawing.Sides.Bottom | Drawing.Sides.Right;

            if(null != this.FullStyle)
            {
                bg = this.FullStyle.CreateBackgroundBrush();
                border = this.FullStyle.CreateBorderPen();
                corner = this.FullStyle.GetValue(PDFStyleKeys.BorderCornerRadiusKey, 0.0);
                sides = this.FullStyle.GetValue(PDFStyleKeys.BorderSidesKey, sides);
            }

            if (null != bg)
                this.OutputBackground(bg, border, corner, context, Drawing.PDFRect.Empty);

            Scryber.Native.PDFObjectRef oref = base.DoOutputToPDF(context, writer);

            if (null != border)
                this.OutputBorder(bg, border, corner, sides, context, Drawing.PDFRect.Empty);

            return oref;
        }
    }

    public class PDFLayoutInlineEnd : PDFLayoutRun
    {

        public PDFLayoutInlineBegin BeginMarker
        {
            get;
            set;
        }

        public override Drawing.PDFUnit Height
        {
            get { return 0; }
        }

        public override Drawing.PDFUnit Width
        {
            get { return 0; }
        }

        public PDFLayoutInlineEnd(PDFLayoutLine line, PDFLayoutInlineBegin begin, IPDFComponent owner, PDFPositionOptions pos)
            : base(line, owner)
        {
            this.BeginMarker = begin;
        }

        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, Drawing.PDFUnit xoffset, Drawing.PDFUnit yoffset)
        {

        }
    }
}
