using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("sub")]
    public class HTMLSubscript : Scryber.Html.Components.HTMLSmallSpan
    {
        

        

        public HTMLSubscript()
            : this(HTMLObjectTypes.SubScript)
        {
        }

        protected HTMLSubscript(ObjectType type): base(type)
        { }


        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Position.DisplayMode = Drawing.DisplayMode.InlineBlock;
            style.SetValue(StyleKeys.PositionModeKey, PositionMode.Relative);
            style.SetValue(StyleKeys.PositionYKey, Unit.Em(0.5));
            return style;
        }
    }

    
}
