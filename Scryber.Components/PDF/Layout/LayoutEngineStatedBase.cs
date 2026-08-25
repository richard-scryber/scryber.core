using System;
using System.Collections.Generic;

using Scryber.Drawing;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.PDF.Layout;

public abstract class LayoutEngineFieldStatedBase : LayoutEngineBase
{
    private readonly FormInputField _field;
    private Unit? _top, _left, _bottom, _right;
    
    public FormInputField Field { get { return _field; } }
    
    
    
    public bool IsLayingOutStates { get; protected set; }
    
    public LayoutEngineFieldStatedBase(FormInputField field, IPDFLayoutEngine parent) : base(field, parent)
    {
        if(null == field)
            throw new NullReferenceException("Field cannot be null.");
        
        _field = field;
    }
    
    /// <summary>
    /// Remembers the position (top, left, bottom and right) values for the style.
    /// </summary>
    /// <param name="forStyle"></param>
    /// <remarks>NOTE: Only 1 set of values can be remembered for an engine instance.</remarks>
    protected void StorePositionValues(Style forStyle)
    {
        if (null != forStyle)
        {
            if (forStyle.IsValueDefined(StyleKeys.PositionXKey))
                _left = forStyle.Position.X;
            if (forStyle.IsValueDefined(StyleKeys.PositionYKey))
                _top = forStyle.Position.Y;
            if (forStyle.IsValueDefined(StyleKeys.PositionBottomKey))
                _bottom = forStyle.Position.Bottom;
            if (forStyle.IsValueDefined(StyleKeys.PositionRightKey))
                _right = forStyle.Position.Right;
        }
    }

    /// <summary>
    /// Removes the position (top, left, bottom and right) values from the style, and clears any cached options.
    /// </summary>
    /// <param name="forStyle"></param>
    protected void ClearPositionValues(Style forStyle)
    {
        if (null != forStyle)
        {
            forStyle.Position.RemoveBottom();
            forStyle.Position.RemoveRight();
            forStyle.Position.RemoveX();
            forStyle.Position.RemoveY();

            if (forStyle is StyleFull full)
                full.ClearFullRefs();
        }
    }

    /// <summary>
    /// Restores the position (top, left, bottom and right) values for the style that we previously stored.
    /// </summary>
    /// <param name="forStyle"></param>
    protected void RestorePositionValues(Style forStyle)
    {
        if (null != forStyle)
        {
            if (_left != null)
                forStyle.Position.X = _left.Value;
            if (_top != null)
                forStyle.Position.Y = _top.Value;
            if (_bottom != null)
                forStyle.Position.Bottom = _bottom.Value;
            if (_right != null)
                forStyle.Position.Right = _right.Value;

            if (forStyle is StyleFull full)
                full.ClearFullRefs();
        }
    }

    protected virtual PDFLayoutLine EnsureAvailableLine(PDFPositionOptions pos)
    {
        if (pos.PositionMode == PositionMode.Static || pos.PositionMode == PositionMode.Relative)
        {
            if (pos.DisplayMode == DisplayMode.Block)
            {
                //We are a standard block, but will be in an xObjectRegion. Close any current line and start a new one
                //That will be closed at the end.
                    
                var currentBlock = this.Context.DocumentLayout.CurrentPage.LastOpenBlock();
                var currentRegion = currentBlock.CurrentRegion;
                if(currentRegion == null)
                    throw new NullReferenceException("Current region cannot be null.");
                if (currentRegion.HasOpenItem && currentRegion.CurrentItem is PDFLayoutLine)
                {
                    currentRegion.CurrentItem.Close();
                    return currentRegion.BeginNewLine();
                }
            }
        }
        return null;
    }
    
    /// <summary>
    /// If the fields display mode is not inline-block, then this will set up a new
    /// inline block positioned region in the current region for the Stated content XObject to be laid out in.
    /// Also sets the position options to inline block and this instance's full style to inline block.
    /// </summary>
    /// <param name="outerPos">The position options to check and optionally update.</param>
    /// <returns>A new positioned region that was created, or null if not needed.</returns>
    /// <exception cref="NullReferenceException">If there is no currently open block on the current page.</exception>
    protected PDFLayoutRegion EnsureAvailableInlineBlock(PDFPositionOptions outerPos)
    {
        if (outerPos.DisplayMode != DisplayMode.InlineBlock)
        {
            var parent = this.Context.DocumentLayout.CurrentPage.LastOpenBlock();
            if(null == parent)
                throw new NullReferenceException("Parent block cannot be null.");
            
            var parentRegion = parent.CurrentRegion;
            var currentItem = parentRegion.CurrentItem;
                
            outerPos.DisplayMode = DisplayMode.InlineBlock;
            this.FullStyle.Position.DisplayMode = DisplayMode.InlineBlock;
                
            return this.BeginNewInlineBlockRegionForChild(outerPos, this._field, this.FullStyle);
        }

        return null;
    }
    
    /// <summary>
    /// A pass whose field is inline-block/absolute/fixed gets its own wrapping block created
    /// for it by the layout machinery, positioned on top of whatever was previously the
    /// current open block/region. Left open, the next pass's own wrapping block gets created
    /// on top of THAT one instead of back on the original region - accumulating nested
    /// positioned blocks with each pass, which is what was overflowing the stack. Closing
    /// whatever got left open (if anything - and only if it's not the block that was already
    /// open before this pass even started) restores LastOpenBlock() to the original region so
    /// the next pass's block gets created there again, matching the very first pass's flow.
    /// </summary>
    protected void CloseAnyLeftoverBlock(PDFLayoutBlock before)
    {
        var after = this.Context.DocumentLayout.CurrentPage.LastOpenBlock();
            
        if (after != null && !ReferenceEquals(after, before) && !after.IsClosed)
            after.Close();
    }
    
    
    public override bool MoveToNextPage(IComponent initiator, Style initiatorStyle, Stack<PDFLayoutBlock> depth, ref PDFLayoutRegion region, ref PDFLayoutBlock block)
    {
        if (this.IsLayingOutStates)
            return false;
        else
            return this.ParentEngine.MoveToNextPage(initiator, initiatorStyle, depth, ref region, ref block);
    }

    public override PDFLayoutBlock CloseCurrentBlockAndStartNewInRegion(PDFLayoutBlock blockToClose, PDFLayoutRegion joinToRegion)
    {
        if (this.IsLayingOutStates)
            return blockToClose;
        else
            return this.ParentEngine.CloseCurrentBlockAndStartNewInRegion(blockToClose, joinToRegion);
    }
}