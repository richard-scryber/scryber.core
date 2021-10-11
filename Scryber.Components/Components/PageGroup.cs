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
using Scryber.PDF;

namespace Scryber.Components
{

    /// <summary>
    /// Encloses a group of pages or sections. Can also be styled and outlined to provide structure to document pages
    /// </summary>
    [PDFParsableComponent("PageGroup")]
    [PDFRemoteParsableComponent("PageGroup-Ref")]
    public class PageGroup : PageBase
    {

        #region  public IPDFTemplate ContinuationHeader {get;set;}

        private ITemplate _header;

        /// <summary>
        /// Gets or sets the template for the continuation footer of this Page Group
        /// (the footer that will be shown on subsequent pages other than the first)
        /// </summary>
        [PDFTemplate()]
        [PDFElement("Continuation-Header")]
        public ITemplate ContinuationHeader
        {
            get { return _header; }
            set { _header = value; }
        }

        #endregion

        #region public IPDFTemplate ContinuationFooter {get;set;}

        private ITemplate _footer;

        /// <summary>
        /// Gets or sets the template for the continuation footer of this Page Group 
        /// (the footer that will be shown on subsequent pages other than the first)
        /// </summary>
        [PDFTemplate()]
        [PDFElement("Continuation-Footer")]
        public ITemplate ContinuationFooter
        {
            get { return _footer; }
            set { _footer = value; }
        }

        #endregion

        #region public PDFPageList Pages {get;set;}

        private PageList _pages;

        /// <summary>
        /// Gets or sets the list of Pages / Sections and also nested PageGroups in this group
        /// </summary>
        [PDFArray(typeof(PageBase))]
        [PDFElement("Pages")]
        public PageList Pages
        {
            get
            {
                if (null == _pages)
                    _pages = new PageList(this.InnerContent);
                return _pages;
            }
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFPageGroup()

        public PageGroup()
            : this(ObjectTypes.PageGroup)
        {
        }

        #endregion

        #region protected PDFPageGroup(PDFObjectType type)

        protected PageGroup(ObjectType type)
            : base(type)
        {
        }

        #endregion

        //
        // methods
        //

        #region public override IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Styles.PDFStyle style)

        /// <summary>
        /// Override the base implementation to return a PageGroupLayoutEngine
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="context"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public override IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDF.PDFLayoutContext context, Styles.Style style)
        {
            return new PDF.Layout.LayoutEnginePageGroup(this, parent, context, style);
        }

        #endregion

        #region public override PDFComponentArrangement GetFirstArrangement()

        /// <summary>
        /// Override the GetFirstArrangement to return the arrangement of the first page
        /// </summary>
        /// <returns></returns>
        public override ComponentArrangement GetFirstArrangement()
        {
            if (this.Pages.Count > 0)
                return this.Pages[0].GetFirstArrangement();
            else
                return null;
        }

        #endregion


    }
}
