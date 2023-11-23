using System;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    /// <summary>
    /// Implements the HTML inline element abbr, replacing the use of the title as an outline element
    /// </summary>
    [PDFParsableComponent("cite")]
    public class HTMLCitation : Scryber.Html.Components.HTMLSpan
    {
        [PDFAttribute("data-outline-title")]
        public override string OutlineTitle {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        /// <summary>
        /// Overrides the default behaviour of having the title as an outline element
        /// </summary>
        [PDFAttribute("title")]
        public string AbbrTitle { get; set; }


        

        public HTMLCitation() : this(HTMLObjectTypes.Cite)
        { }

        protected HTMLCitation(ObjectType type) : base(type)
        {
        }

        /// <summary>
        /// default output style is italic
        /// </summary>
        /// <returns></returns>
        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Font.FontItalic = true;
            return style;
        }
    }
}
