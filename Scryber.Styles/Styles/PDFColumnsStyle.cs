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
    public class PDFColumnsStyle : PDFStyleItemBase
    {
        public static readonly PDFUnit DefaultAlleyWidth = new PDFUnit(10, PageUnits.Points);
        public const bool DefaultAutoFlow = true;


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
                if (this.TryGetValue(PDFStyleKeys.ColumnCountKey,out count))
                    return count;
                else
                    return 1;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ColumnCountKey, value);
            }
        }

        public void RemoveColumnCount()
        {
            this.RemoveValue(PDFStyleKeys.ColumnCountKey);
        }

        #endregion

        #region PDFUnit AlleyWidth {get;set;} RemoveAlleyWidth()

        /// <summary>
        /// Gets or sets the width of the column alley - space between the columns
        /// </summary>
        [PDFAttribute("alley-width")]
        public PDFUnit AlleyWidth
        {
            get
            {
                PDFUnit width;
                if (this.TryGetValue(PDFStyleKeys.ColumnAlleyKey,out width))
                    return width;
                else
                    return DefaultAlleyWidth;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ColumnAlleyKey, value);
            }
        }

        public void RemoveAlleyWidth()
        {
            this.RemoveValue(PDFStyleKeys.ColumnAlleyKey);
        }

        #endregion

        #region public bool AutoFlow {get; set;} + RemoveAutoFlow()

        /// <summary>
        /// If true then the content will fill one available column and then 
        /// automatically flow onto the next column. If false then the content will only flow 
        /// onto the next column after an explicit instruction (e.g. pdf:ColumnBreak). 
        /// The default is true.
        /// </summary>
        [PDFAttribute("auto-flow")]
        public bool AutoFlow
        {
            get
            {
                bool auto;
                if (this.TryGetValue(PDFStyleKeys.ColumnFlowKey,out auto))
                    return auto;
                else
                    return DefaultAutoFlow;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ColumnFlowKey, value);
            }
        }

        /// <summary>
        /// Removes any auto-flow setting from the style
        /// </summary>
        public void RemoveAutoFlow()
        {
            this.RemoveValue(PDFStyleKeys.ColumnFlowKey);
        }

        #endregion

        /// <summary>
        /// Gets or sets the variable width of the columns on a block
        /// </summary>
        [PDFAttribute("column-widths")]
        public PDFColumnWidths ColumnWidths
        {
            get
            {
                PDFColumnWidths widths;
                if (this.TryGetValue(PDFStyleKeys.ColumnWidthKey, out widths))
                    return widths;
                else
                    return PDFColumnWidths.Empty;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ColumnWidthKey, value);
            }
        }

        public void RemoveColumnWidths()
        {
            this.RemoveValue(PDFStyleKeys.ColumnWidthKey);
        }

        public PDFColumnsStyle()
            : base(PDFStyleKeys.ColumnItemKey)
        {
        }

    }
}
