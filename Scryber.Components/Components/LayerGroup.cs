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
    /// The layer group supports an inner collection of layers that will all be positioned over the top of the previous layer(s).
    /// </summary>
    [PDFParsableComponent("LayerGroup")]
    public class LayerGroup : VisualComponent, IPDFViewPortComponent
    {

        #region public PDFLayerList Layers {get;}

        private PDFLayerList _layers = null;
        /// <summary>
        /// Gets the list of pages in this document
        /// </summary>
        [PDFElement("")]
        [PDFArray(typeof(Layer))]
        public PDFLayerList Layers
        {
            get
            {
                if (this._layers == null)
                    this._layers = new PDFLayerList(this.InnerContent);
                return _layers;
            }
        }

        #endregion


        public LayerGroup()
            : this(PDFObjectTypes.LayerGroup)
        {
        }

        public LayerGroup(PDFObjectType type)
            : base(type)
        {
        }



        #region IPDFViewPortComponent Members

        public IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, PDFStyle fullstyle)
        {
            return new Layout.CanvasLayoutEngine(this, parent);
        }

        #endregion
    }
}
