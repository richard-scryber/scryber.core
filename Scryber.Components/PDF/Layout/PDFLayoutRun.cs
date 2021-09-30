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
using Scryber.Components;

namespace Scryber.PDF.Layout
{


    /// <summary>
    /// A significant chunck of visual data that sits on a line
    /// </summary>
    public abstract class PDFLayoutRun : PDFLayoutItem
    {

        #region public PDFLayoutLine Line { get; }

        /// <summary>
        /// Gets the line this run is part of
        /// </summary>
        public PDFLayoutLine Line { get { return this.Parent as PDFLayoutLine; } }

        #endregion

        //
        // ctor
        //

        #region public PDFLayoutRun(PDFLayoutLine line)

        /// <summary>
        /// Creates a new PDFLayoutRun for the specified line
        /// </summary>
        /// <param name="line"></param>
        public PDFLayoutRun(PDFLayoutLine line, IComponent owner)
            : base(line, owner)
        {
        }

        #endregion

        //
        // methods
        //

        #region public virtual void SetOffsetY(PDFUnit y)

        /// <summary>
        /// Sets the y offset of the run wrt it'sparent line.
        /// Inheritors should override if they need to do anything
        /// </summary>
        /// <param name="y"></param>
        public virtual void SetOffsetY(PDFUnit y)
        {
        }

        #endregion
    }



    /// <summary>
    /// A collection of runs
    /// </summary>
    public class PDFLayoutRunCollection : List<PDFLayoutRun>
    {
    }

}
