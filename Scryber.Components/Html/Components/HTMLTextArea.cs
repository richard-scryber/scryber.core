using System;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("textarea")]
    public class HTMLTextArea : FormInputField
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
        
        /// <summary>
        /// Re-declared (with [PDFElement]) so the parser attaches literal inner text
        /// (e.g. &lt;textarea&gt;default text&lt;/textarea&gt;) to Contents - FormInputField
        /// itself doesn't mark Contents as an element target, since &lt;input&gt; has no
        /// inner text content in HTML.
        /// </summary>
        [PDFElement()]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents
        {
            get { return base.Contents; }
        }

        [PDFAttribute("rows")]
        public int Rows { get; set; }

        [PDFAttribute("cols")]
        public int Cols { get; set; }

        public HTMLTextArea() : this(HTMLObjectTypes.TextArea)
        {
        }

        protected HTMLTextArea(ObjectType type) : base(type)
        {
            this.Options |= FormFieldOptions.MultiLine;
        }

        protected override void OnDataBound(DataContext context)
        {
            base.OnDataBound(context);
            this.HarvestInnerTextAsValueIfEmpty();
        }

        protected override Style GetBaseStyle()
        {
            Style style = base.GetBaseStyle();

            if (this.Rows > 0)
                style.Size.Height = Unit.Em(this.Rows);

            if (this.Cols > 0)
                style.Size.Width = Unit.Ex(this.Cols);

            return style;
        }
    }
}
