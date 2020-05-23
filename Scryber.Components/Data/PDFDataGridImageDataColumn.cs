using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;

namespace Scryber.Data
{
    [PDFParsableComponent("ImageDataColumn")]
    public class PDFDataGridImageDataColumn : PDFDataGridColumn
    {
        [PDFAttribute("data")]
        public PDFBase64Data ImageData { get; set; }

        [PDFAttribute("key")]
        public string ImageKey { get; set; }

        public PDFDataGridImageDataColumn()
            : this((PDFObjectType)"DgDi")
        {
        }

        protected PDFDataGridImageDataColumn(PDFObjectType type)
            : base(type)
        {
        }

        public override PDFComponent DoBuildItemCell(PDFTableGrid grid, PDFTableRow row, int rowindex, int columnindex, PDFDataContext context)
        {
            PDFTableCell cell = (PDFTableCell)base.DoBuildItemCell(grid, row, rowindex, columnindex, context);
            PDFDataImage dimg = new PDFDataImage();
            cell.Contents.Add(dimg);
            dimg.DataBinding += new PDFDataBindEventHandler(dimg_DataBinding);

            return cell;
        }

        void dimg_DataBinding(object sender, PDFDataBindEventArgs args)
        {
            this.DataBind(args.Context);
            PDFDataImage dimg = (PDFDataImage)sender;
            if (null != this._autobindItemPath)
            {
                object val = AssertGetDataItemValue(this._autobindItemPath, args);
                this.ImageData = ConvertValueToBase64Data(val);
                this.ImageKey = GetImageKey(args.Context);
            }

            dimg.Data = this.ImageData;
            dimg.ImageKey = this.ImageKey;
            dimg.Visible = this.Visible;
        }

        private string _autobindItemPath;

        protected override void ApplyAutoBindingMember(PDFDataItem item)
        {
            this._autobindItemPath = item.RelativePath;
        }

        protected virtual PDFBase64Data ConvertValueToBase64Data(object value)
        {
            if (null == value)
                return null;
            else if (value is string)
                return PDFBase64Data.Parse((string)value);
            else
                throw new PDFDataException(Errors.CountNoConvertBinaryDataToBitmap);
        }

        protected virtual string GetImageKey(PDFDataContext context)
        {
            return this.UniqueID + ":" + context.CurrentIndex.ToString();
        }
    }
}
