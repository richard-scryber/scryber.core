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

        [PDFAttribute("type")]
        public string MimeType { get; set; }
        
        
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
    }

    
}
