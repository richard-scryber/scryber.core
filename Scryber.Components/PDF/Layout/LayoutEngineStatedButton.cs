using System.Collections.Generic;
using Scryber.Components;
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
    public class LayoutEngineStatedButton : IPDFLayoutEngine
    {
        private readonly FormInputField _field;
        private readonly IPDFLayoutEngine _parent;

        public IPDFLayoutEngine ParentEngine => _parent;

        public bool ContinueLayout { get; set; } = true;

        public PDFLayoutContext Context { get; private set; }

        public LayoutEngineStatedButton(FormInputField field, IPDFLayoutEngine parent)
        {
            _field = field;
            _parent = parent;
        }

        public void Layout(PDFLayoutContext context, Style fullstyle)
        {
            this.Context = context;

            //Normal - a real, in-flow pass via the plain engine, exactly like any other field.
            //Must fully complete (including its own Dispose) before anything else touches
            //LastOpenBlock()/CurrentBlock again.
            PDFLayoutXObjectRun normalXObject;
            using (var normalEngine = new LayoutEngineInput(_field, this))
            {
                normalEngine.Layout(context, fullstyle);
                this.ContinueLayout = normalEngine.ContinueLayout;
                normalXObject = normalEngine.Result;
            }

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

            //Down/Over - each only if a matching :hover/:active rule exists, each a fully
            //independent, isolated pass that only starts once the previous one has entirely closed.
            this.RegisterIndependentState(ComponentState.Down, FormFieldAppearanceState.Down, fullstyle, normalXObject, layoutPage);
            this.RegisterIndependentState(ComponentState.Over, FormFieldAppearanceState.Over, fullstyle, normalXObject, layoutPage);
        }

        /// <summary>
        /// If a :hover/:active rule actually matched this field, lays its content out again -
        /// fully independently, not a colour repaint - using Normal's fully resolved style with
        /// the state's own declared properties overlaid on top (mirroring how StyleDefn originally
        /// built that state style from the matching rule during the main style pass). Without a
        /// matching rule, this state just reuses Normal's own xObject unchanged, exactly as before
        /// independent state layout existed.
        /// </summary>
        private void RegisterIndependentState(ComponentState componentState, FormFieldAppearanceState appearanceState,
            Style fullstyle, PDFLayoutXObjectRun normalXObject, PDFLayoutPage layoutPage)
        {
            Style stateStyle;
            if (!fullstyle.TryGetStyleState(componentState, out stateStyle))
            {
                _field.Widget.SetAppearance(appearanceState, normalXObject, layoutPage, fullstyle);
                return;
            }

            Style merged = new Style();
            fullstyle.MergeInto(merged);
            stateStyle.MergeInto(merged);

            PDFLayoutXObjectRun stateXObject;
            using (var stateEngine = new LayoutEngineButtonState(_field, this))
            {
                stateEngine.Layout(this.Context, merged);
                stateXObject = stateEngine.Result;
            }

            _field.Widget.SetAppearance(appearanceState, stateXObject ?? normalXObject, layoutPage, fullstyle);
        }

        public bool MoveToNextPage(IComponent initiator, Style initiatorStyle, Stack<PDFLayoutBlock> depth, ref PDFLayoutRegion region, ref PDFLayoutBlock block)
        {
            return _parent.MoveToNextPage(initiator, initiatorStyle, depth, ref region, ref block);
        }

        public PDFLayoutBlock CloseCurrentBlockAndStartNewInRegion(PDFLayoutBlock blockToClose, PDFLayoutRegion joinToRegion)
        {
            return _parent.CloseCurrentBlockAndStartNewInRegion(blockToClose, joinToRegion);
        }

        public void Dispose()
        {
        }
    }
}
