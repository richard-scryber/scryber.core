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
    /// Implements a textual label as a block, rather than inline
    /// </summary>
    [PDFParsableComponent("Para")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_para")]
    public class Paragraph : Panel
    {
        /// <summary>
        /// Default collection of contents
        /// </summary>
        [PDFElement()]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents
        {
            get
            {
                return base.Contents;
            }
        }

        public Paragraph()
            : this(PDFObjectTypes.Paragraph)
        {
        }

        protected Paragraph(PDFObjectType type)
            : base(type)
        {
        }

        /// <summary>
        /// overrides base implementation to return a position mode of block
        /// </summary>
        /// <returns></returns>
        protected override Styles.PDFStyle GetBaseStyle()
        {
            Styles.PDFStyle b = base.GetBaseStyle();
            b.Position.PositionMode = Drawing.PositionMode.Block;
            b.Margins.Top = new Drawing.PDFUnit(5,Drawing.PageUnits.Points);
            
            b.Size.FullWidth = true;
            return b;
        }

    }
}
