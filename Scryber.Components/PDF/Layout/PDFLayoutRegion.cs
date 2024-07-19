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

//Fixes a bug that calculate the size
//wrong when overflowing onto a new column or page
#define FIXSIZEFORREGION



using System;
using System.Collections.Generic;
using System.Net;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF.Graphics;

namespace Scryber.PDF.Layout
{

    /// <summary>
    /// All blocks have at least one column. And the column contains the inner items
    /// </summary>
    public class PDFLayoutRegion : PDFLayoutItem
    {
        //
        // properties
        // 

        #region public PDFLayoutItemCollection Contents { get; private set; }

        /// <summary>
        /// Lazy init collection of items
        /// </summary>
        private PDFLayoutItemCollection _contents = null;

        /// <summary>
        /// Gets the inner items of the container item
        /// </summary>
        public PDFLayoutItemCollection Contents
        {
            get
            {
                if (null == _contents)
                    _contents = new PDFLayoutItemCollection();
                return _contents;
            }
        }

        #endregion

        #region public PDFSize UsedSize { get; set; }

        /// <summary>
        /// Gets or sets the used size of the content in this region
        /// </summary>
        public Size UsedSize { get; set; }

        #endregion

        #region public bool AutoOverflow {get;set;}

        private bool _autoOverflow = true;

        /// <summary>
        /// Gets or sets the flag for auto overflowing. 
        /// If true then the move to next region will be supported and content will flow 
        /// from this region to the next linked region
        /// </summary>
        public bool AutoOverflow
        {
            get { return _autoOverflow; }
            set { _autoOverflow = value; }
        }

        #endregion

        #region public virtual PDFRect TotalBounds { get; set; }

        private Rect _contentbounds;

        /// <summary>
        /// Gets or sets the total content bounds of this region
        /// </summary>
        public virtual Rect TotalBounds
        {
            get { return _contentbounds; }
            set { _contentbounds = value; }
        }

        #endregion

        #region public PDFRect UnusedBounds {get;}

        /// <summary>
        /// Gets the rectangle that further content can be added to.
        /// </summary>
        public Rect UnusedBounds
        {
            get
            {
                Rect content = this.TotalBounds;
                Size used = this.UsedSize;
                content.Height -= used.Height;
                content.Y += used.Height;
                return content;
            }
        }

        #endregion

        #region public PDFUnit AvailableHeight

        /// <summary>
        /// Gets the available height in this region that content can be added to
        /// </summary>
        public Unit AvailableHeight
        {
            get
            {
                Rect content = this.TotalBounds;
                Size used = this.UsedSize;
                return content.Height - used.Height;
            }
        }

        #endregion

        #region public PDFUnit OffsetX {get;}

        /// <summary>
        /// Gets the offset of this region in comparison to it's containing block
        /// </summary>
        public override Unit OffsetX
        {
            get { return this.UnusedBounds.X; }
        }

        #endregion

        #region public PDFLayoutLine CurrentLine {get;}

        /// <summary>
        /// Gets the current line (open or closed from this region). 
        /// If the last item is not a line or there are no items in this region then it will return null.
        /// </summary>
        public PDFLayoutItem CurrentItem
        {
            get
            {
                if (this.IsClosed)
                    return null;
                
                int lastindex = this.Contents.Count - 1;
                if (lastindex < 0)
                    return null;

                PDFLayoutItem last = this.Contents[lastindex];
                return last;
            }
        }

        #endregion

        #region public bool HasOpenItem {get;}

        /// <summary>
        /// Returns true if the last item in this region is a line and it is open
        /// </summary>
        public bool HasOpenItem
        {
            get
            {
                if (this.IsClosed)
                    return false;

                int lastindex = this.Contents.Count - 1;
                if (lastindex < 0)
                    return false;

                PDFLayoutItem last = this.Contents[lastindex];
                return !last.IsClosed;
            }
        }

        #endregion

        #region public int ColumnIndex

        /// <summary>
        /// Gets the column index this region represents.
        /// </summary>
        public int ColumnIndex
        {
            get;
            private set;
        }

        #endregion

        #region public PDFLayoutRegion NextRegion {Get;set;}

        private PDFLayoutRegion _next;

        /// <summary>
        /// Gets or sets the next region that content within this region can flow onto.
        /// </summary>
        public PDFLayoutRegion NextRegion
        {
            get { return _next; }
            set { _next = value; }
        }

        #endregion

        #region public HorizontalAlignment HAlignment

        /// <summary>
        /// Gets or sets the horizontal alignment of the content in this region
        /// </summary>
        public HorizontalAlignment HAlignment
        {
            get;
            set;
        }

        #endregion

        #region public VerticalAlignment VAlignment

        /// <summary>
        /// Gets or sets the vertical alignment of the content in this region
        /// </summary>
        public VerticalAlignment VAlignment
        {
            get;
            set;
        }

        #endregion

        #region public override PDFUnit Width {get;}

        /// <summary>
        /// Gets the USED width of this region
        /// </summary>
        public override Unit Width
        {
            get { return this.UsedSize.Width; }
        }

        #endregion

        #region public override PDFUnit Height {get;}

        /// <summary>
        /// Gets the USED height of this region
        /// </summary>
        public override Unit Height
        {
            get { return this.UsedSize.Height; }
        }

        #endregion

        #region public PositionMode PostionMode {get; private set;}

        /// <summary>
        /// Gets or sets the postion mode for this region
        /// </summary>
        public PositionMode PositionMode
        {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Gets or sets the floating blocks in this region.
        /// </summary>
        public PDFFloatAddition Floats { get; set; }


        /// <summary>
        /// If true then this region will not be rendered.
        /// </summary>
        public bool ExcludeFromOutput { get; set; }

        //
        // ctor(s)
        //

        #region public PDFLayoutRegion(PDFLayoutBlock block, IPDFComponent owner, PDFRect contentbounds, int columnindex, HorizontalAlignment halign, VerticalAlignment valign)

        public PDFLayoutRegion(PDFLayoutBlock block, IComponent owner, Rect contentbounds, int columnindex, HorizontalAlignment halign, VerticalAlignment valign)
            : this(block,owner,contentbounds,columnindex,halign,valign, PositionMode.Block)
        {
        }

        #endregion

        #region public PDFLayoutRegion(PDFLayoutBlock block, IPDFComponent owner, PDFRect contentbounds, int columnindex, HorizontalAlignment halign, VerticalAlignment valign, PositionMode mode)

        /// <summary>
        /// Creates a new PDFLayoutRegion.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="columnindex"></param>
        /// <param name="contentbounds"></param>
        public PDFLayoutRegion(PDFLayoutBlock block, IComponent owner, Rect contentbounds, int columnindex, HorizontalAlignment halign, VerticalAlignment valign, PositionMode mode)
            : base(block, owner)
        {
            this.UsedSize = Size.Empty;
            this.ColumnIndex = columnindex;
            this.TotalBounds = contentbounds;
            this.HAlignment = halign;
            this.VAlignment = valign;
            this.PositionMode = mode;
        }

        #endregion

        //
        // methods
        //

        #region public PDFLayoutBlock LastOpenBlock()

        /// <summary>
        /// Returns the last block in this region that is open. 
        /// Returns null if this region does not contain any blocks, or the last block is closed.
        /// </summary>
        /// <returns></returns>
        public PDFLayoutBlock LastOpenBlock()
        {
            if (null != this.Contents && this.Contents.Count > 0)
            {
                PDFLayoutItem item = this.Contents[this.Contents.Count - 1];
                if (item is PDFLayoutBlock)
                {
                    PDFLayoutBlock block = item as PDFLayoutBlock;
                    if (block.IsClosed == false)
                        return block.LastOpenBlock();
                }
                else
                {
                    //the last item is not a block, so any previous blocks must be closed.
                }
            }
            return null;
        }

        #endregion

        #region public void CloseCurrentItem(PDFLayoutContext context)

        /// <summary>
        /// Closes the currently open line or block
        /// </summary>
        /// <returns>true if the line was closed.</returns>
        public void CloseCurrentItem()
        {
            //Current line will throw an exeception if this region is closed.
            PDFLayoutItem last = this.CurrentItem;
            if (null != last && !last.IsClosed)
            {
                //We have a line so lets try to close it.
                last.Close();

                //AddToSize(last);
            }
        }

        #endregion

        #region public void AddToSize(PDFLayoutItem item)

        /// <summary>
        /// Increases the size of this region by the size of the layout item
        /// </summary>
        /// <param name="item"></param>
        public void AddToSize(PDFLayoutItem item)
        {
            Unit h = item.Height;
            Unit w = item.Width;

            //Update the size
            Size sz = this.UsedSize;
            sz.Height += h;
            sz.Width = Unit.Max(sz.Width, w);
            this.UsedSize = sz;
        }

        #endregion

        #region public PDFLayoutLine BeginNewLine()

        /// <summary>
        /// Begins a new line on the current region and sets up it's width.
        /// </summary>
        /// <param name="startparagraph">Set to true if this is the first line in a run or the first line in a new paragraph</param>
        /// <returns>The newly created and added line</returns>
        public PDFLayoutLine BeginNewLine()
        {
            this.AssertIsOpen();
            var last = this.AssertLastItemIsClosed() as PDFLayoutLine;
            
            Unit width = this.GetAvailableLineWidth();

            PDFLayoutLine line = new PDFLayoutLine(this, width, this.HAlignment, VerticalAlignment.Baseline, this.Contents.Count);
            line.SetOffset(line.OffsetX, this.UsedSize.Height);
            //if (null != last)
            //    line.BaseLineOffset = last.BaseLineOffset;

            this.Contents.Add(line);

            return line;
        }

        #endregion

        #region public PDFLayoutItem RemoveLastItem()

        /// <summary>
        /// Removes and returns the last item in this region. If this region doesn't have any contents, then returns null.
        /// </summary>
        /// <returns>The last item removed</returns>
        public PDFLayoutItem RemoveLastItem()
        {
            //this.AssertIsOpen();

            if(this._contents == null || _contents.Count == 0)
                return null;

            int last = this.Contents.Count -1;
            PDFLayoutItem removed = this.Contents[last];
            if (removed is PDFLayoutLine)
                ((PDFLayoutLine)removed).LineIndex = -1;
            this.Contents.RemoveAt(last);
            return removed;
        }

        #endregion

        public override void ResetAvailableHeight(Unit height, bool includeChildren)
        {
            this._contentbounds.Height = height;
            if (includeChildren)
            {

                if (this.HasOpenItem)
                   this.CurrentItem.ResetAvailableHeight(height-this.UsedSize.Height, includeChildren);
            }
        }

        #region public void AssertRemoveLastItem(PDFLayoutItem ensureitem)

        /// <summary>
        /// Removes the last item from this region and checks
        /// to make sure that the item removed matches the povided item
        /// </summary>
        /// <param name="ensureitem"></param>
        public void AssertRemoveLastItem(PDFLayoutItem ensureitem)
        {
            PDFLayoutItem removed = this.RemoveLastItem();
            if (removed != ensureitem)
                throw new ArgumentOutOfRangeException("ensureItem");
            
        }

        /// <summary>
        /// Removes the latout item from this regions contents if it exists. Returns true if it was removed, otherwise false
        /// </summary>
        /// <param name="ensureItem"></param>
        /// <returns></returns>
        public bool RemoveItem(PDFLayoutItem ensureItem)
        {
            if (null != ensureItem)
                return this.Contents.Remove(ensureItem);
            else
                return false;
        }

        #endregion

        #region public virtual void AddExistingItem(PDFLayoutItem item)

        /// <summary>
        /// Adds an existing line to this region, updating the items' parent in the process.
        /// And increasing the size of this region if the block is closed.
        /// </summary>
        /// <param name="item"></param>
        public virtual void AddExistingItem(PDFLayoutLine line)
        {
            if (null == line)
                throw new ArgumentNullException("line");
            this._contents.Add(line);
            line.SetOffset(line.OffsetX, this.Height);
            line.SetParent(this);
            line.LineIndex = this.Contents.Count - 1;

            if(line.IsClosed)
                this.AddToSize(line);
        }


        /// <summary>
        /// Adds an existing block to this region, updating the items parent in the process.
        /// And increasing the size of this region if the block is closed.
        /// </summary>
        /// <param name="block"></param>
        public virtual void AddExistingItem(PDFLayoutBlock block, int atIndex = -1)
        {
            if (null == block)
                throw new ArgumentNullException("block");

            if (block.Parent != this.Parent) //make sure the parent blocks match
                block.SetParent(this.Parent);

            if (atIndex < 0 || atIndex >= this.Contents.Count)
            {
                this.Contents.Add(block);
            }
            else
            {
                this.Contents.Insert(atIndex, block);
            }

            //Set the blocks new available bounds based on our size
            Rect newbounds = block.AvailableBounds;

            newbounds.Y = this.Height;
            newbounds.Height = (this.AvailableHeight - (block.Position.Padding.Top + block.Position.Padding.Bottom)) - this.Height;
            newbounds.X = 0;
            //newbounds.Width = this.TotalBounds.Width - (block.Position.Padding.Left + block.Position.Padding.Right) ;
            block.AvailableBounds = newbounds;

            newbounds = block.TotalBounds;
            newbounds.Y = this.Height;
            block.TotalBounds = newbounds;
            //move it back to the top
            //block.Offset(PDFUnit.Zero, PDFUnit.Zero - block.TotalBounds.Y);

            if (block.IsClosed)
                this.AddToSize(block);
            else
            {
                if(block.CurrentRegion.IsClosed == false)
                {
                    newbounds = block.CurrentRegion.UnusedBounds;
                    newbounds.Height = block.AvailableBounds.Height - block.Position.Margins.Top - block.Position.Margins.Bottom;
                    newbounds.Y = Unit.Zero;
                    block.CurrentRegion.TotalBounds = newbounds;
                }
            }
        }

        #endregion

        public virtual PDFLayoutPositionedRegionRun AddExistingPositionedRegion(PDFLayoutPositionedRegion region, PDFLayoutBlock associatedBlock)
        {
            if (null == region)
                throw new ArgumentNullException("region");
            if (null == associatedBlock)
                throw new ArgumentNullException("associatedBlock");

            if (region.AssociatedRun != null && region.AssociatedRun.Parent != null)
                (region.AssociatedRun.Parent as PDFLayoutLine).Runs.Remove(region.AssociatedRun);


            var origParent = region.GetParentBlock();
            if(null != origParent)
            {
                if (origParent.HasPositionedRegions)
                    origParent.PositionedRegions.Remove(region);
            }

            var newParent = this.GetParentBlock();
            if (null != newParent)
            {
                newParent.PositionedRegions.Add(region);
                region.SetParent(newParent);
                associatedBlock.SetParent(newParent);
                
            }

            var line = this.CurrentItem as PDFLayoutLine;
            if (line == null)
                line = this.BeginNewLine();

            var run = line.AddPositionedRun(region, associatedBlock.Owner);

            return run;

        }

        #region protected virtual PDFUnit GetAvailableWidth()

        /// <summary>
        /// Gets the current available width for a line
        /// </summary>
        /// <returns></returns>
        protected Unit GetAvailableLineWidth()
        {
            return GetAvailableLineWidth(this.UsedSize.Height, 1.0);
        }

        public virtual Unit GetAvailableLineWidth(Unit yoffset, Unit height)
        {
            Unit avail = this.UnusedBounds.Width;
            
            if (null != this.Floats)
                return this.Floats.ApplyWidthInset(avail, yoffset, height);
            else
            {
                var block = this.GetParentBlock();
                if (block.Position.PositionMode == PositionMode.Absolute)
                    return avail;
                else
                {
                    var parent = block.GetParentBlock();
                    if (null != parent)
                    {
                        yoffset += parent.CurrentRegion.UsedSize.Height + block.Position.Margins.Top + block.Position.Padding.Top;
                        avail = parent.CurrentRegion.GetAvailableLineWidth(yoffset, height); 
                        avail -= block.Position.Margins.Left + block.Position.Margins.Right;
                        avail -= block.Position.Padding.Left + block.Position.Padding.Right;
                        return avail;
                    }
                    else
                    {
                        return avail;
                    }
                }
            
            }
        }

        #endregion

        public virtual Unit GetLeftInset(Unit yoffset, Unit height)
        {
            Unit x = Unit.Zero;
            
            if (null != this.Floats)
                return this.Floats.GetLeftOffset(x, yoffset, height);
            else
            {
                var block = this.GetParentBlock();
                
                if (block.Position.PositionMode == PositionMode.Absolute)
                    return x;
                else
                {
                    var parent = block.GetParentBlock();
                    
                    if (null != parent)
                    {
                        yoffset += block.TotalBounds.Y + block.Position.Margins.Top + block.Position.Padding.Top;
                        x = parent.CurrentRegion.GetLeftInset(yoffset, height);
                        //x -= block.Position.Margins.Left;
                        //x -= block.Position.Padding.Left;
                        return x;
                    }
                    else
                        return x;
                }
            
            }

        }

        public virtual Unit GetRightInset(Unit yoffset, Unit height)
        {
            Unit x = Unit.Zero;
            //if (null != this.Floats)
            //    x = this.Floats.GetRightInset(x, yoffset, height);

            if (null != this.Floats)
                return this.Floats.GetRightInset(x, yoffset, height);
            else
            {
                var block = this.GetParentBlock();
                
                if (block.Position.PositionMode == PositionMode.Absolute)
                    return x;
                else
                {
                    var parent = block.GetParentBlock();
                    if (null != parent)
                    {
                        yoffset += parent.CurrentRegion.UsedSize.Height + block.Position.Margins.Top + block.Position.Padding.Top;
                        x = parent.CurrentRegion.GetRightInset(yoffset, height);
                        //x -= block.Position.Margins.Left;
                        //x -= block.Position.Padding.Left;
                        return x;
                    }
                    else
                        return x;
                }
            
            }
            return x;
        }

        #region protected virtual void AssertLastItemIsClosed()

        /// <summary>
        /// Checks that the last item in this region is closed
        /// </summary>
        protected virtual PDFLayoutItem AssertLastItemIsClosed()
        {
            if (null == _contents || _contents.Count == 0)
                return null;
            PDFLayoutItem last = _contents[_contents.Count - 1];
            if (!last.IsClosed)
                throw new InvalidOperationException(Errors.LayoutContainerHasExistingOpenItem);

            return last;
        }

        #endregion

        public virtual void AddFloatingInset(FloatMode mode, Unit floatWidth, Unit floatInset, Unit offsetY, Unit floatHeight)
        {
            var line = this.CurrentItem as PDFLayoutLine;

            //If we have a current line and the float is on this line
            if (null != line && line.OffsetY >= offsetY)
                line.SetMaxWidth(line.FullWidth - floatWidth);

            if (mode == FloatMode.Left)
            {
                this.Floats = new PDFFloatLeftAddition(floatWidth, floatHeight, floatInset, offsetY, this.Floats);
            }
            else if(mode == FloatMode.Right)
            {
                this.Floats = new PDFFloatRightAddition(floatWidth, floatHeight, floatInset, offsetY, this.Floats);
            }
        }


        //
        // overrides
        //

        
        /// <summary>
        /// Overrides the default behaviour to push any arrangements for the child item of this region
        /// </summary>
        /// <param name="context"></param>
        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, Unit origXoffset, Unit origYoffset)
        {
            bool logdebug = context.ShouldLogDebug;
            if (logdebug)
                context.TraceLog.Begin(TraceLevel.Debug, "Layout Region", "Pushing the component layout for the region " + this.ToString());

            
            if (!this.IsClosed)
            {
                if (logdebug)
                    context.TraceLog.Add(TraceLevel.Debug, PDFLayoutItem.LOG_CATEGORY, "Closing the region " + this.ToString() + " before pushing the layout");
                this.Close();
            }

            if(this._contents == null || this.Contents.Count == 0)
            {
                if (logdebug)
                    context.TraceLog.Add(TraceLevel.Debug, PDFLayoutItem.LOG_CATEGORY, "The region " + this.ToString() + " is empty. Exiting the push component early.");
                return;
            }

            bool applyAlignments = this.ShouldApplyAlignment();
            Unit yoffset = origYoffset;

            if (applyAlignments)
            {
                VerticalAlignment v = this.VAlignment;
                if (v != VerticalAlignment.Top)
                {
                    this.EnsureCorrectHeight();
                    if (logdebug)
                        context.TraceLog.Add(TraceLevel.Debug, PDFLayoutItem.LOG_CATEGORY, "Adjusting the vertical offset of the region " + this.ToString() + " as it is not top aligned");
                    
                    Unit space = this.AvailableHeight;

                    if (v == VerticalAlignment.Middle)
                        space = space / 2;
                    yoffset += space;
                }
            }
            

            HorizontalAlignment h = this.HAlignment;

            PDFTextRenderOptions options = (this.Parent as PDFLayoutBlock).FullStyle.CreateTextOptions();
            List<PDFTextRunCharacter> cache = new List<PDFTextRunCharacter>();
            bool lastwasapplied = false;
            Unit lastXInset = 0;

            foreach (PDFLayoutItem item in this.Contents)
            {
                Unit actYOffset = yoffset + item.OffsetY;

                var line = item as PDFLayoutLine;
                Unit xInset = Unit.Zero;
                
                if (null != line)
                    xInset = this.GetLeftInset(actYOffset, item.Height);
                Unit itemXOffset = origXoffset;

                if (xInset != 0) //We have floating left item(s)
                {
                    itemXOffset += xInset;
                }
               

                ///Individually calculate each lines horizontal offset
                if (applyAlignments && h != HorizontalAlignment.Left)
                {
                    Unit width =  this.UnusedBounds.Width;
                    
                    if (null != line)
                        width = this.GetAvailableLineWidth(actYOffset, item.Height);
                    
                    Unit space = width - item.Width;

                    if(h == HorizontalAlignment.Justified)
                    {
                        if (null != line)
                        {
                            if (logdebug)
                                context.TraceLog.Add(TraceLevel.Verbose, PDFLayoutItem.LOG_CATEGORY, "Justifying the textual content of the line " + line.LineIndex);

                            bool didjustify = line.JustifyContent(width, item.Width, space, false, cache, context, ref options);

                            if (!didjustify && lastwasapplied && null != options && !(options.WordSpacing.HasValue || options.CharacterSpacing.HasValue))
                            {
                                line.ResetJustifySpacing(options);
                                if (logdebug)
                                    context.TraceLog.Add(TraceLevel.Verbose, PDFLayoutItem.LOG_CATEGORY, "Justified the textual content of the line " + line.LineIndex + " with character spacing of " + options.CharacterSpacing + " and word spacing of " + options.WordSpacing);
                            }

                            lastwasapplied = didjustify;
                        }
                        space = 0; // reset space to zero as already accounted for.
                    }
                    else if(h == HorizontalAlignment.Center)
                    {
                        space = space / 2;
                    }
                    
                    itemXOffset = itemXOffset + space;
                }
                
                item.PushComponentLayout(context, pageIndex, itemXOffset, yoffset);
            }

            if (logdebug)
                context.TraceLog.End(TraceLevel.Debug, "Layout Region", "Pushed all the component layouts for the region " + this.ToString());
        }

        /// <summary>
        /// Checks if this region should apply allignements - if we are a positioned region then we do not 
        /// as the content of this region (and it should only be 1 block) will specify position data
        /// and the contents within that block are not of our concern.
        /// </summary>
        /// <returns></returns>
        private bool ShouldApplyAlignment()
        {
            if (this.PositionMode == PositionMode.Fixed || this.PositionMode == Drawing.PositionMode.Absolute)
                return false;
            else if(this.PositionMode == PositionMode.Relative)
            {
                return false; //Check with relative unit tests.
            }
            else
                return true;
        }

        /// <summary>
        /// Quick fix for bug that incorrectly calculates the height of a region when an inner panel has overflowed onto
        /// a new column or page.
        /// </summary>
        private void EnsureCorrectHeight()
        {
            //TODO:Fix the height calculation for a region when it has overflowed rather than re-calculate all
#if FIXSIZEFORREGION

            Unit h = Unit.Zero;
            foreach (PDFLayoutItem item in this.Contents)
            {
                h += item.Height;
            }
            this.UsedSize = new Size(this.UsedSize.Width, h);
        

#endif
        }


        #region protected override bool DoClose(ref string msg)

        /// <summary>
        /// Overrides the base layout to close the inner item(s) and then update their final widths
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected override bool DoClose(ref string msg)
        {
            if (this.HasOpenItem)
            {
                this.CloseCurrentItem();
            }

            //Ensure the items are within the region width
            Unit max = this.TotalBounds.Width;

            foreach (PDFLayoutItem item in this.Contents)
            {
                item.SetMaxWidth(max);
            }

            return base.DoClose(ref msg);
        }

        #endregion

        public void ReOpen(bool includeChildren)
        {
            this.IsClosed = false;

            if (includeChildren && this.Contents.Count > 0)
            {
                var item = this.Contents[this.Contents.Count - 1];
                if (item is PDFLayoutBlock block)
                    block.ReOpen();
            }
        }

        #region protected override Native.PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)

        /// <summary>
        /// Outputs this region by rendering the inner item of this region
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected override Native.PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            bool logdebug = context.ShouldLogDebug;
            string name = string.Empty;

            if (logdebug)
            {
                name = this.ToString();
                context.TraceLog.Begin(TraceLevel.Debug, "Layout Region", "Outputting region " + name);
            }

            if(this.ExcludeFromOutput)
            {
                if (logdebug)
                    context.TraceLog.Add(TraceLevel.Debug, "Layout Region", "Region " + name + " is marked as excluded from output, so not rendering anything");
            }
            else if (this._contents != null && this._contents.Count > 0)
            {
                Size prevsize = context.Space;
                Point prevloc = context.Offset;
                Point offset = new Point(prevloc.X + this.TotalBounds.X, prevloc.Y + this.TotalBounds.Y);

                context.Space = this.TotalBounds.Size;
                context.Offset = offset;

                if (logdebug)
                    context.TraceLog.Add(TraceLevel.Debug, "Layout Region", "Adjusted bounds of context for region " + name + " with offset " + context.Offset + " and space " + context.Space + ", now rendering contents");

                foreach (PDFLayoutItem item in this._contents)
                {
                    item.OutputToPDF(context, writer);
                }

                context.Space = prevsize;
                context.Offset = prevloc;
            }
            else if (logdebug)
                context.TraceLog.Add(TraceLevel.Debug, "Layout Region", "Region " + name + " does not have any contents so no inner rendering required");

            

            if (logdebug)
                context.TraceLog.End(TraceLevel.Debug, "Layout Region", "Completed output of region " + name);

            return null;
        }

        #endregion

        public void OutputOverlayColor(PDFBrush fill, PDFGraphics graphics, Point blocklocation)
        {
            Rect rect = new Rect(blocklocation.X + this.OffsetX, blocklocation.Y, this.TotalBounds.Width, this.TotalBounds.Height);
            graphics.FillRectangle(fill, rect);
        }

        #region public PDFLayoutLine StartOrReturnCurrentLine(PositionMode mode)

        /// <summary>
        /// Based on the position mode this method will either close the current line 
        /// and return a new line, or just return the current open line
        /// </summary>
        /// <param name="positionMode"></param>
        /// <returns></returns>
        public PDFLayoutLine StartOrReturnCurrentLine(PositionMode mode)
        {
            PDFLayoutLine line;
            if (mode == PositionMode.Inline)
            {
                //Just make sure we have an open and current line.
                if (this.CurrentItem is PDFLayoutLine)
                    line = (this.CurrentItem.IsClosed == false) ? this.CurrentItem as PDFLayoutLine : this.BeginNewLine();
                else
                {
                    if (this.CurrentItem != null && this.CurrentItem.IsClosed == false)
                        this.CurrentItem.Close();
                    line = this.BeginNewLine();
                }
            }
            else if (mode == PositionMode.Block)
            {
                if (this.HasOpenItem)
                    this.CloseCurrentItem();

                line = this.BeginNewLine();
            }
            else
                throw new NotImplementedException("Absolute and relative positioning on regions is not supported");

            return line;
        }

        #endregion


        #region public override string ToString()

        /// <summary>
        /// Overrides object implementation to return a string representing this Region
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (null != this.Parent)
                return this.Parent.ToString() + ":Region" + this.ColumnIndex.ToString();
            else
                return "[ORPHAN]:Region" + this.ColumnIndex.ToString();
        }

        #endregion

    }

    public class PDFLayoutRegionCollection : List<PDFLayoutRegion>
    {


        /// <summary>
        /// Will return true and set curr to the last region in this collection if it is open.
        /// Otherwise false and null
        /// </summary>
        /// <param name="curr"></param>
        /// <returns></returns>
        public bool TryGetLastOpenPositionedRegion(out PDFLayoutRegion curr)
        {
            curr = null;
            int index = this.Count - 1;
            if (index < 0)
                return false;

            curr = this[index];
            if (curr.IsClosed)
            {
                curr = null;
                return false;
            }
            else
                return true;
        }
    }
}
