using System;
using Scryber.Components;

namespace Scryber.Html.Components
{
    /// <summary>
    /// A single &lt;option&gt; within a &lt;select&gt;. Never independently laid out or rendered -
    /// HTMLSelect harvests every option child into its Choices collection during data binding and
    /// removes it from Contents, the same way a &lt;head&gt;'s metadata children never reach the page.
    /// </summary>
    [PDFParsableComponent("option")]
    public class HTMLOption : Component
    {
        [PDFAttribute("value")]
        public string Value { get; set; }

        [PDFAttribute("selected")]
        public HtmlBoolean Selected { get; set; }

        [PDFElement()]
        public string Label { get; set; }

        public HTMLOption() : this(HTMLObjectTypes.Option)
        {
        }

        protected HTMLOption(ObjectType type) : base(type)
        {
        }
    }
}
