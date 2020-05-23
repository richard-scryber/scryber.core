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

namespace Scryber.Resources
{
    /// <summary>
    /// Empty font widths for the standard fonts in PDF (Helvetica, Times, etc).
    /// </summary>
    public class PDFEmptyFontWidths : PDFFontWidths
    {

        #region public override bool IsEmpty {get;}

        /// <summary>
        /// Overrides base implmentation to return true
        /// </summary>
        public override bool IsEmpty
        {
            get { return true; }
        }

        #endregion

        public PDFEmptyFontWidths()
            : base()
        {
        }

        

        public override void RenderWidthsArrayToPDF(PDFContextBase context, PDFWriter writer)
        {
            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "PDFFontWidths", "No Widths written for empty widths array.");
            //Do Nothing
        }

        public override char RegisterGlyph(char c)
        {
            return c;
        }
    }
}
