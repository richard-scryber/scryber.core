using Scryber.Components;
using Scryber.Styles;

namespace Scryber.PDF.Layout
{
    /// <summary>
    /// A throwaway, isolated layout pass for a single button appearance state (Down/Over). Lays
    /// the field's own children out again into a region that is never attached to the real page
    /// flow - it exists purely so the resulting PDFLayoutXObjectRun can be rendered as that
    /// state's own /AP entry. Driven directly via Layout(context, style), not the normal
    /// GetEngine/CreateLayoutEngine dispatch, so it never gets picked for a real component.
    /// </summary>
    public class LayoutEngineButtonState : LayoutEngineInput
    {
        public LayoutEngineButtonState(FormInputField container, IPDFLayoutEngine parent) : base(container, parent)
        {
        }

        protected override void DoLayoutComponent()
        {
            //Mirrors what the base engine's own DoLayoutAChild would normally have done before
            //ever reaching DoLayoutComponent - pushing the field's (here, state-merged) style
            //onto the stack so children (e.g. the caption text) inherit its colour/font correctly,
            //since this pass is invoked directly via Layout() rather than that normal path.
            this.StyleStack.Push(this.FullStyle);
            try
            {
                base.DoLayoutComponent();
            }
            finally
            {
                this.StyleStack.Pop();
            }
        }

        /// <summary>
        /// Builds an isolated positioned region for this state - registered on the current block
        /// (BeginNewPositionedRegion requires that), but marked ExcludeFromOutput so the page's own
        /// render pass skips it entirely. The widget renders it explicitly and separately instead.
        /// </summary>
        protected override PDFLayoutXObjectRun CreateAndAddInput(PDFPositionOptions pos)
        {
            PDFLayoutBlock containerBlock = this.DocumentLayout.CurrentPage.LastOpenBlock();

            PDFLayoutRegion isolated = containerBlock.BeginNewPositionedRegion(pos, this.DocumentLayout.CurrentPage,
                this.Component, this.FullStyle, isfloating: false, addAssociatedRun: false);
            isolated.ExcludeFromOutput = true;

            this.Line = isolated.BeginNewLine();

            return this.Line.AddXObjectRun(this, this.Field, isolated, pos, this.FullStyle);
        }

        /// <summary>
        /// This run was never attached to any real line/flow, so there's nothing to wrap or close.
        /// </summary>
        protected override void CompleteLineFlow(PDFLayoutXObjectRun xObject, PDFPositionOptions pos)
        {
        }

        /// <summary>
        /// Not a real widget - the base class already captured the result into Result right
        /// after closing xObject, which is all the orchestrating LayoutEngineStatedButton needs.
        /// </summary>
        protected override void RegisterAppearances(PDFLayoutXObjectRun xObject, PDFPositionOptions pos)
        {
        }
    }
}
