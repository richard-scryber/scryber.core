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

namespace Scryber.Styles
{
    [PDFParsableComponent("Overlay-Grid")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class OverlayGridStyle : StyleItemBase
    {

        public static readonly PDFUnit DefaultGridSpacing = new PDFUnit(50, PageUnits.Points);
        public static readonly PDFColor DefaultGridColor = new PDFColor(0, 1, 1);//aqua.
        public static readonly double DefaultGridOpacity = 0.5;
        public static readonly PDFUnit DefaultXOffset = PDFUnit.Empty;
        public static readonly PDFUnit DefaultYOffset = PDFUnit.Empty;
        public static readonly PDFUnit DefaultGridPenWidth = new PDFUnit(0.1, PageUnits.Points);


        #region public bool ShowGrid {get;set;} + RemoveGrid()

        /// <summary>
        /// When set to true the applied component will render a grid overlay ontop of all the inner content
        /// </summary>
        [PDFAttribute("show")]
        public bool ShowGrid
        {
            get
            {
                bool show;
                if (this.TryGetValue(StyleKeys.OverlayShowGridKey, out show))
                    return show;
                else
                    return false;
            }
            set
            {
                this.SetValue(StyleKeys.OverlayShowGridKey, value);
            }
        }

        public void RemoveGrid()
        {
            this.RemoveValue(StyleKeys.OverlayShowGridKey);
        }

        #endregion

        #region public PDFUnit GridSpacing {get;set;} + RemoveGridBonus()

        /// <summary>
        /// Gets or sets the space between each grid square
        /// </summary>
        [PDFAttribute("spacing")]
        public PDFUnit GridSpacing
        {
            get
            {
                PDFUnit size;
                if (this.TryGetValue(StyleKeys.OverlaySpacingKey, out size))
                    return size;
                else
                    return DefaultGridSpacing;
            }
            set
            {
                this.SetValue(StyleKeys.OverlaySpacingKey, value);
            }
        }

        public void RemoveGridSpacing()
        {
            this.RemoveValue(StyleKeys.OverlaySpacingKey);
        }

        #endregion

        #region public PDFColor GridColor {get;set;} + RemoveGridColor()

        /// <summary>
        /// Gets or sets the color of the grid. Default value is 'aqua'
        /// </summary>
        [PDFAttribute("color")]
        public PDFColor GridColor
        {
            get
            {
                PDFColor col;
                if (this.TryGetValue(StyleKeys.OverlayColorKey, out col))
                    return col;
                else
                    return DefaultGridColor;
            }
            set
            {
                this.SetValue(StyleKeys.OverlayColorKey, value);
            }
        }

        public void RemoveGridColor()
        {
            this.RemoveValue(StyleKeys.OverlayColorKey);
        }

        #endregion

        #region public double GridOpacity{get;set;} + removeGridOpacity()

        /// <summary>
        /// Gets or sets the opacity of the overlay grid
        /// </summary>
        [PDFAttribute("opacity")]
        public double GridOpacity
        {
            get
            {
                double op;
                if (this.TryGetValue(StyleKeys.OverlayOpacityKey, out op))
                    return op;
                else
                    return DefaultGridOpacity;
            }
            set
            {
                this.SetValue(StyleKeys.OverlayOpacityKey, value);
            }
        }

        public void RemoveGridOpacity()
        {
            this.RemoveValue(StyleKeys.OverlayOpacityKey);
        }

        #endregion

        #region public PDFUnit GridXOffset {get;set;} + RemoveGridXOffset

        /// <summary>
        /// Gets or sets the horizontal offset from the left of the component to start rendering the grid. Default is 0 (zero).
        /// </summary>
        [PDFAttribute("x-offset")]
        public PDFUnit GridXOffset
        {
            get
            {
                PDFUnit x;
                if (this.TryGetValue(StyleKeys.OverlayXOffsetKey, out x))
                    return x;
                else
                    return DefaultXOffset;
            }
            set
            {
                this.SetValue(StyleKeys.OverlayXOffsetKey, value);
            }
        }


        public void RemoveGridXOffset()
        {
            this.RemoveValue(StyleKeys.OverlayXOffsetKey);
        }

        #endregion

        #region public PDFUnit GridYOffset {get;set;} + RemoveGridYOffset()

        /// <summary>
        /// Gets or sets the vertical offset from the top position of the component to start rendering the grid. 
        /// Default is 0 (zero).
        /// </summary>
        [PDFAttribute("y-offset")]
        public PDFUnit GridYOffset
        {
            get
            {
                PDFUnit y;
                if (this.TryGetValue(StyleKeys.OverlayYOffsetKey, out y))
                    return y;
                else
                    return DefaultYOffset;
            }
            set
            {
                this.SetValue(StyleKeys.OverlayYOffsetKey, value);
            }
        }

        public void RemoveGridYOffset()
        {
            this.RemoveValue(StyleKeys.OverlayYOffsetKey);
        }

        #endregion

        #region public bool HighlightColumns {get;set;} + RemoveHighlightColumns()

        [PDFAttribute("fill-columns")]
        public bool HighlightColumns
        {
            get
            {
                bool b;
                if (this.TryGetValue(StyleKeys.OverlayShowColumnsKey, out b))
                    return b;
                else
                    return false;
            }
            set { this.SetValue(StyleKeys.OverlayShowColumnsKey, value); }
        }

        public void RemoveHighlightColumns()
        {
            this.RemoveValue(StyleKeys.OverlayShowColumnsKey);
        }

        #endregion

        //
        // .ctor
        //

        public OverlayGridStyle()
            : base(StyleKeys.OverlayItemKey)
        {
        }


        #region public PDFPen GetPen()

        /// <summary>
        /// Gets the pen to use to render the grid lines.
        /// </summary>
        /// <returns></returns>
        public PDFPen GetPen()
        {
            return this.AssertOwner().DoCreateOverlayGridPen();
        }

        #endregion

    }
}
