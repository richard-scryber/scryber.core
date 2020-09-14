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

namespace Scryber.Components
{
    [PDFParsableComponent("Blockquote")]
    public class BlockQuote : Panel
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

        public BlockQuote()
            : this(PDFObjectTypes.BlockQuote)
        {
        }

        protected BlockQuote(PDFObjectType type)
            : base(type)
        {
        }

        /// <summary>
        /// Block quote overrides the default panel style to have Block positioning, 10pt margins all around and an italic font
        /// </summary>
        /// <returns></returns>
        protected override PDFStyle GetBaseStyle()
        {
            PDFStyle style = base.GetBaseStyle();
            style.Position.PositionMode = Drawing.PositionMode.Block;
            style.Margins.All = 10;
            style.Font.FontItalic = true;
            return style;
        }
    }
}
