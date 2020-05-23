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
    public class PDFDataGridHtmlColumn : PDFDataGridColumn
    {
        

        [PDFAttribute("content")]
        [PDFDesignable("Cell Html Fragment", Category = "Data", Priority = 1, Type = "String")]
        public string Contents { get; set; }
        


        public PDFDataGridHtmlColumn()
            : this((PDFObjectType)"DgLk")
        {
        }

        protected PDFDataGridHtmlColumn(PDFObjectType type)
            : base(type)
        {
        }

        public override PDFComponent DoBuildItemCell(PDFTableGrid grid, PDFTableRow row, int rowindex, int columnindex, PDFDataContext context)
        {
            PDFTableCell cell = (PDFTableCell)base.DoBuildItemCell(grid, row, rowindex, columnindex, context);
            PDFHtmlFragment fragment = new PDFHtmlFragment();
            fragment.DataBinding += new PDFDataBindEventHandler(link_DataBinding);
            cell.Contents.Add(fragment);

            return cell;
        }

        void link_DataBinding(object sender, PDFDataBindEventArgs args)
        {
            this.DataBind(args.Context);

            PDFHtmlFragment fragment = (PDFHtmlFragment)sender;

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
