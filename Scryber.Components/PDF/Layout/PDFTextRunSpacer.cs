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
    /// Just an empty spacer.
    /// </summary>
    public class PDFTextRunSpacer : PDFTextRun
    {
        #region ivars

        private Unit _w;
        private Unit _h;

        #endregion

        //
        // properties
        // 
        
        #region public override PDFUnit Width {get;}

        /// <summary>
        /// Gets or sets the width of this spacer.
        /// </summary>
        public override Unit Width
        {
            get { return this._w; }
        }

        #endregion

        #region public override PDFUnit Height {get;}

        /// <summary>
        /// Gets or sets the height of this spacer.
        /// </summary>
        public override Unit Height
        {
            get { return _h; }
        }

        #endregion

        /// <summary>
        /// Returns true if this spacer is at the start of a new line - so it will apply any horizontal alignment
        /// </summary>
        public bool IsNewLineSpacer
        {
            get;
            protected set;
        }

        public PDFTextRunNewLine PreviousNewLine
        {
            get;
            protected set;
        }

        //
        // ctor
        //

        #region public PDFTextRunSpacer(PDFUnit width, PDFUnit height, PDFLayoutLine line, IPDFComponent owner)


        public PDFTextRunSpacer(Unit width, Unit height, PDFLayoutLine line, IComponent owner, PDFTextRunNewLine newLineSpacer)
            : base(line, owner)
        {
            this.IsNewLineSpacer = null != newLineSpacer;
            this.PreviousNewLine = newLineSpacer;
            this.SetSpacing(width, height);
        }

        #endregion

        //
        // methods
        //

        #region public virtual void SetSpacing(PDFUnit width, PDFUnit height)

        /// <summary>
        /// Sets the Width and Height spacing values to the required size.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public virtual void SetSpacing(Unit width, Unit height)
        {
            this._w = width;
            this._h = height;
        }

        #endregion

        
        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, Unit xoffset, Unit yoffset)
        {
            if (xoffset > 0 && this.IsNewLineSpacer)
            {
                this._w += xoffset;
            }

            base.DoPushComponentLayout(context, pageIndex, xoffset, yoffset);
        }

        protected override Native.PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            return base.DoOutputToPDF(context, writer);
        }
    }
}
