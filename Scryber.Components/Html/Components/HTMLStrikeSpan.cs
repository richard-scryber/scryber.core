using System;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("strike")]
    public class HTMLStrikeSpan : HTMLSpan
    {
        public HTMLStrikeSpan() : this(HTMLObjectTypes.Strike)
        {
        }

        protected HTMLStrikeSpan(ObjectType type) : base(type)
        { }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Text.Decoration = Text.TextDecoration.StrikeThrough;
            return style;
        }
    }

    [PDFParsableComponent("del")]
    public class HTMLDelSpan : HTMLStrikeSpan
    {
        
        [PDFAttribute("cite")]
        public string Citation { get; set; }

        [PDFAttribute("datetime")]
        public string DateTime { get; set; }

        public HTMLDelSpan() : this(HTMLObjectTypes.Delete)
        {

        }

        protected HTMLDelSpan(ObjectType type) : base(type)
        { }
    }

    /// <summary>
    /// Same as &lt;strike&gt;&lt;/strike&gt;
    /// </summary>
    [PDFParsableComponent("s")]
    public class HTMLSSpan : HTMLStrikeSpan
    {

        public HTMLSSpan(): base()
        { }
    }

}
