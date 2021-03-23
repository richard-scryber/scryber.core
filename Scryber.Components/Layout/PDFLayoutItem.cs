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
using Scryber.Native;
using Scryber.Components;

namespace Scryber.Layout
{
    /// <summary>
    /// A distinct layout item - block, line, table, anything
    /// </summary>
    public abstract class PDFLayoutItem
    {
        protected const int NULL_PAGEINDEX = -1;
        protected const int NULL_COLUMNINDEX = -1;
        protected const string LOG_CATEGORY = "PDFLayout";

        //
        // properties
        //

        #region public PDFLayoutItem Parent { get; private set; }

        /// <summary>
        /// Gets the parent of this layout item 
        /// </summary>
        public PDFLayoutItem Parent { get; private set; }

        #endregion
        
        #region public bool IsClosed { get; protected set; }

        /// <summary>
        /// Returns true if this item is closed and cannot be appended to
        /// </summary>
        public bool IsClosed { get; protected set; }

        #endregion

        #region public IPDFComponent Owner {get;}

        /// <summary>
        /// Gets the (component) owner of this layout item if any.
        /// </summary>
        public IPDFComponent Owner
        {
            get;
            private set;
        }

        #endregion

        //
        // abstract properties
        //

        #region public abstract PDFUnit Height { get; }

        /// <summary>
        /// Gets the height of this item
        /// </summary>
        public abstract PDFUnit Height { get; }

        #endregion

        #region public abstract PDFUnit Width { get; }

        /// <summary>
        /// Gets the width of this item
        /// </summary>
        public abstract PDFUnit Width { get; }

        #endregion

        /// <summary>
        /// Gets the Y Offset from the parent of this item
        /// </summary>
        public virtual PDFUnit OffsetY { get { return PDFUnit.Zero; } }

        /// <summary>
        /// Gets the X Offset from the parent of this item
        /// </summary>
        public virtual PDFUnit OffsetX { get { return PDFUnit.Zero; } }

        //
        // ctor(s)
        //

        #region protected PDFLayoutItem(PDFLayoutItem parent)

        /// <summary>
        /// Creates a new PDFLayout item
        /// </summary>
        /// <param name="parent"></param>
        protected PDFLayoutItem(PDFLayoutItem parent, IPDFComponent owner)
        {
            this.Owner = owner;
            this.Parent = parent;
            this.IsClosed = false;
        }

        #endregion

        //
        // abstract methods
        //

        /// <summary>
        /// Updates position data and layout after everything has been placed
        /// </summary>
        /// <param name="context">The current layout context</param>
        /// <param name="xoffset">Any horizontal adjustment to be applied when the component layout is set</param>
        /// <param name="yoffset">Any vertical adjustment to be applied when the component layout is set</param>
        protected abstract void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, PDFUnit xoffset, PDFUnit yoffset);

        //
        // Implementation Methods
        // 

        #region public virtual bool MoveToNextRegion()

        /// <summary>
        /// Checks if this layout item supports splitting of components across pages
        /// </summary>
        /// <returns></returns>
        public virtual bool MoveToNextRegion( PDFUnit requiredHeight, PDFLayoutContext context)
        {
            return false;
        }

        #endregion
        
        #region public void Close()

        /// <summary>
        /// Closes the Layout Item.
        /// </summary>
        /// <returns>True if the item was successfully closed.</returns>
        /// <remarks>Internally this calls the virtual DoClose method.
        /// Inheritors have an opportunity to perform some cleanup. 
        /// If the item should not be closed then inheritors can return false 
        /// from the DoClose method and an exception will be raised.</remarks>
        public bool Close()
        {
            string msg = "";

            //Only perform the actual closing if we are not already closed
            if (this.IsClosed == false)
            {
                bool perform = this.DoClose(ref msg);
               
                if (perform)
                {
                    if(this.Parent is PDFLayoutRegion)
                        (this.Parent as PDFLayoutRegion).AddToSize(this);

                    this.IsClosed = true;
                }
                else
                {
                    //log.Add(TraceLevel.Error, PDFLayoutItem.LOG_CATEGORY, "Could not close layout " + this.ToString() + ". Its an error to attempt to close an item that is already closed");
                    if (null == msg)
                        msg = String.Empty;
                    throw new InvalidOperationException(Errors.LayoutItemCouldNotBeClosed + msg);
                }
                return perform;
            }
            else
                return true;
        }

        #endregion 

        #region protected virtual bool DoClose(ref string msg)

        /// <summary>
        /// Base implementation simply returns true. 
        /// Inheritors should override this method to perform their own clean up.
        /// If the item should not be closed then return false.
        /// </summary>
        /// <param name="msg">Set this to any message to pass on why the item could not be closed.</param>
        /// <returns>True unless a sub-class decides that it should not be closed and returns an alternative value.</returns>
        protected virtual bool DoClose(ref string msg)
        {
            return true;
        }

        #endregion

        #region protected void AssertIsOpen()

        /// <summary>
        /// Checks to see if this item is open. If not then it raises an exception.
        /// </summary>
        protected void AssertIsOpen()
        {
            if (this.IsClosed)
            {
                this.IsClosed = false;
                return;

                throw new InvalidOperationException(Errors.LayoutItemIsClosed);
            }
        }

        #endregion

        #region internal void SetParent(PDFLayoutItem item)

        /// <summary>
        /// Internal method that changes the parent of this item - cannot be null
        /// </summary>
        /// <param name="item"></param>
        internal void SetParent(PDFLayoutItem item)
        {
            if (null == item)
                throw new ArgumentNullException("item");

            this.Parent = item;
        }

        #endregion

        #region protected int GetPageIndex()

        /// <summary>
        /// Gets the index of the page this item is positioned on
        /// </summary>
        /// <returns></returns>
        protected int GetPageIndex()
        {
            PDFLayoutItem item = this;
            while (null != item)
            {
                if (item is PDFLayoutPage)
                    return (item as PDFLayoutPage).PageIndex;
                else
                    item = item.Parent;
            }
            return NULL_PAGEINDEX;
        }

        #endregion

        #region protected int GetColumnIndex()

        /// <summary>
        /// Gets the index of the column this item is positioned on
        /// </summary>
        /// <returns></returns>
        protected int GetColumnIndex()
        {
            PDFLayoutItem item = this;
            while (null != item)
            {
                if (item is PDFLayoutRegion)
                    return (item as PDFLayoutRegion).ColumnIndex;
                else if (item is PDFLayoutBlock)
                    return (item as PDFLayoutBlock).CurrentRegion.ColumnIndex;
                else
                    item = item.Parent;
            }
            return NULL_COLUMNINDEX;
        }

        #endregion

        internal protected PDFLayoutBlock GetParentBlock()
        {
            PDFLayoutItem parent = this.Parent;
            while (null != parent && !(parent is PDFLayoutBlock))
                parent = parent.Parent;
            return parent as PDFLayoutBlock;
        }

        #region public virtual void SetMaxWidth(PDFUnit width)

        /// <summary>
        /// Adjusts the width of this component so that it does not extend beyond the boundaries of its container.
        /// The base implementation does nothing. Inheritors should override this to perform their own re-sizing
        /// </summary>
        /// <param name="width">The maximum width allowed</param>
        public virtual void SetMaxWidth(PDFUnit width)
        {
            
        }

        #endregion

        #region public void PushComponentLayout(PDFLayoutContext context, int pageIndex, PDFUnit xoffset, PDFUnit yoffset)

        /// <summary>
        /// Pushes the component layout for this item onto the owning component, so that it knows it's own position
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pageIndex"></param>
        /// <param name="xoffset"></param>
        /// <param name="yoffset"></param>
        public void PushComponentLayout(PDFLayoutContext context, int pageIndex, PDFUnit xoffset, PDFUnit yoffset)
        {
            SetOwnerPageIndex(pageIndex);

            this.DoPushComponentLayout(context, pageIndex, xoffset, yoffset);
        }

        protected virtual void SetOwnerPageIndex(int pageIndex)
        {
            Component comp = this.Owner as Component;
            if (null != comp)
                comp.SetPageLayoutIndex(pageIndex);
        }

        #endregion

        #region public virtual void ResetAvailableHeight(PDFUnit height, bool includeChildren)

        /// <summary>
        /// Resets the available hight in this layout item. 
        /// Inheritors should override this method to provide their own required implementation
        /// </summary>
        /// <param name="height"></param>
        /// <param name="includeChildren"></param>
        public virtual void ResetAvailableHeight(PDFUnit height, bool includeChildren)
        {
        }

        #endregion

        #region public virtual PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer) + DoOutputToPDF()

        /// <summary>
        /// Outputs the contents of this item to the writer
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            return this.DoOutputToPDF(context, writer);
        }

        /// <summary>
        /// Base implementation of the LayoutItem output method. Inheritors should override this method to output their required
        /// data.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected virtual PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            return null;
        }

        #endregion

        #region protected void OutputBackground(PDFBackgroundStyle bg, PDFBorderStyle border, PDFRenderContext context, PDFRect rect)

        /// <summary>
        /// Outputs the background for this block in the specified rect with the background style.
        /// </summary>
        /// <param name="bg">The backgrond style</param>
        /// <param name="border">The border style (which indicates the corner radius)</param>
        /// <param name="context">The current render context</param>
        /// <param name="rect">The rectangle to be output</param>
        protected virtual void OutputBackground(PDFBrush bg, PDFUnit? corner, PDFRenderContext context, PDFRect rect)
        {
            PDFGraphics g = context.Graphics;
            
            if (null != bg)
            {
                if (corner.HasValue && corner.Value != PDFUnit.Zero)
                    g.FillRoundRectangle(bg, rect, corner.Value);
                else
                    g.FillRectangle(bg, rect);
            }
        }

        #endregion

        #region protected void OutputBorder(PDFBackgroundStyle bg, PDFBorderStyle border, PDFRenderContext context, PDFRect rect)

        /// <summary>
        /// Outputs the border for this block in the specified rect with the border style
        /// </summary>
        /// <param name="bg">The background style</param>
        /// <param name="border">The border style</param>
        /// <param name="context">The current render context</param>
        /// <param name="rect">The rectangle that shoud be rendered as the border</param>
        protected virtual void OutputBorder(PDFBrush bg, PDFPenBorders border, PDFRenderContext context, PDFRect rect)
        {
            PDFGraphics g = context.Graphics;
            
            if (null != border.AllPen && border.AllSides > 0)
            {
                if (border.CornerRadius.HasValue && border.CornerRadius.Value != PDFUnit.Zero)
                    g.DrawRoundRectangle(border.AllPen, rect, border.AllSides, border.CornerRadius.Value);
                else
                    g.DrawRectangle(border.AllPen, rect, border.AllSides);
            }

            if(null != border.TopPen)
            {
                if (border.CornerRadius.HasValue && border.CornerRadius.Value != PDFUnit.Zero)
                    g.DrawRoundRectangle(border.TopPen, rect, Sides.Top, border.CornerRadius.Value);
                else
                    g.DrawRectangle(border.TopPen, rect, Sides.Top);
            }

            if (null != border.RightPen)
            {
                if (border.CornerRadius.HasValue && border.CornerRadius.Value != PDFUnit.Zero)
                    g.DrawRoundRectangle(border.RightPen, rect, Sides.Right, border.CornerRadius.Value);
                else
                    g.DrawRectangle(border.RightPen, rect, Sides.Right);
            }

            if (null != border.BottomPen)
            {
                if (border.CornerRadius.HasValue && border.CornerRadius.Value != PDFUnit.Zero)
                    g.DrawRoundRectangle(border.BottomPen, rect, Sides.Bottom, border.CornerRadius.Value);
                else
                    g.DrawRectangle(border.BottomPen, rect, Sides.Bottom);
            }

            if (null != border.LeftPen)
            {
                if (border.CornerRadius.HasValue && border.CornerRadius.Value != PDFUnit.Zero)
                    g.DrawRoundRectangle(border.LeftPen, rect, Sides.Left, border.CornerRadius.Value);
                else
                    g.DrawRectangle(border.LeftPen, rect, Sides.Left);
            }
        }

        #endregion

        #region protected virtual void OutputClipping(PDFBorderStyle border, PDFRenderContext context, PDFRect cliprect)

        /// <summary>
        /// Outputs the border for this block in the specified rect with the border style
        /// </summary>
        /// <param name="context">The current render context</param>
        /// <param name="cliprect">The rectangle that shoud be rendered as the border</param>
        protected virtual void OutputClipping(PDFRenderContext context, PDFRect cliprect, PDFUnit corner, Sides sides, PDFThickness inset)
        {
            PDFGraphics g = context.Graphics;
            g.SaveGraphicsState();

            if (inset.IsEmpty == false)
            {
                cliprect.X += inset.Left;
                cliprect.Y += inset.Top;
                cliprect.Width -= (inset.Left + inset.Right);
                cliprect.Height -= (inset.Top + inset.Bottom);
            }

            if (corner != PDFUnit.Zero)
                g.SetClipRect(cliprect, sides, corner);
            else
            {
                g.SetClipRect(cliprect);
            }
        }

        protected virtual void ReleaseClipping(PDFRenderContext context)
        {
            PDFGraphics g = context.Graphics;
            g.RestoreGraphicsState();
        }

        #endregion
    }


    /// <summary>
    /// A collection of layout items
    /// </summary>
    public class PDFLayoutItemCollection : List<PDFLayoutItem>
    {
    }
}
