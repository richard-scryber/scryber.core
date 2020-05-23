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
    public class PDFDataGridNumberColumn : PDFDataGridColumn
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
        [PDFAttribute("number-format", Scryber.Styles.PDFStyle.PDFStylesNamespace)]
        [PDFDesignable("Number Format", Category = "Data", Priority = 1, Type = "NumberFormat")]
        public string NumberFormat
        {
            get
            {
                PDFStyleValue<string> format;
                if (this.HasStyle && this.Style.TryGetValue(PDFStyleKeys.TextNumberFormatKey, out format))
                    return format.Value;
                else
                    return string.Empty;
            }
            set
            {
                this.Style.SetValue(PDFStyleKeys.TextNumberFormatKey, value);
            }
        }

        #endregion

        public PDFDataGridNumberColumn()
            : this((PDFObjectType)"DgNc")
        { }

        protected PDFDataGridNumberColumn(PDFObjectType type)
            : base(type)
        {
        }


        public override PDFComponent DoBuildItemCell(PDFTableGrid grid, PDFTableRow row, int rowindex, int columnindex, PDFDataContext context)
        {
            PDFTableCell cell = (PDFTableCell)base.DoBuildItemCell(grid, row, rowindex, columnindex, context);

            PDFNumber lbl = new PDFNumber();
            lbl.Value = this.Value;
            lbl.DataBinding += new PDFDataBindEventHandler(number_DataBinding);

            cell.Contents.Add(lbl);

            return cell;
        }

        
        

        void number_DataBinding(object sender, PDFDataBindEventArgs args)
        {
            this.DataBind(args.Context);
            PDFNumber num = ((PDFNumber)sender);
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
