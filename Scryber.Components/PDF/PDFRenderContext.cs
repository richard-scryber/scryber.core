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
using Scryber.PDF.Graphics;
using Scryber.Logging;

namespace Scryber.PDF
{
    public class PDFRenderContext : RenderContext
    {

        //
        // properties
        //

        #region public int PageIndex {get;set;}

        private int _pgindex;

        /// <summary>
        /// Gets or sets the page index of this rendercontext (first page is ZERO)
        /// </summary>
        public int PageIndex
        {
            get { return _pgindex; }
            set 
            {
                _pgindex = value;
            }
        }

        #endregion

        #region public int PageCount {get;set;}

        private int _pgCount;

        public int PageCount
        {
            get { return _pgCount; }
            set { _pgCount = value; }
        }

        #endregion

        #region public DrawingOrigin DrawingOrigin {get;set;}

        private DrawingOrigin _origin = DrawingOrigin.TopLeft;

        public DrawingOrigin DrawingOrigin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        #endregion

        #region public Point Offset {get;set;}

        private Point _offset = Point.Empty;

        public Point Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        #endregion

        #region public Size Space {get;set;}

        private Size _space;

        public Size Space
        {
            get { return _space; }
            set { _space = value; }
        }

        #endregion

        #region public Size PageSize

        private Size _size = Size.Empty;

        public Size PageSize
        {
            get { return this._size; }
            set { this._size = value; }
        }

        #endregion

        #region public PDFGraphics Graphics {get;set;}

        /// <summary>
        /// Gets or sets the graphics 
        /// </summary>
        public PDFGraphics Graphics
        {
            get;
            set;
        }

        #endregion

        #region public Style FullStyle

        /// <summary>
        /// Gets or sets the full style for the current component
        /// </summary>
        public Style FullStyle
        {
            get;
            set;
        }

        #endregion

        public PDFTransformationMatrix RenderMatrix { get; set; }

        //
        // .ctor
        //

        internal PDFRenderContext(DrawingOrigin origin, int pageCount, Style root, ItemCollection items, TraceLog log, PerformanceMonitor perfmon, IDocument document) 
            : base(root, items, log, perfmon, document, OutputFormat.PDF)
        {
            this._origin = origin;
            this._offset = new Point();
            this._space = new Size();
            this._pgCount = pageCount;
            this._pgindex = 0;
            
        }
        
    }
}
