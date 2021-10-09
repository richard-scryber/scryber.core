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
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.PDF;
using Scryber.PDF.Native;


namespace Scryber.Components
{
    public abstract class ShapeComponent : VisualComponent, IPDFGraphicPathComponent
    {

        public ShapeComponent(ObjectType type) : base(type) { }

        private GraphicsPath _path;

        protected GraphicsPath Path
        {
            get { return _path; }
            set { _path = value; }
        }

        GraphicsPath IPDFGraphicPathComponent.Path
        {
            get { return this.Path; }
            set { this.Path = value; }
        }


        protected abstract GraphicsPath CreatePath(PDFSize available, Style fullstyle);

        GraphicsPath IPDFGraphicPathComponent.CreatePath(PDFSize available, Style fullstyle)
        {
            return this.CreatePath(available, fullstyle);
        }

        public PDFObjectRef OutputToPDF(PDF.PDFRenderContext context, PDFWriter writer)
        {
            Style fullstyle = context.FullStyle;
            if (null == fullstyle)
                throw new ArgumentNullException("context.FullStyle");

            var graphics = context.Graphics;
            if (null == graphics)
                throw new ArgumentNullException("context.Graphics");

            if (null != this.Path)
            {
                PDFBrush brush = fullstyle.CreateFillBrush();
                PDFPen pen = fullstyle.CreateStrokePen();

                if (null != pen && null != brush)
                    graphics.FillAndStrokePath(brush, pen, context.Offset, this.Path);
                
                else if (null != brush)
                    graphics.FillPath(brush, context.Offset, this.Path);
                
                else if (null != pen)
                    graphics.DrawPath(pen, context.Offset, this.Path);

               
            }
            return null;
        }

    }
}
