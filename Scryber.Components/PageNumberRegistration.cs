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

namespace Scryber
{
    /// <summary>
    /// Represents a contiguous set of pages all with the same style 
    /// running from the first to the last page indices (zero based).
    /// Also used in the layout document to render the page labels on the thumbnails
    /// </summary>
    public class PageNumberRegistration
    {
        private const int UNCLOSED = -1;

        #region public PageNumberGroup Group { get; }

        /// <summary>
        /// Gets the numbering group associated with this registration
        /// </summary>
        public PageNumberGroup Group
        {
            get;
            private set;
        }

        #endregion

        #region public int FirstPageIndex { get; }

        /// <summary>
        /// Gets or sets the global ZERO BASED 
        /// index of a the first page registered.
        /// </summary>
        public int FirstPageIndex { get; private set; }

        #endregion

        #region public int LastPageIndex {get;}

        /// <summary>
        /// Gets or sets the global ZERO BASED last page 
        /// index of this registration
        /// </summary>
        public int LastPageIndex
        {
            get;
            private set;
        }

        #endregion

        #region public bool IsClosed {get;}

        /// <summary>
        /// Gets or sets the flag to indicate if this 
        /// </summary>
        public bool IsClosed
        {
            get { return this.LastPageIndex > UNCLOSED; }
        }

        #endregion

        /// <summary>
        /// Gets the count of previous pages in registrations that match this instances group
        /// </summary>
        public int PreviousLinkedRegistrationPageCount
        {
            get;
            set;
        }

        //
        // ctor
        //

        #region public PageNumberRegistration(int index, PageNumberingGroup grp)

        /// <summary>
        /// Create an new registration of the page numbering group
        /// </summary>
        /// <param name="index"></param>
        /// <param name="grp"></param>
        public PageNumberRegistration(int index, PageNumberGroup grp)
        {
            this.FirstPageIndex = index;
            this.Group = grp;
            this.LastPageIndex = UNCLOSED;
        }

        #endregion

        #region public PageNumberRegistration(int startindex, int endindex, PageNumberingGroup grp)

        /// <summary>
        /// Create an new registration of the page numbering group
        /// </summary>
        /// <param name="index"></param>
        /// <param name="grp"></param>
        public PageNumberRegistration(int startindex, int endindex, PageNumberGroup grp)
        {
            this.FirstPageIndex = startindex;
            this.Group = grp;
            this.LastPageIndex = endindex;
        }

        #endregion

        #region public void Unregister(int lastpageindex)

        /// <summary>
        /// Unregisters (and closes this group) so no more 
        /// pages will be registered with this numbering
        /// </summary>
        /// <param name="lastpageindex"></param>
        public void Unregister(int lastpageindex)
        {
            if (lastpageindex < this.FirstPageIndex)
                lastpageindex = this.FirstPageIndex;

            this.LastPageIndex = lastpageindex;
        }

        #endregion

        #region public void ReRegister() + 1 overload

        /// <summary>
        /// Re-opens the numbering keeping the same first page index
        /// </summary>
        public void ReRegister()
        {
            this.ReRegister(this.FirstPageIndex);
        }

        /// <summary>
        /// Re-opens the numbering with a new first page index
        /// </summary>
        public void ReRegister(int firstpageindex)
        {
            this.FirstPageIndex = firstpageindex;
            this.LastPageIndex = UNCLOSED;
        }

        #endregion

        


        #region public string GetPageNumber(int pageindex)

        /// <summary>
        /// Converts the ZERO BASED page index to a string representation of the page at that index with the group style
        /// </summary>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public string GetPageLabel(int pageindex)
        {
            pageindex = pageindex - FirstPageIndex;
            pageindex = pageindex + this.PreviousLinkedRegistrationPageCount;

            string value = this.Group.GetPageLabel(pageindex);

            return value;
        }

        #endregion

        #region public override string ToString()

        /// <summary>
        /// Returns a string representation of this registration
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.IsClosed)
                return string.Format("[{0} - {1}] : {2}", this.FirstPageIndex, this.LastPageIndex, this.Group);
            else
                return string.Format("[{0} onwards] : {2}", this.FirstPageIndex, this.LastPageIndex, this.Group);
        }

        #endregion
    }
}
