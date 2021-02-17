using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("a")]
    public class HTMLAnchor : Scryber.Components.Link
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


        [PDFAttribute("href")]
        public override string File { get => base.File; set => base.File = value; }


        public HTMLAnchor()
            : base()
        {
        }


        public override string MapPath(string path)
        {
            return base.MapPath(path);
        }

        protected override void DoRegisterArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet set, Style fullstyle)
        {
            if (!string.IsNullOrEmpty(this.File) && this.File.StartsWith("#"))
            {
                this.Destination = this.File;
            }

            base.DoRegisterArtefacts(context, set, fullstyle);
        }

        protected override LinkAction ResolveActionType(string dest, string file)
        {
            LinkAction result;
            if(Enum.TryParse<LinkAction>(file, true, out result))
            {
                switch (result)
                {
                    case (LinkAction.FirstPage):
                    case (LinkAction.LastPage):
                    case (LinkAction.NextPage):
                    case (LinkAction.PrevPage):
                        this.Action = result;
                        return result;
                    default:
                        return LinkAction.Undefined;
                }

            }
            else if (!string.IsNullOrEmpty(file) && file.StartsWith("#"))
            {
                this.Destination = file;
                return LinkAction.Destination;
            }
            else
                return base.ResolveActionType(dest, file);
        }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Fill.Color = Scryber.Drawing.PDFColors.Blue;
            style.Text.Decoration = Text.TextDecoration.Underline;

            return style;
        }
    }
}
