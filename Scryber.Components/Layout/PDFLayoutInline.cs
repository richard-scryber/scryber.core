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

        public Style FullStyle
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

        public PDFLayoutInlineBegin(PDFLayoutLine line, IPDFComponent owner, PDFPositionOptions pos, Style fullStyle)
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
            
            Scryber.Native.PDFObjectRef oref = base.DoOutputToPDF(context, writer);

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
