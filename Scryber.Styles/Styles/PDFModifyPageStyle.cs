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

namespace Scryber.Styles
{
    [PDFParsableComponent("Modify-Page")]
    public class PDFModifyPageStyle : PDFStyleItemBase
    {

        #region public ModificationType ModificationType {get;set;}

        private const string ModifyTypeKey = "type";

        /// <summary>
        /// Gets or sets the modification type to make to any existing or new content within a pdf file.
        /// </summary>
        [PDFAttribute(ModifyTypeKey)]
        public ModificationType ModificationType
        {
            get
            {
                ModificationType value;
                return this.TryGetValue(PDFStyleKeys.ModifyPageTypeKey, out value) ? value : ModificationType.None;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ModifyPageTypeKey, value);
            }
        }

        public void RemoveModificationType()
        {
            this.RemoveValue(PDFStyleKeys.ModifyPageTypeKey);
        }

        #endregion

        #region public ModifiedContentAction ContentRetention {get;set;}

        private const string ContentActionKey = "content-action";

        [PDFAttribute(ContentActionKey)]
        public ModifiedContentAction ContentAction
        {
            get
            {
                ModifiedContentAction value;
                return this.TryGetValue(PDFStyleKeys.ModifyPageActionKey, out value) ? value : ModifiedContentAction.OnTop;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ModifyPageActionKey, value);
            }
        }

        public void RemoveContentAction()
        {
            this.RemoveValue(PDFStyleKeys.ModifyPageActionKey);
        }

        #endregion

        #region public int PageStartIndex {get;set;}

        private const string PageIndexKey = "page-index";

        [PDFAttribute(PageIndexKey)]
        public int PageStartIndex
        {
            get
            {
                int value;
                return this.TryGetValue(PDFStyleKeys.ModifyPageStartIndexKey, out value) ? value : 0;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ModifyPageStartIndexKey, value);
            }

        }

        public void RemovePageStartIndex()
        {
            this.RemoveValue(PDFStyleKeys.ModifyPageStartIndexKey);
        }

        #endregion

        #region public int PageCount {get;set;}

        private const string PageCountKey = "page-count";

        [PDFAttribute(PageCountKey)]
        public int PageCount
        {
            get
            {
                int value;
                return this.TryGetValue(PDFStyleKeys.ModifyPageCountKey, out value) ? value : 1;
            }
            set
            {
                this.SetValue(PDFStyleKeys.ModifyPageCountKey, value);
            }

        }

        public void RemovePageCount()
        {
            this.RemoveValue(PDFStyleKeys.ModifyPageCountKey);
        }

        #endregion


        //
        // .ctor
        //

        public PDFModifyPageStyle()
            : base(PDFStyleKeys.ModifyPageItemKey)
        {
        }
    }
}
