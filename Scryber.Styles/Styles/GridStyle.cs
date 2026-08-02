using System.ComponentModel;
using Scryber.Drawing;

namespace Scryber.Styles
{
    [PDFParsableComponent("Grid")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GridStyle : StyleItemBase
    {
        public GridStyle() : base(StyleKeys.GridItemKey)
        {
        }

        public string TemplateColumns
        {
            get { string v; return this.TryGetValue(StyleKeys.GridTemplateColumnsKey, out v) ? v : null; }
            set { this.SetValue(StyleKeys.GridTemplateColumnsKey, value); }
        }

        public void RemoveTemplateColumns() { this.RemoveValue(StyleKeys.GridTemplateColumnsKey); }

        public string TemplateRows
        {
            get { string v; return this.TryGetValue(StyleKeys.GridTemplateRowsKey, out v) ? v : null; }
            set { this.SetValue(StyleKeys.GridTemplateRowsKey, value); }
        }

        public void RemoveTemplateRows() { this.RemoveValue(StyleKeys.GridTemplateRowsKey); }

        public GridLineValue ColumnStart
        {
            get { GridLineValue v; return this.TryGetValue(StyleKeys.GridColumnStartKey, out v) ? v : GridLineValue.Unset; }
            set { this.SetValue(StyleKeys.GridColumnStartKey, value); }
        }

        public void RemoveColumnStart() { this.RemoveValue(StyleKeys.GridColumnStartKey); }

        public GridLineValue ColumnEnd
        {
            get { GridLineValue v; return this.TryGetValue(StyleKeys.GridColumnEndKey, out v) ? v : GridLineValue.Unset; }
            set { this.SetValue(StyleKeys.GridColumnEndKey, value); }
        }

        public void RemoveColumnEnd() { this.RemoveValue(StyleKeys.GridColumnEndKey); }

        public GridLineValue RowStart
        {
            get { GridLineValue v; return this.TryGetValue(StyleKeys.GridRowStartKey, out v) ? v : GridLineValue.Unset; }
            set { this.SetValue(StyleKeys.GridRowStartKey, value); }
        }

        public void RemoveRowStart() { this.RemoveValue(StyleKeys.GridRowStartKey); }

        public GridLineValue RowEnd
        {
            get { GridLineValue v; return this.TryGetValue(StyleKeys.GridRowEndKey, out v) ? v : GridLineValue.Unset; }
            set { this.SetValue(StyleKeys.GridRowEndKey, value); }
        }

        public void RemoveRowEnd() { this.RemoveValue(StyleKeys.GridRowEndKey); }

        public GridAutoFlow AutoFlow
        {
            get { GridAutoFlow v; return this.TryGetValue(StyleKeys.GridAutoFlowKey, out v) ? v : GridAutoFlow.Row; }
            set { this.SetValue(StyleKeys.GridAutoFlowKey, value); }
        }

        public void RemoveAutoFlow() { this.RemoveValue(StyleKeys.GridAutoFlowKey); }
    }
}
