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
using Scryber.Drawing;
using Scryber.Components;

namespace Scryber.PDF.Layout
{

    /// <summary>
    /// Marks the end of a bunch of characters on a page
    /// </summary>
    public class PDFTextRunEnd : PDFTextRun
    {

        //
        // properties
        //

        #region public PDFTextBeginRun Start {get;}

        private PDFTextRunBegin _start;

        /// <summary>
        /// Reference back to the start of this text block.
        /// </summary>
        public PDFTextRunBegin Start
        {
            get { return _start; }
        }

        #endregion

        #region public override PDFUnit Height {get;}

        /// <summary>
        /// Gets the height of this end run - PDFUnit.Zero
        /// </summary>
        public override PDFUnit Height
        {
            get { return PDFUnit.Zero; }
        }

        #endregion

        #region public override Drawing.PDFUnit Width {get;}

        /// <summary>
        /// Gets the width of this end run - PDFUnit.Zero
        /// </summary>
        public override Drawing.PDFUnit Width
        {
            get { return PDFUnit.Zero; }
        }

        #endregion

        //
        // ctor
        //

        public PDFTextRunEnd(PDFTextRunBegin start, PDFLayoutLine line, IComponent owner)
            : base(line, owner)
        {
            this._start = start;
        }

        //
        // implementation
        //

        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, PDFUnit xoffset, PDFUnit yoffset)
        {
            base.DoPushComponentLayout(context, pageIndex, xoffset, yoffset);

            //As we have now completed the full layout of this begin -> end text block, then we can close it off.
            this.Start.PushCompleteTextBlock(this, context, pageIndex, xoffset, yoffset);
        }

        /// <summary>
        /// Simply ends the text and restores state
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected override Native.PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            context.Graphics.EndText();
            context.Graphics.RestoreGraphicsState();
            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, PDFTextRun.TEXT_LOG_CATEGORY, "Ended the text rendering of component '" + this.Owner.ToString());

            if (null != this.Start)
            {
                if (this.Start.ShouldRenderUnderline(context))
                    this.Start.RenderUnderlines(this, context, writer);

                if (this.Start.ShouldRenderStrikeThrough(context))
                    this.Start.RenderStrikeThrough(this, context, writer);

                if (this.Start.ShouldRenderOverline(context))
                    this.Start.RenderOverLines(this, context, writer);

                if (this.Start.HasCustomSpace)
                    context.Graphics.ResetCustomWordSpace();
            }

            return null;
        }
    }
}
