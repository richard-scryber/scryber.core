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
    public class LayoutEngineStatedCheck : LayoutEngineFieldStatedBase
    {
        
        

        public LayoutEngineStatedCheck(FormInputField field, IPDFLayoutEngine parent) : base(field, parent)
        {
            field.Size = 1;
        }
        
        

        

        

        protected override void DoLayoutComponent()
        {
            var origValue = this.Field.Value;
            
            this.Field.FontFamily = (FontSelector)"zapf dingbats";
            this.FullStyle.Font.FontFamily = (FontSelector)"zapf dingbats";
            //We change the value so that this is the character that is rendered.
            if (this.Field.ButtonType == FormButtonFieldType.Radio)
                Field.Value = "l"; //Thick bullet in zapf
            else
                Field.Value = "4"; //Thick tick mark in zaph
            
            var outerPos = this.FullStyle.CreatePostionOptions(false);
            var createdLine =  this.EnsureAvailableLine(outerPos);
            var createdRegion = this.EnsureAvailableInlineBlock(outerPos);
            
           
            
            var context = this.Context;
            var fullstyle = this.FullStyle;
            
            this.IsLayingOutStates = false;
            
            //Normal - a real, in-flow pass via the plain engine, exactly like any other field.
            //Must fully complete (including its own Dispose) before anything else touches
            //LastOpenBlock()/CurrentBlock again.
            var blockBeforeNormal = context.DocumentLayout.CurrentPage.LastOpenBlock();
            var regionForNormal = blockBeforeNormal.CurrentRegion;
            
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
                    
                    //Belt and Braces - if we are caching in the style, then this will always be the same instance.
                    //if not then the style key values will be used anyway.
                    var fullPos = fullstyle.CreatePostionOptions(true);
                    fullPos.Width = pos.Width.Value;
                    fullPos.Height = pos.Height.Value;
                    
                }

            }
            
            //set the border radius for the radio buttons (if not explicitly set)
            if (this.Field.ButtonType == FormButtonFieldType.Radio)
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
            using (var normalEngine = new LayoutEngineCheckState(Field, this, FormFieldAppearanceState.Normal))
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

            if (null == downXObject || !this.ContinueLayout)
            {
                if (this.Context.Conformance == ParserConformanceMode.Strict)
                    throw new NullReferenceException(
                        "There was no XObject run returned for the layout of the checkbox " + this.Field.UniqueID);
                
                this.Context.TraceLog.Add(TraceLevel.Error, "Form Fields", "There was no XObject run returned for the layout of the checkbox "  + this.Field.UniqueID);
                return;
            }

            if (pos.DisplayMode == DisplayMode.Inline || pos.DisplayMode == DisplayMode.InlineBlock)
            {
                //we have closed the positioned block so can now get our y offset again.
                blockBeforeNormal = context.DocumentLayout.CurrentPage.LastOpenBlock();
                regionForNormal = blockBeforeNormal.CurrentRegion;
                var offsetY = regionForNormal.Height + regionForNormal.OffsetY;
                location = downXObject.Location;
                location.Y += offsetY;
                
            }
            

            var layoutPage = context.DocumentLayout.CurrentPage;
            

            IArtefactCollection annots;
            if (!layoutPage.Artefacts.TryGetCollection(PDFArtefactTypes.Annotations, out annots))
            {
                annots = new PDFAnnotationCollection(PDFArtefactTypes.Annotations);
                layoutPage.Artefacts.Add(annots);
            }
            annots.Register(Field.Widget);
            Field.Widget.SetAppearance(FormFieldAppearanceState.On, downXObject, layoutPage, downStyle);

            
            
            
            
            //Set the states flag so we can make sure we don't overflow onto a new region.
            
            this.IsLayingOutStates = true;
            
            this.Field.Value = " ";

            
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
                Field.Widget.ContainerOffset = location;
            else
            {
                Field.Widget.ContainerOffset = Point.Empty;
            }
            
            if(null != createdLine)
                createdLine.Close();
            
            this.RestorePositionValues(fullstyle);

            //And release after (just in case)
            this.IsLayingOutStates = false;
            this.Field.Value = origValue;
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

            PDFLayoutXObjectRun stateXObject;
            
            using (var stateEngine = new LayoutEngineCheckState(Field, this, appearanceState))
            {
                stateEngine.Layout(this.Context, fullstyle);
                stateXObject = stateEngine.Result;
            }
            
            //null style, so alywys outputs xobject
            if (null != stateXObject)
                Field.Widget.SetAppearance(appearanceState, stateXObject, layoutPage, null);
            else
                Field.Widget.SetAppearance(appearanceState, normalXObject, layoutPage, fullstyle);
            return stateXObject;
        }
        
    }
}
