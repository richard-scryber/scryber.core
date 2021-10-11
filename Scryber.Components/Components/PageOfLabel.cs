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
using Scryber.PDF.Native;
using Scryber.Styles;
using Scryber.PDF.Layout;
using Scryber.PDF;

namespace Scryber.Components
{
    /// <summary>
    /// Renders the page index of a referenced component.
    /// </summary>
    [PDFParsableComponent("PageOf")]
    public class PageOfLabel: TextBase
    {
        //The found component the shows the page of.
        private Component _found;

        //local reference to the layout document
        private PDFLayoutDocument _doc;

        //local value for the page index of this label
        private int _componentpageindex = -1;

        //local reference to the full style of this label
        private Style _fullstyle = null;

        //The text proxy op who's text will be replaced with the page number on render complete.
        Scryber.Text.PDFTextProxyOp _numberProxy;


        #region protected override string BaseText {get; set = Exception;}

        /// <summary>
        /// Overrides the base implementation to return the display text for the page the referenced component is on.
        /// Cannot be set.
        /// </summary>
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

        #endregion

        #region public string ComponentName

        private string _compname;

        /// <summary>
        /// Gets or sets the name or ID of component to get the page for. To specify an ID prefix with #
        /// </summary>
        [PDFAttribute("component")]
        public string ComponentName
        {
            get { return _compname; }
            set { _compname = value; }
        }

        #endregion

        #region public string NotFoundText {get;set;}

        private string _notfound = string.Empty;

        /// <summary>
        /// Gets or sets the text that is used if the referenced component
        /// is not found (Lax only) or does not have an arrangement
        /// </summary>
        [PDFAttribute("not-found-replacement")]
        public string NotFoundText
        {
            get { return _notfound; }
            set { _notfound = value; }
        }

        #endregion

        #region public string DisplayFormat {get;set;}

        /// <summary>
        /// Gets or sets the format of the string to be displayed. Using the indexes of the page numbering data
        /// </summary>
        [PDFAttribute("display-format", Style.PDFStylesNamespace)]
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
        [PDFAttribute("total-count-hint", Style.PDFStylesNamespace)]
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
        [PDFAttribute("group-count-hint", Style.PDFStylesNamespace)]
        public int GroupPageCountHint
        {
            get { return this.Style.PageStyle.PageGroupCountHint; }
            set { this.Style.PageStyle.PageGroupCountHint = value; }
        }

        #endregion

        //
        // ctor(s)
        //

        public PageOfLabel()
            : base((ObjectType)"pgof")
        {
        }


        protected override Text.PDFTextReader CreateReader(ContextBase context, Styles.Style fullstyle)
        {
            if (context is PDF.PDFLayoutContext layout)
            {
                _doc = layout.DocumentLayout;

                _found = this.LookupExternalComponent(false, layout, this.ComponentName);
                _componentpageindex = -1;
                if (null != _found)
                {
                    this._componentpageindex = _found.PageLayoutIndex;
                }

                _fullstyle = fullstyle;

                string text = this.GetDisplayText(_componentpageindex, fullstyle, false);
                Scryber.Text.PDFTextProxyOp op = new Text.PDFTextProxyOp(this, "PageOf", text);
                Scryber.Text.PDFArrayTextReader array = new Text.PDFArrayTextReader(new Text.PDFTextOp[] { op });
                _numberProxy = op;

                return array;
            }
            else
                return null;
        }



        /// <summary>
        /// We know that all components now have the blocks laid out and complete
        /// </summary>
        internal override void RegisterLayoutComplete(LayoutContext context)
        {
            base.RegisterLayoutComplete(context);


            this._componentpageindex = -1;

            if(null == _found)
                _found = this.LookupExternalComponent(true, context, this.ComponentName);

            if (null != _found)
            {
                this._componentpageindex = _found.PageLayoutIndex;
            }

            if (null != _numberProxy)
            {
                string text = this.GetDisplayText(this._componentpageindex, _fullstyle, true);
                _numberProxy.Text = text;
            }
        }

        #region private string GetDisplayText(bool rendering)

        private string GetDisplayText(bool rendering)
        {
            return this.GetDisplayText(this._componentpageindex, this._fullstyle, rendering);
        }

        /// <summary>
        /// Gets the full text for the page label that the referenced component is placed on
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetDisplayText(int pageIndex, Style style, bool rendering)
        {
            string empty = this.NotFoundText;
            if (string.IsNullOrEmpty(empty))
                empty = DefaultNotFoundText;

           string formatted;

           //This is caled when we begin to build the text block
           formatted = GetPageNumberText(pageIndex, empty, style, rendering);


            
            return formatted;
        }

        private const int DefaultGroupPageCountHint = 10;
        private const int DefaultTotalPageCountHint = 99;
        private const string DefaultNotFoundText = "??";

        private string GetPageNumberText(int pageIndex, string notfound, Style style, bool rendering)
        {
            if (null == this._doc)
                throw new ArgumentNullException("The PageOfLabel does not have a layout document associated with it, so cannot get the page number in the document");

            PageNumberData nums;
            if (pageIndex < 0)
            {

                nums = _doc.GetNumbering(0);
            }
            else
            {
                nums = _doc.GetNumbering(pageIndex);
            }

            if (null == nums)
                throw new NullReferenceException("No numbers associated with the page at index '" + pageIndex + "'");


            string format = style.GetValue(StyleKeys.PageNumberFormatKey, string.Empty);
            int total = -1;
            int grp = -1;

            

            if (pageIndex < 0) //Not found
            {
                nums.Label = notfound;
                nums.LastLabel = notfound;
            }

            if (string.IsNullOrEmpty(format))
            {
                return nums.Label;
            }

            if (!rendering)
            {
                grp = style.GetValue(StyleKeys.PageNumberGroupHintKey, DefaultGroupPageCountHint);
                total = style.GetValue(StyleKeys.PageNumberTotalHintKey, DefaultTotalPageCountHint);

                nums.LastPageNumber = total;
                nums.GroupLastNumber = grp;

                if (pageIndex >= 0)
                    nums.LastLabel = nums.GroupOptions.GetPageLabel(grp);
                else
                {
                    nums.GroupNumber = grp;
                    nums.LastLabel = string.Empty.PadLeft(grp.ToString().Length + 1, '?');
                }
            }

            string formatted = nums.ToString(format);
            return formatted;

        }

        #endregion


        #region private void LookupExternalComponent(PDFLayoutContext context, string name)

        /// <summary>
        /// Looks for the component with the specified name or ID and sets instance variables appropriately
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        private Component LookupExternalComponent(bool rendering, LayoutContext context, string name)
        {
            Component comp;

            if (name.StartsWith(Const.ComponentIDPrefix))
            {
                name = name.Substring(Const.ComponentIDPrefix.Length);
                comp = this.Document.FindAComponentById(name);
            }
            else
            {
                comp = Document.FindAComponentByName(name);
            }

            if (null == comp && string.IsNullOrEmpty(this.NotFoundText))
            {
                if (!rendering)
                {
                    //don't need it just yet
                }
                if (context.Conformance == ParserConformanceMode.Strict)
                    throw new PDFException(string.Format(Errors.CouldNotFindControlWithID, name));
                else
                    context.TraceLog.Add(TraceLevel.Error, "Page Locator", string.Format(Errors.CouldNotFindControlWithID, this.ComponentName));

            }
            
            return comp;
        }

        #endregion
    }
}
