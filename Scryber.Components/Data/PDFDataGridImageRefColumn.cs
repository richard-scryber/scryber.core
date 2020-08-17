using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;

namespace Scryber.Data
{
    [PDFParsableComponent("ImageColumn")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_datagridcolumn")]
    public class PDFDataGridImageRefColumn : PDFDataGridColumn
    {
        [PDFAttribute("source")]
        [PDFDesignable("Source", Category = "Data", Priority = 1, Type = "String")]
        public string ImageSource { get; set; }

        [PDFAttribute("hide-on-fail")]
        [PDFDesignable("Hide if fail", Category = "General", Priority = 5, Type = "Boolean")]
        public bool HideOnFail
        {
            get;
            set;
        }

        public PDFDataGridImageRefColumn()
            : this((PDFObjectType)"DgIr")
        {
        }

        protected PDFDataGridImageRefColumn(PDFObjectType type)
            : base(type)
        {
        }


        public override PDFComponent DoBuildItemCell(PDFTableGrid grid, PDFTableRow row, int rowindex, int columnindex, PDFDataContext context)
        {
            PDFTableCell cell = (PDFTableCell)base.DoBuildItemCell(grid, row, rowindex, columnindex, context);
            PDFImage img = new PDFImage();
            cell.Contents.Add(img);
            img.DataBinding += new PDFDataBindEventHandler(img_DataBinding);

            return cell;
        }

        void img_DataBinding(object sender, PDFDataBindEventArgs args)
        {
            this.DataBind(args.Context);
            PDFImage img = (PDFImage)sender;

            if (null != this._autobindItemPath)
                this.ImageSource = AssertGetDataItemValue(this._autobindItemPath, args) as string;

            img.Source = this.ImageSource;
            img.AllowMissingImages = this.HideOnFail;
            img.Visible = this.Visible;
            
        }


        private string _autobindItemPath;

        protected override void ApplyAutoBindingMember(PDFDataItem item)
        {
            this._autobindItemPath = item.RelativePath;
        }

    }
}
