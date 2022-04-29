using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF.Resources;
using Scryber.Styles;
using System.ComponentModel;

namespace Scryber.Data
{
    [PDFRequiredFramework("0.8.4")]
    [PDFParsableComponent("DataImage")]
    public class DataImage : ImageBase
    {
        public static readonly ObjectType DataImageType = (ObjectType)"DImg";
        

        [PDFElement("Data")]
        [PDFAttribute("data")]
        public Base64Data Data { get; set; }


        [PDFAttribute("key")]
        public string ImageKey { get; set; }


        public DataImage()
            : this(DataImageType)
        {
        }

        protected DataImage(ObjectType type)
            : base(type)
        {
        }

        private PDFImageXObject _xobj;

       
        protected override PDFImageXObject InitImageXObject(ContextBase context, Style style)
        {
            Document doc = this.Document;
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
                        name = "DataImage_" + this.Document.GetIncrementID(ObjectTypes.ImageXObject);
                    else
                        name = this.ImageKey;
                    
                    Scryber.Imaging.ImageReader reader = Scryber.Imaging.ImageReader.Create();
                    ImageData data = reader.ReadData(name, this.Data.Raw, compress : false);
                    
                    _xobj = PDFImageXObject.Load(data, this.Document.RenderOptions.Compression, name);
                    this.Document.SharedResources.Add(_xobj);
                }
            }

            return _xobj;
        }

    }
}
