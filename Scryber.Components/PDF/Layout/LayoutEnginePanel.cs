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
    public class LayoutEnginePanel : LayoutEngineBase
    {



        //
        // ctor
        //

        #region protected LayoutEnginePanel(PDFContainerComponent container, IPDFLayoutEngine parent)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="context"></param>
        /// <param name="fullstyle"></param>
        public LayoutEnginePanel(ContainerComponent container, IPDFLayoutEngine parent)
            : base(container, parent)
        {
            
        }


        #endregion



        protected override void DoLayoutComponent()
        {
            if (this.Context.ShouldLogVerbose)
                this.Context.TraceLog.Begin(TraceLevel.Verbose, "Panel Layout Engine", "Beginning layout of component '" + this.Component + "' as a viewport component");

            PDFPositionOptions pos = this.FullStyle.CreatePostionOptions(this.Context.PositionDepth > 0);
            PDFColumnOptions columns = this.FullStyle.CreateColumnOptions();

            //when measuring the content width (e.g. of a flex item) blocks without a width are the width of their content, rather than filling the space.
            if (this.Context.IsMeasuring && !pos.Width.HasValue)
                pos.FillWidth = false;

            if (pos.DisplayMode != Drawing.DisplayMode.Inline)
            {
                //absolute, fixed, floating and inline-block content without a width is the width of its content
                if (this.ShouldShrinkToContentWidth(pos))
                    this.ShrinkToContentWidth(pos, columns);

                this.DoLayoutBlockComponent(pos, columns);
            }
            else
            {
                PDFLayoutInlineBegin begin = CreateAndAddInlineBegin(pos);

                this.DoLayoutChildren();

                this.CreateAndAddInlineEnd(pos, begin);
            }

            if (this.Context.ShouldLogVerbose)
                this.Context.TraceLog.End(TraceLevel.Verbose, "Panel Layout Engine", "Completed layout of component '" + this.Component + "' as a viewport component");

        }

        #region shrink to the content width

        /// <summary>
        /// The space allowed after the measured width, so the rounding of the width can never make the content wrap.
        /// </summary>
        private const double ShrinkWidthAllowance = 0.001;

        /// <summary>
        /// A large height for the measuring layout, so the content always fits.
        /// </summary>
        private const double ShrinkMeasuringHeight = 100000.0;

        /// <summary>
        /// Returns true if this block is absolute, fixed, floating or inline-block, without a width,
        /// and contains block content - which (laid out on its own) would fill the space available.
        /// Text and inline content already sets the width of the block to the widest line.
        /// </summary>
        protected virtual bool ShouldShrinkToContentWidth(PDFPositionOptions pos)
        {
            if (this.Context.IsMeasuring)
                return false;

            if (pos.Width.HasValue || pos.FillWidth)
                return false;

            bool shrinks = pos.PositionMode == PositionMode.Absolute
                           || pos.PositionMode == PositionMode.Fixed
                           || pos.FloatMode != FloatMode.None
                           || pos.DisplayMode == DisplayMode.InlineBlock;

            if (!shrinks)
                return false;

            if (pos.ColumnCount > 1)
                return false;

            bool hasTable = false;
            bool hasBlock = HasBlockContent(this.Component, ref hasTable);

            //a table sizes its columns from the space available, so its measured width is not the width of its content.
            //Content with a table keeps the space available (as before).
            return hasBlock && !hasTable;
        }

        /// <summary>
        /// Returns true if any of the visible content of the component is laid out as a block (rather than inline, or out of the flow).
        /// </summary>
        private static bool HasBlockContent(IComponent component, ref bool hasTable)
        {
            if (!(component is IContainerComponent container) || !container.HasContent)
                return false;

            bool hasBlock = false;
            foreach (Component child in container.Content)
            {
                if (null == child || !child.Visible)
                    continue;

                if (child is ITextComponent || child is ILayoutBreak)
                    continue;

                if (child is IInvisibleContainer)
                {
                    if (HasBlockContent(child, ref hasTable))
                        hasBlock = true;
                    continue;
                }

                if (child is TableGrid)
                {
                    hasTable = true;
                    return true;
                }

                var applied = child.GetAppliedStyle();
                if (null == applied)
                    continue;

                var display = applied.GetValue(StyleKeys.PositionDisplayKey, DisplayMode.Block);
                if (display == DisplayMode.Inline || display == DisplayMode.InlineBlock || display == DisplayMode.Invisible)
                    continue;

                var mode = applied.GetValue(StyleKeys.PositionModeKey, PositionMode.Static);
                if (mode == PositionMode.Absolute || mode == PositionMode.Fixed)
                    continue;

                if (display == DisplayMode.Table)
                {
                    hasTable = true;
                    return true;
                }

                hasBlock = true;

                //look for a table in the content of the block
                HasBlockContent(child, ref hasTable);
                if (hasTable)
                    return true;
            }
            return hasBlock;
        }

        /// <summary>
        /// Lays out the block in a measuring layout (where blocks without a width are the width of their content),
        /// that is then discarded, and sets the width of the position to the width of the measured block.
        /// So the block is as wide as its widest content, and its blocks fill that width (as css shrink-to-fit).
        /// </summary>
        protected virtual void ShrinkToContentWidth(PDFPositionOptions position, PDFColumnOptions columnOptions)
        {
            var page = this.DocumentLayout.CurrentPage;
            var parentBlock = page.LastOpenBlock();
            var parentRegion = parentBlock?.CurrentRegion;
            if (null == parentRegion)
                return;

            int priorCount = parentRegion.Contents.Count;
            var usedBefore = parentRegion.UsedSize;
            var childLists = SnapshotChildLists(this.Component);
            var positioned = SnapshotPositionedRegions(parentBlock);

            var measurePosition = position.Clone();
            measurePosition.OverflowAction = OverflowAction.Visible;
            measurePosition.Height = new Unit(ShrinkMeasuringHeight, PageUnits.Points);
            measurePosition.MinimumHeight = null;
            measurePosition.MaximumHeight = null;

            this.Context.MeasuringDepth++;
            try
            {
                this.DoLayoutBlockComponent(measurePosition, columnOptions);
            }
            finally
            {
                this.Context.MeasuringDepth--;
                this.ContinueLayout = true;
            }

            double width = -1;
            bool wrapped = false;

            if (this.DocumentLayout.CurrentPage == page && parentRegion.Contents.Count > priorCount)
            {
                if (parentRegion.Contents[priorCount] is PDFLayoutBlock measured)
                {
                    width = measured.TotalBounds.Width.PointsValue;
                    if (null != measured.Position)
                        width -= (measured.Position.Margins.Left + measured.Position.Margins.Right).PointsValue;

                    wrapped = measured.HasWrappedText(includeBlocks: true);
                }

                while (parentRegion.Contents.Count > priorCount)
                    parentRegion.Contents.RemoveAt(parentRegion.Contents.Count - 1);

                parentRegion.UsedSize = usedBefore;
            }

            RestorePositionedRegions(positioned);
            RestoreChildLists(childLists);

            if (wrapped)
            {
                //the content is wider than the space available (its text wraps), so the block fills the space (limited by any max-width), as css.
                position.FillWidth = true;
            }
            else if (width > 0)
                position.Width = new Unit(width + ShrinkWidthAllowance, PageUnits.Points);
        }

        #endregion

        private PDFLayoutInlineBegin CreateAndAddInlineBegin(PDFPositionOptions pos)
        {
            PDFLayoutBlock containerBlock = this.DocumentLayout.CurrentPage.LastOpenBlock();
            PDFLayoutRegion containerRegion = containerBlock.CurrentRegion;
            if (containerRegion.HasOpenItem == false)
                containerRegion.BeginNewLine();
            PDFLayoutLine currline = containerRegion.CurrentItem as PDFLayoutLine;
            PDFTextRenderOptions text = this.FullStyle.CreateTextOptions();
            PDFLayoutInlineBegin begin = currline.AddInlineRunStart(this, this.Component, pos, text, this.FullStyle);
            return begin;
        }

        private PDFLayoutInlineEnd CreateAndAddInlineEnd(PDFPositionOptions pos, PDFLayoutInlineBegin begin)
        {
            PDFLayoutBlock containerBlock = this.DocumentLayout.CurrentPage.LastOpenBlock();
            PDFLayoutRegion containerRegion = containerBlock.CurrentRegion;
            if (containerRegion.HasOpenItem == false)
                containerRegion.BeginNewLine();
            PDFLayoutLine currline = containerRegion.CurrentItem as PDFLayoutLine;
            PDFLayoutInlineEnd end = currline.AddInlineRunEnd(this, this.Component, begin, pos);
            return end;
        }

        


        /// <summary>
        /// Lays out all the content of this panel
        /// </summary>
        /// <param name="position"></param>
        protected virtual void DoLayoutBlockComponent(PDFPositionOptions position, PDFColumnOptions columnOptions)
        {
            PDFLayoutBlock containerBlock = CreateContainerBlock(position);
            if (null == containerBlock)
                return;


            CreateBlockRegions(containerBlock, position, columnOptions);

            this.DoLayoutChildren();

            EnsureContentsFit();
            
        }

        

        

        

        

        


        protected void EnsureContentsFit()
        {
            bool newPage;
            PDFLayoutBlock block = this.CurrentBlock;
            //re-retrieve our container block and region
            //it could have changed whilst laying out other regions / pages.
            PDFLayoutBlock containerBlock = block.Parent as PDFLayoutBlock;

            if (block.IsClosed == false)
                block.Close();

            bool updateSize = false;
            bool widthOverridden = false;
            Size updated = block.Size;



            if (block.Position.MinimumHeight.HasValue && block.Height < block.Position.MinimumHeight.Value)
            {
                updated.Height = block.Position.MinimumHeight.Value - (block.Position.Padding.Top + block.Position.Padding.Bottom);
                updateSize = true;
            }

            if (block.Position.MinimumWidth.HasValue && block.Width < block.Position.MinimumWidth.Value)
            {
                updated.Width = block.Position.MinimumWidth.Value - (block.Position.Padding.Left + block.Position.Padding.Right);
                updateSize = true;
                widthOverridden = true;
            }



            if (updateSize)
            {
                Unit contentW = updated.Width;
                if (!widthOverridden)
                {
                    // block.Size.Width after Close() is TotalBounds.Width (content + padding + margin).
                    // SetContentSize expects content-only and re-adds them, so strip first.
                    if (!block.Position.Margins.IsEmpty)
                        contentW -= block.Position.Margins.Left + block.Position.Margins.Right;
                    if (!block.Position.Padding.IsEmpty)
                        contentW -= block.Position.Padding.Left + block.Position.Padding.Right;
                }
                block.SetContentSize(contentW, updated.Height);
            }

            

            PDFLayoutRegion containerRegion = containerBlock.CurrentRegion;

            //ADDED to support blocks flowing to the next region or page
            Unit vspace = containerRegion.TotalBounds.Height - containerRegion.UsedSize.Height;
            Unit req = block.Height;

            

            bool canfitvertical = req <= vspace;


            if (!canfitvertical && containerRegion.Contents.Count > 1 && !this.Context.IsMeasuring)
            {
                
                PDFLayoutRegion prev = containerRegion;
                if (IsLastInClippedBlock(prev))
                {
                    this.Context.TraceLog.Add(TraceLevel.Verbose, LOG_CATEGORY, "Cannot fit the block for '" + this.Component.UniqueID + "', but overflow of the content within the region is allowed becasue we are clipping");
                    containerRegion.AddToSize(block);
                }
                else if (this.MoveToNextRegion(req, ref containerRegion, ref containerBlock, out newPage))
                {
                    prev.Contents.Remove(block);
                    containerRegion.AddExistingItem(block);
                }
                else
                {
                    this.Context.TraceLog.Add(TraceLevel.Warning, LOG_CATEGORY, "Cannot fit the block for component " + this.Component.UniqueID + " in the avilable height (required = '" + req + "', available = '" + vspace + "'), and we cannot overflow to a new region. Layout of component stopped and returning.");

                    this.ContinueLayout = false;
                }
            }
            else
            {
                //Region has not updated its size as the block was directly closed.
                containerRegion.AddToSize(block);
            }
        }


        
        
    }
}
