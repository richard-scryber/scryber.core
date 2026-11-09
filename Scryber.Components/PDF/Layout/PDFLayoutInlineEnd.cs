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
    
    public class PDFLayoutInlineEnd : PDFLayoutRun
    {

        public PDFLayoutInlineBegin BeginMarker
        {
            get;
            set;
        }

        public override Drawing.Unit Height
        {
            get { return 0; }
        }

        private Unit _width;

        public override Drawing.Unit Width
        {
            get { return _width; }
        }

        public PDFLayoutInlineEnd(PDFLayoutLine line, PDFLayoutInlineBegin begin, IComponent owner, PDFPositionOptions pos)
            : base(line, owner)
        {
            this.BeginMarker = begin;

            var style = begin.FullStyle;
            //Set the margin inline end width
            if (style.TryGetValue(StyleKeys.MarginsInlineEnd, out StyleValue<Unit> found))
                this._width = found.Value(style);
            else
                this._width = Unit.Zero;
        }

        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, Drawing.Unit xoffset, Drawing.Unit yoffset)
        {

        }
    }
}
