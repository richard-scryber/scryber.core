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
using Scryber.Styles;
using Scryber.Native;
using Scryber.Components;

namespace Scryber.Layout
{

    /// <summary>
    /// Represents an inline component.
    /// </summary>
    public class PDFLayoutComponentRun : PDFLayoutRun
    {
        #region public PDFStyle FullStyle { get; private set; }

        /// <summary>
        /// The full applied style for the component
        /// </summary>
        public PDFStyle FullStyle { get; private set; }

        #endregion

        #region public PDFRect TotalBounds { get; set; }

        /// <summary>
        /// The total bounds of the component
        /// </summary>
        public PDFRect TotalBounds { get; set; }

        #endregion

        #region public PDFRect BorderRect { get; set; }

        /// <summary>
        /// The border rect relative the top left of the TotalBounds
        /// </summary>
        public PDFRect BorderRect { get; set; }

        #endregion

        #region public PDFRect ContentRect { get; set; }

        /// <summary>
        /// The content rectangle relative to the top left of the TotalBounds
        /// </summary>
        public PDFRect ContentRect { get; set; }

        #endregion

        #region public PDFPositionOptions PositionOptions { get; set; }

        /// <summary>
        /// The positioning options
        /// </summary>
        public PDFPositionOptions PositionOptions { get; set; }

        #endregion

        //
        // .ctor(s)
        //

        #region public PDFLayoutComponentRun(PDFLayoutLine line, IPDFComponent component, PDFStyle style)

        /// <summary>
        /// Creates a new Component Run
        /// </summary>
        /// <param name="line"></param>
        /// <param name="component"></param>
        /// <param name="style"></param>
        public PDFLayoutComponentRun(PDFLayoutLine line, IPDFComponent component, PDFStyle style)
            : base(line, component)
        {
            System.Diagnostics.Debug.Assert(null != component);
            System.Diagnostics.Debug.Assert(null != style);
            this.FullStyle = style;
        }

        #endregion

        //
        // methods
        //

        #region public void InitSize(PDFRect total ...)

        /// <summary>
        /// Initializes all the sizes in a PDFLayoutComponentRun. 
        /// It is not nescessary, as each property can be set individually, 
        /// but this makes sure everything has been provided
        /// </summary>
        /// <param name="total"></param>
        /// <param name="border"></param>
        /// <param name="content"></param>
        /// <param name="margins"></param>
        /// <param name="padding"></param>
        /// <param name="options"></param>
        public void InitSize(PDFRect total, PDFRect border, PDFRect content, PDFPositionOptions options)
        {
            this.TotalBounds = total;
            this.BorderRect = border;
            this.ContentRect = content;
            this.PositionOptions = options;
        }

        #endregion

        #region public override string ToString()

        /// <summary>
        /// Returns a string representation of this component run
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "LayoutComponentRun for " + this.Owner.ToString() + " (bounds :" + this.TotalBounds.ToString() + ")";
        }

        #endregion

        //
        // abstract overrides
        //

        #region public override PDFUnit Width {get;}

        /// <summary>
        /// Gets the height of this component run
        /// </summary>
        public override PDFUnit Width
        {
            get { return this.TotalBounds.Width; }
        }

        #endregion

        #region public override PDFUnit Height {get;}

        /// <summary>
        /// Gets the width of this component run
        /// </summary>
        public override PDFUnit Height
        {
            get { return this.TotalBounds.Height; }
        }

        #endregion

        #region public override void SetOffsetY(PDFUnit y)

        /// <summary>
        /// Sets the vertical offset of this run wrt the line
        /// </summary>
        /// <param name="y"></param>
        public override void SetOffsetY(PDFUnit y)
        {
            this.TotalBounds = this.TotalBounds.Offset(0, y);
        }

        #endregion

        #region public override void PushComponentLayout(PDFLayoutContext context, PDFUnit xoffset, PDFUnit yoffset)

        /// <summary>
        /// Pushes the component arrangement onto this layouts component
        /// </summary>
        /// <param name="context"></param>
        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageindex, PDFUnit xoffset, PDFUnit yoffset)
        {
            PDFRect total = this.TotalBounds.Offset(xoffset, yoffset);
            this.TotalBounds = total;
        }

        #endregion

        protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            PDFSize prevSize = context.Space;
            PDFPoint prevLoc = context.Offset;
            PDFStyle laststyle = context.FullStyle;

            PDFObjectRef oref;
            if (this.Owner is IPDFRenderComponent)
            {
                PDFPoint loc = context.Offset;
                loc = loc.Offset(this.TotalBounds.Location);
                PDFSize size = this.TotalBounds.Size;
               PDFPositionOptions opts = this.PositionOptions;

                context.Offset = loc;
                context.Space = size;

                if (opts.Margins.IsEmpty == false)
                {
                    loc = loc.Offset(opts.Margins.Left, opts.Margins.Top);
                    size = size.Subtract(opts.Margins);
                }
                
                PDFPen border = this.FullStyle.CreateBorderPen();
                PDFUnit corner;
                Sides sides;
                if (null != border)
                {
                    corner = this.FullStyle.GetValue(PDFStyleKeys.BorderCornerRadiusKey, (PDFUnit)0);
                    sides = this.FullStyle.GetValue(PDFStyleKeys.BorderSidesKey, Sides.Top | Sides.Bottom | Sides.Left | Sides.Right);
                }
                else
                {
                    corner = 0;
                    sides = Sides.Top | Sides.Bottom | Sides.Left | Sides.Right;
                }
                    
                

                PDFBrush background = this.FullStyle.CreateBackgroundBrush();

                PDFRect borderRect = new PDFRect(loc, size);
                if (null != background)
                    this.OutputBackground(background, border, corner, context, borderRect);
                
               
                if (opts.Padding.IsEmpty == false)
                {
                    loc = loc.Offset(opts.Padding.Left, opts.Padding.Top);
                    size = size.Subtract(opts.Padding);
                }

                //Set the offset, size and full style on the context
                context.Offset = loc;
                context.Space = size;
                context.FullStyle = this.FullStyle;
                if (context.ShouldLogDebug)
                    context.TraceLog.Begin(TraceLevel.Verbose, "Layout Item", "Beginning the rendering the referenced component " + this.Owner + " with context offset of " + context.Offset + " and space " + context.Space);
                else if(context.ShouldLogVerbose)
                    context.TraceLog.Add(TraceLevel.Verbose, "Layout Item", "Rendering the referenced component " + this.Owner + " with context offset of " + context.Offset + " and space " + context.Space);
                //Then make the component render itself
                oref = (this.Owner as IPDFRenderComponent).OutputToPDF(context, writer);

                PDFComponent owner = this.Owner as PDFComponent;
                if (null != owner)
                {
                    owner.SetArrangement(context, context.FullStyle, borderRect);
                }

                //finally if we have a border then write this
                if (null != border)
                    this.OutputBorder(background, border, corner, sides, context, borderRect);

                if (context.ShouldLogDebug)
                    context.TraceLog.End(TraceLevel.Verbose, "Layout Item", "Completed the rendering the referenced component " + this.Owner + " and arrangement set to border rect " + borderRect);
                
            }
            else
                oref = base.DoOutputToPDF(context, writer);

            context.Space = prevSize;
            context.Offset = prevLoc;
            context.FullStyle = laststyle;

            return oref;
        }
    }
}
