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
    public class LayoutEngineStatedButton : LayoutEngineFieldStatedBase
    {
        

        public LayoutEngineStatedButton(FormInputField field, IPDFLayoutEngine parent) : base(field, parent)
        {
        }

        protected override void DoLayoutComponent()
        {
            var outerPos = this.FullStyle.CreatePostionOptions(false);
            var createdLine = this.EnsureAvailableLine(outerPos);
            var createdRegion = this.EnsureAvailableInlineBlock(outerPos);
            
            var context = this.Context;
            var fullstyle = this.FullStyle;
            
            this.IsLayingOutStates = false;
            
            //Normal - a real, in-flow pass via the plain engine, exactly like any other field.
            //Must fully complete (including its own Dispose) before anything else touches
            //LastOpenBlock()/CurrentBlock again.
            var blockBeforeNormal = context.DocumentLayout.CurrentPage.LastOpenBlock();
            var regionForNormal = blockBeforeNormal.CurrentRegion;
            
            //remember the default positions (top, left, etc.)
            this.StorePositionValues(fullstyle);
            
            Style normalStyle = fullstyle;
            Style overStyle;
            Style downStyle;
            
            PDFLayoutXObjectRun normalXObject;
            PDFLayoutXObjectRun overXObject;
            PDFLayoutXObjectRun downXObject;
            
            Style stateStyle;

            if (fullstyle.TryGetStyleState(ComponentState.Over, out stateStyle))
            {
                Style merged = new Style();
                fullstyle.MergeInto(merged);
                stateStyle.MergeInto(merged);
                overStyle = merged;
            }
            else
            {
                overStyle = null;
            }

            if (fullstyle.TryGetStyleState(ComponentState.Down, out stateStyle))
            {
                Style merged = new Style();
                fullstyle.MergeInto(merged);
                stateStyle.MergeInto(merged);
                downStyle = merged;
            }
            else
            {
                downStyle = null;
            }
            
            //get rid of any explicit positions, as our XObject will render from 0,0
            this.ClearPositionValues(normalStyle);
            this.ClearPositionValues(overStyle);
            this.ClearPositionValues(downStyle);
            
            //TODO: check on padding calculation update as we don't have an explicit size

            
            
            using (var normalEngine = new LayoutEngineButtonState(Field, this, FormFieldAppearanceState.Normal))
            {
                normalEngine.Layout(context, fullstyle);
                this.ContinueLayout = normalEngine.ContinueLayout;
                normalXObject = normalEngine.Result;
            }
            this.CloseAnyLeftoverBlock(blockBeforeNormal);

            if (null != createdRegion)
            {
                createdRegion.Close();
            }
            
            if (null == normalXObject || !this.ContinueLayout)
            {
                if (this.Context.Conformance == ParserConformanceMode.Strict)
                    throw new NullReferenceException(
                        "There was no XObject run returned for the layout of the button " + this.Field.UniqueID);
                
                this.Context.TraceLog.Add(TraceLevel.Error, "Form Fields", "There was no XObject run returned for the layout of the button "  + this.Field.UniqueID);
                return;
            }
            
            var line = normalXObject.Line;
            var location = Point.Empty;
            var pos = normalStyle.CreatePostionOptions(true);
            
            if (pos.DisplayMode == DisplayMode.Inline || pos.DisplayMode == DisplayMode.InlineBlock)
            {
                //we have closed the positioned block so can now get our y offset again.
                blockBeforeNormal = context.DocumentLayout.CurrentPage.LastOpenBlock();
                regionForNormal = blockBeforeNormal.CurrentRegion;
                var offsetY = regionForNormal.Height + regionForNormal.OffsetY;
                location = normalXObject.Location;
                location.Y += offsetY;
                
            }

            var layoutPage = context.DocumentLayout.CurrentPage;

            //Register the Annotation entry and set the widget appearance for normal
            IArtefactCollection annots;
            if (!layoutPage.Artefacts.TryGetCollection(PDFArtefactTypes.Annotations, out annots))
            {
                annots = new PDFAnnotationCollection(PDFArtefactTypes.Annotations);
                layoutPage.Artefacts.Add(annots);
            }
            annots.Register(Field.Widget);
            Field.Widget.SetAppearance(FormFieldAppearanceState.Normal, normalXObject, layoutPage, normalStyle);

            //Set the states flag so we can make sure we don't overflow onto a new region.
            
            this.IsLayingOutStates = true;
            
            //Down/Over - each only if a matching :hover/:active rule exists, each a fully
            //independent, isolated pass that only starts once the previous one has entirely closed.
            downXObject = this.RegisterIndependentState(ComponentState.Down, FormFieldAppearanceState.Down, downStyle, normalXObject, layoutPage);

            overXObject = this.RegisterIndependentState(ComponentState.Over, FormFieldAppearanceState.Over, overStyle, normalXObject, layoutPage);
            
            //pass the location back to the widget.
            if(pos.PositionMode == PositionMode.Fixed)
                Field.Widget.ContainerOffset = Point.Empty;
            else
            {
                Field.Widget.ContainerOffset = location;
            }
            
            //And release after (just in case)
            this.IsLayingOutStates = false;
            
            //put back in any explicit locations.
            this.RestorePositionValues(fullstyle);
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
            Style stateStyle, PDFLayoutXObjectRun normalXObject, PDFLayoutPage layoutPage)
        {
            PDFLayoutXObjectRun stateXObject;
            
            if (null != stateStyle)
            {

                var blockBefore = this.Context.DocumentLayout.CurrentPage.LastOpenBlock();
                if (null == blockBefore)
                    throw new NullReferenceException("There's no current block.");

                var region = blockBefore.CurrentRegion;

                if (null == region)
                    throw new NullReferenceException("There's no current region.");

                

                using (var stateEngine = new LayoutEngineButtonState(Field, this, appearanceState))
                {
                    stateEngine.Layout(this.Context, stateStyle);
                    stateXObject = stateEngine.Result;
                }
            }
            else
            {
                stateXObject = normalXObject;
            }

            //null style, so always outputs xobject
            if (null != stateXObject)
            {
                Field.Widget.SetAppearance(appearanceState, stateXObject, layoutPage, null);
                stateXObject.Line.Runs.Remove(stateXObject);
                stateXObject.Page = layoutPage;
            }
            else
                Field.Widget.SetAppearance(appearanceState, normalXObject, layoutPage, null);
            
            return stateXObject;
        }
        
        
    }
}
