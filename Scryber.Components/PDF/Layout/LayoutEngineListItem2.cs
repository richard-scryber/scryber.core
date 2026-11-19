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
            Unit inset = Unit.Zero;

            //When laying out an item we add an absolutely positioned number block, before the content of the item,
            //to the current child collection.

            if(this.Component.Document.ListNumbering.HasCurrentGroup && null != this.ParentListEngine)
            {
                var nums = this.Component.Document.ListNumbering.CurrentGroup;
                var style = this.FullStyle;
                var explicitStyle = style.GetValue(StyleKeys.ListNumberStyleKey, ListNumberingGroupStyle.Bullet);
                if (nums.Style != ListNumberingGroupStyle.None && explicitStyle != ListNumberingGroupStyle.None)
                {
                    //As css, the indent of the items is the padding of the list (30pt by default), and the marker
                    //is outside the content of the item - the number box (the inset wide) ends the alley before the text.
                    if (style.IsValueDefined(StyleKeys.ListInsetKey))
                        inset = style.GetValue(StyleKeys.ListInsetKey, DefaultNumberWidth);
                    else
                    {
                        //by default the number box reaches back 30pt, or further for a large font so the number or marker fits
                        var fontInset = new Unit(this.GetFontSize(style).PointsValue * DefaultNumberWidthFactor, PageUnits.Points);
                        inset = fontInset > DefaultNumberWidth ? fontInset : DefaultNumberWidth;
                    }

                    Unit alley;
                    if (style.IsValueDefined(StyleKeys.ListAlleyKey))
                        alley = style.GetValue(StyleKeys.ListAlleyKey, Unit.Zero);
                    else
                        alley = this.GetDefaultAlley(style);

                    var comp = BuildAListNumberComponent(this.ListItem, inset, alley, nums);
                    this.LabelComponent = comp;
                }
            }

            base.DoLayoutBlockComponent(position, columnOptions);

        }

        private static bool IsMarkerShape(ListNumberingGroupStyle type)
        {
            return type == ListNumberingGroupStyle.Disc
                   || type == ListNumberingGroupStyle.Circle
                   || type == ListNumberingGroupStyle.Square;
        }

        /// <summary>
        /// Returns the marker for an automatic bullet from the number of lists the list of the item is in -
        /// a disc, then a circle, then a square (as the browser default style sheet).
        /// </summary>
        private static ListNumberingGroupStyle GetBulletForDepth(ListItem item)
        {
            int lists = 0;
            var parent = item.Parent;
            while (null != parent)
            {
                if (parent is ListBase)
                    lists++;
                parent = parent.Parent;
            }

            //the first list is the list of the item
            if (lists <= 1)
                return ListNumberingGroupStyle.Disc;
            else if (lists == 2)
                return ListNumberingGroupStyle.Circle;
            else
                return ListNumberingGroupStyle.Square;
        }

        /// <summary>
        /// The distance from the top of a line of the text of the item to its baseline, so a marker can be drawn on the same line as the text.
        /// </summary>
        private Unit GetTextBaselineOffset(Style style)
        {
            var options = style.CreateTextOptions();
            if (null == options.Font || !(this.Context.Document is Document doc))
                return Unit.Zero;

            if (null == options.Font.FontMetrics)
            {
                var resource = doc.GetFontResource(options.Font, true);
                if (null != resource && null != resource.Definition)
                    options.Font.FontMetrics = resource.Definition.GetFontMetrics(options.Font.Size);
            }
            return options.GetBaselineOffset();
        }

        private Unit GetFontSize(Style style)
        {
            return style.GetValue(StyleKeys.FontSizeKey, new Unit(Font.DefaultFontSize, PageUnits.Points));
        }

        /// <summary>
        /// The default space between the marker and the text, about the width of a space in the font of the item
        /// (as the space after the marker in a browser).
        /// </summary>
        private Unit GetDefaultAlley(Style style)
        {
            var size = this.GetFontSize(style);
            return new Unit(size.PointsValue * DefaultListItemAlleyFactor, PageUnits.Points);
        }

        /// <summary>
        /// The default space between the marker and the text, as a factor of the font size.
        /// </summary>
        public const double DefaultListItemAlleyFactor = 0.3;

        /// <summary>
        /// The least the number box reaches back from the text, as a factor of the font size (30pt at 12pt)
        /// </summary>
        public const double DefaultNumberWidthFactor = 2.5;

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
            else if(prevRegion.Contents.Count > 0)
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
            //the number is in the space the inset reaches back from the text, and ends the alley before the text
            var width = label.NumberWidth - label.AlleyWidth;
            if (width < Unit.Zero)
                width = Unit.Zero;
            panel.Width = width;
            panel.TextWrapping = Text.WordWrap.NoWrap;
            panel.OverflowAction = OverflowAction.Clip;
            //the space between the number and the text is the alley
            panel.HorizontalAlignment = label.Alignment;
            panel.PositionMode = PositionMode.Absolute;
            panel.X = -label.NumberWidth.PointsValue;
            panel.Y = 0;
            //panel.Height = 20;
            //panel.OverflowSplit = OverflowSplit.Never;
            panel.ID = (item.ID ?? "no_id") + "_Num";
            
            if (label is PDFListBulletItemLabel && IsMarkerShape(label.ListType))
                panel.Contents.Add(new ListItemMarker(label.ListType, this.GetFontSize(this.FullStyle), this.GetTextBaselineOffset(this.FullStyle)) { ID = panel.ID + "_Marker" });
            else
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
                else if (type.IsBullet())
                {
                    label = new PDFListBulletItemLabel();
                    label.Text = item.Document.ListNumbering.Increment();

                    //the marker is drawn as a shape - an automatic bullet is a disc, a circle in a nested list,
                    //and a square when nested deeper (as the browser default styles).
                    if (type == ListNumberingGroupStyle.Bullet)
                        type = GetBulletForDepth(item);
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
