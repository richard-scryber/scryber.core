using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    /// <summary>
    /// Explicit sub class of the PDFDiv under the HTML Namespace that can be styled independantly
    /// </summary>
    [PDFParsableComponent("summary")]
    public class HTMLDetailsSummary : Scryber.Components.Panel
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

        [PDFArray(typeof(Component))]
        [PDFElement()]
        public override ComponentList Contents => base.Contents;

        public HTMLDetailsSummary()
            : this(HTMLObjectTypes.DetailsSummary)
        {
        }

        protected HTMLDetailsSummary(ObjectType type): base(type)
        { }


        public override Style GetAppliedStyle(Component forComponent, Style baseStyle)
        {
            var style = base.GetAppliedStyle(forComponent, baseStyle);
            return style;
        }

    }
}
