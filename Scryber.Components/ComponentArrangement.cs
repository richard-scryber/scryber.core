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

namespace Scryber
{
    /// <summary>
    /// Defines the rendering arrangement for a component. 
    /// This is the 'physical' position, size, style etc. to be used when rendering
    /// </summary>
    public class ComponentArrangement
    {
        /// <summary>
        /// Gets or sets the page index of this component arrangement
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Gets or sets the complete style for this component
        /// </summary>
        public Style FullStyle { get; set; }


        /// <summary>
        /// Gets or sets the actual PDF boundary rectangle this component will render in (the border rectangle)
        /// </summary>
        public Rect RenderBounds { get; set; }

        /// <summary>
        /// Creates a new PDFComponentArrangement
        /// </summary>
        public ComponentArrangement() { }

    }

    /// <summary>
    /// Defines the arrangement for a component that can be spread across one or more pages and columns as a linked list
    /// </summary>
    public class ComponentMultiArrangement : ComponentArrangement
    {

        #region public PDFComponentMultiArrangement NextArrangement {get;set;}

        private ComponentMultiArrangement _next;

        /// <summary>
        /// Gets or sets the next arrangement for any multiple display component
        /// </summary>
        public ComponentMultiArrangement NextArrangement
        {
            get { return _next; }
            set { _next = value; }
        }

        #endregion

        #region public ComponentMultiArrangement LastArrangement {get;set;}

        /// <summary>
        /// Gets the last arrangement in a multiple display component.
        /// </summary>
        public ComponentMultiArrangement LastArrangement
        {
            get
            {
                var arrange = this;
                var last = arrange;
                while (null != arrange)
                {
                    last = arrange;
                    arrange = arrange.NextArrangement;

                    if (last == arrange)
                        throw new InvalidOperationException(
                            "Circular referernce to arrangements discovered. This is invalid");
                }

                return last;
            }
        }

        #endregion

        #region public bool IsLastArrangement

        /// <summary>
        /// Returns true if this arrangement is the last in the linked list
        /// </summary>
        public bool IsLastArrangement
        {
            get { return this.NextArrangement == null; }
        }

        #endregion

        //
        // methods
        //

        #region public void AppendArrangement(ComponentMultiArrangement arrange)

        /// <summary>
        /// Appends another arrangement to this linked list
        /// </summary>
        /// <param name="arrange"></param>
        public void AppendArrangement(ComponentMultiArrangement arrange)
        {
            this.LastArrangement._next = arrange;
        }

        #endregion

        #region public ComponentArrangement[] GetArrangementsForPage(int pageindex, out int maxcolumnindex)

        /// <summary>
        /// Gets all the arrangements for a specific page
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="maxcolumnindex"></param>
        /// <returns></returns>
        public ComponentArrangement GetArrangementForPage(int pageindex)
        {
            if (this.PageIndex == pageindex)
                return this;
            else if (null != this.NextArrangement)
                return this.NextArrangement.GetArrangementForPage(pageindex);
            else
                return null;



        }

        #endregion

    }


    
}
