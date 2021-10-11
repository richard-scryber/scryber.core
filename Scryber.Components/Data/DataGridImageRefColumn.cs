using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;

namespace Scryber.Data
{
    [PDFParsableComponent("ImageColumn")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_datagridcolumn")]
    public class DataGridImageRefColumn : DataGridColumn
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

        public DataGridImageRefColumn()
            : this((ObjectType)"DgIr")
        {
        }

        protected DataGridImageRefColumn(ObjectType type)
            : base(type)
        {
        }


        public override Component DoBuildItemCell(TableGrid grid, TableRow row, int rowindex, int columnindex, DataContext context)
        {
            TableCell cell = (TableCell)base.DoBuildItemCell(grid, row, rowindex, columnindex, context);
            Image img = new Image();
            cell.Contents.Add(img);
            img.DataBinding += new DataBindEventHandler(img_DataBinding);

            return cell;
        }

        void img_DataBinding(object sender, DataBindEventArgs args)
        {
            this.DataBind(args.Context);
            Image img = (Image)sender;

            if (null != this._autobindItemPath)
                this.ImageSource = AssertGetDataItemValue(this._autobindItemPath, args) as string;

            img.Source = this.ImageSource;
            img.AllowMissingImages = this.HideOnFail;
            img.Visible = this.Visible;
            
        }


        private string _autobindItemPath;

        protected override void ApplyAutoBindingMember(DataItem item)
        {
            this._autobindItemPath = item.RelativePath;
        }

    }
}
