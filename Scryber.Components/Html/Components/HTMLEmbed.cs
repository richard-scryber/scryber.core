using System;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("embed")]
    public class HTMLEmbed : IconAttachment, IInvisibleContainer
    {

        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        [PDFAttribute("src")]
        public override string Source { get; set; }
        
        [PDFAttribute("type")]
        public string MimeType { get; set; }

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
        
        

        [PDFElement("")]
        [PDFArray(typeof(Component))]
        public ComponentList Contents
        {
            get { return base.InnerContent; }
        }


        public HTMLEmbed() : this(HTMLObjectTypes.Embed)
        {
        }

        protected HTMLEmbed(ObjectType type) : base(type)
        {
            this.DisplayIcon = AttachmentDisplayIcon.None;
        }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.SetValue(StyleKeys.PositionDisplayKey, DisplayMode.Inline);
            return style;
        }
    }
}
