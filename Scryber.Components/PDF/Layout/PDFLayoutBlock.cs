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
using Scryber.PDF.Native;
using Scryber.Components;
using Scryber.PDF.Graphics;
using Scryber.Svg.Components;

namespace Scryber.PDF.Layout
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
        public Unit[] ColumnWidths { get; private set; }

        #endregion

        #region public PDFColumnOptions ColumnOptions { get; private set; }

        /// <summary>
        /// Gets the options for the column layout in this block
        /// </summary>
        public PDFColumnOptions ColumnOptions { get; private set; }

        #endregion

        // #region public bool IsFormXObject { get; set; }
        //
        // /// <summary>
        // /// Returns true if this block should be rendered as an xObject (independent of the main content stream (or out xObject)
        // /// </summary>
        // public bool IsFormXObject { get; set; }
        //
        // #endregion

        #region public PDFRect XObjectViewPort {get;set;}

        /// <summary>
        /// Gets the view port rectangle for the xObject
        /// </summary>
        public Rect XObjectViewPort {get;set;}

        #endregion

        #region public PDFRect AvailableBounds { get; private set; }

        private Rect _avail;

        /// <summary>
        /// Gets or sets the available bounds of this block relative to the Absolute bounds
        /// </summary>
        public Rect AvailableBounds
        {
            get { return _avail; }
            set { _avail = value; }
        }

        #endregion

        #region public PDFRect TotalBounds { get; set;}

        private Rect _tot = Rect.Empty;

        /// <summary>
        /// Gets the absolute bounds (inc margins and padding) for this block
        /// </summary>
        public Rect TotalBounds 
        {
            get { return _tot; }
            set { _tot = value; }
        }

        #endregion

        #region public PDFRect TransformedBounds {get;set;} 


        private Point _transformBounds;

        /// <summary>
        /// Returns any transformation offset to be applied to the block, 
        /// </summary>
        public Point TransformedOffset
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
                return _transformBounds.IsZero == false;
            }
        }

        #endregion

        #region public PDFPositionOptions Position { get; }

        /// <summary>
        /// Gets the position options for this block
        /// </summary>
        public PDFPositionOptions Position
        {
            get; 
            protected set;
        }

        #endregion

        #region public PDFSize Size { get; set; }

        /// <summary>
        /// Gets or sets the total used size
        /// </summary>
        public Size Size { 
            get; 
            set; 
        }

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
        public Point PagePosition { get; set; }


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
        public override Unit Width
        {
            get
            {
                if (this.IsClosed)
                    return this.TotalBounds.Width;
                else
                {
                    if (this.ColumnOptions.ColumnCount == 0)
                        return Unit.Zero;
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
        public override Unit Height
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
                        return Unit.Zero;
                    else if (this.ColumnOptions.ColumnCount == 1)
                        return this.Columns[0].Height;
                    else
                    {
                        Unit h = Unit.Zero;
                        for (int i = 0; i < this.ColumnOptions.ColumnCount; i++)
                        {
                            h = Unit.Max(h, this.Columns[i].Height);
                        }
                        return h;
                    }
                }
            }
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

        private List<IComponent> _children;

        /// <summary>
        /// Gets a list of all the child components that are part of this block
        /// </summary>
        public List<IComponent> ChildComponents
        {
            get
            {
                if (null == _children)
                    _children = new List<IComponent>();
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
        public PDFLayoutBlock(PDFLayoutItem parent, IComponent owner, IPDFLayoutEngine engine, Scryber.Styles.Style fullstyle, OverflowSplit split)
            : base(parent, owner, engine, fullstyle)
        {
            this.OverflowSplit = split;
        }

        #endregion

        //
        // methods
        //

        public Unit GetPageYOffset()
        {
            return Unit.Zero;
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
        public virtual void InitRegions(Rect totalbounds, PDFPositionOptions position, PDFColumnOptions columns, PDFLayoutContext context)
        {
            var log = context.TraceLog;
            if (log.ShouldLog(TraceLevel.Debug))
                log.Add(TraceLevel.Debug, LayoutEngineBase.LOG_CATEGORY, "Initialized layout block for component " + (null == this.Owner ? "'NONE'" : this.Owner.ToString()));

            this.ColumnOptions = columns;
            this.TotalBounds = totalbounds;
            
            //Added to include margins
            Thickness pad = position.Padding;
            Thickness marg = position.Margins;
            Rect avail = new Rect(pad.Left + marg.Left, pad.Top + marg.Top, 
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

            this.Size = Size.Empty;
            this.Position = position;

            
            
            if (columns.ColumnCount == 1 && columns.ColumnWidths.IsEmpty)
            {
                this.Columns = new PDFLayoutRegion[columns.ColumnCount];
                avail = new Rect(Point.Empty, avail.Size);
                this.Columns[0] = new PDFLayoutRegion(this, this.Owner, avail, 0, position.HAlign ?? HorizontalAlignment.Left, position.VAlign ?? VerticalAlignment.Top);
            }
            else
            {
                Unit[] widths;
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
                    widths = Drawing.ColumnWidths.GetEqualColumnWidths(avail.Width, columns.AlleyWidth, count);
                }
                else
                {
                    count = 1;
                    widths = new Unit[] { avail.Width };
                }

                this.Columns = new PDFLayoutRegion[count];
                this.ColumnWidths = widths;
                this.ColumnOptions.ColumnCount = count;

                Unit x = 0;// position.Padding.Left;
                Unit y = 0;// position.Padding.Top;
                Unit h = this.AvailableBounds.Height;
                PDFLayoutRegion previous = null;

                for (int i = 0; i < this.ColumnOptions.ColumnCount; i++)
                {
                    Rect regionbounds = new Rect(x, y, widths[i], h);
                    PDFLayoutRegion column = new PDFLayoutRegion(this, this.Owner, regionbounds, i, position.HAlign ?? HorizontalAlignment.Left, position.VAlign ?? VerticalAlignment.Top);

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

            //Check the last positioned region.
            if (this.HasPositionedRegions)
            {
                reg = this.PositionedRegions[this.PositionedRegions.Count - 1];
                if(!reg.IsClosed)
                {
                    PDFLayoutBlock childlast = reg.LastOpenBlock();
                    if (null != childlast)
                        last = childlast;
                }
            }
            return last;
        }

        #endregion

        

        #region public override bool MoveToNextRegion(PDFUnit requiredHeight, PDFLayoutContext context)

        /// <summary>
        /// Attempts to move to the next region, returning true if possible, otherwise false.
        /// </summary>
        /// <returns></returns>
        public override bool MoveToNextRegion(Unit requiredHeight, PDFLayoutContext context)
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
        public bool MoveToNextRegion(bool force, Unit requiredHeight, PDFLayoutContext context)
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
        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, Unit xoffset, Unit yoffset)
        {
            bool logdebug = context.ShouldLogDebug;

            if (logdebug)
                context.TraceLog.Begin(TraceLevel.Debug, "Layout Block", "Starting to push the component arrangements for block " + this.ToString());
            
            
            //Offset our total bounds
            if (this.MovesWithLayout())
            {
                Rect total = this.TotalBounds;
                total = new Rect(xoffset + total.X, yoffset + total.Y, total.Width, total.Height);
                this.TotalBounds = total;
            }

            this.Size = this.TotalBounds.Size;
            //this.Size = this.Size.Subtract(this.Position.Padding);

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
            if (mode == PositionMode.Absolute || mode == PositionMode.Fixed)
                return false;
            else
                return true;
        }

        #endregion

        #region private PDFSize GetRequiredSize()

        /// <summary>
        /// Gets the required size of the block to fill it's content (or the explicit size)
        /// </summary>
        /// <returns></returns>
        private Size GetRequiredSize(out bool explicitHeight, out bool explicitWidth)
        {
            explicitHeight = false;
            explicitWidth = false;
            Unit w = this.Width;
            Unit h = this.Height;
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

            return new Size(w, h);
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
            Size sz = this.GetRequiredSize(out explicitHeight, out explicitWidth);
            Rect full = this.TotalBounds;
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
                    Rect one = this.Columns[i].TotalBounds;
                    one.Height = full.Height;
                    this.Columns[i].TotalBounds = one;
                }
            }
            else
            {
                Rect content = this.Columns[0].TotalBounds;
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
                        Rect content = reg.TotalBounds;
                        Size bottomright = new Size(content.X + content.Width, content.Y + content.Height);

                        if (reg.PositionOptions.Margins.IsEmpty == false && ((reg.Owner is ImageBase) == false || reg.PositionOptions.FloatMode == FloatMode.None))
                        {
                            //Images already have this applied when floating
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
        public void SetContentSize(Unit width, Unit height)
        {
            this.Size = new Size(width, height);
            Rect total = this.TotalBounds;
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
        public PDFLayoutBlock BeginNewBlock(IComponent owner, IPDFLayoutEngine engine, Style fullstyle, DisplayMode mode)
        {
            if (mode == DisplayMode.Inline)
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
        public PDFLayoutBlock BeginNewContainerBlock(ContainerComponent owner, IPDFLayoutEngine engine, Style fullstyle, DisplayMode mode)
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
        public PDFLayoutRegion BeginNewPositionedRegion(PDFPositionOptions pos, PDFLayoutPage page, IComponent comp, Style full, bool isfloating, bool addAssociatedRun = true)
        {
            
            //TODO: IMPORTANT -  Move this to the Layout engine - as we have already done the segregation on the position mode - so we can size it from there.
            
            PDFLayoutRegion before = this.CurrentRegion;
            PDFLayoutLine beforeline = before.CurrentItem as PDFLayoutLine;
            if (null == beforeline)
                beforeline = before.BeginNewLine();

            var addTo = this;
            
            Rect space;
            if(pos.PositionMode == PositionMode.Fixed)
            {
                space = new Rect(Unit.Zero, Unit.Zero, page.Width, page.Height);
            }
            else if (pos.PositionMode == PositionMode.Absolute)
            {
                //Page sizing
                space = new Rect(Unit.Zero, Unit.Zero, page.Width, page.Height);

            }
             else if (pos.DisplayMode == DisplayMode.InlineBlock)
             {
                 space = new Rect(Point.Empty, AvailableBounds.Size);
                 
                 if (this.Columns.Length > 1)
                     space.Width = this.CurrentRegion.Width;

                 // if (this.CurrentRegion != null && this.CurrentRegion.CurrentItem != null &&
                 //     this.CurrentRegion.CurrentItem is PDFLayoutLine currLine)
                 // {
                 //     if (pos.Width.HasValue)
                 //         space.Width = pos.Width.Value;
                 //     else if (pos.MinimumWidth.HasValue == false)
                 //     {
                 //         //As an inline block on a current line without specific widths
                 //         space.Width = currLine.AvailableWidth;
                 //     }
                 // }
             }
            else
                //Block sizing
                space = new Rect(Point.Empty, this.AvailableBounds.Size);

            //Calculate the available space if we are not a flao
            if (!isfloating)
            {
                if (pos.X.HasValue)
                {
                    if (pos.PositionMode == PositionMode.Static)
                        pos.X = null;
                    else
                    {
                        space.X += pos.X.Value;
                        space.Width -= pos.X.Value;
                    }
                }
                else if (pos.Right.HasValue)
                {
                    if (pos.PositionMode == PositionMode.Static)
                        pos.X = null;
                    else
                    {
                        //space.Y += pos.Y.Value; Needs sorting
                        space.Width -= pos.Right.Value;
                    }
                }

                if (pos.Y.HasValue)
                {
                    if (pos.PositionMode == PositionMode.Static)
                        pos.Y = null;
                    else
                    {
                        space.Y += pos.Y.Value;
                        space.Height -= pos.Y.Value;
                    }
                }
                else if (pos.Bottom.HasValue)
                {
                    if (pos.PositionMode == PositionMode.Static)
                        pos.Y = null;
                    else
                    {
                        //space.Y += pos.Y.Value; Needs Sorting
                        space.Height -= pos.Bottom.Value;
                    }
                }
            }
            else
            {
                if (pos.X.HasValue)
                {
                    space.X = pos.X.Value;
                    space.Width -= pos.X.Value;
                }
                else if (pos.Right.HasValue)
                {
                    space.Width -= pos.Right.Value;
                }
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


            PDFLayoutPositionedRegion created = new PDFLayoutPositionedRegion(this, comp, space, index, pos);
            
            addTo.PositionedRegions.Add(created);

            if (addAssociatedRun)
            {
                PDFLayoutPositionedRegionRun run; 
                if ((pos.PositionMode == PositionMode.Static || pos.PositionMode == PositionMode.Relative) && pos.DisplayMode == DisplayMode.InlineBlock)
                    run = beforeline.AddInlineBlockRun(created, comp);
                else
                {
                    run = beforeline.AddPositionedRun(created, comp);
                    run.IsFloating = isfloating;
                }
               
                if (pos.XObjectRender)
                {
                    run.RenderAsXObject = true;
                    run.OutputName = (PDFName)this.Owner.Document.GetIncrementID(ObjectTypes.CanvasXObject);
                    var rsrc = new Resources.PDFLayoutXObjectResource(Resources.PDFResource.XObjectResourceType, this.ToString(), run);;
                    addTo.GetLayoutPage().PageOwner.Register(rsrc);
                    this.Owner.Document.EnsureResource(rsrc.ResourceType, rsrc.ResourceKey, rsrc);
                }
            }
            return created;
        }

        #endregion


        #region public override void ResetAvailableHeight(PDFUnit height, bool includeChildren)

        public override void ResetAvailableHeight(Unit height, bool includeChildren)
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
            
            var oref = this.DoOutputAsBlock(context, writer);
            return oref;
            

            //This has moved into the DoOutputAsBlock
            //Leaving as comments to validate before removal.
            
            /*
             
             
            Style prevStyle = context.FullStyle;
            Size prevSize = context.Space;
            Point prevLoc = context.Offset;
            //PDFObjectRef xobj = null;

            try
            {
                context.FullStyle = fullstyle;

                //If we have a transformation matrix applied.
                if (position.HasTransformation)
                {
                    if (context.ShouldLogDebug)
                        context.TraceLog.Begin(TraceLevel.Verbose, LOG_CATEGORY, "Setting the transformation matrix (including offsets) for " + this.Owner.ToString() + " and any ");

                    else if (context.ShouldLogVerbose)
                        context.TraceLog.Add(TraceLevel.Verbose, LOG_CATEGORY, "Setting the Graphics state transformation matrix for " + this.Owner.ToString() + " and any children to " + this.Position.TransformMatrix);

                    context.Graphics.SaveGraphicsState();

                    //Start with the original transformation matrix
                    PDFTransformationMatrix full = this.Position.TransformMatrix.Clone(); // offsetToActual * (this.Position.TransformMatrix * offsetToOrigin);
                    if (full.IsIdentity)
                    {
                        //Do Nothing
                    }
                    else if (full.Transformations == MatrixTransformTypes.IsTranslation && this.Owner is Svg.Components.SVGText)
                    {
                        //TODO: Take this out and make the SVGText a Component run rather than a block.
                        //Just a translation of some text so quicker to just offset the translation by the height of the block
                        var offsetX = 0;
                        var offsetY = this.TotalBounds.Height.PointsValue;
                        full = full.Clone();

                        float posOffsetX = 0.0F;
                        float posOffsetY = 0.0F;

                        //set any explicit position offsets

                        

                        full.SetTranslation(offsetX + posOffsetX, offsetY - posOffsetY);

                        context.Graphics.SetTransformationMatrix(full, true, true);
                        this.Position.TransformMatrix = full;
                        this.TransformedOffset = new Point(full.Components[4], full.Components[5]);
                    }
                    else
                    {

                        //distance to move the block so that any rotaion, scale or skew happens around the origin (bottom left of the shape)
                        Unit offsetToOriginX = this.TotalBounds.X - context.Offset.X;
                        Unit offsetToOriginY = this.TotalBounds.Y + context.Offset.Y + this.TotalBounds.Height;
                        //offsetToOriginY -= context.PageSize.Height.PointsValue;
                        if (position.X.HasValue)
                            offsetToOriginX += 0.0; // position.X.Value;
                        if (position.Y.HasValue)
                            offsetToOriginY += 0.0;// position.Y.Value;

                        //Defaulting to transforming around the centre of the shape if we have a rotation

                        offsetToOriginX += this.TotalBounds.Width / 2;
                        offsetToOriginY -= this.TotalBounds.Height / 2;


                        float actualOffsetX = (float)context.Graphics.GetXPosition(offsetToOriginX).Value;
                        float actualOffsetY = (float)context.Graphics.GetYPosition(offsetToOriginY).Value;


                        if (context.ShouldLogDebug)
                            context.TraceLog.Add(TraceLevel.Debug, LOG_CATEGORY, "Transformation matrix to move to origin calculated to (" + actualOffsetX + ", " + actualOffsetY + ")");



                        float posOffsetX = 0.0F;
                        float posOffsetY = 0.0F;

                        //set any explicit position offsets

                        if (position.X.HasValue)
                        {
                            posOffsetX = ((float)position.X.Value.PointsValue);
                        }
                        if (position.Y.HasValue)
                        {
                            posOffsetY = ((float)position.Y.Value.PointsValue);
                        }

                        //Set the translation to the origin and the explicit position
                        full.SetTranslation(actualOffsetX + posOffsetX, actualOffsetY - posOffsetY);

                        if (context.ShouldLogDebug)
                            context.TraceLog.Add(TraceLevel.Warning, LOG_CATEGORY, "Final transformation matrix to move to, transform, and move back from origin calculated to " + full);

                        //mark all future drawing offsets - as these will happen from the page origin now (bottom left)
                        //within the centre of the container

                        context.Graphics.SaveTranslationOffset(
                            actualOffsetX,
                            actualOffsetY);

                        if (context.ShouldLogDebug)
                            context.TraceLog.Add(TraceLevel.Warning, LOG_CATEGORY, "Translation offset set to " + (actualOffsetX).ToString() + ", " + (actualOffsetY).ToString());

                        //apply the actual transformation
                        context.Graphics.SetTransformationMatrix(full, true, true);
                        //save state

                        //Save the newly caclulated values back on the block.
                        this.Position.TransformMatrix = full;
                        this.TransformedOffset = new Point(actualOffsetX, actualOffsetY);

                    }
                }


                else if (position.PositionMode == PositionMode.Relative)
                {
                    var offset = context.Offset;
                    if (position.X.HasValue)
                    {
                        offset.X += position.X.Value;
                    }
                    else if (position.Right.HasValue)
                    {
                        offset.X -= position.Right.Value;
                    }
                    if (position.Y.HasValue)
                    {
                        offset.Y += position.Y.Value;
                    }
                    else if(position.Bottom.HasValue)
                    {
                        offset.Y -= position.Bottom.Value;
                    }
                    context.Offset = offset;

                }


                Rect total = this.TotalBounds;
                total = total.Offset(context.Offset);

                Rect borderRect = total.Inset(this.Position.Margins);
                Rect contentRect = borderRect.Inset(this.Position.Padding);

                //Get the border to draw
                PDFPenBorders border = this.FullStyle.CreateBorderPen();

                this.PagePosition = total.Location;

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

                    if (null != context.RenderMatrix)
                        borderRect = context.RenderMatrix.TransformBounds(borderRect);
                    component.SetArrangement(context, fullstyle, borderRect);
                }
                
                PDFPen grid = this.FullStyle.CreateOverlayGridPen();
                
                if (null != grid)
                {
                    PDFPen major = this.FullStyle.CreateOverlayGridPen(forMajor: true);
                    OverlayGridStyle over = this.FullStyle.OverlayGrid;
                    this.OutputOverlayGrid(grid, major, over, context, total);
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
            {
                //We have a transformation, so restore it to the original state
                context.Graphics.RestoreTranslationOffset();
                context.Graphics.RestoreGraphicsState();
                

                if (context.ShouldLogDebug)
                    context.TraceLog.End(TraceLevel.Warning, LOG_CATEGORY, "Reset the graphics state transformation for " + this.Owner.ToString());
            }

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
            
            */
        }

        
        #endregion

        protected virtual PDFObjectRef DoOutputToXObject(PDFRenderContext context, PDFWriter writer)
        {
            using (var renderer = this.CreateXObject(context, writer))
            {
                var bounds = this.TotalBounds;
                var pos = this.FullStyle.CreatePostionOptions(true);
                
                if (pos.ViewPort.HasValue)
                {
                    var vp = pos.ViewPort.Value;
                    bounds.Width = vp.Width;
                    bounds.Height = vp.Height;
                    this.TotalBounds = bounds;
                }
                else
                {
                    bounds.Width -= this.Position.Margins.Left + this.Position.Margins.Right;
                    bounds.Height -= this.Position.Margins.Top + this.Position.Margins.Bottom;
                }
                
                renderer.SetupGraphics(this.Position, bounds);
                
                this.OutputInnerContent(context, writer);

                renderer.ReleaseGraphics();
                
                return renderer.RenderReference;
                
            }
        }

        private PDFXObjectRenderer CreateXObject(PDFRenderContext context, PDFWriter writer)
        {
            return new PDFXObjectRenderer(this.Owner, this, this.Position, context, writer);
        }

        protected virtual PDFObjectRef DoOutputAsBlock(PDFRenderContext context, PDFWriter writer)
        {
            bool logdebug = context.ShouldLogDebug;
            bool hasTransform = false;
            bool hasClipping = false;
            Style prevStyle = context.FullStyle;
            Size prevSize = context.Space;
            Point prevLoc = context.Offset;
            var component = this.Owner as Component;
            
            try
            {
                context.FullStyle = this.FullStyle;

                if (this.Position.HasTransformation)
                {
                    hasTransform = this.SetupBlockTransformation(context, writer);
                }
                else if (this.Position.PositionMode == PositionMode.Relative)
                {
                    this.UpdateOffsetForRelativeBlock(context, writer);
                }
                
                Rect total = this.TotalBounds;
                total = total.Offset(context.Offset);
                
                if (this.Owner is SVGText svgText)
                {
                    if (svgText.DeltaX != Unit.Zero)
                    {
                        total.X += svgText.DeltaX;
                    }

                    if (svgText.DeltaY != Unit.Zero)
                    {
                        total.Y += svgText.DeltaY;
                    }
                }

                Rect borderRect = total.Inset(this.Position.Margins);
                Rect contentRect = borderRect.Inset(this.Position.Padding);
                
                //save this location before rendering any inner content as the actual point on the page.
                this.PagePosition = total.Location;
                
                //Get the border to draw
                PDFPenBorders border = this.FullStyle.CreateBorderPen();
                //Get the background brush
                PDFBrush background = this.FullStyle.CreateBackgroundBrush();
                
                //set up any clipping
                if (this.Position.OverflowAction == OverflowAction.Clip)
                {
                    if (logdebug)
                        context.TraceLog.Add(TraceLevel.Debug, "Layout Block", "Setting the clipping rectangle " + borderRect);
                    
                    hasClipping = this.OutputClipping(context, borderRect, border.CornerRadius.HasValue ? border.CornerRadius.Value : 0, border.BorderSides, this.Position.ClipInset);
                    
                }
                else if(this.Position.ClipInset.IsEmpty == false)
                {
                    if (logdebug)
                        context.TraceLog.Add(TraceLevel.Debug, "Layout Block", "Setting the clipping rectangle " + borderRect + " as we have a non-zero clipping rect");
                    
                    hasClipping  = this.OutputClipping(context, borderRect, border.CornerRadius.HasValue ? border.CornerRadius.Value : 0, border.BorderSides, this.Position.ClipInset);
                }
                
                if (null != background)
                {
                    this.OutputBackground(background, border.CornerRadius.HasValue ? border.CornerRadius.Value : 0, context, borderRect);
                }
                
                //update the offsets and size to our content rect
                
                
                context.Offset = contentRect.Location;
                
                
                
                context.Space = contentRect.Size;
                
                //Perform the atual writing of this blocks inner conntent
                if (this.ShouldOutputAsXObject(context))
                {
                    this.DoOutputToXObject(context, writer);
                }
                else
                {
                    this.OutputInnerContent(context, writer);
                }

                //restore the state in reverse order.
                
                if (hasClipping)
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
                
                if (null != component)
                {
                    if (logdebug)
                        context.TraceLog.Add(TraceLevel.Debug, "Layout Block", "Setting the arrangement of block " + this.ToString() + " back with component " + component.UniqueID + " for content rectangle " + contentRect.ToString());

                    if (null != context.RenderMatrix)
                        borderRect = context.RenderMatrix.TransformBounds(borderRect);

                    var block = this.GetParentBlock();

                    if (null != block && block.IsExplicitLayout)
                    {
                        //we are explicit so we are always zero - need to take into account page locations

                        borderRect.X += block.PagePosition.X + block.Position.Margins.Left;
                        borderRect.Y += block.PagePosition.Y + block.Position.Margins.Top;
                    }

                    component.SetArrangement(context, this.FullStyle, borderRect);
                }
                
                
                PDFPen grid = this.FullStyle.CreateOverlayGridPen();
                //If we need to draw the overlay grid - do it last
                
                if (null != grid)
                {
                    PDFPen major = this.FullStyle.CreateOverlayGridPen(forMajor: true);
                    
                    OverlayGridStyle over = this.FullStyle.OverlayGrid;
                    
                    this.OutputOverlayGrid(grid, major, over, context, total);
                    if (over.HighlightColumns)
                        this.OutputRegionOverlay(over, context, contentRect);
                }
                
                
                if (hasTransform)
                {
                    if (logdebug)
                        context.TraceLog.Add(TraceLevel.Debug, "Layout Block", "Restoring the transformation matrix for " + this.ToString());
                    
                    this.TeardownBlockTransformation(context, writer);
                }

            }
            catch (PDFRenderException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new PDFRenderException(
                    "Could not output the block '" + this.ToString() + "'. See the inner exception for more details.",
                    ex);
            }
            finally
            {
                context.Space = prevSize;
                context.Offset = prevLoc;
                context.FullStyle = prevStyle;
            }

            if (logdebug)
            {
                if (null != this.Owner)
                    context.TraceLog.End(TraceLevel.Debug, "Layout Block", "Finished writing Block for: " + this.Owner.ID);
                else
                    context.TraceLog.End(TraceLevel.Debug, "Layout Block", "Finished writing un-owned Block");
            }

            return null;
        }
        
        

        #region protected virtual void OutputInnerContent(PDFRenderContext context, PDFWriter writer)

        /// <summary>
        /// Renders the inner content regions in this block
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        protected virtual void OutputInnerContent(PDFRenderContext context, PDFWriter writer)
        {
            
            Point prev = context.Offset;

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

        /// <summary>
        /// Checks the state of the position and render options, and returns true, if this block should be rendered as an xObject (outside of the current stream of drawing instructions)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual bool ShouldOutputAsXObject(PDFRenderContext context)
        {
            var xObj = false;
            
            if (this.Position.XObjectRender)
            {
                return true;
                //we should be as an xObject.
                
                //if we are relative or static and displayed as a block then we should do it here.
                if (this.Position.PositionMode == PositionMode.Relative ||
                    this.Position.PositionMode == PositionMode.Static)
                {
                    if (this.Position.DisplayMode == DisplayMode.Block)
                        xObj = true;
                }
                else
                {
                    //the xObject rendering should be handled by a positioned region run that references this block.
                }
            }

            return xObj;
        }

        #region protected virtual void UpdateOffsetForRelativeBlock(PDFRenderContext context, PDFWriter writer)

        /// <summary>
        /// Applies any relative mode positioning to the current context offset, so further rendering will be done at the changed location.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        protected virtual void UpdateOffsetForRelativeBlock(PDFRenderContext context, PDFWriter writer)
        {
            var position = this.Position;
            var offset = context.Offset;

            if (position.X.HasValue)
            {
                offset.X += position.X.Value;
            }
            else if (position.Right.HasValue)
            {
                offset.X -= position.Right.Value;
            }

            if (position.Y.HasValue)
            {
                if (this.Position.DisplayMode == DisplayMode.Block) //special case - otherwise this is usually taken care of in the positioned region run
                    offset.Y += position.Y.Value; //when we are block display mode then we don't have a positioned region
            }
            else if (position.Bottom.HasValue)
            {
                offset.Y -= position.Bottom.Value;
            }


            context.Offset = offset;
        }

        #endregion
        
        #region SetupBlockTransformation / TeardownBlockTransformation

        /// <summary>
        /// Sets up the current transformation matrix on the 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected bool SetupBlockTransformation(PDFRenderContext context, PDFWriter writer)
        {
            var transformed = false;
            
            if (context.ShouldLogDebug)
                context.TraceLog.Begin(TraceLevel.Verbose, LOG_CATEGORY,
                    "Setting the transformation matrix (including offsets) for " + this.Owner.ToString() + " and any ");

            else if (context.ShouldLogVerbose)
                context.TraceLog.Add(TraceLevel.Verbose, LOG_CATEGORY,
                    "Setting the Graphics state transformation matrix for " + this.Owner.ToString() +
                    " and any children to " + this.Position.TransformMatrix);

            if (null == this.Position.TransformMatrix)
                throw new NullReferenceException(
                    "The was no PDF Transformation matrix on the position to set up the Block transformation - method should not be called in this state.");

            //Start with the original transformation matrix
            PDFTransformationMatrix
                full = this.Position.TransformMatrix
                    .Clone(); // offsetToActual * (this.Position.TransformMatrix * offsetToOrigin);
            
            if (full.IsIdentity)
            {
                //Do Nothing
            }
            else if (full.Transformations == MatrixTransformTypes.IsTranslation && this.Owner is Svg.Components.SVGText)
            {
                //TODO: Take this out and make the SVGText a Component run rather than a block.
                //Just a translation of some text so quicker to just offset the translation by the height of the block
                var offsetX = 0;
                var offsetY = this.TotalBounds.Height.PointsValue;
                full = full.Clone();

                float posOffsetX = 0.0F;
                float posOffsetY = 0.0F;

                //save the graphics state
                context.Graphics.SaveGraphicsState();
                
                //set any explicit position offsets

                full.SetTranslation(offsetX + posOffsetX, offsetY - posOffsetY);

                context.Graphics.SetTransformationMatrix(full, true, true);
                this.Position.TransformMatrix = full;
                this.TransformedOffset = new Point(full.Components[4], full.Components[5]);
                
                //we want to return true - to tear it down afterwards
                transformed = true;
            }
            else
            {

                //distance to move the block so that any rotaion, scale or skew happens around the origin (bottom left of the shape)
                Unit offsetToOriginX = this.TotalBounds.X - context.Offset.X;
                Unit offsetToOriginY = this.TotalBounds.Y + context.Offset.Y + this.TotalBounds.Height;
                //offsetToOriginY -= context.PageSize.Height.PointsValue;
                if (this.Position.X.HasValue)
                    offsetToOriginX += 0.0; // position.X.Value;
                if (this.Position.Y.HasValue)
                    offsetToOriginY += 0.0; // position.Y.Value;

                //Defaulting to transforming around the centre of the shape if we have a rotation

                offsetToOriginX += this.TotalBounds.Width / 2;
                offsetToOriginY -= this.TotalBounds.Height / 2;


                float actualOffsetX = (float)context.Graphics.GetXPosition(offsetToOriginX).Value;
                float actualOffsetY = (float)context.Graphics.GetYPosition(offsetToOriginY).Value;


                if (context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Debug, LOG_CATEGORY,
                        "Transformation matrix to move to origin calculated to (" + actualOffsetX + ", " +
                        actualOffsetY + ")");



                float posOffsetX = 0.0F;
                float posOffsetY = 0.0F;

                //set any explicit position offsets

                if (this.Position.X.HasValue)
                {
                    posOffsetX = ((float)this.Position.X.Value.PointsValue);
                }

                if (this.Position.Y.HasValue)
                {
                    posOffsetY = ((float)this.Position.Y.Value.PointsValue);
                }

                //Set the translation to the origin and the explicit position
                full.SetTranslation(actualOffsetX + posOffsetX, actualOffsetY - posOffsetY);

                if (context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Warning, LOG_CATEGORY,
                        "Final transformation matrix to move to, transform, and move back from origin calculated to " +
                        full);

                //save the current state
                
                context.Graphics.SaveGraphicsState();
                
                //mark all future drawing offsets - as these will happen from the page origin now (bottom left)
                //within the centre of the container

                context.Graphics.SaveTranslationOffset(
                    actualOffsetX,
                    actualOffsetY);

                if (context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Warning, LOG_CATEGORY,
                        "Translation offset set to " + (actualOffsetX).ToString() + ", " + (actualOffsetY).ToString());

                //apply the actual transformation
                context.Graphics.SetTransformationMatrix(full, true, true);
                //save state

                //Save the newly caclulated values back on the block.
                this.Position.TransformMatrix = full;
                this.TransformedOffset = new Point(actualOffsetX, actualOffsetY);
                transformed = true;
            }

            return transformed;
        }
        
        /// <summary>
        /// Tears down any previous transformation - should only be called if the SetupBlockTransformation returned true
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        protected void TeardownBlockTransformation(PDFRenderContext context, PDFWriter writer)
        {
            try
            {
                context.Graphics.RestoreTranslationOffset();
                context.Graphics.RestoreGraphicsState();

                if (context.ShouldLogDebug)
                    context.TraceLog.End(TraceLevel.Warning, LOG_CATEGORY,
                        "Reset the graphics state transformation for " + this.Owner.ToString());
            }
            catch (Exception)
            {
                ;//suck it up - we don't want this to propogate
            }
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
        private void OutputRegionOverlay(OverlayGridStyle grid, PDFRenderContext context, Rect contentRect)
        {
            if (null == grid)
                throw new ArgumentNullException("grid");

            PDFSolidBrush solid = PDFSolidBrush.Create(grid.GridColor);
            solid.Opacity = ColumnOverlayOpacity;
            var graphics = context.Graphics;
            Point pos = contentRect.Location;

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
        private void OutputOverlayGrid(PDFPen minor, PDFPen major, OverlayGridStyle grid, PDFRenderContext context, Rect rect)
        {
            if (null == minor)
                return;

            if (null == grid)
                throw new ArgumentNullException("grid");

            var graphics = context.Graphics;

            
            //Will paint a 50% opacity colour over the columns in a panel
            //so they can be identified
            graphics.SaveGraphicsState();

            

            Rect absolutebounds = rect;
            minor.SetUpGraphics(graphics, absolutebounds);
            var factor = grid.GridMajorCount > 0 ?  grid.GridMajorCount : int.MaxValue;
            
            //vertical lines first
            Unit space = grid.GridSpacing;
            
            if (space <= 0)
                throw new ArgumentOutOfRangeException("Cannot render a grid with zero or less spacing");

            Unit x1 = absolutebounds.X + grid.GridXOffset;
            Unit y1 = absolutebounds.Y;
            Unit y2 = absolutebounds.Y + absolutebounds.Height;
            Unit x2 = x1 + absolutebounds.Width;

            int index = 0;
            while (x1 < x2)
            {
                if (index % factor != 0)
                {
                    graphics.DrawLine(x1, y1, x1, y2);
                }
                x1 += space;
                index++;
            }

            x1 = absolutebounds.X;
            y1 = absolutebounds.Y + grid.GridYOffset;
            index = 0;
            
            while (y1 < y2)
            {
                if (index % factor != 0)
                {
                    graphics.DrawLine(x1, y1, x2, y1);
                }

                y1 += space;
                index++;
            }

            minor.ReleaseGraphics(graphics, absolutebounds);
            graphics.RestoreGraphicsState();

            if (major != null && factor < int.MaxValue)
            {
                graphics.SaveGraphicsState();
                major.SetUpGraphics(graphics, absolutebounds);
                
                x1 = absolutebounds.X + grid.GridXOffset;
                y1 = absolutebounds.Y;
                y2 = absolutebounds.Y + absolutebounds.Height;
                x2 = x1 + absolutebounds.Width;

                index = 0;
                while (x1 < x2)
                {
                    if (index % factor == 0)
                    {
                        graphics.DrawLine(x1, y1, x1, y2);
                    }

                    x1 += space;
                    index++;
                }

                x1 = absolutebounds.X;
                y1 = absolutebounds.Y + grid.GridYOffset;
                index = 0;

                while (y1 < y2)
                {
                    if (index % factor == 0)
                    {
                        graphics.DrawLine(x1, y1, x2, y1);
                    }

                    y1 += space;
                    index++;
                }
                
                minor.ReleaseGraphics(graphics, absolutebounds);
                graphics.RestoreGraphicsState();
            }


        }

        #endregion

        #region internal void Offset(PDFUnit x, PDFUnit y)

        /// <summary>
        /// Offsets this block by the specifed amounts
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        internal void Offset(Unit x, Unit y)
        {
            Rect content = this.AvailableBounds;
            content.Y += y;
            content.X += x;
            this.AvailableBounds = content;
            
            Rect total = this.TotalBounds;
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
        internal void Shrink(Unit width, Unit height)
        {
            Rect content = this.AvailableBounds;
            content.Width -= width;
            content.Height -= height;
            this.AvailableBounds = content;

            Rect total = this.TotalBounds;
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

}
