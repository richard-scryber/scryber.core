using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("li")]
    public class HTMLListItem : Scryber.Components.ListItem
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

        [PDFAttribute("data-li-align")]
        public override HorizontalAlignment NumberAlignment { get => base.NumberAlignment; set => base.NumberAlignment = value; }

        [PDFAttribute("data-li-inset")]
        public override Unit NumberInset { get => base.NumberInset; set => base.NumberInset = value; }

        [PDFAttribute("data-li-label")]
        public override string ItemLabelText { get => base.ItemLabelText; set => base.ItemLabelText = value; }

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        public HTMLListItem()
            : this(HTMLObjectTypes.ListItem)
        {
        }

        protected HTMLListItem(ObjectType type) : base(type)
        { }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.RemoveValue(StyleKeys.OverflowSplitKey);
            return style;
        }

        
    }

}
