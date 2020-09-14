using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;
using Scryber.Html;

namespace Scryber.Data
{
    [PDFParsableComponent("HtmlColumn")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_datagridcolumn")]
    public class DataGridHtmlColumn : DataGridColumn
    {
        

        [PDFAttribute("content")]
        [PDFDesignable("Cell Html Fragment", Category = "Data", Priority = 1, Type = "String")]
        public string Contents { get; set; }
        


        public DataGridHtmlColumn()
            : this((PDFObjectType)"DgLk")
        {
        }

        protected DataGridHtmlColumn(PDFObjectType type)
            : base(type)
        {
        }

        public override Component DoBuildItemCell(TableGrid grid, TableRow row, int rowindex, int columnindex, PDFDataContext context)
        {
            TableCell cell = (TableCell)base.DoBuildItemCell(grid, row, rowindex, columnindex, context);
            HtmlFragment fragment = new HtmlFragment();
            fragment.DataBinding += new PDFDataBindEventHandler(link_DataBinding);
            cell.Contents.Add(fragment);

            return cell;
        }

        void link_DataBinding(object sender, PDFDataBindEventArgs args)
        {
            this.DataBind(args.Context);

            HtmlFragment fragment = (HtmlFragment)sender;

            if (null != this._autobindItemPath)
            {
                string html = AssertGetDataItemValue(_autobindItemPath, args) as string;
                if (!string.IsNullOrEmpty(html))
                {
                    this.Contents = html;
                }

            }
            
            fragment.Visible = this.Visible;
            fragment.RawContents = this.Contents;

            
        }

        private string _autobindItemPath;

        protected override void ApplyAutoBindingMember(PDFDataItem item)
        {
            this._autobindItemPath = item.RelativePath;
        }
        
    }
}
