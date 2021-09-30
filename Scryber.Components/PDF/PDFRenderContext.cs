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
using System.Drawing;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.PDF
{
    public class PDFRenderContext : PDFContextStyleBase
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

        #region public PDFPoint Offset {get;set;}

        private PDFPoint _offset = PDFPoint.Empty;

        public PDFPoint Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        #endregion

        #region public PDFSize Space {get;set;}

        private PDFSize _space;

        public PDFSize Space
        {
            get { return _space; }
            set { _space = value; }
        }

        #endregion

        #region public PDFSize PageSize

        private PDFSize _size = PDFSize.Empty;

        public PDFSize PageSize
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

        #region public PDFStyle FullStyle

        /// <summary>
        /// Gets or sets the full style for the current component
        /// </summary>
        public Style FullStyle
        {
            get;
            set;
        }

        #endregion

        #region public PDFOutputFormatting OutputFormat {get;}

        private PDFOutputFormatting _format;

        /// <summary>
        /// Gets the output format for the document
        /// </summary>
        public PDFOutputFormatting Formatting
        {
            get { return _format; }
        }

        #endregion

        //
        // .ctor
        //

        public PDFRenderContext(DrawingOrigin origin, int pageCount, PDFOutputFormatting format, Styles.Style root, PDFItemCollection items, PDFTraceLog log, PDFPerformanceMonitor perfmon, IDocument document)
            : this(origin,pageCount,format, new Scryber.Styles.StyleStack(root), items, log, perfmon, document)
        {
        }

        internal PDFRenderContext(DrawingOrigin origin, int pageCount, PDFOutputFormatting format, Styles.StyleStack stack, PDFItemCollection items, PDFTraceLog log, PDFPerformanceMonitor perfmon, IDocument document) 
            : base(stack, items, log, perfmon, document)
        {
            this._origin = origin;
            this._offset = new PDFPoint();
            this._space = new PDFSize();
            this._pgCount = pageCount;
            this._pgindex = 0;
            this._format = format;
            
        }
        
    }
}
