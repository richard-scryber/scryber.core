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
    public class PDFListStyle : PDFStyleItemBase
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
                if (this.TryGetValue(PDFStyleKeys.ListNumberStyleKey, out value))
                    return value;
                else
                    return ListNumberingGroupStyle.None;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ListNumberStyleKey, value);
            }
        }

        public void RemoveNumberingStyle()
        {
            this.RemoveValue(PDFStyleKeys.ListNumberStyleKey);
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
                if (this.TryGetValue(PDFStyleKeys.ListGroupKey, out value))
                    return value;
                else
                    return string.Empty;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ListGroupKey, value);
            }
        }

        public void RemoveNumberingGroup()
        {
            this.RemoveValue(PDFStyleKeys.ListGroupKey);
        }


        #endregion

        #region public PDFUnit NumberInset {get;set;}

        /// <summary>
        /// Gets or sets the inset of the number from the start of the line
        /// </summary>
        [PDFAttribute("number-inset")]
        public PDFUnit NumberInset
        {
            get
            {
                PDFUnit value;
                if (this.TryGetValue(PDFStyleKeys.ListInsetKey, out value))
                    return value;
                else
                    return Const.DefaultListNumberInset;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ListInsetKey, value);
            }
        }

        public void RemoveNumberInset()
        {
            this.RemoveValue(PDFStyleKeys.ListInsetKey);
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
                if (this.TryGetValue(PDFStyleKeys.ListPrefixKey, out value))
                    return value;
                else
                    return string.Empty;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ListPrefixKey, value);
            }
        }

        public void RemoveNumberPrefix()
        {
            this.RemoveValue(PDFStyleKeys.ListPrefixKey);
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
                if (this.TryGetValue(PDFStyleKeys.ListPostfixKey, out value))
                    return value;
                else
                    return string.Empty;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ListPostfixKey, value);
            }
        }

        public void RemoveNumberPostfix()
        {
            this.RemoveValue(PDFStyleKeys.ListPostfixKey);
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
                if (this.TryGetValue(PDFStyleKeys.ListAlignmentKey, out value))
                    return value;
                else
                    return Const.DefaultListNumberAlignment;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ListAlignmentKey, value);
            }
        }

        public void RemoveNumberAlignment()
        {
            this.RemoveValue(PDFStyleKeys.ListAlignmentKey);
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
                if (this.TryGetValue(PDFStyleKeys.ListConcatKey, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ListConcatKey, value);
            }
        }

        public void RemoveConcatenateWithParent()
        {
            this.RemoveValue(PDFStyleKeys.ListConcatKey);
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
                if (this.TryGetValue(PDFStyleKeys.ListLabelKey, out value))
                    return value;
                else
                    return string.Empty;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ListLabelKey, value);
            }
        }

        public void RemoveItemLabel()
        {
            this.RemoveValue(PDFStyleKeys.ListLabelKey);
        }

        #endregion


        //
        // .ctor
        //

        public PDFListStyle()
            : base(PDFStyleKeys.ListItemKey)
        {
        }

        
    }
}
