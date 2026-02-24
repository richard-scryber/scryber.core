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
    [PDFParsableComponent("Columns")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [PDFJSConvertor("scryber.studio.design.convertors.styleItem", JSParams = "\"Columns\"")]
    public class ColumnsStyle : StyleItemBase
    {
        public static readonly Unit DefaultAlleyWidth = new Unit(10, PageUnits.Points);
        public const ColumnFillMode DefaultAutoFlow = ColumnFillMode.Auto;


        #region public int ColumnCount {get;set;} + RemoveColumnCount

        /// <summary>
        /// Gets or sets the number of columns
        /// </summary>
        [PDFAttribute("count")]
        [PDFJSConvertor("scryber.studio.design.convertors.integer_attr", JSParams = "\"column-count\"")]
        public int ColumnCount
        {
            get
            {
                int count;
                if (this.TryGetValue(StyleKeys.ColumnCountKey,out count))
                    return count;
                else
                    return 1;
            }
            set
            {
                this.SetValue(StyleKeys.ColumnCountKey, value);
            }
        }

        public void RemoveColumnCount()
        {
            this.RemoveValue(StyleKeys.ColumnCountKey);
        }

        #endregion

        #region PDFUnit AlleyWidth {get;set;} RemoveAlleyWidth()

        /// <summary>
        /// Gets or sets the width of the column alley - space between the columns
        /// </summary>
        [PDFAttribute("alley-width")]
        public Unit AlleyWidth
        {
            get
            {
                Unit width;
                if (this.TryGetValue(StyleKeys.ColumnAlleyKey,out width))
                    return width;
                else
                    return DefaultAlleyWidth;
            }
            set
            {
                this.SetValue(StyleKeys.ColumnAlleyKey, value);
            }
        }

        public void RemoveAlleyWidth()
        {
            this.RemoveValue(StyleKeys.ColumnAlleyKey);
        }

        #endregion

        #region public bool AutoFlow {get; set;} + RemoveAutoFlow()

        /// <summary>
        /// If true then the content will fill one available column and then 
        /// automatically flow onto the next column. If false then the content will only flow 
        /// onto the next column after an explicit instruction (e.g. pdf:ColumnBreak). 
        /// The default is true.
        /// </summary>
        [PDFAttribute("column-fill")]
        public ColumnFillMode FillMode
        {
            get
            {
                ColumnFillMode value;
                if (this.TryGetValue(StyleKeys.ColumnFillKey,out value))
                    return value;
                else
                    return DefaultAutoFlow;
            }
            set
            {
                this.SetValue(StyleKeys.ColumnFillKey, value);
            }
        }

        /// <summary>
        /// Removes any auto-flow setting from the style
        /// </summary>
        public void RemoveFillMode()
        {
            this.RemoveValue(StyleKeys.ColumnFillKey);
        }

        #endregion

        #region public bool BreakBefore {get;set;} + RemoveBreakBefore()

        [PDFAttribute("break-before")]
        public bool BreakBefore
        {
            get
            {
                bool value;
                if (this.TryGetValue(StyleKeys.ColumnBreakBeforeKey, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                this.SetValue(StyleKeys.ColumnBreakBeforeKey, value);
            }
        }

        public void RemoveBreakBefore()
        {
            this.RemoveValue(StyleKeys.ColumnBreakBeforeKey);
        }

        #endregion


        #region public bool BreakAfter {get;set;} + RemoveBreakAfter()

        [PDFAttribute("break-after")]
        public bool BreakAfter
        {
            get
            {
                bool value;
                if (this.TryGetValue(StyleKeys.ColumnBreakAfterKey, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                this.SetValue(StyleKeys.ColumnBreakAfterKey, value);
            }
        }

        public void RemoveBreakAfter()
        {
            this.RemoveValue(StyleKeys.ColumnBreakAfterKey);
        }

        #endregion


        /// <summary>
        /// Gets or sets the variable width of the columns on a block
        /// </summary>
        [PDFAttribute("widths")]
        public ColumnWidths ColumnWidths
        {
            get
            {
                ColumnWidths widths;
                if (this.TryGetValue(StyleKeys.ColumnWidthKey, out widths))
                    return widths;
                else
                    return ColumnWidths.Empty;
            }
            set
            {
                this.SetValue(StyleKeys.ColumnWidthKey, value);
            }
        }

        public void RemoveColumnWidths()
        {
            this.RemoveValue(StyleKeys.ColumnWidthKey);
        }

        public ColumnsStyle()
            : base(StyleKeys.ColumnItemKey)
        {
        }

    }
}
