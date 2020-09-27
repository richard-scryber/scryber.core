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

namespace Scryber.Styles
{
    /// <summary>
    /// Class is not currently used within the framework - intention to group styles by application with inner styles for components
    /// </summary>
    [PDFParsableComponent("StyleGroup")]
    public class StyleGroup : StyleBase, IEnumerable<StyleBase>
    {
        //
        // properties
        //

        #region public string ID {get;set;}

        private string _id;

        /// <summary>
        /// Gets or sets the ID for this instance
        /// </summary>
        [PDFAttribute("id")]
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(_id))
                {
                    _id = string.Empty;
                }
                return this._id;
            }
            set
            {
                _id = value;
            }
        }

        #endregion

        #region protected PDFStyleCollection InnerItems

        private StyleCollection _innerItems;

        protected StyleCollection InnerItems
        {
            get 
            {
                if (this._innerItems == null)
                    this._innerItems = CreateInnerStyles();
                return this._innerItems;
            }
            set
            {
                this._innerItems = value;
            }
        }

        #endregion

        #region public PDFStyleCollection Styles

        /// <summary>
        /// Gets all the styles in this group
        /// </summary>
        [PDFElement()]
        [PDFArray(typeof(StyleBase))]
        public virtual StyleCollection Styles
        {
            get { return this.InnerItems; }
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFStyleGroup()

        /// <summary>
        /// Creates a new instance of the style group
        /// </summary>
        public StyleGroup()
            : this(PDFObjectTypes.StyleGroup)
        {
        }

        #endregion

        #region protected PDFStyleGroup(PDFObjectType type)

        /// <summary>
        /// Protected constructor that allows pass through of the object type
        /// </summary>
        /// <param name="type"></param>
        protected StyleGroup(PDFObjectType type)
            : base(type)
        {
        }

        #endregion

        //
        // methods
        //

        #region protected virtual PDFStyleCollection CreateInnerStyles()

        /// <summary>
        /// Initializes and returns a new style collection for this group. 
        /// Inheritors can override to implement their own initialization
        /// </summary>
        /// <returns></returns>
        protected virtual StyleCollection CreateInnerStyles()
        {
            return new StyleCollection();
        }

        #endregion

        #region protected virtual void ClearInnerStyles()

        /// <summary>
        /// Removes all inner styles from this group
        /// </summary>
        protected virtual void ClearInnerStyles()
        {
            this._innerItems = null;
        }

        #endregion


        #region IEnumerable<PDFStyleBase> Members

        public IEnumerator<StyleBase> GetEnumerator()
        {
            return this.InnerItems.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        //
        // style base overrides
        //

        #region public override void MergeInto(PDFStyle style, IPDFComponent Component, ComponentState state)

        /// <summary>
        /// Overrides the base implementation to call MergeInto on all the inner styles in this group
        /// </summary>
        /// <param name="style"></param>
        /// <param name="Component"></param>
        /// <param name="state"></param>
        public override void MergeInto(Style style, IPDFComponent Component, ComponentState state)
        {
            foreach (StyleBase def in this.InnerItems)
            {
                def.MergeInto(style, Component, state);
            }
        }

        #endregion

        //#region public override PDFStyle MatchClass(string classname)

        ///// <summary>
        ///// Overrides the base implementation to call MatchClass on the inner items of this group
        ///// </summary>
        ///// <param name="classname"></param>
        ///// <returns></returns>
        //public override PDFStyle MatchClass(string classname)
        //{
        //    return this.InnerItems.MatchClass(classname);
        //}

        //#endregion

        #region protected override void DoDataBind(PDFDataContext context, bool includechildren)

        /// <summary>
        /// Overrides the base implementation to databind the inner items of this group
        /// </summary>
        /// <param name="context"></param>
        /// <param name="includechildren"></param>
        protected override void DoDataBind(PDFDataContext context, bool includechildren)
        {
            base.DoDataBind(context, includechildren);
            if (includechildren && this.InnerItems != null && this.InnerItems.Count > 0)
            {
                foreach (StyleBase sb in this.InnerItems)
                {
                    if (sb is IPDFBindableComponent)
                        ((IPDFBindableComponent)sb).DataBind(context);
                }
            }
        }

        #endregion
    }
}
