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

        private PDFUnit _w;
        private PDFUnit _h;

        #endregion

        //
        // properties
        // 
        
        #region public override PDFUnit Width {get;}

        /// <summary>
        /// Gets or sets the width of this spacer.
        /// </summary>
        public override PDFUnit Width
        {
            get { return this._w; }
        }

        #endregion

        #region public override PDFUnit Height {get;}

        /// <summary>
        /// Gets or sets the height of this spacer.
        /// </summary>
        public override PDFUnit Height
        {
            get { return _h; }
        }

        #endregion

        

        //
        // ctor
        //

        #region public PDFTextRunSpacer(PDFUnit width, PDFUnit height, PDFLayoutLine line, IPDFComponent owner)


        public PDFTextRunSpacer(PDFUnit width, PDFUnit height, PDFLayoutLine line, IComponent owner)
            : base(line, owner)
        {
            this.SetSpacing(width, height);
        }

        #endregion

        //
        // methods
        //

        #region protected virtual void SetSpacing(PDFUnit width, PDFUnit height)

        /// <summary>
        /// Sets the Width and Height spacing values to the required size.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected virtual void SetSpacing(PDFUnit width, PDFUnit height)
        {
            this._w = width;
            this._h = height;
        }

        #endregion

        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, PDFUnit xoffset, PDFUnit yoffset)
        {
            if (xoffset > 0)
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
