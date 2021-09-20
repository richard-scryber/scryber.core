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
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Components;

namespace Scryber.Layout
{
    public class LayoutEngineList2 : LayoutEnginePanel
    {
        //
        // const
        //

        private const string ListEngineLogCategory = "List Layout Engine";
        private static readonly PDFUnit DefaultListItemAlley = 10;
        public static readonly PDFUnit DefaultNumberWidth = Const.DefaultListNumberInset;
        public const HorizontalAlignment DefaultListItemAlignment = HorizontalAlignment.Right;

        #region ivars

        private List _list;
        private string _grpname;

        #endregion


        #region protected PDFList List {get;}

        /// <summary>
        /// Gets the list this engin is laying out
        /// </summary>
        protected List List
        {
            get { return _list; }
        }

        #endregion

        public PDFUnit ItemNumberWidth
        {
            get; set;
        }

        public PDFUnit ItemAlleyWidth
        {
            get; set;
        }

        public HorizontalAlignment ItemAlignment
        {
            get; set;
        }

        public string GroupName
        {
            get { return this._grpname; }
            protected set { this._grpname = value; }
        }


        //
        // .ctor
        //

        #region public LayoutEngineList(PDFList list, IPDFLayoutEngine parent)

        public LayoutEngineList2(List list, IPDFLayoutEngine parent)
            : base(list, parent)
        {
            _list = list;
        }

        #endregion

        //
        // main override
        //

        #region protected override void DoLayoutComponent()

        /// <summary>
        /// Performs the actual layout of the list and items in it.
        /// </summary>
        protected override void DoLayoutComponent()
        {
            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.Begin(TraceLevel.Debug, ListEngineLogCategory, string.Format("Starting the layout of the list {0}", this.List.ID));

            this.OpenListNumbering();

            base.DoLayoutComponent();
            
            this.CloseListNumbering();
        }

        #endregion

        //
        // open and close the list numbering
        //

        #region private void OpenListNumbering()

        private void OpenListNumbering()
        {
            string groupname = string.Empty;
            StyleValue<string> grp;

            if (this.FullStyle.TryGetValue(StyleKeys.ListGroupKey,out grp))
            {
                groupname = grp.Value(this.FullStyle);
            }
            PDFListNumbering numbering = this.Component.Document.ListNumbering;

            numbering.PushGroup(groupname, this.FullStyle);
            this.GroupName = groupname;

        }

        #endregion

        #region private void CloseListNumbering()

        /// <summary>
        /// Closes the current document list numbering
        /// </summary>
        private void CloseListNumbering()
        {
            if(this.Component.Document.ListNumbering.HasCurrentGroup)
                this.Component.Document.ListNumbering.PopGroup();
            this.GroupName = string.Empty;
        }

        #endregion


        protected override void DoLayoutAChild(IPDFComponent comp, Style full)
        {
            if(comp is ListItem)
            {
                //PDFUnit inset = full.GetValue(StyleKeys.ListInsetKey, DefaultNumberWidth);
                
                

                //var li = BuildAListNumberComponent(comp as ListItem, full);

                //if (li != null)
                //{
                //    PDFUnit left = li.NumberWidth;
                //    left += DefaultListItemAlley;
                //    full.SetValue(StyleKeys.MarginsLeftKey, left);
                //}

            }
            base.DoLayoutAChild(comp, full);
        }



        #region private PDFComponent BuildAListNumberComponent(PDFListItem item, PDFStyle itemstyle ....)

        /// <summary>
        /// Based on the current numbering creates and returns the appropriate ListItemNumberComponent along with a few other appropriate bits.
        /// </summary>
        /// <param name="item">The list item to build the Number component for</param>
        /// <param name="itemstyle">The full style of this list item</param>
        /// <param name="halign">The alignment as set, can be updated to explict item alignment</param>
        /// <param name="type">The current type of numbering</param>
        /// <param name="itemWidth">If an explicit width has been set then this is returned (otherwise -1)</param>
        /// <param name="text">If this item has explict text (e.g Definiton list) then this is returned, otherwise empty</param>
        /// <returns>The correct PDFListItemLabel for the item</returns>
        private ListItemLabel BuildAListNumberComponent(ListItem item, Style itemstyle)
        {
            PDFListNumbering numbers = this.Component.Document.ListNumbering;
            PDFListNumberGroup group = numbers.CurrentGroup;

            var type = numbers.CurrentGroup.Style;

            var itemWidth = itemstyle.GetValue(StyleKeys.ListInsetKey, DefaultNumberWidth);
            var text = itemstyle.GetValue(StyleKeys.ListLabelKey, string.Empty);
            var halign = itemstyle.GetValue(StyleKeys.ListAlignmentKey, DefaultListItemAlignment);

            ListItemLabel label;

            if (type == ListNumberingGroupStyle.None)
                label = null;
            else
            {
                if (type == ListNumberingGroupStyle.Labels)
                {
                    PDFListDefinitionItemLabel defn = new PDFListDefinitionItemLabel();
                    defn.Text = text;
                    label = defn;
                }
                else if (type == ListNumberingGroupStyle.Bullet)
                {
                    label = new PDFListBulletItemLabel();
                }
                else
                {
                    label = new ListItemLabel();
                }

                label.StyleClass = item.StyleClass;
                label.Alignment = halign;
                label.ListType = type;
                label.Group = group;
                label.NumberWidth = itemWidth;

            }
            return label;
        }


        #endregion

        //
        // inner classes
        //

        #region private class ListNumberEntry

        /// <summary>
        /// Encapsulates the details about a single list entry
        /// </summary>
        private class ListNumberEntry
        {
            /// <summary>
            /// The component to use for the numbering region
            /// </summary>
            public Component NumberComponent;

            /// <summary>
            /// The layout width of the number region
            /// </summary>
            public PDFUnit NumberWidth;

            /// <summary>
            /// The list item this entry holds the details for
            /// </summary>
            public ListItem ListItem;

            /// <summary>
            /// The list item full style
            /// </summary>
            public Style FullStyle;

            /// <summary>
            /// The list item applied style
            /// </summary>
            public Style AppliedStyle;

            /// <summary>
            /// The index of the item in the list
            /// </summary>
            public int NumberIndex;

            /// <summary>
            /// The numbering group style (UppercaseRoman, Decimal, Bullet etc).
            /// </summary>
            public ListNumberingGroupStyle NumberGroupStyle;

            /// <summary>
            /// The alignment of the number component in its region
            /// </summary>
            public HorizontalAlignment NumberAlignment;

            /// <summary>
            /// The actual text associated with the number.
            /// </summary>
            public string NumberText;
        }

        #endregion

    }
}
