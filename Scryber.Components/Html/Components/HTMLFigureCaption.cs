using System;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    /// <summary>
    /// Implements the HTML inline element abbr, replacing the use of the title as an outline element
    /// </summary>
    [PDFParsableComponent("figcaption")]
    public class HTMLFigureCaption : Scryber.Html.Components.HTMLDiv
    {
        
        

        

        public HTMLFigureCaption() : this(HTMLObjectTypes.FigureCaption)
        { }

        protected HTMLFigureCaption(ObjectType type) : base(type)
        { }

        /// <summary>
        /// default output style is italic
        /// </summary>
        /// <returns></returns>
        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            return style;
        }
    }
}
