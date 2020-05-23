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
using System.ComponentModel;

using Scryber;
using Scryber.Drawing;

namespace Scryber.Styles
{
    [PDFParsableComponent("Badge")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFScryberBadgeStyle : PDFStyleItemBase
    {
        public static readonly PDFUnit DefaultXOffset = new PDFUnit(3, PageUnits.Millimeters);
        public static readonly PDFUnit DefaultYOffset = new PDFUnit(3, PageUnits.Millimeters);
        public const BadgeType DefaultDisplayOption = BadgeType.WhiteOnBlack;

        #region public Corner Corner {get;set;}

        [PDFAttribute("corner")]
        public Corner Corner
        {
            get
            {
                Corner val;
                if (this.TryGetValue(PDFStyleKeys.BadgeCornerKey, out val))
                    return val;
                else
                    return Corner.BottomRight;
            }
            set
            {
                this.SetValue(PDFStyleKeys.BadgeCornerKey, value);
            }
        }

        public void RemoveCorner()
        {
            this.RemoveValue(PDFStyleKeys.BadgeCornerKey);
        }

        #endregion

        #region public Reversible DisplayOption {get;set;}

        [PDFAttribute("display")]
        public BadgeType DisplayOption
        {
            get
            {
                BadgeType val;
                if (this.TryGetValue(PDFStyleKeys.BadgeDisplayKey, out val))
                    return val;
                else
                    return DefaultDisplayOption;
            }
            set
            {
                this.SetValue(PDFStyleKeys.BadgeDisplayKey, value);
            }

        }

        public void RemoveDisplayOption()
        {
            this.RemoveValue(PDFStyleKeys.BadgeDisplayKey);
        }

        #endregion

        #region public PDFUnit XOffset {get; set;}

        [PDFAttribute("x-offset")]
        public PDFUnit XOffset
        {
            get
            {
                PDFUnit val;
                if (this.TryGetValue(PDFStyleKeys.BadgeXOffsetKey, out val))
                    return val;
                else
                    return DefaultXOffset;
            }
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 100)
                    value = 100;

                this.SetValue(PDFStyleKeys.BadgeXOffsetKey, value);
            }
        }

        public void RemoveXOffset()
        {
            this.RemoveValue(PDFStyleKeys.BadgeXOffsetKey);
        }

        #endregion

        #region public PDFUnit YOffset {get;set;}

        [PDFAttribute("y-offset")]
        public PDFUnit YOffset
        {
            get
            {
                PDFUnit val;
                if (this.TryGetValue(PDFStyleKeys.BadgeYOffsetKey, out val))
                    return val;
                else
                    return DefaultYOffset;
            }
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 100)
                    value = 100;

                this.SetValue(PDFStyleKeys.BadgeYOffsetKey, value);
            }
        }

        public void RemoveYOffset()
        {
            this.RemoveValue(PDFStyleKeys.BadgeYOffsetKey);
        }

        #endregion

        //
        // .ctor
        //

        public PDFScryberBadgeStyle()
            : base(PDFStyleKeys.BadgeItemKey)
        {
        }


    }
}
