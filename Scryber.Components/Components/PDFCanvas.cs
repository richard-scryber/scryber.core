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
using Scryber.Styles;

namespace Scryber.Components
{
    /// <summary>
    /// The canvas is a panel where each component is positioned (or is at the top left origin)
    /// </summary>
    [PDFParsableComponent("Canvas")]
    public class PDFCanvas : PDFPanel
    {

        [PDFElement()]
        [PDFArray(typeof(PDFComponent))]
        public override PDFComponentList Contents
        {
            get
            {
                return base.Contents;
            }
        }

        public PDFCanvas()
            : this(PDFObjectTypes.Canvas)
        { }

        protected PDFCanvas(PDFObjectType type)
            : base(type)
        {
        }

        protected override PDFStyle GetBaseStyle()
        {
            PDFStyle style = base.GetBaseStyle();
            style.Overflow.Action = Drawing.OverflowAction.Clip;
            return style;
        }

        protected override IPDFLayoutEngine CreateLayoutEngine(IPDFLayoutEngine parent, PDFLayoutContext context, PDFStyle style)
        {
            return new Layout.CanvasLayoutEngine(this, parent);
        }
    }
}
