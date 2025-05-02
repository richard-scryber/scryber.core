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
using Scryber.Data;

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
        private Panel _lblPanel;

        #endregion


        #region protected PDFList List {get;}

        /// <summary>
        /// Gets the list this engin is laying out
        /// </summary>
        protected ListItem ListItem
        {
            get { return _item; }
        }

        /// <summary>
        /// Gets the parent List engine for this item engine
        /// </summary>
        protected LayoutEngineList2 ParentListEngine
        {
            get { return _engine; }
        }

        /// <summary>
        /// Gets the list item label that has been injected into the contents
        /// </summary>
        public ListItemLabel LabelComponent
        {
            get { return _lbl; }
            protected set { _lbl = value; }
        }

        /// <summary>
        /// Gets the panel that contains the label, that is positioned absolutely within the page.
        /// </summary>
        public Panel LabelPanel
        {
            get { return _lblPanel; }
            protected set { _lblPanel = value; }
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
            Thickness margins = Thickness.Empty();
            Unit inset = Unit.Zero;

            //When laying out an item we inset the content by the required amount and then add a
            //relatively positioned number block to the current child collection.

            if(this.Component.Document.ListNumbering.HasCurrentGroup && null != this.ParentListEngine)
            {
                var nums = this.Component.Document.ListNumbering.CurrentGroup;
                var style = this.FullStyle;
                var explicitStyle = style.GetValue(StyleKeys.ListNumberStyleKey, ListNumberingGroupStyle.Bullet);
                if (nums.Style != ListNumberingGroupStyle.None && explicitStyle != ListNumberingGroupStyle.None)
                {
                    margins = position.Margins;
                    var left = margins.Left;

                    inset = style.GetValue(StyleKeys.ListInsetKey, DefaultNumberWidth);
                    var alley = style.GetValue(StyleKeys.ListAlleyKey, DefaultListItemAlley);

                    //Add the extra space for the list item number
                    margins.Left += inset + alley;
                    style.Margins.Left = margins.Left;
                    position.Margins = margins;

                    var comp = BuildAListNumberComponent(this.ListItem, inset, alley, nums);
                    this.LabelComponent = comp;
                    
                    inset += alley;
                }
            }

            base.DoLayoutBlockComponent(position, columnOptions);

        }

        protected override void DoLayoutChildren()
        {
            Panel num = null;
            if (null != this.LabelComponent)
            {
                num = this.AddLabelToBlock(this.LabelComponent, this.ListItem);
                this.LabelPanel = num;
            }

            base.DoLayoutChildren();

            if (null != num)
                this.ListItem.Contents.Remove(num);
        }

        protected internal override bool MoveToNextRegion(Unit requiredHeight, ref PDFLayoutRegion region, ref PDFLayoutBlock block, out bool newPage)
        {
            var prevRegion = region;
            var prevBlock = block;
            
            bool success = base.MoveToNextRegion(requiredHeight, ref region, ref block, out newPage);
            if (!success)
                this.ContinueLayout = false;
            else
            {
                var line = prevRegion.Contents[prevRegion.Contents.Count - 1] as PDFLayoutLine;
                if (null != line)
                {
                    var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
                    //if we are the last line, and we have the 
                    if (null != posRun && posRun.Owner == this.LabelPanel)
                    {
                        try
                        {
                            //Move pos run and associated positioned region to the new region
                            posRun.Line.Runs.Remove(posRun);
                            
                            //if we leave an empty line, then remove it.
                            if (line.Runs.Count == 2 && line.Runs[0] is PDFTextRunBegin &&
                                line.Runs[1] is PDFTextRunEnd)
                                line.Region.Contents.Remove(line);
                            
                            //if the original containing region is empty, and we have just one region (column) then save to remove the block as well
                            if (prevRegion.Contents.Count == 0 && prevBlock.Columns.Length == 1 && prevBlock.Parent is PDFLayoutBlock parentBlock)
                                parentBlock.CurrentRegion.Contents.Remove(prevBlock);

                            //take the positioned region off the old block
                            prevBlock.PositionedRegions.Remove(posRun.Region);

                            //now add it all back in on a line in the new region with the positioned block
                            //And make that relative to the new block.
                            if (region.CurrentItem == null)
                            {
                                line = region.BeginNewLine(line.Height.PointsValue);
                            }
                            else
                            {
                                line = (PDFLayoutLine)region.CurrentItem;
                            }

                            line.AddRun(posRun);
                            posRun.SetParent(line);
                            block.PositionedRegions.Add(posRun.Region);

                            posRun.Region.SetParent(block);
                            (posRun.Region as PDFLayoutPositionedRegion).RelativeTo = block;
                        }
                        catch (Exception ex)
                        {
                            if(this.Context.Conformance == ParserConformanceMode.Strict)
                                throw new PDFLayoutException("Attempted to clean up the previous regions content with the list item block, but an error occurred - " + ex.Message, ex);
                            else
                            this.Context.TraceLog.Add(TraceLevel.Error, "ListItem", "Attempted to clean up the previous regions content with the list item block, but an error occurred - " + ex.Message, ex);
                        }
                    }
                }
            }
            return success;
        }

        protected virtual Panel AddLabelToBlock(ListItemLabel label, ListItem item)
        {
            Panel panel = new Panel();
            panel.Width = label.NumberWidth;
            panel.TextWrapping = Text.WordWrap.NoWrap;
            panel.OverflowAction = OverflowAction.Clip;
            panel.Padding = new Thickness(0, 2, 0, 0); // add a little padding for space.
            panel.HorizontalAlignment = label.Alignment;
            panel.PositionMode = PositionMode.Absolute;
            panel.X = -(label.NumberWidth.PointsValue + label.AlleyWidth.PointsValue);
            panel.Y = 0;
            //panel.Height = 20;
            //panel.OverflowSplit = OverflowSplit.Never;
            panel.ID = (item.ID ?? "no_id") + "_Num";
            
            panel.Contents.Add(label);
            item.Contents.Insert(0, panel);
            
            // if (item.Contents.Count > 0)
            //     item.Contents.Insert(0, panel);
            // else
            //     item.Contents.Add(panel);

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

            if (this.FullStyle.IsValueDefined(StyleKeys.ListNumberStyleKey))
            {
                type = this.FullStyle.GetValue(StyleKeys.ListNumberStyleKey, type);
            }

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
