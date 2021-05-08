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
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Native;
using Scryber.Components;

namespace Scryber.Layout
{

    /// <summary>
    /// Represents a block of content on a page. Each block has an array of available regions to add content to (other blocks and lines).
    /// </summary>
    public class PDFLayoutBlock : PDFLayoutContainerItem
    {
        //
        // properties
        //

        #region public PDFLayoutRegion CurrentRegion { get;}

        private PDFLayoutRegion _current;

        /// <summary>
        /// Gets the current region in the block
        /// </summary>
        /// <remarks>Is there is a current open
        /// positioned region (absolute or relative) then this will be returned, 
        /// otherwise it will be the last open column if any</remarks>
        public PDFLayoutRegion CurrentRegion
        {
            get
            {
                if (this.HasPositionedRegions)
                {
                    PDFLayoutRegion last = this.PositionedRegions[this.PositionedRegions.Count - 1];
                    if (last.IsClosed == false)
                        return last;
                }
                return _current;
            }
            protected set
            {
                _current = value;
            }
        }

        #endregion

        #region public PDFLayoutRegion[] Columns { get; private set; }

        /// <summary>
        /// Gets all the regions in the block.
        /// </summary>
        public PDFLayoutRegion[] Columns { get; private set; }

        #endregion

        #region private PDFLayoutRegionCollection PositionedRegions {get;} + HasPositionedRegions {get;}

        private PDFLayoutRegionCollection _positioned;

        /// <summary>
        /// Gets the collection of regions in this block that have relative or absolute positioning.
        /// </summary>
        public PDFLayoutRegionCollection PositionedRegions
        {
            get
            {
                if (null == _positioned)
                    _positioned = new PDFLayoutRegionCollection();
                return _positioned;
            }
        }

        /// <summary>
        /// True if this block has positioned regions
        /// </summary>
        public bool HasPositionedRegions
        {
            get { return null != _positioned && _positioned.Count > 0; }
        }

        #endregion

        #region public PDFUnit ColumnWidth { get; private set; }

        /// <summary>
        /// Gets the actual width of each column
        /// </summary>
        public PDFUnit[] ColumnWidths { get; private set; }

        #endregion

        #region public PDFColumnOptions ColumnOptions { get; private set; }

        /// <summary>
        /// Gets the options for the column layout in this block
        /// </summary>
        public PDFColumnOptions ColumnOptions { get; private set; }

        #endregion

        #region public bool IsFormXObject { get; set; }

        /// <summary>
        /// Returns true if this block should be rendered as an xObject (independent of the main content stream (or out xObject)
        /// </summary>
        public bool IsFormXObject { get; set; }

        #endregion

        #region public PDFRect XObjectViewPort {get;set;}

        /// <summary>
        /// Gets the view port rectangle for the xObject
        /// </summary>
        public PDFRect XObjectViewPort {get;set;}

        #endregion

        #region public PDFRect AvailableBounds { get; private set; }

        private PDFRect _avail;

        /// <summary>
        /// Gets or sets the available bounds of this block relative to the Absolute bounds
        /// </summary>
        public PDFRect AvailableBounds
        {
            get { return _avail; }
            set { _avail = value; }
        }

        #endregion

        #region public PDFRect TotalBounds { get; set;}

        private PDFRect _total = PDFRect.Empty;

        /// <summary>
        /// Gets the absolute bounds (inc margins and padding) for this block
        /// </summary>
        public PDFRect TotalBounds 
        {
            get { return _total; }
            set { _total = value; }
        }

        #endregion

        #region public PDFRect TransformedBounds {get;set;} 


        private PDFPoint _transformBounds;

        /// <summary>
        /// Returns any transformation offset to be applied to the block, 
        /// </summary>
        public PDFPoint TransformedOffset
        {
            get { return _transformBounds; }
            set
            {
                _transformBounds = value;
            }
        }

        /// <summary>
        /// Returns true if this block has transformation matrices applied.
        /// </summary>
        public bool HasTransformedOffset
        {
            get
            {
                return _transformBounds.IsEmpty == false;
            }
        }

        #endregion

        #region public PDFPositionOptions Position { get; }

        /// <summary>
        /// Gets the position options for this block
        /// </summary>
        public PDFPositionOptions Position { get; protected set; }

        #endregion

        #region public PDFSize Size { get; set; }

        /// <summary>
        /// Gets or sets the total used size
        /// </summary>
        public PDFSize Size { get; set; }

        #endregion

        #region public OverflowSplit OverflowSplit {get; set;}

        /// <summary>
        /// If overflow split is Never then the content in this block should be kept together at all times.
        /// If it it component then each child component must be kept together in one block, otherwise any component can split across regions and pages.
        /// </summary>
        public OverflowSplit OverflowSplit
        {
            get;
            set;
        }

        #endregion


        /// <summary>
        /// Gets or sets the position of this block on the page
        /// </summary>
        public PDFPoint PagePosition { get; set; }


        #region  protected bool IsContainer { get; set; }

        /// <summary>
        /// Marks this block explicitly as a container block 
        /// (holds references to its contents)
        /// </summary>
        protected bool IsContainer { get; set; }

        #endregion

        #region public override PDFUnit Width {get;}

        /// <summary>
        /// Gets the width of this block
        /// </summary>
        public override PDFUnit Width
        {
            get
            {
                if (this.IsClosed)
                    return this.TotalBounds.Width;
                else
                {
                    if (this.ColumnOptions.ColumnCount == 0)
                        return PDFUnit.Zero;
                    else if (this.ColumnOptions.ColumnCount == 1)
                        return this.Columns[0].Width;
                    else
                    {
                        return this.TotalBounds.Width;
                    }
                }
            }
        }

        #endregion

        #region public override PDFUnit Height {get;}

        /// <summary>
        /// Gets the height of this block
        /// </summary>
        public override PDFUnit Height
        {
            get
            {
                if (this.IsClosed)
                    return this.TotalBounds.Height;

                else if (null == this.ColumnOptions)
                    return this.TotalBounds.Height;

                else
                {
                    if (this.ColumnOptions.ColumnCount == 0)
                        return PDFUnit.Zero;
                    else if (this.ColumnOptions.ColumnCount == 1)
                        return this.Columns[0].Height;
                    else
                    {
                        PDFUnit h = PDFUnit.Zero;
                        for (int i = 0; i < this.ColumnOptions.ColumnCount; i++)
                        {
                            h = PDFUnit.Max(h, this.Columns[i].Height);
                        }
                        return h;
                    }
                }
            }
        }

        #endregion

        #region public PDFFloatAddition Floats {get;set;}

        /// <summary>
        /// Gets or sets the linked list of any floating additions to the block.
        /// </summary>
        public PDFFloatAddition Floats
        {
            get;
            set;
        }

        #endregion

        #region public int BlockRepeatIndex { get; set; }

        /// <summary>
        /// Gets or sets the repeat index for this block.
        /// </summary>
        /// <remarks>PanelLayoutEngine uses this to count the number 
        /// of individual blocks generated for each panel</remarks>
        public int BlockRepeatIndex { get; set; }

        #endregion

        #region public bool ExcludeFromOutput

        private bool _excludeFromOutput = false;

        /// <summary>
        /// Set to true if this block (and any child layout items) should be excluded from the output in the PDF.
        /// </summary>
        public bool ExcludeFromOutput
        {
            get { return _excludeFromOutput; }
            set { _excludeFromOutput = value; }
        }

        #endregion

        #region public List<IPDFComponent> ChildComponents {get;} + HasChildComponents

        private List<IPDFComponent> _children;

        /// <summary>
        /// Gets a list of all the child components that are part of this block
        /// </summary>
        public List<IPDFComponent> ChildComponents
        {
            get
            {
                if (null == _children)
                    _children = new List<IPDFComponent>();
                return _children;
            }
        }

        /// <summary>
        /// Returns true if this block has components that can rendered in it
        /// </summary>
        public bool HasChildComponents
        {
            get { return null != _children && _children.Count > 0; }
        }

        #endregion

        //
        // ctor(s)
        //

        #region public PDFLayoutBlock(PDFLayoutItem parent, IPDFComponent owner, IPDFLayoutEngine engine, PDFStyle fullstyle, OverflowSplit split)

        /// <summary>
        /// Creates a new layout block
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="owner"></param>
        /// <param name="fullstyle"></param>
        public PDFLayoutBlock(PDFLayoutItem parent, IPDFComponent owner, IPDFLayoutEngine engine, Scryber.Styles.Style fullstyle, OverflowSplit split)
            : base(parent, owner, engine, fullstyle)
        {
            this.OverflowSplit = split;
        }

        #endregion

        //
        // methods
        //

        public PDFUnit GetPageYOffset()
        {
            return PDFUnit.Zero;
        }

        #region protected override bool DoClose(ref string msg)

        /// <summary>
        /// Closes the entire block
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected override bool DoClose(ref string msg)
        {
            if (this.CurrentRegion != null && !this.CurrentRegion.IsClosed)
                this.CurrentRegion.Close();
           
            //If the owner is a page then we don't want to resize
            //as it's fixed.
            if (!(Owner is Page)) 
            {
                
                this.ShrinkToFit();
            }

            //if (this.Size == PDFSize.Empty && this.IsContainer 
            //    && this.HasChildComponents == false
            //    && this.HasPositionedRegions == false)
            //{
            //    this.TotalBounds = PDFRect.Empty;
            //    this.Size = PDFSize.Empty;
            //}
            

            return base.DoClose(ref msg);
        }


        #endregion

        public void ReOpen()
        {
            this.IsClosed = false;
        }

        #region public virtual void InitRegions(PDFRect totalbounds, PDFPositionOptions position, PDFColumnOptions columns, PDFUnit alley)

        /// <summary>
        /// Initializes and sets up the regions for a block. 
        /// </summary>
        /// <param name="available">The available space inorder to set inner content placement.</param>
        /// <param name="totalbounds">The total bounds including any margins and padding</param>
        /// <param name="margins">The margins associated with this content</param>
        /// <param name="padding">The padding associated with this content</param>
        /// <param name="columncount"></param>
        /// <param name="autoflow">If true and there is more than one column, 
        /// they will be linked so that content overflows from one to the next. Otherwise the MoveToNextRegion(explicit) must be used.</param>
        /// <param name="alley"></param>
        public virtual void InitRegions(PDFRect totalbounds, PDFPositionOptions position, PDFColumnOptions columns, PDFLayoutContext context)
        {
            PDFTraceLog log = context.TraceLog;
            if (log.ShouldLog(TraceLevel.Debug))
                log.Add(TraceLevel.Debug, LayoutEngineBase.LOG_CATEGORY, "Initialized layout block for component " + (null == this.Owner ? "'NONE'" : this.Owner.ToString()));

            this.ColumnOptions = columns;
            this.TotalBounds = totalbounds;
            
            //Added to include margins
            PDFThickness pad = position.Padding;
            PDFThickness marg = position.Margins;
            PDFRect avail = new PDFRect(pad.Left + marg.Left, pad.Top + marg.Top, 
                                        totalbounds.Width - (pad.Left + pad.Right + marg.Left + marg.Right), 
                                        totalbounds.Height - (pad.Top + pad.Bottom + marg.Top + marg.Bottom));
            if (position.Width.HasValue)
            {
                avail.Width = position.Width.Value - (pad.Left + pad.Right);
            }
            if (position.Height.HasValue)
            {
                avail.Height = position.Height.Value - (pad.Top + pad.Bottom);
            }

            //avail.Location = PDFPoint.Empty;

            this.AvailableBounds = avail;

            this.Size = PDFSize.Empty;
            this.Position = position;

            
            
            if (columns.ColumnCount == 1 && columns.ColumnWidths.IsEmpty)
            {
                this.Columns = new PDFLayoutRegion[columns.ColumnCount];
                avail = new PDFRect(PDFPoint.Empty, avail.Size);
                this.Columns[0] = new PDFLayoutRegion(this, this.Owner, avail, 0, position.HAlign, position.VAlign);
            }
            else
            {
                PDFUnit[] widths;
                int count;
                if (columns.ColumnWidths.HasExplicitWidth)
                {
                    widths = columns.ColumnWidths.GetExplicitColumnWidths(avail.Width, columns.AlleyWidth, out count);
                }
                else if(columns.ColumnWidths.IsEmpty == false)
                {
                    count = columns.ColumnCount;
                    widths = columns.ColumnWidths.GetPercentColumnWidths(avail.Width, columns.AlleyWidth, count);
                }
                else if(columns.ColumnCount > 1)
                {
                    count = columns.ColumnCount;
                    widths = PDFColumnWidths.GetEqualColumnWidths(avail.Width, columns.AlleyWidth, count);
                }
                else
                {
                    count = 1;
                    widths = new PDFUnit[] { avail.Width };
                }

                this.Columns = new PDFLayoutRegion[count];
                this.ColumnWidths = widths;
                this.ColumnOptions.ColumnCount = count;

                PDFUnit x = 0;// position.Padding.Left;
                PDFUnit y = 0;// position.Padding.Top;
                PDFUnit h = this.AvailableBounds.Height;
                PDFLayoutRegion previous = null;

                for (int i = 0; i < this.ColumnOptions.ColumnCount; i++)
                {
                    PDFRect regionbounds = new PDFRect(x, y, widths[i], h);
                    PDFLayoutRegion column = new PDFLayoutRegion(this, this.Owner, regionbounds, i, position.HAlign, position.VAlign);

                    //Set up the linked regions
                    if (null != previous)
                    {
                        previous.NextRegion = column;
                        previous.AutoOverflow = columns.AutoFlow;
                    }

                    this.Columns[i] = column;
                    x += widths[i] + columns.AlleyWidth;
                    previous = column;
                }
            }

            //Set up the start column
            if (this.ColumnOptions.ColumnCount > 0)
                this.CurrentRegion = this.Columns[0];
            else
                this.CurrentRegion = null;
        }

        #endregion

        #region public PDFLayoutBlock LastOpenBlock()

        /// <summary>
        /// Gets the last open block in the hierarchy. 
        /// If this block does not contain any open inner blocks, then it returns itself
        /// If this block is closed then an exception is raised.
        /// </summary>
        /// <returns></returns>
        public PDFLayoutBlock LastOpenBlock()
        {
            if (this.IsClosed)
                throw new InvalidOperationException(Errors.CannotLayoutABlockThatHasBeenClosed);

            PDFLayoutBlock last = this;

            PDFLayoutRegion reg = this.CurrentRegion;
            if (!reg.IsClosed)
            {
                PDFLayoutBlock childlast = reg.LastOpenBlock();
                if (null != childlast)
                    last = childlast;
            }
            return last;
        }

        #endregion

        /*
        public virtual void AddFloatingInset(FloatMode mode, PDFUnit inset, PDFUnit offsetY, PDFUnit height)
        {
            if (mode == FloatMode.Left)
            {
                this.Floats = new PDFFloatLeftAddition(inset, height, offsetY, this.Floats);
            }
            else if (mode == FloatMode.Right)
            {
                this.Floats = new PDFFloatRightAddition(inset, height, offsetY, this.Floats);
            }
        }

        */

        #region public override bool MoveToNextRegion(PDFUnit requiredHeight, PDFLayoutContext context)

        /// <summary>
        /// Attempts to move to the next region, returning true if possible, otherwise false.
        /// </summary>
        /// <returns></returns>
        public override bool MoveToNextRegion(PDFUnit requiredHeight, PDFLayoutContext context)
        {
            bool force = false;
            return this.MoveToNextRegion(force, requiredHeight, context);
        }

        /// <summary>
        /// Attempts to move to the next region, returning true if possible, otherwise false.
        /// </summary>
        /// <param name="force">If false then the regions must have been set up to AutoOverflow from one to the next (soft flow),
        /// if force is true then if there is a next region it will be used.</param>
        /// <returns></returns>
        public bool MoveToNextRegion(bool force, PDFUnit requiredHeight, PDFLayoutContext context)
        {
            //requiredHeight = PDFUnit.Zero;
            PDFLayoutRegion region = this.CurrentRegion;
            if (region.NextRegion != null && (region.AutoOverflow || force) && (this.AvailableBounds.Height >= requiredHeight))
            {
                this.CurrentRegion = region.NextRegion;
                return true;
            }
            else
                return false;
        }

        #endregion

        #region public override void PushComponentLayout(PDFLayoutContext context, PDFUnit xoffset, PDFUnit yoffset)

        /// <summary>
        /// Overrides the default implementation to push the component layouts on this blocks regions
        /// </summary>
        /// <param name="context"></param>
        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, PDFUnit xoffset, PDFUnit yoffset)
        {
            bool logdebug = context.ShouldLogDebug;

            if (logdebug)
                context.TraceLog.Begin(TraceLevel.Debug, "Layout Block", "Starting to push the component arrangements for block " + this.ToString());
            
            
            //Offset our total bounds
            if (this.MovesWithLayout())
            {
                PDFRect total = this.TotalBounds;
                total = new PDFRect(xoffset + total.X, yoffset + total.Y, total.Width, total.Height);
                this.TotalBounds = total;
            }

            this.Size = this.TotalBounds.Size;
            this.Size = this.Size.Subtract(this.Position.Padding);

            xoffset = 0;
            yoffset = 0;

            



            for (int i = 0; i < this.Columns.Length; i++)
            {
                this.Columns[i].PushComponentLayout(context, pageIndex, xoffset, yoffset);
                //xoffset += this.AlleyWidth + this.ColumnWidth;
            }

            if (logdebug)
                context.TraceLog.End(TraceLevel.Debug, "Layout Block", "Completed pushing the component arrangements for block " + this.ToString());

        }

        /// <summary>
        /// returns true if this block should move and offset with the layout of other items
        /// </summary>
        /// <returns></returns>
        private bool MovesWithLayout()
        {
            PositionMode mode = this.Position.PositionMode;
            return mode != PositionMode.Absolute;
        }

        #endregion

        #region private PDFSize GetRequiredSize()

        /// <summary>
        /// Gets the required size of the block to fill it's content (or the explicit size)
        /// </summary>
        /// <returns></returns>
        private PDFSize GetRequiredSize(out bool explicitHeight, out bool explicitWidth)
        {
            explicitHeight = false;
            explicitWidth = false;
            PDFUnit w = this.Width;
            PDFUnit h = this.Height;
            if (this.Position.Width.HasValue)
            {
                w = this.Position.Width.Value;
                explicitWidth = true;
            }
            else if (this.Position.FillWidth)
            {
                w = this.AvailableBounds.Width;
            }
            //Added to support clipping on a block with max-width
            if(this.Position.MaximumWidth.HasValue && w > this.Position.MaximumWidth.Value)
            {
                w = this.Position.MaximumWidth.Value;
                explicitWidth = true;
            }

            if (this.Position.Height.HasValue)
            {
                h = this.Position.Height.Value;
                explicitHeight = true;
            }

            //Added to support clipping on a block with max-height
            if (this.Position.MaximumHeight.HasValue && h > this.Position.MaximumHeight.Value)
            {
                h = this.Position.MaximumHeight.Value;
                explicitHeight = true;
            }

            return new PDFSize(w, h);
        }

        #endregion

        #region private void ShrinkToFit(PDFUnit width, PDFUnit height)

        /// <summary>
        /// Reduces the height and width of this block and it's contents
        /// </summary>
        private void ShrinkToFit()
        {
            if(this.ColumnOptions.ColumnCount > 0 && this.ColumnOptions.AutoFlow == true)
            {
                //TODO:Try and balance the columns.
            }

            bool explicitWidth, explicitHeight;
            PDFSize sz = this.GetRequiredSize(out explicitHeight, out explicitWidth);
            PDFRect full = this.TotalBounds;
            full.Height = sz.Height;
            full.Width = sz.Width;

            //Add padding back in for the region sizes
            //If there was explict heights on this block
            if (this.Position.Padding.IsEmpty == false)
            {
                if (this.Position.Height.HasValue)
                    full.Height -= (this.Position.Padding.Top + this.Position.Padding.Bottom);
                if (this.Position.Width.HasValue)
                    full.Width -= (this.Position.Padding.Left + this.Position.Padding.Right);
            }

            if (this.ColumnOptions.ColumnCount > 1)
            {
                for (int i = 0; i < this.ColumnOptions.ColumnCount; i++)
                {
                    //with columns we only shrink the height, not the width
                    PDFRect one = this.Columns[i].TotalBounds;
                    one.Height = full.Height;
                    this.Columns[i].TotalBounds = one;
                }
            }
            else
            {
                PDFRect content = this.Columns[0].TotalBounds;
                content.Height = full.Height;
                content.Width = full.Width;
                this.Columns[0].TotalBounds = content;
            }
            if (this.HasPositionedRegions)
            {
                foreach (PDFLayoutPositionedRegion reg in this.PositionedRegions)
                {
                    if (reg.PositionMode == PositionMode.Relative || reg.PositionOptions.FloatMode != FloatMode.None)
                    {
                        PDFRect content = reg.TotalBounds;
                        PDFSize bottomright = new PDFSize(content.X + content.Width, content.Y + content.Height);
                        if (reg.PositionOptions.Margins.IsEmpty == false)
                        {
                            bottomright.Width += reg.PositionOptions.Margins.Right;
                            bottomright.Height += reg.PositionOptions.Margins.Bottom;
                        }
                        if (full.Width < bottomright.Width)
                            full.Width = bottomright.Width;
                        if (full.Height < bottomright.Height)
                            full.Height = bottomright.Height;
                    }
                }
            }


            if (explicitHeight)
                full.Height = sz.Height;
            else
                full.Height += this.Position.Padding.Top + this.Position.Padding.Bottom;

            if (explicitWidth)
                full.Width = sz.Width;
            else
                full.Width += this.Position.Padding.Left + this.Position.Padding.Right;

            //Add padding back in for setting this blocks bounds
            if (this.Position.Margins.IsEmpty == false)
            {
                full.Width += this.Position.Margins.Left + this.Position.Margins.Right;
                full.Height += this.Position.Margins.Top + this.Position.Margins.Bottom;
            }
            this.TotalBounds = full;
            this.Size = full.Size;
        }

        #endregion

        #region public void SetContentSize(PDFUnit width, PDFUnit height)

        /// <summary>
        /// Changes the content size of this Block to the specified values. THis should exclude any margins or padding on the block,
        /// which will automatically be added to the TotalBounds
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetContentSize(PDFUnit width, PDFUnit height)
        {
            this.Size = new PDFSize(width, height);
            PDFRect total = this.TotalBounds;
            total.Width = width;
            total.Height = height;
            if (this.Position.Margins.IsEmpty == false)
            {
                total.Width += this.Position.Margins.Left + this.Position.Margins.Right;
                total.Height += this.Position.Margins.Top + this.Position.Margins.Bottom;
            }
            if (this.Position.Padding.IsEmpty == false)
            {
                total.Width += this.Position.Padding.Left + this.Position.Padding.Right;
                total.Height += this.Position.Padding.Top + this.Position.Padding.Bottom;
            }
            this.TotalBounds = total;
        }

        #endregion

        #region public override string ToString()

        /// <summary>
        /// Overrides the default implementation to return a string representation of this block
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (this.Owner != null)
                return this.Owner.ToString() + ":Block" + this.BlockRepeatIndex;
            else
                return "Orphaned:Block" + this.BlockRepeatIndex;
        }

        #endregion


        #region public PDFLayoutBlock BeginNewBlock(IPDFComponent owner, PDFStyle fullstyle)

        /// <summary>
        /// Begins a new block on the current region of this block
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="fullstyle"></param>
        /// <returns></returns>
        public PDFLayoutBlock BeginNewBlock(IPDFComponent owner, IPDFLayoutEngine engine, Style fullstyle, PositionMode mode)
        {
            if (mode == PositionMode.Inline)
                throw RecordAndRaise.ArgumentOutOfRange("mode", Errors.CannotBeginABlockThatIsInline);
            //else if (mode == PositionMode.Absolute)
            //    throw RecordAndRaise.ArgumentOutOfRange("mode",Errors.AbsolutePositioningBlocksRegisterWithTheLayoutPage);
            //else if (mode == PositionMode.Relative) //TODO:Relative positioned blocks
            //    throw RecordAndRaise.NotSupported("Need to do relative positioning");

            PDFLayoutBlock container = this.LastOpenBlock();
            if (null == container)
                container = this;

            OverflowSplit split = fullstyle.GetValue(StyleKeys.OverflowSplitKey, OverflowSplit.Any);
            PDFLayoutRegion region = container.CurrentRegion;

            PDFLayoutBlock block = new PDFLayoutBlock(container, owner, engine, fullstyle, split);
            region.Contents.Add(block);
            return block;
        }

        #endregion

        #region public PDFLayoutBlock BeginNewContainerBlock(PDFContainerComponent owner, IPDFLayoutEngine engine, PDFStyle fullstyle, PositionMode mode)

        /// <summary>
        /// Creates and returns a new open container block 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="engine"></param>
        /// <param name="fullstyle"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public PDFLayoutBlock BeginNewContainerBlock(ContainerComponent owner, IPDFLayoutEngine engine, Style fullstyle, PositionMode mode)
        {
            PDFLayoutBlock block = this.BeginNewBlock(owner, engine, fullstyle, mode);
            block.IsContainer = true;
            return block;
        }

        #endregion

        #region public PDFLayoutRegion BeginNewPositionedRegion(PDFPositionOptions pos, PDFLayoutPage page, IPDFComponent comp, PDFStyle full)

        /// <summary>
        /// Starts a new layout region within this block that is either relatively or absolutely positioned.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="page"></param>
        /// <param name="comp"></param>
        /// <param name="full"></param>
        /// <returns></returns>
        public PDFLayoutRegion BeginNewPositionedRegion(PDFPositionOptions pos, PDFLayoutPage page, IPDFComponent comp, Style full, bool addAssociatedRun = true)
        {
            PDFLayoutRegion before = this.CurrentRegion;
            PDFLayoutLine beforeline = before.CurrentItem as PDFLayoutLine;
            if (null == beforeline)
                beforeline = before.BeginNewLine();

            PDFRect space;
            if (pos.PositionMode == PositionMode.Absolute)
                //Page sizing
                space = new PDFRect(PDFUnit.Zero, PDFUnit.Zero, page.Width, page.Height);
            else
                //Block sizing
                space = new PDFRect(PDFPoint.Empty, this.AvailableBounds.Size);

            //Calculate the available space
            
                if (pos.X.HasValue)
                {
                    space.X += pos.X.Value;
                    space.Width -= pos.X.Value;
                }

                if (pos.Y.HasValue)
                {
                    space.Y += pos.Y.Value;
                    space.Height -= pos.Y.Value;
                }
            
            //if (pos.HasWidth)
            //{
            //    space.Width = pos.Width;
            //}

            //if (pos.HasHeight)
            //{
            //    space.Height = pos.Height;
            //}

            int index = -(this.PositionedRegions.Count + 1); //use a negative value for the positioned items

            PDFLayoutRegion created = new PDFLayoutPositionedRegion(this, comp, space, index, pos);
            this.PositionedRegions.Add(created);

            if (addAssociatedRun)
                beforeline.AddPositionedRun(created, comp);

            return created;
        }

        #endregion


        #region public override void ResetAvailableHeight(PDFUnit height, bool includeChildren)

        public override void ResetAvailableHeight(PDFUnit height, bool includeChildren)
        {
            this._avail.Height = height;
            if (includeChildren && null != this.Columns)
            {
                height -= this.Position.Margins.Top + this.Position.Margins.Bottom;

                foreach (PDFLayoutRegion region in this.Columns)
                {
                    region.ResetAvailableHeight(height, includeChildren);
                }
            }
            base.ResetAvailableHeight(height, includeChildren);
        }

        #endregion

        #region public override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)

        /// <summary>
        /// Renders this block to the PDFWriter
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            bool logdebug = context.ShouldLogDebug;
            
            Component component = this.Owner as Component;
            
            if (logdebug)
            {
                if (null != component)
                {
                    context.TraceLog.Begin(TraceLevel.Debug, "Layout Block", "Writing Block for: " + this.Owner.ID);
                }
                else
                    context.TraceLog.Begin(TraceLevel.Debug, "Layout Block", "Writing un-owned Block");
            }
            
            
            Style fullstyle = this.FullStyle;
            if (null == fullstyle)
                throw new NullReferenceException(Errors.ThisBlockDoesNotHaveAnyStyle);
            PDFPositionOptions position = this.Position;
            if (null == position)
                throw new NullReferenceException(Errors.ThisBlockDoesNotHaveAnyPostionOptions);

            bool render = this.ShouldOutput(context);
            //Check that this content should be rendered
            if (!render)
            {
                if (logdebug)
                    context.TraceLog.End(TraceLevel.Debug, "Layout Block", "Skipping the output of block " + this.ToString() + " because it has height '0' or it is not marked as visible.");
                return null;
            }

            Style prevStyle = context.FullStyle;
            PDFSize prevSize = context.Space;
            PDFPoint prevLoc = context.Offset;
            PDFObjectRef xobj = null;

            try
            {
                context.FullStyle = fullstyle;
                
                

                //If we have a transformation matrix applied.
                if(this.Position.HasTransformation)
                {
                    context.Graphics.SaveGraphicsState();
                    //distance to move the block so that any rotaion, scale or skew happens around the origin (bottom left of the shape)
                    PDFUnit offsetToOriginX = this.TotalBounds.X - context.Offset.X;
                    PDFUnit offsetToOriginY = this.TotalBounds.Y + context.Offset.Y + this.TotalBounds.Height;

                    //Defaulting to transforming around the centre of the shape.

                    offsetToOriginX -= this.TotalBounds.Width / 2;
                    offsetToOriginY -= this.TotalBounds.Height / 2;

                    //the translate to origin transformation
                    PDFTransformationMatrix offsetToOrigin = new PDFTransformationMatrix();
                    offsetToOrigin.SetTranslation(context.Graphics.GetXPosition(offsetToOriginX).Value, -context.Graphics.GetYPosition(offsetToOriginY).Value);

                    //the translate back to original location post transformation
                    PDFTransformationMatrix offsetToActual = new PDFTransformationMatrix();
                    offsetToActual.SetTranslation(- context.Graphics.GetXPosition(offsetToOriginX).Value, context.Graphics.GetYPosition(offsetToOriginY).Value);

                    //multiply the matricies into a single set
                    PDFTransformationMatrix full = offsetToActual * (this.Position.TransformMatrix * offsetToOrigin);

                    if (this.HasTransformedOffset)
                    {
                        //If we have an offset for transform (if x and y are not set and resultant transform is < 0 in x or y)

                        PDFTransformationMatrix shift = new PDFTransformationMatrix();
                        
                        //Use negative if our origin is TopLeft, and PDF always drawn from Bottom Left
                        if (context.Graphics.Origin == DrawingOrigin.TopLeft)
                            shift.SetTranslation((float)this.TransformedOffset.X.PointsValue, -(float)this.TransformedOffset.Y.PointsValue);
                        else
                            shift.SetTranslation((float)this.TransformedOffset.X.PointsValue, (float)this.TransformedOffset.Y.PointsValue);

                        context.Graphics.SetTransformationMatrix(shift, true, true);
                        
                    }
                        //finally apply the transformation again
                    context.Graphics.SetTransformationMatrix(full, true, true);



                }



                PDFRect total = this.TotalBounds;
                total = total.Offset(context.Offset);

                PDFRect borderRect = total.Inset(this.Position.Margins);
                PDFRect contentRect = borderRect.Inset(this.Position.Padding);

                //Get the border to draw
                PDFPenBorders border = this.FullStyle.CreateBorderPen();
                

                if (this.Position.OverflowAction == OverflowAction.Clip)
                {
                    if (logdebug)
                       context.TraceLog.Add(TraceLevel.Debug, "Layout Block", "Setting the clipping rectangle " + borderRect);
                    this.OutputClipping(context, borderRect, border.CornerRadius.HasValue ? border.CornerRadius.Value : 0, border.BorderSides, this.Position.ClipInset);
                }
                else if(this.Position.ClipInset.IsEmpty == false)
                {
                    if (logdebug)
                        context.TraceLog.Add(TraceLevel.Debug, "Layout Block", "Setting the clipping rectangle " + borderRect + " as we have a non-zero clipping rect");
                    this.OutputClipping(context, borderRect, border.CornerRadius.HasValue ? border.CornerRadius.Value : 0, border.BorderSides, this.Position.ClipInset);
                }

                //Get the background brush
                PDFBrush background = this.FullStyle.CreateBackgroundBrush();


                if (null != background)
                {
                    this.OutputBackground(background, border.CornerRadius.HasValue ? border.CornerRadius.Value : 0, context, borderRect);
                }

                context.Offset = contentRect.Location;
                context.Space = contentRect.Size;

                if (logdebug)
                    context.TraceLog.Add(TraceLevel.Debug, "Layout Block", "Adjusted bounds of context for block " + this.ToString() + " with offset " + context.Offset + " and space " + context.Space + ", now rendering contents");

                //Perform the atual wrting of this blocks inner conntent
                OutputInnerContent(context, writer);

                if (this.Position.OverflowAction == OverflowAction.Clip)
                {
                    if (logdebug)
                        context.TraceLog.Add(TraceLevel.Debug, "Layout Block", "Releasing the clipping rectangle " + borderRect);
                    this.ReleaseClipping(context);
                }

                if (null != border)
                {
                    if(logdebug)
                        context.TraceLog.Add(TraceLevel.Debug, "Layout Block", "Rendering border of block " + this.ToString() + " with rect " + borderRect.ToString());
                    this.OutputBorder(background, border, context, borderRect);
                }

                if (render && null != component)
                {
                    if (logdebug)
                        context.TraceLog.Add(TraceLevel.Debug, "Layout Block", "Setting the arrangement of block " + this.ToString() + " back with component " + component.UniqueID + " for content rectangle " + contentRect.ToString());

                    component.SetArrangement(context, fullstyle, contentRect);
                }

                PDFPen grid = this.FullStyle.CreateOverlayGridPen();
                if (null != grid)
                {
                    OverlayGridStyle over = this.FullStyle.OverlayGrid;
                    this.OutputOverlayGrid(grid, over, context, total);
                    if (over.HighlightColumns)
                        this.OutputRegionOverlay(over, context, contentRect);
                }
            }
            catch (PDFRenderException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new PDFRenderException("Could not output the block '" + this.ToString() + "'. " + ex.Message, ex);
            }

            if (position.HasTransformation)
                context.Graphics.RestoreGraphicsState();

            context.Space = prevSize;
            context.Offset = prevLoc;
            context.FullStyle = prevStyle;

            if (logdebug)
            {
                if (null != this.Owner)
                    context.TraceLog.End(TraceLevel.Debug, "Layout Block", "Finished writing Block for: " + this.Owner.ID);
                else
                    context.TraceLog.End(TraceLevel.Debug, "Layout Block", "Finished writing un-owned Block");
            }

            return null;
        }

        
#endregion

        #region protected virtual void OutputInnerContent(PDFRenderContext context, PDFWriter writer)

        /// <summary>
        /// Renders the inner content in this block
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        protected virtual void OutputInnerContent(PDFRenderContext context, PDFWriter writer)
        {
            
            PDFPoint prev = context.Offset;

            if (this.Columns.Length > 1)
            {
                foreach (PDFLayoutRegion region in this.Columns)
                {
                    region.OutputToPDF(context, writer);
                }
            }
            else
            {
                this.Columns[0].OutputToPDF(context, writer);
            }
        }

        #endregion

        #region protected virtual bool ShouldOutput(PDFRenderContext context)

        /// <summary>
        /// Returns true if this block should actually be written.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual bool ShouldOutput(PDFRenderContext context)
        {
            if (this.Position.Visibility == Visibility.Visible && this.ExcludeFromOutput == false)
            {
                if (this.Height > 0)
                    return true;
                
                foreach (PDFLayoutRegion reg in this.Columns)
                {
                    if (reg.Height > 0)
                        return true;
                }

                if (this.HasPositionedRegions)
                    return true;
                
                //No height or content with any height so do not render.
                return false;
            }
            else
                return false;
        }

        #endregion

        #region private void OutputRegionOverlay(PDFOverlayGridStyle grid, PDFRenderContext context, PDFRect contentRect)

        private const double ColumnOverlayOpacity = 0.4;

        /// <summary>
        /// Writes a full block for each of the regions in this block
        /// </summary>
        /// <param name="grid">The grid style to use for rendering</param>
        /// <param name="context">The current render context</param>
        /// <param name="contentRect">The content rectagle that encloses all the columns</param>
        private void OutputRegionOverlay(OverlayGridStyle grid, PDFRenderContext context, PDFRect contentRect)
        {
            if (null == grid)
                throw new ArgumentNullException("grid");

            PDFSolidBrush solid = PDFSolidBrush.Create(grid.GridColor);
            solid.Opacity = ColumnOverlayOpacity;
            PDFGraphics graphics = context.Graphics;
            PDFPoint pos = contentRect.Location;

            foreach (PDFLayoutRegion region in this.Columns)
            {
                region.OutputOverlayColor(solid, graphics, pos);
            }

        }

        #endregion

        #region private void OutputOverlayGrid(PDFOverlayGridStyle grid, PDFRenderContext context)

        /// <summary>
        /// Renders any overlay grid and column highlights
        /// </summary>
        /// <param name="grid">The grid to render</param>
        /// <param name="context"></param>
        /// <param name="rect">The rectangle to use to render the grid within</param>
        private void OutputOverlayGrid(PDFPen pen, OverlayGridStyle grid, PDFRenderContext context, PDFRect rect)
        {
            if (null == pen)
                return;

            if (null == grid)
                throw new ArgumentNullException("grid");

            PDFGraphics graphics = context.Graphics;
            

            //Will paint a 50% opacity colour over the columns in a panel
            //so they can be identified
            graphics.SaveGraphicsState();

            

            PDFRect absolutebounds = rect;
            pen.SetUpGraphics(graphics, absolutebounds);
            
            //vertical lines first
            PDFUnit space = grid.GridSpacing;
            if (space <= 0)
                throw new ArgumentOutOfRangeException("Cannot render a grid with zero or less spacing");

            PDFUnit x1 = absolutebounds.X + grid.GridXOffset;
            PDFUnit y1 = absolutebounds.Y;
            PDFUnit y2 = absolutebounds.Y + absolutebounds.Height;
            PDFUnit x2 = x1 + absolutebounds.Width;

            while (x1 < x2)
            {
                graphics.DrawLine(x1, y1, x1, y2);
                x1 += space;
            }

            x1 = absolutebounds.X;
            y1 = absolutebounds.Y + grid.GridYOffset;

            while (y1 < y2)
            {
                graphics.DrawLine(x1, y1, x2, y1);
                y1 += space;
            }

            pen.ReleaseGraphics(graphics, absolutebounds);
            graphics.RestoreGraphicsState();
        }

        #endregion

        #region internal void Offset(PDFUnit x, PDFUnit y)

        /// <summary>
        /// Offsets this block by the specifed amounts
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        internal void Offset(PDFUnit x, PDFUnit y)
        {
            PDFRect content = this.AvailableBounds;
            content.Y += y;
            content.X += x;
            this.AvailableBounds = content;
            
            PDFRect total = this.TotalBounds;
            total.Y += y;
            total.X += x;
            this.TotalBounds = total;
        }

        #endregion

        #region internal void Shrink(PDFUnit width, PDFUnit height)

        /// <summary>
        /// Reduces the size of this layout block bythe specifed amount
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        internal void Shrink(PDFUnit width, PDFUnit height)
        {
            PDFRect content = this.AvailableBounds;
            content.Width -= width;
            content.Height -= height;
            this.AvailableBounds = content;

            PDFRect total = this.TotalBounds;
            total.Width -= width;
            total.Height -= height;
            this.TotalBounds = total;

            if (this.Columns != null && this.Columns.Length > 0)
            {
                for (int i = 0; i < this.Columns.Length; i++)
                {
                    PDFLayoutRegion region = this.Columns[i];
                    content = region.TotalBounds;
                    content.Width -= width;
                    content.Height -= height;
                    region.TotalBounds = content;
                }
            }
        }

        #endregion

    }

    /// <summary>
    /// Represents a collection of blocks
    /// </summary>
    public class PDFLayoutBlockCollection : List<PDFLayoutBlock>
    {

    }
}
