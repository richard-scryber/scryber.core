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

namespace Scryber.Components
{
    /// <summary>
    /// 
    /// </summary>
    [PDFParsableComponent("Li")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_listitem")]
    public class PDFListItem : PDFPanel
    {

        #region public PDFComponent ItemLabelComponent

        private PDFComponent _itemlabel;
        /// <summary>
        /// Gets or sets the component that is the label / number part of the list item
        /// </summary>
        public PDFComponent ItemLabelComponent
        {
            get { return _itemlabel; }
            set { _itemlabel = value; if (null != value) { value.Parent = this; } }
        }

        #endregion

        #region public string ItemLabelText {get; set;}

        /// <summary>
        /// Gets or sets the label text associated with this list item
        /// </summary>
        [PDFAttribute("item-label", Scryber.Styles.PDFStyle.PDFStylesNamespace)]
        public string ItemLabelText
        {
            get { return this.Style.List.ItemLabel; }
            set { this.Style.List.ItemLabel = value; }
        }

        #endregion

        #region public PDFUnit NumberInset {get;set;}

        /// <summary>
        /// Gets or sets the amount of indentation before list item (not including the width of the number)
        /// </summary>
        [PDFAttribute("number-inset", Styles.PDFStyle.PDFStylesNamespace)]
        public PDFUnit NumberInset
        {
            get { return this.Style.List.NumberInset; }
            set { this.Style.List.NumberInset = value; }
        }

        #endregion

        #region public HorizontalAlignment NumberAlignment {get;set;}

        /// <summary>
        /// Gets or sets the alignement of the item number (or label / image) that is shown
        /// </summary>
        [PDFAttribute("number-alignment", Styles.PDFStyle.PDFStylesNamespace)]
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

        #region public override PDFVisualComponentList Contents {get;set;}

        /// <summary>
        /// Gets the content of the ListItem
        /// </summary>
        [PDFElement()]
        [PDFArray(typeof(PDFComponent))]
        public override PDFComponentList Contents
        {
            get
            {
                return base.Contents;
            }
        }

        #endregion


        public PDFListItem()
            : this(PDFObjectTypes.ListItem)
        {
        }

        protected PDFListItem(PDFObjectType type)
            : base(type)
        {
            
        }

        protected override Styles.PDFStyle GetBaseStyle()
        {
            Styles.PDFStyle defaultStyle = base.GetBaseStyle();
            defaultStyle.Overflow.Split = Drawing.OverflowSplit.Never;
            return defaultStyle;
        }

        

    }



    public class PDFListItemList : PDFComponentWrappingList<PDFListItem>
    {

        #region .ctor(PDFComponentList)

        /// <summary>
        /// Creates the list of list items based on the provided component list. 
        /// </summary>
        /// <param name="inner"></param>
        public PDFListItemList(PDFComponentList inner)
            : base(inner)
        {
        }

        #endregion
    }
}
