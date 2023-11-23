using System;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Text;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("time")]
    public class HTMLTime : Scryber.Components.Date
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        [PDFAttribute("datetime")]
        public override DateTime Value { get => base.Value; set => base.Value = value; }

        [PDFAttribute("data-format")]
        public override string DateFormat { get => base.DateFormat; set => base.DateFormat = value; }

        [PDFElement()]
        public string Text { get; set; }

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

        

        public HTMLTime() : this(HTMLObjectTypes.Time)
        {
        }

        protected HTMLTime(ObjectType type): base(type)
        { }

        protected override PDFTextReader CreateReader(ContextBase context, Style fullstyle)
        {
            if (!string.IsNullOrEmpty(this.Text))
            {
                DateTime parsed;
                if (DateTime.TryParse(this.Text, out parsed))
                {
                    this.Value = parsed;
                    return base.CreateReader(context, fullstyle);
                }
                else
                    return Scryber.Text.PDFTextReader.Create(this.Text, TextFormat.Plain, false, context.TraceLog);
            }
            else
                return base.CreateReader(context, fullstyle);
        }
    }
}
