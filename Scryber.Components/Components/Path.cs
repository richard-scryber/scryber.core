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
using Scryber.Drawing;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Components
{
    [PDFParsableComponent("path")]
    public class Path : ShapeComponent
    {

        private PDFGraphicsPath _data;

        [PDFElement()]
        [PDFAttribute("d")]
        public PDFGraphicsPath PathData
        {
            get { return _data; }
            set { _data = value; }
        }


        /// <summary>
        /// Gets the fill style associated with this polygon
        /// </summary>
        [PDFAttribute("fill-style", Scryber.Styles.Style.PDFStylesNamespace)]
        public FillType FillStyle
        {
            get { return this.Style.Fill.Style; }
            set { this.Style.Fill.Style = value; }
        }

        public Path()
            : this(PDFObjectTypes.ShapePath)
        { }

        protected Path(PDFObjectType type)
            : base(type)
        {
        }

        
        protected override Drawing.PDFGraphicsPath CreatePath(Drawing.PDFSize available, Style fullstyle)
        {
            return this.PathData;
        }


    }
}
