using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("ul")]
    public class HTMLListUnordered : Scryber.Components.ListUnordered
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

        [PDFAttribute("data-li-group")]
        public override string NumberingGroup { get => base.NumberingGroup; set => base.NumberingGroup = value; }

        [PDFAttribute("data-li-concat")]
        public override bool ConcatenateNumberWithParent { get => base.ConcatenateNumberWithParent; set => base.ConcatenateNumberWithParent = value; }

        [PDFAttribute("data-li-postfix")]
        public override string NumberPostfix { get => base.NumberPostfix; set => base.NumberPostfix = value; }

        [PDFAttribute("data-li-prefix")]
        public override string NumberPrefix { get => base.NumberPrefix; set => base.NumberPrefix = value; }

        [PDFAttribute("data-li-inset")]
        public override Unit NumberInset { get => base.NumberInset; set => base.NumberInset = value; }


        [PDFAttribute("data-li-align")]
        public override HorizontalAlignment NumberAlignment { get => base.NumberAlignment; set => base.NumberAlignment = value; }


        [PDFAttribute("data-li-style")]
        public override ListNumberingGroupStyle NumberingStyle
        {
            get => base.NumberingStyle;
            set => base.NumberingStyle = value;
        }

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        public HTMLListUnordered()
            : this(HTMLObjectTypes.ListUnOrdered)
        {
        }

        protected HTMLListUnordered(ObjectType type) : base(type)
        { }


    }

}
