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
using System.Text;
using Scryber.Native;
using Scryber.Styles;

namespace Scryber.Components
{
    /// <summary>
    /// The PageNumber component outputs the number of the 
    /// page that the component is placed on in the format specified.
    /// </summary>
    [PDFParsableComponent("PageNumber")]
    [PDFJSConvertor("scryber.studio.design.convertors.pageNumber")]
    public class PageNumberLabel : TextBase
    {

        #region public string PageLabelFormat 

        /// <summary>
        /// Gets or sets the format of the string to be displayed. Using the indexes of the page numbering data
        /// </summary>
        [PDFAttribute("display-format", PDFStyle.PDFStylesNamespace)]
        [PDFDesignable("Format", Category = "General", Priority = 4, Type = "PageFormat")]
        public string DisplayFormat 
        { 
            get { return this.Style.PageStyle.PageNumberFormat; }
            set { this.Style.PageStyle.PageNumberFormat = value; }
        }

        #endregion

        #region public int TotalPageCountHint {get;set;}

        /// <summary>
        /// A value that estimates the total page count in this document
        /// </summary>
        /// <remarks>Becasue we do not know the total page count until layout has completed 
        /// we need something to tell us what length the total page count will be. 
        /// This is the hint.</remarks>
        [PDFAttribute("total-count-hint", PDFStyle.PDFStylesNamespace)]
        public int TotalPageCountHint
        {
            get { return this.Style.PageStyle.PageTotalCountHint; }
            set { this.Style.PageStyle.PageTotalCountHint = value; }
        }

        #endregion


        #region public int GroupPageCountHint {get;set;}

        /// <summary>
        /// A hint to the final number of pages in the group for this page number label
        /// </summary>
        /// <remarks>Becasue we do not know the total page count until layout has completed 
        /// we need something to tell us what length the total page count will be. 
        /// This is the hint and the default value is %%</remarks>
        [PDFAttribute("group-count-hint", PDFStyle.PDFStylesNamespace)]
        public int GroupPageCountHint
        {
            get { return this.Style.PageStyle.PageGroupCountHint; }
            set { this.Style.PageStyle.PageGroupCountHint = value; }
        }

        #endregion

        //local reference to the layout document
        private Layout.PDFLayoutDocument _doc;

        //local value for the page index of this label
        private int _renderpageindex = -1;

        //local reference to the full style of this label
        private PDFStyle _fullstyle = null;

        //local reference to the full style of the layout page
        private PDFStyle _pgstyle = null;
        
        //The text proxy op who's text will be replaced with the page number on render complete.
        Scryber.Text.PDFTextProxyOp _numberProxy;

        protected override string BaseText
        {
            get
            {
                return this.GetDisplayText(false);
            }
            set
            {
                throw new InvalidOperationException(Errors.CannotSetBaseTextOfPageNumber);
                //Do Nothing
            }
        }

        /// <summary>
        /// Gets the actual text proxy for this page number.
        /// </summary>
        public Scryber.Text.PDFTextProxyOp Proxy
        {
            get { return _numberProxy; }
        }


        public PageNumberLabel()
            : base(PDFObjectTypes.Text)
        {
        }

        /// <summary>
        /// Overrides the base text reader to create a new array of operations that has one op - a proxy that this component will update one the layout is completed
        /// </summary>
        /// <param name="context"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        protected override Text.PDFTextReader CreateReader(PDFLayoutContext context, Styles.PDFStyle style)
        {
            _doc = context.DocumentLayout;
            _renderpageindex = _doc.CurrentPageIndex;
            _fullstyle = style;
            _pgstyle = context.DocumentLayout.CurrentPage.FullStyle;

            string text = this.GetDisplayText(_renderpageindex, style, false);
            Scryber.Text.PDFTextProxyOp op = new Text.PDFTextProxyOp(this, "PageNumber", text);
            Scryber.Text.PDFArrayTextReader array = new Text.PDFArrayTextReader(new Text.PDFTextOp[] { op });
            _numberProxy = op;
            

            return array;
        }

        /// <summary>
        /// Once layout is comlete then we can replace the text that was used when not rendering
        /// with the text that was.
        /// </summary>
        internal override void RegisterLayoutComplete(PDFLayoutContext context)
        {
            base.RegisterLayoutComplete(context);

            PDFComponentArrangement arrange = this.GetFirstArrangement();
            if (null != arrange)
            {
                this._renderpageindex = arrange.PageIndex;
                this._fullstyle = arrange.FullStyle;
                this._pgstyle = context.DocumentLayout.CurrentPage.FullStyle;
            }

            if (null != this._doc && null != this._numberProxy)
            {
                string text = this.GetDisplayText(this._renderpageindex, this._fullstyle, true);
                this._numberProxy.Text = text;
            }
        }


        #region private string GetDisplayText(bool rendering)

        private const int DefaultGroupPageCountHint = 10;
        private const int DefaultTotalPageCountHint = 99;

        private string GetDisplayText(bool rendering)
        {
            if (null == this._doc)
                return string.Empty;

            var text = this.GetDisplayText(this._renderpageindex, this._fullstyle, rendering);
            return text;
        }

        /// <summary>
        /// Gets the full text for the page label
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <remarks>
        /// 1. If there is no PageFormat, then the current page index in the label style for this page is used
        /// 2. If there is a format then the format is used with the following fields...
        ///                     {0} = page label, {1} = total label for this pages numbering group, 
        ///                     {2} global page index, {3} global page count,
        ///                     {4} index in group, {5} group count
        /// </remarks>
        private string GetDisplayText(int pageindex, PDFStyle style, bool rendering)
        {
            if (null == this._doc)
                throw new ArgumentNullException("The PageNumberLabel does not have a layout document associated with it, so cannot get the page number in the document");

            PDFPageNumberData num = this._doc.GetNumbering(pageindex);

            if (null == num)
                throw new NullReferenceException("No numbering data was returned for the specified page index '" + pageindex + "'");

            string format = GetPageFormat(style);

            //If we just want the page label - we can skip the rest and return
            if (string.IsNullOrEmpty(format))
                return num.Label;


            
            //If we are not rendering then we don't actually know the group or total page count
            //So we use the hints from the style or our default values
            if (!rendering)
            {
                int grp = style.GetValue(PDFStyleKeys.PageNumberGroupHintKey, DefaultGroupPageCountHint);
                num.GroupLastNumber = grp;
                num.LastPageNumber = style.GetValue(PDFStyleKeys.PageNumberTotalHintKey, DefaultTotalPageCountHint);
                num.LastLabel = num.GroupOptions.GetPageLabel(grp);
            }

            return num.ToString(format);
        }

        #endregion

        private string GetPageFormat(PDFStyle full)
        {
            string format = this.Style.GetValue(PDFStyleKeys.PageNumberFormatKey, string.Empty);

            if (!string.IsNullOrEmpty(format))
                return format;

            format = _pgstyle.GetValue(PDFStyleKeys.PageNumberFormatKey, string.Empty);

            if (!string.IsNullOrEmpty(format))
                return format;

            //Not defined on this component - so search the hierarchy.

            var stack = new List<PDFStyle>();
            var parent = this.Parent;

            while(null != parent)
            {
                if(parent is IPDFStyledComponent)
                {
                    var styled = (IPDFStyledComponent)parent;
                    if (styled.HasStyle)
                        stack.Add(styled.Style);
                }
                parent = parent.Parent;

            }

            foreach (var style in stack)
            {
                PDFStyleValue<string> val;
                if (style.TryGetValue(PDFStyleKeys.PageNumberFormatKey, out val))
                    format = val.Value;
            }

            return format;

        }

    }
}
