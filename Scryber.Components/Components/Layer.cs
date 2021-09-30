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

namespace Scryber.Components
{
    [PDFParsableComponent("Layer")]
    public class Layer : Panel
    {

        [PDFElement()]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents
        {
            get
            {
                return base.Contents;
            }
        }

        public Layer()
            : this(PDFObjectTypes.Layer)
        {
        }

        protected Layer(ObjectType type)
            : base(type)
        {
        }

        protected override Styles.Style GetBaseStyle()
        {
            Styles.Style style = base.GetBaseStyle();
            style.Position.PositionMode = Drawing.PositionMode.Relative;
            return style;
        }
    }



    public class PDFLayerList : ComponentWrappingList<Layer>
    {
        public PDFLayerList(ComponentList innerList)
            : base(innerList)
        {
        }
    }
}
