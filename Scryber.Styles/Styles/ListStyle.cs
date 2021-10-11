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
    [PDFParsableComponent("List")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ListStyle : StyleItemBase
    {

        #region public ListNumberingGroupStyle NumberingStyle {get;set;}

        /// <summary>
        /// Gets or sets the numbering type of this list style
        /// </summary>
        [PDFAttribute("number-style")]
        public ListNumberingGroupStyle NumberingStyle
        {
            get
            {
                ListNumberingGroupStyle value;
                if (this.TryGetValue(StyleKeys.ListNumberStyleKey, out value))
                    return value;
                else
                    return ListNumberingGroupStyle.None;
            }
            set
            {
                this.SetValue(StyleKeys.ListNumberStyleKey, value);
            }
        }

        public void RemoveNumberingStyle()
        {
            this.RemoveValue(StyleKeys.ListNumberStyleKey);
        }

        #endregion

        #region public string NumberingGroup {get;set;}

        /// <summary>
        /// Gets or sets the numbering group for this style
        /// </summary>
        [PDFAttribute("number-group")]
        public string NumberingGroup
        {
            get
            {
                string value;
                if (this.TryGetValue(StyleKeys.ListGroupKey, out value))
                    return value;
                else
                    return string.Empty;
            }
            set
            {
                this.SetValue(StyleKeys.ListGroupKey, value);
            }
        }

        public void RemoveNumberingGroup()
        {
            this.RemoveValue(StyleKeys.ListGroupKey);
        }


        #endregion

        #region public PDFUnit NumberInset {get;set;}

        /// <summary>
        /// Gets or sets the inset of the number from the start of the line
        /// </summary>
        [PDFAttribute("number-inset")]
        public Unit NumberInset
        {
            get
            {
                Unit value;
                if (this.TryGetValue(StyleKeys.ListInsetKey, out value))
                    return value;
                else
                    return Const.DefaultListNumberInset;
            }
            set
            {
                this.SetValue(StyleKeys.ListInsetKey, value);
            }
        }

        public void RemoveNumberInset()
        {
            this.RemoveValue(StyleKeys.ListInsetKey);
        }

        #endregion

        #region public string NumberPrefix {get;set;}

        /// <summary>
        /// Gets or sets the prefix string that will be before the number in the list item
        /// </summary>
        [PDFAttribute("number-prefix")]
        public string NumberPrefix
        {
            get
            {
                string value;
                if (this.TryGetValue(StyleKeys.ListPrefixKey, out value))
                    return value;
                else
                    return string.Empty;
            }
            set
            {
                this.SetValue(StyleKeys.ListPrefixKey, value);
            }
        }

        public void RemoveNumberPrefix()
        {
            this.RemoveValue(StyleKeys.ListPrefixKey);
        }

        #endregion

        #region public string NumberPostfix {get;set;}

        /// <summary>
        /// Gets or sets the postfix string that will be after the number in the list item
        /// </summary>
        [PDFAttribute("number-postfix")]
        public string NumberPostfix
        {
            get
            {
                string value;
                if (this.TryGetValue(StyleKeys.ListPostfixKey, out value))
                    return value;
                else
                    return string.Empty;
            }
            set
            {
                this.SetValue(StyleKeys.ListPostfixKey, value);
            }
        }

        public void RemoveNumberPostfix()
        {
            this.RemoveValue(StyleKeys.ListPostfixKey);
        }

        #endregion

        #region public HorizontalAlignment NumberAlignment {get;set;}

        /// <summary>
        /// Gets or sets the alignement of the item number (or label / image) that is shown
        /// </summary>
        [PDFAttribute("number-alignment")]
        public HorizontalAlignment NumberAlignment
        {
            get
            {
                HorizontalAlignment value;
                if (this.TryGetValue(StyleKeys.ListAlignmentKey, out value))
                    return value;
                else
                    return Const.DefaultListNumberAlignment;
            }
            set
            {
                this.SetValue(StyleKeys.ListAlignmentKey, value);
            }
        }

        public void RemoveNumberAlignment()
        {
            this.RemoveValue(StyleKeys.ListAlignmentKey);
        }

        #endregion

        #region public bool ConcatenateWithParent {get;set;}

        /// <summary>
        /// Gets or sets the flag to indicate if this list style should be concatenated with any parent list number schemes (lists that contain lists) during layout.
        /// </summary>
        [PDFAttribute("number-concat")]
        public bool ConcatenateWithParent
        {
            get
            {
                bool value;
                if (this.TryGetValue(StyleKeys.ListConcatKey, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                this.SetValue(StyleKeys.ListConcatKey, value);
            }
        }

        public void RemoveConcatenateWithParent()
        {
            this.RemoveValue(StyleKeys.ListConcatKey);
        }

        #endregion

        #region public string ItemLabel {get;set;}

        /// <summary>
        /// Gets or sets the label associated with the current list item.
        /// Only used if the number style is label
        /// </summary>
        [PDFAttribute("item-label")]
        public string ItemLabel
        {
            get
            {
                string value;
                if (this.TryGetValue(StyleKeys.ListLabelKey, out value))
                    return value;
                else
                    return string.Empty;
            }
            set
            {
                this.SetValue(StyleKeys.ListLabelKey, value);
            }
        }

        public void RemoveItemLabel()
        {
            this.RemoveValue(StyleKeys.ListLabelKey);
        }

        #endregion


        //
        // .ctor
        //

        public ListStyle()
            : base(StyleKeys.ListItemKey)
        {
        }

        
    }
}
