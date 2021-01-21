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

        protected override void CreateBlockRegions(PDFLayoutBlock containerBlock, PDFPositionOptions position, PDFColumnOptions columnOptions)
        {
            if(position.ViewPort.HasValue)
            {
                this.ApplyViewPort(containerBlock, position);
            }
            base.CreateBlockRegions(containerBlock, position, columnOptions);
        }


        protected virtual void ApplyViewPort(PDFLayoutBlock containerBlock, PDFPositionOptions position)
        {
            containerBlock.IsFormXObject = true;
            containerBlock.XObjectViewPort = position.ViewPort.Value;
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

        protected virtual void AdjustContainerForTextBaseline(PDFPositionOptions pos, IPDFComponent comp, Style full)
        {
            var text = full.CreateTextOptions();
            
            if (text.DrawTextFromTop == false)
            {
                PDFUnit y;
                var font = full.CreateFont();
                if (pos.Y.HasValue)
                    y = pos.Y.Value;
                else
                    y = 0;

                var doc = this.Component.Document;
                var frsrc = doc.GetFontResource(font, true);
                var metrics = frsrc.Definition.GetFontMetrics(font.Size);

                //TODO: Register the font so that we can get the metrics. Or call later on and move
                // But for now it works (sort of).

                if (null != metrics)
                    y -= metrics.Ascent;
                else
                    y -= font.Size * 0.8;

                pos.Y = y;

                full.Position.Y = y;


                if (full is StyleFull)
                    (full as StyleFull).ClearFullRefs();
            }
        }

        protected override PDFLayoutRegion BeginNewRelativeRegionForChild(PDFPositionOptions pos, IPDFComponent comp, Style full)
        {
            this.AdjustContainerForTextBaseline(pos, comp, full);
            return base.BeginNewRelativeRegionForChild(pos, comp, full);
        }
    }
}
