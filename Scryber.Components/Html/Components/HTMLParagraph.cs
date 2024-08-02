﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("p")]
    public class HTMLParagraph : Scryber.Components.Paragraph
    {

        [PDFAttribute("class")]
        public override string StyleClass
        {
            get => base.StyleClass;
            set => base.StyleClass = value;
        }

        [PDFAttribute("style")]
        public override Style Style
        {
            get => base.Style;
            set => base.Style = value;
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

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        public HTMLParagraph()
            : this(HTMLObjectTypes.Paragraph)
        {
        }

        protected HTMLParagraph(ObjectType type) : base(type)
        { }

        protected override Style GetBaseStyle()
        {
            var style= base.GetBaseStyle();
            style.Margins.Top = new Unit(0.5, PageUnits.EMHeight);
            style.Margins.Bottom = new Unit(0.5, PageUnits.EMHeight);

            return style;
        }
    }
}
