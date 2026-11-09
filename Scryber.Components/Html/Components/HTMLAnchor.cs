using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;
using Scryber.PDF;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("a")]
    public class HTMLAnchor : Scryber.Components.Link
    {

        public static readonly string LinkActionPrefix = "!";

        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        [PDFAttribute("target")]
        public string Target
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

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        [PDFAttribute("data-fit-to")]
        public override OutlineFit DestinationFit { get => base.DestinationFit; set => base.DestinationFit = value; }

        [PDFAttribute("href")]
        public override string File { get => base.File; set => base.File = value; }


        public HTMLAnchor()
            : this(HTMLObjectTypes.Anchor)
        {
        }

        protected HTMLAnchor(ObjectType type) : base(type)
        { }


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
            if(!string.IsNullOrEmpty(this.Target))
            {
                if (this.Target == "_blank")
                    this.NewWindow = true;
            }

            if (string.IsNullOrEmpty(file))
            {
                return LinkAction.Undefined;
            }

            LinkAction result;
            if (file.StartsWith(LinkActionPrefix) && file.Length > LinkActionPrefix.Length)
            {
                var named = file.Substring(LinkActionPrefix.Length);
                if (Enum.TryParse<LinkAction>(named, true, out result))
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
                
            }

            if (file.StartsWith("#"))
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
            style.Fill.Color = Scryber.Drawing.StandardColors.Blue;
            style.Text.Decoration = Text.TextDecoration.Underline;

            return style;
        }
    }
}
