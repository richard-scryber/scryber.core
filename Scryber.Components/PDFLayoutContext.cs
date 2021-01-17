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
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Layout;

namespace Scryber
{
    /// <summary>
    /// The current context for laying out the PDF Document
    /// </summary>
    public class PDFLayoutContext : PDFContextStyleBase
    {


        #region public PDFLayoutDocument DocumentLayout { get; set; }

        /// <summary>
        /// Gets or sets the layout for this document
        /// </summary>
        public PDFLayoutDocument DocumentLayout { get; set; }

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

        public PDFLayoutContext(Style style, PDFOutputFormatting format, PDFItemCollection items, PDFTraceLog log, PDFPerformanceMonitor perfmon, IPDFDocument document)
            : base(new StyleStack(style), items, log, perfmon, document)
        {
            this._format = format;
            
        }


    }


    

}
