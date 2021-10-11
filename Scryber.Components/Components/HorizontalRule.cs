using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Components
{
    [PDFParsableComponent("Hr")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_hr")]
    public class HorizontalRule : Line
    {
        [PDFDesignable("Width", Ignore = true)]
        public override Unit Width { get => base.Width; set => base.Width = value; }

        [PDFDesignable("Height", Ignore = true)]
        public override Unit Height { get => base.Height; set => base.Height = value; }


        protected override Style GetBaseStyle()
        {

            Style style = base.GetBaseStyle();
            style.Size.FullWidth = true;

            return style;
        }
    }
}
