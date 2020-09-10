using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;

namespace Scryber.Data
{
    [PDFParsableComponent("ImageDataColumn")]
    public class DataGridImageDataColumn : DataGridColumn
    {
        [PDFAttribute("data")]
        public Base64Data ImageData { get; set; }

        [PDFAttribute("key")]
        public string ImageKey { get; set; }

        public DataGridImageDataColumn()
            : this((PDFObjectType)"DgDi")
        {
        }

        protected DataGridImageDataColumn(PDFObjectType type)
            : base(type)
        {
        }

        public override Component DoBuildItemCell(TableGrid grid, TableRow row, int rowindex, int columnindex, PDFDataContext context)
        {
            TableCell cell = (TableCell)base.DoBuildItemCell(grid, row, rowindex, columnindex, context);
            DataImage dimg = new DataImage();
            cell.Contents.Add(dimg);
            dimg.DataBinding += new PDFDataBindEventHandler(dimg_DataBinding);

            return cell;
        }

        void dimg_DataBinding(object sender, PDFDataBindEventArgs args)
        {
            this.DataBind(args.Context);
            DataImage dimg = (DataImage)sender;
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

        protected virtual Base64Data ConvertValueToBase64Data(object value)
        {
            if (null == value)
                return null;
            else if (value is string)
                return Base64Data.Parse((string)value);
            else
                throw new PDFDataException(Errors.CountNoConvertBinaryDataToBitmap);
        }

        protected virtual string GetImageKey(PDFDataContext context)
        {
            return this.UniqueID + ":" + context.CurrentIndex.ToString();
        }
    }
}
