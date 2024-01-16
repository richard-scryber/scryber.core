using System;
using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.Styles;
using Scryber.Svg.Components;

namespace Scryber.Svg.Layout
{
	public class LayoutEngineSVG : LayoutEngineCanvas
	{

		public bool ContainedInParentSVG { get; set; }

		public LayoutEngineSVG(SVGCanvas canvas, IPDFLayoutEngine parent)
			: base(canvas, parent)
		{
			this.ContainedInParentSVG = (parent is LayoutEngineSVG);
		}


        protected override void DoLayoutAChild(IComponent comp, Style full)
        {
			//With SVG we are wrapped in a LayoutXObject
			//This has it's own context, and view port (Matrix and BBox in PDF)
			//As such all drawing should be done within the context of the XObject
			//And is absolute

			full.SetValue(StyleKeys.PositionModeKey, Drawing.PositionMode.InlineBlock);


            base.DoLayoutAChild(comp, full);
        }
    }
}

