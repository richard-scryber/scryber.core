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
using System.Threading.Tasks;
using Scryber;
using Scryber.Drawing;
using System.ComponentModel;
using Scryber.PDF.Graphics;
using Point = System.Drawing.Point;

namespace Scryber.Styles
{

    /// <summary>
    /// Defines a matrix transform style
    /// </summary>
    /// <remarks>The transformations on a single style are built in to a PDFTransformMatrix 
    /// in the order of Translate, Rotate, Scale and Skew as defined in the style</remarks>
    [PDFParsableComponent("Transform")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Obsolete("Deprecated to use CSS and single attribute values")]
    public class TransformStyle : StyleItemBase
    {

        public TransformStyle() : base(StyleKeys.TransformItemKey)
        {}
    }


}
