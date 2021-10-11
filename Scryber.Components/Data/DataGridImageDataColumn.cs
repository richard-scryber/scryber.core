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
            : this((ObjectType)"DgDi")
        {
        }

        protected DataGridImageDataColumn(ObjectType type)
            : base(type)
        {
        }

        public override Component DoBuildItemCell(TableGrid grid, TableRow row, int rowindex, int columnindex, DataContext context)
        {
            TableCell cell = (TableCell)base.DoBuildItemCell(grid, row, rowindex, columnindex, context);
            DataImage dimg = new DataImage();
            cell.Contents.Add(dimg);
            dimg.DataBinding += new DataBindEventHandler(dimg_DataBinding);

            return cell;
        }

        void dimg_DataBinding(object sender, DataBindEventArgs args)
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

        protected override void ApplyAutoBindingMember(DataItem item)
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

        protected virtual string GetImageKey(DataContext context)
        {
            return this.UniqueID + ":" + context.CurrentIndex.ToString();
        }
    }
}
