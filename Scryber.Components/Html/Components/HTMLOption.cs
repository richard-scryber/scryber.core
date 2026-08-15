using System;
using System.Linq;
using Scryber.Components;

namespace Scryber.Html.Components
{
    /// <summary>
    /// A single &lt;option&gt; within a &lt;select&gt;. Never independently laid out or rendered -
    /// HTMLSelect harvests every option child (via its Items wrapping list) into its Choices
    /// collection during data binding and removes it, the same way a &lt;head&gt;'s metadata
    /// children never reach the page.
    /// </summary>
    [PDFParsableComponent("option")]
    public class HTMLOption : ContainerComponent
    {
        [PDFAttribute("value")]
        public string Value { get; set; }

        [PDFAttribute("selected")]
        public HtmlBoolean Selected { get; set; }

        public string Label { get; private set; }

        /// <summary>
        /// The parser's element target - a real child (rather than a plain [PDFElement] string,
        /// which only ever captures raw, unevaluated XML text) so that a {{binding}} expression in
        /// the option's inner text - e.g. {{this.name}} inside a {{#each}} - is evaluated normally
        /// through the TextLiteral child's own data binding, the same as any other text content.
        /// </summary>
        [PDFArray(typeof(Component))]
        [PDFElement("")]
        public ComponentList Contents
        {
            get { return base.InnerContent; }
        }

        public HTMLOption() : this(HTMLObjectTypes.Option)
        {
        }

        protected HTMLOption(ObjectType type) : base(type)
        {
        }

        protected override void OnDataBound(DataContext context)
        {
            base.OnDataBound(context);

            if (string.IsNullOrEmpty(this.Label))
            {
                var text = this.Contents.OfType<TextLiteral>().FirstOrDefault();
                if (null != text)
                    this.Label = text.Text;
            }
        }
    }

    public class HTMLOptionList : ComponentWrappingList<HTMLOption>
    {
        public HTMLOptionList(ComponentList wrapped) : base(wrapped)
        {
        }
    }
}
