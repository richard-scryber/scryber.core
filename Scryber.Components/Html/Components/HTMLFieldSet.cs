using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("fieldset")]
    public class HTMLFieldSet : Scryber.Components.Div
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

        public HTMLFieldSet()
            : this(HTMLObjectTypes.FieldSet)
        {
        }

        protected HTMLFieldSet(ObjectType type) : base(type)
        { }


        protected override Style GetBaseStyle()
        {
            /* HTML Default
              margin-left: 2px;
              margin-right: 2px;
              padding-top: 0.35em;
              padding-bottom: 0.625em;
              padding-left: 0.75em;
              padding-right: 0.75em;
              border: 2px groove */

            var style= base.GetBaseStyle();

            style.Margins.Left = 2;
            style.Margins.Right = 2;

            style.Padding.Top = new Drawing.Unit(0.35, Drawing.PageUnits.EMHeight);
            style.Padding.Bottom = new Drawing.Unit(0.625, Drawing.PageUnits.EMHeight);
            style.Padding.Left = new Drawing.Unit(0.75, Drawing.PageUnits.EMHeight);
            style.Padding.Right = new Drawing.Unit(0.75, Drawing.PageUnits.EMHeight);

            style.Border.Width = 2;
            style.Border.LineStyle = Drawing.LineType.Solid;

            return style;
        }
    }
}
