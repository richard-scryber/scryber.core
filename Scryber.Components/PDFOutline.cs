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
using Scryber.Drawing;
using Scryber.Components;
 
namespace Scryber
{
    /// <summary>
    /// An outline appears in the navigation tree of the PDF reader application. 
    /// Any component that has an outline text specified will be linked from the outline tree in the viewer
    /// </summary>
    [PDFParsableComponent("Outline")]
    public class PDFOutline : IPDFBindableComponent
    {

        #region Databinding events

        public event PDFDataBindEventHandler DataBinding;

        public event PDFDataBindEventHandler DataBound;

        protected virtual void OnDataBinding(PDFDataContext context)
        {
            if (null != this.DataBinding)
                this.DataBinding(this, new PDFDataBindEventArgs(context));
        }

        protected virtual void OnDataBound(PDFDataContext context)
        {
            if (null != this.DataBound)
                this.DataBound(this, new PDFDataBindEventArgs(context));
        }

        #endregion

        #region ivars

        private Component _belongs;
        private string _title = string.Empty;
        private PDFColor _col = null;
        private bool? _boldstyle = null;
        private bool? _italicstyle = null;
        private bool? _open = null;

        #endregion

        //
        // properties
        //

        #region internal PDFComponent BelongsTo {get;set;}
        
        /// <summary>
        /// Gets the component this PDFOutline belongs to
        /// </summary>
        internal Component BelongsTo 
        { 
            get { return _belongs; } 
            set { _belongs = value; }
        }


        #endregion

        #region public string DestinationName {get;}

        /// <summary>
        /// Gets the name of the destination in the outline dictionary - 
        /// by default this is the unique id of the component this outline belongs to
        /// </summary>
        public string DestinationName
        {
            get
            {
                if (null == _belongs)
                    return string.Empty;
                else if (!string.IsNullOrEmpty(_belongs.Name))
                    return _belongs.Name;
                else
                    return _belongs.UniqueID;
            }
        }

        #endregion

        #region public string Title {get;set;}

        /// <summary>
        /// Gets or sets the title of this Outline item
        /// </summary>
        [PDFAttribute("title")]
        public string Title 
        {
            get { return _title; }
            set { _title = value; }
        }

        #endregion

        #region public PDFColor Color {get;set;}

        /// <summary>
        /// Gets or sets the color (if any) of this outline item
        /// </summary>
        [PDFAttribute("color", Const.PDFStylesNamespace)]
        public PDFColor Color 
        {
            get { return this._col; }
            set { this._col = value; }
        }

        #endregion

        #region public bool FontBold {get;set;} + HasBold {get;}

        /// <summary>
        /// Gets or sets the bold accent on the outline item
        /// </summary>
        [PDFAttribute("bold", Const.PDFStylesNamespace)]
        public bool FontBold
        {
            get { return _boldstyle.HasValue? _boldstyle.Value : false; }
            set { this._boldstyle = value; }
        }



        /// <summary>
        /// Gets whether the FontBold value is set
        /// </summary>
        public bool HasBold
        {
            get { return _boldstyle.HasValue; }
        }

        #endregion

        #region public bool FontItalic {get;set;} + HasItalic {get;}

        /// <summary>
        /// Gets or set the italic accent on the outline item
        /// </summary>
        [PDFAttribute("italic", Const.PDFStylesNamespace)]
        public bool FontItalic
        {
            get { return _italicstyle.HasValue? _italicstyle.Value : false; }
            set { _italicstyle = value; }
        }

        /// <summary>
        /// Gets or sets whether the FontItalic value is set
        /// </summary>
        public bool HasItalic
        {
            get { return _italicstyle.HasValue; }
        }

        #endregion

        #region public bool OutlineOpen {get;set;} + HasOpen {get;}

        /// <summary>
        /// Gets or sets the Open flag on the outline item
        /// </summary>
        [PDFAttribute("open", Const.PDFStylesNamespace)]
        public bool OutlineOpen
        {
            get { return _open.HasValue? _open.Value : false; }
            set { this._open = value; }
        }

        /// <summary>
        /// Gets or sets whether the Open value is set
        /// </summary>
        public bool HasOpen
        {
            get { return _open.HasValue; }
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFOutline()

        /// <summary>
        /// Creates a new PDFOutline instance
        /// </summary>
        public PDFOutline()
        {            

        }

        #endregion

        //
        // Binding
        //

        #region IPDFBindableComponent Members

        
        /// <summary>
        /// Databinds this outline
        /// </summary>
        /// <param name="context"></param>
        public void DataBind(PDFDataContext context)
        {
            this.OnDataBinding(context);
            this.DoDataBind(context);
            this.OnDataBound(context);
        }

        protected virtual void DoDataBind(PDFDataContext context)
        {
        }

        #endregion
    }

    


    
}
