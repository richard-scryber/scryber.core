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
    public class LayoutEngineList : LayoutEngineBase
    {
        //
        // const
        //

        private const string ListEngineLogCategory = "List Layout Engine";
        private static readonly PDFUnit DefaultListItemAlley = 10;
        public static readonly PDFUnit DefaultNumberWidth = Const.DefaultListNumberInset;
        public const HorizontalAlignment DefaultListItemAlignment = HorizontalAlignment.Right;

        #region ivars

        private PDFLayoutBlock _itemblock;
        private PDFLayoutBlock _listBlock;
        private PDFUnit _itemoffset = PDFUnit.Zero;
        private PDFUnit _itemNumberWidth = PDFUnit.Zero;
        private List _list;
        private List<ListNumberEntry> _entries;

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

        //
        // .ctor
        //

        #region public LayoutEngineList(PDFList list, IPDFLayoutEngine parent)

        public LayoutEngineList(List list, IPDFLayoutEngine parent)
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

            this.ContinueLayout = true;

            this.CurrentBlock = this.Context.DocumentLayout.CurrentPage.LastOpenBlock();
            if (this.CurrentBlock.CurrentRegion != null && this.CurrentBlock.CurrentRegion.HasOpenItem)
                this.CurrentBlock.CurrentRegion.CloseCurrentItem();

            PDFPositionOptions pos = this.FullStyle.CreatePostionOptions();
            
            
            //Set up the outer container block that will hold the list and all it's items
            _listBlock = this.CurrentBlock.BeginNewContainerBlock(this.List, this, this.FullStyle, pos.PositionMode);
            PDFRect bounds = this.CurrentBlock.CurrentRegion.UnusedBounds;

            if (bounds.X > 0)
                bounds.X = PDFUnit.Zero;
            
            if (pos.Width.HasValue)
                bounds.Width = pos.Width.Value;
            else if (pos.Margins.IsEmpty == false)
                bounds.Width -= pos.Margins.Left + pos.Margins.Right;
            

            if (pos.Height.HasValue)
                bounds.Height = pos.Height.Value;
            else if (pos.Margins.IsEmpty == false)
                bounds.Height -= pos.Margins.Top + pos.Margins.Bottom;

            PDFColumnOptions columnOptions = new PDFColumnOptions() { AlleyWidth = PDFUnit.Zero, AutoFlow = false, ColumnCount = 1 };
            _listBlock.InitRegions(bounds, pos, columnOptions, this.Context);

            this.OpenListNumbering();

            this.BuildListEntries(out _itemNumberWidth);
            this.LayoutListItems(pos);

            _listBlock.CurrentRegion.CloseCurrentItem();
            _listBlock.CurrentRegion.Close();
            _listBlock.Close();

            this.CurrentBlock.CurrentRegion.AddToSize(_listBlock);

            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.End(TraceLevel.Debug, ListEngineLogCategory, string.Format("Completed the layout of the list {0}", this.List.ID));

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
        }

        #endregion

        //
        // List entry building
        //

        #region private int BuildListEntries(PDFListStyle style, out PDFUnit width)

        /// <summary>
        /// Builds all the ListNumberEntries for all the ListItems in this lists items and stores 
        /// them in a reference in this engine
        /// </summary>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private int BuildListEntries(out PDFUnit width)
        {
            int index = 0;
            HorizontalAlignment halign = this.FullStyle.GetValue(StyleKeys.ListAlignmentKey, DefaultListItemAlignment);
            PDFUnit defaultWidth = this.FullStyle.GetValue(StyleKeys.ListInsetKey, Const.DefaultListNumberInset);


            width = defaultWidth;

            this._entries = new List<ListNumberEntry>();

            foreach (ListItem item in this.List.Items)
            {
                if (item.Visible)
                {
                    Style applied, full;

                    if (string.IsNullOrEmpty(item.DataStyleIdentifier) 
                        || !this.DocumentLayout.TryGetStyleWithIdentifier(item.DataStyleIdentifier, out applied, out full))
                    {
                        this.Context.PerformanceMonitor.Begin(PerformanceMonitorType.Style_Build);
                        applied = item.GetAppliedStyle();
                        this.StyleStack.Push(applied);
                        full = this.StyleStack.GetFullStyle(item);

                        if (!string.IsNullOrEmpty(item.DataStyleIdentifier))
                            this.DocumentLayout.SetStyleWithIdentifier(item.DataStyleIdentifier, applied, full);

                        this.Context.PerformanceMonitor.End(PerformanceMonitorType.Style_Build);
                    }
                    else
                        this.StyleStack.Push(applied);

                    if(full.Position.PositionMode == PositionMode.Invisible)
                    {
                        this.StyleStack.Pop();

                        if (this.Context.ShouldLogDebug)
                            this.Context.TraceLog.Add(TraceLevel.Debug, "Layout", "Skipping over the layout of list item '" + item.UniqueID + "' as it is invisible");

                        continue;
                    }

                    PDFUnit itemWidth;
                    ListNumberingGroupStyle numberStyle;
                    HorizontalAlignment itemHAlign = halign;
                    string text;
                    Component itemNumber = BuildAListNumberComponent(item, full, ref itemHAlign, out numberStyle, out itemWidth, out text);

                    if (itemWidth < PDFUnit.Zero)
                        itemWidth = defaultWidth;
                    else
                        width = PDFUnit.Max(width, itemWidth);

                    ListNumberEntry entry = new ListNumberEntry()
                    {
                        NumberComponent = itemNumber,
                        NumberGroupStyle = numberStyle,
                        ListItem = item,
                        NumberWidth = itemWidth,
                        FullStyle = full,
                        AppliedStyle = applied,
                        NumberIndex = index,
                        NumberAlignment = itemHAlign,
                        NumberText = text
                    };
                    
                    this._entries.Add(entry);
                    index++;

                    this.StyleStack.Pop();
                }
            }
            return index;
        }

        #endregion

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
        private Component BuildAListNumberComponent(ListItem item, Style itemstyle, ref HorizontalAlignment halign, 
            out ListNumberingGroupStyle type, out PDFUnit itemWidth, out string text)
        {
            PDFListNumbering numbers = this.Component.Document.ListNumbering;
            type = numbers.CurrentGroup.Style;
            itemWidth = itemstyle.GetValue(StyleKeys.ListInsetKey, (PDFUnit)(-1));
            text = itemstyle.GetValue(StyleKeys.ListLabelKey, string.Empty);
            halign = itemstyle.GetValue(StyleKeys.ListAlignmentKey, halign);

            ListItemLabel label;

            if (type == ListNumberingGroupStyle.Labels)
            {
                PDFListDefinitionItemLabel defn = new PDFListDefinitionItemLabel();
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
            


            return label;
        }

        #endregion

        //
        // item layout
        //

        #region private void LayoutListItems(PDFPositionOptions listPosOpts)

        /// <summary>
        /// Lays out all the list item entries in sequence
        /// </summary>
        /// <param name="listPosOpts"></param>
        private void LayoutListItems(PDFPositionOptions listPosOpts)
        {
            int index = 1;
            _itemoffset = 0;
            foreach (ListNumberEntry entry in this._entries)
            {
                PDFUnit h = this.LayoutAnItem(index, entry, listPosOpts);
                _itemoffset += h;
                index++;

                if (this.ContinueLayout == false
                    || this.DocumentLayout.CurrentPage.IsClosed
                    || this.DocumentLayout.CurrentPage.CurrentBlock == null)

                    break;
            }
        }

        #endregion

        #region private PDFUnit LayoutAnItem(int index, ListNumberEntry entry, PDFPositionOptions listPosOpts)

        /// <summary>
        /// Lays out a single list item based on the entry, and the list position options
        /// </summary>
        /// <param name="index"></param>
        /// <param name="entry"></param>
        /// <param name="listPosOpts"></param>
        /// <returns></returns>
        private PDFUnit LayoutAnItem(int index, ListNumberEntry entry, PDFPositionOptions listPosOpts)
        {
            //restore the items applied style onto the stack
            this.StyleStack.Push(entry.AppliedStyle);
            
            Style full = entry.FullStyle;
            PDFUnit numberWidth = entry.NumberWidth;



            PDFArtefactRegistrationSet artefacts = entry.ListItem.RegisterLayoutArtefacts(this.Context, full);

            PDFPositionOptions itemopts = full.CreatePostionOptions();

            PDFUnit pageHeight = this.Context.DocumentLayout.CurrentPage.Height;
            PDFUnit h = pageHeight;
            PDFUnit w = _listBlock.AvailableBounds.Width;
            PDFUnit y = _itemoffset;

            PDFUnit alley = DefaultListItemAlley;

            if (itemopts.HasAlleyWidth)
                alley = itemopts.AlleyWidth;
            else if (listPosOpts.HasAlleyWidth)
                alley = listPosOpts.AlleyWidth;

            if (itemopts.Height.HasValue)
                h = itemopts.Height.Value;
            else if (itemopts.Margins.IsEmpty == false)
            {
                h -= itemopts.Margins.Top + itemopts.Margins.Bottom;
            }
            h -= itemopts.Padding.Top + itemopts.Padding.Bottom;

            if (itemopts.Width.HasValue)
                w = itemopts.Width.Value;

            else if (itemopts.Margins.IsEmpty == false) 
            {
                w -= itemopts.Margins.Left + itemopts.Margins.Right;
            }
            w -= itemopts.Padding.Left + itemopts.Padding.Right;

            PDFRect totalBounds = new PDFRect(PDFUnit.Zero,y,w,h);

            this._itemblock = _listBlock.BeginNewContainerBlock(entry.ListItem, this, full, itemopts.PositionMode);

            PDFColumnOptions colOpts = new PDFColumnOptions() { AlleyWidth = alley, AutoFlow = false, ColumnCount = 2 };
            this._itemblock.InitRegions(totalBounds, itemopts, colOpts, this.Context);
            
            //Alter the widths of the regions to allow for only the number width
            
            PDFRect region1bounds = this._itemblock.Columns[0].TotalBounds;
            PDFUnit difference = region1bounds.Width - numberWidth;
            region1bounds.Width = numberWidth;
            this._itemblock.Columns[0].TotalBounds = region1bounds;
            this._itemblock.Columns[0].HAlignment = entry.NumberAlignment;

            PDFRect region2Bounds = this._itemblock.Columns[1].TotalBounds;
            if (region2Bounds.X > 0)
                region2Bounds.X -= difference;
            region2Bounds.Width += difference;
            this._itemblock.Columns[1].TotalBounds = region2Bounds;

            PDFUnit numberHeight = this.LayoutItemNumber(entry, full);
            
            this._itemblock.CurrentRegion.Close();

            bool success = this._itemblock.MoveToNextRegion(true, PDFUnit.Zero, this.Context); //Pass Zero as we are not interested in overflowing yet
            PDFUnit contentHeight = this.LayoutItemContent(entry);

            //check that we can fit - addind the margins and padding back in.
            PDFUnit itemHeight = PDFUnit.Max(numberHeight, contentHeight);
            if (itemopts.Height.HasValue)
                itemHeight = itemopts.Height.Value;
            else if (itemopts.Margins.IsEmpty == false)
            {
                itemHeight += itemopts.Margins.Top + itemopts.Margins.Bottom;
            }
            itemHeight += itemopts.Padding.Top + itemopts.Padding.Bottom;

            if (itemHeight > this._listBlock.AvailableBounds.Height - _itemoffset)
            {
                PDFLayoutBlock origparent = this.CurrentBlock;
                PDFLayoutRegion origregion = this.CurrentBlock.CurrentRegion;
                PDFLayoutBlock origlist = this._listBlock;
                PDFLayoutBlock origItem = this._itemblock;

                if (this._listBlock.Position.OverflowSplit == OverflowSplit.Never)
                {
                    if (this.MoveFullListToNextRegion(_itemoffset + itemHeight))
                    {
                        //_itemoffset += itemHeight;
                        //PDFRect avail = _listBlock.AvailableBounds;
                        //avail.Height = avail.Height - _itemoffset;
                    }
                    else
                    {
                        this.Context.TraceLog.Add(TraceLevel.Warning, LOG_CATEGORY, "List '" + this.List.UniqueID + "' has filled the available space, and cannot overflow onto a new region. Layout has stopped at item index " + index);
                        this.ContinueLayout = false;
                        return 0;
                    }
                }
                else if (this.StartListInAnotherRegion(itemHeight, origItem, index))
                {
                    //origregion.AddToSize(origlist);
                    _itemoffset = 0;
                    origItem.Offset(0, 0);
                }
                else
                {
                    (origItem.Parent as PDFLayoutBlock).CurrentRegion.RemoveItem(origItem);
                    this.Context.TraceLog.Add(TraceLevel.Warning, LOG_CATEGORY, "List '" + this.List.UniqueID + "' has filled the available space, and cannot overflow onto a new region. Layout has stopped at item index " + index);
                    
                    this.ContinueLayout = false;
                }
            }
            else if (this._itemblock.CurrentRegion.IsClosed)
            {
                this._itemblock = this.CurrentBlock.LastOpenBlock();
                this._listBlock = this._itemblock.Parent as PDFLayoutBlock;
            }

            if (null != this._itemblock)
            {
                if (this._itemblock.CurrentRegion.IsClosed == false)
                    this._itemblock.CurrentRegion.Close();

                if (this._itemblock.IsClosed == false)
                {
                    this._itemblock.Close();
                    this._listBlock.CurrentRegion.AddToSize(this._itemblock);
                }
            }

            if (this.ContinueLayout)
                RegisterChildLayout(entry.ListItem);

            if (null != artefacts)
                entry.ListItem.CloseLayoutArtefacts(this.Context, artefacts, full);
            
            this.StyleStack.Pop();

            return itemHeight;
        }

        #endregion

        #region private PDFUnit LayoutItemNumber(ListNumberEntry entry, PDFStyle fullstyle)

        /// <summary>
        /// Layout the entry item number and returns the height used by the item number.
        /// </summary>
        /// <param name="entry">The entry whose number should be laid out</param>
        /// <returns></returns>
        private PDFUnit LayoutItemNumber(ListNumberEntry entry, Style fullstyle)
        {
            ListItem item = entry.ListItem;
            
            
            PDFUnit avail = _itemblock.CurrentRegion.AvailableHeight;

            ListItemLabel literal = (ListItemLabel)entry.NumberComponent;
            Style applied = literal.GetAppliedStyle();
            if (null != applied)
            {
                this.StyleStack.Push(applied);
                fullstyle = this.StyleStack.GetFullStyle(literal);
            }

            //Get the text of the next number
            if (string.IsNullOrEmpty(entry.NumberText))
            {
                literal.Text = this.Component.Document.ListNumbering.Increment();
            }
            else
                literal.Text = entry.NumberText;

            //Set the list items label reference for future use if required.
            item.ItemLabelComponent = entry.NumberComponent;

            //Actually layout the item in the region
            this.DoLayoutAChild(item.ItemLabelComponent, fullstyle);
            this._itemblock.CurrentRegion.CloseCurrentItem();

            // Calculate the height used and return
            PDFUnit newAvail = _itemblock.CurrentRegion.AvailableHeight;
            PDFUnit used = avail - newAvail;

            if (null != applied)
                this.StyleStack.Pop();

            return used;
        }

        #endregion

        #region private PDFUnit LayoutItemContent(ListNumberEntry entry)

        /// <summary>
        /// Lays out all the contents of the list item in the current itemblock and returns the used height of the
        /// components it has laid out.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private PDFUnit LayoutItemContent(ListNumberEntry entry)
        {
            ComponentList contents = null;
            IPDFContainerComponent container = entry.ListItem as IPDFContainerComponent;

            PDFUnit avail = _itemblock.CurrentRegion.AvailableHeight;

            if (container.HasContent)
            {
                //because we are mimicing being the container - set the full style to the item style
                Style last = this.FullStyle;
                this.FullStyle = entry.FullStyle;

                contents = container.Content;
                this.DoLayoutChildren(contents);

                //And restore the full style to the previous item after.
                this.FullStyle = last;
            }
            this._itemblock.CurrentRegion.CloseCurrentItem();

            // Calculate the height used and return
            PDFUnit newAvail = _itemblock.CurrentRegion.AvailableHeight;
            PDFUnit used = avail - newAvail;
            return used;
        }

        #endregion

        //
        // overflow methods
        //

        #region protected virtual bool StartListInAnotherRegion(PDFUnit itemHeight, PDFLayoutBlock item, int itemindex)

        /// <summary>
        /// Closes down the current list and attempts to open a new region to start laying out any further items.
        /// Returns true if a new region was created, otherwise false.
        /// </summary>
        /// <param name="itemHeight"></param>
        /// <param name="item"></param>
        /// <param name="itemindex"></param>
        /// <returns></returns>
        protected virtual bool StartListInAnotherRegion(PDFUnit itemHeight, PDFLayoutBlock item, int itemindex)
        {
            PDFLayoutRegion itemRegion = item.CurrentRegion;
            PDFLayoutBlock origListBlock = item.Parent as PDFLayoutBlock;
            PDFLayoutRegion origListRegion = origListBlock.CurrentRegion;

            if (origListBlock.IsClosed == false)
                origListBlock.Close();
            //(origListBlock.Parent as PDFLayoutBlock).CurrentRegion.AddToSize(origListBlock);

            bool newPage;
            bool started = this.MoveToNextRegion(itemHeight, ref itemRegion, ref item, out newPage);

            if (started)
            {
                if (this.Context.ShouldLogVerbose)
                    this.Context.TraceLog.Add(TraceLevel.Verbose, ListEngineLogCategory, "Started list '" + this.List.ID + "' in a new " + (newPage ? "Page" : "Region") + " and list item block is now '" + item.ToString() + "'");
            }

            return started;
        }

        #endregion

        #region protected virtual bool MoveFullListToNextRegion()

        //Check to see if we have moved the full table to a new region previously
        //if we have, then it means we cannot fit the entire table in one block - so we should truncate.
        private bool _didmovefulllist = false;

        /// <summary>
        /// Moves the entire list block to a new region or page in the layout
        /// </summary>
        /// <param name="requiredHeight"></param>
        /// <returns></returns>
        protected virtual bool MoveFullListToNextRegion(PDFUnit requiredHeight)
        {
            if (_didmovefulllist)
            {
                this.Context.TraceLog.Add(TraceLevel.Error, ListEngineLogCategory, "Already moved list to the next region so cannot move list '" + this.List.ID + "' to the next region");
                return false;
            }

            
            PDFLayoutBlock block = this._listBlock;
            PDFLayoutRegion region = this._listBlock.CurrentRegion;
            bool newPage;
            if (this.MoveToNextRegion(requiredHeight, ref region, ref block, out newPage))
            {
                if (this.Context.ShouldLogVerbose)
                    this.Context.TraceLog.Add(TraceLevel.Verbose, ListEngineLogCategory, "Moved entire list '" + this.List.ID + "' to the next " + (newPage ? "Page" : "Region") + " and list block is now '" + block.ToString() + "'");

                //If we have a new block then we set the old one as invisible
                if (block != _listBlock)
                    _listBlock.Position.Visibility = Visibility.None;

                _listBlock = block;
                _didmovefulllist = true;
                return true;
            }
            else
                return false;
        }

        #endregion

        #region public override PDFLayoutBlock CloseCurrentBlockAndStartNewInRegion(PDFLayoutBlock blockToClose, PDFLayoutRegion joinToRegion)

        /// <summary>
        /// Overrides the base method to close the current list block, and set up a new block in the provided region. 
        /// Updates the references in this engine.
        /// </summary>
        /// <param name="blockToClose"></param>
        /// <param name="joinToRegion"></param>
        /// <returns></returns>
        public override PDFLayoutBlock CloseCurrentBlockAndStartNewInRegion(PDFLayoutBlock blockToClose, PDFLayoutRegion joinToRegion)
        {
            PDFLayoutBlock orig = this.CurrentBlock;
            PDFRect avail = this._listBlock.AvailableBounds;
            avail.Height = joinToRegion.AvailableHeight;
            PDFThickness margins = _listBlock.Position.Margins;
            
            PDFLayoutBlock newList = base.CloseCurrentBlockAndStartNewInRegion(blockToClose, joinToRegion);
            this.CurrentBlock = (PDFLayoutBlock)newList.Parent;
            
            this._listBlock = newList;
            avail.Y = 0;

            avail.Width -= margins.Left + margins.Right;
            avail.Height -= margins.Top + margins.Bottom;
            avail.X += margins.Left;
            avail.Y += margins.Top;

            this._listBlock.AvailableBounds = avail;
            this._itemoffset = 0;

            return newList;

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
