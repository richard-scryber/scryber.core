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
    [PDFParsableComponent("Page")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [PDFJSConvertor("scryber.studio.design.convertors.styleItem", JSParams = "\"page\"")]
    public class PageStyle : StyleItemBase
    {

        #region public PaperSize PaperSize {get;set;} + RemovePaperSize()

        [PDFAttribute("size")]
        [PDFJSConvertor("scryber.studio.design.convertors.papersize_css")]
        [PDFDesignable("Size", Category = "Paper", Priority = 1, Type = "Select")]
        public PaperSize PaperSize
        {
            get
            {
                PaperSize val;
                if (this.TryGetValue(StyleKeys.PagePaperSizeKey, out val))
                    return val;
                else
                    return Const.DefaultPaperSize;
            }
            set
            {
                this.SetValue(StyleKeys.PagePaperSizeKey, value);
            }
        }

        public void RemovePaperSize()
        {
            this.RemoveValue(StyleKeys.PagePaperSizeKey);
        }

        #endregion

        #region public PaperOrientation PaperOrientation {get;set} + RemovePaperOrientation()

        [PDFAttribute("orientation")]
        [PDFJSConvertor("scryber.studio.design.convertors.paperorientation_css")]
        [PDFDesignable("Orientation", Category = "Paper", Priority = 1, Type = "Select")]
        public PaperOrientation PaperOrientation
        {
            get
            {
                PaperOrientation val;
                if (this.TryGetValue(StyleKeys.PageOrientationKey, out val))
                    return val;
                else
                    return Const.DefaultPaperOrientation;
            }
            set
            {
                this.SetValue(StyleKeys.PageOrientationKey, value);
            }
        }

        public void RemovePaperOrientation()
        {
            this.RemoveValue(StyleKeys.PageOrientationKey);
        }

        #endregion

        #region public PDFUnit Width {get;set;} + RemoveWidth()

        [PDFAttribute("width")]
        public PDFUnit Width
        {
            get
            {
                PDFUnit f;
                if (this.TryGetValue(StyleKeys.PageWidthKey, out f))
                    return f;
                else
                    return PDFUnit.Empty;

            }
            set
            {
                this.SetValue(StyleKeys.PageWidthKey, value);
            }
        }

        public void RemoveWidth()
        {
            this.RemoveValue(StyleKeys.PageWidthKey);
        }

        #endregion

        #region public PDFUnit Height {get;set;} + RemoveHeight()

        [PDFAttribute("height")]
        public PDFUnit Height
        {
            get
            {
                PDFUnit f;
                if (this.TryGetValue(StyleKeys.PageHeightKey, out f))
                    return f;
                else
                    return PDFUnit.Empty;

            }
            set
            {
                this.SetValue(StyleKeys.PageHeightKey, value);
            }
        }

        public void RemoveHeight()
        {
            this.RemoveValue(StyleKeys.PageHeightKey);
        }

        #endregion

        #region public PageNumberStyle NumberStyle {get;set;} + RemoveNumberStyle()

        [PDFAttribute("number-style")]
        [PDFDesignable("Numbering Style", Category = "Paper", Priority = 1, Type = "Select")]
        public PageNumberStyle NumberStyle
        {
            get
            {
                PageNumberStyle style;
                if (this.TryGetValue(StyleKeys.PageNumberStyleKey, out style))
                    return style;
                else
                    return PageNumberStyle.Decimals;
            }
            set
            {
                this.SetValue(StyleKeys.PageNumberStyleKey, value);
            }
        }

        public void RemoveNumberStyle()
        {
            this.RemoveValue(StyleKeys.PageNumberStyleKey);
        }

        #endregion

        #region public bool BreakBefore {get;set;} + RemoveBreakBefore()

        [PDFAttribute("break-before")]
        public bool BreakBefore
        {
            get
            {
                bool value;
                if (this.TryGetValue(StyleKeys.PageBreakBeforeKey, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                this.SetValue(StyleKeys.PageBreakBeforeKey, value);
            }
        }

        public void RemoveBreakBefore()
        {
            this.RemoveValue(StyleKeys.PageBreakBeforeKey);
        }

        #endregion

        #region public bool BreakAfter {get;set;} + RemoveBreakAfter()

        [PDFAttribute("break-after")]
        public bool BreakAfter
        {
            get
            {
                bool value;
                if (this.TryGetValue(StyleKeys.PageBreakAfterKey, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                this.SetValue(StyleKeys.PageBreakAfterKey, value);
            }
        }

        public void RemoveBreakAfter()
        {
            this.RemoveValue(StyleKeys.PageBreakAfterKey);
        }

        #endregion

        #region public int NumberStartIndex {get;set;}

        /// <summary>
        /// Gets or sets the stating page index number
        /// </summary>
        [PDFAttribute("number-start-index")]
        [PDFDesignable("Number Start Index", Category = "Paper", Priority = 1)]
        public int NumberStartIndex
        {
            get
            {
                int start;
                if (this.TryGetValue(StyleKeys.PageNumberStartKey, out start))
                    return start;
                else
                    return 1;
            }
            set
            {
                this.SetValue(StyleKeys.PageNumberStartKey, value);
            }
        }

        /// <summary>
        /// Clears any existing value for the number start index from this style
        /// </summary>
        public void RemoveNumberStartIndex()
        {
            this.RemoveValue(StyleKeys.PageNumberStartKey);
        }

        #endregion

        #region public string NumberGroup {get;set;}

        /// <summary>
        /// Not currently used
        /// </summary>
        [PDFAttribute("number-group")]
        public string NumberGroup
        {
            get
            {
                string value;
                if (this.TryGetValue(StyleKeys.PageNumberGroupKey, out value))
                    return value;
                else
                    return string.Empty;
            }
            set
            {
                this.SetValue(StyleKeys.PageNumberGroupKey, value);
            }
        }

        /// <summary>
        /// Clears any existing number group value from this style
        /// </summary>
        public void RemoveNumberGroup()
        {
            this.RemoveValue(StyleKeys.PageNumberGroupKey);
        }

        #endregion

        #region public string PageNumberFormat {get;set;} + RemovePageNumberFormat

        /// <summary>
        /// Gets or sets the page number format string for this page
        /// </summary>
        [PDFAttribute("display-format")]
        [PDFDesignable("Number Display Format", Category = "Paper", Priority = 1)]
        public string PageNumberFormat
        {
            get
            {
                string value;
                if (this.TryGetValue(StyleKeys.PageNumberFormatKey, out value))
                    return value;
                else
                    return string.Empty;
            }
            set
            {
                this.SetValue(StyleKeys.PageNumberFormatKey, value);
            }
        }

        /// <summary>
        /// Clears any existing page number format value from this style
        /// </summary>
        public void RemovePageNumberFormat()
        {
            this.RemoveValue(StyleKeys.PageNumberFormatKey);
        }

        #endregion

        #region public int PageGroupCountHint {get;set;} + RemovePageGroupCountHint

        /// <summary>
        /// Gets or sets the hinted / approximate number of pages expected in this current page group
        /// </summary>
        [PDFAttribute("group-count-hint")]
        public int PageGroupCountHint
        {
            get
            {
                int value;
                if (this.TryGetValue(StyleKeys.PageNumberGroupHintKey, out value))
                    return value;
                else
                    return -1;
            }
            set
            {
                this.SetValue(StyleKeys.PageNumberGroupHintKey, value);
            }
        }

        /// <summary>
        /// Clears any existing page number format value from this style
        /// </summary>
        public void RemovePageGroupCountHint()
        {
            this.RemoveValue(StyleKeys.PageNumberGroupHintKey);
        }

        #endregion

        #region public int PageTotalCountHint {get;set;} + RemovePageGroupCountHint

        /// <summary>
        /// Gets or sets the hinted / approximate number of pages expected in this document
        /// </summary>
        [PDFAttribute("total-count-hint")]
        public int PageTotalCountHint
        {
            get
            {
                int value;
                if (this.TryGetValue(StyleKeys.PageNumberTotalHintKey, out value))
                    return value;
                else
                    return -1;
            }
            set
            {
                this.SetValue(StyleKeys.PageNumberTotalHintKey, value);
            }
        }

        /// <summary>
        /// Clears any existing page count hint value from this style
        /// </summary>
        public void RemovePageTotalCountHint()
        {
            this.RemoveValue(StyleKeys.PageNumberTotalHintKey);
        }

        #endregion

        #region public PageRotationAngles PageAngle {get;set;} + RemovePageAngle

        private static readonly int[] AllowedPageIndexValues = new int[] { 0, 90, 180, 270 };
        /// <summary>
        /// The angle of the page in increments of 90 degrees
        /// </summary>
        [PDFAttribute("page-angle")]
        [PDFDesignable("Page Rotation", Ignore = true,  Category = "Paper", Priority = 1, Type = "Select")]
        public PageRotationAngles PageAngle
        {
            get
            {
                int value;
                if (this.TryGetValue(StyleKeys.PageAngle, out value))
                    return (PageRotationAngles)value;
                else
                    return 0;
            }
            set
            {
                if (Array.IndexOf(AllowedPageIndexValues, value) >= 0)
                    this.SetValue(StyleKeys.PageAngle, (int)value);
            }
        }

        public void RemovePageAngle()
        {
            this.RemoveValue(StyleKeys.PageAngle);
        }

        #endregion

        //
        // ctor
        //

        public PageStyle()
            : base(StyleKeys.PageItemKey)
        {
        }

        public PageSize CreatePageSize()
        {
            return this.AssertOwner().DoCreatePageSize();
        }
    }
}
