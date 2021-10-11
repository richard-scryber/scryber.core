using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Data
{
    [PDFParsableComponent("DateColumn")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_datagridcolumn")]
    public class DataGridDateColumn : DataGridColumn
    {
        #region public DateTime Value { get; set; }

        [PDFAttribute("value")]
        [PDFDesignable("Value", Category = "Data", Priority = 5, Type = "Date")]
        public DateTime Value { get; set; }

        #endregion

        #region public string DateFormat {get;set;}

        /// <summary>
        /// Gets or sets the format to use to convert the number value to a string
        /// </summary>
        [PDFAttribute("date-format", Scryber.Styles.Style.PDFStylesNamespace)]
        [PDFDesignable("Date Format", Category = "General", Priority = 5, Type = "DateFormat")]
        public string DateFormat
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.TextDateFormatKey, string.Empty);
                else
                    return string.Empty;
            }
            set
            {
                this.Style.Text.DateFormat = value;
            }
        }

        #endregion

        public DataGridDateColumn()
            : this((ObjectType)"DgDc")
        {
        }

        protected DataGridDateColumn(ObjectType type)
            : base(type)
        {
        }

        public override Component DoBuildItemCell(TableGrid grid, TableRow row, int rowindex, int columnindex, DataContext context)
        {
            TableCell cell = (TableCell)base.DoBuildItemCell(grid, row, rowindex, columnindex, context);

            Date lbl = new Date();
            lbl.Value = this.Value;
            lbl.Visible = this.Visible;
            lbl.DataBinding += new DataBindEventHandler(date_DataBinding);

            cell.Contents.Add(lbl);

            return cell;
        }



        void date_DataBinding(object sender, DataBindEventArgs args)
        {
            this.DataBind(args.Context);
            Date num = ((Date)sender);

            //check for the autobind item path.
            if (null != this._autobindItemPath)
            {
                object val = AssertGetDataItemValue(this._autobindItemPath, args);
                DateTime result;
                if (null != val && !string.IsNullOrEmpty(val.ToString()) && DateTime.TryParse(val.ToString(), out result))
                    num.Value = result;
            }
            else
                num.Value = this.Value;

            num.DateFormat = this.DateFormat;
        }

        

        private string _autobindItemPath;

        protected override void ApplyAutoBindingMember(DataItem item)
        {
            this._autobindItemPath = item.RelativePath;
        }
    }
}
