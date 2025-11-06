using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Drawing;
using Scryber.PDF.Resources;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("img")]
    public class HTMLImage : Scryber.Components.Image
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        
        [PDFAttribute("data-allow-missing-images")]
        public override bool AllowMissingImages { get => base.AllowMissingImages; set => base.AllowMissingImages = value; }

        /// <summary>
        /// TODO: Check that we can't just use a css min-width, and css min-height, and they can be relative rather than a config value.
        /// </summary>
        [PDFAttribute("data-min-scale")]
        public override double MinimumScaleReduction { get => base.MinimumScaleReduction; set => base.MinimumScaleReduction = value; }

        [PDFAttribute("data-img", BindingOnly = true)]
        public ImageData HtmlImageData
        {
            get { return this.Data; }
            set { this.Data = value; }
        }


        /// <summary>
        /// Gets or sets the binary data for an image.
        /// </summary>
        [PDFAttribute("data-img-data", BindingOnly = true)]
        public byte[] RawImageData
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mime type for the binary image data.
        /// </summary>
        [PDFAttribute("data-img-type")]
        public MimeType RawImageDataType
        {
            get;
            set;
        }
        
        /// <summary>
        /// Global Html hidden attribute used with xhtml as hidden='hidden'
        /// </summary>
        [PDFAttribute("hidden")]
        public string Hidden
        {
            get
            {
                if (this.Visible)
                    return string.Empty;
                else
                    return "hidden";
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value != "hidden")
                    this.Visible = true;
                else
                    this.Visible = false;
            }
        }

        [PDFAttribute("align")]
        public FloatMode Align
        {
            get
            {
                return this.Style.Position.Float;
            }
            set
            {
                this.Style.Position.Float = value;
            }
        }

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        [PDFAttribute("width")]
        public override Unit Width
        {
            get => base.Width;
            set => base.Width = value;
        }
        
        [PDFAttribute("height")]
        public override Unit Height
        {
            get => base.Height;
            set => base.Height = value;
        }

        [PDFAttribute("alt")]
        public string AlternateName
        {
            get;set;
        }

        public HTMLImage()
            : this(HTMLObjectTypes.Image)
        {
        }

        protected HTMLImage(ObjectType type): base(type)
        { }


        public override string MapPath(string path)
        {
            //Override for data images as urls - where System.Uri.IsWellFormedUriString

            if (!string.IsNullOrEmpty(path) && path.StartsWith("data:image/"))
                return path;
            else
                return base.MapPath(path);
        }

        public override string MapPath(string source, out bool isfile)
        {
            return base.MapPath(source, out isfile);
        }

        protected override PDFImageXObject InitImageXObject(ContextBase context, Style style)
        {
            if (null != this.RawImageData && this.RawImageData.Length > 0)
            {
                if (null == this.RawImageDataType)
                {
                    if (context.Conformance == ParserConformanceMode.Strict)
                        throw new NullReferenceException("The raw image data for " + this.ID +
                                                         " was set, but no mime-type was set on data-img-type to identify the actual image type.");
                    context.TraceLog.Add(TraceLevel.Warning, "Image", "The image " + this.ID + " had raw binary data set, but no mime-type was specified on data-img-type. The image type cannot be determined to falling back to default behaviour.");
                }
                else
                {
                    if (!this.TryInitImageFromRawData(context, style, this.RawImageData, this.RawImageDataType))
                        return null;
                }
            }
            return base.InitImageXObject(context, style);
        }

        private bool TryInitImageFromRawData(ContextBase context, Style style, byte[] rawImageData, MimeType rawImageDataType)
        {
            var factories = this.Document.ImageFactories;
            IPDFImageDataFactory found = null;
            foreach (var factory in factories)
            {
                if (factory.ImageType.Equals(rawImageDataType))
                {
                    found = factory;
                    break;
                }
            }

            if (null == found)
            {
                if (context.Conformance == ParserConformanceMode.Strict)
                    throw new NullReferenceException("The raw image data for " + this.ID +
                                                     " was set, but no mime-type was set on data-img-type to identify the actual image type.");
                context.TraceLog.Add(TraceLevel.Warning, "Image", "The image " + this.ID + " had raw binary data set, but no mime-type was specified on data-img-type. The image type cannot be determined to falling back to default behaviour.");
                return false;
            }
            else
            {
                bool success = false;
                ImageData loaded = null;
                
                try
                {
                    loaded = found.LoadImageData(this.Document, this, rawImageData, rawImageDataType);
                    this.Data = loaded;
                }
                catch (Exception e)
                {
                    this.Data = null;
                    this.Source = null;
                }
                
                success = null != this.Data;
                return success;
            }
        }


        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Position.DisplayMode = DisplayMode.Inline;
            
            //Set the min width and min height to 20%
            return style;
        }

    }

}
