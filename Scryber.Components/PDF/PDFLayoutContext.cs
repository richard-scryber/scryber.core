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
using Scryber.PDF.Layout;
using Scryber.PDF.Graphics;
using Scryber.Logging;

namespace Scryber.PDF
{
    /// <summary>
    /// The current context for laying out the PDF Document
    /// </summary>
    public class PDFLayoutContext : LayoutContext
    {


        #region public PDFLayoutDocument DocumentLayout { get; set; }

        /// <summary>
        /// Gets or sets the layout for this document
        /// </summary>
        public PDFLayoutDocument DocumentLayout { get; set; }

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

        

        public PDFLayoutContext(Style style, ItemCollection items, TraceLog log, PerformanceMonitor perfmon, IDocument document)
            : base(style, items, log, perfmon, document, OutputFormat.PDF)
        {
        }

        protected override IDocumentLayout DoGetLayout()
        {
            return this.DocumentLayout;
        }

    }


    

}
