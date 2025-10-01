using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;
using Scryber.Components;
using Scryber.PDF;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("object")]
    public class HTMLObject : Scryber.Components.IconAttachment, IInvisibleContainer
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        
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

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        [PDFAttribute("data")]
        public override string Source
        {
            get => base.Source; 
            set => base.Source = value;
        }

        [PDFAttribute("data-icon")]
        public override AttachmentDisplayIcon DisplayIcon
        {
            get => base.DisplayIcon; 
            set => base.DisplayIcon = value;
        }

        [PDFAttribute("data-file")]
        public override PDFEmbeddedFileData Data
        {
            get => base.Data;
            set => base.Data = value;
        }
        
        [PDFAttribute("data-file-data")]
        public byte[] FileData
        {
            get;
            set;
        }
        
        
        
        

        [PDFAttribute("type")]
        public MimeType MimeType { get; set; }

        [PDFAttribute("alt")]
        public override string Description
        {
            get => base.Description; 
            set=> base.Description = value;
        }


        [PDFElement("")]
        [PDFArray(typeof(Component))]
        public ComponentList Contents
        {
            get { return base.InnerContent; }
        }
        

        public HTMLObject()
            : this(HTMLObjectTypes.Object)
        {
            this.DisplayIcon = AttachmentDisplayIcon.None;
        }

        protected HTMLObject(ObjectType type): base(type)
        { }


        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.SetValue(StyleKeys.PositionDisplayKey, Scryber.Drawing.DisplayMode.InlineBlock);
            return style;
        }

        protected override bool ShouldLoadAttachment(ContextBase context)
        {
            if (null == this.Data)
            {
                if (null != this.FileData && this.FileData.Length > 0)
                {
                    if (string.IsNullOrEmpty(this.Source))
                        this.Source = this.Name;
                    
                    if (string.IsNullOrEmpty(this.Source))
                        this.Source = this.ID;

                    try
                    {
                        var embed  = PDFEmbeddedFileData.LoadFileFromData(context, this.FileData, this.Source);
                        this.Data = embed;
                    }
                    catch (Exception e)
                    {
                        if (context.Conformance == ParserConformanceMode.Strict)
                            throw new PDFDataException(
                                "Could not load the attachment data for " + this.ID + " as the data is not valid - " +
                                e.Message, e);
                        else
                            context.TraceLog.Add(TraceLevel.Error, "Attachment",
                                "Could not load the file data from the binary source - " + e.Message, e);
                    }
                }
            }

            return base.ShouldLoadAttachment(context);
        }
    }

    
}
