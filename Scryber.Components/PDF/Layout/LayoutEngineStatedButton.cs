using System;
using System.Collections.Generic;
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
    public class LayoutEngineStatedButton : LayoutEngineBase
    {
        private readonly FormInputField _field;

        
        public bool IsLayingOutStates { get; private set; }
        

        public LayoutEngineStatedButton(FormInputField field, IPDFLayoutEngine parent) : base(field, parent)
        {
            _field = field;
        }

        protected override void DoLayoutComponent()
        {
            var context = this.Context;
            var fullstyle = this.FullStyle;
            this.IsLayingOutStates = false;
            
            //Normal - a real, in-flow pass via the plain engine, exactly like any other field.
            //Must fully complete (including its own Dispose) before anything else touches
            //LastOpenBlock()/CurrentBlock again.
            var blockBeforeNormal = context.DocumentLayout.CurrentPage.LastOpenBlock();
            var regionForNormal = blockBeforeNormal.CurrentRegion;
            
            

            PDFLayoutXObjectRun normalXObject;
            using (var normalEngine = new LayoutEngineButtonState(_field, this, FormFieldAppearanceState.Normal))
            {
                normalEngine.Layout(context, fullstyle);
                this.ContinueLayout = normalEngine.ContinueLayout;
                normalXObject = normalEngine.Result;
            }
            this.CloseAnyLeftoverBlock(blockBeforeNormal);

            if (null == normalXObject || !this.ContinueLayout)
                return;

            var layoutPage = context.DocumentLayout.CurrentPage;

            IArtefactCollection annots;
            if (!layoutPage.Artefacts.TryGetCollection(PDFArtefactTypes.Annotations, out annots))
            {
                annots = new PDFAnnotationCollection(PDFArtefactTypes.Annotations);
                layoutPage.Artefacts.Add(annots);
            }
            annots.Register(_field.Widget);
            _field.Widget.SetAppearance(FormFieldAppearanceState.Normal, normalXObject, layoutPage, fullstyle);

            //Set the states flag so we can make sure we don't overflow onto a new region.
            
            this.IsLayingOutStates = true;
            
            //Down/Over - each only if a matching :hover/:active rule exists, each a fully
            //independent, isolated pass that only starts once the previous one has entirely closed.
            var downXObject = this.RegisterIndependentState(ComponentState.Down, FormFieldAppearanceState.Down, fullstyle, normalXObject, layoutPage);
           
            //take the run out of the layout, so it doen not impact the width
            if(null != downXObject)
                downXObject.Line.Runs.Remove(downXObject);
            
            var overXObject = this.RegisterIndependentState(ComponentState.Over, FormFieldAppearanceState.Over, fullstyle, normalXObject, layoutPage);
            
            //same - no width impact.
            if(null != overXObject)
                overXObject.Line.Runs.Remove(overXObject);
            
            //And release after (just in case)
            this.IsLayingOutStates = false;
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
            Style stateStyle;
            if (!fullstyle.TryGetStyleState(componentState, out stateStyle))
            {
                _field.Widget.SetAppearance(appearanceState, normalXObject, layoutPage, fullstyle);
                return null;
            }

            Style merged = new Style();
            fullstyle.MergeInto(merged);
            stateStyle.MergeInto(merged);

            var blockBefore = this.Context.DocumentLayout.CurrentPage.LastOpenBlock();
            if(null == blockBefore)
                throw new NullReferenceException("There's no current block.");
            
            var region = blockBefore.CurrentRegion;
            
            if(null == region)
                throw new NullReferenceException("There's no current region.");

            var posOptions = merged.CreatePostionOptions(true);
            var newRegion = region;
            var decrementAfter = false;
            var closeAfter = false;

            PDFLayoutXObjectRun stateXObject;
            
            using (var stateEngine = new LayoutEngineButtonState(_field, this, appearanceState))
            {
                stateEngine.Layout(this.Context, merged);
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
                _field.Widget.SetAppearance(appearanceState, normalXObject, layoutPage, merged);
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
