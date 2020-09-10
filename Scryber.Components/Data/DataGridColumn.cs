using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Data
{
    [PDFRequiredFramework("0.8.4")]
    public abstract class DataGridColumn : VisualComponent
    {

        [PDFAttribute("header-text")]
        [PDFDesignable("Header Text", Category = "General", Priority = 4, Type = "String")]
        public string HeaderText { get; set; }

        [PDFAttribute("footer-text")]
        [PDFDesignable(Ignore = true)]
        public string FooterText { get; set; }

      
        private object _hasheader;

        /// <summary>
        /// Allows the explict setting and retrival of the flag that identifies if this column has a header.
        /// </summary>
        /// <remarks>If this is set then this is the value that is returned. 
        /// If not set, then it defaults to returning true if this column has any valid string for it's HeaderText.
        /// Inheritors can override.</remarks>
        [PDFAttribute("has-header")]
        [PDFDesignable(Ignore = true)]
        public virtual bool HasHeader
        {
            get { return (null == _hasheader) ? (!string.IsNullOrEmpty(this.HeaderText)) : (bool)_hasheader; }
            set { _hasheader = value; }
        }

        private object _hasfooter;

        /// <summary>
        /// Allows the explict setting and retrival of the flag that identifies if this column has a footer.
        /// </summary>
        /// <remarks>If this is set then this is the value that is returned. 
        /// If not set, then it defaults to returning true if this column has any valid string for it's FooterText.
        /// Inheritors can override this behaviour.</remarks>
        [PDFAttribute("has-footer")]
        [PDFDesignable(Ignore = true)]
        public virtual bool HasFooter
        {
            get { return (null == _hasfooter) ? (!string.IsNullOrEmpty(this.FooterText)) : (bool)_hasfooter; }
            set { _hasfooter = value; }
        }
        //
        // style class properties
        //

        [PDFAttribute("cell-class")]
        [PDFDesignable("Cell Class", Category = "Style Classes", Priority = 1, Type = "String")]
        public string CellClass { get; set; }

        [PDFAttribute("alternating-cell-class")]
        [PDFDesignable("Alternate Cell Class", Category = "Style Classes", Priority = 2, Type = "String")]
        public string AlternatingCellClass { get; set; }

        [PDFAttribute("header-cell-class")]
        [PDFDesignable("Header Cell Class", Category = "Style Classes", Priority = 3, Type = "String")]
        public string HeaderCellClass { get; set; }

        [PDFAttribute("footer-cell-class")]
        [PDFDesignable("Footer Cell Class", Category = "Style Classes", Priority = 4, Type = "String")]
        public string FooterCellClass { get; set; }

        //
        // style value properties
        //

        private PDFStyle _itemstyle, _altitemstyle, _headstyle, _footstyle;

        #region  public PDFStyle CellStyle {get;}

        /// <summary>
        /// Gets the style that will be applied to the container of the item template.
        /// </summary>
        [PDFElement("CellStyle")]
        public PDFStyle CellStyle
        {
            get
            {
                if (null == _itemstyle)
                    _itemstyle = new PDFStyle();
                return _itemstyle;
            }
        }

        #endregion

        #region public PDFStyle AlternatingCellStyle {get;}

        /// <summary>
        /// Gets the style that will be applied to the container of any alternatint template.
        /// </summary>
        [PDFElement("AlternatingCellStyle")]
        public PDFStyle AlternatingCellStyle
        {
            get
            {
                if (null == _altitemstyle)
                    _altitemstyle = new PDFStyle();
                return _altitemstyle;
            }
        }

        #endregion

        #region public PDFStyle HeaderCellStyle {get;}

        /// <summary>
        /// Gets the style that will be applied to the container of any header template
        /// </summary>
        [PDFElement("HeaderCellStyle")]
        public PDFStyle HeaderCellStyle
        {
            get
            {
                if (null == _headstyle)
                    _headstyle = new PDFStyle();
                return _headstyle;
            }
        }

        #endregion

        #region public PDFStyle FooterCellStyle {get;}

        /// <summary>
        /// Gets the style that will be applied to the continer of any footer template
        /// </summary>
        [PDFElement("FooterCellStyle")]
        public PDFStyle FooterCellStyle
        {
            get
            {
                if (null == _footstyle)
                    _footstyle = new PDFStyle();
                return _footstyle;
            }
        }

        #endregion

        #region public bool AllInvisible {get;private set;}

        /// <summary>
        /// Returns true if all the cells in this column are invisible
        /// </summary>
        public bool AllInvisible
        {
            get;
            private set;
        }

        #endregion


        int gridColumnIndex = -1;
        TableGrid gridRef = null;

        //
        // .ctor
        //

        protected DataGridColumn(PDFObjectType type)
            : base(type)
        {
        }

        protected override void DoDataBind(PDFDataContext context, bool includeChildren)
        {
            if (null != this._itemstyle)
                this._itemstyle.DataBind(context);
            if (null != this._altitemstyle)
                this._altitemstyle.DataBind(context);
            if (null != this._headstyle)
                this._headstyle.DataBind(context);
            if (null != this._footstyle)
                this._footstyle.DataBind(context);

            base.DoDataBind(context, includeChildren);
        }


        public virtual Component DoBuildHeaderCell(TableGrid grid, TableRow row, int rowindex, int columnindex, PDFDataContext context)
        {
            if (context.TraceLog.ShouldLog(TraceLevel.Debug))
                context.TraceLog.Add(TraceLevel.Debug, "DataGrid Columns", string.Format("Building header cell {0}{1} with column '{2}'", rowindex, columnindex, string.IsNullOrEmpty(this.HeaderText) ? this.ID : this.HeaderText));


            TableHeaderCell cell = new TableHeaderCell();
            row.Cells.Add(cell);


            //keep a reference of the column index and grid for later
            gridColumnIndex = columnindex;
            gridRef = grid;

            cell.DataBound += headerCell_DataBound;

            ApplyHeaderCellStyles(cell);

            Label lbl = new Label();
            lbl.Text = this.HeaderText;
            cell.Contents.Add(lbl);

            
            lbl.DataBound += headerlbl_DataBinding;

            return cell;
        }

        void headerCell_DataBound(object sender, PDFDataBindEventArgs e)
        {
            TableCell cell = (TableCell)sender;

            ApplyHeaderCellStyles(cell);
        }

        private void ApplyHeaderCellStyles(TableCell cell)
        {
            if (!string.IsNullOrEmpty(this.HeaderCellClass))
                cell.StyleClass = this.HeaderCellClass;
            else
                cell.StyleClass = this.CellClass;

            if (this.HasStyle && this.Style.HasValues)
                this.Style.MergeInto(cell.Style);

            if (null != this._headstyle && _headstyle.HasValues)
                this._headstyle.MergeInto(cell.Style);
        }

        void headerlbl_DataBinding(object sender, PDFDataBindEventArgs args)
        {
            this.DataBind(args.Context);
            Label lbl = (Label)sender;
            lbl.Text = this.HeaderText;

            if (!string.IsNullOrEmpty(this.HeaderText))
                this.HasHeader = true;
        }

        public virtual Component DoBuildItemCell(TableGrid grid, TableRow row, int rowindex, int columnindex, PDFDataContext context)
        {
            if (context.TraceLog.ShouldLog(TraceLevel.Debug))
                context.TraceLog.Add(TraceLevel.Debug, "DataGrid Columns", string.Format("Building item cell {0}{1} with column '{2}'", rowindex, columnindex, string.IsNullOrEmpty(this.HeaderText) ? this.ID : this.HeaderText));

            TableCell cell = new TableCell();
            
            row.Cells.Add(cell);

            

            if (gridColumnIndex < 0)
                gridColumnIndex = columnindex;
            else if (gridColumnIndex != columnindex)
                throw new ArgumentOutOfRangeException("This grid column is trying to put a cell into an column index that is different to the original index");
            gridRef = grid;

            cell.DataBound += cell_DataBound;
            return cell;
        }

        void cell_DataBound(object sender, PDFDataBindEventArgs e)
        {
            if (this.Visible)
                this.AllInvisible = false;

            int rowindex = e.Context.CurrentIndex;
            TableCell cell = (TableCell)sender;

            cell.StyleClass = this.CellClass;

            if (!string.IsNullOrEmpty(this.DataStyleIdentifier))
                cell.DataStyleIdentifier = this.DataStyleIdentifier + "_itemcell";

            if (rowindex % 2 == 1 && !string.IsNullOrEmpty(this.AlternatingCellClass))
            {
                if (!string.IsNullOrEmpty(cell.StyleClass))
                    cell.StyleClass += " " + this.AlternatingCellClass;
                else
                    cell.StyleClass = this.AlternatingCellClass;

                if (!string.IsNullOrEmpty(this.DataStyleIdentifier))
                    cell.DataStyleIdentifier = this.DataStyleIdentifier + "_altitemcell";
            }
            

            if (this.HasStyle && this.Style.HasValues)
                this.Style.MergeInto(cell.Style);


            if (rowindex % 2 == 1 && (null != this._altitemstyle && _altitemstyle.HasValues))
                _altitemstyle.MergeInto(cell.Style);
            else if (null != this._itemstyle && _itemstyle.HasValues)
                _itemstyle.MergeInto(cell.Style);
        }


        public virtual Component DoBuildFooterCell(TableGrid grid, TableRow row, int rowindex, int columnindex, PDFDataContext context)
        {
            if (context.TraceLog.ShouldLog(TraceLevel.Debug))
                context.TraceLog.Add(TraceLevel.Debug, "DataGrid Columns", string.Format("Building footer cell {0}{1} with column '{2}'", rowindex, columnindex, string.IsNullOrEmpty(this.HeaderText) ? this.ID : this.HeaderText));

            TableFooterCell cell = new TableFooterCell();
            row.Cells.Add(cell);

            cell.DataBound +=cell_DataBound;

            Label lbl = new Label();
            lbl.Text = this.FooterText;
            cell.Contents.Add(lbl);

            lbl.DataBound += new PDFDataBindEventHandler(footerlbl_DataBound);

            if (gridColumnIndex < 0)
                gridColumnIndex = columnindex;
            else if (gridColumnIndex != columnindex)
                throw new ArgumentOutOfRangeException("This grid column is trying to put a cell into an column index that is different to the original index");
            gridRef = grid;

            return cell;
        }

        void footerCell_DataBound(object sender, PDFDataBindEventArgs args)
        {
            TableCell cell = (TableCell)sender;
            cell.StyleClass = this.HeaderCellClass;

            if (this.HasStyle && this.Style.HasValues)
                this.Style.MergeInto(cell.Style);

            if (null != this._footstyle && _footstyle.HasValues)
                this._footstyle.MergeInto(cell.Style);
        }

        void footerlbl_DataBound(object sender, PDFDataBindEventArgs args)
        {
            this.DataBind(args.Context);
            Label lbl = (Label)sender;
            lbl.Text = this.FooterText;

            if (!string.IsNullOrEmpty(this.FooterText))
                this.HasFooter = true;
        }

        public virtual bool RemoveIfColumnHidden(PDFDataContext context)
        {
            if (this.AllInvisible)
            {
                this.HideEntireColumnAsAllInivisible();
                return true;
            }
            return false;
        }

        private void HideEntireColumnAsAllInivisible()
        {
            if (gridColumnIndex > -1 && gridRef != null)
            {
#if HideDontRemoveColumns

                foreach (PDFTableRow row in gridRef.Rows)
                {
                    row.Cells[this.gridColumnIndex].Visible = false;
                }
#else
                foreach (TableRow row in gridRef.Rows)
                {
                    TableCell cell = row.Cells[gridColumnIndex];
                    row.Cells.Remove(cell);
                }
#endif
            }
        }

        

        public virtual void SetDataSourceBindingItem(PDFDataItem item, PDFDataContext context)
        {
            this.ApplyAutoBindingMember(item);
        }

        protected abstract void ApplyAutoBindingMember(PDFDataItem item);

        /// <summary>
        /// Evaluates the XPath expression against the current data context - raising an exception if there is no current data on the stack
        /// </summary>
        /// <param name="path"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected object AssertGetDataItemValue(string path, PDFDataBindEventArgs args)
        {
            if (args.Context.DataStack.HasData == false)
                throw new PDFDataException(Errors.CouldNotBindDataComponent);

            object value = null;
            try
            {

                value = args.Context.DataStack.Source.Evaluate(path, args.Context.DataStack.Current, args.Context);
            }
            catch (Exception ex)
            {
                throw new PDFDataException(Errors.CouldNotBindDataComponent, ex);
            }
            
            return value;
        }
    }

    
    

    public class PDFDataGridColumnCollection : ComponentWrappingList<DataGridColumn>
    {

        public PDFDataGridColumnCollection(DataGrid grid)
            : base(new ComponentList(grid,(PDFObjectType)"DgCo"))
        {
        }
    }
}
