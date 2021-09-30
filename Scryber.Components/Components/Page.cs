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
using Scryber;
using Scryber.Drawing;

namespace Scryber.Components
{
    /// <summary>
    /// Concrete implementation of the PDFPageBase
    /// </summary>
    [PDFRemoteParsableComponent("Page-Ref")]
    [PDFParsableComponent("Page")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_page")]
    public class Page : PageBase
    {


        //
        // page contents
        //

        #region public PDFComponentList Contents {get;}

        /// <summary>
        /// Gets the page contents
        /// </summary>
        [PDFElement("Content")]
        [PDFArray(typeof(Component))]
        public virtual ComponentList Contents
        {
            get { return this.InnerContent; }
        }

        #endregion


        public Page()
            : this(PDFObjectTypes.Page)
        {
            this.Controller = null;
        }

        protected Page(ObjectType type)
            : base(type)
        {
        }

        #region IPDFViewPortComponent Members

        /// <summary>
        /// Instantiates and returns a new instance of the PageLayoutEngine
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="context"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public override IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDF.PDFLayoutContext context, Style style)
        {
            return new PDF.Layout.LayoutEnginePage(this, parent);
        }

        #endregion
    }
}
