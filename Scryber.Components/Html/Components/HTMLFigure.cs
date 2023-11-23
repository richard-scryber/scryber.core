using System;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    /// <summary>
    /// Implements the HTML inline element abbr, replacing the use of the title as an outline element
    /// </summary>
    [PDFParsableComponent("figure")]
    public class HTMLFigure : Scryber.Html.Components.HTMLDiv
    {
        
        public HTMLFigure() : this(HTMLObjectTypes.Figure)
        { }

        protected HTMLFigure(ObjectType type): base(type)
        { }

        /// <summary>
        /// default output style is italic
        /// </summary>
        /// <returns></returns>
        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Margins.Top = new Unit(1, PageUnits.EMHeight);
            style.Margins.Bottom = new Unit(1, PageUnits.EMHeight);
            style.Margins.Left = new Unit(40, PageUnits.Points);
            style.Margins.Right = new Unit(40, PageUnits.Points);
            return style;
        }
    }
}
