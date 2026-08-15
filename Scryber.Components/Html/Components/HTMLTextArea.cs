using System;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("textarea")]
    public class HTMLTextArea : FormInputField
    {
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
                style.Size.Height = this.Rows * 14;

            if (this.Cols > 0)
                style.Size.Width = this.Cols * 7;

            return style;
        }
    }
}
