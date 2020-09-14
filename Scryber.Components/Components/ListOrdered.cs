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

namespace Scryber.Components
{

    /// <summary>
    /// Not currently Implemented
    /// </summary>
    [PDFParsableComponent("Ol")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_list")]
    public class ListOrdered : List
    {

        public ListOrdered()
            : this(PDFObjectTypes.OrderedList)
        {
        }

        protected ListOrdered(PDFObjectType type)
            : base(type)
        {
        }

        /// <summary>
        /// Overrides the base implementation to apply the Decimals numbering group type
        /// </summary>
        /// <returns></returns>
        protected override Styles.PDFStyle GetBaseStyle()
        {
            Styles.PDFStyle style = base.GetBaseStyle();
            style.List.NumberingStyle = ListNumberingGroupStyle.Decimals;
            return style;
        }


    }
}
