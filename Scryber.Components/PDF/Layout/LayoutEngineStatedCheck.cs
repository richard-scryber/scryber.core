using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.PDF.Layout
{
    /// <summary>
    /// Layout "engine" for pushbutton fields - the only field type that self-renders a real /AP
    /// (everything else relies on /NeedAppearances). Not a LayoutEngineInput/LayoutEngineBase
    /// subclass at all: it does no layout of its own, it just sequences up to 3 independent,
    /// fully self-contained sub-engine passes (Normal, then Down, then Over), each one running
    /// to completion - open, lay out, close - before the next begins. LastOpenBlock()/CurrentBlock
    /// are page-level state shared across engines, not scoped per-instance, so starting a nested
    /// pass while an outer one's call frame is still "open" corrupts that state (confirmed via a
    /// stack overflow when this was first tried as a LayoutEngineInput subclass override).
    /// </summary>
    public class LayoutEngineStatedCheck : LayoutEngineBase
    {
        private readonly FormInputField _field;

        
        public bool IsLayingOutStates { get; private set; }
        

        public LayoutEngineStatedCheck(FormInputField field, IPDFLayoutEngine parent) : base(field, parent)
        {
            if(null == field)
                throw new NullReferenceException("Field cannot be null.");
            
            _field = field;
            field.Size = 1;
        }
        
        private Unit? _top, _left, _bottom, _right;

        private void StorePositionValues(Style forStyle)
        {
            if(forStyle.IsValueDefined(StyleKeys.PositionXKey))
                _left = forStyle.Position.X;
            if(forStyle.IsValueDefined(StyleKeys.PositionYKey))
                _top = forStyle.Position.Y;
            if(forStyle.IsValueDefined(StyleKeys.PositionBottomKey))
                _bottom = forStyle.Position.Bottom;
            if(forStyle.IsValueDefined(StyleKeys.PositionRightKey))
                _right = forStyle.Position.Right;
        }

        private void ClearPositionValues(Style forStyle)
        {
            forStyle.Position.RemoveBottom();
            forStyle.Position.RemoveRight();
            forStyle.Position.RemoveX();
            forStyle.Position.RemoveY();
            
            if(forStyle is StyleFull full)
                full.ClearFullRefs();
        }

        private void RestorePositionValues(Style forStyle)
        {
            if(_left != null)
                forStyle.Position.X = _left.Value;
            if(_top != null)
                forStyle.Position.Y = _top.Value;
            if(_bottom != null)
                forStyle.Position.Bottom = _bottom.Value;
            if(_right != null)
                forStyle.Position.Right = _right.Value;
            
            if(forStyle is StyleFull full)
                full.ClearFullRefs();
        }

        private PDFLayoutLine EnsureAvailableLine(PDFPositionOptions outerPos)
        {
            if (outerPos.PositionMode == PositionMode.Static || outerPos.PositionMode == PositionMode.Relative)
            {
                if (outerPos.DisplayMode == DisplayMode.Block)
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

        private PDFLayoutRegion EnsureAvailableInlineBlock(PDFPositionOptions outerPos)
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

        protected override void DoLayoutComponent()
        {
            var origValue = this._field.Value;
            
            this._field.FontFamily = (FontSelector)"zapf dingbats";
            //We change the value so that this is the character that is rendered.
            if (this._field.ButtonType == FormButtonFieldType.Radio)
                _field.Value = "l"; //Thick bullet in zapf
            else
                _field.Value = "4"; //Thick tick mark in zaph
            
            var outerPos = this.FullStyle.CreatePostionOptions(false);
            
            var createdLine =  this.EnsureAvailableLine(outerPos);
            var createdRegion = this.EnsureAvailableInlineBlock(outerPos);
            
           
            
            var context = this.Context;
            var fullstyle = this.FullStyle;
            var offsetY = Unit.Empty;
            this.IsLayingOutStates = false;
            
            //Normal - a real, in-flow pass via the plain engine, exactly like any other field.
            //Must fully complete (including its own Dispose) before anything else touches
            //LastOpenBlock()/CurrentBlock again.
            var blockBeforeNormal = context.DocumentLayout.CurrentPage.LastOpenBlock();
            var regionForNormal = blockBeforeNormal.CurrentRegion;
            var current = regionForNormal.CurrentItem as PDFLayoutLine;
            if(null != current)
                offsetY = current.OffsetY;
            
            Style stateStyle;
            Style downStyle = fullstyle;
            
            
            if (fullstyle.TryGetStyleState(ComponentState.Down, out stateStyle))
            {
                Style merged = new StyleFull();
                fullstyle.MergeInto(merged);
                stateStyle.MergeInto(merged);
                downStyle = merged;
            }
            
            this.StorePositionValues(fullstyle);
            this.ClearPositionValues(fullstyle);
            this.ClearPositionValues(downStyle);
            

            Unit? top = null;
            

            var pos = downStyle.CreatePostionOptions(true);
            if (pos.Padding.IsEmpty == false)
            {
                var width = pos.Padding.Left + pos.Padding.Right;
                var height = pos.Padding.Top + pos.Padding.Bottom;
                if (pos.Width.HasValue && pos.Height.HasValue)
                {
                    pos.Width = width + pos.Width.Value;
                    pos.Height = height + pos.Height.Value;
                    downStyle.Size.Width = pos.Width.Value;
                    downStyle.Size.Height = pos.Height.Value;
                    
                    fullstyle.Size.Width = pos.Width.Value;
                    fullstyle.Size.Height = pos.Height.Value;
                    var fullPos = fullstyle.CreatePostionOptions(true);
                    fullPos.Width = pos.Width.Value;
                    fullPos.Height = pos.Height.Value;
                    
                }

            }
            
            //set the border radius for the radio buttons (if not explicitly set)
            if (this._field.ButtonType == FormButtonFieldType.Radio)
            {
                var w = downStyle.Size.Width;
                var h = downStyle.Size.Height;
                var half = Unit.Min(w, h) / 2.0;
                if (fullstyle.IsValueDefined(StyleKeys.BorderCornerRadiusKey) == false)
                    fullstyle.Border.CornerRadius = half;
                if (downStyle.IsValueDefined(StyleKeys.BorderCornerRadiusKey) == false)
                    downStyle.Border.CornerRadius = half;
            }

            


            PDFLayoutXObjectRun downXObject;
            using (var normalEngine = new LayoutEngineCheckState(_field, this, FormFieldAppearanceState.Normal))
            {
                normalEngine.Layout(context, downStyle);
                this.ContinueLayout = normalEngine.ContinueLayout;
                downXObject = normalEngine.Result;
            }
            var line = downXObject.Line;
            var location = Point.Empty;
            
            this.CloseAnyLeftoverBlock(blockBeforeNormal);

            if (null != createdRegion)
            {
                createdRegion.Close();
            }

            if (pos.DisplayMode == DisplayMode.Inline || pos.DisplayMode == DisplayMode.InlineBlock)
            {
                //we have closed the positioned block so can now get our y offset again.
                blockBeforeNormal = context.DocumentLayout.CurrentPage.LastOpenBlock();
                regionForNormal = blockBeforeNormal.CurrentRegion;
                offsetY = regionForNormal.Height + regionForNormal.OffsetY;
                location = downXObject.Location;
                location.Y += offsetY;
                
            }
            

            if (null == downXObject || !this.ContinueLayout)
                return;

            var layoutPage = context.DocumentLayout.CurrentPage;
            

            IArtefactCollection annots;
            if (!layoutPage.Artefacts.TryGetCollection(PDFArtefactTypes.Annotations, out annots))
            {
                annots = new PDFAnnotationCollection(PDFArtefactTypes.Annotations);
                layoutPage.Artefacts.Add(annots);
            }
            annots.Register(_field.Widget);
            _field.Widget.SetAppearance(FormFieldAppearanceState.On, downXObject, layoutPage, downStyle);

            
            
            
            
            //Set the states flag so we can make sure we don't overflow onto a new region.
            
            this.IsLayingOutStates = true;
            
            this._field.Value = " ";

            
            //isolated pass that only starts once the previous one has entirely closed.
            var offXObject = this.RegisterIndependentState(ComponentState.Normal, FormFieldAppearanceState.Off, fullstyle, downXObject, layoutPage);
           
            //take the run out of the layout, so it doen not impact the width
            if (null != offXObject)
            {
                offXObject.Line.Runs.Remove(offXObject);
                offXObject.Page = layoutPage; //need to set this as when removed there is no connection to the current page.
            }
            
            if (pos.Margins.IsEmpty == false)
            {
                var vp = new Rect();
                vp.X = pos.Margins.Left;
                vp.Y -= pos.Margins.Top;
                vp.Width = downXObject.Width + pos.Margins.Left;
                vp.Height = downXObject.Height - pos.Margins.Top;
            
                downXObject.PositionOptions.ViewPort = vp;
                offXObject.PositionOptions.ViewPort = vp;
                location.X += pos.Margins.Left;
                location.Y += pos.Margins.Top;
            }

            if (pos.PositionMode != PositionMode.Fixed)
                _field.Widget.ContainerOffset = location;
            else
            {
                _field.Widget.ContainerOffset = Point.Empty;
            }
            
            if(null != createdLine)
                createdLine.Close();
            
            this.RestorePositionValues(fullstyle);

            //And release after (just in case)
            this.IsLayingOutStates = false;
            this._field.Value = origValue;
        }

        /// <summary>
        /// If a :hover/:active rule actually matched this field, lays its content out again -
        /// fully independently, not a colour repaint - using Normal's fully resolved style with
        /// the state's own declared properties overlaid on top (mirroring how StyleDefn originally
        /// built that state style from the matching rule during the main style pass). Without a
        /// matching rule, this state just reuses Normal's own xObject unchanged, exactly as before
        /// independent state layout existed.
        /// </summary>
        private PDFLayoutXObjectRun RegisterIndependentState(ComponentState componentState, FormFieldAppearanceState appearanceState,
            Style fullstyle, PDFLayoutXObjectRun normalXObject, PDFLayoutPage layoutPage)
        {
            
            var blockBefore = this.Context.DocumentLayout.CurrentPage.LastOpenBlock();
            if(null == blockBefore)
                throw new NullReferenceException("There's no current block.");
            
            var region = blockBefore.CurrentRegion;
            
            if(null == region)
                throw new NullReferenceException("There's no current region.");

            var posOptions = fullstyle.CreatePostionOptions(true);
            var newRegion = region;
            var decrementAfter = false;
            var closeAfter = false;

            PDFLayoutXObjectRun stateXObject;
            
            using (var stateEngine = new LayoutEngineCheckState(_field, this, appearanceState))
            {
                stateEngine.Layout(this.Context, fullstyle);
                stateXObject = stateEngine.Result;
            }

            if (closeAfter)
            {
                this.CloseAnyLeftoverBlock(blockBefore);
                newRegion.Close();
            }
            
            if(decrementAfter)
                this.Context.PositionDepth -= 1;
            
            //null style, so alywys outputs xobject
            if (null != stateXObject)
                _field.Widget.SetAppearance(appearanceState, stateXObject, layoutPage, null);
            else
                _field.Widget.SetAppearance(appearanceState, normalXObject, layoutPage, fullstyle);
            return stateXObject;
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
        private void CloseAnyLeftoverBlock(PDFLayoutBlock before)
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
}
