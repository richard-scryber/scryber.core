using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.PDF.Layout
{
    /// <summary>
    /// A throwaway, isolated layout pass for a single button appearance state (Normal/Down/Over). Lays
    /// the field's own children out again into a region that is never attached to the real page
    /// flow - it exists purely so the resulting PDFLayoutXObjectRun can be rendered as that
    /// state's own /AP entry. Driven directly via Layout(context, style), not the normal
    /// GetEngine/CreateLayoutEngine dispatch, so it never gets picked for a real component.
    /// </summary>
    public class LayoutEngineButtonState : LayoutEngineFormField
    {
        protected FormFieldAppearanceState AppearanceState { get; set; }
        
        public LayoutEngineButtonState(FormInputField container, IPDFLayoutEngine parent, FormFieldAppearanceState forState) : base(container, parent)
        {
            this.ShouldAddXObject = true;
            this.ShouldProxyText = false;
            this.AppearanceState = forState;
        }
        
        

        protected override void DoLayoutComponent()
        {
            PDFLayoutRegion posRegion = null;
            if (this.AppearanceState != FormFieldAppearanceState.Normal)
            {
                //For normal, we should already have our block set up.
                var pos = this.FullStyle.CreatePostionOptions(true);
                posRegion = this.BeginNewInlineBlockRegionForChild(pos, this.Field, this.FullStyle);
            }

            base.DoLayoutComponent();

            if (null != posRegion && !posRegion.IsClosed)
                posRegion.Close();
            
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
        protected override void RegisterAppearances(PDFLayoutXObjectRun xObject, PDFPositionOptions pos, Point offset =  default(Point))
        {
        }
    }
}
