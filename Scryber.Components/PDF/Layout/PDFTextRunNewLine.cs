﻿/*  Copyright 2012 PerceiveIT Limited
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
using Scryber.Text;
using Scryber.Drawing;
using Scryber.Components;

namespace Scryber.PDF.Layout
{
    public class PDFTextRunNewLine : PDFTextRun
    {

        #region public override Drawing.PDFUnit Height {get;}

        /// <summary>
        /// Gets the height of the NewLine run - Always Zero.
        /// </summary>
        public override Drawing.Unit Height
        {
            get { return Unit.Zero; }
        }

        #endregion

        #region public override Drawing.PDFUnit Width {get;}

        /// <summary>
        /// Gets the width of the NewLine run - Always Zero
        /// </summary>
        public override Drawing.Unit Width
        {
            get { return Unit.Zero; }
        }

        #endregion

        #region public PDFSize Offset {get; set;}

        private Size _offest;

        /// <summary>
        /// Gets or sets the offset of the line this TextRun is an end to,
        /// to the next running line - e.g. Back 681pt, Down 24pt.
        /// </summary>
        public Size NewLineOffset
        {
            get { return _offest; }
            set { _offest = value; }
        }

        #endregion

        #region public PDFTextRunSpacer NextLineSpacer {get;}

        /// <summary>
        /// Gets or sets the reference to spacer at the front of the next line.
        /// </summary>
        public PDFTextRunSpacer NextLineSpacer
        {
            get;
            set;
        }

        #endregion

        #region public bool IsHardReturn {get;set;}

        /// <summary>
        /// If this new line is a hard return the flag is true
        /// </summary>
        public bool IsHardReturn
        {
            get;
            set;
        }

        #endregion
        
        

        public PDFTextRenderOptions TextOptions { get; private set; }

        //
        // ctor
        //

        #region public PDFTextRunNewLine(bool isHardReturn, PDFLayoutLine line, IPDFComponent owner)

        /// <summary>
        /// Creates a new line text run.
        /// </summary>
        /// <param name="isHardReturn"></param>
        /// <param name="line"></param>
        /// <param name="owner"></param>
        public PDFTextRunNewLine(bool isHardReturn, PDFLayoutLine line, PDFTextRenderOptions opts, IComponent owner)
            : base(line, owner)
        {
            this.IsHardReturn = isHardReturn;
            this.TextOptions = opts;
        }

        #endregion

        //
        // implementation
        //

        /// <summary>
        /// overrides the base implementation to also offset the new line with the spacer.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pageIndex"></param>
        /// <param name="xoffset"></param>
        /// <param name="yoffset"></param>
        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, Unit xoffset, Unit yoffset)
        {
            if (NextLineSpacer != null)
            {
                Size offset = this.NewLineOffset;
                offset.Width += xoffset;

                PDFLayoutLine nextline = this.NextLineSpacer.Line;
                //if (nextline.BaseLineOffset > 0 && this.TextOptions.Leading.HasValue == false)
                //{
                //    PDFUnit maxdescender = nextline.Height - nextline.BaseLineOffset;
                //    PDFUnit ourdescender = this.TextOptions.Font.FontMetrics.LineHeight - this.TextOptions.Font.FontMetrics.Ascent;
                //    PDFUnit difdescender = maxdescender - ourdescender;

                //    offset.Height = nextline.Height - difdescender;
                //}

                this.NewLineOffset = offset;
            }
            base.DoPushComponentLayout(context, pageIndex, xoffset, yoffset);
        }

        /// <summary>
        /// overrides the base implementation to also move the spacer and move the cursor in the graphics context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected override Native.PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            Size offset = this.NewLineOffset;
            
            if (null != this.NextLineSpacer)
            {
                Unit nextoffset = NextLineSpacer.Width;
                offset.Width = nextoffset - offset.Width;
            }
            

            context.Graphics.MoveTextCursor(offset, false);
            return null;
        }
    }
}
