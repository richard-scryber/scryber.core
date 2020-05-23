using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Drawing.Imaging;
using Scryber.Resources;
using Scryber.Styles;
using Scryber.Native;
using System.Drawing;
using System.ComponentModel;

namespace Scryber.Data
{
    [PDFRequiredFramework("0.8.4")]
    [PDFParsableComponent("DataImage")]
    public class PDFDataImage : PDFImageBase
    {
        public static readonly PDFObjectType DataImageType = (PDFObjectType)"DImg";
        

        [PDFElement("Data")]
        [PDFAttribute("data")]
        public PDFBase64Data Data { get; set; }


        [PDFAttribute("key")]
        public string ImageKey { get; set; }


        public PDFDataImage()
            : this(DataImageType)
        {
        }

        protected PDFDataImage(PDFObjectType type)
            : base(type)
        {
        }

        private PDFImageXObject _xobj;

       
        protected override Resources.PDFImageXObject InitImageXObject(PDFContextBase context, PDFStyle style)
        {
            PDFDocument doc = this.Document;
            if (null == doc)
                throw new NullReferenceException(Errors.ParentDocumentCannotBeNull);

            if (null != this.Data)
            {
                _xobj = null;
                if (!string.IsNullOrEmpty(this.ImageKey))
                    _xobj = this.Document.GetImageResource(this.ImageKey, this, false);

                if (null == _xobj)
                {

                    string name;
                    if (string.IsNullOrEmpty(this.ImageKey))
                        name = "DataImage_" + this.Document.GetIncrementID(PDFObjectTypes.ImageXObject);
                    else
                        name = this.ImageKey;

                    System.ComponentModel.TypeConverter BitmapConverter = TypeDescriptor.GetConverter(typeof(Bitmap));
                    Bitmap img = (Bitmap)BitmapConverter.ConvertFrom(this.Data.Raw);


                    PDFImageData data = PDFImageData.LoadImageFromBitmap(name, img, this.Compress);

                    _xobj = PDFImageXObject.Load(data, name);
                    this.Document.SharedResources.Add(_xobj);
                }
            }

            return _xobj;
        }

        #region Legacy Code

        //protected override void DoRegisterArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet set, PDFStyle fullstyle)
        //{
        //    IPDFResourceContainer resources = this.GetResourceContainer();
        //    if (null == resources)
        //        throw new NullReferenceException(string.Format(Errors.ResourceContainerOfComponentNotFound, "Image", this.ID));
        //    PDFImageXObject xobj = this.GetImageObject(context, fullstyle);

        //    if (null != xobj)
        //        resources.Register(xobj);

        //    base.DoRegisterArtefacts(context, set, fullstyle);
        //}

        //internal static PDFSize AdjustImageSize(PDFStyle style, PDFSize imgsize)
        //{
        //    PDFSize rendersize = imgsize;

        //    PDFStyleValue<PDFUnit> width;
        //    bool scaleWidth = style.TryGetValue(PDFStyleKeys.SizeWidthKey, out width);

        //    PDFStyleValue<PDFUnit> height;
        //    bool scaleHeight = style.TryGetValue(PDFStyleKeys.SizeHeightKey, out height);

        //    if (scaleWidth || scaleHeight)
        //    {

        //        if (scaleWidth)
        //        {
        //            rendersize.Width = width.Value;
        //        }
        //        if (scaleHeight)
        //        {
        //            rendersize.Height = height.Value;
        //        }


        //        if (scaleWidth && scaleHeight)
        //        {
        //            //Do nothing as the size is set for both height and width.
        //        }
        //        else if (scaleWidth)
        //        {
        //            double val = rendersize.Width.PointsValue;
        //            double scale = rendersize.Height.PointsValue / imgsize.Height.PointsValue;
        //            rendersize.Width = (PDFUnit)(val * scale);
        //        }
        //        else if (scaleHeight)
        //        {
        //            double val = rendersize.Height.PointsValue;
        //            double scale = rendersize.Width.PointsValue / imgsize.Width.PointsValue;
        //            rendersize.Height = (PDFUnit)(val * scale);
        //        }

        //        imgsize = rendersize;
        //    }
        //    return imgsize;
        //}





        //public PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        //{
        //    PDFGraphics graphics = context.Graphics;
        //    PDFStyle style;
        //    PDFComponentArrangement arrange = this.GetFirstArrangement();
        //    if (null != arrange)
        //        style = arrange.FullStyle;
        //    else
        //        style = null;

        //    PDFImageXObject img = this.GetImageObject(context, style);
        //    if (img != null)
        //    {
        //        PDFPoint pos = context.Offset;


        //        PDFSize imgsize = context.Space;

        //        //the pictures are drawn from their bottom left corner, so take off the height.
        //        //if (context.DrawingOrigin == DrawingOrigin.TopLeft)
        //        //    pos.Y = pos.Y + imgsize.Height;

        //        graphics.SaveGraphicsState();

        //        PDFStyleValue<double> op;
        //        if (null != style && style.TryGetValue(PDFStyleKeys.FillOpacityKey,out op) && op.Value < 1.0)
        //        {
        //            graphics.SetFillOpacity(op.Value);
        //        }
        //        PDFObjectRef imgref = img.EnsureRendered(context, writer);
        //        graphics.PaintImageRef(img, imgsize, pos);
        //        graphics.RestoreGraphicsState();
        //        return imgref;
        //    }
        //    else
        //        return null;// base.DoRenderToPDF(context, fullstyle, graphics, writer);
        //}

        #endregion
    }
}
