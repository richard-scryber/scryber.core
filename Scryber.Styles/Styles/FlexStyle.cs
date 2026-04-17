using System.ComponentModel;
using Scryber.Drawing;

namespace Scryber.Styles
{
    [PDFParsableComponent("Flex")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FlexStyle : StyleItemBase
    {
        public FlexStyle() : base(StyleKeys.FlexItemKey)
        {
        }

        public FlexDirection Direction
        {
            get { FlexDirection v; return this.TryGetValue(StyleKeys.FlexDirectionKey, out v) ? v : FlexDirection.Row; }
            set { this.SetValue(StyleKeys.FlexDirectionKey, value); }
        }

        public void RemoveDirection() { this.RemoveValue(StyleKeys.FlexDirectionKey); }

        public FlexWrap Wrap
        {
            get { FlexWrap v; return this.TryGetValue(StyleKeys.FlexWrapKey, out v) ? v : FlexWrap.Nowrap; }
            set { this.SetValue(StyleKeys.FlexWrapKey, value); }
        }

        public void RemoveWrap() { this.RemoveValue(StyleKeys.FlexWrapKey); }

        public FlexJustify JustifyContent
        {
            get { FlexJustify v; return this.TryGetValue(StyleKeys.FlexJustifyKey, out v) ? v : FlexJustify.FlexStart; }
            set { this.SetValue(StyleKeys.FlexJustifyKey, value); }
        }

        public void RemoveJustifyContent() { this.RemoveValue(StyleKeys.FlexJustifyKey); }

        public FlexAlignMode AlignItems
        {
            get { FlexAlignMode v; return this.TryGetValue(StyleKeys.FlexAlignItemsKey, out v) ? v : FlexAlignMode.Stretch; }
            set { this.SetValue(StyleKeys.FlexAlignItemsKey, value); }
        }

        public void RemoveAlignItems() { this.RemoveValue(StyleKeys.FlexAlignItemsKey); }

        public FlexAlignMode AlignContent
        {
            get { FlexAlignMode v; return this.TryGetValue(StyleKeys.FlexAlignContentKey, out v) ? v : FlexAlignMode.Stretch; }
            set { this.SetValue(StyleKeys.FlexAlignContentKey, value); }
        }

        public void RemoveAlignContent() { this.RemoveValue(StyleKeys.FlexAlignContentKey); }

        public Unit Gap
        {
            get { Unit v; return this.TryGetValue(StyleKeys.FlexGapKey, out v) ? v : Unit.Zero; }
            set { this.SetValue(StyleKeys.FlexGapKey, value); }
        }

        public void RemoveGap() { this.RemoveValue(StyleKeys.FlexGapKey); }

        public Unit RowGap
        {
            get { Unit v; return this.TryGetValue(StyleKeys.FlexRowGapKey, out v) ? v : Unit.Zero; }
            set { this.SetValue(StyleKeys.FlexRowGapKey, value); }
        }

        public void RemoveRowGap() { this.RemoveValue(StyleKeys.FlexRowGapKey); }

        public Unit ColumnGap
        {
            get { Unit v; return this.TryGetValue(StyleKeys.FlexColumnGapKey, out v) ? v : Unit.Zero; }
            set { this.SetValue(StyleKeys.FlexColumnGapKey, value); }
        }

        public void RemoveColumnGap() { this.RemoveValue(StyleKeys.FlexColumnGapKey); }

        public double Grow
        {
            get { double v; return this.TryGetValue(StyleKeys.FlexGrowKey, out v) ? v : 0.0; }
            set { this.SetValue(StyleKeys.FlexGrowKey, value); }
        }

        public void RemoveGrow() { this.RemoveValue(StyleKeys.FlexGrowKey); }

        public double Shrink
        {
            get { double v; return this.TryGetValue(StyleKeys.FlexShrinkKey, out v) ? v : 1.0; }
            set { this.SetValue(StyleKeys.FlexShrinkKey, value); }
        }

        public void RemoveShrink() { this.RemoveValue(StyleKeys.FlexShrinkKey); }

        public Unit Basis
        {
            get { Unit v; return this.TryGetValue(StyleKeys.FlexBasisKey, out v) ? v : Unit.Zero; }
            set { this.SetValue(StyleKeys.FlexBasisKey, value); }
        }

        public void RemoveBasis() { this.RemoveValue(StyleKeys.FlexBasisKey); }

        public bool BasisAuto
        {
            get { bool v; return this.TryGetValue(StyleKeys.FlexBasisAutoKey, out v) && v; }
            set { this.SetValue(StyleKeys.FlexBasisAutoKey, value); }
        }

        public void RemoveBasisAuto() { this.RemoveValue(StyleKeys.FlexBasisAutoKey); }

        public FlexAlignMode AlignSelf
        {
            get { FlexAlignMode v; return this.TryGetValue(StyleKeys.FlexAlignSelfKey, out v) ? v : FlexAlignMode.Auto; }
            set { this.SetValue(StyleKeys.FlexAlignSelfKey, value); }
        }

        public void RemoveAlignSelf() { this.RemoveValue(StyleKeys.FlexAlignSelfKey); }

        public int Order
        {
            get { int v; return this.TryGetValue(StyleKeys.FlexOrderKey, out v) ? v : 0; }
            set { this.SetValue(StyleKeys.FlexOrderKey, value); }
        }

        public void RemoveOrder() { this.RemoveValue(StyleKeys.FlexOrderKey); }
    }
}
