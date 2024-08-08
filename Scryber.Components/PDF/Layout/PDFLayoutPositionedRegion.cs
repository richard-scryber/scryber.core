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

        /// <summary>
        /// Calculates the new total bounds for the absolute positioned region based on
        /// not having a positioned parent.
        /// </summary>
        /// <param name="currentOffset">The context offset for the rendering</param>
        protected void UpdateTotalBoundsForBlockParent(Point currentOffset)
        {
            var bounds = this.TotalBounds;

            var xoffset = (this.RelativeTo.PagePosition.X + this.RelativeTo.Position.Margins.Left);
            var yoffset = (this.RelativeTo.PagePosition.Y + this.RelativeTo.Position.Margins.Top);
            if (this.PositionOptions.FloatMode == FloatMode.None)
            {
                bounds.X = xoffset;
            }
            else if (this.PositionOptions.FloatMode == FloatMode.Left)
            {
                //we can have a left inset on the float mode.
                bounds.X += xoffset;
            }

            bounds.Y = yoffset;
            
            var relativeOffset = this.RelativeOffset;

            if (this.PositionOptions.X.HasValue)
            {
                //We have no positioned parent, so the x is taken from the page
                bounds.X = this.PositionOptions.X.Value;
                relativeOffset.X = 0;
            }
            else if (this.PositionOptions.Right.HasValue)
            {
                //we have no positioned parent so the x is taken from the width of
                // the page - right and width of this region
                
                var farRight = this.RelativeTo.GetLayoutPage().Width;
                farRight -= this.PositionOptions.Right.Value;
                farRight -= this.RelativeTo.Position.Padding.Right;
                bounds.X = farRight - this.Width;
                
                relativeOffset.X = 0;
            }
            else if(!this.RelativeTo.Position.Padding.IsEmpty)
            {
                //we have no horizontal offset so we include the padding
                bounds.X += this.RelativeTo.Position.Padding.Left;
            }


            if (this.PositionOptions.Y.HasValue)
            {
                //we have no positioned parent so y is taken from the page
                bounds.Y = this.PositionOptions.Y.Value;
                relativeOffset.Y = 0;
            }
            else if (this.PositionOptions.Bottom.HasValue)
            {
                var bottom = this.RelativeTo.GetLayoutPage().Height;
                bottom -= this.PositionOptions.Bottom.Value;
                bounds.Y = bottom - this.Height;
                relativeOffset.Y = 0;
            }
            else if(!this.RelativeTo.Position.Padding.IsEmpty)
            {
                //we have no vertical position so we include the padding in the offsets
                bounds.Y += this.RelativeTo.Position.Padding.Top;
            }


            bounds.Location = bounds.Location.Offset(relativeOffset);

            this.TotalBounds = bounds;
        }

        /// <summary>
        /// Calculates the new total bounds for the absolute positioned region based on
        /// having a relatively positioned parent.
        /// </summary>
        /// <param name="currentOffset">The context offset for the rendering</param>
        protected void UpdateTotalBoundsForRelativeParent(Point currentOffset)
        {
            var bounds = this.TotalBounds;
            var floatInset = Unit.Zero;

            if (this.PositionOptions.FloatMode == FloatMode.Left)
            {
                floatInset = bounds.X; //previously calculated inset
            }
            else if(this.PositionOptions.FloatMode == FloatMode.Right)
            {
                floatInset = 0 - (this.RelativeTo.Position.Padding.Right); //normally padding is not accounted for with relative parents, however floats should account for it.
            }

            var xoffset = (this.RelativeTo.PagePosition.X);
            var yoffset = (this.RelativeTo.PagePosition.Y);
            bounds.X = xoffset;
            bounds.Y = yoffset;

            var relativeOffset = this.RelativeOffset;

            if (this.PositionOptions.X.HasValue)
            {
                bounds.X += this.PositionOptions.X.Value + this.RelativeTo.Position.Margins.Left;
            }
            else if (this.PositionOptions.Right.HasValue)
            {
                var farRight = this.RelativeTo.PagePosition.X + this.RelativeTo.Width;
                farRight -= this.RelativeTo.Position.Margins.Right;
                farRight -= this.PositionOptions.Right.Value;
                bounds.X = farRight - this.Width;
                
            }
            else
            {
                //we have no horizontal offset so we include the padding
                bounds.X += this.RelativeTo.Position.Padding.Left + this.RelativeTo.Position.Margins.Left;
            }


            if (this.PositionOptions.Y.HasValue)
            {
                bounds.Y += this.PositionOptions.Y.Value + this.RelativeTo.Position.Margins.Top;
            }
            else if (this.PositionOptions.Bottom.HasValue)
            {
                bounds.Y -= this.Height;
                bounds.Y += this.RelativeTo.Height -
                            (this.RelativeTo.Position.Margins.Bottom);
                bounds.Y -= this.PositionOptions.Bottom.Value;
            }
            else
            {
                //we have no vertical position so we include the padding in the offsets
                bounds.Y += this.RelativeTo.Position.Padding.Top;
            }

            relativeOffset.X += floatInset;
            
            bounds.Location = bounds.Location.Offset(relativeOffset);

            this.TotalBounds = bounds;
        }

        /// <summary>
        /// Calculates the new total bounds for the absolute positioned region based on
        /// having an absolutely (or fixed) positioned parent, whose Total bounds will already have been calculated.
        /// </summary>
        /// <param name="currentOffset">The context offset for the rendering</param>
        protected void UpdateTotalBoundsForAbsoluteParent(Point contextOffset)
        {
            var bounds = this.TotalBounds;
            
            var floatOffset = Unit.Zero;
            if (this.PositionOptions.FloatMode != FloatMode.None)
            {
                if (this.PositionOptions.FloatMode == FloatMode.Left)
                    floatOffset = bounds.X;
                else if (this.PositionOptions.FloatMode == FloatMode.Right)
                    floatOffset = bounds.X;
            }

            var xoffset = (this.RelativeTo.PagePosition.X);
            var yoffset = (this.RelativeTo.PagePosition.Y);
            bounds.X = xoffset;
            bounds.Y = yoffset;
            var relativeOffset = this.RelativeOffset;


            if (this.PositionOptions.X.HasValue)
            {
                bounds.X += this.PositionOptions.X.Value + this.RelativeTo.Position.Margins.Left;
                relativeOffset.X = 0;

            }
            else if (this.PositionOptions.Right.HasValue)
            {
                var farRight = this.RelativeTo.PagePosition.X + this.RelativeTo.Width;
                farRight -= this.RelativeTo.Position.Margins.Right + this.RelativeTo.Position.Padding.Right;
                farRight -= this.PositionOptions.Right.Value;
                bounds.X = farRight - this.Width;

                relativeOffset.X = 0;
            }
            else
            {
                bounds.X += this.RelativeTo.Position.Margins.Left + this.RelativeTo.Position.Padding.Left;
            }


            if (this.PositionOptions.Y.HasValue)
            {
                bounds.Y += this.PositionOptions.Y.Value  + this.RelativeTo.Position.Margins.Top;
                relativeOffset.Y = 0;
            }
            else if (this.PositionOptions.Bottom.HasValue)
            {
                var bottom = this.RelativeTo.PagePosition.Y + this.RelativeTo.Height;
                
                bottom -= this.RelativeTo.Position.Margins.Bottom;
                bottom -= this.PositionOptions.Bottom.Value;
                
                bounds.Y = bottom - this.Height;

                relativeOffset.Y = 0;
            }
            else
            {
                bounds.Y += this.RelativeTo.Position.Padding.Top;
            }

            relativeOffset.X += floatOffset;
            
            bounds.Location = bounds.Location.Offset(relativeOffset);

            this.TotalBounds = bounds;
        }

        protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            if(this.RelativeTo != null)
            {
                var mode = this.RelativeTo.Position.PositionMode;
                
                if (this.PositionOptions.FloatMode == FloatMode.Right)
                {
                    //this.RelativeTo region float inset
                    //needs to take account of the possible multiple insets.
                    // if it's not positioned directly.
                    if (mode != PositionMode.Absolute && mode != PositionMode.Fixed && mode != PositionMode.Relative)
                        this.PositionOptions.X = this.RelativeTo.PagePosition.X -
                                                 (this.RelativeTo.Position.Margins.Right + this.RelativeTo.Position.Padding.Right) +
                                                 (this.RelativeTo.Width - this.Width) -
                                                 this.PositionOptions.Right;
                }
                else if (this.PositionOptions.FloatMode == FloatMode.Left)
                {
                    //Todo - push the floating block to the right.
                }
                
                
                if (mode == PositionMode.Fixed)
                {
                    this.UpdateTotalBoundsForAbsoluteParent(context.Offset);
                }
                else if (mode == PositionMode.Absolute)
                {
                    this.UpdateTotalBoundsForAbsoluteParent(context.Offset);
                }
                else if(mode == PositionMode.Relative)
                {
                    this.UpdateTotalBoundsForRelativeParent(context.Offset);
                }
                else
                {
                    this.UpdateTotalBoundsForBlockParent(context.Offset);
                }
            }
            //HACK: Stops any spacing set on the line propogating to the positioned region
            context.Graphics.SaveGraphicsState();
            
            if (null != context.Graphics.CurrentFont)
                context.Graphics.SetTextSpacing(0, 0, context.Graphics.CurrentFont.Size);
            
            var result = base.DoOutputToPDF(context, writer);
            
            //Puts the line spacing back
            context.Graphics.RestoreGraphicsState();

            return result;
        }

    }
}
