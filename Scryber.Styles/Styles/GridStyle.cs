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

        public GridTemplateAreasValue TemplateAreas
        {
            get { GridTemplateAreasValue v; return this.TryGetValue(StyleKeys.GridTemplateAreasKey, out v) ? v : default; }
            set { this.SetValue(StyleKeys.GridTemplateAreasKey, value); }
        }

        public void RemoveTemplateAreas() { this.RemoveValue(StyleKeys.GridTemplateAreasKey); }

        public string AreaName
        {
            get { string v; return this.TryGetValue(StyleKeys.GridAreaNameKey, out v) ? v : null; }
            set { this.SetValue(StyleKeys.GridAreaNameKey, value); }
        }

        public void RemoveAreaName() { this.RemoveValue(StyleKeys.GridAreaNameKey); }

        public GridAutoFlow AutoFlow
        {
            get { GridAutoFlow v; return this.TryGetValue(StyleKeys.GridAutoFlowKey, out v) ? v : GridAutoFlow.Row; }
            set { this.SetValue(StyleKeys.GridAutoFlowKey, value); }
        }

        public void RemoveAutoFlow() { this.RemoveValue(StyleKeys.GridAutoFlowKey); }

        public string AutoColumns
        {
            get { string v; return this.TryGetValue(StyleKeys.GridAutoColumnsKey, out v) ? v : null; }
            set { this.SetValue(StyleKeys.GridAutoColumnsKey, value); }
        }

        public void RemoveAutoColumns() { this.RemoveValue(StyleKeys.GridAutoColumnsKey); }

        public string AutoRows
        {
            get { string v; return this.TryGetValue(StyleKeys.GridAutoRowsKey, out v) ? v : null; }
            set { this.SetValue(StyleKeys.GridAutoRowsKey, value); }
        }

        public void RemoveAutoRows() { this.RemoveValue(StyleKeys.GridAutoRowsKey); }
    }
}
