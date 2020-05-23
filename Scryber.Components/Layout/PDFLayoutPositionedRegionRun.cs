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

namespace Scryber.Layout
{
    public class PDFLayoutPositionedRegionRun : PDFLayoutRun
    {
        /// <summary>
        /// Gets the RelativeRegion associated with this
        /// </summary>
        public PDFLayoutRegion Region
        {
            get;
            private set;
        }

        public PDFLayoutPositionedRegionRun(PDFLayoutRegion region, PDFLayoutLine line, IPDFComponent owner)
            : base(line, owner)
        {
            this.Region = region;
        }

        public override void SetOffsetY(Drawing.PDFUnit y)
        {
            //Do Nothing
        }

        public override Drawing.PDFUnit Height
        {
            get { return Drawing.PDFUnit.Zero; }
        }

        public override Drawing.PDFUnit Width
        {
            get { return Drawing.PDFUnit.Zero; }
        }

        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, Drawing.PDFUnit xoffset, Drawing.PDFUnit yoffset)
        {
            xoffset = 0;
            yoffset = 0;
            this.Region.PushComponentLayout(context, pageIndex, xoffset, yoffset);
        }

        protected override bool DoClose(ref string msg)
        {
            return this.Region.Close();
        }

        protected override Native.PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            Scryber.Drawing.PDFPoint oldOffset = context.Offset;

            if (this.Region.PositionMode == Drawing.PositionMode.Absolute)
            {
                context.Offset = Scryber.Drawing.PDFPoint.Empty;
            }
            Native.PDFObjectRef oref = this.Region.OutputToPDF(context, writer);

            context.Offset = oldOffset;
            return oref;
        }
    }
}
