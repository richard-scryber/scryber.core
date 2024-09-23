using System;
using Scryber.Components;
using Scryber.Data;
using Scryber.Drawing;
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

		public LayoutEngineSVG(SVGGroup group, IPDFLayoutEngine parent)
			: base(group, parent)
		{
			this.ContainedInParentSVG = true;
			
		}

		protected Size UsedSize { get; set; }

		protected override void DoLayoutComponent()
		{
			StyleValue<DisplayMode> mode;
			if (this.FullStyle.TryGetValue(StyleKeys.PositionDisplayKey, out mode))
			{
				if(mode.Value(this.FullStyle) == DisplayMode.Inline)
					this.FullStyle.SetValue(StyleKeys.PositionDisplayKey, DisplayMode.InlineBlock);
			}
			base.DoLayoutComponent();
		}

		protected override void DoLayoutChildren()
		{
			this.UsedSize = Size.Empty;
			this.CurrentBlock.IsExplicitLayout = true;
			this.CurrentBlock.CurrentRegion.IsExplicitLayout = true;
			
			
			this.ContinueLayout = true;
			
			if (this.TryGetComponentChildren(this.Component, out ComponentList children))
			{
				this.DoLayoutChildren(children);
			}

			if (this.ContainedInParentSVG)
			{
				this.CurrentBlock.Size = this.UsedSize;
				this.CurrentBlock.CurrentRegion.UsedSize = this.UsedSize;
			}
		}

		protected override void DoLayoutChildren(ComponentList children)
		{
			foreach (var child in children)
			{
				if(child.Visible)
					this.DoLayoutAChild(child);
				
				//Avoid the check for closed or overflow - we are inside the SVG so expect explicit positions
			}
			
		}

		protected override void DoLayoutAChild(IComponent comp, Style full)
        {
			//With SVG we are wrapped in a LayoutXObject
			//This has it's own context, and view port (Matrix and BBox in PDF)
			//As such all drawing should be done within the context of the XObject
			//And is absolute

			full.SetValue(StyleKeys.PositionModeKey, Drawing.PositionMode.Absolute);
			
			//Any x or y is set on the SVGGeometry positions.
			full.SetValue(StyleKeys.PositionXKey, 0);
			full.SetValue(StyleKeys.PositionYKey, 0);

			if (comp is IInvisibleContainer container)
			{
				ComponentList contents;
				if (this.TryGetComponentChildren(container, out contents))
					this.DoLayoutChildren(contents);
			}
			else if (comp is IPDFViewPortComponent viewPortComponent)
			{
				var block = this.CurrentBlock;
				var pos = full.CreatePostionOptions(true);
				var reg = block.BeginNewPositionedRegion(pos, this.CurrentBlock.GetLayoutPage(), comp, full, false,
					true) as PDFLayoutPositionedRegion;
				
				reg.IsExplicitLayout = true;
				
				this.DoLayoutViewPortComponent(viewPortComponent, full);

				var loc = Point.Empty;
				StyleValue<Unit> dim;
				if (full.TryGetValue(StyleKeys.SVGGeometryXKey, out dim))
					loc.X = dim.Value(full);
				if (full.TryGetValue(StyleKeys.SVGGeometryYKey, out dim))
					loc.Y = dim.Value(full);

				reg.CloseCurrentItem();
				
				if (reg.IsClosed == false)
					reg.Close();

				reg.RelativeOffset = loc;
				reg.RelativeTo = block;

			}
			else if (comp is IGraphicPathComponent path)
			{
				this.DoLayoutPathComponent(path, full);
			}
			
            //base.DoLayoutAChild(comp, full);
        }

		protected override bool AddComponentRunToLayoutWithSize(Size required, IComponent component, Style style, ref PDFLayoutLine linetoAddTo,
			PDFPositionOptions options, bool isInternalCall = false)
		{
			var pos = style.CreatePostionOptions(false);

			
			StyleValue<Unit> dim;
			if (style.TryGetValue(StyleKeys.SVGGeometryXKey, out dim))
				required.Width += dim.Value(style);
			
			if (style.TryGetValue(StyleKeys.SVGGeometryYKey, out dim))
				required.Height += dim.Value(style);
			pos.PositionMode = PositionMode.Absolute;

			this.UsedSize = Size.Max(this.UsedSize, required);

			Rect total = Rect.Empty; //reset to empty so no space is actually taken up
			linetoAddTo.AddComponentRun(component, total, total, total, Unit.Empty, pos, style);

			return true;
			
		}

		protected void ApplyViewPort()
		{
			var pos = this.FullStyle.CreatePostionOptions(true);

			Rect vp;
			if (pos.ViewPort.HasValue)
				vp = pos.ViewPort.Value;
			else
			{
				var w = pos.Width.HasValue ? pos.Width.Value : Unit.Pt(300);
				var h = pos.Height.HasValue ? pos.Height.Value : Unit.Pt(150);
				vp = new Rect(0, 0, w, h);
			}

			this.ApplyViewPort(pos, vp);
		}
	}
}

