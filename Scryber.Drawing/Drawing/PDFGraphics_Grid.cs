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
using System.Text;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;

namespace Scryber.Drawing
{
    public partial class PDFGraphics
    {

        #region public void RenderGrid(PDFPen pen, PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height, PDFUnit spacing) + 2 overloads

        /// <summary>
        /// Renders and event grid based upon the rectangle and spacing
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="rect"></param>
        /// <param name="spacing"></param>
        public void RenderGrid(PDFPen pen, PDFRect rect, PDFUnit spacing)
        {
            this.RenderGrid(pen, rect.Location, rect.Size, spacing);
        }

        /// <summary>
        /// Renders an even grid based upon the size and space
        /// </summary>
        /// <param name="pen">The pen to use</param>
        /// <param name="location">The starting location for the grid</param>
        /// <param name="size">The total size of the grid</param>
        /// <param name="spacing">The spacing between gridlines</param>
        public void RenderGrid(PDFPen pen, PDFPoint location, PDFSize size, PDFUnit spacing)
        {
            this.RenderGrid(pen, location.X, location.Y, size.Width, size.Height, spacing);
        }

        
        /// <summary>
        /// Renders an even grid based upon the size and space
        /// </summary>
        /// <param name="pen">The pen to use</param>
        /// <param name="x">The starting X location for the grid</param>
        /// <param name="y">The starting Y location for the grid</param>
        /// <param name="width">The total width of the grid</param>
        /// <param name="height">The total height of the grid</param>
        /// <param name="spacing">The spacing between gridlines</param>
        public void RenderGrid(PDFPen pen, PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height, PDFUnit spacing)
        {
            this.DoRenderGrid(pen, x, y, width, height, spacing);
        }

        /// <summary>
        /// Internal method to render the grid
        /// </summary>
        protected virtual void DoRenderGrid(PDFPen pen, PDFUnit x, PDFUnit y, PDFUnit width, PDFUnit height, PDFUnit spacing)
        {
            this.Writer.WriteOpCodeS(PDFOpCode.SaveState);
            PDFRect bounds = new PDFRect(x, y, width, height);

            pen.SetUpGraphics(this, bounds);

            
            //Draw the vertical lines 
            OutputVerticalLines(x.RealValue, y.RealValue, width.RealValue, height.RealValue, spacing.RealValue);
            
            //Draw the horizontal Lines
            OutputHorizontalLines(x.RealValue, y.RealValue, width.RealValue, height.RealValue, spacing.RealValue);


            pen.ReleaseGraphics(this, bounds);

            this.Writer.WriteOpCodeS(PDFOpCode.RestoreState);

        }

        /// <summary>
        /// Renders a series of horizontal line with the passed spacing
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="spacing"></param>
        private void OutputHorizontalLines(PDFReal x, PDFReal y, PDFReal width, PDFReal height, PDFReal spacing)
        {
            PDFReal maxy = y + height;

            PDFReal minx = GetXPosition(x);
            PDFReal maxx = GetXPosition(x + width);

            while (y < maxy)
            {
                PDFReal ypos = GetYPosition(y);

                this.Writer.WriteOpCodeS(PDFOpCode.GraphMove, minx, ypos);
                this.Writer.WriteOpCodeS(PDFOpCode.GraphLineTo, maxx, ypos);
                this.Writer.WriteOpCodeS(PDFOpCode.GraphStrokePath);

                y += spacing;
            }
        }

        /// <summary>
        /// Renders a series of vertical lines with the passed spacing
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="spacing"></param>
        private void OutputVerticalLines(PDFReal x, PDFReal y, PDFReal width, PDFReal height, PDFReal spacing)
        {
            PDFReal maxx = x + width;

            PDFReal miny = GetYPosition(y);
            PDFReal maxy = GetYPosition(y + height);
            
            while (x < maxx)
            {
                PDFReal xpos = GetXPosition(x);

                this.Writer.WriteOpCodeS(PDFOpCode.GraphMove, xpos, miny);
                this.Writer.WriteOpCodeS(PDFOpCode.GraphLineTo, xpos, maxy);
                this.Writer.WriteOpCodeS(PDFOpCode.GraphStrokePath);

                x += spacing;
            }
        }

        #endregion
    }
}
