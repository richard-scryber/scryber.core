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

namespace Scryber.Layout
{
    /// <summary>
    /// Implements the layout engine for a canvas component. This will set all the canvas' child
    /// components, to relatively positioned unless explictly set to Absolute
    /// </summary>
    public class CanvasLayoutEngine : LayoutEnginePanel
    {

        public CanvasLayoutEngine(ContainerComponent component, IPDFLayoutEngine parent)
            : base(component, parent)
        {
        }

        #region protected override void DoLayoutAChild(IPDFComponent comp, Styles.PDFStyle full)

        /// <summary>
        /// Overrides the base implementation to set the explict position mode before
        /// continuing on as normal
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="full"></param>
        protected override void DoLayoutAChild(IPDFComponent comp, Styles.Style full)
        {

            //For each child if there is not an explict Absolute setting then
            //we should treat them as relative
            Styles.PositionStyle pos = full.Position;
            PositionMode mode = pos.PositionMode;
            
            if (mode != PositionMode.Absolute)
            {
                pos.PositionMode = PositionMode.Relative;
            }

            base.DoLayoutAChild(comp, full);

        }

        #endregion
    }
}
