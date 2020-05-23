using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;

namespace Scryber.Data
{
    [PDFParsableComponent("TextColumn")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_datagridcolumn")]
    public class PDFDataGridTextColumn : PDFDataGridColumn
    {
        private string _txt;

        [PDFAttribute("text")]
        [PDFDesignable("Text",Category ="Data",Priority =1,Type = "String")]
        public string Text {
            get { return this._txt; }
            set { this._txt = value; }
        }


        public PDFDataGridTextColumn()
            : this((PDFObjectType)"DgTc")
        { }

        protected PDFDataGridTextColumn(PDFObjectType type)
            : base(type)
        {
        }


        public override PDFComponent DoBuildItemCell(PDFTableGrid grid, PDFTableRow row, int rowindex, int columnindex, PDFDataContext context)
        {
            PDFTableCell cell = (PDFTableCell)base.DoBuildItemCell(grid, row, rowindex, columnindex, context);

            PDFLabel lbl = new PDFLabel();
            lbl.Text = this.Text;
            lbl.DataBinding += new PDFDataBindEventHandler(lbl_DataBinding);

            cell.Contents.Add(lbl);

            return cell;
        }

       

        void lbl_DataBinding(object sender, PDFDataBindEventArgs args)
        {
            this.DataBind(args.Context);

            if (null != _autobindItemPath)
            {
                object value = AssertGetDataItemValue(_autobindItemPath, args);
                if (null != value)
                    this.Text = value.ToString();
                else
                    this.Text = "";
            }

            PDFLabel lbl = (PDFLabel)sender;
            lbl.Text = this.Text;
            lbl.Visible = this.Visible;
        }

        

        private string _autobindItemPath;

        protected override void ApplyAutoBindingMember(PDFDataItem item)
        {
            this._autobindItemPath = item.RelativePath;
        }

    }

}
