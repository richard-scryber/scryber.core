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

namespace Scryber.Layout
{
    public class PDFLayoutPositionedRegion : PDFLayoutRegion
    {
        //
        // properties
        //

        #region public PDFPositionOptions PositionOptions {get; protected set;}

        private PDFPositionOptions _pos;

        /// <summary>
        /// Gets the position options for this positioned layout
        /// </summary>
        public PDFPositionOptions PositionOptions
        {
            get { return _pos; }
            protected set { _pos = value; }
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFLayoutPositionedRegion(...)

        /// <summary>
        /// Instantiates a new positioned region - relative or absolute.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="owner"></param>
        /// <param name="contentbounds"></param>
        /// <param name="columnindex"></param>
        /// <param name="position"></param>
        public PDFLayoutPositionedRegion(PDFLayoutBlock block, IPDFComponent owner, PDFRect contentbounds, int columnindex, PDFPositionOptions position)
            : base(block, owner, contentbounds, columnindex, position.HAlign, position.VAlign, position.PositionMode)
        {
            this.PositionOptions = position;
        }

        #endregion

        //
        // overrides
        //

        #region protected override bool DoClose(ref string msg)

        /// <summary>
        /// Overrides the default implementation to resize this region to either 
        /// the explicit values or calculated values
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected override bool DoClose(ref string msg)
        {
            //We override this to reduce the size of the region so that it
            //fits the explicit size or the content within it

            PDFUnit h = PDFUnit.Zero;
            PDFUnit w = PDFUnit.Zero;

            //In theory there should just be one block
            foreach (PDFLayoutItem item in this.Contents)
            {
                h += item.Height;
                w = PDFUnit.Max(w, item.Width);
            }

            if (this.PositionMode != Drawing.PositionMode.Absolute && this.PositionMode != Drawing.PositionMode.Relative)
            {
                if (this.PositionOptions.Width.HasValue)
                    w = this.PositionOptions.Width.Value;
                if (this.PositionOptions.Height.HasValue)
                    h = this.PositionOptions.Height.Value;
            }
            this.UsedSize = new PDFSize(w, h);
            this.TotalBounds = new PDFRect(this.TotalBounds.Location, this.UsedSize);

            return base.DoClose(ref msg);
        }

        #endregion

    }
}
