using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Data
{
    [PDFParsableComponent("NumberColumn")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_datagridcolumn")]
    public class DataGridNumberColumn : DataGridColumn
    {

        #region public double Value { get; set; }

        [PDFAttribute("value")]
        [PDFDesignable("Value", Category = "Data", Priority = 1, Type = "Number")]
        public double Value { get; set; }

        #endregion

        #region public string NumberFormat {get;set;}

        /// <summary>
        /// Gets or sets the format to use to convert the number value to a string
        /// </summary>
        [PDFAttribute("number-format", Scryber.Styles.Style.PDFStylesNamespace)]
        [PDFDesignable("Number Format", Category = "Data", Priority = 1, Type = "NumberFormat")]
        public string NumberFormat
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.TextNumberFormatKey, string.Empty);
                else
                    return string.Empty;
            }
            set
            {
                this.Style.SetValue(StyleKeys.TextNumberFormatKey, value);
            }
        }

        #endregion

        public DataGridNumberColumn()
            : this((ObjectType)"DgNc")
        { }

        protected DataGridNumberColumn(ObjectType type)
            : base(type)
        {
        }


        public override Component DoBuildItemCell(TableGrid grid, TableRow row, int rowindex, int columnindex, PDFDataContext context)
        {
            TableCell cell = (TableCell)base.DoBuildItemCell(grid, row, rowindex, columnindex, context);

            Number lbl = new Number();
            lbl.Value = this.Value;
            lbl.DataBinding += new PDFDataBindEventHandler(number_DataBinding);

            cell.Contents.Add(lbl);

            return cell;
        }

        
        

        void number_DataBinding(object sender, PDFDataBindEventArgs args)
        {
            this.DataBind(args.Context);
            Number num = ((Number)sender);
            if (null != _autobindItemPath)
            {
                object value = AssertGetDataItemValue(this._autobindItemPath, args);
                if (null != value)
                {
                    string numStr = value.ToString();
                    if (!string.IsNullOrEmpty(numStr))
                    {
                        this.Value = double.Parse(numStr);
                    }
                }
            }
            num.Value = this.Value;
            num.NumberFormat = this.NumberFormat;
            num.Visible = this.Visible;
        }

        private string _autobindItemPath;

        protected override void ApplyAutoBindingMember(PDFDataItem item)
        {
            this._autobindItemPath = item.RelativePath;
        }
    }
}
