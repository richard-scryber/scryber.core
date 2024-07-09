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
using Scryber.PDF.Native;

namespace Scryber.PDF.Layout
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

        /// <summary>
        /// Gets or set the run associated with this positioned region
        /// </summary>
        public PDFLayoutPositionedRegionRun AssociatedRun { get; set; }

        public PDFLayoutBlock RelativeTo { get; set; }

        public Point RelativeOffset { get; set; }

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
        public PDFLayoutPositionedRegion(PDFLayoutBlock block, IComponent owner, Rect contentbounds, int columnindex, PDFPositionOptions position)
            : base(block, owner, contentbounds, columnindex, position.HAlign ?? HorizontalAlignment.Left, position.VAlign ?? VerticalAlignment.Top, position.PositionMode)
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

            Unit h = Unit.Zero;
            Unit w = Unit.Zero;

            //In theory there should just be one block
            foreach (PDFLayoutItem item in this.Contents)
            {
                h += item.Height;
                w = Unit.Max(w, item.Width);
            }

            if (this.PositionMode == PositionMode.Fixed)
            {
                if (this.PositionOptions.Height.HasValue)
                    h = this.PositionOptions.Height.Value;
            }
            else if (this.PositionMode != Drawing.PositionMode.Absolute)
            {
                if (this.PositionOptions.Width.HasValue)
                    w = this.PositionOptions.Width.Value;
                if (this.PositionOptions.Height.HasValue)
                    h = this.PositionOptions.Height.Value;
            }
            this.UsedSize = new Size(w, h);
            this.TotalBounds = new Rect(this.TotalBounds.Location, this.UsedSize);

            return base.DoClose(ref msg);

        }

        #endregion

        protected void UpdateTotalBoundsForRelativeParent(Point currentOffset)
        {
            var bounds = this.TotalBounds;

            var xoffset = (this.RelativeTo.PagePosition.X + this.RelativeTo.Position.Margins.Left);
            var yoffset = (this.RelativeTo.PagePosition.Y + this.RelativeTo.Position.Margins.Top);
            bounds.X +=  xoffset;
            bounds.Y +=  yoffset;

            if (this.PositionOptions.X.HasValue)
                bounds.X += this.PositionOptions.X.Value;
            else if (this.PositionOptions.Right.HasValue)
            {
                var farRight = this.RelativeTo.PagePosition.X + this.RelativeTo.Width;
                farRight -= this.RelativeTo.Position.Margins.Right;
                farRight -= this.PositionOptions.Right.Value;
                bounds.X = farRight - this.Width;
                
            }

            if (this.PositionOptions.Y.HasValue)
                bounds.Y += this.PositionOptions.Y.Value;
            else if (this.PositionOptions.Bottom.HasValue)
            {
                
                bounds.Y -= this.Height;
                bounds.Y += this.RelativeTo.Height - (this.RelativeTo.Position.Margins.Top + this.RelativeTo.Position.Margins.Bottom);
                bounds.Y -= this.PositionOptions.Bottom.Value;
                //bounds.Y -= this.PositionOptions.Bottom.Value;
                
            }

            this.TotalBounds = bounds;
        }

        protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            if(this.RelativeTo != null)
            {
                this.UpdateTotalBoundsForRelativeParent(context.Offset);
            }
            return base.DoOutputToPDF(context, writer);
        }

    }
}
