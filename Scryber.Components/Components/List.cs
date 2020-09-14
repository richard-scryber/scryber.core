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
using Scryber.Styles;

namespace Scryber.Components
{
    /// <summary>
    /// Not currently Implemented
    /// </summary>
    [PDFParsableComponent("List")]
    public class List : Panel
    {

        #region public PDFListItemList Items {get;}

        private ListItemList _items;

        /// <summary>
        /// Gets the collection of PDFListItem(s). This will return a wrapped collection of List Items 
        /// </summary>
        [PDFArray(typeof(ListItem))]
        [PDFElement("")]
        public ListItemList Items
        {
            get
            {
                if (this._items == null)
                    this._items = new ListItemList(this.InnerContent);
                return this._items;
            }
        }

        #endregion

        #region public ListNumberingGroupStyle NumberingStyle {get;set;}

        /// <summary>
        /// Gets or sets the list numbering style
        /// </summary>
        [PDFAttribute("number-style", Styles.PDFStyle.PDFStylesNamespace)]
        [PDFJSConvertor("scryber.studio.design.convertors.listNumbers_attr")]
        [PDFDesignable("Number Style", Category ="General", Priority = 2, Type = "ListNumberSelect")]
        public ListNumberingGroupStyle NumberingStyle
        {
            get { return this.Style.List.NumberingStyle; }
            set { this.Style.List.NumberingStyle = value; }
        }

        #endregion

        #region public string NumberingGroup {get;set;}

        /// <summary>
        /// Gets or sets thelist numbering group
        /// </summary>
        [PDFAttribute("number-group", Styles.PDFStyle.PDFStylesNamespace)]
        [PDFDesignable("Number Group", Ignore = true)]
        public string NumberingGroup
        {
            get { return this.Style.List.NumberingGroup; }
            set { this.Style.List.NumberingGroup = value; }
        }

        #endregion

        #region public string NumberPrefix {get;set;}

        /// <summary>
        /// Gets or sets the number prefix string that will appear before the actual value
        /// </summary>
        [PDFAttribute("number-prefix", Styles.PDFStyle.PDFStylesNamespace)]
        [PDFDesignable("Prefix", Ignore = true)]
        public string NumberPrefix
        {
            get { return this.Style.List.NumberPrefix; }
            set { this.Style.List.NumberPrefix = value; }
        }

        #endregion

        #region public string NumberPostfix {get;set;}

        /// <summary>
        /// Gets or sets the number prefix string that will appear after the actual value
        /// </summary>
        [PDFAttribute("number-postfix", Styles.PDFStyle.PDFStylesNamespace)]
        [PDFDesignable("Postfix", Ignore = true)]
        public string NumberPostfix
        {
            get { return this.Style.List.NumberPostfix; }
            set { this.Style.List.NumberPostfix = value; }
        }

        #endregion

        #region public PDFUnit NumberInset {get;set;}

        /// <summary>
        /// Gets or sets the amount of indentation before list item (not including the width of the number)
        /// </summary>
        [PDFAttribute("number-inset", Styles.PDFStyle.PDFStylesNamespace)]
        [PDFDesignable("Inset", Ignore = true)]
        public PDFUnit NumberInset
        {
            get { return this.Style.GetValue(PDFStyleKeys.ListInsetKey, Const.DefaultListNumberInset); }
            set { this.Style.SetValue(PDFStyleKeys.ListInsetKey, value); }
        }

        #endregion

        #region public HorizontalAlignment NumberAlignment {get;set;}

        /// <summary>
        /// Gets or sets the alignement of the item number (or label / image) that is shown
        /// </summary>
        [PDFAttribute("number-alignment", Styles.PDFStyle.PDFStylesNamespace)]
        [PDFDesignable("Alignment", Ignore = true)]
        public HorizontalAlignment NumberAlignment
        {
            get
            {
                return this.Style.List.NumberAlignment;
            }
            set
            {
                this.Style.List.NumberAlignment = value;
            }
        }

        #endregion

        #region public bool ConcatenateNumberWithParent {get;set;}

        /// <summary>
        /// Gets the flag that indicates if the list item numbering should be concatenated with the parent list.
        /// </summary>
        [PDFAttribute("number-concat", Styles.PDFStyle.PDFStylesNamespace)]
        [PDFDesignable("Concatenate", Ignore = true)]
        public bool ConcatenateNumberWithParent
        {
            get { return this.Style.List.ConcatenateWithParent; }
            set { this.Style.List.ConcatenateWithParent = value; }
        }

        #endregion


        //
        // ctor
        //

        public List()
            : this(PDFObjectTypes.UnorderedList)
        {
        }

        protected List(PDFObjectType type)
            : base(type)
        {
            
        }

        /// <summary>
        /// Overrides the base implementation to apply the Decimals numbering group type
        /// </summary>
        /// <returns></returns>
        protected override Styles.PDFStyle GetBaseStyle()
        {
            Styles.PDFStyle style = base.GetBaseStyle();
            style.List.NumberingStyle = ListNumberingGroupStyle.Bullet;
            style.List.NumberAlignment = HorizontalAlignment.Right;
            return style;
        }

        protected override IPDFLayoutEngine CreateLayoutEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Styles.PDFStyle style)
        {
            return new Layout.LayoutEngineList(this, parent);
        }

    }
}
