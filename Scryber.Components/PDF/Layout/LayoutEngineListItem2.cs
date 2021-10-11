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

namespace Scryber.PDF.Layout
{
    public class LayoutEngineListItem2 : LayoutEnginePanel
    {
        //
        // const
        //

        private const string ListEngineLogCategory = "List Item Layout Engine";
        private static readonly Unit DefaultListItemAlley = 10;
        public static readonly Unit DefaultNumberWidth = Const.DefaultListNumberInset;
        public const HorizontalAlignment DefaultListItemAlignment = HorizontalAlignment.Right;

        #region ivars

        private ListItem _item;
        private LayoutEngineList2 _engine;
        private ListItemLabel _lbl;

        #endregion


        #region protected PDFList List {get;}

        /// <summary>
        /// Gets the list this engin is laying out
        /// </summary>
        protected ListItem ListItem
        {
            get { return _item; }
        }

        protected LayoutEngineList2 ParentListEngine
        {
            get { return _engine; }
        }

        public ListItemLabel LabelComponent
        {
            get { return _lbl; }
            protected set { _lbl = value; }
        }


        #endregion

        //
        // .ctor
        //

        #region public LayoutEngineList(PDFList list, IPDFLayoutEngine parent)

        public LayoutEngineListItem2(ListItem item, IPDFLayoutEngine parent)
            : base(item, parent)
        {
            _item = item;
            _engine = GetListEngine(parent);
        }

        #endregion


        private LayoutEngineList2 GetListEngine(IPDFLayoutEngine engine)
        {
            if (engine is LayoutEngineList2 list)
                return list;
            else if (null == engine.ParentEngine)
                return null;
            else
                return GetListEngine(engine.ParentEngine);
        }
        //
        // main override
        //

        #region protected override void DoLayoutComponent()

        /// <summary>
        /// Performs the actual layout of the list and items in it.
        /// </summary>
        protected override void DoLayoutComponent()
        {
            base.DoLayoutComponent();
        }

        #endregion


        protected override void DoLayoutBlockComponent(PDFPositionOptions position, PDFColumnOptions columnOptions)
        {
            Thickness padding = Thickness.Empty();
            Unit inset = Unit.Zero;

            //When laying out an item we inset the content by the required amount and then add a
            //relatively positioned number block to the current child collection.

            if(this.Component.Document.ListNumbering.HasCurrentGroup && null != this.ParentListEngine)
            {
                var nums = this.Component.Document.ListNumbering.CurrentGroup;
                var style = this.FullStyle;
                padding = position.Padding;
                var left = padding.Left;

                inset = style.GetValue(StyleKeys.ListInsetKey, DefaultNumberWidth);
                var alley = style.GetValue(StyleKeys.ListAlleyKey, DefaultListItemAlley);

                //Add the extra space for the list item number
                padding.Left += inset + alley;
                style.Padding.Left = padding.Left;
                position.Padding = padding;

                var comp = BuildAListNumberComponent(this.ListItem, inset, alley, nums);
                this.LabelComponent = comp;

                inset += alley;
            }

            base.DoLayoutBlockComponent(position, columnOptions);

        }

        protected override void DoLayoutChildren()
        {
            Panel num = null;
            if (null != this.LabelComponent)
                num = this.AddLabelToBlock(this.LabelComponent, this.ListItem);

            base.DoLayoutChildren();

            if (null != num)
                this.ListItem.Contents.Remove(num);
        }

        protected internal override bool MoveToNextRegion(Unit requiredHeight, ref PDFLayoutRegion region, ref PDFLayoutBlock block, out bool newPage)
        {
            bool success = base.MoveToNextRegion(requiredHeight, ref region, ref block, out newPage);
            if (!success)
                this.ContinueLayout = false;
            return success;
        }

        protected virtual Panel AddLabelToBlock(ListItemLabel label, ListItem item)
        {
            Panel panel = new Panel();
            panel.Width = label.NumberWidth;
            panel.HorizontalAlignment = label.Alignment;
            panel.PositionMode = PositionMode.Relative;
            panel.X = -(label.NumberWidth.PointsValue + label.AlleyWidth.PointsValue);
            panel.Y = 0;
            panel.OverflowSplit = OverflowSplit.Never;
            panel.ID = (item.ID ?? "no_id") + "_Num";
            
            panel.Contents.Add(label);

            item.Contents.Insert(0, panel);

            return panel;
        }

        protected override void PushBlockStackOntoNewRegion(Stack<PDFLayoutBlock> stack, PDFLayoutBlock tomove, PDFLayoutBlock current, PDFLayoutRegion currentRegion, ref PDFLayoutRegion region, ref PDFLayoutBlock block)
        {
            base.PushBlockStackOntoNewRegion(stack, tomove, current, currentRegion, ref region, ref block);
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
        private ListItemLabel BuildAListNumberComponent(ListItem item, Unit itemWidth, Unit alleyWidth, PDFListNumberGroup group)
        {
            

            var type = group.Style;

            var text = this.FullStyle.GetValue(StyleKeys.ListLabelKey, string.Empty);
            var halign = this.FullStyle.GetValue(StyleKeys.ListAlignmentKey, DefaultListItemAlignment);

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
                    label.Text = item.Document.ListNumbering.Increment();
                }
                else
                {
                    label = new ListItemLabel();
                    label.Text = item.Document.ListNumbering.Increment();
                }

                label.Alignment = halign;
                label.ListType = type;
                label.Group = group;
                label.NumberWidth = itemWidth;
                label.AlleyWidth = alleyWidth;

            }
            return label;
        }


        #endregion

        

    }
}
