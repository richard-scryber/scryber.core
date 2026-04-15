using System.ComponentModel;

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

        public int ColumnSpan
        {
            get { int v; return this.TryGetValue(StyleKeys.GridColumnSpanKey, out v) ? v : 1; }
            set { this.SetValue(StyleKeys.GridColumnSpanKey, value); }
        }

        public void RemoveColumnSpan() { this.RemoveValue(StyleKeys.GridColumnSpanKey); }

        public int RowSpan
        {
            get { int v; return this.TryGetValue(StyleKeys.GridRowSpanKey, out v) ? v : 1; }
            set { this.SetValue(StyleKeys.GridRowSpanKey, value); }
        }

        public void RemoveRowSpan() { this.RemoveValue(StyleKeys.GridRowSpanKey); }
    }
}
