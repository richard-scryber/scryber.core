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
using Scryber.PDF.Layout;

namespace Scryber.Components
{
    /// <summary>
    /// A list of definition terms. The inner items 
    /// </summary>
    [PDFParsableComponent("Dl")]
    public class ListDefinition : VisualComponent, IPDFViewPortComponent
    {

        #region public DefinitionItemList Items {get;}

        private DefinitionItemList _items;

        /// <summary>
        /// Gets the collection of PDFListItem(s). This will return a wrapped collection of List Items 
        /// </summary>
        [PDFArray(typeof(ListDefinitionItemBase))]
        [PDFElement("")]
        public DefinitionItemList Items
        {
            get
            {
                if (this._items == null)
                    this._items = new DefinitionItemList(this.InnerContent);
                return this._items;
            }
        }

        #endregion

        public ListDefinition()
            : this(ObjectTypes.DefinitionList)
        {
        }

        protected ListDefinition(ObjectType type)
            : base(type)
        {
        }


        #region IPDFViewPortComponent Members

        IPDFLayoutEngine IPDFViewPortComponent.GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        {
            return this.CreateLayoutEngine(parent, context, style);
        }

        protected virtual IPDFLayoutEngine CreateLayoutEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        {
            return new PDF.Layout.LayoutEnginePanel(this, parent);
        }

        #endregion

    }
}
