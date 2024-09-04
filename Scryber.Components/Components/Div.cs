﻿/*  Copyright 2012 PerceiveIT Limited
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
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Components
{
    [PDFParsableComponent("Div")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_div")]
    public class Div : Panel
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

        public Div()
            : this(ObjectTypes.Div)
        {
        }

        protected Div(ObjectType type)
            : base(type)
        {
        }

        protected override Style GetBaseStyle()
        {
            Style style = base.GetBaseStyle();
            style.Size.FullWidth = true;
            style.Position.PositionMode = PositionMode.Static;
            style.Position.DisplayMode = Drawing.DisplayMode.Block;
            return style;
        }
    }
}
