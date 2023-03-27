/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;
using Scryber.Drawing;

namespace Scryber.PDF.Layout
{
    public class PDFLayoutInlineBlockRun : PDFLayoutPositionedRegionRun
    {
        PDFPositionOptions _position;

        public PDFPositionOptions PositionOptions
        {
            get { return _position; }
        }

        private Unit _offsetY = Unit.Zero;

        public override Unit OffsetY
        {
            get { return this._offsetY; }
        }

        public PDFLayoutInlineBlockRun(PDFLayoutRegion region, PDFLayoutLine line, IComponent owner, PDFPositionOptions position)
            : base(region, line, owner)
        {
            _position = position;
        }

        

        public override void SetOffsetY(Drawing.Unit y)
        {
            this._offsetY = y;
        }

        public override Drawing.Unit Height
        {
            get
            {
                //In normal relative or absolute positioning margins are ignored.
                //But inline blocks shoud account for them
                var h = this.Region.Height;
                
                if (this.PositionOptions != null && this.PositionOptions.Margins.IsEmpty == false)
                    h += this.PositionOptions.Margins.Top + this.PositionOptions.Margins.Bottom;
                return h;
            }
        }

        

        public override Drawing.Unit Width
        {
            get
            {
                var w = this.Region.Width;
                if (this.PositionOptions != null && this.PositionOptions.Margins.IsEmpty == false)
                    w += this.PositionOptions.Margins.Left + this.PositionOptions.Margins.Right;
                return w;
            }
        }

        public bool ContentHasMargins(out Thickness margins)
        {
            margins = this.PositionOptions.Margins;
            return !margins.IsEmpty;
        }

        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, Drawing.Unit xoffset, Drawing.Unit yoffset)
        {
            xoffset = Unit.Zero;
            if (this.OffsetY > Unit.Zero)
                yoffset = this.Line.OffsetY + this.OffsetY;
            else
                yoffset = this.Line.OffsetY;

            this.Region.PushComponentLayout(context, pageIndex, xoffset, yoffset);
        }

        protected override bool DoClose(ref string msg)
        {
            var closed = this.Region.Close();
            return closed;
        }

        protected override Native.PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            Native.PDFObjectRef oref = this.Region.OutputToPDF(context, writer);
            return oref;
        }
    }
}
